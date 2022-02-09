using FastColoredTextBoxNS;
using System.Windows.Forms;
using static Manina.Windows.Forms.ImageListView;
using Krypton.Toolkit;
using System;

namespace PhotoTagsSynchronizer
{

    public class ComboBoxHandler
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        #region SelectionChangeCommitted - KryptonTextBox
        public static void SelectionChangeCommitted(KryptonTextBox textBox, string insertText)
        {
            try
            {
                textBox.Focus();
                Clipboard.SetText(insertText);
                textBox.Paste();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region SelectionChangeCommitted - FastColoredTextBox
        public static void SelectionChangeCommitted(FastColoredTextBox textBox, string insertText)
        {
            try
            {
                textBox.Focus();
                var selectionIndex = textBox.SelectionStart;
                textBox.Text = textBox.Text.Remove(selectionIndex, textBox.SelectionLength);
                textBox.Text = textBox.Text.Insert(selectionIndex, insertText);
                textBox.SelectionStart = selectionIndex + insertText.Length;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region RemeberComboBoxSelection
        public static void RemeberComboBoxSelection(KryptonComboBox comboBox)
        {
            try
            {
                comboBox.Tag = new ComboBoxSelection(comboBox);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region SetComboBoxSelection
        public static void SetComboBoxSelection(KryptonComboBox comboBox)
        {
            try
            {
                if (comboBox.Tag is ComboBoxSelection comboBoxSelection)
                {
                    comboBox.SelectionStart = comboBoxSelection.SelectionStart;
                    comboBox.SelectionLength = comboBoxSelection.SelectionLength;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ComboBox Collection to string
        public static string ComboBoxStringCollection(KryptonComboBox comboBox)
        {
            string resultListString = "";
            try
            {
                foreach (object item in comboBox.Items)
                {
                    if (item != null && !string.IsNullOrWhiteSpace((string)item)) resultListString += (resultListString == "" ? "" : "\r\n") + item.ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            return resultListString;
        }
        #endregion

        #region ConvertStringToArray
        public static string[] ConvertStringToArray(string valueListString)
        {
            return valueListString.Replace("\r\n", "\n").Split('\n');
        }
        #endregion

        #region Populate ComboBox - Populate - string[]
        public static void ComboBoxPopulateAppend(KryptonComboBox comboBox, string valueItem)
        {
            if (comboBox.Items.Count >= maxCount) return;
            if (!string.IsNullOrEmpty(valueItem) && !comboBox.Items.Contains(valueItem)) comboBox.Items.Add(valueItem);
        }

        public static void ComboBoxPopulateAppend(KryptonComboBox comboBox, string[] valueList, string defaultValue)
        {
            try
            {
                if (valueList != null)
                {
                    foreach (string valueItem in valueList) ComboBoxPopulateAppend(comboBox, valueItem);                    
                }
                comboBox.Text = defaultValue == null ? "" : defaultValue;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region Populate ComboBox - Populate - string
        public static void ComboBoxPopulateClear(KryptonComboBox comboBox, string valueListString, string defaultValue)
        {
            comboBox.Items.Clear();
            ComboBoxPopulateAppend(comboBox, ConvertStringToArray(valueListString), defaultValue);
        }
        #endregion

        const int maxCount = 30;
        #region ComboBox - Remeber last text and Add Text to list
        public static void ComboBoxAddLastTextFirstInList(KryptonComboBox comboBox)
        {
            try
            {
                string text = comboBox.Text;
                if (!string.IsNullOrWhiteSpace(comboBox.Text))
                {
                    int indexOfText = comboBox.Items.IndexOf(text); //Does it exist from before, remove to put first
                    if (indexOfText > -1) comboBox.Items.RemoveAt(indexOfText); //Remove if exist, in not already first
                    comboBox.Items.Insert(0, text); //Add first

                    while (comboBox.Items.Count > maxCount) comboBox.Items.RemoveAt(maxCount);
                    comboBox.Text = text;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region AutoCompleteStringCollection - Settings - Convert List to string
        public static string AutoCompleteStringCollectionToString(AutoCompleteStringCollection autoCompleteStringCollection)
        {
            string resultListString = "";
            try
            {
                foreach (string item in autoCompleteStringCollection)
                {
                    if (!string.IsNullOrWhiteSpace(item)) resultListString += (resultListString == "" ? "" : "\r\n") + item.ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            return resultListString;
        }
        #endregion

        #region AutoCompleteStringCollection - Remeber last text and Add Text to list
        public static void AutoCompleteStringCollectionAppend(AutoCompleteStringCollection autoCompleteStringCollection, string valueItem)
        {
            if (autoCompleteStringCollection.Count >= maxCount) return;
            if (!string.IsNullOrEmpty(valueItem) && !autoCompleteStringCollection.Contains((string)valueItem)) autoCompleteStringCollection.Add((string)valueItem);
        }

        public static void AutoCompleteStringCollectionAppend(AutoCompleteStringCollection autoCompleteStringCollection, string[] newValues)
        {
            foreach (string value in newValues) AutoCompleteStringCollectionAppend(autoCompleteStringCollection, value);            
        }

        public static void AddLastTextFirstInAutoCompleteStringCollection(AutoCompleteStringCollection autoCompleteStringCollection, string newValue)
        {
            if (!string.IsNullOrWhiteSpace(newValue))
            {
                try
                {
                    int indexOfText = autoCompleteStringCollection.IndexOf(newValue); //Does it exist from before, remove to put first
                    if (indexOfText > -1) autoCompleteStringCollection.RemoveAt(indexOfText); //Remove if exist, in not already first
                    autoCompleteStringCollection.Insert(0, newValue); //Add first

                    while (autoCompleteStringCollection.Count > maxCount) autoCompleteStringCollection.RemoveAt(maxCount);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
                }
            } 
        }
        #endregion
    }

    #region ComboBox - Text Selction Hack - Remember ComboBoxSelection
    public class ComboBoxSelection
    {
        public int SelectionStart { get; set; }
        public int SelectionLength { get; set; }

        public ComboBoxSelection(KryptonComboBox comboBox)
        {
            SelectionStart = comboBox.SelectionStart;
            SelectionLength = comboBox.SelectionLength;
        }
    }
    #endregion 
}



