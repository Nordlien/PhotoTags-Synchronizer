using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Manina.Windows.Forms;
using Krypton.Toolkit;

namespace PhotoTagsSynchronizer
{
    public partial class FormChooseColumns : Form
    {
        public ImageListView imageListView;

        private bool isPopulating = true;
        public FormChooseColumns()
        {
            InitializeComponent();            
        }

        public void Populate(int selectedIndex)
        {
            isPopulating = true;
            this.kryptonWorkspaceCell1.SelectedIndex = selectedIndex;

            PopulateComboBox(comboBoxTitleLine1);
            comboBoxTitleLine1.Text = Properties.Settings.Default.ImageListViewTitleLine1;
            imageListView.TitleLine1 = GetColumnTypeByText(Properties.Settings.Default.ImageListViewTitleLine1);

            PopulateComboBox(comboBoxTitleLine2);
            comboBoxTitleLine2.Text = Properties.Settings.Default.ImageListViewTitleLine2;
            imageListView.TitleLine2 = GetColumnTypeByText(Properties.Settings.Default.ImageListViewTitleLine2);

            PopulateComboBox(comboBoxTitleLine3);
            comboBoxTitleLine3.Text = Properties.Settings.Default.ImageListViewTitleLine3;
            imageListView.TitleLine3 = GetColumnTypeByText(Properties.Settings.Default.ImageListViewTitleLine3);

            PopulateComboBox(comboBoxTitleLine4);
            comboBoxTitleLine4.Text = Properties.Settings.Default.ImageListViewTitleLine4;
            imageListView.TitleLine4 = GetColumnTypeByText(Properties.Settings.Default.ImageListViewTitleLine4);

            PopulateComboBox(comboBoxTitleLine5);
            comboBoxTitleLine5.Text = Properties.Settings.Default.ImageListViewTitleLine5;
            imageListView.TitleLine5 = GetColumnTypeByText(Properties.Settings.Default.ImageListViewTitleLine5);

            isPopulating = false;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ImageListViewSelectedColumns = ImageListViewHandler.ImageListViewStringCollection(imageListView);
            Properties.Settings.Default.ImageListViewTitleLine1 = comboBoxTitleLine1.Text;
            Properties.Settings.Default.ImageListViewTitleLine2 = comboBoxTitleLine2.Text;
            Properties.Settings.Default.ImageListViewTitleLine3 = comboBoxTitleLine3.Text;
            Properties.Settings.Default.ImageListViewTitleLine4 = comboBoxTitleLine4.Text;
            Properties.Settings.Default.ImageListViewTitleLine5 = comboBoxTitleLine5.Text;
            Close();
        }

        private void checkedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            ImageListView.ImageListViewColumnHeader column = imageListView.Columns[e.Index];
            column.Visible = (e.NewValue == CheckState.Checked);
        }

        private void ChooseColumns_Load(object sender, EventArgs e)
        {
            foreach (ImageListView.ImageListViewColumnHeader column in imageListView.Columns)
            {
                int index = checkedListBox.Items.Add(column.Text);

                foreach (ImageListView.ImageListViewColumnHeader item in imageListView.Columns)
                {
                    if (column.Visible && column.Text == item.Text) checkedListBox.SetItemChecked(index, true);
                }

            }
        }

        private void PopulateComboBox(KryptonComboBox comboBox)
        {
            //JTN: MediaFileAttributes 
            comboBox.Items.Clear();
            comboBox.Items.Add("FileName");
            comboBox.Items.Add("FileDate");
            comboBox.Items.Add("FileSmartDate");
            comboBox.Items.Add("FileDateCreated");
            comboBox.Items.Add("FileDateModified");

            comboBox.Items.Add("MediaDateTaken");
            comboBox.Items.Add("LocationDateTime");
            comboBox.Items.Add("LocationTimeZone");

            comboBox.Items.Add("FileType");
            comboBox.Items.Add("FileFullPath");
            comboBox.Items.Add("FileDirectory");
            comboBox.Items.Add("FileSize");

            comboBox.Items.Add("MediaAlbum");
            comboBox.Items.Add("MediaTitle");
            comboBox.Items.Add("MediaDescription");
            comboBox.Items.Add("MediaComment");
            comboBox.Items.Add("MediaAuthor");
            comboBox.Items.Add("MediaRating");

            comboBox.Items.Add("LocationName");
            comboBox.Items.Add("LocationRegionState");
            comboBox.Items.Add("LocationCity");
            comboBox.Items.Add("LocationCountry");

            comboBox.Items.Add("CameraMake");
            comboBox.Items.Add("CameraModel");

            comboBox.Items.Add("MediaDimensions");            
        }

        public static ColumnType GetColumnTypeByText(string text)
        {
            switch (text)
            {   
                //JTN: MediaFileAttributes                       
                case "FileDate": return ColumnType.FileDate;
                case "FileSmartDate": return ColumnType.FileSmartDate;
                case "FileDateCreated": return ColumnType.FileDateCreated;
                case "FileDateModified": return ColumnType.FileDateModified;
                case "MediaDateTaken": return ColumnType.MediaDateTaken;
                case "FileType": return ColumnType.FileType;
                case "FileFullPath": return ColumnType.FileFullPath;
                case "FileDirectory": return ColumnType.FileDirectory;
                case "FileSize": return ColumnType.FileSize;
                case "MediaAlbum": return ColumnType.MediaAlbum;
                case "MediaTitle": return ColumnType.MediaTitle;
                case "MediaDescription": return ColumnType.MediaDescription;
                case "MediaComment": return ColumnType.MediaComment;
                case "MediaAuthor": return ColumnType.MediaAuthor;
                case "MediaRating": return ColumnType.MediaRating;
                case "LocationDateTime": return ColumnType.LocationDateTime;
                case "LocationTimeZone": return ColumnType.LocationTimeZone;
                case "LocationName": return ColumnType.LocationName;
                case "LocationRegionState": return ColumnType.LocationRegionState;
                case "LocationCity": return ColumnType.LocationCity;
                case "LocationCountry": return ColumnType.LocationCountry;
                case "CameraMake": return ColumnType.CameraMake;
                case "CameraModel": return ColumnType.CameraModel;
                case "MediaDimensions": return ColumnType.MediaDimensions;
                default: /*"FileName":*/ return ColumnType.FileName;
            }
        }

        private static string GetTextOfColumnType(ColumnType columnType)
        {
            switch (columnType)
            {
                //JTN: MediaFileAttributes 
                case ColumnType.FileDate: return "FileDate";
                case ColumnType.FileSmartDate: return "FileSmartDate";
                case ColumnType.FileDateCreated: return "FileDateCreated";
                case ColumnType.FileDateModified: return "FileDateModified";
                case ColumnType.MediaDateTaken: return "MediaDateTaken";
                case ColumnType.FileType: return "FileType";
                case ColumnType.FileFullPath: return "FileFullPath";
                case ColumnType.FileDirectory: return "FileDirectory";
                case ColumnType.FileSize: return "FileSize";
                case ColumnType.MediaAlbum: return "MediaAlbum";
                case ColumnType.MediaTitle: return "MediaTitle";
                case ColumnType.MediaDescription: return "MediaDescription";
                case ColumnType.MediaComment: return "MediaComment";
                case ColumnType.MediaAuthor: return "MediaAuthor";
                case ColumnType.MediaRating: return "MediaRating";
                case ColumnType.LocationDateTime: return "LocationDateTime";
                case ColumnType.LocationTimeZone: return "LocationTimeZone";
                case ColumnType.LocationName: return "LocationName";
                case ColumnType.LocationRegionState: return "LocationRegionState";
                case ColumnType.LocationCity: return "LocationCity";
                case ColumnType.LocationCountry: return "LocationCountry";
                case ColumnType.CameraMake: return "CameraMake";
                case ColumnType.CameraModel: return "CameraModel";
                case ColumnType.MediaDimensions: return "MediaDimensions";
                default: /*"FileName":*/ return "FileName";
            }
        }

        private void GetComboBoxValues()
        {
            if (isPopulating) return;

            imageListView.TitleLine1 = GetColumnTypeByText(comboBoxTitleLine1.Text);
            imageListView.TitleLine2 = GetColumnTypeByText(comboBoxTitleLine2.Text);
            imageListView.TitleLine3 = GetColumnTypeByText(comboBoxTitleLine3.Text);
            imageListView.TitleLine4 = GetColumnTypeByText(comboBoxTitleLine4.Text);
            imageListView.TitleLine5 = GetColumnTypeByText(comboBoxTitleLine5.Text);
        }

        private void comboBoxTitleLine1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            GetComboBoxValues();
        }

        private void comboBoxTitleLine2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            GetComboBoxValues();
        }

        private void comboBoxTitleLine3_SelectionChangeCommitted(object sender, EventArgs e)
        {
            GetComboBoxValues();
        }

        private void comboBoxTitleLine4_SelectionChangeCommitted(object sender, EventArgs e)
        {
            GetComboBoxValues();
        }

        private void comboBoxTitleLine5_SelectionChangeCommitted(object sender, EventArgs e)
        {
            GetComboBoxValues();
        }
    }

    
}
