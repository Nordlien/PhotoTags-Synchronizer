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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Names");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Album");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Date");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusFilesAndSelected = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusQueue = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusAction = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
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
            this.tabPageFilterTags = new System.Windows.Forms.TabPage();
            this.treeViewFilter = new System.Windows.Forms.TreeView();
            this.splitContainerImages = new System.Windows.Forms.SplitContainer();
            this.imageListView1 = new Manina.Windows.Forms.ImageListView();
            this.contextMenuStripImageListView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemImageListViewCut = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemImageListViewCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemImageListViewPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemImageListViewDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemImageListViewRefreshFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemImageListViewReloadThumbnailAndMetadata = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemImageListViewReloadThumbnailAndMetadataClearThumbnailAndMetadataHistory = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemImageListViewSelectAll = new System.Windows.Forms.ToolStripMenuItem();
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
            this.peopleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemPeopleSelected = new System.Windows.Forms.ToolStripMenuItem();
            this.meToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.comboBoxMapZoomLevel = new System.Windows.Forms.ComboBox();
            this.textBoxBrowserURL = new System.Windows.Forms.TextBox();
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
            this.toolStripMenuItemExiftoolReplace = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExiftoolMarkFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExiftoolRemoveFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExiftoolToggleFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExiftoolSHowFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExiftoolHideEqual = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPageExifToolWarning = new System.Windows.Forms.TabPage();
            this.dataGridViewExifToolWarning = new System.Windows.Forms.DataGridView();
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
            this.rotateCWToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
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
            this.toolStripButtonAbout = new System.Windows.Forms.ToolStripButton();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.timerShowErrorMessage = new System.Windows.Forms.Timer(this.components);
            this.timerActionStatusRemove = new System.Windows.Forms.Timer(this.components);
            this.toolStripMenuItemImageListViewAutoCorrect = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerFolder)).BeginInit();
            this.splitContainerFolder.Panel1.SuspendLayout();
            this.splitContainerFolder.Panel2.SuspendLayout();
            this.splitContainerFolder.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageFilterFolder.SuspendLayout();
            this.contextMenuStripTreeViewFolder.SuspendLayout();
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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabPageDate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDate)).BeginInit();
            this.tabPageExifTool.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewExifTool)).BeginInit();
            this.contextMenuStripExifTool.SuspendLayout();
            this.tabPageExifToolWarning.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewExifToolWarning)).BeginInit();
            this.tabPageFileProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProperties)).BeginInit();
            this.tabPageFileRename.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRename)).BeginInit();
            this.toolStrip.SuspendLayout();
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
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainerMain);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(1387, 673);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.RightToolStripPanelVisible = false;
            this.toolStripContainer1.Size = new System.Drawing.Size(1387, 731);
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
            this.toolStripStatusAction.Name = "toolStripStatusAction";
            this.toolStripStatusAction.Size = new System.Drawing.Size(49, 24);
            this.toolStripStatusAction.Text = "Status";
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.BackColor = System.Drawing.Color.Black;
            this.splitContainerMain.Cursor = System.Windows.Forms.Cursors.Default;
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainerMain.Panel1.Controls.Add(this.splitContainerFolder);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainerMain.Size = new System.Drawing.Size(1387, 673);
            this.splitContainerMain.SplitterDistance = 493;
            this.splitContainerMain.SplitterWidth = 10;
            this.splitContainerMain.TabIndex = 1;
            this.splitContainerMain.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainerMain_SplitterMoved);
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
            this.splitContainerFolder.Size = new System.Drawing.Size(1387, 493);
            this.splitContainerFolder.SplitterDistance = 339;
            this.splitContainerFolder.SplitterWidth = 10;
            this.splitContainerFolder.TabIndex = 0;
            this.splitContainerFolder.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainerFolder_SplitterMoved);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageFilterFolder);
            this.tabControl1.Controls.Add(this.tabPageFilterTags);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(339, 493);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageFilterFolder
            // 
            this.tabPageFilterFolder.Controls.Add(this.folderTreeViewFolder);
            this.tabPageFilterFolder.Location = new System.Drawing.Point(4, 26);
            this.tabPageFilterFolder.Name = "tabPageFilterFolder";
            this.tabPageFilterFolder.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFilterFolder.Size = new System.Drawing.Size(331, 463);
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
            this.folderTreeViewFolder.Size = new System.Drawing.Size(325, 457);
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
            this.toolStripMenuItemTreeViewFolderAutoCorrectMetadata});
            this.contextMenuStripTreeViewFolder.Name = "contextMenuStripImageListView";
            this.contextMenuStripTreeViewFolder.Size = new System.Drawing.Size(390, 238);
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
            // tabPageFilterTags
            // 
            this.tabPageFilterTags.Controls.Add(this.treeViewFilter);
            this.tabPageFilterTags.Location = new System.Drawing.Point(4, 25);
            this.tabPageFilterTags.Name = "tabPageFilterTags";
            this.tabPageFilterTags.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFilterTags.Size = new System.Drawing.Size(331, 461);
            this.tabPageFilterTags.TabIndex = 1;
            this.tabPageFilterTags.Text = "Tags";
            this.tabPageFilterTags.UseVisualStyleBackColor = true;
            // 
            // treeViewFilter
            // 
            this.treeViewFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewFilter.Location = new System.Drawing.Point(3, 3);
            this.treeViewFilter.Name = "treeViewFilter";
            treeNode1.Checked = true;
            treeNode1.Name = "Node0";
            treeNode1.Text = "Names";
            treeNode2.Name = "Node1";
            treeNode2.Text = "Album";
            treeNode3.Checked = true;
            treeNode3.Name = "Node2";
            treeNode3.Text = "Date";
            this.treeViewFilter.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3});
            this.treeViewFilter.Size = new System.Drawing.Size(325, 455);
            this.treeViewFilter.TabIndex = 0;
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
            this.splitContainerImages.Size = new System.Drawing.Size(1038, 493);
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
            this.imageListView1.Size = new System.Drawing.Size(484, 493);
            this.imageListView1.TabIndex = 1;
            this.imageListView1.Text = "";
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
            this.toolStripMenuItemImageListViewPaste,
            this.toolStripMenuItemImageListViewDelete,
            this.toolStripMenuItemImageListViewRefreshFolder,
            this.toolStripMenuItemImageListViewReloadThumbnailAndMetadata,
            this.toolStripMenuItemImageListViewReloadThumbnailAndMetadataClearThumbnailAndMetadataHistory,
            this.toolStripMenuItemImageListViewSelectAll,
            this.toolStripMenuItemImageListViewAutoCorrect});
            this.contextMenuStripImageListView.Name = "contextMenuStripImageListView";
            this.contextMenuStripImageListView.Size = new System.Drawing.Size(390, 266);
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
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.tabControlToolbox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(544, 493);
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
            this.tabControlToolbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlToolbox.Location = new System.Drawing.Point(0, 0);
            this.tabControlToolbox.Name = "tabControlToolbox";
            this.tabControlToolbox.SelectedIndex = 0;
            this.tabControlToolbox.Size = new System.Drawing.Size(544, 493);
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
            this.tabPageTags.Size = new System.Drawing.Size(536, 463);
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
            this.comboBoxAuthor.FormattingEnabled = true;
            this.comboBoxAuthor.Location = new System.Drawing.Point(88, 115);
            this.comboBoxAuthor.Name = "comboBoxAuthor";
            this.comboBoxAuthor.Size = new System.Drawing.Size(439, 25);
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
            this.dataGridViewTagsAndKeywords.Size = new System.Drawing.Size(530, 246);
            this.dataGridViewTagsAndKeywords.TabIndex = 10;
            this.dataGridViewTagsAndKeywords.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewTagsAndKeywords_CellBeginEdit);
            this.dataGridViewTagsAndKeywords.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTagsAndKeywords_CellEndEdit);
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
            this.removeTagsAndKeywordsToolStripMenuItem});
            this.contextMenuStripTagsAndKeywords.Name = "contextMenuStripMap";
            this.contextMenuStripTagsAndKeywords.Size = new System.Drawing.Size(521, 498);
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
            this.comboBoxAlbum.FormattingEnabled = true;
            this.comboBoxAlbum.Location = new System.Drawing.Point(88, 7);
            this.comboBoxAlbum.Name = "comboBoxAlbum";
            this.comboBoxAlbum.Size = new System.Drawing.Size(439, 25);
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
            this.comboBoxComments.FormattingEnabled = true;
            this.comboBoxComments.Location = new System.Drawing.Point(88, 88);
            this.comboBoxComments.Name = "comboBoxComments";
            this.comboBoxComments.Size = new System.Drawing.Size(439, 25);
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
            this.comboBoxDescription.FormattingEnabled = true;
            this.comboBoxDescription.Location = new System.Drawing.Point(88, 61);
            this.comboBoxDescription.Name = "comboBoxDescription";
            this.comboBoxDescription.Size = new System.Drawing.Size(439, 25);
            this.comboBoxDescription.TabIndex = 1;
            this.comboBoxDescription.TextChanged += new System.EventHandler(this.comboBoxDescription_TextChanged);
            // 
            // comboBoxTitle
            // 
            this.comboBoxTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxTitle.FormattingEnabled = true;
            this.comboBoxTitle.Location = new System.Drawing.Point(88, 34);
            this.comboBoxTitle.Name = "comboBoxTitle";
            this.comboBoxTitle.Size = new System.Drawing.Size(439, 25);
            this.comboBoxTitle.TabIndex = 0;
            this.comboBoxTitle.TextChanged += new System.EventHandler(this.comboBoxTitle_TextChanged);
            // 
            // tabPagePeople
            // 
            this.tabPagePeople.Controls.Add(this.dataGridViewPeople);
            this.tabPagePeople.Location = new System.Drawing.Point(4, 25);
            this.tabPagePeople.Name = "tabPagePeople";
            this.tabPagePeople.Size = new System.Drawing.Size(536, 461);
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
            this.dataGridViewPeople.Size = new System.Drawing.Size(533, 465);
            this.dataGridViewPeople.TabIndex = 0;
            this.dataGridViewPeople.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewPeople_CellBeginEdit);
            this.dataGridViewPeople.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewPeople_CellMouseClick);
            this.dataGridViewPeople.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewPeople_CellMouseDown);
            this.dataGridViewPeople.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewPeople_CellMouseLeave);
            this.dataGridViewPeople.CellMouseMove += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewPeople_CellMouseMove);
            this.dataGridViewPeople.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewPeople_CellMouseUp);
            this.dataGridViewPeople.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridViewPeople_CellPainting);
            this.dataGridViewPeople.CellParsing += new System.Windows.Forms.DataGridViewCellParsingEventHandler(this.dataGridViewPeople_CellParsing);
            this.dataGridViewPeople.SelectionChanged += new System.EventHandler(this.dataGridViewPeople_SelectionChanged);
            this.dataGridViewPeople.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridViewPeople_KeyDown);
            // 
            // contextMenuStripPeople
            // 
            this.contextMenuStripPeople.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripPeople.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.peopleToolStripMenuItem,
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
            this.toolStripMenuItemPeopleRemovePeopleTag});
            this.contextMenuStripPeople.Name = "contextMenuStripMap";
            this.contextMenuStripPeople.Size = new System.Drawing.Size(368, 472);
            // 
            // peopleToolStripMenuItem
            // 
            this.peopleToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemPeopleSelected,
            this.meToolStripMenuItem});
            this.peopleToolStripMenuItem.Name = "peopleToolStripMenuItem";
            this.peopleToolStripMenuItem.Size = new System.Drawing.Size(367, 26);
            this.peopleToolStripMenuItem.Text = "People";
            // 
            // ToolStripMenuItemPeopleSelected
            // 
            this.ToolStripMenuItemPeopleSelected.Name = "ToolStripMenuItemPeopleSelected";
            this.ToolStripMenuItemPeopleSelected.Size = new System.Drawing.Size(163, 26);
            this.ToolStripMenuItemPeopleSelected.Text = "(Unknown)";
            // 
            // meToolStripMenuItem
            // 
            this.meToolStripMenuItem.Name = "meToolStripMenuItem";
            this.meToolStripMenuItem.Size = new System.Drawing.Size(163, 26);
            this.meToolStripMenuItem.Text = "Me";
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
            // tabPageMap
            // 
            this.tabPageMap.Controls.Add(this.splitContainerMap);
            this.tabPageMap.Location = new System.Drawing.Point(4, 25);
            this.tabPageMap.Name = "tabPageMap";
            this.tabPageMap.Size = new System.Drawing.Size(536, 461);
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
            this.splitContainerMap.Panel2.Controls.Add(this.pictureBox1);
            this.splitContainerMap.Panel2.Controls.Add(this.comboBoxMapZoomLevel);
            this.splitContainerMap.Panel2.Controls.Add(this.textBoxBrowserURL);
            this.splitContainerMap.Panel2.Controls.Add(this.panelBrowser);
            this.splitContainerMap.Size = new System.Drawing.Size(536, 461);
            this.splitContainerMap.SplitterDistance = 214;
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
            this.dataGridViewMap.Size = new System.Drawing.Size(536, 177);
            this.dataGridViewMap.TabIndex = 10;
            this.dataGridViewMap.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewMap_CellBeginEdit);
            this.dataGridViewMap.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewMap_CellEndEdit);
            this.dataGridViewMap.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewMap_CellMouseDoubleClick);
            this.dataGridViewMap.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridViewMap_CellPainting);
            this.dataGridViewMap.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewMap_CellValueChanged);
            this.dataGridViewMap.Enter += new System.EventHandler(this.dataGridViewMap_Enter);
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
            this.toolStripMenuItemShowCoordinateOnMap});
            this.contextMenuStripMap.Name = "contextMenuStripMap";
            this.contextMenuStripMap.Size = new System.Drawing.Size(521, 446);
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
            this.toolStripMenuItemShowCoordinateOnMap.Name = "toolStripMenuItemShowCoordinateOnMap";
            this.toolStripMenuItemShowCoordinateOnMap.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.toolStripMenuItemShowCoordinateOnMap.Size = new System.Drawing.Size(520, 26);
            this.toolStripMenuItemShowCoordinateOnMap.Text = "Show Coordinate on Map";
            this.toolStripMenuItemShowCoordinateOnMap.Click += new System.EventHandler(this.toolStripMenuItemShowCoordinateOnMap_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(5, 2);
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
            this.comboBoxMapZoomLevel.Location = new System.Drawing.Point(45, 3);
            this.comboBoxMapZoomLevel.Name = "comboBoxMapZoomLevel";
            this.comboBoxMapZoomLevel.Size = new System.Drawing.Size(97, 25);
            this.comboBoxMapZoomLevel.TabIndex = 15;
            this.comboBoxMapZoomLevel.SelectedIndexChanged += new System.EventHandler(this.comboBoxMapZoomLevel_SelectedIndexChanged);
            // 
            // textBoxBrowserURL
            // 
            this.textBoxBrowserURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBrowserURL.Location = new System.Drawing.Point(148, 3);
            this.textBoxBrowserURL.Name = "textBoxBrowserURL";
            this.textBoxBrowserURL.Size = new System.Drawing.Size(383, 24);
            this.textBoxBrowserURL.TabIndex = 9;
            this.textBoxBrowserURL.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxBrowserURL_KeyPress);
            // 
            // panelBrowser
            // 
            this.panelBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelBrowser.BackColor = System.Drawing.SystemColors.Control;
            this.panelBrowser.Cursor = System.Windows.Forms.Cursors.Default;
            this.panelBrowser.Location = new System.Drawing.Point(0, 33);
            this.panelBrowser.Name = "panelBrowser";
            this.panelBrowser.Size = new System.Drawing.Size(536, 147);
            this.panelBrowser.TabIndex = 1;
            // 
            // tabPageDate
            // 
            this.tabPageDate.Controls.Add(this.textBox1);
            this.tabPageDate.Controls.Add(this.dataGridViewDate);
            this.tabPageDate.Location = new System.Drawing.Point(4, 25);
            this.tabPageDate.Name = "tabPageDate";
            this.tabPageDate.Size = new System.Drawing.Size(536, 461);
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
            this.textBox1.Size = new System.Drawing.Size(527, 80);
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
            this.dataGridViewDate.Size = new System.Drawing.Size(533, 376);
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
            this.tabPageExifTool.Size = new System.Drawing.Size(536, 461);
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
            this.dataGridViewExifTool.Size = new System.Drawing.Size(536, 461);
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
            this.toolStripMenuItemExiftoolReplace,
            this.toolStripMenuItemExiftoolMarkFavorite,
            this.toolStripMenuItemExiftoolRemoveFavorite,
            this.toolStripMenuItemExiftoolToggleFavorite,
            this.toolStripMenuItemExiftoolSHowFavorite,
            this.toolStripMenuItemExiftoolHideEqual});
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
            // toolStripMenuItemExiftoolReplace
            // 
            this.toolStripMenuItemExiftoolReplace.Image = global::PhotoTagsSynchronizer.Properties.Resources.Replace;
            this.toolStripMenuItemExiftoolReplace.Name = "toolStripMenuItemExiftoolReplace";
            this.toolStripMenuItemExiftoolReplace.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.toolStripMenuItemExiftoolReplace.Size = new System.Drawing.Size(302, 26);
            this.toolStripMenuItemExiftoolReplace.Text = "Replace";
            this.toolStripMenuItemExiftoolReplace.Click += new System.EventHandler(this.toolStripMenuItemExiftoolReplace_Click);
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
            // tabPageExifToolWarning
            // 
            this.tabPageExifToolWarning.Controls.Add(this.dataGridViewExifToolWarning);
            this.tabPageExifToolWarning.Location = new System.Drawing.Point(4, 25);
            this.tabPageExifToolWarning.Name = "tabPageExifToolWarning";
            this.tabPageExifToolWarning.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageExifToolWarning.Size = new System.Drawing.Size(536, 461);
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
            this.dataGridViewExifToolWarning.Size = new System.Drawing.Size(530, 455);
            this.dataGridViewExifToolWarning.TabIndex = 0;
            this.dataGridViewExifToolWarning.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewExifToolWarning_CellBeginEdit);
            this.dataGridViewExifToolWarning.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridViewExifToolWarning_CellPainting);
            this.dataGridViewExifToolWarning.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridViewExifToolWarningData_KeyDown);
            // 
            // tabPageFileProperties
            // 
            this.tabPageFileProperties.Controls.Add(this.dataGridViewProperties);
            this.tabPageFileProperties.Location = new System.Drawing.Point(4, 25);
            this.tabPageFileProperties.Name = "tabPageFileProperties";
            this.tabPageFileProperties.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFileProperties.Size = new System.Drawing.Size(536, 461);
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
            this.dataGridViewProperties.Size = new System.Drawing.Size(530, 455);
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
            this.tabPageFileRename.Size = new System.Drawing.Size(536, 461);
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
            this.comboBoxRenameVariableList.Size = new System.Drawing.Size(411, 25);
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
            this.textBoxRenameNewName.Size = new System.Drawing.Size(411, 24);
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
            this.dataGridViewRename.Size = new System.Drawing.Size(533, 357);
            this.dataGridViewRename.TabIndex = 0;
            this.dataGridViewRename.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewRename_CellBeginEdit);
            this.dataGridViewRename.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridViewRename_CellPainting);
            this.dataGridViewRename.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridViewRename_KeyDown);
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
            this.rotateCWToolStripButton,
            this.toolStripSeparator2,
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
            this.toolStripButtonAbout});
            this.toolStrip.Location = new System.Drawing.Point(4, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(837, 28);
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
            this.rotateCCWToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("rotateCCWToolStripButton.Image")));
            this.rotateCCWToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.rotateCCWToolStripButton.Name = "rotateCCWToolStripButton";
            this.rotateCCWToolStripButton.Size = new System.Drawing.Size(29, 25);
            this.rotateCCWToolStripButton.Text = "Rotate Counter-clockwise";
            this.rotateCCWToolStripButton.Click += new System.EventHandler(this.rotateCCWToolStripButton_Click);
            // 
            // rotateCWToolStripButton
            // 
            this.rotateCWToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.rotateCWToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("rotateCWToolStripButton.Image")));
            this.rotateCWToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.rotateCWToolStripButton.Name = "rotateCWToolStripButton";
            this.rotateCWToolStripButton.Size = new System.Drawing.Size(29, 25);
            this.rotateCWToolStripButton.Text = "Rotate Clockwise";
            this.rotateCWToolStripButton.Click += new System.EventHandler(this.rotateCWToolStripButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 28);
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
            // openFileDialog
            // 
            this.openFileDialog.Filter = resources.GetString("openFileDialog.Filter");
            this.openFileDialog.Multiselect = true;
            this.openFileDialog.ShowReadOnly = true;
            // 
            // timerShowErrorMessage
            // 
            this.timerShowErrorMessage.Interval = 1000;
            this.timerShowErrorMessage.Tick += new System.EventHandler(this.timerShowErrorMessage_Tick);
            // 
            // timerActionStatusRemove
            // 
            this.timerActionStatusRemove.Interval = 2000;
            this.timerActionStatusRemove.Tick += new System.EventHandler(this.timerActionStatusRemove_Tick);
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
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1387, 731);
            this.Controls.Add(this.toolStripContainer1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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
            this.splitContainerMain.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.splitContainerFolder.Panel1.ResumeLayout(false);
            this.splitContainerFolder.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerFolder)).EndInit();
            this.splitContainerFolder.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPageFilterFolder.ResumeLayout(false);
            this.contextMenuStripTreeViewFolder.ResumeLayout(false);
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
            this.splitContainerMap.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMap)).EndInit();
            this.splitContainerMap.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMap)).EndInit();
            this.contextMenuStripMap.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabPageDate.ResumeLayout(false);
            this.tabPageDate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDate)).EndInit();
            this.tabPageExifTool.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewExifTool)).EndInit();
            this.contextMenuStripExifTool.ResumeLayout(false);
            this.tabPageExifToolWarning.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewExifToolWarning)).EndInit();
            this.tabPageFileProperties.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProperties)).EndInit();
            this.tabPageFileRename.ResumeLayout(false);
            this.tabPageFileRename.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRename)).EndInit();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
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
        private System.Windows.Forms.SplitContainer splitContainerMain;
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
        private System.Windows.Forms.TreeView treeViewFilter;
        private System.Windows.Forms.ToolStripMenuItem peopleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemPeopleSelected;
        private System.Windows.Forms.ToolStripMenuItem meToolStripMenuItem;
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
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExiftoolReplace;
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
    }
}

