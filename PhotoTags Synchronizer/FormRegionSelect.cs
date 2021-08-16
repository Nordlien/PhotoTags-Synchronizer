using System;
using System.Drawing;
using ComponentFactory.Krypton.Toolkit;

namespace PhotoTagsSynchronizer
{

    public partial class FormRegionSelect : KryptonForm
    {
        public delegate void RegionSelectedEvent(object sender, RegionSelectedEventArgs e);
        public event RegionSelectedEvent OnRegionSelected;
        private int RowIndex { get; set; }
        private int ColumnIndex { get; set; }

        public FormRegionSelect()
        {
            InitializeComponent();
        }

        public void SetImage(Image image, string title, int columnIndex = -1, int rowIndex = -1)
        {
            this.Text = title;
            ColumnIndex = columnIndex;
            RowIndex = rowIndex;
            imageBox1.Text = "";
            imageBox1.Image = image;
            imageBox1.ZoomToFit();
        }

        public void SetSelection(RectangleF rectangleF)
        {
            imageBox1.SelectionRegion = rectangleF;
        }

        public void SetSelectionNone()
        {
            imageBox1.SelectNone();
        }

        public void SetImageNone()
        {
            this.Text = "No valid media file is selected.";
            imageBox1.Image = null;
            imageBox1.Text = this.Text;
        }

        public void SetImageText(string text)
        {
            imageBox1.Text = text;
            imageBox1.Image = null;
        }

        private void FormRegionSelect_Resize(object sender, EventArgs e)
        {
            imageBox1.ZoomToFit();
        }

        private void FormRegionSelect_ResizeEnd(object sender, EventArgs e)
        {
            imageBox1.ZoomToFit();
        }

        private void imageBox1_RegionChanged(object sender, EventArgs e)
        {
            
        }

        private void imageBox1_Selected(object sender, EventArgs e)
        {
            if (imageBox1.Image == null) return;
            RegionSelectedEventArgs regionSelectedEventArgs = new RegionSelectedEventArgs();
            regionSelectedEventArgs.ImageSize = imageBox1.Image.Size;
            regionSelectedEventArgs.Selection = imageBox1.SelectionRegion;
            regionSelectedEventArgs.ColumnIndex = ColumnIndex;
            regionSelectedEventArgs.RowIndex = RowIndex;
            if (OnRegionSelected != null) OnRegionSelected(this, regionSelectedEventArgs);
        }

        private void imageBox1_Selecting(object sender, Cyotek.Windows.Forms.ImageBoxCancelEventArgs e)
        {

        }
    }
}
