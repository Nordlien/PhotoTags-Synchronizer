namespace PhotoTagsSynchronizer
{
    partial class RunCommand
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RunCommand));
            this.panelMain = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageOpenWith = new System.Windows.Forms.TabPage();
            this.checkBoxOpenWithWaitForExit = new System.Windows.Forms.CheckBox();
            this.dataGridViewVideos = new System.Windows.Forms.DataGridView();
            this.dataGridViewPictures = new System.Windows.Forms.DataGridView();
            this.buttonOpenWithOpenWith = new System.Windows.Forms.Button();
            this.textBoxOpenWithSelectedFiles = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPageBatchCommand = new System.Windows.Forms.TabPage();
            this.checkBoxBatchCommandWaitForExit = new System.Windows.Forms.CheckBox();
            this.textBoxRunCommandExamples = new System.Windows.Forms.TextBox();
            this.textBoxCommandCommandExample = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxBatchCommandCommand = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.buttonBatchCommandBatchRun = new System.Windows.Forms.Button();
            this.comboBoxBatchCommandCommandVariables = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxBatchCommandSelectedFiles = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPageArgumentFile = new System.Windows.Forms.TabPage();
            this.buttonArgumentFileRun = new System.Windows.Forms.Button();
            this.textBoxArgumentFileCommand = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxArgumentFileCommandVariables = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxArgumentFileArgumentFile = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBoxArgumentWaitForExit = new System.Windows.Forms.CheckBox();
            this.panelMain.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageOpenWith.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewVideos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPictures)).BeginInit();
            this.tabPageBatchCommand.SuspendLayout();
            this.tabPageArgumentFile.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.tabControl1);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(838, 554);
            this.panelMain.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageOpenWith);
            this.tabControl1.Controls.Add(this.tabPageBatchCommand);
            this.tabControl1.Controls.Add(this.tabPageArgumentFile);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(838, 554);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageOpenWith
            // 
            this.tabPageOpenWith.Controls.Add(this.checkBoxOpenWithWaitForExit);
            this.tabPageOpenWith.Controls.Add(this.dataGridViewVideos);
            this.tabPageOpenWith.Controls.Add(this.dataGridViewPictures);
            this.tabPageOpenWith.Controls.Add(this.buttonOpenWithOpenWith);
            this.tabPageOpenWith.Controls.Add(this.textBoxOpenWithSelectedFiles);
            this.tabPageOpenWith.Controls.Add(this.label1);
            this.tabPageOpenWith.Location = new System.Drawing.Point(4, 25);
            this.tabPageOpenWith.Name = "tabPageOpenWith";
            this.tabPageOpenWith.Size = new System.Drawing.Size(830, 525);
            this.tabPageOpenWith.TabIndex = 2;
            this.tabPageOpenWith.Text = "Open with...";
            this.tabPageOpenWith.UseVisualStyleBackColor = true;
            // 
            // checkBoxOpenWithWaitForExit
            // 
            this.checkBoxOpenWithWaitForExit.AutoSize = true;
            this.checkBoxOpenWithWaitForExit.Location = new System.Drawing.Point(9, 126);
            this.checkBoxOpenWithWaitForExit.Name = "checkBoxOpenWithWaitForExit";
            this.checkBoxOpenWithWaitForExit.Size = new System.Drawing.Size(273, 21);
            this.checkBoxOpenWithWaitForExit.TabIndex = 20;
            this.checkBoxOpenWithWaitForExit.Text = "Wait for exit before run next command.";
            this.checkBoxOpenWithWaitForExit.UseVisualStyleBackColor = true;
            // 
            // dataGridViewVideos
            // 
            this.dataGridViewVideos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewVideos.Location = new System.Drawing.Point(8, 307);
            this.dataGridViewVideos.Name = "dataGridViewVideos";
            this.dataGridViewVideos.RowHeadersWidth = 51;
            this.dataGridViewVideos.RowTemplate.Height = 24;
            this.dataGridViewVideos.Size = new System.Drawing.Size(811, 125);
            this.dataGridViewVideos.TabIndex = 15;
            // 
            // dataGridViewPictures
            // 
            this.dataGridViewPictures.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPictures.Location = new System.Drawing.Point(8, 154);
            this.dataGridViewPictures.Name = "dataGridViewPictures";
            this.dataGridViewPictures.RowHeadersWidth = 51;
            this.dataGridViewPictures.RowTemplate.Height = 24;
            this.dataGridViewPictures.Size = new System.Drawing.Size(811, 125);
            this.dataGridViewPictures.TabIndex = 14;
            // 
            // buttonOpenWithOpenWith
            // 
            this.buttonOpenWithOpenWith.Location = new System.Drawing.Point(683, 487);
            this.buttonOpenWithOpenWith.Name = "buttonOpenWithOpenWith";
            this.buttonOpenWithOpenWith.Size = new System.Drawing.Size(139, 30);
            this.buttonOpenWithOpenWith.TabIndex = 13;
            this.buttonOpenWithOpenWith.Text = "Open with";
            this.buttonOpenWithOpenWith.UseVisualStyleBackColor = true;
            this.buttonOpenWithOpenWith.Click += new System.EventHandler(this.buttonOpenWithOpenWith_Click);
            // 
            // textBoxOpenWithSelectedFiles
            // 
            this.textBoxOpenWithSelectedFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOpenWithSelectedFiles.Location = new System.Drawing.Point(9, 35);
            this.textBoxOpenWithSelectedFiles.Multiline = true;
            this.textBoxOpenWithSelectedFiles.Name = "textBoxOpenWithSelectedFiles";
            this.textBoxOpenWithSelectedFiles.ReadOnly = true;
            this.textBoxOpenWithSelectedFiles.Size = new System.Drawing.Size(811, 85);
            this.textBoxOpenWithSelectedFiles.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Selected files:";
            // 
            // tabPageBatchCommand
            // 
            this.tabPageBatchCommand.Controls.Add(this.checkBoxBatchCommandWaitForExit);
            this.tabPageBatchCommand.Controls.Add(this.textBoxRunCommandExamples);
            this.tabPageBatchCommand.Controls.Add(this.textBoxCommandCommandExample);
            this.tabPageBatchCommand.Controls.Add(this.label8);
            this.tabPageBatchCommand.Controls.Add(this.textBoxBatchCommandCommand);
            this.tabPageBatchCommand.Controls.Add(this.label7);
            this.tabPageBatchCommand.Controls.Add(this.buttonBatchCommandBatchRun);
            this.tabPageBatchCommand.Controls.Add(this.comboBoxBatchCommandCommandVariables);
            this.tabPageBatchCommand.Controls.Add(this.label3);
            this.tabPageBatchCommand.Controls.Add(this.textBoxBatchCommandSelectedFiles);
            this.tabPageBatchCommand.Controls.Add(this.label2);
            this.tabPageBatchCommand.Location = new System.Drawing.Point(4, 25);
            this.tabPageBatchCommand.Name = "tabPageBatchCommand";
            this.tabPageBatchCommand.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBatchCommand.Size = new System.Drawing.Size(830, 525);
            this.tabPageBatchCommand.TabIndex = 0;
            this.tabPageBatchCommand.Text = "Batch command...";
            this.tabPageBatchCommand.UseVisualStyleBackColor = true;
            // 
            // checkBoxBatchCommandWaitForExit
            // 
            this.checkBoxBatchCommandWaitForExit.AutoSize = true;
            this.checkBoxBatchCommandWaitForExit.Location = new System.Drawing.Point(85, 186);
            this.checkBoxBatchCommandWaitForExit.Name = "checkBoxBatchCommandWaitForExit";
            this.checkBoxBatchCommandWaitForExit.Size = new System.Drawing.Size(273, 21);
            this.checkBoxBatchCommandWaitForExit.TabIndex = 19;
            this.checkBoxBatchCommandWaitForExit.Text = "Wait for exit before run next command.";
            this.checkBoxBatchCommandWaitForExit.UseVisualStyleBackColor = true;
            // 
            // textBoxRunCommandExamples
            // 
            this.textBoxRunCommandExamples.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxRunCommandExamples.Location = new System.Drawing.Point(11, 408);
            this.textBoxRunCommandExamples.Multiline = true;
            this.textBoxRunCommandExamples.Name = "textBoxRunCommandExamples";
            this.textBoxRunCommandExamples.ReadOnly = true;
            this.textBoxRunCommandExamples.Size = new System.Drawing.Size(811, 73);
            this.textBoxRunCommandExamples.TabIndex = 18;
            this.textBoxRunCommandExamples.Text = resources.GetString("textBoxRunCommandExamples.Text");
            // 
            // textBoxCommandCommandExample
            // 
            this.textBoxCommandCommandExample.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCommandCommandExample.Location = new System.Drawing.Point(11, 230);
            this.textBoxCommandCommandExample.Multiline = true;
            this.textBoxCommandCommandExample.Name = "textBoxCommandCommandExample";
            this.textBoxCommandCommandExample.ReadOnly = true;
            this.textBoxCommandCommandExample.Size = new System.Drawing.Size(811, 159);
            this.textBoxCommandCommandExample.TabIndex = 17;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 210);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(149, 17);
            this.label8.TabIndex = 16;
            this.label8.Text = "Command lines to run:";
            // 
            // textBoxBatchCommandCommand
            // 
            this.textBoxBatchCommandCommand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBatchCommandCommand.Location = new System.Drawing.Point(85, 159);
            this.textBoxBatchCommandCommand.Name = "textBoxBatchCommandCommand";
            this.textBoxBatchCommandCommand.Size = new System.Drawing.Size(735, 22);
            this.textBoxBatchCommandCommand.TabIndex = 15;
            this.textBoxBatchCommandCommand.TextChanged += new System.EventHandler(this.textBoxBatchCommandCommand_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 162);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 17);
            this.label7.TabIndex = 14;
            this.label7.Text = "Command:";
            // 
            // buttonBatchCommandBatchRun
            // 
            this.buttonBatchCommandBatchRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBatchCommandBatchRun.Location = new System.Drawing.Point(681, 487);
            this.buttonBatchCommandBatchRun.Name = "buttonBatchCommandBatchRun";
            this.buttonBatchCommandBatchRun.Size = new System.Drawing.Size(139, 30);
            this.buttonBatchCommandBatchRun.TabIndex = 13;
            this.buttonBatchCommandBatchRun.Text = "Batch run";
            this.buttonBatchCommandBatchRun.UseVisualStyleBackColor = true;
            this.buttonBatchCommandBatchRun.Click += new System.EventHandler(this.buttonBatchCommandBatchRun_Click);
            // 
            // comboBoxBatchCommandCommandVariables
            // 
            this.comboBoxBatchCommandCommandVariables.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxBatchCommandCommandVariables.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBatchCommandCommandVariables.FormattingEnabled = true;
            this.comboBoxBatchCommandCommandVariables.Location = new System.Drawing.Point(85, 129);
            this.comboBoxBatchCommandCommandVariables.Name = "comboBoxBatchCommandCommandVariables";
            this.comboBoxBatchCommandCommandVariables.Size = new System.Drawing.Size(735, 24);
            this.comboBoxBatchCommandCommandVariables.TabIndex = 5;
            this.comboBoxBatchCommandCommandVariables.SelectionChangeCommitted += new System.EventHandler(this.comboBoxBatchCommandCommandVariables_SelectionChangeCommitted);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 132);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Variables:";
            // 
            // textBoxBatchCommandSelectedFiles
            // 
            this.textBoxBatchCommandSelectedFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBatchCommandSelectedFiles.Location = new System.Drawing.Point(9, 35);
            this.textBoxBatchCommandSelectedFiles.Multiline = true;
            this.textBoxBatchCommandSelectedFiles.Name = "textBoxBatchCommandSelectedFiles";
            this.textBoxBatchCommandSelectedFiles.ReadOnly = true;
            this.textBoxBatchCommandSelectedFiles.Size = new System.Drawing.Size(811, 85);
            this.textBoxBatchCommandSelectedFiles.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Selected files:";
            // 
            // tabPageArgumentFile
            // 
            this.tabPageArgumentFile.Controls.Add(this.checkBoxArgumentWaitForExit);
            this.tabPageArgumentFile.Controls.Add(this.buttonArgumentFileRun);
            this.tabPageArgumentFile.Controls.Add(this.textBoxArgumentFileCommand);
            this.tabPageArgumentFile.Controls.Add(this.label6);
            this.tabPageArgumentFile.Controls.Add(this.comboBoxArgumentFileCommandVariables);
            this.tabPageArgumentFile.Controls.Add(this.label4);
            this.tabPageArgumentFile.Controls.Add(this.textBoxArgumentFileArgumentFile);
            this.tabPageArgumentFile.Controls.Add(this.label5);
            this.tabPageArgumentFile.Location = new System.Drawing.Point(4, 25);
            this.tabPageArgumentFile.Name = "tabPageArgumentFile";
            this.tabPageArgumentFile.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageArgumentFile.Size = new System.Drawing.Size(830, 525);
            this.tabPageArgumentFile.TabIndex = 1;
            this.tabPageArgumentFile.Text = "Argument file";
            this.tabPageArgumentFile.UseVisualStyleBackColor = true;
            // 
            // buttonArgumentFileRun
            // 
            this.buttonArgumentFileRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonArgumentFileRun.Location = new System.Drawing.Point(683, 487);
            this.buttonArgumentFileRun.Name = "buttonArgumentFileRun";
            this.buttonArgumentFileRun.Size = new System.Drawing.Size(139, 30);
            this.buttonArgumentFileRun.TabIndex = 12;
            this.buttonArgumentFileRun.Text = "Run";
            this.buttonArgumentFileRun.UseVisualStyleBackColor = true;
            this.buttonArgumentFileRun.Click += new System.EventHandler(this.buttonArgumentFileRun_Click);
            // 
            // textBoxArgumentFileCommand
            // 
            this.textBoxArgumentFileCommand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxArgumentFileCommand.Location = new System.Drawing.Point(83, 41);
            this.textBoxArgumentFileCommand.Name = "textBoxArgumentFileCommand";
            this.textBoxArgumentFileCommand.Size = new System.Drawing.Size(735, 22);
            this.textBoxArgumentFileCommand.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 44);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 17);
            this.label6.TabIndex = 10;
            this.label6.Text = "Command:";
            // 
            // comboBoxArgumentFileCommandVariables
            // 
            this.comboBoxArgumentFileCommandVariables.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxArgumentFileCommandVariables.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxArgumentFileCommandVariables.FormattingEnabled = true;
            this.comboBoxArgumentFileCommandVariables.Location = new System.Drawing.Point(83, 12);
            this.comboBoxArgumentFileCommandVariables.Name = "comboBoxArgumentFileCommandVariables";
            this.comboBoxArgumentFileCommandVariables.Size = new System.Drawing.Size(735, 24);
            this.comboBoxArgumentFileCommandVariables.TabIndex = 9;
            this.comboBoxArgumentFileCommandVariables.SelectionChangeCommitted += new System.EventHandler(this.comboBoxArgumentFileCommandVariables_SelectionChangeCommitted);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 17);
            this.label4.TabIndex = 8;
            this.label4.Text = "Variables:";
            // 
            // textBoxArgumentFileArgumentFile
            // 
            this.textBoxArgumentFileArgumentFile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxArgumentFileArgumentFile.Location = new System.Drawing.Point(7, 145);
            this.textBoxArgumentFileArgumentFile.Multiline = true;
            this.textBoxArgumentFileArgumentFile.Name = "textBoxArgumentFileArgumentFile";
            this.textBoxArgumentFileArgumentFile.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxArgumentFileArgumentFile.Size = new System.Drawing.Size(811, 332);
            this.textBoxArgumentFileArgumentFile.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 112);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 17);
            this.label5.TabIndex = 6;
            this.label5.Text = "Argument file:";
            // 
            // checkBoxArgumentWaitForExit
            // 
            this.checkBoxArgumentWaitForExit.AutoSize = true;
            this.checkBoxArgumentWaitForExit.Location = new System.Drawing.Point(83, 69);
            this.checkBoxArgumentWaitForExit.Name = "checkBoxArgumentWaitForExit";
            this.checkBoxArgumentWaitForExit.Size = new System.Drawing.Size(273, 21);
            this.checkBoxArgumentWaitForExit.TabIndex = 20;
            this.checkBoxArgumentWaitForExit.Text = "Wait for exit before run next command.";
            this.checkBoxArgumentWaitForExit.UseVisualStyleBackColor = true;
            // 
            // RunCommand
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(838, 554);
            this.Controls.Add(this.panelMain);
            this.Name = "RunCommand";
            this.Text = "Run...";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RunCommand_FormClosing);
            this.panelMain.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPageOpenWith.ResumeLayout(false);
            this.tabPageOpenWith.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewVideos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPictures)).EndInit();
            this.tabPageBatchCommand.ResumeLayout(false);
            this.tabPageBatchCommand.PerformLayout();
            this.tabPageArgumentFile.ResumeLayout(false);
            this.tabPageArgumentFile.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageOpenWith;
        private System.Windows.Forms.Button buttonOpenWithOpenWith;
        private System.Windows.Forms.TextBox textBoxOpenWithSelectedFiles;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPageBatchCommand;
        private System.Windows.Forms.TextBox textBoxBatchCommandCommand;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button buttonBatchCommandBatchRun;
        private System.Windows.Forms.ComboBox comboBoxBatchCommandCommandVariables;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxBatchCommandSelectedFiles;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage tabPageArgumentFile;
        private System.Windows.Forms.Button buttonArgumentFileRun;
        private System.Windows.Forms.TextBox textBoxArgumentFileCommand;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBoxArgumentFileCommandVariables;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxArgumentFileArgumentFile;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxCommandCommandExample;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DataGridView dataGridViewPictures;
        private System.Windows.Forms.DataGridView dataGridViewVideos;
        private System.Windows.Forms.TextBox textBoxRunCommandExamples;
        private System.Windows.Forms.CheckBox checkBoxOpenWithWaitForExit;
        private System.Windows.Forms.CheckBox checkBoxBatchCommandWaitForExit;
        private System.Windows.Forms.CheckBox checkBoxArgumentWaitForExit;
    }
}