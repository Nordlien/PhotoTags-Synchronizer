using System;
using System.Windows.Forms;
using System.Collections.Generic;
using MetadataLibrary;
using Krypton.Toolkit;
using System.Drawing;

namespace PhotoTagsSynchronizer
{ 
    public partial class MainForm : KryptonForm
    {
        #region TreeViewFilter - BeforeCheck - Not allow all fileres to be checked
        bool isCorrectingDoubleClikcBug = false;
        private void treeViewFilter_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            if (GlobalData.IsApplicationClosing) e.Cancel = true;
            if (GlobalData.DoNotTrigger_TreeViewFilter_BeforeAndAfterCheck) e.Cancel = true;
            else
            {
                if (IsPopulatingAnything("Selecting Filter")) e.Cancel = true;
                if (SaveBeforeContinue(true) == DialogResult.Cancel) e.Cancel = true;
            }
            if (!e.Cancel)
            {
                GlobalData.IsPerformingAButtonAction = true;

                if (isCorrectingDoubleClikcBug) return;

                TreeNode treeNode = e.Node;
                if (treeNode.Tag is int)
                {
                    if ((int)treeNode.Tag == FilterVerifyer.TagRegionOr) e.Cancel = true;
                }
            }
        }
        #endregion 

        #region TreeViewFilter - AfterCheck
        private void treeViewFilter_AfterCheck(object sender, TreeViewEventArgs e)
        {
            //Hack for double - click bug
            TreeNode treeNode = e.Node;
            if (treeNode.Tag is int)
            {

                if ((int)treeNode.Tag == FilterVerifyer.TagRegionOrAnd ||
                    (int)treeNode.Tag == FilterVerifyer.TagRoot) treeNode.Text = FilterVerifyer.GetTreeNodeText(GlobalData.SearchFolder, treeNode.Name, treeNode.Checked);
            }

            GlobalData.IsPerformingAButtonAction = false;

            //GlobalData.DoNotTrigger_ImageListView_SelectionChanged = true;
            //GlobalData.IsPopulatingImageListViewFromFolderOrDatabaseList = true;
            ImageListView_Aggregate_UsingFiltersOnExistingFiles(treeViewFilter);
            //GlobalData.DoNotTrigger_ImageListView_SelectionChanged = false;
            //GlobalData.IsPopulatingImageListViewFromFolderOrDatabaseList = false;
        }
        #endregion

        #region Filter Helper - FilterReplaceNullWithIsNotDefineText
        private void FilterReplaceNullWithIsNotDefineText(List<string> list)
        {
            if (list.Contains(null)) list.Remove(null);
            if (!list.Contains("(Is not defined)")) list.Insert(0, "(Is not defined)");
        }
        #endregion

        #region Filter Helper - PopulateDatabaseFilter
        public void PopulateDatabaseFilter()
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action(PopulateDatabaseFilter));
                return;
            }

            comboBoxSearchKeyword.Text = "";
            comboBoxSearchKeyword.Items.Clear();
            comboBoxSearchKeyword.Items.Add("Example1;Example2");
            
            List<string> albums = databaseAndCacheMetadataExiftool.ListAllPersonalAlbumsCache(MetadataBrokerType.ExifTool);
            albums.Sort();
            FilterReplaceNullWithIsNotDefineText(albums);
            comboBoxSearchAlbum.SuspendLayout();
            comboBoxSearchAlbum.Text = null;
            comboBoxSearchAlbum.Items.Clear();
            comboBoxSearchAlbum.Items.AddRange(albums.ToArray());
            comboBoxSearchAlbum.ResumeLayout();
            
            //List<string> authors = databaseAndCacheMetadataExiftool.ListAllPersonalAuthors(MetadataBrokerType.ExifTool);
            //authors.Sort();
            //ListViewRemoveNull(authors);
            //comboBoxSearchAuthor.Items.Clear();
            //comboBoxSearchAuthor.Items.AddRange(authors.ToArray());

            List<string> comments = databaseAndCacheMetadataExiftool.ListAllPersonalCommentsCache(MetadataBrokerType.ExifTool);
            comments.Sort();
            FilterReplaceNullWithIsNotDefineText(comments);
            comboBoxSearchComments.SuspendLayout();
            comboBoxSearchComments.Text = "";
            comboBoxSearchComments.Items.Clear();
            comboBoxSearchComments.Items.AddRange(comments.ToArray());
            comboBoxSearchComments.ResumeLayout();

            List<string> descriptions = databaseAndCacheMetadataExiftool.ListAllPersonalDescriptionsCache(MetadataBrokerType.ExifTool);
            descriptions.Sort();
            FilterReplaceNullWithIsNotDefineText(descriptions);
            comboBoxSearchDescription.SuspendLayout();
            comboBoxSearchDescription.Text = "";
            comboBoxSearchDescription.Items.Clear();
            comboBoxSearchDescription.Items.AddRange(descriptions.ToArray());
            comboBoxSearchDescription.ResumeLayout();

            List<string> titles = databaseAndCacheMetadataExiftool.ListAllPersonalTitlesCache(MetadataBrokerType.ExifTool);
            titles.Sort();
            FilterReplaceNullWithIsNotDefineText(titles);
            comboBoxSearchTitle.SuspendLayout();
            comboBoxSearchTitle.Text = "";
            comboBoxSearchTitle.Items.Clear();
            comboBoxSearchTitle.Items.AddRange(titles.ToArray());
            comboBoxSearchTitle.ResumeLayout();

            List<string> locations = databaseAndCacheMetadataExiftool.ListAllLocationNamesCache(MetadataBrokerType.ExifTool);
            locations.Sort();
            FilterReplaceNullWithIsNotDefineText(locations);
            comboBoxSearchLocationName.SuspendLayout();
            comboBoxSearchLocationName.Text = "";
            comboBoxSearchLocationName.Items.Clear();
            comboBoxSearchLocationName.Items.AddRange(locations.ToArray());
            comboBoxSearchLocationName.ResumeLayout();

            List<string> cities = databaseAndCacheMetadataExiftool.ListAllLocationCitiesCache(MetadataBrokerType.ExifTool);
            cities.Sort();
            FilterReplaceNullWithIsNotDefineText(cities);
            comboBoxSearchLocationCity.SuspendLayout();
            comboBoxSearchLocationCity.Text = "";
            comboBoxSearchLocationCity.Items.Clear();
            comboBoxSearchLocationCity.Items.AddRange(cities.ToArray());
            comboBoxSearchLocationCity.ResumeLayout();

            List<string> states = databaseAndCacheMetadataExiftool.ListAllLocationStatesCache(MetadataBrokerType.ExifTool);
            states.Sort();
            FilterReplaceNullWithIsNotDefineText(states);
            comboBoxSearchLocationState.SuspendLayout();
            comboBoxSearchLocationState.Text = "";
            comboBoxSearchLocationState.Items.Clear();
            comboBoxSearchLocationState.Items.AddRange(states.ToArray());
            comboBoxSearchLocationState.ResumeLayout();

            List<string> countries = databaseAndCacheMetadataExiftool.ListAllLocationCountriesCache(MetadataBrokerType.ExifTool);
            countries.Sort();
            FilterReplaceNullWithIsNotDefineText(countries);
            comboBoxSearchLocationCountry.SuspendLayout();
            comboBoxSearchLocationCountry.Text = "";
            comboBoxSearchLocationCountry.Items.Clear();
            comboBoxSearchLocationCountry.Items.AddRange(countries.ToArray());
            comboBoxSearchLocationCountry.ResumeLayout();

            List<string> peoples = databaseAndCacheMetadataExiftool.ListAllPersonalRegionNameCache(MetadataBrokerType.ExifTool);
            peoples.Sort();
            FilterReplaceNullWithIsNotDefineText(peoples);
            checkedListBoxSearchPeople.SuspendLayout();
            checkedListBoxSearchPeople.Items.Clear();

            

            foreach (string people in peoples)
            {
                KryptonListItem item = new KryptonListItem();
                item.ShortText = people;
                item.LongText = "";
                Image image = databaseAndCacheMetadataExiftool.ReadRandomThumbnailFromCacheOrDatabase(people);
                if (image != null) item.Image = image;
                checkedListBoxSearchPeople.Items.Add(item);
            }
            if (checkedListBoxSearchPeople.Items.Count == 0)
            {
                checkedListBoxSearchPeople.Items.Insert(0, "No people added yet");
                checkedListBoxSearchPeople.SelectedItem = checkedListBoxSearchPeople.Items[0];
            }

            checkedListBoxSearchPeople.ResumeLayout();
        }
        #endregion 

        #region Filter Helper - PopulateTreeViewFolderFilter - Add - Invoke
        private void PopulateTreeViewFolderFilterAdd(Metadata metadata) 
        {
            if (metadata != null) FilterVerifyer.PopulateTreeViewFolderFilterAdd(metadata);
        }
        #endregion 

        #region Filter Helper - PopulateTreeViewFolderFilterUpdatedTreeViewFilterInvoke
        private void PopulateTreeViewFolderFilterUpdatedTreeViewFilterInvoke()
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action(PopulateTreeViewFolderFilterUpdatedTreeViewFilterInvoke));
                return;
            }

            FilterVerifyer.PopulateTreeViewFilterWithValues(treeViewFilter); 
        }
        #endregion

    }
}

