using ApplicationAssociations;
using Exiftool;
using ImageAndMovieFileExtentions;
using MetadataLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Collections;

namespace PhotoTagsSynchronizer
{
    public partial class RunCommand : Form
    {
        public string ArguFile { get; set; } = "";
        public List<Metadata> Metadatas { get; set; } = new List<Metadata>();
        public List<string> AllowedFileNameDateTimeFormats { get; set; } = new List<string>();
        private bool isPopulation = false;

        private ApplicationAssociationsHandler applicationAssociationsHandler = new ApplicationAssociationsHandler();
        private SortedList<string, ApplicationData> applicationDatas;
        
        public RunCommand()
        {
            InitializeComponent();
            
        }

        #region Common - Init - Load Config
        public void Init()
        {
            isPopulation = true;
            //Populate Vaiable combobox
            //comboBoxArgumentFileCommandVariables.Items.AddRange(Metadata.ListOfProperties());
            comboBoxArgumentFileCommandVariables.Items.Add("{TempFileArgumentFullPath}");
            comboBoxBatchCommandCommandVariables.Items.AddRange(Metadata.ListOfProperties());

            //List of files info box
            foreach (Metadata metadata in Metadatas)
            {
                textBoxOpenWithSelectedFiles.Text += (textBoxOpenWithSelectedFiles.Text == "" ? "" : "\r\n") + metadata.FileFullPath;
                textBoxBatchCommandSelectedFiles.Text += (textBoxBatchCommandSelectedFiles.Text == "" ? "" : "\r\n") + metadata.FileFullPath;
            }

            //Argu file
            textBoxArgumentFileArgumentFile.Text = ArguFile;
            isPopulation = false;

            //Commands
            textBoxArgumentFileCommand.Text = Properties.Settings.Default.RunArgumentCommand;
            textBoxBatchCommandCommand.Text = Properties.Settings.Default.RunBatchCommand;
            textBoxOpenImageWithCommand.Text = Properties.Settings.Default.RunOpenPictureWith;
            textBoxOpenVideoWithCommand.Text = Properties.Settings.Default.RunOpenVideoWith;
            checkBoxBatchCommandWaitForExit.Checked = Properties.Settings.Default.RunBatchCommandWaitExit;
            checkBoxOpenImageWithWaitForExit.Checked = Properties.Settings.Default.RunOpenImageWithWaitExit;
            checkBoxOpenVideoWithWaitForExit.Checked = Properties.Settings.Default.RunOpenVideoWithWaitExit;

            //DataGridView video Open With...
            List<string> videoExtensions = ImageAndMovieFileExtentionsUtility.GetVideoExtension(Metadatas);
            List<string> imageExtensions = ImageAndMovieFileExtentionsUtility.GetImageExtension(Metadatas);

            PopulateOpenWithDataGridView(dataGridViewImages, imageExtensions);
            PopulateOpenWithDataGridView(dataGridViewVideos, videoExtensions);
            SelectApplicationRow(dataGridViewImages, textBoxOpenImageWithCommand.Text);
            SelectApplicationRow(dataGridViewVideos, textBoxOpenVideoWithCommand.Text);

            //Batch run
            //applicationDatas = ApplicationInstalled.ListInstalledApps2();
            applicationDatas = AppxPackage.GetApps();
            for (int index = 0; index < applicationDatas.Count; index++)
            {
                comboBoxBatchCommandImageApp.Items.Add(applicationDatas.Values[index].FriendlyAppName);
            }
        }
        #endregion 

        #region Common - FormClosing - Save Config
        private void RunCommand_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.RunArgumentCommand = textBoxArgumentFileCommand.Text;
            Properties.Settings.Default.RunBatchCommand = textBoxBatchCommandCommand.Text;
            Properties.Settings.Default.RunOpenPictureWith = textBoxOpenImageWithCommand.Text;
            Properties.Settings.Default.RunOpenVideoWith = textBoxOpenVideoWithCommand.Text;
            Properties.Settings.Default.RunBatchCommandWaitExit = checkBoxBatchCommandWaitForExit.Checked;
            Properties.Settings.Default.RunOpenImageWithWaitExit = checkBoxOpenImageWithWaitForExit.Checked;
            Properties.Settings.Default.RunOpenVideoWithWaitExit = checkBoxOpenVideoWithWaitForExit.Checked;
            Properties.Settings.Default.Save();
        }
        #endregion

        #region Common Selection Change DropDown - Insert Textbox
        private void SelectionChangeCommitted(TextBox textBox, string insertText)
        {
            if (!isPopulation)
            {
                textBox.Focus();
                var selectionIndex = textBox.SelectionStart;
                textBox.Text = textBox.Text.Remove(selectionIndex, textBox.SelectionLength);
                textBox.Text = textBox.Text.Insert(selectionIndex, insertText);
                textBox.SelectionStart = selectionIndex + insertText.Length;
            }
        }
        #endregion 

        #region OpenWith - SelectApplicationRow
        private void SelectApplicationRow(DataGridView dataGridView, string applicationLink)
        {
            for (int rowIndex = 0; rowIndex < dataGridView.Rows.Count; rowIndex++)
            {
                if (dataGridView.Rows[rowIndex].Cells["Command"].Value.ToString() == applicationLink)
                {
                    dataGridView.CurrentCell = dataGridView[1, rowIndex];
                    dataGridView.Rows[rowIndex].Selected = true;
                }
            }
        }
        #endregion

        #region OpenWith - PopulateOpenWithDataGridView
        private void PopulateOpenWithDataGridView(DataGridView dataGridView, List<string> extensions)
        {
            try
            {
                DataTable dtApps = new DataTable("Application");
                dtApps.Columns.Add("Icon", typeof(Icon));
                dtApps.Columns.Add("Name");
                dtApps.Columns.Add("Command");
                dtApps.Columns.Add("ProgId");
                dtApps.Columns.Add("AppId");

                List<ApplicationData> listOfCommonOpenWith = applicationAssociationsHandler.OpenWithInCommon(extensions);

                if (listOfCommonOpenWith != null && listOfCommonOpenWith.Count > 0)
                {
                    foreach (ApplicationData data in listOfCommonOpenWith)
                    {
                        dtApps.Rows.Add(new object[] { data.Icon, data.FriendlyAppName, data.Command, data.ProgId, data.ApplicationId });
                    }
                    dataGridView.Enabled = true;
                }
                else
                {
                    dataGridView.Enabled = false;
                }

                dataGridView.DataSource = dtApps;                
                dataGridView.Columns["Icon"].Width = 50;
                dataGridView.Columns["Name"].Width = 240;
                dataGridView.Columns["Command"].Visible = true;
                dataGridView.Columns["ProgId"].Visible = true;
                dataGridView.Columns["AppId"].Visible = true;
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message.ToString());
                MessageBox.Show(ex.Message.ToString());
            }
        }
        #endregion

        #region OpenWith - CellEnter - Select row
        private void dataGridViewImages_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = dataGridViewImages;
            dataGridView.Rows[e.RowIndex].Selected = true;
        }

        private void dataGridViewVideos_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = dataGridViewVideos;
            dataGridView.Rows[e.RowIndex].Selected = true;
        }
        #endregion 

        #region OpenWith - FillComboBoxWithVerbs
        private void FillComboBoxWithVerbs(ComboBox comboBox, string progId)
        {
            comboBox.Items.Clear();
            comboBox.Text = "";

            if (!string.IsNullOrEmpty(progId))
            {
                List<VerbLink> verbLinks = applicationAssociationsHandler.GetVerbLinks(progId);
                if (verbLinks != null)
                {
                    foreach (VerbLink verbLink in verbLinks) comboBox.Items.Add(verbLink.Verb);
                }
            }
        }
        #endregion

        #region OpenWith - Select Verb
        private void SelectVerb(ComboBox comboBox, string verb)
        {
            int foundIndex = -1;
            for (int index = 0; index < comboBox.Items.Count; index++)
            {
                if (comboBox.Items[index].ToString().Equals(verb, StringComparison.InvariantCultureIgnoreCase)) foundIndex = index;
            }
            if (foundIndex != -1) comboBox.SelectedIndex = foundIndex;
        }
        #endregion

        #region OpenWith - Image SelectionChanged
        private void dataGridViewImages_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewImages;

            if (dataGridView.SelectedRows.Count == 1)
            {
                textBoxOpenImageWithApplication.Text = dataGridView.Rows[dataGridView.SelectedRows[0].Index].Cells["Name"].Value.ToString();
                textBoxOpenImageWithCommand.Text = dataGridView.Rows[dataGridView.SelectedRows[0].Index].Cells["Command"].Value.ToString();
                textBoxOpenImageWithAppId.Text = dataGridView.Rows[dataGridView.SelectedRows[0].Index].Cells["AppId"].Value.ToString();

                ComboBox comboBox = comboBoxOpenImageWithVerbs;
                FillComboBoxWithVerbs(comboBox, dataGridView.Rows[dataGridView.SelectedRows[0].Index].Cells["ProgId"].Value.ToString());
                SelectVerb(comboBox, "Open");
                if (string.IsNullOrWhiteSpace(comboBox.Text)) SelectVerb(comboBox, "Edit");
                if (string.IsNullOrWhiteSpace(comboBox.Text)) SelectVerb(comboBox, "Play");
            }
        }
        #endregion

        #region OpenWith - Video SelectionChanged
        private void dataGridViewVideos_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewVideos;

            if (dataGridView.SelectedRows.Count == 1)
            {
                textBoxOpenVideoWithApplication.Text = dataGridView.Rows[dataGridView.SelectedRows[0].Index].Cells["Name"].Value.ToString();
                textBoxOpenVideoWithCommand.Text = dataGridView.Rows[dataGridView.SelectedRows[0].Index].Cells["Command"].Value.ToString();
                textBoxOpenVideoWithAppId.Text = dataGridView.Rows[dataGridView.SelectedRows[0].Index].Cells["AppId"].Value.ToString();
                FillComboBoxWithVerbs(comboBoxOpenVideoWithVerbs, dataGridView.Rows[dataGridView.SelectedRows[0].Index].Cells["ProgId"].Value.ToString());
                ComboBox comboBox = comboBoxOpenVideoWithVerbs;
                FillComboBoxWithVerbs(comboBox, dataGridView.Rows[dataGridView.SelectedRows[0].Index].Cells["ProgId"].Value.ToString());
                SelectVerb(comboBox, "Open");
                if (string.IsNullOrWhiteSpace(comboBox.Text)) SelectVerb(comboBox, "Edit");
                if (string.IsNullOrWhiteSpace(comboBox.Text)) SelectVerb(comboBox, "Play");
            }
        }
        #endregion

        #region OpenWith - GetSelectedRowIndex
        private int GetSelectedRowIndex (DataGridView dataGridView)
        {
            if (dataGridView.SelectedRows.Count >= 1) return dataGridView.SelectedRows[0].Index;
            if (dataGridView.SelectedCells.Count >= 1) return dataGridView.SelectedCells[0].RowIndex;
            return -1;
        }
        #endregion

        #region OpenWith - Image - New verb selected
        private void comboBoxOpenImageWithVerbs_SelectionChangeCommitted(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewImages;
            int rowIndex = GetSelectedRowIndex(dataGridView);
            if (rowIndex > -1)
            {
                VerbLink verbLink = applicationAssociationsHandler.GetVerbLink(dataGridView.Rows[rowIndex].Cells["ProgId"].Value.ToString(), comboBoxOpenImageWithVerbs.SelectedItem.ToString());
                if (verbLink != null) textBoxOpenImageWithCommand.Text = verbLink.Command;
            }
        }
        #endregion

        #region OpenWith - Video - New verb selected
        private void comboBoxOpenVideoWithVerbs_SelectionChangeCommitted(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewVideos;
            int rowIndex = GetSelectedRowIndex(dataGridView);
            if (rowIndex > -1)
            {
                VerbLink verbLink = applicationAssociationsHandler.GetVerbLink(dataGridView.Rows[rowIndex].Cells["ProgId"].Value.ToString(), comboBoxOpenImageWithVerbs.SelectedItem.ToString());
                if (verbLink != null) textBoxOpenVideoWithCommand.Text = verbLink.Command;
            }
                       
        }
        #endregion 

        #region OpenWith - Run
        private void buttonOpenWithOpenWith_Click(object sender, EventArgs e)
        {
            string errors = "";
            foreach (Metadata metadata in Metadatas)
            {
                try
                {
                    if (ImageAndMovieFileExtentionsUtility.IsImageFormat(metadata.FileFullPath))
                        ApplicationActivation.ProcessRun(textBoxOpenImageWithCommand.Text, textBoxOpenImageWithAppId.Text, metadata.FileFullPath, comboBoxOpenImageWithVerbs.Text, checkBoxOpenImageWithWaitForExit.Checked);
                    if (ImageAndMovieFileExtentionsUtility.IsVideoFormat(metadata.FileFullPath))
                        ApplicationActivation.ProcessRun(textBoxOpenVideoWithCommand.Text, textBoxOpenVideoWithAppId.Text, metadata.FileFullPath, comboBoxOpenVideoWithVerbs.Text, checkBoxOpenVideoWithWaitForExit.Checked);
                }
                catch (Exception ex)
                {
                    errors += (errors == "" ? "" : "\r\n") + "File: " + metadata.FileFullPath + "\r\nError message: " + ex.Message;
                }
            }
            if (errors != "") MessageBox.Show(errors);

        }
        #endregion

        #region Batch run - textBoxBatchCommandCommand_TextChanged
        private void textBoxBatchCommandCommand_TextChanged(object sender, EventArgs e)
        {
            textBoxCommandCommandExample.Text = "";
            foreach (Metadata metadata in Metadatas)
            {
                textBoxCommandCommandExample.Text += (textBoxCommandCommandExample.Text == "" ? "" : "\r\n") + metadata.ReplaceVariables(textBoxBatchCommandCommand.Text, AllowedFileNameDateTimeFormats);
            }
        }
        #endregion

        #region Batch run - Click
        private void buttonBatchCommandBatchRun_Click(object sender, EventArgs e)
        {
            string errors = "";
            foreach (Metadata metadata in Metadatas)
            {
                string commandWithArguments = metadata.ReplaceVariables(textBoxBatchCommandCommand.Text, AllowedFileNameDateTimeFormats);
                try
                {
                    ApplicationActivation.ProcessRun(commandWithArguments, checkBoxBatchCommandWaitForExit.Checked);
                } catch (Exception ex)
                {
                    errors += (errors == "" ? "" : "\r\n") + "File: " + metadata.FileFullPath + "\r\nError message: " + ex.Message;
                }
            }
            if (errors != "") MessageBox.Show(errors);
        }
        #endregion

        #region Batch run - Variable selected
        private void comboBoxBatchCommandCommandVariables_SelectionChangeCommitted(object sender, EventArgs e)
        {
            SelectionChangeCommitted(textBoxBatchCommandCommand, comboBoxBatchCommandCommandVariables.Text);
        }
        #endregion 

        private void buttonOpenImageWithBrowser_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select exe file you want to use for Image files.";
            openFileDialog.Filter = "Executeable file (*.exe)|*.exe";
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.Multiselect = false;
            openFileDialog.ReadOnlyChecked = false;
            openFileDialog.ShowReadOnly = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxOpenImageWithCommand.Text = openFileDialog.FileName;
            }
        }

        private void buttonOPenVideoWithBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select exe file you want to use for Video files.";
            openFileDialog.Filter = "Executeable file (*.exe)|*.exe";
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.Multiselect = false;
            openFileDialog.ReadOnlyChecked = false;
            openFileDialog.ShowReadOnly = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxOpenVideoWithCommand.Text = openFileDialog.FileName;
            }
        }

        #region ArumentFile run - Click 
        private void buttonArgumentFileRun_Click(object sender, EventArgs e)
        {
            string tempArguFileFullPath = ExiftoolWriter.GetTempArguFileFullPath();
            string commandWithArguments = textBoxArgumentFileCommand.Text.Replace("{TempFileArgumentFullPath}", tempArguFileFullPath);

            System.IO.File.WriteAllText(tempArguFileFullPath, textBoxArgumentFileArgumentFile.Text);
            try
            {
                ApplicationActivation.ProcessRun(commandWithArguments, false);
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ArgumentFile run - Variable selected
        private void comboBoxArgumentFileCommandVariables_SelectionChangeCommitted(object sender, EventArgs e)
        {
            SelectionChangeCommitted(textBoxArgumentFileCommand, comboBoxArgumentFileCommandVariables.Text);
        }


        #endregion


        // These could be properties used to customize the ComboBox appearance
        Color cboForeColor = Color.Black;
        Color cboBackColor = Color.WhiteSmoke;
        private void comboBoxBatchCommandImageApp_DrawItem(object sender, DrawItemEventArgs e)
        {
            //ApplicationInstalled
            ComboBox comboBox = (ComboBox)sender; //comboBoxBatchCommandImageApp;
            if (e.Index < 0) return;
            Color foreColor = e.ForeColor;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            if (e.State.HasFlag(DrawItemState.Focus) && !e.State.HasFlag(DrawItemState.ComboBoxEdit))
            {
                e.DrawBackground();
                e.DrawFocusRectangle();
            }
            else
            {
                using (Brush backgbrush = new SolidBrush(cboBackColor))
                {
                    e.Graphics.FillRectangle(backgbrush, e.Bounds);
                    foreColor = cboForeColor;
                }
            }
            using (Brush textbrush = new SolidBrush(foreColor))
            {
                e.Graphics.DrawString(comboBox.Items[e.Index].ToString(),
                                      e.Font, textbrush, e.Bounds.Height + 10, e.Bounds.Y,
                                      StringFormat.GenericTypographic);
            }

            //applicationDatas.Values[index]
            e.Graphics.DrawImage(applicationDatas.Values[e.Index].Icon.ToBitmap(),
                                 new Rectangle(e.Bounds.Location,
                                 new Size(e.Bounds.Height - 2, e.Bounds.Height - 2)));
            
        }
    }
}
