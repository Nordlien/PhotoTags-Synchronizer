using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhotoTagsSynchronizer
{
    public partial class Form_AutoCorrect : Form
    {
        public string Album { get { return comboBoxAlbum.Text; } }
        public string Author { get { return comboBoxAuthor.Text; } }
        public string Comments { get { return comboBoxComments.Text; } }
        public string Description { get { return comboBoxDescription.Text; } }
        public string Title { get { return comboBoxTitle.Text; } }

        public bool UseAlbum { get { return checkBoxAlbum.Checked; } }
        public bool UseAuthor { get { return checkBoxAuthor.Checked; } }
        public bool UseComments { get { return checkBoxComments.Checked; } }
        public bool UseDescription { get { return checkBoxDescription.Checked; } }
        public bool UseTitle { get { return checkBoxTitle.Checked; } }

        private bool isPopulation = false;
        


        public Form_AutoCorrect()
        {
            InitializeComponent();
            this.DialogResult = DialogResult.None;
            isPopulation = true;
            ComboBoxHandler.ComboBoxPopulate(comboBoxAlbum, Properties.Settings.Default.AutoCorrectFormAlbum, "");
            ComboBoxHandler.ComboBoxPopulate(comboBoxAuthor, Properties.Settings.Default.AutoCorrectFormAuthor, "");
            ComboBoxHandler.ComboBoxPopulate(comboBoxComments, Properties.Settings.Default.AutoCorrectFormComments, "");
            ComboBoxHandler.ComboBoxPopulate(comboBoxDescription, Properties.Settings.Default.AutoCorrectFormDescription, "");
            ComboBoxHandler.ComboBoxPopulate(comboBoxTitle, Properties.Settings.Default.AutoCorrectFormTitle, "");
            checkBoxAlbum.Checked = Properties.Settings.Default.UseAutoCorrectFormAlbum;
            checkBoxAuthor.Checked = Properties.Settings.Default.UseAutoCorrectFormAuthor;
            checkBoxComments.Checked = Properties.Settings.Default.UseAutoCorrectFormComments;
            checkBoxDescription.Checked = Properties.Settings.Default.UseAutoCorrectFormDescription;
            checkBoxTitle.Checked = Properties.Settings.Default.UseAutoCorrectFormTitle;
            
            isPopulation = false;
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

            ComboBoxHandler.ComboBoxAddTextToList(comboBoxAlbum);
            ComboBoxHandler.ComboBoxAddTextToList(comboBoxAuthor);
            ComboBoxHandler.ComboBoxAddTextToList(comboBoxComments);
            ComboBoxHandler.ComboBoxAddTextToList(comboBoxDescription);
            ComboBoxHandler.ComboBoxAddTextToList(comboBoxTitle);


            Properties.Settings.Default.AutoCorrectFormAlbum = ComboBoxHandler.ComboBoxStringCollection(comboBoxAlbum);
            Properties.Settings.Default.AutoCorrectFormAuthor = ComboBoxHandler.ComboBoxStringCollection(comboBoxAuthor);
            Properties.Settings.Default.AutoCorrectFormComments = ComboBoxHandler.ComboBoxStringCollection(comboBoxComments);
            Properties.Settings.Default.AutoCorrectFormDescription = ComboBoxHandler.ComboBoxStringCollection(comboBoxDescription);
            Properties.Settings.Default.AutoCorrectFormTitle = ComboBoxHandler.ComboBoxStringCollection(comboBoxTitle);
            this.DialogResult = DialogResult.OK;
        }

    }
}
