namespace PhotoTagsSynchronizer
{
    partial class FormCompareText
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCompareText));
            this.label6 = new Krypton.Toolkit.KryptonLabel();
            this.label7 = new Krypton.Toolkit.KryptonLabel();
            this.label5 = new Krypton.Toolkit.KryptonLabel();
            this.label4 = new Krypton.Toolkit.KryptonLabel();
            this.ofdFile = new System.Windows.Forms.OpenFileDialog();
            this.fctb1 = new FastColoredTextBoxNS.FastColoredTextBox();
            this.fctb2 = new FastColoredTextBoxNS.FastColoredTextBox();
            this.kryptonManager1 = new Krypton.Toolkit.KryptonManager(this.components);
            this.kryptonWorkspaceCompareText = new Krypton.Workspace.KryptonWorkspace();
            this.kryptonPageSourceFirst = new Krypton.Navigator.KryptonPage();
            this.kryptonWorkspaceCellSourceFirst = new Krypton.Workspace.KryptonWorkspaceCell();
            this.kryptonWorkspaceCellSourceSecond = new Krypton.Workspace.KryptonWorkspaceCell();
            this.kryptonPageSourceSecond = new Krypton.Navigator.KryptonPage();
            this.kryptonPage4 = new Krypton.Navigator.KryptonPage();
            ((System.ComponentModel.ISupportInitialize)(this.fctb1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fctb2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCompareText)).BeginInit();
            this.kryptonWorkspaceCompareText.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageSourceFirst)).BeginInit();
            this.kryptonPageSourceFirst.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCellSourceFirst)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCellSourceSecond)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageSourceSecond)).BeginInit();
            this.kryptonPageSourceSecond.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage4)).BeginInit();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.Location = new System.Drawing.Point(176, 505);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 24);
            this.label6.TabIndex = 24;
            this.label6.Values.Text = "Deleted lines";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.BackColor = System.Drawing.Color.Pink;
            this.label7.Location = new System.Drawing.Point(157, 503);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(11, 24);
            this.label7.TabIndex = 23;
            this.label7.Values.Text = " ";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.Location = new System.Drawing.Point(1, 505);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(102, 24);
            this.label5.TabIndex = 22;
            this.label5.Values.Text = "Inserted lines";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.BackColor = System.Drawing.Color.PaleGreen;
            this.label4.Location = new System.Drawing.Point(16, 503);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(11, 24);
            this.label4.TabIndex = 21;
            this.label4.Values.Text = " ";
            // 
            // fctb1
            // 
            this.fctb1.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.fctb1.AutoScrollMinSize = new System.Drawing.Size(221, 18);
            this.fctb1.BackBrush = null;
            this.fctb1.CharHeight = 18;
            this.fctb1.CharWidth = 10;
            this.fctb1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.fctb1.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.fctb1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fctb1.IsReplaceMode = false;
            this.fctb1.Location = new System.Drawing.Point(0, 0);
            this.fctb1.Margin = new System.Windows.Forms.Padding(4);
            this.fctb1.Name = "fctb1";
            this.fctb1.Paddings = new System.Windows.Forms.Padding(0);
            this.fctb1.ReadOnly = true;
            this.fctb1.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.fctb1.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fctb1.ServiceColors")));
            this.fctb1.Size = new System.Drawing.Size(476, 469);
            this.fctb1.TabIndex = 26;
            this.fctb1.Text = "fastColoredTextBox1";
            this.fctb1.Zoom = 100;
            this.fctb1.SelectionChanged += new System.EventHandler(this.tb_VisibleRangeChanged);
            this.fctb1.VisibleRangeChanged += new System.EventHandler(this.tb_VisibleRangeChanged);
            // 
            // fctb2
            // 
            this.fctb2.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.fctb2.AutoScrollMinSize = new System.Drawing.Size(221, 18);
            this.fctb2.BackBrush = null;
            this.fctb2.CharHeight = 18;
            this.fctb2.CharWidth = 10;
            this.fctb2.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.fctb2.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.fctb2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fctb2.IsReplaceMode = false;
            this.fctb2.Location = new System.Drawing.Point(0, 0);
            this.fctb2.Margin = new System.Windows.Forms.Padding(4);
            this.fctb2.Name = "fctb2";
            this.fctb2.Paddings = new System.Windows.Forms.Padding(0);
            this.fctb2.ReadOnly = true;
            this.fctb2.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.fctb2.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fctb2.ServiceColors")));
            this.fctb2.Size = new System.Drawing.Size(477, 469);
            this.fctb2.TabIndex = 27;
            this.fctb2.Text = "fastColoredTextBox2";
            this.fctb2.Zoom = 100;
            this.fctb2.SelectionChanged += new System.EventHandler(this.tb_VisibleRangeChanged);
            this.fctb2.VisibleRangeChanged += new System.EventHandler(this.tb_VisibleRangeChanged);
            // 
            // kryptonWorkspaceCompareText
            // 
            this.kryptonWorkspaceCompareText.ActivePage = this.kryptonPageSourceFirst;
            this.kryptonWorkspaceCompareText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonWorkspaceCompareText.Location = new System.Drawing.Point(0, 0);
            this.kryptonWorkspaceCompareText.Name = "kryptonWorkspaceCompareText";
            // 
            // 
            // 
            this.kryptonWorkspaceCompareText.Root.Children.AddRange(new System.ComponentModel.Component[] {
            this.kryptonWorkspaceCellSourceFirst,
            this.kryptonWorkspaceCellSourceSecond});
            this.kryptonWorkspaceCompareText.Root.UniqueName = "6bd8016232854e828e9bb293fd2fb6f9";
            this.kryptonWorkspaceCompareText.Root.WorkspaceControl = this.kryptonWorkspaceCompareText;
            this.kryptonWorkspaceCompareText.SeparatorStyle = Krypton.Toolkit.SeparatorStyle.HighProfile;
            this.kryptonWorkspaceCompareText.Size = new System.Drawing.Size(962, 500);
            this.kryptonWorkspaceCompareText.TabIndex = 29;
            this.kryptonWorkspaceCompareText.TabStop = true;
            // 
            // kryptonPageSourceFirst
            // 
            this.kryptonPageSourceFirst.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageSourceFirst.Controls.Add(this.fctb1);
            this.kryptonPageSourceFirst.Flags = 65534;
            this.kryptonPageSourceFirst.LastVisibleSet = true;
            this.kryptonPageSourceFirst.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageSourceFirst.Name = "kryptonPageSourceFirst";
            this.kryptonPageSourceFirst.Size = new System.Drawing.Size(476, 469);
            this.kryptonPageSourceFirst.Text = "Source: first";
            this.kryptonPageSourceFirst.ToolTipTitle = "Page ToolTip";
            this.kryptonPageSourceFirst.UniqueName = "cd6063e74a1746f2a1963210d7142aa6";
            // 
            // kryptonWorkspaceCellSourceFirst
            // 
            this.kryptonWorkspaceCellSourceFirst.AllowPageDrag = true;
            this.kryptonWorkspaceCellSourceFirst.AllowTabFocus = false;
            this.kryptonWorkspaceCellSourceFirst.Name = "kryptonWorkspaceCellSourceFirst";
            this.kryptonWorkspaceCellSourceFirst.NavigatorMode = Krypton.Navigator.NavigatorMode.StackCheckButtonGroup;
            this.kryptonWorkspaceCellSourceFirst.Pages.AddRange(new Krypton.Navigator.KryptonPage[] {
            this.kryptonPageSourceFirst});
            this.kryptonWorkspaceCellSourceFirst.SelectedIndex = 0;
            this.kryptonWorkspaceCellSourceFirst.UniqueName = "68b3b17c02604b04b7dec4bd3c88254a";
            // 
            // kryptonWorkspaceCellSourceSecond
            // 
            this.kryptonWorkspaceCellSourceSecond.AllowPageDrag = true;
            this.kryptonWorkspaceCellSourceSecond.AllowTabFocus = false;
            this.kryptonWorkspaceCellSourceSecond.Name = "kryptonWorkspaceCellSourceSecond";
            this.kryptonWorkspaceCellSourceSecond.NavigatorMode = Krypton.Navigator.NavigatorMode.StackCheckButtonGroup;
            this.kryptonWorkspaceCellSourceSecond.Pages.AddRange(new Krypton.Navigator.KryptonPage[] {
            this.kryptonPageSourceSecond});
            this.kryptonWorkspaceCellSourceSecond.SelectedIndex = 0;
            this.kryptonWorkspaceCellSourceSecond.UniqueName = "8c2d2848c8204a9b9ec99cc7ba041180";
            // 
            // kryptonPageSourceSecond
            // 
            this.kryptonPageSourceSecond.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageSourceSecond.Controls.Add(this.fctb2);
            this.kryptonPageSourceSecond.Flags = 65534;
            this.kryptonPageSourceSecond.LastVisibleSet = true;
            this.kryptonPageSourceSecond.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageSourceSecond.Name = "kryptonPageSourceSecond";
            this.kryptonPageSourceSecond.Size = new System.Drawing.Size(477, 469);
            this.kryptonPageSourceSecond.Text = "Source: second";
            this.kryptonPageSourceSecond.ToolTipTitle = "Page ToolTip";
            this.kryptonPageSourceSecond.UniqueName = "c36efafd5d2b4e40a48624b6e1055fb7";
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
            this.kryptonPage4.UniqueName = "7032f579ee824e1794d0891940a2b1ef";
            // 
            // FormCompareText
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(963, 538);
            this.Controls.Add(this.kryptonWorkspaceCompareText);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormCompareText";
            this.Text = "Compare text";
            ((System.ComponentModel.ISupportInitialize)(this.fctb1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fctb2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCompareText)).EndInit();
            this.kryptonWorkspaceCompareText.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageSourceFirst)).EndInit();
            this.kryptonPageSourceFirst.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCellSourceFirst)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCellSourceSecond)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageSourceSecond)).EndInit();
            this.kryptonPageSourceSecond.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Krypton.Toolkit.KryptonLabel label6;
        private Krypton.Toolkit.KryptonLabel label7;
        private Krypton.Toolkit.KryptonLabel label5;
        private Krypton.Toolkit.KryptonLabel label4;
        private System.Windows.Forms.OpenFileDialog ofdFile;
        private FastColoredTextBoxNS.FastColoredTextBox fctb1;
        private FastColoredTextBoxNS.FastColoredTextBox fctb2;
        private Krypton.Toolkit.KryptonManager kryptonManager1;
        private Krypton.Workspace.KryptonWorkspace kryptonWorkspaceCompareText;
        private Krypton.Navigator.KryptonPage kryptonPageSourceFirst;
        private Krypton.Workspace.KryptonWorkspaceCell kryptonWorkspaceCellSourceFirst;
        private Krypton.Workspace.KryptonWorkspaceCell kryptonWorkspaceCellSourceSecond;
        private Krypton.Navigator.KryptonPage kryptonPageSourceSecond;
        private Krypton.Navigator.KryptonPage kryptonPage4;
    }
}