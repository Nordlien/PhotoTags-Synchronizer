
namespace PhotoTagsSynchronizer
{
    partial class FormWebScraper
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWebScraper));
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panelBrowser = new System.Windows.Forms.Panel();
            this.textBoxBrowserURL = new System.Windows.Forms.TextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.fastColoredTextBoxJavaScript = new FastColoredTextBoxNS.FastColoredTextBox();
            this.fastColoredTextBoxJavaScriptResult = new FastColoredTextBoxNS.FastColoredTextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.buttonWebScrapingStart = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeaderMediaFilesFilename = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.textBoxActiveTag = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxActiveAlbum = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.listViewLinks = new System.Windows.Forms.ListView();
            this.columnHeaderCategoryName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderCategoryLink = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.checkBoxClickNext = new System.Windows.Forms.CheckBox();
            this.checkBoxRecord = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonBrowserShowDevTool = new System.Windows.Forms.Button();
            this.WebScrapingCategories = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fastColoredTextBoxJavaScript)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fastColoredTextBoxJavaScriptResult)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.splitContainer1);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1121, 737);
            this.panel1.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel3);
            this.splitContainer1.Size = new System.Drawing.Size(1115, 731);
            this.splitContainer1.SplitterDistance = 896;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.panel2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.panel4);
            this.splitContainer2.Size = new System.Drawing.Size(894, 731);
            this.splitContainer2.SplitterDistance = 527;
            this.splitContainer2.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.panelBrowser);
            this.panel2.Controls.Add(this.textBoxBrowserURL);
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(894, 525);
            this.panel2.TabIndex = 2;
            // 
            // panelBrowser
            // 
            this.panelBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelBrowser.Location = new System.Drawing.Point(3, 3);
            this.panelBrowser.Name = "panelBrowser";
            this.panelBrowser.Size = new System.Drawing.Size(886, 489);
            this.panelBrowser.TabIndex = 0;
            // 
            // textBoxBrowserURL
            // 
            this.textBoxBrowserURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBrowserURL.Location = new System.Drawing.Point(3, 498);
            this.textBoxBrowserURL.Name = "textBoxBrowserURL";
            this.textBoxBrowserURL.Size = new System.Drawing.Size(886, 22);
            this.textBoxBrowserURL.TabIndex = 1;
            this.textBoxBrowserURL.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxBrowserURL_KeyPress);
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.splitContainer3);
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(894, 200);
            this.panel4.TabIndex = 0;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.fastColoredTextBoxJavaScript);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.fastColoredTextBoxJavaScriptResult);
            this.splitContainer3.Size = new System.Drawing.Size(893, 199);
            this.splitContainer3.SplitterDistance = 100;
            this.splitContainer3.TabIndex = 0;
            // 
            // fastColoredTextBoxJavaScript
            // 
            this.fastColoredTextBoxJavaScript.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fastColoredTextBoxJavaScript.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.fastColoredTextBoxJavaScript.AutoIndentCharsPatterns = "\r\n^\\s*[\\w\\.]+(\\s\\w+)?\\s*(?<range>=)\\s*(?<range>[^;]+);\r\n";
            this.fastColoredTextBoxJavaScript.AutoScrollMinSize = new System.Drawing.Size(0, 126);
            this.fastColoredTextBoxJavaScript.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.fastColoredTextBoxJavaScript.BackBrush = null;
            this.fastColoredTextBoxJavaScript.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fastColoredTextBoxJavaScript.BracketsHighlightStrategy = FastColoredTextBoxNS.BracketsHighlightStrategy.Strategy2;
            this.fastColoredTextBoxJavaScript.CharHeight = 18;
            this.fastColoredTextBoxJavaScript.CharWidth = 10;
            this.fastColoredTextBoxJavaScript.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.fastColoredTextBoxJavaScript.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.fastColoredTextBoxJavaScript.IsReplaceMode = false;
            this.fastColoredTextBoxJavaScript.Language = FastColoredTextBoxNS.Language.JS;
            this.fastColoredTextBoxJavaScript.LeftBracket = '(';
            this.fastColoredTextBoxJavaScript.LeftBracket2 = '{';
            this.fastColoredTextBoxJavaScript.Location = new System.Drawing.Point(0, 0);
            this.fastColoredTextBoxJavaScript.Name = "fastColoredTextBoxJavaScript";
            this.fastColoredTextBoxJavaScript.Paddings = new System.Windows.Forms.Padding(0);
            this.fastColoredTextBoxJavaScript.RightBracket = ')';
            this.fastColoredTextBoxJavaScript.RightBracket2 = '}';
            this.fastColoredTextBoxJavaScript.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.fastColoredTextBoxJavaScript.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fastColoredTextBoxJavaScript.ServiceColors")));
            this.fastColoredTextBoxJavaScript.Size = new System.Drawing.Size(889, 97);
            this.fastColoredTextBoxJavaScript.TabIndex = 27;
            this.fastColoredTextBoxJavaScript.Text = "1\r\n2\r\n3\r\n4\r\n5\r\n6\r\n7";
            this.fastColoredTextBoxJavaScript.WordWrap = true;
            this.fastColoredTextBoxJavaScript.Zoom = 100;
            // 
            // fastColoredTextBoxJavaScriptResult
            // 
            this.fastColoredTextBoxJavaScriptResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fastColoredTextBoxJavaScriptResult.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.fastColoredTextBoxJavaScriptResult.AutoIndentCharsPatterns = "";
            this.fastColoredTextBoxJavaScriptResult.AutoScrollMinSize = new System.Drawing.Size(0, 126);
            this.fastColoredTextBoxJavaScriptResult.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.fastColoredTextBoxJavaScriptResult.BackBrush = null;
            this.fastColoredTextBoxJavaScriptResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fastColoredTextBoxJavaScriptResult.CharHeight = 18;
            this.fastColoredTextBoxJavaScriptResult.CharWidth = 10;
            this.fastColoredTextBoxJavaScriptResult.CommentPrefix = null;
            this.fastColoredTextBoxJavaScriptResult.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.fastColoredTextBoxJavaScriptResult.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.fastColoredTextBoxJavaScriptResult.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.fastColoredTextBoxJavaScriptResult.IsReplaceMode = false;
            this.fastColoredTextBoxJavaScriptResult.Language = FastColoredTextBoxNS.Language.XML;
            this.fastColoredTextBoxJavaScriptResult.LeftBracket = '<';
            this.fastColoredTextBoxJavaScriptResult.LeftBracket2 = '(';
            this.fastColoredTextBoxJavaScriptResult.Location = new System.Drawing.Point(0, 0);
            this.fastColoredTextBoxJavaScriptResult.Name = "fastColoredTextBoxJavaScriptResult";
            this.fastColoredTextBoxJavaScriptResult.Paddings = new System.Windows.Forms.Padding(0);
            this.fastColoredTextBoxJavaScriptResult.RightBracket = '>';
            this.fastColoredTextBoxJavaScriptResult.RightBracket2 = ')';
            this.fastColoredTextBoxJavaScriptResult.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.fastColoredTextBoxJavaScriptResult.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fastColoredTextBoxJavaScriptResult.ServiceColors")));
            this.fastColoredTextBoxJavaScriptResult.Size = new System.Drawing.Size(889, 88);
            this.fastColoredTextBoxJavaScriptResult.TabIndex = 28;
            this.fastColoredTextBoxJavaScriptResult.Text = "1\r\n2\r\n3\r\n4\r\n5\r\n6\r\n7";
            this.fastColoredTextBoxJavaScriptResult.WordWrap = true;
            this.fastColoredTextBoxJavaScriptResult.Zoom = 100;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.WebScrapingCategories);
            this.panel3.Controls.Add(this.buttonWebScrapingStart);
            this.panel3.Controls.Add(this.listView1);
            this.panel3.Controls.Add(this.textBoxActiveTag);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.textBoxActiveAlbum);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.listViewLinks);
            this.panel3.Controls.Add(this.checkBoxClickNext);
            this.panel3.Controls.Add(this.checkBoxRecord);
            this.panel3.Controls.Add(this.button1);
            this.panel3.Controls.Add(this.buttonBrowserShowDevTool);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(215, 731);
            this.panel3.TabIndex = 0;
            // 
            // buttonWebScrapingStart
            // 
            this.buttonWebScrapingStart.Location = new System.Drawing.Point(2, 308);
            this.buttonWebScrapingStart.Name = "buttonWebScrapingStart";
            this.buttonWebScrapingStart.Size = new System.Drawing.Size(203, 25);
            this.buttonWebScrapingStart.TabIndex = 12;
            this.buttonWebScrapingStart.Text = "Start Scraping";
            this.buttonWebScrapingStart.UseVisualStyleBackColor = true;
            this.buttonWebScrapingStart.Click += new System.EventHandler(this.buttonWebScrapingStart_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderMediaFilesFilename});
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(-1, 530);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(207, 200);
            this.listView1.TabIndex = 11;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderMediaFilesFilename
            // 
            this.columnHeaderMediaFilesFilename.Text = "Filename";
            this.columnHeaderMediaFilesFilename.Width = 81;
            // 
            // textBoxActiveTag
            // 
            this.textBoxActiveTag.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxActiveTag.Location = new System.Drawing.Point(2, 412);
            this.textBoxActiveTag.Name = "textBoxActiveTag";
            this.textBoxActiveTag.Size = new System.Drawing.Size(204, 22);
            this.textBoxActiveTag.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 392);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 17);
            this.label3.TabIndex = 9;
            this.label3.Text = "Active tag:";
            // 
            // textBoxActiveAlbum
            // 
            this.textBoxActiveAlbum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxActiveAlbum.Location = new System.Drawing.Point(3, 356);
            this.textBoxActiveAlbum.Name = "textBoxActiveAlbum";
            this.textBoxActiveAlbum.Size = new System.Drawing.Size(204, 22);
            this.textBoxActiveAlbum.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 336);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "Active album:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "Categories:";
            // 
            // listViewLinks
            // 
            this.listViewLinks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewLinks.CheckBoxes = true;
            this.listViewLinks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderCategoryName,
            this.columnHeaderCategoryLink});
            this.listViewLinks.HideSelection = false;
            this.listViewLinks.Location = new System.Drawing.Point(2, 101);
            this.listViewLinks.Name = "listViewLinks";
            this.listViewLinks.Size = new System.Drawing.Size(204, 167);
            this.listViewLinks.TabIndex = 5;
            this.listViewLinks.UseCompatibleStateImageBehavior = false;
            this.listViewLinks.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderCategoryName
            // 
            this.columnHeaderCategoryName.Text = "Name";
            this.columnHeaderCategoryName.Width = 101;
            // 
            // columnHeaderCategoryLink
            // 
            this.columnHeaderCategoryLink.Text = "Link";
            this.columnHeaderCategoryLink.Width = 90;
            // 
            // checkBoxClickNext
            // 
            this.checkBoxClickNext.AutoSize = true;
            this.checkBoxClickNext.Location = new System.Drawing.Point(-2, 477);
            this.checkBoxClickNext.Name = "checkBoxClickNext";
            this.checkBoxClickNext.Size = new System.Drawing.Size(170, 21);
            this.checkBoxClickNext.TabIndex = 4;
            this.checkBoxClickNext.Text = "Auto Click Right-Arrow";
            this.checkBoxClickNext.UseVisualStyleBackColor = true;
            // 
            // checkBoxRecord
            // 
            this.checkBoxRecord.AutoSize = true;
            this.checkBoxRecord.Checked = true;
            this.checkBoxRecord.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxRecord.Location = new System.Drawing.Point(-1, 416);
            this.checkBoxRecord.Name = "checkBoxRecord";
            this.checkBoxRecord.Size = new System.Drawing.Size(76, 21);
            this.checkBoxRecord.TabIndex = 3;
            this.checkBoxRecord.Text = "Record";
            this.checkBoxRecord.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(3, 37);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(203, 25);
            this.button1.TabIndex = 2;
            this.button1.Text = "Run JavaScript";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.buttonRunJavaScript_Click);
            // 
            // buttonBrowserShowDevTool
            // 
            this.buttonBrowserShowDevTool.Location = new System.Drawing.Point(3, 8);
            this.buttonBrowserShowDevTool.Name = "buttonBrowserShowDevTool";
            this.buttonBrowserShowDevTool.Size = new System.Drawing.Size(203, 25);
            this.buttonBrowserShowDevTool.TabIndex = 1;
            this.buttonBrowserShowDevTool.Text = "Show DevTools";
            this.buttonBrowserShowDevTool.UseVisualStyleBackColor = true;
            this.buttonBrowserShowDevTool.Click += new System.EventHandler(this.buttonBrowserShowDevTool_Click);
            // 
            // WebScrapingCategories
            // 
            this.WebScrapingCategories.Location = new System.Drawing.Point(2, 277);
            this.WebScrapingCategories.Name = "WebScrapingCategories";
            this.WebScrapingCategories.Size = new System.Drawing.Size(203, 25);
            this.WebScrapingCategories.TabIndex = 13;
            this.WebScrapingCategories.Text = "Fetch categries";
            this.WebScrapingCategories.UseVisualStyleBackColor = true;
            this.WebScrapingCategories.Click += new System.EventHandler(this.WebScrapingCategories_Click);
            // 
            // FormWebScraper
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1123, 739);
            this.Controls.Add(this.panel1);
            this.Name = "FormWebScraper";
            this.Text = "FormWebScraper";
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fastColoredTextBoxJavaScript)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fastColoredTextBoxJavaScriptResult)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TextBox textBoxBrowserURL;
        private System.Windows.Forms.Panel panelBrowser;
        private FastColoredTextBoxNS.FastColoredTextBox fastColoredTextBoxJavaScript;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button buttonBrowserShowDevTool;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private FastColoredTextBoxNS.FastColoredTextBox fastColoredTextBoxJavaScriptResult;
        private System.Windows.Forms.CheckBox checkBoxClickNext;
        private System.Windows.Forms.CheckBox checkBoxRecord;
        private System.Windows.Forms.ListView listViewLinks;
        private System.Windows.Forms.ColumnHeader columnHeaderCategoryName;
        private System.Windows.Forms.ColumnHeader columnHeaderCategoryLink;
        private System.Windows.Forms.Button buttonWebScrapingStart;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeaderMediaFilesFilename;
        private System.Windows.Forms.TextBox textBoxActiveTag;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxActiveAlbum;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button WebScrapingCategories;
    }
}