using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using MetadataLibrary;
using ImageAndMovieFileExtentions;
using Manina.Windows.Forms;
using System.Drawing;

namespace PhotoTagsSynchronizer
{
    
    public class Filter
    {
        public bool IsAndBetweenValues { get; set; }
        public string FieldName { get; set; }
        public List<string> FilterValues { get; set; } = new List<string>();

        public Filter(bool isAnd, string fieldName)
        {
            IsAndBetweenValues = isAnd;
            FieldName = fieldName;
        }

        public void AddValue(string value)
        {
            if (!FilterValues.Contains(value)) FilterValues.Add(value);
        }
    }

    public class FilterVerifyer
    {
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
        
        public bool IsAndBewteenFieldTags { get; set; }
        public List<Filter> filters = new List<Filter>();

        public FilterVerifyer(bool isAnd)
        {
            IsAndBewteenFieldTags = isAnd;
        }

        public void Add (Filter filter)
        {
            filters.Add(filter);
        }

        
        public static void PopulateTreeViewBasicNodes(TreeView treeView, string rootNode)
        {
            treeView.Nodes[rootNode].Nodes.Clear();
            TreeNode treeNode;
            treeNode = treeView.Nodes[rootNode].Nodes.Add(FilterVerifyer.Albums, FilterVerifyer.Albums);
            treeNode = treeView.Nodes[rootNode].Nodes.Add(FilterVerifyer.Titles, FilterVerifyer.Titles);
            treeNode = treeView.Nodes[rootNode].Nodes.Add(FilterVerifyer.Comments, FilterVerifyer.Comments);
            treeNode = treeView.Nodes[rootNode].Nodes.Add(FilterVerifyer.Descriptions, FilterVerifyer.Descriptions);
            treeNode = treeView.Nodes[rootNode].Nodes.Add(FilterVerifyer.Authors, FilterVerifyer.Authors);
            treeNode = treeView.Nodes[rootNode].Nodes.Add(FilterVerifyer.Ratings, FilterVerifyer.Ratings);
            treeNode = treeView.Nodes[rootNode].Nodes.Add(FilterVerifyer.Dates, FilterVerifyer.Dates);
            treeNode = treeView.Nodes[rootNode].Nodes.Add(FilterVerifyer.Locations, FilterVerifyer.Locations);
            treeNode = treeView.Nodes[rootNode].Nodes.Add(FilterVerifyer.Cities, FilterVerifyer.Cities);
            treeNode = treeView.Nodes[rootNode].Nodes.Add(FilterVerifyer.States, FilterVerifyer.States);
            treeNode = treeView.Nodes[rootNode].Nodes.Add(FilterVerifyer.Countries, FilterVerifyer.Countries);
            treeNode = treeView.Nodes[rootNode].Nodes.Add(FilterVerifyer.Peoples, FilterVerifyer.Peoples);
            treeNode = treeView.Nodes[rootNode].Nodes.Add(FilterVerifyer.Keywords, FilterVerifyer.Keywords);
        }

        public static void PopulateTreeViewWithValues(TreeView treeView, string keyRoot, string key, List<string> nodes)
        {
            foreach (string node in nodes)
            {
                TreeNode treeNode = treeView.Nodes[keyRoot].Nodes[key].Nodes.Add(node);
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
                    filter.AddValue(treeNode.Text);
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
        const string rootNodeFolder = "NodeFolder";
        private void GetFilters(TreeView treeView)
        {                       
            bool isAndBetweenFieldTagsFolder = treeView.Nodes[rootNodeFolder].Checked;
            FilterVerifyer filterVerifyerFolder = new FilterVerifyer(isAndBetweenFieldTagsFolder);

            filterVerifyerFolder.ReadValuesFromRootNodesWithChilds(treeView, rootNodeFolder);
            
            FolderSelected(GlobalData.lastReadFolderWasRecursive, false);
        }


        private void treeViewFilter_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (isThreeViewPopulating) return;
            GetFilters(treeViewFilter);
        }

        private void ListViewRemoveNull (List<string> list)
        {
            if (list.Contains(null))
            {
                list.Remove(null);
                list.Insert(0, "(Is blank)");
            }
        }

        private bool isThreeViewPopulating = false;
        private void PopulateDatabaseFilter()
        {
            
            List<string> albums = databaseAndCacheMetadataExiftool.ListAllPersonalAlbums();
            albums.Sort();
            ListViewRemoveNull(albums);
            comboBoxSearchAlbum.Items.AddRange(albums.ToArray());

            List<string> authors = databaseAndCacheMetadataExiftool.ListAllPersonalAuthors();
            authors.Sort();
            ListViewRemoveNull(authors);
            comboBoxSearchAuthor.Items.AddRange(authors.ToArray());

            List<string> comments = databaseAndCacheMetadataExiftool.ListAllPersonalComments();
            comments.Sort();
            ListViewRemoveNull(comments);
            comboBoxSearchComments.Items.AddRange(comments.ToArray());

            List<string> descriptions = databaseAndCacheMetadataExiftool.ListAllPersonalDescriptions();
            descriptions.Sort();
            ListViewRemoveNull(descriptions);
            comboBoxSearchDescription.Items.AddRange(descriptions.ToArray());

            List<string> titles = databaseAndCacheMetadataExiftool.ListAllPersonalTitles();
            titles.Sort();
            ListViewRemoveNull(titles);
            comboBoxSearchTitle.Items.AddRange(titles.ToArray());

            List<string> locations = databaseAndCacheMetadataExiftool.ListAllLocationNames();
            locations.Sort();
            ListViewRemoveNull(locations);
            comboBoxSearchLocationName.Items.AddRange(locations.ToArray());

            List<string> cities = databaseAndCacheMetadataExiftool.ListAllLocationCities();
            cities.Sort();
            ListViewRemoveNull(cities);
            comboBoxSearchLocationCity.Items.AddRange(cities.ToArray());

            List<string> states = databaseAndCacheMetadataExiftool.ListAllLocationStates();
            states.Sort();
            ListViewRemoveNull(states);
            comboBoxSearchLocationState.Items.AddRange(states.ToArray());
            
            List<string> countries = databaseAndCacheMetadataExiftool.ListAllLocationCountries();
            countries.Sort();
            ListViewRemoveNull(countries);
            comboBoxSearchLocationCountry.Items.AddRange(countries.ToArray());

            List<string> peoples = databaseAndCacheMetadataExiftool.ListAllPersonalRegions();
            peoples.Sort();
            ListViewRemoveNull(peoples);
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
            FilterVerifyer.PopulateTreeViewBasicNodes(treeViewFilter, rootNodeFolder);
            
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
                Metadata metadata = databaseAndCacheMetadataExiftool.ReadCache(new FileEntryBroker(imageListViewItem.FileFullPath, imageListViewItem.DateModified, MetadataBrokerTypes.ExifTool));

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

            string node = rootNodeFolder;
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

    }
}

