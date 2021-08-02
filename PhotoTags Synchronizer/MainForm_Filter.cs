using System;
using System.Windows.Forms;
using System.Collections.Generic;
using MetadataLibrary;
using System.Threading;

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

        public const int TagRoot = 0;
        public const int TagRegionOr = 1;
        public const int TagRegionOrAnd = 3;
        public const int TagValue = 4;

        public bool IsAndBewteenFieldTags { get; set; } = true;
        public List<Filter> filters = new List<Filter>();

        public FilterVerifyer()
        {
        }

        #region Add filter
        public void Add (Filter filter)
        {
            filters.Add(filter);
        }
        #endregion 

        #region GetTreeNodeText
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
        #endregion 

        #region ClearTreeViewNodes
        public static void ClearTreeViewNodes(TreeView treeView)
        {
            GlobalData.IsPopulatingFilter = true;
            treeView.Nodes.Clear();
            GlobalData.IsPopulatingFilter = false;
        }
        #endregion

        #region PopulateTreeViewBasicNodes
        public static void PopulateTreeViewBasicNodes(TreeView treeView, string rootNode)
        {
            GlobalData.IsPopulatingFilter = true;
            treeView.Nodes.Clear();
            TreeNode treeNode;
            TreeNode treeNodeRoot;

            treeNodeRoot = treeView.Nodes.Add(FilterVerifyer.Root, FilterVerifyer.GetTreeNodeText(GlobalData.SearchFolder, FilterVerifyer.Root, true));
            treeNodeRoot.Tag = FilterVerifyer.TagRoot;
            treeNodeRoot.Checked = true;

            treeNode = treeView.Nodes[rootNode].Nodes.Add(FilterVerifyer.Albums, FilterVerifyer.GetTreeNodeText(GlobalData.SearchFolder, FilterVerifyer.Albums, false));
            treeNode.Tag = FilterVerifyer.TagRegionOr;

            treeNode = treeView.Nodes[rootNode].Nodes.Add(FilterVerifyer.Titles, FilterVerifyer.GetTreeNodeText(GlobalData.SearchFolder, FilterVerifyer.Titles, false));
            treeNode.Tag = FilterVerifyer.TagRegionOr;

            treeNode = treeView.Nodes[rootNode].Nodes.Add(FilterVerifyer.Comments, FilterVerifyer.GetTreeNodeText(GlobalData.SearchFolder, FilterVerifyer.Comments, false));
            treeNode.Tag = FilterVerifyer.TagRegionOr;

            treeNode = treeView.Nodes[rootNode].Nodes.Add(FilterVerifyer.Descriptions, FilterVerifyer.GetTreeNodeText(GlobalData.SearchFolder, FilterVerifyer.Descriptions, false));
            treeNode.Tag = FilterVerifyer.TagRegionOr;

            treeNode = treeView.Nodes[rootNode].Nodes.Add(FilterVerifyer.Authors, FilterVerifyer.GetTreeNodeText(GlobalData.SearchFolder, FilterVerifyer.Authors, false));
            treeNode.Tag = FilterVerifyer.TagRegionOr;

            treeNode = treeView.Nodes[rootNode].Nodes.Add(FilterVerifyer.Ratings, FilterVerifyer.GetTreeNodeText(GlobalData.SearchFolder, FilterVerifyer.Ratings, false));
            treeNode.Tag = FilterVerifyer.TagRegionOr;

            treeNode = treeView.Nodes[rootNode].Nodes.Add(FilterVerifyer.Dates, FilterVerifyer.GetTreeNodeText(GlobalData.SearchFolder, FilterVerifyer.Dates, false));
            treeNode.Tag = FilterVerifyer.TagRegionOr;

            treeNode = treeView.Nodes[rootNode].Nodes.Add(FilterVerifyer.Locations, FilterVerifyer.GetTreeNodeText(GlobalData.SearchFolder, FilterVerifyer.Locations, false));
            treeNode.Tag = FilterVerifyer.TagRegionOr;

            treeNode = treeView.Nodes[rootNode].Nodes.Add(FilterVerifyer.Cities, FilterVerifyer.GetTreeNodeText(GlobalData.SearchFolder, FilterVerifyer.Cities, false));
            treeNode.Tag = FilterVerifyer.TagRegionOr;

            treeNode = treeView.Nodes[rootNode].Nodes.Add(FilterVerifyer.States, FilterVerifyer.GetTreeNodeText(GlobalData.SearchFolder, FilterVerifyer.States, false));
            treeNode.Tag = FilterVerifyer.TagRegionOr;

            treeNode = treeView.Nodes[rootNode].Nodes.Add(FilterVerifyer.Countries, FilterVerifyer.GetTreeNodeText(GlobalData.SearchFolder, FilterVerifyer.Countries, false));
            treeNode.Tag = FilterVerifyer.TagRegionOr;

            treeNode = treeView.Nodes[rootNode].Nodes.Add(FilterVerifyer.Peoples, FilterVerifyer.GetTreeNodeText(GlobalData.SearchFolder, FilterVerifyer.Peoples, true));
            treeNode.Tag = FilterVerifyer.TagRegionOrAnd;
            treeNode.Checked = true;

            treeNode = treeView.Nodes[rootNode].Nodes.Add(FilterVerifyer.Keywords, FilterVerifyer.GetTreeNodeText(GlobalData.SearchFolder, FilterVerifyer.Keywords, true));
            treeNode.Tag = FilterVerifyer.TagRegionOrAnd;
            treeNode.Checked = true;

            treeNodeRoot.Expand();

            GlobalData.IsPopulatingFilter = false;
        }
        #endregion 

        

        #region PopulateTreeViewWithValues
        public static void PopulateTreeViewWithValues(TreeView treeView, string keyRoot, string key, List<string> nodes)
        {
            foreach (string node in nodes)
            {
                bool found = false;
                for (int indexNode = 0; indexNode < treeView.Nodes[keyRoot].Nodes[key].Nodes.Count; indexNode++)
                {
                    if (treeView.Nodes[keyRoot].Nodes[key].Nodes[indexNode].Text == node)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    TreeNode treeNode = treeView.Nodes[keyRoot].Nodes[key].Nodes.Add(node, FilterVerifyer.GetTreeNodeText(GlobalData.SearchFolder, node, false));
                }
            }
        }
        #endregion 

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
        #region PopulateImageListViewUsingFilters(TreeView treeView)
        private void PopulateImageListViewFromFolderOrUsingFilters(TreeView treeView)
        {
            if (treeView.Nodes == null) return;
            if (treeView.Nodes[FilterVerifyer.Root] == null) return;


            FilterVerifyer filterVerifyerFolder = new FilterVerifyer();
            filterVerifyerFolder.ReadValuesFromRootNodesWithChilds(treeView, FilterVerifyer.Root);

            if (GlobalData.SearchFolder)
                PopulateImageListView_FromFolderSelected(GlobalData.lastReadFolderWasRecursive, false);
            else
                PopulateImageListView_FromSearchTab(GlobalData.SerachFilterResult, false);
        }
        #endregion

        #region TreeViewFilter - BeforeCheck - Not allow all fileres to be checked
        bool isCorrectingDoubleClikcBug = false;
        private void treeViewFilter_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            if (GlobalData.IsPopulatingFilter) return;
            if (isCorrectingDoubleClikcBug) return;

            TreeNode treeNode = e.Node;
            if (treeNode.Tag is int)
            {
                if ((int)treeNode.Tag == FilterVerifyer.TagRegionOr) e.Cancel = true;
            }
        }
        #endregion 

        #region TreeViewFilter - AfterCheck
        private void treeViewFilter_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (GlobalData.IsPopulatingFilter) return;

            //Hack for double - click bug
            TreeNode treeNode = e.Node;
            if (treeNode.Tag is int)
            {

                if ((int)treeNode.Tag == FilterVerifyer.TagRegionOrAnd ||
                    (int)treeNode.Tag == FilterVerifyer.TagRoot) treeNode.Text = FilterVerifyer.GetTreeNodeText(GlobalData.SearchFolder, treeNode.Name, treeNode.Checked);
            }

            PopulateImageListViewFromFolderOrUsingFilters(treeViewFilter);
        }
        #endregion

        #region FilterReplaceNullWithIsNotDefineText
        private void FilterReplaceNullWithIsNotDefineText(List<string> list)
        {
            if (list.Contains(null)) list.Remove(null);
            list.Insert(0, "(Is not defined)");
        }
        #endregion


        private static List<string> treeViewFolderFilterAlbums = new List<string>();
        private static int treeViewFolderFilterAlbumsCount = 0;
        private static List<string> treeViewFolderFilterTitles = new List<string>();
        private static int treeViewFolderFilterTitlesCount = 0;
        private static List<string> treeViewFolderFilterComments = new List<string>();
        private static int treeViewFolderFilterCommentsCount = 0;
        private static List<string> treeViewFolderFilterDescriptions = new List<string>();
        private static int treeViewFolderFilterDescriptionsCount = 0;
        private static List<string> treeViewFolderFilterAuthors = new List<string>();
        private static int treeViewFolderFilterAuthorsCount = 0;
        private static List<string> treeViewFolderFilterRatings = new List<string>();
        private static int treeViewFolderFilterRatingsCount = 0;
        private static List<string> treeViewFolderFilterDates = new List<string>();
        private static int treeViewFolderFilterDatesCount = 0;
        private static List<string> treeViewFolderFilterLocations = new List<string>();
        private static int treeViewFolderFilterLocationsCount = 0;
        private static List<string> treeViewFolderFilterCities = new List<string>();
        private static int treeViewFolderFilterCitiesCount = 0;
        private static List<string> treeViewFolderFilterStates = new List<string>();
        private static int treeViewFolderFilterStatesCount = 0;
        private static List<string> treeViewFolderFilterCountries = new List<string>();
        private static int treeViewFolderFilterCountriesCount = 0;
        private static List<string> treeViewFolderFilterPeoples = new List<string>();
        private static int treeViewFolderFilterPeoplesCount = 0;
        private static List<string> treeViewFolderFilterKeywords = new List<string>();
        private static int treeViewFolderFilterKeywordsCount = 0;


        #region PopulateDatabaseFilter
        public void PopulateDatabaseFilter()
        {

            List<string> albums = databaseAndCacheMetadataExiftool.ListAllPersonalAlbums(MetadataBrokerType.ExifTool);
            albums.Sort();
            FilterReplaceNullWithIsNotDefineText(albums);
            comboBoxSearchAlbum.Items.Clear();
            comboBoxSearchAlbum.Items.AddRange(albums.ToArray());

            //List<string> authors = databaseAndCacheMetadataExiftool.ListAllPersonalAuthors(MetadataBrokerType.ExifTool);
            //authors.Sort();
            //ListViewRemoveNull(authors);
            //comboBoxSearchAuthor.Items.Clear();
            //comboBoxSearchAuthor.Items.AddRange(authors.ToArray());

            List<string> comments = databaseAndCacheMetadataExiftool.ListAllPersonalComments(MetadataBrokerType.ExifTool);
            comments.Sort();
            FilterReplaceNullWithIsNotDefineText(comments);
            comboBoxSearchComments.Items.Clear();
            comboBoxSearchComments.Items.AddRange(comments.ToArray());

            List<string> descriptions = databaseAndCacheMetadataExiftool.ListAllPersonalDescriptions(MetadataBrokerType.ExifTool);
            descriptions.Sort();
            FilterReplaceNullWithIsNotDefineText(descriptions);
            comboBoxSearchDescription.Items.Clear();
            comboBoxSearchDescription.Items.AddRange(descriptions.ToArray());

            List<string> titles = databaseAndCacheMetadataExiftool.ListAllPersonalTitles(MetadataBrokerType.ExifTool);
            titles.Sort();
            FilterReplaceNullWithIsNotDefineText(titles);
            comboBoxSearchTitle.Items.Clear();
            comboBoxSearchTitle.Items.AddRange(titles.ToArray());

            List<string> locations = databaseAndCacheMetadataExiftool.ListAllLocationNames(MetadataBrokerType.ExifTool);
            locations.Sort();
            FilterReplaceNullWithIsNotDefineText(locations);
            comboBoxSearchLocationName.Items.Clear();
            comboBoxSearchLocationName.Items.AddRange(locations.ToArray());

            List<string> cities = databaseAndCacheMetadataExiftool.ListAllLocationCities(MetadataBrokerType.ExifTool);
            cities.Sort();
            FilterReplaceNullWithIsNotDefineText(cities);
            comboBoxSearchLocationCity.Items.Clear();
            comboBoxSearchLocationCity.Items.AddRange(cities.ToArray());

            List<string> states = databaseAndCacheMetadataExiftool.ListAllLocationStates(MetadataBrokerType.ExifTool);
            states.Sort();
            FilterReplaceNullWithIsNotDefineText(states);
            comboBoxSearchLocationState.Items.Clear();
            comboBoxSearchLocationState.Items.AddRange(states.ToArray());

            List<string> countries = databaseAndCacheMetadataExiftool.ListAllLocationCountries(MetadataBrokerType.ExifTool);
            countries.Sort();
            FilterReplaceNullWithIsNotDefineText(countries);
            comboBoxSearchLocationCountry.Items.Clear();
            comboBoxSearchLocationCountry.Items.AddRange(countries.ToArray());

            List<string> peoples = databaseAndCacheMetadataExiftool.ListAllPersonalRegionsCache(MetadataBrokerType.ExifTool);
            peoples.Sort();
            FilterReplaceNullWithIsNotDefineText(peoples);
            checkedListBoxSearchPeople.Items.Clear();
            checkedListBoxSearchPeople.Items.AddRange(peoples.ToArray());

            //dateTimePickerSearchDateFrom.
            //dateTimePickerSearchDateFrom.
            //comboBoxSearch.Items.AddRange(.ToArray());
            /*
            List<string> dates = databaseAndCacheMetadataExiftool.ListAllMediaDateTakenYearAndMonth(MetadataBrokerType.ExifTool);
            .Sort();
            comboBoxSearch.Items.AddRange(.ToArray());
            */

        }
        #endregion 

        #region PopulateTreeViewFolderFilter - Thread
        private bool IsPopulateTreeViewFolderFilterThreadRunning { get; set; }
        Thread threadPopulateFilter = null;
        private void PopulateTreeViewFolderFilterThread(HashSet<FileEntry> imageListViewFileEntryItems)
        {
            if (imageListViewFileEntryItems == null) return;
            GlobalData.IsImageListViewForEachInProgressRequestStop = true;

            if (threadPopulateFilter != null) // || threadPopulateFilter.ThreadState == System.Threading.ThreadState.Running || threadPopulateFilter.ThreadState == System.Threading.ThreadState.WaitSleepJoin)                   
            {
                WaitThread_PopulateTreeViewFolderFilter_Stopped.WaitOne(10000);
            }

            threadPopulateFilter = new Thread(() =>
            {
                IsPopulateTreeViewFolderFilterThreadRunning = true;
                PopulateTreeViewFolderFilterInvoke(imageListViewFileEntryItems);
                threadPopulateFilter = null;
                IsPopulateTreeViewFolderFilterThreadRunning = false;
            });
            threadPopulateFilter.Priority = ThreadPriority.Lowest;
            threadPopulateFilter.Start();
        }
        #endregion

        
        
        #region PopulateTreeViewFolderFilter - Add - Invoke
        private void PopulateTreeViewFolderFilterAdd(FileEntryBroker fileEntryBroker) //new FileEntryBroker(fileEntry, MetadataBrokerType.ExifTool)
        {
            Metadata metadata = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOnly(fileEntryBroker);
            //Application.DoEvents();

            if (metadata != null)
            {
                if (!string.IsNullOrEmpty(metadata.LocationName) && !treeViewFolderFilterLocations.Contains(metadata.LocationName)) treeViewFolderFilterLocations.Add(metadata.LocationName);
                if (!string.IsNullOrEmpty(metadata.LocationCity) && !treeViewFolderFilterCities.Contains(metadata.LocationCity)) treeViewFolderFilterCities.Add(metadata.LocationCity);
                if (!string.IsNullOrEmpty(metadata.LocationState) && !treeViewFolderFilterStates.Contains(metadata.LocationState)) treeViewFolderFilterStates.Add(metadata.LocationState);
                if (!string.IsNullOrEmpty(metadata.LocationCountry) && !treeViewFolderFilterCountries.Contains(metadata.LocationCountry)) treeViewFolderFilterCountries.Add(metadata.LocationCountry);

                if (!string.IsNullOrEmpty(metadata.PersonalAlbum) && !treeViewFolderFilterAlbums.Contains(metadata.PersonalAlbum)) treeViewFolderFilterAlbums.Add(metadata.PersonalAlbum);
                if (!string.IsNullOrEmpty(metadata.PersonalTitle) && !treeViewFolderFilterTitles.Contains(metadata.PersonalTitle)) treeViewFolderFilterTitles.Add(metadata.PersonalTitle);
                if (!string.IsNullOrEmpty(metadata.PersonalComments) && !treeViewFolderFilterComments.Contains(metadata.PersonalComments)) treeViewFolderFilterComments.Add(metadata.PersonalComments);
                if (!string.IsNullOrEmpty(metadata.PersonalDescription) && !treeViewFolderFilterDescriptions.Contains(metadata.PersonalDescription)) treeViewFolderFilterDescriptions.Add(metadata.PersonalDescription);
                if (!string.IsNullOrEmpty(metadata.PersonalAuthor) && !treeViewFolderFilterAuthors.Contains(metadata.PersonalAuthor)) treeViewFolderFilterAuthors.Add(metadata.PersonalAuthor);
                if ((metadata.PersonalRating != null) && !treeViewFolderFilterRatings.Contains(metadata.PersonalRating.ToString())) treeViewFolderFilterRatings.Add(metadata.PersonalRating.ToString());

                if (metadata.MediaDateTaken != null)
                {
                    string yearAndMonth = ((DateTime)metadata.MediaDateTaken).ToString("yyyy-MM");
                    if (!treeViewFolderFilterDates.Contains(yearAndMonth)) treeViewFolderFilterDates.Add(yearAndMonth);
                }
                foreach (RegionStructure regionStructure in metadata.PersonalRegionList)
                {
                    if (!string.IsNullOrEmpty(regionStructure.Name) && !treeViewFolderFilterPeoples.Contains(regionStructure.Name)) treeViewFolderFilterPeoples.Add(regionStructure.Name);
                }

                foreach (KeywordTag keywordTag in metadata.PersonalKeywordTags)
                {
                    if (!string.IsNullOrEmpty(keywordTag.Keyword) && !treeViewFolderFilterKeywords.Contains(keywordTag.Keyword)) treeViewFolderFilterKeywords.Add(keywordTag.Keyword);
                }
            }
        }
        #endregion 

        #region PopulateTreeViewFolderFilterUpdatedTreeViewFilterInvoke
        private void PopulateTreeViewFolderFilterUpdatedTreeViewFilterInvoke()
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action(PopulateTreeViewFolderFilterUpdatedTreeViewFilterInvoke));
                return;
            }

            if (!GlobalData.IsImageListViewForEachInProgressRequestStop)
            {
                string node = FilterVerifyer.Root;

                if (treeViewFolderFilterAlbums.Count != treeViewFolderFilterAlbumsCount)
                {
                    treeViewFolderFilterAlbums.Sort();
                    FilterVerifyer.PopulateTreeViewWithValues(treeViewFilter, node, FilterVerifyer.Albums, treeViewFolderFilterAlbums);
                    treeViewFolderFilterAlbumsCount = treeViewFolderFilterAlbums.Count;
                }

                if (treeViewFolderFilterTitles.Count != treeViewFolderFilterTitlesCount)
                {
                    treeViewFolderFilterTitles.Sort();
                    FilterVerifyer.PopulateTreeViewWithValues(treeViewFilter, node, FilterVerifyer.Titles, treeViewFolderFilterTitles);
                    treeViewFolderFilterTitlesCount = treeViewFolderFilterTitles.Count;
                }

                if (treeViewFolderFilterComments.Count != treeViewFolderFilterCommentsCount)
                {
                    treeViewFolderFilterComments.Sort();
                    FilterVerifyer.PopulateTreeViewWithValues(treeViewFilter, node, FilterVerifyer.Comments, treeViewFolderFilterComments);
                    treeViewFolderFilterCommentsCount = treeViewFolderFilterComments.Count;
                }

                if (treeViewFolderFilterDescriptions.Count != treeViewFolderFilterDescriptionsCount)
                {
                    treeViewFolderFilterDescriptions.Sort();
                    FilterVerifyer.PopulateTreeViewWithValues(treeViewFilter, node, FilterVerifyer.Descriptions, treeViewFolderFilterDescriptions);
                    treeViewFolderFilterDescriptionsCount = treeViewFolderFilterDescriptions.Count;
                }

                if (treeViewFolderFilterAuthors.Count != treeViewFolderFilterAuthorsCount)
                {
                    treeViewFolderFilterAuthors.Sort();
                    FilterVerifyer.PopulateTreeViewWithValues(treeViewFilter, node, FilterVerifyer.Authors, treeViewFolderFilterAuthors);
                    treeViewFolderFilterAuthorsCount = treeViewFolderFilterAuthors.Count;
                }
                
                if (treeViewFolderFilterRatings.Count != treeViewFolderFilterRatingsCount)
                {
                    treeViewFolderFilterRatings.Sort();
                    FilterVerifyer.PopulateTreeViewWithValues(treeViewFilter, node, FilterVerifyer.Ratings, treeViewFolderFilterRatings);
                    treeViewFolderFilterRatingsCount = treeViewFolderFilterRatings.Count;
                }

                if (treeViewFolderFilterDates.Count != treeViewFolderFilterDatesCount)
                {
                    treeViewFolderFilterDates.Sort();
                    FilterVerifyer.PopulateTreeViewWithValues(treeViewFilter, node, FilterVerifyer.Dates, treeViewFolderFilterDates);
                    treeViewFolderFilterDatesCount = treeViewFolderFilterDates.Count;
                }

                if (treeViewFolderFilterLocations.Count != treeViewFolderFilterLocationsCount)
                {
                    treeViewFolderFilterLocations.Sort();
                    FilterVerifyer.PopulateTreeViewWithValues(treeViewFilter, node, FilterVerifyer.Locations, treeViewFolderFilterLocations);
                    treeViewFolderFilterLocationsCount = treeViewFolderFilterLocations.Count;
                }

                if (treeViewFolderFilterCities.Count != treeViewFolderFilterCitiesCount)
                {
                    treeViewFolderFilterCities.Sort();
                    FilterVerifyer.PopulateTreeViewWithValues(treeViewFilter, node, FilterVerifyer.Cities, treeViewFolderFilterCities);
                    treeViewFolderFilterCitiesCount = treeViewFolderFilterCities.Count;
                }

                if (treeViewFolderFilterStates.Count != treeViewFolderFilterStatesCount)
                {
                    treeViewFolderFilterStates.Sort();
                    FilterVerifyer.PopulateTreeViewWithValues(treeViewFilter, node, FilterVerifyer.States, treeViewFolderFilterStates);
                    treeViewFolderFilterStatesCount = treeViewFolderFilterStates.Count;
                }

                if (treeViewFolderFilterCountries.Count != treeViewFolderFilterCountriesCount)
                {
                    treeViewFolderFilterCountries.Sort();
                    FilterVerifyer.PopulateTreeViewWithValues(treeViewFilter, node, FilterVerifyer.Countries, treeViewFolderFilterCountries);
                    treeViewFolderFilterCountriesCount = treeViewFolderFilterCountries.Count;
                }

                if (treeViewFolderFilterPeoples.Count != treeViewFolderFilterPeoplesCount)
                {
                    treeViewFolderFilterPeoples.Sort();
                    FilterVerifyer.PopulateTreeViewWithValues(treeViewFilter, node, FilterVerifyer.Peoples, treeViewFolderFilterPeoples);
                    treeViewFolderFilterPeoplesCount = treeViewFolderFilterPeoples.Count;
                }

                if (treeViewFolderFilterKeywords.Count != treeViewFolderFilterKeywordsCount)
                {
                    treeViewFolderFilterKeywords.Sort();
                    FilterVerifyer.PopulateTreeViewWithValues(treeViewFilter, node, FilterVerifyer.Keywords, treeViewFolderFilterKeywords);
                    treeViewFolderFilterKeywordsCount = treeViewFolderFilterKeywords.Count;
                }
            }
        }
        #endregion

        #region PopulateTreeViewFolderFilterInvoke     
        private void PopulateTreeViewFolderFilterInvoke(HashSet<FileEntry> imageListViewFileEntryItems)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<HashSet<FileEntry>>(PopulateTreeViewFolderFilterInvoke), imageListViewFileEntryItems);
                return;
            }

            WaitThread_PopulateTreeViewFolderFilter_Stopped = new AutoResetEvent(false);

            GlobalData.IsImageListViewForEachInProgressRequestStop = false;
            treeViewFilter.Enabled = false;
            treeViewFilter.SuspendLayout();

            FilterVerifyer.PopulateTreeViewBasicNodes(treeViewFilter, FilterVerifyer.Root);
            treeViewFolderFilterAlbums.Clear();
            treeViewFolderFilterTitles.Clear();
            treeViewFolderFilterComments.Clear();
            treeViewFolderFilterDescriptions.Clear(); 
            treeViewFolderFilterAuthors.Clear();
            treeViewFolderFilterRatings.Clear();
            treeViewFolderFilterDates.Clear();
            treeViewFolderFilterLocations.Clear();
            treeViewFolderFilterCities.Clear();
            treeViewFolderFilterStates.Clear();
            treeViewFolderFilterCountries.Clear();
            treeViewFolderFilterPeoples.Clear();
            treeViewFolderFilterKeywords.Clear();
            
            treeViewFolderFilterAlbumsCount = 0;
            treeViewFolderFilterTitlesCount = 0;
            treeViewFolderFilterCommentsCount = 0;
            treeViewFolderFilterDescriptionsCount = 0;
            treeViewFolderFilterAuthorsCount = 0;
            treeViewFolderFilterRatingsCount = 0;
            treeViewFolderFilterDatesCount = 0;
            treeViewFolderFilterLocationsCount = 0;
            treeViewFolderFilterCitiesCount = 0;
            treeViewFolderFilterStatesCount = 0;
            treeViewFolderFilterCountriesCount = 0;
            treeViewFolderFilterPeoplesCount = 0;
            treeViewFolderFilterKeywordsCount = 0;
            
            try
            {
                foreach (FileEntry fileEntry in imageListViewFileEntryItems)
                {
                    if (GlobalData.IsStopAndEmptyExiftoolReadQueueRequest) break;
                    if (GlobalData.IsApplicationClosing) break;
                    if (GlobalData.IsImageListViewForEachInProgressRequestStop) break;

                    PopulateTreeViewFolderFilterAdd(new FileEntryBroker(fileEntry, MetadataBrokerType.ExifTool));
                }

                PopulateTreeViewFolderFilterUpdatedTreeViewFilterInvoke();
            }
            catch
            {
                //Do nothing, collreaction modified, don't care.
            }

            if (WaitThread_PopulateTreeViewFolderFilter_Stopped != null)
            {
                WaitThread_PopulateTreeViewFolderFilter_Stopped.Set();
            }

            imageListViewFileEntryItems = null;
            treeViewFilter.Enabled = true;
            treeViewFilter.ResumeLayout();
        }
        #endregion

        #region Filter - Search - click
        private void buttonFilterSearch_Click(object sender, EventArgs e)
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

            GlobalData.SerachFilterResult = databaseAndCacheMetadataExiftool.ListAllSearch(MetadataBrokerType.ExifTool, useAndBetweenGroups, 
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
            PopulateImageListView_FromSearchTab(GlobalData.SerachFilterResult, true);
        }
        #endregion 
    }
}

