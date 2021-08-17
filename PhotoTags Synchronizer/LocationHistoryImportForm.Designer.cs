namespace PhotoTagsSynchronizer
{
    partial class LocationHistoryImportForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LocationHistoryImportForm));
            this.buttonImportLocationHistory = new System.Windows.Forms.Button();
            this.label1 = new Krypton.Toolkit.KryptonLabel();
            this.comboBoxUserAccount = new Krypton.Toolkit.KryptonComboBox();
            this.label2 = new Krypton.Toolkit.KryptonLabel();
            this.statusStripStatus = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBarprogressBarLoading = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabelStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.kryptonManager1 = new Krypton.Toolkit.KryptonManager(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxUserAccount)).BeginInit();
            this.statusStripStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonImportLocationHistory
            // 
            this.buttonImportLocationHistory.Location = new System.Drawing.Point(81, 95);
            this.buttonImportLocationHistory.Name = "buttonImportLocationHistory";
            this.buttonImportLocationHistory.Size = new System.Drawing.Size(244, 28);
            this.buttonImportLocationHistory.TabIndex = 1;
            this.buttonImportLocationHistory.Text = "Select file and Import";
            this.buttonImportLocationHistory.UseVisualStyleBackColor = true;
            this.buttonImportLocationHistory.Click += new System.EventHandler(this.buttonImportLocationHistory_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(19, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 24);
            this.label1.TabIndex = 3;
            this.label1.Values.Text = "Name:";
            // 
            // comboBoxUserAccount
            // 
            this.comboBoxUserAccount.DropDownWidth = 244;
            this.comboBoxUserAccount.FormattingEnabled = true;
            this.comboBoxUserAccount.Location = new System.Drawing.Point(81, 33);
            this.comboBoxUserAccount.Name = "comboBoxUserAccount";
            this.comboBoxUserAccount.Size = new System.Drawing.Size(244, 25);
            this.comboBoxUserAccount.TabIndex = 0;
            this.comboBoxUserAccount.SelectionChangeCommitted += new System.EventHandler(this.comboBoxUserAccount_SelectionChangeCommitted);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(81, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(440, 24);
            this.label2.TabIndex = 2;
            this.label2.Values.Text = "Enter name for the user you want to import location history for";
            // 
            // statusStripStatus
            // 
            this.statusStripStatus.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStripStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBarprogressBarLoading,
            this.toolStripStatusLabelStatus});
            this.statusStripStatus.Location = new System.Drawing.Point(0, 157);
            this.statusStripStatus.Name = "statusStripStatus";
            this.statusStripStatus.Size = new System.Drawing.Size(579, 26);
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
            this.toolStripStatusLabelStatus.Size = new System.Drawing.Size(140, 20);
            this.toolStripStatusLabelStatus.Text = "Waiting command...";
            // 
            // LocationHistoryImportForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(579, 183);
            this.Controls.Add(this.statusStripStatus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxUserAccount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonImportLocationHistory);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LocationHistoryImportForm";
            this.Text = "Import Location Histotory";
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxUserAccount)).EndInit();
            this.statusStripStatus.ResumeLayout(false);
            this.statusStripStatus.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonImportLocationHistory;
        private Krypton.Toolkit.KryptonLabel label1;
        private Krypton.Toolkit.KryptonComboBox comboBoxUserAccount;
        private Krypton.Toolkit.KryptonLabel label2;
        private System.Windows.Forms.StatusStrip statusStripStatus;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBarprogressBarLoading;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelStatus;
        private Krypton.Toolkit.KryptonManager kryptonManager1;
    }
}