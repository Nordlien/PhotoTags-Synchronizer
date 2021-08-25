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
            this.buttonMoveUp = new Krypton.Toolkit.KryptonButton();
            this.buttonMoveDown = new Krypton.Toolkit.KryptonButton();
            this.imageListViewDragAndDrop1 = new PhotoTagsCommonComponets.ImageListViewDragAndDrop();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // buttonMoveUp
            // 
            this.buttonMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMoveUp.BackgroundImage = global::PhotoTagsCommonComponets.Properties.Resources.Arrow_up;
            this.buttonMoveUp.Location = new System.Drawing.Point(198, 2);
            this.buttonMoveUp.Margin = new System.Windows.Forms.Padding(2);
            this.buttonMoveUp.Name = "buttonMoveUp";
            this.buttonMoveUp.Size = new System.Drawing.Size(26, 28);
            this.buttonMoveUp.TabIndex = 1;
            this.buttonMoveUp.Click += new System.EventHandler(this.buttonMoveUp_Click);
            // 
            // buttonMoveDown
            // 
            this.buttonMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMoveDown.BackgroundImage = global::PhotoTagsCommonComponets.Properties.Resources.Arrow_down;
            this.buttonMoveDown.Location = new System.Drawing.Point(198, 59);
            this.buttonMoveDown.Margin = new System.Windows.Forms.Padding(2);
            this.buttonMoveDown.Name = "buttonMoveDown";
            this.buttonMoveDown.Size = new System.Drawing.Size(26, 28);
            this.buttonMoveDown.TabIndex = 2;
            this.buttonMoveDown.Click += new System.EventHandler(this.buttonMoveDown_Click);
            // 
            // imageListViewDragAndDrop1
            // 
            this.imageListViewDragAndDrop1.AllowDrop = true;
            this.imageListViewDragAndDrop1.AllowReorder = true;
            this.imageListViewDragAndDrop1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.imageListViewDragAndDrop1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.imageListViewDragAndDrop1.FullRowSelect = true;
            this.imageListViewDragAndDrop1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.imageListViewDragAndDrop1.HideSelection = false;
            this.imageListViewDragAndDrop1.LineColor = System.Drawing.Color.Red;
            this.imageListViewDragAndDrop1.Location = new System.Drawing.Point(0, 0);
            this.imageListViewDragAndDrop1.Margin = new System.Windows.Forms.Padding(2);
            this.imageListViewDragAndDrop1.MultiSelect = false;
            this.imageListViewDragAndDrop1.Name = "imageListViewDragAndDrop1";
            this.imageListViewDragAndDrop1.OwnerDraw = true;
            this.imageListViewDragAndDrop1.Size = new System.Drawing.Size(195, 87);
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
            this.Controls.Add(this.buttonMoveDown);
            this.Controls.Add(this.buttonMoveUp);
            this.Controls.Add(this.imageListViewDragAndDrop1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ImageListViewOrder";
            this.Size = new System.Drawing.Size(226, 89);
            this.ResumeLayout(false);

        }

        #endregion

        private PhotoTagsCommonComponets.ImageListViewDragAndDrop imageListViewDragAndDrop1;
        private Krypton.Toolkit.KryptonButton buttonMoveUp;
        private Krypton.Toolkit.KryptonButton buttonMoveDown;
        private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}
