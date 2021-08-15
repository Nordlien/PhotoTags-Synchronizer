namespace PhotoTagsSynchronizer
{
    partial class ChooseColumns
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChooseColumns));
            this.btnClose = new System.Windows.Forms.Button();
            this.checkedListBox = new PhotoTagsCommonComponets.CheckedListBoxHigherRows();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxTitleLine1 = new System.Windows.Forms.ComboBox();
            this.comboBoxTitleLine2 = new System.Windows.Forms.ComboBox();
            this.comboBoxTitleLine3 = new System.Windows.Forms.ComboBox();
            this.comboBoxTitleLine4 = new System.Windows.Forms.ComboBox();
            this.comboBoxTitleLine5 = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(484, 531);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // checkedListBox
            // 
            this.checkedListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBox.CheckOnClick = true;
            this.checkedListBox.FormattingEnabled = true;
            this.checkedListBox.Location = new System.Drawing.Point(6, 21);
            this.checkedListBox.Name = "checkedListBox";
            this.checkedListBox.Size = new System.Drawing.Size(530, 238);
            this.checkedListBox.TabIndex = 2;
            this.checkedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBox_ItemCheck);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.checkedListBox);
            this.groupBox1.Location = new System.Drawing.Point(14, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(542, 265);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Details view";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.comboBoxTitleLine5);
            this.groupBox2.Controls.Add(this.comboBoxTitleLine4);
            this.groupBox2.Controls.Add(this.comboBoxTitleLine3);
            this.groupBox2.Controls.Add(this.comboBoxTitleLine2);
            this.groupBox2.Controls.Add(this.comboBoxTitleLine1);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(14, 8);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(542, 215);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Title view:";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Size = new System.Drawing.Size(559, 503);
            this.splitContainer1.SplitterDistance = 271;
            this.splitContainer1.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Title Line 1:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Title Line 2:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Title Line 3:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 124);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 17);
            this.label4.TabIndex = 3;
            this.label4.Text = "Title Line 4:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 154);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 17);
            this.label5.TabIndex = 4;
            this.label5.Text = "Title Line 5:";
            // 
            // comboBoxTitleLine1
            // 
            this.comboBoxTitleLine1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTitleLine1.FormattingEnabled = true;
            this.comboBoxTitleLine1.Location = new System.Drawing.Point(103, 31);
            this.comboBoxTitleLine1.Name = "comboBoxTitleLine1";
            this.comboBoxTitleLine1.Size = new System.Drawing.Size(433, 24);
            this.comboBoxTitleLine1.TabIndex = 5;
            this.comboBoxTitleLine1.SelectionChangeCommitted += new System.EventHandler(this.comboBoxTitleLine1_SelectionChangeCommitted);
            // 
            // comboBoxTitleLine2
            // 
            this.comboBoxTitleLine2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTitleLine2.FormattingEnabled = true;
            this.comboBoxTitleLine2.Location = new System.Drawing.Point(103, 61);
            this.comboBoxTitleLine2.Name = "comboBoxTitleLine2";
            this.comboBoxTitleLine2.Size = new System.Drawing.Size(433, 24);
            this.comboBoxTitleLine2.TabIndex = 6;
            this.comboBoxTitleLine2.SelectionChangeCommitted += new System.EventHandler(this.comboBoxTitleLine2_SelectionChangeCommitted);
            // 
            // comboBoxTitleLine3
            // 
            this.comboBoxTitleLine3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTitleLine3.FormattingEnabled = true;
            this.comboBoxTitleLine3.Location = new System.Drawing.Point(103, 91);
            this.comboBoxTitleLine3.Name = "comboBoxTitleLine3";
            this.comboBoxTitleLine3.Size = new System.Drawing.Size(433, 24);
            this.comboBoxTitleLine3.TabIndex = 7;
            this.comboBoxTitleLine3.SelectionChangeCommitted += new System.EventHandler(this.comboBoxTitleLine3_SelectionChangeCommitted);
            // 
            // comboBoxTitleLine4
            // 
            this.comboBoxTitleLine4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTitleLine4.FormattingEnabled = true;
            this.comboBoxTitleLine4.Location = new System.Drawing.Point(103, 121);
            this.comboBoxTitleLine4.Name = "comboBoxTitleLine4";
            this.comboBoxTitleLine4.Size = new System.Drawing.Size(433, 24);
            this.comboBoxTitleLine4.TabIndex = 8;
            this.comboBoxTitleLine4.SelectionChangeCommitted += new System.EventHandler(this.comboBoxTitleLine4_SelectionChangeCommitted);
            // 
            // comboBoxTitleLine5
            // 
            this.comboBoxTitleLine5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTitleLine5.FormattingEnabled = true;
            this.comboBoxTitleLine5.Location = new System.Drawing.Point(103, 151);
            this.comboBoxTitleLine5.Name = "comboBoxTitleLine5";
            this.comboBoxTitleLine5.Size = new System.Drawing.Size(433, 24);
            this.comboBoxTitleLine5.TabIndex = 9;
            this.comboBoxTitleLine5.SelectionChangeCommitted += new System.EventHandler(this.comboBoxTitleLine5_SelectionChangeCommitted);
            // 
            // ChooseColumns
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(571, 566);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChooseColumns";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Choose Columns";
            this.Load += new System.EventHandler(this.ChooseColumns_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private PhotoTagsCommonComponets.CheckedListBoxHigherRows checkedListBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox comboBoxTitleLine5;
        private System.Windows.Forms.ComboBox comboBoxTitleLine4;
        private System.Windows.Forms.ComboBox comboBoxTitleLine3;
        private System.Windows.Forms.ComboBox comboBoxTitleLine2;
        private System.Windows.Forms.ComboBox comboBoxTitleLine1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}