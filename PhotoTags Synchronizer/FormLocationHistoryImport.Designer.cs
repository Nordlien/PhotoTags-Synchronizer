namespace PhotoTagsSynchronizer
{
    partial class FormLocationHistoryImport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLocationHistoryImport));
            this.buttonImportLocationHistory = new Krypton.Toolkit.KryptonButton();
            this.comboBoxUserAccount = new Krypton.Toolkit.KryptonComboBox();
            this.label2 = new Krypton.Toolkit.KryptonLabel();
            this.statusStripStatus = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBarprogressBarLoading = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabelStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.kryptonLinkLabel1 = new Krypton.Toolkit.KryptonLinkLabel();
            this.kryptonLabel1 = new Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel2 = new Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel3 = new Krypton.Toolkit.KryptonLabel();
            this.kryptonPanel1 = new Krypton.Toolkit.KryptonPanel();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxUserAccount)).BeginInit();
            this.statusStripStatus.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonImportLocationHistory
            // 
            this.buttonImportLocationHistory.Location = new System.Drawing.Point(124, 76);
            this.buttonImportLocationHistory.Name = "buttonImportLocationHistory";
            this.buttonImportLocationHistory.Size = new System.Drawing.Size(244, 28);
            this.buttonImportLocationHistory.TabIndex = 1;
            this.buttonImportLocationHistory.Values.Text = "Select file and Import";
            this.buttonImportLocationHistory.Click += new System.EventHandler(this.buttonImportLocationHistory_Click);
            // 
            // comboBoxUserAccount
            // 
            this.comboBoxUserAccount.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxUserAccount.DropDownWidth = 244;
            this.comboBoxUserAccount.FormattingEnabled = true;
            this.comboBoxUserAccount.IntegralHeight = false;
            this.comboBoxUserAccount.Location = new System.Drawing.Point(124, 49);
            this.comboBoxUserAccount.Name = "comboBoxUserAccount";
            this.comboBoxUserAccount.Size = new System.Drawing.Size(244, 21);
            this.comboBoxUserAccount.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxUserAccount.TabIndex = 0;
            this.comboBoxUserAccount.SelectionChangeCommitted += new System.EventHandler(this.comboBoxUserAccount_SelectionChangeCommitted);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(124, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(256, 20);
            this.label2.TabIndex = 2;
            this.label2.Values.Text = "Enter the name of owner the location history.";
            // 
            // statusStripStatus
            // 
            this.statusStripStatus.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStripStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBarprogressBarLoading,
            this.toolStripStatusLabelStatus});
            this.statusStripStatus.Location = new System.Drawing.Point(0, 159);
            this.statusStripStatus.Name = "statusStripStatus";
            this.statusStripStatus.Size = new System.Drawing.Size(579, 24);
            this.statusStripStatus.TabIndex = 5;
            this.statusStripStatus.Text = "statusStrip1";
            // 
            // toolStripProgressBarprogressBarLoading
            // 
            this.toolStripProgressBarprogressBarLoading.Name = "toolStripProgressBarprogressBarLoading";
            this.toolStripProgressBarprogressBarLoading.Size = new System.Drawing.Size(300, 18);
            // 
            // toolStripStatusLabelStatus
            // 
            this.toolStripStatusLabelStatus.Name = "toolStripStatusLabelStatus";
            this.toolStripStatusLabelStatus.Size = new System.Drawing.Size(115, 19);
            this.toolStripStatusLabelStatus.Text = "Waiting command...";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.buttonImportLocationHistory, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.kryptonLinkLabel1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.kryptonLabel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.kryptonLabel2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.kryptonLabel3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxUserAccount, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(579, 159);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // kryptonLinkLabel1
            // 
            this.kryptonLinkLabel1.Location = new System.Drawing.Point(124, 3);
            this.kryptonLinkLabel1.Name = "kryptonLinkLabel1";
            this.kryptonLinkLabel1.Size = new System.Drawing.Size(164, 14);
            this.kryptonLinkLabel1.TabIndex = 4;
            this.kryptonLinkLabel1.Values.Text = "https://takeout.google.com/";
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(3, 3);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(115, 14);
            this.kryptonLabel1.TabIndex = 5;
            this.kryptonLabel1.Values.Text = "1. Export (example)";
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.Location = new System.Drawing.Point(3, 49);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(81, 20);
            this.kryptonLabel2.TabIndex = 6;
            this.kryptonLabel2.Values.Text = "2. Give name";
            // 
            // kryptonLabel3
            // 
            this.kryptonLabel3.Location = new System.Drawing.Point(3, 76);
            this.kryptonLabel3.Name = "kryptonLabel3";
            this.kryptonLabel3.Size = new System.Drawing.Size(60, 20);
            this.kryptonLabel3.TabIndex = 7;
            this.kryptonLabel3.Values.Text = "3. Import";
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.tableLayoutPanel1);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(579, 159);
            this.kryptonPanel1.TabIndex = 7;
            // 
            // FormLocationHistoryImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(579, 183);
            this.Controls.Add(this.kryptonPanel1);
            this.Controls.Add(this.statusStripStatus);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormLocationHistoryImport";
            this.Text = "Import Location Histotory";
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxUserAccount)).EndInit();
            this.statusStripStatus.ResumeLayout(false);
            this.statusStripStatus.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Krypton.Toolkit.KryptonButton buttonImportLocationHistory;
        private Krypton.Toolkit.KryptonComboBox comboBoxUserAccount;
        private Krypton.Toolkit.KryptonLabel label2;
        private System.Windows.Forms.StatusStrip statusStripStatus;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBarprogressBarLoading;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelStatus;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Krypton.Toolkit.KryptonLinkLabel kryptonLinkLabel1;
        private Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private Krypton.Toolkit.KryptonLabel kryptonLabel2;
        private Krypton.Toolkit.KryptonLabel kryptonLabel3;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
    }
}