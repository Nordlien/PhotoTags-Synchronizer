
namespace PhotoTagsSynchronizer
{
    partial class FormAutoCorrect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAutoCorrect));
            this.panel1 = new Krypton.Toolkit.KryptonPanel();
            this.label2 = new Krypton.Toolkit.KryptonLabel();
            this.label1 = new Krypton.Toolkit.KryptonLabel();
            this.textBoxKeywords = new Krypton.Toolkit.KryptonTextBox();
            this.checkBoxAuthor = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxComments = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxDescription = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxTitle = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxAlbum = new Krypton.Toolkit.KryptonCheckBox();
            this.buttonClose = new Krypton.Toolkit.KryptonButton();
            this.buttonAutoCorrect = new Krypton.Toolkit.KryptonButton();
            this.comboBoxAuthor = new Krypton.Toolkit.KryptonComboBox();
            this.comboBoxAlbum = new Krypton.Toolkit.KryptonComboBox();
            this.comboBoxComments = new Krypton.Toolkit.KryptonComboBox();
            this.comboBoxDescription = new Krypton.Toolkit.KryptonComboBox();
            this.comboBoxTitle = new Krypton.Toolkit.KryptonComboBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.kryptonLabel1 = new Krypton.Toolkit.KryptonLabel();
            this.kryptonLabelDescriptionBecomesAlbum = new Krypton.Toolkit.KryptonLabel();
            ((System.ComponentModel.ISupportInitialize)(this.panel1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxAuthor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxAlbum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxComments)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxDescription)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxTitle)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(769, 350);
            this.panel1.TabIndex = 0;
            // 
            // label2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.label2, 2);
            this.label2.Location = new System.Drawing.Point(103, 4);
            this.label2.Margin = new System.Windows.Forms.Padding(4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(376, 20);
            this.label2.TabIndex = 14;
            this.label2.Values.Text = "AutoCorrect algorithm will be run fist, this value will override result.";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(4, 183);
            this.label1.Margin = new System.Windows.Forms.Padding(4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 20);
            this.label1.TabIndex = 13;
            this.label1.Values.Text = "Add keywords:";
            // 
            // textBoxKeywords
            // 
            this.textBoxKeywords.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.textBoxKeywords, 2);
            this.textBoxKeywords.Location = new System.Drawing.Point(103, 183);
            this.textBoxKeywords.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxKeywords.Multiline = true;
            this.textBoxKeywords.Name = "textBoxKeywords";
            this.textBoxKeywords.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxKeywords.Size = new System.Drawing.Size(662, 96);
            this.textBoxKeywords.TabIndex = 12;
            // 
            // checkBoxAuthor
            // 
            this.checkBoxAuthor.Checked = true;
            this.checkBoxAuthor.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAuthor.Location = new System.Drawing.Point(3, 156);
            this.checkBoxAuthor.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBoxAuthor.Name = "checkBoxAuthor";
            this.checkBoxAuthor.Size = new System.Drawing.Size(61, 20);
            this.checkBoxAuthor.TabIndex = 9;
            this.checkBoxAuthor.Values.Text = "Author";
            // 
            // checkBoxComments
            // 
            this.checkBoxComments.Checked = true;
            this.checkBoxComments.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxComments.Location = new System.Drawing.Point(3, 131);
            this.checkBoxComments.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBoxComments.Name = "checkBoxComments";
            this.checkBoxComments.Size = new System.Drawing.Size(83, 20);
            this.checkBoxComments.TabIndex = 8;
            this.checkBoxComments.Values.Text = "Comments";
            // 
            // checkBoxDescription
            // 
            this.checkBoxDescription.Checked = true;
            this.checkBoxDescription.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDescription.Location = new System.Drawing.Point(3, 80);
            this.checkBoxDescription.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBoxDescription.Name = "checkBoxDescription";
            this.checkBoxDescription.Size = new System.Drawing.Size(86, 20);
            this.checkBoxDescription.TabIndex = 7;
            this.checkBoxDescription.Values.Text = "Description";
            // 
            // checkBoxTitle
            // 
            this.checkBoxTitle.Checked = true;
            this.checkBoxTitle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxTitle.Location = new System.Drawing.Point(3, 55);
            this.checkBoxTitle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBoxTitle.Name = "checkBoxTitle";
            this.checkBoxTitle.Size = new System.Drawing.Size(47, 20);
            this.checkBoxTitle.TabIndex = 6;
            this.checkBoxTitle.Values.Text = "Title";
            // 
            // checkBoxAlbum
            // 
            this.checkBoxAlbum.Checked = true;
            this.checkBoxAlbum.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAlbum.Location = new System.Drawing.Point(3, 30);
            this.checkBoxAlbum.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBoxAlbum.Name = "checkBoxAlbum";
            this.checkBoxAlbum.Size = new System.Drawing.Size(59, 20);
            this.checkBoxAlbum.TabIndex = 5;
            this.checkBoxAlbum.Values.Text = "Album";
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(261, 311);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(153, 39);
            this.buttonClose.TabIndex = 11;
            this.buttonClose.Values.Text = "Close";
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonAutoCorrect
            // 
            this.buttonAutoCorrect.Location = new System.Drawing.Point(102, 311);
            this.buttonAutoCorrect.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAutoCorrect.Name = "buttonAutoCorrect";
            this.buttonAutoCorrect.Size = new System.Drawing.Size(153, 39);
            this.buttonAutoCorrect.TabIndex = 10;
            this.buttonAutoCorrect.Values.Text = "Run AutoCorrect";
            this.buttonAutoCorrect.Click += new System.EventHandler(this.buttonAutoCorrect_Click);
            // 
            // comboBoxAuthor
            // 
            this.comboBoxAuthor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxAuthor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxAuthor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.tableLayoutPanel1.SetColumnSpan(this.comboBoxAuthor, 2);
            this.comboBoxAuthor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxAuthor.DropDownWidth = 588;
            this.comboBoxAuthor.FormattingEnabled = true;
            this.comboBoxAuthor.IntegralHeight = false;
            this.comboBoxAuthor.Location = new System.Drawing.Point(102, 156);
            this.comboBoxAuthor.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBoxAuthor.Name = "comboBoxAuthor";
            this.comboBoxAuthor.Size = new System.Drawing.Size(664, 21);
            this.comboBoxAuthor.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxAuthor.TabIndex = 4;
            // 
            // comboBoxAlbum
            // 
            this.comboBoxAlbum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxAlbum.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxAlbum.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.tableLayoutPanel1.SetColumnSpan(this.comboBoxAlbum, 2);
            this.comboBoxAlbum.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxAlbum.DropDownWidth = 588;
            this.comboBoxAlbum.FormattingEnabled = true;
            this.comboBoxAlbum.IntegralHeight = false;
            this.comboBoxAlbum.Location = new System.Drawing.Point(102, 30);
            this.comboBoxAlbum.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBoxAlbum.Name = "comboBoxAlbum";
            this.comboBoxAlbum.Size = new System.Drawing.Size(664, 21);
            this.comboBoxAlbum.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxAlbum.TabIndex = 0;
            // 
            // comboBoxComments
            // 
            this.comboBoxComments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxComments.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxComments.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.tableLayoutPanel1.SetColumnSpan(this.comboBoxComments, 2);
            this.comboBoxComments.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxComments.DropDownWidth = 588;
            this.comboBoxComments.FormattingEnabled = true;
            this.comboBoxComments.IntegralHeight = false;
            this.comboBoxComments.Location = new System.Drawing.Point(102, 131);
            this.comboBoxComments.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBoxComments.Name = "comboBoxComments";
            this.comboBoxComments.Size = new System.Drawing.Size(664, 21);
            this.comboBoxComments.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxComments.TabIndex = 3;
            // 
            // comboBoxDescription
            // 
            this.comboBoxDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxDescription.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxDescription.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.tableLayoutPanel1.SetColumnSpan(this.comboBoxDescription, 2);
            this.comboBoxDescription.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxDescription.DropDownWidth = 588;
            this.comboBoxDescription.FormattingEnabled = true;
            this.comboBoxDescription.IntegralHeight = false;
            this.comboBoxDescription.Location = new System.Drawing.Point(102, 80);
            this.comboBoxDescription.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBoxDescription.Name = "comboBoxDescription";
            this.comboBoxDescription.Size = new System.Drawing.Size(664, 21);
            this.comboBoxDescription.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxDescription.TabIndex = 2;
            // 
            // comboBoxTitle
            // 
            this.comboBoxTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxTitle.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxTitle.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.tableLayoutPanel1.SetColumnSpan(this.comboBoxTitle, 2);
            this.comboBoxTitle.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxTitle.DropDownWidth = 588;
            this.comboBoxTitle.FormattingEnabled = true;
            this.comboBoxTitle.IntegralHeight = false;
            this.comboBoxTitle.Location = new System.Drawing.Point(102, 55);
            this.comboBoxTitle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBoxTitle.Name = "comboBoxTitle";
            this.comboBoxTitle.Size = new System.Drawing.Size(664, 21);
            this.comboBoxTitle.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxTitle.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBoxKeywords, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxAlbum, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxTitle, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxAuthor, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxDescription, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxAuthor, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxComments, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxComments, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxAlbum, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxDescription, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxTitle, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonAutoCorrect, 1, 9);
            this.tableLayoutPanel1.Controls.Add(this.buttonClose, 2, 9);
            this.tableLayoutPanel1.Controls.Add(this.kryptonLabel1, 1, 8);
            this.tableLayoutPanel1.Controls.Add(this.kryptonLabelDescriptionBecomesAlbum, 1, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 10;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(769, 350);
            this.tableLayoutPanel1.TabIndex = 15;
            // 
            // kryptonLabel1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.kryptonLabel1, 2);
            this.kryptonLabel1.Location = new System.Drawing.Point(102, 286);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(194, 20);
            this.kryptonLabel1.TabIndex = 15;
            this.kryptonLabel1.Values.Text = "Separate keywords with enter key";
            // 
            // kryptonLabelDescriptionBecomesAlbum
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.kryptonLabelDescriptionBecomesAlbum, 2);
            this.kryptonLabelDescriptionBecomesAlbum.Location = new System.Drawing.Point(102, 106);
            this.kryptonLabelDescriptionBecomesAlbum.Name = "kryptonLabelDescriptionBecomesAlbum";
            this.kryptonLabelDescriptionBecomesAlbum.Size = new System.Drawing.Size(340, 20);
            this.kryptonLabelDescriptionBecomesAlbum.TabIndex = 16;
            this.kryptonLabelDescriptionBecomesAlbum.Values.Text = "You can in Config setup Albut to overwrite Description value ";
            // 
            // FormAutoCorrect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(769, 350);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FormAutoCorrect";
            this.Text = "AutoCorrect";
            ((System.ComponentModel.ISupportInitialize)(this.panel1)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxAuthor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxAlbum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxComments)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxDescription)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxTitle)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonPanel panel1;
        private Krypton.Toolkit.KryptonComboBox comboBoxAuthor;
        private Krypton.Toolkit.KryptonComboBox comboBoxAlbum;
        private Krypton.Toolkit.KryptonComboBox comboBoxComments;
        private Krypton.Toolkit.KryptonComboBox comboBoxDescription;
        private Krypton.Toolkit.KryptonComboBox comboBoxTitle;
        private Krypton.Toolkit.KryptonButton buttonClose;
        private Krypton.Toolkit.KryptonButton buttonAutoCorrect;
        private Krypton.Toolkit.KryptonCheckBox checkBoxAuthor;
        private Krypton.Toolkit.KryptonCheckBox checkBoxComments;
        private Krypton.Toolkit.KryptonCheckBox checkBoxDescription;
        private Krypton.Toolkit.KryptonCheckBox checkBoxTitle;
        private Krypton.Toolkit.KryptonCheckBox checkBoxAlbum;
        private Krypton.Toolkit.KryptonLabel label2;
        private Krypton.Toolkit.KryptonLabel label1;
        private Krypton.Toolkit.KryptonTextBox textBoxKeywords;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private Krypton.Toolkit.KryptonLabel kryptonLabelDescriptionBecomesAlbum;
    }
}