using Manina.Windows.Forms;
using static Manina.Windows.Forms.ImageListView;

namespace PhotoTagsSynchronizer
{
    public static class ImageListViewHandler
    {
        #region ImageListView - FindItem
        public static ImageListViewItem FindItemInImageListView(ImageListViewItemCollection imageListViewItemCollection, string fullFilename)
        {
            ImageListViewItem foundItem = null;

            foreach (ImageListViewItem item in imageListViewItemCollection)
            {
                if (item.FileFullPath == fullFilename)
                {
                    foundItem = item;
                    break;
                }
            }
            return foundItem;
        }
        #endregion

        #region ImageListView - SetItemDirty
        public static void ImageListViewSetItemDirty(ImageListView imageListView, string fullfilename)
        {
            ImageListViewItem imageListViewItem = FindItemInImageListView(imageListView.Items, fullfilename);
            if (imageListViewItem != null)
            {
                imageListViewItem.Dirty();

            }
            imageListView.Refresh();
        }
        #endregion

        #region ImageListViewEnable
        public static void Enable(ImageListView imageListView, bool enabled)
        {
            //imageListView.Enabled = enabled;
        }
        #endregion
    }
}
