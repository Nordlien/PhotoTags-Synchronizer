namespace PhotoTagsSynchronizer
{
    partial class Config
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Config));
            this.tabControlConfig = new System.Windows.Forms.TabControl();
            this.tabPageFileDates = new System.Windows.Forms.TabPage();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.dataGridViewMetadataReadPriority = new System.Windows.Forms.DataGridView();
            this.contextMenuStripMetadataRead = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemMetadataReadMove = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMetadataReadCut = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMetadataReadCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMetadataReadPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMetadataReadDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMetadataReadUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMetadataReadRedo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMetadataReadFind = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMetadataReadReplace = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMetadataReadMarkFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMetadataReadRemoveFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMetadataReadToggleFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMetadataReadShowFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBoxConfigFilenameDateFormats = new System.Windows.Forms.TextBox();
            this.buttonConfigSave = new System.Windows.Forms.Button();
            this.buttonConfigCancel = new System.Windows.Forms.Button();
            this.tabControlConfig.SuspendLayout();
            this.tabPageFileDates.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMetadataReadPriority)).BeginInit();
            this.contextMenuStripMetadataRead.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlConfig
            // 
            this.tabControlConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlConfig.Controls.Add(this.tabPageFileDates);
            this.tabControlConfig.Controls.Add(this.tabPage2);
            this.tabControlConfig.Controls.Add(this.tabPage3);
            this.tabControlConfig.Location = new System.Drawing.Point(0, 0);
            this.tabControlConfig.Name = "tabControlConfig";
            this.tabControlConfig.SelectedIndex = 0;
            this.tabControlConfig.Size = new System.Drawing.Size(800, 411);
            this.tabControlConfig.TabIndex = 0;
            this.tabControlConfig.SelectedIndexChanged += new System.EventHandler(this.tabControlConfig_SelectedIndexChanged);
            // 
            // tabPageFileDates
            // 
            this.tabPageFileDates.Controls.Add(this.textBox1);
            this.tabPageFileDates.Controls.Add(this.dataGridViewMetadataReadPriority);
            this.tabPageFileDates.Location = new System.Drawing.Point(4, 25);
            this.tabPageFileDates.Name = "tabPageFileDates";
            this.tabPageFileDates.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFileDates.Size = new System.Drawing.Size(792, 382);
            this.tabPageFileDates.TabIndex = 0;
            this.tabPageFileDates.Text = "Metadata Read";
            this.tabPageFileDates.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(3, 6);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(786, 43);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // dataGridViewMetadataReadPriority
            // 
            this.dataGridViewMetadataReadPriority.AllowDrop = true;
            this.dataGridViewMetadataReadPriority.AllowUserToAddRows = false;
            this.dataGridViewMetadataReadPriority.AllowUserToDeleteRows = false;
            this.dataGridViewMetadataReadPriority.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewMetadataReadPriority.ColumnHeadersHeight = 29;
            this.dataGridViewMetadataReadPriority.ContextMenuStrip = this.contextMenuStripMetadataRead;
            this.dataGridViewMetadataReadPriority.Location = new System.Drawing.Point(3, 55);
            this.dataGridViewMetadataReadPriority.Name = "dataGridViewMetadataReadPriority";
            this.dataGridViewMetadataReadPriority.RowHeadersWidth = 51;
            this.dataGridViewMetadataReadPriority.RowTemplate.Height = 24;
            this.dataGridViewMetadataReadPriority.Size = new System.Drawing.Size(786, 324);
            this.dataGridViewMetadataReadPriority.TabIndex = 0;
            this.dataGridViewMetadataReadPriority.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewMetadataReadPriority_CellBeginEdit);
            this.dataGridViewMetadataReadPriority.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridViewMetadataReadPriority_CellPainting);
            this.dataGridViewMetadataReadPriority.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewMetadataReadPriority_CellValueChanged);
            this.dataGridViewMetadataReadPriority.DragDrop += new System.Windows.Forms.DragEventHandler(this.dataGridViewMetadataReadPriority_DragDrop);
            this.dataGridViewMetadataReadPriority.DragOver += new System.Windows.Forms.DragEventHandler(this.dataGridViewMetadataReadPriority_DragOver);
            this.dataGridViewMetadataReadPriority.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridViewMetadataReadPriority_KeyDown);
            this.dataGridViewMetadataReadPriority.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridViewMetadataReadPriority_MouseDown);
            this.dataGridViewMetadataReadPriority.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dataGridViewMetadataReadPriority_MouseMove);
            // 
            // contextMenuStripMetadataRead
            // 
            this.contextMenuStripMetadataRead.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripMetadataRead.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemMetadataReadMove,
            this.toolStripMenuItemMetadataReadCut,
            this.toolStripMenuItemMetadataReadCopy,
            this.toolStripMenuItemMetadataReadPaste,
            this.toolStripMenuItemMetadataReadDelete,
            this.toolStripMenuItemMetadataReadUndo,
            this.toolStripMenuItemMetadataReadRedo,
            this.toolStripMenuItemMetadataReadFind,
            this.toolStripMenuItemMetadataReadReplace,
            this.toolStripMenuItemMetadataReadMarkFavorite,
            this.toolStripMenuItemMetadataReadRemoveFavorite,
            this.toolStripMenuItemMetadataReadToggleFavorite,
            this.toolStripMenuItemMetadataReadShowFavorite});
            this.contextMenuStripMetadataRead.Name = "contextMenuStripMap";
            this.contextMenuStripMetadataRead.Size = new System.Drawing.Size(210, 342);
            // 
            // toolStripMenuItemMetadataReadMove
            // 
            this.toolStripMenuItemMetadataReadMove.Name = "toolStripMenuItemMetadataReadMove";
            this.toolStripMenuItemMetadataReadMove.Size = new System.Drawing.Size(209, 26);
            this.toolStripMenuItemMetadataReadMove.Text = "Assign to tag";
            // 
            // toolStripMenuItemMetadataReadCut
            // 
            this.toolStripMenuItemMetadataReadCut.Image = global::PhotoTagsSynchronizer.Properties.Resources.Cut;
            this.toolStripMenuItemMetadataReadCut.Name = "toolStripMenuItemMetadataReadCut";
            this.toolStripMenuItemMetadataReadCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.toolStripMenuItemMetadataReadCut.Size = new System.Drawing.Size(209, 26);
            this.toolStripMenuItemMetadataReadCut.Text = "Cut";
            this.toolStripMenuItemMetadataReadCut.Click += new System.EventHandler(this.toolStripMenuItemMetadataReadCut_Click);
            // 
            // toolStripMenuItemMetadataReadCopy
            // 
            this.toolStripMenuItemMetadataReadCopy.Image = global::PhotoTagsSynchronizer.Properties.Resources.Copy;
            this.toolStripMenuItemMetadataReadCopy.Name = "toolStripMenuItemMetadataReadCopy";
            this.toolStripMenuItemMetadataReadCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.toolStripMenuItemMetadataReadCopy.Size = new System.Drawing.Size(209, 26);
            this.toolStripMenuItemMetadataReadCopy.Text = "Copy";
            this.toolStripMenuItemMetadataReadCopy.Click += new System.EventHandler(this.toolStripMenuItemMetadataReadCopy_Click);
            // 
            // toolStripMenuItemMetadataReadPaste
            // 
            this.toolStripMenuItemMetadataReadPaste.Image = global::PhotoTagsSynchronizer.Properties.Resources.Paste;
            this.toolStripMenuItemMetadataReadPaste.Name = "toolStripMenuItemMetadataReadPaste";
            this.toolStripMenuItemMetadataReadPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.toolStripMenuItemMetadataReadPaste.Size = new System.Drawing.Size(209, 26);
            this.toolStripMenuItemMetadataReadPaste.Text = "Paste";
            this.toolStripMenuItemMetadataReadPaste.Click += new System.EventHandler(this.toolStripMenuItemMetadataReadPaste_Click);
            // 
            // toolStripMenuItemMetadataReadDelete
            // 
            this.toolStripMenuItemMetadataReadDelete.Image = global::PhotoTagsSynchronizer.Properties.Resources.Delete;
            this.toolStripMenuItemMetadataReadDelete.Name = "toolStripMenuItemMetadataReadDelete";
            this.toolStripMenuItemMetadataReadDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.toolStripMenuItemMetadataReadDelete.Size = new System.Drawing.Size(209, 26);
            this.toolStripMenuItemMetadataReadDelete.Text = "Delete";
            this.toolStripMenuItemMetadataReadDelete.Click += new System.EventHandler(this.toolStripMenuItemMetadataReadDelete_Click);
            // 
            // toolStripMenuItemMetadataReadUndo
            // 
            this.toolStripMenuItemMetadataReadUndo.Image = global::PhotoTagsSynchronizer.Properties.Resources.Undo;
            this.toolStripMenuItemMetadataReadUndo.Name = "toolStripMenuItemMetadataReadUndo";
            this.toolStripMenuItemMetadataReadUndo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.toolStripMenuItemMetadataReadUndo.Size = new System.Drawing.Size(209, 26);
            this.toolStripMenuItemMetadataReadUndo.Text = "Undo";
            this.toolStripMenuItemMetadataReadUndo.Click += new System.EventHandler(this.toolStripMenuItemMetadataReadUndo_Click);
            // 
            // toolStripMenuItemMetadataReadRedo
            // 
            this.toolStripMenuItemMetadataReadRedo.Image = global::PhotoTagsSynchronizer.Properties.Resources.Redo;
            this.toolStripMenuItemMetadataReadRedo.Name = "toolStripMenuItemMetadataReadRedo";
            this.toolStripMenuItemMetadataReadRedo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.toolStripMenuItemMetadataReadRedo.Size = new System.Drawing.Size(209, 26);
            this.toolStripMenuItemMetadataReadRedo.Text = "Redo";
            this.toolStripMenuItemMetadataReadRedo.Click += new System.EventHandler(this.toolStripMenuItemMetadataReadRedo_Click);
            // 
            // toolStripMenuItemMetadataReadFind
            // 
            this.toolStripMenuItemMetadataReadFind.Image = global::PhotoTagsSynchronizer.Properties.Resources.Find;
            this.toolStripMenuItemMetadataReadFind.Name = "toolStripMenuItemMetadataReadFind";
            this.toolStripMenuItemMetadataReadFind.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.toolStripMenuItemMetadataReadFind.Size = new System.Drawing.Size(209, 26);
            this.toolStripMenuItemMetadataReadFind.Text = "Find";
            this.toolStripMenuItemMetadataReadFind.Click += new System.EventHandler(this.toolStripMenuItemMetadataReadFind_Click);
            // 
            // toolStripMenuItemMetadataReadReplace
            // 
            this.toolStripMenuItemMetadataReadReplace.Image = global::PhotoTagsSynchronizer.Properties.Resources.Replace;
            this.toolStripMenuItemMetadataReadReplace.Name = "toolStripMenuItemMetadataReadReplace";
            this.toolStripMenuItemMetadataReadReplace.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.toolStripMenuItemMetadataReadReplace.Size = new System.Drawing.Size(209, 26);
            this.toolStripMenuItemMetadataReadReplace.Text = "Replace";
            this.toolStripMenuItemMetadataReadReplace.Click += new System.EventHandler(this.toolStripMenuItemMetadataReadReplace_Click);
            // 
            // toolStripMenuItemMetadataReadMarkFavorite
            // 
            this.toolStripMenuItemMetadataReadMarkFavorite.Image = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteSelect;
            this.toolStripMenuItemMetadataReadMarkFavorite.Name = "toolStripMenuItemMetadataReadMarkFavorite";
            this.toolStripMenuItemMetadataReadMarkFavorite.Size = new System.Drawing.Size(209, 26);
            this.toolStripMenuItemMetadataReadMarkFavorite.Text = "Mark as favorite";
            this.toolStripMenuItemMetadataReadMarkFavorite.Click += new System.EventHandler(this.toolStripMenuItemMetadataReadMarkFavorite_Click);
            // 
            // toolStripMenuItemMetadataReadRemoveFavorite
            // 
            this.toolStripMenuItemMetadataReadRemoveFavorite.Image = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteRemove;
            this.toolStripMenuItemMetadataReadRemoveFavorite.Name = "toolStripMenuItemMetadataReadRemoveFavorite";
            this.toolStripMenuItemMetadataReadRemoveFavorite.Size = new System.Drawing.Size(209, 26);
            this.toolStripMenuItemMetadataReadRemoveFavorite.Text = "Remove as favorite";
            this.toolStripMenuItemMetadataReadRemoveFavorite.Click += new System.EventHandler(this.toolStripMenuItemMetadataReadRemoveFavorite_Click);
            // 
            // toolStripMenuItemMetadataReadToggleFavorite
            // 
            this.toolStripMenuItemMetadataReadToggleFavorite.Image = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteToggle;
            this.toolStripMenuItemMetadataReadToggleFavorite.Name = "toolStripMenuItemMetadataReadToggleFavorite";
            this.toolStripMenuItemMetadataReadToggleFavorite.Size = new System.Drawing.Size(209, 26);
            this.toolStripMenuItemMetadataReadToggleFavorite.Text = "Toggle favorite";
            this.toolStripMenuItemMetadataReadToggleFavorite.Click += new System.EventHandler(this.toolStripMenuItemMetadataReadToggleFavorite_Click);
            // 
            // toolStripMenuItemMetadataReadShowFavorite
            // 
            this.toolStripMenuItemMetadataReadShowFavorite.Name = "toolStripMenuItemMetadataReadShowFavorite";
            this.toolStripMenuItemMetadataReadShowFavorite.Size = new System.Drawing.Size(209, 26);
            this.toolStripMenuItemMetadataReadShowFavorite.Text = "Show favorite rows";
            this.toolStripMenuItemMetadataReadShowFavorite.Click += new System.EventHandler(this.toolStripMenuItemMetadataReadShowFavorite_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Controls.Add(this.textBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(792, 382);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Metadata Write";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.listBox1);
            this.panel1.Controls.Add(this.textBox5);
            this.panel1.Controls.Add(this.textBox4);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.textBox3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(3, 55);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(786, 321);
            this.panel1.TabIndex = 4;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 16;
            this.listBox1.Location = new System.Drawing.Point(358, 1006);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(120, 84);
            this.listBox1.TabIndex = 5;
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(5, 282);
            this.textBox5.Multiline = true;
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(754, 117);
            this.textBox5.TabIndex = 4;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(5, 163);
            this.textBox4.Multiline = true;
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(754, 75);
            this.textBox4.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 143);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "label2";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(5, 32);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(754, 90);
            this.textBox3.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2.Location = new System.Drawing.Point(3, 6);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(786, 43);
            this.textBox2.TabIndex = 3;
            this.textBox2.Text = resources.GetString("textBox2.Text");
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.textBox6);
            this.tabPage3.Controls.Add(this.textBoxConfigFilenameDateFormats);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(792, 382);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "File date formats";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // textBox6
            // 
            this.textBox6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox6.Location = new System.Drawing.Point(3, 6);
            this.textBox6.Multiline = true;
            this.textBox6.Name = "textBox6";
            this.textBox6.ReadOnly = true;
            this.textBox6.Size = new System.Drawing.Size(786, 43);
            this.textBox6.TabIndex = 4;
            this.textBox6.Text = "When renaming media files. Date and time can be removed. This is list of date and" +
    " time formats, that will be removed from filename during rename tool. ";
            // 
            // textBoxConfigFilenameDateFormats
            // 
            this.textBoxConfigFilenameDateFormats.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxConfigFilenameDateFormats.Location = new System.Drawing.Point(3, 63);
            this.textBoxConfigFilenameDateFormats.Multiline = true;
            this.textBoxConfigFilenameDateFormats.Name = "textBoxConfigFilenameDateFormats";
            this.textBoxConfigFilenameDateFormats.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxConfigFilenameDateFormats.Size = new System.Drawing.Size(786, 316);
            this.textBoxConfigFilenameDateFormats.TabIndex = 0;
            this.textBoxConfigFilenameDateFormats.Text = resources.GetString("textBoxConfigFilenameDateFormats.Text");
            // 
            // buttonConfigSave
            // 
            this.buttonConfigSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonConfigSave.Location = new System.Drawing.Point(284, 417);
            this.buttonConfigSave.Name = "buttonConfigSave";
            this.buttonConfigSave.Size = new System.Drawing.Size(75, 23);
            this.buttonConfigSave.TabIndex = 1;
            this.buttonConfigSave.Text = "Save";
            this.buttonConfigSave.UseVisualStyleBackColor = true;
            this.buttonConfigSave.Click += new System.EventHandler(this.buttonConfigSave_Click);
            // 
            // buttonConfigCancel
            // 
            this.buttonConfigCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonConfigCancel.Location = new System.Drawing.Point(386, 417);
            this.buttonConfigCancel.Name = "buttonConfigCancel";
            this.buttonConfigCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonConfigCancel.TabIndex = 2;
            this.buttonConfigCancel.Text = "Cancel";
            this.buttonConfigCancel.UseVisualStyleBackColor = true;
            this.buttonConfigCancel.Click += new System.EventHandler(this.buttonConfigCancel_Click);
            // 
            // Config
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.buttonConfigCancel);
            this.Controls.Add(this.buttonConfigSave);
            this.Controls.Add(this.tabControlConfig);
            this.Name = "Config";
            this.Text = "Config";
            this.Load += new System.EventHandler(this.Config_Load);
            this.tabControlConfig.ResumeLayout(false);
            this.tabPageFileDates.ResumeLayout(false);
            this.tabPageFileDates.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMetadataReadPriority)).EndInit();
            this.contextMenuStripMetadataRead.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlConfig;
        private System.Windows.Forms.TabPage tabPageFileDates;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dataGridViewMetadataReadPriority;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox textBoxConfigFilenameDateFormats;
        private System.Windows.Forms.Button buttonConfigSave;
        private System.Windows.Forms.Button buttonConfigCancel;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripMetadataRead;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMetadataReadMove;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMetadataReadCut;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMetadataReadCopy;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMetadataReadPaste;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMetadataReadDelete;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMetadataReadUndo;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMetadataReadRedo;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMetadataReadFind;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMetadataReadReplace;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMetadataReadMarkFavorite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMetadataReadRemoveFavorite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMetadataReadToggleFavorite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMetadataReadShowFavorite;
    }
}