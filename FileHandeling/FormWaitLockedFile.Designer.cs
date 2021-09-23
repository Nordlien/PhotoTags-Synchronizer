
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
            this.textBoxFilesLockedByProcess = new Krypton.Toolkit.KryptonTextBox();
            this.buttonCheck = new Krypton.Toolkit.KryptonButton();
            this.textBoxFiles = new Krypton.Toolkit.KryptonTextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.kryptonWorkspace1 = new Krypton.Workspace.KryptonWorkspace();
            this.kryptonPage9 = new Krypton.Navigator.KryptonPage();
            this.kryptonWorkspaceSequence2 = new Krypton.Workspace.KryptonWorkspaceSequence();
            this.kryptonWorkspaceCell1 = new Krypton.Workspace.KryptonWorkspaceCell();
            this.kryptonPage1 = new Krypton.Navigator.KryptonPage();
            this.kryptonWorkspaceCell5 = new Krypton.Workspace.KryptonWorkspaceCell();
            this.kryptonWorkspaceSequence1 = new Krypton.Workspace.KryptonWorkspaceSequence();
            this.kryptonWorkspaceCell2 = new Krypton.Workspace.KryptonWorkspaceCell();
            this.kryptonPage3 = new Krypton.Navigator.KryptonPage();
            this.kryptonWorkspaceCell3 = new Krypton.Workspace.KryptonWorkspaceCell();
            this.kryptonPage5 = new Krypton.Navigator.KryptonPage();
            this.kryptonWorkspaceCell4 = new Krypton.Workspace.KryptonWorkspaceCell();
            this.kryptonPage7 = new Krypton.Navigator.KryptonPage();
            this.kryptonPage2 = new Krypton.Navigator.KryptonPage();
            this.kryptonPage4 = new Krypton.Navigator.KryptonPage();
            this.kryptonPage6 = new Krypton.Navigator.KryptonPage();
            this.kryptonPage8 = new Krypton.Navigator.KryptonPage();
            this.kryptonPage10 = new Krypton.Navigator.KryptonPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspace1)).BeginInit();
            this.kryptonWorkspace1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage9)).BeginInit();
            this.kryptonPage9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCell1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage1)).BeginInit();
            this.kryptonPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCell5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCell2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage3)).BeginInit();
            this.kryptonPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCell3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage5)).BeginInit();
            this.kryptonPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCell4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage7)).BeginInit();
            this.kryptonPage7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage10)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonRetry
            // 
            this.buttonRetry.Location = new System.Drawing.Point(3, 508);
            this.buttonRetry.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonRetry.Name = "buttonRetry";
            this.buttonRetry.Size = new System.Drawing.Size(131, 38);
            this.buttonRetry.TabIndex = 0;
            this.buttonRetry.Values.Text = "Retry";
            this.buttonRetry.Click += new System.EventHandler(this.buttonRetry_Click);
            // 
            // buttonIgnor
            // 
            this.buttonIgnor.Location = new System.Drawing.Point(140, 508);
            this.buttonIgnor.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonIgnor.Name = "buttonIgnor";
            this.buttonIgnor.Size = new System.Drawing.Size(131, 38);
            this.buttonIgnor.TabIndex = 1;
            this.buttonIgnor.Values.Text = "Ignor";
            this.buttonIgnor.Click += new System.EventHandler(this.buttonIgnor_Click);
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(363, 136);
            this.textBox1.TabIndex = 3;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // textBoxFilesLockedByProcess
            // 
            this.textBoxFilesLockedByProcess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxFilesLockedByProcess.Location = new System.Drawing.Point(0, 0);
            this.textBoxFilesLockedByProcess.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxFilesLockedByProcess.Multiline = true;
            this.textBoxFilesLockedByProcess.Name = "textBoxFilesLockedByProcess";
            this.textBoxFilesLockedByProcess.ReadOnly = true;
            this.textBoxFilesLockedByProcess.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxFilesLockedByProcess.Size = new System.Drawing.Size(363, 137);
            this.textBoxFilesLockedByProcess.TabIndex = 6;
            // 
            // buttonCheck
            // 
            this.buttonCheck.Location = new System.Drawing.Point(278, 510);
            this.buttonCheck.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCheck.Name = "buttonCheck";
            this.buttonCheck.Size = new System.Drawing.Size(131, 38);
            this.buttonCheck.TabIndex = 5;
            this.buttonCheck.Values.Text = "Check who lock";
            this.buttonCheck.Click += new System.EventHandler(this.buttonCheck_Click);
            // 
            // textBoxFiles
            // 
            this.textBoxFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxFiles.Location = new System.Drawing.Point(0, 0);
            this.textBoxFiles.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxFiles.Multiline = true;
            this.textBoxFiles.Name = "textBoxFiles";
            this.textBoxFiles.ReadOnly = true;
            this.textBoxFiles.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxFiles.Size = new System.Drawing.Size(363, 136);
            this.textBoxFiles.TabIndex = 4;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.ErrorImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.ErrorImage")));
            this.pictureBox1.Image = global::FileHandeling.Properties.Resources.Backup_And_Sync_From_Google_Pause;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(364, 221);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox2.Image = global::FileHandeling.Properties.Resources.OneDrive_Pause;
            this.pictureBox2.Location = new System.Drawing.Point(0, 0);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(364, 220);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 5;
            this.pictureBox2.TabStop = false;
            // 
            // kryptonWorkspace1
            // 
            this.kryptonWorkspace1.ActivePage = this.kryptonPage1;
            this.kryptonWorkspace1.AllowPageDrag = false;
            this.tableLayoutPanel1.SetColumnSpan(this.kryptonWorkspace1, 3);
            this.kryptonWorkspace1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonWorkspace1.Location = new System.Drawing.Point(3, 3);
            this.kryptonWorkspace1.Name = "kryptonWorkspace1";
            // 
            // 
            // 
            this.kryptonWorkspace1.Root.Children.AddRange(new System.ComponentModel.Component[] {
            this.kryptonWorkspaceSequence1,
            this.kryptonWorkspaceSequence2});
            this.kryptonWorkspace1.Root.UniqueName = "45ce2683bad74a96bb709259d513cc6a";
            this.kryptonWorkspace1.Root.WorkspaceControl = this.kryptonWorkspace1;
            this.kryptonWorkspace1.SeparatorStyle = Krypton.Toolkit.SeparatorStyle.HighProfile;
            this.kryptonWorkspace1.Size = new System.Drawing.Size(736, 500);
            this.kryptonWorkspace1.TabIndex = 7;
            this.kryptonWorkspace1.TabStop = true;
            // 
            // kryptonPage9
            // 
            this.kryptonPage9.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage9.Controls.Add(this.pictureBox1);
            this.kryptonPage9.Flags = 65534;
            this.kryptonPage9.LastVisibleSet = true;
            this.kryptonPage9.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPage9.Name = "kryptonPage9";
            this.kryptonPage9.Size = new System.Drawing.Size(364, 221);
            this.kryptonPage9.Text = "Backup and SYnc from Google screenshot";
            this.kryptonPage9.ToolTipTitle = "Page ToolTip";
            this.kryptonPage9.UniqueName = "d25dd6ff393c426581b38e2d0486e03c";
            // 
            // kryptonWorkspaceSequence2
            // 
            this.kryptonWorkspaceSequence2.Children.AddRange(new System.ComponentModel.Component[] {
            this.kryptonWorkspaceCell1,
            this.kryptonWorkspaceCell5});
            this.kryptonWorkspaceSequence2.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.kryptonWorkspaceSequence2.UniqueName = "32c588d60faf4546bc9309c831079fbf";
            this.kryptonWorkspaceSequence2.WorkspaceControl = null;
            // 
            // kryptonWorkspaceCell1
            // 
            this.kryptonWorkspaceCell1.AllowPageDrag = true;
            this.kryptonWorkspaceCell1.AllowTabFocus = false;
            this.kryptonWorkspaceCell1.Name = "kryptonWorkspaceCell1";
            this.kryptonWorkspaceCell1.NavigatorMode = Krypton.Navigator.NavigatorMode.StackCheckButtonGroup;
            this.kryptonWorkspaceCell1.Pages.AddRange(new Krypton.Navigator.KryptonPage[] {
            this.kryptonPage1});
            this.kryptonWorkspaceCell1.SelectedIndex = 0;
            this.kryptonWorkspaceCell1.UniqueName = "1e06d07149384147a54062d9cc851bab";
            // 
            // kryptonPage1
            // 
            this.kryptonPage1.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage1.Controls.Add(this.pictureBox2);
            this.kryptonPage1.Flags = 65534;
            this.kryptonPage1.LastVisibleSet = true;
            this.kryptonPage1.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPage1.Name = "kryptonPage1";
            this.kryptonPage1.Size = new System.Drawing.Size(364, 220);
            this.kryptonPage1.Text = "OneDrive screenshot";
            this.kryptonPage1.ToolTipTitle = "Page ToolTip";
            this.kryptonPage1.UniqueName = "4c100ed38b2d4ad5b5ac6cd0fa88cde3";
            // 
            // kryptonWorkspaceCell5
            // 
            this.kryptonWorkspaceCell5.AllowPageDrag = true;
            this.kryptonWorkspaceCell5.AllowTabFocus = false;
            this.kryptonWorkspaceCell5.Name = "kryptonWorkspaceCell5";
            this.kryptonWorkspaceCell5.NavigatorMode = Krypton.Navigator.NavigatorMode.StackCheckButtonGroup;
            this.kryptonWorkspaceCell5.Pages.AddRange(new Krypton.Navigator.KryptonPage[] {
            this.kryptonPage9});
            this.kryptonWorkspaceCell5.SelectedIndex = 0;
            this.kryptonWorkspaceCell5.UniqueName = "4a9cec5150a541fe955c6f5ec8c4518a";
            // 
            // kryptonWorkspaceSequence1
            // 
            this.kryptonWorkspaceSequence1.Children.AddRange(new System.ComponentModel.Component[] {
            this.kryptonWorkspaceCell2,
            this.kryptonWorkspaceCell3,
            this.kryptonWorkspaceCell4});
            this.kryptonWorkspaceSequence1.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.kryptonWorkspaceSequence1.UniqueName = "94794706437d4bca94b7d4955af34b08";
            this.kryptonWorkspaceSequence1.WorkspaceControl = null;
            // 
            // kryptonWorkspaceCell2
            // 
            this.kryptonWorkspaceCell2.AllowPageDrag = true;
            this.kryptonWorkspaceCell2.AllowTabFocus = false;
            this.kryptonWorkspaceCell2.Name = "kryptonWorkspaceCell2";
            this.kryptonWorkspaceCell2.NavigatorMode = Krypton.Navigator.NavigatorMode.StackCheckButtonGroup;
            this.kryptonWorkspaceCell2.Pages.AddRange(new Krypton.Navigator.KryptonPage[] {
            this.kryptonPage3});
            this.kryptonWorkspaceCell2.SelectedIndex = 0;
            this.kryptonWorkspaceCell2.UniqueName = "7ca1c7a460244ca6bc413ade54663092";
            // 
            // kryptonPage3
            // 
            this.kryptonPage3.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage3.Controls.Add(this.textBox1);
            this.kryptonPage3.Flags = 65534;
            this.kryptonPage3.LastVisibleSet = true;
            this.kryptonPage3.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPage3.Name = "kryptonPage3";
            this.kryptonPage3.Size = new System.Drawing.Size(363, 136);
            this.kryptonPage3.Text = "Information";
            this.kryptonPage3.ToolTipTitle = "Page ToolTip";
            this.kryptonPage3.UniqueName = "e7485ceb769647e4ae6cbf5ebc21ecd3";
            // 
            // kryptonWorkspaceCell3
            // 
            this.kryptonWorkspaceCell3.AllowPageDrag = true;
            this.kryptonWorkspaceCell3.AllowTabFocus = false;
            this.kryptonWorkspaceCell3.Name = "kryptonWorkspaceCell3";
            this.kryptonWorkspaceCell3.NavigatorMode = Krypton.Navigator.NavigatorMode.StackCheckButtonGroup;
            this.kryptonWorkspaceCell3.Pages.AddRange(new Krypton.Navigator.KryptonPage[] {
            this.kryptonPage5});
            this.kryptonWorkspaceCell3.SelectedIndex = 0;
            this.kryptonWorkspaceCell3.UniqueName = "a47b335a9ffd4ee68c77c859479b4a80";
            // 
            // kryptonPage5
            // 
            this.kryptonPage5.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage5.Controls.Add(this.textBoxFiles);
            this.kryptonPage5.Flags = 65534;
            this.kryptonPage5.LastVisibleSet = true;
            this.kryptonPage5.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPage5.Name = "kryptonPage5";
            this.kryptonPage5.Size = new System.Drawing.Size(363, 136);
            this.kryptonPage5.Text = "Files:";
            this.kryptonPage5.ToolTipTitle = "Page ToolTip";
            this.kryptonPage5.UniqueName = "5b515d5d018b4cc6b4ec5ae066585d8c";
            // 
            // kryptonWorkspaceCell4
            // 
            this.kryptonWorkspaceCell4.AllowPageDrag = true;
            this.kryptonWorkspaceCell4.AllowTabFocus = false;
            this.kryptonWorkspaceCell4.Name = "kryptonWorkspaceCell4";
            this.kryptonWorkspaceCell4.NavigatorMode = Krypton.Navigator.NavigatorMode.StackCheckButtonGroup;
            this.kryptonWorkspaceCell4.Pages.AddRange(new Krypton.Navigator.KryptonPage[] {
            this.kryptonPage7});
            this.kryptonWorkspaceCell4.SelectedIndex = 0;
            this.kryptonWorkspaceCell4.UniqueName = "24f7a93e3c3a4bcc86edb8075af6f9ae";
            // 
            // kryptonPage7
            // 
            this.kryptonPage7.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage7.Controls.Add(this.textBoxFilesLockedByProcess);
            this.kryptonPage7.Flags = 65534;
            this.kryptonPage7.LastVisibleSet = true;
            this.kryptonPage7.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPage7.Name = "kryptonPage7";
            this.kryptonPage7.Size = new System.Drawing.Size(363, 137);
            this.kryptonPage7.Text = "Locked by procss";
            this.kryptonPage7.ToolTipTitle = "Page ToolTip";
            this.kryptonPage7.UniqueName = "0a4ab2a4e3d541f5ae5d376bde164856";
            // 
            // kryptonPage2
            // 
            this.kryptonPage2.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage2.Flags = 65534;
            this.kryptonPage2.LastVisibleSet = true;
            this.kryptonPage2.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPage2.Name = "kryptonPage2";
            this.kryptonPage2.Size = new System.Drawing.Size(100, 100);
            this.kryptonPage2.Text = "kryptonPage2";
            this.kryptonPage2.ToolTipTitle = "Page ToolTip";
            this.kryptonPage2.UniqueName = "74516567277a48fc8f26f319d7541741";
            // 
            // kryptonPage4
            // 
            this.kryptonPage4.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage4.Flags = 65534;
            this.kryptonPage4.LastVisibleSet = true;
            this.kryptonPage4.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPage4.Name = "kryptonPage4";
            this.kryptonPage4.Size = new System.Drawing.Size(100, 100);
            this.kryptonPage4.Text = "kryptonPage4";
            this.kryptonPage4.ToolTipTitle = "Page ToolTip";
            this.kryptonPage4.UniqueName = "b5093862e5b345e2a7870d68d2a18f68";
            // 
            // kryptonPage6
            // 
            this.kryptonPage6.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage6.Flags = 65534;
            this.kryptonPage6.LastVisibleSet = true;
            this.kryptonPage6.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPage6.Name = "kryptonPage6";
            this.kryptonPage6.Size = new System.Drawing.Size(100, 100);
            this.kryptonPage6.Text = "kryptonPage6";
            this.kryptonPage6.ToolTipTitle = "Page ToolTip";
            this.kryptonPage6.UniqueName = "7a27dc7942f041e8a1d2ac3ffdf2364e";
            // 
            // kryptonPage8
            // 
            this.kryptonPage8.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage8.Flags = 65534;
            this.kryptonPage8.LastVisibleSet = true;
            this.kryptonPage8.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPage8.Name = "kryptonPage8";
            this.kryptonPage8.Size = new System.Drawing.Size(100, 100);
            this.kryptonPage8.Text = "kryptonPage8";
            this.kryptonPage8.ToolTipTitle = "Page ToolTip";
            this.kryptonPage8.UniqueName = "3ee51f02136f4ba2ae591dc6eb2ec15a";
            // 
            // kryptonPage10
            // 
            this.kryptonPage10.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage10.Flags = 65534;
            this.kryptonPage10.LastVisibleSet = true;
            this.kryptonPage10.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPage10.Name = "kryptonPage10";
            this.kryptonPage10.Size = new System.Drawing.Size(100, 100);
            this.kryptonPage10.Text = "kryptonPage10";
            this.kryptonPage10.ToolTipTitle = "Page ToolTip";
            this.kryptonPage10.UniqueName = "467222314cf54e9a8e9e3a8972e64317";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.buttonCheck, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonRetry, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonIgnor, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.kryptonWorkspace1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(742, 562);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // FormWaitLockedFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(742, 562);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FormWaitLockedFile";
            this.Text = "File is been locked by another process";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormWaitLockedFile_FormClosed);
            this.Shown += new System.EventHandler(this.FormWaitLockedFile_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspace1)).EndInit();
            this.kryptonWorkspace1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage9)).EndInit();
            this.kryptonPage9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCell1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage1)).EndInit();
            this.kryptonPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCell5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCell2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage3)).EndInit();
            this.kryptonPage3.ResumeLayout(false);
            this.kryptonPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCell3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage5)).EndInit();
            this.kryptonPage5.ResumeLayout(false);
            this.kryptonPage5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCell4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage7)).EndInit();
            this.kryptonPage7.ResumeLayout(false);
            this.kryptonPage7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage10)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Krypton.Toolkit.KryptonButton buttonRetry;
        private Krypton.Toolkit.KryptonButton buttonIgnor;
        private Krypton.Toolkit.KryptonTextBox textBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private Krypton.Toolkit.KryptonTextBox textBoxFiles;
        private Krypton.Toolkit.KryptonButton buttonCheck;
        private Krypton.Toolkit.KryptonTextBox textBoxFilesLockedByProcess;
        private Krypton.Workspace.KryptonWorkspace kryptonWorkspace1;
        private Krypton.Navigator.KryptonPage kryptonPage9;
        private Krypton.Workspace.KryptonWorkspaceSequence kryptonWorkspaceSequence2;
        private Krypton.Workspace.KryptonWorkspaceCell kryptonWorkspaceCell1;
        private Krypton.Navigator.KryptonPage kryptonPage1;
        private Krypton.Workspace.KryptonWorkspaceCell kryptonWorkspaceCell5;
        private Krypton.Workspace.KryptonWorkspaceSequence kryptonWorkspaceSequence1;
        private Krypton.Workspace.KryptonWorkspaceCell kryptonWorkspaceCell2;
        private Krypton.Navigator.KryptonPage kryptonPage3;
        private Krypton.Workspace.KryptonWorkspaceCell kryptonWorkspaceCell3;
        private Krypton.Navigator.KryptonPage kryptonPage5;
        private Krypton.Workspace.KryptonWorkspaceCell kryptonWorkspaceCell4;
        private Krypton.Navigator.KryptonPage kryptonPage7;
        private Krypton.Navigator.KryptonPage kryptonPage2;
        private Krypton.Navigator.KryptonPage kryptonPage4;
        private Krypton.Navigator.KryptonPage kryptonPage6;
        private Krypton.Navigator.KryptonPage kryptonPage8;
        private Krypton.Navigator.KryptonPage kryptonPage10;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}