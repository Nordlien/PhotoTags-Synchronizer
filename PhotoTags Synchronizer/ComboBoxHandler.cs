using FastColoredTextBoxNS;
using System.Windows.Forms;
using static Manina.Windows.Forms.ImageListView;
using Krypton.Toolkit;

namespace PhotoTagsSynchronizer
{

    public class ComboBoxHandler
    {
        public static void SelectionChangeCommitted(KryptonTextBox textBox, string insertText)
        {
            textBox.Focus();
            var selectionIndex = textBox.SelectionStart;
            textBox.Text = textBox.Text.Remove(selectionIndex, textBox.SelectionLength);
            textBox.Text = textBox.Text.Insert(selectionIndex, insertText);
            textBox.SelectionStart = selectionIndex + insertText.Length;
        }

        public static void SelectionChangeCommitted(FastColoredTextBox textBox, string insertText)
        {
            textBox.Focus();
            var selectionIndex = textBox.SelectionStart;
            textBox.Text = textBox.Text.Remove(selectionIndex, textBox.SelectionLength);
            textBox.Text = textBox.Text.Insert(selectionIndex, insertText);
            textBox.SelectionStart = selectionIndex + insertText.Length;
        }

        public static void RemeberComboBoxSelection(KryptonComboBox comboBox)
        {
            comboBox.Tag = new ComboBoxSelection(comboBox);
        }

        public static void SetComboBoxSelection(KryptonComboBox comboBox)
        {
            if (comboBox.Tag is ComboBoxSelection comboBoxSelection)
            {
                comboBox.SelectionStart = comboBoxSelection.SelectionStart;
                comboBox.SelectionLength = comboBoxSelection.SelectionLength;
            }
        }

        #region ComboBox - Settings - Convert List to string
        public static string ComboBoxStringCollection(KryptonComboBox comboBox)
        {
            string resultListString = "";
            foreach (object item in comboBox.Items)
            {
                resultListString += (resultListString == "" ? "" : "\r\n") + item.ToString();
            }
            return resultListString;
        }
        #endregion

        #region ComboBox - Settings - Convert String add to List
        public static void ComboBoxPopulate(KryptonComboBox comboBox, string valueListString, string defaultValue)
        {
            comboBox.Items.Clear();

            string[] valueList = valueListString.Replace("\r\n", "\n").Split('\n');
            if (valueList != null)
            {
                foreach (string valueItem in valueList)
                {
                    comboBox.Items.Add(valueItem);
                }
            }
            comboBox.Text = defaultValue == null ? "" : defaultValue;
        }
        #endregion

        #region ComboBox - Remeber last text and Add Text to list
        public static void ComboBoxAddTextToList(KryptonComboBox comboBox)
        {
            string text = comboBox.Text;
            int indexOfText = comboBox.Items.IndexOf(text); //Does it exist from before, remove to put first
            if (indexOfText > -1) comboBox.Items.RemoveAt(indexOfText); //Remove if exist, in not already first
            comboBox.Items.Insert(0, text); //Add first

            int maxCount = 15;
            while (comboBox.Items.Count > maxCount) comboBox.Items.RemoveAt(maxCount);
            comboBox.Text = text;
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



