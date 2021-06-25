
namespace PhotoTagsSynchronizer
{
    partial class Form_AutoCorrect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_AutoCorrect));
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonAutoCorrect = new System.Windows.Forms.Button();
            this.comboBoxAuthor = new System.Windows.Forms.ComboBox();
            this.comboBoxAlbum = new System.Windows.Forms.ComboBox();
            this.comboBoxComments = new System.Windows.Forms.ComboBox();
            this.comboBoxDescription = new System.Windows.Forms.ComboBox();
            this.comboBoxTitle = new System.Windows.Forms.ComboBox();
            this.checkBoxAlbum = new System.Windows.Forms.CheckBox();
            this.checkBoxTitle = new System.Windows.Forms.CheckBox();
            this.checkBoxDescription = new System.Windows.Forms.CheckBox();
            this.checkBoxComments = new System.Windows.Forms.CheckBox();
            this.checkBoxAuthor = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 269);
            this.panel1.TabIndex = 0;
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.Location = new System.Drawing.Point(610, 189);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(125, 40);
            this.buttonClose.TabIndex = 26;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonAutoCorrect
            // 
            this.buttonAutoCorrect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAutoCorrect.Location = new System.Drawing.Point(479, 189);
            this.buttonAutoCorrect.Name = "buttonAutoCorrect";
            this.buttonAutoCorrect.Size = new System.Drawing.Size(125, 40);
            this.buttonAutoCorrect.TabIndex = 25;
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
            this.comboBoxAuthor.FormattingEnabled = true;
            this.comboBoxAuthor.Location = new System.Drawing.Point(166, 142);
            this.comboBoxAuthor.Name = "comboBoxAuthor";
            this.comboBoxAuthor.Size = new System.Drawing.Size(569, 24);
            this.comboBoxAuthor.TabIndex = 23;
            this.comboBoxAuthor.SelectionChangeCommitted += new System.EventHandler(this.comboBoxAuthor_SelectionChangeCommitted);
            // 
            // comboBoxAlbum
            // 
            this.comboBoxAlbum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxAlbum.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxAlbum.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxAlbum.FormattingEnabled = true;
            this.comboBoxAlbum.Location = new System.Drawing.Point(166, 34);
            this.comboBoxAlbum.Name = "comboBoxAlbum";
            this.comboBoxAlbum.Size = new System.Drawing.Size(569, 24);
            this.comboBoxAlbum.TabIndex = 21;
            this.comboBoxAlbum.SelectionChangeCommitted += new System.EventHandler(this.comboBoxAlbum_SelectionChangeCommitted);
            // 
            // comboBoxComments
            // 
            this.comboBoxComments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxComments.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxComments.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxComments.FormattingEnabled = true;
            this.comboBoxComments.Location = new System.Drawing.Point(166, 115);
            this.comboBoxComments.Name = "comboBoxComments";
            this.comboBoxComments.Size = new System.Drawing.Size(569, 24);
            this.comboBoxComments.TabIndex = 19;
            this.comboBoxComments.SelectionChangeCommitted += new System.EventHandler(this.comboBoxComments_SelectionChangeCommitted);
            // 
            // comboBoxDescription
            // 
            this.comboBoxDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxDescription.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxDescription.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxDescription.FormattingEnabled = true;
            this.comboBoxDescription.Location = new System.Drawing.Point(166, 88);
            this.comboBoxDescription.Name = "comboBoxDescription";
            this.comboBoxDescription.Size = new System.Drawing.Size(569, 24);
            this.comboBoxDescription.TabIndex = 15;
            this.comboBoxDescription.SelectionChangeCommitted += new System.EventHandler(this.comboBoxDescription_SelectionChangeCommitted);
            // 
            // comboBoxTitle
            // 
            this.comboBoxTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxTitle.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxTitle.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxTitle.FormattingEnabled = true;
            this.comboBoxTitle.Location = new System.Drawing.Point(166, 61);
            this.comboBoxTitle.Name = "comboBoxTitle";
            this.comboBoxTitle.Size = new System.Drawing.Size(569, 24);
            this.comboBoxTitle.TabIndex = 14;
            this.comboBoxTitle.SelectionChangeCommitted += new System.EventHandler(this.comboBoxTitle_SelectionChangeCommitted);
            // 
            // checkBoxAlbum
            // 
            this.checkBoxAlbum.AutoSize = true;
            this.checkBoxAlbum.Checked = true;
            this.checkBoxAlbum.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAlbum.Location = new System.Drawing.Point(25, 36);
            this.checkBoxAlbum.Name = "checkBoxAlbum";
            this.checkBoxAlbum.Size = new System.Drawing.Size(69, 21);
            this.checkBoxAlbum.TabIndex = 27;
            this.checkBoxAlbum.Text = "Album";
            this.checkBoxAlbum.UseVisualStyleBackColor = true;
            // 
            // checkBoxTitle
            // 
            this.checkBoxTitle.AutoSize = true;
            this.checkBoxTitle.Checked = true;
            this.checkBoxTitle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxTitle.Location = new System.Drawing.Point(25, 63);
            this.checkBoxTitle.Name = "checkBoxTitle";
            this.checkBoxTitle.Size = new System.Drawing.Size(57, 21);
            this.checkBoxTitle.TabIndex = 28;
            this.checkBoxTitle.Text = "Title";
            this.checkBoxTitle.UseVisualStyleBackColor = true;
            // 
            // checkBoxDescription
            // 
            this.checkBoxDescription.AutoSize = true;
            this.checkBoxDescription.Checked = true;
            this.checkBoxDescription.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDescription.Location = new System.Drawing.Point(25, 90);
            this.checkBoxDescription.Name = "checkBoxDescription";
            this.checkBoxDescription.Size = new System.Drawing.Size(101, 21);
            this.checkBoxDescription.TabIndex = 29;
            this.checkBoxDescription.Text = "Description";
            this.checkBoxDescription.UseVisualStyleBackColor = true;
            // 
            // checkBoxComments
            // 
            this.checkBoxComments.AutoSize = true;
            this.checkBoxComments.Checked = true;
            this.checkBoxComments.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxComments.Location = new System.Drawing.Point(25, 118);
            this.checkBoxComments.Name = "checkBoxComments";
            this.checkBoxComments.Size = new System.Drawing.Size(96, 21);
            this.checkBoxComments.TabIndex = 30;
            this.checkBoxComments.Text = "Comments";
            this.checkBoxComments.UseVisualStyleBackColor = true;
            // 
            // checkBoxAuthor
            // 
            this.checkBoxAuthor.AutoSize = true;
            this.checkBoxAuthor.Checked = true;
            this.checkBoxAuthor.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAuthor.Location = new System.Drawing.Point(25, 144);
            this.checkBoxAuthor.Name = "checkBoxAuthor";
            this.checkBoxAuthor.Size = new System.Drawing.Size(72, 21);
            this.checkBoxAuthor.TabIndex = 31;
            this.checkBoxAuthor.Text = "Author";
            this.checkBoxAuthor.UseVisualStyleBackColor = true;
            // 
            // Form_AutoCorrect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 267);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_AutoCorrect";
            this.Text = "AutoCorrect";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox comboBoxAuthor;
        private System.Windows.Forms.ComboBox comboBoxAlbum;
        private System.Windows.Forms.ComboBox comboBoxComments;
        private System.Windows.Forms.ComboBox comboBoxDescription;
        private System.Windows.Forms.ComboBox comboBoxTitle;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonAutoCorrect;
        private System.Windows.Forms.CheckBox checkBoxAuthor;
        private System.Windows.Forms.CheckBox checkBoxComments;
        private System.Windows.Forms.CheckBox checkBoxDescription;
        private System.Windows.Forms.CheckBox checkBoxTitle;
        private System.Windows.Forms.CheckBox checkBoxAlbum;
    }
}