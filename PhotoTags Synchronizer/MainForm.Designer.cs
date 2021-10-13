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
            this.panelMain = new Krypton.Toolkit.KryptonPanel();
            this.kryptonWorkspaceMain = new Krypton.Workspace.KryptonWorkspace();
            this.kryptonPageFolderSearchFilterFolder = new Krypton.Navigator.KryptonPage();
            this.treeViewFolderBrowser1 = new Raccoom.Windows.Forms.TreeViewFolderBrowser();
            this.kryptonWorkspaceCellFolderSearchFilter = new Krypton.Workspace.KryptonWorkspaceCell();
            this.buttonSpecNavigatorExpandCollapse = new Krypton.Navigator.ButtonSpecNavigator();
            this.kryptonPageFolderSearchFilterSearch = new Krypton.Navigator.KryptonPage();
            this.kryptonWorkspaceSearchFilter = new Krypton.Workspace.KryptonWorkspace();
            this.kryptonPageSearchFiler = new Krypton.Navigator.KryptonPage();
            this.tableLayoutPanelSerachSearch = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxSearchFileSystem = new Krypton.Toolkit.KryptonGroupBox();
            this.tableLayoutPanelSearchFileSystem = new System.Windows.Forms.TableLayoutPanel();
            this.kryptonLabelSearchDirectory = new Krypton.Toolkit.KryptonLabel();
            this.kryptonTextBoxSearchFilename = new Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabelSearchFilename = new Krypton.Toolkit.KryptonLabel();
            this.kryptonTextBoxSearchDirectory = new Krypton.Toolkit.KryptonTextBox();
            this.groupBoxSearchPeople = new Krypton.Toolkit.KryptonGroupBox();
            this.tableLayoutPanelSearchPeople = new System.Windows.Forms.TableLayoutPanel();
            this.checkedListBoxSearchPeople = new Krypton.Toolkit.KryptonCheckedListBox();
            this.checkBoxSearchNeedAllNames = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxSearchWithoutRegions = new Krypton.Toolkit.KryptonCheckBox();
            this.groupBoxSearchTags = new Krypton.Toolkit.KryptonGroupBox();
            this.tableLayoutPanelSearchDetails = new System.Windows.Forms.TableLayoutPanel();
            this.checkBoxSearchUseAndBetweenTextTagFields = new Krypton.Toolkit.KryptonCheckBox();
            this.label3 = new Krypton.Toolkit.KryptonLabel();
            this.comboBoxSearchLocationCountry = new Krypton.Toolkit.KryptonComboBox();
            this.label10 = new Krypton.Toolkit.KryptonLabel();
            this.comboBoxSearchLocationState = new Krypton.Toolkit.KryptonComboBox();
            this.label13 = new Krypton.Toolkit.KryptonLabel();
            this.label11 = new Krypton.Toolkit.KryptonLabel();
            this.comboBoxSearchLocationCity = new Krypton.Toolkit.KryptonComboBox();
            this.comboBoxSearchLocationName = new Krypton.Toolkit.KryptonComboBox();
            this.comboBoxSearchComments = new Krypton.Toolkit.KryptonComboBox();
            this.label6 = new Krypton.Toolkit.KryptonLabel();
            this.label12 = new Krypton.Toolkit.KryptonLabel();
            this.comboBoxSearchTitle = new Krypton.Toolkit.KryptonComboBox();
            this.label7 = new Krypton.Toolkit.KryptonLabel();
            this.comboBoxSearchDescription = new Krypton.Toolkit.KryptonComboBox();
            this.label8 = new Krypton.Toolkit.KryptonLabel();
            this.comboBoxSearchAlbum = new Krypton.Toolkit.KryptonComboBox();
            this.groupBoxSearchKeywords = new Krypton.Toolkit.KryptonGroupBox();
            this.tableLayoutPanelSearchKeywords = new System.Windows.Forms.TableLayoutPanel();
            this.checkBoxSearchWithoutKeyword = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxSearchNeedAllKeywords = new Krypton.Toolkit.KryptonCheckBox();
            this.label9 = new Krypton.Toolkit.KryptonLabel();
            this.label15 = new Krypton.Toolkit.KryptonLabel();
            this.comboBoxSearchKeyword = new Krypton.Toolkit.KryptonComboBox();
            this.groupBoxSearchMediaTaken = new Krypton.Toolkit.KryptonGroupBox();
            this.tableLayoutPanelSearchMediaTaken = new System.Windows.Forms.TableLayoutPanel();
            this.label14 = new Krypton.Toolkit.KryptonLabel();
            this.dateTimePickerSearchDateTo = new Krypton.Toolkit.KryptonDateTimePicker();
            this.dateTimePickerSearchDateFrom = new Krypton.Toolkit.KryptonDateTimePicker();
            this.label17 = new Krypton.Toolkit.KryptonLabel();
            this.checkBoxSearchMediaTakenIsNull = new Krypton.Toolkit.KryptonCheckBox();
            this.groupBoxSearchExtra = new Krypton.Toolkit.KryptonGroupBox();
            this.tableLayoutPanelSearchAttributes = new System.Windows.Forms.TableLayoutPanel();
            this.checkBoxSearchHasWarning = new Krypton.Toolkit.KryptonCheckBox();
            this.groupBoxSearchRating = new Krypton.Toolkit.KryptonGroupBox();
            this.tableLayoutPanelSearchRating = new System.Windows.Forms.TableLayoutPanel();
            this.checkBoxSearchRatingEmpty = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxSearchRating1 = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxSearchRating0 = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxSearchRating5 = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxSearchRating2 = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxSearchRating3 = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxSearchRating4 = new Krypton.Toolkit.KryptonCheckBox();
            this.kryptonLabelEndOfPage = new Krypton.Toolkit.KryptonLabel();
            this.kryptonWorkspaceCellSearchFiler = new Krypton.Workspace.KryptonWorkspaceCell();
            this.kryptonWorkspaceCellSearchFilterAction = new Krypton.Workspace.KryptonWorkspaceCell();
            this.kryptonPageSearchFilterAction = new Krypton.Navigator.KryptonPage();
            this.tableLayoutPanelSerachActions = new System.Windows.Forms.TableLayoutPanel();
            this.buttonSearch = new Krypton.Toolkit.KryptonButton();
            this.kryptonCheckBoxSearchUseRegEx = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxSerachFitsAllValues = new Krypton.Toolkit.KryptonCheckBox();
            this.kryptonPageFolderSearchFilterFilter = new Krypton.Navigator.KryptonPage();
            this.treeViewFilter = new PhotoTagsCommonComponets.TreeViewWithoutDoubleClick();
            this.kryptonWorkspaceCellMediaFiles = new Krypton.Workspace.KryptonWorkspaceCell();
            this.kryptonPageMediaFiles = new Krypton.Navigator.KryptonPage();
            this.imageListView1 = new Manina.Windows.Forms.ImageListView();
            this.kryptonWorkspaceCellToolbox = new Krypton.Workspace.KryptonWorkspaceCell();
            this.buttonSpecNavigatorDataGridViewProgressCircle = new Krypton.Navigator.ButtonSpecNavigator();
            this.kryptonPageToolboxTags = new Krypton.Navigator.KryptonPage();
            this.kryptonWorkspaceToolboxTags = new Krypton.Workspace.KryptonWorkspace();
            this.kryptonPageToolboxTagsDetails = new Krypton.Navigator.KryptonPage();
            this.tableLayoutPanelTags = new System.Windows.Forms.TableLayoutPanel();
            this.kryptonGroupBoxTagsDetails = new Krypton.Toolkit.KryptonGroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelAuthor = new Krypton.Toolkit.KryptonLabel();
            this.labelDescription = new Krypton.Toolkit.KryptonLabel();
            this.labelComments = new Krypton.Toolkit.KryptonLabel();
            this.comboBoxAuthor = new Krypton.Toolkit.KryptonComboBox();
            this.labelTitle = new Krypton.Toolkit.KryptonLabel();
            this.comboBoxComments = new Krypton.Toolkit.KryptonComboBox();
            this.comboBoxDescription = new Krypton.Toolkit.KryptonComboBox();
            this.comboBoxAlbum = new Krypton.Toolkit.KryptonComboBox();
            this.comboBoxTitle = new Krypton.Toolkit.KryptonComboBox();
            this.label4 = new Krypton.Toolkit.KryptonLabel();
            this.kryptonGroupBoxToolboxTagsTags = new Krypton.Toolkit.KryptonGroupBox();
            this.tableLayoutPanelTagConfidence = new System.Windows.Forms.TableLayoutPanel();
            this.label5 = new Krypton.Toolkit.KryptonLabel();
            this.comboBoxMediaAiConfidence = new Krypton.Toolkit.KryptonComboBox();
            this.groupBoxRating = new Krypton.Toolkit.KryptonGroupBox();
            this.tableLayoutPanelTagRationg = new System.Windows.Forms.TableLayoutPanel();
            this.radioButtonRating1 = new Krypton.Toolkit.KryptonRadioButton();
            this.radioButtonRating5 = new Krypton.Toolkit.KryptonRadioButton();
            this.radioButtonRating2 = new Krypton.Toolkit.KryptonRadioButton();
            this.radioButtonRating4 = new Krypton.Toolkit.KryptonRadioButton();
            this.radioButtonRating3 = new Krypton.Toolkit.KryptonRadioButton();
            this.kryptonWorkspaceCellToolboxTagsDetails = new Krypton.Workspace.KryptonWorkspaceCell();
            this.kryptonWorkspaceCellToolboxTagsKeywords = new Krypton.Workspace.KryptonWorkspaceCell();
            this.kryptonPageToolboxTagsKeywords = new Krypton.Navigator.KryptonPage();
            this.dataGridViewTagsAndKeywords = new Krypton.Toolkit.KryptonDataGridView();
            this.kryptonPageToolboxPeople = new Krypton.Navigator.KryptonPage();
            this.dataGridViewPeople = new Krypton.Toolkit.KryptonDataGridView();
            this.kryptonContextMenuGenericBase = new Krypton.Toolkit.KryptonContextMenu();
            this.kryptonContextMenuItemsGenericBaseList = new Krypton.Toolkit.KryptonContextMenuItems();
            this.kryptonContextMenuItemAssignCompositeTag = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItemsAssignCompositeTagList = new Krypton.Toolkit.KryptonContextMenuItems();
            this.kryptonContextMenuItemAssignCompositeTagExample = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItemGenericRegionRename1 = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItemGenericRegionRename2 = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItemGenericRegionRename3 = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItemGenericRegionRenameFromLastUsed = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItemsGenericRegionRenameFromLastUsedList = new Krypton.Toolkit.KryptonContextMenuItems();
            this.kryptonContextMenuItemGenericRegionRenameFormLastUsedExample = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItemGenericRegionRenameListAll = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuImageSelect3 = new Krypton.Toolkit.KryptonContextMenuImageSelect();
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
            this.tableLayoutPanelMap = new System.Windows.Forms.TableLayoutPanel();
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
            this.kryptonPageToolboxWarnings = new Krypton.Navigator.KryptonPage();
            this.dataGridViewExiftoolWarning = new Krypton.Toolkit.KryptonDataGridView();
            this.kryptonPageToolboxProperties = new Krypton.Navigator.KryptonPage();
            this.dataGridViewProperties = new Krypton.Toolkit.KryptonDataGridView();
            this.kryptonPageToolboxRename = new Krypton.Navigator.KryptonPage();
            this.kryptonWorkspaceToolboxRename = new Krypton.Workspace.KryptonWorkspace();
            this.kryptonPageToolboxRenameVariables = new Krypton.Navigator.KryptonPage();
            this.tableLayoutPanelRename = new System.Windows.Forms.TableLayoutPanel();
            this.buttonRenameSave = new Krypton.Toolkit.KryptonButton();
            this.buttonRenameUpdate = new Krypton.Toolkit.KryptonButton();
            this.label2 = new Krypton.Toolkit.KryptonLabel();
            this.checkBoxRenameShowFullPath = new Krypton.Toolkit.KryptonCheckBox();
            this.comboBoxRenameVariableList = new Krypton.Toolkit.KryptonComboBox();
            this.label1 = new Krypton.Toolkit.KryptonLabel();
            this.textBoxRenameNewName = new Krypton.Toolkit.KryptonTextBox();
            this.kryptonWorkspaceCellToolboxRenameVariables = new Krypton.Workspace.KryptonWorkspaceCell();
            this.kryptonWorkspaceCellToolboxRenameResult = new Krypton.Workspace.KryptonWorkspaceCell();
            this.kryptonPageToolboxRenameResult = new Krypton.Navigator.KryptonPage();
            this.dataGridViewRename = new Krypton.Toolkit.KryptonDataGridView();
            this.kryptonPageToolboxConvertAndMerge = new Krypton.Navigator.KryptonPage();
            this.dataGridViewConvertAndMerge = new Krypton.Toolkit.KryptonDataGridView();
            this.timerShowErrorMessage = new System.Windows.Forms.Timer(this.components);
            this.timerShowStatusText_RemoveTimer = new System.Windows.Forms.Timer(this.components);
            this.timerStartThread = new System.Windows.Forms.Timer(this.components);
            this.timerShowExiftoolSaveProgress = new System.Windows.Forms.Timer(this.components);
            this.timerStatusThreadQueue = new System.Windows.Forms.Timer(this.components);
            this.panelMediaPreview = new Krypton.Toolkit.KryptonPanel();
            this.imageBoxPreview = new Cyotek.Windows.Forms.ImageBox();
            this.videoView1 = new LibVLCSharp.WinForms.VideoView();
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
            this.kryptonRibbonQATButtonSelectPrevius = new Krypton.Ribbon.KryptonRibbonQATButton();
            this.kryptonRibbonQATButtonSelectNext = new Krypton.Ribbon.KryptonRibbonQATButton();
            this.kryptonRibbonQATButtonSelectEqual = new Krypton.Ribbon.KryptonRibbonQATButton();
            this.kryptonRibbonQATButtonSelectAll = new Krypton.Ribbon.KryptonRibbonQATButton();
            this.kryptonRibbonQATButtonSelectNone = new Krypton.Ribbon.KryptonRibbonQATButton();
            this.kryptonRibbonQATButtonSelectToggle = new Krypton.Ribbon.KryptonRibbonQATButton();
            this.kryptonRibbonQATButtonMediaPlayerPrevious = new Krypton.Ribbon.KryptonRibbonQATButton();
            this.kryptonRibbonQATButtonMediaPlayerNext = new Krypton.Ribbon.KryptonRibbonQATButton();
            this.kryptonRibbonQATButtonMediaPlayerPlay = new Krypton.Ribbon.KryptonRibbonQATButton();
            this.kryptonRibbonQATButtonMediaPlayerPause = new Krypton.Ribbon.KryptonRibbonQATButton();
            this.kryptonRibbonQATButtonMediaPlayerStop = new Krypton.Ribbon.KryptonRibbonQATButton();
            this.kryptonRibbonQATButtonMediaPlayerFastBackwards = new Krypton.Ribbon.KryptonRibbonQATButton();
            this.kryptonRibbonQATButtonMediaPlayerFastForward = new Krypton.Ribbon.KryptonRibbonQATButton();
            this.kryptonRibbonQATButtonMediaPlayerSlideshowPlay = new Krypton.Ribbon.KryptonRibbonQATButton();
            this.kryptonRibbonTabHome = new Krypton.Ribbon.KryptonRibbonTab();
            this.kryptonRibbonGroupHomeClipboard = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupTripleHomeCopyCutPaste = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonHomeCopy = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonHomeCut = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonHomePaste = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupTripleHomeUndoRedo = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonHomeUndo = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonRedo = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupHomeManage = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupTripleHomeCopy = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonHomeCopyText = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonHomeFastCopyNoOverwrite = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonHomeFastCopyOverwrite = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupTripleHomeSortFindAndSearch = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonHomeFind = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonHomeReplace = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonHomeSortColumn = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupHomeFileSystem = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupTripleHomeDeleteRenameRefresh = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonHomeFileSystemDelete = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonHomeFileSystemRename = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonHomeFileSystemRefresh = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupTripleHomeOpenWith = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonHomeFileSystemOpen = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonHomeFileSystemOpenWith = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonFileSystemOpenAssociateDialog = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupTripleHomeRunEdit = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonHomeFileSystemOpenExplorer = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonFileSystemRunCommand = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonHomeFileSystemEdit = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupHomeRotate = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupTripleHomeRotate = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonMediaFileRotate90CCW = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonMediaFileRotate180 = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonMediaFileRotate90CW = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupHomeMetadata = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupTripleHomeAutoCorrect = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonHomeAutoCorrectRun = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonHomeAutoCorrectForm = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupTripleHomeMetadataRefresh = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonHomeMetadataRefresh = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonHomeMetadataReload = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupTripleHomeTriState = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonHomeTagSelectOn = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonHomeTagSelectToggle = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonHomeTagSelectOff = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupHomeSave = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupTripleHomeSave = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonHomeSaveSave = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonTabView = new Krypton.Ribbon.KryptonRibbonTab();
            this.kryptonRibbonGroupViewViewModes = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupTripleViewGalleryDetailsPane = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonImageListViewModeGallery = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonImageListViewModeDetails = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonImageListViewModePane = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupTripleViewThumbnailsMode = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonImageListViewModeThumbnails = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonImageListViewModeThumbnailRenders = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupSeparator4 = new Krypton.Ribbon.KryptonRibbonGroupSeparator();
            this.kryptonRibbonGroupTripleViewColumnsSort = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonImageListViewDetailviewColumns = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupViewThumbnailSize = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupTripleViewTumbnailSizeLargeMedium = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonThumbnailSizeXLarge = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonThumbnailSizeLarge = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonThumbnailSizeMedium = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupTripleViewThumbnailSizeXSmall = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonThumbnailSizeSmall = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonThumbnailSizeXSmall = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupViewCellSize = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupTripleViewGridSize = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonDataGridViewCellSizeBig = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonDataGridViewCellSizeMedium = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonDataGridViewCellSizeSmall = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupViewColumns = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupTripleViewShowHideColumns = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonDataGridViewColumnsHistory = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonDataGridViewColumnsErrors = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupViewRows = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupTripleViewShowHideRows = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonDataGridViewRowsFavorite = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonDataGridViewRowsHideEqual = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroup14 = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupTriple18 = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonDataGridViewFavouriteAdd = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonDataGridViewFavouriteToggle = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonDataGridViewFavouriteDelete = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonTabSelect = new Krypton.Ribbon.KryptonRibbonTab();
            this.kryptonRibbonGroupImageListViewSelect = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupTripleSelectForwardBackwards = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonSelectBackwards = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonSelectForwards = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonSelectEqual = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupSeparator2 = new Krypton.Ribbon.KryptonRibbonGroupSeparator();
            this.kryptonRibbonGroupTripleSelectAllNoneToggle = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonSelectAll = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonSelectNone = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonSelectToggle = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroup2 = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupLinesLocatonDatePriorities = new Krypton.Ribbon.KryptonRibbonGroupLines();
            this.kryptonRibbonGroupCheckBoxSelectFileCreated = new Krypton.Ribbon.KryptonRibbonGroupCheckBox();
            this.kryptonRibbonGroupCheckBoxSelectMediaTaken = new Krypton.Ribbon.KryptonRibbonGroupCheckBox();
            this.kryptonRibbonGroupCheckBoxSelectCheckAllDates = new Krypton.Ribbon.KryptonRibbonGroupCheckBox();
            this.kryptonRibbonGroupLinesSelectByDateInterval = new Krypton.Ribbon.KryptonRibbonGroupLines();
            this.kryptonRibbonGroupRadioButtonSelectDateRange1 = new Krypton.Ribbon.KryptonRibbonGroupRadioButton();
            this.kryptonRibbonGroupRadioButtonSelectDateRange3 = new Krypton.Ribbon.KryptonRibbonGroupRadioButton();
            this.kryptonRibbonGroupRadioButtonSelectDateRange7 = new Krypton.Ribbon.KryptonRibbonGroupRadioButton();
            this.kryptonRibbonGroupRadioButtonSelectDateRange14 = new Krypton.Ribbon.KryptonRibbonGroupRadioButton();
            this.kryptonRibbonGroupRadioButtonSelectDateRange30 = new Krypton.Ribbon.KryptonRibbonGroupRadioButton();
            this.kryptonRibbonGroup8 = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupLinesSelectLocation = new Krypton.Ribbon.KryptonRibbonGroupLines();
            this.kryptonRibbonGroupCheckBoxSelectLocationName = new Krypton.Ribbon.KryptonRibbonGroupCheckBox();
            this.kryptonRibbonGroupCheckBoxSelectLocationCity = new Krypton.Ribbon.KryptonRibbonGroupCheckBox();
            this.kryptonRibbonGroupCheckBoxSelectLocationStateRegion = new Krypton.Ribbon.KryptonRibbonGroupCheckBox();
            this.kryptonRibbonGroupCheckBoxSelectLocationCountry = new Krypton.Ribbon.KryptonRibbonGroupCheckBox();
            this.kryptonRibbonGroupCheckBoxSelectCheckAllLocations = new Krypton.Ribbon.KryptonRibbonGroupCheckBox();
            this.kryptonRibbonGroup7 = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupLinesLocationAmount = new Krypton.Ribbon.KryptonRibbonGroupLines();
            this.kryptonRibbonGroupRadioButtonSelectMediaAmount10 = new Krypton.Ribbon.KryptonRibbonGroupRadioButton();
            this.kryptonRibbonGroupRadioButtonSelectMediaAmount30 = new Krypton.Ribbon.KryptonRibbonGroupRadioButton();
            this.kryptonRibbonGroupRadioButtonSelectMediaAmount50 = new Krypton.Ribbon.KryptonRibbonGroupRadioButton();
            this.kryptonRibbonGroupRadioButtonSelectMediaAmount100 = new Krypton.Ribbon.KryptonRibbonGroupRadioButton();
            this.kryptonRibbonTabTools = new Krypton.Ribbon.KryptonRibbonTab();
            this.kryptonRibbonGroupToolsMain = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupTriple6 = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonToolsImportLocations = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonToolsWebScraping = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroup1 = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupTriple8 = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonToolsConfig = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonToolsAbout = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupToolsProgressStatus = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupTripleProgressStatusSave = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupLabelToolsProgressSave = new Krypton.Ribbon.KryptonRibbonGroupLabel();
            this.kryptonRibbonGroupCustomControlToolsProgressSave = new Krypton.Ribbon.KryptonRibbonGroupCustomControl();
            this.kryptonRibbonGroupLabelToolsProgressSaveText = new Krypton.Ribbon.KryptonRibbonGroupLabel();
            this.kryptonRibbonGroupTripleToolsProgressStatusWork = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupLabelToolsProgressLazyloading = new Krypton.Ribbon.KryptonRibbonGroupLabel();
            this.kryptonRibbonGroupCustomControlToolsProgressLazyloading = new Krypton.Ribbon.KryptonRibbonGroupCustomControl();
            this.kryptonRibbonGroupTripleProgressStatusBackground = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupLabelToolsProgressBackground = new Krypton.Ribbon.KryptonRibbonGroupLabel();
            this.kryptonRibbonGroupCustomControlToolsProgressBackground = new Krypton.Ribbon.KryptonRibbonGroupCustomControl();
            this.kryptonRibbonGroupLabelToolsProgressBackgroundBackgroundProcessText = new Krypton.Ribbon.KryptonRibbonGroupLabel();
            this.kryptonRibbonGroupTriple1 = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupLabelCurrentActionsHeading = new Krypton.Ribbon.KryptonRibbonGroupLabel();
            this.kryptonRibbonGroupLabelToolsCurrentActions = new Krypton.Ribbon.KryptonRibbonGroupLabel();
            this.kryptonRibbonGroupButtonToolsProgressLazyloadingShowStatus = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonTabPreview = new Krypton.Ribbon.KryptonRibbonTab();
            this.kryptonRibbonGroupPreviewPreview = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupTriplePreviewPreview = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonPreviewPreview = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupPreviewNavigate = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupTriplePreviewSkipPrevNext = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonPreviewSkipPrev = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonPreviewSkipNext = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupTriplePreviewPlayPause = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonPreviewPlay = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonPreviewPause = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupTriplePreviewRewindForwardStop = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonPreviewRewind = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonPreviewForward = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonPreviewStop = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupLinesPreviewTimer = new Krypton.Ribbon.KryptonRibbonGroupLines();
            this.kryptonRibbonGroupTrackBarPreviewTimer = new Krypton.Ribbon.KryptonRibbonGroupTrackBar();
            this.kryptonRibbonGroupPreviewRotate = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupTriplePreviewRotate = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonPreviewRotate270 = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonPreviewRotate180 = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonPreviewRotate90 = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupPreviewSlideshow = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupTriple2 = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonPreviewSlideshowPlay = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupTriplePreviewSlideshow = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupButtonPreviewSlideshow = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonPreviewSlideshowTimeInterval = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupButtonPreviewChromecast = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupPreviewStatus = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupLinesPreviewStatus = new Krypton.Ribbon.KryptonRibbonGroupLines();
            this.kryptonRibbonGroupLabelPreviewTimer = new Krypton.Ribbon.KryptonRibbonGroupLabel();
            this.kryptonRibbonGroupLabelPreviewStatus = new Krypton.Ribbon.KryptonRibbonGroupLabel();
            this.kryptonContextMenuFileSystemColumnSort = new Krypton.Toolkit.KryptonContextMenu();
            this.kryptonContextMenuRadioButtonFileSystemColumnSortFilename = new Krypton.Toolkit.KryptonContextMenuRadioButton();
            this.kryptonContextMenuRadioButtonFileSystemColumnSortFileCreateDate = new Krypton.Toolkit.KryptonContextMenuRadioButton();
            this.kryptonContextMenuRadioButtonFileSystemColumnSortFileModifiedDate = new Krypton.Toolkit.KryptonContextMenuRadioButton();
            this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaDateTaken = new Krypton.Toolkit.KryptonContextMenuRadioButton();
            this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaAlbum = new Krypton.Toolkit.KryptonContextMenuRadioButton();
            this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaTitle = new Krypton.Toolkit.KryptonContextMenuRadioButton();
            this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaDescription = new Krypton.Toolkit.KryptonContextMenuRadioButton();
            this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaComments = new Krypton.Toolkit.KryptonContextMenuRadioButton();
            this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaAuthor = new Krypton.Toolkit.KryptonContextMenuRadioButton();
            this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaRating = new Krypton.Toolkit.KryptonContextMenuRadioButton();
            this.kryptonContextMenuRadioButtonFileSystemColumnSortLocationName = new Krypton.Toolkit.KryptonContextMenuRadioButton();
            this.kryptonContextMenuRadioButtonFileSystemColumnSortLocationRegionState = new Krypton.Toolkit.KryptonContextMenuRadioButton();
            this.kryptonContextMenuRadioButtonFileSystemColumnSortLocationCity = new Krypton.Toolkit.KryptonContextMenuRadioButton();
            this.kryptonContextMenuRadioButtonFileSystemColumnSortLocationCountry = new Krypton.Toolkit.KryptonContextMenuRadioButton();
            this.kryptonContextMenuItemsCloseMenuList = new Krypton.Toolkit.KryptonContextMenuItems();
            this.kryptonContextMenuItemCloseMenu = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuImageListViewModeThumbnailRenders = new Krypton.Toolkit.KryptonContextMenu();
            this.kryptonContextMenuItems3 = new Krypton.Toolkit.KryptonContextMenuItems();
            this.kryptonContextMenuPreviewSlideshowInterval = new Krypton.Toolkit.KryptonContextMenu();
            this.kryptonContextMenuRadioButtonSlideshow2sec = new Krypton.Toolkit.KryptonContextMenuRadioButton();
            this.kryptonContextMenuRadioButtonSlideshow4sec = new Krypton.Toolkit.KryptonContextMenuRadioButton();
            this.kryptonContextMenuRadioButtonSlideshow6sec = new Krypton.Toolkit.KryptonContextMenuRadioButton();
            this.kryptonContextMenuRadioButtonSlideshow8sec = new Krypton.Toolkit.KryptonContextMenuRadioButton();
            this.kryptonContextMenuRadioButtonSlideshow10sec = new Krypton.Toolkit.KryptonContextMenuRadioButton();
            this.kryptonContextMenuItemsPreviewSlideshowIntervalList = new Krypton.Toolkit.KryptonContextMenuItems();
            this.kryptonContextMenuItemPreviewSlideshowIntervalStop = new Krypton.Toolkit.KryptonContextMenuItem();
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
            this.kryptonRibbonQATButtonViewMediaPoster = new Krypton.Ribbon.KryptonRibbonQATButton();
            this.kryptonRibbonQATButtonViewMediaPreview = new Krypton.Ribbon.KryptonRibbonQATButton();
            this.kryptonContextMenuRadioButton1 = new Krypton.Toolkit.KryptonContextMenuRadioButton();
            this.kryptonContextMenuRadioButton2 = new Krypton.Toolkit.KryptonContextMenuRadioButton();
            this.kryptonContextMenuRadioButton3 = new Krypton.Toolkit.KryptonContextMenuRadioButton();
            this.kryptonContextMenuRadioButton4 = new Krypton.Toolkit.KryptonContextMenuRadioButton();
            this.kryptonContextMenuRadioButton5 = new Krypton.Toolkit.KryptonContextMenuRadioButton();
            this.kryptonContextMenuRadioButton6 = new Krypton.Toolkit.KryptonContextMenuRadioButton();
            this.kryptonContextMenuItem9 = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuMonthCalendar3 = new Krypton.Toolkit.KryptonContextMenuMonthCalendar();
            this.kryptonContextMenuMonthCalendar4 = new Krypton.Toolkit.KryptonContextMenuMonthCalendar();
            this.kryptonContextMenuImageSelect2 = new Krypton.Toolkit.KryptonContextMenuImageSelect();
            this.kryptonContextMenuColorColumns1 = new Krypton.Toolkit.KryptonContextMenuColorColumns();
            this.kryptonContextMenuLinkLabel1 = new Krypton.Toolkit.KryptonContextMenuLinkLabel();
            this.kryptonContextMenuRadioButton7 = new Krypton.Toolkit.KryptonContextMenuRadioButton();
            this.kryptonContextMenuCheckButton1 = new Krypton.Toolkit.KryptonContextMenuCheckButton();
            this.kryptonContextMenuCheckBox1 = new Krypton.Toolkit.KryptonContextMenuCheckBox();
            this.kryptonContextMenuMonthCalendar5 = new Krypton.Toolkit.KryptonContextMenuMonthCalendar();
            this.kryptonContextMenuColorColumns2 = new Krypton.Toolkit.KryptonContextMenuColorColumns();
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
            this.tableLayoutPanelSerachSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchFileSystem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchFileSystem.Panel)).BeginInit();
            this.groupBoxSearchFileSystem.Panel.SuspendLayout();
            this.groupBoxSearchFileSystem.SuspendLayout();
            this.tableLayoutPanelSearchFileSystem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchPeople)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchPeople.Panel)).BeginInit();
            this.groupBoxSearchPeople.Panel.SuspendLayout();
            this.groupBoxSearchPeople.SuspendLayout();
            this.tableLayoutPanelSearchPeople.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchTags)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchTags.Panel)).BeginInit();
            this.groupBoxSearchTags.Panel.SuspendLayout();
            this.groupBoxSearchTags.SuspendLayout();
            this.tableLayoutPanelSearchDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxSearchLocationCountry)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxSearchLocationState)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxSearchLocationCity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxSearchLocationName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxSearchComments)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxSearchTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxSearchDescription)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxSearchAlbum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchKeywords)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchKeywords.Panel)).BeginInit();
            this.groupBoxSearchKeywords.Panel.SuspendLayout();
            this.groupBoxSearchKeywords.SuspendLayout();
            this.tableLayoutPanelSearchKeywords.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxSearchKeyword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchMediaTaken)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchMediaTaken.Panel)).BeginInit();
            this.groupBoxSearchMediaTaken.Panel.SuspendLayout();
            this.groupBoxSearchMediaTaken.SuspendLayout();
            this.tableLayoutPanelSearchMediaTaken.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchExtra)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchExtra.Panel)).BeginInit();
            this.groupBoxSearchExtra.Panel.SuspendLayout();
            this.groupBoxSearchExtra.SuspendLayout();
            this.tableLayoutPanelSearchAttributes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchRating)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchRating.Panel)).BeginInit();
            this.groupBoxSearchRating.Panel.SuspendLayout();
            this.groupBoxSearchRating.SuspendLayout();
            this.tableLayoutPanelSearchRating.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCellSearchFiler)).BeginInit();
            this.kryptonWorkspaceCellSearchFiler.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCellSearchFilterAction)).BeginInit();
            this.kryptonWorkspaceCellSearchFilterAction.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageSearchFilterAction)).BeginInit();
            this.kryptonPageSearchFilterAction.SuspendLayout();
            this.tableLayoutPanelSerachActions.SuspendLayout();
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
            this.tableLayoutPanelTags.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxTagsDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxTagsDetails.Panel)).BeginInit();
            this.kryptonGroupBoxTagsDetails.Panel.SuspendLayout();
            this.kryptonGroupBoxTagsDetails.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxAuthor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxComments)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxDescription)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxAlbum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxToolboxTagsTags)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxToolboxTagsTags.Panel)).BeginInit();
            this.kryptonGroupBoxToolboxTagsTags.Panel.SuspendLayout();
            this.kryptonGroupBoxToolboxTagsTags.SuspendLayout();
            this.tableLayoutPanelTagConfidence.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxMediaAiConfidence)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxRating)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxRating.Panel)).BeginInit();
            this.groupBoxRating.Panel.SuspendLayout();
            this.groupBoxRating.SuspendLayout();
            this.tableLayoutPanelTagRationg.SuspendLayout();
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
            this.tableLayoutPanelMap.SuspendLayout();
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
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageToolboxWarnings)).BeginInit();
            this.kryptonPageToolboxWarnings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewExiftoolWarning)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageToolboxProperties)).BeginInit();
            this.kryptonPageToolboxProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageToolboxRename)).BeginInit();
            this.kryptonPageToolboxRename.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceToolboxRename)).BeginInit();
            this.kryptonWorkspaceToolboxRename.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageToolboxRenameVariables)).BeginInit();
            this.kryptonPageToolboxRenameVariables.SuspendLayout();
            this.tableLayoutPanelRename.SuspendLayout();
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
            ((System.ComponentModel.ISupportInitialize)(this.panelMediaPreview)).BeginInit();
            this.panelMediaPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.videoView1)).BeginInit();
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
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.kryptonWorkspaceMain);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 115);
            this.panelMain.Margin = new System.Windows.Forms.Padding(0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1214, 779);
            this.panelMain.TabIndex = 2;
            // 
            // kryptonWorkspaceMain
            // 
            this.kryptonWorkspaceMain.ActivePage = this.kryptonPageFolderSearchFilterFolder;
            this.kryptonWorkspaceMain.AllowPageDrag = false;
            this.kryptonWorkspaceMain.ContextMenus.ShowContextMenu = false;
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
            this.kryptonWorkspaceMain.Size = new System.Drawing.Size(1214, 779);
            this.kryptonWorkspaceMain.TabIndex = 0;
            this.kryptonWorkspaceMain.TabStop = true;
            // 
            // kryptonPageFolderSearchFilterFolder
            // 
            this.kryptonPageFolderSearchFilterFolder.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageFolderSearchFilterFolder.Controls.Add(this.treeViewFolderBrowser1);
            this.kryptonPageFolderSearchFilterFolder.Flags = 65534;
            this.kryptonPageFolderSearchFilterFolder.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.Search_FolderTree48x48;
            this.kryptonPageFolderSearchFilterFolder.ImageMedium = global::PhotoTagsSynchronizer.Properties.Resources.Search_FolderTree24x24;
            this.kryptonPageFolderSearchFilterFolder.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.Search_FolderTree16x16;
            this.kryptonPageFolderSearchFilterFolder.LastVisibleSet = true;
            this.kryptonPageFolderSearchFilterFolder.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageFolderSearchFilterFolder.Name = "kryptonPageFolderSearchFilterFolder";
            this.kryptonPageFolderSearchFilterFolder.Size = new System.Drawing.Size(399, 660);
            this.kryptonPageFolderSearchFilterFolder.Text = "Folder";
            this.kryptonPageFolderSearchFilterFolder.TextDescription = "Browse folders on your device";
            this.kryptonPageFolderSearchFilterFolder.TextTitle = "Folder";
            this.kryptonPageFolderSearchFilterFolder.ToolTipTitle = "Browse folders on your device";
            this.kryptonPageFolderSearchFilterFolder.UniqueName = "70c41531c9904af0b0213b722bb7750d";
            this.kryptonPageFolderSearchFilterFolder.Enter += new System.EventHandler(this.kryptonPageFolderSearchFilterFolder_Enter);
            // 
            // treeViewFolderBrowser1
            // 
            this.treeViewFolderBrowser1.AllowDrop = true;
            this.treeViewFolderBrowser1.CheckBoxBehaviorMode = Raccoom.Windows.Forms.CheckBoxBehaviorMode.None;
            this.treeViewFolderBrowser1.DataSource = null;
            this.treeViewFolderBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewFolderBrowser1.HideSelection = false;
            this.treeViewFolderBrowser1.KryptonContextMenu = this.kryptonContextMenuGenericBase;
            this.treeViewFolderBrowser1.Location = new System.Drawing.Point(0, 0);
            this.treeViewFolderBrowser1.Name = "treeViewFolderBrowser1";
            this.treeViewFolderBrowser1.ShowLines = false;
            this.treeViewFolderBrowser1.ShowRootLines = false;
            this.treeViewFolderBrowser1.Size = new System.Drawing.Size(399, 660);
            this.treeViewFolderBrowser1.TabIndex = 1;
            this.treeViewFolderBrowser1.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeViewFolderBrowser1_AfterLabelEdit);
            this.treeViewFolderBrowser1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewFolderBrowser1_AfterSelect);
            this.treeViewFolderBrowser1.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeViewFolderBrowser1_BeforeLabelEdit);
            this.treeViewFolderBrowser1.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeViewFolderBrowser1_ItemDrag);
            this.treeViewFolderBrowser1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewFolderBrowser1_NodeMouseClick);
            this.treeViewFolderBrowser1.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeViewFolderBrowser1_DragDrop);
            this.treeViewFolderBrowser1.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeViewFolderBrowser1_DragEnter);
            this.treeViewFolderBrowser1.DragOver += new System.Windows.Forms.DragEventHandler(this.treeViewFolderBrowser1_DragOver);
            this.treeViewFolderBrowser1.DragLeave += new System.EventHandler(this.treeViewFolderBrowser1_DragLeave);
            // 
            // kryptonWorkspaceCellFolderSearchFilter
            // 
            this.kryptonWorkspaceCellFolderSearchFilter.AllowDroppingPages = false;
            this.kryptonWorkspaceCellFolderSearchFilter.AllowPageDrag = true;
            this.kryptonWorkspaceCellFolderSearchFilter.AllowPageReorder = false;
            this.kryptonWorkspaceCellFolderSearchFilter.AllowTabFocus = false;
            this.kryptonWorkspaceCellFolderSearchFilter.Button.ButtonSpecs.AddRange(new Krypton.Navigator.ButtonSpecNavigator[] {
            this.buttonSpecNavigatorExpandCollapse});
            this.kryptonWorkspaceCellFolderSearchFilter.Button.CloseButtonAction = Krypton.Navigator.CloseButtonAction.None;
            this.kryptonWorkspaceCellFolderSearchFilter.Button.CloseButtonDisplay = Krypton.Navigator.ButtonDisplay.Hide;
            this.kryptonWorkspaceCellFolderSearchFilter.Button.ContextButtonAction = Krypton.Navigator.ContextButtonAction.None;
            this.kryptonWorkspaceCellFolderSearchFilter.Button.ContextButtonDisplay = Krypton.Navigator.ButtonDisplay.Hide;
            this.kryptonWorkspaceCellFolderSearchFilter.Name = "kryptonWorkspaceCellFolderSearchFilter";
            this.kryptonWorkspaceCellFolderSearchFilter.NavigatorMode = Krypton.Navigator.NavigatorMode.StackCheckButtonHeaderGroup;
            this.kryptonWorkspaceCellFolderSearchFilter.Pages.AddRange(new Krypton.Navigator.KryptonPage[] {
            this.kryptonPageFolderSearchFilterFolder,
            this.kryptonPageFolderSearchFilterSearch,
            this.kryptonPageFolderSearchFilterFilter});
            this.kryptonWorkspaceCellFolderSearchFilter.SelectedIndex = 0;
            this.kryptonWorkspaceCellFolderSearchFilter.UniqueName = "7f1f5ae72b174949ac870f12642643a5";
            this.kryptonWorkspaceCellFolderSearchFilter.MaximizeRestoreClicked += new System.EventHandler(this.kryptonWorkspaceCellFolderSearchFilter_MaximizeRestoreClicked);
            this.kryptonWorkspaceCellFolderSearchFilter.DisplayPopupPage += new System.EventHandler<Krypton.Navigator.PopupPageEventArgs>(this.kryptonWorkspaceCellFolderSearchFilter_DisplayPopupPage);
            // 
            // buttonSpecNavigatorExpandCollapse
            // 
            this.buttonSpecNavigatorExpandCollapse.Type = Krypton.Toolkit.PaletteButtonSpecStyle.ArrowLeft;
            this.buttonSpecNavigatorExpandCollapse.TypeRestricted = Krypton.Navigator.PaletteNavButtonSpecStyle.ArrowLeft;
            this.buttonSpecNavigatorExpandCollapse.UniqueName = "6148fdb3d44844208d3b3992d9cae43c";
            this.buttonSpecNavigatorExpandCollapse.Click += new System.EventHandler(this.buttonSpecNavigatorExpandCollapse_Click);
            // 
            // kryptonPageFolderSearchFilterSearch
            // 
            this.kryptonPageFolderSearchFilterSearch.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageFolderSearchFilterSearch.AutoScroll = true;
            this.kryptonPageFolderSearchFilterSearch.Controls.Add(this.kryptonWorkspaceSearchFilter);
            this.kryptonPageFolderSearchFilterSearch.Flags = 65534;
            this.kryptonPageFolderSearchFilterSearch.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.Search_Search48x48;
            this.kryptonPageFolderSearchFilterSearch.ImageMedium = global::PhotoTagsSynchronizer.Properties.Resources.Search_Search24x24;
            this.kryptonPageFolderSearchFilterSearch.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.Search_Search16x16;
            this.kryptonPageFolderSearchFilterSearch.LastVisibleSet = true;
            this.kryptonPageFolderSearchFilterSearch.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageFolderSearchFilterSearch.Name = "kryptonPageFolderSearchFilterSearch";
            this.kryptonPageFolderSearchFilterSearch.Size = new System.Drawing.Size(399, 660);
            this.kryptonPageFolderSearchFilterSearch.Text = "Search";
            this.kryptonPageFolderSearchFilterSearch.TextDescription = "Search for media files in database";
            this.kryptonPageFolderSearchFilterSearch.TextTitle = "Search";
            this.kryptonPageFolderSearchFilterSearch.ToolTipTitle = "Search for media files in database";
            this.kryptonPageFolderSearchFilterSearch.UniqueName = "408ae935c7f1463495958366914474e7";
            this.kryptonPageFolderSearchFilterSearch.Enter += new System.EventHandler(this.kryptonPageFolderSearchFilterSearch_Enter);
            this.kryptonPageFolderSearchFilterSearch.Resize += new System.EventHandler(this.kryptonPageFolderSearchFilterSearch_Resize);
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
            this.kryptonWorkspaceSearchFilter.Size = new System.Drawing.Size(399, 660);
            this.kryptonWorkspaceSearchFilter.TabIndex = 43;
            this.kryptonWorkspaceSearchFilter.TabStop = true;
            // 
            // kryptonPageSearchFiler
            // 
            this.kryptonPageSearchFiler.AutoHiddenSlideSize = new System.Drawing.Size(0, 0);
            this.kryptonPageSearchFiler.AutoScroll = true;
            this.kryptonPageSearchFiler.Controls.Add(this.tableLayoutPanelSerachSearch);
            this.kryptonPageSearchFiler.Flags = 65534;
            this.kryptonPageSearchFiler.LastVisibleSet = true;
            this.kryptonPageSearchFiler.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageSearchFiler.Name = "kryptonPageSearchFiler";
            this.kryptonPageSearchFiler.Size = new System.Drawing.Size(399, 600);
            this.kryptonPageSearchFiler.Text = "Search Filter";
            this.kryptonPageSearchFiler.ToolTipTitle = "Page ToolTip";
            this.kryptonPageSearchFiler.UniqueName = "6322e48c4013478f8c3bf8f0b4f220ae";
            // 
            // tableLayoutPanelSerachSearch
            // 
            this.tableLayoutPanelSerachSearch.AutoSize = true;
            this.tableLayoutPanelSerachSearch.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanelSerachSearch.ColumnCount = 1;
            this.tableLayoutPanelSerachSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSerachSearch.Controls.Add(this.groupBoxSearchFileSystem, 0, 0);
            this.tableLayoutPanelSerachSearch.Controls.Add(this.groupBoxSearchPeople, 0, 6);
            this.tableLayoutPanelSerachSearch.Controls.Add(this.groupBoxSearchTags, 0, 3);
            this.tableLayoutPanelSerachSearch.Controls.Add(this.groupBoxSearchKeywords, 0, 5);
            this.tableLayoutPanelSerachSearch.Controls.Add(this.groupBoxSearchMediaTaken, 0, 1);
            this.tableLayoutPanelSerachSearch.Controls.Add(this.groupBoxSearchExtra, 0, 4);
            this.tableLayoutPanelSerachSearch.Controls.Add(this.groupBoxSearchRating, 0, 2);
            this.tableLayoutPanelSerachSearch.Controls.Add(this.kryptonLabelEndOfPage, 0, 8);
            this.tableLayoutPanelSerachSearch.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelSerachSearch.MinimumSize = new System.Drawing.Size(280, 1026);
            this.tableLayoutPanelSerachSearch.Name = "tableLayoutPanelSerachSearch";
            this.tableLayoutPanelSerachSearch.RowCount = 9;
            this.tableLayoutPanelSerachSearch.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSerachSearch.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSerachSearch.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSerachSearch.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSerachSearch.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSerachSearch.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSerachSearch.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSerachSearch.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSerachSearch.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSerachSearch.Size = new System.Drawing.Size(286, 1045);
            this.tableLayoutPanelSerachSearch.TabIndex = 29;
            // 
            // groupBoxSearchFileSystem
            // 
            this.groupBoxSearchFileSystem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSearchFileSystem.AutoSize = true;
            this.groupBoxSearchFileSystem.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
            this.groupBoxSearchFileSystem.Location = new System.Drawing.Point(3, 3);
            this.groupBoxSearchFileSystem.Name = "groupBoxSearchFileSystem";
            // 
            // groupBoxSearchFileSystem.Panel
            // 
            this.groupBoxSearchFileSystem.Panel.Controls.Add(this.tableLayoutPanelSearchFileSystem);
            this.groupBoxSearchFileSystem.Size = new System.Drawing.Size(280, 74);
            this.groupBoxSearchFileSystem.TabIndex = 28;
            this.groupBoxSearchFileSystem.Values.Heading = "FileSystem";
            // 
            // tableLayoutPanelSearchFileSystem
            // 
            this.tableLayoutPanelSearchFileSystem.AutoSize = true;
            this.tableLayoutPanelSearchFileSystem.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanelSearchFileSystem.ColumnCount = 2;
            this.tableLayoutPanelSearchFileSystem.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanelSearchFileSystem.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSearchFileSystem.Controls.Add(this.kryptonLabelSearchDirectory, 0, 0);
            this.tableLayoutPanelSearchFileSystem.Controls.Add(this.kryptonTextBoxSearchFilename, 1, 1);
            this.tableLayoutPanelSearchFileSystem.Controls.Add(this.kryptonLabelSearchFilename, 0, 1);
            this.tableLayoutPanelSearchFileSystem.Controls.Add(this.kryptonTextBoxSearchDirectory, 1, 0);
            this.tableLayoutPanelSearchFileSystem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelSearchFileSystem.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelSearchFileSystem.MinimumSize = new System.Drawing.Size(270, 52);
            this.tableLayoutPanelSearchFileSystem.Name = "tableLayoutPanelSearchFileSystem";
            this.tableLayoutPanelSearchFileSystem.RowCount = 2;
            this.tableLayoutPanelSearchFileSystem.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSearchFileSystem.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSearchFileSystem.Size = new System.Drawing.Size(276, 54);
            this.tableLayoutPanelSearchFileSystem.TabIndex = 4;
            // 
            // kryptonLabelSearchDirectory
            // 
            this.kryptonLabelSearchDirectory.Location = new System.Drawing.Point(3, 3);
            this.kryptonLabelSearchDirectory.Name = "kryptonLabelSearchDirectory";
            this.kryptonLabelSearchDirectory.Size = new System.Drawing.Size(59, 18);
            this.kryptonLabelSearchDirectory.TabIndex = 2;
            this.kryptonLabelSearchDirectory.Values.Text = "Directory:";
            // 
            // kryptonTextBoxSearchFilename
            // 
            this.kryptonTextBoxSearchFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonTextBoxSearchFilename.Location = new System.Drawing.Point(103, 29);
            this.kryptonTextBoxSearchFilename.Name = "kryptonTextBoxSearchFilename";
            this.kryptonTextBoxSearchFilename.Size = new System.Drawing.Size(170, 20);
            this.kryptonTextBoxSearchFilename.TabIndex = 1;
            // 
            // kryptonLabelSearchFilename
            // 
            this.kryptonLabelSearchFilename.Location = new System.Drawing.Point(3, 29);
            this.kryptonLabelSearchFilename.Name = "kryptonLabelSearchFilename";
            this.kryptonLabelSearchFilename.Size = new System.Drawing.Size(60, 18);
            this.kryptonLabelSearchFilename.TabIndex = 3;
            this.kryptonLabelSearchFilename.Values.Text = "Filename:";
            // 
            // kryptonTextBoxSearchDirectory
            // 
            this.kryptonTextBoxSearchDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonTextBoxSearchDirectory.Location = new System.Drawing.Point(103, 3);
            this.kryptonTextBoxSearchDirectory.Name = "kryptonTextBoxSearchDirectory";
            this.kryptonTextBoxSearchDirectory.Size = new System.Drawing.Size(170, 20);
            this.kryptonTextBoxSearchDirectory.TabIndex = 0;
            // 
            // groupBoxSearchPeople
            // 
            this.groupBoxSearchPeople.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSearchPeople.AutoSize = true;
            this.groupBoxSearchPeople.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
            this.groupBoxSearchPeople.Location = new System.Drawing.Point(3, 691);
            this.groupBoxSearchPeople.Name = "groupBoxSearchPeople";
            // 
            // groupBoxSearchPeople.Panel
            // 
            this.groupBoxSearchPeople.Panel.Controls.Add(this.tableLayoutPanelSearchPeople);
            this.groupBoxSearchPeople.Size = new System.Drawing.Size(280, 276);
            this.groupBoxSearchPeople.TabIndex = 27;
            this.groupBoxSearchPeople.Values.Heading = "People:";
            // 
            // tableLayoutPanelSearchPeople
            // 
            this.tableLayoutPanelSearchPeople.AutoSize = true;
            this.tableLayoutPanelSearchPeople.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanelSearchPeople.ColumnCount = 2;
            this.tableLayoutPanelSearchPeople.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanelSearchPeople.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSearchPeople.Controls.Add(this.checkedListBoxSearchPeople, 0, 2);
            this.tableLayoutPanelSearchPeople.Controls.Add(this.checkBoxSearchNeedAllNames, 1, 0);
            this.tableLayoutPanelSearchPeople.Controls.Add(this.checkBoxSearchWithoutRegions, 1, 1);
            this.tableLayoutPanelSearchPeople.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelSearchPeople.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelSearchPeople.MinimumSize = new System.Drawing.Size(270, 254);
            this.tableLayoutPanelSearchPeople.Name = "tableLayoutPanelSearchPeople";
            this.tableLayoutPanelSearchPeople.RowCount = 3;
            this.tableLayoutPanelSearchPeople.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSearchPeople.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSearchPeople.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSearchPeople.Size = new System.Drawing.Size(276, 256);
            this.tableLayoutPanelSearchPeople.TabIndex = 48;
            // 
            // checkedListBoxSearchPeople
            // 
            this.checkedListBoxSearchPeople.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelSearchPeople.SetColumnSpan(this.checkedListBoxSearchPeople, 2);
            this.checkedListBoxSearchPeople.Location = new System.Drawing.Point(3, 51);
            this.checkedListBoxSearchPeople.MinimumSize = new System.Drawing.Size(260, 200);
            this.checkedListBoxSearchPeople.Name = "checkedListBoxSearchPeople";
            this.checkedListBoxSearchPeople.Size = new System.Drawing.Size(270, 200);
            this.checkedListBoxSearchPeople.TabIndex = 45;
            this.checkedListBoxSearchPeople.Tag = "SearchPeople";
            // 
            // checkBoxSearchNeedAllNames
            // 
            this.checkBoxSearchNeedAllNames.Checked = true;
            this.checkBoxSearchNeedAllNames.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSearchNeedAllNames.Location = new System.Drawing.Point(103, 3);
            this.checkBoxSearchNeedAllNames.Name = "checkBoxSearchNeedAllNames";
            this.checkBoxSearchNeedAllNames.Size = new System.Drawing.Size(144, 18);
            this.checkBoxSearchNeedAllNames.TabIndex = 46;
            this.checkBoxSearchNeedAllNames.Values.Text = "When contain all names";
            // 
            // checkBoxSearchWithoutRegions
            // 
            this.checkBoxSearchWithoutRegions.Location = new System.Drawing.Point(103, 27);
            this.checkBoxSearchWithoutRegions.Name = "checkBoxSearchWithoutRegions";
            this.checkBoxSearchWithoutRegions.Size = new System.Drawing.Size(136, 18);
            this.checkBoxSearchWithoutRegions.TabIndex = 47;
            this.checkBoxSearchWithoutRegions.Values.Text = "Or whitout any regions";
            // 
            // groupBoxSearchTags
            // 
            this.groupBoxSearchTags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSearchTags.AutoSize = true;
            this.groupBoxSearchTags.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
            this.groupBoxSearchTags.Location = new System.Drawing.Point(3, 237);
            this.groupBoxSearchTags.MinimumSize = new System.Drawing.Size(261, 261);
            this.groupBoxSearchTags.Name = "groupBoxSearchTags";
            // 
            // groupBoxSearchTags.Panel
            // 
            this.groupBoxSearchTags.Panel.Controls.Add(this.tableLayoutPanelSearchDetails);
            this.groupBoxSearchTags.Size = new System.Drawing.Size(280, 263);
            this.groupBoxSearchTags.TabIndex = 9;
            this.groupBoxSearchTags.Values.Heading = "Search inside text field tags:";
            // 
            // tableLayoutPanelSearchDetails
            // 
            this.tableLayoutPanelSearchDetails.AutoSize = true;
            this.tableLayoutPanelSearchDetails.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanelSearchDetails.ColumnCount = 2;
            this.tableLayoutPanelSearchDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanelSearchDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSearchDetails.Controls.Add(this.checkBoxSearchUseAndBetweenTextTagFields, 1, 8);
            this.tableLayoutPanelSearchDetails.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanelSearchDetails.Controls.Add(this.comboBoxSearchLocationCountry, 1, 7);
            this.tableLayoutPanelSearchDetails.Controls.Add(this.label10, 0, 7);
            this.tableLayoutPanelSearchDetails.Controls.Add(this.comboBoxSearchLocationState, 1, 6);
            this.tableLayoutPanelSearchDetails.Controls.Add(this.label13, 0, 4);
            this.tableLayoutPanelSearchDetails.Controls.Add(this.label11, 0, 6);
            this.tableLayoutPanelSearchDetails.Controls.Add(this.comboBoxSearchLocationCity, 1, 5);
            this.tableLayoutPanelSearchDetails.Controls.Add(this.comboBoxSearchLocationName, 1, 4);
            this.tableLayoutPanelSearchDetails.Controls.Add(this.comboBoxSearchComments, 1, 3);
            this.tableLayoutPanelSearchDetails.Controls.Add(this.label6, 0, 1);
            this.tableLayoutPanelSearchDetails.Controls.Add(this.label12, 0, 5);
            this.tableLayoutPanelSearchDetails.Controls.Add(this.comboBoxSearchTitle, 1, 1);
            this.tableLayoutPanelSearchDetails.Controls.Add(this.label7, 0, 2);
            this.tableLayoutPanelSearchDetails.Controls.Add(this.comboBoxSearchDescription, 1, 2);
            this.tableLayoutPanelSearchDetails.Controls.Add(this.label8, 0, 3);
            this.tableLayoutPanelSearchDetails.Controls.Add(this.comboBoxSearchAlbum, 1, 0);
            this.tableLayoutPanelSearchDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelSearchDetails.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelSearchDetails.MinimumSize = new System.Drawing.Size(270, 241);
            this.tableLayoutPanelSearchDetails.Name = "tableLayoutPanelSearchDetails";
            this.tableLayoutPanelSearchDetails.RowCount = 9;
            this.tableLayoutPanelSearchDetails.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSearchDetails.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSearchDetails.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSearchDetails.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSearchDetails.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSearchDetails.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSearchDetails.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSearchDetails.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSearchDetails.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSearchDetails.Size = new System.Drawing.Size(276, 243);
            this.tableLayoutPanelSearchDetails.TabIndex = 2;
            // 
            // checkBoxSearchUseAndBetweenTextTagFields
            // 
            this.checkBoxSearchUseAndBetweenTextTagFields.Location = new System.Drawing.Point(103, 195);
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
            this.comboBoxSearchLocationCountry.Location = new System.Drawing.Point(103, 171);
            this.comboBoxSearchLocationCountry.Name = "comboBoxSearchLocationCountry";
            this.comboBoxSearchLocationCountry.Size = new System.Drawing.Size(170, 18);
            this.comboBoxSearchLocationCountry.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxSearchLocationCountry.TabIndex = 42;
            this.comboBoxSearchLocationCountry.Tag = "Country";
            this.comboBoxSearchLocationCountry.ToolTipValues.Description = "List of suggested Countries";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(3, 171);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 18);
            this.label10.TabIndex = 34;
            this.label10.Values.Text = "Country:";
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
            this.comboBoxSearchLocationState.Location = new System.Drawing.Point(103, 147);
            this.comboBoxSearchLocationState.Name = "comboBoxSearchLocationState";
            this.comboBoxSearchLocationState.Size = new System.Drawing.Size(170, 18);
            this.comboBoxSearchLocationState.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxSearchLocationState.TabIndex = 41;
            this.comboBoxSearchLocationState.Tag = "State";
            this.comboBoxSearchLocationState.ToolTipValues.Description = "List of suggested States";
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(3, 99);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(56, 18);
            this.label13.TabIndex = 31;
            this.label13.Values.Text = "Location:";
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(3, 147);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(40, 18);
            this.label11.TabIndex = 33;
            this.label11.Values.Text = "State:";
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
            this.comboBoxSearchLocationCity.Location = new System.Drawing.Point(103, 123);
            this.comboBoxSearchLocationCity.Name = "comboBoxSearchLocationCity";
            this.comboBoxSearchLocationCity.Size = new System.Drawing.Size(170, 18);
            this.comboBoxSearchLocationCity.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxSearchLocationCity.TabIndex = 40;
            this.comboBoxSearchLocationCity.Tag = "City";
            this.comboBoxSearchLocationCity.ToolTipValues.Description = "List of suggested Cities";
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
            this.comboBoxSearchLocationName.Location = new System.Drawing.Point(103, 99);
            this.comboBoxSearchLocationName.Name = "comboBoxSearchLocationName";
            this.comboBoxSearchLocationName.Size = new System.Drawing.Size(170, 18);
            this.comboBoxSearchLocationName.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxSearchLocationName.TabIndex = 39;
            this.comboBoxSearchLocationName.Tag = "Location";
            this.comboBoxSearchLocationName.ToolTipValues.Description = "List of suggested Location names";
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
            this.comboBoxSearchComments.Location = new System.Drawing.Point(103, 75);
            this.comboBoxSearchComments.Name = "comboBoxSearchComments";
            this.comboBoxSearchComments.Size = new System.Drawing.Size(170, 18);
            this.comboBoxSearchComments.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxSearchComments.TabIndex = 38;
            this.comboBoxSearchComments.Tag = "Comments";
            this.comboBoxSearchComments.ToolTipValues.Description = "List of suggested Comments";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(3, 27);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 18);
            this.label6.TabIndex = 28;
            this.label6.Values.Text = "Title:";
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(3, 123);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(33, 18);
            this.label12.TabIndex = 32;
            this.label12.Values.Text = "City:";
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
            this.comboBoxSearchTitle.Location = new System.Drawing.Point(103, 27);
            this.comboBoxSearchTitle.Name = "comboBoxSearchTitle";
            this.comboBoxSearchTitle.Size = new System.Drawing.Size(170, 18);
            this.comboBoxSearchTitle.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxSearchTitle.TabIndex = 36;
            this.comboBoxSearchTitle.Tag = "Title";
            this.comboBoxSearchTitle.ToolTipValues.Description = "List of suggested Titles";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(3, 51);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(70, 18);
            this.label7.TabIndex = 29;
            this.label7.Values.Text = "Description:";
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
            this.comboBoxSearchDescription.Location = new System.Drawing.Point(103, 51);
            this.comboBoxSearchDescription.Name = "comboBoxSearchDescription";
            this.comboBoxSearchDescription.Size = new System.Drawing.Size(170, 18);
            this.comboBoxSearchDescription.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxSearchDescription.TabIndex = 37;
            this.comboBoxSearchDescription.Tag = "Description";
            this.comboBoxSearchDescription.ToolTipValues.Description = "List of suggested Descriptions";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(3, 75);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(68, 18);
            this.label8.TabIndex = 30;
            this.label8.Values.Text = "Comments:";
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
            this.comboBoxSearchAlbum.Location = new System.Drawing.Point(103, 3);
            this.comboBoxSearchAlbum.Name = "comboBoxSearchAlbum";
            this.comboBoxSearchAlbum.Size = new System.Drawing.Size(170, 18);
            this.comboBoxSearchAlbum.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxSearchAlbum.TabIndex = 35;
            this.comboBoxSearchAlbum.Tag = "Album";
            this.comboBoxSearchAlbum.ToolTipValues.Description = "List of suggested Albums";
            // 
            // groupBoxSearchKeywords
            // 
            this.groupBoxSearchKeywords.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSearchKeywords.AutoSize = true;
            this.groupBoxSearchKeywords.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
            this.groupBoxSearchKeywords.Location = new System.Drawing.Point(3, 560);
            this.groupBoxSearchKeywords.Name = "groupBoxSearchKeywords";
            // 
            // groupBoxSearchKeywords.Panel
            // 
            this.groupBoxSearchKeywords.Panel.Controls.Add(this.tableLayoutPanelSearchKeywords);
            this.groupBoxSearchKeywords.Size = new System.Drawing.Size(280, 125);
            this.groupBoxSearchKeywords.TabIndex = 9;
            this.groupBoxSearchKeywords.Values.Heading = "Keywords";
            // 
            // tableLayoutPanelSearchKeywords
            // 
            this.tableLayoutPanelSearchKeywords.AutoSize = true;
            this.tableLayoutPanelSearchKeywords.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanelSearchKeywords.ColumnCount = 2;
            this.tableLayoutPanelSearchKeywords.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanelSearchKeywords.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSearchKeywords.Controls.Add(this.checkBoxSearchWithoutKeyword, 1, 3);
            this.tableLayoutPanelSearchKeywords.Controls.Add(this.checkBoxSearchNeedAllKeywords, 1, 2);
            this.tableLayoutPanelSearchKeywords.Controls.Add(this.label9, 0, 1);
            this.tableLayoutPanelSearchKeywords.Controls.Add(this.label15, 0, 0);
            this.tableLayoutPanelSearchKeywords.Controls.Add(this.comboBoxSearchKeyword, 1, 0);
            this.tableLayoutPanelSearchKeywords.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelSearchKeywords.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelSearchKeywords.MinimumSize = new System.Drawing.Size(270, 103);
            this.tableLayoutPanelSearchKeywords.Name = "tableLayoutPanelSearchKeywords";
            this.tableLayoutPanelSearchKeywords.RowCount = 4;
            this.tableLayoutPanelSearchKeywords.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSearchKeywords.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSearchKeywords.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSearchKeywords.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSearchKeywords.Size = new System.Drawing.Size(276, 105);
            this.tableLayoutPanelSearchKeywords.TabIndex = 2;
            // 
            // checkBoxSearchWithoutKeyword
            // 
            this.checkBoxSearchWithoutKeyword.Location = new System.Drawing.Point(103, 75);
            this.checkBoxSearchWithoutKeyword.Name = "checkBoxSearchWithoutKeyword";
            this.checkBoxSearchWithoutKeyword.Size = new System.Drawing.Size(147, 18);
            this.checkBoxSearchWithoutKeyword.TabIndex = 47;
            this.checkBoxSearchWithoutKeyword.Values.Text = "Or without any keywords";
            // 
            // checkBoxSearchNeedAllKeywords
            // 
            this.checkBoxSearchNeedAllKeywords.Checked = true;
            this.checkBoxSearchNeedAllKeywords.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSearchNeedAllKeywords.Location = new System.Drawing.Point(103, 51);
            this.checkBoxSearchNeedAllKeywords.Name = "checkBoxSearchNeedAllKeywords";
            this.checkBoxSearchNeedAllKeywords.Size = new System.Drawing.Size(158, 18);
            this.checkBoxSearchNeedAllKeywords.TabIndex = 48;
            this.checkBoxSearchNeedAllKeywords.Values.Text = "When contain all keywords";
            // 
            // label9
            // 
            this.tableLayoutPanelSearchKeywords.SetColumnSpan(this.label9, 2);
            this.label9.Location = new System.Drawing.Point(3, 27);
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
            // comboBoxSearchKeyword
            // 
            this.comboBoxSearchKeyword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSearchKeyword.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxSearchKeyword.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxSearchKeyword.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxSearchKeyword.DropDownWidth = 183;
            this.comboBoxSearchKeyword.IntegralHeight = false;
            this.comboBoxSearchKeyword.Items.AddRange(new object[] {
            "Example keywords"});
            this.comboBoxSearchKeyword.Location = new System.Drawing.Point(103, 3);
            this.comboBoxSearchKeyword.Name = "comboBoxSearchKeyword";
            this.comboBoxSearchKeyword.Size = new System.Drawing.Size(170, 18);
            this.comboBoxSearchKeyword.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxSearchKeyword.TabIndex = 46;
            this.comboBoxSearchKeyword.Tag = "Keywords";
            this.comboBoxSearchKeyword.ToolTipValues.Description = "List of suggested Keywords.\r\nYou can add more keywords just sepearte them with ;";
            // 
            // groupBoxSearchMediaTaken
            // 
            this.groupBoxSearchMediaTaken.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSearchMediaTaken.AutoSize = true;
            this.groupBoxSearchMediaTaken.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
            this.groupBoxSearchMediaTaken.Location = new System.Drawing.Point(3, 83);
            this.groupBoxSearchMediaTaken.Name = "groupBoxSearchMediaTaken";
            // 
            // groupBoxSearchMediaTaken.Panel
            // 
            this.groupBoxSearchMediaTaken.Panel.Controls.Add(this.tableLayoutPanelSearchMediaTaken);
            this.groupBoxSearchMediaTaken.Size = new System.Drawing.Size(280, 94);
            this.groupBoxSearchMediaTaken.TabIndex = 9;
            this.groupBoxSearchMediaTaken.Values.Heading = "Media taken:";
            // 
            // tableLayoutPanelSearchMediaTaken
            // 
            this.tableLayoutPanelSearchMediaTaken.AutoSize = true;
            this.tableLayoutPanelSearchMediaTaken.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanelSearchMediaTaken.ColumnCount = 2;
            this.tableLayoutPanelSearchMediaTaken.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanelSearchMediaTaken.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSearchMediaTaken.Controls.Add(this.label14, 0, 0);
            this.tableLayoutPanelSearchMediaTaken.Controls.Add(this.dateTimePickerSearchDateTo, 1, 1);
            this.tableLayoutPanelSearchMediaTaken.Controls.Add(this.dateTimePickerSearchDateFrom, 1, 0);
            this.tableLayoutPanelSearchMediaTaken.Controls.Add(this.label17, 0, 1);
            this.tableLayoutPanelSearchMediaTaken.Controls.Add(this.checkBoxSearchMediaTakenIsNull, 1, 2);
            this.tableLayoutPanelSearchMediaTaken.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelSearchMediaTaken.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelSearchMediaTaken.MinimumSize = new System.Drawing.Size(270, 72);
            this.tableLayoutPanelSearchMediaTaken.Name = "tableLayoutPanelSearchMediaTaken";
            this.tableLayoutPanelSearchMediaTaken.RowCount = 3;
            this.tableLayoutPanelSearchMediaTaken.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSearchMediaTaken.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSearchMediaTaken.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSearchMediaTaken.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelSearchMediaTaken.Size = new System.Drawing.Size(276, 74);
            this.tableLayoutPanelSearchMediaTaken.TabIndex = 2;
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(3, 3);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(85, 18);
            this.label14.TabIndex = 39;
            this.label14.Values.Text = "DateTaken >=:";
            // 
            // dateTimePickerSearchDateTo
            // 
            this.dateTimePickerSearchDateTo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerSearchDateTo.Checked = false;
            this.dateTimePickerSearchDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerSearchDateTo.Location = new System.Drawing.Point(103, 27);
            this.dateTimePickerSearchDateTo.MaxDate = new System.DateTime(2200, 12, 31, 0, 0, 0, 0);
            this.dateTimePickerSearchDateTo.MinDate = new System.DateTime(1900, 1, 1, 0, 0, 0, 0);
            this.dateTimePickerSearchDateTo.Name = "dateTimePickerSearchDateTo";
            this.dateTimePickerSearchDateTo.ShowCheckBox = true;
            this.dateTimePickerSearchDateTo.Size = new System.Drawing.Size(170, 18);
            this.dateTimePickerSearchDateTo.TabIndex = 41;
            this.dateTimePickerSearchDateTo.Tag = "DateTakenTo";
            // 
            // dateTimePickerSearchDateFrom
            // 
            this.dateTimePickerSearchDateFrom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerSearchDateFrom.Checked = false;
            this.dateTimePickerSearchDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerSearchDateFrom.Location = new System.Drawing.Point(103, 3);
            this.dateTimePickerSearchDateFrom.MaxDate = new System.DateTime(2200, 12, 31, 0, 0, 0, 0);
            this.dateTimePickerSearchDateFrom.MinDate = new System.DateTime(1900, 1, 1, 0, 0, 0, 0);
            this.dateTimePickerSearchDateFrom.Name = "dateTimePickerSearchDateFrom";
            this.dateTimePickerSearchDateFrom.ShowCheckBox = true;
            this.dateTimePickerSearchDateFrom.Size = new System.Drawing.Size(170, 18);
            this.dateTimePickerSearchDateFrom.TabIndex = 38;
            this.dateTimePickerSearchDateFrom.Tag = "DateTakenFrom";
            // 
            // label17
            // 
            this.label17.Location = new System.Drawing.Point(3, 27);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(85, 18);
            this.label17.TabIndex = 40;
            this.label17.Values.Text = "DateTaken <=:";
            // 
            // checkBoxSearchMediaTakenIsNull
            // 
            this.checkBoxSearchMediaTakenIsNull.Location = new System.Drawing.Point(103, 51);
            this.checkBoxSearchMediaTakenIsNull.Name = "checkBoxSearchMediaTakenIsNull";
            this.checkBoxSearchMediaTakenIsNull.Size = new System.Drawing.Size(138, 18);
            this.checkBoxSearchMediaTakenIsNull.TabIndex = 42;
            this.checkBoxSearchMediaTakenIsNull.Tag = "OrWhenMissingValue";
            this.checkBoxSearchMediaTakenIsNull.Values.Text = "Or when missing value";
            // 
            // groupBoxSearchExtra
            // 
            this.groupBoxSearchExtra.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSearchExtra.AutoSize = true;
            this.groupBoxSearchExtra.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
            this.groupBoxSearchExtra.Location = new System.Drawing.Point(3, 506);
            this.groupBoxSearchExtra.Name = "groupBoxSearchExtra";
            // 
            // groupBoxSearchExtra.Panel
            // 
            this.groupBoxSearchExtra.Panel.Controls.Add(this.tableLayoutPanelSearchAttributes);
            this.groupBoxSearchExtra.Size = new System.Drawing.Size(280, 48);
            this.groupBoxSearchExtra.TabIndex = 9;
            this.groupBoxSearchExtra.Values.Heading = "Attributes:";
            // 
            // tableLayoutPanelSearchAttributes
            // 
            this.tableLayoutPanelSearchAttributes.AutoSize = true;
            this.tableLayoutPanelSearchAttributes.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanelSearchAttributes.ColumnCount = 2;
            this.tableLayoutPanelSearchAttributes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanelSearchAttributes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSearchAttributes.Controls.Add(this.checkBoxSearchHasWarning, 1, 0);
            this.tableLayoutPanelSearchAttributes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelSearchAttributes.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelSearchAttributes.MinimumSize = new System.Drawing.Size(270, 26);
            this.tableLayoutPanelSearchAttributes.Name = "tableLayoutPanelSearchAttributes";
            this.tableLayoutPanelSearchAttributes.RowCount = 1;
            this.tableLayoutPanelSearchAttributes.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSearchAttributes.Size = new System.Drawing.Size(276, 28);
            this.tableLayoutPanelSearchAttributes.TabIndex = 2;
            // 
            // checkBoxSearchHasWarning
            // 
            this.checkBoxSearchHasWarning.Location = new System.Drawing.Point(103, 3);
            this.checkBoxSearchHasWarning.Name = "checkBoxSearchHasWarning";
            this.checkBoxSearchHasWarning.Size = new System.Drawing.Size(148, 18);
            this.checkBoxSearchHasWarning.TabIndex = 30;
            this.checkBoxSearchHasWarning.Values.Text = "Has warning message(s)";
            // 
            // groupBoxSearchRating
            // 
            this.groupBoxSearchRating.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSearchRating.AutoSize = true;
            this.groupBoxSearchRating.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
            this.groupBoxSearchRating.Location = new System.Drawing.Point(3, 183);
            this.groupBoxSearchRating.Name = "groupBoxSearchRating";
            // 
            // groupBoxSearchRating.Panel
            // 
            this.groupBoxSearchRating.Panel.Controls.Add(this.tableLayoutPanelSearchRating);
            this.groupBoxSearchRating.Size = new System.Drawing.Size(280, 48);
            this.groupBoxSearchRating.TabIndex = 10;
            this.groupBoxSearchRating.Values.Heading = "Rating";
            // 
            // tableLayoutPanelSearchRating
            // 
            this.tableLayoutPanelSearchRating.AutoSize = true;
            this.tableLayoutPanelSearchRating.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanelSearchRating.ColumnCount = 7;
            this.tableLayoutPanelSearchRating.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelSearchRating.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelSearchRating.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelSearchRating.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelSearchRating.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelSearchRating.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelSearchRating.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelSearchRating.Controls.Add(this.checkBoxSearchRatingEmpty, 6, 0);
            this.tableLayoutPanelSearchRating.Controls.Add(this.checkBoxSearchRating1, 0, 0);
            this.tableLayoutPanelSearchRating.Controls.Add(this.checkBoxSearchRating0, 5, 0);
            this.tableLayoutPanelSearchRating.Controls.Add(this.checkBoxSearchRating5, 4, 0);
            this.tableLayoutPanelSearchRating.Controls.Add(this.checkBoxSearchRating2, 1, 0);
            this.tableLayoutPanelSearchRating.Controls.Add(this.checkBoxSearchRating3, 2, 0);
            this.tableLayoutPanelSearchRating.Controls.Add(this.checkBoxSearchRating4, 3, 0);
            this.tableLayoutPanelSearchRating.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelSearchRating.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelSearchRating.MinimumSize = new System.Drawing.Size(270, 26);
            this.tableLayoutPanelSearchRating.Name = "tableLayoutPanelSearchRating";
            this.tableLayoutPanelSearchRating.RowCount = 1;
            this.tableLayoutPanelSearchRating.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSearchRating.Size = new System.Drawing.Size(276, 28);
            this.tableLayoutPanelSearchRating.TabIndex = 2;
            // 
            // checkBoxSearchRatingEmpty
            // 
            this.checkBoxSearchRatingEmpty.Location = new System.Drawing.Point(213, 3);
            this.checkBoxSearchRatingEmpty.Name = "checkBoxSearchRatingEmpty";
            this.checkBoxSearchRatingEmpty.Size = new System.Drawing.Size(29, 18);
            this.checkBoxSearchRatingEmpty.TabIndex = 44;
            this.checkBoxSearchRatingEmpty.Values.Text = "?";
            // 
            // checkBoxSearchRating1
            // 
            this.checkBoxSearchRating1.Location = new System.Drawing.Point(3, 3);
            this.checkBoxSearchRating1.Name = "checkBoxSearchRating1";
            this.checkBoxSearchRating1.Size = new System.Drawing.Size(29, 18);
            this.checkBoxSearchRating1.TabIndex = 38;
            this.checkBoxSearchRating1.Values.Text = "1";
            // 
            // checkBoxSearchRating0
            // 
            this.checkBoxSearchRating0.Location = new System.Drawing.Point(178, 3);
            this.checkBoxSearchRating0.Name = "checkBoxSearchRating0";
            this.checkBoxSearchRating0.Size = new System.Drawing.Size(29, 18);
            this.checkBoxSearchRating0.TabIndex = 43;
            this.checkBoxSearchRating0.Values.Text = "0";
            // 
            // checkBoxSearchRating5
            // 
            this.checkBoxSearchRating5.Location = new System.Drawing.Point(143, 3);
            this.checkBoxSearchRating5.Name = "checkBoxSearchRating5";
            this.checkBoxSearchRating5.Size = new System.Drawing.Size(29, 18);
            this.checkBoxSearchRating5.TabIndex = 42;
            this.checkBoxSearchRating5.Values.Text = "5";
            // 
            // checkBoxSearchRating2
            // 
            this.checkBoxSearchRating2.Location = new System.Drawing.Point(38, 3);
            this.checkBoxSearchRating2.Name = "checkBoxSearchRating2";
            this.checkBoxSearchRating2.Size = new System.Drawing.Size(29, 18);
            this.checkBoxSearchRating2.TabIndex = 39;
            this.checkBoxSearchRating2.Values.Text = "2";
            // 
            // checkBoxSearchRating3
            // 
            this.checkBoxSearchRating3.Location = new System.Drawing.Point(73, 3);
            this.checkBoxSearchRating3.Name = "checkBoxSearchRating3";
            this.checkBoxSearchRating3.Size = new System.Drawing.Size(29, 18);
            this.checkBoxSearchRating3.TabIndex = 40;
            this.checkBoxSearchRating3.Values.Text = "3";
            // 
            // checkBoxSearchRating4
            // 
            this.checkBoxSearchRating4.Location = new System.Drawing.Point(108, 3);
            this.checkBoxSearchRating4.Name = "checkBoxSearchRating4";
            this.checkBoxSearchRating4.Size = new System.Drawing.Size(29, 18);
            this.checkBoxSearchRating4.TabIndex = 41;
            this.checkBoxSearchRating4.Values.Text = "4";
            // 
            // kryptonLabelEndOfPage
            // 
            this.kryptonLabelEndOfPage.Location = new System.Drawing.Point(3, 973);
            this.kryptonLabelEndOfPage.Name = "kryptonLabelEndOfPage";
            this.kryptonLabelEndOfPage.Size = new System.Drawing.Size(71, 18);
            this.kryptonLabelEndOfPage.TabIndex = 29;
            this.kryptonLabelEndOfPage.Values.Text = "End of page";
            // 
            // kryptonWorkspaceCellSearchFiler
            // 
            this.kryptonWorkspaceCellSearchFiler.AllowDroppingPages = false;
            this.kryptonWorkspaceCellSearchFiler.AllowPageDrag = true;
            this.kryptonWorkspaceCellSearchFiler.AllowPageReorder = false;
            this.kryptonWorkspaceCellSearchFiler.AllowTabFocus = false;
            this.kryptonWorkspaceCellSearchFiler.Button.CloseButtonAction = Krypton.Navigator.CloseButtonAction.None;
            this.kryptonWorkspaceCellSearchFiler.Button.CloseButtonDisplay = Krypton.Navigator.ButtonDisplay.Hide;
            this.kryptonWorkspaceCellSearchFiler.Button.ContextButtonAction = Krypton.Navigator.ContextButtonAction.None;
            this.kryptonWorkspaceCellSearchFiler.Button.ContextButtonDisplay = Krypton.Navigator.ButtonDisplay.Hide;
            this.kryptonWorkspaceCellSearchFiler.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonWorkspaceCellSearchFiler.Name = "kryptonWorkspaceCellSearchFiler";
            this.kryptonWorkspaceCellSearchFiler.NavigatorMode = Krypton.Navigator.NavigatorMode.Panel;
            this.kryptonWorkspaceCellSearchFiler.Pages.AddRange(new Krypton.Navigator.KryptonPage[] {
            this.kryptonPageSearchFiler});
            this.kryptonWorkspaceCellSearchFiler.SelectedIndex = 0;
            this.kryptonWorkspaceCellSearchFiler.UniqueName = "ab26a27670ff4e65af82413befa923c5";
            // 
            // kryptonWorkspaceCellSearchFilterAction
            // 
            this.kryptonWorkspaceCellSearchFilterAction.AllowDroppingPages = false;
            this.kryptonWorkspaceCellSearchFilterAction.AllowPageDrag = true;
            this.kryptonWorkspaceCellSearchFilterAction.AllowPageReorder = false;
            this.kryptonWorkspaceCellSearchFilterAction.AllowTabFocus = false;
            this.kryptonWorkspaceCellSearchFilterAction.AutoSize = true;
            this.kryptonWorkspaceCellSearchFilterAction.Button.CloseButtonAction = Krypton.Navigator.CloseButtonAction.None;
            this.kryptonWorkspaceCellSearchFilterAction.Button.CloseButtonDisplay = Krypton.Navigator.ButtonDisplay.Hide;
            this.kryptonWorkspaceCellSearchFilterAction.Button.ContextButtonAction = Krypton.Navigator.ContextButtonAction.None;
            this.kryptonWorkspaceCellSearchFilterAction.Button.ContextButtonDisplay = Krypton.Navigator.ButtonDisplay.Hide;
            this.kryptonWorkspaceCellSearchFilterAction.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonWorkspaceCellSearchFilterAction.Name = "kryptonWorkspaceCellSearchFilterAction";
            this.kryptonWorkspaceCellSearchFilterAction.NavigatorMode = Krypton.Navigator.NavigatorMode.Panel;
            this.kryptonWorkspaceCellSearchFilterAction.Pages.AddRange(new Krypton.Navigator.KryptonPage[] {
            this.kryptonPageSearchFilterAction});
            this.kryptonWorkspaceCellSearchFilterAction.SelectedIndex = 0;
            this.kryptonWorkspaceCellSearchFilterAction.StarSize = "50*,55";
            this.kryptonWorkspaceCellSearchFilterAction.UniqueName = "61505220abf44dd890138f2c3d8e0e7e";
            // 
            // kryptonPageSearchFilterAction
            // 
            this.kryptonPageSearchFilterAction.AutoHiddenSlideSize = new System.Drawing.Size(50, 50);
            this.kryptonPageSearchFilterAction.Controls.Add(this.tableLayoutPanelSerachActions);
            this.kryptonPageSearchFilterAction.Flags = 65534;
            this.kryptonPageSearchFilterAction.LastVisibleSet = true;
            this.kryptonPageSearchFilterAction.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageSearchFilterAction.Name = "kryptonPageSearchFilterAction";
            this.kryptonPageSearchFilterAction.Size = new System.Drawing.Size(399, 55);
            this.kryptonPageSearchFilterAction.Text = "Search filter actions";
            this.kryptonPageSearchFilterAction.ToolTipTitle = "Page ToolTip";
            this.kryptonPageSearchFilterAction.UniqueName = "f5d8c6f255674566b2ec626051f2ca4f";
            // 
            // tableLayoutPanelSerachActions
            // 
            this.tableLayoutPanelSerachActions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelSerachActions.AutoSize = true;
            this.tableLayoutPanelSerachActions.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanelSerachActions.ColumnCount = 2;
            this.tableLayoutPanelSerachActions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelSerachActions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelSerachActions.Controls.Add(this.buttonSearch, 1, 0);
            this.tableLayoutPanelSerachActions.Controls.Add(this.kryptonCheckBoxSearchUseRegEx, 0, 1);
            this.tableLayoutPanelSerachActions.Controls.Add(this.checkBoxSerachFitsAllValues, 0, 0);
            this.tableLayoutPanelSerachActions.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelSerachActions.MinimumSize = new System.Drawing.Size(289, 48);
            this.tableLayoutPanelSerachActions.Name = "tableLayoutPanelSerachActions";
            this.tableLayoutPanelSerachActions.RowCount = 2;
            this.tableLayoutPanelSerachActions.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSerachActions.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSerachActions.Size = new System.Drawing.Size(293, 52);
            this.tableLayoutPanelSerachActions.TabIndex = 2;
            // 
            // buttonSearch
            // 
            this.buttonSearch.Location = new System.Drawing.Point(139, 3);
            this.buttonSearch.Name = "buttonSearch";
            this.tableLayoutPanelSerachActions.SetRowSpan(this.buttonSearch, 2);
            this.buttonSearch.Size = new System.Drawing.Size(142, 35);
            this.buttonSearch.TabIndex = 42;
            this.buttonSearch.Values.Text = "Search";
            this.buttonSearch.Click += new System.EventHandler(this.buttonFilterSearch_Click);
            // 
            // kryptonCheckBoxSearchUseRegEx
            // 
            this.kryptonCheckBoxSearchUseRegEx.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.kryptonCheckBoxSearchUseRegEx.Location = new System.Drawing.Point(3, 31);
            this.kryptonCheckBoxSearchUseRegEx.Name = "kryptonCheckBoxSearchUseRegEx";
            this.kryptonCheckBoxSearchUseRegEx.Size = new System.Drawing.Size(80, 18);
            this.kryptonCheckBoxSearchUseRegEx.TabIndex = 43;
            this.kryptonCheckBoxSearchUseRegEx.Values.Text = "Use RexEx";
            this.kryptonCheckBoxSearchUseRegEx.Visible = false;
            // 
            // checkBoxSerachFitsAllValues
            // 
            this.checkBoxSerachFitsAllValues.Checked = true;
            this.checkBoxSerachFitsAllValues.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSerachFitsAllValues.Location = new System.Drawing.Point(3, 3);
            this.checkBoxSerachFitsAllValues.Name = "checkBoxSerachFitsAllValues";
            this.checkBoxSerachFitsAllValues.Size = new System.Drawing.Size(130, 18);
            this.checkBoxSerachFitsAllValues.TabIndex = 26;
            this.checkBoxSerachFitsAllValues.Values.Text = "Use And (or fit some)";
            // 
            // kryptonPageFolderSearchFilterFilter
            // 
            this.kryptonPageFolderSearchFilterFilter.AutoHiddenSlideSize = new System.Drawing.Size(0, 0);
            this.kryptonPageFolderSearchFilterFilter.Controls.Add(this.treeViewFilter);
            this.kryptonPageFolderSearchFilterFilter.Flags = 65534;
            this.kryptonPageFolderSearchFilterFilter.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.Search_Filter48x48;
            this.kryptonPageFolderSearchFilterFilter.ImageMedium = global::PhotoTagsSynchronizer.Properties.Resources.Search_Filter24x24;
            this.kryptonPageFolderSearchFilterFilter.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.Search_Filter16x16;
            this.kryptonPageFolderSearchFilterFilter.LastVisibleSet = true;
            this.kryptonPageFolderSearchFilterFilter.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageFolderSearchFilterFilter.Name = "kryptonPageFolderSearchFilterFilter";
            this.kryptonPageFolderSearchFilterFilter.Size = new System.Drawing.Size(399, 652);
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
            treeNode1.Name = "NodeFolder";
            treeNode1.Tag = "Filter";
            treeNode1.Text = "Filter";
            this.treeViewFilter.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.treeViewFilter.Size = new System.Drawing.Size(399, 652);
            this.treeViewFilter.TabIndex = 0;
            this.treeViewFilter.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeViewFilter_AfterCheck);
            this.treeViewFilter.BeforeCheck += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeViewFilter_BeforeCheck);
            // 
            // kryptonWorkspaceCellMediaFiles
            // 
            this.kryptonWorkspaceCellMediaFiles.AllowDroppingPages = false;
            this.kryptonWorkspaceCellMediaFiles.AllowPageDrag = true;
            this.kryptonWorkspaceCellMediaFiles.AllowPageReorder = false;
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
            this.kryptonWorkspaceCellMediaFiles.MaximizeRestoreClicked += new System.EventHandler(this.kryptonWorkspaceCellMediaFiles_MaximizeRestoreClicked);
            // 
            // kryptonPageMediaFiles
            // 
            this.kryptonPageMediaFiles.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageMediaFiles.Controls.Add(this.imageListView1);
            this.kryptonPageMediaFiles.Flags = 65534;
            this.kryptonPageMediaFiles.LastVisibleSet = true;
            this.kryptonPageMediaFiles.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageMediaFiles.Name = "kryptonPageMediaFiles";
            this.kryptonPageMediaFiles.Size = new System.Drawing.Size(399, 735);
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
            this.imageListView1.Size = new System.Drawing.Size(399, 735);
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
            this.kryptonWorkspaceCellToolbox.AllowDroppingPages = false;
            this.kryptonWorkspaceCellToolbox.AllowPageDrag = true;
            this.kryptonWorkspaceCellToolbox.AllowPageReorder = false;
            this.kryptonWorkspaceCellToolbox.AllowTabFocus = false;
            this.kryptonWorkspaceCellToolbox.Button.ButtonSpecs.AddRange(new Krypton.Navigator.ButtonSpecNavigator[] {
            this.buttonSpecNavigatorDataGridViewProgressCircle});
            this.kryptonWorkspaceCellToolbox.Button.CloseButtonAction = Krypton.Navigator.CloseButtonAction.None;
            this.kryptonWorkspaceCellToolbox.Button.CloseButtonDisplay = Krypton.Navigator.ButtonDisplay.Hide;
            this.kryptonWorkspaceCellToolbox.Button.ContextButtonAction = Krypton.Navigator.ContextButtonAction.None;
            this.kryptonWorkspaceCellToolbox.Button.ContextButtonDisplay = Krypton.Navigator.ButtonDisplay.Hide;
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
            this.kryptonWorkspaceCellToolbox.MaximizeRestoreClicked += new System.EventHandler(this.kryptonWorkspaceCellToolbox_MaximizeRestoreClicked);
            this.kryptonWorkspaceCellToolbox.SelectedPageChanged += new System.EventHandler(this.kryptonWorkspaceCellToolbox_SelectedPageChanged);
            // 
            // buttonSpecNavigatorDataGridViewProgressCircle
            // 
            this.buttonSpecNavigatorDataGridViewProgressCircle.ImageStates.ImageNormal = global::PhotoTagsSynchronizer.Properties.Resources.ProgressCircle01_16x16;
            this.buttonSpecNavigatorDataGridViewProgressCircle.ImageStates.ImageTracking = global::PhotoTagsSynchronizer.Properties.Resources.ProgressCircle01_16x16;
            this.buttonSpecNavigatorDataGridViewProgressCircle.UniqueName = "c070f7a821ae44f59780f716a397a94b";
            // 
            // kryptonPageToolboxTags
            // 
            this.kryptonPageToolboxTags.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageToolboxTags.Controls.Add(this.kryptonWorkspaceToolboxTags);
            this.kryptonPageToolboxTags.Flags = 65534;
            this.kryptonPageToolboxTags.LastVisibleSet = true;
            this.kryptonPageToolboxTags.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageToolboxTags.Name = "kryptonPageToolboxTags";
            this.kryptonPageToolboxTags.Size = new System.Drawing.Size(400, 751);
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
            this.kryptonWorkspaceToolboxTags.Size = new System.Drawing.Size(400, 751);
            this.kryptonWorkspaceToolboxTags.TabIndex = 0;
            this.kryptonWorkspaceToolboxTags.TabStop = true;
            // 
            // kryptonPageToolboxTagsDetails
            // 
            this.kryptonPageToolboxTagsDetails.AutoHiddenSlideSize = new System.Drawing.Size(0, 0);
            this.kryptonPageToolboxTagsDetails.AutoScroll = true;
            this.kryptonPageToolboxTagsDetails.Controls.Add(this.tableLayoutPanelTags);
            this.kryptonPageToolboxTagsDetails.Flags = 65534;
            this.kryptonPageToolboxTagsDetails.LastVisibleSet = true;
            this.kryptonPageToolboxTagsDetails.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageToolboxTagsDetails.Name = "kryptonPageToolboxTagsDetails";
            this.kryptonPageToolboxTagsDetails.Size = new System.Drawing.Size(398, 329);
            this.kryptonPageToolboxTagsDetails.Text = "Tags Details";
            this.kryptonPageToolboxTagsDetails.TextDescription = "Edit media details";
            this.kryptonPageToolboxTagsDetails.TextTitle = "Tags Details";
            this.kryptonPageToolboxTagsDetails.ToolTipTitle = "Edit media details";
            this.kryptonPageToolboxTagsDetails.UniqueName = "f7053440e7e94c0b988482712a5e99a9";
            this.kryptonPageToolboxTagsDetails.Resize += new System.EventHandler(this.kryptonPageToolboxTagsDetails_Resize);
            // 
            // tableLayoutPanelTags
            // 
            this.tableLayoutPanelTags.AutoScroll = true;
            this.tableLayoutPanelTags.AutoSize = true;
            this.tableLayoutPanelTags.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanelTags.ColumnCount = 1;
            this.tableLayoutPanelTags.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTags.Controls.Add(this.kryptonGroupBoxTagsDetails, 0, 0);
            this.tableLayoutPanelTags.Controls.Add(this.kryptonGroupBoxToolboxTagsTags, 0, 2);
            this.tableLayoutPanelTags.Controls.Add(this.groupBoxRating, 0, 1);
            this.tableLayoutPanelTags.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelTags.MinimumSize = new System.Drawing.Size(272, 300);
            this.tableLayoutPanelTags.Name = "tableLayoutPanelTags";
            this.tableLayoutPanelTags.RowCount = 3;
            this.tableLayoutPanelTags.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelTags.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelTags.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelTags.Size = new System.Drawing.Size(272, 300);
            this.tableLayoutPanelTags.TabIndex = 22;
            // 
            // kryptonGroupBoxTagsDetails
            // 
            this.kryptonGroupBoxTagsDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonGroupBoxTagsDetails.AutoSize = true;
            this.kryptonGroupBoxTagsDetails.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
            this.kryptonGroupBoxTagsDetails.Location = new System.Drawing.Point(3, 3);
            this.kryptonGroupBoxTagsDetails.MinimumSize = new System.Drawing.Size(266, 171);
            this.kryptonGroupBoxTagsDetails.Name = "kryptonGroupBoxTagsDetails";
            // 
            // kryptonGroupBoxTagsDetails.Panel
            // 
            this.kryptonGroupBoxTagsDetails.Panel.Controls.Add(this.tableLayoutPanel1);
            this.kryptonGroupBoxTagsDetails.Size = new System.Drawing.Size(266, 171);
            this.kryptonGroupBoxTagsDetails.TabIndex = 20;
            this.kryptonGroupBoxTagsDetails.Values.Heading = "Details:";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.labelAuthor, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.labelDescription, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelComments, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxAuthor, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.labelTitle, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxComments, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxDescription, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxAlbum, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxTitle, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(262, 151);
            this.tableLayoutPanel1.TabIndex = 30;
            // 
            // labelAuthor
            // 
            this.labelAuthor.Location = new System.Drawing.Point(3, 99);
            this.labelAuthor.Name = "labelAuthor";
            this.labelAuthor.Size = new System.Drawing.Size(44, 18);
            this.labelAuthor.TabIndex = 24;
            this.labelAuthor.Values.Text = "Author";
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(3, 51);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(67, 18);
            this.labelDescription.TabIndex = 22;
            this.labelDescription.Values.Text = "Description";
            // 
            // labelComments
            // 
            this.labelComments.Location = new System.Drawing.Point(3, 75);
            this.labelComments.Name = "labelComments";
            this.labelComments.Size = new System.Drawing.Size(65, 18);
            this.labelComments.TabIndex = 23;
            this.labelComments.Values.Text = "Comments";
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
            this.comboBoxAuthor.Location = new System.Drawing.Point(103, 99);
            this.comboBoxAuthor.Name = "comboBoxAuthor";
            this.comboBoxAuthor.Size = new System.Drawing.Size(156, 18);
            this.comboBoxAuthor.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxAuthor.TabIndex = 29;
            this.comboBoxAuthor.Tag = "Author";
            this.comboBoxAuthor.SelectionChangeCommitted += new System.EventHandler(this.comboBoxAuthor_SelectionChangeCommitted);
            this.comboBoxAuthor.TextUpdate += new System.EventHandler(this.comboBoxAuthor_TextUpdate);
            // 
            // labelTitle
            // 
            this.labelTitle.Location = new System.Drawing.Point(3, 27);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(32, 18);
            this.labelTitle.TabIndex = 21;
            this.labelTitle.Values.Text = "Title";
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
            this.comboBoxComments.Location = new System.Drawing.Point(103, 75);
            this.comboBoxComments.Name = "comboBoxComments";
            this.comboBoxComments.Size = new System.Drawing.Size(156, 18);
            this.comboBoxComments.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxComments.TabIndex = 28;
            this.comboBoxComments.Tag = "Comments";
            this.comboBoxComments.SelectionChangeCommitted += new System.EventHandler(this.comboBoxComments_SelectionChangeCommitted);
            this.comboBoxComments.TextUpdate += new System.EventHandler(this.comboBoxComments_TextUpdate);
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
            this.comboBoxDescription.Location = new System.Drawing.Point(103, 51);
            this.comboBoxDescription.Name = "comboBoxDescription";
            this.comboBoxDescription.Size = new System.Drawing.Size(156, 18);
            this.comboBoxDescription.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxDescription.TabIndex = 27;
            this.comboBoxDescription.Tag = "Description";
            this.comboBoxDescription.SelectionChangeCommitted += new System.EventHandler(this.comboBoxDescription_SelectionChangeCommitted);
            this.comboBoxDescription.TextUpdate += new System.EventHandler(this.comboBoxDescription_TextUpdate);
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
            this.comboBoxAlbum.Location = new System.Drawing.Point(103, 3);
            this.comboBoxAlbum.Name = "comboBoxAlbum";
            this.comboBoxAlbum.Size = new System.Drawing.Size(156, 18);
            this.comboBoxAlbum.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxAlbum.TabIndex = 25;
            this.comboBoxAlbum.Tag = "Album";
            this.comboBoxAlbum.SelectionChangeCommitted += new System.EventHandler(this.comboBoxAlbum_SelectionChangeCommitted);
            this.comboBoxAlbum.TextUpdate += new System.EventHandler(this.comboBoxAlbum_TextUpdate);
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
            this.comboBoxTitle.Location = new System.Drawing.Point(103, 27);
            this.comboBoxTitle.Name = "comboBoxTitle";
            this.comboBoxTitle.Size = new System.Drawing.Size(156, 18);
            this.comboBoxTitle.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxTitle.TabIndex = 26;
            this.comboBoxTitle.Tag = "Title";
            this.comboBoxTitle.SelectionChangeCommitted += new System.EventHandler(this.comboBoxTitle_SelectionChangeCommitted);
            this.comboBoxTitle.TextUpdate += new System.EventHandler(this.comboBoxTitle_TextUpdate);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(3, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 18);
            this.label4.TabIndex = 20;
            this.label4.Values.Text = "Album";
            // 
            // kryptonGroupBoxToolboxTagsTags
            // 
            this.kryptonGroupBoxToolboxTagsTags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonGroupBoxToolboxTagsTags.AutoSize = true;
            this.kryptonGroupBoxToolboxTagsTags.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
            this.kryptonGroupBoxToolboxTagsTags.Location = new System.Drawing.Point(3, 239);
            this.kryptonGroupBoxToolboxTagsTags.MinimumSize = new System.Drawing.Size(266, 58);
            this.kryptonGroupBoxToolboxTagsTags.Name = "kryptonGroupBoxToolboxTagsTags";
            // 
            // kryptonGroupBoxToolboxTagsTags.Panel
            // 
            this.kryptonGroupBoxToolboxTagsTags.Panel.Controls.Add(this.tableLayoutPanelTagConfidence);
            this.kryptonGroupBoxToolboxTagsTags.Size = new System.Drawing.Size(266, 58);
            this.kryptonGroupBoxToolboxTagsTags.TabIndex = 21;
            this.kryptonGroupBoxToolboxTagsTags.Values.Heading = "Confidence accurisity:";
            // 
            // tableLayoutPanelTagConfidence
            // 
            this.tableLayoutPanelTagConfidence.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanelTagConfidence.ColumnCount = 2;
            this.tableLayoutPanelTagConfidence.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanelTagConfidence.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTagConfidence.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanelTagConfidence.Controls.Add(this.comboBoxMediaAiConfidence, 1, 0);
            this.tableLayoutPanelTagConfidence.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelTagConfidence.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelTagConfidence.Name = "tableLayoutPanelTagConfidence";
            this.tableLayoutPanelTagConfidence.RowCount = 1;
            this.tableLayoutPanelTagConfidence.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTagConfidence.Size = new System.Drawing.Size(262, 38);
            this.tableLayoutPanelTagConfidence.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(3, 3);
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
            this.comboBoxMediaAiConfidence.Location = new System.Drawing.Point(103, 3);
            this.comboBoxMediaAiConfidence.Name = "comboBoxMediaAiConfidence";
            this.comboBoxMediaAiConfidence.Size = new System.Drawing.Size(147, 18);
            this.comboBoxMediaAiConfidence.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxMediaAiConfidence.TabIndex = 8;
            this.comboBoxMediaAiConfidence.SelectionChangeCommitted += new System.EventHandler(this.comboBoxMediaAiConfidence_SelectionChangeCommitted);
            // 
            // groupBoxRating
            // 
            this.groupBoxRating.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxRating.AutoSize = true;
            this.groupBoxRating.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
            this.groupBoxRating.Location = new System.Drawing.Point(3, 180);
            this.groupBoxRating.MinimumSize = new System.Drawing.Size(266, 53);
            this.groupBoxRating.Name = "groupBoxRating";
            // 
            // groupBoxRating.Panel
            // 
            this.groupBoxRating.Panel.Controls.Add(this.tableLayoutPanelTagRationg);
            this.groupBoxRating.Size = new System.Drawing.Size(266, 53);
            this.groupBoxRating.TabIndex = 5;
            this.groupBoxRating.Values.Heading = "Rating";
            // 
            // tableLayoutPanelTagRationg
            // 
            this.tableLayoutPanelTagRationg.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanelTagRationg.ColumnCount = 5;
            this.tableLayoutPanelTagRationg.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTagRationg.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTagRationg.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTagRationg.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTagRationg.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTagRationg.Controls.Add(this.radioButtonRating1, 0, 0);
            this.tableLayoutPanelTagRationg.Controls.Add(this.radioButtonRating5, 4, 0);
            this.tableLayoutPanelTagRationg.Controls.Add(this.radioButtonRating2, 1, 0);
            this.tableLayoutPanelTagRationg.Controls.Add(this.radioButtonRating4, 3, 0);
            this.tableLayoutPanelTagRationg.Controls.Add(this.radioButtonRating3, 2, 0);
            this.tableLayoutPanelTagRationg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelTagRationg.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelTagRationg.Name = "tableLayoutPanelTagRationg";
            this.tableLayoutPanelTagRationg.RowCount = 1;
            this.tableLayoutPanelTagRationg.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTagRationg.Size = new System.Drawing.Size(262, 33);
            this.tableLayoutPanelTagRationg.TabIndex = 5;
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
            this.radioButtonRating1.Location = new System.Drawing.Point(3, 3);
            this.radioButtonRating1.Name = "radioButtonRating1";
            this.radioButtonRating1.Size = new System.Drawing.Size(32, 18);
            this.radioButtonRating1.TabIndex = 0;
            this.radioButtonRating1.Values.Text = "1";
            this.radioButtonRating1.CheckedChanged += new System.EventHandler(this.radioButtonRating1_CheckedChanged);
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
            this.radioButtonRating5.Location = new System.Drawing.Point(155, 3);
            this.radioButtonRating5.Name = "radioButtonRating5";
            this.radioButtonRating5.Size = new System.Drawing.Size(32, 18);
            this.radioButtonRating5.TabIndex = 4;
            this.radioButtonRating5.Values.Text = "5";
            this.radioButtonRating5.CheckedChanged += new System.EventHandler(this.radioButtonRating5_CheckedChanged);
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
            this.radioButtonRating2.Location = new System.Drawing.Point(41, 3);
            this.radioButtonRating2.Name = "radioButtonRating2";
            this.radioButtonRating2.Size = new System.Drawing.Size(32, 18);
            this.radioButtonRating2.TabIndex = 1;
            this.radioButtonRating2.Values.Text = "2";
            this.radioButtonRating2.CheckedChanged += new System.EventHandler(this.radioButtonRating2_CheckedChanged);
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
            this.radioButtonRating4.Location = new System.Drawing.Point(117, 3);
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
            this.radioButtonRating3.Location = new System.Drawing.Point(79, 3);
            this.radioButtonRating3.Name = "radioButtonRating3";
            this.radioButtonRating3.Size = new System.Drawing.Size(32, 18);
            this.radioButtonRating3.TabIndex = 2;
            this.radioButtonRating3.Values.Text = "3";
            this.radioButtonRating3.CheckedChanged += new System.EventHandler(this.radioButtonRating3_CheckedChanged);
            // 
            // kryptonWorkspaceCellToolboxTagsDetails
            // 
            this.kryptonWorkspaceCellToolboxTagsDetails.AllowDroppingPages = false;
            this.kryptonWorkspaceCellToolboxTagsDetails.AllowPageDrag = true;
            this.kryptonWorkspaceCellToolboxTagsDetails.AllowPageReorder = false;
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
            this.kryptonWorkspaceCellToolboxTagsDetails.MaximizeRestoreClicked += new System.EventHandler(this.kryptonWorkspaceCellToolboxTagsDetails_MaximizeRestoreClicked);
            // 
            // kryptonWorkspaceCellToolboxTagsKeywords
            // 
            this.kryptonWorkspaceCellToolboxTagsKeywords.AllowDroppingPages = false;
            this.kryptonWorkspaceCellToolboxTagsKeywords.AllowPageDrag = true;
            this.kryptonWorkspaceCellToolboxTagsKeywords.AllowPageReorder = false;
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
            this.kryptonWorkspaceCellToolboxTagsKeywords.MaximizeRestoreClicked += new System.EventHandler(this.kryptonWorkspaceCellToolboxTagsKeywords_MaximizeRestoreClicked);
            // 
            // kryptonPageToolboxTagsKeywords
            // 
            this.kryptonPageToolboxTagsKeywords.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageToolboxTagsKeywords.Controls.Add(this.dataGridViewTagsAndKeywords);
            this.kryptonPageToolboxTagsKeywords.Flags = 65534;
            this.kryptonPageToolboxTagsKeywords.LastVisibleSet = true;
            this.kryptonPageToolboxTagsKeywords.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageToolboxTagsKeywords.Name = "kryptonPageToolboxTagsKeywords";
            this.kryptonPageToolboxTagsKeywords.Size = new System.Drawing.Size(398, 329);
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
            this.dataGridViewTagsAndKeywords.Size = new System.Drawing.Size(398, 329);
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
            this.kryptonPageToolboxPeople.Size = new System.Drawing.Size(400, 751);
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
            this.dataGridViewPeople.Size = new System.Drawing.Size(400, 751);
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
            this.kryptonContextMenuItemAssignCompositeTag,
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
            // kryptonContextMenuItemAssignCompositeTag
            // 
            this.kryptonContextMenuItemAssignCompositeTag.Items.AddRange(new Krypton.Toolkit.KryptonContextMenuItemBase[] {
            this.kryptonContextMenuItemsAssignCompositeTagList});
            this.kryptonContextMenuItemAssignCompositeTag.Text = "Assign Composite Tag";
            // 
            // kryptonContextMenuItemsAssignCompositeTagList
            // 
            this.kryptonContextMenuItemsAssignCompositeTagList.Items.AddRange(new Krypton.Toolkit.KryptonContextMenuItemBase[] {
            this.kryptonContextMenuItemAssignCompositeTagExample});
            // 
            // kryptonContextMenuItemAssignCompositeTagExample
            // 
            this.kryptonContextMenuItemAssignCompositeTagExample.Text = "Assign Composite Tag Example";
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
            this.kryptonContextMenuImageSelect3,
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
            this.kryptonPageToolboxMap.Size = new System.Drawing.Size(400, 751);
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
            this.kryptonWorkspaceToolboxMap.Size = new System.Drawing.Size(400, 751);
            this.kryptonWorkspaceToolboxMap.TabIndex = 0;
            this.kryptonWorkspaceToolboxMap.TabStop = true;
            // 
            // kryptonPageToolboxMapProperties
            // 
            this.kryptonPageToolboxMapProperties.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageToolboxMapProperties.Controls.Add(this.tableLayoutPanelMap);
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
            // tableLayoutPanelMap
            // 
            this.tableLayoutPanelMap.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanelMap.ColumnCount = 2;
            this.tableLayoutPanelMap.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMap.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMap.Controls.Add(this.comboBoxGoogleLocationInterval, 1, 0);
            this.tableLayoutPanelMap.Controls.Add(this.comboBoxGoogleTimeZoneShift, 0, 0);
            this.tableLayoutPanelMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMap.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMap.Name = "tableLayoutPanelMap";
            this.tableLayoutPanelMap.RowCount = 1;
            this.tableLayoutPanelMap.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMap.Size = new System.Drawing.Size(398, 50);
            this.tableLayoutPanelMap.TabIndex = 11;
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
            this.comboBoxGoogleLocationInterval.Location = new System.Drawing.Point(131, 3);
            this.comboBoxGoogleLocationInterval.Name = "comboBoxGoogleLocationInterval";
            this.comboBoxGoogleLocationInterval.Size = new System.Drawing.Size(127, 18);
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
            this.comboBoxGoogleTimeZoneShift.Location = new System.Drawing.Point(3, 3);
            this.comboBoxGoogleTimeZoneShift.Name = "comboBoxGoogleTimeZoneShift";
            this.comboBoxGoogleTimeZoneShift.Size = new System.Drawing.Size(122, 18);
            this.comboBoxGoogleTimeZoneShift.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxGoogleTimeZoneShift.TabIndex = 13;
            this.comboBoxGoogleTimeZoneShift.SelectedIndexChanged += new System.EventHandler(this.comboBoxGoogleTimeZoneShift_SelectedIndexChanged);
            // 
            // kryptonWorkspaceCellToolboxMapProperties
            // 
            this.kryptonWorkspaceCellToolboxMapProperties.AllowDroppingPages = false;
            this.kryptonWorkspaceCellToolboxMapProperties.AllowPageDrag = true;
            this.kryptonWorkspaceCellToolboxMapProperties.AllowPageReorder = false;
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
            this.kryptonWorkspaceCellToolboxMapDetails.AllowDroppingPages = false;
            this.kryptonWorkspaceCellToolboxMapDetails.AllowPageDrag = true;
            this.kryptonWorkspaceCellToolboxMapDetails.AllowPageReorder = false;
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
            this.kryptonPageToolboxMapDetails.Size = new System.Drawing.Size(398, 334);
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
            this.dataGridViewMap.Size = new System.Drawing.Size(398, 334);
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
            this.kryptonWorkspaceCellToolboxMapBroswer.AllowDroppingPages = false;
            this.kryptonWorkspaceCellToolboxMapBroswer.AllowPageDrag = true;
            this.kryptonWorkspaceCellToolboxMapBroswer.AllowPageReorder = false;
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
            this.kryptonPageToolboxMapBroswer.Size = new System.Drawing.Size(398, 334);
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
            this.panelBrowser.Size = new System.Drawing.Size(398, 334);
            this.panelBrowser.TabIndex = 1;
            // 
            // kryptonWorkspaceCellToolboxMapBroswerProperties
            // 
            this.kryptonWorkspaceCellToolboxMapBroswerProperties.AllowDroppingPages = false;
            this.kryptonWorkspaceCellToolboxMapBroswerProperties.AllowPageDrag = true;
            this.kryptonWorkspaceCellToolboxMapBroswerProperties.AllowPageReorder = false;
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
            this.comboBoxMapZoomLevel.Size = new System.Drawing.Size(85, 18);
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
            this.kryptonPageToolboxDates.Size = new System.Drawing.Size(400, 751);
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
            this.dataGridViewDate.Size = new System.Drawing.Size(400, 751);
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
            this.kryptonPageToolboxExiftool.Size = new System.Drawing.Size(400, 751);
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
            this.dataGridViewExiftool.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewExiftool.KryptonContextMenu = this.kryptonContextMenuGenericBase;
            this.dataGridViewExiftool.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewExiftool.Name = "dataGridViewExiftool";
            this.dataGridViewExiftool.RowHeadersWidth = 51;
            this.dataGridViewExiftool.RowTemplate.Height = 24;
            this.dataGridViewExiftool.Size = new System.Drawing.Size(400, 751);
            this.dataGridViewExiftool.TabIndex = 0;
            this.dataGridViewExiftool.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewExifTool_CellBeginEdit);
            this.dataGridViewExiftool.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewExifTool_CellEnter);
            this.dataGridViewExiftool.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewExifTool_CellMouseClick);
            this.dataGridViewExiftool.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridViewExifTool_CellPainting);
            // 
            // kryptonPageToolboxWarnings
            // 
            this.kryptonPageToolboxWarnings.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageToolboxWarnings.Controls.Add(this.dataGridViewExiftoolWarning);
            this.kryptonPageToolboxWarnings.Flags = 65534;
            this.kryptonPageToolboxWarnings.LastVisibleSet = true;
            this.kryptonPageToolboxWarnings.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageToolboxWarnings.Name = "kryptonPageToolboxWarnings";
            this.kryptonPageToolboxWarnings.Size = new System.Drawing.Size(400, 751);
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
            this.dataGridViewExiftoolWarning.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewExiftoolWarning.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewExiftoolWarning.Name = "dataGridViewExiftoolWarning";
            this.dataGridViewExiftoolWarning.ReadOnly = true;
            this.dataGridViewExiftoolWarning.RowHeadersWidth = 51;
            this.dataGridViewExiftoolWarning.RowTemplate.Height = 24;
            this.dataGridViewExiftoolWarning.Size = new System.Drawing.Size(400, 751);
            this.dataGridViewExiftoolWarning.TabIndex = 0;
            this.dataGridViewExiftoolWarning.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewExifToolWarning_CellBeginEdit);
            this.dataGridViewExiftoolWarning.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewExifToolWarning_CellEnter);
            this.dataGridViewExiftoolWarning.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewExifToolWarning_CellMouseClick);
            this.dataGridViewExiftoolWarning.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridViewExifToolWarning_CellPainting);
            // 
            // kryptonPageToolboxProperties
            // 
            this.kryptonPageToolboxProperties.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageToolboxProperties.Controls.Add(this.dataGridViewProperties);
            this.kryptonPageToolboxProperties.Flags = 65534;
            this.kryptonPageToolboxProperties.LastVisibleSet = true;
            this.kryptonPageToolboxProperties.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageToolboxProperties.Name = "kryptonPageToolboxProperties";
            this.kryptonPageToolboxProperties.Size = new System.Drawing.Size(400, 751);
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
            this.dataGridViewProperties.Size = new System.Drawing.Size(400, 751);
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
            this.kryptonPageToolboxRename.Size = new System.Drawing.Size(400, 751);
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
            this.kryptonWorkspaceToolboxRename.Size = new System.Drawing.Size(400, 751);
            this.kryptonWorkspaceToolboxRename.TabIndex = 0;
            this.kryptonWorkspaceToolboxRename.TabStop = true;
            // 
            // kryptonPageToolboxRenameVariables
            // 
            this.kryptonPageToolboxRenameVariables.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageToolboxRenameVariables.AutoScroll = true;
            this.kryptonPageToolboxRenameVariables.Controls.Add(this.tableLayoutPanelRename);
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
            // tableLayoutPanelRename
            // 
            this.tableLayoutPanelRename.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanelRename.ColumnCount = 2;
            this.tableLayoutPanelRename.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelRename.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelRename.Controls.Add(this.buttonRenameSave, 1, 3);
            this.tableLayoutPanelRename.Controls.Add(this.buttonRenameUpdate, 0, 3);
            this.tableLayoutPanelRename.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanelRename.Controls.Add(this.checkBoxRenameShowFullPath, 1, 2);
            this.tableLayoutPanelRename.Controls.Add(this.comboBoxRenameVariableList, 1, 0);
            this.tableLayoutPanelRename.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanelRename.Controls.Add(this.textBoxRenameNewName, 1, 1);
            this.tableLayoutPanelRename.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelRename.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelRename.Name = "tableLayoutPanelRename";
            this.tableLayoutPanelRename.RowCount = 4;
            this.tableLayoutPanelRename.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRename.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRename.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRename.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRename.Size = new System.Drawing.Size(398, 148);
            this.tableLayoutPanelRename.TabIndex = 1;
            // 
            // buttonRenameSave
            // 
            this.buttonRenameSave.Location = new System.Drawing.Point(101, 77);
            this.buttonRenameSave.Name = "buttonRenameSave";
            this.buttonRenameSave.Size = new System.Drawing.Size(66, 22);
            this.buttonRenameSave.TabIndex = 3;
            this.buttonRenameSave.Values.Text = "Rename";
            this.buttonRenameSave.Click += new System.EventHandler(this.buttonRenameSave_Click);
            // 
            // buttonRenameUpdate
            // 
            this.buttonRenameUpdate.Location = new System.Drawing.Point(3, 77);
            this.buttonRenameUpdate.Name = "buttonRenameUpdate";
            this.buttonRenameUpdate.Size = new System.Drawing.Size(66, 22);
            this.buttonRenameUpdate.TabIndex = 2;
            this.buttonRenameUpdate.Values.Text = "Update";
            this.buttonRenameUpdate.Click += new System.EventHandler(this.buttonRenameUpdate_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(3, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 18);
            this.label2.TabIndex = 3;
            this.label2.Values.Text = "List of variables:";
            // 
            // checkBoxRenameShowFullPath
            // 
            this.checkBoxRenameShowFullPath.Location = new System.Drawing.Point(101, 53);
            this.checkBoxRenameShowFullPath.Name = "checkBoxRenameShowFullPath";
            this.checkBoxRenameShowFullPath.Size = new System.Drawing.Size(94, 18);
            this.checkBoxRenameShowFullPath.TabIndex = 5;
            this.checkBoxRenameShowFullPath.Values.Text = "Show full path";
            this.checkBoxRenameShowFullPath.CheckedChanged += new System.EventHandler(this.checkBoxRenameShowFullPath_CheckedChanged);
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
            this.comboBoxRenameVariableList.Location = new System.Drawing.Point(101, 3);
            this.comboBoxRenameVariableList.Name = "comboBoxRenameVariableList";
            this.comboBoxRenameVariableList.Size = new System.Drawing.Size(294, 18);
            this.comboBoxRenameVariableList.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxRenameVariableList.TabIndex = 0;
            this.comboBoxRenameVariableList.SelectionChangeCommitted += new System.EventHandler(this.comboBoxRenameVariableList_SelectionChangeCommitted);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 18);
            this.label1.TabIndex = 2;
            this.label1.Values.Text = "New file name:";
            // 
            // textBoxRenameNewName
            // 
            this.textBoxRenameNewName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxRenameNewName.Location = new System.Drawing.Point(101, 27);
            this.textBoxRenameNewName.Name = "textBoxRenameNewName";
            this.textBoxRenameNewName.Size = new System.Drawing.Size(294, 21);
            this.textBoxRenameNewName.TabIndex = 1;
            this.textBoxRenameNewName.Leave += new System.EventHandler(this.textBoxRenameNewName_Leave);
            // 
            // kryptonWorkspaceCellToolboxRenameVariables
            // 
            this.kryptonWorkspaceCellToolboxRenameVariables.AllowDroppingPages = false;
            this.kryptonWorkspaceCellToolboxRenameVariables.AllowPageDrag = true;
            this.kryptonWorkspaceCellToolboxRenameVariables.AllowPageReorder = false;
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
            this.kryptonWorkspaceCellToolboxRenameResult.AllowDroppingPages = false;
            this.kryptonWorkspaceCellToolboxRenameResult.AllowPageDrag = true;
            this.kryptonWorkspaceCellToolboxRenameResult.AllowPageReorder = false;
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
            this.kryptonPageToolboxRenameResult.Size = new System.Drawing.Size(398, 594);
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
            this.dataGridViewRename.Size = new System.Drawing.Size(398, 594);
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
            this.kryptonPageToolboxConvertAndMerge.Size = new System.Drawing.Size(400, 751);
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
            this.dataGridViewConvertAndMerge.Size = new System.Drawing.Size(400, 751);
            this.dataGridViewConvertAndMerge.TabIndex = 1;
            this.dataGridViewConvertAndMerge.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewConvertAndMerge_CellEnter);
            this.dataGridViewConvertAndMerge.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewConvertAndMerge_CellMouseClick);
            this.dataGridViewConvertAndMerge.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridViewConvertAndMerge_CellPainting);
            this.dataGridViewConvertAndMerge.DragDrop += new System.Windows.Forms.DragEventHandler(this.dataGridViewConvertAndMerge_DragDrop);
            this.dataGridViewConvertAndMerge.DragOver += new System.Windows.Forms.DragEventHandler(this.dataGridViewConvertAndMerge_DragOver);
            this.dataGridViewConvertAndMerge.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridViewConvertAndMerge_MouseDown);
            this.dataGridViewConvertAndMerge.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dataGridViewConvertAndMerge_MouseMove);
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
            // panelMediaPreview
            // 
            this.panelMediaPreview.Controls.Add(this.imageBoxPreview);
            this.panelMediaPreview.Controls.Add(this.videoView1);
            this.panelMediaPreview.Location = new System.Drawing.Point(236, 196);
            this.panelMediaPreview.Margin = new System.Windows.Forms.Padding(0);
            this.panelMediaPreview.Name = "panelMediaPreview";
            this.panelMediaPreview.Size = new System.Drawing.Size(147, 76);
            this.panelMediaPreview.TabIndex = 7;
            this.panelMediaPreview.Visible = false;
            // 
            // imageBoxPreview
            // 
            this.imageBoxPreview.BackColor = System.Drawing.Color.Black;
            this.imageBoxPreview.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.imageBoxPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageBoxPreview.GridDisplayMode = Cyotek.Windows.Forms.ImageBoxGridDisplayMode.None;
            this.imageBoxPreview.Location = new System.Drawing.Point(0, 0);
            this.imageBoxPreview.Name = "imageBoxPreview";
            this.imageBoxPreview.Size = new System.Drawing.Size(147, 76);
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
            this.videoView1.Size = new System.Drawing.Size(147, 76);
            this.videoView1.TabIndex = 2;
            this.videoView1.Text = "videoView1";
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
            this.kryptonRibbonQATButtonMediaPoster,
            this.kryptonRibbonQATButtonSelectPrevius,
            this.kryptonRibbonQATButtonSelectNext,
            this.kryptonRibbonQATButtonSelectEqual,
            this.kryptonRibbonQATButtonSelectAll,
            this.kryptonRibbonQATButtonSelectNone,
            this.kryptonRibbonQATButtonSelectToggle,
            this.kryptonRibbonQATButtonMediaPlayerPrevious,
            this.kryptonRibbonQATButtonMediaPlayerNext,
            this.kryptonRibbonQATButtonMediaPlayerPlay,
            this.kryptonRibbonQATButtonMediaPlayerPause,
            this.kryptonRibbonQATButtonMediaPlayerStop,
            this.kryptonRibbonQATButtonMediaPlayerFastBackwards,
            this.kryptonRibbonQATButtonMediaPlayerFastForward,
            this.kryptonRibbonQATButtonMediaPlayerSlideshowPlay});
            this.kryptonRibbonMain.QATUserChange = false;
            this.kryptonRibbonMain.RibbonAppButton.AppButtonImage = global::PhotoTagsSynchronizer.Properties.Resources.AppIcon;
            this.kryptonRibbonMain.RibbonTabs.AddRange(new Krypton.Ribbon.KryptonRibbonTab[] {
            this.kryptonRibbonTabHome,
            this.kryptonRibbonTabView,
            this.kryptonRibbonTabSelect,
            this.kryptonRibbonTabTools,
            this.kryptonRibbonTabPreview});
            this.kryptonRibbonMain.SelectedTab = this.kryptonRibbonTabHome;
            this.kryptonRibbonMain.Size = new System.Drawing.Size(1214, 115);
            this.kryptonRibbonMain.TabIndex = 12;
            this.kryptonRibbonMain.SelectedTabChanged += new System.EventHandler(this.kryptonRibbonMain_SelectedTabChanged);
            // 
            // kryptonRibbonQATButtonSave
            // 
            this.kryptonRibbonQATButtonSave.Image = global::PhotoTagsSynchronizer.Properties.Resources.MetadataSave16x16;
            this.kryptonRibbonQATButtonSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.kryptonRibbonQATButtonSave.ToolTipBody = "Save your changes back to media files.";
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
            // kryptonRibbonQATButtonSelectPrevius
            // 
            this.kryptonRibbonQATButtonSelectPrevius.Image = global::PhotoTagsSynchronizer.Properties.Resources.SelectPrevious16x16;
            this.kryptonRibbonQATButtonSelectPrevius.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.Left)));
            this.kryptonRibbonQATButtonSelectPrevius.ToolTipBody = "Select Previous group of media files. Using the properties set in this ribbon.";
            this.kryptonRibbonQATButtonSelectPrevius.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.SelectPrevious;
            this.kryptonRibbonQATButtonSelectPrevius.ToolTipTitle = "Select Previous (Ctrl+Shift+Left)";
            this.kryptonRibbonQATButtonSelectPrevius.Click += new System.EventHandler(this.kryptonRibbonQATButtonSelectPrevius_Click);
            // 
            // kryptonRibbonQATButtonSelectNext
            // 
            this.kryptonRibbonQATButtonSelectNext.Image = global::PhotoTagsSynchronizer.Properties.Resources.SelectNext16x16;
            this.kryptonRibbonQATButtonSelectNext.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.Right)));
            this.kryptonRibbonQATButtonSelectNext.ToolTipBody = "Select Next group of media files. Using the properties set in this ribbon.";
            this.kryptonRibbonQATButtonSelectNext.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.SelectNext;
            this.kryptonRibbonQATButtonSelectNext.ToolTipTitle = "Select Next (Ctrl+Shift+Right)";
            this.kryptonRibbonQATButtonSelectNext.Click += new System.EventHandler(this.kryptonRibbonQATButtonSelectNext_Click);
            // 
            // kryptonRibbonQATButtonSelectEqual
            // 
            this.kryptonRibbonQATButtonSelectEqual.Image = global::PhotoTagsSynchronizer.Properties.Resources.SelectEqual16x16;
            this.kryptonRibbonQATButtonSelectEqual.ToolTipBody = "Select all media files that match criterias from selected files.";
            this.kryptonRibbonQATButtonSelectEqual.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.SelectEqual;
            this.kryptonRibbonQATButtonSelectEqual.ToolTipTitle = "Select match (Ctrl+Shift+Alt+A)";
            this.kryptonRibbonQATButtonSelectEqual.Click += new System.EventHandler(this.kryptonRibbonQATButtonSelectEqual_Click);
            // 
            // kryptonRibbonQATButtonSelectAll
            // 
            this.kryptonRibbonQATButtonSelectAll.Image = global::PhotoTagsSynchronizer.Properties.Resources.SelectAll16x16;
            this.kryptonRibbonQATButtonSelectAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.kryptonRibbonQATButtonSelectAll.ToolTipBody = "Select all media files";
            this.kryptonRibbonQATButtonSelectAll.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.SelectAll;
            this.kryptonRibbonQATButtonSelectAll.ToolTipTitle = "Select All (Ctrl+A)";
            this.kryptonRibbonQATButtonSelectAll.Click += new System.EventHandler(this.kryptonRibbonQATButtonSelectAll_Click);
            // 
            // kryptonRibbonQATButtonSelectNone
            // 
            this.kryptonRibbonQATButtonSelectNone.Image = global::PhotoTagsSynchronizer.Properties.Resources.SelectNone16x16;
            this.kryptonRibbonQATButtonSelectNone.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.A)));
            this.kryptonRibbonQATButtonSelectNone.ToolTipBody = "Deselect / Select none of the media files";
            this.kryptonRibbonQATButtonSelectNone.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.SelectNone;
            this.kryptonRibbonQATButtonSelectNone.ToolTipTitle = "Select None (Ctrl+Shift+A)";
            this.kryptonRibbonQATButtonSelectNone.Click += new System.EventHandler(this.kryptonRibbonQATButtonSelectNone_Click);
            // 
            // kryptonRibbonQATButtonSelectToggle
            // 
            this.kryptonRibbonQATButtonSelectToggle.Image = global::PhotoTagsSynchronizer.Properties.Resources.SelectToggle16x16;
            this.kryptonRibbonQATButtonSelectToggle.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.A)));
            this.kryptonRibbonQATButtonSelectToggle.ToolTipBody = "Invert selection of the media files.";
            this.kryptonRibbonQATButtonSelectToggle.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.SelectToggle;
            this.kryptonRibbonQATButtonSelectToggle.ToolTipTitle = "Invert Selection (Ctrl+Alt+A)";
            this.kryptonRibbonQATButtonSelectToggle.Click += new System.EventHandler(this.kryptonRibbonQATButtonSelectToggle_Click);
            // 
            // kryptonRibbonQATButtonMediaPlayerPrevious
            // 
            this.kryptonRibbonQATButtonMediaPlayerPrevious.Image = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerPrevious16x16;
            this.kryptonRibbonQATButtonMediaPlayerPrevious.Visible = false;
            this.kryptonRibbonQATButtonMediaPlayerPrevious.Click += new System.EventHandler(this.kryptonRibbonQATButtonMediaPlayerPrevious_Click);
            // 
            // kryptonRibbonQATButtonMediaPlayerNext
            // 
            this.kryptonRibbonQATButtonMediaPlayerNext.Image = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerNext16x16;
            this.kryptonRibbonQATButtonMediaPlayerNext.Visible = false;
            this.kryptonRibbonQATButtonMediaPlayerNext.Click += new System.EventHandler(this.kryptonRibbonQATButtonMediaPlayerNext_Click);
            // 
            // kryptonRibbonQATButtonMediaPlayerPlay
            // 
            this.kryptonRibbonQATButtonMediaPlayerPlay.Image = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerPlay16x16;
            this.kryptonRibbonQATButtonMediaPlayerPlay.Visible = false;
            this.kryptonRibbonQATButtonMediaPlayerPlay.Click += new System.EventHandler(this.kryptonRibbonQATButtonMediaPlayerPlay_Click);
            // 
            // kryptonRibbonQATButtonMediaPlayerPause
            // 
            this.kryptonRibbonQATButtonMediaPlayerPause.Image = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerPause16x16;
            this.kryptonRibbonQATButtonMediaPlayerPause.Visible = false;
            this.kryptonRibbonQATButtonMediaPlayerPause.Click += new System.EventHandler(this.kryptonRibbonQATButtonMediaPlayerPause_Click);
            // 
            // kryptonRibbonQATButtonMediaPlayerStop
            // 
            this.kryptonRibbonQATButtonMediaPlayerStop.Image = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerStop16x16;
            this.kryptonRibbonQATButtonMediaPlayerStop.Visible = false;
            this.kryptonRibbonQATButtonMediaPlayerStop.Click += new System.EventHandler(this.kryptonRibbonQATButtonMediaPlayerStop_Click);
            // 
            // kryptonRibbonQATButtonMediaPlayerFastBackwards
            // 
            this.kryptonRibbonQATButtonMediaPlayerFastBackwards.Image = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerFastBackward16x16;
            this.kryptonRibbonQATButtonMediaPlayerFastBackwards.Visible = false;
            this.kryptonRibbonQATButtonMediaPlayerFastBackwards.Click += new System.EventHandler(this.kryptonRibbonQATButtonMediaPlayerFastBackwards_Click);
            // 
            // kryptonRibbonQATButtonMediaPlayerFastForward
            // 
            this.kryptonRibbonQATButtonMediaPlayerFastForward.Image = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerFastForward16x16;
            this.kryptonRibbonQATButtonMediaPlayerFastForward.Visible = false;
            this.kryptonRibbonQATButtonMediaPlayerFastForward.Click += new System.EventHandler(this.kryptonRibbonQATButtonMediaPlayerFastForward_Click);
            // 
            // kryptonRibbonQATButtonMediaPlayerSlideshowPlay
            // 
            this.kryptonRibbonQATButtonMediaPlayerSlideshowPlay.Image = global::PhotoTagsSynchronizer.Properties.Resources.MediaPreviewSlideshowPlay16x16;
            this.kryptonRibbonQATButtonMediaPlayerSlideshowPlay.Visible = false;
            this.kryptonRibbonQATButtonMediaPlayerSlideshowPlay.Click += new System.EventHandler(this.kryptonRibbonQATButtonMediaPlayerSlideshowPlay_Click);
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
            this.kryptonRibbonGroupHomeClipboard.DialogBoxLauncher = false;
            this.kryptonRibbonGroupHomeClipboard.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupTripleHomeCopyCutPaste,
            this.kryptonRibbonGroupTripleHomeUndoRedo});
            this.kryptonRibbonGroupHomeClipboard.KeyTipDialogLauncher = "Q";
            this.kryptonRibbonGroupHomeClipboard.KeyTipGroup = "Q";
            this.kryptonRibbonGroupHomeClipboard.TextLine1 = "Clipboard";
            // 
            // kryptonRibbonGroupTripleHomeCopyCutPaste
            // 
            this.kryptonRibbonGroupTripleHomeCopyCutPaste.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButtonHomeCopy,
            this.kryptonRibbonGroupButtonHomeCut,
            this.kryptonRibbonGroupButtonHomePaste});
            this.kryptonRibbonGroupTripleHomeCopyCutPaste.MaximumSize = Krypton.Ribbon.GroupItemSize.Medium;
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
            // kryptonRibbonGroupTripleHomeUndoRedo
            // 
            this.kryptonRibbonGroupTripleHomeUndoRedo.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButtonHomeUndo,
            this.kryptonRibbonGroupButtonRedo});
            this.kryptonRibbonGroupTripleHomeUndoRedo.MaximumSize = Krypton.Ribbon.GroupItemSize.Medium;
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
            this.kryptonRibbonGroupHomeManage.DialogBoxLauncher = false;
            this.kryptonRibbonGroupHomeManage.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupTripleHomeCopy,
            this.kryptonRibbonGroupTripleHomeSortFindAndSearch});
            this.kryptonRibbonGroupHomeManage.KeyTipDialogLauncher = "Q";
            this.kryptonRibbonGroupHomeManage.KeyTipGroup = "Q";
            this.kryptonRibbonGroupHomeManage.TextLine1 = "Manage";
            // 
            // kryptonRibbonGroupTripleHomeCopy
            // 
            this.kryptonRibbonGroupTripleHomeCopy.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButtonHomeCopyText,
            this.kryptonRibbonGroupButtonHomeFastCopyNoOverwrite,
            this.kryptonRibbonGroupButtonHomeFastCopyOverwrite});
            this.kryptonRibbonGroupTripleHomeCopy.MaximumSize = Krypton.Ribbon.GroupItemSize.Medium;
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
            // kryptonRibbonGroupTripleHomeSortFindAndSearch
            // 
            this.kryptonRibbonGroupTripleHomeSortFindAndSearch.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButtonHomeFind,
            this.kryptonRibbonGroupButtonHomeReplace,
            this.kryptonRibbonGroupButtonHomeSortColumn});
            this.kryptonRibbonGroupTripleHomeSortFindAndSearch.MaximumSize = Krypton.Ribbon.GroupItemSize.Medium;
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
            // kryptonRibbonGroupButtonHomeSortColumn
            // 
            this.kryptonRibbonGroupButtonHomeSortColumn.ButtonType = Krypton.Ribbon.GroupButtonType.DropDown;
            this.kryptonRibbonGroupButtonHomeSortColumn.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.FileSystemColumnSort;
            this.kryptonRibbonGroupButtonHomeSortColumn.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.FileSystemColumnSort;
            this.kryptonRibbonGroupButtonHomeSortColumn.KeyTip = "S";
            this.kryptonRibbonGroupButtonHomeSortColumn.TextLine1 = "Sort";
            this.kryptonRibbonGroupButtonHomeSortColumn.ToolTipBody = "Select what column your want to sort the mediafiles";
            this.kryptonRibbonGroupButtonHomeSortColumn.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.FileSystemColumnSort;
            this.kryptonRibbonGroupButtonHomeSortColumn.ToolTipTitle = "Sort Column ";
            // 
            // kryptonRibbonGroupHomeFileSystem
            // 
            this.kryptonRibbonGroupHomeFileSystem.DialogBoxLauncher = false;
            this.kryptonRibbonGroupHomeFileSystem.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupTripleHomeDeleteRenameRefresh,
            this.kryptonRibbonGroupTripleHomeOpenWith,
            this.kryptonRibbonGroupTripleHomeRunEdit});
            this.kryptonRibbonGroupHomeFileSystem.KeyTipDialogLauncher = "Q";
            this.kryptonRibbonGroupHomeFileSystem.KeyTipGroup = "Q";
            this.kryptonRibbonGroupHomeFileSystem.TextLine1 = "Organise";
            // 
            // kryptonRibbonGroupTripleHomeDeleteRenameRefresh
            // 
            this.kryptonRibbonGroupTripleHomeDeleteRenameRefresh.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButtonHomeFileSystemDelete,
            this.kryptonRibbonGroupButtonHomeFileSystemRename,
            this.kryptonRibbonGroupButtonHomeFileSystemRefresh});
            this.kryptonRibbonGroupTripleHomeDeleteRenameRefresh.MaximumSize = Krypton.Ribbon.GroupItemSize.Medium;
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
            // kryptonRibbonGroupTripleHomeOpenWith
            // 
            this.kryptonRibbonGroupTripleHomeOpenWith.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButtonHomeFileSystemOpen,
            this.kryptonRibbonGroupButtonHomeFileSystemOpenWith,
            this.kryptonRibbonGroupButtonFileSystemOpenAssociateDialog});
            this.kryptonRibbonGroupTripleHomeOpenWith.MaximumSize = Krypton.Ribbon.GroupItemSize.Medium;
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
            // kryptonRibbonGroupTripleHomeRunEdit
            // 
            this.kryptonRibbonGroupTripleHomeRunEdit.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButtonHomeFileSystemOpenExplorer,
            this.kryptonRibbonGroupButtonFileSystemRunCommand,
            this.kryptonRibbonGroupButtonHomeFileSystemEdit});
            this.kryptonRibbonGroupTripleHomeRunEdit.MaximumSize = Krypton.Ribbon.GroupItemSize.Medium;
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
            this.kryptonRibbonGroupHomeRotate.DialogBoxLauncher = false;
            this.kryptonRibbonGroupHomeRotate.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupTripleHomeRotate});
            this.kryptonRibbonGroupHomeRotate.KeyTipDialogLauncher = "Q";
            this.kryptonRibbonGroupHomeRotate.KeyTipGroup = "Q";
            this.kryptonRibbonGroupHomeRotate.TextLine1 = "Rotate";
            // 
            // kryptonRibbonGroupTripleHomeRotate
            // 
            this.kryptonRibbonGroupTripleHomeRotate.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButtonMediaFileRotate90CCW,
            this.kryptonRibbonGroupButtonMediaFileRotate180,
            this.kryptonRibbonGroupButtonMediaFileRotate90CW});
            this.kryptonRibbonGroupTripleHomeRotate.MaximumSize = Krypton.Ribbon.GroupItemSize.Medium;
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
            this.kryptonRibbonGroupHomeMetadata.DialogBoxLauncher = false;
            this.kryptonRibbonGroupHomeMetadata.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupTripleHomeAutoCorrect,
            this.kryptonRibbonGroupTripleHomeMetadataRefresh,
            this.kryptonRibbonGroupTripleHomeTriState});
            this.kryptonRibbonGroupHomeMetadata.KeyTipDialogLauncher = "Q";
            this.kryptonRibbonGroupHomeMetadata.KeyTipGroup = "Q";
            this.kryptonRibbonGroupHomeMetadata.TextLine1 = "Metadata";
            // 
            // kryptonRibbonGroupTripleHomeAutoCorrect
            // 
            this.kryptonRibbonGroupTripleHomeAutoCorrect.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
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
            // kryptonRibbonGroupTripleHomeMetadataRefresh
            // 
            this.kryptonRibbonGroupTripleHomeMetadataRefresh.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButtonHomeMetadataRefresh,
            this.kryptonRibbonGroupButtonHomeMetadataReload});
            this.kryptonRibbonGroupTripleHomeMetadataRefresh.MaximumSize = Krypton.Ribbon.GroupItemSize.Medium;
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
            // kryptonRibbonGroupTripleHomeTriState
            // 
            this.kryptonRibbonGroupTripleHomeTriState.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButtonHomeTagSelectOn,
            this.kryptonRibbonGroupButtonHomeTagSelectToggle,
            this.kryptonRibbonGroupButtonHomeTagSelectOff});
            this.kryptonRibbonGroupTripleHomeTriState.MaximumSize = Krypton.Ribbon.GroupItemSize.Medium;
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
            this.kryptonRibbonGroupHomeSave.DialogBoxLauncher = false;
            this.kryptonRibbonGroupHomeSave.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupTripleHomeSave});
            this.kryptonRibbonGroupHomeSave.TextLine1 = "Save";
            // 
            // kryptonRibbonGroupTripleHomeSave
            // 
            this.kryptonRibbonGroupTripleHomeSave.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
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
            this.kryptonRibbonGroupViewViewModes.DialogBoxLauncher = false;
            this.kryptonRibbonGroupViewViewModes.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupTripleViewGalleryDetailsPane,
            this.kryptonRibbonGroupTripleViewThumbnailsMode,
            this.kryptonRibbonGroupSeparator4,
            this.kryptonRibbonGroupTripleViewColumnsSort});
            this.kryptonRibbonGroupViewViewModes.TextLine1 = "Media view";
            // 
            // kryptonRibbonGroupTripleViewGalleryDetailsPane
            // 
            this.kryptonRibbonGroupTripleViewGalleryDetailsPane.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
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
            // kryptonRibbonGroupTripleViewThumbnailsMode
            // 
            this.kryptonRibbonGroupTripleViewThumbnailsMode.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
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
            // kryptonRibbonGroupTripleViewColumnsSort
            // 
            this.kryptonRibbonGroupTripleViewColumnsSort.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
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
            this.kryptonRibbonGroupViewThumbnailSize.DialogBoxLauncher = false;
            this.kryptonRibbonGroupViewThumbnailSize.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupTripleViewTumbnailSizeLargeMedium,
            this.kryptonRibbonGroupTripleViewThumbnailSizeXSmall});
            this.kryptonRibbonGroupViewThumbnailSize.TextLine1 = "Thumbnail size";
            // 
            // kryptonRibbonGroupTripleViewTumbnailSizeLargeMedium
            // 
            this.kryptonRibbonGroupTripleViewTumbnailSizeLargeMedium.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButtonThumbnailSizeXLarge,
            this.kryptonRibbonGroupButtonThumbnailSizeLarge,
            this.kryptonRibbonGroupButtonThumbnailSizeMedium});
            this.kryptonRibbonGroupTripleViewTumbnailSizeLargeMedium.MaximumSize = Krypton.Ribbon.GroupItemSize.Medium;
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
            // kryptonRibbonGroupTripleViewThumbnailSizeXSmall
            // 
            this.kryptonRibbonGroupTripleViewThumbnailSizeXSmall.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButtonThumbnailSizeSmall,
            this.kryptonRibbonGroupButtonThumbnailSizeXSmall});
            this.kryptonRibbonGroupTripleViewThumbnailSizeXSmall.MaximumSize = Krypton.Ribbon.GroupItemSize.Medium;
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
            this.kryptonRibbonGroupViewCellSize.DialogBoxLauncher = false;
            this.kryptonRibbonGroupViewCellSize.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupTripleViewGridSize});
            this.kryptonRibbonGroupViewCellSize.TextLine1 = "Cell size";
            // 
            // kryptonRibbonGroupTripleViewGridSize
            // 
            this.kryptonRibbonGroupTripleViewGridSize.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
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
            this.kryptonRibbonGroupViewColumns.DialogBoxLauncher = false;
            this.kryptonRibbonGroupViewColumns.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupTripleViewShowHideColumns});
            this.kryptonRibbonGroupViewColumns.TextLine1 = "Columns";
            // 
            // kryptonRibbonGroupTripleViewShowHideColumns
            // 
            this.kryptonRibbonGroupTripleViewShowHideColumns.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
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
            this.kryptonRibbonGroupViewRows.DialogBoxLauncher = false;
            this.kryptonRibbonGroupViewRows.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupTripleViewShowHideRows});
            this.kryptonRibbonGroupViewRows.TextLine1 = "Rows";
            // 
            // kryptonRibbonGroupTripleViewShowHideRows
            // 
            this.kryptonRibbonGroupTripleViewShowHideRows.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
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
            this.kryptonRibbonGroup14.DialogBoxLauncher = false;
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
            this.kryptonRibbonGroup8,
            this.kryptonRibbonGroup7});
            this.kryptonRibbonTabSelect.KeyTip = "S";
            this.kryptonRibbonTabSelect.Text = "Select";
            // 
            // kryptonRibbonGroupImageListViewSelect
            // 
            this.kryptonRibbonGroupImageListViewSelect.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupTripleSelectForwardBackwards,
            this.kryptonRibbonGroupSeparator2,
            this.kryptonRibbonGroupTripleSelectAllNoneToggle});
            this.kryptonRibbonGroupImageListViewSelect.TextLine1 = "Select group";
            // 
            // kryptonRibbonGroupTripleSelectForwardBackwards
            // 
            this.kryptonRibbonGroupTripleSelectForwardBackwards.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButtonSelectBackwards,
            this.kryptonRibbonGroupButtonSelectForwards,
            this.kryptonRibbonGroupButtonSelectEqual});
            // 
            // kryptonRibbonGroupButtonSelectBackwards
            // 
            this.kryptonRibbonGroupButtonSelectBackwards.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.SelectPrevious;
            this.kryptonRibbonGroupButtonSelectBackwards.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.SelectPrevious;
            this.kryptonRibbonGroupButtonSelectBackwards.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.Left)));
            this.kryptonRibbonGroupButtonSelectBackwards.TextLine1 = "Select";
            this.kryptonRibbonGroupButtonSelectBackwards.TextLine2 = "Backwards";
            this.kryptonRibbonGroupButtonSelectBackwards.ToolTipBody = "Select Previous group of media files. Using the properties set in this ribbon.";
            this.kryptonRibbonGroupButtonSelectBackwards.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.SelectPrevious;
            this.kryptonRibbonGroupButtonSelectBackwards.ToolTipTitle = "Select Backwards (Ctrl+Shift+Left)";
            this.kryptonRibbonGroupButtonSelectBackwards.Click += new System.EventHandler(this.kryptonRibbonGroupButtonSelectBackwards_Click);
            // 
            // kryptonRibbonGroupButtonSelectForwards
            // 
            this.kryptonRibbonGroupButtonSelectForwards.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.SelectNext;
            this.kryptonRibbonGroupButtonSelectForwards.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.SelectNext;
            this.kryptonRibbonGroupButtonSelectForwards.KeyTip = "F";
            this.kryptonRibbonGroupButtonSelectForwards.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.Right)));
            this.kryptonRibbonGroupButtonSelectForwards.TextLine1 = "Select";
            this.kryptonRibbonGroupButtonSelectForwards.TextLine2 = "Forwards";
            this.kryptonRibbonGroupButtonSelectForwards.ToolTipBody = "Select Next group of media files. Using the properties set in this ribbon.";
            this.kryptonRibbonGroupButtonSelectForwards.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.SelectNext;
            this.kryptonRibbonGroupButtonSelectForwards.ToolTipTitle = "Select Forwards (Ctrl+Shift+Right)";
            this.kryptonRibbonGroupButtonSelectForwards.Click += new System.EventHandler(this.kryptonRibbonGroupButtonSelectForwards_Click);
            // 
            // kryptonRibbonGroupButtonSelectEqual
            // 
            this.kryptonRibbonGroupButtonSelectEqual.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.SelectEqual;
            this.kryptonRibbonGroupButtonSelectEqual.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.SelectEqual;
            this.kryptonRibbonGroupButtonSelectEqual.ShortcutKeys = ((System.Windows.Forms.Keys)((((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.A)));
            this.kryptonRibbonGroupButtonSelectEqual.TextLine1 = "Select";
            this.kryptonRibbonGroupButtonSelectEqual.TextLine2 = "match";
            this.kryptonRibbonGroupButtonSelectEqual.ToolTipBody = "Select all media files that match criterias from selected files.";
            this.kryptonRibbonGroupButtonSelectEqual.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.SelectEqual;
            this.kryptonRibbonGroupButtonSelectEqual.ToolTipTitle = "Select match (Ctrl+Alt+Shift+A)";
            this.kryptonRibbonGroupButtonSelectEqual.Click += new System.EventHandler(this.kryptonRibbonGroupButtonSelectEqual_Click);
            // 
            // kryptonRibbonGroupTripleSelectAllNoneToggle
            // 
            this.kryptonRibbonGroupTripleSelectAllNoneToggle.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButtonSelectAll,
            this.kryptonRibbonGroupButtonSelectNone,
            this.kryptonRibbonGroupButtonSelectToggle});
            // 
            // kryptonRibbonGroupButtonSelectAll
            // 
            this.kryptonRibbonGroupButtonSelectAll.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.SelectAll;
            this.kryptonRibbonGroupButtonSelectAll.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.SelectAll;
            this.kryptonRibbonGroupButtonSelectAll.KeyTip = "A";
            this.kryptonRibbonGroupButtonSelectAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.kryptonRibbonGroupButtonSelectAll.TextLine1 = "Select";
            this.kryptonRibbonGroupButtonSelectAll.TextLine2 = "all";
            this.kryptonRibbonGroupButtonSelectAll.ToolTipBody = "Select all media files";
            this.kryptonRibbonGroupButtonSelectAll.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.SelectAll;
            this.kryptonRibbonGroupButtonSelectAll.ToolTipTitle = "Select all (Ctrl+A)";
            this.kryptonRibbonGroupButtonSelectAll.Click += new System.EventHandler(this.kryptonRibbonGroupButtonSelectAll_Click);
            // 
            // kryptonRibbonGroupButtonSelectNone
            // 
            this.kryptonRibbonGroupButtonSelectNone.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.SelectNone;
            this.kryptonRibbonGroupButtonSelectNone.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.SelectNone;
            this.kryptonRibbonGroupButtonSelectNone.KeyTip = "N";
            this.kryptonRibbonGroupButtonSelectNone.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.A)));
            this.kryptonRibbonGroupButtonSelectNone.TextLine1 = "Select ";
            this.kryptonRibbonGroupButtonSelectNone.TextLine2 = "none";
            this.kryptonRibbonGroupButtonSelectNone.ToolTipBody = "Deselect / Select none of the media files";
            this.kryptonRibbonGroupButtonSelectNone.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.SelectNone;
            this.kryptonRibbonGroupButtonSelectNone.ToolTipTitle = "Select All (Ctrl+Shift+A)";
            this.kryptonRibbonGroupButtonSelectNone.Click += new System.EventHandler(this.kryptonRibbonGroupButtonSelectNone_Click);
            // 
            // kryptonRibbonGroupButtonSelectToggle
            // 
            this.kryptonRibbonGroupButtonSelectToggle.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.SelectToggle;
            this.kryptonRibbonGroupButtonSelectToggle.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.SelectToggle;
            this.kryptonRibbonGroupButtonSelectToggle.KeyTip = "I";
            this.kryptonRibbonGroupButtonSelectToggle.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.A)));
            this.kryptonRibbonGroupButtonSelectToggle.TextLine1 = "Invert ";
            this.kryptonRibbonGroupButtonSelectToggle.TextLine2 = "selection";
            this.kryptonRibbonGroupButtonSelectToggle.ToolTipBody = "Invert selection of the media files.";
            this.kryptonRibbonGroupButtonSelectToggle.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.SelectToggle;
            this.kryptonRibbonGroupButtonSelectToggle.ToolTipTitle = "Invert selection (Ctrl+Alt+A)";
            this.kryptonRibbonGroupButtonSelectToggle.Click += new System.EventHandler(this.kryptonRibbonGroupButtonSelectToggle_Click);
            // 
            // kryptonRibbonGroup2
            // 
            this.kryptonRibbonGroup2.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupLinesLocatonDatePriorities,
            this.kryptonRibbonGroupLinesSelectByDateInterval});
            this.kryptonRibbonGroup2.TextLine1 = "Date range";
            // 
            // kryptonRibbonGroupLinesLocatonDatePriorities
            // 
            this.kryptonRibbonGroupLinesLocatonDatePriorities.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupCheckBoxSelectFileCreated,
            this.kryptonRibbonGroupCheckBoxSelectMediaTaken,
            this.kryptonRibbonGroupCheckBoxSelectCheckAllDates});
            // 
            // kryptonRibbonGroupCheckBoxSelectFileCreated
            // 
            this.kryptonRibbonGroupCheckBoxSelectFileCreated.KeyTip = "PF";
            this.kryptonRibbonGroupCheckBoxSelectFileCreated.TextLine1 = "File Created";
            this.kryptonRibbonGroupCheckBoxSelectFileCreated.Click += new System.EventHandler(this.kryptonRibbonGroupCheckBoxSelectFileCreated_Click);
            // 
            // kryptonRibbonGroupCheckBoxSelectMediaTaken
            // 
            this.kryptonRibbonGroupCheckBoxSelectMediaTaken.KeyTip = "PM";
            this.kryptonRibbonGroupCheckBoxSelectMediaTaken.TextLine1 = "Media taken";
            // 
            // kryptonRibbonGroupCheckBoxSelectCheckAllDates
            // 
            this.kryptonRibbonGroupCheckBoxSelectCheckAllDates.TextLine1 = "Match all (otherwise minimum one)";
            this.kryptonRibbonGroupCheckBoxSelectCheckAllDates.Click += new System.EventHandler(this.kryptonRibbonGroupCheckBoxSelectCheckAllDates_Click);
            // 
            // kryptonRibbonGroupLinesSelectByDateInterval
            // 
            this.kryptonRibbonGroupLinesSelectByDateInterval.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupRadioButtonSelectDateRange1,
            this.kryptonRibbonGroupRadioButtonSelectDateRange3,
            this.kryptonRibbonGroupRadioButtonSelectDateRange7,
            this.kryptonRibbonGroupRadioButtonSelectDateRange14,
            this.kryptonRibbonGroupRadioButtonSelectDateRange30});
            // 
            // kryptonRibbonGroupRadioButtonSelectDateRange1
            // 
            this.kryptonRibbonGroupRadioButtonSelectDateRange1.KeyTip = "D1";
            this.kryptonRibbonGroupRadioButtonSelectDateRange1.TextLine1 = "1 day";
            this.kryptonRibbonGroupRadioButtonSelectDateRange1.Click += new System.EventHandler(this.kryptonRibbonGroupRadioButtonSelectDateRange1_Click);
            // 
            // kryptonRibbonGroupRadioButtonSelectDateRange3
            // 
            this.kryptonRibbonGroupRadioButtonSelectDateRange3.KeyTip = "D3";
            this.kryptonRibbonGroupRadioButtonSelectDateRange3.TextLine1 = "3 days";
            this.kryptonRibbonGroupRadioButtonSelectDateRange3.Click += new System.EventHandler(this.kryptonRibbonGroupRadioButtonSelectDateRange3_Click);
            // 
            // kryptonRibbonGroupRadioButtonSelectDateRange7
            // 
            this.kryptonRibbonGroupRadioButtonSelectDateRange7.KeyTip = "DW";
            this.kryptonRibbonGroupRadioButtonSelectDateRange7.TextLine1 = "Week";
            this.kryptonRibbonGroupRadioButtonSelectDateRange7.Click += new System.EventHandler(this.kryptonRibbonGroupRadioButtonSelectDateRange7_Click);
            // 
            // kryptonRibbonGroupRadioButtonSelectDateRange14
            // 
            this.kryptonRibbonGroupRadioButtonSelectDateRange14.KeyTip = "D2";
            this.kryptonRibbonGroupRadioButtonSelectDateRange14.TextLine1 = "2 weeks";
            this.kryptonRibbonGroupRadioButtonSelectDateRange14.Click += new System.EventHandler(this.kryptonRibbonGroupRadioButtonSelectDateRange14_Click);
            // 
            // kryptonRibbonGroupRadioButtonSelectDateRange30
            // 
            this.kryptonRibbonGroupRadioButtonSelectDateRange30.KeyTip = "DM";
            this.kryptonRibbonGroupRadioButtonSelectDateRange30.TextLine1 = "Month";
            this.kryptonRibbonGroupRadioButtonSelectDateRange30.Click += new System.EventHandler(this.kryptonRibbonGroupRadioButtonSelectDateRange30_Click);
            // 
            // kryptonRibbonGroup8
            // 
            this.kryptonRibbonGroup8.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupLinesSelectLocation});
            this.kryptonRibbonGroup8.TextLine1 = "Location";
            // 
            // kryptonRibbonGroupLinesSelectLocation
            // 
            this.kryptonRibbonGroupLinesSelectLocation.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupCheckBoxSelectLocationName,
            this.kryptonRibbonGroupCheckBoxSelectLocationCity,
            this.kryptonRibbonGroupCheckBoxSelectLocationStateRegion,
            this.kryptonRibbonGroupCheckBoxSelectLocationCountry,
            this.kryptonRibbonGroupCheckBoxSelectCheckAllLocations});
            // 
            // kryptonRibbonGroupCheckBoxSelectLocationName
            // 
            this.kryptonRibbonGroupCheckBoxSelectLocationName.KeyTip = "LN";
            this.kryptonRibbonGroupCheckBoxSelectLocationName.TextLine1 = "Location name";
            this.kryptonRibbonGroupCheckBoxSelectLocationName.Click += new System.EventHandler(this.kryptonRibbonGroupCheckBoxSelectLocationName_Click);
            // 
            // kryptonRibbonGroupCheckBoxSelectLocationCity
            // 
            this.kryptonRibbonGroupCheckBoxSelectLocationCity.KeyTip = "LI";
            this.kryptonRibbonGroupCheckBoxSelectLocationCity.TextLine1 = "City";
            this.kryptonRibbonGroupCheckBoxSelectLocationCity.Click += new System.EventHandler(this.kryptonRibbonGroupCheckBoxSelectLocationCity_Click);
            // 
            // kryptonRibbonGroupCheckBoxSelectLocationStateRegion
            // 
            this.kryptonRibbonGroupCheckBoxSelectLocationStateRegion.KeyTip = "LS";
            this.kryptonRibbonGroupCheckBoxSelectLocationStateRegion.TextLine1 = "State/Region";
            this.kryptonRibbonGroupCheckBoxSelectLocationStateRegion.Click += new System.EventHandler(this.kryptonRibbonGroupCheckBoxSelectLocationStateRegion_Click);
            // 
            // kryptonRibbonGroupCheckBoxSelectLocationCountry
            // 
            this.kryptonRibbonGroupCheckBoxSelectLocationCountry.KeyTip = "LO";
            this.kryptonRibbonGroupCheckBoxSelectLocationCountry.TextLine1 = "Country";
            this.kryptonRibbonGroupCheckBoxSelectLocationCountry.Click += new System.EventHandler(this.kryptonRibbonGroupCheckBoxSelectLocationCountry_Click);
            // 
            // kryptonRibbonGroupCheckBoxSelectCheckAllLocations
            // 
            this.kryptonRibbonGroupCheckBoxSelectCheckAllLocations.TextLine1 = "Match all (otherwise minimum one)";
            this.kryptonRibbonGroupCheckBoxSelectCheckAllLocations.Click += new System.EventHandler(this.kryptonRibbonGroupCheckBoxSelectCheckAllLocations_Click);
            // 
            // kryptonRibbonGroup7
            // 
            this.kryptonRibbonGroup7.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupLinesLocationAmount});
            this.kryptonRibbonGroup7.TextLine1 = "Media amount";
            // 
            // kryptonRibbonGroupLinesLocationAmount
            // 
            this.kryptonRibbonGroupLinesLocationAmount.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupRadioButtonSelectMediaAmount10,
            this.kryptonRibbonGroupRadioButtonSelectMediaAmount30,
            this.kryptonRibbonGroupRadioButtonSelectMediaAmount50,
            this.kryptonRibbonGroupRadioButtonSelectMediaAmount100});
            // 
            // kryptonRibbonGroupRadioButtonSelectMediaAmount10
            // 
            this.kryptonRibbonGroupRadioButtonSelectMediaAmount10.KeyTip = "M1";
            this.kryptonRibbonGroupRadioButtonSelectMediaAmount10.TextLine1 = "Select max";
            this.kryptonRibbonGroupRadioButtonSelectMediaAmount10.TextLine2 = "10 files";
            this.kryptonRibbonGroupRadioButtonSelectMediaAmount10.Click += new System.EventHandler(this.kryptonRibbonGroupRadioButtonSelectMediaAmount10_Click);
            // 
            // kryptonRibbonGroupRadioButtonSelectMediaAmount30
            // 
            this.kryptonRibbonGroupRadioButtonSelectMediaAmount30.KeyTip = "M3";
            this.kryptonRibbonGroupRadioButtonSelectMediaAmount30.TextLine1 = "Select max";
            this.kryptonRibbonGroupRadioButtonSelectMediaAmount30.TextLine2 = "30 files";
            this.kryptonRibbonGroupRadioButtonSelectMediaAmount30.Click += new System.EventHandler(this.kryptonRibbonGroupRadioButtonSelectMediaAmount30_Click);
            // 
            // kryptonRibbonGroupRadioButtonSelectMediaAmount50
            // 
            this.kryptonRibbonGroupRadioButtonSelectMediaAmount50.KeyTip = "M5";
            this.kryptonRibbonGroupRadioButtonSelectMediaAmount50.TextLine1 = "Select max";
            this.kryptonRibbonGroupRadioButtonSelectMediaAmount50.TextLine2 = "50 files";
            this.kryptonRibbonGroupRadioButtonSelectMediaAmount50.Click += new System.EventHandler(this.kryptonRibbonGroupRadioButtonSelectMediaAmount50_Click);
            // 
            // kryptonRibbonGroupRadioButtonSelectMediaAmount100
            // 
            this.kryptonRibbonGroupRadioButtonSelectMediaAmount100.KeyTip = "S0";
            this.kryptonRibbonGroupRadioButtonSelectMediaAmount100.TextLine1 = "Select max";
            this.kryptonRibbonGroupRadioButtonSelectMediaAmount100.TextLine2 = "100 files";
            this.kryptonRibbonGroupRadioButtonSelectMediaAmount100.Click += new System.EventHandler(this.kryptonRibbonGroupRadioButtonSelectMediaAmount100_Click);
            // 
            // kryptonRibbonTabTools
            // 
            this.kryptonRibbonTabTools.Groups.AddRange(new Krypton.Ribbon.KryptonRibbonGroup[] {
            this.kryptonRibbonGroupToolsMain,
            this.kryptonRibbonGroup1,
            this.kryptonRibbonGroupToolsProgressStatus});
            this.kryptonRibbonTabTools.Text = "Tools";
            // 
            // kryptonRibbonGroupToolsMain
            // 
            this.kryptonRibbonGroupToolsMain.DialogBoxLauncher = false;
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
            this.kryptonRibbonGroupButtonToolsImportLocations.KeyTip = "I";
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
            this.kryptonRibbonGroupButtonToolsWebScraping.KeyTip = "W";
            this.kryptonRibbonGroupButtonToolsWebScraping.TextLine1 = "Web";
            this.kryptonRibbonGroupButtonToolsWebScraping.TextLine2 = "Scraping";
            this.kryptonRibbonGroupButtonToolsWebScraping.ToolTipBody = "Fetch your own data from web albums that belongs to you.";
            this.kryptonRibbonGroupButtonToolsWebScraping.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.ToolsWebScraping32x32;
            this.kryptonRibbonGroupButtonToolsWebScraping.ToolTipTitle = "WebScraping";
            this.kryptonRibbonGroupButtonToolsWebScraping.Click += new System.EventHandler(this.kryptonRibbonGroupButtonToolsWebScraping_Click);
            // 
            // kryptonRibbonGroup1
            // 
            this.kryptonRibbonGroup1.DialogBoxLauncher = false;
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
            this.kryptonRibbonGroupButtonToolsConfig.KeyTip = "C";
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
            this.kryptonRibbonGroupButtonToolsAbout.KeyTip = "A";
            this.kryptonRibbonGroupButtonToolsAbout.TextLine1 = "About";
            this.kryptonRibbonGroupButtonToolsAbout.ToolTipBody = "Read about this application";
            this.kryptonRibbonGroupButtonToolsAbout.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.ToolsAbout32x32;
            this.kryptonRibbonGroupButtonToolsAbout.ToolTipTitle = "ABout";
            this.kryptonRibbonGroupButtonToolsAbout.Click += new System.EventHandler(this.kryptonRibbonGroupButtonToolsAbout_Click);
            // 
            // kryptonRibbonGroupToolsProgressStatus
            // 
            this.kryptonRibbonGroupToolsProgressStatus.DialogBoxLauncher = false;
            this.kryptonRibbonGroupToolsProgressStatus.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupTripleProgressStatusSave,
            this.kryptonRibbonGroupTripleToolsProgressStatusWork,
            this.kryptonRibbonGroupTripleProgressStatusBackground,
            this.kryptonRibbonGroupTriple1});
            this.kryptonRibbonGroupToolsProgressStatus.TextLine1 = "Progress and workload status";
            // 
            // kryptonRibbonGroupTripleProgressStatusSave
            // 
            this.kryptonRibbonGroupTripleProgressStatusSave.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupLabelToolsProgressSave,
            this.kryptonRibbonGroupCustomControlToolsProgressSave,
            this.kryptonRibbonGroupLabelToolsProgressSaveText});
            this.kryptonRibbonGroupTripleProgressStatusSave.MaximumSize = Krypton.Ribbon.GroupItemSize.Medium;
            this.kryptonRibbonGroupTripleProgressStatusSave.MinimumSize = Krypton.Ribbon.GroupItemSize.Medium;
            // 
            // kryptonRibbonGroupLabelToolsProgressSave
            // 
            this.kryptonRibbonGroupLabelToolsProgressSave.TextLine1 = "Writing:";
            // 
            // kryptonRibbonGroupCustomControlToolsProgressSave
            // 
            this.kryptonRibbonGroupCustomControlToolsProgressSave.CustomControl = null;
            // 
            // kryptonRibbonGroupLabelToolsProgressSaveText
            // 
            this.kryptonRibbonGroupLabelToolsProgressSaveText.TextLine1 = "Video convertion";
            // 
            // kryptonRibbonGroupTripleToolsProgressStatusWork
            // 
            this.kryptonRibbonGroupTripleToolsProgressStatusWork.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupLabelToolsProgressLazyloading,
            this.kryptonRibbonGroupCustomControlToolsProgressLazyloading});
            this.kryptonRibbonGroupTripleToolsProgressStatusWork.MaximumSize = Krypton.Ribbon.GroupItemSize.Medium;
            this.kryptonRibbonGroupTripleToolsProgressStatusWork.MinimumSize = Krypton.Ribbon.GroupItemSize.Medium;
            // 
            // kryptonRibbonGroupLabelToolsProgressLazyloading
            // 
            this.kryptonRibbonGroupLabelToolsProgressLazyloading.TextLine1 = "Loading:";
            // 
            // kryptonRibbonGroupCustomControlToolsProgressLazyloading
            // 
            this.kryptonRibbonGroupCustomControlToolsProgressLazyloading.CustomControl = null;
            // 
            // kryptonRibbonGroupTripleProgressStatusBackground
            // 
            this.kryptonRibbonGroupTripleProgressStatusBackground.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupLabelToolsProgressBackground,
            this.kryptonRibbonGroupCustomControlToolsProgressBackground,
            this.kryptonRibbonGroupLabelToolsProgressBackgroundBackgroundProcessText});
            this.kryptonRibbonGroupTripleProgressStatusBackground.MaximumSize = Krypton.Ribbon.GroupItemSize.Medium;
            this.kryptonRibbonGroupTripleProgressStatusBackground.MinimumSize = Krypton.Ribbon.GroupItemSize.Medium;
            // 
            // kryptonRibbonGroupLabelToolsProgressBackground
            // 
            this.kryptonRibbonGroupLabelToolsProgressBackground.TextLine1 = "Backgound:";
            // 
            // kryptonRibbonGroupCustomControlToolsProgressBackground
            // 
            this.kryptonRibbonGroupCustomControlToolsProgressBackground.CustomControl = null;
            // 
            // kryptonRibbonGroupLabelToolsProgressBackgroundBackgroundProcessText
            // 
            this.kryptonRibbonGroupLabelToolsProgressBackgroundBackgroundProcessText.TextLine1 = "Nothing...";
            // 
            // kryptonRibbonGroupTriple1
            // 
            this.kryptonRibbonGroupTriple1.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupLabelCurrentActionsHeading,
            this.kryptonRibbonGroupLabelToolsCurrentActions,
            this.kryptonRibbonGroupButtonToolsProgressLazyloadingShowStatus});
            this.kryptonRibbonGroupTriple1.MaximumSize = Krypton.Ribbon.GroupItemSize.Medium;
            this.kryptonRibbonGroupTriple1.MinimumSize = Krypton.Ribbon.GroupItemSize.Medium;
            // 
            // kryptonRibbonGroupLabelCurrentActionsHeading
            // 
            this.kryptonRibbonGroupLabelCurrentActionsHeading.TextLine1 = "Current task:";
            // 
            // kryptonRibbonGroupLabelToolsCurrentActions
            // 
            this.kryptonRibbonGroupLabelToolsCurrentActions.TextLine1 = "Wating task...";
            // 
            // kryptonRibbonGroupButtonToolsProgressLazyloadingShowStatus
            // 
            this.kryptonRibbonGroupButtonToolsProgressLazyloadingShowStatus.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.ToolsProcessQueue;
            this.kryptonRibbonGroupButtonToolsProgressLazyloadingShowStatus.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.ToolsProcessQueue;
            this.kryptonRibbonGroupButtonToolsProgressLazyloadingShowStatus.KeyTip = "T";
            this.kryptonRibbonGroupButtonToolsProgressLazyloadingShowStatus.TextLine1 = "Task list";
            this.kryptonRibbonGroupButtonToolsProgressLazyloadingShowStatus.ToolTipBody = "Show the queue of task wating to be processed.";
            this.kryptonRibbonGroupButtonToolsProgressLazyloadingShowStatus.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.ToolsProcessQueue;
            this.kryptonRibbonGroupButtonToolsProgressLazyloadingShowStatus.ToolTipTitle = "Task list";
            this.kryptonRibbonGroupButtonToolsProgressLazyloadingShowStatus.Click += new System.EventHandler(this.kryptonRibbonGroupButtonToolsProgressLazyloadingShowStatus_Click);
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
            this.kryptonRibbonGroupPreviewPreview.DialogBoxLauncher = false;
            this.kryptonRibbonGroupPreviewPreview.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupTriplePreviewPreview});
            this.kryptonRibbonGroupPreviewPreview.TextLine1 = "Preview";
            // 
            // kryptonRibbonGroupTriplePreviewPreview
            // 
            this.kryptonRibbonGroupTriplePreviewPreview.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButtonPreviewPreview});
            // 
            // kryptonRibbonGroupButtonPreviewPreview
            // 
            this.kryptonRibbonGroupButtonPreviewPreview.ButtonType = Krypton.Ribbon.GroupButtonType.Check;
            this.kryptonRibbonGroupButtonPreviewPreview.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.MediaPreview;
            this.kryptonRibbonGroupButtonPreviewPreview.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.MediaPreview;
            this.kryptonRibbonGroupButtonPreviewPreview.KeyTip = "P";
            this.kryptonRibbonGroupButtonPreviewPreview.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.kryptonRibbonGroupButtonPreviewPreview.TextLine1 = "Preview";
            this.kryptonRibbonGroupButtonPreviewPreview.ToolTipBody = "After selection of one of more media files you can show video and photos covering" +
    " full window.";
            this.kryptonRibbonGroupButtonPreviewPreview.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.MediaPreview;
            this.kryptonRibbonGroupButtonPreviewPreview.ToolTipTitle = "Preview video and photos";
            this.kryptonRibbonGroupButtonPreviewPreview.Click += new System.EventHandler(this.kryptonRibbonGroupButtonPreviewPreview_Click);
            // 
            // kryptonRibbonGroupPreviewNavigate
            // 
            this.kryptonRibbonGroupPreviewNavigate.DialogBoxLauncher = false;
            this.kryptonRibbonGroupPreviewNavigate.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupTriplePreviewSkipPrevNext,
            this.kryptonRibbonGroupTriplePreviewPlayPause,
            this.kryptonRibbonGroupTriplePreviewRewindForwardStop,
            this.kryptonRibbonGroupLinesPreviewTimer});
            this.kryptonRibbonGroupPreviewNavigate.TextLine1 = "Navigate";
            // 
            // kryptonRibbonGroupTriplePreviewSkipPrevNext
            // 
            this.kryptonRibbonGroupTriplePreviewSkipPrevNext.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButtonPreviewSkipPrev,
            this.kryptonRibbonGroupButtonPreviewSkipNext});
            // 
            // kryptonRibbonGroupButtonPreviewSkipPrev
            // 
            this.kryptonRibbonGroupButtonPreviewSkipPrev.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerPrevious;
            this.kryptonRibbonGroupButtonPreviewSkipPrev.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerPrevious;
            this.kryptonRibbonGroupButtonPreviewSkipPrev.KeyTip = "NPR";
            this.kryptonRibbonGroupButtonPreviewSkipPrev.TextLine1 = "Skip";
            this.kryptonRibbonGroupButtonPreviewSkipPrev.TextLine2 = "Prev";
            this.kryptonRibbonGroupButtonPreviewSkipPrev.ToolTipBody = "Show previous media file from the list of selection ";
            this.kryptonRibbonGroupButtonPreviewSkipPrev.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerPrevious;
            this.kryptonRibbonGroupButtonPreviewSkipPrev.ToolTipTitle = "Skip previous";
            this.kryptonRibbonGroupButtonPreviewSkipPrev.Click += new System.EventHandler(this.kryptonRibbonGroupButtonPreviewSkipPrev_Click);
            // 
            // kryptonRibbonGroupButtonPreviewSkipNext
            // 
            this.kryptonRibbonGroupButtonPreviewSkipNext.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerNext;
            this.kryptonRibbonGroupButtonPreviewSkipNext.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerNext;
            this.kryptonRibbonGroupButtonPreviewSkipNext.KeyTip = "NN";
            this.kryptonRibbonGroupButtonPreviewSkipNext.TextLine1 = "Skip";
            this.kryptonRibbonGroupButtonPreviewSkipNext.TextLine2 = "Next";
            this.kryptonRibbonGroupButtonPreviewSkipNext.ToolTipBody = "Show next media file from the list of selection ";
            this.kryptonRibbonGroupButtonPreviewSkipNext.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerNext;
            this.kryptonRibbonGroupButtonPreviewSkipNext.ToolTipTitle = "Skip next";
            this.kryptonRibbonGroupButtonPreviewSkipNext.Click += new System.EventHandler(this.kryptonRibbonGroupButtonPreviewSkipNext_Click);
            // 
            // kryptonRibbonGroupTriplePreviewPlayPause
            // 
            this.kryptonRibbonGroupTriplePreviewPlayPause.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButtonPreviewPlay,
            this.kryptonRibbonGroupButtonPreviewPause});
            // 
            // kryptonRibbonGroupButtonPreviewPlay
            // 
            this.kryptonRibbonGroupButtonPreviewPlay.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerPlay;
            this.kryptonRibbonGroupButtonPreviewPlay.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerPlay;
            this.kryptonRibbonGroupButtonPreviewPlay.KeyTip = "NP";
            this.kryptonRibbonGroupButtonPreviewPlay.TextLine1 = "Play";
            this.kryptonRibbonGroupButtonPreviewPlay.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerPlay;
            this.kryptonRibbonGroupButtonPreviewPlay.Click += new System.EventHandler(this.kryptonRibbonGroupButtonPreviewPlay_Click);
            // 
            // kryptonRibbonGroupButtonPreviewPause
            // 
            this.kryptonRibbonGroupButtonPreviewPause.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerPause;
            this.kryptonRibbonGroupButtonPreviewPause.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerPause;
            this.kryptonRibbonGroupButtonPreviewPause.KeyTip = "NPA";
            this.kryptonRibbonGroupButtonPreviewPause.TextLine1 = "Pause";
            this.kryptonRibbonGroupButtonPreviewPause.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerPause;
            this.kryptonRibbonGroupButtonPreviewPause.Click += new System.EventHandler(this.kryptonRibbonGroupButtonPreviewPause_Click);
            // 
            // kryptonRibbonGroupTriplePreviewRewindForwardStop
            // 
            this.kryptonRibbonGroupTriplePreviewRewindForwardStop.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButtonPreviewRewind,
            this.kryptonRibbonGroupButtonPreviewForward,
            this.kryptonRibbonGroupButtonPreviewStop});
            // 
            // kryptonRibbonGroupButtonPreviewRewind
            // 
            this.kryptonRibbonGroupButtonPreviewRewind.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerFastBackward;
            this.kryptonRibbonGroupButtonPreviewRewind.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerFastBackward;
            this.kryptonRibbonGroupButtonPreviewRewind.KeyTip = "NR";
            this.kryptonRibbonGroupButtonPreviewRewind.TextLine1 = "Rewind";
            this.kryptonRibbonGroupButtonPreviewRewind.ToolTipBody = "When showing video files, this will jump the movie 10 secouns back in time";
            this.kryptonRibbonGroupButtonPreviewRewind.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerFastBackward;
            this.kryptonRibbonGroupButtonPreviewRewind.ToolTipTitle = "Rewind";
            this.kryptonRibbonGroupButtonPreviewRewind.Click += new System.EventHandler(this.kryptonRibbonGroupButtonPreviewRewind_Click);
            // 
            // kryptonRibbonGroupButtonPreviewForward
            // 
            this.kryptonRibbonGroupButtonPreviewForward.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerFastForward;
            this.kryptonRibbonGroupButtonPreviewForward.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerFastForward;
            this.kryptonRibbonGroupButtonPreviewForward.KeyTip = "NF";
            this.kryptonRibbonGroupButtonPreviewForward.TextLine1 = "Forward";
            this.kryptonRibbonGroupButtonPreviewForward.ToolTipBody = "When showing video files, this will jump the movie 10 secouns forward in time";
            this.kryptonRibbonGroupButtonPreviewForward.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerFastForward;
            this.kryptonRibbonGroupButtonPreviewForward.ToolTipTitle = "Forward";
            this.kryptonRibbonGroupButtonPreviewForward.Click += new System.EventHandler(this.kryptonRibbonGroupButtonPreviewForward_Click);
            // 
            // kryptonRibbonGroupButtonPreviewStop
            // 
            this.kryptonRibbonGroupButtonPreviewStop.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerStop;
            this.kryptonRibbonGroupButtonPreviewStop.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerStop;
            this.kryptonRibbonGroupButtonPreviewStop.KeyTip = "NS";
            this.kryptonRibbonGroupButtonPreviewStop.TextLine1 = "Stop";
            this.kryptonRibbonGroupButtonPreviewStop.ToolTipBody = "Stop chasting / video playpack";
            this.kryptonRibbonGroupButtonPreviewStop.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerStop;
            this.kryptonRibbonGroupButtonPreviewStop.ToolTipTitle = "Stop";
            this.kryptonRibbonGroupButtonPreviewStop.Click += new System.EventHandler(this.kryptonRibbonGroupButtonPreviewStop_Click);
            // 
            // kryptonRibbonGroupLinesPreviewTimer
            // 
            this.kryptonRibbonGroupLinesPreviewTimer.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupTrackBarPreviewTimer});
            // 
            // kryptonRibbonGroupTrackBarPreviewTimer
            // 
            this.kryptonRibbonGroupTrackBarPreviewTimer.MaximumLength = 55;
            this.kryptonRibbonGroupTrackBarPreviewTimer.MinimumLength = 55;
            this.kryptonRibbonGroupTrackBarPreviewTimer.ValueChanged += new System.EventHandler(this.kryptonRibbonGroupTrackBarPreviewTimer_ValueChanged);
            // 
            // kryptonRibbonGroupPreviewRotate
            // 
            this.kryptonRibbonGroupPreviewRotate.DialogBoxLauncher = false;
            this.kryptonRibbonGroupPreviewRotate.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupTriplePreviewRotate});
            this.kryptonRibbonGroupPreviewRotate.TextLine1 = "Rotate";
            // 
            // kryptonRibbonGroupTriplePreviewRotate
            // 
            this.kryptonRibbonGroupTriplePreviewRotate.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButtonPreviewRotate270,
            this.kryptonRibbonGroupButtonPreviewRotate180,
            this.kryptonRibbonGroupButtonPreviewRotate90});
            // 
            // kryptonRibbonGroupButtonPreviewRotate270
            // 
            this.kryptonRibbonGroupButtonPreviewRotate270.ButtonType = Krypton.Ribbon.GroupButtonType.Check;
            this.kryptonRibbonGroupButtonPreviewRotate270.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.MediaRotate90CCW;
            this.kryptonRibbonGroupButtonPreviewRotate270.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.MediaRotate90CCW;
            this.kryptonRibbonGroupButtonPreviewRotate270.KeyTip = "2";
            this.kryptonRibbonGroupButtonPreviewRotate270.TextLine1 = "90° CCW";
            this.kryptonRibbonGroupButtonPreviewRotate270.ToolTipBody = "Rotate the media on screen only";
            this.kryptonRibbonGroupButtonPreviewRotate270.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.MediaRotate90CCW;
            this.kryptonRibbonGroupButtonPreviewRotate270.ToolTipTitle = "Roatet 270° / 90° CCW";
            this.kryptonRibbonGroupButtonPreviewRotate270.Click += new System.EventHandler(this.kryptonRibbonGroupButtonPreviewRotate270_Click);
            // 
            // kryptonRibbonGroupButtonPreviewRotate180
            // 
            this.kryptonRibbonGroupButtonPreviewRotate180.ButtonType = Krypton.Ribbon.GroupButtonType.Check;
            this.kryptonRibbonGroupButtonPreviewRotate180.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.MediaRotate180;
            this.kryptonRibbonGroupButtonPreviewRotate180.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.MediaRotate180;
            this.kryptonRibbonGroupButtonPreviewRotate180.KeyTip = "1";
            this.kryptonRibbonGroupButtonPreviewRotate180.TextLine1 = "180°";
            this.kryptonRibbonGroupButtonPreviewRotate180.ToolTipBody = "Rotate the media on screen only";
            this.kryptonRibbonGroupButtonPreviewRotate180.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.MediaRotate180;
            this.kryptonRibbonGroupButtonPreviewRotate180.ToolTipTitle = "Roatet 180° CW";
            this.kryptonRibbonGroupButtonPreviewRotate180.Click += new System.EventHandler(this.kryptonRibbonGroupButtonPreviewRotate180_Click);
            // 
            // kryptonRibbonGroupButtonPreviewRotate90
            // 
            this.kryptonRibbonGroupButtonPreviewRotate90.ButtonType = Krypton.Ribbon.GroupButtonType.Check;
            this.kryptonRibbonGroupButtonPreviewRotate90.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.MediaRotate90CW;
            this.kryptonRibbonGroupButtonPreviewRotate90.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.MediaRotate90CW;
            this.kryptonRibbonGroupButtonPreviewRotate90.KeyTip = "9";
            this.kryptonRibbonGroupButtonPreviewRotate90.TextLine1 = "90° CW";
            this.kryptonRibbonGroupButtonPreviewRotate90.ToolTipBody = "Rotate the media on screen only";
            this.kryptonRibbonGroupButtonPreviewRotate90.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.MediaRotate90CW;
            this.kryptonRibbonGroupButtonPreviewRotate90.ToolTipTitle = "Roatet 90° CW";
            this.kryptonRibbonGroupButtonPreviewRotate90.Click += new System.EventHandler(this.kryptonRibbonGroupButtonPreviewRotate90_Click);
            // 
            // kryptonRibbonGroupPreviewSlideshow
            // 
            this.kryptonRibbonGroupPreviewSlideshow.DialogBoxLauncher = false;
            this.kryptonRibbonGroupPreviewSlideshow.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupTriple2,
            this.kryptonRibbonGroupTriplePreviewSlideshow});
            this.kryptonRibbonGroupPreviewSlideshow.TextLine1 = "Slideshow";
            // 
            // kryptonRibbonGroupTriple2
            // 
            this.kryptonRibbonGroupTriple2.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButtonPreviewSlideshowPlay});
            // 
            // kryptonRibbonGroupButtonPreviewSlideshowPlay
            // 
            this.kryptonRibbonGroupButtonPreviewSlideshowPlay.ButtonType = Krypton.Ribbon.GroupButtonType.Check;
            this.kryptonRibbonGroupButtonPreviewSlideshowPlay.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.MediaPreviewSlideshowPlay;
            this.kryptonRibbonGroupButtonPreviewSlideshowPlay.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.MediaPreviewSlideshowPlay;
            this.kryptonRibbonGroupButtonPreviewSlideshowPlay.KeyTip = "SS";
            this.kryptonRibbonGroupButtonPreviewSlideshowPlay.TextLine1 = "Start";
            this.kryptonRibbonGroupButtonPreviewSlideshowPlay.TextLine2 = "Slideshow";
            this.kryptonRibbonGroupButtonPreviewSlideshowPlay.ToolTipBody = "Start / stop slideshow of selected media files";
            this.kryptonRibbonGroupButtonPreviewSlideshowPlay.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.MediaPreviewSlideshowPlay;
            this.kryptonRibbonGroupButtonPreviewSlideshowPlay.ToolTipTitle = "Start / stop slideshow";
            this.kryptonRibbonGroupButtonPreviewSlideshowPlay.Click += new System.EventHandler(this.kryptonRibbonGroupButtonPreviewSlideshowPlay_Click);
            // 
            // kryptonRibbonGroupTriplePreviewSlideshow
            // 
            this.kryptonRibbonGroupTriplePreviewSlideshow.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupButtonPreviewSlideshow,
            this.kryptonRibbonGroupButtonPreviewSlideshowTimeInterval,
            this.kryptonRibbonGroupButtonPreviewChromecast});
            // 
            // kryptonRibbonGroupButtonPreviewSlideshow
            // 
            this.kryptonRibbonGroupButtonPreviewSlideshow.ButtonType = Krypton.Ribbon.GroupButtonType.DropDown;
            this.kryptonRibbonGroupButtonPreviewSlideshow.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.MediaPreviewSlideshow;
            this.kryptonRibbonGroupButtonPreviewSlideshow.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.MediaPreviewSlideshow;
            this.kryptonRibbonGroupButtonPreviewSlideshow.KeyTip = "SM";
            this.kryptonRibbonGroupButtonPreviewSlideshow.TextLine1 = "Select";
            this.kryptonRibbonGroupButtonPreviewSlideshow.TextLine2 = "Media";
            this.kryptonRibbonGroupButtonPreviewSlideshow.ToolTipBody = "Jump to a given media file";
            this.kryptonRibbonGroupButtonPreviewSlideshow.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.MediaPreviewSlideshow;
            this.kryptonRibbonGroupButtonPreviewSlideshow.ToolTipTitle = "Select media";
            // 
            // kryptonRibbonGroupButtonPreviewSlideshowTimeInterval
            // 
            this.kryptonRibbonGroupButtonPreviewSlideshowTimeInterval.ButtonType = Krypton.Ribbon.GroupButtonType.DropDown;
            this.kryptonRibbonGroupButtonPreviewSlideshowTimeInterval.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.MediaPreviewSlideshowInterval;
            this.kryptonRibbonGroupButtonPreviewSlideshowTimeInterval.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.MediaPreviewSlideshowInterval;
            this.kryptonRibbonGroupButtonPreviewSlideshowTimeInterval.KeyTip = "ST";
            this.kryptonRibbonGroupButtonPreviewSlideshowTimeInterval.TextLine1 = "Time";
            this.kryptonRibbonGroupButtonPreviewSlideshowTimeInterval.TextLine2 = "Interval";
            this.kryptonRibbonGroupButtonPreviewSlideshowTimeInterval.ToolTipBody = "Set the pause between move to next media files during slideshow";
            this.kryptonRibbonGroupButtonPreviewSlideshowTimeInterval.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.MediaPreviewSlideshowInterval;
            this.kryptonRibbonGroupButtonPreviewSlideshowTimeInterval.ToolTipTitle = "Time interval";
            // 
            // kryptonRibbonGroupButtonPreviewChromecast
            // 
            this.kryptonRibbonGroupButtonPreviewChromecast.ButtonType = Krypton.Ribbon.GroupButtonType.DropDown;
            this.kryptonRibbonGroupButtonPreviewChromecast.ImageLarge = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerChromecast;
            this.kryptonRibbonGroupButtonPreviewChromecast.ImageSmall = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerChromecast;
            this.kryptonRibbonGroupButtonPreviewChromecast.KeyTip = "C";
            this.kryptonRibbonGroupButtonPreviewChromecast.TextLine1 = "Chromecast";
            this.kryptonRibbonGroupButtonPreviewChromecast.ToolTipBody = "Select where to chromecast";
            this.kryptonRibbonGroupButtonPreviewChromecast.ToolTipImage = global::PhotoTagsSynchronizer.Properties.Resources.MediaPlayerChromecast;
            this.kryptonRibbonGroupButtonPreviewChromecast.ToolTipTitle = "Chromecast";
            // 
            // kryptonRibbonGroupPreviewStatus
            // 
            this.kryptonRibbonGroupPreviewStatus.DialogBoxLauncher = false;
            this.kryptonRibbonGroupPreviewStatus.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupLinesPreviewStatus});
            this.kryptonRibbonGroupPreviewStatus.TextLine1 = "Status";
            // 
            // kryptonRibbonGroupLinesPreviewStatus
            // 
            this.kryptonRibbonGroupLinesPreviewStatus.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.kryptonRibbonGroupLabelPreviewTimer,
            this.kryptonRibbonGroupLabelPreviewStatus});
            // 
            // kryptonRibbonGroupLabelPreviewTimer
            // 
            this.kryptonRibbonGroupLabelPreviewTimer.TextLine1 = "Timer:";
            this.kryptonRibbonGroupLabelPreviewTimer.TextLine2 = "00:00";
            // 
            // kryptonRibbonGroupLabelPreviewStatus
            // 
            this.kryptonRibbonGroupLabelPreviewStatus.TextLine1 = "Status:";
            this.kryptonRibbonGroupLabelPreviewStatus.TextLine2 = "Waiting";
            // 
            // kryptonContextMenuFileSystemColumnSort
            // 
            this.kryptonContextMenuFileSystemColumnSort.Items.AddRange(new Krypton.Toolkit.KryptonContextMenuItemBase[] {
            this.kryptonContextMenuRadioButtonFileSystemColumnSortFilename,
            this.kryptonContextMenuRadioButtonFileSystemColumnSortFileCreateDate,
            this.kryptonContextMenuRadioButtonFileSystemColumnSortFileModifiedDate,
            this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaDateTaken,
            this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaAlbum,
            this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaTitle,
            this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaDescription,
            this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaComments,
            this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaAuthor,
            this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaRating,
            this.kryptonContextMenuRadioButtonFileSystemColumnSortLocationName,
            this.kryptonContextMenuRadioButtonFileSystemColumnSortLocationRegionState,
            this.kryptonContextMenuRadioButtonFileSystemColumnSortLocationCity,
            this.kryptonContextMenuRadioButtonFileSystemColumnSortLocationCountry,
            this.kryptonContextMenuItemsCloseMenuList});
            // 
            // kryptonContextMenuRadioButtonFileSystemColumnSortFilename
            // 
            this.kryptonContextMenuRadioButtonFileSystemColumnSortFilename.ExtraText = "";
            this.kryptonContextMenuRadioButtonFileSystemColumnSortFilename.Text = "Filename";
            // 
            // kryptonContextMenuRadioButtonFileSystemColumnSortFileCreateDate
            // 
            this.kryptonContextMenuRadioButtonFileSystemColumnSortFileCreateDate.ExtraText = "";
            this.kryptonContextMenuRadioButtonFileSystemColumnSortFileCreateDate.Text = "FileCreatedDate";
            // 
            // kryptonContextMenuRadioButtonFileSystemColumnSortFileModifiedDate
            // 
            this.kryptonContextMenuRadioButtonFileSystemColumnSortFileModifiedDate.ExtraText = "";
            this.kryptonContextMenuRadioButtonFileSystemColumnSortFileModifiedDate.Text = "FileModifiedDate";
            // 
            // kryptonContextMenuRadioButtonFileSystemColumnSortMediaDateTaken
            // 
            this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaDateTaken.ExtraText = "";
            this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaDateTaken.Text = "MediaDateTaken";
            // 
            // kryptonContextMenuRadioButtonFileSystemColumnSortMediaAlbum
            // 
            this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaAlbum.ExtraText = "";
            this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaAlbum.Text = "MediaAlbum";
            // 
            // kryptonContextMenuRadioButtonFileSystemColumnSortMediaTitle
            // 
            this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaTitle.ExtraText = "";
            this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaTitle.Text = "MediaTitle";
            // 
            // kryptonContextMenuRadioButtonFileSystemColumnSortMediaDescription
            // 
            this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaDescription.ExtraText = "";
            this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaDescription.Text = "MediaDescription";
            // 
            // kryptonContextMenuRadioButtonFileSystemColumnSortMediaComments
            // 
            this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaComments.ExtraText = "";
            this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaComments.Text = "MediaComments";
            // 
            // kryptonContextMenuRadioButtonFileSystemColumnSortMediaAuthor
            // 
            this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaAuthor.ExtraText = "";
            this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaAuthor.Text = "MediaAuthor";
            // 
            // kryptonContextMenuRadioButtonFileSystemColumnSortMediaRating
            // 
            this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaRating.ExtraText = "";
            this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaRating.Text = "MediaRating";
            // 
            // kryptonContextMenuRadioButtonFileSystemColumnSortLocationName
            // 
            this.kryptonContextMenuRadioButtonFileSystemColumnSortLocationName.ExtraText = "";
            this.kryptonContextMenuRadioButtonFileSystemColumnSortLocationName.Text = "LocationName";
            // 
            // kryptonContextMenuRadioButtonFileSystemColumnSortLocationRegionState
            // 
            this.kryptonContextMenuRadioButtonFileSystemColumnSortLocationRegionState.ExtraText = "";
            this.kryptonContextMenuRadioButtonFileSystemColumnSortLocationRegionState.Text = "LocationRegionState";
            // 
            // kryptonContextMenuRadioButtonFileSystemColumnSortLocationCity
            // 
            this.kryptonContextMenuRadioButtonFileSystemColumnSortLocationCity.ExtraText = "";
            this.kryptonContextMenuRadioButtonFileSystemColumnSortLocationCity.Text = "LocationCity";
            // 
            // kryptonContextMenuRadioButtonFileSystemColumnSortLocationCountry
            // 
            this.kryptonContextMenuRadioButtonFileSystemColumnSortLocationCountry.ExtraText = "";
            this.kryptonContextMenuRadioButtonFileSystemColumnSortLocationCountry.Text = "LocationCountry";
            // 
            // kryptonContextMenuItemsCloseMenuList
            // 
            this.kryptonContextMenuItemsCloseMenuList.Items.AddRange(new Krypton.Toolkit.KryptonContextMenuItemBase[] {
            this.kryptonContextMenuItemCloseMenu});
            // 
            // kryptonContextMenuItemCloseMenu
            // 
            this.kryptonContextMenuItemCloseMenu.Text = "Close menu";
            // 
            // kryptonContextMenuImageListViewModeThumbnailRenders
            // 
            this.kryptonContextMenuImageListViewModeThumbnailRenders.Items.AddRange(new Krypton.Toolkit.KryptonContextMenuItemBase[] {
            this.kryptonContextMenuItems3});
            // 
            // kryptonContextMenuPreviewSlideshowInterval
            // 
            this.kryptonContextMenuPreviewSlideshowInterval.Items.AddRange(new Krypton.Toolkit.KryptonContextMenuItemBase[] {
            this.kryptonContextMenuRadioButtonSlideshow2sec,
            this.kryptonContextMenuRadioButtonSlideshow4sec,
            this.kryptonContextMenuRadioButtonSlideshow6sec,
            this.kryptonContextMenuRadioButtonSlideshow8sec,
            this.kryptonContextMenuRadioButtonSlideshow10sec,
            this.kryptonContextMenuItemsPreviewSlideshowIntervalList});
            // 
            // kryptonContextMenuRadioButtonSlideshow2sec
            // 
            this.kryptonContextMenuRadioButtonSlideshow2sec.ExtraText = "";
            this.kryptonContextMenuRadioButtonSlideshow2sec.Text = "Slideshow 2 sec";
            // 
            // kryptonContextMenuRadioButtonSlideshow4sec
            // 
            this.kryptonContextMenuRadioButtonSlideshow4sec.ExtraText = "";
            this.kryptonContextMenuRadioButtonSlideshow4sec.Text = "Slideshow 4 sec";
            // 
            // kryptonContextMenuRadioButtonSlideshow6sec
            // 
            this.kryptonContextMenuRadioButtonSlideshow6sec.ExtraText = "";
            this.kryptonContextMenuRadioButtonSlideshow6sec.Text = "Slideshow 6 sec";
            // 
            // kryptonContextMenuRadioButtonSlideshow8sec
            // 
            this.kryptonContextMenuRadioButtonSlideshow8sec.ExtraText = "";
            this.kryptonContextMenuRadioButtonSlideshow8sec.Text = "Slideshow8 sec";
            // 
            // kryptonContextMenuRadioButtonSlideshow10sec
            // 
            this.kryptonContextMenuRadioButtonSlideshow10sec.ExtraText = "";
            this.kryptonContextMenuRadioButtonSlideshow10sec.Text = "Slideshow 10 sec";
            // 
            // kryptonContextMenuItemsPreviewSlideshowIntervalList
            // 
            this.kryptonContextMenuItemsPreviewSlideshowIntervalList.Items.AddRange(new Krypton.Toolkit.KryptonContextMenuItemBase[] {
            this.kryptonContextMenuItemPreviewSlideshowIntervalStop});
            // 
            // kryptonContextMenuItemPreviewSlideshowIntervalStop
            // 
            this.kryptonContextMenuItemPreviewSlideshowIntervalStop.Enabled = false;
            this.kryptonContextMenuItemPreviewSlideshowIntervalStop.Text = "Slideshow stop";
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
            // kryptonContextMenuRadioButton1
            // 
            this.kryptonContextMenuRadioButton1.ExtraText = "";
            // 
            // kryptonContextMenuRadioButton2
            // 
            this.kryptonContextMenuRadioButton2.ExtraText = "";
            // 
            // kryptonContextMenuRadioButton3
            // 
            this.kryptonContextMenuRadioButton3.ExtraText = "";
            // 
            // kryptonContextMenuRadioButton4
            // 
            this.kryptonContextMenuRadioButton4.ExtraText = "";
            // 
            // kryptonContextMenuRadioButton5
            // 
            this.kryptonContextMenuRadioButton5.ExtraText = "";
            // 
            // kryptonContextMenuRadioButton6
            // 
            this.kryptonContextMenuRadioButton6.ExtraText = "";
            // 
            // kryptonContextMenuItem9
            // 
            this.kryptonContextMenuItem9.Items.AddRange(new Krypton.Toolkit.KryptonContextMenuItemBase[] {
            this.kryptonContextMenuMonthCalendar3,
            this.kryptonContextMenuMonthCalendar4,
            this.kryptonContextMenuImageSelect2,
            this.kryptonContextMenuColorColumns1,
            this.kryptonContextMenuLinkLabel1,
            this.kryptonContextMenuRadioButton7,
            this.kryptonContextMenuCheckButton1,
            this.kryptonContextMenuCheckBox1});
            this.kryptonContextMenuItem9.Text = "Menu Item";
            // 
            // kryptonContextMenuMonthCalendar3
            // 
            this.kryptonContextMenuMonthCalendar3.SelectionEnd = new System.DateTime(2021, 10, 8, 0, 0, 0, 0);
            this.kryptonContextMenuMonthCalendar3.SelectionStart = new System.DateTime(2021, 10, 8, 0, 0, 0, 0);
            this.kryptonContextMenuMonthCalendar3.TodayDate = new System.DateTime(2021, 10, 8, 0, 0, 0, 0);
            // 
            // kryptonContextMenuMonthCalendar4
            // 
            this.kryptonContextMenuMonthCalendar4.SelectionEnd = new System.DateTime(2021, 10, 8, 0, 0, 0, 0);
            this.kryptonContextMenuMonthCalendar4.SelectionStart = new System.DateTime(2021, 10, 8, 0, 0, 0, 0);
            this.kryptonContextMenuMonthCalendar4.TodayDate = new System.DateTime(2021, 10, 8, 0, 0, 0, 0);
            // 
            // kryptonContextMenuLinkLabel1
            // 
            this.kryptonContextMenuLinkLabel1.ExtraText = "";
            // 
            // kryptonContextMenuRadioButton7
            // 
            this.kryptonContextMenuRadioButton7.ExtraText = "";
            // 
            // kryptonContextMenuCheckButton1
            // 
            this.kryptonContextMenuCheckButton1.Text = "CheckButton";
            // 
            // kryptonContextMenuCheckBox1
            // 
            this.kryptonContextMenuCheckBox1.ExtraText = "";
            // 
            // kryptonContextMenuMonthCalendar5
            // 
            this.kryptonContextMenuMonthCalendar5.SelectionEnd = new System.DateTime(2021, 10, 8, 0, 0, 0, 0);
            this.kryptonContextMenuMonthCalendar5.SelectionStart = new System.DateTime(2021, 10, 8, 0, 0, 0, 0);
            this.kryptonContextMenuMonthCalendar5.TodayDate = new System.DateTime(2021, 10, 8, 0, 0, 0, 0);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1214, 894);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelMediaPreview);
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
            this.kryptonWorkspaceSearchFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageSearchFiler)).EndInit();
            this.kryptonPageSearchFiler.ResumeLayout(false);
            this.kryptonPageSearchFiler.PerformLayout();
            this.tableLayoutPanelSerachSearch.ResumeLayout(false);
            this.tableLayoutPanelSerachSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchFileSystem.Panel)).EndInit();
            this.groupBoxSearchFileSystem.Panel.ResumeLayout(false);
            this.groupBoxSearchFileSystem.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchFileSystem)).EndInit();
            this.groupBoxSearchFileSystem.ResumeLayout(false);
            this.tableLayoutPanelSearchFileSystem.ResumeLayout(false);
            this.tableLayoutPanelSearchFileSystem.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchPeople.Panel)).EndInit();
            this.groupBoxSearchPeople.Panel.ResumeLayout(false);
            this.groupBoxSearchPeople.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchPeople)).EndInit();
            this.groupBoxSearchPeople.ResumeLayout(false);
            this.tableLayoutPanelSearchPeople.ResumeLayout(false);
            this.tableLayoutPanelSearchPeople.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchTags.Panel)).EndInit();
            this.groupBoxSearchTags.Panel.ResumeLayout(false);
            this.groupBoxSearchTags.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchTags)).EndInit();
            this.groupBoxSearchTags.ResumeLayout(false);
            this.tableLayoutPanelSearchDetails.ResumeLayout(false);
            this.tableLayoutPanelSearchDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxSearchLocationCountry)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxSearchLocationState)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxSearchLocationCity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxSearchLocationName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxSearchComments)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxSearchTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxSearchDescription)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxSearchAlbum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchKeywords.Panel)).EndInit();
            this.groupBoxSearchKeywords.Panel.ResumeLayout(false);
            this.groupBoxSearchKeywords.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchKeywords)).EndInit();
            this.groupBoxSearchKeywords.ResumeLayout(false);
            this.tableLayoutPanelSearchKeywords.ResumeLayout(false);
            this.tableLayoutPanelSearchKeywords.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxSearchKeyword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchMediaTaken.Panel)).EndInit();
            this.groupBoxSearchMediaTaken.Panel.ResumeLayout(false);
            this.groupBoxSearchMediaTaken.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchMediaTaken)).EndInit();
            this.groupBoxSearchMediaTaken.ResumeLayout(false);
            this.tableLayoutPanelSearchMediaTaken.ResumeLayout(false);
            this.tableLayoutPanelSearchMediaTaken.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchExtra.Panel)).EndInit();
            this.groupBoxSearchExtra.Panel.ResumeLayout(false);
            this.groupBoxSearchExtra.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchExtra)).EndInit();
            this.groupBoxSearchExtra.ResumeLayout(false);
            this.tableLayoutPanelSearchAttributes.ResumeLayout(false);
            this.tableLayoutPanelSearchAttributes.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchRating.Panel)).EndInit();
            this.groupBoxSearchRating.Panel.ResumeLayout(false);
            this.groupBoxSearchRating.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSearchRating)).EndInit();
            this.groupBoxSearchRating.ResumeLayout(false);
            this.tableLayoutPanelSearchRating.ResumeLayout(false);
            this.tableLayoutPanelSearchRating.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCellSearchFiler)).EndInit();
            this.kryptonWorkspaceCellSearchFiler.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCellSearchFilterAction)).EndInit();
            this.kryptonWorkspaceCellSearchFilterAction.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageSearchFilterAction)).EndInit();
            this.kryptonPageSearchFilterAction.ResumeLayout(false);
            this.kryptonPageSearchFilterAction.PerformLayout();
            this.tableLayoutPanelSerachActions.ResumeLayout(false);
            this.tableLayoutPanelSerachActions.PerformLayout();
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
            this.kryptonPageToolboxTagsDetails.PerformLayout();
            this.tableLayoutPanelTags.ResumeLayout(false);
            this.tableLayoutPanelTags.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxTagsDetails.Panel)).EndInit();
            this.kryptonGroupBoxTagsDetails.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxTagsDetails)).EndInit();
            this.kryptonGroupBoxTagsDetails.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxAuthor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxComments)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxDescription)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxAlbum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxToolboxTagsTags.Panel)).EndInit();
            this.kryptonGroupBoxToolboxTagsTags.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxToolboxTagsTags)).EndInit();
            this.kryptonGroupBoxToolboxTagsTags.ResumeLayout(false);
            this.tableLayoutPanelTagConfidence.ResumeLayout(false);
            this.tableLayoutPanelTagConfidence.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxMediaAiConfidence)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxRating.Panel)).EndInit();
            this.groupBoxRating.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxRating)).EndInit();
            this.groupBoxRating.ResumeLayout(false);
            this.tableLayoutPanelTagRationg.ResumeLayout(false);
            this.tableLayoutPanelTagRationg.PerformLayout();
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
            this.tableLayoutPanelMap.ResumeLayout(false);
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
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageToolboxWarnings)).EndInit();
            this.kryptonPageToolboxWarnings.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewExiftoolWarning)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageToolboxProperties)).EndInit();
            this.kryptonPageToolboxProperties.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageToolboxRename)).EndInit();
            this.kryptonPageToolboxRename.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceToolboxRename)).EndInit();
            this.kryptonWorkspaceToolboxRename.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageToolboxRenameVariables)).EndInit();
            this.kryptonPageToolboxRenameVariables.ResumeLayout(false);
            this.tableLayoutPanelRename.ResumeLayout(false);
            this.tableLayoutPanelRename.PerformLayout();
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
            ((System.ComponentModel.ISupportInitialize)(this.panelMediaPreview)).EndInit();
            this.panelMediaPreview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.videoView1)).EndInit();
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timerShowErrorMessage;
        private Manina.Windows.Forms.ImageListView imageListView1;
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
        private Krypton.Toolkit.KryptonButton buttonSearch;
        private Krypton.Toolkit.KryptonCheckBox checkBoxSerachFitsAllValues;
        private PhotoTagsCommonComponets.TreeViewWithoutDoubleClick treeViewFilter;
        private System.Windows.Forms.Timer timerStatusThreadQueue;
        private Krypton.Toolkit.KryptonPanel panelMediaPreview;
        private LibVLCSharp.WinForms.VideoView videoView1;
        private Cyotek.Windows.Forms.ImageBox imageBoxPreview;
        private Krypton.Toolkit.KryptonDataGridView dataGridViewConvertAndMerge;
        private System.Windows.Forms.Timer timerFindGoogleCast;
        private System.Windows.Forms.Timer timerPreviewNextTimer;
        private System.Windows.Forms.Timer timerSaveProgessRemoveProgress;
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
        private Krypton.Toolkit.KryptonCheckedListBox checkedListBoxSearchPeople;
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
        private Krypton.Toolkit.KryptonDateTimePicker dateTimePickerSearchDateFrom;
        private Krypton.Toolkit.KryptonDateTimePicker dateTimePickerSearchDateTo;
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
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTripleViewGalleryDetailsPane;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonImageListViewModeGallery;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonImageListViewModeDetails;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonImageListViewModePane;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTripleViewThumbnailsMode;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonImageListViewModeThumbnails;
        private Krypton.Ribbon.KryptonRibbonGroupSeparator kryptonRibbonGroupSeparator4;
        private Krypton.Ribbon.KryptonRibbonGroup kryptonRibbonGroupViewCellSize;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTripleViewGridSize;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonDataGridViewCellSizeBig;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonDataGridViewCellSizeMedium;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonDataGridViewCellSizeSmall;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTripleViewShowHideColumns;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonDataGridViewColumnsHistory;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonDataGridViewColumnsErrors;
        private Krypton.Ribbon.KryptonRibbonTab kryptonRibbonTabSelect;
        private Krypton.Ribbon.KryptonRibbonGroup kryptonRibbonGroupImageListViewSelect;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTripleSelectForwardBackwards;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonSelectBackwards;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonSelectForwards;
        private Krypton.Ribbon.KryptonRibbonGroupLines kryptonRibbonGroupLinesSelectByDateInterval;
        private Krypton.Ribbon.KryptonRibbonGroupRadioButton kryptonRibbonGroupRadioButtonSelectDateRange1;
        private Krypton.Ribbon.KryptonRibbonGroupRadioButton kryptonRibbonGroupRadioButtonSelectDateRange3;
        private Krypton.Ribbon.KryptonRibbonGroupRadioButton kryptonRibbonGroupRadioButtonSelectDateRange7;
        private Krypton.Ribbon.KryptonRibbonGroupRadioButton kryptonRibbonGroupRadioButtonSelectDateRange14;
        private Krypton.Ribbon.KryptonRibbonGroupRadioButton kryptonRibbonGroupRadioButtonSelectDateRange30;
        private Krypton.Ribbon.KryptonRibbonGroupSeparator kryptonRibbonGroupSeparator2;
        private Krypton.Ribbon.KryptonRibbonGroupLines kryptonRibbonGroupLinesLocationAmount;
        private Krypton.Ribbon.KryptonRibbonGroupRadioButton kryptonRibbonGroupRadioButtonSelectMediaAmount10;
        private Krypton.Ribbon.KryptonRibbonGroupRadioButton kryptonRibbonGroupRadioButtonSelectMediaAmount30;
        private Krypton.Ribbon.KryptonRibbonGroupRadioButton kryptonRibbonGroupRadioButtonSelectMediaAmount50;
        private Krypton.Ribbon.KryptonRibbonGroupRadioButton kryptonRibbonGroupRadioButtonSelectMediaAmount100;
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
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTripleViewColumnsSort;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonImageListViewDetailviewColumns;
        private Krypton.Ribbon.KryptonRibbonGroup kryptonRibbonGroupViewThumbnailSize;
        private Krypton.Ribbon.KryptonRibbonGroup kryptonRibbonGroupViewColumns;
        private Krypton.Ribbon.KryptonRibbonTab kryptonRibbonTabHome;
        private Krypton.Ribbon.KryptonRibbonGroup kryptonRibbonGroupHomeRotate;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTripleHomeRotate;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonMediaFileRotate90CCW;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonMediaFileRotate180;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonMediaFileRotate90CW;
        private Krypton.Ribbon.KryptonRibbonGroup kryptonRibbonGroupHomeClipboard;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTripleHomeCopyCutPaste;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonHomeCut;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonHomeCopy;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonHomePaste;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTripleHomeUndoRedo;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonHomeUndo;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonRedo;
        private Krypton.Ribbon.KryptonRibbonGroupLines kryptonRibbonGroupLinesLocatonDatePriorities;
        private Krypton.Ribbon.KryptonRibbonGroup kryptonRibbonGroup7;
        private Krypton.Ribbon.KryptonRibbonGroup kryptonRibbonGroup8;
        private Krypton.Ribbon.KryptonRibbonGroupLines kryptonRibbonGroupLinesSelectLocation;
        private Krypton.Ribbon.KryptonRibbonGroupCheckBox kryptonRibbonGroupCheckBoxSelectLocationName;
        private Krypton.Ribbon.KryptonRibbonGroupCheckBox kryptonRibbonGroupCheckBoxSelectLocationCity;
        private Krypton.Ribbon.KryptonRibbonGroupCheckBox kryptonRibbonGroupCheckBoxSelectLocationStateRegion;
        private Krypton.Ribbon.KryptonRibbonGroupCheckBox kryptonRibbonGroupCheckBoxSelectLocationCountry;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTripleViewTumbnailSizeLargeMedium;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonThumbnailSizeXLarge;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonThumbnailSizeLarge;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonThumbnailSizeMedium;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTripleViewThumbnailSizeXSmall;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonThumbnailSizeSmall;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonThumbnailSizeXSmall;
        private Krypton.Ribbon.KryptonRibbonGroup kryptonRibbonGroupViewRows;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTripleViewShowHideRows;
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
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTripleHomeMetadataRefresh;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonHomeMetadataRefresh;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonHomeMetadataReload;
        private Krypton.Ribbon.KryptonRibbonGroup kryptonRibbonGroupHomeMetadata;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTripleHomeAutoCorrect;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonHomeAutoCorrectRun;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonHomeAutoCorrectForm;
        private Krypton.Ribbon.KryptonRibbonGroup kryptonRibbonGroupHomeManage;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTripleHomeSortFindAndSearch;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonHomeFind;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonHomeReplace;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTripleHomeCopy;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonHomeFastCopyNoOverwrite;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonHomeFastCopyOverwrite;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTripleHomeTriState;
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
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTriplePreviewPreview;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonPreviewPreview;
        private Krypton.Ribbon.KryptonRibbonGroup kryptonRibbonGroupPreviewNavigate;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTriplePreviewSkipPrevNext;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonPreviewSkipPrev;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonPreviewSkipNext;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTriplePreviewRewindForwardStop;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonPreviewRewind;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonPreviewForward;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonPreviewStop;
        private Krypton.Ribbon.KryptonRibbonGroup kryptonRibbonGroupPreviewRotate;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTriplePreviewRotate;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonPreviewRotate270;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonPreviewRotate180;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonPreviewRotate90;
        private Krypton.Ribbon.KryptonRibbonGroup kryptonRibbonGroupPreviewStatus;
        private Krypton.Ribbon.KryptonRibbonGroupLines kryptonRibbonGroupLinesPreviewStatus;
        private Krypton.Ribbon.KryptonRibbonGroupLabel kryptonRibbonGroupLabelPreviewTimer;
        private Krypton.Ribbon.KryptonRibbonGroupLabel kryptonRibbonGroupLabelPreviewStatus;
        private Krypton.Ribbon.KryptonRibbonGroup kryptonRibbonGroupPreviewSlideshow;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTriplePreviewSlideshow;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonPreviewSlideshow;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonPreviewSlideshowTimeInterval;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonPreviewChromecast;
        private Krypton.Ribbon.KryptonRibbonGroupLines kryptonRibbonGroupLinesPreviewTimer;
        private Krypton.Ribbon.KryptonRibbonGroupTrackBar kryptonRibbonGroupTrackBarPreviewTimer;
        private Krypton.Ribbon.KryptonRibbonGroup kryptonRibbonGroupHomeFileSystem;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTripleHomeDeleteRenameRefresh;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonHomeFileSystemDelete;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonHomeFileSystemRename;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTripleHomeOpenWith;
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
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTripleHomeRunEdit;
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
        private Krypton.Toolkit.KryptonGroupBox groupBoxSearchFileSystem;
        private Krypton.Toolkit.KryptonLabel kryptonLabelSearchFilename;
        private Krypton.Toolkit.KryptonLabel kryptonLabelSearchDirectory;
        private Krypton.Toolkit.KryptonTextBox kryptonTextBoxSearchFilename;
        private Krypton.Toolkit.KryptonTextBox kryptonTextBoxSearchDirectory;
        private Krypton.Toolkit.KryptonCheckBox kryptonCheckBoxSearchUseRegEx;
        private Krypton.Ribbon.KryptonRibbonGroup kryptonRibbonGroupHomeSave;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTripleHomeSave;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonHomeSaveSave;
        private Krypton.Ribbon.KryptonRibbonQATButton kryptonRibbonQATButtonViewMediaPoster;
        private Krypton.Ribbon.KryptonRibbonQATButton kryptonRibbonQATButtonViewMediaPreview;
        private Krypton.Ribbon.KryptonRibbonQATButton kryptonRibbonQATButtonMediaPreview;
        private Krypton.Ribbon.KryptonRibbonQATButton kryptonRibbonQATButtonMediaPoster;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemOpenAndAssociateWithDialog;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonHomeSortColumn;
        private Krypton.Toolkit.KryptonContextMenu kryptonContextMenuFileSystemColumnSort;
        private Krypton.Toolkit.KryptonContextMenuRadioButton kryptonContextMenuRadioButtonFileSystemColumnSortFilename;
        private Krypton.Toolkit.KryptonContextMenuRadioButton kryptonContextMenuRadioButtonFileSystemColumnSortFileCreateDate;
        private Krypton.Toolkit.KryptonContextMenuRadioButton kryptonContextMenuRadioButtonFileSystemColumnSortFileModifiedDate;
        private Krypton.Toolkit.KryptonContextMenuRadioButton kryptonContextMenuRadioButtonFileSystemColumnSortMediaDateTaken;
        private Krypton.Toolkit.KryptonContextMenuRadioButton kryptonContextMenuRadioButtonFileSystemColumnSortMediaAlbum;
        private Krypton.Toolkit.KryptonContextMenuRadioButton kryptonContextMenuRadioButtonFileSystemColumnSortMediaTitle;
        private Krypton.Toolkit.KryptonContextMenuRadioButton kryptonContextMenuRadioButtonFileSystemColumnSortMediaDescription;
        private Krypton.Toolkit.KryptonContextMenuRadioButton kryptonContextMenuRadioButtonFileSystemColumnSortMediaComments;
        private Krypton.Toolkit.KryptonContextMenuRadioButton kryptonContextMenuRadioButtonFileSystemColumnSortMediaAuthor;
        private Krypton.Toolkit.KryptonContextMenuRadioButton kryptonContextMenuRadioButtonFileSystemColumnSortMediaRating;
        private Krypton.Toolkit.KryptonContextMenuRadioButton kryptonContextMenuRadioButtonFileSystemColumnSortLocationName;
        private Krypton.Toolkit.KryptonContextMenuRadioButton kryptonContextMenuRadioButtonFileSystemColumnSortLocationRegionState;
        private Krypton.Toolkit.KryptonContextMenuRadioButton kryptonContextMenuRadioButtonFileSystemColumnSortLocationCity;
        private Krypton.Toolkit.KryptonContextMenuRadioButton kryptonContextMenuRadioButtonFileSystemColumnSortLocationCountry;
        private Krypton.Toolkit.KryptonContextMenuItems kryptonContextMenuItemsCloseMenuList;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemCloseMenu;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTripleSelectAllNoneToggle;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonSelectAll;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonSelectNone;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonSelectToggle;
        private Krypton.Ribbon.KryptonRibbonQATButton kryptonRibbonQATButtonSelectPrevius;
        private Krypton.Ribbon.KryptonRibbonQATButton kryptonRibbonQATButtonSelectNext;
        private Krypton.Ribbon.KryptonRibbonQATButton kryptonRibbonQATButtonSelectAll;
        private Krypton.Ribbon.KryptonRibbonQATButton kryptonRibbonQATButtonSelectNone;
        private Krypton.Ribbon.KryptonRibbonQATButton kryptonRibbonQATButtonSelectToggle;
        private Krypton.Ribbon.KryptonRibbonGroupCheckBox kryptonRibbonGroupCheckBoxSelectMediaTaken;
        private Krypton.Ribbon.KryptonRibbonGroupCheckBox kryptonRibbonGroupCheckBoxSelectFileCreated;
        private Krypton.Ribbon.KryptonRibbonGroupCheckBox kryptonRibbonGroupCheckBoxSelectCheckAllLocations;
        private Krypton.Ribbon.KryptonRibbonGroupCheckBox kryptonRibbonGroupCheckBoxSelectCheckAllDates;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonSelectEqual;
        private Krypton.Ribbon.KryptonRibbonQATButton kryptonRibbonQATButtonSelectEqual;
        private Krypton.Toolkit.KryptonContextMenu kryptonContextMenuPreviewSlideshowInterval;
        private Krypton.Toolkit.KryptonContextMenuItems kryptonContextMenuItemsPreviewSlideshowIntervalList;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemPreviewSlideshowIntervalStop;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTriplePreviewPlayPause;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonPreviewPlay;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonPreviewPause;
        private Krypton.Toolkit.KryptonContextMenuRadioButton kryptonContextMenuRadioButtonSlideshow2sec;
        private Krypton.Toolkit.KryptonContextMenuRadioButton kryptonContextMenuRadioButtonSlideshow4sec;
        private Krypton.Toolkit.KryptonContextMenuRadioButton kryptonContextMenuRadioButtonSlideshow6sec;
        private Krypton.Toolkit.KryptonContextMenuRadioButton kryptonContextMenuRadioButtonSlideshow8sec;
        private Krypton.Toolkit.KryptonContextMenuRadioButton kryptonContextMenuRadioButtonSlideshow10sec;
        private Krypton.Toolkit.KryptonContextMenuRadioButton kryptonContextMenuRadioButton1;
        private Krypton.Toolkit.KryptonContextMenuRadioButton kryptonContextMenuRadioButton2;
        private Krypton.Toolkit.KryptonContextMenuRadioButton kryptonContextMenuRadioButton3;
        private Krypton.Toolkit.KryptonContextMenuRadioButton kryptonContextMenuRadioButton4;
        private Krypton.Toolkit.KryptonContextMenuRadioButton kryptonContextMenuRadioButton5;
        private Krypton.Toolkit.KryptonContextMenuRadioButton kryptonContextMenuRadioButton6;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemAssignCompositeTag;
        private Krypton.Toolkit.KryptonContextMenuItems kryptonContextMenuItemsAssignCompositeTagList;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItemAssignCompositeTagExample;
        private Krypton.Ribbon.KryptonRibbonGroup kryptonRibbonGroupToolsProgressStatus;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTripleToolsProgressStatusWork;
        private Krypton.Ribbon.KryptonRibbonGroupLabel kryptonRibbonGroupLabelToolsProgressLazyloading;
        private Krypton.Ribbon.KryptonRibbonGroupCustomControl kryptonRibbonGroupCustomControlToolsProgressLazyloading;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTripleProgressStatusBackground;
        private Krypton.Ribbon.KryptonRibbonGroupLabel kryptonRibbonGroupLabelToolsProgressBackground;
        private Krypton.Ribbon.KryptonRibbonGroupCustomControl kryptonRibbonGroupCustomControlToolsProgressBackground;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTripleProgressStatusSave;
        private Krypton.Ribbon.KryptonRibbonGroupLabel kryptonRibbonGroupLabelToolsProgressSave;
        private Krypton.Ribbon.KryptonRibbonGroupCustomControl kryptonRibbonGroupCustomControlToolsProgressSave;
        private Krypton.Ribbon.KryptonRibbonGroupLabel kryptonRibbonGroupLabelToolsProgressBackgroundBackgroundProcessText;
        private Krypton.Ribbon.KryptonRibbonGroupLabel kryptonRibbonGroupLabelToolsProgressSaveText;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTriple1;
        private Krypton.Ribbon.KryptonRibbonGroupLabel kryptonRibbonGroupLabelToolsCurrentActions;
        private Krypton.Ribbon.KryptonRibbonGroupLabel kryptonRibbonGroupLabelCurrentActionsHeading;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonToolsProgressLazyloadingShowStatus;
        private Krypton.Navigator.ButtonSpecNavigator buttonSpecNavigatorDataGridViewProgressCircle;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTriple2;
        private Krypton.Ribbon.KryptonRibbonGroupButton kryptonRibbonGroupButtonPreviewSlideshowPlay;
        private Krypton.Ribbon.KryptonRibbonQATButton kryptonRibbonQATButtonMediaPlayerPrevious;
        private Krypton.Ribbon.KryptonRibbonQATButton kryptonRibbonQATButtonMediaPlayerNext;
        private Krypton.Ribbon.KryptonRibbonQATButton kryptonRibbonQATButtonMediaPlayerPlay;
        private Krypton.Ribbon.KryptonRibbonQATButton kryptonRibbonQATButtonMediaPlayerPause;
        private Krypton.Ribbon.KryptonRibbonQATButton kryptonRibbonQATButtonMediaPlayerStop;
        private Krypton.Ribbon.KryptonRibbonQATButton kryptonRibbonQATButtonMediaPlayerSlideshowPlay;
        private Krypton.Ribbon.KryptonRibbonQATButton kryptonRibbonQATButtonMediaPlayerFastBackwards;
        private Krypton.Ribbon.KryptonRibbonQATButton kryptonRibbonQATButtonMediaPlayerFastForward;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTags;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTagConfidence;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTagRationg;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSerachSearch;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSearchFileSystem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSearchPeople;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSearchDetails;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSearchKeywords;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSearchMediaTaken;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSearchAttributes;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSearchRating;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSerachActions;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMap;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelRename;
        private Krypton.Navigator.ButtonSpecNavigator buttonSpecNavigatorExpandCollapse;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItem9;
        private Krypton.Toolkit.KryptonContextMenuMonthCalendar kryptonContextMenuMonthCalendar3;
        private Krypton.Toolkit.KryptonContextMenuMonthCalendar kryptonContextMenuMonthCalendar4;
        private Krypton.Toolkit.KryptonContextMenuImageSelect kryptonContextMenuImageSelect2;
        private Krypton.Toolkit.KryptonContextMenuColorColumns kryptonContextMenuColorColumns1;
        private Krypton.Toolkit.KryptonContextMenuLinkLabel kryptonContextMenuLinkLabel1;
        private Krypton.Toolkit.KryptonContextMenuRadioButton kryptonContextMenuRadioButton7;
        private Krypton.Toolkit.KryptonContextMenuCheckButton kryptonContextMenuCheckButton1;
        private Krypton.Toolkit.KryptonContextMenuCheckBox kryptonContextMenuCheckBox1;
        private Krypton.Toolkit.KryptonContextMenuMonthCalendar kryptonContextMenuMonthCalendar5;
        private Krypton.Toolkit.KryptonContextMenuColorColumns kryptonContextMenuColorColumns2;
        private Krypton.Toolkit.KryptonContextMenuImageSelect kryptonContextMenuImageSelect3;
        private Krypton.Toolkit.KryptonLabel kryptonLabelEndOfPage;
        private Raccoom.Windows.Forms.TreeViewFolderBrowser treeViewFolderBrowser1;
    }
}

