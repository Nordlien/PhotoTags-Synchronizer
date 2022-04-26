using System;
using System.Windows.Forms;
using System.Collections.Generic;
using MetadataLibrary;
using Krypton.Toolkit;
using System.Diagnostics;

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
        private static HashSet<string> albums = new HashSet<string>();
        public const string Titles = "Titles";
        private static HashSet<string> titles = new HashSet<string>();
        public const string Comments = "Comments";
        private static HashSet<string> comments = new HashSet<string>();
        public const string Descriptions = "Descriptions";
        private static HashSet<string> descriptions = new HashSet<string>();
        public const string Authors = "Authors";
        private static HashSet<string> authors = new HashSet<string>();
        public const string Ratings = "Ratings";
        private static HashSet<string> ratings = new HashSet<string>();
        public const string Dates = "Dates";
        private static HashSet<string> dates = new HashSet<string>();

        public const string Locations = "Locations";
        private static HashSet<string> locations = new HashSet<string>();
        public const string Cities = "Cities";
        private static HashSet<string> cities = new HashSet<string>();
        public const string States = "States";
        private static HashSet<string> states = new HashSet<string>();
        public const string Countries = "Countries";
        private static HashSet<string> countries = new HashSet<string>();
        public const string Peoples = "Peoples";
        private static HashSet<string> peoples = new HashSet<string>();
        public const string Keywords = "Keywords";
        private static HashSet<string> keywords = new HashSet<string>();

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
        public void Add(Filter filter)
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

        #region Read Values - From One Tree Node
        public int ReadValuesFromTreeNodes(KryptonTreeView treeView, string rootNode, string tagNode)
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
        public int ReadValuesFromRootNodesWithChilds(KryptonTreeView treeView, string rootNode)
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
            bool hasValueTitle = (indexTitle == -1 ? false : filters[indexTitle].FilterValues.Count > 0);
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

            if (!(hasValueAlbum | hasValueComment | hasValueKeyword | hasValuePeople | hasValueTitle | hasValueDescriptions | hasValueAuthor | hasValueRating | hasValueDate |
                hasValueLocation | hasValueCity | hasValueState | hasValueCountry)) return true;
            if (IsAndBewteenFieldTags) return
                    foundAlbum & foundTitle & foundComment & foundPeople & foundKeyword & foundDescriptions & foundAuthor & foundRating & foundDate & foundLocation & foundCity & foundState & foundCountry;
            else return
                    foundAlbum | foundTitle | foundComment | foundPeople | foundKeyword | foundDescriptions | foundAuthor | foundRating | foundDate | foundLocation | foundCity | foundState | foundCountry;
        }
        #endregion

        #region ClearTreeViewNodes
        public static void ClearTreeViewNodes(KryptonTreeView treeView)
        {
            GlobalData.DoNotTrigger_TreeViewFilter_BeforeAndAfterCheck = true;

            treeView.Nodes.Clear();
            albums = new HashSet<string>();
            titles = new HashSet<string>();
            comments = new HashSet<string>();
            descriptions = new HashSet<string>();
            authors = new HashSet<string>();
            ratings = new HashSet<string>();
            dates = new HashSet<string>();
            locations = new HashSet<string>();
            cities = new HashSet<string>();
            states = new HashSet<string>();
            countries = new HashSet<string>();
            peoples = new HashSet<string>();
            keywords = new HashSet<string>();

            PopulateTreeViewBasicNodes(treeView, Root);
            GlobalData.DoNotTrigger_TreeViewFilter_BeforeAndAfterCheck = false;
        }
        #endregion

        #region PopulateTreeViewBasicNodes
        public static void PopulateTreeViewBasicNodes(KryptonTreeView treeView, string rootNode)
        {
            GlobalData.DoNotTrigger_TreeViewFilter_BeforeAndAfterCheck = true;
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

            GlobalData.DoNotTrigger_TreeViewFilter_BeforeAndAfterCheck = false;
        }
        #endregion

        #region PopulateTreeViewFolderFilterAdd
        public static void PopulateTreeViewFolderFilterAdd(Metadata metadata)
        {
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
                foreach (RegionStructure regionStructure in metadata.PersonalRegionList) //Not thread safe
                {
                    if (!string.IsNullOrEmpty(regionStructure.Name) && !peoples.Contains(regionStructure.Name)) peoples.Add(regionStructure.Name);
                }

                foreach (KeywordTag keywordTag in metadata.PersonalKeywordTags) //Not thread safe
                {
                    if (!string.IsNullOrEmpty(keywordTag.Keyword) && !keywords.Contains(keywordTag.Keyword)) keywords.Add(keywordTag.Keyword);
                }
            }
        }
        #endregion

        #region StopPopulate
        public static bool StopPopulate = false;
        #endregion

        #region PopulateTreeViewWithValues
        private static void PopulateTreeViewLeafNodeWithValues(KryptonTreeView treeView, string keyRoot, string key, HashSet<string> nodes)
        {
            
            try
            {
                foreach (string node in nodes)
                {
                    if (StopPopulate) break;
                    treeView.Nodes[keyRoot].Nodes[key].Nodes.Add(node, FilterVerifyer.GetTreeNodeText(GlobalData.SearchFolder, node, false));
                }
            }
            catch { 

            }
        }
        #endregion

        #region PopulateTreeViewFolderFilterUpdatedTreeViewFilterInvoke
        public static void PopulateTreeViewFilterWithValues(KryptonTreeView treeViewFilter)
        {
            StopPopulate = false;

            string node = FilterVerifyer.Root;

            try
            {
                FilterVerifyer.PopulateTreeViewLeafNodeWithValues(treeViewFilter, node, FilterVerifyer.Albums, albums);
                FilterVerifyer.PopulateTreeViewLeafNodeWithValues(treeViewFilter, node, FilterVerifyer.Titles, titles);
                FilterVerifyer.PopulateTreeViewLeafNodeWithValues(treeViewFilter, node, FilterVerifyer.Comments, comments);
                FilterVerifyer.PopulateTreeViewLeafNodeWithValues(treeViewFilter, node, FilterVerifyer.Descriptions, descriptions);
                FilterVerifyer.PopulateTreeViewLeafNodeWithValues(treeViewFilter, node, FilterVerifyer.Authors, authors);
                FilterVerifyer.PopulateTreeViewLeafNodeWithValues(treeViewFilter, node, FilterVerifyer.Ratings, ratings);
                FilterVerifyer.PopulateTreeViewLeafNodeWithValues(treeViewFilter, node, FilterVerifyer.Dates, dates);
                FilterVerifyer.PopulateTreeViewLeafNodeWithValues(treeViewFilter, node, FilterVerifyer.Locations, locations);
                FilterVerifyer.PopulateTreeViewLeafNodeWithValues(treeViewFilter, node, FilterVerifyer.Cities, cities);
                FilterVerifyer.PopulateTreeViewLeafNodeWithValues(treeViewFilter, node, FilterVerifyer.States, states);
                FilterVerifyer.PopulateTreeViewLeafNodeWithValues(treeViewFilter, node, FilterVerifyer.Countries, countries);
                FilterVerifyer.PopulateTreeViewLeafNodeWithValues(treeViewFilter, node, FilterVerifyer.Peoples, peoples);
                FilterVerifyer.PopulateTreeViewLeafNodeWithValues(treeViewFilter, node, FilterVerifyer.Keywords, keywords);
            }catch
            {

            }
        }
        #endregion

    }
}

