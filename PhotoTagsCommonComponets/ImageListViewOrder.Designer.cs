namespace PhotoTagsCommonComponets
{
    partial class ImageListViewOrder
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonMoveDown = new Krypton.Toolkit.KryptonButton();
            this.buttonMoveUp = new Krypton.Toolkit.KryptonButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.imageListViewDragAndDrop1 = new PhotoTagsCommonComponets.ImageListViewDragAndDrop();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonMoveDown
            // 
            this.buttonMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonMoveDown.BackgroundImage = global::PhotoTagsCommonComponets.Properties.Resources.Arrow_down;
            this.buttonMoveDown.Location = new System.Drawing.Point(198, 59);
            this.buttonMoveDown.Margin = new System.Windows.Forms.Padding(2);
            this.buttonMoveDown.Name = "buttonMoveDown";
            this.buttonMoveDown.Size = new System.Drawing.Size(26, 28);
            this.buttonMoveDown.StateCommon.Back.Image = global::PhotoTagsCommonComponets.Properties.Resources.Arrow_down;
            this.buttonMoveDown.StateCommon.Back.ImageAlign = Krypton.Toolkit.PaletteRectangleAlign.Control;
            this.buttonMoveDown.StateCommon.Back.ImageStyle = Krypton.Toolkit.PaletteImageStyle.CenterMiddle;
            this.buttonMoveDown.StateDisabled.Back.Image = global::PhotoTagsCommonComponets.Properties.Resources.Arrow_down;
            this.buttonMoveDown.StateDisabled.Back.ImageAlign = Krypton.Toolkit.PaletteRectangleAlign.Control;
            this.buttonMoveDown.StateDisabled.Back.ImageStyle = Krypton.Toolkit.PaletteImageStyle.CenterMiddle;
            this.buttonMoveDown.StateNormal.Back.Image = global::PhotoTagsCommonComponets.Properties.Resources.Arrow_down;
            this.buttonMoveDown.StateNormal.Back.ImageAlign = Krypton.Toolkit.PaletteRectangleAlign.Control;
            this.buttonMoveDown.StateNormal.Back.ImageStyle = Krypton.Toolkit.PaletteImageStyle.CenterMiddle;
            this.buttonMoveDown.StatePressed.Back.Image = global::PhotoTagsCommonComponets.Properties.Resources.Arrow_down;
            this.buttonMoveDown.StatePressed.Back.ImageAlign = Krypton.Toolkit.PaletteRectangleAlign.Control;
            this.buttonMoveDown.StatePressed.Back.ImageStyle = Krypton.Toolkit.PaletteImageStyle.CenterMiddle;
            this.buttonMoveDown.StateTracking.Back.Image = global::PhotoTagsCommonComponets.Properties.Resources.Arrow_down;
            this.buttonMoveDown.StateTracking.Back.ImageAlign = Krypton.Toolkit.PaletteRectangleAlign.Control;
            this.buttonMoveDown.StateTracking.Back.ImageStyle = Krypton.Toolkit.PaletteImageStyle.CenterMiddle;
            this.buttonMoveDown.TabIndex = 2;
            this.buttonMoveDown.Values.Text = "";
            this.buttonMoveDown.Click += new System.EventHandler(this.buttonMoveDown_Click);
            // 
            // buttonMoveUp
            // 
            this.buttonMoveUp.BackgroundImage = global::PhotoTagsCommonComponets.Properties.Resources.Arrow_up;
            this.buttonMoveUp.Location = new System.Drawing.Point(198, 2);
            this.buttonMoveUp.Margin = new System.Windows.Forms.Padding(2);
            this.buttonMoveUp.Name = "buttonMoveUp";
            this.buttonMoveUp.Size = new System.Drawing.Size(26, 28);
            this.buttonMoveUp.StateCommon.Back.Image = global::PhotoTagsCommonComponets.Properties.Resources.Arrow_up;
            this.buttonMoveUp.StateCommon.Back.ImageAlign = Krypton.Toolkit.PaletteRectangleAlign.Control;
            this.buttonMoveUp.StateCommon.Back.ImageStyle = Krypton.Toolkit.PaletteImageStyle.CenterMiddle;
            this.buttonMoveUp.StateDisabled.Back.Image = global::PhotoTagsCommonComponets.Properties.Resources.Arrow_up;
            this.buttonMoveUp.StateDisabled.Back.ImageStyle = Krypton.Toolkit.PaletteImageStyle.Inherit;
            this.buttonMoveUp.StateNormal.Back.Image = global::PhotoTagsCommonComponets.Properties.Resources.Arrow_up;
            this.buttonMoveUp.StateNormal.Back.ImageAlign = Krypton.Toolkit.PaletteRectangleAlign.Control;
            this.buttonMoveUp.StateNormal.Back.ImageStyle = Krypton.Toolkit.PaletteImageStyle.CenterMiddle;
            this.buttonMoveUp.StatePressed.Back.Image = global::PhotoTagsCommonComponets.Properties.Resources.Arrow_up;
            this.buttonMoveUp.StatePressed.Back.ImageAlign = Krypton.Toolkit.PaletteRectangleAlign.Control;
            this.buttonMoveUp.StatePressed.Back.ImageStyle = Krypton.Toolkit.PaletteImageStyle.CenterMiddle;
            this.buttonMoveUp.StateTracking.Back.Image = global::PhotoTagsCommonComponets.Properties.Resources.Arrow_up;
            this.buttonMoveUp.StateTracking.Back.ImageAlign = Krypton.Toolkit.PaletteRectangleAlign.Control;
            this.buttonMoveUp.StateTracking.Back.ImageStyle = Krypton.Toolkit.PaletteImageStyle.CenterMiddle;
            this.buttonMoveUp.TabIndex = 1;
            this.buttonMoveUp.Values.Text = "";
            this.buttonMoveUp.Click += new System.EventHandler(this.buttonMoveUp_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.imageListViewDragAndDrop1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonMoveDown, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonMoveUp, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(226, 89);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // imageListViewDragAndDrop1
            // 
            this.imageListViewDragAndDrop1.AllowDrop = true;
            this.imageListViewDragAndDrop1.AllowReorder = true;
            this.imageListViewDragAndDrop1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.imageListViewDragAndDrop1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageListViewDragAndDrop1.FullRowSelect = true;
            this.imageListViewDragAndDrop1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.imageListViewDragAndDrop1.HideSelection = false;
            this.imageListViewDragAndDrop1.LineColor = System.Drawing.Color.Red;
            this.imageListViewDragAndDrop1.Location = new System.Drawing.Point(2, 2);
            this.imageListViewDragAndDrop1.Margin = new System.Windows.Forms.Padding(2);
            this.imageListViewDragAndDrop1.MultiSelect = false;
            this.imageListViewDragAndDrop1.Name = "imageListViewDragAndDrop1";
            this.imageListViewDragAndDrop1.OwnerDraw = true;
            this.tableLayoutPanel1.SetRowSpan(this.imageListViewDragAndDrop1, 3);
            this.imageListViewDragAndDrop1.Size = new System.Drawing.Size(192, 85);
            this.imageListViewDragAndDrop1.TabIndex = 0;
            this.imageListViewDragAndDrop1.TileSize = new System.Drawing.Size(130, 36);
            this.imageListViewDragAndDrop1.UseCompatibleStateImageBehavior = false;
            this.imageListViewDragAndDrop1.View = System.Windows.Forms.View.Details;
            this.imageListViewDragAndDrop1.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.imageListViewDragAndDrop1_DrawColumnHeader);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Source";
            this.columnHeader1.Width = 190;
            // 
            // ImageListViewOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ImageListViewOrder";
            this.Size = new System.Drawing.Size(226, 89);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private PhotoTagsCommonComponets.ImageListViewDragAndDrop imageListViewDragAndDrop1;
        private Krypton.Toolkit.KryptonButton buttonMoveUp;
        private Krypton.Toolkit.KryptonButton buttonMoveDown;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
