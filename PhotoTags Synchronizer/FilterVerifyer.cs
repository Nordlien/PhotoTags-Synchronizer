using System;
using System.Windows.Forms;
using System.Collections.Generic;
using MetadataLibrary;
using Krypton.Toolkit;

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

        #region ClearTreeViewNodes
        public static void ClearTreeViewNodes(KryptonTreeView treeView)
        {
            GlobalData.IsPopulatingFilter = true;
            treeView.Nodes.Clear();
            GlobalData.IsPopulatingFilter = false;
        }
        #endregion

        #region PopulateTreeViewBasicNodes
        public static void PopulateTreeViewBasicNodes(KryptonTreeView treeView, string rootNode)
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
        public static void PopulateTreeViewWithValues(KryptonTreeView treeView, string keyRoot, string key, List<string> nodes)
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
    }
}

