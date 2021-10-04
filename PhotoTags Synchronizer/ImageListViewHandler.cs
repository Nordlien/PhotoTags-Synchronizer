using Manina.Windows.Forms;
using System;

namespace PhotoTagsSynchronizer
{
    #region ImageListViewHandler
    public class ImageListViewHandler
    {
        #region ImageListView - Settings - Convert List to string
        public static string ImageListViewStringCollection(ImageListView imageListView)
        {
            string resultListString = "";
            foreach (ImageListView.ImageListViewColumnHeader item in imageListView.Columns)
            {
                if (item.Visible) resultListString += (resultListString == "" ? "" : "\r\n") + item.Text.ToString();
            }
            return resultListString;
        }
        #endregion

        #region CheckedListBox - Settings - Convert String add to CheckedListBox
        public static void SetImageListViewCheckedValues(ImageListView imageListView, string valueListString)
        {
            if (string.IsNullOrWhiteSpace(valueListString)) valueListString = "";

            string[] valueList = valueListString.Replace("\r\n", "\n").Split('\n');

            if (valueList != null)
            {
                for (int index = 0; index < imageListView.Columns.Count; index++)
                {

                    ImageListView.ImageListViewColumnHeader column = imageListView.Columns[index];
                    column.Visible = Array.IndexOf(valueList, column.Text) > 0;
                }
            }
        }
        #endregion
    }
    #endregion 
}



