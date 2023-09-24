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
            try
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion 

        #region TreeViewFilter - AfterCheck
        private void treeViewFilter_AfterCheck(object sender, TreeViewEventArgs e)
        {
            try
            {
                //Hack for double - click bug
                TreeNode treeNode = e.Node;
                if (treeNode.Tag is int)
                {

                    if ((int)treeNode.Tag == FilterVerifyer.TagRegionOrAnd ||
                        (int)treeNode.Tag == FilterVerifyer.TagRoot) treeNode.Text = FilterVerifyer.GetTreeNodeText(GlobalData.SearchFolder, treeNode.Name, treeNode.Checked);
                }

                GlobalData.IsPerformingAButtonAction = false;
                ImageListView_Aggregate_UsingFiltersOnExistingFiles(treeViewFilter);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region Filter Helper - FilterReplaceNullWithIsNotDefineText
        private void FilterReplaceNullWithIsNotDefineText(List<string> list, bool addIsNotDefined, bool addEmpty = true, bool moveEmptyToTop = true)
        {
            try
            {
                if (list.Contains(null)) list.Remove(null);

                if (addEmpty && !list.Contains("")) list.Add("");

                if (moveEmptyToTop)
                {
                    if (list.Contains(""))
                    {
                        list.Remove("");
                        list.Insert(0, "");
                    }
                }

                if (addIsNotDefined && !list.Contains("(Is not defined)")) list.Insert(0, "(Is not defined)");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
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

            try
            {
                comboBoxSearchKeyword.Text = "";
                comboBoxSearchKeyword.Items.Clear();
                comboBoxSearchKeyword.Items.Add("Example1;Example2");

                List<string> albums = databaseAndCacheMetadataExiftool.ListAllPersonalAlbumsCache(MetadataBrokerType.UserSavedData);
                albums.Sort();
                FilterReplaceNullWithIsNotDefineText(albums, addIsNotDefined: true, addEmpty: true, moveEmptyToTop: true);
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

                List<string> comments = databaseAndCacheMetadataExiftool.ListAllPersonalCommentsCache(MetadataBrokerType.UserSavedData);
                comments.Sort();
                FilterReplaceNullWithIsNotDefineText(comments, addIsNotDefined: true, addEmpty: true, moveEmptyToTop: true);
                comboBoxSearchComments.SuspendLayout();
                comboBoxSearchComments.Text = "";
                comboBoxSearchComments.Items.Clear();
                comboBoxSearchComments.Items.AddRange(comments.ToArray());
                comboBoxSearchComments.ResumeLayout();

                List<string> descriptions = databaseAndCacheMetadataExiftool.ListAllPersonalDescriptionsCache(MetadataBrokerType.UserSavedData);
                descriptions.Sort();
                FilterReplaceNullWithIsNotDefineText(descriptions, addIsNotDefined: true, addEmpty: true, moveEmptyToTop: true);
                comboBoxSearchDescription.SuspendLayout();
                comboBoxSearchDescription.Text = "";
                comboBoxSearchDescription.Items.Clear();
                comboBoxSearchDescription.Items.AddRange(descriptions.ToArray());
                comboBoxSearchDescription.ResumeLayout();

                List<string> titles = databaseAndCacheMetadataExiftool.ListAllPersonalTitlesCache(MetadataBrokerType.UserSavedData);
                titles.Sort();
                FilterReplaceNullWithIsNotDefineText(titles, addIsNotDefined: true, addEmpty: true, moveEmptyToTop: true);
                comboBoxSearchTitle.SuspendLayout();
                comboBoxSearchTitle.Text = "";
                comboBoxSearchTitle.Items.Clear();
                comboBoxSearchTitle.Items.AddRange(titles.ToArray());
                comboBoxSearchTitle.ResumeLayout();

                List<string> locations = databaseAndCacheMetadataExiftool.ListAllLocationNamesCache(MetadataBrokerType.UserSavedData);
                locations.Sort();
                FilterReplaceNullWithIsNotDefineText(locations, addIsNotDefined: true, addEmpty: true, moveEmptyToTop: true);
                comboBoxSearchLocationName.SuspendLayout();
                comboBoxSearchLocationName.Text = "";
                comboBoxSearchLocationName.Items.Clear();
                comboBoxSearchLocationName.Items.AddRange(locations.ToArray());
                comboBoxSearchLocationName.ResumeLayout();

                List<string> cities = databaseAndCacheMetadataExiftool.ListAllLocationCitiesCache(MetadataBrokerType.UserSavedData);
                cities.Sort();
                FilterReplaceNullWithIsNotDefineText(cities, addIsNotDefined: true, addEmpty: true, moveEmptyToTop: true);
                comboBoxSearchLocationCity.SuspendLayout();
                comboBoxSearchLocationCity.Text = "";
                comboBoxSearchLocationCity.Items.Clear();
                comboBoxSearchLocationCity.Items.AddRange(cities.ToArray());
                comboBoxSearchLocationCity.ResumeLayout();

                List<string> states = databaseAndCacheMetadataExiftool.ListAllLocationStatesCache(MetadataBrokerType.UserSavedData);
                states.Sort();
                FilterReplaceNullWithIsNotDefineText(states, addIsNotDefined: true, addEmpty: true, moveEmptyToTop: true);
                comboBoxSearchLocationState.SuspendLayout();
                comboBoxSearchLocationState.Text = "";
                comboBoxSearchLocationState.Items.Clear();
                comboBoxSearchLocationState.Items.AddRange(states.ToArray());
                comboBoxSearchLocationState.ResumeLayout();

                List<string> countries = databaseAndCacheMetadataExiftool.ListAllLocationCountriesCache(MetadataBrokerType.UserSavedData);
                countries.Sort();
                FilterReplaceNullWithIsNotDefineText(countries, addIsNotDefined: true, addEmpty: true, moveEmptyToTop: true);
                comboBoxSearchLocationCountry.SuspendLayout();
                comboBoxSearchLocationCountry.Text = "";
                comboBoxSearchLocationCountry.Items.Clear();
                comboBoxSearchLocationCountry.Items.AddRange(countries.ToArray());
                comboBoxSearchLocationCountry.ResumeLayout();

                List<string> peoples = databaseAndCacheMetadataExiftool.ListAllPersonalRegionNameCache(MetadataBrokerType.UserSavedData);
                peoples.Sort();

                FilterReplaceNullWithIsNotDefineText(peoples, addIsNotDefined: true, addEmpty: false, moveEmptyToTop: false);
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion 

        #region Filter Helper - PopulateTreeViewFolderFilter - Add - Invoke
        private void PopulateTreeViewFolderFilterAdd(Metadata metadata) 
        {
            try
            {
                if (metadata != null) FilterVerifyer.PopulateTreeViewFolderFilterAdd(metadata);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
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

            try
            {
                FilterVerifyer.PopulateTreeViewFilterWithValues(treeViewFilter);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

    }
}

