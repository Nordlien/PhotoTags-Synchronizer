using FastColoredTextBoxNS;
using MetadataLibrary;
using MetadataPriorityLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Krypton.Toolkit;

namespace PhotoTagsSynchronizer
{
    public class FastColoredTextBoxHandler
    {
        //styles
        static TextStyle StyleLogWarning = new TextStyle(Brushes.Blue, null, FontStyle.Bold);
        TextStyle StyleVaiables = new TextStyle(Brushes.Blue, null, FontStyle.Bold);
        TextStyle StyleMetadataTags = new TextStyle(Brushes.Magenta, null, FontStyle.Bold);
        TextStyle StyleMetadataRegions = new TextStyle(Brushes.Red, null, FontStyle.Bold);

        //Log Word wrap
        static private Regex regexLogWordWrap = new Regex(@"&&|&|\|\||\|");
        private Regex regexProperties = null; 
        private Regex regexMetadataTags = null;
        private Regex regexMetadataRegions = null;

        private AutocompleteMenu popupMenuMetadataProperties;

        public FastColoredTextBoxHandler(FastColoredTextBox fastColoredTextBox, KryptonComboBox comboBox, string[] arg2, string[] arg3)
        {
            //create autocomplete popup menu
            popupMenuMetadataProperties = new AutocompleteMenu(fastColoredTextBox);
            popupMenuMetadataProperties.MinFragmentLength = 2;

            List<AutocompleteItem> autoCompleteItems = new List<AutocompleteItem>();
            
            string regexPatternProperties = "";
            foreach (string item in comboBox.Items)
            {
                regexPatternProperties += (regexPatternProperties == "" ? "" : "|") + item;
                autoCompleteItems.Add(new AutocompleteItem(item.TrimStart('{').TrimEnd('}')));
            }

            //Tags
            string regexPatternMetadataTags = "";
            var arg2sorted = new List<string>(arg2).OrderByDescending(x => x.Length);

            foreach (string value in arg2sorted)
            {
                if (value.Length > 2) regexPatternMetadataTags += (regexPatternMetadataTags == "" ? "" : "|") + value;
            }

            //Regions
            string regexPatternMetadataRegions = "";
            var arg3sorted = new List<string>(arg3).OrderByDescending(x => x.Length);
            foreach (string value in arg3sorted)
            {
                if (value.Length > 2) regexPatternMetadataRegions += (regexPatternMetadataRegions == "" ? "" : "|") + value;
            }
            
            regexProperties = new Regex("(" + regexPatternProperties + ")", RegexOptions.IgnoreCase);
            regexMetadataTags = new Regex("(" + regexPatternMetadataTags + ")", RegexOptions.IgnoreCase);
            regexMetadataRegions = new Regex("(" + regexPatternMetadataRegions + ")", RegexOptions.IgnoreCase);

            popupMenuMetadataProperties.Items.SetAutocompleteItems(autoCompleteItems);
            popupMenuMetadataProperties.Items.MaximumSize = new System.Drawing.Size(200, 300);
            popupMenuMetadataProperties.Items.Width = 200;
        }

        public FastColoredTextBoxHandler(FastColoredTextBox fastColoredTextBox, bool addKeywordItem, Dictionary<MetadataPriorityKey, MetadataPriorityValues> metadataPrioityDictionary)
        {            
            //create autocomplete popup menu
            popupMenuMetadataProperties = new AutocompleteMenu(fastColoredTextBox);
            popupMenuMetadataProperties.MinFragmentLength = 2;

            List<AutocompleteItem> autoCompleteItems = new List<AutocompleteItem>();
            string[] listOfProperties = Metadata.ListOfProperties(addKeywordItem);

            string regexPatternProperties = "";
            foreach (string item in listOfProperties)
            {
                regexPatternProperties += (regexPatternProperties == "" ? "" : "|") + item;
                autoCompleteItems.Add(new AutocompleteItem(item.TrimStart('{').TrimEnd('}')));
            }
            
            List<string> addeMetadataTags = new List<string>();
            List<string> addeMetadataRegions = new List<string>();
            List<string> addedAutoComplete = new List<string>();
            foreach (KeyValuePair<MetadataPriorityKey, MetadataPriorityValues> keyValuePair in metadataPrioityDictionary)
            {
                if (!addedAutoComplete.Contains(keyValuePair.Key.Region)) addedAutoComplete.Add(keyValuePair.Key.Region);
                if (!addedAutoComplete.Contains(keyValuePair.Key.Tag)) addedAutoComplete.Add(keyValuePair.Key.Tag);
                
                string[] regions = keyValuePair.Key.Region.Split(':');
                string tag = "[-:]" + keyValuePair.Key.Tag + "[+-]?=";

                foreach (string regionSplit in regions)
                {
                    string region = regionSplit + ":";
                    if (!addeMetadataRegions.Contains(region)) addeMetadataRegions.Add(region);
                }
                if (!addeMetadataTags.Contains(tag)) addeMetadataTags.Add(tag);
            }

            //Auto Complete
            addedAutoComplete.Sort();
            foreach (string autoCompleteItem in addedAutoComplete) autoCompleteItems.Add(new AutocompleteItem(autoCompleteItem));

            //Tags
            string regexPatternMetadataTags = "";
            IEnumerable<string> query = addeMetadataTags.OrderBy(pet => pet.Length);
            foreach (string value in query)
            {
                regexPatternMetadataTags += (regexPatternMetadataTags == "" ? "" : "|") + value;
            }

            //Regions
            string regexPatternMetadataRegions = "";
            IEnumerable<string> queryRegions = addeMetadataRegions.OrderBy(pet => pet.Length);
            foreach (string value in queryRegions)
            {
                regexPatternMetadataRegions += (regexPatternMetadataRegions == "" ? "" : "|") + value;
            }

            regexProperties = new Regex("(" + regexPatternProperties + ")", RegexOptions.IgnoreCase);
            regexMetadataTags = new Regex("(" + regexPatternMetadataTags + ")", RegexOptions.IgnoreCase);
            regexMetadataRegions = new Regex("(" + regexPatternMetadataRegions + ")", RegexOptions.IgnoreCase);

            popupMenuMetadataProperties.Items.SetAutocompleteItems(autoCompleteItems);
            popupMenuMetadataProperties.Items.MaximumSize = new System.Drawing.Size(200, 300);
            popupMenuMetadataProperties.Items.Width = 200;
        }

        public void KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.K | Keys.Control))
            {
                popupMenuMetadataProperties.Show(true);
                e.Handled = true;
            }
        }
        
        public static void WordWrapNeededLog(object sender, FastColoredTextBoxNS.WordWrapNeededEventArgs e)
        {
            e.CutOffPositions.Clear();
            foreach (Match m in regexLogWordWrap.Matches(e.Line.Text)) e.CutOffPositions.Add(m.Index);
        }

        public void SyntaxHighlightProperties(object sender, TextChangedEventArgs e)
        {
            FastColoredTextBox fastColoredTextBox = (FastColoredTextBox)sender;
            fastColoredTextBox.Range.ClearStyle(StyleVaiables, StyleMetadataRegions, StyleMetadataTags);
            fastColoredTextBox.Range.SetStyle(StyleVaiables, regexProperties);
            fastColoredTextBox.Range.SetStyle(StyleMetadataTags, regexMetadataTags);
            fastColoredTextBox.Range.SetStyle(StyleMetadataRegions, regexMetadataRegions);
        }

        public static void SyntaxHighlightLog(FastColoredTextBox fastColoredTextBox)
        {
            const int margin = 2000;
            var startLine = Math.Max(0, fastColoredTextBox.VisibleRange.Start.iLine - margin);
            var endLine = Math.Min(fastColoredTextBox.LinesCount - 1, fastColoredTextBox.VisibleRange.End.iLine + margin);
            var range = new Range(fastColoredTextBox, 0, startLine, 0, endLine);
            //clear folding markers
            range.ClearFoldingMarkers();
            range.ClearStyle(StyleIndex.All);
            range.SetStyle(StyleLogWarning, @"(ERROR|WARN|INFO|DEBUG|FATAL|TRACE)", RegexOptions.IgnoreCase);
        }

    }
}

/*
-Keywords-={KeywordItem}
-Subject-={KeywordItem}
-TagsList-={KeywordItem}
-CatalogSets-={KeywordItem}

-Keywords+={KeywordItem}
-Subject+={KeywordItem}
-TagsList+={KeywordItem}
-CatalogSets+={KeywordItem}


-charset
filename=UTF8
-overwrite_original
-m
-F
{IfLocationDateTimeChanged}-XMP-exif:GPSDateTime={LocationDateTimeUTC}
{IfLocationDateTimeChanged}-XMP:GPSDateTime={LocationDateTimeUTC}
{IfLocationDateTimeChanged}-GPS:GPSDateStamp={LocationDateTimeDateStamp}
{IfLocationDateTimeChanged}-GPS:GPSTimeStamp={LocationDateTimeTimeStamp}
{IfLocationDateTimeChanged}-GPSDateStamp={LocationDateTimeDateStamp}
{IfLocationDateTimeChanged}-GPSTimeStamp={LocationDateTimeTimeStamp}
{IfMediaDateTakenChanged}-Composite:SubSecCreateDate={MediaDateTaken}
{IfMediaDateTakenChanged}-EXIF:CreateDate={MediaDateTaken}
{IfMediaDateTakenChanged}-XMP-xmp:CreateDate={MediaDateTaken}
{IfMediaDateTakenChanged}-XMP:CreateDate={MediaDateTaken}
{IfMediaDateTakenChanged}-XMP:DateTimeOriginal={MediaDateTaken}
{IfMediaDateTakenChanged}-IPTC:DigitalCreationDate={MediaDateTakenDateStamp}
{IfMediaDateTakenChanged}-IPTC:DigitalCreationTime={MediaDateTakenTimeStamp}
{IfMediaDateTakenChanged}-Composite:SubSecDateTimeOriginal={MediaDateTaken}
{IfMediaDateTakenChanged}-ExifIFD:DateTimeOriginal={MediaDateTaken}
{IfMediaDateTakenChanged}-EXIF:DateTimeOriginal={MediaDateTaken}
{IfMediaDateTakenChanged}-XMP-photoshop:DateCreated={MediaDateTaken}
{IfMediaDateTakenChanged}-IPTC:DateCreated={MediaDateTakenDateStamp}
{IfMediaDateTakenChanged}-IPTC:TimeCreated={MediaDateTakenTimeStamp}
{IfMediaDateTakenChanged}-CreateDate={MediaDateTaken}
{IfPersonalAlbumChanged}-XMP-xmpDM:Album={PersonalAlbum}
{IfPersonalAlbumChanged}-XMP:Album={PersonalAlbum}
{IfPersonalAlbumChanged}-IPTC:Headline={PersonalAlbum}
{IfPersonalAlbumChanged}-XMP-photoshop:Headline={PersonalAlbum}
{IfPersonalAlbumChanged}-ItemList:Album={PersonalAlbum}
{IfPersonalAuthorChanged}-EXIF:Artist={PersonalAuthor}
{IfPersonalAuthorChanged}-IPTC:By-line={PersonalAuthor}
{IfPersonalAuthorChanged}-EXIF:XPAuthor={PersonalAuthor}
{IfPersonalAuthorChanged}-ItemList:Author={PersonalAuthor}
{IfPersonalAuthorChanged}-Creator={PersonalAuthor}
{IfPersonalCommentsChanged}-File:Comment={PersonalComments}
{IfPersonalCommentsChanged}-ExifIFD:UserComment={PersonalComments}
{IfPersonalCommentsChanged}-EXIF:UserComment={PersonalComments}
{IfPersonalCommentsChanged}-EXIF:XPComment={PersonalComments}
{IfPersonalCommentsChanged}-XMP-album:Notes={PersonalComments}
{IfPersonalCommentsChanged}-XMP-acdsee:Notes={PersonalComments}
{IfPersonalCommentsChanged}-XMP:UserComment={PersonalComments}
{IfPersonalCommentsChanged}-XMP:Notes={PersonalComments}
{IfPersonalCommentsChanged}-ItemList:Comment={PersonalComments}
{IfPersonalDescriptionChanged}-EXIF:ImageDescription={PersonalDescription}
{IfPersonalDescriptionChanged}-XMP:ImageDescription={PersonalDescription}
{IfPersonalDescriptionChanged}-XMP-dc:Description={PersonalDescription}
{IfPersonalDescriptionChanged}-XMP:Description={PersonalDescription}
{IfPersonalDescriptionChanged}-IPTC:Caption-Abstract={PersonalDescription}
{IfPersonalDescriptionChanged}-ItemList:Description={PersonalDescription}
{IfPersonalDescriptionChanged}-Description={PersonalDescription}
{IfPersonalRatingChanged}-XMP-microsoft:RatingPercent={PersonalRatingPercent}
{IfPersonalRatingChanged}-XMP:RatingPercent={PersonalRatingPercent}
{IfPersonalRatingChanged}-EXIF:RatingPercent={PersonalRatingPercent}
{IfPersonalRatingChanged}-XMP-xmp:Rating={PersonalRating}
{IfPersonalRatingChanged}-XMP:Rating={PersonalRating}
{IfPersonalRatingChanged}-XMP-acdsee:Rating={PersonalRating}
{IfPersonalRatingChanged}-EXIF:Rating={PersonalRating}
{IfPersonalRatingChanged}-Rating={PersonalRating}
{IfPersonalTitleChanged}-ItemList:Title={PersonalTitle}
{IfPersonalTitleChanged}-EXIF:XPTitle={PersonalTitle}
{IfPersonalTitleChanged}-XMP-dc:Title={PersonalTitle}
{IfPersonalTitleChanged}-XMP:Title={PersonalTitle}
{IfPersonalTitleChanged}-ItemList:Title={PersonalTitle}
{IfLocationLatitudeChanged}-EXIF:GPSLatitude={LocationLatitude}
{IfLocationLatitudeChanged}-XMP-exif:GPSLatitude={LocationLatitude}
{IfLocationLatitudeChanged}-XMP:GPSLatitude={LocationLatitude}
{IfLocationLatitudeChanged}-GPS:GPSLatitude={LocationLatitude}
{IfLocationLatitudeChanged}-GPSLatitude={LocationLatitude}
{IfLocationLongitudeChanged}-EXIF:GPSLongitude={LocationLongitude}
{IfLocationLongitudeChanged}-XMP-exif:GPSLongitude={LocationLongitude}
{IfLocationLongitudeChanged}-XMP:GPSLongitude={LocationLongitude}
{IfLocationLongitudeChanged}-GPS:GPSLongitude={LocationLongitude}
{IfLocationLongitudeChanged}-GPSLongitude={LocationLongitude}
{IfLocationNameChanged}-XMP:Location={LocationName}
{IfLocationNameChanged}-XMP-iptcCore:Location={LocationName}
{IfLocationNameChanged}-XMP-iptcExt:LocationShownSublocation={LocationName}
{IfLocationNameChanged}-XMP:LocationCreatedSublocation={LocationName}
{IfLocationNameChanged}-IPTC:Sub-location={LocationName}
{IfLocationNameChanged}-Sub-location={LocationName}
{IfLocationNameChanged}-Location={LocationName}
{IfLocationStateChanged}-XMP-iptcExt:LocationShownProvinceState={LocationState}
{IfLocationStateChanged}-XMP-photoshop:State={LocationState}
{IfLocationStateChanged}-IPTC:Province-State={LocationState}
{IfLocationStateChanged}-XMP:State={LocationState}
{IfLocationStateChanged}-State={LocationState}
{IfLocationCityChanged}-XMP-photoshop:City={LocationCity}
{IfLocationCityChanged}-XMP-iptcExt:LocationShownCity={LocationCity}
{IfLocationCityChanged}-IPTC:City={LocationCity}
{IfLocationCityChanged}-XMP:City={LocationCity}
{IfLocationCityChanged}-City={LocationCity}
{IfLocationCountryChanged}-IPTC:Country-PrimaryLocationName={LocationCountry}
{IfLocationCountryChanged}-XMP-photoshop:Country={LocationCountry}
{IfLocationCountryChanged}-XMP-iptcExt:LocationShownCountryName={LocationCountry}
{IfLocationCountryChanged}-XMP:Country={LocationCountry}
{IfLocationCountryChanged}-Country={LocationCountry}
{IfPersonalRegionChanged}-ImageRegion=
{IfPersonalRegionChanged}-RegionInfoMP={PersonalRegionInfoMP}
{IfPersonalRegionChanged}-RegionInfo={PersonalRegionInfo}
{IfPersonalKeywordsChanged}-Subject=
{IfPersonalKeywordsChanged}-Keyword=
{IfPersonalKeywordsChanged}-Keywords=
{IfPersonalKeywordsChanged}-XPKeywords=
{IfPersonalKeywordsChanged}-Category=
{IfPersonalKeywordsChanged}-Categories=
{IfPersonalKeywordsChanged}-CatalogSets=
{IfPersonalKeywordsChanged}-HierarchicalKeywords=
{IfPersonalKeywordsChanged}-HierarchicalSubject=
{IfPersonalKeywordsChanged}-LastKeywordXMP=
{IfPersonalKeywordsChanged}-LastKeywordIPTC=
{IfPersonalKeywordsChanged}-TagsList=
{IfPersonalKeywordsChanged}{PersonalKeywordItemsDelete}
{IfPersonalKeywordsChanged}{PersonalKeywordItemsAdd}
{IfPersonalKeywordsChanged}-Categories={PersonalKeywordsXML}
{IfPersonalKeywordsChanged}-XPKeywords={PersonalKeywordsList}
{FileFullPath}
-execute
*/

