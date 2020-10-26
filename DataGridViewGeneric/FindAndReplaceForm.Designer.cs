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
            this.components = new System.ComponentModel.Container();
            this.tabControlFindAndReplace = new System.Windows.Forms.TabControl();
            this.FindPage = new System.Windows.Forms.TabPage();
            this.checkBoxSearchAlsoRowHeaders = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonSearchUp1 = new System.Windows.Forms.RadioButton();
            this.radioButtonSearchDown1 = new System.Windows.Forms.RadioButton();
            this.buttonFindAll1 = new System.Windows.Forms.Button();
            this.FindButton1 = new System.Windows.Forms.Button();
            this.comboBoxFindMode = new System.Windows.Forms.ComboBox();
            this.FindOptionGroupBox1 = new System.Windows.Forms.GroupBox();
            this.MatchCellCheckBox1 = new System.Windows.Forms.CheckBox();
            this.MatchCaseCheckBox1 = new System.Windows.Forms.CheckBox();
            this.FindWhatTextBox1 = new System.Windows.Forms.TextBox();
            this.FindLabel1 = new System.Windows.Forms.Label();
            this.ReplacePage = new System.Windows.Forms.TabPage();
            this.groupBoxFindDirection2 = new System.Windows.Forms.GroupBox();
            this.radioButtonSearchUp2 = new System.Windows.Forms.RadioButton();
            this.radioButtonSearchDown2 = new System.Windows.Forms.RadioButton();
            this.buttonFindAll2 = new System.Windows.Forms.Button();
            this.FindOptionGroup2 = new System.Windows.Forms.GroupBox();
            this.MatchCellCheckBox2 = new System.Windows.Forms.CheckBox();
            this.MatchCaseCheckBox2 = new System.Windows.Forms.CheckBox();
            this.UseComboBox2 = new System.Windows.Forms.ComboBox();
            this.ReplaceAllButton = new System.Windows.Forms.Button();
            this.ReplaceButton = new System.Windows.Forms.Button();
            this.FindButton2 = new System.Windows.Forms.Button();
            this.ReplaceWithTextBox = new System.Windows.Forms.TextBox();
            this.ReplaceLabel = new System.Windows.Forms.Label();
            this.FindWhatTextBox2 = new System.Windows.Forms.TextBox();
            this.FindLabel2 = new System.Windows.Forms.Label();
            this.contextMenuStripDataGridViewGeneric = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toggleShowFavouriteRowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleHideEqualRowsValuesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMapSave = new System.Windows.Forms.ToolStripMenuItem();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.markAsFavoriteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeAsFavoriteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleRowsAsFavouriteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControlFindAndReplace.SuspendLayout();
            this.FindPage.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.FindOptionGroupBox1.SuspendLayout();
            this.ReplacePage.SuspendLayout();
            this.groupBoxFindDirection2.SuspendLayout();
            this.FindOptionGroup2.SuspendLayout();
            this.contextMenuStripDataGridViewGeneric.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlFindAndReplace
            // 
            this.tabControlFindAndReplace.Controls.Add(this.FindPage);
            this.tabControlFindAndReplace.Controls.Add(this.ReplacePage);
            this.tabControlFindAndReplace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlFindAndReplace.Location = new System.Drawing.Point(0, 0);
            this.tabControlFindAndReplace.Margin = new System.Windows.Forms.Padding(4);
            this.tabControlFindAndReplace.Name = "tabControlFindAndReplace";
            this.tabControlFindAndReplace.SelectedIndex = 0;
            this.tabControlFindAndReplace.Size = new System.Drawing.Size(699, 267);
            this.tabControlFindAndReplace.TabIndex = 0;
            // 
            // FindPage
            // 
            this.FindPage.BackColor = System.Drawing.Color.White;
            this.FindPage.Controls.Add(this.checkBoxSearchAlsoRowHeaders);
            this.FindPage.Controls.Add(this.groupBox1);
            this.FindPage.Controls.Add(this.buttonFindAll1);
            this.FindPage.Controls.Add(this.FindButton1);
            this.FindPage.Controls.Add(this.comboBoxFindMode);
            this.FindPage.Controls.Add(this.FindOptionGroupBox1);
            this.FindPage.Controls.Add(this.FindWhatTextBox1);
            this.FindPage.Controls.Add(this.FindLabel1);
            this.FindPage.Location = new System.Drawing.Point(4, 25);
            this.FindPage.Margin = new System.Windows.Forms.Padding(4);
            this.FindPage.Name = "FindPage";
            this.FindPage.Padding = new System.Windows.Forms.Padding(4);
            this.FindPage.Size = new System.Drawing.Size(691, 238);
            this.FindPage.TabIndex = 0;
            this.FindPage.Text = "Find";
            // 
            // checkBoxSearchAlsoRowHeaders
            // 
            this.checkBoxSearchAlsoRowHeaders.AutoSize = true;
            this.checkBoxSearchAlsoRowHeaders.Location = new System.Drawing.Point(111, 211);
            this.checkBoxSearchAlsoRowHeaders.Name = "checkBoxSearchAlsoRowHeaders";
            this.checkBoxSearchAlsoRowHeaders.Size = new System.Drawing.Size(209, 21);
            this.checkBoxSearchAlsoRowHeaders.TabIndex = 11;
            this.checkBoxSearchAlsoRowHeaders.Text = "Search also in Row Headers";
            this.checkBoxSearchAlsoRowHeaders.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonSearchUp1);
            this.groupBox1.Controls.Add(this.radioButtonSearchDown1);
            this.groupBox1.Location = new System.Drawing.Point(334, 69);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(216, 100);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Find directions";
            // 
            // radioButtonSearchUp1
            // 
            this.radioButtonSearchUp1.AutoSize = true;
            this.radioButtonSearchUp1.Location = new System.Drawing.Point(11, 27);
            this.radioButtonSearchUp1.Name = "radioButtonSearchUp1";
            this.radioButtonSearchUp1.Size = new System.Drawing.Size(113, 21);
            this.radioButtonSearchUp1.TabIndex = 6;
            this.radioButtonSearchUp1.Text = "Find upwards";
            this.radioButtonSearchUp1.UseVisualStyleBackColor = true;
            this.radioButtonSearchUp1.CheckedChanged += new System.EventHandler(this.radioButtonSearchUp1_CheckedChanged);
            // 
            // radioButtonSearchDown1
            // 
            this.radioButtonSearchDown1.AutoSize = true;
            this.radioButtonSearchDown1.Checked = true;
            this.radioButtonSearchDown1.Location = new System.Drawing.Point(11, 54);
            this.radioButtonSearchDown1.Name = "radioButtonSearchDown1";
            this.radioButtonSearchDown1.Size = new System.Drawing.Size(130, 21);
            this.radioButtonSearchDown1.TabIndex = 7;
            this.radioButtonSearchDown1.TabStop = true;
            this.radioButtonSearchDown1.Text = "Find downwards";
            this.radioButtonSearchDown1.UseVisualStyleBackColor = true;
            this.radioButtonSearchDown1.CheckedChanged += new System.EventHandler(this.radioButtonSearchDown1_CheckedChanged);
            // 
            // buttonFindAll1
            // 
            this.buttonFindAll1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFindAll1.Location = new System.Drawing.Point(557, 105);
            this.buttonFindAll1.Margin = new System.Windows.Forms.Padding(4);
            this.buttonFindAll1.Name = "buttonFindAll1";
            this.buttonFindAll1.Size = new System.Drawing.Size(119, 28);
            this.buttonFindAll1.TabIndex = 10;
            this.buttonFindAll1.Text = "Find All";
            this.buttonFindAll1.UseVisualStyleBackColor = true;
            this.buttonFindAll1.Click += new System.EventHandler(this.buttonFindAll1_Click);
            // 
            // FindButton1
            // 
            this.FindButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FindButton1.Location = new System.Drawing.Point(557, 69);
            this.FindButton1.Margin = new System.Windows.Forms.Padding(4);
            this.FindButton1.Name = "FindButton1";
            this.FindButton1.Size = new System.Drawing.Size(119, 28);
            this.FindButton1.TabIndex = 9;
            this.FindButton1.Text = "Find Next";
            this.FindButton1.UseVisualStyleBackColor = true;
            this.FindButton1.Click += new System.EventHandler(this.FindButton1_Click);
            // 
            // comboBoxFindMode
            // 
            this.comboBoxFindMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxFindMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFindMode.FormattingEnabled = true;
            this.comboBoxFindMode.Items.AddRange(new object[] {
            "Find text that match",
            "Find text that match using regular expressions",
            "Find text that match wildcards"});
            this.comboBoxFindMode.Location = new System.Drawing.Point(111, 180);
            this.comboBoxFindMode.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxFindMode.Name = "comboBoxFindMode";
            this.comboBoxFindMode.Size = new System.Drawing.Size(438, 24);
            this.comboBoxFindMode.TabIndex = 8;
            this.comboBoxFindMode.SelectedIndexChanged += new System.EventHandler(this.UseComboBox1_SelectedIndexChanged);
            // 
            // FindOptionGroupBox1
            // 
            this.FindOptionGroupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FindOptionGroupBox1.Controls.Add(this.MatchCellCheckBox1);
            this.FindOptionGroupBox1.Controls.Add(this.MatchCaseCheckBox1);
            this.FindOptionGroupBox1.Location = new System.Drawing.Point(111, 69);
            this.FindOptionGroupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.FindOptionGroupBox1.Name = "FindOptionGroupBox1";
            this.FindOptionGroupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.FindOptionGroupBox1.Size = new System.Drawing.Size(216, 100);
            this.FindOptionGroupBox1.TabIndex = 2;
            this.FindOptionGroupBox1.TabStop = false;
            this.FindOptionGroupBox1.Text = "Find options";
            // 
            // MatchCellCheckBox1
            // 
            this.MatchCellCheckBox1.AutoSize = true;
            this.MatchCellCheckBox1.Location = new System.Drawing.Point(11, 54);
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
            this.MatchCaseCheckBox1.AutoSize = true;
            this.MatchCaseCheckBox1.Location = new System.Drawing.Point(11, 27);
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
            this.FindWhatTextBox1.Location = new System.Drawing.Point(111, 7);
            this.FindWhatTextBox1.Margin = new System.Windows.Forms.Padding(4);
            this.FindWhatTextBox1.Name = "FindWhatTextBox1";
            this.FindWhatTextBox1.Size = new System.Drawing.Size(565, 22);
            this.FindWhatTextBox1.TabIndex = 0;
            this.FindWhatTextBox1.TextChanged += new System.EventHandler(this.FindWhatTextBox1_TextChanged);
            // 
            // FindLabel1
            // 
            this.FindLabel1.AutoSize = true;
            this.FindLabel1.Location = new System.Drawing.Point(3, 12);
            this.FindLabel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.FindLabel1.Name = "FindLabel1";
            this.FindLabel1.Size = new System.Drawing.Size(72, 17);
            this.FindLabel1.TabIndex = 4;
            this.FindLabel1.Text = "Find what:";
            // 
            // ReplacePage
            // 
            this.ReplacePage.Controls.Add(this.groupBoxFindDirection2);
            this.ReplacePage.Controls.Add(this.buttonFindAll2);
            this.ReplacePage.Controls.Add(this.FindOptionGroup2);
            this.ReplacePage.Controls.Add(this.UseComboBox2);
            this.ReplacePage.Controls.Add(this.ReplaceAllButton);
            this.ReplacePage.Controls.Add(this.ReplaceButton);
            this.ReplacePage.Controls.Add(this.FindButton2);
            this.ReplacePage.Controls.Add(this.ReplaceWithTextBox);
            this.ReplacePage.Controls.Add(this.ReplaceLabel);
            this.ReplacePage.Controls.Add(this.FindWhatTextBox2);
            this.ReplacePage.Controls.Add(this.FindLabel2);
            this.ReplacePage.Location = new System.Drawing.Point(4, 25);
            this.ReplacePage.Margin = new System.Windows.Forms.Padding(4);
            this.ReplacePage.Name = "ReplacePage";
            this.ReplacePage.Padding = new System.Windows.Forms.Padding(4);
            this.ReplacePage.Size = new System.Drawing.Size(691, 238);
            this.ReplacePage.TabIndex = 1;
            this.ReplacePage.Text = "Replace";
            this.ReplacePage.UseVisualStyleBackColor = true;
            // 
            // groupBoxFindDirection2
            // 
            this.groupBoxFindDirection2.Controls.Add(this.radioButtonSearchUp2);
            this.groupBoxFindDirection2.Controls.Add(this.radioButtonSearchDown2);
            this.groupBoxFindDirection2.Location = new System.Drawing.Point(334, 69);
            this.groupBoxFindDirection2.Name = "groupBoxFindDirection2";
            this.groupBoxFindDirection2.Size = new System.Drawing.Size(216, 100);
            this.groupBoxFindDirection2.TabIndex = 6;
            this.groupBoxFindDirection2.TabStop = false;
            this.groupBoxFindDirection2.Text = "Find directions";
            // 
            // radioButtonSearchUp2
            // 
            this.radioButtonSearchUp2.AutoSize = true;
            this.radioButtonSearchUp2.Location = new System.Drawing.Point(11, 27);
            this.radioButtonSearchUp2.Name = "radioButtonSearchUp2";
            this.radioButtonSearchUp2.Size = new System.Drawing.Size(113, 21);
            this.radioButtonSearchUp2.TabIndex = 7;
            this.radioButtonSearchUp2.TabStop = true;
            this.radioButtonSearchUp2.Text = "Find upwards";
            this.radioButtonSearchUp2.UseVisualStyleBackColor = true;
            this.radioButtonSearchUp2.CheckedChanged += new System.EventHandler(this.radioButtonSearchUp2_CheckedChanged);
            // 
            // radioButtonSearchDown2
            // 
            this.radioButtonSearchDown2.AutoSize = true;
            this.radioButtonSearchDown2.Checked = true;
            this.radioButtonSearchDown2.Location = new System.Drawing.Point(11, 54);
            this.radioButtonSearchDown2.Name = "radioButtonSearchDown2";
            this.radioButtonSearchDown2.Size = new System.Drawing.Size(130, 21);
            this.radioButtonSearchDown2.TabIndex = 8;
            this.radioButtonSearchDown2.TabStop = true;
            this.radioButtonSearchDown2.Text = "Find downwards";
            this.radioButtonSearchDown2.UseVisualStyleBackColor = true;
            this.radioButtonSearchDown2.CheckedChanged += new System.EventHandler(this.radioButtonSearchDown2_CheckedChanged);
            // 
            // buttonFindAll2
            // 
            this.buttonFindAll2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFindAll2.Location = new System.Drawing.Point(557, 105);
            this.buttonFindAll2.Margin = new System.Windows.Forms.Padding(4);
            this.buttonFindAll2.Name = "buttonFindAll2";
            this.buttonFindAll2.Size = new System.Drawing.Size(119, 28);
            this.buttonFindAll2.TabIndex = 11;
            this.buttonFindAll2.Text = "Find All";
            this.buttonFindAll2.UseVisualStyleBackColor = true;
            this.buttonFindAll2.Click += new System.EventHandler(this.buttonFindAll2_Click);
            // 
            // FindOptionGroup2
            // 
            this.FindOptionGroup2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FindOptionGroup2.Controls.Add(this.MatchCellCheckBox2);
            this.FindOptionGroup2.Controls.Add(this.MatchCaseCheckBox2);
            this.FindOptionGroup2.Location = new System.Drawing.Point(111, 69);
            this.FindOptionGroup2.Margin = new System.Windows.Forms.Padding(4);
            this.FindOptionGroup2.Name = "FindOptionGroup2";
            this.FindOptionGroup2.Padding = new System.Windows.Forms.Padding(4);
            this.FindOptionGroup2.Size = new System.Drawing.Size(216, 100);
            this.FindOptionGroup2.TabIndex = 3;
            this.FindOptionGroup2.TabStop = false;
            this.FindOptionGroup2.Text = "Find and replace options";
            // 
            // MatchCellCheckBox2
            // 
            this.MatchCellCheckBox2.AutoSize = true;
            this.MatchCellCheckBox2.Location = new System.Drawing.Point(11, 54);
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
            this.MatchCaseCheckBox2.AutoSize = true;
            this.MatchCaseCheckBox2.Location = new System.Drawing.Point(11, 27);
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
            this.UseComboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.UseComboBox2.FormattingEnabled = true;
            this.UseComboBox2.Items.AddRange(new object[] {
            "Find text that match",
            "Find text that match using regular expressions",
            "Find text that match wildcards"});
            this.UseComboBox2.Location = new System.Drawing.Point(111, 180);
            this.UseComboBox2.Margin = new System.Windows.Forms.Padding(4);
            this.UseComboBox2.Name = "UseComboBox2";
            this.UseComboBox2.Size = new System.Drawing.Size(438, 24);
            this.UseComboBox2.TabIndex = 9;
            this.UseComboBox2.SelectedIndexChanged += new System.EventHandler(this.UseComboBox2_SelectedIndexChanged);
            // 
            // ReplaceAllButton
            // 
            this.ReplaceAllButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ReplaceAllButton.Location = new System.Drawing.Point(557, 177);
            this.ReplaceAllButton.Margin = new System.Windows.Forms.Padding(4);
            this.ReplaceAllButton.Name = "ReplaceAllButton";
            this.ReplaceAllButton.Size = new System.Drawing.Size(119, 28);
            this.ReplaceAllButton.TabIndex = 13;
            this.ReplaceAllButton.Text = "Replace All";
            this.ReplaceAllButton.UseVisualStyleBackColor = true;
            this.ReplaceAllButton.Click += new System.EventHandler(this.ReplaceAllButton_Click);
            // 
            // ReplaceButton
            // 
            this.ReplaceButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ReplaceButton.Location = new System.Drawing.Point(557, 141);
            this.ReplaceButton.Margin = new System.Windows.Forms.Padding(4);
            this.ReplaceButton.Name = "ReplaceButton";
            this.ReplaceButton.Size = new System.Drawing.Size(119, 28);
            this.ReplaceButton.TabIndex = 12;
            this.ReplaceButton.Text = "Replace";
            this.ReplaceButton.UseVisualStyleBackColor = true;
            this.ReplaceButton.Click += new System.EventHandler(this.ReplaceButton_Click);
            // 
            // FindButton2
            // 
            this.FindButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FindButton2.Location = new System.Drawing.Point(557, 69);
            this.FindButton2.Margin = new System.Windows.Forms.Padding(4);
            this.FindButton2.Name = "FindButton2";
            this.FindButton2.Size = new System.Drawing.Size(119, 28);
            this.FindButton2.TabIndex = 10;
            this.FindButton2.Text = "Find Next";
            this.FindButton2.UseVisualStyleBackColor = true;
            this.FindButton2.Click += new System.EventHandler(this.FindButton2_Click);
            // 
            // ReplaceWithTextBox
            // 
            this.ReplaceWithTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ReplaceWithTextBox.Location = new System.Drawing.Point(111, 39);
            this.ReplaceWithTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.ReplaceWithTextBox.Name = "ReplaceWithTextBox";
            this.ReplaceWithTextBox.Size = new System.Drawing.Size(565, 22);
            this.ReplaceWithTextBox.TabIndex = 2;
            // 
            // ReplaceLabel
            // 
            this.ReplaceLabel.AutoSize = true;
            this.ReplaceLabel.Location = new System.Drawing.Point(3, 44);
            this.ReplaceLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ReplaceLabel.Name = "ReplaceLabel";
            this.ReplaceLabel.Size = new System.Drawing.Size(92, 17);
            this.ReplaceLabel.TabIndex = 4;
            this.ReplaceLabel.Text = "Replace with:";
            // 
            // FindWhatTextBox2
            // 
            this.FindWhatTextBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FindWhatTextBox2.Location = new System.Drawing.Point(111, 7);
            this.FindWhatTextBox2.Margin = new System.Windows.Forms.Padding(4);
            this.FindWhatTextBox2.Name = "FindWhatTextBox2";
            this.FindWhatTextBox2.Size = new System.Drawing.Size(565, 22);
            this.FindWhatTextBox2.TabIndex = 0;
            this.FindWhatTextBox2.TextChanged += new System.EventHandler(this.FindWhatTextBox2_TextChanged);
            // 
            // FindLabel2
            // 
            this.FindLabel2.AutoSize = true;
            this.FindLabel2.Location = new System.Drawing.Point(3, 12);
            this.FindLabel2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.FindLabel2.Name = "FindLabel2";
            this.FindLabel2.Size = new System.Drawing.Size(72, 17);
            this.FindLabel2.TabIndex = 2;
            this.FindLabel2.Text = "Find what:";
            // 
            // contextMenuStripDataGridViewGeneric
            // 
            this.contextMenuStripDataGridViewGeneric.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripDataGridViewGeneric.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.findToolStripMenuItem,
            this.replaceToolStripMenuItem,
            this.toolStripMenuItemMapSave,
            this.markAsFavoriteToolStripMenuItem,
            this.removeAsFavoriteToolStripMenuItem,
            this.toggleRowsAsFavouriteToolStripMenuItem,
            this.toggleShowFavouriteRowsToolStripMenuItem,
            this.toggleHideEqualRowsValuesToolStripMenuItem});
            this.contextMenuStripDataGridViewGeneric.Name = "contextMenuStripMap";
            this.contextMenuStripDataGridViewGeneric.Size = new System.Drawing.Size(241, 396);
            // 
            // toggleShowFavouriteRowsToolStripMenuItem
            // 
            this.toggleShowFavouriteRowsToolStripMenuItem.Checked = true;
            this.toggleShowFavouriteRowsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.toggleShowFavouriteRowsToolStripMenuItem.Name = "toggleShowFavouriteRowsToolStripMenuItem";
            this.toggleShowFavouriteRowsToolStripMenuItem.Size = new System.Drawing.Size(240, 26);
            this.toggleShowFavouriteRowsToolStripMenuItem.Text = "Show only favorite rows";
            this.toggleShowFavouriteRowsToolStripMenuItem.Click += new System.EventHandler(this.toggleShowFavouriteRowsToolStripMenuItem_Click);
            // 
            // toggleHideEqualRowsValuesToolStripMenuItem
            // 
            this.toggleHideEqualRowsValuesToolStripMenuItem.Checked = true;
            this.toggleHideEqualRowsValuesToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.toggleHideEqualRowsValuesToolStripMenuItem.Name = "toggleHideEqualRowsValuesToolStripMenuItem";
            this.toggleHideEqualRowsValuesToolStripMenuItem.Size = new System.Drawing.Size(240, 26);
            this.toggleHideEqualRowsValuesToolStripMenuItem.Text = "Hide equal rows";
            this.toggleHideEqualRowsValuesToolStripMenuItem.Click += new System.EventHandler(this.toggleHideEqualRowsValuesToolStripMenuItem_Click);
            // 
            // toolStripMenuItemMapSave
            // 
            this.toolStripMenuItemMapSave.Image = global::DataGridViewGeneric.Properties.Resources.Save;
            this.toolStripMenuItemMapSave.Name = "toolStripMenuItemMapSave";
            this.toolStripMenuItemMapSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.toolStripMenuItemMapSave.Size = new System.Drawing.Size(240, 26);
            this.toolStripMenuItemMapSave.Text = "Save";
            this.toolStripMenuItemMapSave.Click += new System.EventHandler(this.toolStripMenuItemMapSave_Click);
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Image = global::DataGridViewGeneric.Properties.Resources.Cut;
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(240, 26);
            this.cutToolStripMenuItem.Text = "Cut";
            this.cutToolStripMenuItem.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Image = global::DataGridViewGeneric.Properties.Resources.Copy;
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(240, 26);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Image = global::DataGridViewGeneric.Properties.Resources.Paste;
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(240, 26);
            this.pasteToolStripMenuItem.Text = "Paste";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Image = global::DataGridViewGeneric.Properties.Resources.Delete;
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(240, 26);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Image = global::DataGridViewGeneric.Properties.Resources.Undo;
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(240, 26);
            this.undoToolStripMenuItem.Text = "Undo";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Image = global::DataGridViewGeneric.Properties.Resources.Redo;
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(240, 26);
            this.redoToolStripMenuItem.Text = "Redo";
            this.redoToolStripMenuItem.Click += new System.EventHandler(this.redoToolStripMenuItem_Click);
            // 
            // findToolStripMenuItem
            // 
            this.findToolStripMenuItem.Image = global::DataGridViewGeneric.Properties.Resources.Find;
            this.findToolStripMenuItem.Name = "findToolStripMenuItem";
            this.findToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.findToolStripMenuItem.Size = new System.Drawing.Size(240, 26);
            this.findToolStripMenuItem.Text = "Find";
            this.findToolStripMenuItem.Click += new System.EventHandler(this.findToolStripMenuItem_Click);
            // 
            // replaceToolStripMenuItem
            // 
            this.replaceToolStripMenuItem.Image = global::DataGridViewGeneric.Properties.Resources.Replace;
            this.replaceToolStripMenuItem.Name = "replaceToolStripMenuItem";
            this.replaceToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.replaceToolStripMenuItem.Size = new System.Drawing.Size(240, 26);
            this.replaceToolStripMenuItem.Text = "Replace";
            this.replaceToolStripMenuItem.Click += new System.EventHandler(this.replaceToolStripMenuItem_Click);
            // 
            // markAsFavoriteToolStripMenuItem
            // 
            this.markAsFavoriteToolStripMenuItem.Image = global::DataGridViewGeneric.Properties.Resources.FavoriteSelect;
            this.markAsFavoriteToolStripMenuItem.Name = "markAsFavoriteToolStripMenuItem";
            this.markAsFavoriteToolStripMenuItem.Size = new System.Drawing.Size(240, 26);
            this.markAsFavoriteToolStripMenuItem.Text = "Mark as favorite";
            this.markAsFavoriteToolStripMenuItem.Click += new System.EventHandler(this.markAsFavoriteToolStripMenuItem_Click);
            // 
            // removeAsFavoriteToolStripMenuItem
            // 
            this.removeAsFavoriteToolStripMenuItem.Image = global::DataGridViewGeneric.Properties.Resources.FavoriteRemove;
            this.removeAsFavoriteToolStripMenuItem.Name = "removeAsFavoriteToolStripMenuItem";
            this.removeAsFavoriteToolStripMenuItem.Size = new System.Drawing.Size(240, 26);
            this.removeAsFavoriteToolStripMenuItem.Text = "Remove as favorite";
            this.removeAsFavoriteToolStripMenuItem.Click += new System.EventHandler(this.removeAsFavoriteToolStripMenuItem_Click);
            // 
            // toggleRowsAsFavouriteToolStripMenuItem
            // 
            this.toggleRowsAsFavouriteToolStripMenuItem.Image = global::DataGridViewGeneric.Properties.Resources.FavoriteToggle;
            this.toggleRowsAsFavouriteToolStripMenuItem.Name = "toggleRowsAsFavouriteToolStripMenuItem";
            this.toggleRowsAsFavouriteToolStripMenuItem.Size = new System.Drawing.Size(240, 26);
            this.toggleRowsAsFavouriteToolStripMenuItem.Text = "Toggle favorite";
            this.toggleRowsAsFavouriteToolStripMenuItem.Click += new System.EventHandler(this.toggleRowsAsFavouriteToolStripMenuItem_Click);
            // 
            // FindAndReplaceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(699, 267);
            this.Controls.Add(this.tabControlFindAndReplace);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FindAndReplaceForm";
            this.Text = "Find and Replace";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FindAndReplaceForm_FormClosing);
            this.tabControlFindAndReplace.ResumeLayout(false);
            this.FindPage.ResumeLayout(false);
            this.FindPage.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.FindOptionGroupBox1.ResumeLayout(false);
            this.FindOptionGroupBox1.PerformLayout();
            this.ReplacePage.ResumeLayout(false);
            this.ReplacePage.PerformLayout();
            this.groupBoxFindDirection2.ResumeLayout(false);
            this.groupBoxFindDirection2.PerformLayout();
            this.FindOptionGroup2.ResumeLayout(false);
            this.FindOptionGroup2.PerformLayout();
            this.contextMenuStripDataGridViewGeneric.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.TabControl tabControlFindAndReplace;
        private System.Windows.Forms.TabPage FindPage;
        private System.Windows.Forms.TabPage ReplacePage;
        private System.Windows.Forms.TextBox FindWhatTextBox1;
        private System.Windows.Forms.Label FindLabel1;
        private System.Windows.Forms.GroupBox FindOptionGroup2;
        private System.Windows.Forms.CheckBox MatchCellCheckBox2;
        private System.Windows.Forms.CheckBox MatchCaseCheckBox2;
        private System.Windows.Forms.TextBox ReplaceWithTextBox;
        private System.Windows.Forms.Label ReplaceLabel;
        private System.Windows.Forms.TextBox FindWhatTextBox2;
        private System.Windows.Forms.Label FindLabel2;
        private System.Windows.Forms.ComboBox UseComboBox2;
        private System.Windows.Forms.GroupBox FindOptionGroupBox1;
        private System.Windows.Forms.ComboBox comboBoxFindMode;
        private System.Windows.Forms.CheckBox MatchCellCheckBox1;
        private System.Windows.Forms.CheckBox MatchCaseCheckBox1;
        private System.Windows.Forms.Button FindButton1;
        private System.Windows.Forms.Button FindButton2;
        private System.Windows.Forms.Button ReplaceButton;
        private System.Windows.Forms.Button ReplaceAllButton;
        private System.Windows.Forms.Button buttonFindAll1;
        private System.Windows.Forms.Button buttonFindAll2;
        private System.Windows.Forms.RadioButton radioButtonSearchUp2;
        private System.Windows.Forms.RadioButton radioButtonSearchDown2;
        private System.Windows.Forms.RadioButton radioButtonSearchUp1;
        private System.Windows.Forms.RadioButton radioButtonSearchDown1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBoxFindDirection2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripDataGridViewGeneric;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem replaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toggleRowsAsFavouriteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toggleShowFavouriteRowsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toggleHideEqualRowsValuesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem markAsFavoriteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeAsFavoriteToolStripMenuItem;
        private System.Windows.Forms.CheckBox checkBoxSearchAlsoRowHeaders;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapSave;
    }
}