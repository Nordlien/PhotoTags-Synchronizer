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
            this.kryptonButtonDatabaseCleanerExiftoolData = new Krypton.Toolkit.KryptonButton();
            this.kryptonLabelStatusLabel = new Krypton.Toolkit.KryptonLabel();
            this.kryptonLabelStatus = new Krypton.Toolkit.KryptonLabel();
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
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.kryptonLabelStatusLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.kryptonLabelStatus, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.kryptonButtonDatabaseCleanerExiftoolData, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(673, 315);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // kryptonButtonDatabaseCleanerExiftoolData
            // 
            this.kryptonButtonDatabaseCleanerExiftoolData.Location = new System.Drawing.Point(339, 160);
            this.kryptonButtonDatabaseCleanerExiftoolData.Name = "kryptonButtonDatabaseCleanerExiftoolData";
            this.kryptonButtonDatabaseCleanerExiftoolData.Size = new System.Drawing.Size(90, 25);
            this.kryptonButtonDatabaseCleanerExiftoolData.TabIndex = 0;
            this.kryptonButtonDatabaseCleanerExiftoolData.Values.Text = "Clean Exifdata";
            this.kryptonButtonDatabaseCleanerExiftoolData.Click += new System.EventHandler(this.kryptonButtonDatabaseCleanerExiftoolData_Click);
            // 
            // kryptonLabelStatusLabel
            // 
            this.kryptonLabelStatusLabel.Location = new System.Drawing.Point(3, 3);
            this.kryptonLabelStatusLabel.Name = "kryptonLabelStatusLabel";
            this.kryptonLabelStatusLabel.Size = new System.Drawing.Size(45, 18);
            this.kryptonLabelStatusLabel.TabIndex = 1;
            this.kryptonLabelStatusLabel.Values.Text = "Status:";
            // 
            // kryptonLabelStatus
            // 
            this.kryptonLabelStatus.Location = new System.Drawing.Point(339, 3);
            this.kryptonLabelStatus.Name = "kryptonLabelStatus";
            this.kryptonLabelStatus.Size = new System.Drawing.Size(88, 18);
            this.kryptonLabelStatus.TabIndex = 2;
            this.kryptonLabelStatus.Values.Text = "Wating action...";
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
    }
}