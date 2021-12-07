using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Krypton.Toolkit;

namespace PhotoTagsSynchronizer
{
    public partial class FormAutoCorrect : KryptonForm
    {
        public string Album { get { return comboBoxAlbum.Text; } }
        public string Author { get { return comboBoxAuthor.Text; } }
        public string Comments { get { return comboBoxComments.Text; } }
        public string Description { get { return comboBoxDescription.Text; } }
        public string Title { get { return comboBoxTitle.Text; } }
        public List<string> Keywords
        {
            get
            {
                List<string> keywords = new List<string>();
                string[] keywordArray = textBoxKeywords.Text.Replace("\r\n", "\n").Split('\n');
                foreach (string keyword in keywordArray)
                {
                    if (!string.IsNullOrWhiteSpace(keyword) && !keywords.Contains(keyword)) keywords.Add(keyword);
                }
                return keywords;
            }
        }

        public bool UseAlbum { get { return checkBoxAlbum.Checked; } }
        public bool UseAuthor { get { return checkBoxAuthor.Checked; } }
        public bool UseComments { get { return checkBoxComments.Checked; } }
        public bool UseDescription { get { return checkBoxDescription.Checked; } }
        public bool UseTitle { get { return checkBoxTitle.Checked; } }

        public AutoCorrectFormVaraibles AutoCorrectFormVaraibles
        {
            get {
                AutoCorrectFormVaraibles autoCorrectFormVaraibles = new AutoCorrectFormVaraibles();
                autoCorrectFormVaraibles.Album = Album;
                autoCorrectFormVaraibles.Author = Author;
                autoCorrectFormVaraibles.Comments = Comments;
                autoCorrectFormVaraibles.Description = Description;
                autoCorrectFormVaraibles.Title = Title;
                autoCorrectFormVaraibles.Keywords = Keywords;

                autoCorrectFormVaraibles.UseAlbum = UseAlbum;
                autoCorrectFormVaraibles.UseAuthor = UseAuthor;
                autoCorrectFormVaraibles.UseComments = UseComments;
                autoCorrectFormVaraibles.UseDescription = UseDescription;
                autoCorrectFormVaraibles.UseTitle = UseTitle;
                return autoCorrectFormVaraibles;
            }
        }

        public FormAutoCorrect()
        {
            InitializeComponent();
            this.DialogResult = DialogResult.None;
            ComboBoxHandler.ComboBoxPopulateClear(comboBoxAlbum, Properties.Settings.Default.AutoCorrectFormAlbum, "");
            ComboBoxHandler.ComboBoxPopulateClear(comboBoxAuthor, Properties.Settings.Default.AutoCorrectFormAuthor, "");
            ComboBoxHandler.ComboBoxPopulateClear(comboBoxComments, Properties.Settings.Default.AutoCorrectFormComments, "");
            ComboBoxHandler.ComboBoxPopulateClear(comboBoxDescription, Properties.Settings.Default.AutoCorrectFormDescription, "");
            ComboBoxHandler.ComboBoxPopulateClear(comboBoxTitle, Properties.Settings.Default.AutoCorrectFormTitle, "");
            checkBoxAlbum.Checked = Properties.Settings.Default.UseAutoCorrectFormAlbum;
            checkBoxAuthor.Checked = Properties.Settings.Default.UseAutoCorrectFormAuthor;
            checkBoxComments.Checked = Properties.Settings.Default.UseAutoCorrectFormComments;
            checkBoxDescription.Checked = Properties.Settings.Default.UseAutoCorrectFormDescription;
            checkBoxTitle.Checked = Properties.Settings.Default.UseAutoCorrectFormTitle;
            textBoxKeywords.Text = "";

            AutoCorrect autoCorrect = AutoCorrect.ConvertConfigValue(Properties.Settings.Default.AutoCorrect);
            if (autoCorrect.UpdateDescription)
            {
                checkBoxDescription.Enabled = false;
                comboBoxDescription.Enabled = false;
                kryptonLabelDescriptionBecomesAlbum.Text = "Album values will overwrite Descriptions values, as setup in Config";
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonAutoCorrect_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.UseAutoCorrectFormAlbum = checkBoxAlbum.Checked;
            Properties.Settings.Default.UseAutoCorrectFormAuthor = checkBoxAuthor.Checked;
            Properties.Settings.Default.UseAutoCorrectFormComments = checkBoxComments.Checked;
            Properties.Settings.Default.UseAutoCorrectFormDescription = checkBoxDescription.Checked;
            Properties.Settings.Default.UseAutoCorrectFormTitle = checkBoxTitle.Checked;

            ComboBoxHandler.ComboBoxAddLastTextFirstInList(comboBoxAlbum);
            ComboBoxHandler.ComboBoxAddLastTextFirstInList(comboBoxAuthor);
            ComboBoxHandler.ComboBoxAddLastTextFirstInList(comboBoxComments);
            ComboBoxHandler.ComboBoxAddLastTextFirstInList(comboBoxDescription);
            ComboBoxHandler.ComboBoxAddLastTextFirstInList(comboBoxTitle);


            Properties.Settings.Default.AutoCorrectFormAlbum = ComboBoxHandler.ComboBoxStringCollection(comboBoxAlbum);
            Properties.Settings.Default.AutoCorrectFormAuthor = ComboBoxHandler.ComboBoxStringCollection(comboBoxAuthor);
            Properties.Settings.Default.AutoCorrectFormComments = ComboBoxHandler.ComboBoxStringCollection(comboBoxComments);
            Properties.Settings.Default.AutoCorrectFormDescription = ComboBoxHandler.ComboBoxStringCollection(comboBoxDescription);
            Properties.Settings.Default.AutoCorrectFormTitle = ComboBoxHandler.ComboBoxStringCollection(comboBoxTitle);
            this.DialogResult = DialogResult.OK;
        }

    }
}
