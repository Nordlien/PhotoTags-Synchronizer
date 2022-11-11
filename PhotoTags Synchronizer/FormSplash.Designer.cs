namespace PhotoTagsSynchronizer
{
    partial class FormSplash
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
            this.labelStatus = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.textBoxWarning = new System.Windows.Forms.TextBox();
            this.labelWarnings = new System.Windows.Forms.Label();
            this.checkBoxCloseWarning = new System.Windows.Forms.CheckBox();
            this.tabControlMessages = new System.Windows.Forms.TabControl();
            this.tabPageWarning = new System.Windows.Forms.TabPage();
            this.tabPageKeepYourTags = new System.Windows.Forms.TabPage();
            this.pictureBoxWhereBelongs = new System.Windows.Forms.PictureBox();
            this.tabPageInternetAccess = new System.Windows.Forms.TabPage();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.tabPageDelayReading = new System.Windows.Forms.TabPage();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.tabPageImportLocation = new System.Windows.Forms.TabPage();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.linkLabelHomepage = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControlMessages.SuspendLayout();
            this.tabPageWarning.SuspendLayout();
            this.tabPageKeepYourTags.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWhereBelongs)).BeginInit();
            this.tabPageInternetAccess.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.tabPageDelayReading.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.tabPageImportLocation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(92, 54);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(70, 13);
            this.labelStatus.TabIndex = 0;
            this.labelStatus.Text = "Processing...";
            this.labelStatus.UseWaitCursor = true;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(92, 73);
            this.progressBar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(399, 23);
            this.progressBar.TabIndex = 1;
            this.progressBar.UseWaitCursor = true;
            // 
            // textBoxWarning
            // 
            this.textBoxWarning.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxWarning.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBoxWarning.ForeColor = System.Drawing.Color.White;
            this.textBoxWarning.Location = new System.Drawing.Point(3, 25);
            this.textBoxWarning.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxWarning.Multiline = true;
            this.textBoxWarning.Name = "textBoxWarning";
            this.textBoxWarning.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxWarning.Size = new System.Drawing.Size(573, 121);
            this.textBoxWarning.TabIndex = 2;
            this.textBoxWarning.Visible = false;
            // 
            // labelWarnings
            // 
            this.labelWarnings.Location = new System.Drawing.Point(7, 3);
            this.labelWarnings.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelWarnings.Name = "labelWarnings";
            this.labelWarnings.Size = new System.Drawing.Size(64, 20);
            this.labelWarnings.TabIndex = 3;
            this.labelWarnings.Text = "Warnings:";
            this.labelWarnings.Visible = false;
            // 
            // checkBoxCloseWarning
            // 
            this.checkBoxCloseWarning.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxCloseWarning.Location = new System.Drawing.Point(95, 26);
            this.checkBoxCloseWarning.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxCloseWarning.Name = "checkBoxCloseWarning";
            this.checkBoxCloseWarning.Size = new System.Drawing.Size(231, 24);
            this.checkBoxCloseWarning.TabIndex = 4;
            this.checkBoxCloseWarning.Text = "Close warning window automatically";
            this.checkBoxCloseWarning.Visible = false;
            this.checkBoxCloseWarning.CheckedChanged += new System.EventHandler(this.checkBoxCloseWarning_CheckedChanged);
            // 
            // tabControlMessages
            // 
            this.tabControlMessages.Controls.Add(this.tabPageWarning);
            this.tabControlMessages.Controls.Add(this.tabPageKeepYourTags);
            this.tabControlMessages.Controls.Add(this.tabPageInternetAccess);
            this.tabControlMessages.Controls.Add(this.tabPageDelayReading);
            this.tabControlMessages.Controls.Add(this.tabPageImportLocation);
            this.tabControlMessages.Controls.Add(this.tabPage1);
            this.tabControlMessages.Location = new System.Drawing.Point(2, 97);
            this.tabControlMessages.Name = "tabControlMessages";
            this.tabControlMessages.SelectedIndex = 0;
            this.tabControlMessages.Size = new System.Drawing.Size(594, 182);
            this.tabControlMessages.TabIndex = 6;
            // 
            // tabPageWarning
            // 
            this.tabPageWarning.Controls.Add(this.labelWarnings);
            this.tabPageWarning.Controls.Add(this.textBoxWarning);
            this.tabPageWarning.Location = new System.Drawing.Point(4, 22);
            this.tabPageWarning.Name = "tabPageWarning";
            this.tabPageWarning.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageWarning.Size = new System.Drawing.Size(586, 156);
            this.tabPageWarning.TabIndex = 0;
            this.tabPageWarning.Text = "Warning";
            this.tabPageWarning.UseVisualStyleBackColor = true;
            // 
            // tabPageKeepYourTags
            // 
            this.tabPageKeepYourTags.Controls.Add(this.pictureBoxWhereBelongs);
            this.tabPageKeepYourTags.Location = new System.Drawing.Point(4, 22);
            this.tabPageKeepYourTags.Name = "tabPageKeepYourTags";
            this.tabPageKeepYourTags.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageKeepYourTags.Size = new System.Drawing.Size(586, 156);
            this.tabPageKeepYourTags.TabIndex = 1;
            this.tabPageKeepYourTags.Text = "KeepYourTags";
            this.tabPageKeepYourTags.UseVisualStyleBackColor = true;
            // 
            // pictureBoxWhereBelongs
            // 
            this.pictureBoxWhereBelongs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxWhereBelongs.Image = global::PhotoTagsSynchronizer.Properties.Resources.Hint_KeepWhereBelongs;
            this.pictureBoxWhereBelongs.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxWhereBelongs.Name = "pictureBoxWhereBelongs";
            this.pictureBoxWhereBelongs.Size = new System.Drawing.Size(580, 150);
            this.pictureBoxWhereBelongs.TabIndex = 0;
            this.pictureBoxWhereBelongs.TabStop = false;
            // 
            // tabPageInternetAccess
            // 
            this.tabPageInternetAccess.Controls.Add(this.pictureBox2);
            this.tabPageInternetAccess.Location = new System.Drawing.Point(4, 22);
            this.tabPageInternetAccess.Name = "tabPageInternetAccess";
            this.tabPageInternetAccess.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageInternetAccess.Size = new System.Drawing.Size(586, 156);
            this.tabPageInternetAccess.TabIndex = 2;
            this.tabPageInternetAccess.Text = "InternetAccess";
            this.tabPageInternetAccess.UseVisualStyleBackColor = true;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox2.Image = global::PhotoTagsSynchronizer.Properties.Resources.Hint_GiveInternetAccess;
            this.pictureBox2.Location = new System.Drawing.Point(3, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(580, 150);
            this.pictureBox2.TabIndex = 0;
            this.pictureBox2.TabStop = false;
            // 
            // tabPageDelayReading
            // 
            this.tabPageDelayReading.Controls.Add(this.pictureBox3);
            this.tabPageDelayReading.Location = new System.Drawing.Point(4, 22);
            this.tabPageDelayReading.Name = "tabPageDelayReading";
            this.tabPageDelayReading.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDelayReading.Size = new System.Drawing.Size(586, 156);
            this.tabPageDelayReading.TabIndex = 3;
            this.tabPageDelayReading.Text = "DelayReading";
            this.tabPageDelayReading.UseVisualStyleBackColor = true;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox3.Image = global::PhotoTagsSynchronizer.Properties.Resources.Hint_DelayedLoadReload;
            this.pictureBox3.Location = new System.Drawing.Point(3, 3);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(580, 150);
            this.pictureBox3.TabIndex = 0;
            this.pictureBox3.TabStop = false;
            // 
            // tabPageImportLocation
            // 
            this.tabPageImportLocation.Controls.Add(this.pictureBox4);
            this.tabPageImportLocation.Location = new System.Drawing.Point(4, 22);
            this.tabPageImportLocation.Name = "tabPageImportLocation";
            this.tabPageImportLocation.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageImportLocation.Size = new System.Drawing.Size(586, 156);
            this.tabPageImportLocation.TabIndex = 4;
            this.tabPageImportLocation.Text = "ImportLocation";
            this.tabPageImportLocation.UseVisualStyleBackColor = true;
            // 
            // pictureBox4
            // 
            this.pictureBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox4.Image = global::PhotoTagsSynchronizer.Properties.Resources.Hint_ImportGPSLocations;
            this.pictureBox4.Location = new System.Drawing.Point(3, 3);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(580, 150);
            this.pictureBox4.TabIndex = 1;
            this.pictureBox4.TabStop = false;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.pictureBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(586, 156);
            this.tabPage1.TabIndex = 5;
            this.tabPage1.Text = "tabPageHintCloud";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(6, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(568, 135);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // linkLabelHomepage
            // 
            this.linkLabelHomepage.AutoSize = true;
            this.linkLabelHomepage.Location = new System.Drawing.Point(238, 9);
            this.linkLabelHomepage.Name = "linkLabelHomepage";
            this.linkLabelHomepage.Size = new System.Drawing.Size(253, 13);
            this.linkLabelHomepage.TabIndex = 7;
            this.linkLabelHomepage.TabStop = true;
            this.linkLabelHomepage.Text = "https://nordlien.github.io/PhotoTags-Synchronizer/";
            this.linkLabelHomepage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelHomepage_LinkClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(92, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Need help? Open link:";
            // 
            // FormSplash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(599, 277);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.linkLabelHomepage);
            this.Controls.Add(this.tabControlMessages);
            this.Controls.Add(this.checkBoxCloseWarning);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.labelStatus);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.HelpButton = true;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.Name = "FormSplash";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PhotoTags Synchronizer...";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SplashForm_FormClosing);
            this.tabControlMessages.ResumeLayout(false);
            this.tabPageWarning.ResumeLayout(false);
            this.tabPageWarning.PerformLayout();
            this.tabPageKeepYourTags.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWhereBelongs)).EndInit();
            this.tabPageInternetAccess.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.tabPageDelayReading.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.tabPageImportLocation.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.TextBox textBoxWarning;
        private System.Windows.Forms.Label labelWarnings;
        private System.Windows.Forms.CheckBox checkBoxCloseWarning;
        private System.Windows.Forms.TabControl tabControlMessages;
        private System.Windows.Forms.TabPage tabPageWarning;
        private System.Windows.Forms.TabPage tabPageKeepYourTags;
        private System.Windows.Forms.TabPage tabPageInternetAccess;
        private System.Windows.Forms.TabPage tabPageDelayReading;
        private System.Windows.Forms.TabPage tabPageImportLocation;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBoxWhereBelongs;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.LinkLabel linkLabelHomepage;
        private System.Windows.Forms.Label label1;
    }
}