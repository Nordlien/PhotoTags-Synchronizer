
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAutoCorrect));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.label1 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.textBoxKeywords = new System.Windows.Forms.TextBox();
            this.checkBoxAuthor = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxComments = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxDescription = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxTitle = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxAlbum = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonAutoCorrect = new System.Windows.Forms.Button();
            this.comboBoxAuthor = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.comboBoxAlbum = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.comboBoxComments = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.comboBoxDescription = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.comboBoxTitle = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.kryptonManager1 = new ComponentFactory.Krypton.Toolkit.KryptonManager(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxAuthor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxAlbum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxComments)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxDescription)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxTitle)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.textBoxKeywords);
            this.panel1.Controls.Add(this.checkBoxAuthor);
            this.panel1.Controls.Add(this.checkBoxComments);
            this.panel1.Controls.Add(this.checkBoxDescription);
            this.panel1.Controls.Add(this.checkBoxTitle);
            this.panel1.Controls.Add(this.checkBoxAlbum);
            this.panel1.Controls.Add(this.buttonClose);
            this.panel1.Controls.Add(this.buttonAutoCorrect);
            this.panel1.Controls.Add(this.comboBoxAuthor);
            this.panel1.Controls.Add(this.comboBoxAlbum);
            this.panel1.Controls.Add(this.comboBoxComments);
            this.panel1.Controls.Add(this.comboBoxDescription);
            this.panel1.Controls.Add(this.comboBoxTitle);
            this.panel1.Location = new System.Drawing.Point(0, -2);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(769, 352);
            this.panel1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(16, 311);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(469, 24);
            this.label2.TabIndex = 14;
            this.label2.Values.Text = "AutoCorrect algorithm will be run fist, this value will override result.";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 188);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 24);
            this.label1.TabIndex = 13;
            this.label1.Values.Text = "Add keywords:";
            // 
            // textBoxKeywords
            // 
            this.textBoxKeywords.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxKeywords.Location = new System.Drawing.Point(165, 185);
            this.textBoxKeywords.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxKeywords.Multiline = true;
            this.textBoxKeywords.Name = "textBoxKeywords";
            this.textBoxKeywords.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxKeywords.Size = new System.Drawing.Size(587, 98);
            this.textBoxKeywords.TabIndex = 12;
            // 
            // checkBoxAuthor
            // 
            this.checkBoxAuthor.Checked = true;
            this.checkBoxAuthor.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAuthor.Location = new System.Drawing.Point(15, 156);
            this.checkBoxAuthor.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBoxAuthor.Name = "checkBoxAuthor";
            this.checkBoxAuthor.Size = new System.Drawing.Size(72, 24);
            this.checkBoxAuthor.TabIndex = 9;
            this.checkBoxAuthor.Values.Text = "Author";
            // 
            // checkBoxComments
            // 
            this.checkBoxComments.Checked = true;
            this.checkBoxComments.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxComments.Location = new System.Drawing.Point(15, 126);
            this.checkBoxComments.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBoxComments.Name = "checkBoxComments";
            this.checkBoxComments.Size = new System.Drawing.Size(98, 24);
            this.checkBoxComments.TabIndex = 8;
            this.checkBoxComments.Values.Text = "Comments";
            // 
            // checkBoxDescription
            // 
            this.checkBoxDescription.Checked = true;
            this.checkBoxDescription.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDescription.Location = new System.Drawing.Point(15, 95);
            this.checkBoxDescription.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBoxDescription.Name = "checkBoxDescription";
            this.checkBoxDescription.Size = new System.Drawing.Size(102, 24);
            this.checkBoxDescription.TabIndex = 7;
            this.checkBoxDescription.Values.Text = "Description";
            // 
            // checkBoxTitle
            // 
            this.checkBoxTitle.Checked = true;
            this.checkBoxTitle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxTitle.Location = new System.Drawing.Point(15, 64);
            this.checkBoxTitle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBoxTitle.Name = "checkBoxTitle";
            this.checkBoxTitle.Size = new System.Drawing.Size(53, 24);
            this.checkBoxTitle.TabIndex = 6;
            this.checkBoxTitle.Values.Text = "Title";
            // 
            // checkBoxAlbum
            // 
            this.checkBoxAlbum.Checked = true;
            this.checkBoxAlbum.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAlbum.Location = new System.Drawing.Point(15, 33);
            this.checkBoxAlbum.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBoxAlbum.Name = "checkBoxAlbum";
            this.checkBoxAlbum.Size = new System.Drawing.Size(69, 24);
            this.checkBoxAlbum.TabIndex = 5;
            this.checkBoxAlbum.Values.Text = "Album";
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.Location = new System.Drawing.Point(628, 299);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(125, 39);
            this.buttonClose.TabIndex = 11;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonAutoCorrect
            // 
            this.buttonAutoCorrect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAutoCorrect.Location = new System.Drawing.Point(496, 299);
            this.buttonAutoCorrect.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAutoCorrect.Name = "buttonAutoCorrect";
            this.buttonAutoCorrect.Size = new System.Drawing.Size(125, 39);
            this.buttonAutoCorrect.TabIndex = 10;
            this.buttonAutoCorrect.Text = "Run AutoCorrect";
            this.buttonAutoCorrect.UseVisualStyleBackColor = true;
            this.buttonAutoCorrect.Click += new System.EventHandler(this.buttonAutoCorrect_Click);
            // 
            // comboBoxAuthor
            // 
            this.comboBoxAuthor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxAuthor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxAuthor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxAuthor.DropDownWidth = 588;
            this.comboBoxAuthor.FormattingEnabled = true;
            this.comboBoxAuthor.Location = new System.Drawing.Point(165, 154);
            this.comboBoxAuthor.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBoxAuthor.Name = "comboBoxAuthor";
            this.comboBoxAuthor.Size = new System.Drawing.Size(588, 25);
            this.comboBoxAuthor.TabIndex = 4;
            // 
            // comboBoxAlbum
            // 
            this.comboBoxAlbum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxAlbum.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxAlbum.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxAlbum.DropDownWidth = 588;
            this.comboBoxAlbum.FormattingEnabled = true;
            this.comboBoxAlbum.Location = new System.Drawing.Point(165, 31);
            this.comboBoxAlbum.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBoxAlbum.Name = "comboBoxAlbum";
            this.comboBoxAlbum.Size = new System.Drawing.Size(588, 25);
            this.comboBoxAlbum.TabIndex = 0;
            // 
            // comboBoxComments
            // 
            this.comboBoxComments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxComments.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxComments.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxComments.DropDownWidth = 588;
            this.comboBoxComments.FormattingEnabled = true;
            this.comboBoxComments.Location = new System.Drawing.Point(165, 123);
            this.comboBoxComments.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBoxComments.Name = "comboBoxComments";
            this.comboBoxComments.Size = new System.Drawing.Size(588, 25);
            this.comboBoxComments.TabIndex = 3;
            // 
            // comboBoxDescription
            // 
            this.comboBoxDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxDescription.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxDescription.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxDescription.DropDownWidth = 588;
            this.comboBoxDescription.FormattingEnabled = true;
            this.comboBoxDescription.Location = new System.Drawing.Point(165, 92);
            this.comboBoxDescription.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBoxDescription.Name = "comboBoxDescription";
            this.comboBoxDescription.Size = new System.Drawing.Size(588, 25);
            this.comboBoxDescription.TabIndex = 2;
            // 
            // comboBoxTitle
            // 
            this.comboBoxTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxTitle.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxTitle.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxTitle.DropDownWidth = 588;
            this.comboBoxTitle.FormattingEnabled = true;
            this.comboBoxTitle.Location = new System.Drawing.Point(165, 62);
            this.comboBoxTitle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBoxTitle.Name = "comboBoxTitle";
            this.comboBoxTitle.Size = new System.Drawing.Size(588, 25);
            this.comboBoxTitle.TabIndex = 1;
            // 
            // FormAutoCorrect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(769, 350);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FormAutoCorrect";
            this.Text = "AutoCorrect";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxAuthor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxAlbum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxComments)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxDescription)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxTitle)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private ComponentFactory.Krypton.Toolkit.KryptonComboBox comboBoxAuthor;
        private ComponentFactory.Krypton.Toolkit.KryptonComboBox comboBoxAlbum;
        private ComponentFactory.Krypton.Toolkit.KryptonComboBox comboBoxComments;
        private ComponentFactory.Krypton.Toolkit.KryptonComboBox comboBoxDescription;
        private ComponentFactory.Krypton.Toolkit.KryptonComboBox comboBoxTitle;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonAutoCorrect;
        private ComponentFactory.Krypton.Toolkit.KryptonCheckBox checkBoxAuthor;
        private ComponentFactory.Krypton.Toolkit.KryptonCheckBox checkBoxComments;
        private ComponentFactory.Krypton.Toolkit.KryptonCheckBox checkBoxDescription;
        private ComponentFactory.Krypton.Toolkit.KryptonCheckBox checkBoxTitle;
        private ComponentFactory.Krypton.Toolkit.KryptonCheckBox checkBoxAlbum;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel label2;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel label1;
        private System.Windows.Forms.TextBox textBoxKeywords;
        private ComponentFactory.Krypton.Toolkit.KryptonManager kryptonManager1;
    }
}