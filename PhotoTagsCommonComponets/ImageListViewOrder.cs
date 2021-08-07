using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhotoTagsSynchronizer
{

    public partial class ImageListViewOrder: UserControl
    {
        public ImageListViewOrder()
        {
            
            InitializeComponent();
            imageListViewDragAndDrop1.DrawItem += (sender, e) => { e.DrawDefault = true; };
            imageListViewDragAndDrop1.DrawSubItem += (sender, e) => { e.DrawDefault = true; };
        }

        public void AutoResize()
        {
            if (imageListViewDragAndDrop1.Columns.Count >= 1) imageListViewDragAndDrop1.Columns[0].Width = imageListViewDragAndDrop1.Width - 10;
        }

      
		[Category("Behavior")]
		public bool AllowReorder
		{
			get { return imageListViewDragAndDrop1.AllowReorder; }
			set { imageListViewDragAndDrop1.AllowReorder = value; }
		}

		[Category("Behavior")]
		public ListView.ListViewItemCollection Items
		{
            set 
            {
                imageListViewDragAndDrop1.Items.AddRange(value);
                                
            }

			get 
            {               
                return imageListViewDragAndDrop1.Items; 
            }			
		}



		[Category("Appearance")]
		public Color LineColor
		{
			get { return imageListViewDragAndDrop1.LineColor; }
			set { imageListViewDragAndDrop1.LineColor = value; }
		}

        private void buttonMoveUp_Click(object sender, EventArgs e)
        {
			if (imageListViewDragAndDrop1.SelectedItems.Count == 1)
            {
                int itemIndex = imageListViewDragAndDrop1.SelectedItems[0].Index;
                if (itemIndex > 0)
                {
                    ListViewItem item = imageListViewDragAndDrop1.SelectedItems[0]; //.Clone();
                    imageListViewDragAndDrop1.Items.Remove(item);
                    imageListViewDragAndDrop1.Items.Insert(itemIndex - 1, item);
                }
            }
        }

        private void buttonMoveDown_Click(object sender, EventArgs e)
        {
            if (imageListViewDragAndDrop1.SelectedItems.Count == 1)
            {
                int itemIndex = imageListViewDragAndDrop1.SelectedItems[0].Index;
                if (itemIndex < imageListViewDragAndDrop1.Items.Count - 1)
                {
                    ListViewItem item = imageListViewDragAndDrop1.SelectedItems[0]; //.Clone();
                    imageListViewDragAndDrop1.Items.Remove(item);
                    imageListViewDragAndDrop1.Items.Insert(itemIndex + 1, item);
                }
            }
        }

        private void imageListViewDragAndDrop1_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(SystemColors.Control), e.Bounds);
            e.DrawText();
        }

    }
}
