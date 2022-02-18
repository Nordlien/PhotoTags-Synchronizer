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
            string[] listOfProperties = Metadata.ListOfPropertiesCombined(addKeywordItem);

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
