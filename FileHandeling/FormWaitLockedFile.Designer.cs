
namespace FileHandeling
{
    partial class FormWaitLockedFile
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWaitLockedFile));
            this.buttonRetry = new Krypton.Toolkit.KryptonButton();
            this.buttonIgnor = new Krypton.Toolkit.KryptonButton();
            this.textBox1 = new Krypton.Toolkit.KryptonTextBox();
            this.panel1 = new Krypton.Toolkit.KryptonPanel();
            this.groupBox3 = new Krypton.Toolkit.KryptonGroupBox();
            this.textBoxFilesLockedByProcess = new Krypton.Toolkit.KryptonTextBox();
            this.buttonCheck = new Krypton.Toolkit.KryptonButton();
            this.textBoxFiles = new Krypton.Toolkit.KryptonTextBox();
            this.groupBox2 = new Krypton.Toolkit.KryptonGroupBox();
            this.groupBox1 = new Krypton.Toolkit.KryptonGroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonRetry
            // 
            this.buttonRetry.Location = new System.Drawing.Point(5, 426);
            this.buttonRetry.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonRetry.Name = "buttonRetry";
            this.buttonRetry.Size = new System.Drawing.Size(131, 38);
            this.buttonRetry.TabIndex = 0;
            this.buttonRetry.Text = "Retry";
            this.buttonRetry.Click += new System.EventHandler(this.buttonRetry_Click);
            // 
            // buttonIgnor
            // 
            this.buttonIgnor.Location = new System.Drawing.Point(141, 426);
            this.buttonIgnor.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonIgnor.Name = "buttonIgnor";
            this.buttonIgnor.Size = new System.Drawing.Size(131, 38);
            this.buttonIgnor.TabIndex = 1;
            this.buttonIgnor.Text = "Ignor";
            this.buttonIgnor.Click += new System.EventHandler(this.buttonIgnor_Click);
            // 
            // textBox1
            // 
            this.textBox1.InputControlStyle = Krypton.Toolkit.InputControlStyle.Standalone;
            this.textBox1.Location = new System.Drawing.Point(8, 38);
            this.textBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(403, 144);
            this.textBox1.TabIndex = 3;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Location = new System.Drawing.Point(1, 1);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(783, 503);
            this.panel1.TabIndex = 6;
            // 
            // groupBox3
            // 
            this.groupBox3.Panel.Controls.Add(this.textBoxFilesLockedByProcess);
            this.groupBox3.Panel.Controls.Add(this.buttonCheck);
            this.groupBox3.Panel.Controls.Add(this.textBoxFiles);
            this.groupBox3.Panel.Controls.Add(this.buttonIgnor);
            this.groupBox3.Panel.Controls.Add(this.textBox1);
            this.groupBox3.Panel.Controls.Add(this.buttonRetry);
            this.groupBox3.Location = new System.Drawing.Point(3, 5);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox3.Size = new System.Drawing.Size(417, 494);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "File(s) are locked by another applications.";
            // 
            // textBoxFilesLockedByProcess
            // 
            this.textBoxFilesLockedByProcess.Location = new System.Drawing.Point(5, 351);
            this.textBoxFilesLockedByProcess.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxFilesLockedByProcess.Multiline = true;
            this.textBoxFilesLockedByProcess.Name = "textBoxFilesLockedByProcess";
            this.textBoxFilesLockedByProcess.ReadOnly = true;
            this.textBoxFilesLockedByProcess.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxFilesLockedByProcess.Size = new System.Drawing.Size(405, 68);
            this.textBoxFilesLockedByProcess.TabIndex = 6;
            // 
            // buttonCheck
            // 
            this.buttonCheck.Location = new System.Drawing.Point(280, 426);
            this.buttonCheck.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCheck.Name = "buttonCheck";
            this.buttonCheck.Size = new System.Drawing.Size(131, 38);
            this.buttonCheck.TabIndex = 5;
            this.buttonCheck.Text = "Check who lock";
            this.buttonCheck.Click += new System.EventHandler(this.buttonCheck_Click);
            // 
            // textBoxFiles
            // 
            this.textBoxFiles.Location = new System.Drawing.Point(5, 188);
            this.textBoxFiles.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxFiles.Multiline = true;
            this.textBoxFiles.Name = "textBoxFiles";
            this.textBoxFiles.ReadOnly = true;
            this.textBoxFiles.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxFiles.Size = new System.Drawing.Size(405, 157);
            this.textBoxFiles.TabIndex = 4;
            // 
            // groupBox2
            // 
            this.groupBox2.Panel.Controls.Add(this.pictureBox1);
            this.groupBox2.Location = new System.Drawing.Point(427, 247);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Size = new System.Drawing.Size(347, 252);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Backup and Sync from Google screenshot";
            // 
            // groupBox1
            // 
            this.groupBox1.Panel.Controls.Add(this.pictureBox2);
            this.groupBox1.Location = new System.Drawing.Point(427, 5);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(347, 238);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "OneDrive sync screenshot";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.ErrorImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.ErrorImage")));
            this.pictureBox1.Image = global::FileHandeling.Properties.Resources.Backup_And_Sync_From_Google_Pause;
            this.pictureBox1.Location = new System.Drawing.Point(3, 17);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(341, 233);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox2.Image = global::FileHandeling.Properties.Resources.OneDrive_Pause;
            this.pictureBox2.Location = new System.Drawing.Point(3, 17);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(341, 219);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 5;
            this.pictureBox2.TabStop = false;
            // 
            // FormWaitLockedFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(787, 516);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FormWaitLockedFile";
            this.Text = "File is been locked by another process";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormWaitLockedFile_FormClosed);
            this.Shown += new System.EventHandler(this.FormWaitLockedFile_Shown);
            this.panel1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonButton buttonRetry;
        private Krypton.Toolkit.KryptonButton buttonIgnor;
        private Krypton.Toolkit.KryptonTextBox textBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private Krypton.Toolkit.KryptonPanel panel1;
        private Krypton.Toolkit.KryptonGroupBox groupBox3;
        private Krypton.Toolkit.KryptonGroupBox groupBox2;
        private Krypton.Toolkit.KryptonGroupBox groupBox1;
        private Krypton.Toolkit.KryptonTextBox textBoxFiles;
        private Krypton.Toolkit.KryptonButton buttonCheck;
        private Krypton.Toolkit.KryptonTextBox textBoxFilesLockedByProcess;
    }
}