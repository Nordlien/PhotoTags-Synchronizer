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
        private bool isDoingInitializeComponent = false;
        //private ImageListViewOrderCollection listViewItemCollection = new List<ListViewItem>();
        

        public ImageListViewOrder()
        {
            isDoingInitializeComponent = true;
            
            InitializeComponent();

            isDoingInitializeComponent = false;
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
                //if (_itemDnD != itemOver)
                //ListViewItem newItem = (ListViewItem)data.DragItems[i];
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
    }
}
