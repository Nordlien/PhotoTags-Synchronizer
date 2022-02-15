namespace PhotoTagsSynchronizer
{
    partial class FormDatabaseCleaner
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDatabaseCleaner));
            this.kryptonPanelMain = new Krypton.Toolkit.KryptonPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.kryptonButtonDatabaseOptimize = new Krypton.Toolkit.KryptonButton();
            this.kryptonButtonDatabaseForeignKeyCheck = new Krypton.Toolkit.KryptonButton();
            this.kryptonLabelStatusLabel = new Krypton.Toolkit.KryptonLabel();
            this.kryptonLabelStatus = new Krypton.Toolkit.KryptonLabel();
            this.kryptonButtonDatabaseCleanerExiftoolData = new Krypton.Toolkit.KryptonButton();
            this.kryptonButtonCheckDatabaseIntegrityCheck = new Krypton.Toolkit.KryptonButton();
            this.kryptonButtonDatabaseQuickCheck = new Krypton.Toolkit.KryptonButton();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelMain)).BeginInit();
            this.kryptonPanelMain.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // kryptonPanelMain
            // 
            this.kryptonPanelMain.Controls.Add(this.tableLayoutPanel1);
            this.kryptonPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanelMain.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanelMain.Name = "kryptonPanelMain";
            this.kryptonPanelMain.Size = new System.Drawing.Size(673, 315);
            this.kryptonPanelMain.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.kryptonButtonDatabaseOptimize, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.kryptonButtonDatabaseForeignKeyCheck, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.kryptonLabelStatusLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.kryptonLabelStatus, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.kryptonButtonDatabaseCleanerExiftoolData, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.kryptonButtonCheckDatabaseIntegrityCheck, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.kryptonButtonDatabaseQuickCheck, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(673, 315);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // kryptonButtonDatabaseOptimize
            // 
            this.kryptonButtonDatabaseOptimize.Location = new System.Drawing.Point(3, 287);
            this.kryptonButtonDatabaseOptimize.Name = "kryptonButtonDatabaseOptimize";
            this.kryptonButtonDatabaseOptimize.Size = new System.Drawing.Size(114, 25);
            this.kryptonButtonDatabaseOptimize.TabIndex = 7;
            this.kryptonButtonDatabaseOptimize.Values.Text = "Optimize database";
            this.kryptonButtonDatabaseOptimize.Click += new System.EventHandler(this.kryptonButtonDatabaseOptimize_Click);
            // 
            // kryptonButtonDatabaseForeignKeyCheck
            // 
            this.kryptonButtonDatabaseForeignKeyCheck.Location = new System.Drawing.Point(153, 287);
            this.kryptonButtonDatabaseForeignKeyCheck.Name = "kryptonButtonDatabaseForeignKeyCheck";
            this.kryptonButtonDatabaseForeignKeyCheck.Size = new System.Drawing.Size(114, 25);
            this.kryptonButtonDatabaseForeignKeyCheck.TabIndex = 6;
            this.kryptonButtonDatabaseForeignKeyCheck.Values.Text = "Foreign key check";
            this.kryptonButtonDatabaseForeignKeyCheck.Click += new System.EventHandler(this.kryptonButtonDatabaseForeignKeyCheck_Click);
            // 
            // kryptonLabelStatusLabel
            // 
            this.kryptonLabelStatusLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonLabelStatusLabel.Location = new System.Drawing.Point(3, 3);
            this.kryptonLabelStatusLabel.Name = "kryptonLabelStatusLabel";
            this.kryptonLabelStatusLabel.Size = new System.Drawing.Size(144, 247);
            this.kryptonLabelStatusLabel.TabIndex = 1;
            this.kryptonLabelStatusLabel.Values.Text = "Status:";
            // 
            // kryptonLabelStatus
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.kryptonLabelStatus, 2);
            this.kryptonLabelStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonLabelStatus.Location = new System.Drawing.Point(153, 3);
            this.kryptonLabelStatus.Name = "kryptonLabelStatus";
            this.kryptonLabelStatus.Size = new System.Drawing.Size(517, 247);
            this.kryptonLabelStatus.TabIndex = 2;
            this.kryptonLabelStatus.Values.Text = "Wating action...";
            // 
            // kryptonButtonDatabaseCleanerExiftoolData
            // 
            this.kryptonButtonDatabaseCleanerExiftoolData.Location = new System.Drawing.Point(3, 256);
            this.kryptonButtonDatabaseCleanerExiftoolData.Name = "kryptonButtonDatabaseCleanerExiftoolData";
            this.kryptonButtonDatabaseCleanerExiftoolData.Size = new System.Drawing.Size(114, 25);
            this.kryptonButtonDatabaseCleanerExiftoolData.TabIndex = 0;
            this.kryptonButtonDatabaseCleanerExiftoolData.Values.Text = "Clean Exifdata";
            this.kryptonButtonDatabaseCleanerExiftoolData.Click += new System.EventHandler(this.kryptonButtonDatabaseCleanerExiftoolData_Click);
            // 
            // kryptonButtonCheckDatabaseIntegrityCheck
            // 
            this.kryptonButtonCheckDatabaseIntegrityCheck.Location = new System.Drawing.Point(153, 256);
            this.kryptonButtonCheckDatabaseIntegrityCheck.Name = "kryptonButtonCheckDatabaseIntegrityCheck";
            this.kryptonButtonCheckDatabaseIntegrityCheck.Size = new System.Drawing.Size(114, 25);
            this.kryptonButtonCheckDatabaseIntegrityCheck.TabIndex = 3;
            this.kryptonButtonCheckDatabaseIntegrityCheck.Values.Text = "Integrity check";
            this.kryptonButtonCheckDatabaseIntegrityCheck.Click += new System.EventHandler(this.kryptonButtonDatabaseIntegrityCheck_Click);
            // 
            // kryptonButtonDatabaseQuickCheck
            // 
            this.kryptonButtonDatabaseQuickCheck.Location = new System.Drawing.Point(303, 256);
            this.kryptonButtonDatabaseQuickCheck.Name = "kryptonButtonDatabaseQuickCheck";
            this.kryptonButtonDatabaseQuickCheck.Size = new System.Drawing.Size(114, 25);
            this.kryptonButtonDatabaseQuickCheck.TabIndex = 4;
            this.kryptonButtonDatabaseQuickCheck.Values.Text = "Quick check";
            this.kryptonButtonDatabaseQuickCheck.Click += new System.EventHandler(this.kryptonButtonDatabaseQuickCheck_Click);
            // 
            // FormDatabaseCleaner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(673, 315);
            this.Controls.Add(this.kryptonPanelMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormDatabaseCleaner";
            this.Text = "DatabaseCleaner";
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelMain)).EndInit();
            this.kryptonPanelMain.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonPanel kryptonPanelMain;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Krypton.Toolkit.KryptonButton kryptonButtonDatabaseCleanerExiftoolData;
        private Krypton.Toolkit.KryptonLabel kryptonLabelStatusLabel;
        private Krypton.Toolkit.KryptonLabel kryptonLabelStatus;
        private Krypton.Toolkit.KryptonButton kryptonButtonCheckDatabaseIntegrityCheck;
        private Krypton.Toolkit.KryptonButton kryptonButtonDatabaseQuickCheck;
        private Krypton.Toolkit.KryptonButton kryptonButtonDatabaseOptimize;
        private Krypton.Toolkit.KryptonButton kryptonButtonDatabaseForeignKeyCheck;
    }
}