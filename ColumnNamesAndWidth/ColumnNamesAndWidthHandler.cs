using Manina.Windows.Forms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ColumnNamesAndWidth
{
    #region ImageListViewHandler
    public class ColumnNamesAndWidthHandler
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
                    column.Visible = Array.IndexOf(valueList, column.Text) >= 0;
                }
            }
        }
        #endregion

        #region ImageListView - Settings - Convert List to string
        public static List<ColumnNameAndWidth> GetColumnNameAndWidths(ImageListView imageListView)
        {
            List<ColumnNameAndWidth> columnNameAndWidths = new List<ColumnNameAndWidth>();
            foreach (ImageListView.ImageListViewColumnHeader item in imageListView.Columns)
            {
                columnNameAndWidths.Add(new ColumnNameAndWidth(item.Text, item.Width));
            }
            return columnNameAndWidths;
        }
        #endregion

        #region ImageListView - Settings - Convert List to string
        public static void SetColumnNameWithWidth(List<ColumnNameAndWidth> columnNameAndWidths, ImageListView imageListView)
        {
            if (columnNameAndWidths == null) return;
            foreach (ColumnNameAndWidth columnNameAndWidth in columnNameAndWidths)
            {
                foreach (ImageListView.ImageListViewColumnHeader item in imageListView.Columns)
                {
                    if (item.Text == columnNameAndWidth.Name) item.Width = columnNameAndWidth.Width;
                }
            }
        }
        #endregion

        #region ImageListView - Settings - Convert List to string
        public static int GetColumnWidth(List<ColumnNameAndWidth> columnNameAndWidths, string name, int defaultWith)
        {
            if (columnNameAndWidths == null) return defaultWith;
            foreach (ColumnNameAndWidth columnNameAndWidth in columnNameAndWidths)
            {
                if (columnNameAndWidth.Name == name) return columnNameAndWidth.Width;                
            }
            return defaultWith;
        }
        #endregion

        #region ConvertColumnNameAndWidthsToConfigString
        public static string ConvertColumnNameAndWidthsToConfigString(List<ColumnNameAndWidth> columnNameAndWidths)
        {
            return JsonConvert.SerializeObject(columnNameAndWidths, Formatting.Indented);
        }
        #endregion

        #region ConvertConfigStringToColumnNameAndWidths
        public static List<ColumnNameAndWidth> ConvertConfigStringToColumnNameAndWidths(string configString)
        {
            List<ColumnNameAndWidth> columnNameAndWidths = new List<ColumnNameAndWidth>();
            try
            {
                columnNameAndWidths = JsonConvert.DeserializeObject<List<ColumnNameAndWidth>>(configString);
                if (columnNameAndWidths == null) columnNameAndWidths = new List<ColumnNameAndWidth>();
            }
            catch { }
            return columnNameAndWidths;
        }
        #endregion
    }
    #endregion 
}
