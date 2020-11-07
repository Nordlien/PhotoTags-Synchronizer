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
            this.buttonImportLocationHistory = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxUserAccount = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.statusStripStatus = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBarprogressBarLoading = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabelStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStripStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonImportLocationHistory
            // 
            this.buttonImportLocationHistory.Location = new System.Drawing.Point(81, 60);
            this.buttonImportLocationHistory.Name = "buttonImportLocationHistory";
            this.buttonImportLocationHistory.Size = new System.Drawing.Size(244, 28);
            this.buttonImportLocationHistory.TabIndex = 0;
            this.buttonImportLocationHistory.Text = "Select file and Import";
            this.buttonImportLocationHistory.UseVisualStyleBackColor = true;
            this.buttonImportLocationHistory.Click += new System.EventHandler(this.buttonImportLocationHistory_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name:";
            // 
            // comboBoxUserAccount
            // 
            this.comboBoxUserAccount.FormattingEnabled = true;
            this.comboBoxUserAccount.Location = new System.Drawing.Point(81, 33);
            this.comboBoxUserAccount.Name = "comboBoxUserAccount";
            this.comboBoxUserAccount.Size = new System.Drawing.Size(244, 24);
            this.comboBoxUserAccount.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(342, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(397, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Enter name for the user you want to import location history for";
            // 
            // statusStripStatus
            // 
            this.statusStripStatus.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStripStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBarprogressBarLoading,
            this.toolStripStatusLabelStatus});
            this.statusStripStatus.Location = new System.Drawing.Point(0, 185);
            this.statusStripStatus.Name = "statusStripStatus";
            this.statusStripStatus.Size = new System.Drawing.Size(771, 26);
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
            this.ClientSize = new System.Drawing.Size(771, 211);
            this.Controls.Add(this.statusStripStatus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxUserAccount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonImportLocationHistory);
            this.Name = "LocationHistoryImportForm";
            this.Text = "Import Location Histotory";
            this.statusStripStatus.ResumeLayout(false);
            this.statusStripStatus.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonImportLocationHistory;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxUserAccount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.StatusStrip statusStripStatus;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBarprogressBarLoading;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelStatus;
        
    }
}