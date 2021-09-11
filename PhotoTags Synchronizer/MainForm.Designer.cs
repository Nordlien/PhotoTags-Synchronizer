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
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Filter");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStripContainerMainForm = new System.Windows.Forms.ToolStripContainer();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusFilesAndSelected = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripLabelLazyLoadingDataGridViewProgress = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBarLazyLoadingDataGridViewProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabelSaveProgress = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBarSaveProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripLabelThreadQueue = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBarThreadQueue = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusThreadQueueCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusAction = new System.Windows.Forms.ToolStripStatusLabel();
            this.panelMain = new Krypton.Toolkit.KryptonPanel();
            this.kryptonWorkspaceMain = new Krypton.Workspace.KryptonWorkspace();
            this.kryptonPageFolderSearchFilterFolder = new Krypton.Navigator.KryptonPage();
            this.folderTreeViewFolder = new Furty.Windows.Forms.FolderTreeView();
            this.kryptonWorkspaceCellFolderSearchFilter = new Krypton.Workspace.KryptonWorkspaceCell();
            this.kryptonPageFolderSearchFilterSearch = new Krypton.Navigator.KryptonPage();
            this.kryptonWorkspaceSearchFilter = new Krypton.Workspace.KryptonWorkspace();
            this.kryptonPageSearchFiler = new Krypton.Navigator.KryptonPage();
            this.groupBoxSearchFileSystem = new Krypton.Toolkit.KryptonGroupBox();
            this.kryptonLabelSearchFilename = new Krypton.Toolkit.KryptonLabel();
            this.kryptonLabelSearchDirectory = new Krypton.Toolkit.KryptonLabel();
            this.kryptonTextBoxSearchFilename = new Krypton.Toolkit.KryptonTextBox();
            this.kryptonTextBoxSearchDirectory = new Krypton.Toolkit.KryptonTextBox();
            this.groupBoxSearchTags = new Krypton.Toolkit.KryptonGroupBox();
            this.checkBoxSearchUseAndBetweenTextTagFields = new Krypton.Toolkit.KryptonCheckBox();
            this.label3 = new Krypton.Toolkit.KryptonLabel();
            this.comboBoxSearchLocationState = new Krypton.Toolkit.KryptonComboBox();
            this.comboBoxSearchAlbum = new Krypton.Toolkit.KryptonComboBox();
            this.comboBoxSearchComments = new Krypton.Toolkit.KryptonComboBox();
            this.comboBoxSearchTitle = new Krypton.Toolkit.KryptonComboBox();
            this.label13 = new Krypton.Toolkit.KryptonLabel();
            this.label10 = new Krypton.Toolkit.KryptonLabel();
            this.comboBoxSearchLocationName = new Krypton.Toolkit.KryptonComboBox();
            this.comboBoxSearchLocationCity = new Krypton.Toolkit.KryptonComboBox();
            this.label8 = new Krypton.Toolkit.KryptonLabel();
            this.comboBoxSearchDescription = new Krypton.Toolkit.KryptonComboBox();
            this.label6 = new Krypton.Toolkit.KryptonLabel();
            this.label11 = new Krypton.Toolkit.KryptonLabel();
            this.comboBoxSearchLocationCountry = new Krypton.Toolkit.KryptonComboBox();
            this.label12 = new Krypton.Toolkit.KryptonLabel();
            this.label7 = new Krypton.Toolkit.KryptonLabel();
            this.groupBoxSearchPeople = new Krypton.Toolkit.KryptonGroupBox();
            this.panel5 = new Krypton.Toolkit.KryptonPanel();
            this.checkedListBoxSearchPeople = new System.Windows.Forms.CheckedListBox();
            this.checkBoxSearchNeedAllNames = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxSearchWithoutRegions = new Krypton.Toolkit.KryptonCheckBox();
            this.groupBoxSearchMediaTaken = new Krypton.Toolkit.KryptonGroupBox();
            this.checkBoxSearchMediaTakenIsNull = new Krypton.Toolkit.KryptonCheckBox();
            this.label14 = new Krypton.Toolkit.KryptonLabel();
            this.dateTimePickerSearchDateFrom = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerSearchDateTo = new System.Windows.Forms.DateTimePicker();
            this.label17 = new Krypton.Toolkit.KryptonLabel();
            this.groupBoxSearchRating = new Krypton.Toolkit.KryptonGroupBox();
            this.checkBoxSearchRatingEmpty = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxSearchRating1 = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxSearchRating5 = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxSearchRating0 = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxSearchRating4 = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxSearchRating2 = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxSearchRating3 = new Krypton.Toolkit.KryptonCheckBox();
            this.groupBoxSearchKeywords = new Krypton.Toolkit.KryptonGroupBox();
            this.label9 = new Krypton.Toolkit.KryptonLabel();
            this.label15 = new Krypton.Toolkit.KryptonLabel();
            this.checkBoxSearchNeedAllKeywords = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxSearchWithoutKeyword = new Krypton.Toolkit.KryptonCheckBox();
            this.comboBoxSearchKeyword = new Krypton.Toolkit.KryptonComboBox();
            this.groupBoxSearchExtra = new Krypton.Toolkit.KryptonGroupBox();
            this.checkBoxSearchHasWarning = new Krypton.Toolkit.KryptonCheckBox();
            this.kryptonWorkspaceCellSearchFiler = new Krypton.Workspace.KryptonWorkspaceCell();
            this.kryptonWorkspaceCellSearchFilterAction = new Krypton.Workspace.KryptonWorkspaceCell();
            this.kryptonPageSearchFilterAction = new Krypton.Navigator.KryptonPage();
            this.kryptonCheckBoxSearchUseRegEx = new Krypton.Toolkit.KryptonCheckBox();
            this.buttonSearch = new Krypton.Toolkit.KryptonButton();
            this.checkBoxSerachFitsAllValues = new Krypton.Toolkit.KryptonCheckBox();
            this.kryptonPageFolderSearchFilterFilter = new Krypton.Navigator.KryptonPage();
            this.treeViewFilter = new PhotoTagsCommonComponets.TreeViewWithoutDoubleClick();
            this.kryptonWorkspaceCellMediaFiles = new Krypton.Workspace.KryptonWorkspaceCell();
            this.kryptonPageMediaFiles = new Krypton.Navigator.KryptonPage();
            this.imageListView1 = new Manina.Windows.Forms.ImageListView();
            this.kryptonWorkspaceCellToolbox = new Krypton.Workspace.KryptonWorkspaceCell();
            this.kryptonPageToolboxTags = new Krypton.Navigator.KryptonPage();
            this.kryptonWorkspaceToolboxTags = new Krypton.Workspace.KryptonWorkspace();
            this.kryptonPageToolboxTagsDetails = new Krypton.Navigator.KryptonPage();
            this.kryptonGroupBoxTagsDetails = new Krypton.Toolkit.KryptonGroupBox();
            this.label4 = new Krypton.Toolkit.KryptonLabel();
            this.comboBoxAuthor = new Krypton.Toolkit.KryptonComboBox();
            this.labelTitle = new Krypton.Toolkit.KryptonLabel();
            this.comboBoxAlbum = new Krypton.Toolkit.KryptonComboBox();
            this.comboBoxDescription = new Krypton.Toolkit.KryptonComboBox();
            this.comboBoxComments = new Krypton.Toolkit.KryptonComboBox();
            this.labelDescription = new Krypton.Toolkit.KryptonLabel();
            this.labelAuthor = new Krypton.Toolkit.KryptonLabel();
            this.comboBoxTitle = new Krypton.Toolkit.KryptonComboBox();
            this.labelComments = new Krypton.Toolkit.KryptonLabel();
            this.kryptonGroupBoxToolboxTagsTags = new Krypton.Toolkit.KryptonGroupBox();
            this.label5 = new Krypton.Toolkit.KryptonLabel();
            this.comboBoxMediaAiConfidence = new Krypton.Toolkit.KryptonComboBox();
            this.groupBoxRating = new Krypton.Toolkit.KryptonGroupBox();
            this.radioButtonRating5 = new Krypton.Toolkit.KryptonRadioButton();
            this.radioButtonRating4 = new Krypton.Toolkit.KryptonRadioButton();
            this.radioButtonRating3 = new Krypton.Toolkit.KryptonRadioButton();
            this.radioButtonRating2 = new Krypton.Toolkit.KryptonRadioButton();
            this.radioButtonRating1 = new Krypton.Toolkit.KryptonRadioButton();
            this.kryptonWorkspaceCellToolboxTagsDetails = new Krypton.Workspace.KryptonWorkspaceCell();
            this.kryptonWorkspaceCellToolboxTagsKeywords = new Krypton.Workspace.KryptonWorkspaceCell();
            this.kryptonPageToolboxTagsKeywords = new Krypton.Navigator.KryptonPage();
            this.dataGridViewTagsAndKeywords = new Krypton.Toolkit.KryptonDataGridView();
            this.kryptonPageToolboxPeople = new Krypton.Navigator.KryptonPage();
            this.dataGridViewPeople = new Krypton.Toolkit.KryptonDataGridView();
            this.kryptonContextMenuGenericBase = new Krypton.Toolkit.KryptonContextMenu();
            this.kryptonContextMenuItemsGenericBaseList = new Krypton.Toolkit.KryptonContextMenuItems();
            this.kryptonContextMenuItemGenericRegionRename1 = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItemGenericRegionRename2 = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItemGenericRegionRename3 = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItemGenericRegionRenameFromLastUsed = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItemsGenericRegionRenameFromLastUsedList = new Krypton.Toolkit.KryptonContextMenuItems();
            this.kryptonContextMenuItemGenericRegionRenameFormLastUsedExample = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItemGenericRegionRenameListAll = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItemsGenericRegionRenameListAllList = new Krypton.Toolkit.KryptonContextMenuItems();
            this.kryptonContextMenuItemGenericRegionRenameListAllExample = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuSeparatorGenericEndOfRegionRename = new Krypton.Toolkit.KryptonContextMenuSeparator();
            this.kryptonContextMenuItemGenericCut = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItemGenericCopy = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItemGenericCopyText = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItemGenericPaste = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItemGenericDelete = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItemGenericRename = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItemGenericUndo = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItemGenericRedo = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItemGenericFind = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItemGenericReplace = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItemGenericSave = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuSeparatorGenericEndOfClipboard = new Krypton.Toolkit.KryptonContextMenuSeparator();
            this.kryptonContextMenuItemGenericRefreshFolder = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItemGenericReadSubfolders = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItemGenericOpenFolderLocation = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItemGenericOpen = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItemGenericOpenWith = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItemsGenericOpenWithAppList = new Krypton.Toolkit.KryptonContextMenuItems();
            this.kryptonContextMenuItemsGenericOpenWithAppListExample = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItemOpenAndAssociateWithDialog = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItemGenericOpenVerbEdit = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItemGenericRunCommand = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuSeparatorGenericEndOfFileSystem = new Krypton.Toolkit.KryptonContextMenuSeparator();
            this.kryptonContextMenuItemGenericAutoCorrectRun = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItemGenericAutoCorrectForm = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItemGenericMetadataRefreshLast = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItemGenericMetadataDeleteHistory = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuSeparatorGenericEndOfMetadata = new Krypton.Toolkit.KryptonContextMenuSeparator();
            this.kryptonContextMenuItemGenericRotate270 = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItemGenericRotate180 = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItemGenericRotate90 = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuSeparatorEndOfRotate = new Krypton.Toolkit.KryptonContextMenuSeparator();
            this.kryptonContextMenuItemGenericFavoriteAdd = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItemGenericFavoriteDelete = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItemGenericFavoriteToggle = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuSeparatorGenericEndOfFavorite = new Krypton.Toolkit.KryptonContextMenuSeparator();
            this.kryptonContextMenuItemGenericRowShowFavorite = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItemGenericRowHideEqual = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuSeparatorGenericEndOfShowHideRows = new Krypton.Toolkit.KryptonContextMenuSeparator();
            this.kryptonContextMenuItemGenericTriStateOn = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItemGenericTriStateOff = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItemGenericTriStateToggle = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuSeparatorGenericEndOfTriState = new Krypton.Toolkit.KryptonContextMenuSeparator();
            this.kryptonContextMenuItemGenericMediaViewAsPoster = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItemGenericMediaViewAsFull = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonPageToolboxMap = new Krypton.Navigator.KryptonPage();
            this.kryptonWorkspaceToolboxMap = new Krypton.Workspace.KryptonWorkspace();
            this.kryptonPageToolboxMapProperties = new Krypton.Navigator.KryptonPage();
            this.comboBoxGoogleLocationInterval = new Krypton.Toolkit.KryptonComboBox();
            this.comboBoxGoogleTimeZoneShift = new Krypton.Toolkit.KryptonComboBox();
            this.kryptonWorkspaceCellToolboxMapProperties = new Krypton.Workspace.KryptonWorkspaceCell();
            this.kryptonWorkspaceCellToolboxMapDetails = new Krypton.Workspace.KryptonWorkspaceCell();
            this.kryptonPageToolboxMapDetails = new Krypton.Navigator.KryptonPage();
            this.dataGridViewMap = new Krypton.Toolkit.KryptonDataGridView();
            this.kryptonWorkspaceCellToolboxMapBroswer = new Krypton.Workspace.KryptonWorkspaceCell();
            this.kryptonPageToolboxMapBroswer = new Krypton.Navigator.KryptonPage();
            this.panelBrowser = new Krypton.Toolkit.KryptonPanel();
            this.kryptonWorkspaceCellToolboxMapBroswerProperties = new Krypton.Workspace.KryptonWorkspaceCell();
            this.kryptonPageToolboxMapBroswerProperties = new Krypton.Navigator.KryptonPage();
            this.textBoxBrowserURL = new Krypton.Toolkit.KryptonTextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.comboBoxMapZoomLevel = new Krypton.Toolkit.KryptonComboBox();
            this.kryptonPageToolboxDates = new Krypton.Navigator.KryptonPage();
            this.dataGridViewDate = new Krypton.Toolkit.KryptonDataGridView();
            this.kryptonPageToolboxExiftool = new Krypton.Navigator.KryptonPage();
            this.dataGridViewExiftool = new Krypton.Toolkit.KryptonDataGridView();
            this.contextMenuStripExifTool = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemExiftoolAssignCompositeTag = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExiftoolCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExiftoolFind = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExiftoolMarkFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExiftoolRemoveFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExiftoolToggleFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExiftoolSHowFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExiftoolHideEqual = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemShowPosterWindowExiftool = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMediaPreview = new System.Windows.Forms.ToolStripMenuItem();
            this.kryptonPageToolboxWarnings = new Krypton.Navigator.KryptonPage();
            this.dataGridViewExiftoolWarning = new Krypton.Toolkit.KryptonDataGridView();
            this.contextMenuStripExiftoolWarning = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemtoolExiftoolWarningAssignCompositeTag = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExiftoolWarningCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExiftoolWarningFind = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExiftoolWarningMarkFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExiftoolWarningRemoveFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExiftoolWarningToggleFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExiftoolWarningShowFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExiftoolWarningHideEqual = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemShowPosterWindowWarnings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExiftoolWarningMediaPreview = new System.Windows.Forms.ToolStripMenuItem();
            this.kryptonPageToolboxProperties = new Krypton.Navigator.KryptonPage();
            this.dataGridViewProperties = new Krypton.Toolkit.KryptonDataGridView();
            this.kryptonPageToolboxRename = new Krypton.Navigator.KryptonPage();
            this.kryptonWorkspaceToolboxRename = new Krypton.Workspace.KryptonWorkspace();
            this.kryptonPageToolboxRenameVariables = new Krypton.Navigator.KryptonPage();
            this.buttonRenameUpdate = new Krypton.Toolkit.KryptonButton();
            this.checkBoxRenameShowFullPath = new Krypton.Toolkit.KryptonCheckBox();
            this.buttonRenameSave = new Krypton.Toolkit.KryptonButton();
            this.label2 = new Krypton.Toolkit.KryptonLabel();
            this.textBoxRenameNewName = new Krypton.Toolkit.KryptonTextBox();
            this.label1 = new Krypton.Toolkit.KryptonLabel();
            this.comboBoxRenameVariableList = new Krypton.Toolkit.KryptonComboBox();
            this.kryptonWorkspaceCellToolboxRenameVariables = new Krypton.Workspace.KryptonWorkspaceCell();
            this.kryptonWorkspaceCellToolboxRenameResult = new Krypton.Workspace.KryptonWorkspaceCell();
            this.kryptonPageToolboxRenameResult = new Krypton.Navigator.KryptonPage();
            this.dataGridViewRename = new Krypton.Toolkit.KryptonDataGridView();
            this.kryptonPageToolboxConvertAndMerge = new Krypton.Navigator.KryptonPage();
            this.dataGridViewConvertAndMerge = new Krypton.Toolkit.KryptonDataGridView();
            this.toolStripContainerStripMainForm = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonPreview = new System.Windows.Forms.ToolStripButton();
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
            this.toolStripMenuItemKeywordsShowFavoriteRows = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemKeywordsHideEqualRows = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemTagsBrokerCopyText = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemTagsAndKeywordsBrokerOverwriteText = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleTagsAndKeywordsSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectTagsAndKeywordsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeTagsAndKeywordsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemShowPosterWindowKeywords = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemTagsAndKeywordMediaPreview = new System.Windows.Forms.ToolStripMenuItem();
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
            this.toolStripMenuItemShowCoordinateOnGoogleMap = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMapReloadLocationUsingNominatim = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemShowPosterWindowMap = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMapMediaPreview = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripDate = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemDateCut = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDateCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDatePaste = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDateDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDateUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDateRedo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDateFind = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDateReplace = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDateSave = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDateMarkFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDateRemoveFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDateToggleFavourite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDateShowFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDateHideEqualRows = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemShowPosterWindowDate = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDateMediaPreview = new System.Windows.Forms.ToolStripMenuItem();
            this.imageListFilter = new System.Windows.Forms.ImageList(this.components);
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.timerShowErrorMessage = new System.Windows.Forms.Timer(this.components);
            this.timerShowStatusText_RemoveTimer = new System.Windows.Forms.Timer(this.components);
            this.timerStartThread = new System.Windows.Forms.Timer(this.components);
            this.timerShowExiftoolSaveProgress = new System.Windows.Forms.Timer(this.components);
            this.timerStatusThreadQueue = new System.Windows.Forms.Timer(this.components);
            this.timerLazyLoadingDataGridViewProgressRemoveProgessbar = new System.Windows.Forms.Timer(this.components);
            this.panelMediaPreview = new Krypton.Toolkit.KryptonPanel();
            this.toolStripContainerMediaPreview = new System.Windows.Forms.ToolStripContainer();
            this.imageBoxPreview = new Cyotek.Windows.Forms.ImageBox();
            this.videoView1 = new LibVLCSharp.WinForms.VideoView();
            this.toolStripContainerStripMediaPreview = new System.Windows.Forms.ToolStrip();
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
            this.toolStripTraceBarItemMediaPreviewTimer = new PhotoTagsCommonComponets.ToolStripTraceBarItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabelMediaPreviewTimer = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabelMediaPreviewStatus = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.timerFindGoogleCast = new System.Windows.Forms.Timer(this.components);
            this.timerPreviewNextTimer = new System.Windows.Forms.Timer(this.components);
            this.timerSaveProgessRemoveProgress = new System.Windows.Forms.Timer(this.components);
            this.kryptonManager1 = new Krypton.Toolkit.KryptonManager(this.components);
            this.kryptonPage5 = new Krypton.Navigator.KryptonPage();
            this.kryptonPage7 = new Krypton.Navigator.KryptonPage();
            this.kryptonPage2 = new Krypton.Navigator.KryptonPage();
            this.kryptonPage4 = new Krypton.Navigator.KryptonPage();
            this.kryptonPage3 = new Krypton.Navigator.KryptonPage();
            this.kryptonPage10 = new Krypton.Navigator.KryptonPage();
            this.kryptonPage12 = new Krypton.Navigator.KryptonPage();
            this.kryptonPage6 = new Krypton.Navigator.KryptonPage();
            this.kryptonPage9 = new Krypton.Navigator.KryptonPage();
            this.buttonSpecAny1 = new Krypton.Toolkit.ButtonSpecAny();
            this.buttonSpecAny2 = new Krypton.Toolkit.ButtonSpecAny();
            this.kryptonRibbonMain = new Krypton.Ribbon.KryptonRibbon();
            this.kryptonRibbonQATButtonSave = new Krypton.Ribbon.KryptonRibbonQATButton();
            this.kryptonRibbonQATButtonMediaPreview = new Krypton.Ribbon.KryptonRibbonQATButton();
            this.kryptonRibbonQATButtonMediaPoster = new Krypton.Ribbon.KryptonRibbonQATButton();
            this.kryptonRibbonTabHome = new Krypton.Ribbon.KryptonRibbonTab();
            this.kryptonRibbonGroupHomeClipboard = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupTriple10 = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonHomeCopy = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonHomeCut = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonHomePaste = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupTriple11 = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonHomeUndo = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonRedo = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupHomeManage = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupTriple19 = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonHomeCopyText = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonHomeFastCopyNoOverwrite = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonHomeFastCopyOverwrite = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupTriple20 = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonHomeFind = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonHomeReplace = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupHomeFileSystem = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupTriple27 = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonHomeFileSystemDelete = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonHomeFileSystemRename = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonHomeFileSystemRefresh = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupTriple28 = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonHomeFileSystemOpen = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonHomeFileSystemOpenWith = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonFileSystemOpenAssociateDialog = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupTriple17 = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonHomeFileSystemOpenExplorer = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonFileSystemRunCommand = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonHomeFileSystemEdit = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupHomeRotate = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupTriple9 = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonMediaFileRotate90CCW = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonMediaFileRotate180 = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonMediaFileRotate90CW = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupHomeMetadata = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupTriple16 = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonHomeAutoCorrectRun = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonHomeAutoCorrectForm = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupTriple15 = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonHomeMetadataRefresh = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonHomeMetadataReload = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupTriple21 = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonHomeTagSelectOn = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonHomeTagSelectToggle = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonHomeTagSelectOff = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupHomeSave = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupTriple29 = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonHomeSaveSave = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonTabView = new Krypton.Ribbon.KryptonRibbonTab();
            this.kryptonRibbonGroupViewViewModes = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupTriple2 = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonImageListViewModeGallery = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonImageListViewModeDetails = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonImageListViewModePane = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupTriple1 = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonImageListViewModeThumbnails = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonImageListViewModeThumbnailRenders = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupSeparator4 = new Krypton.Ribbon.KryptonRibbonGroupSeparator();
            this.kryptonRibbonGroupTriple5 = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonImageListViewDetailviewColumns = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupViewThumbnailSize = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupTriple12 = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonThumbnailSizeXLarge = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonThumbnailSizeLarge = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonThumbnailSizeMedium = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupTriple13 = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonThumbnailSizeSmall = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonThumbnailSizeXSmall = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupViewCellSize = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupTriple3 = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonDataGridViewCellSizeBig = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonDataGridViewCellSizeMedium = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonDataGridViewCellSizeSmall = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupViewColumns = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupTriple4 = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonDataGridViewColumnsHistory = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonDataGridViewColumnsErrors = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupViewRows = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupTriple14 = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonDataGridViewRowsFavorite = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonDataGridViewRowsHideEqual = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroup14 = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupTriple18 = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonDataGridViewFavouriteAdd = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonDataGridViewFavouriteToggle = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonDataGridViewFavouriteDelete = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonTabSelect = new Krypton.Ribbon.KryptonRibbonTab();
            this.kryptonRibbonGroupImageListViewSelect = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupTriple7 = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButton20 = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButton21 = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupSeparator2 = new Krypton.Ribbon.KryptonRibbonGroupSeparator();
            this.kryptonRibbonGroup2 = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupLinesSelectByDateInterval = new Krypton.Ribbon.KryptonRibbonGroupLines();
            this.kryptonRibbonGroupRadioButton1 = new Krypton.Ribbon.KryptonRibbonGroupRadioButton();
            this.kryptonRibbonGroupRadioButton2 = new Krypton.Ribbon.KryptonRibbonGroupRadioButton();
            this.kryptonRibbonGroupRadioButton3 = new Krypton.Ribbon.KryptonRibbonGroupRadioButton();
            this.kryptonRibbonGroupRadioButton4 = new Krypton.Ribbon.KryptonRibbonGroupRadioButton();
            this.kryptonRibbonGroupRadioButton5 = new Krypton.Ribbon.KryptonRibbonGroupRadioButton();
            this.kryptonRibbonGroup9 = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupLines4 = new Krypton.Ribbon.KryptonRibbonGroupLines();
            this.kryptonRibbonGroupRadioButton10 = new Krypton.Ribbon.KryptonRibbonGroupRadioButton();
            this.kryptonRibbonGroupRadioButton11 = new Krypton.Ribbon.KryptonRibbonGroupRadioButton();
            this.kryptonRibbonGroup7 = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupLines2 = new Krypton.Ribbon.KryptonRibbonGroupLines();
            this.kryptonRibbonGroupRadioButton6 = new Krypton.Ribbon.KryptonRibbonGroupRadioButton();
            this.kryptonRibbonGroupRadioButton7 = new Krypton.Ribbon.KryptonRibbonGroupRadioButton();
            this.kryptonRibbonGroupRadioButton8 = new Krypton.Ribbon.KryptonRibbonGroupRadioButton();
            this.kryptonRibbonGroupRadioButton9 = new Krypton.Ribbon.KryptonRibbonGroupRadioButton();
            this.kryptonRibbonGroup8 = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupLines3 = new Krypton.Ribbon.KryptonRibbonGroupLines();
            this.kryptonRibbonGroupCheckBox1 = new Krypton.Ribbon.KryptonRibbonGroupCheckBox();
            this.kryptonRibbonGroupCheckBox2 = new Krypton.Ribbon.KryptonRibbonGroupCheckBox();
            this.kryptonRibbonGroupCheckBox3 = new Krypton.Ribbon.KryptonRibbonGroupCheckBox();
            this.kryptonRibbonGroupCheckBox4 = new Krypton.Ribbon.KryptonRibbonGroupCheckBox();
            this.kryptonRibbonTabTools = new Krypton.Ribbon.KryptonRibbonTab();
            this.kryptonRibbonGroupToolsMain = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupTriple6 = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonToolsImportLocations = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonToolsWebScraping = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroup1 = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupTriple8 = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonToolsConfig = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonToolsAbout = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonTabPreview = new Krypton.Ribbon.KryptonRibbonTab();
            this.kryptonRibbonGroupPreviewPreview = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupTriple23 = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonPreviewPreview = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupPreviewNavigate = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupTriple22 = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButton17 = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButton26 = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButton27 = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupTriple24 = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButton29 = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButton30 = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButton31 = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupLines5 = new Krypton.Ribbon.KryptonRibbonGroupLines();
            this.kryptonRibbonGroupTrackBar1 = new Krypton.Ribbon.KryptonRibbonGroupTrackBar();
            this.kryptonRibbonGroupPreviewRotate = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupTriple25 = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButton32 = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButton33 = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButton34 = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupPreviewSlideshow = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupTriple26 = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButton35 = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButton36 = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButton37 = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupPreviewStatus = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupLines1 = new Krypton.Ribbon.KryptonRibbonGroupLines();
            this.kryptonRibbonGroupLabel1 = new Krypton.Ribbon.KryptonRibbonGroupLabel();
            this.kryptonRibbonGroupLabel2 = new Krypton.Ribbon.KryptonRibbonGroupLabel();
            this.kryptonContextMenuImageListViewModeThumbnailRenders = new Krypton.Toolkit.KryptonContextMenu();
            this.kryptonContextMenuItems3 = new Krypton.Toolkit.KryptonContextMenuItems();
            this.kryptonContextMenuItems1 = new Krypton.Toolkit.KryptonContextMenuItems();
            this.kryptonContextMenuItems2 = new Krypton.Toolkit.KryptonContextMenuItems();
            this.kryptonContextMenuItem1 = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItem2 = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItem3 = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuHeading1 = new Krypton.Toolkit.KryptonContextMenuHeading();
            this.kryptonContextMenuMonthCalendar1 = new Krypton.Toolkit.KryptonContextMenuMonthCalendar();
            this.kryptonContextMenuMonthCalendar2 = new Krypton.Toolkit.KryptonContextMenuMonthCalendar();
            this.kryptonContextMenuImageSelect1 = new Krypton.Toolkit.KryptonContextMenuImageSelect();
            this.kryptonContextMenuItems4 = new Krypton.Toolkit.KryptonContextMenuItems();
            this.kryptonContextMenuItem4 = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItems5 = new Krypton.Toolkit.KryptonContextMenuItems();
            this.kryptonContextMenuItem21 = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItems6 = new Krypton.Toolkit.KryptonContextMenuItems();
            this.kryptonContextMenuItem5 = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItem6 = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItem7 = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuSeparator8 = new Krypton.Toolkit.KryptonContextMenuSeparator();
            this.kryptonContextMenuItem8 = new Krypton.Toolkit.KryptonContextMenuItem();
            this.sortMediaFileByToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSortByFilename = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSortByFileCreatedDate = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSortByFileModifiedDate = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSortByMediaDateTaken = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSortByMediaAlbum = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSortByMediaTitle = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSortByMediaDescription = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSortByMediaComments = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSortByMediaAuthor = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSortByMediaRating = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSortByLocationName = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSortByLocationRegionState = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSortByLocationCity = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSortByLocationCountry = new System.Windows.Forms.ToolStripMenuItem();
            this.openMediaFilesWithToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.toolStripMenuItemImageListViewAutoCorrectForm = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileWithAssociatedApplicationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editFileWithAssociatedApplicationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runSelectedToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.openWithDialogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rotateCW90ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rotate180ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ratateCCW270ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showMediaPosterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mediaPreviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripTreeViewFolder = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemTreeViewFolderCut = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemTreeViewFolderCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemTreeViewFolderCopyFolderNameToClipboard = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemTreeViewFolderPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemTreeViewFolderDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemTreeViewFolderRefreshFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemTreeViewFolderReadSubfolders = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemTreeViewFolderReload = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemTreeViewFolderClearCache = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemTreeViewFolderAutoCorrectMetadata = new System.Windows.Forms.ToolStripMenuItem();
            this.openFolderLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kryptonRibbonQATButtonViewMediaPoster = new Krypton.Ribbon.KryptonRibbonQATButton();
            this.kryptonRibbonQATButtonViewMediaPreview = new Krypton.Ribbon.KryptonRibbonQATButton();
            this.toolStripContainerMainForm.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainerMainForm.ContentPanel.SuspendLayout();
            this.toolStripContainerMainForm.TopToolStripPanel.SuspendLayout();
            this.toolStripContainerMainForm.SuspendLayout();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelMain)).BeginInit();
            this.panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceMain)).BeginInit();
            this.kryptonWorkspaceMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageFolderSearchFilterFolder)).BeginInit();
            this.kryptonPageFolderSearchFilterFolder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCellFolderSearchFilter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageFolderSearchFilterSearch)).BeginInit();
            this.kryptonPageFolderSearchFilterSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceSearchFilter)).BeginInit();
            this.kryptonWorkspaceSearchFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageSearchFiler)).BeginInit();
            this.kryptonPageSearchFiler.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchFileSystem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchFileSystem.Panel)).BeginInit();
            this.groupBoxSearchFileSystem.Panel.SuspendLayout();
            this.groupBoxSearchFileSystem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchTags)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchTags.Panel)).BeginInit();
            this.groupBoxSearchTags.Panel.SuspendLayout();
            this.groupBoxSearchTags.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxSearchLocationState)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxSearchAlbum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxSearchComments)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxSearchTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxSearchLocationName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxSearchLocationCity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxSearchDescription)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxSearchLocationCountry)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchPeople)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchPeople.Panel)).BeginInit();
            this.groupBoxSearchPeople.Panel.SuspendLayout();
            this.groupBoxSearchPeople.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panel5)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchMediaTaken)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchMediaTaken.Panel)).BeginInit();
            this.groupBoxSearchMediaTaken.Panel.SuspendLayout();
            this.groupBoxSearchMediaTaken.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchRating)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchRating.Panel)).BeginInit();
            this.groupBoxSearchRating.Panel.SuspendLayout();
            this.groupBoxSearchRating.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchKeywords)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchKeywords.Panel)).BeginInit();
            this.groupBoxSearchKeywords.Panel.SuspendLayout();
            this.groupBoxSearchKeywords.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxSearchKeyword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchExtra)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchExtra.Panel)).BeginInit();
            this.groupBoxSearchExtra.Panel.SuspendLayout();
            this.groupBoxSearchExtra.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCellSearchFiler)).BeginInit();
            this.kryptonWorkspaceCellSearchFiler.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCellSearchFilterAction)).BeginInit();
            this.kryptonWorkspaceCellSearchFilterAction.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageSearchFilterAction)).BeginInit();
            this.kryptonPageSearchFilterAction.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageFolderSearchFilterFilter)).BeginInit();
            this.kryptonPageFolderSearchFilterFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCellMediaFiles)).BeginInit();
            this.kryptonWorkspaceCellMediaFiles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageMediaFiles)).BeginInit();
            this.kryptonPageMediaFiles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCellToolbox)).BeginInit();
            this.kryptonWorkspaceCellToolbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageToolboxTags)).BeginInit();
            this.kryptonPageToolboxTags.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceToolboxTags)).BeginInit();
            this.kryptonWorkspaceToolboxTags.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageToolboxTagsDetails)).BeginInit();
            this.kryptonPageToolboxTagsDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxTagsDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxTagsDetails.Panel)).BeginInit();
            this.kryptonGroupBoxTagsDetails.Panel.SuspendLayout();
            this.kryptonGroupBoxTagsDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxAuthor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxAlbum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxDescription)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxComments)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxToolboxTagsTags)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxToolboxTagsTags.Panel)).BeginInit();
            this.kryptonGroupBoxToolboxTagsTags.Panel.SuspendLayout();
            this.kryptonGroupBoxToolboxTagsTags.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxMediaAiConfidence)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxRating)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxRating.Panel)).BeginInit();
            this.groupBoxRating.Panel.SuspendLayout();
            this.groupBoxRating.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCellToolboxTagsDetails)).BeginInit();
            this.kryptonWorkspaceCellToolboxTagsDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCellToolboxTagsKeywords)).BeginInit();
            this.kryptonWorkspaceCellToolboxTagsKeywords.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageToolboxTagsKeywords)).BeginInit();
            this.kryptonPageToolboxTagsKeywords.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTagsAndKeywords)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageToolboxPeople)).BeginInit();
            this.kryptonPageToolboxPeople.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPeople)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageToolboxMap)).BeginInit();
            this.kryptonPageToolboxMap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceToolboxMap)).BeginInit();
            this.kryptonWorkspaceToolboxMap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageToolboxMapProperties)).BeginInit();
            this.kryptonPageToolboxMapProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxGoogleLocationInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxGoogleTimeZoneShift)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCellToolboxMapProperties)).BeginInit();
            this.kryptonWorkspaceCellToolboxMapProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCellToolboxMapDetails)).BeginInit();
            this.kryptonWorkspaceCellToolboxMapDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageToolboxMapDetails)).BeginInit();
            this.kryptonPageToolboxMapDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCellToolboxMapBroswer)).BeginInit();
            this.kryptonWorkspaceCellToolboxMapBroswer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageToolboxMapBroswer)).BeginInit();
            this.kryptonPageToolboxMapBroswer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelBrowser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCellToolboxMapBroswerProperties)).BeginInit();
            this.kryptonWorkspaceCellToolboxMapBroswerProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageToolboxMapBroswerProperties)).BeginInit();
            this.kryptonPageToolboxMapBroswerProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxMapZoomLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageToolboxDates)).BeginInit();
            this.kryptonPageToolboxDates.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageToolboxExiftool)).BeginInit();
            this.kryptonPageToolboxExiftool.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewExiftool)).BeginInit();
            this.contextMenuStripExifTool.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageToolboxWarnings)).BeginInit();
            this.kryptonPageToolboxWarnings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewExiftoolWarning)).BeginInit();
            this.contextMenuStripExiftoolWarning.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageToolboxProperties)).BeginInit();
            this.kryptonPageToolboxProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageToolboxRename)).BeginInit();
            this.kryptonPageToolboxRename.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceToolboxRename)).BeginInit();
            this.kryptonWorkspaceToolboxRename.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageToolboxRenameVariables)).BeginInit();
            this.kryptonPageToolboxRenameVariables.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxRenameVariableList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCellToolboxRenameVariables)).BeginInit();
            this.kryptonWorkspaceCellToolboxRenameVariables.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCellToolboxRenameResult)).BeginInit();
            this.kryptonWorkspaceCellToolboxRenameResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageToolboxRenameResult)).BeginInit();
            this.kryptonPageToolboxRenameResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRename)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageToolboxConvertAndMerge)).BeginInit();
            this.kryptonPageToolboxConvertAndMerge.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewConvertAndMerge)).BeginInit();
            this.toolStripContainerStripMainForm.SuspendLayout();
            this.contextMenuStripTagsAndKeywords.SuspendLayout();
            this.contextMenuStripPeople.SuspendLayout();
            this.contextMenuStripMap.SuspendLayout();
            this.contextMenuStripDate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelMediaPreview)).BeginInit();
            this.panelMediaPreview.SuspendLayout();
            this.toolStripContainerMediaPreview.ContentPanel.SuspendLayout();
            this.toolStripContainerMediaPreview.TopToolStripPanel.SuspendLayout();
            this.toolStripContainerMediaPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.videoView1)).BeginInit();
            this.toolStripContainerStripMediaPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonRibbonMain)).BeginInit();
            this.contextMenuStripImageListView.SuspendLayout();
            this.contextMenuStripTreeViewFolder.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainerMainForm
            // 
            // 
            // toolStripContainerMainForm.BottomToolStripPanel
            // 
            this.toolStripContainerMainForm.BottomToolStripPanel.Controls.Add(this.statusStrip);
            // 
            // toolStripContainerMainForm.ContentPanel
            // 
            this.toolStripContainerMainForm.ContentPanel.AutoScroll = true;
            this.toolStripContainerMainForm.ContentPanel.Controls.Add(this.panelMain);
            this.toolStripContainerMainForm.ContentPanel.Size = new System.Drawing.Size(1214, 728);
            this.toolStripContainerMainForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainerMainForm.LeftToolStripPanelVisible = false;
            this.toolStripContainerMainForm.Location = new System.Drawing.Point(0, 115);
            this.toolStripContainerMainForm.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripContainerMainForm.Name = "toolStripContainerMainForm";
            // 
            // toolStripContainerMainForm.RightToolStripPanel
            // 
            this.toolStripContainerMainForm.RightToolStripPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.toolStripContainerMainForm.RightToolStripPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.toolStripContainerMainForm.RightToolStripPanelVisible = false;
            this.toolStripContainerMainForm.Size = new System.Drawing.Size(1214, 779);
            this.toolStripContainerMainForm.TabIndex = 0;
            this.toolStripContainerMainForm.Text = "toolStripContainer1";
            // 
            // toolStripContainerMainForm.TopToolStripPanel
            // 
            this.toolStripContainerMainForm.TopToolStripPanel.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripContainerMainForm.TopToolStripPanel.Controls.Add(this.toolStripContainerStripMainForm);
            this.toolStripContainerMainForm.TopToolStripPanel.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            // 
            // statusStrip
            // 
            this.statusStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusFilesAndSelected,
            this.toolStripLabelLazyLoadingDataGridViewProgress,
            this.toolStripProgressBarLazyLoadingDataGridViewProgress,
            this.toolStripStatusLabelSaveProgress,
            this.toolStripProgressBarSaveProgress,
            this.toolStripLabelThreadQueue,
            this.toolStripProgressBarThreadQueue,
            this.toolStripStatusThreadQueueCount,
            this.toolStripStatusAction});
            this.statusStrip.Location = new System.Drawing.Point(0, 0);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1214, 24);
            this.statusStrip.TabIndex = 0;
            // 
            // toolStripStatusFilesAndSelected
            // 
            this.toolStripStatusFilesAndSelected.Name = "toolStripStatusFilesAndSelected";
            this.toolStripStatusFilesAndSelected.Size = new System.Drawing.Size(101, 19);
            this.toolStripStatusFilesAndSelected.Text = "Files: 0 Selected: 0";
            // 
            // toolStripLabelLazyLoadingDataGridViewProgress
            // 
            this.toolStripLabelLazyLoadingDataGridViewProgress.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.toolStripLabelLazyLoadingDataGridViewProgress.Name = "toolStripLabelLazyLoadingDataGridViewProgress";
            this.toolStripLabelLazyLoadingDataGridViewProgress.Size = new System.Drawing.Size(59, 23);
            this.toolStripLabelLazyLoadingDataGridViewProgress.Text = "Working:";
            this.toolStripLabelLazyLoadingDataGridViewProgress.Visible = false;
            // 
            // toolStripProgressBarLazyLoadingDataGridViewProgress
            // 
            this.toolStripProgressBarLazyLoadingDataGridViewProgress.Name = "toolStripProgressBarLazyLoadingDataGridViewProgress";
            this.toolStripProgressBarLazyLoadingDataGridViewProgress.Size = new System.Drawing.Size(100, 22);
            this.toolStripProgressBarLazyLoadingDataGridViewProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.toolStripProgressBarLazyLoadingDataGridViewProgress.ToolTipText = "Update DataGridView ";
            this.toolStripProgressBarLazyLoadingDataGridViewProgress.Value = 50;
            this.toolStripProgressBarLazyLoadingDataGridViewProgress.Visible = false;
            // 
            // toolStripStatusLabelSaveProgress
            // 
            this.toolStripStatusLabelSaveProgress.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.toolStripStatusLabelSaveProgress.Name = "toolStripStatusLabelSaveProgress";
            this.toolStripStatusLabelSaveProgress.Size = new System.Drawing.Size(38, 23);
            this.toolStripStatusLabelSaveProgress.Text = "Save:";
            this.toolStripStatusLabelSaveProgress.Visible = false;
            // 
            // toolStripProgressBarSaveProgress
            // 
            this.toolStripProgressBarSaveProgress.Name = "toolStripProgressBarSaveProgress";
            this.toolStripProgressBarSaveProgress.Size = new System.Drawing.Size(100, 22);
            this.toolStripProgressBarSaveProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.toolStripProgressBarSaveProgress.ToolTipText = "Save progress";
            this.toolStripProgressBarSaveProgress.Value = 50;
            this.toolStripProgressBarSaveProgress.Visible = false;
            // 
            // toolStripLabelThreadQueue
            // 
            this.toolStripLabelThreadQueue.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.toolStripLabelThreadQueue.Name = "toolStripLabelThreadQueue";
            this.toolStripLabelThreadQueue.Size = new System.Drawing.Size(132, 23);
            this.toolStripLabelThreadQueue.Text = "Background processes:";
            this.toolStripLabelThreadQueue.Visible = false;
            // 
            // toolStripProgressBarThreadQueue
            // 
            this.toolStripProgressBarThreadQueue.ForeColor = System.Drawing.Color.Red;
            this.toolStripProgressBarThreadQueue.Name = "toolStripProgressBarThreadQueue";
            this.toolStripProgressBarThreadQueue.Size = new System.Drawing.Size(100, 22);
            this.toolStripProgressBarThreadQueue.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.toolStripProgressBarThreadQueue.ToolTipText = "Fetching Exifdata from media";
            this.toolStripProgressBarThreadQueue.Value = 50;
            this.toolStripProgressBarThreadQueue.Visible = false;
            this.toolStripProgressBarThreadQueue.Click += new System.EventHandler(this.toolStripProgressBarThreadQueue_Click);
            // 
            // toolStripStatusThreadQueueCount
            // 
            this.toolStripStatusThreadQueueCount.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.toolStripStatusThreadQueueCount.Name = "toolStripStatusThreadQueueCount";
            this.toolStripStatusThreadQueueCount.Size = new System.Drawing.Size(46, 19);
            this.toolStripStatusThreadQueueCount.Text = "Queue";
            // 
            // toolStripStatusAction
            // 
            this.toolStripStatusAction.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.toolStripStatusAction.Name = "toolStripStatusAction";
            this.toolStripStatusAction.Size = new System.Drawing.Size(102, 19);
            this.toolStripStatusAction.Text = "Waiting actions...";
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.kryptonWorkspaceMain);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Margin = new System.Windows.Forms.Padding(0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1214, 728);
            this.panelMain.TabIndex = 2;
            // 
            // kryptonWorkspaceMain
            // 
            this.kryptonWorkspaceMain.ActivePage = this.kryptonPageFolderSearchFilterFolder;
            this.kryptonWorkspaceMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonWorkspaceMain.Location = new System.Drawing.Point(0, 0);
            this.kryptonWorkspaceMain.Name = "kryptonWorkspaceMain";
            // 
            // 
            // 
            this.kryptonWorkspaceMain.Root.Children.AddRange(new System.ComponentModel.Component[] {
            this.kryptonWorkspaceCellFolderSearchFilter,
            this.kryptonWorkspaceCellMediaFiles,
            this.kryptonWorkspaceCellToolbox});
            this.kryptonWorkspaceMain.Root.UniqueName = "f0dd7d6331c4426ab0a5c0a429f4ea9b";
            this.kryptonWorkspaceMain.Root.WorkspaceControl = this.kryptonWorkspaceMain;
            this.kryptonWorkspaceMain.SeparatorStyle = Krypton.Toolkit.SeparatorStyle.HighProfile;
            this.kryptonWorkspaceMain.Size = new System.Drawing.Size(1214, 728);
            this.kryptonWorkspaceMain.TabIndex = 0;
            this.kryptonWorkspaceMain.TabStop = true;
            // 
            // kryptonPageFolderSearchFilterFolder
            // 
            this.kryptonPageFolderSearchFilterFolder.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageFolderSearchFilterFolder.Controls.Add(this.folderTreeViewFolder);
            this.kryptonPageFolderSearchFilterFolder.Flags = 65534;
            this.kryptonPageFolderSearchFilterFolder.LastVisibleSet = true;
            this.kryptonPageFolderSearchFilterFolder.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageFolderSearchFilterFolder.Name = "kryptonPageFolderSearchFilterFolder";
            this.kryptonPageFolderSearchFilterFolder.Size = new System.Drawing.Size(399, 601);
            this.kryptonPageFolderSearchFilterFolder.Text = "Folder";
            this.kryptonPageFolderSearchFilterFolder.TextDescription = "Browse folders on your device";
            this.kryptonPageFolderSearchFilterFolder.TextTitle = "Folder";
            this.kryptonPageFolderSearchFilterFolder.ToolTipTitle = "Browse folders on your device";
            this.kryptonPageFolderSearchFilterFolder.UniqueName = "70c41531c9904af0b0213b722bb7750d";
            this.kryptonPageFolderSearchFilterFolder.Enter += new System.EventHandler(this.kryptonPageFolderSearchFilterFolder_Enter);
            // 
            // folderTreeViewFolder
            // 
            this.folderTreeViewFolder.AllowDrop = true;
            this.folderTreeViewFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.folderTreeViewFolder.HideSelection = false;
            this.folderTreeViewFolder.LabelEdit = true;
            this.folderTreeViewFolder.Location = new System.Drawing.Point(0, 0);
            this.folderTreeViewFolder.Name = "folderTreeViewFolder";
            this.folderTreeViewFolder.Size = new System.Drawing.Size(399, 601);
            this.folderTreeViewFolder.TabIndex = 0;
            this.folderTreeViewFolder.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.folderTreeView1_AfterSelect);
            this.folderTreeViewFolder.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.folderTreeViewFolder_ItemDrag);
            this.folderTreeViewFolder.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.folderTreeViewFolder_NodeMouseClick);
            this.folderTreeViewFolder.DragDrop += new System.Windows.Forms.DragEventHandler(this.folderTreeViewFolder_DragDrop);
            this.folderTreeViewFolder.DragEnter += new System.Windows.Forms.DragEventHandler(this.folderTreeViewFolder_DragEnter);
            this.folderTreeViewFolder.DragOver += new System.Windows.Forms.DragEventHandler(this.folderTreeViewFolder_DragOver);
            this.folderTreeViewFolder.DragLeave += new System.EventHandler(this.folderTreeViewFolder_DragLeave);
            // 
            // kryptonWorkspaceCellFolderSearchFilter
            // 
            this.kryptonWorkspaceCellFolderSearchFilter.AllowPageDrag = true;
            this.kryptonWorkspaceCellFolderSearchFilter.AllowTabFocus = false;
            this.kryptonWorkspaceCellFolderSearchFilter.Button.CloseButtonAction = Krypton.Navigator.CloseButtonAction.None;
            this.kryptonWorkspaceCellFolderSearchFilter.Button.CloseButtonDisplay = Krypton.Navigator.ButtonDisplay.Hide;
            this.kryptonWorkspaceCellFolderSearchFilter.Name = "kryptonWorkspaceCellFolderSearchFilter";
            this.kryptonWorkspaceCellFolderSearchFilter.NavigatorMode = Krypton.Navigator.NavigatorMode.OutlookFull;
            this.kryptonWorkspaceCellFolderSearchFilter.Pages.AddRange(new Krypton.Navigator.KryptonPage[] {
            this.kryptonPageFolderSearchFilterFolder,
            this.kryptonPageFolderSearchFilterSearch,
            this.kryptonPageFolderSearchFilterFilter});
            this.kryptonWorkspaceCellFolderSearchFilter.SelectedIndex = 0;
            this.kryptonWorkspaceCellFolderSearchFilter.UniqueName = "7f1f5ae72b174949ac870f12642643a5";
            // 
            // kryptonPageFolderSearchFilterSearch
            // 
            this.kryptonPageFolderSearchFilterSearch.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageFolderSearchFilterSearch.Controls.Add(this.kryptonWorkspaceSearchFilter);
            this.kryptonPageFolderSearchFilterSearch.Flags = 65534;
            this.kryptonPageFolderSearchFilterSearch.LastVisibleSet = true;
            this.kryptonPageFolderSearchFilterSearch.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageFolderSearchFilterSearch.Name = "kryptonPageFolderSearchFilterSearch";
            this.kryptonPageFolderSearchFilterSearch.Size = new System.Drawing.Size(399, 572);
            this.kryptonPageFolderSearchFilterSearch.Text = "Search";
            this.kryptonPageFolderSearchFilterSearch.TextDescription = "Search for media files in database";
            this.kryptonPageFolderSearchFilterSearch.TextTitle = "Search";
            this.kryptonPageFolderSearchFilterSearch.ToolTipTitle = "Search for media files in database";
            this.kryptonPageFolderSearchFilterSearch.UniqueName = "408ae935c7f1463495958366914474e7";
            this.kryptonPageFolderSearchFilterSearch.Enter += new System.EventHandler(this.kryptonPageFolderSearchFilterSearch_Enter);
            // 
            // kryptonWorkspaceSearchFilter
            // 
            this.kryptonWorkspaceSearchFilter.ActivePage = this.kryptonPageSearchFiler;
            this.kryptonWorkspaceSearchFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonWorkspaceSearchFilter.Location = new System.Drawing.Point(0, 0);
            this.kryptonWorkspaceSearchFilter.Name = "kryptonWorkspaceSearchFilter";
            // 
            // 
            // 
            this.kryptonWorkspaceSearchFilter.Root.Children.AddRange(new System.ComponentModel.Component[] {
            this.kryptonWorkspaceCellSearchFiler,
            this.kryptonWorkspaceCellSearchFilterAction});
            this.kryptonWorkspaceSearchFilter.Root.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.kryptonWorkspaceSearchFilter.Root.UniqueName = "5007ad6954eb4341a5453ee45ac4505a";
            this.kryptonWorkspaceSearchFilter.Root.WorkspaceControl = this.kryptonWorkspaceSearchFilter;
            this.kryptonWorkspaceSearchFilter.Size = new System.Drawing.Size(399, 572);
            this.kryptonWorkspaceSearchFilter.TabIndex = 43;
            this.kryptonWorkspaceSearchFilter.TabStop = true;
            // 
            // kryptonPageSearchFiler
            // 
            this.kryptonPageSearchFiler.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageSearchFiler.AutoScroll = true;
            this.kryptonPageSearchFiler.AutoScrollMinSize = new System.Drawing.Size(100, 100);
            this.kryptonPageSearchFiler.Controls.Add(this.groupBoxSearchFileSystem);
            this.kryptonPageSearchFiler.Controls.Add(this.groupBoxSearchTags);
            this.kryptonPageSearchFiler.Controls.Add(this.groupBoxSearchPeople);
            this.kryptonPageSearchFiler.Controls.Add(this.groupBoxSearchMediaTaken);
            this.kryptonPageSearchFiler.Controls.Add(this.groupBoxSearchRating);
            this.kryptonPageSearchFiler.Controls.Add(this.groupBoxSearchKeywords);
            this.kryptonPageSearchFiler.Controls.Add(this.groupBoxSearchExtra);
            this.kryptonPageSearchFiler.Flags = 65534;
            this.kryptonPageSearchFiler.LastVisibleSet = true;
            this.kryptonPageSearchFiler.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageSearchFiler.Name = "kryptonPageSearchFiler";
            this.kryptonPageSearchFiler.Size = new System.Drawing.Size(399, 522);
            this.kryptonPageSearchFiler.Text = "Search Filter";
            this.kryptonPageSearchFiler.ToolTipTitle = "Page ToolTip";
            this.kryptonPageSearchFiler.UniqueName = "6322e48c4013478f8c3bf8f0b4f220ae";
            // 
            // groupBoxSearchFileSystem
            // 
            this.groupBoxSearchFileSystem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSearchFileSystem.Location = new System.Drawing.Point(3, 3);
            this.groupBoxSearchFileSystem.MinimumSize = new System.Drawing.Size(261, 55);
            this.groupBoxSearchFileSystem.Name = "groupBoxSearchFileSystem";
            // 
            // groupBoxSearchFileSystem.Panel
            // 
            this.groupBoxSearchFileSystem.Panel.Controls.Add(this.kryptonLabelSearchFilename);
            this.groupBoxSearchFileSystem.Panel.Controls.Add(this.kryptonLabelSearchDirectory);
            this.groupBoxSearchFileSystem.Panel.Controls.Add(this.kryptonTextBoxSearchFilename);
            this.groupBoxSearchFileSystem.Panel.Controls.Add(this.kryptonTextBoxSearchDirectory);
            this.groupBoxSearchFileSystem.Size = new System.Drawing.Size(261, 90);
            this.groupBoxSearchFileSystem.TabIndex = 28;
            this.groupBoxSearchFileSystem.Values.Heading = "FileSystem";
            // 
            // kryptonLabelSearchFilename
            // 
            this.kryptonLabelSearchFilename.Location = new System.Drawing.Point(6, 35);
            this.kryptonLabelSearchFilename.Name = "kryptonLabelSearchFilename";
            this.kryptonLabelSearchFilename.Size = new System.Drawing.Size(60, 18);
            this.kryptonLabelSearchFilename.TabIndex = 3;
            this.kryptonLabelSearchFilename.Values.Text = "Filename:";
            // 
            // kryptonLabelSearchDirectory
            // 
            this.kryptonLabelSearchDirectory.Location = new System.Drawing.Point(5, 6);
            this.kryptonLabelSearchDirectory.Name = "kryptonLabelSearchDirectory";
            this.kryptonLabelSearchDirectory.Size = new System.Drawing.Size(59, 18);
            this.kryptonLabelSearchDirectory.TabIndex = 2;
            this.kryptonLabelSearchDirectory.Values.Text = "Directory:";
            // 
            // kryptonTextBoxSearchFilename
            // 
            this.kryptonTextBoxSearchFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonTextBoxSearchFilename.Location = new System.Drawing.Point(114, 32);
            this.kryptonTextBoxSearchFilename.Name = "kryptonTextBoxSearchFilename";
            this.kryptonTextBoxSearchFilename.Size = new System.Drawing.Size(122, 21);
            this.kryptonTextBoxSearchFilename.TabIndex = 1;
            // 
            // kryptonTextBoxSearchDirectory
            // 
            this.kryptonTextBoxSearchDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonTextBoxSearchDirectory.Location = new System.Drawing.Point(114, 3);
            this.kryptonTextBoxSearchDirectory.Name = "kryptonTextBoxSearchDirectory";
            this.kryptonTextBoxSearchDirectory.Size = new System.Drawing.Size(122, 21);
            this.kryptonTextBoxSearchDirectory.TabIndex = 0;
            // 
            // groupBoxSearchTags
            // 
            this.groupBoxSearchTags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSearchTags.Location = new System.Drawing.Point(3, 274);
            this.groupBoxSearchTags.MinimumSize = new System.Drawing.Size(261, 277);
            this.groupBoxSearchTags.Name = "groupBoxSearchTags";
            // 
            // groupBoxSearchTags.Panel
            // 
            this.groupBoxSearchTags.Panel.Controls.Add(this.checkBoxSearchUseAndBetweenTextTagFields);
            this.groupBoxSearchTags.Panel.Controls.Add(this.label3);
            this.groupBoxSearchTags.Panel.Controls.Add(this.comboBoxSearchLocationState);
            this.groupBoxSearchTags.Panel.Controls.Add(this.comboBoxSearchAlbum);
            this.groupBoxSearchTags.Panel.Controls.Add(this.comboBoxSearchComments);
            this.groupBoxSearchTags.Panel.Controls.Add(this.comboBoxSearchTitle);
            this.groupBoxSearchTags.Panel.Controls.Add(this.label13);
            this.groupBoxSearchTags.Panel.Controls.Add(this.label10);
            this.groupBoxSearchTags.Panel.Controls.Add(this.comboBoxSearchLocationName);
            this.groupBoxSearchTags.Panel.Controls.Add(this.comboBoxSearchLocationCity);
            this.groupBoxSearchTags.Panel.Controls.Add(this.label8);
            this.groupBoxSearchTags.Panel.Controls.Add(this.comboBoxSearchDescription);
            this.groupBoxSearchTags.Panel.Controls.Add(this.label6);
            this.groupBoxSearchTags.Panel.Controls.Add(this.label11);
            this.groupBoxSearchTags.Panel.Controls.Add(this.comboBoxSearchLocationCountry);
            this.groupBoxSearchTags.Panel.Controls.Add(this.label12);
            this.groupBoxSearchTags.Panel.Controls.Add(this.label7);
            this.groupBoxSearchTags.Size = new System.Drawing.Size(261, 277);
            this.groupBoxSearchTags.TabIndex = 9;
            this.groupBoxSearchTags.Values.Heading = "Search inside text field tags:";
            // 
            // checkBoxSearchUseAndBetweenTextTagFields
            // 
            this.checkBoxSearchUseAndBetweenTextTagFields.Location = new System.Drawing.Point(108, 219);
            this.checkBoxSearchUseAndBetweenTextTagFields.Name = "checkBoxSearchUseAndBetweenTextTagFields";
            this.checkBoxSearchUseAndBetweenTextTagFields.Size = new System.Drawing.Size(89, 18);
            this.checkBoxSearchUseAndBetweenTextTagFields.TabIndex = 43;
            this.checkBoxSearchUseAndBetweenTextTagFields.Tag = "NeedAllToFir";
            this.checkBoxSearchUseAndBetweenTextTagFields.Values.Text = "Need all to fit";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(3, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 18);
            this.label3.TabIndex = 27;
            this.label3.Values.Text = "Album:";
            // 
            // comboBoxSearchLocationState
            // 
            this.comboBoxSearchLocationState.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSearchLocationState.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxSearchLocationState.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxSearchLocationState.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxSearchLocationState.DropDownWidth = 183;
            this.comboBoxSearchLocationState.IntegralHeight = false;
            this.comboBoxSearchLocationState.Items.AddRange(new object[] {
            "List of States"});
            this.comboBoxSearchLocationState.Location = new System.Drawing.Point(108, 165);
            this.comboBoxSearchLocationState.Name = "comboBoxSearchLocationState";
            this.comboBoxSearchLocationState.Size = new System.Drawing.Size(129, 21);
            this.comboBoxSearchLocationState.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxSearchLocationState.TabIndex = 41;
            this.comboBoxSearchLocationState.Tag = "State";
            this.comboBoxSearchLocationState.ToolTipValues.Description = "List of suggested States";
            // 
            // comboBoxSearchAlbum
            // 
            this.comboBoxSearchAlbum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSearchAlbum.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxSearchAlbum.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxSearchAlbum.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxSearchAlbum.DropDownWidth = 183;
            this.comboBoxSearchAlbum.IntegralHeight = false;
            this.comboBoxSearchAlbum.Items.AddRange(new object[] {
            "List of Albums"});
            this.comboBoxSearchAlbum.Location = new System.Drawing.Point(108, 2);
            this.comboBoxSearchAlbum.Name = "comboBoxSearchAlbum";
            this.comboBoxSearchAlbum.Size = new System.Drawing.Size(129, 21);
            this.comboBoxSearchAlbum.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxSearchAlbum.TabIndex = 35;
            this.comboBoxSearchAlbum.Tag = "Album";
            this.comboBoxSearchAlbum.ToolTipValues.Description = "List of suggested Albums";
            // 
            // comboBoxSearchComments
            // 
            this.comboBoxSearchComments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSearchComments.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxSearchComments.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxSearchComments.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxSearchComments.DropDownWidth = 290;
            this.comboBoxSearchComments.IntegralHeight = false;
            this.comboBoxSearchComments.Items.AddRange(new object[] {
            "List of Comments"});
            this.comboBoxSearchComments.Location = new System.Drawing.Point(108, 83);
            this.comboBoxSearchComments.Name = "comboBoxSearchComments";
            this.comboBoxSearchComments.Size = new System.Drawing.Size(129, 21);
            this.comboBoxSearchComments.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxSearchComments.TabIndex = 38;
            this.comboBoxSearchComments.Tag = "Comments";
            this.comboBoxSearchComments.ToolTipValues.Description = "List of suggested Comments";
            // 
            // comboBoxSearchTitle
            // 
            this.comboBoxSearchTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSearchTitle.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxSearchTitle.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxSearchTitle.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxSearchTitle.DropDownWidth = 183;
            this.comboBoxSearchTitle.IntegralHeight = false;
            this.comboBoxSearchTitle.Items.AddRange(new object[] {
            "List of Titles"});
            this.comboBoxSearchTitle.Location = new System.Drawing.Point(108, 30);
            this.comboBoxSearchTitle.Name = "comboBoxSearchTitle";
            this.comboBoxSearchTitle.Size = new System.Drawing.Size(129, 21);
            this.comboBoxSearchTitle.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxSearchTitle.TabIndex = 36;
            this.comboBoxSearchTitle.Tag = "Title";
            this.comboBoxSearchTitle.ToolTipValues.Description = "List of suggested Titles";
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(3, 111);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(56, 18);
            this.label13.TabIndex = 31;
            this.label13.Values.Text = "Location:";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(3, 193);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 18);
            this.label10.TabIndex = 34;
            this.label10.Values.Text = "Country:";
            // 
            // comboBoxSearchLocationName
            // 
            this.comboBoxSearchLocationName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSearchLocationName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxSearchLocationName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxSearchLocationName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxSearchLocationName.DropDownWidth = 183;
            this.comboBoxSearchLocationName.IntegralHeight = false;
            this.comboBoxSearchLocationName.Items.AddRange(new object[] {
            "List of Locations"});
            this.comboBoxSearchLocationName.Location = new System.Drawing.Point(108, 110);
            this.comboBoxSearchLocationName.Name = "comboBoxSearchLocationName";
            this.comboBoxSearchLocationName.Size = new System.Drawing.Size(129, 21);
            this.comboBoxSearchLocationName.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxSearchLocationName.TabIndex = 39;
            this.comboBoxSearchLocationName.Tag = "Location";
            this.comboBoxSearchLocationName.ToolTipValues.Description = "List of suggested Location names";
            // 
            // comboBoxSearchLocationCity
            // 
            this.comboBoxSearchLocationCity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSearchLocationCity.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxSearchLocationCity.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxSearchLocationCity.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxSearchLocationCity.DropDownWidth = 183;
            this.comboBoxSearchLocationCity.IntegralHeight = false;
            this.comboBoxSearchLocationCity.Items.AddRange(new object[] {
            "List of Cities"});
            this.comboBoxSearchLocationCity.Location = new System.Drawing.Point(108, 138);
            this.comboBoxSearchLocationCity.Name = "comboBoxSearchLocationCity";
            this.comboBoxSearchLocationCity.Size = new System.Drawing.Size(129, 21);
            this.comboBoxSearchLocationCity.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxSearchLocationCity.TabIndex = 40;
            this.comboBoxSearchLocationCity.Tag = "City";
            this.comboBoxSearchLocationCity.ToolTipValues.Description = "List of suggested Cities";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(3, 84);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(68, 18);
            this.label8.TabIndex = 30;
            this.label8.Values.Text = "Comments:";
            // 
            // comboBoxSearchDescription
            // 
            this.comboBoxSearchDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSearchDescription.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxSearchDescription.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxSearchDescription.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxSearchDescription.DropDownWidth = 183;
            this.comboBoxSearchDescription.IntegralHeight = false;
            this.comboBoxSearchDescription.Items.AddRange(new object[] {
            "List of Desxriptions"});
            this.comboBoxSearchDescription.Location = new System.Drawing.Point(108, 56);
            this.comboBoxSearchDescription.Name = "comboBoxSearchDescription";
            this.comboBoxSearchDescription.Size = new System.Drawing.Size(129, 21);
            this.comboBoxSearchDescription.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxSearchDescription.TabIndex = 37;
            this.comboBoxSearchDescription.Tag = "Description";
            this.comboBoxSearchDescription.ToolTipValues.Description = "List of suggested Descriptions";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(3, 30);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 18);
            this.label6.TabIndex = 28;
            this.label6.Values.Text = "Title:";
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(3, 166);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(40, 18);
            this.label11.TabIndex = 33;
            this.label11.Values.Text = "State:";
            // 
            // comboBoxSearchLocationCountry
            // 
            this.comboBoxSearchLocationCountry.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSearchLocationCountry.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxSearchLocationCountry.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxSearchLocationCountry.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxSearchLocationCountry.DropDownWidth = 183;
            this.comboBoxSearchLocationCountry.IntegralHeight = false;
            this.comboBoxSearchLocationCountry.Items.AddRange(new object[] {
            "List of Contries"});
            this.comboBoxSearchLocationCountry.Location = new System.Drawing.Point(108, 192);
            this.comboBoxSearchLocationCountry.Name = "comboBoxSearchLocationCountry";
            this.comboBoxSearchLocationCountry.Size = new System.Drawing.Size(129, 21);
            this.comboBoxSearchLocationCountry.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxSearchLocationCountry.TabIndex = 42;
            this.comboBoxSearchLocationCountry.Tag = "Country";
            this.comboBoxSearchLocationCountry.ToolTipValues.Description = "List of suggested Countries";
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(3, 139);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(33, 18);
            this.label12.TabIndex = 32;
            this.label12.Values.Text = "City:";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(3, 57);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(70, 18);
            this.label7.TabIndex = 29;
            this.label7.Values.Text = "Description:";
            // 
            // groupBoxSearchPeople
            // 
            this.groupBoxSearchPeople.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSearchPeople.Location = new System.Drawing.Point(3, 760);
            this.groupBoxSearchPeople.MinimumSize = new System.Drawing.Size(261, 274);
            this.groupBoxSearchPeople.Name = "groupBoxSearchPeople";
            // 
            // groupBoxSearchPeople.Panel
            // 
            this.groupBoxSearchPeople.Panel.Controls.Add(this.panel5);
            this.groupBoxSearchPeople.Size = new System.Drawing.Size(261, 274);
            this.groupBoxSearchPeople.TabIndex = 27;
            this.groupBoxSearchPeople.Values.Heading = "People:";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.checkedListBoxSearchPeople);
            this.panel5.Controls.Add(this.checkBoxSearchNeedAllNames);
            this.panel5.Controls.Add(this.checkBoxSearchWithoutRegions);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Margin = new System.Windows.Forms.Padding(0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(257, 254);
            this.panel5.TabIndex = 16;
            // 
            // checkedListBoxSearchPeople
            // 
            this.checkedListBoxSearchPeople.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBoxSearchPeople.Location = new System.Drawing.Point(5, 53);
            this.checkedListBoxSearchPeople.Name = "checkedListBoxSearchPeople";
            this.checkedListBoxSearchPeople.Size = new System.Drawing.Size(231, 180);
            this.checkedListBoxSearchPeople.TabIndex = 45;
            this.checkedListBoxSearchPeople.Tag = "SearchPeople";
            // 
            // checkBoxSearchNeedAllNames
            // 
            this.checkBoxSearchNeedAllNames.Checked = true;
            this.checkBoxSearchNeedAllNames.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSearchNeedAllNames.Location = new System.Drawing.Point(6, 3);
            this.checkBoxSearchNeedAllNames.Name = "checkBoxSearchNeedAllNames";
            this.checkBoxSearchNeedAllNames.Size = new System.Drawing.Size(144, 18);
            this.checkBoxSearchNeedAllNames.TabIndex = 46;
            this.checkBoxSearchNeedAllNames.Values.Text = "When contain all names";
            // 
            // checkBoxSearchWithoutRegions
            // 
            this.checkBoxSearchWithoutRegions.Location = new System.Drawing.Point(6, 28);
            this.checkBoxSearchWithoutRegions.Name = "checkBoxSearchWithoutRegions";
            this.checkBoxSearchWithoutRegions.Size = new System.Drawing.Size(136, 18);
            this.checkBoxSearchWithoutRegions.TabIndex = 47;
            this.checkBoxSearchWithoutRegions.Values.Text = "Or whitout any regions";
            // 
            // groupBoxSearchMediaTaken
            // 
            this.groupBoxSearchMediaTaken.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSearchMediaTaken.Location = new System.Drawing.Point(3, 99);
            this.groupBoxSearchMediaTaken.MinimumSize = new System.Drawing.Size(261, 110);
            this.groupBoxSearchMediaTaken.Name = "groupBoxSearchMediaTaken";
            // 
            // groupBoxSearchMediaTaken.Panel
            // 
            this.groupBoxSearchMediaTaken.Panel.Controls.Add(this.checkBoxSearchMediaTakenIsNull);
            this.groupBoxSearchMediaTaken.Panel.Controls.Add(this.label14);
            this.groupBoxSearchMediaTaken.Panel.Controls.Add(this.dateTimePickerSearchDateFrom);
            this.groupBoxSearchMediaTaken.Panel.Controls.Add(this.dateTimePickerSearchDateTo);
            this.groupBoxSearchMediaTaken.Panel.Controls.Add(this.label17);
            this.groupBoxSearchMediaTaken.Size = new System.Drawing.Size(261, 110);
            this.groupBoxSearchMediaTaken.TabIndex = 9;
            this.groupBoxSearchMediaTaken.Values.Heading = "Media taken:";
            // 
            // checkBoxSearchMediaTakenIsNull
            // 
            this.checkBoxSearchMediaTakenIsNull.Location = new System.Drawing.Point(6, 58);
            this.checkBoxSearchMediaTakenIsNull.Name = "checkBoxSearchMediaTakenIsNull";
            this.checkBoxSearchMediaTakenIsNull.Size = new System.Drawing.Size(138, 18);
            this.checkBoxSearchMediaTakenIsNull.TabIndex = 42;
            this.checkBoxSearchMediaTakenIsNull.Tag = "OrWhenMissingValue";
            this.checkBoxSearchMediaTakenIsNull.Values.Text = "Or when missing value";
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(3, 5);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(85, 18);
            this.label14.TabIndex = 39;
            this.label14.Values.Text = "DateTaken >=:";
            // 
            // dateTimePickerSearchDateFrom
            // 
            this.dateTimePickerSearchDateFrom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerSearchDateFrom.Checked = false;
            this.dateTimePickerSearchDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerSearchDateFrom.Location = new System.Drawing.Point(108, 4);
            this.dateTimePickerSearchDateFrom.MaxDate = new System.DateTime(2200, 12, 31, 0, 0, 0, 0);
            this.dateTimePickerSearchDateFrom.MinDate = new System.DateTime(1900, 1, 1, 0, 0, 0, 0);
            this.dateTimePickerSearchDateFrom.Name = "dateTimePickerSearchDateFrom";
            this.dateTimePickerSearchDateFrom.ShowCheckBox = true;
            this.dateTimePickerSearchDateFrom.Size = new System.Drawing.Size(129, 21);
            this.dateTimePickerSearchDateFrom.TabIndex = 38;
            this.dateTimePickerSearchDateFrom.Tag = "DateTakenFrom";
            // 
            // dateTimePickerSearchDateTo
            // 
            this.dateTimePickerSearchDateTo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerSearchDateTo.Checked = false;
            this.dateTimePickerSearchDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerSearchDateTo.Location = new System.Drawing.Point(108, 31);
            this.dateTimePickerSearchDateTo.MaxDate = new System.DateTime(2200, 12, 31, 0, 0, 0, 0);
            this.dateTimePickerSearchDateTo.MinDate = new System.DateTime(1900, 1, 1, 0, 0, 0, 0);
            this.dateTimePickerSearchDateTo.Name = "dateTimePickerSearchDateTo";
            this.dateTimePickerSearchDateTo.ShowCheckBox = true;
            this.dateTimePickerSearchDateTo.Size = new System.Drawing.Size(128, 21);
            this.dateTimePickerSearchDateTo.TabIndex = 41;
            this.dateTimePickerSearchDateTo.Tag = "DateTakenTo";
            // 
            // label17
            // 
            this.label17.Location = new System.Drawing.Point(3, 32);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(85, 18);
            this.label17.TabIndex = 40;
            this.label17.Values.Text = "DateTaken <=:";
            // 
            // groupBoxSearchRating
            // 
            this.groupBoxSearchRating.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSearchRating.Location = new System.Drawing.Point(3, 215);
            this.groupBoxSearchRating.MinimumSize = new System.Drawing.Size(261, 53);
            this.groupBoxSearchRating.Name = "groupBoxSearchRating";
            // 
            // groupBoxSearchRating.Panel
            // 
            this.groupBoxSearchRating.Panel.Controls.Add(this.checkBoxSearchRatingEmpty);
            this.groupBoxSearchRating.Panel.Controls.Add(this.checkBoxSearchRating1);
            this.groupBoxSearchRating.Panel.Controls.Add(this.checkBoxSearchRating5);
            this.groupBoxSearchRating.Panel.Controls.Add(this.checkBoxSearchRating0);
            this.groupBoxSearchRating.Panel.Controls.Add(this.checkBoxSearchRating4);
            this.groupBoxSearchRating.Panel.Controls.Add(this.checkBoxSearchRating2);
            this.groupBoxSearchRating.Panel.Controls.Add(this.checkBoxSearchRating3);
            this.groupBoxSearchRating.Size = new System.Drawing.Size(261, 53);
            this.groupBoxSearchRating.TabIndex = 10;
            this.groupBoxSearchRating.Values.Heading = "Rating";
            // 
            // checkBoxSearchRatingEmpty
            // 
            this.checkBoxSearchRatingEmpty.Location = new System.Drawing.Point(225, 4);
            this.checkBoxSearchRatingEmpty.Name = "checkBoxSearchRatingEmpty";
            this.checkBoxSearchRatingEmpty.Size = new System.Drawing.Size(29, 18);
            this.checkBoxSearchRatingEmpty.TabIndex = 44;
            this.checkBoxSearchRatingEmpty.Values.Text = "?";
            // 
            // checkBoxSearchRating1
            // 
            this.checkBoxSearchRating1.Location = new System.Drawing.Point(6, 4);
            this.checkBoxSearchRating1.Name = "checkBoxSearchRating1";
            this.checkBoxSearchRating1.Size = new System.Drawing.Size(29, 18);
            this.checkBoxSearchRating1.TabIndex = 38;
            this.checkBoxSearchRating1.Values.Text = "1";
            // 
            // checkBoxSearchRating5
            // 
            this.checkBoxSearchRating5.Location = new System.Drawing.Point(150, 4);
            this.checkBoxSearchRating5.Name = "checkBoxSearchRating5";
            this.checkBoxSearchRating5.Size = new System.Drawing.Size(29, 18);
            this.checkBoxSearchRating5.TabIndex = 42;
            this.checkBoxSearchRating5.Values.Text = "5";
            // 
            // checkBoxSearchRating0
            // 
            this.checkBoxSearchRating0.Location = new System.Drawing.Point(186, 4);
            this.checkBoxSearchRating0.Name = "checkBoxSearchRating0";
            this.checkBoxSearchRating0.Size = new System.Drawing.Size(29, 18);
            this.checkBoxSearchRating0.TabIndex = 43;
            this.checkBoxSearchRating0.Values.Text = "0";
            // 
            // checkBoxSearchRating4
            // 
            this.checkBoxSearchRating4.Location = new System.Drawing.Point(114, 4);
            this.checkBoxSearchRating4.Name = "checkBoxSearchRating4";
            this.checkBoxSearchRating4.Size = new System.Drawing.Size(29, 18);
            this.checkBoxSearchRating4.TabIndex = 41;
            this.checkBoxSearchRating4.Values.Text = "4";
            // 
            // checkBoxSearchRating2
            // 
            this.checkBoxSearchRating2.Location = new System.Drawing.Point(42, 4);
            this.checkBoxSearchRating2.Name = "checkBoxSearchRating2";
            this.checkBoxSearchRating2.Size = new System.Drawing.Size(29, 18);
            this.checkBoxSearchRating2.TabIndex = 39;
            this.checkBoxSearchRating2.Values.Text = "2";
            // 
            // checkBoxSearchRating3
            // 
            this.checkBoxSearchRating3.Location = new System.Drawing.Point(78, 4);
            this.checkBoxSearchRating3.Name = "checkBoxSearchRating3";
            this.checkBoxSearchRating3.Size = new System.Drawing.Size(29, 18);
            this.checkBoxSearchRating3.TabIndex = 40;
            this.checkBoxSearchRating3.Values.Text = "3";
            // 
            // groupBoxSearchKeywords
            // 
            this.groupBoxSearchKeywords.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSearchKeywords.Location = new System.Drawing.Point(3, 618);
            this.groupBoxSearchKeywords.MinimumSize = new System.Drawing.Size(261, 136);
            this.groupBoxSearchKeywords.Name = "groupBoxSearchKeywords";
            // 
            // groupBoxSearchKeywords.Panel
            // 
            this.groupBoxSearchKeywords.Panel.Controls.Add(this.label9);
            this.groupBoxSearchKeywords.Panel.Controls.Add(this.label15);
            this.groupBoxSearchKeywords.Panel.Controls.Add(this.checkBoxSearchNeedAllKeywords);
            this.groupBoxSearchKeywords.Panel.Controls.Add(this.checkBoxSearchWithoutKeyword);
            this.groupBoxSearchKeywords.Panel.Controls.Add(this.comboBoxSearchKeyword);
            this.groupBoxSearchKeywords.Size = new System.Drawing.Size(261, 136);
            this.groupBoxSearchKeywords.TabIndex = 9;
            this.groupBoxSearchKeywords.Values.Heading = "Keywords";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(104, 29);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(136, 18);
            this.label9.TabIndex = 49;
            this.label9.Values.Text = "Separate keywords with ;";
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(3, 3);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(57, 18);
            this.label15.TabIndex = 45;
            this.label15.Values.Text = "Keyword:";
            // 
            // checkBoxSearchNeedAllKeywords
            // 
            this.checkBoxSearchNeedAllKeywords.Checked = true;
            this.checkBoxSearchNeedAllKeywords.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSearchNeedAllKeywords.Location = new System.Drawing.Point(6, 55);
            this.checkBoxSearchNeedAllKeywords.Name = "checkBoxSearchNeedAllKeywords";
            this.checkBoxSearchNeedAllKeywords.Size = new System.Drawing.Size(158, 18);
            this.checkBoxSearchNeedAllKeywords.TabIndex = 48;
            this.checkBoxSearchNeedAllKeywords.Values.Text = "When contain all keywords";
            // 
            // checkBoxSearchWithoutKeyword
            // 
            this.checkBoxSearchWithoutKeyword.Location = new System.Drawing.Point(6, 81);
            this.checkBoxSearchWithoutKeyword.Name = "checkBoxSearchWithoutKeyword";
            this.checkBoxSearchWithoutKeyword.Size = new System.Drawing.Size(147, 18);
            this.checkBoxSearchWithoutKeyword.TabIndex = 47;
            this.checkBoxSearchWithoutKeyword.Values.Text = "Or without any keywords";
            // 
            // comboBoxSearchKeyword
            // 
            this.comboBoxSearchKeyword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSearchKeyword.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxSearchKeyword.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxSearchKeyword.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxSearchKeyword.DropDownWidth = 183;
            this.comboBoxSearchKeyword.IntegralHeight = false;
            this.comboBoxSearchKeyword.Location = new System.Drawing.Point(108, 2);
            this.comboBoxSearchKeyword.Name = "comboBoxSearchKeyword";
            this.comboBoxSearchKeyword.Size = new System.Drawing.Size(128, 21);
            this.comboBoxSearchKeyword.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxSearchKeyword.TabIndex = 46;
            this.comboBoxSearchKeyword.Tag = "Keywords";
            // 
            // groupBoxSearchExtra
            // 
            this.groupBoxSearchExtra.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSearchExtra.Location = new System.Drawing.Point(3, 557);
            this.groupBoxSearchExtra.MinimumSize = new System.Drawing.Size(261, 55);
            this.groupBoxSearchExtra.Name = "groupBoxSearchExtra";
            // 
            // groupBoxSearchExtra.Panel
            // 
            this.groupBoxSearchExtra.Panel.Controls.Add(this.checkBoxSearchHasWarning);
            this.groupBoxSearchExtra.Size = new System.Drawing.Size(261, 55);
            this.groupBoxSearchExtra.TabIndex = 9;
            this.groupBoxSearchExtra.Values.Heading = "Attributes:";
            // 
            // checkBoxSearchHasWarning
            // 
            this.checkBoxSearchHasWarning.Location = new System.Drawing.Point(107, 6);
            this.checkBoxSearchHasWarning.Name = "checkBoxSearchHasWarning";
            this.checkBoxSearchHasWarning.Size = new System.Drawing.Size(148, 18);
            this.checkBoxSearchHasWarning.TabIndex = 30;
            this.checkBoxSearchHasWarning.Values.Text = "Has warning message(s)";
            // 
            // kryptonWorkspaceCellSearchFiler
            // 
            this.kryptonWorkspaceCellSearchFiler.AllowPageDrag = true;
            this.kryptonWorkspaceCellSearchFiler.AllowTabFocus = false;
            this.kryptonWorkspaceCellSearchFiler.Button.CloseButtonAction = Krypton.Navigator.CloseButtonAction.None;
            this.kryptonWorkspaceCellSearchFiler.Button.CloseButtonDisplay = Krypton.Navigator.ButtonDisplay.Hide;
            this.kryptonWorkspaceCellSearchFiler.Name = "kryptonWorkspaceCellSearchFiler";
            this.kryptonWorkspaceCellSearchFiler.NavigatorMode = Krypton.Navigator.NavigatorMode.Panel;
            this.kryptonWorkspaceCellSearchFiler.Pages.AddRange(new Krypton.Navigator.KryptonPage[] {
            this.kryptonPageSearchFiler});
            this.kryptonWorkspaceCellSearchFiler.SelectedIndex = 0;
            this.kryptonWorkspaceCellSearchFiler.UniqueName = "ab26a27670ff4e65af82413befa923c5";
            // 
            // kryptonWorkspaceCellSearchFilterAction
            // 
            this.kryptonWorkspaceCellSearchFilterAction.AllowPageDrag = true;
            this.kryptonWorkspaceCellSearchFilterAction.AllowTabFocus = false;
            this.kryptonWorkspaceCellSearchFilterAction.Name = "kryptonWorkspaceCellSearchFilterAction";
            this.kryptonWorkspaceCellSearchFilterAction.NavigatorMode = Krypton.Navigator.NavigatorMode.Panel;
            this.kryptonWorkspaceCellSearchFilterAction.Pages.AddRange(new Krypton.Navigator.KryptonPage[] {
            this.kryptonPageSearchFilterAction});
            this.kryptonWorkspaceCellSearchFilterAction.SelectedIndex = 0;
            this.kryptonWorkspaceCellSearchFilterAction.StarSize = "50*,45";
            this.kryptonWorkspaceCellSearchFilterAction.UniqueName = "61505220abf44dd890138f2c3d8e0e7e";
            // 
            // kryptonPageSearchFilterAction
            // 
            this.kryptonPageSearchFilterAction.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageSearchFilterAction.Controls.Add(this.kryptonCheckBoxSearchUseRegEx);
            this.kryptonPageSearchFilterAction.Controls.Add(this.buttonSearch);
            this.kryptonPageSearchFilterAction.Controls.Add(this.checkBoxSerachFitsAllValues);
            this.kryptonPageSearchFilterAction.Flags = 65534;
            this.kryptonPageSearchFilterAction.LastVisibleSet = true;
            this.kryptonPageSearchFilterAction.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageSearchFilterAction.Name = "kryptonPageSearchFilterAction";
            this.kryptonPageSearchFilterAction.Size = new System.Drawing.Size(399, 50);
            this.kryptonPageSearchFilterAction.Text = "Search filter actions";
            this.kryptonPageSearchFilterAction.ToolTipTitle = "Page ToolTip";
            this.kryptonPageSearchFilterAction.UniqueName = "f5d8c6f255674566b2ec626051f2ca4f";
            // 
            // kryptonCheckBoxSearchUseRegEx
            // 
            this.kryptonCheckBoxSearchUseRegEx.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.kryptonCheckBoxSearchUseRegEx.Enabled = false;
            this.kryptonCheckBoxSearchUseRegEx.Location = new System.Drawing.Point(8, 27);
            this.kryptonCheckBoxSearchUseRegEx.Name = "kryptonCheckBoxSearchUseRegEx";
            this.kryptonCheckBoxSearchUseRegEx.Size = new System.Drawing.Size(80, 18);
            this.kryptonCheckBoxSearchUseRegEx.TabIndex = 43;
            this.kryptonCheckBoxSearchUseRegEx.Values.Text = "Use RexEx";
            this.kryptonCheckBoxSearchUseRegEx.Visible = false;
            // 
            // buttonSearch
            // 
            this.buttonSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSearch.Location = new System.Drawing.Point(244, 7);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(142, 35);
            this.buttonSearch.TabIndex = 42;
            this.buttonSearch.Values.Text = "Search";
            this.buttonSearch.Click += new System.EventHandler(this.buttonFilterSearch_Click);
            // 
            // checkBoxSerachFitsAllValues
            // 
            this.checkBoxSerachFitsAllValues.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxSerachFitsAllValues.Checked = true;
            this.checkBoxSerachFitsAllValues.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSerachFitsAllValues.Location = new System.Drawing.Point(8, 5);
            this.checkBoxSerachFitsAllValues.Name = "checkBoxSerachFitsAllValues";
            this.checkBoxSerachFitsAllValues.Size = new System.Drawing.Size(233, 18);
            this.checkBoxSerachFitsAllValues.TabIndex = 26;
            this.checkBoxSerachFitsAllValues.Values.Text = "Use And (All needs to fit, otherwise some)";
            // 
            // kryptonPageFolderSearchFilterFilter
            // 
            this.kryptonPageFolderSearchFilterFilter.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageFolderSearchFilterFilter.Controls.Add(this.treeViewFilter);
            this.kryptonPageFolderSearchFilterFilter.Flags = 65534;
            this.kryptonPageFolderSearchFilterFilter.LastVisibleSet = true;
            this.kryptonPageFolderSearchFilterFilter.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageFolderSearchFilterFilter.Name = "kryptonPageFolderSearchFilterFilter";
            this.kryptonPageFolderSearchFilterFilter.Size = new System.Drawing.Size(399, 600);
            this.kryptonPageFolderSearchFilterFilter.Text = "Filter";
            this.kryptonPageFolderSearchFilterFilter.TextDescription = "Filter media files from serach or folder selected";
            this.kryptonPageFolderSearchFilterFilter.TextTitle = "Filter";
            this.kryptonPageFolderSearchFilterFilter.ToolTipTitle = "Filter media files from serach or folder selected";
            this.kryptonPageFolderSearchFilterFilter.UniqueName = "67fe34cef6984ea0b5cd1d20a2f9e0f0";
            this.kryptonPageFolderSearchFilterFilter.Enter += new System.EventHandler(this.kryptonPageFolderSearchFilterFilter_Enter);
            // 
            // treeViewFilter
            // 
            this.treeViewFilter.CheckBoxes = true;
            this.treeViewFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewFilter.Location = new System.Drawing.Point(0, 0);
            this.treeViewFilter.Name = "treeViewFilter";
            treeNode2.Name = "NodeFolder";
            treeNode2.Tag = "Filter";
            treeNode2.Text = "Filter";
            this.treeViewFilter.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2});
            this.treeViewFilter.Size = new System.Drawing.Size(399, 600);
            this.treeViewFilter.TabIndex = 0;
            this.treeViewFilter.BeforeCheck += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeViewFilter_BeforeCheck);
            this.treeViewFilter.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeViewFilter_AfterCheck);
            // 
            // kryptonWorkspaceCellMediaFiles
            // 
            this.kryptonWorkspaceCellMediaFiles.AllowPageDrag = true;
            this.kryptonWorkspaceCellMediaFiles.AllowTabFocus = false;
            this.kryptonWorkspaceCellMediaFiles.Button.CloseButtonAction = Krypton.Navigator.CloseButtonAction.None;
            this.kryptonWorkspaceCellMediaFiles.Button.CloseButtonDisplay = Krypton.Navigator.ButtonDisplay.Hide;
            this.kryptonWorkspaceCellMediaFiles.Button.ContextButtonAction = Krypton.Navigator.ContextButtonAction.None;
            this.kryptonWorkspaceCellMediaFiles.Button.ContextButtonDisplay = Krypton.Navigator.ButtonDisplay.Hide;
            this.kryptonWorkspaceCellMediaFiles.Name = "kryptonWorkspaceCellMediaFiles";
            this.kryptonWorkspaceCellMediaFiles.NavigatorMode = Krypton.Navigator.NavigatorMode.HeaderGroup;
            this.kryptonWorkspaceCellMediaFiles.Pages.AddRange(new Krypton.Navigator.KryptonPage[] {
            this.kryptonPageMediaFiles});
            this.kryptonWorkspaceCellMediaFiles.SelectedIndex = 0;
            this.kryptonWorkspaceCellMediaFiles.UniqueName = "2a847c84f28146c08ce3d5c18cad4f8f";
            // 
            // kryptonPageMediaFiles
            // 
            this.kryptonPageMediaFiles.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageMediaFiles.Controls.Add(this.imageListView1);
            this.kryptonPageMediaFiles.Flags = 65534;
            this.kryptonPageMediaFiles.LastVisibleSet = true;
            this.kryptonPageMediaFiles.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageMediaFiles.Name = "kryptonPageMediaFiles";
            this.kryptonPageMediaFiles.Size = new System.Drawing.Size(399, 684);
            this.kryptonPageMediaFiles.Text = "Media files";
            this.kryptonPageMediaFiles.TextDescription = "List of media files from search result or selected folder";
            this.kryptonPageMediaFiles.TextTitle = "Media files";
            this.kryptonPageMediaFiles.ToolTipTitle = "List of media files from search result or selected folder";
            this.kryptonPageMediaFiles.UniqueName = "c65f9588ce604c629e8f8bc5d5908dfb";
            this.kryptonPageMediaFiles.Enter += new System.EventHandler(this.kryptonPageMediaFiles_Enter);
            // 
            // imageListView1
            // 
            this.imageListView1.AllowDrag = true;
            this.imageListView1.AllowDrop = true;
            this.imageListView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.imageListView1.CacheLimit = "0";
            this.imageListView1.CacheMode = Manina.Windows.Forms.CacheMode.Continuous;
            this.imageListView1.DefaultImage = ((System.Drawing.Image)(resources.GetObject("imageListView1.DefaultImage")));
            this.imageListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageListView1.ErrorImage = ((System.Drawing.Image)(resources.GetObject("imageListView1.ErrorImage")));
            this.imageListView1.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.imageListView1.ImeMode = System.Windows.Forms.ImeMode.Katakana;
            this.imageListView1.KryptonContextMenu = null;
            this.imageListView1.Location = new System.Drawing.Point(0, 0);
            this.imageListView1.Name = "imageListView1";
            this.imageListView1.RetryOnError = false;
            this.imageListView1.Size = new System.Drawing.Size(399, 684);
            this.imageListView1.SortColumn = Manina.Windows.Forms.ColumnType.FileName;
            this.imageListView1.TabIndex = 1;
            this.imageListView1.Text = "";
            this.imageListView1.TitleLine2 = Manina.Windows.Forms.ColumnType.FileName;
            this.imageListView1.TitleLine3 = Manina.Windows.Forms.ColumnType.FileName;
            this.imageListView1.TitleLine4 = Manina.Windows.Forms.ColumnType.FileName;
            this.imageListView1.TitleLine5 = Manina.Windows.Forms.ColumnType.FileName;
            this.imageListView1.ItemHover += new Manina.Windows.Forms.ItemHoverEventHandler(this.imageListView1_ItemHover);
            this.imageListView1.ItemDoubleClick += new Manina.Windows.Forms.ItemDoubleClickEventHandler(this.imageListView1_ItemDoubleClick);
            this.imageListView1.SelectionChanged += new System.EventHandler(this.imageListView1_SelectionChanged);
            this.imageListView1.ThumbnailCaching += new Manina.Windows.Forms.ThumbnailCachingEventHandler(this.imageListView1_ThumbnailCaching);
            this.imageListView1.RetrieveItemImage += new Manina.Windows.Forms.RetrieveItemImageEventHandler(this.imageListView1_RetrieveImage);
            this.imageListView1.RetrieveItemThumbnail += new Manina.Windows.Forms.RetrieveItemThumbnailEventHandler(this.imageListView1_RetrieveItemThumbnail);
            this.imageListView1.RetrieveItemMetadataDetails += new Manina.Windows.Forms.RetrieveItemMetadataDetailsEventHandler(this.imageListView1_RetrieveItemMetadataDetails);
            // 
            // kryptonWorkspaceCellToolbox
            // 
            this.kryptonWorkspaceCellToolbox.AllowPageDrag = true;
            this.kryptonWorkspaceCellToolbox.AllowTabFocus = false;
            this.kryptonWorkspaceCellToolbox.Button.CloseButtonAction = Krypton.Navigator.CloseButtonAction.None;
            this.kryptonWorkspaceCellToolbox.Button.CloseButtonDisplay = Krypton.Navigator.ButtonDisplay.Hide;
            this.kryptonWorkspaceCellToolbox.Name = "kryptonWorkspaceCellToolbox";
            this.kryptonWorkspaceCellToolbox.Pages.AddRange(new Krypton.Navigator.KryptonPage[] {
            this.kryptonPageToolboxTags,
            this.kryptonPageToolboxPeople,
            this.kryptonPageToolboxMap,
            this.kryptonPageToolboxDates,
            this.kryptonPageToolboxExiftool,
            this.kryptonPageToolboxWarnings,
            this.kryptonPageToolboxProperties,
            this.kryptonPageToolboxRename,
            this.kryptonPageToolboxConvertAndMerge});
            this.kryptonWorkspaceCellToolbox.SelectedIndex = 0;
            this.kryptonWorkspaceCellToolbox.UniqueName = "f75dbe6bb099427c9a831e9adb110255";
            this.kryptonWorkspaceCellToolbox.SelectedPageChanged += new System.EventHandler(this.kryptonWorkspaceCellToolbox_SelectedPageChanged);
            // 
            // kryptonPageToolboxTags
            // 
            this.kryptonPageToolboxTags.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageToolboxTags.Controls.Add(this.kryptonWorkspaceToolboxTags);
            this.kryptonPageToolboxTags.Flags = 65534;
            this.kryptonPageToolboxTags.LastVisibleSet = true;
            this.kryptonPageToolboxTags.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageToolboxTags.Name = "kryptonPageToolboxTags";
            this.kryptonPageToolboxTags.Size = new System.Drawing.Size(400, 701);
            this.kryptonPageToolboxTags.Tag = "Tags";
            this.kryptonPageToolboxTags.Text = "Tags";
            this.kryptonPageToolboxTags.TextDescription = "Edit tags and keywords";
            this.kryptonPageToolboxTags.TextTitle = "Tags";
            this.kryptonPageToolboxTags.ToolTipTitle = "Edit tags and keywords";
            this.kryptonPageToolboxTags.UniqueName = "15f06b43982b412c921df38443edd1f8";
            this.kryptonPageToolboxTags.Enter += new System.EventHandler(this.kryptonPageToolboxTags_Enter);
            // 
            // kryptonWorkspaceToolboxTags
            // 
            this.kryptonWorkspaceToolboxTags.ActivePage = this.kryptonPageToolboxTagsDetails;
            this.kryptonWorkspaceToolboxTags.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonWorkspaceToolboxTags.Location = new System.Drawing.Point(0, 0);
            this.kryptonWorkspaceToolboxTags.Name = "kryptonWorkspaceToolboxTags";
            // 
            // 
            // 
            this.kryptonWorkspaceToolboxTags.Root.Children.AddRange(new System.ComponentModel.Component[] {
            this.kryptonWorkspaceCellToolboxTagsDetails,
            this.kryptonWorkspaceCellToolboxTagsKeywords});
            this.kryptonWorkspaceToolboxTags.Root.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.kryptonWorkspaceToolboxTags.Root.StarSize = "200,50*";
            this.kryptonWorkspaceToolboxTags.Root.UniqueName = "5a3ba2c5cc184db6ac82fbd639c9f04f";
            this.kryptonWorkspaceToolboxTags.Root.WorkspaceControl = this.kryptonWorkspaceToolboxTags;
            this.kryptonWorkspaceToolboxTags.SeparatorStyle = Krypton.Toolkit.SeparatorStyle.HighProfile;
            this.kryptonWorkspaceToolboxTags.Size = new System.Drawing.Size(400, 701);
            this.kryptonWorkspaceToolboxTags.TabIndex = 0;
            this.kryptonWorkspaceToolboxTags.TabStop = true;
            // 
            // kryptonPageToolboxTagsDetails
            // 
            this.kryptonPageToolboxTagsDetails.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageToolboxTagsDetails.AutoScroll = true;
            this.kryptonPageToolboxTagsDetails.Controls.Add(this.kryptonGroupBoxTagsDetails);
            this.kryptonPageToolboxTagsDetails.Controls.Add(this.kryptonGroupBoxToolboxTagsTags);
            this.kryptonPageToolboxTagsDetails.Controls.Add(this.groupBoxRating);
            this.kryptonPageToolboxTagsDetails.Flags = 65534;
            this.kryptonPageToolboxTagsDetails.LastVisibleSet = true;
            this.kryptonPageToolboxTagsDetails.MinimumSize = new System.Drawing.Size(240, 100);
            this.kryptonPageToolboxTagsDetails.Name = "kryptonPageToolboxTagsDetails";
            this.kryptonPageToolboxTagsDetails.Size = new System.Drawing.Size(398, 304);
            this.kryptonPageToolboxTagsDetails.Text = "Tags Details";
            this.kryptonPageToolboxTagsDetails.TextDescription = "Edit media details";
            this.kryptonPageToolboxTagsDetails.TextTitle = "Tags Details";
            this.kryptonPageToolboxTagsDetails.ToolTipTitle = "Edit media details";
            this.kryptonPageToolboxTagsDetails.UniqueName = "f7053440e7e94c0b988482712a5e99a9";
            // 
            // kryptonGroupBoxTagsDetails
            // 
            this.kryptonGroupBoxTagsDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonGroupBoxTagsDetails.Location = new System.Drawing.Point(3, 7);
            this.kryptonGroupBoxTagsDetails.MinimumSize = new System.Drawing.Size(238, 166);
            this.kryptonGroupBoxTagsDetails.Name = "kryptonGroupBoxTagsDetails";
            // 
            // kryptonGroupBoxTagsDetails.Panel
            // 
            this.kryptonGroupBoxTagsDetails.Panel.Controls.Add(this.label4);
            this.kryptonGroupBoxTagsDetails.Panel.Controls.Add(this.comboBoxAuthor);
            this.kryptonGroupBoxTagsDetails.Panel.Controls.Add(this.labelTitle);
            this.kryptonGroupBoxTagsDetails.Panel.Controls.Add(this.comboBoxAlbum);
            this.kryptonGroupBoxTagsDetails.Panel.Controls.Add(this.comboBoxDescription);
            this.kryptonGroupBoxTagsDetails.Panel.Controls.Add(this.comboBoxComments);
            this.kryptonGroupBoxTagsDetails.Panel.Controls.Add(this.labelDescription);
            this.kryptonGroupBoxTagsDetails.Panel.Controls.Add(this.labelAuthor);
            this.kryptonGroupBoxTagsDetails.Panel.Controls.Add(this.comboBoxTitle);
            this.kryptonGroupBoxTagsDetails.Panel.Controls.Add(this.labelComments);
            this.kryptonGroupBoxTagsDetails.Size = new System.Drawing.Size(255, 166);
            this.kryptonGroupBoxTagsDetails.TabIndex = 20;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(3, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 18);
            this.label4.TabIndex = 20;
            this.label4.Values.Text = "Album";
            // 
            // comboBoxAuthor
            // 
            this.comboBoxAuthor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxAuthor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxAuthor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxAuthor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxAuthor.DropDownWidth = 121;
            this.comboBoxAuthor.IntegralHeight = false;
            this.comboBoxAuthor.Items.AddRange(new object[] {
            "a"});
            this.comboBoxAuthor.Location = new System.Drawing.Point(99, 108);
            this.comboBoxAuthor.Name = "comboBoxAuthor";
            this.comboBoxAuthor.Size = new System.Drawing.Size(136, 21);
            this.comboBoxAuthor.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxAuthor.TabIndex = 29;
            this.comboBoxAuthor.Tag = "Author";
            // 
            // labelTitle
            // 
            this.labelTitle.Location = new System.Drawing.Point(3, 31);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(32, 18);
            this.labelTitle.TabIndex = 21;
            this.labelTitle.Values.Text = "Title";
            // 
            // comboBoxAlbum
            // 
            this.comboBoxAlbum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxAlbum.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxAlbum.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxAlbum.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxAlbum.DropDownWidth = 290;
            this.comboBoxAlbum.IntegralHeight = false;
            this.comboBoxAlbum.Items.AddRange(new object[] {
            "sdd"});
            this.comboBoxAlbum.Location = new System.Drawing.Point(99, 3);
            this.comboBoxAlbum.Name = "comboBoxAlbum";
            this.comboBoxAlbum.Size = new System.Drawing.Size(136, 21);
            this.comboBoxAlbum.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxAlbum.TabIndex = 25;
            this.comboBoxAlbum.Tag = "Album";
            // 
            // comboBoxDescription
            // 
            this.comboBoxDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxDescription.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxDescription.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxDescription.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxDescription.DropDownWidth = 121;
            this.comboBoxDescription.IntegralHeight = false;
            this.comboBoxDescription.Items.AddRange(new object[] {
            "a"});
            this.comboBoxDescription.Location = new System.Drawing.Point(99, 54);
            this.comboBoxDescription.Name = "comboBoxDescription";
            this.comboBoxDescription.Size = new System.Drawing.Size(136, 21);
            this.comboBoxDescription.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxDescription.TabIndex = 27;
            this.comboBoxDescription.Tag = "Description";
            // 
            // comboBoxComments
            // 
            this.comboBoxComments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxComments.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxComments.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxComments.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxComments.DropDownWidth = 121;
            this.comboBoxComments.IntegralHeight = false;
            this.comboBoxComments.Items.AddRange(new object[] {
            "a"});
            this.comboBoxComments.Location = new System.Drawing.Point(99, 81);
            this.comboBoxComments.Name = "comboBoxComments";
            this.comboBoxComments.Size = new System.Drawing.Size(136, 21);
            this.comboBoxComments.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxComments.TabIndex = 28;
            this.comboBoxComments.Tag = "Comments";
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(3, 55);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(67, 18);
            this.labelDescription.TabIndex = 22;
            this.labelDescription.Values.Text = "Description";
            // 
            // labelAuthor
            // 
            this.labelAuthor.Location = new System.Drawing.Point(3, 109);
            this.labelAuthor.Name = "labelAuthor";
            this.labelAuthor.Size = new System.Drawing.Size(44, 18);
            this.labelAuthor.TabIndex = 24;
            this.labelAuthor.Values.Text = "Author";
            // 
            // comboBoxTitle
            // 
            this.comboBoxTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxTitle.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxTitle.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxTitle.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxTitle.DropDownWidth = 121;
            this.comboBoxTitle.IntegralHeight = false;
            this.comboBoxTitle.Items.AddRange(new object[] {
            "a"});
            this.comboBoxTitle.Location = new System.Drawing.Point(99, 29);
            this.comboBoxTitle.Name = "comboBoxTitle";
            this.comboBoxTitle.Size = new System.Drawing.Size(136, 21);
            this.comboBoxTitle.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxTitle.TabIndex = 26;
            this.comboBoxTitle.Tag = "Title";
            // 
            // labelComments
            // 
            this.labelComments.Location = new System.Drawing.Point(3, 82);
            this.labelComments.Name = "labelComments";
            this.labelComments.Size = new System.Drawing.Size(65, 18);
            this.labelComments.TabIndex = 23;
            this.labelComments.Values.Text = "Comments";
            // 
            // kryptonGroupBoxToolboxTagsTags
            // 
            this.kryptonGroupBoxToolboxTagsTags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonGroupBoxToolboxTagsTags.Location = new System.Drawing.Point(3, 229);
            this.kryptonGroupBoxToolboxTagsTags.MinimumSize = new System.Drawing.Size(238, 58);
            this.kryptonGroupBoxToolboxTagsTags.Name = "kryptonGroupBoxToolboxTagsTags";
            // 
            // kryptonGroupBoxToolboxTagsTags.Panel
            // 
            this.kryptonGroupBoxToolboxTagsTags.Panel.Controls.Add(this.label5);
            this.kryptonGroupBoxToolboxTagsTags.Panel.Controls.Add(this.comboBoxMediaAiConfidence);
            this.kryptonGroupBoxToolboxTagsTags.Size = new System.Drawing.Size(255, 58);
            this.kryptonGroupBoxToolboxTagsTags.TabIndex = 21;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(3, 4);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 18);
            this.label5.TabIndex = 7;
            this.label5.Values.Text = "Tags";
            // 
            // comboBoxMediaAiConfidence
            // 
            this.comboBoxMediaAiConfidence.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxMediaAiConfidence.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMediaAiConfidence.DropDownWidth = 198;
            this.comboBoxMediaAiConfidence.IntegralHeight = false;
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
            this.comboBoxMediaAiConfidence.Location = new System.Drawing.Point(96, 4);
            this.comboBoxMediaAiConfidence.Name = "comboBoxMediaAiConfidence";
            this.comboBoxMediaAiConfidence.Size = new System.Drawing.Size(134, 21);
            this.comboBoxMediaAiConfidence.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxMediaAiConfidence.TabIndex = 8;
            this.comboBoxMediaAiConfidence.SelectedIndexChanged += new System.EventHandler(this.comboBoxMediaAiConfidence_SelectedIndexChanged);
            // 
            // groupBoxRating
            // 
            this.groupBoxRating.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxRating.Location = new System.Drawing.Point(3, 175);
            this.groupBoxRating.MinimumSize = new System.Drawing.Size(238, 48);
            this.groupBoxRating.Name = "groupBoxRating";
            // 
            // groupBoxRating.Panel
            // 
            this.groupBoxRating.Panel.Controls.Add(this.radioButtonRating5);
            this.groupBoxRating.Panel.Controls.Add(this.radioButtonRating4);
            this.groupBoxRating.Panel.Controls.Add(this.radioButtonRating3);
            this.groupBoxRating.Panel.Controls.Add(this.radioButtonRating2);
            this.groupBoxRating.Panel.Controls.Add(this.radioButtonRating1);
            this.groupBoxRating.Size = new System.Drawing.Size(255, 48);
            this.groupBoxRating.TabIndex = 5;
            this.groupBoxRating.Values.Heading = "Rating";
            // 
            // radioButtonRating5
            // 
            this.radioButtonRating5.Images.CheckedDisabled = ((System.Drawing.Image)(resources.GetObject("radioButtonRating5.Images.CheckedDisabled")));
            this.radioButtonRating5.Images.CheckedNormal = ((System.Drawing.Image)(resources.GetObject("radioButtonRating5.Images.CheckedNormal")));
            this.radioButtonRating5.Images.CheckedPressed = ((System.Drawing.Image)(resources.GetObject("radioButtonRating5.Images.CheckedPressed")));
            this.radioButtonRating5.Images.CheckedTracking = ((System.Drawing.Image)(resources.GetObject("radioButtonRating5.Images.CheckedTracking")));
            this.radioButtonRating5.Images.UncheckedDisabled = ((System.Drawing.Image)(resources.GetObject("radioButtonRating5.Images.UncheckedDisabled")));
            this.radioButtonRating5.Images.UncheckedNormal = ((System.Drawing.Image)(resources.GetObject("radioButtonRating5.Images.UncheckedNormal")));
            this.radioButtonRating5.Images.UncheckedPressed = ((System.Drawing.Image)(resources.GetObject("radioButtonRating5.Images.UncheckedPressed")));
            this.radioButtonRating5.Images.UncheckedTracking = ((System.Drawing.Image)(resources.GetObject("radioButtonRating5.Images.UncheckedTracking")));
            this.radioButtonRating5.Location = new System.Drawing.Point(141, 3);
            this.radioButtonRating5.Name = "radioButtonRating5";
            this.radioButtonRating5.Size = new System.Drawing.Size(32, 18);
            this.radioButtonRating5.TabIndex = 4;
            this.radioButtonRating5.Values.Text = "5";
            this.radioButtonRating5.CheckedChanged += new System.EventHandler(this.radioButtonRating5_CheckedChanged);
            // 
            // radioButtonRating4
            // 
            this.radioButtonRating4.Images.CheckedDisabled = ((System.Drawing.Image)(resources.GetObject("radioButtonRating4.Images.CheckedDisabled")));
            this.radioButtonRating4.Images.CheckedNormal = ((System.Drawing.Image)(resources.GetObject("radioButtonRating4.Images.CheckedNormal")));
            this.radioButtonRating4.Images.CheckedPressed = ((System.Drawing.Image)(resources.GetObject("radioButtonRating4.Images.CheckedPressed")));
            this.radioButtonRating4.Images.CheckedTracking = ((System.Drawing.Image)(resources.GetObject("radioButtonRating4.Images.CheckedTracking")));
            this.radioButtonRating4.Images.UncheckedDisabled = ((System.Drawing.Image)(resources.GetObject("radioButtonRating4.Images.UncheckedDisabled")));
            this.radioButtonRating4.Images.UncheckedNormal = ((System.Drawing.Image)(resources.GetObject("radioButtonRating4.Images.UncheckedNormal")));
            this.radioButtonRating4.Images.UncheckedPressed = ((System.Drawing.Image)(resources.GetObject("radioButtonRating4.Images.UncheckedPressed")));
            this.radioButtonRating4.Images.UncheckedTracking = ((System.Drawing.Image)(resources.GetObject("radioButtonRating4.Images.UncheckedTracking")));
            this.radioButtonRating4.Location = new System.Drawing.Point(106, 3);
            this.radioButtonRating4.Name = "radioButtonRating4";
            this.radioButtonRating4.Size = new System.Drawing.Size(32, 18);
            this.radioButtonRating4.TabIndex = 3;
            this.radioButtonRating4.Values.Text = "4";
            this.radioButtonRating4.CheckedChanged += new System.EventHandler(this.radioButtonRating4_CheckedChanged);
            // 
            // radioButtonRating3
            // 
            this.radioButtonRating3.Images.CheckedDisabled = ((System.Drawing.Image)(resources.GetObject("radioButtonRating3.Images.CheckedDisabled")));
            this.radioButtonRating3.Images.CheckedNormal = ((System.Drawing.Image)(resources.GetObject("radioButtonRating3.Images.CheckedNormal")));
            this.radioButtonRating3.Images.CheckedPressed = ((System.Drawing.Image)(resources.GetObject("radioButtonRating3.Images.CheckedPressed")));
            this.radioButtonRating3.Images.CheckedTracking = ((System.Drawing.Image)(resources.GetObject("radioButtonRating3.Images.CheckedTracking")));
            this.radioButtonRating3.Images.UncheckedDisabled = ((System.Drawing.Image)(resources.GetObject("radioButtonRating3.Images.UncheckedDisabled")));
            this.radioButtonRating3.Images.UncheckedNormal = ((System.Drawing.Image)(resources.GetObject("radioButtonRating3.Images.UncheckedNormal")));
            this.radioButtonRating3.Images.UncheckedPressed = ((System.Drawing.Image)(resources.GetObject("radioButtonRating3.Images.UncheckedPressed")));
            this.radioButtonRating3.Images.UncheckedTracking = ((System.Drawing.Image)(resources.GetObject("radioButtonRating3.Images.UncheckedTracking")));
            this.radioButtonRating3.Location = new System.Drawing.Point(71, 3);
            this.radioButtonRating3.Name = "radioButtonRating3";
            this.radioButtonRating3.Size = new System.Drawing.Size(32, 18);
            this.radioButtonRating3.TabIndex = 2;
            this.radioButtonRating3.Values.Text = "3";
            this.radioButtonRating3.CheckedChanged += new System.EventHandler(this.radioButtonRating3_CheckedChanged);
            // 
            // radioButtonRating2
            // 
            this.radioButtonRating2.Images.CheckedDisabled = ((System.Drawing.Image)(resources.GetObject("radioButtonRating2.Images.CheckedDisabled")));
            this.radioButtonRating2.Images.CheckedNormal = ((System.Drawing.Image)(resources.GetObject("radioButtonRating2.Images.CheckedNormal")));
            this.radioButtonRating2.Images.CheckedPressed = ((System.Drawing.Image)(resources.GetObject("radioButtonRating2.Images.CheckedPressed")));
            this.radioButtonRating2.Images.CheckedTracking = ((System.Drawing.Image)(resources.GetObject("radioButtonRating2.Images.CheckedTracking")));
            this.radioButtonRating2.Images.UncheckedDisabled = ((System.Drawing.Image)(resources.GetObject("radioButtonRating2.Images.UncheckedDisabled")));
            this.radioButtonRating2.Images.UncheckedNormal = ((System.Drawing.Image)(resources.GetObject("radioButtonRating2.Images.UncheckedNormal")));
            this.radioButtonRating2.Images.UncheckedPressed = ((System.Drawing.Image)(resources.GetObject("radioButtonRating2.Images.UncheckedPressed")));
            this.radioButtonRating2.Images.UncheckedTracking = ((System.Drawing.Image)(resources.GetObject("radioButtonRating2.Images.UncheckedTracking")));
            this.radioButtonRating2.Location = new System.Drawing.Point(36, 3);
            this.radioButtonRating2.Name = "radioButtonRating2";
            this.radioButtonRating2.Size = new System.Drawing.Size(32, 18);
            this.radioButtonRating2.TabIndex = 1;
            this.radioButtonRating2.Values.Text = "2";
            this.radioButtonRating2.CheckedChanged += new System.EventHandler(this.radioButtonRating2_CheckedChanged);
            // 
            // radioButtonRating1
            // 
            this.radioButtonRating1.Images.CheckedDisabled = ((System.Drawing.Image)(resources.GetObject("radioButtonRating1.Images.CheckedDisabled")));
            this.radioButtonRating1.Images.CheckedNormal = ((System.Drawing.Image)(resources.GetObject("radioButtonRating1.Images.CheckedNormal")));
            this.radioButtonRating1.Images.CheckedPressed = ((System.Drawing.Image)(resources.GetObject("radioButtonRating1.Images.CheckedPressed")));
            this.radioButtonRating1.Images.CheckedTracking = ((System.Drawing.Image)(resources.GetObject("radioButtonRating1.Images.CheckedTracking")));
            this.radioButtonRating1.Images.UncheckedDisabled = ((System.Drawing.Image)(resources.GetObject("radioButtonRating1.Images.UncheckedDisabled")));
            this.radioButtonRating1.Images.UncheckedNormal = ((System.Drawing.Image)(resources.GetObject("radioButtonRating1.Images.UncheckedNormal")));
            this.radioButtonRating1.Images.UncheckedPressed = ((System.Drawing.Image)(resources.GetObject("radioButtonRating1.Images.UncheckedPressed")));
            this.radioButtonRating1.Images.UncheckedTracking = ((System.Drawing.Image)(resources.GetObject("radioButtonRating1.Images.UncheckedTracking")));
            this.radioButtonRating1.Location = new System.Drawing.Point(1, 3);
            this.radioButtonRating1.Name = "radioButtonRating1";
            this.radioButtonRating1.Size = new System.Drawing.Size(32, 18);
            this.radioButtonRating1.TabIndex = 0;
            this.radioButtonRating1.Values.Text = "1";
            this.radioButtonRating1.CheckedChanged += new System.EventHandler(this.radioButtonRating1_CheckedChanged);
            // 
            // kryptonWorkspaceCellToolboxTagsDetails
            // 
            this.kryptonWorkspaceCellToolboxTagsDetails.AllowPageDrag = true;
            this.kryptonWorkspaceCellToolboxTagsDetails.AllowTabFocus = false;
            this.kryptonWorkspaceCellToolboxTagsDetails.Button.CloseButtonAction = Krypton.Navigator.CloseButtonAction.None;
            this.kryptonWorkspaceCellToolboxTagsDetails.Button.CloseButtonDisplay = Krypton.Navigator.ButtonDisplay.Hide;
            this.kryptonWorkspaceCellToolboxTagsDetails.Button.ContextButtonAction = Krypton.Navigator.ContextButtonAction.None;
            this.kryptonWorkspaceCellToolboxTagsDetails.Button.ContextButtonDisplay = Krypton.Navigator.ButtonDisplay.Hide;
            this.kryptonWorkspaceCellToolboxTagsDetails.Name = "kryptonWorkspaceCellToolboxTagsDetails";
            this.kryptonWorkspaceCellToolboxTagsDetails.NavigatorMode = Krypton.Navigator.NavigatorMode.HeaderGroup;
            this.kryptonWorkspaceCellToolboxTagsDetails.Pages.AddRange(new Krypton.Navigator.KryptonPage[] {
            this.kryptonPageToolboxTagsDetails});
            this.kryptonWorkspaceCellToolboxTagsDetails.SelectedIndex = 0;
            this.kryptonWorkspaceCellToolboxTagsDetails.UniqueName = "f22605bf03394f62aaba2ee0fa74b712";
            // 
            // kryptonWorkspaceCellToolboxTagsKeywords
            // 
            this.kryptonWorkspaceCellToolboxTagsKeywords.AllowPageDrag = true;
            this.kryptonWorkspaceCellToolboxTagsKeywords.AllowTabFocus = false;
            this.kryptonWorkspaceCellToolboxTagsKeywords.Button.CloseButtonAction = Krypton.Navigator.CloseButtonAction.None;
            this.kryptonWorkspaceCellToolboxTagsKeywords.Button.CloseButtonDisplay = Krypton.Navigator.ButtonDisplay.Hide;
            this.kryptonWorkspaceCellToolboxTagsKeywords.Button.ContextButtonAction = Krypton.Navigator.ContextButtonAction.None;
            this.kryptonWorkspaceCellToolboxTagsKeywords.Button.ContextButtonDisplay = Krypton.Navigator.ButtonDisplay.Hide;
            this.kryptonWorkspaceCellToolboxTagsKeywords.Name = "kryptonWorkspaceCellToolboxTagsKeywords";
            this.kryptonWorkspaceCellToolboxTagsKeywords.NavigatorMode = Krypton.Navigator.NavigatorMode.HeaderGroup;
            this.kryptonWorkspaceCellToolboxTagsKeywords.Pages.AddRange(new Krypton.Navigator.KryptonPage[] {
            this.kryptonPageToolboxTagsKeywords});
            this.kryptonWorkspaceCellToolboxTagsKeywords.SelectedIndex = 0;
            this.kryptonWorkspaceCellToolboxTagsKeywords.UniqueName = "3a5efc874fa54d6e9209f112114974f8";
            // 
            // kryptonPageToolboxTagsKeywords
            // 
            this.kryptonPageToolboxTagsKeywords.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageToolboxTagsKeywords.Controls.Add(this.dataGridViewTagsAndKeywords);
            this.kryptonPageToolboxTagsKeywords.Flags = 65534;
            this.kryptonPageToolboxTagsKeywords.LastVisibleSet = true;
            this.kryptonPageToolboxTagsKeywords.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageToolboxTagsKeywords.Name = "kryptonPageToolboxTagsKeywords";
            this.kryptonPageToolboxTagsKeywords.Size = new System.Drawing.Size(398, 304);
            this.kryptonPageToolboxTagsKeywords.Text = "Keywords";
            this.kryptonPageToolboxTagsKeywords.TextDescription = "Edit media keywords";
            this.kryptonPageToolboxTagsKeywords.TextTitle = "Keywords";
            this.kryptonPageToolboxTagsKeywords.ToolTipTitle = "Edit media keywords";
            this.kryptonPageToolboxTagsKeywords.UniqueName = "3ec8345c56dd4e25aff73753d638f4a4";
            // 
            // dataGridViewTagsAndKeywords
            // 
            this.dataGridViewTagsAndKeywords.ColumnHeadersHeight = 200;
            this.dataGridViewTagsAndKeywords.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewTagsAndKeywords.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewTagsAndKeywords.Name = "dataGridViewTagsAndKeywords";
            this.dataGridViewTagsAndKeywords.RowHeadersWidth = 51;
            this.dataGridViewTagsAndKeywords.RowTemplate.Height = 24;
            this.dataGridViewTagsAndKeywords.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTagsAndKeywords.ShowCellErrors = false;
            this.dataGridViewTagsAndKeywords.ShowRowErrors = false;
            this.dataGridViewTagsAndKeywords.Size = new System.Drawing.Size(398, 304);
            this.dataGridViewTagsAndKeywords.TabIndex = 9;
            this.dataGridViewTagsAndKeywords.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewTagsAndKeywords_CellBeginEdit);
            this.dataGridViewTagsAndKeywords.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTagsAndKeywords_CellEnter);
            this.dataGridViewTagsAndKeywords.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewTagsAndKeywords_CellMouseClick);
            this.dataGridViewTagsAndKeywords.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridViewTagsAndKeywords_CellPainting);
            this.dataGridViewTagsAndKeywords.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTagsAndKeywords_CellValueChanged);
            this.dataGridViewTagsAndKeywords.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dataGridViewTagsAndKeywords_RowsAdded);
            this.dataGridViewTagsAndKeywords.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridViewTagsAndKeywords_KeyDown);
            // 
            // kryptonPageToolboxPeople
            // 
            this.kryptonPageToolboxPeople.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageToolboxPeople.Controls.Add(this.dataGridViewPeople);
            this.kryptonPageToolboxPeople.Flags = 65534;
            this.kryptonPageToolboxPeople.LastVisibleSet = true;
            this.kryptonPageToolboxPeople.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageToolboxPeople.Name = "kryptonPageToolboxPeople";
            this.kryptonPageToolboxPeople.Size = new System.Drawing.Size(400, 701);
            this.kryptonPageToolboxPeople.Tag = "People";
            this.kryptonPageToolboxPeople.Text = "People";
            this.kryptonPageToolboxPeople.TextDescription = "Edit region size and name";
            this.kryptonPageToolboxPeople.TextTitle = "People";
            this.kryptonPageToolboxPeople.ToolTipTitle = "Edit region size and name";
            this.kryptonPageToolboxPeople.UniqueName = "8eae8c29f1b74a139868e1294b11cff0";
            this.kryptonPageToolboxPeople.Enter += new System.EventHandler(this.kryptonPageToolboxPeople_Enter);
            // 
            // dataGridViewPeople
            // 
            this.dataGridViewPeople.ColumnHeadersHeight = 29;
            this.dataGridViewPeople.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewPeople.KryptonContextMenu = this.kryptonContextMenuGenericBase;
            this.dataGridViewPeople.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewPeople.Name = "dataGridViewPeople";
            this.dataGridViewPeople.RowHeadersWidth = 51;
            this.dataGridViewPeople.RowTemplate.Height = 24;
            this.dataGridViewPeople.Size = new System.Drawing.Size(400, 701);
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
            // kryptonContextMenuGenericBase
            // 
            this.kryptonContextMenuGenericBase.Items.AddRange(new Krypton.Toolkit.KryptonContextMenuItemBase[] {
            this.kryptonContextMenuItemsGenericBaseList});
            this.kryptonContextMenuGenericBase.Opening += new System.ComponentModel.CancelEventHandler(this.kryptonContextMenuGenericBase_Opening);
            // 
            // kryptonContextMenuItemsGenericBaseList
            // 
            this.kryptonContextMenuItemsGenericBaseList.Items.AddRange(new Krypton.Toolkit.KryptonContextMenuItemBase[] {
            this.kryptonContextMenuItemGenericRegionRename1,
            this.kryptonContextMenuItemGenericRegionRename2,
            this.kryptonContextMenuItemGenericRegionRename3,
            this.kryptonContextMenuItemGenericRegionRenameFromLastUsed,
            this.kryptonContextMenuItemGenericRegionRenameListAll,
            this.kryptonContextMenuSeparatorGenericEndOfRegionRename,
            this.kryptonContextMenuItemGenericCut,
            this.kryptonContextMenuItemGenericCopy,
            this.kryptonContextMenuItemGenericCopyText,
            this.kryptonContextMenuItemGenericPaste,
            this.kryptonContextMenuItemGenericDelete,
            this.kryptonContextMenuItemGenericRename,
            this.kryptonContextMenuItemGenericUndo,
            this.kryptonContextMenuItemGenericRedo,
            this.kryptonContextMenuItemGenericFind,
            this.kryptonContextMenuItemGenericReplace,
            this.kryptonContextMenuItemGenericSave,
            this.kryptonContextMenuSeparatorGenericEndOfClipboard,
            this.kryptonContextMenuItemGenericRefreshFolder,
            this.kryptonContextMenuItemGenericReadSubfolders,
            this.kryptonContextMenuItemGenericOpenFolderLocation,
            this.kryptonContextMenuItemGenericOpen,
            this.kryptonContextMenuItemGenericOpenWith,
            this.kryptonContextMenuItemOpenAndAssociateWithDialog,
            this.kryptonContextMenuItemGenericOpenVerbEdit,
            this.kryptonContextMenuItemGenericRunCommand,
            this.kryptonContextMenuSeparatorGenericEndOfFileSystem,
            this.kryptonContextMenuItemGenericAutoCorrectRun,
            this.kryptonContextMenuItemGenericAutoCorrectForm,
            this.kryptonContextMenuItemGenericMetadataRefreshLast,
            this.kryptonContextMenuItemGenericMetadataDeleteHistory,
            this.kryptonContextMenuSeparatorGenericEndOfMetadata,
            this.kryptonContextMenuItemGenericRotate270,
            this.kryptonContextMenuItemGenericRotate180,
            this.kryptonContextMenuItemGenericRotate90,
            this.kryptonContextMenuSeparatorEndOfRotate,
            this.kryptonContextMenuItemGenericFavoriteAdd,
            this.kryptonContextMenuItemGenericFavoriteDelete,
            this.kryptonContextMenuItemGenericFavoriteToggle,
            this.kryptonContextMenuSeparatorGenericEndOfFavorite,
            this.kryptonContextMenuItemGenericRowShowFavorite,
            this.kryptonContextMenuItemGenericRowHideEqual,
            this.kryptonContextMenuSeparatorGenericEndOfShowHideRows,
            this.kryptonContextMenuItemGenericTriStateOn,
            this.kryptonContextMenuItemGenericTriStateOff,
            this.kryptonContextMenuItemGenericTriStateToggle,
            this.kryptonContextMenuSeparatorGenericEndOfTriState,
            this.kryptonContextMenuItemGenericMediaViewAsPoster,
            this.kryptonContextMenuItemGenericMediaViewAsFull});
            // 
            // kryptonContextMenuItemGenericRegionRename1
            // 
            this.kryptonContextMenuItemGenericRegionRename1.Text = "Rename #1";
            // 
            // kryptonContextMenuItemGenericRegionRename2
            // 
            this.kryptonContextMenuItemGenericRegionRename2.Text = "Rename #2";
            // 
            // kryptonContextMenuItemGenericRegionRename3
            // 
            this.kryptonContextMenuItemGenericRegionRename3.Text = "Rename #3";
            // 
            // kryptonContextMenuItemGenericRegionRenameFromLastUsed
            // 
            this.kryptonContextMenuItemGenericRegionRenameFromLastUsed.Items.AddRange(new Krypton.Toolkit.KryptonContextMenuItemBase[] {
            this.kryptonContextMenuItemsGenericRegionRenameFromLastUsedList});
            this.kryptonContextMenuItemGenericRegionRenameFromLastUsed.Text = "Rename from last used";
            // 
            // kryptonContextMenuItemsGenericRegionRenameFromLastUsedList
            // 
            this.kryptonContextMenuItemsGenericRegionRenameFromLastUsedList.Items.AddRange(new Krypton.Toolkit.KryptonContextMenuItemBase[] {
            this.kryptonContextMenuItemGenericRegionRenameFormLastUsedExample});
            // 
            // kryptonContextMenuItemGenericRegionRenameFormLastUsedExample
            // 
            this.kryptonContextMenuItemGenericRegionRenameFormLastUsedExample.Text = "Last use example";
            // 
            // kryptonContextMenuItemGenericRegionRenameListAll
            // 
            this.kryptonContextMenuItemGenericRegionRenameListAll.Items.AddRange(new Krypton.Toolkit.KryptonContextMenuItemBase[] {
            this.kryptonContextMenuItemsGenericRegionRenameListAllList});
            this.kryptonContextMenuItemGenericRegionRenameListAll.Text = "Rename - List all";
            // 
            // kryptonContextMenuItemsGenericRegionRenameListAllList
            // 
            this.kryptonContextMenuItemsGenericRegionRenameListAllList.Items.AddRange(new Krypton.Toolkit.KryptonContextMenuItemBase[] {
            this.kryptonContextMenuItemGenericRegionRenameListAllExample});
            // 
            // kryptonContextMenuItemGenericRegionRenameListAllExample
            // 
            this.kryptonContextMenuItemGenericRegionRenameListAllExample.Text = "List all example";
            // 
            // kryptonContextMenuItemGenericCut
            // 
            this.kryptonContextMenuItemGenericCut.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditCut32x32;
            this.kryptonContextMenuItemGenericCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.kryptonContextMenuItemGenericCut.Text = "Cut";
            // 
            // kryptonContextMenuItemGenericCopy
            // 
            this.kryptonContextMenuItemGenericCopy.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditCopy32x32;
            this.kryptonContextMenuItemGenericCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.kryptonContextMenuItemGenericCopy.Text = "Copy";
            // 
            // kryptonContextMenuItemGenericCopyText
            // 
            this.kryptonContextMenuItemGenericCopyText.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditCopyText32x32;
            this.kryptonContextMenuItemGenericCopyText.ShortcutKeys = ((System.Windows.Forms.Keys)((((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.C)));
            this.kryptonContextMenuItemGenericCopyText.Text = "Copy text";
            // 
            // kryptonContextMenuItemGenericPaste
            // 
            this.kryptonContextMenuItemGenericPaste.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditPaste32x32;
            this.kryptonContextMenuItemGenericPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.kryptonContextMenuItemGenericPaste.Text = "Paste";
            // 
            // kryptonContextMenuItemGenericDelete
            // 
            this.kryptonContextMenuItemGenericDelete.Image = global::PhotoTagsSynchronizer.Properties.Resources.FileSystemDelete32x32;
            this.kryptonContextMenuItemGenericDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.kryptonContextMenuItemGenericDelete.Text = "Delete";
            // 
            // kryptonContextMenuItemGenericRename
            // 
            this.kryptonContextMenuItemGenericRename.Image = global::PhotoTagsSynchronizer.Properties.Resources.FileSystemRename;
            this.kryptonContextMenuItemGenericRename.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.kryptonContextMenuItemGenericRename.Text = "Rename";
            // 
            // kryptonContextMenuItemGenericUndo
            // 
            this.kryptonContextMenuItemGenericUndo.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditUndo32x32;
            this.kryptonContextMenuItemGenericUndo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.kryptonContextMenuItemGenericUndo.Text = "Undo";
            // 
            // kryptonContextMenuItemGenericRedo
            // 
            this.kryptonContextMenuItemGenericRedo.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditRedo32x32;
            this.kryptonContextMenuItemGenericRedo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.kryptonContextMenuItemGenericRedo.Text = "Redo";
            // 
            // kryptonContextMenuItemGenericFind
            // 
            this.kryptonContextMenuItemGenericFind.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditFind32x32;
            this.kryptonContextMenuItemGenericFind.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.kryptonContextMenuItemGenericFind.Text = "Find";
            // 
            // kryptonContextMenuItemGenericReplace
            // 
            this.kryptonContextMenuItemGenericReplace.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditReplace32x32;
            this.kryptonContextMenuItemGenericReplace.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.kryptonContextMenuItemGenericReplace.Text = "Replace";
            // 
            // kryptonContextMenuItemGenericSave
            // 
            this.kryptonContextMenuItemGenericSave.Image = global::PhotoTagsSynchronizer.Properties.Resources.MetadataSave32x32;
            this.kryptonContextMenuItemGenericSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.kryptonContextMenuItemGenericSave.Text = "Save";
            // 
            // kryptonContextMenuItemGenericRefreshFolder
            // 
            this.kryptonContextMenuItemGenericRefreshFolder.Image = global::PhotoTagsSynchronizer.Properties.Resources.FileSystemRefresh32x32;
            this.kryptonContextMenuItemGenericRefreshFolder.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.kryptonContextMenuItemGenericRefreshFolder.Text = "Refresh folder";
            // 
            // kryptonContextMenuItemGenericReadSubfolders
            // 
            this.kryptonContextMenuItemGenericReadSubfolders.Image = global::PhotoTagsSynchronizer.Properties.Resources.MetadataAddSubfolders32x32;
            this.kryptonContextMenuItemGenericReadSubfolders.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.kryptonContextMenuItemGenericReadSubfolders.Text = "Read subfolers";
            // 
            // kryptonContextMenuItemGenericOpenFolderLocation
            // 
            this.kryptonContextMenuItemGenericOpenFolderLocation.Image = global::PhotoTagsSynchronizer.Properties.Resources.FileSystemExplorer32x32;
            this.kryptonContextMenuItemGenericOpenFolderLocation.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.L)));
            this.kryptonContextMenuItemGenericOpenFolderLocation.Text = "Open Location in Explorer";
            // 
            // kryptonContextMenuItemGenericOpen
            // 
            this.kryptonContextMenuItemGenericOpen.Image = global::PhotoTagsSynchronizer.Properties.Resources.FileSystemOpen;
            this.kryptonContextMenuItemGenericOpen.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.kryptonContextMenuItemGenericOpen.Text = "Open";
            // 
            // kryptonContextMenuItemGenericOpenWith
            // 
            this.kryptonContextMenuItemGenericOpenWith.Image = global::PhotoTagsSynchronizer.Properties.Resources.FileSystemOpenWith;
            this.kryptonContextMenuItemGenericOpenWith.Items.AddRange(new Krypton.Toolkit.KryptonContextMenuItemBase[] {
            this.kryptonContextMenuItemsGenericOpenWithAppList});
            this.kryptonContextMenuItemGenericOpenWith.Text = "Open with...";
            // 
            // kryptonContextMenuItemsGenericOpenWithAppList
            // 
            this.kryptonContextMenuItemsGenericOpenWithAppList.Items.AddRange(new Krypton.Toolkit.KryptonContextMenuItemBase[] {
            this.kryptonContextMenuItemsGenericOpenWithAppListExample});
            // 
            // kryptonContextMenuItemsGenericOpenWithAppListExample
            // 
            this.kryptonContextMenuItemsGenericOpenWithAppListExample.Text = "Open with example";
            // 
            // kryptonContextMenuItemOpenAndAssociateWithDialog
            // 
            this.kryptonContextMenuItemOpenAndAssociateWithDialog.Image = global::PhotoTagsSynchronizer.Properties.Resources.FileSystemOpenWIthAssociationApp32x32;
            this.kryptonContextMenuItemOpenAndAssociateWithDialog.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.kryptonContextMenuItemOpenAndAssociateWithDialog.Text = "Open and associate with dialog...";
            // 
            // kryptonContextMenuItemGenericOpenVerbEdit
            // 
            this.kryptonContextMenuItemGenericOpenVerbEdit.Image = global::PhotoTagsSynchronizer.Properties.Resources.FileSystemEdit;
            this.kryptonContextMenuItemGenericOpenVerbEdit.ShortcutKeys = System.Windows.Forms.Keys.F4;
            this.kryptonContextMenuItemGenericOpenVerbEdit.Text = "Edit";
            // 
            // kryptonContextMenuItemGenericRunCommand
            // 
            this.kryptonContextMenuItemGenericRunCommand.Image = global::PhotoTagsSynchronizer.Properties.Resources.ToolsRunCommand32x32;
            this.kryptonContextMenuItemGenericRunCommand.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.R)));
            this.kryptonContextMenuItemGenericRunCommand.Text = "Run app/command batch";
            // 
            // kryptonContextMenuItemGenericAutoCorrectRun
            // 
            this.kryptonContextMenuItemGenericAutoCorrectRun.Image = global::PhotoTagsSynchronizer.Properties.Resources.MetadataAutoCorrectRun32x32;
            this.kryptonContextMenuItemGenericAutoCorrectRun.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.T)));
            this.kryptonContextMenuItemGenericAutoCorrectRun.Text = "AutoCorrect Run";
            // 
            // kryptonContextMenuItemGenericAutoCorrectForm
            // 
            this.kryptonContextMenuItemGenericAutoCorrectForm.Image = global::PhotoTagsSynchronizer.Properties.Resources.MetadataAutoCorrectForm32x32;
            this.kryptonContextMenuItemGenericAutoCorrectForm.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.T)));
            this.kryptonContextMenuItemGenericAutoCorrectForm.Text = "AutoCorrect Form";
            // 
            // kryptonContextMenuItemGenericMetadataRefreshLast
            // 
            this.kryptonContextMenuItemGenericMetadataRefreshLast.Image = global::PhotoTagsSynchronizer.Properties.Resources.MetadataReload;
            this.kryptonContextMenuItemGenericMetadataRefreshLast.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F5)));
            this.kryptonContextMenuItemGenericMetadataRefreshLast.Text = "Refresh Metadata (Reload last)";
            // 
            // kryptonContextMenuItemGenericMetadataDeleteHistory
            // 
            this.kryptonContextMenuItemGenericMetadataDeleteHistory.Image = global::PhotoTagsSynchronizer.Properties.Resources.MetadataDeleteHistory;
            this.kryptonContextMenuItemGenericMetadataDeleteHistory.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F5)));
            this.kryptonContextMenuItemGenericMetadataDeleteHistory.Text = "Reload Metadata (Forget history)";
            // 
            // kryptonContextMenuItemGenericRotate270
            // 
            this.kryptonContextMenuItemGenericRotate270.Image = global::PhotoTagsSynchronizer.Properties.Resources.MediaRotate90CCW;
            this.kryptonContextMenuItemGenericRotate270.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D9)));
            this.kryptonContextMenuItemGenericRotate270.Text = "Rotate 270° (90 CCW)";
            // 
            // kryptonContextMenuItemGenericRotate180
            // 
            this.kryptonContextMenuItemGenericRotate180.Image = global::PhotoTagsSynchronizer.Properties.Resources.MediaRotate180;
            this.kryptonContextMenuItemGenericRotate180.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D1)));
            this.kryptonContextMenuItemGenericRotate180.Text = "Rotate 180°";
            // 
            // kryptonContextMenuItemGenericRotate90
            // 
            this.kryptonContextMenuItemGenericRotate90.Image = global::PhotoTagsSynchronizer.Properties.Resources.MediaRotate90CW;
            this.kryptonContextMenuItemGenericRotate90.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D2)));
            this.kryptonContextMenuItemGenericRotate90.Text = "Rotate 90° (90 CW)";
            // 
            // kryptonContextMenuItemGenericFavoriteAdd
            // 
            this.kryptonContextMenuItemGenericFavoriteAdd.Image = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteSelect32x32;
            this.kryptonContextMenuItemGenericFavoriteAdd.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.kryptonContextMenuItemGenericFavoriteAdd.Text = "Mark as favorite";
            // 
            // kryptonContextMenuItemGenericFavoriteDelete
            // 
            this.kryptonContextMenuItemGenericFavoriteDelete.Image = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteRemove32x32;
            this.kryptonContextMenuItemGenericFavoriteDelete.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D)));
            this.kryptonContextMenuItemGenericFavoriteDelete.Text = "Remove as favorite";
            // 
            // kryptonContextMenuItemGenericFavoriteToggle
            // 
            this.kryptonContextMenuItemGenericFavoriteToggle.Image = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteToggle32x32;
            this.kryptonContextMenuItemGenericFavoriteToggle.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.D)));
            this.kryptonContextMenuItemGenericFavoriteToggle.Text = "Toggle as favorite";
            // 
            // kryptonContextMenuItemGenericRowShowFavorite
            // 
            this.kryptonContextMenuItemGenericRowShowFavorite.Image = global::PhotoTagsSynchronizer.Properties.Resources.GridFavorite32x32;
            this.kryptonContextMenuItemGenericRowShowFavorite.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.B)));
            this.kryptonContextMenuItemGenericRowShowFavorite.Text = "Show favorite rows";
            // 
            // kryptonContextMenuItemGenericRowHideEqual
            // 
            this.kryptonContextMenuItemGenericRowHideEqual.Image = global::PhotoTagsSynchronizer.Properties.Resources.GridEqual32x32;
            this.kryptonContextMenuItemGenericRowHideEqual.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.B)));
            this.kryptonContextMenuItemGenericRowHideEqual.Text = "Hide equal rows";
            // 
            // kryptonContextMenuItemGenericTriStateOn
            // 
            this.kryptonContextMenuItemGenericTriStateOn.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditTriStateSelect32x32;
            this.kryptonContextMenuItemGenericTriStateOn.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.Space)));
            this.kryptonContextMenuItemGenericTriStateOn.Text = "Select tags";
            // 
            // kryptonContextMenuItemGenericTriStateOff
            // 
            this.kryptonContextMenuItemGenericTriStateOff.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditTriStateDelete32x32;
            this.kryptonContextMenuItemGenericTriStateOff.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.Delete)));
            this.kryptonContextMenuItemGenericTriStateOff.Text = "Remove tags";
            // 
            // kryptonContextMenuItemGenericTriStateToggle
            // 
            this.kryptonContextMenuItemGenericTriStateToggle.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditTriStateToggle32x32;
            this.kryptonContextMenuItemGenericTriStateToggle.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Space)));
            this.kryptonContextMenuItemGenericTriStateToggle.Text = "Toggle tags";
            // 
            // kryptonContextMenuItemGenericMediaViewAsPoster
            // 
            this.kryptonContextMenuItemGenericMediaViewAsPoster.Image = global::PhotoTagsSynchronizer.Properties.Resources.MediaRegionSelector;
            this.kryptonContextMenuItemGenericMediaViewAsPoster.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.kryptonContextMenuItemGenericMediaViewAsPoster.Text = "View media as Poster";
            // 
            // kryptonContextMenuItemGenericMediaViewAsFull
            // 
            this.kryptonContextMenuItemGenericMediaViewAsFull.Image = global::PhotoTagsSynchronizer.Properties.Resources.MediaPreview;
            this.kryptonContextMenuItemGenericMediaViewAsFull.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.kryptonContextMenuItemGenericMediaViewAsFull.Text = "View full media";
            // 
            // kryptonPageToolboxMap
            // 
            this.kryptonPageToolboxMap.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageToolboxMap.Controls.Add(this.kryptonWorkspaceToolboxMap);
            this.kryptonPageToolboxMap.Flags = 65534;
            this.kryptonPageToolboxMap.LastVisibleSet = true;
            this.kryptonPageToolboxMap.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageToolboxMap.Name = "kryptonPageToolboxMap";
            this.kryptonPageToolboxMap.Size = new System.Drawing.Size(400, 701);
            this.kryptonPageToolboxMap.Tag = "Map";
            this.kryptonPageToolboxMap.Text = "Map";
            this.kryptonPageToolboxMap.TextDescription = "Edit location for media files";
            this.kryptonPageToolboxMap.TextTitle = "Map";
            this.kryptonPageToolboxMap.ToolTipTitle = "Edit location for media files";
            this.kryptonPageToolboxMap.UniqueName = "da0b71fc084045079eeec453bcb515fe";
            this.kryptonPageToolboxMap.Enter += new System.EventHandler(this.kryptonPageToolboxMap_Enter);
            // 
            // kryptonWorkspaceToolboxMap
            // 
            this.kryptonWorkspaceToolboxMap.ActivePage = this.kryptonPageToolboxMapProperties;
            this.kryptonWorkspaceToolboxMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonWorkspaceToolboxMap.Location = new System.Drawing.Point(0, 0);
            this.kryptonWorkspaceToolboxMap.Name = "kryptonWorkspaceToolboxMap";
            // 
            // 
            // 
            this.kryptonWorkspaceToolboxMap.Root.Children.AddRange(new System.ComponentModel.Component[] {
            this.kryptonWorkspaceCellToolboxMapProperties,
            this.kryptonWorkspaceCellToolboxMapDetails,
            this.kryptonWorkspaceCellToolboxMapBroswer,
            this.kryptonWorkspaceCellToolboxMapBroswerProperties});
            this.kryptonWorkspaceToolboxMap.Root.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.kryptonWorkspaceToolboxMap.Root.UniqueName = "c387e773b281413ab65d11529c5261b8";
            this.kryptonWorkspaceToolboxMap.Root.WorkspaceControl = this.kryptonWorkspaceToolboxMap;
            this.kryptonWorkspaceToolboxMap.SeparatorStyle = Krypton.Toolkit.SeparatorStyle.HighProfile;
            this.kryptonWorkspaceToolboxMap.Size = new System.Drawing.Size(400, 701);
            this.kryptonWorkspaceToolboxMap.TabIndex = 0;
            this.kryptonWorkspaceToolboxMap.TabStop = true;
            // 
            // kryptonPageToolboxMapProperties
            // 
            this.kryptonPageToolboxMapProperties.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageToolboxMapProperties.Controls.Add(this.comboBoxGoogleLocationInterval);
            this.kryptonPageToolboxMapProperties.Controls.Add(this.comboBoxGoogleTimeZoneShift);
            this.kryptonPageToolboxMapProperties.Flags = 65534;
            this.kryptonPageToolboxMapProperties.LastVisibleSet = true;
            this.kryptonPageToolboxMapProperties.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageToolboxMapProperties.Name = "kryptonPageToolboxMapProperties";
            this.kryptonPageToolboxMapProperties.Size = new System.Drawing.Size(398, 50);
            this.kryptonPageToolboxMapProperties.Text = "Location properties";
            this.kryptonPageToolboxMapProperties.TextDescription = "Edit Location Metadata properties";
            this.kryptonPageToolboxMapProperties.TextTitle = "Location properties";
            this.kryptonPageToolboxMapProperties.ToolTipTitle = "Edit Location Metadata properties";
            this.kryptonPageToolboxMapProperties.UniqueName = "8b820c2b24ee4ac4a3ce963c19a01838";
            // 
            // comboBoxGoogleLocationInterval
            // 
            this.comboBoxGoogleLocationInterval.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxGoogleLocationInterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxGoogleLocationInterval.DropDownWidth = 178;
            this.comboBoxGoogleLocationInterval.IntegralHeight = false;
            this.comboBoxGoogleLocationInterval.Items.AddRange(new object[] {
            "1 minute",
            "5 minutes",
            "30 minutes",
            "1 hour",
            "1 day",
            "2 days",
            "10 days"});
            this.comboBoxGoogleLocationInterval.Location = new System.Drawing.Point(130, 3);
            this.comboBoxGoogleLocationInterval.Name = "comboBoxGoogleLocationInterval";
            this.comboBoxGoogleLocationInterval.Size = new System.Drawing.Size(127, 21);
            this.comboBoxGoogleLocationInterval.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxGoogleLocationInterval.TabIndex = 14;
            this.comboBoxGoogleLocationInterval.SelectedIndexChanged += new System.EventHandler(this.comboBoxGoogleLocationInterval_SelectedIndexChanged);
            // 
            // comboBoxGoogleTimeZoneShift
            // 
            this.comboBoxGoogleTimeZoneShift.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxGoogleTimeZoneShift.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxGoogleTimeZoneShift.DropDownWidth = 139;
            this.comboBoxGoogleTimeZoneShift.IntegralHeight = false;
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
            this.comboBoxGoogleTimeZoneShift.Location = new System.Drawing.Point(2, 3);
            this.comboBoxGoogleTimeZoneShift.Name = "comboBoxGoogleTimeZoneShift";
            this.comboBoxGoogleTimeZoneShift.Size = new System.Drawing.Size(122, 21);
            this.comboBoxGoogleTimeZoneShift.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxGoogleTimeZoneShift.TabIndex = 13;
            this.comboBoxGoogleTimeZoneShift.SelectedIndexChanged += new System.EventHandler(this.comboBoxGoogleTimeZoneShift_SelectedIndexChanged);
            // 
            // kryptonWorkspaceCellToolboxMapProperties
            // 
            this.kryptonWorkspaceCellToolboxMapProperties.AllowPageDrag = true;
            this.kryptonWorkspaceCellToolboxMapProperties.AllowTabFocus = false;
            this.kryptonWorkspaceCellToolboxMapProperties.Button.CloseButtonAction = Krypton.Navigator.CloseButtonAction.None;
            this.kryptonWorkspaceCellToolboxMapProperties.Button.CloseButtonDisplay = Krypton.Navigator.ButtonDisplay.Hide;
            this.kryptonWorkspaceCellToolboxMapProperties.Button.ContextButtonAction = Krypton.Navigator.ContextButtonAction.None;
            this.kryptonWorkspaceCellToolboxMapProperties.Button.ContextButtonDisplay = Krypton.Navigator.ButtonDisplay.Hide;
            this.kryptonWorkspaceCellToolboxMapProperties.Name = "kryptonWorkspaceCellToolboxMapProperties";
            this.kryptonWorkspaceCellToolboxMapProperties.NavigatorMode = Krypton.Navigator.NavigatorMode.Group;
            this.kryptonWorkspaceCellToolboxMapProperties.Pages.AddRange(new Krypton.Navigator.KryptonPage[] {
            this.kryptonPageToolboxMapProperties});
            this.kryptonWorkspaceCellToolboxMapProperties.SelectedIndex = 0;
            this.kryptonWorkspaceCellToolboxMapProperties.StarSize = "50*,29";
            this.kryptonWorkspaceCellToolboxMapProperties.UniqueName = "edd2a942ef254327a222d3cc58771fe1";
            // 
            // kryptonWorkspaceCellToolboxMapDetails
            // 
            this.kryptonWorkspaceCellToolboxMapDetails.AllowPageDrag = true;
            this.kryptonWorkspaceCellToolboxMapDetails.AllowTabFocus = false;
            this.kryptonWorkspaceCellToolboxMapDetails.Button.CloseButtonAction = Krypton.Navigator.CloseButtonAction.None;
            this.kryptonWorkspaceCellToolboxMapDetails.Button.CloseButtonDisplay = Krypton.Navigator.ButtonDisplay.Hide;
            this.kryptonWorkspaceCellToolboxMapDetails.Button.ContextButtonAction = Krypton.Navigator.ContextButtonAction.None;
            this.kryptonWorkspaceCellToolboxMapDetails.Button.ContextButtonDisplay = Krypton.Navigator.ButtonDisplay.Hide;
            this.kryptonWorkspaceCellToolboxMapDetails.Name = "kryptonWorkspaceCellToolboxMapDetails";
            this.kryptonWorkspaceCellToolboxMapDetails.NavigatorMode = Krypton.Navigator.NavigatorMode.Group;
            this.kryptonWorkspaceCellToolboxMapDetails.Pages.AddRange(new Krypton.Navigator.KryptonPage[] {
            this.kryptonPageToolboxMapDetails});
            this.kryptonWorkspaceCellToolboxMapDetails.SelectedIndex = 0;
            this.kryptonWorkspaceCellToolboxMapDetails.UniqueName = "326edd0d792e47c0a2695882b824e3c8";
            // 
            // kryptonPageToolboxMapDetails
            // 
            this.kryptonPageToolboxMapDetails.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageToolboxMapDetails.Controls.Add(this.dataGridViewMap);
            this.kryptonPageToolboxMapDetails.Flags = 65534;
            this.kryptonPageToolboxMapDetails.LastVisibleSet = true;
            this.kryptonPageToolboxMapDetails.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageToolboxMapDetails.Name = "kryptonPageToolboxMapDetails";
            this.kryptonPageToolboxMapDetails.Size = new System.Drawing.Size(398, 309);
            this.kryptonPageToolboxMapDetails.Text = "Location metadata";
            this.kryptonPageToolboxMapDetails.TextDescription = "Edit Location Metadata";
            this.kryptonPageToolboxMapDetails.TextTitle = "Location metadata";
            this.kryptonPageToolboxMapDetails.ToolTipTitle = "Edit Location Metadata";
            this.kryptonPageToolboxMapDetails.UniqueName = "d4bbcd822dfb4db481dbb09da5fcf3cc";
            // 
            // dataGridViewMap
            // 
            this.dataGridViewMap.AllowUserToAddRows = false;
            this.dataGridViewMap.AllowUserToDeleteRows = false;
            this.dataGridViewMap.AllowUserToResizeRows = false;
            this.dataGridViewMap.ColumnHeadersHeight = 29;
            this.dataGridViewMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewMap.KryptonContextMenu = this.kryptonContextMenuGenericBase;
            this.dataGridViewMap.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewMap.Name = "dataGridViewMap";
            this.dataGridViewMap.RowHeadersWidth = 51;
            this.dataGridViewMap.RowTemplate.Height = 24;
            this.dataGridViewMap.ShowCellErrors = false;
            this.dataGridViewMap.ShowCellToolTips = false;
            this.dataGridViewMap.ShowEditingIcon = false;
            this.dataGridViewMap.ShowRowErrors = false;
            this.dataGridViewMap.Size = new System.Drawing.Size(398, 309);
            this.dataGridViewMap.TabIndex = 10;
            this.dataGridViewMap.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewMap_CellBeginEdit);
            this.dataGridViewMap.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewMap_CellEnter);
            this.dataGridViewMap.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewMap_CellMouseClick);
            this.dataGridViewMap.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewMap_CellMouseDoubleClick);
            this.dataGridViewMap.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridViewMap_CellPainting);
            this.dataGridViewMap.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewMap_CellValueChanged);
            // 
            // kryptonWorkspaceCellToolboxMapBroswer
            // 
            this.kryptonWorkspaceCellToolboxMapBroswer.AllowPageDrag = true;
            this.kryptonWorkspaceCellToolboxMapBroswer.AllowTabFocus = false;
            this.kryptonWorkspaceCellToolboxMapBroswer.Button.CloseButtonAction = Krypton.Navigator.CloseButtonAction.None;
            this.kryptonWorkspaceCellToolboxMapBroswer.Button.CloseButtonDisplay = Krypton.Navigator.ButtonDisplay.Hide;
            this.kryptonWorkspaceCellToolboxMapBroswer.Button.ContextButtonAction = Krypton.Navigator.ContextButtonAction.None;
            this.kryptonWorkspaceCellToolboxMapBroswer.Button.ContextButtonDisplay = Krypton.Navigator.ButtonDisplay.Hide;
            this.kryptonWorkspaceCellToolboxMapBroswer.Name = "kryptonWorkspaceCellToolboxMapBroswer";
            this.kryptonWorkspaceCellToolboxMapBroswer.NavigatorMode = Krypton.Navigator.NavigatorMode.Group;
            this.kryptonWorkspaceCellToolboxMapBroswer.Pages.AddRange(new Krypton.Navigator.KryptonPage[] {
            this.kryptonPageToolboxMapBroswer});
            this.kryptonWorkspaceCellToolboxMapBroswer.SelectedIndex = 0;
            this.kryptonWorkspaceCellToolboxMapBroswer.UniqueName = "dd30213ad6e64deb908656ba538ff942";
            // 
            // kryptonPageToolboxMapBroswer
            // 
            this.kryptonPageToolboxMapBroswer.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageToolboxMapBroswer.Controls.Add(this.panelBrowser);
            this.kryptonPageToolboxMapBroswer.Flags = 65534;
            this.kryptonPageToolboxMapBroswer.LastVisibleSet = true;
            this.kryptonPageToolboxMapBroswer.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageToolboxMapBroswer.Name = "kryptonPageToolboxMapBroswer";
            this.kryptonPageToolboxMapBroswer.Size = new System.Drawing.Size(398, 309);
            this.kryptonPageToolboxMapBroswer.Text = "Map browser";
            this.kryptonPageToolboxMapBroswer.TextDescription = "See locations for selected coordinates";
            this.kryptonPageToolboxMapBroswer.TextTitle = "Map browser";
            this.kryptonPageToolboxMapBroswer.ToolTipTitle = "See locations for selected coordinates";
            this.kryptonPageToolboxMapBroswer.UniqueName = "c414786b6c454b48aea5ba8700e98799";
            // 
            // panelBrowser
            // 
            this.panelBrowser.BackColor = System.Drawing.SystemColors.Control;
            this.panelBrowser.Cursor = System.Windows.Forms.Cursors.Default;
            this.panelBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBrowser.Location = new System.Drawing.Point(0, 0);
            this.panelBrowser.Margin = new System.Windows.Forms.Padding(0);
            this.panelBrowser.Name = "panelBrowser";
            this.panelBrowser.Size = new System.Drawing.Size(398, 309);
            this.panelBrowser.TabIndex = 1;
            // 
            // kryptonWorkspaceCellToolboxMapBroswerProperties
            // 
            this.kryptonWorkspaceCellToolboxMapBroswerProperties.AllowPageDrag = true;
            this.kryptonWorkspaceCellToolboxMapBroswerProperties.AllowTabFocus = false;
            this.kryptonWorkspaceCellToolboxMapBroswerProperties.Button.CloseButtonAction = Krypton.Navigator.CloseButtonAction.None;
            this.kryptonWorkspaceCellToolboxMapBroswerProperties.Button.CloseButtonDisplay = Krypton.Navigator.ButtonDisplay.Hide;
            this.kryptonWorkspaceCellToolboxMapBroswerProperties.Button.ContextButtonAction = Krypton.Navigator.ContextButtonAction.None;
            this.kryptonWorkspaceCellToolboxMapBroswerProperties.Button.ContextButtonDisplay = Krypton.Navigator.ButtonDisplay.Hide;
            this.kryptonWorkspaceCellToolboxMapBroswerProperties.Name = "kryptonWorkspaceCellToolboxMapBroswerProperties";
            this.kryptonWorkspaceCellToolboxMapBroswerProperties.NavigatorMode = Krypton.Navigator.NavigatorMode.Group;
            this.kryptonWorkspaceCellToolboxMapBroswerProperties.Pages.AddRange(new Krypton.Navigator.KryptonPage[] {
            this.kryptonPageToolboxMapBroswerProperties});
            this.kryptonWorkspaceCellToolboxMapBroswerProperties.SelectedIndex = 0;
            this.kryptonWorkspaceCellToolboxMapBroswerProperties.StarSize = "50*,35";
            this.kryptonWorkspaceCellToolboxMapBroswerProperties.UniqueName = "9f148b26dc6b46478d368ab2b9de069d";
            // 
            // kryptonPageToolboxMapBroswerProperties
            // 
            this.kryptonPageToolboxMapBroswerProperties.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageToolboxMapBroswerProperties.Controls.Add(this.textBoxBrowserURL);
            this.kryptonPageToolboxMapBroswerProperties.Controls.Add(this.pictureBox1);
            this.kryptonPageToolboxMapBroswerProperties.Controls.Add(this.comboBoxMapZoomLevel);
            this.kryptonPageToolboxMapBroswerProperties.Flags = 65534;
            this.kryptonPageToolboxMapBroswerProperties.LastVisibleSet = true;
            this.kryptonPageToolboxMapBroswerProperties.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageToolboxMapBroswerProperties.Name = "kryptonPageToolboxMapBroswerProperties";
            this.kryptonPageToolboxMapBroswerProperties.Size = new System.Drawing.Size(398, 50);
            this.kryptonPageToolboxMapBroswerProperties.Text = "Map browser properties";
            this.kryptonPageToolboxMapBroswerProperties.TextDescription = "Edit Browser Properties";
            this.kryptonPageToolboxMapBroswerProperties.TextTitle = "Map browser properties";
            this.kryptonPageToolboxMapBroswerProperties.ToolTipTitle = "Edit Browser Properties";
            this.kryptonPageToolboxMapBroswerProperties.UniqueName = "0ad94e47b0de40a1bec5b796350ad60e";
            // 
            // textBoxBrowserURL
            // 
            this.textBoxBrowserURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBrowserURL.Location = new System.Drawing.Point(132, 6);
            this.textBoxBrowserURL.Name = "textBoxBrowserURL";
            this.textBoxBrowserURL.Size = new System.Drawing.Size(252, 21);
            this.textBoxBrowserURL.TabIndex = 9;
            this.textBoxBrowserURL.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxBrowserURL_KeyPress);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(5, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(30, 24);
            this.pictureBox1.TabIndex = 16;
            this.pictureBox1.TabStop = false;
            // 
            // comboBoxMapZoomLevel
            // 
            this.comboBoxMapZoomLevel.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxMapZoomLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMapZoomLevel.DropDownWidth = 97;
            this.comboBoxMapZoomLevel.IntegralHeight = false;
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
            this.comboBoxMapZoomLevel.Location = new System.Drawing.Point(41, 6);
            this.comboBoxMapZoomLevel.Name = "comboBoxMapZoomLevel";
            this.comboBoxMapZoomLevel.Size = new System.Drawing.Size(85, 21);
            this.comboBoxMapZoomLevel.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxMapZoomLevel.TabIndex = 15;
            this.comboBoxMapZoomLevel.SelectedIndexChanged += new System.EventHandler(this.comboBoxMapZoomLevel_SelectedIndexChanged);
            // 
            // kryptonPageToolboxDates
            // 
            this.kryptonPageToolboxDates.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageToolboxDates.Controls.Add(this.dataGridViewDate);
            this.kryptonPageToolboxDates.Flags = 65534;
            this.kryptonPageToolboxDates.LastVisibleSet = true;
            this.kryptonPageToolboxDates.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageToolboxDates.Name = "kryptonPageToolboxDates";
            this.kryptonPageToolboxDates.Size = new System.Drawing.Size(400, 701);
            this.kryptonPageToolboxDates.Tag = "Dates";
            this.kryptonPageToolboxDates.Text = "Dates";
            this.kryptonPageToolboxDates.TextDescription = "Edit dates for media files";
            this.kryptonPageToolboxDates.TextTitle = "Dates";
            this.kryptonPageToolboxDates.ToolTipTitle = resources.GetString("kryptonPageToolboxDates.ToolTipTitle");
            this.kryptonPageToolboxDates.UniqueName = "2eb78cffa8954462951c575893a4244f";
            this.kryptonPageToolboxDates.Enter += new System.EventHandler(this.kryptonPageToolboxDates_Enter);
            // 
            // dataGridViewDate
            // 
            this.dataGridViewDate.ColumnHeadersHeight = 29;
            this.dataGridViewDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewDate.KryptonContextMenu = this.kryptonContextMenuGenericBase;
            this.dataGridViewDate.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewDate.Name = "dataGridViewDate";
            this.dataGridViewDate.RowHeadersWidth = 51;
            this.dataGridViewDate.RowTemplate.Height = 24;
            this.dataGridViewDate.Size = new System.Drawing.Size(400, 701);
            this.dataGridViewDate.TabIndex = 0;
            this.dataGridViewDate.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewDate_CellBeginEdit);
            this.dataGridViewDate.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewDate_CellEndEdit);
            this.dataGridViewDate.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewDate_CellEnter);
            this.dataGridViewDate.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewDate_CellMouseClick);
            this.dataGridViewDate.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridViewDate_CellPainting);
            this.dataGridViewDate.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewDate_CellValueChanged);
            // 
            // kryptonPageToolboxExiftool
            // 
            this.kryptonPageToolboxExiftool.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageToolboxExiftool.Controls.Add(this.dataGridViewExiftool);
            this.kryptonPageToolboxExiftool.Flags = 65534;
            this.kryptonPageToolboxExiftool.LastVisibleSet = true;
            this.kryptonPageToolboxExiftool.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageToolboxExiftool.Name = "kryptonPageToolboxExiftool";
            this.kryptonPageToolboxExiftool.Size = new System.Drawing.Size(400, 701);
            this.kryptonPageToolboxExiftool.Tag = "Exiftool";
            this.kryptonPageToolboxExiftool.Text = "Exiftool";
            this.kryptonPageToolboxExiftool.TextDescription = "See all metadata grabbed by Exiftool";
            this.kryptonPageToolboxExiftool.TextTitle = "Exiftool";
            this.kryptonPageToolboxExiftool.ToolTipTitle = "See all metadata grabbed by Exiftool";
            this.kryptonPageToolboxExiftool.UniqueName = "25a8e9e2bffd4f2cb111d55fb598a6a4";
            this.kryptonPageToolboxExiftool.Enter += new System.EventHandler(this.kryptonPageToolboxExiftool_Enter);
            // 
            // dataGridViewExiftool
            // 
            this.dataGridViewExiftool.AllowUserToAddRows = false;
            this.dataGridViewExiftool.ColumnHeadersHeight = 29;
            this.dataGridViewExiftool.ContextMenuStrip = this.contextMenuStripExifTool;
            this.dataGridViewExiftool.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewExiftool.KryptonContextMenu = this.kryptonContextMenuGenericBase;
            this.dataGridViewExiftool.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewExiftool.Name = "dataGridViewExiftool";
            this.dataGridViewExiftool.RowHeadersWidth = 51;
            this.dataGridViewExiftool.RowTemplate.Height = 24;
            this.dataGridViewExiftool.Size = new System.Drawing.Size(400, 701);
            this.dataGridViewExiftool.TabIndex = 0;
            this.dataGridViewExiftool.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewExifTool_CellBeginEdit);
            this.dataGridViewExiftool.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewExifTool_CellEnter);
            this.dataGridViewExiftool.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewExifTool_CellMouseClick);
            this.dataGridViewExiftool.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridViewExifTool_CellPainting);
            // 
            // contextMenuStripExifTool
            // 
            this.contextMenuStripExifTool.Font = new System.Drawing.Font("Segoe UI", 9F);
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
            this.toolStripMenuItemShowPosterWindowExiftool,
            this.toolStripMenuItemMediaPreview});
            this.contextMenuStripExifTool.Name = "contextMenuStripMap";
            this.contextMenuStripExifTool.Size = new System.Drawing.Size(268, 264);
            // 
            // toolStripMenuItemExiftoolAssignCompositeTag
            // 
            this.toolStripMenuItemExiftoolAssignCompositeTag.Name = "toolStripMenuItemExiftoolAssignCompositeTag";
            this.toolStripMenuItemExiftoolAssignCompositeTag.Size = new System.Drawing.Size(267, 26);
            this.toolStripMenuItemExiftoolAssignCompositeTag.Text = "Assign Composite Tag";
            // 
            // toolStripMenuItemExiftoolCopy
            // 
            this.toolStripMenuItemExiftoolCopy.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditCopy32x32;
            this.toolStripMenuItemExiftoolCopy.Name = "toolStripMenuItemExiftoolCopy";
            this.toolStripMenuItemExiftoolCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.toolStripMenuItemExiftoolCopy.Size = new System.Drawing.Size(267, 26);
            this.toolStripMenuItemExiftoolCopy.Text = "Copy";
            // 
            // toolStripMenuItemExiftoolFind
            // 
            this.toolStripMenuItemExiftoolFind.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditFind32x32;
            this.toolStripMenuItemExiftoolFind.Name = "toolStripMenuItemExiftoolFind";
            this.toolStripMenuItemExiftoolFind.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.toolStripMenuItemExiftoolFind.Size = new System.Drawing.Size(267, 26);
            this.toolStripMenuItemExiftoolFind.Text = "Find";
            // 
            // toolStripMenuItemExiftoolMarkFavorite
            // 
            this.toolStripMenuItemExiftoolMarkFavorite.Image = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteSelect32x32;
            this.toolStripMenuItemExiftoolMarkFavorite.Name = "toolStripMenuItemExiftoolMarkFavorite";
            this.toolStripMenuItemExiftoolMarkFavorite.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.toolStripMenuItemExiftoolMarkFavorite.Size = new System.Drawing.Size(267, 26);
            this.toolStripMenuItemExiftoolMarkFavorite.Text = "Mark as favorite";
            // 
            // toolStripMenuItemExiftoolRemoveFavorite
            // 
            this.toolStripMenuItemExiftoolRemoveFavorite.Image = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteRemove32x32;
            this.toolStripMenuItemExiftoolRemoveFavorite.Name = "toolStripMenuItemExiftoolRemoveFavorite";
            this.toolStripMenuItemExiftoolRemoveFavorite.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D)));
            this.toolStripMenuItemExiftoolRemoveFavorite.Size = new System.Drawing.Size(267, 26);
            this.toolStripMenuItemExiftoolRemoveFavorite.Text = "Remove as favorite";
            // 
            // toolStripMenuItemExiftoolToggleFavorite
            // 
            this.toolStripMenuItemExiftoolToggleFavorite.Image = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteToggle32x32;
            this.toolStripMenuItemExiftoolToggleFavorite.Name = "toolStripMenuItemExiftoolToggleFavorite";
            this.toolStripMenuItemExiftoolToggleFavorite.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.D)));
            this.toolStripMenuItemExiftoolToggleFavorite.Size = new System.Drawing.Size(267, 26);
            this.toolStripMenuItemExiftoolToggleFavorite.Text = "Toggle favorite";
            // 
            // toolStripMenuItemExiftoolSHowFavorite
            // 
            this.toolStripMenuItemExiftoolSHowFavorite.Name = "toolStripMenuItemExiftoolSHowFavorite";
            this.toolStripMenuItemExiftoolSHowFavorite.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.B)));
            this.toolStripMenuItemExiftoolSHowFavorite.Size = new System.Drawing.Size(267, 26);
            this.toolStripMenuItemExiftoolSHowFavorite.Text = "Show favorite rows";
            // 
            // toolStripMenuItemExiftoolHideEqual
            // 
            this.toolStripMenuItemExiftoolHideEqual.Name = "toolStripMenuItemExiftoolHideEqual";
            this.toolStripMenuItemExiftoolHideEqual.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.B)));
            this.toolStripMenuItemExiftoolHideEqual.Size = new System.Drawing.Size(267, 26);
            this.toolStripMenuItemExiftoolHideEqual.Text = "Hide equal rows";
            // 
            // toolStripMenuItemShowPosterWindowExiftool
            // 
            this.toolStripMenuItemShowPosterWindowExiftool.Image = global::PhotoTagsSynchronizer.Properties.Resources.MediaRegionSelector;
            this.toolStripMenuItemShowPosterWindowExiftool.Name = "toolStripMenuItemShowPosterWindowExiftool";
            this.toolStripMenuItemShowPosterWindowExiftool.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.toolStripMenuItemShowPosterWindowExiftool.Size = new System.Drawing.Size(267, 26);
            this.toolStripMenuItemShowPosterWindowExiftool.Text = "Show Media Poster Window";
            // 
            // toolStripMenuItemMediaPreview
            // 
            this.toolStripMenuItemMediaPreview.Image = global::PhotoTagsSynchronizer.Properties.Resources.MediaPreview;
            this.toolStripMenuItemMediaPreview.Name = "toolStripMenuItemMediaPreview";
            this.toolStripMenuItemMediaPreview.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.toolStripMenuItemMediaPreview.Size = new System.Drawing.Size(267, 26);
            this.toolStripMenuItemMediaPreview.Text = "Media Preview";
            // 
            // kryptonPageToolboxWarnings
            // 
            this.kryptonPageToolboxWarnings.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageToolboxWarnings.Controls.Add(this.dataGridViewExiftoolWarning);
            this.kryptonPageToolboxWarnings.Flags = 65534;
            this.kryptonPageToolboxWarnings.LastVisibleSet = true;
            this.kryptonPageToolboxWarnings.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageToolboxWarnings.Name = "kryptonPageToolboxWarnings";
            this.kryptonPageToolboxWarnings.Size = new System.Drawing.Size(400, 701);
            this.kryptonPageToolboxWarnings.Tag = "MetadataWarning";
            this.kryptonPageToolboxWarnings.Text = "Warnings";
            this.kryptonPageToolboxWarnings.TextDescription = "See metadata warnings. Example mismatch between fields.";
            this.kryptonPageToolboxWarnings.TextTitle = "Warnings";
            this.kryptonPageToolboxWarnings.ToolTipTitle = "See metadata warnings. Example mismatch between fields.";
            this.kryptonPageToolboxWarnings.UniqueName = "620b41eecac6478baf7ba08ef2fdd905";
            this.kryptonPageToolboxWarnings.Enter += new System.EventHandler(this.kryptonPageToolboxWarnings_Enter);
            // 
            // dataGridViewExiftoolWarning
            // 
            this.dataGridViewExiftoolWarning.AllowUserToAddRows = false;
            this.dataGridViewExiftoolWarning.ColumnHeadersHeight = 29;
            this.dataGridViewExiftoolWarning.ContextMenuStrip = this.contextMenuStripExiftoolWarning;
            this.dataGridViewExiftoolWarning.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewExiftoolWarning.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewExiftoolWarning.Name = "dataGridViewExiftoolWarning";
            this.dataGridViewExiftoolWarning.ReadOnly = true;
            this.dataGridViewExiftoolWarning.RowHeadersWidth = 51;
            this.dataGridViewExiftoolWarning.RowTemplate.Height = 24;
            this.dataGridViewExiftoolWarning.Size = new System.Drawing.Size(400, 701);
            this.dataGridViewExiftoolWarning.TabIndex = 0;
            this.dataGridViewExiftoolWarning.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewExifToolWarning_CellBeginEdit);
            this.dataGridViewExiftoolWarning.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewExifToolWarning_CellEnter);
            this.dataGridViewExiftoolWarning.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewExifToolWarning_CellMouseClick);
            this.dataGridViewExiftoolWarning.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridViewExifToolWarning_CellPainting);
            // 
            // contextMenuStripExiftoolWarning
            // 
            this.contextMenuStripExiftoolWarning.Font = new System.Drawing.Font("Segoe UI", 9F);
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
            this.toolStripMenuItemShowPosterWindowWarnings,
            this.toolStripMenuItemExiftoolWarningMediaPreview});
            this.contextMenuStripExiftoolWarning.Name = "contextMenuStripMap";
            this.contextMenuStripExiftoolWarning.Size = new System.Drawing.Size(268, 264);
            // 
            // toolStripMenuItemtoolExiftoolWarningAssignCompositeTag
            // 
            this.toolStripMenuItemtoolExiftoolWarningAssignCompositeTag.Name = "toolStripMenuItemtoolExiftoolWarningAssignCompositeTag";
            this.toolStripMenuItemtoolExiftoolWarningAssignCompositeTag.Size = new System.Drawing.Size(267, 26);
            this.toolStripMenuItemtoolExiftoolWarningAssignCompositeTag.Text = "Assign Composite Tag";
            // 
            // toolStripMenuItemExiftoolWarningCopy
            // 
            this.toolStripMenuItemExiftoolWarningCopy.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditCopy32x32;
            this.toolStripMenuItemExiftoolWarningCopy.Name = "toolStripMenuItemExiftoolWarningCopy";
            this.toolStripMenuItemExiftoolWarningCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.toolStripMenuItemExiftoolWarningCopy.Size = new System.Drawing.Size(267, 26);
            this.toolStripMenuItemExiftoolWarningCopy.Text = "Copy";
            // 
            // toolStripMenuItemExiftoolWarningFind
            // 
            this.toolStripMenuItemExiftoolWarningFind.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditFind32x32;
            this.toolStripMenuItemExiftoolWarningFind.Name = "toolStripMenuItemExiftoolWarningFind";
            this.toolStripMenuItemExiftoolWarningFind.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.toolStripMenuItemExiftoolWarningFind.Size = new System.Drawing.Size(267, 26);
            this.toolStripMenuItemExiftoolWarningFind.Text = "Find";
            // 
            // toolStripMenuItemExiftoolWarningMarkFavorite
            // 
            this.toolStripMenuItemExiftoolWarningMarkFavorite.Image = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteSelect32x32;
            this.toolStripMenuItemExiftoolWarningMarkFavorite.Name = "toolStripMenuItemExiftoolWarningMarkFavorite";
            this.toolStripMenuItemExiftoolWarningMarkFavorite.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.toolStripMenuItemExiftoolWarningMarkFavorite.Size = new System.Drawing.Size(267, 26);
            this.toolStripMenuItemExiftoolWarningMarkFavorite.Text = "Mark as favorite";
            // 
            // toolStripMenuItemExiftoolWarningRemoveFavorite
            // 
            this.toolStripMenuItemExiftoolWarningRemoveFavorite.Image = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteRemove32x32;
            this.toolStripMenuItemExiftoolWarningRemoveFavorite.Name = "toolStripMenuItemExiftoolWarningRemoveFavorite";
            this.toolStripMenuItemExiftoolWarningRemoveFavorite.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D)));
            this.toolStripMenuItemExiftoolWarningRemoveFavorite.Size = new System.Drawing.Size(267, 26);
            this.toolStripMenuItemExiftoolWarningRemoveFavorite.Text = "Remove as favorite";
            // 
            // toolStripMenuItemExiftoolWarningToggleFavorite
            // 
            this.toolStripMenuItemExiftoolWarningToggleFavorite.Image = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteToggle32x32;
            this.toolStripMenuItemExiftoolWarningToggleFavorite.Name = "toolStripMenuItemExiftoolWarningToggleFavorite";
            this.toolStripMenuItemExiftoolWarningToggleFavorite.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.D)));
            this.toolStripMenuItemExiftoolWarningToggleFavorite.Size = new System.Drawing.Size(267, 26);
            this.toolStripMenuItemExiftoolWarningToggleFavorite.Text = "Toggle favorite";
            // 
            // toolStripMenuItemExiftoolWarningShowFavorite
            // 
            this.toolStripMenuItemExiftoolWarningShowFavorite.Name = "toolStripMenuItemExiftoolWarningShowFavorite";
            this.toolStripMenuItemExiftoolWarningShowFavorite.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.B)));
            this.toolStripMenuItemExiftoolWarningShowFavorite.Size = new System.Drawing.Size(267, 26);
            this.toolStripMenuItemExiftoolWarningShowFavorite.Text = "Show favorite rows";
            // 
            // toolStripMenuItemExiftoolWarningHideEqual
            // 
            this.toolStripMenuItemExiftoolWarningHideEqual.Name = "toolStripMenuItemExiftoolWarningHideEqual";
            this.toolStripMenuItemExiftoolWarningHideEqual.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.B)));
            this.toolStripMenuItemExiftoolWarningHideEqual.Size = new System.Drawing.Size(267, 26);
            this.toolStripMenuItemExiftoolWarningHideEqual.Text = "Hide equal rows";
            // 
            // toolStripMenuItemShowPosterWindowWarnings
            // 
            this.toolStripMenuItemShowPosterWindowWarnings.Image = global::PhotoTagsSynchronizer.Properties.Resources.MediaRegionSelector;
            this.toolStripMenuItemShowPosterWindowWarnings.Name = "toolStripMenuItemShowPosterWindowWarnings";
            this.toolStripMenuItemShowPosterWindowWarnings.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.toolStripMenuItemShowPosterWindowWarnings.Size = new System.Drawing.Size(267, 26);
            this.toolStripMenuItemShowPosterWindowWarnings.Text = "Show Media Poster Window";
            // 
            // toolStripMenuItemExiftoolWarningMediaPreview
            // 
            this.toolStripMenuItemExiftoolWarningMediaPreview.Image = global::PhotoTagsSynchronizer.Properties.Resources.MediaPreview;
            this.toolStripMenuItemExiftoolWarningMediaPreview.Name = "toolStripMenuItemExiftoolWarningMediaPreview";
            this.toolStripMenuItemExiftoolWarningMediaPreview.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.toolStripMenuItemExiftoolWarningMediaPreview.Size = new System.Drawing.Size(267, 26);
            this.toolStripMenuItemExiftoolWarningMediaPreview.Text = "Media Preview";
            // 
            // kryptonPageToolboxProperties
            // 
            this.kryptonPageToolboxProperties.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageToolboxProperties.Controls.Add(this.dataGridViewProperties);
            this.kryptonPageToolboxProperties.Flags = 65534;
            this.kryptonPageToolboxProperties.LastVisibleSet = true;
            this.kryptonPageToolboxProperties.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageToolboxProperties.Name = "kryptonPageToolboxProperties";
            this.kryptonPageToolboxProperties.Size = new System.Drawing.Size(400, 701);
            this.kryptonPageToolboxProperties.Tag = "Properties";
            this.kryptonPageToolboxProperties.Text = "Properties";
            this.kryptonPageToolboxProperties.TextDescription = "See and edit Windows File Properties";
            this.kryptonPageToolboxProperties.TextTitle = "Properties";
            this.kryptonPageToolboxProperties.ToolTipTitle = "See and edit Windows File Properties";
            this.kryptonPageToolboxProperties.UniqueName = "5eb7f51ea28b40e79e3a029142f9d29c";
            this.kryptonPageToolboxProperties.Enter += new System.EventHandler(this.kryptonPageToolboxProperties_Enter);
            // 
            // dataGridViewProperties
            // 
            this.dataGridViewProperties.AllowUserToAddRows = false;
            this.dataGridViewProperties.AllowUserToDeleteRows = false;
            this.dataGridViewProperties.AllowUserToOrderColumns = true;
            this.dataGridViewProperties.ColumnHeadersHeight = 29;
            this.dataGridViewProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewProperties.KryptonContextMenu = this.kryptonContextMenuGenericBase;
            this.dataGridViewProperties.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewProperties.Name = "dataGridViewProperties";
            this.dataGridViewProperties.RowHeadersWidth = 51;
            this.dataGridViewProperties.RowTemplate.Height = 24;
            this.dataGridViewProperties.Size = new System.Drawing.Size(400, 701);
            this.dataGridViewProperties.TabIndex = 0;
            this.dataGridViewProperties.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewProperties_CellBeginEdit);
            this.dataGridViewProperties.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewProperties_CellEnter);
            this.dataGridViewProperties.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewProperties_CellMouseClick);
            this.dataGridViewProperties.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridViewProperties_CellPainting);
            // 
            // kryptonPageToolboxRename
            // 
            this.kryptonPageToolboxRename.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageToolboxRename.Controls.Add(this.kryptonWorkspaceToolboxRename);
            this.kryptonPageToolboxRename.Flags = 65534;
            this.kryptonPageToolboxRename.LastVisibleSet = true;
            this.kryptonPageToolboxRename.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageToolboxRename.Name = "kryptonPageToolboxRename";
            this.kryptonPageToolboxRename.Size = new System.Drawing.Size(400, 701);
            this.kryptonPageToolboxRename.Tag = "Rename";
            this.kryptonPageToolboxRename.Text = "Rename";
            this.kryptonPageToolboxRename.TextDescription = "Batch rename media files using variables";
            this.kryptonPageToolboxRename.TextTitle = "Rename";
            this.kryptonPageToolboxRename.ToolTipTitle = "Batch rename media files using variables";
            this.kryptonPageToolboxRename.UniqueName = "5b97b62a7db147f48fdb03761fa8ad89";
            this.kryptonPageToolboxRename.Enter += new System.EventHandler(this.kryptonPageToolboxRename_Enter);
            // 
            // kryptonWorkspaceToolboxRename
            // 
            this.kryptonWorkspaceToolboxRename.ActivePage = this.kryptonPageToolboxRenameVariables;
            this.kryptonWorkspaceToolboxRename.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonWorkspaceToolboxRename.Location = new System.Drawing.Point(0, 0);
            this.kryptonWorkspaceToolboxRename.Name = "kryptonWorkspaceToolboxRename";
            // 
            // 
            // 
            this.kryptonWorkspaceToolboxRename.Root.Children.AddRange(new System.ComponentModel.Component[] {
            this.kryptonWorkspaceCellToolboxRenameVariables,
            this.kryptonWorkspaceCellToolboxRenameResult});
            this.kryptonWorkspaceToolboxRename.Root.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.kryptonWorkspaceToolboxRename.Root.UniqueName = "d2af397dfebe40509007f3a61a52a985";
            this.kryptonWorkspaceToolboxRename.Root.WorkspaceControl = this.kryptonWorkspaceToolboxRename;
            this.kryptonWorkspaceToolboxRename.SeparatorStyle = Krypton.Toolkit.SeparatorStyle.HighProfile;
            this.kryptonWorkspaceToolboxRename.Size = new System.Drawing.Size(400, 701);
            this.kryptonWorkspaceToolboxRename.TabIndex = 0;
            this.kryptonWorkspaceToolboxRename.TabStop = true;
            // 
            // kryptonPageToolboxRenameVariables
            // 
            this.kryptonPageToolboxRenameVariables.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageToolboxRenameVariables.AutoScroll = true;
            this.kryptonPageToolboxRenameVariables.Controls.Add(this.buttonRenameUpdate);
            this.kryptonPageToolboxRenameVariables.Controls.Add(this.checkBoxRenameShowFullPath);
            this.kryptonPageToolboxRenameVariables.Controls.Add(this.buttonRenameSave);
            this.kryptonPageToolboxRenameVariables.Controls.Add(this.label2);
            this.kryptonPageToolboxRenameVariables.Controls.Add(this.textBoxRenameNewName);
            this.kryptonPageToolboxRenameVariables.Controls.Add(this.label1);
            this.kryptonPageToolboxRenameVariables.Controls.Add(this.comboBoxRenameVariableList);
            this.kryptonPageToolboxRenameVariables.Flags = 65534;
            this.kryptonPageToolboxRenameVariables.LastVisibleSet = true;
            this.kryptonPageToolboxRenameVariables.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageToolboxRenameVariables.Name = "kryptonPageToolboxRenameVariables";
            this.kryptonPageToolboxRenameVariables.Size = new System.Drawing.Size(398, 148);
            this.kryptonPageToolboxRenameVariables.Text = "Rename variables";
            this.kryptonPageToolboxRenameVariables.TextDescription = "Rename media files using variables from metadata";
            this.kryptonPageToolboxRenameVariables.TextTitle = "Rename variables";
            this.kryptonPageToolboxRenameVariables.ToolTipTitle = "Page ToolTip";
            this.kryptonPageToolboxRenameVariables.UniqueName = "f45d1fdbb6b34b7ebbaa8f9537efeb8c";
            // 
            // buttonRenameUpdate
            // 
            this.buttonRenameUpdate.Location = new System.Drawing.Point(5, 85);
            this.buttonRenameUpdate.Name = "buttonRenameUpdate";
            this.buttonRenameUpdate.Size = new System.Drawing.Size(66, 22);
            this.buttonRenameUpdate.TabIndex = 2;
            this.buttonRenameUpdate.Values.Text = "Update";
            this.buttonRenameUpdate.Click += new System.EventHandler(this.buttonRenameUpdate_Click);
            // 
            // checkBoxRenameShowFullPath
            // 
            this.checkBoxRenameShowFullPath.Location = new System.Drawing.Point(117, 60);
            this.checkBoxRenameShowFullPath.Name = "checkBoxRenameShowFullPath";
            this.checkBoxRenameShowFullPath.Size = new System.Drawing.Size(94, 18);
            this.checkBoxRenameShowFullPath.TabIndex = 5;
            this.checkBoxRenameShowFullPath.Values.Text = "Show full path";
            this.checkBoxRenameShowFullPath.CheckedChanged += new System.EventHandler(this.checkBoxRenameShowFullPath_CheckedChanged);
            // 
            // buttonRenameSave
            // 
            this.buttonRenameSave.Location = new System.Drawing.Point(76, 85);
            this.buttonRenameSave.Name = "buttonRenameSave";
            this.buttonRenameSave.Size = new System.Drawing.Size(66, 22);
            this.buttonRenameSave.TabIndex = 3;
            this.buttonRenameSave.Values.Text = "Rename";
            this.buttonRenameSave.Click += new System.EventHandler(this.buttonRenameSave_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(3, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 18);
            this.label2.TabIndex = 3;
            this.label2.Values.Text = "List of variables:";
            // 
            // textBoxRenameNewName
            // 
            this.textBoxRenameNewName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxRenameNewName.Location = new System.Drawing.Point(118, 31);
            this.textBoxRenameNewName.Name = "textBoxRenameNewName";
            this.textBoxRenameNewName.Size = new System.Drawing.Size(270, 21);
            this.textBoxRenameNewName.TabIndex = 1;
            this.textBoxRenameNewName.Leave += new System.EventHandler(this.textBoxRenameNewName_Leave);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 18);
            this.label1.TabIndex = 2;
            this.label1.Values.Text = "New file name:";
            // 
            // comboBoxRenameVariableList
            // 
            this.comboBoxRenameVariableList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxRenameVariableList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxRenameVariableList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRenameVariableList.DropDownWidth = 581;
            this.comboBoxRenameVariableList.IntegralHeight = false;
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
            "%LocationRegion%",
            "%LocationCity%"});
            this.comboBoxRenameVariableList.Location = new System.Drawing.Point(118, 2);
            this.comboBoxRenameVariableList.Name = "comboBoxRenameVariableList";
            this.comboBoxRenameVariableList.Size = new System.Drawing.Size(270, 21);
            this.comboBoxRenameVariableList.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxRenameVariableList.TabIndex = 0;
            this.comboBoxRenameVariableList.SelectionChangeCommitted += new System.EventHandler(this.comboBoxRenameVariableList_SelectionChangeCommitted);
            // 
            // kryptonWorkspaceCellToolboxRenameVariables
            // 
            this.kryptonWorkspaceCellToolboxRenameVariables.AllowPageDrag = true;
            this.kryptonWorkspaceCellToolboxRenameVariables.AllowTabFocus = false;
            this.kryptonWorkspaceCellToolboxRenameVariables.Button.CloseButtonAction = Krypton.Navigator.CloseButtonAction.None;
            this.kryptonWorkspaceCellToolboxRenameVariables.Button.CloseButtonDisplay = Krypton.Navigator.ButtonDisplay.Hide;
            this.kryptonWorkspaceCellToolboxRenameVariables.Button.ContextButtonAction = Krypton.Navigator.ContextButtonAction.None;
            this.kryptonWorkspaceCellToolboxRenameVariables.Button.ContextButtonDisplay = Krypton.Navigator.ButtonDisplay.Hide;
            this.kryptonWorkspaceCellToolboxRenameVariables.Name = "kryptonWorkspaceCellToolboxRenameVariables";
            this.kryptonWorkspaceCellToolboxRenameVariables.NavigatorMode = Krypton.Navigator.NavigatorMode.Group;
            this.kryptonWorkspaceCellToolboxRenameVariables.Pages.AddRange(new Krypton.Navigator.KryptonPage[] {
            this.kryptonPageToolboxRenameVariables});
            this.kryptonWorkspaceCellToolboxRenameVariables.SelectedIndex = 0;
            this.kryptonWorkspaceCellToolboxRenameVariables.StarSize = "50*,150";
            this.kryptonWorkspaceCellToolboxRenameVariables.UniqueName = "806962740d2445e0b6cd5dd93b9d3b16";
            // 
            // kryptonWorkspaceCellToolboxRenameResult
            // 
            this.kryptonWorkspaceCellToolboxRenameResult.AllowPageDrag = true;
            this.kryptonWorkspaceCellToolboxRenameResult.AllowTabFocus = false;
            this.kryptonWorkspaceCellToolboxRenameResult.Button.CloseButtonAction = Krypton.Navigator.CloseButtonAction.None;
            this.kryptonWorkspaceCellToolboxRenameResult.Button.CloseButtonDisplay = Krypton.Navigator.ButtonDisplay.Hide;
            this.kryptonWorkspaceCellToolboxRenameResult.Button.ContextButtonAction = Krypton.Navigator.ContextButtonAction.None;
            this.kryptonWorkspaceCellToolboxRenameResult.Button.ContextButtonDisplay = Krypton.Navigator.ButtonDisplay.Hide;
            this.kryptonWorkspaceCellToolboxRenameResult.Name = "kryptonWorkspaceCellToolboxRenameResult";
            this.kryptonWorkspaceCellToolboxRenameResult.NavigatorMode = Krypton.Navigator.NavigatorMode.Group;
            this.kryptonWorkspaceCellToolboxRenameResult.Pages.AddRange(new Krypton.Navigator.KryptonPage[] {
            this.kryptonPageToolboxRenameResult});
            this.kryptonWorkspaceCellToolboxRenameResult.SelectedIndex = 0;
            this.kryptonWorkspaceCellToolboxRenameResult.UniqueName = "d60b5ad8d1ab44a1a379eddf3a2fa018";
            // 
            // kryptonPageToolboxRenameResult
            // 
            this.kryptonPageToolboxRenameResult.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageToolboxRenameResult.Controls.Add(this.dataGridViewRename);
            this.kryptonPageToolboxRenameResult.Flags = 65534;
            this.kryptonPageToolboxRenameResult.LastVisibleSet = true;
            this.kryptonPageToolboxRenameResult.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageToolboxRenameResult.Name = "kryptonPageToolboxRenameResult";
            this.kryptonPageToolboxRenameResult.Size = new System.Drawing.Size(398, 544);
            this.kryptonPageToolboxRenameResult.Text = "Rename to";
            this.kryptonPageToolboxRenameResult.TextDescription = "Rename to this names";
            this.kryptonPageToolboxRenameResult.TextTitle = "Rename to";
            this.kryptonPageToolboxRenameResult.ToolTipTitle = "Rename to this names. When click update the variable are used and result will be " +
    "present here.";
            this.kryptonPageToolboxRenameResult.UniqueName = "2559c619897041ffb2113270757adb71";
            // 
            // dataGridViewRename
            // 
            this.dataGridViewRename.AllowUserToAddRows = false;
            this.dataGridViewRename.AllowUserToDeleteRows = false;
            this.dataGridViewRename.ColumnHeadersHeight = 29;
            this.dataGridViewRename.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewRename.KryptonContextMenu = this.kryptonContextMenuGenericBase;
            this.dataGridViewRename.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewRename.Name = "dataGridViewRename";
            this.dataGridViewRename.RowHeadersWidth = 51;
            this.dataGridViewRename.RowTemplate.Height = 24;
            this.dataGridViewRename.Size = new System.Drawing.Size(398, 544);
            this.dataGridViewRename.TabIndex = 4;
            this.dataGridViewRename.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewRename_CellBeginEdit);
            this.dataGridViewRename.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewRename_CellEnter);
            this.dataGridViewRename.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewRename_CellMouseClick);
            this.dataGridViewRename.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridViewRename_CellPainting);
            // 
            // kryptonPageToolboxConvertAndMerge
            // 
            this.kryptonPageToolboxConvertAndMerge.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageToolboxConvertAndMerge.Controls.Add(this.dataGridViewConvertAndMerge);
            this.kryptonPageToolboxConvertAndMerge.Flags = 65534;
            this.kryptonPageToolboxConvertAndMerge.LastVisibleSet = true;
            this.kryptonPageToolboxConvertAndMerge.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageToolboxConvertAndMerge.Name = "kryptonPageToolboxConvertAndMerge";
            this.kryptonPageToolboxConvertAndMerge.Size = new System.Drawing.Size(400, 701);
            this.kryptonPageToolboxConvertAndMerge.Tag = "Convert and Merge";
            this.kryptonPageToolboxConvertAndMerge.Text = "Convert & Merge";
            this.kryptonPageToolboxConvertAndMerge.TextDescription = "Convert & Merge image and video into slideshow";
            this.kryptonPageToolboxConvertAndMerge.TextTitle = "Convert & Merge";
            this.kryptonPageToolboxConvertAndMerge.ToolTipTitle = "Convert & Merge image and video into slideshow";
            this.kryptonPageToolboxConvertAndMerge.UniqueName = "02e9b4cff49e4613ab21aae890bf63ce";
            this.kryptonPageToolboxConvertAndMerge.Enter += new System.EventHandler(this.kryptonPageToolboxConvertAndMerge_Enter);
            // 
            // dataGridViewConvertAndMerge
            // 
            this.dataGridViewConvertAndMerge.AllowDrop = true;
            this.dataGridViewConvertAndMerge.AllowUserToAddRows = false;
            this.dataGridViewConvertAndMerge.AllowUserToDeleteRows = false;
            this.dataGridViewConvertAndMerge.ColumnHeadersHeight = 29;
            this.dataGridViewConvertAndMerge.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewConvertAndMerge.KryptonContextMenu = this.kryptonContextMenuGenericBase;
            this.dataGridViewConvertAndMerge.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewConvertAndMerge.Name = "dataGridViewConvertAndMerge";
            this.dataGridViewConvertAndMerge.RowHeadersWidth = 51;
            this.dataGridViewConvertAndMerge.RowTemplate.Height = 24;
            this.dataGridViewConvertAndMerge.Size = new System.Drawing.Size(400, 701);
            this.dataGridViewConvertAndMerge.TabIndex = 1;
            this.dataGridViewConvertAndMerge.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewConvertAndMerge_CellEnter);
            this.dataGridViewConvertAndMerge.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewConvertAndMerge_CellMouseClick);
            this.dataGridViewConvertAndMerge.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridViewConvertAndMerge_CellPainting);
            this.dataGridViewConvertAndMerge.DragDrop += new System.Windows.Forms.DragEventHandler(this.dataGridViewConvertAndMerge_DragDrop);
            this.dataGridViewConvertAndMerge.DragOver += new System.Windows.Forms.DragEventHandler(this.dataGridViewConvertAndMerge_DragOver);
            this.dataGridViewConvertAndMerge.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridViewConvertAndMerge_MouseDown);
            this.dataGridViewConvertAndMerge.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dataGridViewConvertAndMerge_MouseMove);
            // 
            // toolStripContainerStripMainForm
            // 
            this.toolStripContainerStripMainForm.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripContainerStripMainForm.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripContainerStripMainForm.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStripContainerStripMainForm.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonPreview,
            this.toolStripButtonSelectPrevious,
            this.toolStripDropDownButtonSelectGroupBy,
            this.toolStripButtonSelectNext});
            this.toolStripContainerStripMainForm.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolStripContainerStripMainForm.Location = new System.Drawing.Point(3, 0);
            this.toolStripContainerStripMainForm.Name = "toolStripContainerStripMainForm";
            this.toolStripContainerStripMainForm.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStripContainerStripMainForm.Size = new System.Drawing.Size(178, 27);
            this.toolStripContainerStripMainForm.TabIndex = 0;
            // 
            // toolStripButtonPreview
            // 
            this.toolStripButtonPreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPreview.Image = global::PhotoTagsSynchronizer.Properties.Resources.MediaPreview;
            this.toolStripButtonPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPreview.Name = "toolStripButtonPreview";
            this.toolStripButtonPreview.Size = new System.Drawing.Size(24, 24);
            this.toolStripButtonPreview.Text = "Preview media";
            this.toolStripButtonPreview.ToolTipText = "Preview media";
            // 
            // toolStripButtonSelectPrevious
            // 
            this.toolStripButtonSelectPrevious.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSelectPrevious.Image = global::PhotoTagsSynchronizer.Properties.Resources.SelectPrevious;
            this.toolStripButtonSelectPrevious.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSelectPrevious.Name = "toolStripButtonSelectPrevious";
            this.toolStripButtonSelectPrevious.Size = new System.Drawing.Size(24, 24);
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
            this.toolStripDropDownButtonSelectGroupBy.Size = new System.Drawing.Size(105, 19);
            this.toolStripDropDownButtonSelectGroupBy.Text = "Select group by:";
            // 
            // toolStripMenuItemSelectSameDay
            // 
            this.toolStripMenuItemSelectSameDay.Name = "toolStripMenuItemSelectSameDay";
            this.toolStripMenuItemSelectSameDay.Size = new System.Drawing.Size(313, 22);
            this.toolStripMenuItemSelectSameDay.Text = "Select on same Day";
            this.toolStripMenuItemSelectSameDay.Click += new System.EventHandler(this.toolStripMenuItemSelectSameDay_Click);
            // 
            // toolStripMenuItemSelectSame3Day
            // 
            this.toolStripMenuItemSelectSame3Day.Name = "toolStripMenuItemSelectSame3Day";
            this.toolStripMenuItemSelectSame3Day.Size = new System.Drawing.Size(313, 22);
            this.toolStripMenuItemSelectSame3Day.Text = "Select within 3 Day range";
            this.toolStripMenuItemSelectSame3Day.Click += new System.EventHandler(this.toolStripMenuItemSelectSame3Day_Click);
            // 
            // toolStripMenuItemSelectSameWeek
            // 
            this.toolStripMenuItemSelectSameWeek.Name = "toolStripMenuItemSelectSameWeek";
            this.toolStripMenuItemSelectSameWeek.Size = new System.Drawing.Size(313, 22);
            this.toolStripMenuItemSelectSameWeek.Text = "Select within Week range (7 days)";
            this.toolStripMenuItemSelectSameWeek.Click += new System.EventHandler(this.toolStripMenuItemSelectSameWeek_Click);
            // 
            // toolStripMenuItemSelectSame2week
            // 
            this.toolStripMenuItemSelectSame2week.Name = "toolStripMenuItemSelectSame2week";
            this.toolStripMenuItemSelectSame2week.Size = new System.Drawing.Size(313, 22);
            this.toolStripMenuItemSelectSame2week.Text = "Select within 2 weeks range (14 days)";
            this.toolStripMenuItemSelectSame2week.Click += new System.EventHandler(this.toolStripMenuItemSelectSame2week_Click);
            // 
            // toolStripMenuItemSelectSameMonth
            // 
            this.toolStripMenuItemSelectSameMonth.Name = "toolStripMenuItemSelectSameMonth";
            this.toolStripMenuItemSelectSameMonth.Size = new System.Drawing.Size(313, 22);
            this.toolStripMenuItemSelectSameMonth.Text = "Select within Month range (30 days)";
            this.toolStripMenuItemSelectSameMonth.Click += new System.EventHandler(this.toolStripMenuItemSelectSameMonth_Click);
            // 
            // toolStripMenuItemSelectFallbackOnFileCreated
            // 
            this.toolStripMenuItemSelectFallbackOnFileCreated.Name = "toolStripMenuItemSelectFallbackOnFileCreated";
            this.toolStripMenuItemSelectFallbackOnFileCreated.Size = new System.Drawing.Size(313, 22);
            this.toolStripMenuItemSelectFallbackOnFileCreated.Text = "Use Date Taken with Fall back to Date Created";
            this.toolStripMenuItemSelectFallbackOnFileCreated.Click += new System.EventHandler(this.toolStripMenuItemSelectFallbackOnFileCreated_Click);
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            this.toolStripSeparator13.Size = new System.Drawing.Size(310, 6);
            // 
            // toolStripMenuItemSelectMax10items
            // 
            this.toolStripMenuItemSelectMax10items.Name = "toolStripMenuItemSelectMax10items";
            this.toolStripMenuItemSelectMax10items.Size = new System.Drawing.Size(313, 22);
            this.toolStripMenuItemSelectMax10items.Text = "Select max 10 media files";
            this.toolStripMenuItemSelectMax10items.Click += new System.EventHandler(this.toolStripMenuItemSelectMax10items_Click);
            // 
            // toolStripMenuItemSelectMax30items
            // 
            this.toolStripMenuItemSelectMax30items.Name = "toolStripMenuItemSelectMax30items";
            this.toolStripMenuItemSelectMax30items.Size = new System.Drawing.Size(313, 22);
            this.toolStripMenuItemSelectMax30items.Text = "Select max 30 media files";
            this.toolStripMenuItemSelectMax30items.Click += new System.EventHandler(this.toolStripMenuItemSelectMax30items_Click);
            // 
            // toolStripMenuItemSelectMax50items
            // 
            this.toolStripMenuItemSelectMax50items.Name = "toolStripMenuItemSelectMax50items";
            this.toolStripMenuItemSelectMax50items.Size = new System.Drawing.Size(313, 22);
            this.toolStripMenuItemSelectMax50items.Text = "Select max 50 media files";
            this.toolStripMenuItemSelectMax50items.Click += new System.EventHandler(this.toolStripMenuItemSelectMax50items_Click);
            // 
            // toolStripMenuItemSelectMax100items
            // 
            this.toolStripMenuItemSelectMax100items.Name = "toolStripMenuItemSelectMax100items";
            this.toolStripMenuItemSelectMax100items.Size = new System.Drawing.Size(313, 22);
            this.toolStripMenuItemSelectMax100items.Text = "Select max 100 media files";
            this.toolStripMenuItemSelectMax100items.Click += new System.EventHandler(this.toolStripMenuItemSelectMax100items_Click);
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(310, 6);
            // 
            // toolStripMenuItemSelectSameLocationName
            // 
            this.toolStripMenuItemSelectSameLocationName.Name = "toolStripMenuItemSelectSameLocationName";
            this.toolStripMenuItemSelectSameLocationName.Size = new System.Drawing.Size(313, 22);
            this.toolStripMenuItemSelectSameLocationName.Text = "Select same Location Name";
            this.toolStripMenuItemSelectSameLocationName.Click += new System.EventHandler(this.toolStripMenuItemSelectSameLocationName_Click);
            // 
            // toolStripMenuItemSelectSameCity
            // 
            this.toolStripMenuItemSelectSameCity.Name = "toolStripMenuItemSelectSameCity";
            this.toolStripMenuItemSelectSameCity.Size = new System.Drawing.Size(313, 22);
            this.toolStripMenuItemSelectSameCity.Text = "Select same City";
            this.toolStripMenuItemSelectSameCity.Click += new System.EventHandler(this.toolStripMenuItemSelectSameCity_Click);
            // 
            // toolStripMenuItemSelectSameDistrict
            // 
            this.toolStripMenuItemSelectSameDistrict.Name = "toolStripMenuItemSelectSameDistrict";
            this.toolStripMenuItemSelectSameDistrict.Size = new System.Drawing.Size(313, 22);
            this.toolStripMenuItemSelectSameDistrict.Text = "Select same District";
            this.toolStripMenuItemSelectSameDistrict.Click += new System.EventHandler(this.toolStripMenuItemSelectSameDistrict_Click);
            // 
            // toolStripMenuItemSelectSameCountry
            // 
            this.toolStripMenuItemSelectSameCountry.Name = "toolStripMenuItemSelectSameCountry";
            this.toolStripMenuItemSelectSameCountry.Size = new System.Drawing.Size(313, 22);
            this.toolStripMenuItemSelectSameCountry.Text = "Select same Country";
            this.toolStripMenuItemSelectSameCountry.Click += new System.EventHandler(this.toolStripMenuItemSelectSameCountry_Click);
            // 
            // toolStripButtonSelectNext
            // 
            this.toolStripButtonSelectNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSelectNext.Image = global::PhotoTagsSynchronizer.Properties.Resources.SelectNext;
            this.toolStripButtonSelectNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSelectNext.Name = "toolStripButtonSelectNext";
            this.toolStripButtonSelectNext.Size = new System.Drawing.Size(24, 24);
            this.toolStripButtonSelectNext.Text = "Select Next Group";
            this.toolStripButtonSelectNext.Click += new System.EventHandler(this.toolStripButtonSelectNext_Click);
            // 
            // contextMenuStripTagsAndKeywords
            // 
            this.contextMenuStripTagsAndKeywords.Font = new System.Drawing.Font("Segoe UI", 9F);
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
            this.toolStripMenuItemKeywordsShowFavoriteRows,
            this.toolStripMenuItemKeywordsHideEqualRows,
            this.toolStripMenuItemTagsBrokerCopyText,
            this.toolStripMenuItemTagsAndKeywordsBrokerOverwriteText,
            this.toggleTagsAndKeywordsSelectionToolStripMenuItem,
            this.selectTagsAndKeywordsToolStripMenuItem,
            this.removeTagsAndKeywordsToolStripMenuItem,
            this.toolStripMenuItemShowPosterWindowKeywords,
            this.toolStripMenuItemTagsAndKeywordMediaPreview});
            this.contextMenuStripTagsAndKeywords.Name = "contextMenuStripMap";
            this.contextMenuStripTagsAndKeywords.Size = new System.Drawing.Size(428, 550);
            // 
            // cutToolStripMenuTagsBrokerCut
            // 
            this.cutToolStripMenuTagsBrokerCut.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditCut32x32;
            this.cutToolStripMenuTagsBrokerCut.Name = "cutToolStripMenuTagsBrokerCut";
            this.cutToolStripMenuTagsBrokerCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.cutToolStripMenuTagsBrokerCut.Size = new System.Drawing.Size(427, 26);
            this.cutToolStripMenuTagsBrokerCut.Text = "Cut";
            // 
            // copyToolStripMenuTagsBrokerCopy
            // 
            this.copyToolStripMenuTagsBrokerCopy.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditCopy32x32;
            this.copyToolStripMenuTagsBrokerCopy.Name = "copyToolStripMenuTagsBrokerCopy";
            this.copyToolStripMenuTagsBrokerCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyToolStripMenuTagsBrokerCopy.Size = new System.Drawing.Size(427, 26);
            this.copyToolStripMenuTagsBrokerCopy.Text = "Copy";
            // 
            // pasteToolStripMenuTagsBrokerPaste
            // 
            this.pasteToolStripMenuTagsBrokerPaste.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditPaste32x32;
            this.pasteToolStripMenuTagsBrokerPaste.Name = "pasteToolStripMenuTagsBrokerPaste";
            this.pasteToolStripMenuTagsBrokerPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.pasteToolStripMenuTagsBrokerPaste.Size = new System.Drawing.Size(427, 26);
            this.pasteToolStripMenuTagsBrokerPaste.Text = "Paste";
            // 
            // deleteToolStripMenuTagsBrokerDelete
            // 
            this.deleteToolStripMenuTagsBrokerDelete.Image = global::PhotoTagsSynchronizer.Properties.Resources.FileSystemDelete32x32;
            this.deleteToolStripMenuTagsBrokerDelete.Name = "deleteToolStripMenuTagsBrokerDelete";
            this.deleteToolStripMenuTagsBrokerDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.deleteToolStripMenuTagsBrokerDelete.Size = new System.Drawing.Size(427, 26);
            this.deleteToolStripMenuTagsBrokerDelete.Text = "Delete";
            // 
            // undoToolStripMenuTags
            // 
            this.undoToolStripMenuTags.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditUndo32x32;
            this.undoToolStripMenuTags.Name = "undoToolStripMenuTags";
            this.undoToolStripMenuTags.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoToolStripMenuTags.Size = new System.Drawing.Size(427, 26);
            this.undoToolStripMenuTags.Text = "Undo";
            // 
            // redoToolStripMenuTags
            // 
            this.redoToolStripMenuTags.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditRedo32x32;
            this.redoToolStripMenuTags.Name = "redoToolStripMenuTags";
            this.redoToolStripMenuTags.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.redoToolStripMenuTags.Size = new System.Drawing.Size(427, 26);
            this.redoToolStripMenuTags.Text = "Redo";
            // 
            // findToolStripMenuTag
            // 
            this.findToolStripMenuTag.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditFind32x32;
            this.findToolStripMenuTag.Name = "findToolStripMenuTag";
            this.findToolStripMenuTag.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.findToolStripMenuTag.Size = new System.Drawing.Size(427, 26);
            this.findToolStripMenuTag.Text = "Find";
            // 
            // replaceToolStripMenuTag
            // 
            this.replaceToolStripMenuTag.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditReplace32x32;
            this.replaceToolStripMenuTag.Name = "replaceToolStripMenuTag";
            this.replaceToolStripMenuTag.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.replaceToolStripMenuTag.Size = new System.Drawing.Size(427, 26);
            this.replaceToolStripMenuTag.Text = "Replace";
            // 
            // toolStripMenuTagsBrokerSave
            // 
            this.toolStripMenuTagsBrokerSave.Image = global::PhotoTagsSynchronizer.Properties.Resources.MetadataSave32x32;
            this.toolStripMenuTagsBrokerSave.Name = "toolStripMenuTagsBrokerSave";
            this.toolStripMenuTagsBrokerSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.toolStripMenuTagsBrokerSave.Size = new System.Drawing.Size(427, 26);
            this.toolStripMenuTagsBrokerSave.Text = "Save";
            // 
            // markAsFavoriteToolStripMenuItem
            // 
            this.markAsFavoriteToolStripMenuItem.Image = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteSelect32x32;
            this.markAsFavoriteToolStripMenuItem.Name = "markAsFavoriteToolStripMenuItem";
            this.markAsFavoriteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.markAsFavoriteToolStripMenuItem.Size = new System.Drawing.Size(427, 26);
            this.markAsFavoriteToolStripMenuItem.Text = "Mark as favorite";
            // 
            // removeAsFavoriteToolStripMenuItem
            // 
            this.removeAsFavoriteToolStripMenuItem.Image = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteRemove32x32;
            this.removeAsFavoriteToolStripMenuItem.Name = "removeAsFavoriteToolStripMenuItem";
            this.removeAsFavoriteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D)));
            this.removeAsFavoriteToolStripMenuItem.Size = new System.Drawing.Size(427, 26);
            this.removeAsFavoriteToolStripMenuItem.Text = "Remove as favorite";
            // 
            // toggleFavoriteToolStripMenuItem
            // 
            this.toggleFavoriteToolStripMenuItem.Image = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteToggle32x32;
            this.toggleFavoriteToolStripMenuItem.Name = "toggleFavoriteToolStripMenuItem";
            this.toggleFavoriteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.D)));
            this.toggleFavoriteToolStripMenuItem.Size = new System.Drawing.Size(427, 26);
            this.toggleFavoriteToolStripMenuItem.Text = "Toggle favorite";
            // 
            // toolStripMenuItemKeywordsShowFavoriteRows
            // 
            this.toolStripMenuItemKeywordsShowFavoriteRows.Name = "toolStripMenuItemKeywordsShowFavoriteRows";
            this.toolStripMenuItemKeywordsShowFavoriteRows.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.B)));
            this.toolStripMenuItemKeywordsShowFavoriteRows.Size = new System.Drawing.Size(427, 26);
            this.toolStripMenuItemKeywordsShowFavoriteRows.Text = "Show favorite rows";
            // 
            // toolStripMenuItemKeywordsHideEqualRows
            // 
            this.toolStripMenuItemKeywordsHideEqualRows.Name = "toolStripMenuItemKeywordsHideEqualRows";
            this.toolStripMenuItemKeywordsHideEqualRows.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.B)));
            this.toolStripMenuItemKeywordsHideEqualRows.Size = new System.Drawing.Size(427, 26);
            this.toolStripMenuItemKeywordsHideEqualRows.Text = "Hide equal rows";
            // 
            // toolStripMenuItemTagsBrokerCopyText
            // 
            this.toolStripMenuItemTagsBrokerCopyText.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItemTagsBrokerCopyText.Image")));
            this.toolStripMenuItemTagsBrokerCopyText.Name = "toolStripMenuItemTagsBrokerCopyText";
            this.toolStripMenuItemTagsBrokerCopyText.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.C)));
            this.toolStripMenuItemTagsBrokerCopyText.Size = new System.Drawing.Size(427, 26);
            this.toolStripMenuItemTagsBrokerCopyText.Text = "Copy selected values to media file without overwrite";
            // 
            // toolStripMenuItemTagsAndKeywordsBrokerOverwriteText
            // 
            this.toolStripMenuItemTagsAndKeywordsBrokerOverwriteText.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItemTagsAndKeywordsBrokerOverwriteText.Image")));
            this.toolStripMenuItemTagsAndKeywordsBrokerOverwriteText.Name = "toolStripMenuItemTagsAndKeywordsBrokerOverwriteText";
            this.toolStripMenuItemTagsAndKeywordsBrokerOverwriteText.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.C)));
            this.toolStripMenuItemTagsAndKeywordsBrokerOverwriteText.Size = new System.Drawing.Size(427, 26);
            this.toolStripMenuItemTagsAndKeywordsBrokerOverwriteText.Text = "Copy selected values to media file and overwrite";
            // 
            // toggleTagsAndKeywordsSelectionToolStripMenuItem
            // 
            this.toggleTagsAndKeywordsSelectionToolStripMenuItem.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditTriStateToggle32x32;
            this.toggleTagsAndKeywordsSelectionToolStripMenuItem.Name = "toggleTagsAndKeywordsSelectionToolStripMenuItem";
            this.toggleTagsAndKeywordsSelectionToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Space)));
            this.toggleTagsAndKeywordsSelectionToolStripMenuItem.Size = new System.Drawing.Size(427, 26);
            this.toggleTagsAndKeywordsSelectionToolStripMenuItem.Text = "Toggle selected keyword tag";
            // 
            // selectTagsAndKeywordsToolStripMenuItem
            // 
            this.selectTagsAndKeywordsToolStripMenuItem.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditTriStateSelect32x32;
            this.selectTagsAndKeywordsToolStripMenuItem.Name = "selectTagsAndKeywordsToolStripMenuItem";
            this.selectTagsAndKeywordsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.Space)));
            this.selectTagsAndKeywordsToolStripMenuItem.Size = new System.Drawing.Size(427, 26);
            this.selectTagsAndKeywordsToolStripMenuItem.Text = "Set selected keyword tags";
            // 
            // removeTagsAndKeywordsToolStripMenuItem
            // 
            this.removeTagsAndKeywordsToolStripMenuItem.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditTriStateDelete32x32;
            this.removeTagsAndKeywordsToolStripMenuItem.Name = "removeTagsAndKeywordsToolStripMenuItem";
            this.removeTagsAndKeywordsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete)));
            this.removeTagsAndKeywordsToolStripMenuItem.Size = new System.Drawing.Size(427, 26);
            this.removeTagsAndKeywordsToolStripMenuItem.Text = "Remove selected keyword tags";
            // 
            // toolStripMenuItemShowPosterWindowKeywords
            // 
            this.toolStripMenuItemShowPosterWindowKeywords.Image = global::PhotoTagsSynchronizer.Properties.Resources.MediaRegionSelector;
            this.toolStripMenuItemShowPosterWindowKeywords.Name = "toolStripMenuItemShowPosterWindowKeywords";
            this.toolStripMenuItemShowPosterWindowKeywords.Size = new System.Drawing.Size(427, 26);
            this.toolStripMenuItemShowPosterWindowKeywords.Text = "Show Media Poster Window";
            // 
            // toolStripMenuItemTagsAndKeywordMediaPreview
            // 
            this.toolStripMenuItemTagsAndKeywordMediaPreview.Image = global::PhotoTagsSynchronizer.Properties.Resources.MediaPreview;
            this.toolStripMenuItemTagsAndKeywordMediaPreview.Name = "toolStripMenuItemTagsAndKeywordMediaPreview";
            this.toolStripMenuItemTagsAndKeywordMediaPreview.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.toolStripMenuItemTagsAndKeywordMediaPreview.Size = new System.Drawing.Size(427, 26);
            this.toolStripMenuItemTagsAndKeywordMediaPreview.Text = "Media Preview";
            // 
            // contextMenuStripPeople
            // 
            this.contextMenuStripPeople.Font = new System.Drawing.Font("Segoe UI", 9F);
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
            this.contextMenuStripPeople.Size = new System.Drawing.Size(302, 628);
            // 
            // toolStripMenuItemPeopleRenameFromLast1
            // 
            this.toolStripMenuItemPeopleRenameFromLast1.Name = "toolStripMenuItemPeopleRenameFromLast1";
            this.toolStripMenuItemPeopleRenameFromLast1.Size = new System.Drawing.Size(301, 26);
            this.toolStripMenuItemPeopleRenameFromLast1.Tag = "Unknown 1";
            this.toolStripMenuItemPeopleRenameFromLast1.Text = "Rename #1 - Unknown 1";
            this.toolStripMenuItemPeopleRenameFromLast1.Click += new System.EventHandler(this.toolStripMenuItemPeopleRenameFromLast1_Click);
            // 
            // toolStripMenuItemPeopleRenameFromLast2
            // 
            this.toolStripMenuItemPeopleRenameFromLast2.Name = "toolStripMenuItemPeopleRenameFromLast2";
            this.toolStripMenuItemPeopleRenameFromLast2.Size = new System.Drawing.Size(301, 26);
            this.toolStripMenuItemPeopleRenameFromLast2.Tag = "Unknown 2";
            this.toolStripMenuItemPeopleRenameFromLast2.Text = "Rename #2 - Unknown 2";
            this.toolStripMenuItemPeopleRenameFromLast2.Click += new System.EventHandler(this.toolStripMenuItemPeopleRenameFromLast2_Click);
            // 
            // toolStripMenuItemPeopleRenameFromLast3
            // 
            this.toolStripMenuItemPeopleRenameFromLast3.Name = "toolStripMenuItemPeopleRenameFromLast3";
            this.toolStripMenuItemPeopleRenameFromLast3.Size = new System.Drawing.Size(301, 26);
            this.toolStripMenuItemPeopleRenameFromLast3.Tag = "Unknown 3";
            this.toolStripMenuItemPeopleRenameFromLast3.Text = "Rename #3 - Unknown 3";
            this.toolStripMenuItemPeopleRenameFromLast3.Click += new System.EventHandler(this.toolStripMenuItemPeopleRenameFromLast3_Click);
            // 
            // toolStripMenuItemPeopleRenameFromMostUsed
            // 
            this.toolStripMenuItemPeopleRenameFromMostUsed.Name = "toolStripMenuItemPeopleRenameFromMostUsed";
            this.toolStripMenuItemPeopleRenameFromMostUsed.Size = new System.Drawing.Size(301, 26);
            this.toolStripMenuItemPeopleRenameFromMostUsed.Text = "Rename - From most used";
            // 
            // toolStripMenuItemPeopleRenameFromAll
            // 
            this.toolStripMenuItemPeopleRenameFromAll.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemPeopleSelected,
            this.toolStripMenuItemPeopleRenameSelected});
            this.toolStripMenuItemPeopleRenameFromAll.Name = "toolStripMenuItemPeopleRenameFromAll";
            this.toolStripMenuItemPeopleRenameFromAll.Size = new System.Drawing.Size(301, 26);
            this.toolStripMenuItemPeopleRenameFromAll.Text = "Rename - List all";
            // 
            // ToolStripMenuItemPeopleSelected
            // 
            this.ToolStripMenuItemPeopleSelected.Name = "ToolStripMenuItemPeopleSelected";
            this.ToolStripMenuItemPeopleSelected.Size = new System.Drawing.Size(133, 22);
            this.ToolStripMenuItemPeopleSelected.Text = "(Unknown)";
            // 
            // toolStripMenuItemPeopleRenameSelected
            // 
            this.toolStripMenuItemPeopleRenameSelected.Name = "toolStripMenuItemPeopleRenameSelected";
            this.toolStripMenuItemPeopleRenameSelected.Size = new System.Drawing.Size(133, 22);
            this.toolStripMenuItemPeopleRenameSelected.Text = "Me";
            this.toolStripMenuItemPeopleRenameSelected.Click += new System.EventHandler(this.toolStripMenuItemPeopleRenameSelected_Click);
            // 
            // toolStripMenuItemPeopleCut
            // 
            this.toolStripMenuItemPeopleCut.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditCut32x32;
            this.toolStripMenuItemPeopleCut.Name = "toolStripMenuItemPeopleCut";
            this.toolStripMenuItemPeopleCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.toolStripMenuItemPeopleCut.Size = new System.Drawing.Size(301, 26);
            this.toolStripMenuItemPeopleCut.Text = "Cut";
            // 
            // toolStripMenuItemPeopleCopy
            // 
            this.toolStripMenuItemPeopleCopy.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditCopy32x32;
            this.toolStripMenuItemPeopleCopy.Name = "toolStripMenuItemPeopleCopy";
            this.toolStripMenuItemPeopleCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.toolStripMenuItemPeopleCopy.Size = new System.Drawing.Size(301, 26);
            this.toolStripMenuItemPeopleCopy.Text = "Copy";
            // 
            // toolStripMenuItemPeoplePaste
            // 
            this.toolStripMenuItemPeoplePaste.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditPaste32x32;
            this.toolStripMenuItemPeoplePaste.Name = "toolStripMenuItemPeoplePaste";
            this.toolStripMenuItemPeoplePaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.toolStripMenuItemPeoplePaste.Size = new System.Drawing.Size(301, 26);
            this.toolStripMenuItemPeoplePaste.Text = "Paste";
            // 
            // toolStripMenuItemPeopleDelete
            // 
            this.toolStripMenuItemPeopleDelete.Image = global::PhotoTagsSynchronizer.Properties.Resources.FileSystemDelete32x32;
            this.toolStripMenuItemPeopleDelete.Name = "toolStripMenuItemPeopleDelete";
            this.toolStripMenuItemPeopleDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.toolStripMenuItemPeopleDelete.Size = new System.Drawing.Size(301, 26);
            this.toolStripMenuItemPeopleDelete.Text = "Delete";
            // 
            // toolStripMenuItemPeopleUndo
            // 
            this.toolStripMenuItemPeopleUndo.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditUndo32x32;
            this.toolStripMenuItemPeopleUndo.Name = "toolStripMenuItemPeopleUndo";
            this.toolStripMenuItemPeopleUndo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.toolStripMenuItemPeopleUndo.Size = new System.Drawing.Size(301, 26);
            this.toolStripMenuItemPeopleUndo.Text = "Undo";
            // 
            // toolStripMenuItemPeopleRedo
            // 
            this.toolStripMenuItemPeopleRedo.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditRedo32x32;
            this.toolStripMenuItemPeopleRedo.Name = "toolStripMenuItemPeopleRedo";
            this.toolStripMenuItemPeopleRedo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.toolStripMenuItemPeopleRedo.Size = new System.Drawing.Size(301, 26);
            this.toolStripMenuItemPeopleRedo.Text = "Redo";
            // 
            // toolStripMenuItemPeopleFind
            // 
            this.toolStripMenuItemPeopleFind.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditFind32x32;
            this.toolStripMenuItemPeopleFind.Name = "toolStripMenuItemPeopleFind";
            this.toolStripMenuItemPeopleFind.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.toolStripMenuItemPeopleFind.Size = new System.Drawing.Size(301, 26);
            this.toolStripMenuItemPeopleFind.Text = "Find";
            // 
            // toolStripMenuItemPeopleReplace
            // 
            this.toolStripMenuItemPeopleReplace.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditReplace32x32;
            this.toolStripMenuItemPeopleReplace.Name = "toolStripMenuItemPeopleReplace";
            this.toolStripMenuItemPeopleReplace.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.toolStripMenuItemPeopleReplace.Size = new System.Drawing.Size(301, 26);
            this.toolStripMenuItemPeopleReplace.Text = "Replace";
            // 
            // toolStripMenuItemPeopleSave
            // 
            this.toolStripMenuItemPeopleSave.Image = global::PhotoTagsSynchronizer.Properties.Resources.MetadataSave32x32;
            this.toolStripMenuItemPeopleSave.Name = "toolStripMenuItemPeopleSave";
            this.toolStripMenuItemPeopleSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.toolStripMenuItemPeopleSave.Size = new System.Drawing.Size(301, 26);
            this.toolStripMenuItemPeopleSave.Text = "Save";
            // 
            // toolStripMenuItemPeopleMarkFavorite
            // 
            this.toolStripMenuItemPeopleMarkFavorite.Image = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteSelect32x32;
            this.toolStripMenuItemPeopleMarkFavorite.Name = "toolStripMenuItemPeopleMarkFavorite";
            this.toolStripMenuItemPeopleMarkFavorite.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.toolStripMenuItemPeopleMarkFavorite.Size = new System.Drawing.Size(301, 26);
            this.toolStripMenuItemPeopleMarkFavorite.Text = "Mark as favorite";
            // 
            // toolStripMenuItemPeopleRemoveFavorite
            // 
            this.toolStripMenuItemPeopleRemoveFavorite.Image = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteRemove32x32;
            this.toolStripMenuItemPeopleRemoveFavorite.Name = "toolStripMenuItemPeopleRemoveFavorite";
            this.toolStripMenuItemPeopleRemoveFavorite.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D)));
            this.toolStripMenuItemPeopleRemoveFavorite.Size = new System.Drawing.Size(301, 26);
            this.toolStripMenuItemPeopleRemoveFavorite.Text = "Remove as favorite";
            // 
            // toolStripMenuItemPeopleToggleFavorite
            // 
            this.toolStripMenuItemPeopleToggleFavorite.Image = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteToggle32x32;
            this.toolStripMenuItemPeopleToggleFavorite.Name = "toolStripMenuItemPeopleToggleFavorite";
            this.toolStripMenuItemPeopleToggleFavorite.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.D)));
            this.toolStripMenuItemPeopleToggleFavorite.Size = new System.Drawing.Size(301, 26);
            this.toolStripMenuItemPeopleToggleFavorite.Text = "Toggle favorite";
            // 
            // toolStripMenuItemPeopleShowFavorite
            // 
            this.toolStripMenuItemPeopleShowFavorite.Name = "toolStripMenuItemPeopleShowFavorite";
            this.toolStripMenuItemPeopleShowFavorite.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.B)));
            this.toolStripMenuItemPeopleShowFavorite.Size = new System.Drawing.Size(301, 26);
            this.toolStripMenuItemPeopleShowFavorite.Text = "Show favorite rows";
            // 
            // toolStripMenuItemPeopleHideEqualRows
            // 
            this.toolStripMenuItemPeopleHideEqualRows.Name = "toolStripMenuItemPeopleHideEqualRows";
            this.toolStripMenuItemPeopleHideEqualRows.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.B)));
            this.toolStripMenuItemPeopleHideEqualRows.Size = new System.Drawing.Size(301, 26);
            this.toolStripMenuItemPeopleHideEqualRows.Text = "Hide equal rows";
            // 
            // toolStripMenuItemPeopleTogglePeopleTag
            // 
            this.toolStripMenuItemPeopleTogglePeopleTag.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditTriStateToggle32x32;
            this.toolStripMenuItemPeopleTogglePeopleTag.Name = "toolStripMenuItemPeopleTogglePeopleTag";
            this.toolStripMenuItemPeopleTogglePeopleTag.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Space)));
            this.toolStripMenuItemPeopleTogglePeopleTag.Size = new System.Drawing.Size(301, 26);
            this.toolStripMenuItemPeopleTogglePeopleTag.Text = "Toggle selected people tag";
            // 
            // toolStripMenuItemPeopleSelectPeopleTag
            // 
            this.toolStripMenuItemPeopleSelectPeopleTag.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditTriStateSelect32x32;
            this.toolStripMenuItemPeopleSelectPeopleTag.Name = "toolStripMenuItemPeopleSelectPeopleTag";
            this.toolStripMenuItemPeopleSelectPeopleTag.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.Space)));
            this.toolStripMenuItemPeopleSelectPeopleTag.Size = new System.Drawing.Size(301, 26);
            this.toolStripMenuItemPeopleSelectPeopleTag.Text = "Set selected people tags";
            // 
            // toolStripMenuItemPeopleRemovePeopleTag
            // 
            this.toolStripMenuItemPeopleRemovePeopleTag.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditTriStateDelete32x32;
            this.toolStripMenuItemPeopleRemovePeopleTag.Name = "toolStripMenuItemPeopleRemovePeopleTag";
            this.toolStripMenuItemPeopleRemovePeopleTag.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete)));
            this.toolStripMenuItemPeopleRemovePeopleTag.Size = new System.Drawing.Size(301, 26);
            this.toolStripMenuItemPeopleRemovePeopleTag.Text = "Remove selected people tags";
            // 
            // toolStripMenuItemPeopleShowRegionSelector
            // 
            this.toolStripMenuItemPeopleShowRegionSelector.Image = global::PhotoTagsSynchronizer.Properties.Resources.MediaRegionSelector;
            this.toolStripMenuItemPeopleShowRegionSelector.Name = "toolStripMenuItemPeopleShowRegionSelector";
            this.toolStripMenuItemPeopleShowRegionSelector.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.toolStripMenuItemPeopleShowRegionSelector.Size = new System.Drawing.Size(301, 26);
            this.toolStripMenuItemPeopleShowRegionSelector.Text = "Show Region Selector Window";
            // 
            // toolStripMenuItemPeopleMediaPreview
            // 
            this.toolStripMenuItemPeopleMediaPreview.Image = global::PhotoTagsSynchronizer.Properties.Resources.MediaPreview;
            this.toolStripMenuItemPeopleMediaPreview.Name = "toolStripMenuItemPeopleMediaPreview";
            this.toolStripMenuItemPeopleMediaPreview.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.toolStripMenuItemPeopleMediaPreview.Size = new System.Drawing.Size(301, 26);
            this.toolStripMenuItemPeopleMediaPreview.Text = "Media Preview";
            // 
            // contextMenuStripMap
            // 
            this.contextMenuStripMap.Font = new System.Drawing.Font("Segoe UI", 9F);
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
            this.toolStripMenuItemShowCoordinateOnGoogleMap,
            this.toolStripMenuItemMapReloadLocationUsingNominatim,
            this.toolStripMenuItemShowPosterWindowMap,
            this.toolStripMenuItemMapMediaPreview});
            this.contextMenuStripMap.Name = "contextMenuStripMap";
            this.contextMenuStripMap.Size = new System.Drawing.Size(428, 550);
            // 
            // toolStripMenuItemMapCut
            // 
            this.toolStripMenuItemMapCut.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditCut32x32;
            this.toolStripMenuItemMapCut.Name = "toolStripMenuItemMapCut";
            this.toolStripMenuItemMapCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.toolStripMenuItemMapCut.Size = new System.Drawing.Size(427, 26);
            this.toolStripMenuItemMapCut.Text = "Cut";
            // 
            // toolStripMenuItemMapCopy
            // 
            this.toolStripMenuItemMapCopy.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditCopy32x32;
            this.toolStripMenuItemMapCopy.Name = "toolStripMenuItemMapCopy";
            this.toolStripMenuItemMapCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.toolStripMenuItemMapCopy.Size = new System.Drawing.Size(427, 26);
            this.toolStripMenuItemMapCopy.Text = "Copy";
            // 
            // toolStripMenuItemMapPaste
            // 
            this.toolStripMenuItemMapPaste.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditPaste32x32;
            this.toolStripMenuItemMapPaste.Name = "toolStripMenuItemMapPaste";
            this.toolStripMenuItemMapPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.toolStripMenuItemMapPaste.Size = new System.Drawing.Size(427, 26);
            this.toolStripMenuItemMapPaste.Text = "Paste";
            // 
            // toolStripMenuItemMapDelete
            // 
            this.toolStripMenuItemMapDelete.Image = global::PhotoTagsSynchronizer.Properties.Resources.FileSystemDelete32x32;
            this.toolStripMenuItemMapDelete.Name = "toolStripMenuItemMapDelete";
            this.toolStripMenuItemMapDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.toolStripMenuItemMapDelete.Size = new System.Drawing.Size(427, 26);
            this.toolStripMenuItemMapDelete.Text = "Delete";
            // 
            // toolStripMenuItemMapUndo
            // 
            this.toolStripMenuItemMapUndo.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditUndo32x32;
            this.toolStripMenuItemMapUndo.Name = "toolStripMenuItemMapUndo";
            this.toolStripMenuItemMapUndo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.toolStripMenuItemMapUndo.Size = new System.Drawing.Size(427, 26);
            this.toolStripMenuItemMapUndo.Text = "Undo";
            // 
            // toolStripMenuItemMapRedo
            // 
            this.toolStripMenuItemMapRedo.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditRedo32x32;
            this.toolStripMenuItemMapRedo.Name = "toolStripMenuItemMapRedo";
            this.toolStripMenuItemMapRedo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.toolStripMenuItemMapRedo.Size = new System.Drawing.Size(427, 26);
            this.toolStripMenuItemMapRedo.Text = "Redo";
            // 
            // toolStripMenuItemMapFind
            // 
            this.toolStripMenuItemMapFind.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditFind32x32;
            this.toolStripMenuItemMapFind.Name = "toolStripMenuItemMapFind";
            this.toolStripMenuItemMapFind.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Space)));
            this.toolStripMenuItemMapFind.Size = new System.Drawing.Size(427, 26);
            this.toolStripMenuItemMapFind.Text = "Find";
            // 
            // toolStripMenuItemMapReplace
            // 
            this.toolStripMenuItemMapReplace.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditReplace32x32;
            this.toolStripMenuItemMapReplace.Name = "toolStripMenuItemMapReplace";
            this.toolStripMenuItemMapReplace.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.toolStripMenuItemMapReplace.Size = new System.Drawing.Size(427, 26);
            this.toolStripMenuItemMapReplace.Text = "Replace";
            // 
            // toolStripMenuItemMapSave
            // 
            this.toolStripMenuItemMapSave.Image = global::PhotoTagsSynchronizer.Properties.Resources.MetadataSave32x32;
            this.toolStripMenuItemMapSave.Name = "toolStripMenuItemMapSave";
            this.toolStripMenuItemMapSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.toolStripMenuItemMapSave.Size = new System.Drawing.Size(427, 26);
            this.toolStripMenuItemMapSave.Text = "Save";
            // 
            // toolStripMenuItemMapMarkFavorite
            // 
            this.toolStripMenuItemMapMarkFavorite.Image = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteSelect32x32;
            this.toolStripMenuItemMapMarkFavorite.Name = "toolStripMenuItemMapMarkFavorite";
            this.toolStripMenuItemMapMarkFavorite.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.toolStripMenuItemMapMarkFavorite.Size = new System.Drawing.Size(427, 26);
            this.toolStripMenuItemMapMarkFavorite.Text = "Mark as favorite";
            // 
            // toolStripMenuItemMapRemoveFavorite
            // 
            this.toolStripMenuItemMapRemoveFavorite.Image = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteRemove32x32;
            this.toolStripMenuItemMapRemoveFavorite.Name = "toolStripMenuItemMapRemoveFavorite";
            this.toolStripMenuItemMapRemoveFavorite.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D)));
            this.toolStripMenuItemMapRemoveFavorite.Size = new System.Drawing.Size(427, 26);
            this.toolStripMenuItemMapRemoveFavorite.Text = "Remove as favorite";
            // 
            // toolStripMenuItemMapToggleFavorite
            // 
            this.toolStripMenuItemMapToggleFavorite.Image = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteToggle32x32;
            this.toolStripMenuItemMapToggleFavorite.Name = "toolStripMenuItemMapToggleFavorite";
            this.toolStripMenuItemMapToggleFavorite.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.D)));
            this.toolStripMenuItemMapToggleFavorite.Size = new System.Drawing.Size(427, 26);
            this.toolStripMenuItemMapToggleFavorite.Text = "Toggle favorite";
            // 
            // toolStripMenuItemMapShowFavorite
            // 
            this.toolStripMenuItemMapShowFavorite.Name = "toolStripMenuItemMapShowFavorite";
            this.toolStripMenuItemMapShowFavorite.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.B)));
            this.toolStripMenuItemMapShowFavorite.Size = new System.Drawing.Size(427, 26);
            this.toolStripMenuItemMapShowFavorite.Text = "Show favorite rows";
            // 
            // toolStripMenuItemMapHideEqual
            // 
            this.toolStripMenuItemMapHideEqual.Name = "toolStripMenuItemMapHideEqual";
            this.toolStripMenuItemMapHideEqual.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.B)));
            this.toolStripMenuItemMapHideEqual.Size = new System.Drawing.Size(427, 26);
            this.toolStripMenuItemMapHideEqual.Text = "Hide equal rows";
            // 
            // toolStripMenuItemMapCopyNotOverwrite
            // 
            this.toolStripMenuItemMapCopyNotOverwrite.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItemMapCopyNotOverwrite.Image")));
            this.toolStripMenuItemMapCopyNotOverwrite.Name = "toolStripMenuItemMapCopyNotOverwrite";
            this.toolStripMenuItemMapCopyNotOverwrite.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.C)));
            this.toolStripMenuItemMapCopyNotOverwrite.Size = new System.Drawing.Size(427, 26);
            this.toolStripMenuItemMapCopyNotOverwrite.Text = "Copy selected values to media file without overwrite";
            // 
            // toolStripMenuItemMapCopyAndOverwrite
            // 
            this.toolStripMenuItemMapCopyAndOverwrite.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItemMapCopyAndOverwrite.Image")));
            this.toolStripMenuItemMapCopyAndOverwrite.Name = "toolStripMenuItemMapCopyAndOverwrite";
            this.toolStripMenuItemMapCopyAndOverwrite.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.C)));
            this.toolStripMenuItemMapCopyAndOverwrite.Size = new System.Drawing.Size(427, 26);
            this.toolStripMenuItemMapCopyAndOverwrite.Text = "Copy selected values to media file and overwrite";
            // 
            // toolStripMenuItemShowCoordinateOnMap
            // 
            this.toolStripMenuItemShowCoordinateOnMap.Image = global::PhotoTagsSynchronizer.Properties.Resources.ShowLocation;
            this.toolStripMenuItemShowCoordinateOnMap.Name = "toolStripMenuItemShowCoordinateOnMap";
            this.toolStripMenuItemShowCoordinateOnMap.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.toolStripMenuItemShowCoordinateOnMap.Size = new System.Drawing.Size(427, 26);
            this.toolStripMenuItemShowCoordinateOnMap.Text = "Show Coordinate on OpenStreetMap";
            this.toolStripMenuItemShowCoordinateOnMap.Click += new System.EventHandler(this.toolStripMenuItemShowCoordinateOnMap_Click);
            // 
            // toolStripMenuItemShowCoordinateOnGoogleMap
            // 
            this.toolStripMenuItemShowCoordinateOnGoogleMap.Image = global::PhotoTagsSynchronizer.Properties.Resources.ShowLocation;
            this.toolStripMenuItemShowCoordinateOnGoogleMap.Name = "toolStripMenuItemShowCoordinateOnGoogleMap";
            this.toolStripMenuItemShowCoordinateOnGoogleMap.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.M)));
            this.toolStripMenuItemShowCoordinateOnGoogleMap.Size = new System.Drawing.Size(427, 26);
            this.toolStripMenuItemShowCoordinateOnGoogleMap.Text = "Show Coordinate on Google Map";
            this.toolStripMenuItemShowCoordinateOnGoogleMap.Click += new System.EventHandler(this.toolStripMenuItemShowCoordinateOnGoogleMap_Click);
            // 
            // toolStripMenuItemMapReloadLocationUsingNominatim
            // 
            this.toolStripMenuItemMapReloadLocationUsingNominatim.Image = global::PhotoTagsSynchronizer.Properties.Resources.LocationReload;
            this.toolStripMenuItemMapReloadLocationUsingNominatim.Name = "toolStripMenuItemMapReloadLocationUsingNominatim";
            this.toolStripMenuItemMapReloadLocationUsingNominatim.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.toolStripMenuItemMapReloadLocationUsingNominatim.Size = new System.Drawing.Size(427, 26);
            this.toolStripMenuItemMapReloadLocationUsingNominatim.Text = "Reload Location using Nominatim";
            this.toolStripMenuItemMapReloadLocationUsingNominatim.Click += new System.EventHandler(this.toolStripMenuItemMapReloadLocationUsingNominatim_Click);
            // 
            // toolStripMenuItemShowPosterWindowMap
            // 
            this.toolStripMenuItemShowPosterWindowMap.Image = global::PhotoTagsSynchronizer.Properties.Resources.MediaRegionSelector;
            this.toolStripMenuItemShowPosterWindowMap.Name = "toolStripMenuItemShowPosterWindowMap";
            this.toolStripMenuItemShowPosterWindowMap.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.toolStripMenuItemShowPosterWindowMap.Size = new System.Drawing.Size(427, 26);
            this.toolStripMenuItemShowPosterWindowMap.Text = "Show Media Poster Window";
            // 
            // toolStripMenuItemMapMediaPreview
            // 
            this.toolStripMenuItemMapMediaPreview.Image = global::PhotoTagsSynchronizer.Properties.Resources.MediaPreview;
            this.toolStripMenuItemMapMediaPreview.Name = "toolStripMenuItemMapMediaPreview";
            this.toolStripMenuItemMapMediaPreview.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.toolStripMenuItemMapMediaPreview.Size = new System.Drawing.Size(427, 26);
            this.toolStripMenuItemMapMediaPreview.Text = "Media Preview";
            // 
            // contextMenuStripDate
            // 
            this.contextMenuStripDate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.contextMenuStripDate.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripDate.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemDateCut,
            this.toolStripMenuItemDateCopy,
            this.toolStripMenuItemDatePaste,
            this.toolStripMenuItemDateDelete,
            this.toolStripMenuItemDateUndo,
            this.toolStripMenuItemDateRedo,
            this.toolStripMenuItemDateFind,
            this.toolStripMenuItemDateReplace,
            this.toolStripMenuItemDateSave,
            this.toolStripMenuItemDateMarkFavorite,
            this.toolStripMenuItemDateRemoveFavorite,
            this.toolStripMenuItemDateToggleFavourite,
            this.toolStripMenuItemDateShowFavorite,
            this.toolStripMenuItemDateHideEqualRows,
            this.toolStripMenuItemShowPosterWindowDate,
            this.toolStripMenuItemDateMediaPreview});
            this.contextMenuStripDate.Name = "contextMenuStripMap";
            this.contextMenuStripDate.Size = new System.Drawing.Size(268, 420);
            // 
            // toolStripMenuItemDateCut
            // 
            this.toolStripMenuItemDateCut.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditCut32x32;
            this.toolStripMenuItemDateCut.Name = "toolStripMenuItemDateCut";
            this.toolStripMenuItemDateCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.toolStripMenuItemDateCut.Size = new System.Drawing.Size(267, 26);
            this.toolStripMenuItemDateCut.Text = "Cut";
            // 
            // toolStripMenuItemDateCopy
            // 
            this.toolStripMenuItemDateCopy.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditCopy32x32;
            this.toolStripMenuItemDateCopy.Name = "toolStripMenuItemDateCopy";
            this.toolStripMenuItemDateCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.toolStripMenuItemDateCopy.Size = new System.Drawing.Size(267, 26);
            this.toolStripMenuItemDateCopy.Text = "Copy";
            // 
            // toolStripMenuItemDatePaste
            // 
            this.toolStripMenuItemDatePaste.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditPaste32x32;
            this.toolStripMenuItemDatePaste.Name = "toolStripMenuItemDatePaste";
            this.toolStripMenuItemDatePaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.toolStripMenuItemDatePaste.Size = new System.Drawing.Size(267, 26);
            this.toolStripMenuItemDatePaste.Text = "Paste";
            // 
            // toolStripMenuItemDateDelete
            // 
            this.toolStripMenuItemDateDelete.Image = global::PhotoTagsSynchronizer.Properties.Resources.FileSystemDelete32x32;
            this.toolStripMenuItemDateDelete.Name = "toolStripMenuItemDateDelete";
            this.toolStripMenuItemDateDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.toolStripMenuItemDateDelete.Size = new System.Drawing.Size(267, 26);
            this.toolStripMenuItemDateDelete.Text = "Delete";
            // 
            // toolStripMenuItemDateUndo
            // 
            this.toolStripMenuItemDateUndo.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditUndo32x32;
            this.toolStripMenuItemDateUndo.Name = "toolStripMenuItemDateUndo";
            this.toolStripMenuItemDateUndo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.toolStripMenuItemDateUndo.Size = new System.Drawing.Size(267, 26);
            this.toolStripMenuItemDateUndo.Text = "Undo";
            // 
            // toolStripMenuItemDateRedo
            // 
            this.toolStripMenuItemDateRedo.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditRedo32x32;
            this.toolStripMenuItemDateRedo.Name = "toolStripMenuItemDateRedo";
            this.toolStripMenuItemDateRedo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.toolStripMenuItemDateRedo.Size = new System.Drawing.Size(267, 26);
            this.toolStripMenuItemDateRedo.Text = "Redo";
            // 
            // toolStripMenuItemDateFind
            // 
            this.toolStripMenuItemDateFind.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditFind32x32;
            this.toolStripMenuItemDateFind.Name = "toolStripMenuItemDateFind";
            this.toolStripMenuItemDateFind.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.toolStripMenuItemDateFind.Size = new System.Drawing.Size(267, 26);
            this.toolStripMenuItemDateFind.Text = "Find";
            // 
            // toolStripMenuItemDateReplace
            // 
            this.toolStripMenuItemDateReplace.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditReplace32x32;
            this.toolStripMenuItemDateReplace.Name = "toolStripMenuItemDateReplace";
            this.toolStripMenuItemDateReplace.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.toolStripMenuItemDateReplace.Size = new System.Drawing.Size(267, 26);
            this.toolStripMenuItemDateReplace.Text = "Replace";
            // 
            // toolStripMenuItemDateSave
            // 
            this.toolStripMenuItemDateSave.Image = global::PhotoTagsSynchronizer.Properties.Resources.MetadataSave32x32;
            this.toolStripMenuItemDateSave.Name = "toolStripMenuItemDateSave";
            this.toolStripMenuItemDateSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.toolStripMenuItemDateSave.Size = new System.Drawing.Size(267, 26);
            this.toolStripMenuItemDateSave.Text = "Save";
            // 
            // toolStripMenuItemDateMarkFavorite
            // 
            this.toolStripMenuItemDateMarkFavorite.Image = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteSelect32x32;
            this.toolStripMenuItemDateMarkFavorite.Name = "toolStripMenuItemDateMarkFavorite";
            this.toolStripMenuItemDateMarkFavorite.Size = new System.Drawing.Size(267, 26);
            this.toolStripMenuItemDateMarkFavorite.Text = "Mark as favorite";
            // 
            // toolStripMenuItemDateRemoveFavorite
            // 
            this.toolStripMenuItemDateRemoveFavorite.Image = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteRemove32x32;
            this.toolStripMenuItemDateRemoveFavorite.Name = "toolStripMenuItemDateRemoveFavorite";
            this.toolStripMenuItemDateRemoveFavorite.Size = new System.Drawing.Size(267, 26);
            this.toolStripMenuItemDateRemoveFavorite.Text = "Remove as favorite";
            // 
            // toolStripMenuItemDateToggleFavourite
            // 
            this.toolStripMenuItemDateToggleFavourite.Image = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteToggle32x32;
            this.toolStripMenuItemDateToggleFavourite.Name = "toolStripMenuItemDateToggleFavourite";
            this.toolStripMenuItemDateToggleFavourite.Size = new System.Drawing.Size(267, 26);
            this.toolStripMenuItemDateToggleFavourite.Text = "Toggle favorite";
            // 
            // toolStripMenuItemDateShowFavorite
            // 
            this.toolStripMenuItemDateShowFavorite.Checked = true;
            this.toolStripMenuItemDateShowFavorite.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.toolStripMenuItemDateShowFavorite.Name = "toolStripMenuItemDateShowFavorite";
            this.toolStripMenuItemDateShowFavorite.Size = new System.Drawing.Size(267, 26);
            this.toolStripMenuItemDateShowFavorite.Text = "Show only favorite rows";
            // 
            // toolStripMenuItemDateHideEqualRows
            // 
            this.toolStripMenuItemDateHideEqualRows.Checked = true;
            this.toolStripMenuItemDateHideEqualRows.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.toolStripMenuItemDateHideEqualRows.Name = "toolStripMenuItemDateHideEqualRows";
            this.toolStripMenuItemDateHideEqualRows.Size = new System.Drawing.Size(267, 26);
            this.toolStripMenuItemDateHideEqualRows.Text = "Hide equal rows";
            // 
            // toolStripMenuItemShowPosterWindowDate
            // 
            this.toolStripMenuItemShowPosterWindowDate.Image = global::PhotoTagsSynchronizer.Properties.Resources.MediaRegionSelector;
            this.toolStripMenuItemShowPosterWindowDate.Name = "toolStripMenuItemShowPosterWindowDate";
            this.toolStripMenuItemShowPosterWindowDate.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.toolStripMenuItemShowPosterWindowDate.Size = new System.Drawing.Size(267, 26);
            this.toolStripMenuItemShowPosterWindowDate.Text = "Show Media Poster Window";
            // 
            // toolStripMenuItemDateMediaPreview
            // 
            this.toolStripMenuItemDateMediaPreview.Image = global::PhotoTagsSynchronizer.Properties.Resources.MediaPreview;
            this.toolStripMenuItemDateMediaPreview.Name = "toolStripMenuItemDateMediaPreview";
            this.toolStripMenuItemDateMediaPreview.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.toolStripMenuItemDateMediaPreview.Size = new System.Drawing.Size(267, 26);
            this.toolStripMenuItemDateMediaPreview.Text = "Media Preview";
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
            // timerShowStatusText_RemoveTimer
            // 
            this.timerShowStatusText_RemoveTimer.Interval = 1500;
            this.timerShowStatusText_RemoveTimer.Tick += new System.EventHandler(this.timerShowStatusText_RemoveTimer_Tick);
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
            // timerStatusThreadQueue
            // 
            this.timerStatusThreadQueue.Enabled = true;
            this.timerStatusThreadQueue.Interval = 400;
            this.timerStatusThreadQueue.Tick += new System.EventHandler(this.timerStatusThreadQueue_Tick);
            // 
            // timerLazyLoadingDataGridViewProgressRemoveProgessbar
            // 
            this.timerLazyLoadingDataGridViewProgressRemoveProgessbar.Interval = 500;
            // 
            // panelMediaPreview
            // 
            this.panelMediaPreview.Controls.Add(this.toolStripContainerMediaPreview);
            this.panelMediaPreview.Location = new System.Drawing.Point(452, 599);
            this.panelMediaPreview.Margin = new System.Windows.Forms.Padding(0);
            this.panelMediaPreview.Name = "panelMediaPreview";
            this.panelMediaPreview.Size = new System.Drawing.Size(757, 76);
            this.panelMediaPreview.TabIndex = 7;
            this.panelMediaPreview.Visible = false;
            // 
            // toolStripContainerMediaPreview
            // 
            this.toolStripContainerMediaPreview.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainerMediaPreview.ContentPanel
            // 
            this.toolStripContainerMediaPreview.ContentPanel.Controls.Add(this.imageBoxPreview);
            this.toolStripContainerMediaPreview.ContentPanel.Controls.Add(this.videoView1);
            this.toolStripContainerMediaPreview.ContentPanel.Size = new System.Drawing.Size(757, 28);
            this.toolStripContainerMediaPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainerMediaPreview.LeftToolStripPanelVisible = false;
            this.toolStripContainerMediaPreview.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainerMediaPreview.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripContainerMediaPreview.Name = "toolStripContainerMediaPreview";
            this.toolStripContainerMediaPreview.RightToolStripPanelVisible = false;
            this.toolStripContainerMediaPreview.Size = new System.Drawing.Size(757, 76);
            this.toolStripContainerMediaPreview.TabIndex = 3;
            this.toolStripContainerMediaPreview.Text = "toolStripContainer2";
            // 
            // toolStripContainerMediaPreview.TopToolStripPanel
            // 
            this.toolStripContainerMediaPreview.TopToolStripPanel.Controls.Add(this.toolStripContainerStripMediaPreview);
            // 
            // imageBoxPreview
            // 
            this.imageBoxPreview.BackColor = System.Drawing.Color.Black;
            this.imageBoxPreview.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.imageBoxPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageBoxPreview.GridDisplayMode = Cyotek.Windows.Forms.ImageBoxGridDisplayMode.None;
            this.imageBoxPreview.Location = new System.Drawing.Point(0, 0);
            this.imageBoxPreview.Name = "imageBoxPreview";
            this.imageBoxPreview.Size = new System.Drawing.Size(757, 28);
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
            this.videoView1.Size = new System.Drawing.Size(757, 28);
            this.videoView1.TabIndex = 2;
            this.videoView1.Text = "videoView1";
            // 
            // toolStripContainerStripMediaPreview
            // 
            this.toolStripContainerStripMediaPreview.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripContainerStripMediaPreview.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripContainerStripMediaPreview.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStripContainerStripMediaPreview.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
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
            this.toolStripContainerStripMediaPreview.Location = new System.Drawing.Point(3, 0);
            this.toolStripContainerStripMediaPreview.Name = "toolStripContainerStripMediaPreview";
            this.toolStripContainerStripMediaPreview.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStripContainerStripMediaPreview.Size = new System.Drawing.Size(728, 48);
            this.toolStripContainerStripMediaPreview.TabIndex = 0;
            this.toolStripContainerStripMediaPreview.Text = "toolStrip1";
            // 
            // toolStripButtonMediaPreviewPrevious
            // 
            this.toolStripButtonMediaPreviewPrevious.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonMediaPreviewPrevious.Image = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerPrevious;
            this.toolStripButtonMediaPreviewPrevious.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMediaPreviewPrevious.Name = "toolStripButtonMediaPreviewPrevious";
            this.toolStripButtonMediaPreviewPrevious.Size = new System.Drawing.Size(24, 45);
            this.toolStripButtonMediaPreviewPrevious.Text = "Previous media";
            this.toolStripButtonMediaPreviewPrevious.Click += new System.EventHandler(this.toolStripButtonMediaPreviewPrevious_Click);
            // 
            // toolStripButtonMediaPreviewNext
            // 
            this.toolStripButtonMediaPreviewNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonMediaPreviewNext.Image = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerNext;
            this.toolStripButtonMediaPreviewNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMediaPreviewNext.Name = "toolStripButtonMediaPreviewNext";
            this.toolStripButtonMediaPreviewNext.Size = new System.Drawing.Size(24, 45);
            this.toolStripButtonMediaPreviewNext.Text = "Next media";
            this.toolStripButtonMediaPreviewNext.Click += new System.EventHandler(this.toolStripButtonMediaPreviewNext_Click);
            // 
            // toolStripButtonMediaPreviewPlay
            // 
            this.toolStripButtonMediaPreviewPlay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonMediaPreviewPlay.Enabled = false;
            this.toolStripButtonMediaPreviewPlay.Image = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerPlay;
            this.toolStripButtonMediaPreviewPlay.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMediaPreviewPlay.Name = "toolStripButtonMediaPreviewPlay";
            this.toolStripButtonMediaPreviewPlay.Size = new System.Drawing.Size(24, 45);
            this.toolStripButtonMediaPreviewPlay.Text = "Play";
            this.toolStripButtonMediaPreviewPlay.Click += new System.EventHandler(this.toolStripButtonMediaPreviewPlay_Click);
            // 
            // toolStripButtonMediaPreviewPause
            // 
            this.toolStripButtonMediaPreviewPause.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonMediaPreviewPause.Enabled = false;
            this.toolStripButtonMediaPreviewPause.Image = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerPause;
            this.toolStripButtonMediaPreviewPause.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMediaPreviewPause.Name = "toolStripButtonMediaPreviewPause";
            this.toolStripButtonMediaPreviewPause.Size = new System.Drawing.Size(24, 45);
            this.toolStripButtonMediaPreviewPause.Text = "Pause";
            this.toolStripButtonMediaPreviewPause.Click += new System.EventHandler(this.toolStripButtonMediaPreviewPause_Click);
            // 
            // toolStripButtonMediaPreviewFastBackward
            // 
            this.toolStripButtonMediaPreviewFastBackward.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonMediaPreviewFastBackward.Enabled = false;
            this.toolStripButtonMediaPreviewFastBackward.Image = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerFastBackward;
            this.toolStripButtonMediaPreviewFastBackward.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMediaPreviewFastBackward.Name = "toolStripButtonMediaPreviewFastBackward";
            this.toolStripButtonMediaPreviewFastBackward.Size = new System.Drawing.Size(24, 45);
            this.toolStripButtonMediaPreviewFastBackward.Text = "Fast Backward";
            this.toolStripButtonMediaPreviewFastBackward.Click += new System.EventHandler(this.toolStripButtonMediaPreviewFastBackward_Click);
            // 
            // toolStripButtonMediaPreviewFastForward
            // 
            this.toolStripButtonMediaPreviewFastForward.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonMediaPreviewFastForward.Enabled = false;
            this.toolStripButtonMediaPreviewFastForward.Image = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerFastForward;
            this.toolStripButtonMediaPreviewFastForward.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMediaPreviewFastForward.Name = "toolStripButtonMediaPreviewFastForward";
            this.toolStripButtonMediaPreviewFastForward.Size = new System.Drawing.Size(24, 45);
            this.toolStripButtonMediaPreviewFastForward.Text = "Fast Forward";
            this.toolStripButtonMediaPreviewFastForward.Click += new System.EventHandler(this.toolStripButtonMediaPreviewFastForward_Click);
            // 
            // toolStripButtonMediaPreviewStop
            // 
            this.toolStripButtonMediaPreviewStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonMediaPreviewStop.Enabled = false;
            this.toolStripButtonMediaPreviewStop.Image = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerStop;
            this.toolStripButtonMediaPreviewStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMediaPreviewStop.Name = "toolStripButtonMediaPreviewStop";
            this.toolStripButtonMediaPreviewStop.Size = new System.Drawing.Size(24, 45);
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
            this.toolStripDropDownButtonChromecastList.Image = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerChromecast;
            this.toolStripDropDownButtonChromecastList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonChromecastList.Name = "toolStripDropDownButtonChromecastList";
            this.toolStripDropDownButtonChromecastList.Size = new System.Drawing.Size(33, 45);
            this.toolStripDropDownButtonChromecastList.Text = "Select Chromecast device";
            // 
            // toolStripMenuItemMediaChromecast
            // 
            this.toolStripMenuItemMediaChromecast.Name = "toolStripMenuItemMediaChromecast";
            this.toolStripMenuItemMediaChromecast.Size = new System.Drawing.Size(91, 22);
            this.toolStripMenuItemMediaChromecast.Text = "Tv1";
            // 
            // tv2ToolStripMenuItem
            // 
            this.tv2ToolStripMenuItem.Name = "tv2ToolStripMenuItem";
            this.tv2ToolStripMenuItem.Size = new System.Drawing.Size(91, 22);
            this.tv2ToolStripMenuItem.Text = "Tv2";
            // 
            // toolStripDropDownButtonMediaList
            // 
            this.toolStripDropDownButtonMediaList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButtonMediaList.Image = global::PhotoTagsSynchronizer.Properties.Resources.MediaPreview;
            this.toolStripDropDownButtonMediaList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonMediaList.Name = "toolStripDropDownButtonMediaList";
            this.toolStripDropDownButtonMediaList.Size = new System.Drawing.Size(33, 45);
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
            this.toolStripMenuItemPreviewSlideShowMenu.Size = new System.Drawing.Size(74, 45);
            this.toolStripMenuItemPreviewSlideShowMenu.Text = "SlideShow";
            // 
            // toolStripMenuItemPreviewSlideShow2sec
            // 
            this.toolStripMenuItemPreviewSlideShow2sec.Name = "toolStripMenuItemPreviewSlideShow2sec";
            this.toolStripMenuItemPreviewSlideShow2sec.Size = new System.Drawing.Size(163, 22);
            this.toolStripMenuItemPreviewSlideShow2sec.Text = "SlideShow 2 sec";
            this.toolStripMenuItemPreviewSlideShow2sec.Click += new System.EventHandler(this.toolStripMenuItemPreviewSlideShow2sec_Click);
            // 
            // toolStripMenuItemPreviewSlideShow4sec
            // 
            this.toolStripMenuItemPreviewSlideShow4sec.Name = "toolStripMenuItemPreviewSlideShow4sec";
            this.toolStripMenuItemPreviewSlideShow4sec.Size = new System.Drawing.Size(163, 22);
            this.toolStripMenuItemPreviewSlideShow4sec.Text = "SlideShow 4 sec";
            this.toolStripMenuItemPreviewSlideShow4sec.Click += new System.EventHandler(this.toolStripMenuItemPreviewSlideShow4sec_Click);
            // 
            // toolStripMenuItemPreviewSlideShow6sec
            // 
            this.toolStripMenuItemPreviewSlideShow6sec.Name = "toolStripMenuItemPreviewSlideShow6sec";
            this.toolStripMenuItemPreviewSlideShow6sec.Size = new System.Drawing.Size(163, 22);
            this.toolStripMenuItemPreviewSlideShow6sec.Text = "SlideShow 6 sec";
            this.toolStripMenuItemPreviewSlideShow6sec.Click += new System.EventHandler(this.toolStripMenuItemPreviewSlideShow6sec_Click);
            // 
            // toolStripMenuItemPreviewSlideShow8sec
            // 
            this.toolStripMenuItemPreviewSlideShow8sec.Name = "toolStripMenuItemPreviewSlideShow8sec";
            this.toolStripMenuItemPreviewSlideShow8sec.Size = new System.Drawing.Size(163, 22);
            this.toolStripMenuItemPreviewSlideShow8sec.Text = "SlideShow 8 sec";
            this.toolStripMenuItemPreviewSlideShow8sec.Click += new System.EventHandler(this.toolStripMenuItemPreviewSlideShow8sec_Click);
            // 
            // toolStripMenuItemPreviewSlideShow10sec
            // 
            this.toolStripMenuItemPreviewSlideShow10sec.Name = "toolStripMenuItemPreviewSlideShow10sec";
            this.toolStripMenuItemPreviewSlideShow10sec.Size = new System.Drawing.Size(163, 22);
            this.toolStripMenuItemPreviewSlideShow10sec.Text = "SlideShow 10 sec";
            this.toolStripMenuItemPreviewSlideShow10sec.Click += new System.EventHandler(this.toolStripMenuItemPreviewSlideShow10sec_Click);
            // 
            // toolStripMenuItemPreviewSlideShowStop
            // 
            this.toolStripMenuItemPreviewSlideShowStop.Enabled = false;
            this.toolStripMenuItemPreviewSlideShowStop.Name = "toolStripMenuItemPreviewSlideShowStop";
            this.toolStripMenuItemPreviewSlideShowStop.Size = new System.Drawing.Size(163, 22);
            this.toolStripMenuItemPreviewSlideShowStop.Text = "SlideShow stop";
            this.toolStripMenuItemPreviewSlideShowStop.Click += new System.EventHandler(this.toolStripMenuItemPreviewSlideShowStop_Click);
            // 
            // toolStripButtonMediaPreviewRotateCCW
            // 
            this.toolStripButtonMediaPreviewRotateCCW.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonMediaPreviewRotateCCW.Image = global::PhotoTagsSynchronizer.Properties.Resources.MediaRotate90CCW;
            this.toolStripButtonMediaPreviewRotateCCW.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMediaPreviewRotateCCW.Name = "toolStripButtonMediaPreviewRotateCCW";
            this.toolStripButtonMediaPreviewRotateCCW.Size = new System.Drawing.Size(24, 45);
            this.toolStripButtonMediaPreviewRotateCCW.Text = "Rotate CCW";
            this.toolStripButtonMediaPreviewRotateCCW.Click += new System.EventHandler(this.toolStripButtonMediaPreviewRotateCCW_Click);
            // 
            // toolStripButtonMediaPreviewRotate180
            // 
            this.toolStripButtonMediaPreviewRotate180.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonMediaPreviewRotate180.Image = global::PhotoTagsSynchronizer.Properties.Resources.MediaRotate180;
            this.toolStripButtonMediaPreviewRotate180.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMediaPreviewRotate180.Name = "toolStripButtonMediaPreviewRotate180";
            this.toolStripButtonMediaPreviewRotate180.Size = new System.Drawing.Size(24, 45);
            this.toolStripButtonMediaPreviewRotate180.Text = "Rotate 180";
            this.toolStripButtonMediaPreviewRotate180.Click += new System.EventHandler(this.toolStripButtonMediaPreviewRotate180_Click);
            // 
            // toolStripButtonMediaPreviewRotateCW
            // 
            this.toolStripButtonMediaPreviewRotateCW.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonMediaPreviewRotateCW.Image = global::PhotoTagsSynchronizer.Properties.Resources.MediaRotate90CW;
            this.toolStripButtonMediaPreviewRotateCW.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMediaPreviewRotateCW.Name = "toolStripButtonMediaPreviewRotateCW";
            this.toolStripButtonMediaPreviewRotateCW.Size = new System.Drawing.Size(24, 45);
            this.toolStripButtonMediaPreviewRotateCW.Text = "Rotate CW";
            this.toolStripButtonMediaPreviewRotateCW.Click += new System.EventHandler(this.toolStripButtonMediaPreviewRotateCW_Click);
            // 
            // toolStripButtonMediaPreviewClose
            // 
            this.toolStripButtonMediaPreviewClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonMediaPreviewClose.Image = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerClose;
            this.toolStripButtonMediaPreviewClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMediaPreviewClose.Name = "toolStripButtonMediaPreviewClose";
            this.toolStripButtonMediaPreviewClose.Size = new System.Drawing.Size(24, 45);
            this.toolStripButtonMediaPreviewClose.Text = "Close preview media";
            this.toolStripButtonMediaPreviewClose.Click += new System.EventHandler(this.toolStripButtonMediaPreviewClose_Click);
            // 
            // toolStripTraceBarItemMediaPreviewTimer
            // 
            this.toolStripTraceBarItemMediaPreviewTimer.Name = "toolStripTraceBarItemMediaPreviewTimer";
            this.toolStripTraceBarItemMediaPreviewTimer.Size = new System.Drawing.Size(200, 45);
            this.toolStripTraceBarItemMediaPreviewTimer.Text = "Video timer";
            this.toolStripTraceBarItemMediaPreviewTimer.ToolTipText = "Video timer";
            this.toolStripTraceBarItemMediaPreviewTimer.ValueChanged += new System.EventHandler(this.toolStripTraceBarItemSeekPosition_ValueChanged);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 48);
            // 
            // toolStripLabelMediaPreviewTimer
            // 
            this.toolStripLabelMediaPreviewTimer.Name = "toolStripLabelMediaPreviewTimer";
            this.toolStripLabelMediaPreviewTimer.Size = new System.Drawing.Size(64, 45);
            this.toolStripLabelMediaPreviewTimer.Text = "Timer: 0.00";
            this.toolStripLabelMediaPreviewTimer.ToolTipText = "Timer";
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(6, 48);
            // 
            // toolStripLabelMediaPreviewStatus
            // 
            this.toolStripLabelMediaPreviewStatus.Name = "toolStripLabelMediaPreviewStatus";
            this.toolStripLabelMediaPreviewStatus.Size = new System.Drawing.Size(39, 45);
            this.toolStripLabelMediaPreviewStatus.Text = "Status";
            this.toolStripLabelMediaPreviewStatus.ToolTipText = "Status";
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(6, 48);
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
            // kryptonManager1
            // 
            this.kryptonManager1.GlobalPaletteMode = Krypton.Toolkit.PaletteModeManager.ProfessionalSystem;
            // 
            // kryptonPage5
            // 
            this.kryptonPage5.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage5.Flags = 65534;
            this.kryptonPage5.LastVisibleSet = true;
            this.kryptonPage5.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPage5.Name = "kryptonPage5";
            this.kryptonPage5.Size = new System.Drawing.Size(100, 100);
            this.kryptonPage5.Text = "kryptonPage5";
            this.kryptonPage5.ToolTipTitle = "Page ToolTip";
            this.kryptonPage5.UniqueName = "9de38782c5bd4790b3383ad816626969";
            // 
            // kryptonPage7
            // 
            this.kryptonPage7.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage7.Flags = 65534;
            this.kryptonPage7.LastVisibleSet = true;
            this.kryptonPage7.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPage7.Name = "kryptonPage7";
            this.kryptonPage7.Size = new System.Drawing.Size(100, 100);
            this.kryptonPage7.Text = "kryptonPage7";
            this.kryptonPage7.ToolTipTitle = "Page ToolTip";
            this.kryptonPage7.UniqueName = "ea23941d46dd492a90655f3fc1eaa4c4";
            // 
            // kryptonPage2
            // 
            this.kryptonPage2.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage2.Flags = 65534;
            this.kryptonPage2.LastVisibleSet = true;
            this.kryptonPage2.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPage2.Name = "kryptonPage2";
            this.kryptonPage2.Size = new System.Drawing.Size(100, 100);
            this.kryptonPage2.Text = "kryptonPage2";
            this.kryptonPage2.ToolTipTitle = "Page ToolTip";
            this.kryptonPage2.UniqueName = "ee48040a6f884f6996a54d47b97f0d4d";
            // 
            // kryptonPage4
            // 
            this.kryptonPage4.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage4.Flags = 65534;
            this.kryptonPage4.LastVisibleSet = true;
            this.kryptonPage4.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPage4.Name = "kryptonPage4";
            this.kryptonPage4.Size = new System.Drawing.Size(100, 100);
            this.kryptonPage4.Text = "kryptonPage4";
            this.kryptonPage4.ToolTipTitle = "Page ToolTip";
            this.kryptonPage4.UniqueName = "29d1e78d2142416e90f48e1eb0ab44f2";
            // 
            // kryptonPage3
            // 
            this.kryptonPage3.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage3.Flags = 65534;
            this.kryptonPage3.LastVisibleSet = true;
            this.kryptonPage3.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPage3.Name = "kryptonPage3";
            this.kryptonPage3.Size = new System.Drawing.Size(100, 100);
            this.kryptonPage3.Text = "kryptonPage3";
            this.kryptonPage3.ToolTipTitle = "Page ToolTip";
            this.kryptonPage3.UniqueName = "f911c170661140658752023223f2ddb0";
            // 
            // kryptonPage10
            // 
            this.kryptonPage10.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage10.Flags = 65534;
            this.kryptonPage10.LastVisibleSet = true;
            this.kryptonPage10.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPage10.Name = "kryptonPage10";
            this.kryptonPage10.Size = new System.Drawing.Size(100, 100);
            this.kryptonPage10.Text = "kryptonPage10";
            this.kryptonPage10.ToolTipTitle = "Page ToolTip";
            this.kryptonPage10.UniqueName = "b4f640a98d0f41bf848cf4d672024ebf";
            // 
            // kryptonPage12
            // 
            this.kryptonPage12.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage12.Flags = 65534;
            this.kryptonPage12.LastVisibleSet = true;
            this.kryptonPage12.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPage12.Name = "kryptonPage12";
            this.kryptonPage12.Size = new System.Drawing.Size(100, 100);
            this.kryptonPage12.Text = "kryptonPage12";
            this.kryptonPage12.ToolTipTitle = "Page ToolTip";
            this.kryptonPage12.UniqueName = "4a8b11102bd24edd8447132efe919b49";
            // 
            // kryptonPage6
            // 
            this.kryptonPage6.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage6.Flags = 65534;
            this.kryptonPage6.LastVisibleSet = true;
            this.kryptonPage6.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPage6.Name = "kryptonPage6";
            this.kryptonPage6.Size = new System.Drawing.Size(100, 100);
            this.kryptonPage6.Text = "kryptonPage6";
            this.kryptonPage6.ToolTipTitle = "Page ToolTip";
            this.kryptonPage6.UniqueName = "bcb5e540e6854142bb477c098360ec11";
            // 
            // kryptonPage9
            // 
            this.kryptonPage9.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage9.Flags = 65534;
            this.kryptonPage9.LastVisibleSet = true;
            this.kryptonPage9.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPage9.Name = "kryptonPage9";
            this.kryptonPage9.Size = new System.Drawing.Size(100, 100);
            this.kryptonPage9.Text = "kryptonPage9";
            this.kryptonPage9.ToolTipTitle = "Page ToolTip";
            this.kryptonPage9.UniqueName = "918bd60282f94d88aa925101cbd10e90";
            // 
            // buttonSpecAny1
            // 
            this.buttonSpecAny1.UniqueName = "e34ed1d91e6445c5905a8d09d4053bde";
            // 
            // buttonSpecAny2
            // 
            this.buttonSpecAny2.UniqueName = "405915ab25ff439e99d11761a4b02629";
            // 
            // kryptonRibbonMain
            // 
            this.kryptonRibbonMain.InDesignHelperMode = true;
            this.kryptonRibbonMain.Name = "kryptonRibbonMain";
            this.kryptonRibbonMain.QATButtons.AddRange(new System.ComponentModel.Component[] {
            this.kryptonRibbonQATButtonSave,
            this.kryptonRibbonQATButtonMediaPreview,
            this.kryptonRibbonQATButtonMediaPoster});
            this.kryptonRibbonMain.QATUserChange = false;
            this.kryptonRibbonMain.RibbonAppButton.AppButtonImage = global::PhotoTagsSynchronizer.Properties.Resources.AppIcon;
            this.kryptonRibbonMain.RibbonTabs.AddRange(new Krypton.Ribbon.KryptonRibbonTab[] {
            this.kryptonRibbonTabHome,
            this.kryptonRibbonTabView,
            this.kryptonRibbonTabSelect,
            this.kryptonRibbonTabTools,
            this.kryptonRibbonTabPreview});
            this.kryptonRibbonMain.SelectedContext = null;
            this.kryptonRibbonMain.SelectedTab = this.kryptonRibbonTabHome;
            this.kryptonRibbonMain.Size = new System.Drawing.Size(1214, 115);
            this.kryptonRibbonMain.TabIndex = 12;
            // 
            // kryptonRibbonQATButtonSave
            // 
            this.kryptonRibbonQATButtonSave.Image = global::PhotoTagsSynchronizer.Properties.Resources.MetadataSave16x16;
            this.kryptonRibbonQATButtonSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.kryptonRibbonQATButtonSave.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.MetadataSave32x32;
            this.kryptonRibbonQATButtonSave.ToolTipTitle = "Save (Ctrl+S)";
            this.kryptonRibbonQATButtonSave.Click += new System.EventHandler(this.kryptonRibbonQATButtonSave_Click);
            // 
            // kryptonRibbonQATButtonMediaPreview
            // 
            this.kryptonRibbonQATButtonMediaPreview.Image = global::PhotoTagsSynchronizer.Properties.Resources.MediaPreview16x16;
            this.kryptonRibbonQATButtonMediaPreview.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.kryptonRibbonQATButtonMediaPreview.ToolTipBody = "Play video files and view photos.\r\nPlay as slideshow\r\nView on Google Chromecast";
            this.kryptonRibbonQATButtonMediaPreview.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.MediaPreview;
            this.kryptonRibbonQATButtonMediaPreview.ToolTipTitle = "Show Media files (Ctrl+M)";
            this.kryptonRibbonQATButtonMediaPreview.Click += new System.EventHandler(this.kryptonRibbonQATButtonMediaPreview_Click);
            // 
            // kryptonRibbonQATButtonMediaPoster
            // 
            this.kryptonRibbonQATButtonMediaPoster.Image = global::PhotoTagsSynchronizer.Properties.Resources.MediaRegionSelector16x16;
            this.kryptonRibbonQATButtonMediaPoster.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.kryptonRibbonQATButtonMediaPoster.ToolTipBody = "Show media poster of the active media file. When edit Region/People/Faces you can" +
    " also change and add regions.";
            this.kryptonRibbonQATButtonMediaPoster.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.MediaRegionSelector;
            this.kryptonRibbonQATButtonMediaPoster.ToolTipTitle = "Show media poster and Region selector (Ctrl+R)";
            this.kryptonRibbonQATButtonMediaPoster.Click += new System.EventHandler(this.kryptonRibbonQATButtonMediaPoster_Click);
            // 
            // kryptonRibbonTabHome
            // 
            this.kryptonRibbonTabHome.Groups.AddRange(new Krypton.Ribbon.KryptonRibbonGroup[] {
            this.kryptonRibbonGroupHomeClipboard,
            this.kryptonRibbonGroupHomeManage,
            this.kryptonRibbonGroupHomeFileSystem,
            this.kryptonRibbonGroupHomeRotate,
            this.kryptonRibbonGroupHomeMetadata,
            this.kryptonRibbonGroupHomeSave});
            this.kryptonRibbonTabHome.KeyTip = "H";
            this.kryptonRibbonTabHome.Text = "Home";
            // 
            // kryptonRibbonGroupHomeClipboard
            // 
            this.kryptonRibbonGroupHomeClipboard.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupTriple10,
            this.kryptonRibbonGroupTriple11});
            this.kryptonRibbonGroupHomeClipboard.KeyTipDialogLauncher = "Q";
            this.kryptonRibbonGroupHomeClipboard.KeyTipGroup = "Q";
            this.kryptonRibbonGroupHomeClipboard.TextLine1 = "Clipboard";
            // 
            // kryptonRibbonGroupTriple10
            // 
            this.kryptonRibbonGroupTriple10.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButtonHomeCopy,
            this.kryptonRibbonGroupButtonHomeCut,
            this.kryptonRibbonGroupButtonHomePaste});
            this.kryptonRibbonGroupTriple10.MaximumSize = Krypton.Ribbon.GroupItemSize.Medium;
            // 
            // kryptonRibbonGroupButtonHomeCopy
            // 
            this.kryptonRibbonGroupButtonHomeCopy.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.EditCopy32x32;
            this.kryptonRibbonGroupButtonHomeCopy.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.EditCopy32x32;
            this.kryptonRibbonGroupButtonHomeCopy.KeyTip = "C";
            this.kryptonRibbonGroupButtonHomeCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.kryptonRibbonGroupButtonHomeCopy.TextLine1 = "Copy";
            this.kryptonRibbonGroupButtonHomeCopy.ToolTipBody = "Copy selected content to Clipboard";
            this.kryptonRibbonGroupButtonHomeCopy.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.EditCopy32x32;
            this.kryptonRibbonGroupButtonHomeCopy.ToolTipTitle = "Copy (Ctrl+C)";
            this.kryptonRibbonGroupButtonHomeCopy.Click += new System.EventHandler(this.kryptonRibbonGroupButtonHomeCopy_Click);
            // 
            // kryptonRibbonGroupButtonHomeCut
            // 
            this.kryptonRibbonGroupButtonHomeCut.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.EditCut32x32;
            this.kryptonRibbonGroupButtonHomeCut.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.EditCut32x32;
            this.kryptonRibbonGroupButtonHomeCut.KeyTip = "T";
            this.kryptonRibbonGroupButtonHomeCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.kryptonRibbonGroupButtonHomeCut.TextLine1 = "Cut";
            this.kryptonRibbonGroupButtonHomeCut.ToolTipBody = "Cut selected content to Clipboard";
            this.kryptonRibbonGroupButtonHomeCut.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.EditCut32x32;
            this.kryptonRibbonGroupButtonHomeCut.ToolTipTitle = "Cut (Ctrl+X)";
            this.kryptonRibbonGroupButtonHomeCut.Click += new System.EventHandler(this.kryptonRibbonGroupButtonHomeCut_Click);
            // 
            // kryptonRibbonGroupButtonHomePaste
            // 
            this.kryptonRibbonGroupButtonHomePaste.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.EditPaste32x32;
            this.kryptonRibbonGroupButtonHomePaste.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.EditPaste32x32;
            this.kryptonRibbonGroupButtonHomePaste.KeyTip = "P";
            this.kryptonRibbonGroupButtonHomePaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.kryptonRibbonGroupButtonHomePaste.TextLine1 = "Paste";
            this.kryptonRibbonGroupButtonHomePaste.ToolTipBody = "Paste content from Clipboard";
            this.kryptonRibbonGroupButtonHomePaste.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.EditPaste32x32;
            this.kryptonRibbonGroupButtonHomePaste.ToolTipTitle = "Paste (Ctrl+V)";
            this.kryptonRibbonGroupButtonHomePaste.Click += new System.EventHandler(this.kryptonRibbonGroupButtonHomePaste_Click);
            // 
            // kryptonRibbonGroupTriple11
            // 
            this.kryptonRibbonGroupTriple11.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButtonHomeUndo,
            this.kryptonRibbonGroupButtonRedo});
            this.kryptonRibbonGroupTriple11.MaximumSize = Krypton.Ribbon.GroupItemSize.Medium;
            // 
            // kryptonRibbonGroupButtonHomeUndo
            // 
            this.kryptonRibbonGroupButtonHomeUndo.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.EditUndo32x32;
            this.kryptonRibbonGroupButtonHomeUndo.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.EditUndo32x32;
            this.kryptonRibbonGroupButtonHomeUndo.KeyTip = "U";
            this.kryptonRibbonGroupButtonHomeUndo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.kryptonRibbonGroupButtonHomeUndo.TextLine1 = "Undo";
            this.kryptonRibbonGroupButtonHomeUndo.ToolTipBody = "Undo last command, or keep undo more commands.";
            this.kryptonRibbonGroupButtonHomeUndo.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.EditUndo32x32;
            this.kryptonRibbonGroupButtonHomeUndo.ToolTipTitle = "Undo (Ctrl+X)";
            this.kryptonRibbonGroupButtonHomeUndo.Click += new System.EventHandler(this.kryptonRibbonGroupButtonHomeUndo_Click);
            // 
            // kryptonRibbonGroupButtonRedo
            // 
            this.kryptonRibbonGroupButtonRedo.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.EditRedo32x32;
            this.kryptonRibbonGroupButtonRedo.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.EditRedo32x32;
            this.kryptonRibbonGroupButtonRedo.KeyTip = "R";
            this.kryptonRibbonGroupButtonRedo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.kryptonRibbonGroupButtonRedo.TextLine1 = "Redo";
            this.kryptonRibbonGroupButtonRedo.ToolTipBody = "Cancel last or many undo(s).";
            this.kryptonRibbonGroupButtonRedo.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.EditRedo32x32;
            this.kryptonRibbonGroupButtonRedo.ToolTipTitle = "Redo (Ctrl+Y)";
            this.kryptonRibbonGroupButtonRedo.Click += new System.EventHandler(this.kryptonRibbonGroupButtonHomeRedo_Click);
            // 
            // kryptonRibbonGroupHomeManage
            // 
            this.kryptonRibbonGroupHomeManage.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupTriple19,
            this.kryptonRibbonGroupTriple20});
            this.kryptonRibbonGroupHomeManage.KeyTipDialogLauncher = "Q";
            this.kryptonRibbonGroupHomeManage.KeyTipGroup = "Q";
            this.kryptonRibbonGroupHomeManage.TextLine1 = "Manage";
            // 
            // kryptonRibbonGroupTriple19
            // 
            this.kryptonRibbonGroupTriple19.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButtonHomeCopyText,
            this.kryptonRibbonGroupButtonHomeFastCopyNoOverwrite,
            this.kryptonRibbonGroupButtonHomeFastCopyOverwrite});
            this.kryptonRibbonGroupTriple19.MaximumSize = Krypton.Ribbon.GroupItemSize.Medium;
            // 
            // kryptonRibbonGroupButtonHomeCopyText
            // 
            this.kryptonRibbonGroupButtonHomeCopyText.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.EditCopyText32x32;
            this.kryptonRibbonGroupButtonHomeCopyText.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.EditCopyText32x32;
            this.kryptonRibbonGroupButtonHomeCopyText.KeyTip = "MCT";
            this.kryptonRibbonGroupButtonHomeCopyText.ShortcutKeys = ((System.Windows.Forms.Keys)((((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.C)));
            this.kryptonRibbonGroupButtonHomeCopyText.TextLine1 = "Copy";
            this.kryptonRibbonGroupButtonHomeCopyText.TextLine2 = "text";
            this.kryptonRibbonGroupButtonHomeCopyText.ToolTipBody = "Copy text from selected content. Example: filename or path, depending what is sel" +
    "ected.";
            this.kryptonRibbonGroupButtonHomeCopyText.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.EditCopyText32x32;
            this.kryptonRibbonGroupButtonHomeCopyText.ToolTipTitle = "Copy only text (Ctrl+Alt+Shift+C)";
            this.kryptonRibbonGroupButtonHomeCopyText.Click += new System.EventHandler(this.kryptonRibbonGroupButtonHomeCopyText_Click);
            // 
            // kryptonRibbonGroupButtonHomeFastCopyNoOverwrite
            // 
            this.kryptonRibbonGroupButtonHomeFastCopyNoOverwrite.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.EditFastTextCopyNotOverwrite32x32;
            this.kryptonRibbonGroupButtonHomeFastCopyNoOverwrite.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.EditFastTextCopyNotOverwrite32x32;
            this.kryptonRibbonGroupButtonHomeFastCopyNoOverwrite.KeyTip = "MCN";
            this.kryptonRibbonGroupButtonHomeFastCopyNoOverwrite.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.C)));
            this.kryptonRibbonGroupButtonHomeFastCopyNoOverwrite.TextLine1 = "Fast copy";
            this.kryptonRibbonGroupButtonHomeFastCopyNoOverwrite.TextLine2 = "No overwrite";
            this.kryptonRibbonGroupButtonHomeFastCopyNoOverwrite.ToolTipBody = resources.GetString("kryptonRibbonGroupButtonHomeFastCopyNoOverwrite.ToolTipBody");
            this.kryptonRibbonGroupButtonHomeFastCopyNoOverwrite.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.EditFastTextCopyNotOverwrite32x32;
            this.kryptonRibbonGroupButtonHomeFastCopyNoOverwrite.ToolTipTitle = "Fast Copy Text and no Overwrite (Ctrl+Shift+C)";
            this.kryptonRibbonGroupButtonHomeFastCopyNoOverwrite.Click += new System.EventHandler(this.kryptonRibbonGroupButtonHomeFastCopyNoOverwrite_Click);
            // 
            // kryptonRibbonGroupButtonHomeFastCopyOverwrite
            // 
            this.kryptonRibbonGroupButtonHomeFastCopyOverwrite.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.EditFastTextCopyAndReplace32x32;
            this.kryptonRibbonGroupButtonHomeFastCopyOverwrite.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.EditFastTextCopyAndReplace32x32;
            this.kryptonRibbonGroupButtonHomeFastCopyOverwrite.KeyTip = "MCO";
            this.kryptonRibbonGroupButtonHomeFastCopyOverwrite.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.C)));
            this.kryptonRibbonGroupButtonHomeFastCopyOverwrite.TextLine1 = "Fast Copy";
            this.kryptonRibbonGroupButtonHomeFastCopyOverwrite.TextLine2 = "Overwrite";
            this.kryptonRibbonGroupButtonHomeFastCopyOverwrite.ToolTipBody = resources.GetString("kryptonRibbonGroupButtonHomeFastCopyOverwrite.ToolTipBody");
            this.kryptonRibbonGroupButtonHomeFastCopyOverwrite.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.EditFastTextCopyAndReplace32x32;
            this.kryptonRibbonGroupButtonHomeFastCopyOverwrite.ToolTipTitle = "Fast Copy Text and Overwrite (Ctrl+Alt+C)";
            this.kryptonRibbonGroupButtonHomeFastCopyOverwrite.Click += new System.EventHandler(this.kryptonRibbonGroupButtonHomeFastCopyOverwrite_Click);
            // 
            // kryptonRibbonGroupTriple20
            // 
            this.kryptonRibbonGroupTriple20.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButtonHomeFind,
            this.kryptonRibbonGroupButtonHomeReplace});
            this.kryptonRibbonGroupTriple20.MaximumSize = Krypton.Ribbon.GroupItemSize.Medium;
            // 
            // kryptonRibbonGroupButtonHomeFind
            // 
            this.kryptonRibbonGroupButtonHomeFind.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.EditFind32x32;
            this.kryptonRibbonGroupButtonHomeFind.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.EditFind32x32;
            this.kryptonRibbonGroupButtonHomeFind.KeyTip = "MF";
            this.kryptonRibbonGroupButtonHomeFind.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.kryptonRibbonGroupButtonHomeFind.TextLine1 = "Find";
            this.kryptonRibbonGroupButtonHomeFind.ToolTipBody = "Find text";
            this.kryptonRibbonGroupButtonHomeFind.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.EditFind32x32;
            this.kryptonRibbonGroupButtonHomeFind.ToolTipTitle = "Find (Ctrl+F)";
            this.kryptonRibbonGroupButtonHomeFind.Click += new System.EventHandler(this.kryptonRibbonGroupButtonHomeFind_Click);
            // 
            // kryptonRibbonGroupButtonHomeReplace
            // 
            this.kryptonRibbonGroupButtonHomeReplace.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.EditReplace32x32;
            this.kryptonRibbonGroupButtonHomeReplace.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.EditReplace32x32;
            this.kryptonRibbonGroupButtonHomeReplace.KeyTip = "MR";
            this.kryptonRibbonGroupButtonHomeReplace.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.kryptonRibbonGroupButtonHomeReplace.TextLine1 = "Replace";
            this.kryptonRibbonGroupButtonHomeReplace.ToolTipBody = "Find text and replace wuth your text";
            this.kryptonRibbonGroupButtonHomeReplace.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.EditReplace32x32;
            this.kryptonRibbonGroupButtonHomeReplace.ToolTipTitle = "Replace (Ctrl+H)";
            this.kryptonRibbonGroupButtonHomeReplace.Click += new System.EventHandler(this.kryptonRibbonGroupButtonHomeReplace_Click);
            // 
            // kryptonRibbonGroupHomeFileSystem
            // 
            this.kryptonRibbonGroupHomeFileSystem.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupTriple27,
            this.kryptonRibbonGroupTriple28,
            this.kryptonRibbonGroupTriple17});
            this.kryptonRibbonGroupHomeFileSystem.KeyTipDialogLauncher = "Q";
            this.kryptonRibbonGroupHomeFileSystem.KeyTipGroup = "Q";
            this.kryptonRibbonGroupHomeFileSystem.TextLine1 = "Organise";
            // 
            // kryptonRibbonGroupTriple27
            // 
            this.kryptonRibbonGroupTriple27.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButtonHomeFileSystemDelete,
            this.kryptonRibbonGroupButtonHomeFileSystemRename,
            this.kryptonRibbonGroupButtonHomeFileSystemRefresh});
            this.kryptonRibbonGroupTriple27.MaximumSize = Krypton.Ribbon.GroupItemSize.Medium;
            // 
            // kryptonRibbonGroupButtonHomeFileSystemDelete
            // 
            this.kryptonRibbonGroupButtonHomeFileSystemDelete.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.FileSystemDelete32x32;
            this.kryptonRibbonGroupButtonHomeFileSystemDelete.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.FileSystemDelete32x32;
            this.kryptonRibbonGroupButtonHomeFileSystemDelete.KeyTip = "OD";
            this.kryptonRibbonGroupButtonHomeFileSystemDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.kryptonRibbonGroupButtonHomeFileSystemDelete.TextLine1 = "Delete";
            this.kryptonRibbonGroupButtonHomeFileSystemDelete.TextLine2 = "folder";
            this.kryptonRibbonGroupButtonHomeFileSystemDelete.ToolTipBody = "Delete selected content";
            this.kryptonRibbonGroupButtonHomeFileSystemDelete.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.FileSystemDelete32x32;
            this.kryptonRibbonGroupButtonHomeFileSystemDelete.ToolTipTitle = "Delete (Del)";
            this.kryptonRibbonGroupButtonHomeFileSystemDelete.Click += new System.EventHandler(this.kryptonRibbonGroupButtonHomeFileSystemDelete_Click);
            // 
            // kryptonRibbonGroupButtonHomeFileSystemRename
            // 
            this.kryptonRibbonGroupButtonHomeFileSystemRename.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.FileSystemRename;
            this.kryptonRibbonGroupButtonHomeFileSystemRename.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.FileSystemRename;
            this.kryptonRibbonGroupButtonHomeFileSystemRename.KeyTip = "ON";
            this.kryptonRibbonGroupButtonHomeFileSystemRename.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.kryptonRibbonGroupButtonHomeFileSystemRename.TextLine1 = "Rename";
            this.kryptonRibbonGroupButtonHomeFileSystemRename.TextLine2 = "folder name";
            this.kryptonRibbonGroupButtonHomeFileSystemRename.ToolTipBody = "Rename selected content";
            this.kryptonRibbonGroupButtonHomeFileSystemRename.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.FileSystemRename;
            this.kryptonRibbonGroupButtonHomeFileSystemRename.ToolTipTitle = "Rename (F2)";
            this.kryptonRibbonGroupButtonHomeFileSystemRename.Click += new System.EventHandler(this.kryptonRibbonGroupButtonHomeFileSystemRename_Click);
            // 
            // kryptonRibbonGroupButtonHomeFileSystemRefresh
            // 
            this.kryptonRibbonGroupButtonHomeFileSystemRefresh.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.FileSystemRefresh32x32;
            this.kryptonRibbonGroupButtonHomeFileSystemRefresh.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.FileSystemRefresh32x32;
            this.kryptonRibbonGroupButtonHomeFileSystemRefresh.KeyTip = "OF";
            this.kryptonRibbonGroupButtonHomeFileSystemRefresh.TextLine1 = "Refresh";
            this.kryptonRibbonGroupButtonHomeFileSystemRefresh.TextLine2 = "folder";
            this.kryptonRibbonGroupButtonHomeFileSystemRefresh.ToolTipBody = "Refresh the view with current information in the FileSystem";
            this.kryptonRibbonGroupButtonHomeFileSystemRefresh.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.FileSystemRefresh32x32;
            this.kryptonRibbonGroupButtonHomeFileSystemRefresh.ToolTipTitle = "Refresh (F5)";
            this.kryptonRibbonGroupButtonHomeFileSystemRefresh.Click += new System.EventHandler(this.kryptonRibbonGroupButtonHomeFileSystemRefreshFolder_Click);
            // 
            // kryptonRibbonGroupTriple28
            // 
            this.kryptonRibbonGroupTriple28.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButtonHomeFileSystemOpen,
            this.kryptonRibbonGroupButtonHomeFileSystemOpenWith,
            this.kryptonRibbonGroupButtonFileSystemOpenAssociateDialog});
            this.kryptonRibbonGroupTriple28.MaximumSize = Krypton.Ribbon.GroupItemSize.Medium;
            // 
            // kryptonRibbonGroupButtonHomeFileSystemOpen
            // 
            this.kryptonRibbonGroupButtonHomeFileSystemOpen.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.FileSystemOpen;
            this.kryptonRibbonGroupButtonHomeFileSystemOpen.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.FileSystemOpen;
            this.kryptonRibbonGroupButtonHomeFileSystemOpen.KeyTip = "OP";
            this.kryptonRibbonGroupButtonHomeFileSystemOpen.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.kryptonRibbonGroupButtonHomeFileSystemOpen.TextLine1 = "Open";
            this.kryptonRibbonGroupButtonHomeFileSystemOpen.ToolTipTitle = "Open (F3)";
            this.kryptonRibbonGroupButtonHomeFileSystemOpen.Click += new System.EventHandler(this.kryptonRibbonGroupButtonHomeFileSystemOpen_Click);
            // 
            // kryptonRibbonGroupButtonHomeFileSystemOpenWith
            // 
            this.kryptonRibbonGroupButtonHomeFileSystemOpenWith.ButtonType = Krypton.Ribbon.GroupButtonType.DropDown;
            this.kryptonRibbonGroupButtonHomeFileSystemOpenWith.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.FileSystemOpenWith;
            this.kryptonRibbonGroupButtonHomeFileSystemOpenWith.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.FileSystemOpenWith;
            this.kryptonRibbonGroupButtonHomeFileSystemOpenWith.KeyTip = "OW";
            this.kryptonRibbonGroupButtonHomeFileSystemOpenWith.TextLine1 = "Open with";
            this.kryptonRibbonGroupButtonHomeFileSystemOpenWith.ToolTipBody = "Open media file(s) with the selected application from the dropdown list";
            this.kryptonRibbonGroupButtonHomeFileSystemOpenWith.ToolTipTitle = "Open with";
            this.kryptonRibbonGroupButtonHomeFileSystemOpenWith.Click += new System.EventHandler(this.kryptonRibbonGroupButtonHomeFileSystemOpenWith_Click);
            // 
            // kryptonRibbonGroupButtonFileSystemOpenAssociateDialog
            // 
            this.kryptonRibbonGroupButtonFileSystemOpenAssociateDialog.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.FileSystemOpenWIthAssociationApp32x32;
            this.kryptonRibbonGroupButtonFileSystemOpenAssociateDialog.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.FileSystemOpenWIthAssociationApp32x32;
            this.kryptonRibbonGroupButtonFileSystemOpenAssociateDialog.KeyTip = "OA";
            this.kryptonRibbonGroupButtonFileSystemOpenAssociateDialog.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.kryptonRibbonGroupButtonFileSystemOpenAssociateDialog.TextLine1 = "Associate dialog";
            this.kryptonRibbonGroupButtonFileSystemOpenAssociateDialog.ToolTipBody = "Opens the Windows Associate File dialog windows";
            this.kryptonRibbonGroupButtonFileSystemOpenAssociateDialog.ToolTipTitle = "Associate dialog (Ctrl+O)";
            this.kryptonRibbonGroupButtonFileSystemOpenAssociateDialog.Click += new System.EventHandler(this.kryptonRibbonGroupButtonFileSystemOpenAssociateDialog_Click);
            // 
            // kryptonRibbonGroupTriple17
            // 
            this.kryptonRibbonGroupTriple17.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButtonHomeFileSystemOpenExplorer,
            this.kryptonRibbonGroupButtonFileSystemRunCommand,
            this.kryptonRibbonGroupButtonHomeFileSystemEdit});
            this.kryptonRibbonGroupTriple17.MaximumSize = Krypton.Ribbon.GroupItemSize.Medium;
            // 
            // kryptonRibbonGroupButtonHomeFileSystemOpenExplorer
            // 
            this.kryptonRibbonGroupButtonHomeFileSystemOpenExplorer.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.FileSystemExplorer32x32;
            this.kryptonRibbonGroupButtonHomeFileSystemOpenExplorer.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.FileSystemExplorer32x32;
            this.kryptonRibbonGroupButtonHomeFileSystemOpenExplorer.KeyTip = "OL";
            this.kryptonRibbonGroupButtonHomeFileSystemOpenExplorer.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.L)));
            this.kryptonRibbonGroupButtonHomeFileSystemOpenExplorer.TextLine1 = "Open ";
            this.kryptonRibbonGroupButtonHomeFileSystemOpenExplorer.TextLine2 = "location";
            this.kryptonRibbonGroupButtonHomeFileSystemOpenExplorer.ToolTipBody = "Open File Explorer in folder where media files is located";
            this.kryptonRibbonGroupButtonHomeFileSystemOpenExplorer.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.FileSystemExplorer32x32;
            this.kryptonRibbonGroupButtonHomeFileSystemOpenExplorer.ToolTipTitle = "Open File Explore (Alt+L)";
            this.kryptonRibbonGroupButtonHomeFileSystemOpenExplorer.Click += new System.EventHandler(this.kryptonRibbonGroupButtonHomeFileSystemOpenExplorerLocation_Click);
            // 
            // kryptonRibbonGroupButtonFileSystemRunCommand
            // 
            this.kryptonRibbonGroupButtonFileSystemRunCommand.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.ToolsRunCommand32x32;
            this.kryptonRibbonGroupButtonFileSystemRunCommand.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.ToolsRunCommand32x32;
            this.kryptonRibbonGroupButtonFileSystemRunCommand.KeyTip = "OR";
            this.kryptonRibbonGroupButtonFileSystemRunCommand.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.R)));
            this.kryptonRibbonGroupButtonFileSystemRunCommand.TextLine1 = "Run";
            this.kryptonRibbonGroupButtonFileSystemRunCommand.TextLine2 = "Command";
            this.kryptonRibbonGroupButtonFileSystemRunCommand.ToolTipBody = "Open a Run Command Window, where commands can be run with lot of parameters";
            this.kryptonRibbonGroupButtonFileSystemRunCommand.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.ToolsRunCommand32x32;
            this.kryptonRibbonGroupButtonFileSystemRunCommand.ToolTipTitle = "Run command (Alt+R)";
            this.kryptonRibbonGroupButtonFileSystemRunCommand.Click += new System.EventHandler(this.kryptonRibbonGroupButtonFileSystemRunCommand_Click);
            // 
            // kryptonRibbonGroupButtonHomeFileSystemEdit
            // 
            this.kryptonRibbonGroupButtonHomeFileSystemEdit.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.FileSystemEdit;
            this.kryptonRibbonGroupButtonHomeFileSystemEdit.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.FileSystemEdit;
            this.kryptonRibbonGroupButtonHomeFileSystemEdit.KeyTip = "OE";
            this.kryptonRibbonGroupButtonHomeFileSystemEdit.ShortcutKeys = System.Windows.Forms.Keys.F4;
            this.kryptonRibbonGroupButtonHomeFileSystemEdit.TextLine1 = "Edit";
            this.kryptonRibbonGroupButtonHomeFileSystemEdit.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.FileSystemEdit;
            this.kryptonRibbonGroupButtonHomeFileSystemEdit.Click += new System.EventHandler(this.kryptonRibbonGroupButtonHomeFileSystemVerbEdit_Click);
            // 
            // kryptonRibbonGroupHomeRotate
            // 
            this.kryptonRibbonGroupHomeRotate.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupTriple9});
            this.kryptonRibbonGroupHomeRotate.KeyTipDialogLauncher = "Q";
            this.kryptonRibbonGroupHomeRotate.KeyTipGroup = "Q";
            this.kryptonRibbonGroupHomeRotate.TextLine1 = "Rotate";
            // 
            // kryptonRibbonGroupTriple9
            // 
            this.kryptonRibbonGroupTriple9.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButtonMediaFileRotate90CCW,
            this.kryptonRibbonGroupButtonMediaFileRotate180,
            this.kryptonRibbonGroupButtonMediaFileRotate90CW});
            this.kryptonRibbonGroupTriple9.MaximumSize = Krypton.Ribbon.GroupItemSize.Medium;
            // 
            // kryptonRibbonGroupButtonMediaFileRotate90CCW
            // 
            this.kryptonRibbonGroupButtonMediaFileRotate90CCW.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.MediaRotate90CCW;
            this.kryptonRibbonGroupButtonMediaFileRotate90CCW.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.MediaRotate90CCW;
            this.kryptonRibbonGroupButtonMediaFileRotate90CCW.KeyTip = "2";
            this.kryptonRibbonGroupButtonMediaFileRotate90CCW.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D2)));
            this.kryptonRibbonGroupButtonMediaFileRotate90CCW.TextLine1 = "90 CCW";
            this.kryptonRibbonGroupButtonMediaFileRotate90CCW.ToolTipBody = "Load media, rotate 270° (90° CCW) and write back to media file.";
            this.kryptonRibbonGroupButtonMediaFileRotate90CCW.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.MediaRotate90CCW;
            this.kryptonRibbonGroupButtonMediaFileRotate90CCW.ToolTipTitle = "Rotate media 270° (90° CCW) (Alt+2)";
            this.kryptonRibbonGroupButtonMediaFileRotate90CCW.Click += new System.EventHandler(this.kryptonRibbonGroupButtonMediaFileRotate90CCW_Click);
            // 
            // kryptonRibbonGroupButtonMediaFileRotate180
            // 
            this.kryptonRibbonGroupButtonMediaFileRotate180.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.MediaRotate180;
            this.kryptonRibbonGroupButtonMediaFileRotate180.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.MediaRotate180;
            this.kryptonRibbonGroupButtonMediaFileRotate180.KeyTip = "1";
            this.kryptonRibbonGroupButtonMediaFileRotate180.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D1)));
            this.kryptonRibbonGroupButtonMediaFileRotate180.TextLine1 = "180";
            this.kryptonRibbonGroupButtonMediaFileRotate180.ToolTipBody = "Load media, rotate 180° (180° CW) and write back to media file.";
            this.kryptonRibbonGroupButtonMediaFileRotate180.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.MediaRotate180;
            this.kryptonRibbonGroupButtonMediaFileRotate180.ToolTipTitle = "Rotate media 180° (180° CW) (Alt+1)";
            this.kryptonRibbonGroupButtonMediaFileRotate180.Click += new System.EventHandler(this.kryptonRibbonGroupButtonMediaFileRotate180_Click);
            // 
            // kryptonRibbonGroupButtonMediaFileRotate90CW
            // 
            this.kryptonRibbonGroupButtonMediaFileRotate90CW.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.MediaRotate90CW;
            this.kryptonRibbonGroupButtonMediaFileRotate90CW.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.MediaRotate90CW;
            this.kryptonRibbonGroupButtonMediaFileRotate90CW.KeyTip = "9";
            this.kryptonRibbonGroupButtonMediaFileRotate90CW.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D9)));
            this.kryptonRibbonGroupButtonMediaFileRotate90CW.TextLine1 = "90 CW";
            this.kryptonRibbonGroupButtonMediaFileRotate90CW.ToolTipBody = "Load media, rotate 90° (90° CW) and write back to media file.";
            this.kryptonRibbonGroupButtonMediaFileRotate90CW.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.MediaRotate90CW;
            this.kryptonRibbonGroupButtonMediaFileRotate90CW.ToolTipTitle = "Rotate media 90° (90° CW) (Alt+9)";
            this.kryptonRibbonGroupButtonMediaFileRotate90CW.Click += new System.EventHandler(this.kryptonRibbonGroupButtonMediaFileRotate90CW_Click);
            // 
            // kryptonRibbonGroupHomeMetadata
            // 
            this.kryptonRibbonGroupHomeMetadata.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupTriple16,
            this.kryptonRibbonGroupTriple15,
            this.kryptonRibbonGroupTriple21});
            this.kryptonRibbonGroupHomeMetadata.KeyTipDialogLauncher = "Q";
            this.kryptonRibbonGroupHomeMetadata.KeyTipGroup = "Q";
            this.kryptonRibbonGroupHomeMetadata.TextLine1 = "Metadata";
            // 
            // kryptonRibbonGroupTriple16
            // 
            this.kryptonRibbonGroupTriple16.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButtonHomeAutoCorrectRun,
            this.kryptonRibbonGroupButtonHomeAutoCorrectForm});
            // 
            // kryptonRibbonGroupButtonHomeAutoCorrectRun
            // 
            this.kryptonRibbonGroupButtonHomeAutoCorrectRun.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.MetadataAutoCorrectRun32x32;
            this.kryptonRibbonGroupButtonHomeAutoCorrectRun.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.MetadataAutoCorrectRun32x32;
            this.kryptonRibbonGroupButtonHomeAutoCorrectRun.KeyTip = "AR";
            this.kryptonRibbonGroupButtonHomeAutoCorrectRun.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.T)));
            this.kryptonRibbonGroupButtonHomeAutoCorrectRun.TextLine1 = "AutoCorrect";
            this.kryptonRibbonGroupButtonHomeAutoCorrectRun.TextLine2 = "Run";
            this.kryptonRibbonGroupButtonHomeAutoCorrectRun.ToolTipBody = "AutoCorrect use diffrent rules set in Config to fix metadata contence in the medi" +
    "a file.";
            this.kryptonRibbonGroupButtonHomeAutoCorrectRun.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.MetadataAutoCorrectRun32x32;
            this.kryptonRibbonGroupButtonHomeAutoCorrectRun.ToolTipTitle = "AutoCorrect Metadata (Alt+Shift+L)";
            this.kryptonRibbonGroupButtonHomeAutoCorrectRun.Click += new System.EventHandler(this.kryptonRibbonGroupButtonHomeAutoCorrectRun_Click);
            // 
            // kryptonRibbonGroupButtonHomeAutoCorrectForm
            // 
            this.kryptonRibbonGroupButtonHomeAutoCorrectForm.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.MetadataAutoCorrectForm32x32;
            this.kryptonRibbonGroupButtonHomeAutoCorrectForm.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.MetadataAutoCorrectForm32x32;
            this.kryptonRibbonGroupButtonHomeAutoCorrectForm.KeyTip = "AF";
            this.kryptonRibbonGroupButtonHomeAutoCorrectForm.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.T)));
            this.kryptonRibbonGroupButtonHomeAutoCorrectForm.TextLine1 = "AutoCorrect";
            this.kryptonRibbonGroupButtonHomeAutoCorrectForm.TextLine2 = "Form";
            this.kryptonRibbonGroupButtonHomeAutoCorrectForm.ToolTipBody = "AutoCorrect use diffrent rules set in Config to fix metadata contence in the medi" +
    "a file.";
            this.kryptonRibbonGroupButtonHomeAutoCorrectForm.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.MetadataAutoCorrectForm32x32;
            this.kryptonRibbonGroupButtonHomeAutoCorrectForm.ToolTipTitle = "AutoCorrect Metadata Form (Alt+Shift+L)";
            this.kryptonRibbonGroupButtonHomeAutoCorrectForm.Click += new System.EventHandler(this.kryptonRibbonGroupButtonHomeAutoCorrectForm_Click);
            // 
            // kryptonRibbonGroupTriple15
            // 
            this.kryptonRibbonGroupTriple15.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButtonHomeMetadataRefresh,
            this.kryptonRibbonGroupButtonHomeMetadataReload});
            this.kryptonRibbonGroupTriple15.MaximumSize = Krypton.Ribbon.GroupItemSize.Medium;
            // 
            // kryptonRibbonGroupButtonHomeMetadataRefresh
            // 
            this.kryptonRibbonGroupButtonHomeMetadataRefresh.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.MetadataReload;
            this.kryptonRibbonGroupButtonHomeMetadataRefresh.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.MetadataReload;
            this.kryptonRibbonGroupButtonHomeMetadataRefresh.KeyTip = "AF";
            this.kryptonRibbonGroupButtonHomeMetadataRefresh.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F5)));
            this.kryptonRibbonGroupButtonHomeMetadataRefresh.TextLine1 = "Refresh";
            this.kryptonRibbonGroupButtonHomeMetadataRefresh.ToolTipBody = "Delete last metadata information and last thumbnail and refresh the information";
            this.kryptonRibbonGroupButtonHomeMetadataRefresh.ToolTipTitle = "Refresh (Shift+F5)";
            this.kryptonRibbonGroupButtonHomeMetadataRefresh.Click += new System.EventHandler(this.kryptonRibbonGroupButtonHomeMetadataRefreshLast_Click);
            // 
            // kryptonRibbonGroupButtonHomeMetadataReload
            // 
            this.kryptonRibbonGroupButtonHomeMetadataReload.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.MetadataDeleteHistory;
            this.kryptonRibbonGroupButtonHomeMetadataReload.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.MetadataDeleteHistory;
            this.kryptonRibbonGroupButtonHomeMetadataReload.KeyTip = "AL";
            this.kryptonRibbonGroupButtonHomeMetadataReload.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F5)));
            this.kryptonRibbonGroupButtonHomeMetadataReload.TextLine1 = "Reload";
            this.kryptonRibbonGroupButtonHomeMetadataReload.ToolTipBody = "Delete all metadata history and thumbnail and reload latest information";
            this.kryptonRibbonGroupButtonHomeMetadataReload.ToolTipTitle = "Delete History and Reload(Ctrl+F5)";
            this.kryptonRibbonGroupButtonHomeMetadataReload.Click += new System.EventHandler(this.kryptonRibbonGroupButtonHomeMetadataReloadDeleteHistory_Click);
            // 
            // kryptonRibbonGroupTriple21
            // 
            this.kryptonRibbonGroupTriple21.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButtonHomeTagSelectOn,
            this.kryptonRibbonGroupButtonHomeTagSelectToggle,
            this.kryptonRibbonGroupButtonHomeTagSelectOff});
            this.kryptonRibbonGroupTriple21.MaximumSize = Krypton.Ribbon.GroupItemSize.Medium;
            // 
            // kryptonRibbonGroupButtonHomeTagSelectOn
            // 
            this.kryptonRibbonGroupButtonHomeTagSelectOn.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.EditTriStateSelect32x32;
            this.kryptonRibbonGroupButtonHomeTagSelectOn.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.EditTriStateSelect32x32;
            this.kryptonRibbonGroupButtonHomeTagSelectOn.KeyTip = "MSS";
            this.kryptonRibbonGroupButtonHomeTagSelectOn.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.Space)));
            this.kryptonRibbonGroupButtonHomeTagSelectOn.TextLine1 = "Select";
            this.kryptonRibbonGroupButtonHomeTagSelectOn.ToolTipBody = "Turn on all for all selected selected cells";
            this.kryptonRibbonGroupButtonHomeTagSelectOn.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.EditTriStateSelect32x32;
            this.kryptonRibbonGroupButtonHomeTagSelectOn.ToolTipTitle = "Turn on (Ctrl+Shift+Space)";
            this.kryptonRibbonGroupButtonHomeTagSelectOn.Click += new System.EventHandler(this.kryptonRibbonGroupButtonHomeTriStateOn_Click);
            // 
            // kryptonRibbonGroupButtonHomeTagSelectToggle
            // 
            this.kryptonRibbonGroupButtonHomeTagSelectToggle.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.EditTriStateToggle32x32;
            this.kryptonRibbonGroupButtonHomeTagSelectToggle.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.EditTriStateToggle32x32;
            this.kryptonRibbonGroupButtonHomeTagSelectToggle.KeyTip = "MST";
            this.kryptonRibbonGroupButtonHomeTagSelectToggle.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Space)));
            this.kryptonRibbonGroupButtonHomeTagSelectToggle.TextLine1 = "Toggle";
            this.kryptonRibbonGroupButtonHomeTagSelectToggle.ToolTipBody = "Toggle on/off state for all selected selected cells";
            this.kryptonRibbonGroupButtonHomeTagSelectToggle.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.EditTriStateToggle32x32;
            this.kryptonRibbonGroupButtonHomeTagSelectToggle.ToolTipTitle = "Toggle (Ctrl+Space)";
            this.kryptonRibbonGroupButtonHomeTagSelectToggle.Click += new System.EventHandler(this.kryptonRibbonGroupButtonHomeTriStateToggle_Click);
            // 
            // kryptonRibbonGroupButtonHomeTagSelectOff
            // 
            this.kryptonRibbonGroupButtonHomeTagSelectOff.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.EditTriStateDelete32x32;
            this.kryptonRibbonGroupButtonHomeTagSelectOff.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.EditTriStateDelete32x32;
            this.kryptonRibbonGroupButtonHomeTagSelectOff.KeyTip = "MSR";
            this.kryptonRibbonGroupButtonHomeTagSelectOff.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete)));
            this.kryptonRibbonGroupButtonHomeTagSelectOff.TextLine1 = "Remove";
            this.kryptonRibbonGroupButtonHomeTagSelectOff.ToolTipBody = "Turn off all for all selected selected cells";
            this.kryptonRibbonGroupButtonHomeTagSelectOff.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.EditTriStateDelete32x32;
            this.kryptonRibbonGroupButtonHomeTagSelectOff.ToolTipTitle = "Turn off (Ctrl+Del)";
            this.kryptonRibbonGroupButtonHomeTagSelectOff.Click += new System.EventHandler(this.kryptonRibbonGroupButtonHomeTriStateOff_Click);
            // 
            // kryptonRibbonGroupHomeSave
            // 
            this.kryptonRibbonGroupHomeSave.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupTriple29});
            this.kryptonRibbonGroupHomeSave.TextLine1 = "Save";
            // 
            // kryptonRibbonGroupTriple29
            // 
            this.kryptonRibbonGroupTriple29.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButtonHomeSaveSave});
            // 
            // kryptonRibbonGroupButtonHomeSaveSave
            // 
            this.kryptonRibbonGroupButtonHomeSaveSave.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.MetadataSave32x32;
            this.kryptonRibbonGroupButtonHomeSaveSave.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.MetadataSave32x32;
            this.kryptonRibbonGroupButtonHomeSaveSave.TextLine1 = "Save";
            this.kryptonRibbonGroupButtonHomeSaveSave.ToolTipBody = "Save you changes";
            this.kryptonRibbonGroupButtonHomeSaveSave.ToolTipTitle = "Save";
            this.kryptonRibbonGroupButtonHomeSaveSave.Click += new System.EventHandler(this.kryptonRibbonGroupButtonHomeSaveSave_Click);
            // 
            // kryptonRibbonTabView
            // 
            this.kryptonRibbonTabView.Groups.AddRange(new Krypton.Ribbon.KryptonRibbonGroup[] {
            this.kryptonRibbonGroupViewViewModes,
            this.kryptonRibbonGroupViewThumbnailSize,
            this.kryptonRibbonGroupViewCellSize,
            this.kryptonRibbonGroupViewColumns,
            this.kryptonRibbonGroupViewRows,
            this.kryptonRibbonGroup14});
            this.kryptonRibbonTabView.KeyTip = "V";
            this.kryptonRibbonTabView.Text = "View";
            // 
            // kryptonRibbonGroupViewViewModes
            // 
            this.kryptonRibbonGroupViewViewModes.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupTriple2,
            this.kryptonRibbonGroupTriple1,
            this.kryptonRibbonGroupSeparator4,
            this.kryptonRibbonGroupTriple5});
            this.kryptonRibbonGroupViewViewModes.TextLine1 = "Media view";
            // 
            // kryptonRibbonGroupTriple2
            // 
            this.kryptonRibbonGroupTriple2.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButtonImageListViewModeGallery,
            this.kryptonRibbonGroupButtonImageListViewModeDetails,
            this.kryptonRibbonGroupButtonImageListViewModePane});
            // 
            // kryptonRibbonGroupButtonImageListViewModeGallery
            // 
            this.kryptonRibbonGroupButtonImageListViewModeGallery.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.ImageListViewModeGallery32x32;
            this.kryptonRibbonGroupButtonImageListViewModeGallery.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.ImageListViewModeGallery32x32;
            this.kryptonRibbonGroupButtonImageListViewModeGallery.KeyTip = "G";
            this.kryptonRibbonGroupButtonImageListViewModeGallery.TextLine1 = "Gallery";
            this.kryptonRibbonGroupButtonImageListViewModeGallery.Click += new System.EventHandler(this.kryptonRibbonGroupButtonImageListViewModeGallery_Click);
            // 
            // kryptonRibbonGroupButtonImageListViewModeDetails
            // 
            this.kryptonRibbonGroupButtonImageListViewModeDetails.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.ImageListViewModeDetails32x32;
            this.kryptonRibbonGroupButtonImageListViewModeDetails.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.ImageListViewModeDetails32x32;
            this.kryptonRibbonGroupButtonImageListViewModeDetails.KeyTip = "D";
            this.kryptonRibbonGroupButtonImageListViewModeDetails.TextLine1 = "Details";
            this.kryptonRibbonGroupButtonImageListViewModeDetails.Click += new System.EventHandler(this.kryptonRibbonGroupButtonImageListViewModeDetails_Click);
            // 
            // kryptonRibbonGroupButtonImageListViewModePane
            // 
            this.kryptonRibbonGroupButtonImageListViewModePane.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.ImageListViewModePane32x32;
            this.kryptonRibbonGroupButtonImageListViewModePane.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.ImageListViewModePane32x32;
            this.kryptonRibbonGroupButtonImageListViewModePane.KeyTip = "P";
            this.kryptonRibbonGroupButtonImageListViewModePane.TextLine1 = "Pane";
            this.kryptonRibbonGroupButtonImageListViewModePane.Click += new System.EventHandler(this.kryptonRibbonGroupButtonImageListViewModePane_Click);
            // 
            // kryptonRibbonGroupTriple1
            // 
            this.kryptonRibbonGroupTriple1.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButtonImageListViewModeThumbnails,
            this.kryptonRibbonGroupButtonImageListViewModeThumbnailRenders});
            // 
            // kryptonRibbonGroupButtonImageListViewModeThumbnails
            // 
            this.kryptonRibbonGroupButtonImageListViewModeThumbnails.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.ImageListViewModeThumbnail32x32;
            this.kryptonRibbonGroupButtonImageListViewModeThumbnails.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.ImageListViewModeThumbnail32x32;
            this.kryptonRibbonGroupButtonImageListViewModeThumbnails.KeyTip = "T";
            this.kryptonRibbonGroupButtonImageListViewModeThumbnails.TextLine1 = "Thumbnails";
            this.kryptonRibbonGroupButtonImageListViewModeThumbnails.Click += new System.EventHandler(this.kryptonRibbonGroupButtonImageListViewModeThumbnails_Click);
            // 
            // kryptonRibbonGroupButtonImageListViewModeThumbnailRenders
            // 
            this.kryptonRibbonGroupButtonImageListViewModeThumbnailRenders.ButtonType = Krypton.Ribbon.GroupButtonType.DropDown;
            this.kryptonRibbonGroupButtonImageListViewModeThumbnailRenders.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.ImageListViewViewModesThumbnail32x32;
            this.kryptonRibbonGroupButtonImageListViewModeThumbnailRenders.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.ImageListViewViewModesThumbnail32x32;
            this.kryptonRibbonGroupButtonImageListViewModeThumbnailRenders.KeyTip = "V";
            this.kryptonRibbonGroupButtonImageListViewModeThumbnailRenders.TextLine1 = "Thumbnails";
            this.kryptonRibbonGroupButtonImageListViewModeThumbnailRenders.TextLine2 = "View mode";
            // 
            // kryptonRibbonGroupTriple5
            // 
            this.kryptonRibbonGroupTriple5.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButtonImageListViewDetailviewColumns});
            // 
            // kryptonRibbonGroupButtonImageListViewDetailviewColumns
            // 
            this.kryptonRibbonGroupButtonImageListViewDetailviewColumns.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.ImageListViewModeColumns32x32;
            this.kryptonRibbonGroupButtonImageListViewDetailviewColumns.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.ImageListViewModeColumns32x32;
            this.kryptonRibbonGroupButtonImageListViewDetailviewColumns.KeyTip = "C";
            this.kryptonRibbonGroupButtonImageListViewDetailviewColumns.TextLine1 = "View";
            this.kryptonRibbonGroupButtonImageListViewDetailviewColumns.TextLine2 = "Columns";
            this.kryptonRibbonGroupButtonImageListViewDetailviewColumns.Click += new System.EventHandler(this.kryptonRibbonGroupButtonImageListViewDetailviewColumns_Click);
            // 
            // kryptonRibbonGroupViewThumbnailSize
            // 
            this.kryptonRibbonGroupViewThumbnailSize.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupTriple12,
            this.kryptonRibbonGroupTriple13});
            this.kryptonRibbonGroupViewThumbnailSize.TextLine1 = "Thumbnail size";
            // 
            // kryptonRibbonGroupTriple12
            // 
            this.kryptonRibbonGroupTriple12.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButtonThumbnailSizeXLarge,
            this.kryptonRibbonGroupButtonThumbnailSizeLarge,
            this.kryptonRibbonGroupButtonThumbnailSizeMedium});
            this.kryptonRibbonGroupTriple12.MaximumSize = Krypton.Ribbon.GroupItemSize.Medium;
            // 
            // kryptonRibbonGroupButtonThumbnailSizeXLarge
            // 
            this.kryptonRibbonGroupButtonThumbnailSizeXLarge.ButtonType = Krypton.Ribbon.GroupButtonType.Check;
            this.kryptonRibbonGroupButtonThumbnailSizeXLarge.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.ThumbnailSizeXLarge32x32;
            this.kryptonRibbonGroupButtonThumbnailSizeXLarge.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.ThumbnailSizeXLarge32x32;
            this.kryptonRibbonGroupButtonThumbnailSizeXLarge.KeyTip = "5";
            this.kryptonRibbonGroupButtonThumbnailSizeXLarge.TextLine1 = "XLarge";
            this.kryptonRibbonGroupButtonThumbnailSizeXLarge.Click += new System.EventHandler(this.kryptonRibbonGroupButtonThumbnailSizeXLarge_Click);
            // 
            // kryptonRibbonGroupButtonThumbnailSizeLarge
            // 
            this.kryptonRibbonGroupButtonThumbnailSizeLarge.ButtonType = Krypton.Ribbon.GroupButtonType.Check;
            this.kryptonRibbonGroupButtonThumbnailSizeLarge.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.ThumbnailSizeLarge32x32;
            this.kryptonRibbonGroupButtonThumbnailSizeLarge.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.ThumbnailSizeLarge32x32;
            this.kryptonRibbonGroupButtonThumbnailSizeLarge.KeyTip = "4";
            this.kryptonRibbonGroupButtonThumbnailSizeLarge.TextLine1 = "Large";
            this.kryptonRibbonGroupButtonThumbnailSizeLarge.Click += new System.EventHandler(this.kryptonRibbonGroupButtonThumbnailSizeLarge_Click);
            // 
            // kryptonRibbonGroupButtonThumbnailSizeMedium
            // 
            this.kryptonRibbonGroupButtonThumbnailSizeMedium.ButtonType = Krypton.Ribbon.GroupButtonType.Check;
            this.kryptonRibbonGroupButtonThumbnailSizeMedium.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.ThumbnailSizeMedium32x32;
            this.kryptonRibbonGroupButtonThumbnailSizeMedium.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.ThumbnailSizeMedium32x32;
            this.kryptonRibbonGroupButtonThumbnailSizeMedium.KeyTip = "3";
            this.kryptonRibbonGroupButtonThumbnailSizeMedium.TextLine1 = "Medium";
            this.kryptonRibbonGroupButtonThumbnailSizeMedium.Click += new System.EventHandler(this.kryptonRibbonGroupButtonThumbnailSizeMedium_Click);
            // 
            // kryptonRibbonGroupTriple13
            // 
            this.kryptonRibbonGroupTriple13.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButtonThumbnailSizeSmall,
            this.kryptonRibbonGroupButtonThumbnailSizeXSmall});
            this.kryptonRibbonGroupTriple13.MaximumSize = Krypton.Ribbon.GroupItemSize.Medium;
            // 
            // kryptonRibbonGroupButtonThumbnailSizeSmall
            // 
            this.kryptonRibbonGroupButtonThumbnailSizeSmall.ButtonType = Krypton.Ribbon.GroupButtonType.Check;
            this.kryptonRibbonGroupButtonThumbnailSizeSmall.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.ThumbnailSizeSmall32x32;
            this.kryptonRibbonGroupButtonThumbnailSizeSmall.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.ThumbnailSizeSmall32x32;
            this.kryptonRibbonGroupButtonThumbnailSizeSmall.KeyTip = "2";
            this.kryptonRibbonGroupButtonThumbnailSizeSmall.TextLine1 = "Small";
            this.kryptonRibbonGroupButtonThumbnailSizeSmall.Click += new System.EventHandler(this.kryptonRibbonGroupButtonThumbnailSizeSmall_Click);
            // 
            // kryptonRibbonGroupButtonThumbnailSizeXSmall
            // 
            this.kryptonRibbonGroupButtonThumbnailSizeXSmall.ButtonType = Krypton.Ribbon.GroupButtonType.Check;
            this.kryptonRibbonGroupButtonThumbnailSizeXSmall.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.ThumbnailSizeXSmall32x32;
            this.kryptonRibbonGroupButtonThumbnailSizeXSmall.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.ThumbnailSizeXSmall32x32;
            this.kryptonRibbonGroupButtonThumbnailSizeXSmall.KeyTip = "1";
            this.kryptonRibbonGroupButtonThumbnailSizeXSmall.TextLine1 = "XSmall";
            this.kryptonRibbonGroupButtonThumbnailSizeXSmall.Click += new System.EventHandler(this.kryptonRibbonGroupButtonThumbnailSizeXSmall_Click);
            // 
            // kryptonRibbonGroupViewCellSize
            // 
            this.kryptonRibbonGroupViewCellSize.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupTriple3});
            this.kryptonRibbonGroupViewCellSize.TextLine1 = "Cell size";
            // 
            // kryptonRibbonGroupTriple3
            // 
            this.kryptonRibbonGroupTriple3.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButtonDataGridViewCellSizeBig,
            this.kryptonRibbonGroupButtonDataGridViewCellSizeMedium,
            this.kryptonRibbonGroupButtonDataGridViewCellSizeSmall});
            // 
            // kryptonRibbonGroupButtonDataGridViewCellSizeBig
            // 
            this.kryptonRibbonGroupButtonDataGridViewCellSizeBig.ButtonType = Krypton.Ribbon.GroupButtonType.Check;
            this.kryptonRibbonGroupButtonDataGridViewCellSizeBig.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.DataGridViewBig32x32;
            this.kryptonRibbonGroupButtonDataGridViewCellSizeBig.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.DataGridViewBig32x32;
            this.kryptonRibbonGroupButtonDataGridViewCellSizeBig.TextLine1 = "Big";
            this.kryptonRibbonGroupButtonDataGridViewCellSizeBig.Click += new System.EventHandler(this.kryptonRibbonGroupButtonDataGridViewCellSizeBig_Click);
            // 
            // kryptonRibbonGroupButtonDataGridViewCellSizeMedium
            // 
            this.kryptonRibbonGroupButtonDataGridViewCellSizeMedium.ButtonType = Krypton.Ribbon.GroupButtonType.Check;
            this.kryptonRibbonGroupButtonDataGridViewCellSizeMedium.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.DataGridViewNormal32x32;
            this.kryptonRibbonGroupButtonDataGridViewCellSizeMedium.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.DataGridViewNormal32x32;
            this.kryptonRibbonGroupButtonDataGridViewCellSizeMedium.KeyTip = "M";
            this.kryptonRibbonGroupButtonDataGridViewCellSizeMedium.TextLine1 = "Medim";
            this.kryptonRibbonGroupButtonDataGridViewCellSizeMedium.Click += new System.EventHandler(this.kryptonRibbonGroupButtonDataGridViewCellSizeMedium_Click);
            // 
            // kryptonRibbonGroupButtonDataGridViewCellSizeSmall
            // 
            this.kryptonRibbonGroupButtonDataGridViewCellSizeSmall.ButtonType = Krypton.Ribbon.GroupButtonType.Check;
            this.kryptonRibbonGroupButtonDataGridViewCellSizeSmall.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.DataGridViewSmall32x32;
            this.kryptonRibbonGroupButtonDataGridViewCellSizeSmall.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.DataGridViewSmall32x32;
            this.kryptonRibbonGroupButtonDataGridViewCellSizeSmall.KeyTip = "S";
            this.kryptonRibbonGroupButtonDataGridViewCellSizeSmall.TextLine1 = "Small";
            this.kryptonRibbonGroupButtonDataGridViewCellSizeSmall.Click += new System.EventHandler(this.kryptonRibbonGroupButtonDataGridViewCellSizeSmall_Click);
            // 
            // kryptonRibbonGroupViewColumns
            // 
            this.kryptonRibbonGroupViewColumns.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupTriple4});
            this.kryptonRibbonGroupViewColumns.TextLine1 = "Columns";
            // 
            // kryptonRibbonGroupTriple4
            // 
            this.kryptonRibbonGroupTriple4.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButtonDataGridViewColumnsHistory,
            this.kryptonRibbonGroupButtonDataGridViewColumnsErrors});
            // 
            // kryptonRibbonGroupButtonDataGridViewColumnsHistory
            // 
            this.kryptonRibbonGroupButtonDataGridViewColumnsHistory.ButtonType = Krypton.Ribbon.GroupButtonType.Check;
            this.kryptonRibbonGroupButtonDataGridViewColumnsHistory.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.DataGridViewHistoryColumn32x32;
            this.kryptonRibbonGroupButtonDataGridViewColumnsHistory.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.DataGridViewHistoryColumn32x32;
            this.kryptonRibbonGroupButtonDataGridViewColumnsHistory.KeyTip = "H";
            this.kryptonRibbonGroupButtonDataGridViewColumnsHistory.TextLine1 = "History";
            this.kryptonRibbonGroupButtonDataGridViewColumnsHistory.Click += new System.EventHandler(this.kryptonRibbonGroupButtonDataGridViewColumnsHistory_Click);
            // 
            // kryptonRibbonGroupButtonDataGridViewColumnsErrors
            // 
            this.kryptonRibbonGroupButtonDataGridViewColumnsErrors.ButtonType = Krypton.Ribbon.GroupButtonType.Check;
            this.kryptonRibbonGroupButtonDataGridViewColumnsErrors.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.DataGridViewErrorColumn32x32;
            this.kryptonRibbonGroupButtonDataGridViewColumnsErrors.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.DataGridViewErrorColumn32x32;
            this.kryptonRibbonGroupButtonDataGridViewColumnsErrors.KeyTip = "E";
            this.kryptonRibbonGroupButtonDataGridViewColumnsErrors.TextLine1 = "Errors";
            this.kryptonRibbonGroupButtonDataGridViewColumnsErrors.Click += new System.EventHandler(this.kryptonRibbonGroupButtonDataGridViewColumnsErrors_Click);
            // 
            // kryptonRibbonGroupViewRows
            // 
            this.kryptonRibbonGroupViewRows.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupTriple14});
            this.kryptonRibbonGroupViewRows.TextLine1 = "Rows";
            // 
            // kryptonRibbonGroupTriple14
            // 
            this.kryptonRibbonGroupTriple14.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButtonDataGridViewRowsFavorite,
            this.kryptonRibbonGroupButtonDataGridViewRowsHideEqual});
            // 
            // kryptonRibbonGroupButtonDataGridViewRowsFavorite
            // 
            this.kryptonRibbonGroupButtonDataGridViewRowsFavorite.ButtonType = Krypton.Ribbon.GroupButtonType.Check;
            this.kryptonRibbonGroupButtonDataGridViewRowsFavorite.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.GridFavorite32x32;
            this.kryptonRibbonGroupButtonDataGridViewRowsFavorite.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.GridFavorite32x32;
            this.kryptonRibbonGroupButtonDataGridViewRowsFavorite.KeyTip = "F";
            this.kryptonRibbonGroupButtonDataGridViewRowsFavorite.TextLine1 = "Show only";
            this.kryptonRibbonGroupButtonDataGridViewRowsFavorite.TextLine2 = "Favorite";
            this.kryptonRibbonGroupButtonDataGridViewRowsFavorite.Click += new System.EventHandler(this.kryptonRibbonGroupButtonDataGridViewRowsFavorite_Click);
            // 
            // kryptonRibbonGroupButtonDataGridViewRowsHideEqual
            // 
            this.kryptonRibbonGroupButtonDataGridViewRowsHideEqual.ButtonType = Krypton.Ribbon.GroupButtonType.Check;
            this.kryptonRibbonGroupButtonDataGridViewRowsHideEqual.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.GridEqual32x32;
            this.kryptonRibbonGroupButtonDataGridViewRowsHideEqual.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.GridEqual32x32;
            this.kryptonRibbonGroupButtonDataGridViewRowsHideEqual.KeyTip = "=";
            this.kryptonRibbonGroupButtonDataGridViewRowsHideEqual.TextLine1 = "Hide";
            this.kryptonRibbonGroupButtonDataGridViewRowsHideEqual.TextLine2 = "Equal rows";
            this.kryptonRibbonGroupButtonDataGridViewRowsHideEqual.Click += new System.EventHandler(this.kryptonRibbonGroupButtonDataGridViewRowsHideEqual_Click);
            // 
            // kryptonRibbonGroup14
            // 
            this.kryptonRibbonGroup14.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupTriple18});
            this.kryptonRibbonGroup14.TextLine1 = "Favorite";
            // 
            // kryptonRibbonGroupTriple18
            // 
            this.kryptonRibbonGroupTriple18.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButtonDataGridViewFavouriteAdd,
            this.kryptonRibbonGroupButtonDataGridViewFavouriteToggle,
            this.kryptonRibbonGroupButtonDataGridViewFavouriteDelete});
            // 
            // kryptonRibbonGroupButtonDataGridViewFavouriteAdd
            // 
            this.kryptonRibbonGroupButtonDataGridViewFavouriteAdd.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteSelect32x32;
            this.kryptonRibbonGroupButtonDataGridViewFavouriteAdd.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteSelect32x32;
            this.kryptonRibbonGroupButtonDataGridViewFavouriteAdd.TextLine1 = "Make as";
            this.kryptonRibbonGroupButtonDataGridViewFavouriteAdd.TextLine2 = "Fovorite";
            // 
            // kryptonRibbonGroupButtonDataGridViewFavouriteToggle
            // 
            this.kryptonRibbonGroupButtonDataGridViewFavouriteToggle.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteToggle32x32;
            this.kryptonRibbonGroupButtonDataGridViewFavouriteToggle.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteToggle32x32;
            this.kryptonRibbonGroupButtonDataGridViewFavouriteToggle.TextLine1 = "Toggle";
            this.kryptonRibbonGroupButtonDataGridViewFavouriteToggle.TextLine2 = "Selection";
            // 
            // kryptonRibbonGroupButtonDataGridViewFavouriteDelete
            // 
            this.kryptonRibbonGroupButtonDataGridViewFavouriteDelete.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteRemove32x32;
            this.kryptonRibbonGroupButtonDataGridViewFavouriteDelete.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteRemove32x32;
            this.kryptonRibbonGroupButtonDataGridViewFavouriteDelete.TextLine1 = "Set as";
            this.kryptonRibbonGroupButtonDataGridViewFavouriteDelete.TextLine2 = "Normal";
            // 
            // kryptonRibbonTabSelect
            // 
            this.kryptonRibbonTabSelect.Groups.AddRange(new Krypton.Ribbon.KryptonRibbonGroup[] {
            this.kryptonRibbonGroupImageListViewSelect,
            this.kryptonRibbonGroup2,
            this.kryptonRibbonGroup9,
            this.kryptonRibbonGroup7,
            this.kryptonRibbonGroup8});
            this.kryptonRibbonTabSelect.KeyTip = "S";
            this.kryptonRibbonTabSelect.Text = "Select";
            // 
            // kryptonRibbonGroupImageListViewSelect
            // 
            this.kryptonRibbonGroupImageListViewSelect.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupTriple7,
            this.kryptonRibbonGroupSeparator2});
            this.kryptonRibbonGroupImageListViewSelect.TextLine1 = "Select group";
            // 
            // kryptonRibbonGroupTriple7
            // 
            this.kryptonRibbonGroupTriple7.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButton20,
            this.kryptonRibbonGroupButton21});
            // 
            // kryptonRibbonGroupButton20
            // 
            this.kryptonRibbonGroupButton20.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.SelectPrevious;
            this.kryptonRibbonGroupButton20.TextLine1 = "Select";
            this.kryptonRibbonGroupButton20.TextLine2 = "Backwards";
            // 
            // kryptonRibbonGroupButton21
            // 
            this.kryptonRibbonGroupButton21.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.SelectNext;
            this.kryptonRibbonGroupButton21.TextLine1 = "Select";
            this.kryptonRibbonGroupButton21.TextLine2 = "Forwards";
            // 
            // kryptonRibbonGroup2
            // 
            this.kryptonRibbonGroup2.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupLinesSelectByDateInterval});
            this.kryptonRibbonGroup2.TextLine1 = "Date range";
            // 
            // kryptonRibbonGroupLinesSelectByDateInterval
            // 
            this.kryptonRibbonGroupLinesSelectByDateInterval.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupRadioButton1,
            this.kryptonRibbonGroupRadioButton2,
            this.kryptonRibbonGroupRadioButton3,
            this.kryptonRibbonGroupRadioButton4,
            this.kryptonRibbonGroupRadioButton5});
            // 
            // kryptonRibbonGroupRadioButton1
            // 
            this.kryptonRibbonGroupRadioButton1.TextLine1 = "1 day";
            // 
            // kryptonRibbonGroupRadioButton2
            // 
            this.kryptonRibbonGroupRadioButton2.TextLine1 = "3 days";
            // 
            // kryptonRibbonGroupRadioButton3
            // 
            this.kryptonRibbonGroupRadioButton3.TextLine1 = "Week";
            // 
            // kryptonRibbonGroupRadioButton4
            // 
            this.kryptonRibbonGroupRadioButton4.TextLine1 = "2 weeks";
            // 
            // kryptonRibbonGroupRadioButton5
            // 
            this.kryptonRibbonGroupRadioButton5.TextLine1 = "Month";
            // 
            // kryptonRibbonGroup9
            // 
            this.kryptonRibbonGroup9.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupLines4});
            this.kryptonRibbonGroup9.TextLine1 = "Date Priorities";
            // 
            // kryptonRibbonGroupLines4
            // 
            this.kryptonRibbonGroupLines4.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupRadioButton10,
            this.kryptonRibbonGroupRadioButton11});
            // 
            // kryptonRibbonGroupRadioButton10
            // 
            this.kryptonRibbonGroupRadioButton10.TextLine1 = "File created";
            // 
            // kryptonRibbonGroupRadioButton11
            // 
            this.kryptonRibbonGroupRadioButton11.TextLine1 = "Media taken";
            this.kryptonRibbonGroupRadioButton11.TextLine2 = "(if exist)";
            // 
            // kryptonRibbonGroup7
            // 
            this.kryptonRibbonGroup7.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupLines2});
            this.kryptonRibbonGroup7.TextLine1 = "Media amount";
            // 
            // kryptonRibbonGroupLines2
            // 
            this.kryptonRibbonGroupLines2.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupRadioButton6,
            this.kryptonRibbonGroupRadioButton7,
            this.kryptonRibbonGroupRadioButton8,
            this.kryptonRibbonGroupRadioButton9});
            // 
            // kryptonRibbonGroupRadioButton6
            // 
            this.kryptonRibbonGroupRadioButton6.TextLine1 = "Select max";
            this.kryptonRibbonGroupRadioButton6.TextLine2 = "10 files";
            // 
            // kryptonRibbonGroupRadioButton7
            // 
            this.kryptonRibbonGroupRadioButton7.TextLine1 = "Select max";
            this.kryptonRibbonGroupRadioButton7.TextLine2 = "30 files";
            // 
            // kryptonRibbonGroupRadioButton8
            // 
            this.kryptonRibbonGroupRadioButton8.TextLine1 = "Select max";
            this.kryptonRibbonGroupRadioButton8.TextLine2 = "50 files";
            // 
            // kryptonRibbonGroupRadioButton9
            // 
            this.kryptonRibbonGroupRadioButton9.TextLine1 = "Select max";
            this.kryptonRibbonGroupRadioButton9.TextLine2 = "100 files";
            // 
            // kryptonRibbonGroup8
            // 
            this.kryptonRibbonGroup8.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupLines3});
            this.kryptonRibbonGroup8.TextLine1 = "Location";
            // 
            // kryptonRibbonGroupLines3
            // 
            this.kryptonRibbonGroupLines3.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupCheckBox1,
            this.kryptonRibbonGroupCheckBox2,
            this.kryptonRibbonGroupCheckBox3,
            this.kryptonRibbonGroupCheckBox4});
            // 
            // kryptonRibbonGroupCheckBox1
            // 
            this.kryptonRibbonGroupCheckBox1.TextLine1 = "Location name";
            // 
            // kryptonRibbonGroupCheckBox2
            // 
            this.kryptonRibbonGroupCheckBox2.TextLine1 = "City";
            // 
            // kryptonRibbonGroupCheckBox3
            // 
            this.kryptonRibbonGroupCheckBox3.TextLine1 = "State/Region";
            // 
            // kryptonRibbonGroupCheckBox4
            // 
            this.kryptonRibbonGroupCheckBox4.TextLine1 = "Country";
            // 
            // kryptonRibbonTabTools
            // 
            this.kryptonRibbonTabTools.Groups.AddRange(new Krypton.Ribbon.KryptonRibbonGroup[] {
            this.kryptonRibbonGroupToolsMain,
            this.kryptonRibbonGroup1});
            this.kryptonRibbonTabTools.Text = "Tools";
            // 
            // kryptonRibbonGroupToolsMain
            // 
            this.kryptonRibbonGroupToolsMain.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupTriple6});
            this.kryptonRibbonGroupToolsMain.TextLine1 = "Import Tools";
            // 
            // kryptonRibbonGroupTriple6
            // 
            this.kryptonRibbonGroupTriple6.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButtonToolsImportLocations,
            this.kryptonRibbonGroupButtonToolsWebScraping});
            // 
            // kryptonRibbonGroupButtonToolsImportLocations
            // 
            this.kryptonRibbonGroupButtonToolsImportLocations.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.ToolsImportGoogleLocation32x32;
            this.kryptonRibbonGroupButtonToolsImportLocations.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.ToolsImportGoogleLocation32x32;
            this.kryptonRibbonGroupButtonToolsImportLocations.TextLine1 = "Import";
            this.kryptonRibbonGroupButtonToolsImportLocations.TextLine2 = "Locations";
            this.kryptonRibbonGroupButtonToolsImportLocations.ToolTipBody = resources.GetString("kryptonRibbonGroupButtonToolsImportLocations.ToolTipBody");
            this.kryptonRibbonGroupButtonToolsImportLocations.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.ToolsImportGoogleLocation32x32;
            this.kryptonRibbonGroupButtonToolsImportLocations.ToolTipTitle = "Import Locations";
            this.kryptonRibbonGroupButtonToolsImportLocations.Click += new System.EventHandler(this.kryptonRibbonGroupButtonToolsImportLocations_Click);
            // 
            // kryptonRibbonGroupButtonToolsWebScraping
            // 
            this.kryptonRibbonGroupButtonToolsWebScraping.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.ToolsWebScraping32x32;
            this.kryptonRibbonGroupButtonToolsWebScraping.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.ToolsWebScraping32x32;
            this.kryptonRibbonGroupButtonToolsWebScraping.TextLine1 = "Web";
            this.kryptonRibbonGroupButtonToolsWebScraping.TextLine2 = "Scraping";
            this.kryptonRibbonGroupButtonToolsWebScraping.ToolTipBody = "Fetch your own data from web albums that belongs to you.";
            this.kryptonRibbonGroupButtonToolsWebScraping.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.ToolsWebScraping32x32;
            this.kryptonRibbonGroupButtonToolsWebScraping.ToolTipTitle = "WebScraping";
            this.kryptonRibbonGroupButtonToolsWebScraping.Click += new System.EventHandler(this.kryptonRibbonGroupButtonToolsWebScraping_Click);
            // 
            // kryptonRibbonGroup1
            // 
            this.kryptonRibbonGroup1.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupTriple8});
            this.kryptonRibbonGroup1.TextLine1 = "Help";
            // 
            // kryptonRibbonGroupTriple8
            // 
            this.kryptonRibbonGroupTriple8.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButtonToolsConfig,
            this.kryptonRibbonGroupButtonToolsAbout});
            // 
            // kryptonRibbonGroupButtonToolsConfig
            // 
            this.kryptonRibbonGroupButtonToolsConfig.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.ToolsConfig32x32;
            this.kryptonRibbonGroupButtonToolsConfig.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.ToolsConfig32x32;
            this.kryptonRibbonGroupButtonToolsConfig.TextLine1 = "Config";
            this.kryptonRibbonGroupButtonToolsConfig.ToolTipBody = "Here you can confiurate the application so it fits to your needs";
            this.kryptonRibbonGroupButtonToolsConfig.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.ToolsConfig32x32;
            this.kryptonRibbonGroupButtonToolsConfig.ToolTipTitle = "Config";
            this.kryptonRibbonGroupButtonToolsConfig.Click += new System.EventHandler(this.kryptonRibbonGroupButtonToolsConfig_Click);
            // 
            // kryptonRibbonGroupButtonToolsAbout
            // 
            this.kryptonRibbonGroupButtonToolsAbout.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.ToolsAbout32x32;
            this.kryptonRibbonGroupButtonToolsAbout.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.ToolsAbout32x32;
            this.kryptonRibbonGroupButtonToolsAbout.TextLine1 = "About";
            this.kryptonRibbonGroupButtonToolsAbout.ToolTipBody = "Read about this application";
            this.kryptonRibbonGroupButtonToolsAbout.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.ToolsAbout32x32;
            this.kryptonRibbonGroupButtonToolsAbout.ToolTipTitle = "ABout";
            this.kryptonRibbonGroupButtonToolsAbout.Click += new System.EventHandler(this.kryptonRibbonGroupButtonToolsAbout_Click);
            // 
            // kryptonRibbonTabPreview
            // 
            this.kryptonRibbonTabPreview.Groups.AddRange(new Krypton.Ribbon.KryptonRibbonGroup[] {
            this.kryptonRibbonGroupPreviewPreview,
            this.kryptonRibbonGroupPreviewNavigate,
            this.kryptonRibbonGroupPreviewRotate,
            this.kryptonRibbonGroupPreviewSlideshow,
            this.kryptonRibbonGroupPreviewStatus});
            this.kryptonRibbonTabPreview.KeyTip = "P";
            this.kryptonRibbonTabPreview.Text = "Preview";
            // 
            // kryptonRibbonGroupPreviewPreview
            // 
            this.kryptonRibbonGroupPreviewPreview.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupTriple23});
            this.kryptonRibbonGroupPreviewPreview.TextLine1 = "Preview";
            // 
            // kryptonRibbonGroupTriple23
            // 
            this.kryptonRibbonGroupTriple23.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButtonPreviewPreview});
            // 
            // kryptonRibbonGroupButtonPreviewPreview
            // 
            this.kryptonRibbonGroupButtonPreviewPreview.ButtonType = Krypton.Ribbon.GroupButtonType.Check;
            this.kryptonRibbonGroupButtonPreviewPreview.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.MediaPreview;
            this.kryptonRibbonGroupButtonPreviewPreview.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.MediaPreview;
            this.kryptonRibbonGroupButtonPreviewPreview.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.kryptonRibbonGroupButtonPreviewPreview.TextLine1 = "Preview";
            // 
            // kryptonRibbonGroupPreviewNavigate
            // 
            this.kryptonRibbonGroupPreviewNavigate.DialogBoxLauncher = false;
            this.kryptonRibbonGroupPreviewNavigate.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupTriple22,
            this.kryptonRibbonGroupTriple24,
            this.kryptonRibbonGroupLines5});
            this.kryptonRibbonGroupPreviewNavigate.TextLine1 = "Navigate";
            // 
            // kryptonRibbonGroupTriple22
            // 
            this.kryptonRibbonGroupTriple22.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButton17,
            this.kryptonRibbonGroupButton26,
            this.kryptonRibbonGroupButton27});
            // 
            // kryptonRibbonGroupButton17
            // 
            this.kryptonRibbonGroupButton17.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerPrevious;
            this.kryptonRibbonGroupButton17.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerPrevious;
            this.kryptonRibbonGroupButton17.TextLine1 = "Skip";
            this.kryptonRibbonGroupButton17.TextLine2 = "Prev";
            // 
            // kryptonRibbonGroupButton26
            // 
            this.kryptonRibbonGroupButton26.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerNext;
            this.kryptonRibbonGroupButton26.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerNext;
            this.kryptonRibbonGroupButton26.TextLine1 = "Skip";
            this.kryptonRibbonGroupButton26.TextLine2 = "Next";
            // 
            // kryptonRibbonGroupButton27
            // 
            this.kryptonRibbonGroupButton27.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerPause;
            this.kryptonRibbonGroupButton27.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerPause;
            // 
            // kryptonRibbonGroupTriple24
            // 
            this.kryptonRibbonGroupTriple24.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButton29,
            this.kryptonRibbonGroupButton30,
            this.kryptonRibbonGroupButton31});
            // 
            // kryptonRibbonGroupButton29
            // 
            this.kryptonRibbonGroupButton29.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerFastBackward;
            this.kryptonRibbonGroupButton29.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerFastBackward;
            this.kryptonRibbonGroupButton29.TextLine1 = "Rewind";
            // 
            // kryptonRibbonGroupButton30
            // 
            this.kryptonRibbonGroupButton30.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerFastForward;
            this.kryptonRibbonGroupButton30.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerFastForward;
            this.kryptonRibbonGroupButton30.TextLine1 = "Forward";
            // 
            // kryptonRibbonGroupButton31
            // 
            this.kryptonRibbonGroupButton31.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerStop;
            this.kryptonRibbonGroupButton31.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerStop;
            this.kryptonRibbonGroupButton31.TextLine1 = "Stop";
            // 
            // kryptonRibbonGroupLines5
            // 
            this.kryptonRibbonGroupLines5.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupTrackBar1});
            // 
            // kryptonRibbonGroupTrackBar1
            // 
            this.kryptonRibbonGroupTrackBar1.MaximumLength = 55;
            this.kryptonRibbonGroupTrackBar1.MinimumLength = 55;
            // 
            // kryptonRibbonGroupPreviewRotate
            // 
            this.kryptonRibbonGroupPreviewRotate.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupTriple25});
            this.kryptonRibbonGroupPreviewRotate.TextLine1 = "Rotate";
            // 
            // kryptonRibbonGroupTriple25
            // 
            this.kryptonRibbonGroupTriple25.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButton32,
            this.kryptonRibbonGroupButton33,
            this.kryptonRibbonGroupButton34});
            // 
            // kryptonRibbonGroupButton32
            // 
            this.kryptonRibbonGroupButton32.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.MediaRotate90CCW;
            this.kryptonRibbonGroupButton32.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.MediaRotate90CCW;
            this.kryptonRibbonGroupButton32.TextLine1 = "90° CCW";
            // 
            // kryptonRibbonGroupButton33
            // 
            this.kryptonRibbonGroupButton33.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.MediaRotate180;
            this.kryptonRibbonGroupButton33.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.MediaRotate180;
            this.kryptonRibbonGroupButton33.TextLine1 = "180°";
            // 
            // kryptonRibbonGroupButton34
            // 
            this.kryptonRibbonGroupButton34.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.MediaRotate90CW;
            this.kryptonRibbonGroupButton34.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.MediaRotate90CW;
            this.kryptonRibbonGroupButton34.TextLine1 = "90° CW";
            // 
            // kryptonRibbonGroupPreviewSlideshow
            // 
            this.kryptonRibbonGroupPreviewSlideshow.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupTriple26});
            this.kryptonRibbonGroupPreviewSlideshow.TextLine1 = "Slideshow";
            // 
            // kryptonRibbonGroupTriple26
            // 
            this.kryptonRibbonGroupTriple26.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButton35,
            this.kryptonRibbonGroupButton36,
            this.kryptonRibbonGroupButton37});
            // 
            // kryptonRibbonGroupButton35
            // 
            this.kryptonRibbonGroupButton35.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.MediaPreviewSlideshow;
            this.kryptonRibbonGroupButton35.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.MediaPreviewSlideshow;
            this.kryptonRibbonGroupButton35.TextLine1 = "Slideshow";
            // 
            // kryptonRibbonGroupButton36
            // 
            this.kryptonRibbonGroupButton36.ButtonType = Krypton.Ribbon.GroupButtonType.DropDown;
            this.kryptonRibbonGroupButton36.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.MediaPreviewSlideshowInterval;
            this.kryptonRibbonGroupButton36.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.MediaPreviewSlideshowInterval;
            this.kryptonRibbonGroupButton36.TextLine1 = "Time";
            this.kryptonRibbonGroupButton36.TextLine2 = "Interval";
            // 
            // kryptonRibbonGroupButton37
            // 
            this.kryptonRibbonGroupButton37.ButtonType = Krypton.Ribbon.GroupButtonType.DropDown;
            this.kryptonRibbonGroupButton37.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerChromecast;
            this.kryptonRibbonGroupButton37.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerChromecast;
            this.kryptonRibbonGroupButton37.TextLine1 = "Chromecast";
            // 
            // kryptonRibbonGroupPreviewStatus
            // 
            this.kryptonRibbonGroupPreviewStatus.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupLines1});
            this.kryptonRibbonGroupPreviewStatus.TextLine1 = "Status";
            // 
            // kryptonRibbonGroupLines1
            // 
            this.kryptonRibbonGroupLines1.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupLabel1,
            this.kryptonRibbonGroupLabel2});
            // 
            // kryptonRibbonGroupLabel1
            // 
            this.kryptonRibbonGroupLabel1.TextLine1 = "Timer:";
            this.kryptonRibbonGroupLabel1.TextLine2 = "00:00";
            // 
            // kryptonRibbonGroupLabel2
            // 
            this.kryptonRibbonGroupLabel2.TextLine1 = "Status:";
            this.kryptonRibbonGroupLabel2.TextLine2 = "Waiting";
            // 
            // kryptonContextMenuImageListViewModeThumbnailRenders
            // 
            this.kryptonContextMenuImageListViewModeThumbnailRenders.Items.AddRange(new Krypton.Toolkit.KryptonContextMenuItemBase[] {
            this.kryptonContextMenuItems3});
            // 
            // kryptonContextMenuItem1
            // 
            this.kryptonContextMenuItem1.Text = "Menu Item";
            // 
            // kryptonContextMenuItem2
            // 
            this.kryptonContextMenuItem2.Text = "Menu Item";
            // 
            // kryptonContextMenuItem3
            // 
            this.kryptonContextMenuItem3.Text = "Menu Item";
            // 
            // kryptonContextMenuHeading1
            // 
            this.kryptonContextMenuHeading1.ExtraText = "";
            // 
            // kryptonContextMenuMonthCalendar1
            // 
            this.kryptonContextMenuMonthCalendar1.SelectionEnd = new System.DateTime(2021, 9, 1, 0, 0, 0, 0);
            this.kryptonContextMenuMonthCalendar1.SelectionStart = new System.DateTime(2021, 9, 1, 0, 0, 0, 0);
            this.kryptonContextMenuMonthCalendar1.TodayDate = new System.DateTime(2021, 9, 1, 0, 0, 0, 0);
            // 
            // kryptonContextMenuMonthCalendar2
            // 
            this.kryptonContextMenuMonthCalendar2.SelectionEnd = new System.DateTime(2021, 9, 1, 0, 0, 0, 0);
            this.kryptonContextMenuMonthCalendar2.SelectionStart = new System.DateTime(2021, 9, 1, 0, 0, 0, 0);
            this.kryptonContextMenuMonthCalendar2.TodayDate = new System.DateTime(2021, 9, 1, 0, 0, 0, 0);
            // 
            // kryptonContextMenuItem4
            // 
            this.kryptonContextMenuItem4.Text = "Menu Item";
            // 
            // kryptonContextMenuItem21
            // 
            this.kryptonContextMenuItem21.Text = "Menu Item";
            // 
            // kryptonContextMenuItem5
            // 
            this.kryptonContextMenuItem5.Text = "Menu Item";
            // 
            // kryptonContextMenuItem6
            // 
            this.kryptonContextMenuItem6.Text = "Menu Item";
            // 
            // kryptonContextMenuItem7
            // 
            this.kryptonContextMenuItem7.Text = "Menu Item";
            // 
            // kryptonContextMenuItem8
            // 
            this.kryptonContextMenuItem8.Text = "Menu Item";
            // 
            // sortMediaFileByToolStripMenuItem
            // 
            this.sortMediaFileByToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemSortByFilename,
            this.ToolStripMenuItemSortByFileCreatedDate,
            this.ToolStripMenuItemSortByFileModifiedDate,
            this.ToolStripMenuItemSortByMediaDateTaken,
            this.ToolStripMenuItemSortByMediaAlbum,
            this.ToolStripMenuItemSortByMediaTitle,
            this.ToolStripMenuItemSortByMediaDescription,
            this.ToolStripMenuItemSortByMediaComments,
            this.ToolStripMenuItemSortByMediaAuthor,
            this.ToolStripMenuItemSortByMediaRating,
            this.ToolStripMenuItemSortByLocationName,
            this.ToolStripMenuItemSortByLocationRegionState,
            this.ToolStripMenuItemSortByLocationCity,
            this.ToolStripMenuItemSortByLocationCountry});
            this.sortMediaFileByToolStripMenuItem.Name = "sortMediaFileByToolStripMenuItem";
            this.sortMediaFileByToolStripMenuItem.Size = new System.Drawing.Size(324, 26);
            this.sortMediaFileByToolStripMenuItem.Text = "Sort media files by...";
            // 
            // ToolStripMenuItemSortByFilename
            // 
            this.ToolStripMenuItemSortByFilename.Name = "ToolStripMenuItemSortByFilename";
            this.ToolStripMenuItemSortByFilename.Size = new System.Drawing.Size(191, 22);
            this.ToolStripMenuItemSortByFilename.Text = "File name";
            this.ToolStripMenuItemSortByFilename.Click += new System.EventHandler(this.ToolStripMenuItemSortByFilename_Click);
            // 
            // ToolStripMenuItemSortByFileCreatedDate
            // 
            this.ToolStripMenuItemSortByFileCreatedDate.Name = "ToolStripMenuItemSortByFileCreatedDate";
            this.ToolStripMenuItemSortByFileCreatedDate.Size = new System.Drawing.Size(191, 22);
            this.ToolStripMenuItemSortByFileCreatedDate.Text = "File created date";
            this.ToolStripMenuItemSortByFileCreatedDate.Click += new System.EventHandler(this.ToolStripMenuItemSortByFileCreatedDate_Click);
            // 
            // ToolStripMenuItemSortByFileModifiedDate
            // 
            this.ToolStripMenuItemSortByFileModifiedDate.Name = "ToolStripMenuItemSortByFileModifiedDate";
            this.ToolStripMenuItemSortByFileModifiedDate.Size = new System.Drawing.Size(191, 22);
            this.ToolStripMenuItemSortByFileModifiedDate.Text = "File modified date";
            this.ToolStripMenuItemSortByFileModifiedDate.Click += new System.EventHandler(this.ToolStripMenuItemSortByFileModifiedDate_Click);
            // 
            // ToolStripMenuItemSortByMediaDateTaken
            // 
            this.ToolStripMenuItemSortByMediaDateTaken.Name = "ToolStripMenuItemSortByMediaDateTaken";
            this.ToolStripMenuItemSortByMediaDateTaken.Size = new System.Drawing.Size(191, 22);
            this.ToolStripMenuItemSortByMediaDateTaken.Text = "Media Date Taken";
            this.ToolStripMenuItemSortByMediaDateTaken.Click += new System.EventHandler(this.ToolStripMenuItemSortByMediaDateTaken_Click);
            // 
            // ToolStripMenuItemSortByMediaAlbum
            // 
            this.ToolStripMenuItemSortByMediaAlbum.Name = "ToolStripMenuItemSortByMediaAlbum";
            this.ToolStripMenuItemSortByMediaAlbum.Size = new System.Drawing.Size(191, 22);
            this.ToolStripMenuItemSortByMediaAlbum.Text = "Media Album";
            this.ToolStripMenuItemSortByMediaAlbum.Click += new System.EventHandler(this.ToolStripMenuItemSortByMediaAlbum_Click);
            // 
            // ToolStripMenuItemSortByMediaTitle
            // 
            this.ToolStripMenuItemSortByMediaTitle.Name = "ToolStripMenuItemSortByMediaTitle";
            this.ToolStripMenuItemSortByMediaTitle.Size = new System.Drawing.Size(191, 22);
            this.ToolStripMenuItemSortByMediaTitle.Text = "Media Title";
            this.ToolStripMenuItemSortByMediaTitle.Click += new System.EventHandler(this.ToolStripMenuItemSortByMediaTitle_Click);
            // 
            // ToolStripMenuItemSortByMediaDescription
            // 
            this.ToolStripMenuItemSortByMediaDescription.Name = "ToolStripMenuItemSortByMediaDescription";
            this.ToolStripMenuItemSortByMediaDescription.Size = new System.Drawing.Size(191, 22);
            this.ToolStripMenuItemSortByMediaDescription.Text = "Media Description";
            this.ToolStripMenuItemSortByMediaDescription.Click += new System.EventHandler(this.ToolStripMenuItemSortByMediaDescription_Click);
            // 
            // ToolStripMenuItemSortByMediaComments
            // 
            this.ToolStripMenuItemSortByMediaComments.Name = "ToolStripMenuItemSortByMediaComments";
            this.ToolStripMenuItemSortByMediaComments.Size = new System.Drawing.Size(191, 22);
            this.ToolStripMenuItemSortByMediaComments.Text = "Media Comments";
            this.ToolStripMenuItemSortByMediaComments.Click += new System.EventHandler(this.ToolStripMenuItemSortByMediaComments_Click);
            // 
            // ToolStripMenuItemSortByMediaAuthor
            // 
            this.ToolStripMenuItemSortByMediaAuthor.Name = "ToolStripMenuItemSortByMediaAuthor";
            this.ToolStripMenuItemSortByMediaAuthor.Size = new System.Drawing.Size(191, 22);
            this.ToolStripMenuItemSortByMediaAuthor.Text = "Media Author";
            this.ToolStripMenuItemSortByMediaAuthor.Click += new System.EventHandler(this.ToolStripMenuItemSortByMediaAuthor_Click);
            // 
            // ToolStripMenuItemSortByMediaRating
            // 
            this.ToolStripMenuItemSortByMediaRating.Name = "ToolStripMenuItemSortByMediaRating";
            this.ToolStripMenuItemSortByMediaRating.Size = new System.Drawing.Size(191, 22);
            this.ToolStripMenuItemSortByMediaRating.Text = "Media Rating";
            this.ToolStripMenuItemSortByMediaRating.Click += new System.EventHandler(this.ToolStripMenuItemSortByMediaRating_Click);
            // 
            // ToolStripMenuItemSortByLocationName
            // 
            this.ToolStripMenuItemSortByLocationName.Name = "ToolStripMenuItemSortByLocationName";
            this.ToolStripMenuItemSortByLocationName.Size = new System.Drawing.Size(191, 22);
            this.ToolStripMenuItemSortByLocationName.Text = "Location Name";
            this.ToolStripMenuItemSortByLocationName.Click += new System.EventHandler(this.ToolStripMenuItemSortByLocationName_Click);
            // 
            // ToolStripMenuItemSortByLocationRegionState
            // 
            this.ToolStripMenuItemSortByLocationRegionState.Name = "ToolStripMenuItemSortByLocationRegionState";
            this.ToolStripMenuItemSortByLocationRegionState.Size = new System.Drawing.Size(191, 22);
            this.ToolStripMenuItemSortByLocationRegionState.Text = "Location Region/State";
            this.ToolStripMenuItemSortByLocationRegionState.Click += new System.EventHandler(this.ToolStripMenuItemSortByLocationRegionState_Click);
            // 
            // ToolStripMenuItemSortByLocationCity
            // 
            this.ToolStripMenuItemSortByLocationCity.Name = "ToolStripMenuItemSortByLocationCity";
            this.ToolStripMenuItemSortByLocationCity.Size = new System.Drawing.Size(191, 22);
            this.ToolStripMenuItemSortByLocationCity.Text = "Location City";
            this.ToolStripMenuItemSortByLocationCity.Click += new System.EventHandler(this.ToolStripMenuItemSortByLocationCity_Click);
            // 
            // ToolStripMenuItemSortByLocationCountry
            // 
            this.ToolStripMenuItemSortByLocationCountry.Name = "ToolStripMenuItemSortByLocationCountry";
            this.ToolStripMenuItemSortByLocationCountry.Size = new System.Drawing.Size(191, 22);
            this.ToolStripMenuItemSortByLocationCountry.Text = "Location Country";
            this.ToolStripMenuItemSortByLocationCountry.Click += new System.EventHandler(this.ToolStripMenuItemSortByLocationCountry_Click);
            // 
            // openMediaFilesWithToolStripMenuItem
            // 
            this.openMediaFilesWithToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testToolStripMenuItem});
            this.openMediaFilesWithToolStripMenuItem.Name = "openMediaFilesWithToolStripMenuItem";
            this.openMediaFilesWithToolStripMenuItem.Size = new System.Drawing.Size(324, 26);
            this.openMediaFilesWithToolStripMenuItem.Text = "Open media files with...";
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.testToolStripMenuItem.Text = "Test";
            // 
            // contextMenuStripImageListView
            // 
            this.contextMenuStripImageListView.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.contextMenuStripImageListView.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripImageListView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sortMediaFileByToolStripMenuItem,
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
            this.toolStripMenuItemImageListViewAutoCorrectForm,
            this.openFileWithAssociatedApplicationToolStripMenuItem,
            this.openMediaFilesWithToolStripMenuItem,
            this.editFileWithAssociatedApplicationToolStripMenuItem,
            this.runSelectedToolStripMenuItem1,
            this.openWithDialogToolStripMenuItem,
            this.openFileLocationToolStripMenuItem,
            this.rotateCW90ToolStripMenuItem,
            this.rotate180ToolStripMenuItem,
            this.ratateCCW270ToolStripMenuItem,
            this.showMediaPosterToolStripMenuItem,
            this.mediaPreviewToolStripMenuItem});
            this.contextMenuStripImageListView.Name = "contextMenuStripImageListView";
            this.contextMenuStripImageListView.Size = new System.Drawing.Size(325, 602);
            // 
            // toolStripMenuItemImageListViewCut
            // 
            this.toolStripMenuItemImageListViewCut.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditCut32x32;
            this.toolStripMenuItemImageListViewCut.Name = "toolStripMenuItemImageListViewCut";
            this.toolStripMenuItemImageListViewCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.toolStripMenuItemImageListViewCut.Size = new System.Drawing.Size(324, 26);
            this.toolStripMenuItemImageListViewCut.Text = "Cut";
            // 
            // toolStripMenuItemImageListViewCopy
            // 
            this.toolStripMenuItemImageListViewCopy.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditCopy32x32;
            this.toolStripMenuItemImageListViewCopy.Name = "toolStripMenuItemImageListViewCopy";
            this.toolStripMenuItemImageListViewCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.toolStripMenuItemImageListViewCopy.Size = new System.Drawing.Size(324, 26);
            this.toolStripMenuItemImageListViewCopy.Text = "Copy";
            // 
            // copyFileNamesToClipboardToolStripMenuItem
            // 
            this.copyFileNamesToClipboardToolStripMenuItem.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditCopy32x32;
            this.copyFileNamesToClipboardToolStripMenuItem.Name = "copyFileNamesToClipboardToolStripMenuItem";
            this.copyFileNamesToClipboardToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.C)));
            this.copyFileNamesToClipboardToolStripMenuItem.Size = new System.Drawing.Size(324, 26);
            this.copyFileNamesToClipboardToolStripMenuItem.Text = "Copy file names to Clipboard";
            // 
            // toolStripMenuItemImageListViewPaste
            // 
            this.toolStripMenuItemImageListViewPaste.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditPaste32x32;
            this.toolStripMenuItemImageListViewPaste.Name = "toolStripMenuItemImageListViewPaste";
            this.toolStripMenuItemImageListViewPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.toolStripMenuItemImageListViewPaste.Size = new System.Drawing.Size(324, 26);
            this.toolStripMenuItemImageListViewPaste.Text = "Paste";
            // 
            // toolStripMenuItemImageListViewDelete
            // 
            this.toolStripMenuItemImageListViewDelete.Image = global::PhotoTagsSynchronizer.Properties.Resources.FileSystemDelete32x32;
            this.toolStripMenuItemImageListViewDelete.Name = "toolStripMenuItemImageListViewDelete";
            this.toolStripMenuItemImageListViewDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.toolStripMenuItemImageListViewDelete.Size = new System.Drawing.Size(324, 26);
            this.toolStripMenuItemImageListViewDelete.Text = "Delete";
            // 
            // toolStripMenuItemImageListViewRefreshFolder
            // 
            this.toolStripMenuItemImageListViewRefreshFolder.Image = global::PhotoTagsSynchronizer.Properties.Resources.FileSystemRefresh32x32;
            this.toolStripMenuItemImageListViewRefreshFolder.Name = "toolStripMenuItemImageListViewRefreshFolder";
            this.toolStripMenuItemImageListViewRefreshFolder.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.toolStripMenuItemImageListViewRefreshFolder.Size = new System.Drawing.Size(324, 26);
            this.toolStripMenuItemImageListViewRefreshFolder.Text = "Refresh folder";
            // 
            // toolStripMenuItemImageListViewReloadThumbnailAndMetadata
            // 
            this.toolStripMenuItemImageListViewReloadThumbnailAndMetadata.Image = global::PhotoTagsSynchronizer.Properties.Resources.MetadataReload;
            this.toolStripMenuItemImageListViewReloadThumbnailAndMetadata.Name = "toolStripMenuItemImageListViewReloadThumbnailAndMetadata";
            this.toolStripMenuItemImageListViewReloadThumbnailAndMetadata.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F5)));
            this.toolStripMenuItemImageListViewReloadThumbnailAndMetadata.Size = new System.Drawing.Size(324, 26);
            this.toolStripMenuItemImageListViewReloadThumbnailAndMetadata.Text = "Reload thumbnail and metadata";
            // 
            // toolStripMenuItemImageListViewReloadThumbnailAndMetadataClearThumbnailAndMetadataHistory
            // 
            this.toolStripMenuItemImageListViewReloadThumbnailAndMetadataClearThumbnailAndMetadataHistory.Image = global::PhotoTagsSynchronizer.Properties.Resources.MetadataDeleteHistory;
            this.toolStripMenuItemImageListViewReloadThumbnailAndMetadataClearThumbnailAndMetadataHistory.Name = "toolStripMenuItemImageListViewReloadThumbnailAndMetadataClearThumbnailAndMetadata" +
    "History";
            this.toolStripMenuItemImageListViewReloadThumbnailAndMetadataClearThumbnailAndMetadataHistory.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F5)));
            this.toolStripMenuItemImageListViewReloadThumbnailAndMetadataClearThumbnailAndMetadataHistory.Size = new System.Drawing.Size(324, 26);
            this.toolStripMenuItemImageListViewReloadThumbnailAndMetadataClearThumbnailAndMetadataHistory.Text = "Clear thumbnail and metadata history";
            // 
            // toolStripMenuItemImageListViewSelectAll
            // 
            this.toolStripMenuItemImageListViewSelectAll.Image = global::PhotoTagsSynchronizer.Properties.Resources.CheckAll16x16;
            this.toolStripMenuItemImageListViewSelectAll.Name = "toolStripMenuItemImageListViewSelectAll";
            this.toolStripMenuItemImageListViewSelectAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.toolStripMenuItemImageListViewSelectAll.Size = new System.Drawing.Size(324, 26);
            this.toolStripMenuItemImageListViewSelectAll.Text = "Select All";
            this.toolStripMenuItemImageListViewSelectAll.Click += new System.EventHandler(this.toolStripMenuItemSelectAll_Click);
            // 
            // toolStripMenuItemImageListViewAutoCorrect
            // 
            this.toolStripMenuItemImageListViewAutoCorrect.Image = global::PhotoTagsSynchronizer.Properties.Resources.MetadataAutoCorrectRun32x32;
            this.toolStripMenuItemImageListViewAutoCorrect.Name = "toolStripMenuItemImageListViewAutoCorrect";
            this.toolStripMenuItemImageListViewAutoCorrect.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.T)));
            this.toolStripMenuItemImageListViewAutoCorrect.Size = new System.Drawing.Size(324, 26);
            this.toolStripMenuItemImageListViewAutoCorrect.Text = "AutoCorrect metadata";
            // 
            // toolStripMenuItemImageListViewAutoCorrectForm
            // 
            this.toolStripMenuItemImageListViewAutoCorrectForm.Image = global::PhotoTagsSynchronizer.Properties.Resources.MetadataAutoCorrectRun32x32;
            this.toolStripMenuItemImageListViewAutoCorrectForm.Name = "toolStripMenuItemImageListViewAutoCorrectForm";
            this.toolStripMenuItemImageListViewAutoCorrectForm.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.T)));
            this.toolStripMenuItemImageListViewAutoCorrectForm.Size = new System.Drawing.Size(324, 26);
            this.toolStripMenuItemImageListViewAutoCorrectForm.Text = "AutoCorrect metadata form...";
            // 
            // openFileWithAssociatedApplicationToolStripMenuItem
            // 
            this.openFileWithAssociatedApplicationToolStripMenuItem.Image = global::PhotoTagsSynchronizer.Properties.Resources.FileSystemOpen;
            this.openFileWithAssociatedApplicationToolStripMenuItem.Name = "openFileWithAssociatedApplicationToolStripMenuItem";
            this.openFileWithAssociatedApplicationToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.openFileWithAssociatedApplicationToolStripMenuItem.Size = new System.Drawing.Size(324, 26);
            this.openFileWithAssociatedApplicationToolStripMenuItem.Text = "Open";
            // 
            // editFileWithAssociatedApplicationToolStripMenuItem
            // 
            this.editFileWithAssociatedApplicationToolStripMenuItem.Image = global::PhotoTagsSynchronizer.Properties.Resources.FileSystemEdit;
            this.editFileWithAssociatedApplicationToolStripMenuItem.Name = "editFileWithAssociatedApplicationToolStripMenuItem";
            this.editFileWithAssociatedApplicationToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F4;
            this.editFileWithAssociatedApplicationToolStripMenuItem.Size = new System.Drawing.Size(324, 26);
            this.editFileWithAssociatedApplicationToolStripMenuItem.Text = "Edit";
            // 
            // runSelectedToolStripMenuItem1
            // 
            this.runSelectedToolStripMenuItem1.Image = global::PhotoTagsSynchronizer.Properties.Resources.ToolsRunCommand32x32;
            this.runSelectedToolStripMenuItem1.Name = "runSelectedToolStripMenuItem1";
            this.runSelectedToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.R)));
            this.runSelectedToolStripMenuItem1.Size = new System.Drawing.Size(324, 26);
            this.runSelectedToolStripMenuItem1.Text = "Run batch app or command...";
            // 
            // openWithDialogToolStripMenuItem
            // 
            this.openWithDialogToolStripMenuItem.Image = global::PhotoTagsSynchronizer.Properties.Resources.FileSystemOpenWIthAssociationApp32x32;
            this.openWithDialogToolStripMenuItem.Name = "openWithDialogToolStripMenuItem";
            this.openWithDialogToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openWithDialogToolStripMenuItem.Size = new System.Drawing.Size(324, 26);
            this.openWithDialogToolStripMenuItem.Text = "Open and associate with dialog...";
            // 
            // openFileLocationToolStripMenuItem
            // 
            this.openFileLocationToolStripMenuItem.Image = global::PhotoTagsSynchronizer.Properties.Resources.FileSystemExplorer32x32;
            this.openFileLocationToolStripMenuItem.Name = "openFileLocationToolStripMenuItem";
            this.openFileLocationToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.L)));
            this.openFileLocationToolStripMenuItem.Size = new System.Drawing.Size(324, 26);
            this.openFileLocationToolStripMenuItem.Text = "Open file Location";
            // 
            // rotateCW90ToolStripMenuItem
            // 
            this.rotateCW90ToolStripMenuItem.Image = global::PhotoTagsSynchronizer.Properties.Resources.MediaRotate90CW;
            this.rotateCW90ToolStripMenuItem.Name = "rotateCW90ToolStripMenuItem";
            this.rotateCW90ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D9)));
            this.rotateCW90ToolStripMenuItem.Size = new System.Drawing.Size(324, 26);
            this.rotateCW90ToolStripMenuItem.Text = "Rotate CW - 90°";
            this.rotateCW90ToolStripMenuItem.ToolTipText = "Rotate CW - 90°";
            // 
            // rotate180ToolStripMenuItem
            // 
            this.rotate180ToolStripMenuItem.Image = global::PhotoTagsSynchronizer.Properties.Resources.MediaRotate180;
            this.rotate180ToolStripMenuItem.Name = "rotate180ToolStripMenuItem";
            this.rotate180ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D1)));
            this.rotate180ToolStripMenuItem.Size = new System.Drawing.Size(324, 26);
            this.rotate180ToolStripMenuItem.Text = "Rotate 180°";
            this.rotate180ToolStripMenuItem.ToolTipText = "Rotate 180°";
            // 
            // ratateCCW270ToolStripMenuItem
            // 
            this.ratateCCW270ToolStripMenuItem.Image = global::PhotoTagsSynchronizer.Properties.Resources.MediaRotate90CCW;
            this.ratateCCW270ToolStripMenuItem.Name = "ratateCCW270ToolStripMenuItem";
            this.ratateCCW270ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D2)));
            this.ratateCCW270ToolStripMenuItem.Size = new System.Drawing.Size(324, 26);
            this.ratateCCW270ToolStripMenuItem.Text = "Ratate CCW - 270°";
            this.ratateCCW270ToolStripMenuItem.ToolTipText = "Ratate CCW - 270°";
            // 
            // showMediaPosterToolStripMenuItem
            // 
            this.showMediaPosterToolStripMenuItem.Image = global::PhotoTagsSynchronizer.Properties.Resources.MediaRegionSelector;
            this.showMediaPosterToolStripMenuItem.Name = "showMediaPosterToolStripMenuItem";
            this.showMediaPosterToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.showMediaPosterToolStripMenuItem.Size = new System.Drawing.Size(324, 26);
            this.showMediaPosterToolStripMenuItem.Text = "Show Media Poster Window";
            // 
            // mediaPreviewToolStripMenuItem
            // 
            this.mediaPreviewToolStripMenuItem.Image = global::PhotoTagsSynchronizer.Properties.Resources.MediaPreview;
            this.mediaPreviewToolStripMenuItem.Name = "mediaPreviewToolStripMenuItem";
            this.mediaPreviewToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.mediaPreviewToolStripMenuItem.Size = new System.Drawing.Size(324, 26);
            this.mediaPreviewToolStripMenuItem.Text = "Media preview";
            // 
            // contextMenuStripTreeViewFolder
            // 
            this.contextMenuStripTreeViewFolder.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.contextMenuStripTreeViewFolder.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripTreeViewFolder.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemTreeViewFolderCut,
            this.toolStripMenuItemTreeViewFolderCopy,
            this.toolStripMenuItemTreeViewFolderCopyFolderNameToClipboard,
            this.toolStripMenuItemTreeViewFolderPaste,
            this.toolStripMenuItemTreeViewFolderDelete,
            this.toolStripMenuItemTreeViewFolderRefreshFolder,
            this.toolStripMenuItemTreeViewFolderReadSubfolders,
            this.toolStripMenuItemTreeViewFolderReload,
            this.toolStripMenuItemTreeViewFolderClearCache,
            this.toolStripMenuItemTreeViewFolderAutoCorrectMetadata,
            this.openFolderLocationToolStripMenuItem});
            this.contextMenuStripTreeViewFolder.Name = "contextMenuStripImageListView";
            this.contextMenuStripTreeViewFolder.Size = new System.Drawing.Size(325, 290);
            // 
            // toolStripMenuItemTreeViewFolderCut
            // 
            this.toolStripMenuItemTreeViewFolderCut.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditCut32x32;
            this.toolStripMenuItemTreeViewFolderCut.Name = "toolStripMenuItemTreeViewFolderCut";
            this.toolStripMenuItemTreeViewFolderCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.toolStripMenuItemTreeViewFolderCut.Size = new System.Drawing.Size(324, 26);
            this.toolStripMenuItemTreeViewFolderCut.Text = "Cut";
            // 
            // toolStripMenuItemTreeViewFolderCopy
            // 
            this.toolStripMenuItemTreeViewFolderCopy.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditCopy32x32;
            this.toolStripMenuItemTreeViewFolderCopy.Name = "toolStripMenuItemTreeViewFolderCopy";
            this.toolStripMenuItemTreeViewFolderCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.toolStripMenuItemTreeViewFolderCopy.Size = new System.Drawing.Size(324, 26);
            this.toolStripMenuItemTreeViewFolderCopy.Text = "Copy";
            // 
            // toolStripMenuItemTreeViewFolderCopyFolderNameToClipboard
            // 
            this.toolStripMenuItemTreeViewFolderCopyFolderNameToClipboard.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditCopy32x32;
            this.toolStripMenuItemTreeViewFolderCopyFolderNameToClipboard.Name = "toolStripMenuItemTreeViewFolderCopyFolderNameToClipboard";
            this.toolStripMenuItemTreeViewFolderCopyFolderNameToClipboard.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.C)));
            this.toolStripMenuItemTreeViewFolderCopyFolderNameToClipboard.Size = new System.Drawing.Size(324, 26);
            this.toolStripMenuItemTreeViewFolderCopyFolderNameToClipboard.Text = "Copy folder path to Clipboard";
            // 
            // toolStripMenuItemTreeViewFolderPaste
            // 
            this.toolStripMenuItemTreeViewFolderPaste.Image = global::PhotoTagsSynchronizer.Properties.Resources.EditPaste32x32;
            this.toolStripMenuItemTreeViewFolderPaste.Name = "toolStripMenuItemTreeViewFolderPaste";
            this.toolStripMenuItemTreeViewFolderPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.toolStripMenuItemTreeViewFolderPaste.Size = new System.Drawing.Size(324, 26);
            this.toolStripMenuItemTreeViewFolderPaste.Text = "Paste";
            // 
            // toolStripMenuItemTreeViewFolderDelete
            // 
            this.toolStripMenuItemTreeViewFolderDelete.Image = global::PhotoTagsSynchronizer.Properties.Resources.FileSystemDelete32x32;
            this.toolStripMenuItemTreeViewFolderDelete.Name = "toolStripMenuItemTreeViewFolderDelete";
            this.toolStripMenuItemTreeViewFolderDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.toolStripMenuItemTreeViewFolderDelete.Size = new System.Drawing.Size(324, 26);
            this.toolStripMenuItemTreeViewFolderDelete.Text = "Delete";
            // 
            // toolStripMenuItemTreeViewFolderRefreshFolder
            // 
            this.toolStripMenuItemTreeViewFolderRefreshFolder.Image = global::PhotoTagsSynchronizer.Properties.Resources.FileSystemRefresh32x32;
            this.toolStripMenuItemTreeViewFolderRefreshFolder.Name = "toolStripMenuItemTreeViewFolderRefreshFolder";
            this.toolStripMenuItemTreeViewFolderRefreshFolder.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.toolStripMenuItemTreeViewFolderRefreshFolder.Size = new System.Drawing.Size(324, 26);
            this.toolStripMenuItemTreeViewFolderRefreshFolder.Text = "Refresh folder";
            // 
            // toolStripMenuItemTreeViewFolderReadSubfolders
            // 
            this.toolStripMenuItemTreeViewFolderReadSubfolders.Image = global::PhotoTagsSynchronizer.Properties.Resources.MetadataAddSubfolders32x32;
            this.toolStripMenuItemTreeViewFolderReadSubfolders.Name = "toolStripMenuItemTreeViewFolderReadSubfolders";
            this.toolStripMenuItemTreeViewFolderReadSubfolders.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.toolStripMenuItemTreeViewFolderReadSubfolders.Size = new System.Drawing.Size(324, 26);
            this.toolStripMenuItemTreeViewFolderReadSubfolders.Text = "Read subfolders";
            // 
            // toolStripMenuItemTreeViewFolderReload
            // 
            this.toolStripMenuItemTreeViewFolderReload.Image = global::PhotoTagsSynchronizer.Properties.Resources.MetadataReload;
            this.toolStripMenuItemTreeViewFolderReload.Name = "toolStripMenuItemTreeViewFolderReload";
            this.toolStripMenuItemTreeViewFolderReload.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F5)));
            this.toolStripMenuItemTreeViewFolderReload.Size = new System.Drawing.Size(324, 26);
            this.toolStripMenuItemTreeViewFolderReload.Text = "Reload thumbnail and metadata";
            // 
            // toolStripMenuItemTreeViewFolderClearCache
            // 
            this.toolStripMenuItemTreeViewFolderClearCache.Image = global::PhotoTagsSynchronizer.Properties.Resources.MetadataDeleteHistory;
            this.toolStripMenuItemTreeViewFolderClearCache.Name = "toolStripMenuItemTreeViewFolderClearCache";
            this.toolStripMenuItemTreeViewFolderClearCache.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F5)));
            this.toolStripMenuItemTreeViewFolderClearCache.Size = new System.Drawing.Size(324, 26);
            this.toolStripMenuItemTreeViewFolderClearCache.Text = "Clear thumbnail and metadata history";
            // 
            // toolStripMenuItemTreeViewFolderAutoCorrectMetadata
            // 
            this.toolStripMenuItemTreeViewFolderAutoCorrectMetadata.Image = global::PhotoTagsSynchronizer.Properties.Resources.MetadataAutoCorrectRun32x32;
            this.toolStripMenuItemTreeViewFolderAutoCorrectMetadata.Name = "toolStripMenuItemTreeViewFolderAutoCorrectMetadata";
            this.toolStripMenuItemTreeViewFolderAutoCorrectMetadata.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.T)));
            this.toolStripMenuItemTreeViewFolderAutoCorrectMetadata.Size = new System.Drawing.Size(324, 26);
            this.toolStripMenuItemTreeViewFolderAutoCorrectMetadata.Text = "AutoCorrect metadata";
            // 
            // openFolderLocationToolStripMenuItem
            // 
            this.openFolderLocationToolStripMenuItem.Image = global::PhotoTagsSynchronizer.Properties.Resources.FileSystemExplorer32x32;
            this.openFolderLocationToolStripMenuItem.Name = "openFolderLocationToolStripMenuItem";
            this.openFolderLocationToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.L)));
            this.openFolderLocationToolStripMenuItem.Size = new System.Drawing.Size(324, 26);
            this.openFolderLocationToolStripMenuItem.Text = "Open Folder Location";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1214, 894);
            this.Controls.Add(this.panelMediaPreview);
            this.Controls.Add(this.toolStripContainerMainForm);
            this.Controls.Add(this.kryptonRibbonMain);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MinimumSize = new System.Drawing.Size(632, 455);
            this.Name = "MainForm";
            this.Text = "PhotoTags Synchronizer";
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.toolStripContainerMainForm.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainerMainForm.BottomToolStripPanel.PerformLayout();
            this.toolStripContainerMainForm.ContentPanel.ResumeLayout(false);
            this.toolStripContainerMainForm.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainerMainForm.TopToolStripPanel.PerformLayout();
            this.toolStripContainerMainForm.ResumeLayout(false);
            this.toolStripContainerMainForm.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelMain)).EndInit();
            this.panelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceMain)).EndInit();
            this.kryptonWorkspaceMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageFolderSearchFilterFolder)).EndInit();
            this.kryptonPageFolderSearchFilterFolder.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCellFolderSearchFilter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageFolderSearchFilterSearch)).EndInit();
            this.kryptonPageFolderSearchFilterSearch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceSearchFilter)).EndInit();
            this.kryptonWorkspaceSearchFilter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageSearchFiler)).EndInit();
            this.kryptonPageSearchFiler.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchFileSystem.Panel)).EndInit();
            this.groupBoxSearchFileSystem.Panel.ResumeLayout(false);
            this.groupBoxSearchFileSystem.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchFileSystem)).EndInit();
            this.groupBoxSearchFileSystem.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchTags.Panel)).EndInit();
            this.groupBoxSearchTags.Panel.ResumeLayout(false);
            this.groupBoxSearchTags.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchTags)).EndInit();
            this.groupBoxSearchTags.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxSearchLocationState)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxSearchAlbum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxSearchComments)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxSearchTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxSearchLocationName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxSearchLocationCity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxSearchDescription)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxSearchLocationCountry)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchPeople.Panel)).EndInit();
            this.groupBoxSearchPeople.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchPeople)).EndInit();
            this.groupBoxSearchPeople.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panel5)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchMediaTaken.Panel)).EndInit();
            this.groupBoxSearchMediaTaken.Panel.ResumeLayout(false);
            this.groupBoxSearchMediaTaken.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchMediaTaken)).EndInit();
            this.groupBoxSearchMediaTaken.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchRating.Panel)).EndInit();
            this.groupBoxSearchRating.Panel.ResumeLayout(false);
            this.groupBoxSearchRating.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchRating)).EndInit();
            this.groupBoxSearchRating.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchKeywords.Panel)).EndInit();
            this.groupBoxSearchKeywords.Panel.ResumeLayout(false);
            this.groupBoxSearchKeywords.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchKeywords)).EndInit();
            this.groupBoxSearchKeywords.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxSearchKeyword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchExtra.Panel)).EndInit();
            this.groupBoxSearchExtra.Panel.ResumeLayout(false);
            this.groupBoxSearchExtra.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchExtra)).EndInit();
            this.groupBoxSearchExtra.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCellSearchFiler)).EndInit();
            this.kryptonWorkspaceCellSearchFiler.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCellSearchFilterAction)).EndInit();
            this.kryptonWorkspaceCellSearchFilterAction.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageSearchFilterAction)).EndInit();
            this.kryptonPageSearchFilterAction.ResumeLayout(false);
            this.kryptonPageSearchFilterAction.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageFolderSearchFilterFilter)).EndInit();
            this.kryptonPageFolderSearchFilterFilter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCellMediaFiles)).EndInit();
            this.kryptonWorkspaceCellMediaFiles.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageMediaFiles)).EndInit();
            this.kryptonPageMediaFiles.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCellToolbox)).EndInit();
            this.kryptonWorkspaceCellToolbox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageToolboxTags)).EndInit();
            this.kryptonPageToolboxTags.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceToolboxTags)).EndInit();
            this.kryptonWorkspaceToolboxTags.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageToolboxTagsDetails)).EndInit();
            this.kryptonPageToolboxTagsDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxTagsDetails.Panel)).EndInit();
            this.kryptonGroupBoxTagsDetails.Panel.ResumeLayout(false);
            this.kryptonGroupBoxTagsDetails.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxTagsDetails)).EndInit();
            this.kryptonGroupBoxTagsDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxAuthor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxAlbum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxDescription)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxComments)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxToolboxTagsTags.Panel)).EndInit();
            this.kryptonGroupBoxToolboxTagsTags.Panel.ResumeLayout(false);
            this.kryptonGroupBoxToolboxTagsTags.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxToolboxTagsTags)).EndInit();
            this.kryptonGroupBoxToolboxTagsTags.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxMediaAiConfidence)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxRating.Panel)).EndInit();
            this.groupBoxRating.Panel.ResumeLayout(false);
            this.groupBoxRating.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxRating)).EndInit();
            this.groupBoxRating.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCellToolboxTagsDetails)).EndInit();
            this.kryptonWorkspaceCellToolboxTagsDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCellToolboxTagsKeywords)).EndInit();
            this.kryptonWorkspaceCellToolboxTagsKeywords.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageToolboxTagsKeywords)).EndInit();
            this.kryptonPageToolboxTagsKeywords.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTagsAndKeywords)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageToolboxPeople)).EndInit();
            this.kryptonPageToolboxPeople.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPeople)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageToolboxMap)).EndInit();
            this.kryptonPageToolboxMap.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceToolboxMap)).EndInit();
            this.kryptonWorkspaceToolboxMap.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageToolboxMapProperties)).EndInit();
            this.kryptonPageToolboxMapProperties.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxGoogleLocationInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxGoogleTimeZoneShift)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCellToolboxMapProperties)).EndInit();
            this.kryptonWorkspaceCellToolboxMapProperties.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCellToolboxMapDetails)).EndInit();
            this.kryptonWorkspaceCellToolboxMapDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageToolboxMapDetails)).EndInit();
            this.kryptonPageToolboxMapDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCellToolboxMapBroswer)).EndInit();
            this.kryptonWorkspaceCellToolboxMapBroswer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageToolboxMapBroswer)).EndInit();
            this.kryptonPageToolboxMapBroswer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelBrowser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCellToolboxMapBroswerProperties)).EndInit();
            this.kryptonWorkspaceCellToolboxMapBroswerProperties.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageToolboxMapBroswerProperties)).EndInit();
            this.kryptonPageToolboxMapBroswerProperties.ResumeLayout(false);
            this.kryptonPageToolboxMapBroswerProperties.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxMapZoomLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageToolboxDates)).EndInit();
            this.kryptonPageToolboxDates.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageToolboxExiftool)).EndInit();
            this.kryptonPageToolboxExiftool.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewExiftool)).EndInit();
            this.contextMenuStripExifTool.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageToolboxWarnings)).EndInit();
            this.kryptonPageToolboxWarnings.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewExiftoolWarning)).EndInit();
            this.contextMenuStripExiftoolWarning.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageToolboxProperties)).EndInit();
            this.kryptonPageToolboxProperties.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageToolboxRename)).EndInit();
            this.kryptonPageToolboxRename.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceToolboxRename)).EndInit();
            this.kryptonWorkspaceToolboxRename.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageToolboxRenameVariables)).EndInit();
            this.kryptonPageToolboxRenameVariables.ResumeLayout(false);
            this.kryptonPageToolboxRenameVariables.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxRenameVariableList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCellToolboxRenameVariables)).EndInit();
            this.kryptonWorkspaceCellToolboxRenameVariables.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCellToolboxRenameResult)).EndInit();
            this.kryptonWorkspaceCellToolboxRenameResult.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageToolboxRenameResult)).EndInit();
            this.kryptonPageToolboxRenameResult.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRename)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageToolboxConvertAndMerge)).EndInit();
            this.kryptonPageToolboxConvertAndMerge.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewConvertAndMerge)).EndInit();
            this.toolStripContainerStripMainForm.ResumeLayout(false);
            this.toolStripContainerStripMainForm.PerformLayout();
            this.contextMenuStripTagsAndKeywords.ResumeLayout(false);
            this.contextMenuStripPeople.ResumeLayout(false);
            this.contextMenuStripMap.ResumeLayout(false);
            this.contextMenuStripDate.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelMediaPreview)).EndInit();
            this.panelMediaPreview.ResumeLayout(false);
            this.toolStripContainerMediaPreview.ContentPanel.ResumeLayout(false);
            this.toolStripContainerMediaPreview.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainerMediaPreview.TopToolStripPanel.PerformLayout();
            this.toolStripContainerMediaPreview.ResumeLayout(false);
            this.toolStripContainerMediaPreview.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.videoView1)).EndInit();
            this.toolStripContainerStripMediaPreview.ResumeLayout(false);
            this.toolStripContainerStripMediaPreview.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonRibbonMain)).EndInit();
            this.contextMenuStripImageListView.ResumeLayout(false);
            this.contextMenuStripTreeViewFolder.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainerMainForm;
        private System.Windows.Forms.ToolStrip toolStripContainerStripMainForm;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusFilesAndSelected;
        private System.Windows.Forms.Timer timerShowErrorMessage;
        private Manina.Windows.Forms.ImageListView imageListView1;
        private Furty.Windows.Forms.FolderTreeView folderTreeViewFolder;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusThreadQueueCount;
        private Krypton.Toolkit.KryptonDataGridView dataGridViewExiftool;
        private Krypton.Toolkit.KryptonDataGridView dataGridViewTagsAndKeywords;
        private Krypton.Toolkit.KryptonLabel label5;
        private Krypton.Toolkit.KryptonGroupBox groupBoxRating;
        private Krypton.Toolkit.KryptonRadioButton radioButtonRating5;
        private Krypton.Toolkit.KryptonRadioButton radioButtonRating4;
        private Krypton.Toolkit.KryptonRadioButton radioButtonRating3;
        private Krypton.Toolkit.KryptonRadioButton radioButtonRating2;
        private Krypton.Toolkit.KryptonRadioButton radioButtonRating1;
        private Krypton.Toolkit.KryptonDataGridView dataGridViewPeople;
        private Krypton.Toolkit.KryptonDataGridView dataGridViewMap;
        private Krypton.Toolkit.KryptonTextBox textBoxBrowserURL;
        private Krypton.Toolkit.KryptonPanel panelBrowser;
        private Krypton.Toolkit.KryptonComboBox comboBoxMapZoomLevel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Krypton.Toolkit.KryptonDataGridView dataGridViewExiftoolWarning;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusAction;
        private System.Windows.Forms.Timer timerShowStatusText_RemoveTimer;
        private Krypton.Toolkit.KryptonDataGridView dataGridViewDate;
        private Krypton.Toolkit.KryptonDataGridView dataGridViewProperties;
        private Krypton.Toolkit.KryptonDataGridView dataGridViewRename;
        private Krypton.Toolkit.KryptonLabel label2;
        private Krypton.Toolkit.KryptonLabel label1;
        private Krypton.Toolkit.KryptonTextBox textBoxRenameNewName;
        private Krypton.Toolkit.KryptonButton buttonRenameSave;
        private Krypton.Toolkit.KryptonButton buttonRenameUpdate;
        private Krypton.Toolkit.KryptonPanel panelMain;
        private System.Windows.Forms.Timer timerStartThread;
        private System.Windows.Forms.Timer timerShowExiftoolSaveProgress;
        private System.Windows.Forms.ImageList imageListFilter;
        private Krypton.Toolkit.KryptonButton buttonSearch;
        private Krypton.Toolkit.KryptonCheckBox checkBoxSerachFitsAllValues;
        private PhotoTagsCommonComponets.TreeViewWithoutDoubleClick treeViewFilter;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBarLazyLoadingDataGridViewProgress;
        private System.Windows.Forms.Timer timerStatusThreadQueue;
        private System.Windows.Forms.Timer timerLazyLoadingDataGridViewProgressRemoveProgessbar;
        private Krypton.Toolkit.KryptonPanel panelMediaPreview;
        private System.Windows.Forms.ToolStrip toolStripContainerStripMediaPreview;
        private System.Windows.Forms.ToolStripButton toolStripButtonMediaPreviewPrevious;
        private System.Windows.Forms.ToolStripButton toolStripButtonMediaPreviewNext;
        private System.Windows.Forms.ToolStripButton toolStripButtonMediaPreviewPlay;
        private System.Windows.Forms.ToolStripButton toolStripButtonMediaPreviewPause;
        private System.Windows.Forms.ToolStripButton toolStripButtonMediaPreviewClose;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonChromecastList;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMediaChromecast;
        private System.Windows.Forms.ToolStripMenuItem tv2ToolStripMenuItem;
        private LibVLCSharp.WinForms.VideoView videoView1;
        private System.Windows.Forms.ToolStripContainer toolStripContainerMediaPreview;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonMediaList;
        private Cyotek.Windows.Forms.ImageBox imageBoxPreview;
        private PhotoTagsCommonComponets.ToolStripTraceBarItem toolStripTraceBarItemMediaPreviewTimer;
        private System.Windows.Forms.ToolStripButton toolStripButtonMediaPreviewFastBackward;
        private System.Windows.Forms.ToolStripButton toolStripButtonMediaPreviewFastForward;
        private System.Windows.Forms.ToolStripLabel toolStripLabelMediaPreviewTimer;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripLabel toolStripLabelMediaPreviewStatus;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private Krypton.Toolkit.KryptonDataGridView dataGridViewConvertAndMerge;
        private System.Windows.Forms.ToolStripButton toolStripButtonMediaPreviewStop;
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
        private System.Windows.Forms.ToolStripButton toolStripButtonSelectPrevious;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonSelectGroupBy;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSelectSameDay;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSelectSameCountry;
        private System.Windows.Forms.ToolStripButton toolStripButtonSelectNext;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSelectSameLocationName;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSelectSameCity;
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
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBarThreadQueue;
        private System.Windows.Forms.ToolStripStatusLabel toolStripLabelLazyLoadingDataGridViewProgress;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelSaveProgress;
        private System.Windows.Forms.ToolStripStatusLabel toolStripLabelThreadQueue;
        private Krypton.Toolkit.KryptonCheckBox checkBoxRenameShowFullPath;
        private Krypton.Toolkit.KryptonComboBox comboBoxGoogleLocationInterval;
        private Krypton.Toolkit.KryptonComboBox comboBoxGoogleTimeZoneShift;
        private Krypton.Toolkit.KryptonComboBox comboBoxMediaAiConfidence;
        private Krypton.Toolkit.KryptonComboBox comboBoxRenameVariableList;
        private Krypton.Toolkit.KryptonManager kryptonManager1;
        private Krypton.Workspace.KryptonWorkspace kryptonWorkspaceMain;
        private Krypton.Navigator.KryptonPage kryptonPageFolderSearchFilterFolder;
        private Krypton.Workspace.KryptonWorkspaceCell kryptonWorkspaceCellFolderSearchFilter;
        private Krypton.Navigator.KryptonPage kryptonPageFolderSearchFilterSearch;
        private Krypton.Navigator.KryptonPage kryptonPageFolderSearchFilterFilter;
        private Krypton.Workspace.KryptonWorkspaceCell kryptonWorkspaceCellMediaFiles;
        private Krypton.Navigator.KryptonPage kryptonPageMediaFiles;
        private Krypton.Workspace.KryptonWorkspaceCell kryptonWorkspaceCellToolbox;
        private Krypton.Navigator.KryptonPage kryptonPageToolboxTags;
        private Krypton.Navigator.KryptonPage kryptonPageToolboxPeople;
        private Krypton.Navigator.KryptonPage kryptonPageToolboxMap;
        private Krypton.Navigator.KryptonPage kryptonPageToolboxDates;
        private Krypton.Navigator.KryptonPage kryptonPageToolboxExiftool;
        private Krypton.Navigator.KryptonPage kryptonPageToolboxWarnings;
        private Krypton.Navigator.KryptonPage kryptonPageToolboxProperties;
        private Krypton.Navigator.KryptonPage kryptonPageToolboxRename;
        private Krypton.Navigator.KryptonPage kryptonPageToolboxConvertAndMerge;
        private Krypton.Navigator.KryptonPage kryptonPage5;
        private Krypton.Workspace.KryptonWorkspace kryptonWorkspaceToolboxTags;
        private Krypton.Navigator.KryptonPage kryptonPageToolboxTagsDetails;
        private Krypton.Workspace.KryptonWorkspaceCell kryptonWorkspaceCellToolboxTagsDetails;
        private Krypton.Workspace.KryptonWorkspaceCell kryptonWorkspaceCellToolboxTagsKeywords;
        private Krypton.Navigator.KryptonPage kryptonPageToolboxTagsKeywords;
        private Krypton.Navigator.KryptonPage kryptonPage7;
        private Krypton.Workspace.KryptonWorkspace kryptonWorkspaceToolboxRename;
        private Krypton.Navigator.KryptonPage kryptonPageToolboxRenameVariables;
        private Krypton.Workspace.KryptonWorkspaceCell kryptonWorkspaceCellToolboxRenameVariables;
        private Krypton.Workspace.KryptonWorkspaceCell kryptonWorkspaceCellToolboxRenameResult;
        private Krypton.Navigator.KryptonPage kryptonPageToolboxRenameResult;
        private Krypton.Navigator.KryptonPage kryptonPage2;
        private Krypton.Navigator.KryptonPage kryptonPage4;
        private Krypton.Workspace.KryptonWorkspace kryptonWorkspaceToolboxMap;
        private Krypton.Navigator.KryptonPage kryptonPageToolboxMapProperties;
        private Krypton.Workspace.KryptonWorkspaceCell kryptonWorkspaceCellToolboxMapProperties;
        private Krypton.Workspace.KryptonWorkspaceCell kryptonWorkspaceCellToolboxMapDetails;
        private Krypton.Navigator.KryptonPage kryptonPageToolboxMapDetails;
        private Krypton.Workspace.KryptonWorkspaceCell kryptonWorkspaceCellToolboxMapBroswer;
        private Krypton.Navigator.KryptonPage kryptonPageToolboxMapBroswer;
        private Krypton.Workspace.KryptonWorkspaceCell kryptonWorkspaceCellToolboxMapBroswerProperties;
        private Krypton.Navigator.KryptonPage kryptonPageToolboxMapBroswerProperties;
        private Krypton.Navigator.KryptonPage kryptonPage3;
        private Krypton.Navigator.KryptonPage kryptonPage10;
        private Krypton.Navigator.KryptonPage kryptonPage12;
        private Krypton.Toolkit.KryptonGroupBox groupBoxSearchPeople;
        private Krypton.Toolkit.KryptonPanel panel5;
        private System.Windows.Forms.CheckedListBox checkedListBoxSearchPeople;
        private Krypton.Toolkit.KryptonCheckBox checkBoxSearchNeedAllNames;
        private Krypton.Toolkit.KryptonCheckBox checkBoxSearchWithoutRegions;
        private Krypton.Toolkit.KryptonGroupBox groupBoxSearchKeywords;
        private Krypton.Toolkit.KryptonLabel label9;
        private Krypton.Toolkit.KryptonLabel label15;
        private Krypton.Toolkit.KryptonCheckBox checkBoxSearchNeedAllKeywords;
        private Krypton.Toolkit.KryptonCheckBox checkBoxSearchWithoutKeyword;
        private Krypton.Toolkit.KryptonComboBox comboBoxSearchKeyword;
        private Krypton.Toolkit.KryptonGroupBox groupBoxSearchExtra;
        private Krypton.Toolkit.KryptonCheckBox checkBoxSearchHasWarning;
        private Krypton.Toolkit.KryptonGroupBox groupBoxSearchTags;
        private Krypton.Toolkit.KryptonCheckBox checkBoxSearchUseAndBetweenTextTagFields;
        private Krypton.Toolkit.KryptonLabel label3;
        private Krypton.Toolkit.KryptonComboBox comboBoxSearchComments;
        private Krypton.Toolkit.KryptonLabel label10;
        private Krypton.Toolkit.KryptonLabel label8;
        private Krypton.Toolkit.KryptonLabel label11;
        private Krypton.Toolkit.KryptonLabel label7;
        private Krypton.Toolkit.KryptonLabel label12;
        private Krypton.Toolkit.KryptonLabel label6;
        private Krypton.Toolkit.KryptonComboBox comboBoxSearchLocationCity;
        private Krypton.Toolkit.KryptonLabel label13;
        private Krypton.Toolkit.KryptonComboBox comboBoxSearchAlbum;
        private Krypton.Toolkit.KryptonComboBox comboBoxSearchLocationState;
        private Krypton.Toolkit.KryptonComboBox comboBoxSearchTitle;
        private Krypton.Toolkit.KryptonComboBox comboBoxSearchLocationName;
        private Krypton.Toolkit.KryptonComboBox comboBoxSearchDescription;
        private Krypton.Toolkit.KryptonComboBox comboBoxSearchLocationCountry;
        private Krypton.Toolkit.KryptonGroupBox groupBoxSearchRating;
        private Krypton.Toolkit.KryptonCheckBox checkBoxSearchRatingEmpty;
        private Krypton.Toolkit.KryptonCheckBox checkBoxSearchRating1;
        private Krypton.Toolkit.KryptonCheckBox checkBoxSearchRating5;
        private Krypton.Toolkit.KryptonCheckBox checkBoxSearchRating0;
        private Krypton.Toolkit.KryptonCheckBox checkBoxSearchRating4;
        private Krypton.Toolkit.KryptonCheckBox checkBoxSearchRating2;
        private Krypton.Toolkit.KryptonCheckBox checkBoxSearchRating3;
        private Krypton.Toolkit.KryptonGroupBox groupBoxSearchMediaTaken;
        private Krypton.Toolkit.KryptonCheckBox checkBoxSearchMediaTakenIsNull;
        private Krypton.Toolkit.KryptonLabel label14;
        private System.Windows.Forms.DateTimePicker dateTimePickerSearchDateFrom;
        private System.Windows.Forms.DateTimePicker dateTimePickerSearchDateTo;
        private Krypton.Toolkit.KryptonLabel label17;
        private Krypton.Workspace.KryptonWorkspace kryptonWorkspaceSearchFilter;
        private Krypton.Navigator.KryptonPage kryptonPageSearchFiler;
        private Krypton.Workspace.KryptonWorkspaceCell kryptonWorkspaceCellSearchFiler;
        private Krypton.Workspace.KryptonWorkspaceCell kryptonWorkspaceCellSearchFilterAction;
        private Krypton.Navigator.KryptonPage kryptonPageSearchFilterAction;
        private Krypton.Navigator.KryptonPage kryptonPage6;
        private Krypton.Navigator.KryptonPage kryptonPage9;
        private Krypton.Toolkit.KryptonGroupBox kryptonGroupBoxToolboxTagsTags;
        private Krypton.Toolkit.KryptonGroupBox kryptonGroupBoxTagsDetails;
        private Krypton.Toolkit.KryptonLabel label4;
        private Krypton.Toolkit.KryptonComboBox comboBoxAuthor;
        private Krypton.Toolkit.KryptonLabel labelComments;
        private Krypton.Toolkit.KryptonComboBox comboBoxComments;
        private Krypton.Toolkit.KryptonLabel labelTitle;
        private Krypton.Toolkit.KryptonLabel labelAuthor;
        private Krypton.Toolkit.KryptonComboBox comboBoxDescription;
        private Krypton.Toolkit.KryptonLabel labelDescription;
        private Krypton.Toolkit.KryptonComboBox comboBoxTitle;
        private Krypton.Toolkit.KryptonComboBox comboBoxAlbum;
        private Krypton.Toolkit.ButtonSpecAny buttonSpecAny1;
        private Krypton.Toolkit.ButtonSpecAny buttonSpecAny2;
        private Krypton.Ribbon.KryptonRibbon kryptonRibbonMain;
        private Krypton.Ribbon.KryptonRibbonQATButton kryptonRibbonQATButtonSave;
        private Krypton.Ribbon.KryptonRibbonTab kryptonRibbonTabView;
        private Krypton.Ribbon.KryptonRibbonGroup kryptonRibbonGroupViewViewModes;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTriple2;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonImageListViewModeGallery;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonImageListViewModeDetails;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonImageListViewModePane;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTriple1;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonImageListViewModeThumbnails;
        private Krypton.Ribbon.KryptonRibbonGroupSeparator kryptonRibbonGroupSeparator4;
        private Krypton.Ribbon.KryptonRibbonGroup kryptonRibbonGroupViewCellSize;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTriple3;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonDataGridViewCellSizeBig;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonDataGridViewCellSizeMedium;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonDataGridViewCellSizeSmall;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTriple4;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonDataGridViewColumnsHistory;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonDataGridViewColumnsErrors;
        private Krypton.Ribbon.KryptonRibbonTab kryptonRibbonTabSelect;
        private Krypton.Ribbon.KryptonRibbonGroup kryptonRibbonGroupImageListViewSelect;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTriple7;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButton20;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButton21;
        private Krypton.Ribbon.KryptonRibbonGroupLines kryptonRibbonGroupLinesSelectByDateInterval;
        private Krypton.Ribbon.KryptonRibbonGroupRadioButton kryptonRibbonGroupRadioButton1;
        private Krypton.Ribbon.KryptonRibbonGroupRadioButton kryptonRibbonGroupRadioButton2;
        private Krypton.Ribbon.KryptonRibbonGroupRadioButton kryptonRibbonGroupRadioButton3;
        private Krypton.Ribbon.KryptonRibbonGroupRadioButton kryptonRibbonGroupRadioButton4;
        private Krypton.Ribbon.KryptonRibbonGroupRadioButton kryptonRibbonGroupRadioButton5;
        private Krypton.Ribbon.KryptonRibbonGroupSeparator kryptonRibbonGroupSeparator2;
        private Krypton.Ribbon.KryptonRibbonGroupLines kryptonRibbonGroupLines2;
        private Krypton.Ribbon.KryptonRibbonGroupRadioButton kryptonRibbonGroupRadioButton6;
        private Krypton.Ribbon.KryptonRibbonGroupRadioButton kryptonRibbonGroupRadioButton7;
        private Krypton.Ribbon.KryptonRibbonGroupRadioButton kryptonRibbonGroupRadioButton8;
        private Krypton.Ribbon.KryptonRibbonGroupRadioButton kryptonRibbonGroupRadioButton9;
        private Krypton.Ribbon.KryptonRibbonTab kryptonRibbonTabTools;
        private Krypton.Ribbon.KryptonRibbonGroup kryptonRibbonGroupToolsMain;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTriple6;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonToolsImportLocations;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonToolsWebScraping;
        private Krypton.Ribbon.KryptonRibbonGroup kryptonRibbonGroup1;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTriple8;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonToolsConfig;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonToolsAbout;
        private Krypton.Ribbon.KryptonRibbonGroup kryptonRibbonGroup2;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTriple5;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonImageListViewDetailviewColumns;
        private Krypton.Ribbon.KryptonRibbonGroup kryptonRibbonGroupViewThumbnailSize;
        private Krypton.Ribbon.KryptonRibbonGroup kryptonRibbonGroupViewColumns;
        private Krypton.Ribbon.KryptonRibbonTab kryptonRibbonTabHome;
        private Krypton.Ribbon.KryptonRibbonGroup kryptonRibbonGroupHomeRotate;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTriple9;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonMediaFileRotate90CCW;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonMediaFileRotate180;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonMediaFileRotate90CW;
        private Krypton.Ribbon.KryptonRibbonGroup kryptonRibbonGroupHomeClipboard;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTriple10;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonHomeCut;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonHomeCopy;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonHomePaste;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTriple11;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonHomeUndo;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonRedo;
        private Krypton.Ribbon.KryptonRibbonGroup kryptonRibbonGroup9;
        private Krypton.Ribbon.KryptonRibbonGroupLines kryptonRibbonGroupLines4;
        private Krypton.Ribbon.KryptonRibbonGroupRadioButton kryptonRibbonGroupRadioButton10;
        private Krypton.Ribbon.KryptonRibbonGroupRadioButton kryptonRibbonGroupRadioButton11;
        private Krypton.Ribbon.KryptonRibbonGroup kryptonRibbonGroup7;
        private Krypton.Ribbon.KryptonRibbonGroup kryptonRibbonGroup8;
        private Krypton.Ribbon.KryptonRibbonGroupLines kryptonRibbonGroupLines3;
        private Krypton.Ribbon.KryptonRibbonGroupCheckBox kryptonRibbonGroupCheckBox1;
        private Krypton.Ribbon.KryptonRibbonGroupCheckBox kryptonRibbonGroupCheckBox2;
        private Krypton.Ribbon.KryptonRibbonGroupCheckBox kryptonRibbonGroupCheckBox3;
        private Krypton.Ribbon.KryptonRibbonGroupCheckBox kryptonRibbonGroupCheckBox4;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTriple12;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonThumbnailSizeXLarge;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonThumbnailSizeLarge;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonThumbnailSizeMedium;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTriple13;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonThumbnailSizeSmall;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonThumbnailSizeXSmall;
        private Krypton.Ribbon.KryptonRibbonGroup kryptonRibbonGroupViewRows;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTriple14;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonDataGridViewRowsFavorite;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonDataGridViewRowsHideEqual;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonImageListViewModeThumbnailRenders;
        private Krypton.Toolkit.KryptonContextMenu kryptonContextMenuImageListViewModeThumbnailRenders;
        private Krypton.Toolkit.KryptonContextMenuItems kryptonContextMenuItems3;
        private Krypton.Toolkit.KryptonContextMenuItems kryptonContextMenuItems1;
        private Krypton.Toolkit.KryptonContextMenuItems kryptonContextMenuItems2;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItem1;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItem2;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItem3;
        private Krypton.Toolkit.KryptonContextMenuHeading kryptonContextMenuHeading1;
        private Krypton.Toolkit.KryptonContextMenuMonthCalendar kryptonContextMenuMonthCalendar1;
        private Krypton.Toolkit.KryptonContextMenuMonthCalendar kryptonContextMenuMonthCalendar2;
        private Krypton.Toolkit.KryptonContextMenuImageSelect kryptonContextMenuImageSelect1;
        private Krypton.Toolkit.KryptonContextMenuItems kryptonContextMenuItems4;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItem4;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTriple15;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonHomeMetadataRefresh;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonHomeMetadataReload;
        private Krypton.Ribbon.KryptonRibbonGroup kryptonRibbonGroupHomeMetadata;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTriple16;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonHomeAutoCorrectRun;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonHomeAutoCorrectForm;
        private Krypton.Ribbon.KryptonRibbonGroup kryptonRibbonGroupHomeManage;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTriple20;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonHomeFind;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonHomeReplace;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTriple19;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonHomeFastCopyNoOverwrite;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonHomeFastCopyOverwrite;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTriple21;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonHomeTagSelectOn;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonHomeTagSelectToggle;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonHomeTagSelectOff;
        private Krypton.Ribbon.KryptonRibbonGroup kryptonRibbonGroup14;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTriple18;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonDataGridViewFavouriteAdd;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonDataGridViewFavouriteToggle;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonDataGridViewFavouriteDelete;
        private Krypton.Ribbon.KryptonRibbonTab kryptonRibbonTabPreview;
        private Krypton.Ribbon.KryptonRibbonGroup kryptonRibbonGroupPreviewPreview;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTriple23;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonPreviewPreview;
        private Krypton.Ribbon.KryptonRibbonGroup kryptonRibbonGroupPreviewNavigate;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTriple22;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButton17;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButton26;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButton27;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTriple24;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButton29;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButton30;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButton31;
        private Krypton.Ribbon.KryptonRibbonGroup kryptonRibbonGroupPreviewRotate;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTriple25;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButton32;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButton33;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButton34;
        private Krypton.Ribbon.KryptonRibbonGroup kryptonRibbonGroupPreviewStatus;
        private Krypton.Ribbon.KryptonRibbonGroupLines kryptonRibbonGroupLines1;
        private Krypton.Ribbon.KryptonRibbonGroupLabel kryptonRibbonGroupLabel1;
        private Krypton.Ribbon.KryptonRibbonGroupLabel kryptonRibbonGroupLabel2;
        private Krypton.Ribbon.KryptonRibbonGroup kryptonRibbonGroupPreviewSlideshow;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTriple26;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButton35;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButton36;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButton37;
        private Krypton.Ribbon.KryptonRibbonGroupLines kryptonRibbonGroupLines5;
        private Krypton.Ribbon.KryptonRibbonGroupTrackBar kryptonRibbonGroupTrackBar1;
        private Krypton.Ribbon.KryptonRibbonGroup kryptonRibbonGroupHomeFileSystem;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTriple27;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonHomeFileSystemDelete;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonHomeFileSystemRename;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTriple28;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonHomeFileSystemOpen;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonHomeFileSystemOpenWith;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonFileSystemOpenAssociateDialog;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonHomeFileSystemRefresh;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonHomeCopyText;
        private Krypton.Toolkit.KryptonContextMenuItems kryptonContextMenuItemsGenericBaseList;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemGenericCut;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemGenericCopy;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemGenericCopyText;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemGenericPaste;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemGenericDelete;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemGenericRename;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemGenericUndo;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemGenericRedo;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemGenericFind;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemGenericReplace;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemGenericSave;
        private Krypton.Toolkit.KryptonContextMenuSeparator kryptonContextMenuSeparatorGenericEndOfFileSystem;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemGenericFavoriteAdd;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemGenericFavoriteDelete;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemGenericFavoriteToggle;
        private Krypton.Toolkit.KryptonContextMenuSeparator kryptonContextMenuSeparatorGenericEndOfFavorite;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemGenericRowShowFavorite;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemGenericRowHideEqual;
        private Krypton.Toolkit.KryptonContextMenuSeparator kryptonContextMenuSeparatorGenericEndOfShowHideRows;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemGenericTriStateOn;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemGenericTriStateOff;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemGenericTriStateToggle;
        private Krypton.Toolkit.KryptonContextMenuSeparator kryptonContextMenuSeparatorGenericEndOfTriState;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemGenericMediaViewAsPoster;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemGenericMediaViewAsFull;
        private Krypton.Toolkit.KryptonContextMenuItems kryptonContextMenuItems5;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItem21;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTriple17;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonHomeFileSystemOpenExplorer;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonFileSystemRunCommand;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonHomeFileSystemEdit;
        public Krypton.Toolkit.KryptonContextMenu kryptonContextMenuGenericBase;
        private Krypton.Toolkit.KryptonContextMenuSeparator kryptonContextMenuSeparatorGenericEndOfClipboard;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemGenericRefreshFolder;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemGenericReadSubfolders;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemGenericOpenFolderLocation;
        private Krypton.Toolkit.KryptonContextMenuItems kryptonContextMenuItems6;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemGenericOpen;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemGenericOpenWith;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemGenericOpenVerbEdit;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemGenericRunCommand;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemGenericAutoCorrectRun;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemGenericAutoCorrectForm;
        private Krypton.Toolkit.KryptonContextMenuSeparator kryptonContextMenuSeparatorGenericEndOfMetadata;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemGenericRotate270;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemGenericRotate180;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemGenericRotate90;
        private Krypton.Toolkit.KryptonContextMenuSeparator kryptonContextMenuSeparatorEndOfRotate;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemGenericMetadataRefreshLast;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemGenericMetadataDeleteHistory;
        private Krypton.Toolkit.KryptonContextMenuItems kryptonContextMenuItemsGenericOpenWithAppList;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItem5;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItem6;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItem7;
        private Krypton.Toolkit.KryptonContextMenuSeparator kryptonContextMenuSeparator8;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItem8;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemsGenericOpenWithAppListExample;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemGenericRegionRename1;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemGenericRegionRename2;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemGenericRegionRename3;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemGenericRegionRenameFromLastUsed;
        private Krypton.Toolkit.KryptonContextMenuItems kryptonContextMenuItemsGenericRegionRenameFromLastUsedList;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemGenericRegionRenameFormLastUsedExample;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemGenericRegionRenameListAll;
        private Krypton.Toolkit.KryptonContextMenuItems kryptonContextMenuItemsGenericRegionRenameListAllList;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemGenericRegionRenameListAllExample;
        private Krypton.Toolkit.KryptonContextMenuSeparator kryptonContextMenuSeparatorGenericEndOfRegionRename;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTagsAndKeywords;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuTagsBrokerCut;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuTagsBrokerCopy;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuTagsBrokerPaste;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuTagsBrokerDelete;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuTags;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuTags;
        private System.Windows.Forms.ToolStripMenuItem findToolStripMenuTag;
        private System.Windows.Forms.ToolStripMenuItem replaceToolStripMenuTag;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuTagsBrokerSave;
        private System.Windows.Forms.ToolStripMenuItem markAsFavoriteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeAsFavoriteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toggleFavoriteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemKeywordsShowFavoriteRows;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemKeywordsHideEqualRows;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTagsBrokerCopyText;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTagsAndKeywordsBrokerOverwriteText;
        private System.Windows.Forms.ToolStripMenuItem toggleTagsAndKeywordsSelectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectTagsAndKeywordsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeTagsAndKeywordsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemShowPosterWindowKeywords;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTagsAndKeywordMediaPreview;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripPeople;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleRenameFromLast1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleRenameFromLast2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleRenameFromLast3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleRenameFromMostUsed;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleRenameFromAll;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemPeopleSelected;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleRenameSelected;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleCut;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleCopy;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeoplePaste;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleDelete;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleUndo;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleRedo;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleFind;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleReplace;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleSave;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleMarkFavorite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleRemoveFavorite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleToggleFavorite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleShowFavorite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleHideEqualRows;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleTogglePeopleTag;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleSelectPeopleTag;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleRemovePeopleTag;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleShowRegionSelector;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleMediaPreview;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripMap;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapCut;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapCopy;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapPaste;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapDelete;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapUndo;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapRedo;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapFind;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapReplace;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapSave;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapMarkFavorite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapRemoveFavorite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapToggleFavorite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapShowFavorite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapHideEqual;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapCopyNotOverwrite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapCopyAndOverwrite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemShowCoordinateOnMap;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemShowCoordinateOnGoogleMap;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapReloadLocationUsingNominatim;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemShowPosterWindowMap;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapMediaPreview;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripDate;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDateCut;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDateCopy;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDatePaste;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDateDelete;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDateUndo;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDateRedo;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDateFind;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDateReplace;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDateSave;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDateMarkFavorite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDateRemoveFavorite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDateToggleFavourite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDateShowFavorite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDateHideEqualRows;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemShowPosterWindowDate;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDateMediaPreview;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripExifTool;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExiftoolAssignCompositeTag;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExiftoolCopy;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExiftoolFind;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExiftoolMarkFavorite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExiftoolRemoveFavorite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExiftoolToggleFavorite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExiftoolSHowFavorite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExiftoolHideEqual;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemShowPosterWindowExiftool;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMediaPreview;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripExiftoolWarning;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemtoolExiftoolWarningAssignCompositeTag;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExiftoolWarningCopy;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExiftoolWarningFind;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExiftoolWarningMarkFavorite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExiftoolWarningRemoveFavorite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExiftoolWarningToggleFavorite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExiftoolWarningShowFavorite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExiftoolWarningHideEqual;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemShowPosterWindowWarnings;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExiftoolWarningMediaPreview;
        private System.Windows.Forms.ToolStripMenuItem sortMediaFileByToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSortByFilename;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSortByFileCreatedDate;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSortByFileModifiedDate;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSortByMediaDateTaken;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSortByMediaAlbum;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSortByMediaTitle;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSortByMediaDescription;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSortByMediaComments;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSortByMediaAuthor;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSortByMediaRating;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSortByLocationName;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSortByLocationRegionState;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSortByLocationCity;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSortByLocationCountry;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemImageListViewCut;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemImageListViewCopy;
        private System.Windows.Forms.ToolStripMenuItem copyFileNamesToClipboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemImageListViewPaste;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemImageListViewDelete;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemImageListViewRefreshFolder;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemImageListViewReloadThumbnailAndMetadata;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemImageListViewReloadThumbnailAndMetadataClearThumbnailAndMetadataHistory;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemImageListViewSelectAll;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemImageListViewAutoCorrect;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemImageListViewAutoCorrectForm;
        private System.Windows.Forms.ToolStripMenuItem openFileWithAssociatedApplicationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openMediaFilesWithToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editFileWithAssociatedApplicationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runSelectedToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem openWithDialogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFileLocationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rotateCW90ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rotate180ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ratateCCW270ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showMediaPosterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mediaPreviewToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripImageListView;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTreeViewFolderCut;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTreeViewFolderCopy;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTreeViewFolderCopyFolderNameToClipboard;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTreeViewFolderPaste;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTreeViewFolderDelete;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTreeViewFolderRefreshFolder;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTreeViewFolderReadSubfolders;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTreeViewFolderReload;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTreeViewFolderClearCache;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTreeViewFolderAutoCorrectMetadata;
        private System.Windows.Forms.ToolStripMenuItem openFolderLocationToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTreeViewFolder;
        private Krypton.Toolkit.KryptonGroupBox groupBoxSearchFileSystem;
        private Krypton.Toolkit.KryptonLabel kryptonLabelSearchFilename;
        private Krypton.Toolkit.KryptonLabel kryptonLabelSearchDirectory;
        private Krypton.Toolkit.KryptonTextBox kryptonTextBoxSearchFilename;
        private Krypton.Toolkit.KryptonTextBox kryptonTextBoxSearchDirectory;
        private Krypton.Toolkit.KryptonCheckBox kryptonCheckBoxSearchUseRegEx;
        private Krypton.Ribbon.KryptonRibbonGroup kryptonRibbonGroupHomeSave;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTriple29;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonHomeSaveSave;
        private Krypton.Ribbon.KryptonRibbonQATButton kryptonRibbonQATButtonViewMediaPoster;
        private Krypton.Ribbon.KryptonRibbonQATButton kryptonRibbonQATButtonViewMediaPreview;
        private Krypton.Ribbon.KryptonRibbonQATButton kryptonRibbonQATButtonMediaPreview;
        private Krypton.Ribbon.KryptonRibbonQATButton kryptonRibbonQATButtonMediaPoster;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemOpenAndAssociateWithDialog;
    }
}

