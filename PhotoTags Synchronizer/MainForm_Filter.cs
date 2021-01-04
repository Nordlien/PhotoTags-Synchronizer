using System;
using System.Windows.Forms;
using System.Collections.Generic;
using MetadataLibrary;
using Manina.Windows.Forms;

namespace PhotoTagsSynchronizer
{
    
    public class Filter
    {
        public bool IsAndBetweenValues { get; set; }
        public string FieldName { get; set; }
        public List<string> FilterValues { get; set; } = new List<string>();

        public Filter(bool isAndBetweenValues, string fieldName)
        {
            IsAndBetweenValues = isAndBetweenValues;
            FieldName = fieldName;
        }

        public void AddValue(string value)
        {
            if (!FilterValues.Contains(value)) FilterValues.Add(value);
        }
    }

    public class FilterVerifyer
    {
        public const string Root = "Root";
        public const string Albums = "Albums";
        public const string Titles = "Titles";
        public const string Comments = "Comments";
        public const string Descriptions = "Descriptions";
        public const string Authors = "Authors";
        public const string Ratings = "Ratings";
        public const string Dates = "Dates";

        public const string Locations = "Locations";
        public const string Cities = "Cities";
        public const string States = "States";
        public const string Countries = "Countries";

        public const string Peoples = "Peoples";
        public const string Keywords = "Keywords";

        public bool IsAndBewteenFieldTags { get; set; } = true;
        public List<Filter> filters = new List<Filter>();

        public FilterVerifyer()
        {
        }

        public void Add (Filter filter)
        {
            filters.Add(filter);
        }

        public static string GetTreeNodeText(bool searchFolder, string treeNodeName, bool treeNodeChecked)
        {
            switch (treeNodeName)
            {
                case FilterVerifyer.Root:
                    return (searchFolder ? "Folder filter " : "Search filter ") + (treeNodeChecked ? " when all sub-groups fits" : " when some sub-group fits");
                case FilterVerifyer.Albums:
                case FilterVerifyer.Titles:
                case FilterVerifyer.Comments:
                case FilterVerifyer.Descriptions:
                case FilterVerifyer.Authors:
                case FilterVerifyer.Ratings:
                case FilterVerifyer.Dates:
                case FilterVerifyer.Locations:
                case FilterVerifyer.Cities:
                case FilterVerifyer.States:
                case FilterVerifyer.Countries:
                    return treeNodeName + (treeNodeChecked ? " - added" : "");
                case FilterVerifyer.Peoples: 
                    return (treeNodeChecked ? "When contains all this " : "When has some of this ") + treeNodeName;
                case FilterVerifyer.Keywords: 
                    return (treeNodeChecked ? "When contains all this " : "When has some of this ") + treeNodeName;            
            }
            return treeNodeName;
        }
        
        public static void PopulateTreeViewBasicNodes(TreeView treeView, string rootNode)
        {
            treeView.Nodes.Clear();
            TreeNode treeNode;

            treeNode = treeView.Nodes.Add(FilterVerifyer.Root, FilterVerifyer.GetTreeNodeText(GlobalData.SearchFolder, FilterVerifyer.Root, true));
            treeNode.Checked = true;

            treeView.Nodes[rootNode].Nodes.Add(FilterVerifyer.Albums, FilterVerifyer.GetTreeNodeText(GlobalData.SearchFolder, FilterVerifyer.Albums, false));
            treeNode = treeView.Nodes[rootNode].Nodes.Add(FilterVerifyer.Titles, FilterVerifyer.GetTreeNodeText(GlobalData.SearchFolder, FilterVerifyer.Titles, false));
            treeNode = treeView.Nodes[rootNode].Nodes.Add(FilterVerifyer.Comments, FilterVerifyer.GetTreeNodeText(GlobalData.SearchFolder, FilterVerifyer.Comments, false));
            treeNode = treeView.Nodes[rootNode].Nodes.Add(FilterVerifyer.Descriptions, FilterVerifyer.GetTreeNodeText(GlobalData.SearchFolder, FilterVerifyer.Descriptions, false));
            treeNode = treeView.Nodes[rootNode].Nodes.Add(FilterVerifyer.Authors, FilterVerifyer.GetTreeNodeText(GlobalData.SearchFolder, FilterVerifyer.Authors, false));
            treeNode = treeView.Nodes[rootNode].Nodes.Add(FilterVerifyer.Ratings, FilterVerifyer.GetTreeNodeText(GlobalData.SearchFolder, FilterVerifyer.Ratings, false));
            treeNode = treeView.Nodes[rootNode].Nodes.Add(FilterVerifyer.Dates, FilterVerifyer.GetTreeNodeText(GlobalData.SearchFolder, FilterVerifyer.Dates, false));
            treeNode = treeView.Nodes[rootNode].Nodes.Add(FilterVerifyer.Locations, FilterVerifyer.GetTreeNodeText(GlobalData.SearchFolder, FilterVerifyer.Locations, false));
            treeNode = treeView.Nodes[rootNode].Nodes.Add(FilterVerifyer.Cities, FilterVerifyer.GetTreeNodeText(GlobalData.SearchFolder, FilterVerifyer.Cities, false));
            treeNode = treeView.Nodes[rootNode].Nodes.Add(FilterVerifyer.States, FilterVerifyer.GetTreeNodeText(GlobalData.SearchFolder, FilterVerifyer.States, false));
            treeNode = treeView.Nodes[rootNode].Nodes.Add(FilterVerifyer.Countries, FilterVerifyer.GetTreeNodeText(GlobalData.SearchFolder, FilterVerifyer.Countries, false));
            treeNode = treeView.Nodes[rootNode].Nodes.Add(FilterVerifyer.Peoples, FilterVerifyer.GetTreeNodeText(GlobalData.SearchFolder, FilterVerifyer.Peoples, true));
            treeNode.Checked = true;
            treeNode = treeView.Nodes[rootNode].Nodes.Add(FilterVerifyer.Keywords, FilterVerifyer.GetTreeNodeText(GlobalData.SearchFolder, FilterVerifyer.Keywords, true));
            treeNode.Checked = true;
        }

        public static void PopulateTreeViewWithValues(TreeView treeView, string keyRoot, string key, List<string> nodes)
        {
            foreach (string node in nodes)
            {
                TreeNode treeNode = treeView.Nodes[keyRoot].Nodes[key].Nodes.Add(node, FilterVerifyer.GetTreeNodeText(GlobalData.SearchFolder, node, false));
            }
        }

        #region Read Values - From One Tree Node
        public int ReadValuesFromTreeNodes(TreeView treeView, string rootNode, string tagNode)
        {
            int valuesCountAdded = 0;
            if (treeView.Nodes[rootNode] == null) return valuesCountAdded;
            if (treeView.Nodes[rootNode].Nodes[tagNode] == null) return valuesCountAdded;
            
            bool isAndBetweenValues = treeView.Nodes[rootNode].Nodes[tagNode].Checked;

            Filter filter = new Filter(isAndBetweenValues, tagNode);

            foreach (TreeNode treeNode in treeView.Nodes[rootNode].Nodes[tagNode].Nodes)
            {
                if (treeNode.Checked)
                {
                    filter.AddValue(treeNode.Name);
                    valuesCountAdded++;
                }
            }
            Add(filter);
            return valuesCountAdded;
        }
        #endregion 

        #region Read Values - From All Tree Nodes 
        public int ReadValuesFromRootNodesWithChilds(TreeView treeView, string rootNode)
        {
            int valuesCountAdded = 0;
            IsAndBewteenFieldTags = (treeView.Nodes[rootNode] == null ? true : treeView.Nodes[rootNode].Checked);
            valuesCountAdded += ReadValuesFromTreeNodes(treeView, rootNode, FilterVerifyer.Albums);
            valuesCountAdded += ReadValuesFromTreeNodes(treeView, rootNode, FilterVerifyer.Titles);
            valuesCountAdded += ReadValuesFromTreeNodes(treeView, rootNode, FilterVerifyer.Comments);
            valuesCountAdded += ReadValuesFromTreeNodes(treeView, rootNode, FilterVerifyer.Descriptions);
            valuesCountAdded += ReadValuesFromTreeNodes(treeView, rootNode, FilterVerifyer.Authors);
            valuesCountAdded += ReadValuesFromTreeNodes(treeView, rootNode, FilterVerifyer.Ratings);
            valuesCountAdded += ReadValuesFromTreeNodes(treeView, rootNode, FilterVerifyer.Dates);

            valuesCountAdded += ReadValuesFromTreeNodes(treeView, rootNode, FilterVerifyer.Locations);
            valuesCountAdded += ReadValuesFromTreeNodes(treeView, rootNode, FilterVerifyer.Cities);
            valuesCountAdded += ReadValuesFromTreeNodes(treeView, rootNode, FilterVerifyer.States);
            valuesCountAdded += ReadValuesFromTreeNodes(treeView, rootNode, FilterVerifyer.Countries);

            valuesCountAdded += ReadValuesFromTreeNodes(treeView, rootNode, FilterVerifyer.Peoples);
            valuesCountAdded += ReadValuesFromTreeNodes(treeView, rootNode, FilterVerifyer.Keywords);

            return valuesCountAdded;
        }
        #endregion 

        #region IndexOfFilter
        public int IndexOfFilter(string fieldName)
        {
            for (int index = 0; index < filters.Count; index++)
            {
                if (filters[index].FieldName == fieldName) return index;
            }
            return -1;
        }
        #endregion

        #region Verify Metadata agaist Read filter in class
        public bool VerifyMetadata(Metadata metadata)
        {
            if (metadata == null) return true;

            int indexAlbum = IndexOfFilter(Albums);
            bool foundAlbum;
            bool hasValueAlbum = (indexAlbum == -1 ? false : filters[indexAlbum].FilterValues.Count > 0);
            if (indexAlbum == -1 || filters[indexAlbum].FilterValues.Count == 0) foundAlbum = IsAndBewteenFieldTags;
            else foundAlbum = filters[indexAlbum].FilterValues.Contains(metadata.PersonalAlbum);

            int indexTitle = IndexOfFilter(Titles);
            bool foundTitle;
            bool hasValueTitle= (indexTitle == -1 ? false : filters[indexTitle].FilterValues.Count > 0);
            if (indexTitle == -1 || filters[indexTitle].FilterValues.Count == 0) foundTitle = IsAndBewteenFieldTags;
            else foundTitle = filters[indexTitle].FilterValues.Contains(metadata.PersonalTitle);

            int indexComment = IndexOfFilter(Comments);
            bool foundComment;
            bool hasValueComment = (indexComment == -1 ? false : filters[indexComment].FilterValues.Count > 0);
            if (indexComment == -1 || filters[indexComment].FilterValues.Count == 0) foundComment = IsAndBewteenFieldTags;
            else foundComment = filters[indexComment].FilterValues.Contains(metadata.PersonalComments);

            int indexDescription = IndexOfFilter(Descriptions);
            bool foundDescriptions;
            bool hasValueDescriptions = (indexDescription == -1 ? false : filters[indexDescription].FilterValues.Count > 0);
            if (indexDescription == -1 || filters[indexDescription].FilterValues.Count == 0) foundDescriptions = IsAndBewteenFieldTags;
            else foundDescriptions = filters[indexDescription].FilterValues.Contains(metadata.PersonalDescription);

            int indexAuthor = IndexOfFilter(Authors);
            bool foundAuthor;
            bool hasValueAuthor = (indexAuthor == -1 ? false : filters[indexAuthor].FilterValues.Count > 0);
            if (indexAuthor == -1 || filters[indexAuthor].FilterValues.Count == 0) foundAuthor = IsAndBewteenFieldTags;
            else foundAuthor = filters[indexAuthor].FilterValues.Contains(metadata.PersonalAuthor);

            int indexRating = IndexOfFilter(Ratings);
            bool foundRating;
            bool hasValueRating = (indexRating == -1 ? false : filters[indexRating].FilterValues.Count > 0);
            if (indexRating == -1 || filters[indexRating].FilterValues.Count == 0) foundRating = IsAndBewteenFieldTags;
            else foundRating = filters[indexRating].FilterValues.Contains(metadata.PersonalRating == null ? null : metadata.PersonalRating.ToString());

            int indexDate = IndexOfFilter(Dates);
            bool foundDate;
            bool hasValueDate = (indexDate == -1 ? false : filters[indexDate].FilterValues.Count > 0);
            if (indexDate == -1 || filters[indexDate].FilterValues.Count == 0) foundDate = IsAndBewteenFieldTags;
            else foundDate = filters[indexDate].FilterValues.Contains(metadata.MediaDateTaken == null ? null : ((DateTime)metadata.MediaDateTaken).ToString("yyyy-MM"));

            int indexLocation = IndexOfFilter(Locations);
            bool foundLocation;
            bool hasValueLocation = (indexLocation == -1 ? false : filters[indexLocation].FilterValues.Count > 0);
            if (indexLocation == -1 || filters[indexLocation].FilterValues.Count == 0) foundLocation = IsAndBewteenFieldTags;
            else foundLocation = filters[indexLocation].FilterValues.Contains(metadata.LocationName);

            int indexCity = IndexOfFilter(Cities);
            bool foundCity;
            bool hasValueCity = (indexCity == -1 ? false : filters[indexCity].FilterValues.Count > 0);
            if (indexCity == -1 || filters[indexCity].FilterValues.Count == 0) foundCity = IsAndBewteenFieldTags;
            else foundCity = filters[indexCity].FilterValues.Contains(metadata.LocationCity);

            int indexState = IndexOfFilter(States);
            bool foundState;
            bool hasValueState = (indexState == -1 ? false : filters[indexState].FilterValues.Count > 0);
            if (indexState == -1 || filters[indexState].FilterValues.Count == 0) foundState = IsAndBewteenFieldTags;
            else foundState = filters[indexState].FilterValues.Contains(metadata.LocationState);

            int indexCountry = IndexOfFilter(Countries);
            bool foundCountry;
            bool hasValueCountry = (indexCountry == -1 ? false : filters[indexCountry].FilterValues.Count > 0);
            if (indexCountry == -1 || filters[indexCountry].FilterValues.Count == 0) foundCountry = IsAndBewteenFieldTags;
            else foundCountry = filters[indexCountry].FilterValues.Contains(metadata.LocationCountry);

            //
            int indexPeople = IndexOfFilter(Peoples);
            bool foundPeople;
            bool hasValuePeople = (indexPeople == -1 ? false : filters[indexPeople].FilterValues.Count > 0);
            if (indexPeople > -1 && filters[indexPeople].FilterValues.Count > 0)
            {
                bool foundPeopleSome = false;
                bool foundPeopleAll = true;
                foreach (string name in filters[indexPeople].FilterValues)
                {
                    if (RegionStructure.DoesThisNameExistInList(metadata.PersonalRegionList, name))
                        foundPeopleSome = true;
                    else 
                        foundPeopleAll = false;
                }

                if (filters[indexPeople].IsAndBetweenValues) foundPeople = foundPeopleAll; //And / Intersection
                else foundPeople = foundPeopleSome; //Or / Union
            }
            else foundPeople = IsAndBewteenFieldTags;

            int indexKeyword = IndexOfFilter(Keywords);
            bool foundKeyword;
            bool hasValueKeyword = (indexKeyword == -1 ? false : filters[indexKeyword].FilterValues.Count > 0);
            if (indexKeyword > -1 && filters[indexKeyword].FilterValues.Count > 0)
            {
                bool foundKeywordSome = false;
                bool foundKeywordAll = true;
                foreach (string name in filters[indexKeyword].FilterValues)
                {
                    if (metadata.PersonalKeywordTags.Contains(new KeywordTag(name))) foundKeywordSome = true;
                    else foundKeywordAll = false;
                }

                if (filters[indexKeyword].IsAndBetweenValues) foundKeyword = foundKeywordAll; //And / Intersection
                else foundKeyword = foundKeywordSome; //Or / Union
            }
            else foundKeyword = IsAndBewteenFieldTags;

            if ( !(hasValueAlbum | hasValueComment | hasValueKeyword | hasValuePeople | hasValueTitle | hasValueDescriptions | hasValueAuthor | hasValueRating | hasValueDate | 
                hasValueLocation | hasValueCity | hasValueState | hasValueCountry)) return true;
            if (IsAndBewteenFieldTags) return 
                    foundAlbum & foundTitle & foundComment & foundPeople & foundKeyword & foundDescriptions & foundAuthor & foundRating & foundDate & foundLocation & foundCity & foundState & foundCountry;
            else return 
                    foundAlbum | foundTitle | foundComment | foundPeople | foundKeyword | foundDescriptions | foundAuthor | foundRating | foundDate | foundLocation | foundCity | foundState | foundCountry;
        }
        #endregion 
    }


    public partial class MainForm : Form
    {        
        private void GetFilters(TreeView treeView)
        {
            if (treeView.Nodes == null) return;
            if (treeView.Nodes[FilterVerifyer.Root] == null) return; 

            FilterVerifyer filterVerifyerFolder = new FilterVerifyer();
            filterVerifyerFolder.ReadValuesFromRootNodesWithChilds(treeView, FilterVerifyer.Root);

            if (GlobalData.SearchFolder)
                FolderSelected(GlobalData.lastReadFolderWasRecursive, false);
            else 
                FolderSearchFilter(GlobalData.SerachFilterResult, false);

        }


        private void treeViewFilter_AfterCheck(object sender, TreeViewEventArgs e)
        {
            TreeNode treeNode = e.Node;
            treeNode.Text = FilterVerifyer.GetTreeNodeText(GlobalData.SearchFolder, treeNode.Name, treeNode.Checked);

            if (isThreeViewPopulating) return;
            GetFilters(treeViewFilter);
        }

        private void ListViewReplaceNullWithText (List<string> list)
        {
            if (list.Contains(null)) list.Remove(null);                
            list.Insert(0, "(Is not defined)");
        }

        private bool isThreeViewPopulating = false;
        private void PopulateDatabaseFilter()
        {
            
            List<string> albums = databaseAndCacheMetadataExiftool.ListAllPersonalAlbums();
            albums.Sort();
            ListViewReplaceNullWithText(albums);
            comboBoxSearchAlbum.Items.Clear();
            comboBoxSearchAlbum.Items.AddRange(albums.ToArray());

            //List<string> authors = databaseAndCacheMetadataExiftool.ListAllPersonalAuthors();
            //authors.Sort();
            //ListViewRemoveNull(authors);
            //comboBoxSearchAuthor.Items.Clear();
            //comboBoxSearchAuthor.Items.AddRange(authors.ToArray());

            List<string> comments = databaseAndCacheMetadataExiftool.ListAllPersonalComments();
            comments.Sort();
            ListViewReplaceNullWithText(comments);
            comboBoxSearchComments.Items.Clear();
            comboBoxSearchComments.Items.AddRange(comments.ToArray());

            List<string> descriptions = databaseAndCacheMetadataExiftool.ListAllPersonalDescriptions();
            descriptions.Sort();
            ListViewReplaceNullWithText(descriptions);
            comboBoxSearchDescription.Items.Clear();
            comboBoxSearchDescription.Items.AddRange(descriptions.ToArray());

            List<string> titles = databaseAndCacheMetadataExiftool.ListAllPersonalTitles();
            titles.Sort();
            ListViewReplaceNullWithText(titles);
            comboBoxSearchTitle.Items.Clear();
            comboBoxSearchTitle.Items.AddRange(titles.ToArray());

            List<string> locations = databaseAndCacheMetadataExiftool.ListAllLocationNames();
            locations.Sort();
            ListViewReplaceNullWithText(locations);
            comboBoxSearchLocationName.Items.Clear();
            comboBoxSearchLocationName.Items.AddRange(locations.ToArray());

            List<string> cities = databaseAndCacheMetadataExiftool.ListAllLocationCities();
            cities.Sort();
            ListViewReplaceNullWithText(cities);
            comboBoxSearchLocationCity.Items.Clear();
            comboBoxSearchLocationCity.Items.AddRange(cities.ToArray());

            List<string> states = databaseAndCacheMetadataExiftool.ListAllLocationStates();
            states.Sort();
            ListViewReplaceNullWithText(states);
            comboBoxSearchLocationState.Items.Clear();
            comboBoxSearchLocationState.Items.AddRange(states.ToArray());
            
            List<string> countries = databaseAndCacheMetadataExiftool.ListAllLocationCountries();
            countries.Sort();
            ListViewReplaceNullWithText(countries);
            comboBoxSearchLocationCountry.Items.Clear();
            comboBoxSearchLocationCountry.Items.AddRange(countries.ToArray());

            List<string> peoples = databaseAndCacheMetadataExiftool.ListAllPersonalRegionsCache();
            peoples.Sort();
            ListViewReplaceNullWithText(peoples);
            checkedListBoxSearchPeople.Items.Clear();
            checkedListBoxSearchPeople.Items.AddRange(peoples.ToArray());

            //dateTimePickerSearchDateFrom.
            //dateTimePickerSearchDateFrom.
            //comboBoxSearch.Items.AddRange(.ToArray());
            /*
            List<string> dates = databaseAndCacheMetadataExiftool.ListAllMediaDateTakenYearAndMonth();
            .Sort();
            comboBoxSearch.Items.AddRange(.ToArray());
            */

        }

        #region PopulateTreeViewFolderFilter
        private void PopulateTreeViewFolderFilter(ImageListView.ImageListViewItemCollection imageListViewSelectedItems)
        {
            FilterVerifyer.PopulateTreeViewBasicNodes(treeViewFilter, FilterVerifyer.Root);
            
            List<string> albums = new List<string>();
            List<string> titles = new List<string>();
            List<string> comments = new List<string>();
            List<string> descriptions = new List<string>();
            List<string> authors = new List<string>();
            List<string> ratings = new List<string>();
            List<string> dates = new List<string>();

            List<string> locations = new List<string>();
            List<string> cities = new List<string>();
            List<string> states = new List<string>();
            List<string> countries = new List<string>();

            List<string> peoples = new List<string>();
            List<string> keywords = new List<string>();

            foreach (ImageListViewItem imageListViewItem in imageListViewSelectedItems)
            {
                Metadata metadata = databaseAndCacheMetadataExiftool.MetadataCacheRead(new FileEntryBroker(imageListViewItem.FileFullPath, imageListViewItem.DateModified, MetadataBrokerTypes.ExifTool));

                if (metadata != null)
                {
                    if (!string.IsNullOrEmpty(metadata.LocationName) && !locations.Contains(metadata.LocationName)) locations.Add(metadata.LocationName);
                    if (!string.IsNullOrEmpty(metadata.LocationCity) && !cities.Contains(metadata.LocationCity)) cities.Add(metadata.LocationCity);
                    if (!string.IsNullOrEmpty(metadata.LocationState) && !states.Contains(metadata.LocationState)) states.Add(metadata.LocationState);
                    if (!string.IsNullOrEmpty(metadata.LocationCountry) && !countries.Contains(metadata.LocationCountry)) countries.Add(metadata.LocationCountry);

                    if (!string.IsNullOrEmpty(metadata.PersonalAlbum) && !albums.Contains(metadata.PersonalAlbum)) albums.Add(metadata.PersonalAlbum);
                    if (!string.IsNullOrEmpty(metadata.PersonalTitle) && !titles.Contains(metadata.PersonalTitle)) titles.Add(metadata.PersonalTitle);
                    if (!string.IsNullOrEmpty(metadata.PersonalComments) && !comments.Contains(metadata.PersonalComments)) comments.Add(metadata.PersonalComments);
                    if (!string.IsNullOrEmpty(metadata.PersonalDescription) && !descriptions.Contains(metadata.PersonalDescription)) descriptions.Add(metadata.PersonalDescription);
                    if (!string.IsNullOrEmpty(metadata.PersonalAuthor) && !authors.Contains(metadata.PersonalAuthor)) authors.Add(metadata.PersonalAuthor);
                    if ((metadata.PersonalRating != null) && !ratings.Contains(metadata.PersonalRating.ToString())) ratings.Add(metadata.PersonalRating.ToString());

                    if (metadata.MediaDateTaken != null)
                    {
                        string yearAndMonth = ((DateTime)metadata.MediaDateTaken).ToString("yyyy-MM");
                        if (!dates.Contains(yearAndMonth)) dates.Add(yearAndMonth);
                    }
                    foreach (RegionStructure regionStructure in metadata.PersonalRegionList)
                    {
                        if (!string.IsNullOrEmpty(regionStructure.Name) && !peoples.Contains(regionStructure.Name)) peoples.Add(regionStructure.Name);
                    }

                    foreach (KeywordTag keywordTag in metadata.PersonalKeywordTags)
                    {
                        if (!string.IsNullOrEmpty(keywordTag.Keyword) && !keywords.Contains(keywordTag.Keyword)) keywords.Add(keywordTag.Keyword);
                    }

                }
            }

            string node = FilterVerifyer.Root;
            albums.Sort();
            FilterVerifyer.PopulateTreeViewWithValues(treeViewFilter, node, FilterVerifyer.Albums, albums);
            titles.Sort();
            FilterVerifyer.PopulateTreeViewWithValues(treeViewFilter, node, FilterVerifyer.Titles, titles);
            comments.Sort();
            FilterVerifyer.PopulateTreeViewWithValues(treeViewFilter, node, FilterVerifyer.Comments, comments);
            descriptions.Sort();
            FilterVerifyer.PopulateTreeViewWithValues(treeViewFilter, node, FilterVerifyer.Descriptions, descriptions);
            authors.Sort();
            FilterVerifyer.PopulateTreeViewWithValues(treeViewFilter, node, FilterVerifyer.Authors, authors);
            ratings.Sort();
            FilterVerifyer.PopulateTreeViewWithValues(treeViewFilter, node, FilterVerifyer.Ratings, ratings);
            dates.Sort();
            FilterVerifyer.PopulateTreeViewWithValues(treeViewFilter, node, FilterVerifyer.Dates, dates);
            locations.Sort();
            FilterVerifyer.PopulateTreeViewWithValues(treeViewFilter, node, FilterVerifyer.Locations, locations);
            cities.Sort();
            FilterVerifyer.PopulateTreeViewWithValues(treeViewFilter, node, FilterVerifyer.Cities, cities);
            states.Sort();
            FilterVerifyer.PopulateTreeViewWithValues(treeViewFilter, node, FilterVerifyer.States, states);
            countries.Sort();
            FilterVerifyer.PopulateTreeViewWithValues(treeViewFilter, node, FilterVerifyer.Countries, countries);
            peoples.Sort();
            FilterVerifyer.PopulateTreeViewWithValues(treeViewFilter, node, FilterVerifyer.Peoples, peoples);
            keywords.Sort();
            FilterVerifyer.PopulateTreeViewWithValues(treeViewFilter, node, FilterVerifyer.Keywords, keywords);

        }
        #endregion 


        private void buttonSearch_Click(object sender, EventArgs e)
        {
            #region DateTaken
            bool useMediaTakenFrom = dateTimePickerSearchDateFrom.Checked;
            DateTime mediaTakenFrom = dateTimePickerSearchDateFrom.Value;
            bool useMediaTakenTo = dateTimePickerSearchDateTo.Checked;
            DateTime mediaTakenTo = dateTimePickerSearchDateTo.Value;
            bool isMediaTakenNull = checkBoxSearchMediaTakenIsNull.Checked;
            #endregion 

            #region Text tags
            bool usePersonalAlbum = !string.IsNullOrWhiteSpace(comboBoxSearchAlbum.Text);
            string personalAlbum = comboBoxSearchAlbum.SelectedIndex == 0 ? null : comboBoxSearchAlbum.Text;
            bool usePersonalTitle = !string.IsNullOrWhiteSpace(comboBoxSearchTitle.Text);
            string personalTitle = comboBoxSearchTitle.SelectedIndex == 0 ? null : comboBoxSearchTitle.Text;
            bool usePersonalComments = !string.IsNullOrWhiteSpace(comboBoxSearchComments.Text);
            string personalComments = comboBoxSearchComments.SelectedIndex == 0 ? null : comboBoxSearchComments.Text;
            bool usePersonalDescription = !string.IsNullOrWhiteSpace(comboBoxSearchDescription.Text);
            string personalDescription = comboBoxSearchDescription.SelectedIndex == 0 ? null : comboBoxSearchDescription.Text;
            bool useLocationName = !string.IsNullOrWhiteSpace(comboBoxSearchLocationName.Text);
            string locationName = comboBoxSearchLocationName.SelectedIndex == 0 ? null : comboBoxSearchLocationName.Text;
            bool useLocationCity = !string.IsNullOrWhiteSpace(comboBoxSearchLocationCity.Text);
            string locationCity = comboBoxSearchLocationCity.SelectedIndex == 0 ? null : comboBoxSearchLocationCity.Text;
            bool useLocationState = !string.IsNullOrWhiteSpace(comboBoxSearchLocationState.Text);
            string locationState = comboBoxSearchLocationState.SelectedIndex == 0 ? null : comboBoxSearchLocationState.Text;
            bool useLocationCountry = !string.IsNullOrWhiteSpace(comboBoxSearchLocationCountry.Text);
            string locationCountry = comboBoxSearchLocationCountry.SelectedIndex == 0 ? null : comboBoxSearchLocationCountry.Text;
            bool useAndBetweenTextTagFields = checkBoxSearchUseAndBetweenTextTagFields.Checked;
            #endregion 

            #region Rating
            bool isRatingNull = checkBoxSearchRatingEmpty.Checked;
            bool hasRating0 = checkBoxSearchRating0.Checked;
            bool hasRating1 = checkBoxSearchRating1.Checked;
            bool hasRating2 = checkBoxSearchRating2.Checked;
            bool hasRating3 = checkBoxSearchRating3.Checked;
            bool hasRating4 = checkBoxSearchRating4.Checked;
            bool hasRating5 = checkBoxSearchRating5.Checked;
            #endregion

            #region Region Names
            bool useRegionNameList = checkedListBoxSearchPeople.CheckedItems.Count > 0;
            bool needAllRegionNames = checkBoxSearchNeedAllNames.Checked;
            bool withoutRegions = checkBoxSearchWithoutRegions.Checked;

            List<string> regionNameList = new List<string>();
            for (int index = 0; index < checkedListBoxSearchPeople.Items.Count; index++)
            {
                if (checkedListBoxSearchPeople.GetItemChecked(index))
                {
                    if (index == 0) regionNameList.Add(null);
                    else
                        regionNameList.Add(checkedListBoxSearchPeople.Items[index].ToString());
                }
            }
            #endregion

            #region Keywords
            bool useKeywordList = !string.IsNullOrWhiteSpace(comboBoxSearchKeyword.Text); 
            bool needAllKeywords = checkBoxSearchNeedAllKeywords.Checked;
            bool withoutKeywords = checkBoxSearchWithoutKeyword.Checked;

            List<string> keywords = new List<string>();
            keywords.AddRange(comboBoxSearchKeyword.Text.Split(';'));
            #endregion

            #region Warning
            bool checkIfHasExifWarning = checkBoxSearchHasWarning.Checked;
            #endregion

            #region Between Groups
            bool useAndBetweenGroups = checkBoxSerachFitsAllValues.Checked;
            #endregion 

            int maxRowsInResult = Properties.Settings.Default.MaxRowsInSearchResult;

            GlobalData.SerachFilterResult = databaseAndCacheMetadataExiftool.ListAllSearch(MetadataBrokerTypes.ExifTool, useAndBetweenGroups, 
                useMediaTakenFrom, mediaTakenFrom, useMediaTakenTo, mediaTakenTo, isMediaTakenNull,
                useAndBetweenTextTagFields,
                usePersonalAlbum, personalAlbum,
                usePersonalTitle, personalTitle,
                usePersonalComments, personalComments,
                usePersonalDescription, personalDescription,
                isRatingNull, hasRating0, hasRating1, hasRating2, hasRating3, hasRating4, hasRating5,
                useLocationName, locationName,
                useLocationCity, locationCity,
                useLocationState, locationState,
                useLocationCountry, locationCountry,
                useRegionNameList, needAllRegionNames, regionNameList, withoutRegions,
                useKeywordList, needAllKeywords, keywords, withoutKeywords,
                checkIfHasExifWarning, maxRowsInResult);
            GlobalData.SearchFolder = false;
            FolderSearchFilter(GlobalData.SerachFilterResult, true);
        }
    }
}

