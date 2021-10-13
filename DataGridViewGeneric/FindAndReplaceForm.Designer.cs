using Krypton.Navigator;

namespace DataGridViewGeneric
{
    partial class FindAndReplaceForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FindAndReplaceForm));
            this.checkBoxSearchAlsoRowHeaders = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.radioButtonSearchDown1 = new Krypton.Toolkit.KryptonRadioButton();
            this.radioButtonSearchUp1 = new Krypton.Toolkit.KryptonRadioButton();
            this.buttonFindAll1 = new Krypton.Toolkit.KryptonButton();
            this.FindButton1 = new Krypton.Toolkit.KryptonButton();
            this.comboBoxFindMode = new System.Windows.Forms.ComboBox();
            this.FindOptionGroupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.MatchCellCheckBox1 = new System.Windows.Forms.CheckBox();
            this.MatchCaseCheckBox1 = new System.Windows.Forms.CheckBox();
            this.FindWhatTextBox1 = new Krypton.Toolkit.KryptonTextBox();
            this.FindLabel1 = new Krypton.Toolkit.KryptonLabel();
            this.groupBoxFindDirection2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.radioButtonSearchDown2 = new Krypton.Toolkit.KryptonRadioButton();
            this.radioButtonSearchUp2 = new Krypton.Toolkit.KryptonRadioButton();
            this.buttonFindAll2 = new Krypton.Toolkit.KryptonButton();
            this.FindOptionGroup2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.MatchCellCheckBox2 = new System.Windows.Forms.CheckBox();
            this.MatchCaseCheckBox2 = new System.Windows.Forms.CheckBox();
            this.UseComboBox2 = new System.Windows.Forms.ComboBox();
            this.ReplaceAllButton = new Krypton.Toolkit.KryptonButton();
            this.ReplaceButton = new Krypton.Toolkit.KryptonButton();
            this.FindButton2 = new Krypton.Toolkit.KryptonButton();
            this.ReplaceWithTextBox = new Krypton.Toolkit.KryptonTextBox();
            this.ReplaceLabel = new Krypton.Toolkit.KryptonLabel();
            this.FindWhatTextBox2 = new Krypton.Toolkit.KryptonTextBox();
            this.FindLabel2 = new Krypton.Toolkit.KryptonLabel();
            this.kryptonNavigatorFindAndReplace = new Krypton.Navigator.KryptonNavigator();
            this.kryptonPageFind = new Krypton.Navigator.KryptonPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.kryptonPageReplace = new Krypton.Navigator.KryptonPage();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.FindOptionGroupBox1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBoxFindDirection2.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.FindOptionGroup2.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonNavigatorFindAndReplace)).BeginInit();
            this.kryptonNavigatorFindAndReplace.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageFind)).BeginInit();
            this.kryptonPageFind.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageReplace)).BeginInit();
            this.kryptonPageReplace.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBoxSearchAlsoRowHeaders
            // 
            this.checkBoxSearchAlsoRowHeaders.Location = new System.Drawing.Point(73, 176);
            this.checkBoxSearchAlsoRowHeaders.Name = "checkBoxSearchAlsoRowHeaders";
            this.checkBoxSearchAlsoRowHeaders.Size = new System.Drawing.Size(209, 21);
            this.checkBoxSearchAlsoRowHeaders.TabIndex = 11;
            this.checkBoxSearchAlsoRowHeaders.Text = "Search also in Row Headers";
            this.checkBoxSearchAlsoRowHeaders.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel3);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(323, 39);
            this.groupBox1.Name = "groupBox1";
            this.tableLayoutPanel1.SetRowSpan(this.groupBox1, 2);
            this.groupBox1.Size = new System.Drawing.Size(244, 102);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Find directions";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.radioButtonSearchDown1, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.radioButtonSearchUp1, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(238, 83);
            this.tableLayoutPanel3.TabIndex = 12;
            // 
            // radioButtonSearchDown1
            // 
            this.radioButtonSearchDown1.Checked = true;
            this.radioButtonSearchDown1.Location = new System.Drawing.Point(3, 27);
            this.radioButtonSearchDown1.Name = "radioButtonSearchDown1";
            this.radioButtonSearchDown1.Size = new System.Drawing.Size(106, 18);
            this.radioButtonSearchDown1.TabIndex = 7;
            this.radioButtonSearchDown1.Values.Text = "Find downwards";
            this.radioButtonSearchDown1.CheckedChanged += new System.EventHandler(this.radioButtonSearchDown1_CheckedChanged);
            // 
            // radioButtonSearchUp1
            // 
            this.radioButtonSearchUp1.Location = new System.Drawing.Point(3, 3);
            this.radioButtonSearchUp1.Name = "radioButtonSearchUp1";
            this.radioButtonSearchUp1.Size = new System.Drawing.Size(91, 18);
            this.radioButtonSearchUp1.TabIndex = 6;
            this.radioButtonSearchUp1.Values.Text = "Find upwards";
            this.radioButtonSearchUp1.CheckedChanged += new System.EventHandler(this.radioButtonSearchUp1_CheckedChanged);
            // 
            // buttonFindAll1
            // 
            this.buttonFindAll1.Location = new System.Drawing.Point(574, 40);
            this.buttonFindAll1.Margin = new System.Windows.Forms.Padding(4);
            this.buttonFindAll1.Name = "buttonFindAll1";
            this.buttonFindAll1.Size = new System.Drawing.Size(119, 28);
            this.buttonFindAll1.TabIndex = 10;
            this.buttonFindAll1.Values.Text = "Find All";
            this.buttonFindAll1.Click += new System.EventHandler(this.buttonFindAll1_Click);
            // 
            // FindButton1
            // 
            this.FindButton1.Location = new System.Drawing.Point(574, 4);
            this.FindButton1.Margin = new System.Windows.Forms.Padding(4);
            this.FindButton1.Name = "FindButton1";
            this.FindButton1.Size = new System.Drawing.Size(119, 28);
            this.FindButton1.TabIndex = 9;
            this.FindButton1.Values.Text = "Find Next";
            this.FindButton1.Click += new System.EventHandler(this.FindButton1_Click);
            // 
            // comboBoxFindMode
            // 
            this.comboBoxFindMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.comboBoxFindMode, 2);
            this.comboBoxFindMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFindMode.FormattingEnabled = true;
            this.comboBoxFindMode.Items.AddRange(new object[] {
            "Find text that match",
            "Find text that match using regular expressions",
            "Find text that match wildcards"});
            this.comboBoxFindMode.Location = new System.Drawing.Point(74, 148);
            this.comboBoxFindMode.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxFindMode.Name = "comboBoxFindMode";
            this.comboBoxFindMode.Size = new System.Drawing.Size(492, 21);
            this.comboBoxFindMode.TabIndex = 8;
            this.comboBoxFindMode.SelectedIndexChanged += new System.EventHandler(this.UseComboBox1_SelectedIndexChanged);
            // 
            // FindOptionGroupBox1
            // 
            this.FindOptionGroupBox1.Controls.Add(this.tableLayoutPanel2);
            this.FindOptionGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FindOptionGroupBox1.Location = new System.Drawing.Point(74, 40);
            this.FindOptionGroupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.FindOptionGroupBox1.Name = "FindOptionGroupBox1";
            this.FindOptionGroupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.SetRowSpan(this.FindOptionGroupBox1, 2);
            this.FindOptionGroupBox1.Size = new System.Drawing.Size(242, 100);
            this.FindOptionGroupBox1.TabIndex = 2;
            this.FindOptionGroupBox1.TabStop = false;
            this.FindOptionGroupBox1.Text = "Find options";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.MatchCellCheckBox1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.MatchCaseCheckBox1, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(4, 17);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(234, 79);
            this.tableLayoutPanel2.TabIndex = 12;
            // 
            // MatchCellCheckBox1
            // 
            this.MatchCellCheckBox1.Location = new System.Drawing.Point(4, 33);
            this.MatchCellCheckBox1.Margin = new System.Windows.Forms.Padding(4);
            this.MatchCellCheckBox1.Name = "MatchCellCheckBox1";
            this.MatchCellCheckBox1.Size = new System.Drawing.Size(133, 21);
            this.MatchCellCheckBox1.TabIndex = 4;
            this.MatchCellCheckBox1.Text = "Match whole cell";
            this.MatchCellCheckBox1.UseVisualStyleBackColor = true;
            this.MatchCellCheckBox1.CheckedChanged += new System.EventHandler(this.MatchCellCheckBox1_CheckedChanged);
            // 
            // MatchCaseCheckBox1
            // 
            this.MatchCaseCheckBox1.Location = new System.Drawing.Point(4, 4);
            this.MatchCaseCheckBox1.Margin = new System.Windows.Forms.Padding(4);
            this.MatchCaseCheckBox1.Name = "MatchCaseCheckBox1";
            this.MatchCaseCheckBox1.Size = new System.Drawing.Size(102, 21);
            this.MatchCaseCheckBox1.TabIndex = 3;
            this.MatchCaseCheckBox1.Text = "Match case";
            this.MatchCaseCheckBox1.UseVisualStyleBackColor = true;
            this.MatchCaseCheckBox1.CheckedChanged += new System.EventHandler(this.MatchCaseCheckBox1_CheckedChanged);
            // 
            // FindWhatTextBox1
            // 
            this.FindWhatTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.FindWhatTextBox1, 2);
            this.FindWhatTextBox1.Location = new System.Drawing.Point(74, 4);
            this.FindWhatTextBox1.Margin = new System.Windows.Forms.Padding(4);
            this.FindWhatTextBox1.Name = "FindWhatTextBox1";
            this.FindWhatTextBox1.Size = new System.Drawing.Size(492, 20);
            this.FindWhatTextBox1.TabIndex = 0;
            this.FindWhatTextBox1.TextChanged += new System.EventHandler(this.FindWhatTextBox1_TextChanged);
            // 
            // FindLabel1
            // 
            this.FindLabel1.Location = new System.Drawing.Point(4, 0);
            this.FindLabel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.FindLabel1.Name = "FindLabel1";
            this.FindLabel1.Size = new System.Drawing.Size(62, 18);
            this.FindLabel1.TabIndex = 4;
            this.FindLabel1.Values.Text = "Find what:";
            // 
            // groupBoxFindDirection2
            // 
            this.groupBoxFindDirection2.Controls.Add(this.tableLayoutPanel6);
            this.groupBoxFindDirection2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxFindDirection2.Location = new System.Drawing.Point(331, 75);
            this.groupBoxFindDirection2.Name = "groupBoxFindDirection2";
            this.tableLayoutPanel4.SetRowSpan(this.groupBoxFindDirection2, 3);
            this.groupBoxFindDirection2.Size = new System.Drawing.Size(236, 102);
            this.groupBoxFindDirection2.TabIndex = 6;
            this.groupBoxFindDirection2.TabStop = false;
            this.groupBoxFindDirection2.Text = "Find directions";
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 1;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.Controls.Add(this.radioButtonSearchDown2, 0, 1);
            this.tableLayoutPanel6.Controls.Add(this.radioButtonSearchUp2, 0, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 2;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.Size = new System.Drawing.Size(230, 83);
            this.tableLayoutPanel6.TabIndex = 1;
            // 
            // radioButtonSearchDown2
            // 
            this.radioButtonSearchDown2.Checked = true;
            this.radioButtonSearchDown2.Location = new System.Drawing.Point(3, 27);
            this.radioButtonSearchDown2.Name = "radioButtonSearchDown2";
            this.radioButtonSearchDown2.Size = new System.Drawing.Size(106, 18);
            this.radioButtonSearchDown2.TabIndex = 8;
            this.radioButtonSearchDown2.Values.Text = "Find downwards";
            this.radioButtonSearchDown2.CheckedChanged += new System.EventHandler(this.radioButtonSearchDown2_CheckedChanged);
            // 
            // radioButtonSearchUp2
            // 
            this.radioButtonSearchUp2.Location = new System.Drawing.Point(3, 3);
            this.radioButtonSearchUp2.Name = "radioButtonSearchUp2";
            this.radioButtonSearchUp2.Size = new System.Drawing.Size(91, 18);
            this.radioButtonSearchUp2.TabIndex = 7;
            this.radioButtonSearchUp2.Values.Text = "Find upwards";
            this.radioButtonSearchUp2.CheckedChanged += new System.EventHandler(this.radioButtonSearchUp2_CheckedChanged);
            // 
            // buttonFindAll2
            // 
            this.buttonFindAll2.Location = new System.Drawing.Point(574, 40);
            this.buttonFindAll2.Margin = new System.Windows.Forms.Padding(4);
            this.buttonFindAll2.Name = "buttonFindAll2";
            this.buttonFindAll2.Size = new System.Drawing.Size(119, 28);
            this.buttonFindAll2.TabIndex = 11;
            this.buttonFindAll2.Values.Text = "Find All";
            this.buttonFindAll2.Click += new System.EventHandler(this.buttonFindAll2_Click);
            // 
            // FindOptionGroup2
            // 
            this.FindOptionGroup2.Controls.Add(this.tableLayoutPanel5);
            this.FindOptionGroup2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FindOptionGroup2.Location = new System.Drawing.Point(90, 76);
            this.FindOptionGroup2.Margin = new System.Windows.Forms.Padding(4);
            this.FindOptionGroup2.Name = "FindOptionGroup2";
            this.FindOptionGroup2.Padding = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel4.SetRowSpan(this.FindOptionGroup2, 3);
            this.FindOptionGroup2.Size = new System.Drawing.Size(234, 100);
            this.FindOptionGroup2.TabIndex = 3;
            this.FindOptionGroup2.TabStop = false;
            this.FindOptionGroup2.Text = "Find and replace options";
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.Controls.Add(this.MatchCellCheckBox2, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.MatchCaseCheckBox2, 0, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(4, 17);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.Size = new System.Drawing.Size(226, 79);
            this.tableLayoutPanel5.TabIndex = 0;
            // 
            // MatchCellCheckBox2
            // 
            this.MatchCellCheckBox2.Location = new System.Drawing.Point(4, 33);
            this.MatchCellCheckBox2.Margin = new System.Windows.Forms.Padding(4);
            this.MatchCellCheckBox2.Name = "MatchCellCheckBox2";
            this.MatchCellCheckBox2.Size = new System.Drawing.Size(133, 21);
            this.MatchCellCheckBox2.TabIndex = 5;
            this.MatchCellCheckBox2.Text = "Match whole cell";
            this.MatchCellCheckBox2.UseVisualStyleBackColor = true;
            this.MatchCellCheckBox2.CheckedChanged += new System.EventHandler(this.MatchCellCheckBox2_CheckedChanged);
            // 
            // MatchCaseCheckBox2
            // 
            this.MatchCaseCheckBox2.Location = new System.Drawing.Point(4, 4);
            this.MatchCaseCheckBox2.Margin = new System.Windows.Forms.Padding(4);
            this.MatchCaseCheckBox2.Name = "MatchCaseCheckBox2";
            this.MatchCaseCheckBox2.Size = new System.Drawing.Size(102, 21);
            this.MatchCaseCheckBox2.TabIndex = 4;
            this.MatchCaseCheckBox2.Text = "Match case";
            this.MatchCaseCheckBox2.UseVisualStyleBackColor = true;
            this.MatchCaseCheckBox2.CheckedChanged += new System.EventHandler(this.MatchCaseCheckBox2_CheckedChanged);
            // 
            // UseComboBox2
            // 
            this.UseComboBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel4.SetColumnSpan(this.UseComboBox2, 2);
            this.UseComboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.UseComboBox2.FormattingEnabled = true;
            this.UseComboBox2.Items.AddRange(new object[] {
            "Find text that match",
            "Find text that match using regular expressions",
            "Find text that match wildcards"});
            this.UseComboBox2.Location = new System.Drawing.Point(90, 184);
            this.UseComboBox2.Margin = new System.Windows.Forms.Padding(4);
            this.UseComboBox2.Name = "UseComboBox2";
            this.UseComboBox2.Size = new System.Drawing.Size(476, 21);
            this.UseComboBox2.TabIndex = 9;
            this.UseComboBox2.SelectedIndexChanged += new System.EventHandler(this.UseComboBox2_SelectedIndexChanged);
            // 
            // ReplaceAllButton
            // 
            this.ReplaceAllButton.Location = new System.Drawing.Point(574, 112);
            this.ReplaceAllButton.Margin = new System.Windows.Forms.Padding(4);
            this.ReplaceAllButton.Name = "ReplaceAllButton";
            this.ReplaceAllButton.Size = new System.Drawing.Size(119, 28);
            this.ReplaceAllButton.TabIndex = 13;
            this.ReplaceAllButton.Values.Text = "Replace All";
            this.ReplaceAllButton.Click += new System.EventHandler(this.ReplaceAllButton_Click);
            // 
            // ReplaceButton
            // 
            this.ReplaceButton.Location = new System.Drawing.Point(574, 76);
            this.ReplaceButton.Margin = new System.Windows.Forms.Padding(4);
            this.ReplaceButton.Name = "ReplaceButton";
            this.ReplaceButton.Size = new System.Drawing.Size(119, 28);
            this.ReplaceButton.TabIndex = 12;
            this.ReplaceButton.Values.Text = "Replace";
            this.ReplaceButton.Click += new System.EventHandler(this.ReplaceButton_Click);
            // 
            // FindButton2
            // 
            this.FindButton2.Location = new System.Drawing.Point(574, 4);
            this.FindButton2.Margin = new System.Windows.Forms.Padding(4);
            this.FindButton2.Name = "FindButton2";
            this.FindButton2.Size = new System.Drawing.Size(119, 28);
            this.FindButton2.TabIndex = 10;
            this.FindButton2.Values.Text = "Find Next";
            this.FindButton2.Click += new System.EventHandler(this.FindButton2_Click);
            // 
            // ReplaceWithTextBox
            // 
            this.ReplaceWithTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel4.SetColumnSpan(this.ReplaceWithTextBox, 2);
            this.ReplaceWithTextBox.Location = new System.Drawing.Point(90, 40);
            this.ReplaceWithTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.ReplaceWithTextBox.Name = "ReplaceWithTextBox";
            this.ReplaceWithTextBox.Size = new System.Drawing.Size(476, 20);
            this.ReplaceWithTextBox.TabIndex = 2;
            // 
            // ReplaceLabel
            // 
            this.ReplaceLabel.Location = new System.Drawing.Point(4, 36);
            this.ReplaceLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ReplaceLabel.Name = "ReplaceLabel";
            this.ReplaceLabel.Size = new System.Drawing.Size(78, 18);
            this.ReplaceLabel.TabIndex = 4;
            this.ReplaceLabel.Values.Text = "Replace with:";
            // 
            // FindWhatTextBox2
            // 
            this.FindWhatTextBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel4.SetColumnSpan(this.FindWhatTextBox2, 2);
            this.FindWhatTextBox2.Location = new System.Drawing.Point(90, 4);
            this.FindWhatTextBox2.Margin = new System.Windows.Forms.Padding(4);
            this.FindWhatTextBox2.Name = "FindWhatTextBox2";
            this.FindWhatTextBox2.Size = new System.Drawing.Size(476, 20);
            this.FindWhatTextBox2.TabIndex = 0;
            this.FindWhatTextBox2.TextChanged += new System.EventHandler(this.FindWhatTextBox2_TextChanged);
            // 
            // FindLabel2
            // 
            this.FindLabel2.Location = new System.Drawing.Point(4, 0);
            this.FindLabel2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.FindLabel2.Name = "FindLabel2";
            this.FindLabel2.Size = new System.Drawing.Size(62, 18);
            this.FindLabel2.TabIndex = 2;
            this.FindLabel2.Values.Text = "Find what:";
            // 
            // kryptonNavigatorFindAndReplace
            // 
            this.kryptonNavigatorFindAndReplace.AllowPageDrag = true;
            this.kryptonNavigatorFindAndReplace.AllowPageReorder = false;
            this.kryptonNavigatorFindAndReplace.Button.ButtonDisplayLogic = Krypton.Navigator.ButtonDisplayLogic.Context;
            this.kryptonNavigatorFindAndReplace.Button.CloseButtonAction = Krypton.Navigator.CloseButtonAction.None;
            this.kryptonNavigatorFindAndReplace.Button.CloseButtonDisplay = Krypton.Navigator.ButtonDisplay.Hide;
            this.kryptonNavigatorFindAndReplace.Button.ContextButtonAction = Krypton.Navigator.ContextButtonAction.SelectPage;
            this.kryptonNavigatorFindAndReplace.Button.ContextButtonDisplay = Krypton.Navigator.ButtonDisplay.Logic;
            this.kryptonNavigatorFindAndReplace.Button.ContextMenuMapImage = Krypton.Navigator.MapKryptonPageImage.Small;
            this.kryptonNavigatorFindAndReplace.Button.ContextMenuMapText = Krypton.Navigator.MapKryptonPageText.TextTitle;
            this.kryptonNavigatorFindAndReplace.Button.NextButtonAction = Krypton.Navigator.DirectionButtonAction.ModeAppropriateAction;
            this.kryptonNavigatorFindAndReplace.Button.NextButtonDisplay = Krypton.Navigator.ButtonDisplay.Logic;
            this.kryptonNavigatorFindAndReplace.Button.PreviousButtonAction = Krypton.Navigator.DirectionButtonAction.ModeAppropriateAction;
            this.kryptonNavigatorFindAndReplace.Button.PreviousButtonDisplay = Krypton.Navigator.ButtonDisplay.Logic;
            this.kryptonNavigatorFindAndReplace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonNavigatorFindAndReplace.Location = new System.Drawing.Point(0, 0);
            this.kryptonNavigatorFindAndReplace.Name = "kryptonNavigatorFindAndReplace";
            this.kryptonNavigatorFindAndReplace.NavigatorMode = Krypton.Navigator.NavigatorMode.BarTabGroup;
            this.kryptonNavigatorFindAndReplace.Pages.AddRange(new Krypton.Navigator.KryptonPage[] {
            this.kryptonPageFind,
            this.kryptonPageReplace});
            this.kryptonNavigatorFindAndReplace.SelectedIndex = 0;
            this.kryptonNavigatorFindAndReplace.Size = new System.Drawing.Size(699, 262);
            this.kryptonNavigatorFindAndReplace.TabIndex = 1;
            this.kryptonNavigatorFindAndReplace.Text = "Find";
            this.kryptonNavigatorFindAndReplace.TabIndexChanged += new System.EventHandler(this.kryptonNavigatorFindAndReplace_TabIndexChanged);
            // 
            // kryptonPageFind
            // 
            this.kryptonPageFind.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageFind.Controls.Add(this.tableLayoutPanel1);
            this.kryptonPageFind.Flags = 65534;
            this.kryptonPageFind.LastVisibleSet = true;
            this.kryptonPageFind.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageFind.Name = "kryptonPageFind";
            this.kryptonPageFind.Size = new System.Drawing.Size(697, 235);
            this.kryptonPageFind.Text = "Find";
            this.kryptonPageFind.ToolTipTitle = "Page ToolTip";
            this.kryptonPageFind.UniqueName = "3fd1874db2814f399d420d919b30463e";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.checkBoxSearchAlsoRowHeaders, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.FindLabel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxFindMode, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.FindWhatTextBox1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.FindOptionGroupBox1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.FindButton1, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonFindAll1, 3, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(697, 235);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // kryptonPageReplace
            // 
            this.kryptonPageReplace.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageReplace.Controls.Add(this.tableLayoutPanel4);
            this.kryptonPageReplace.Flags = 65534;
            this.kryptonPageReplace.LastVisibleSet = true;
            this.kryptonPageReplace.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageReplace.Name = "kryptonPageReplace";
            this.kryptonPageReplace.Size = new System.Drawing.Size(697, 235);
            this.kryptonPageReplace.Text = "Replace";
            this.kryptonPageReplace.ToolTipTitle = "Page ToolTip";
            this.kryptonPageReplace.UniqueName = "a68e71d3ea1c44efb837b0df6e0f0055";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel4.ColumnCount = 4;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.Controls.Add(this.buttonFindAll2, 3, 1);
            this.tableLayoutPanel4.Controls.Add(this.ReplaceAllButton, 3, 3);
            this.tableLayoutPanel4.Controls.Add(this.groupBoxFindDirection2, 2, 2);
            this.tableLayoutPanel4.Controls.Add(this.ReplaceButton, 3, 2);
            this.tableLayoutPanel4.Controls.Add(this.FindLabel2, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.ReplaceLabel, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.FindOptionGroup2, 1, 2);
            this.tableLayoutPanel4.Controls.Add(this.FindButton2, 3, 0);
            this.tableLayoutPanel4.Controls.Add(this.FindWhatTextBox2, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.ReplaceWithTextBox, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.UseComboBox2, 1, 5);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 6;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.Size = new System.Drawing.Size(697, 235);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // FindAndReplaceForm
            // 
            this.AcceptButton = this.FindButton1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(699, 262);
            this.Controls.Add(this.kryptonNavigatorFindAndReplace);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FindAndReplaceForm";
            this.Text = "Find and Replace";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FindAndReplaceForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.FindOptionGroupBox1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.groupBoxFindDirection2.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.FindOptionGroup2.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonNavigatorFindAndReplace)).EndInit();
            this.kryptonNavigatorFindAndReplace.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageFind)).EndInit();
            this.kryptonPageFind.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageReplace)).EndInit();
            this.kryptonPageReplace.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion
        private Krypton.Toolkit.KryptonTextBox FindWhatTextBox1;
        private Krypton.Toolkit.KryptonLabel FindLabel1;
        private System.Windows.Forms.GroupBox FindOptionGroup2;
        private System.Windows.Forms.CheckBox MatchCellCheckBox2;
        private System.Windows.Forms.CheckBox MatchCaseCheckBox2;
        private Krypton.Toolkit.KryptonTextBox ReplaceWithTextBox;
        private Krypton.Toolkit.KryptonLabel ReplaceLabel;
        private Krypton.Toolkit.KryptonTextBox FindWhatTextBox2;
        private Krypton.Toolkit.KryptonLabel FindLabel2;
        private System.Windows.Forms.ComboBox UseComboBox2;
        private System.Windows.Forms.GroupBox FindOptionGroupBox1;
        private System.Windows.Forms.ComboBox comboBoxFindMode;
        private System.Windows.Forms.CheckBox MatchCellCheckBox1;
        private System.Windows.Forms.CheckBox MatchCaseCheckBox1;
        private Krypton.Toolkit.KryptonButton FindButton1;
        private Krypton.Toolkit.KryptonButton FindButton2;
        private Krypton.Toolkit.KryptonButton ReplaceButton;
        private Krypton.Toolkit.KryptonButton ReplaceAllButton;
        private Krypton.Toolkit.KryptonButton buttonFindAll1;
        private Krypton.Toolkit.KryptonButton buttonFindAll2;
        private Krypton.Toolkit.KryptonRadioButton radioButtonSearchUp2;
        private Krypton.Toolkit.KryptonRadioButton radioButtonSearchDown2;
        private Krypton.Toolkit.KryptonRadioButton radioButtonSearchUp1;
        private Krypton.Toolkit.KryptonRadioButton radioButtonSearchDown1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBoxFindDirection2;
        private System.Windows.Forms.CheckBox checkBoxSearchAlsoRowHeaders;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private Krypton.Navigator.KryptonNavigator kryptonNavigatorFindAndReplace;
        private Krypton.Navigator.KryptonPage kryptonPageFind;
        private Krypton.Navigator.KryptonPage kryptonPageReplace;
    }
}