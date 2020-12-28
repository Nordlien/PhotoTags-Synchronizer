using ApplicationAssociations;
using Exiftool;
using ImageAndMovieFileExtentions;
using MetadataLibrary;
using MetadataPriorityLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace PhotoTagsSynchronizer
{
    public partial class RunCommand : Form
    {
        public string ArguFile { get; set; } = "";
        public string ArguFileAutoCorrect { get; set; } = "";


        public MetadataReadPrioity MetadataPrioity { get; set; } 
        public List<Metadata> Metadatas { get; set; } = new List<Metadata>();
        public List<string> AllowedFileNameDateTimeFormats { get; set; } = new List<string>();
        private bool isPopulation = false;

        private FastColoredTextBoxHandler fastColoredTextBoxHandlerRunArgumentFile = null;
        private FastColoredTextBoxHandler fastColoredTextBoxHandlerRunArgumentFileAutoCorrect = null;

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


            #region Tab - Argument file
            fastColoredTextBoxHandlerRunArgumentFile = new FastColoredTextBoxHandler(fastColoredTextBoxArgumentFileArgumentFile, false, MetadataPrioity.MetadataPrioityDictionary);
            fastColoredTextBoxHandlerRunArgumentFileAutoCorrect = new FastColoredTextBoxHandler(fastColoredTextBoxArgumentFileArgumentFileAutoCorrect, false, MetadataPrioity.MetadataPrioityDictionary);

            comboBoxArgumentFileCommandVariables.Items.Add("{TempFileArgumentFullPath}");
            ComboBoxPopulate(comboBoxArgumentFileCommand, Properties.Settings.Default.RunArgumentCommandList, Properties.Settings.Default.RunArgumentCommand);
            fastColoredTextBoxArgumentFileArgumentFile.Text = ArguFile;
            fastColoredTextBoxArgumentFileArgumentFileAutoCorrect.Text = ArguFileAutoCorrect;

            #endregion

            #region Tab - Run batch - Command
            comboBoxBatchRunImageVariables.Items.AddRange(Metadata.ListOfProperties(false));
            comboBoxBatchRunVideoVariables.Items.AddRange(Metadata.ListOfProperties(false));
            ComboBoxPopulate(comboBoxBatchRunImageCommand, Properties.Settings.Default.RunBatchImageCommandList, Properties.Settings.Default.RunBatchImageCommand);
            ComboBoxPopulate(comboBoxBatchRunVideoCommand, Properties.Settings.Default.RunBatchVideoCommandList, Properties.Settings.Default.RunBatchVideoCommand);
            checkBoxBatchRunImageWaitForCommandExit.Checked = Properties.Settings.Default.RunBatchImageWaitForCommand;
            checkBoxBatchRunVideoWaitForCommandExit.Checked = Properties.Settings.Default.RunBatchVideoWaitForCommand;
            #endregion

            #region Tab - Run batch - App
            foreach (Metadata metadata in Metadatas) textBoxBatchCommandSelectedFiles.Text += (textBoxBatchCommandSelectedFiles.Text == "" ? "" : "\r\n") + metadata.FileFullPath;

            applicationDatas = ApplicationInstalled.ListInstalledApps();
            for (int index = 0; index < applicationDatas.Count; index++)
            {
                comboBoxBatchRunImageAppExample.Items.Add(applicationDatas.Values[index].FriendlyAppName);
                comboBoxBatchRunVideoAppExample.Items.Add(applicationDatas.Values[index].FriendlyAppName);
            }
            ComboBoxPopulate(comboBoxBatchRunImageAppId, Properties.Settings.Default.RunBatchImageAppIdList, Properties.Settings.Default.RunBatchImageAppId);
            ComboBoxPopulate(comboBoxBatchRunVideoAppId, Properties.Settings.Default.RunBatchVideoAppIdList, Properties.Settings.Default.RunBatchVideoAppId);
            ComboBoxPopulate(comboBoxBatchRunImageVerb, Properties.Settings.Default.RunBatchImageVerbList, Properties.Settings.Default.RunBatchImageVerb);
            ComboBoxPopulate(comboBoxBatchRunVideoVerb, Properties.Settings.Default.RunBatchVideoVerbList, Properties.Settings.Default.RunBatchVideoVerb);
            checkBoxBatchRunImageWaitForAppExit.Checked = Properties.Settings.Default.RunBatchImageWaitForApp;
            checkBoxBatchRunVideoWaitForAppExit.Checked = Properties.Settings.Default.RunBatchVideoWaitForApp;
            #endregion

            #region Tab - Open with
            foreach (Metadata metadata in Metadatas) textBoxOpenWithSelectedFiles.Text += (textBoxOpenWithSelectedFiles.Text == "" ? "" : "\r\n") + metadata.FileFullPath;
            //DataGridView video Open With...
            List<string> videoExtensions = ImageAndMovieFileExtentionsUtility.GetVideoExtension(Metadatas);
            List<string> imageExtensions = ImageAndMovieFileExtentionsUtility.GetImageExtension(Metadatas);

            PopulateOpenWithDataGridView(dataGridViewImages, imageExtensions);
            PopulateOpenWithDataGridView(dataGridViewVideos, videoExtensions);

            SelectApplicationRow(dataGridViewImages, Properties.Settings.Default.OpenWithImageProgId);
            SelectApplicationRow(dataGridViewVideos, Properties.Settings.Default.OpenWithVideoProgId);

            comboBoxBatchRunImageVerb.Text = Properties.Settings.Default.OpenWithImageVerb;
            comboBoxBatchRunVideoVerb.Text = Properties.Settings.Default.OpenWithVideoVerb;

            checkBoxOpenImageWithWaitForExit.Checked = Properties.Settings.Default.OpenWithImageWaitForExit;
            checkBoxOpenVideoWithWaitForExit.Checked = Properties.Settings.Default.OpenWithVideoWaitForExit;
            #endregion


            isPopulation = false;

        }
        #endregion 

        #region Common - FormClosing - Save Config
        private void RunCommand_FormClosing(object sender, FormClosingEventArgs e)
        {
            #region Tab - Argument file
            Properties.Settings.Default.RunArgumentCommandList = ComboBoxStringCollection(comboBoxArgumentFileCommand);
            Properties.Settings.Default.RunArgumentCommand = comboBoxArgumentFileCommand.Text;
            #endregion

            #region Tab - Run batch - Command
            Properties.Settings.Default.RunBatchImageCommandList = ComboBoxStringCollection(comboBoxBatchRunImageCommand);
            Properties.Settings.Default.RunBatchImageCommand = comboBoxBatchRunImageCommand.Text;

            Properties.Settings.Default.RunBatchVideoCommandList = ComboBoxStringCollection(comboBoxBatchRunVideoCommand);
            Properties.Settings.Default.RunBatchVideoCommand = comboBoxBatchRunVideoCommand.Text;


            Properties.Settings.Default.RunBatchImageWaitForCommand = checkBoxBatchRunImageWaitForCommandExit.Checked;
            Properties.Settings.Default.RunBatchVideoWaitForCommand = checkBoxBatchRunVideoWaitForCommandExit.Checked;
            #endregion

            #region Tab - Run batch - App

            Properties.Settings.Default.RunBatchImageAppIdList = ComboBoxStringCollection(comboBoxBatchRunImageAppId);
            Properties.Settings.Default.RunBatchImageAppId = comboBoxBatchRunImageAppId.Text;

            Properties.Settings.Default.RunBatchVideoAppIdList = ComboBoxStringCollection(comboBoxBatchRunVideoAppId);
            Properties.Settings.Default.RunBatchVideoAppId = comboBoxBatchRunVideoAppId.Text;

            Properties.Settings.Default.RunBatchImageVerbList = ComboBoxStringCollection(comboBoxBatchRunImageVerb);
            Properties.Settings.Default.RunBatchImageVerb = comboBoxBatchRunImageVerb.Text;

            Properties.Settings.Default.RunBatchVideoVerbList = ComboBoxStringCollection(comboBoxBatchRunVideoVerb);
            Properties.Settings.Default.RunBatchVideoVerb = comboBoxBatchRunVideoVerb.Text;

            checkBoxBatchRunImageWaitForAppExit.Checked = Properties.Settings.Default.RunBatchImageWaitForApp;
            checkBoxBatchRunVideoWaitForAppExit.Checked = Properties.Settings.Default.RunBatchVideoWaitForApp;
            #endregion

            #region Tab - Open with
            Properties.Settings.Default.OpenWithImageProgId = GetSelectProgId(dataGridViewImages); 
            Properties.Settings.Default.OpenWithVideoProgId = GetSelectProgId(dataGridViewVideos);

            Properties.Settings.Default.OpenWithImageVerb = comboBoxBatchRunImageVerb.Text;
            Properties.Settings.Default.OpenWithVideoVerb = comboBoxBatchRunVideoVerb.Text;

            Properties.Settings.Default.OpenWithImageWaitForExit = checkBoxOpenImageWithWaitForExit.Checked;
            Properties.Settings.Default.OpenWithVideoWaitForExit = checkBoxOpenVideoWithWaitForExit.Checked;
            #endregion 

            Properties.Settings.Default.Save();
        }
        #endregion


        #region OpenWith - Common - SelectApplicationRow
        private void SelectApplicationRow(DataGridView dataGridView, string progId)
        {
            for (int rowIndex = 0; rowIndex < dataGridView.Rows.Count; rowIndex++)
            {
                if (dataGridView.Rows[rowIndex].Cells["ProgId"].Value.ToString() == progId)
                {
                    dataGridView.CurrentCell = dataGridView[1, rowIndex];
                    dataGridView.Rows[rowIndex].Selected = true;
                }
            }
        }
        #endregion

        #region OpenWith - Common - PopulateOpenWithDataGridView
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
                dataGridView.Columns["Name"].Width = 340;
                dataGridView.Columns["Command"].Visible = false;
                dataGridView.Columns["ProgId"].Visible = false;
                dataGridView.Columns["AppId"].Visible = false;
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message.ToString());
                MessageBox.Show(ex.Message.ToString());
            }
        }
        #endregion

        #region OpenWith - Common - FillComboBoxWithVerbs
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

        #region OpenWith - Common - Select Verb
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

        #region OpenWith - Common - GetSelectedRowIndex
        private int GetSelectedRowIndex(DataGridView dataGridView)
        {
            if (dataGridView.SelectedRows.Count >= 1) return dataGridView.SelectedRows[0].Index;
            if (dataGridView.SelectedCells.Count >= 1) return dataGridView.SelectedCells[0].RowIndex;
            return -1;
        }
        #endregion

        #region OpenWith - Common - Get selected ProgId

        private string GetSelectProgId(DataGridView dataGridView)
        {
            int rowIndex = GetSelectedRowIndex(dataGridView);
            if (rowIndex > -1)
            {
                return dataGridView.Rows[rowIndex].Cells["ProgId"].Value.ToString();
            }
            return "";
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

        #region OpenWith - Image - New verb selected
        private void comboBoxOpenImageWithVerbs_SelectionChangeCommitted(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewImages;            
            VerbLink verbLink = applicationAssociationsHandler.GetVerbLink(GetSelectProgId(dataGridView), comboBoxOpenImageWithVerbs.SelectedItem.ToString());
            if (verbLink != null) textBoxOpenImageWithCommand.Text = verbLink.Command;

        }
        #endregion

        #region OpenWith - Video - New verb selected
        private void comboBoxOpenVideoWithVerbs_SelectionChangeCommitted(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewVideos;            
            VerbLink verbLink = applicationAssociationsHandler.GetVerbLink(GetSelectProgId(dataGridView), comboBoxOpenVideoWithVerbs.SelectedItem.ToString());
            if (verbLink != null) textBoxOpenVideoWithCommand.Text = verbLink.Command;
        }
        #endregion

        #region OpenWith - Click
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


        #region Batch run - Show RunBatch Eaxmple when - TextChanged

        private void ShowRunBatchExample()
        {
            
            textBoxRunBatchImageExample.Text = "";

            if (tabControlBatchRunImage.SelectedTab.Tag.ToString() == "Command")
                foreach (Metadata metadata in Metadatas) textBoxRunBatchImageExample.Text += (textBoxRunBatchImageExample.Text == "" ? "" : "\r\n") + 
                        metadata.ReplaceVariables(comboBoxBatchRunImageCommand.Text, AllowedFileNameDateTimeFormats);
            else
                foreach (Metadata metadata in Metadatas) textBoxRunBatchImageExample.Text += (textBoxRunBatchImageExample.Text == "" ? "" : "\r\n") +
                        comboBoxBatchRunImageVerb.Text + " " + comboBoxBatchRunImageAppId.Text + " " + metadata.FileFullPath;

            textBoxRunBatchVideoExample.Text = "";
            if (tabControlBatchRunVideo.SelectedTab.Tag.ToString() == "Command")
                foreach (Metadata metadata in Metadatas) textBoxRunBatchVideoExample.Text += (textBoxRunBatchVideoExample.Text == "" ? "" : "\r\n") + 
                        metadata.ReplaceVariables(comboBoxBatchRunVideoCommand.Text, AllowedFileNameDateTimeFormats);
            else
                foreach (Metadata metadata in Metadatas) textBoxRunBatchVideoExample.Text += (textBoxRunBatchVideoExample.Text == "" ? "" : "\r\n") +
                        comboBoxBatchRunVideoVerb.Text + " " + comboBoxBatchRunVideoAppId.Text + " " + metadata.FileFullPath;
        }

        private void tabControlBatchRunImage_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowRunBatchExample();
        }

        private void tabControlBatchRunVideo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowRunBatchExample();
        }

        private void comboBoxBatchRunImageCommand_TextChanged(object sender, EventArgs e)
        {
            ShowRunBatchExample();
        }

        private void comboBoxBatchRunImageAppId_TextChanged(object sender, EventArgs e)
        {
            ShowRunBatchExample();
        }

        private void comboBoxBatchRunImageVerb_TextChanged(object sender, EventArgs e)
        {
            ShowRunBatchExample();
        }

        private void comboBoxBatchRunVideoCommand_TextChanged(object sender, EventArgs e)
        {
            ShowRunBatchExample();
        }

        private void comboBoxBatchRunVideoAppId_TextChanged(object sender, EventArgs e)
        {
            ShowRunBatchExample();
        }

        private void comboBoxBatchRunVideoVerb_TextChanged(object sender, EventArgs e)
        {
            ShowRunBatchExample();
        }
        #endregion

        #region Batch run - Click
        private void buttonBatchCommandBatchRun_Click(object sender, EventArgs e)
        {
            ComboBoxAddTextToList(comboBoxBatchRunImageCommand);
            ComboBoxAddTextToList(comboBoxBatchRunVideoCommand);

            ComboBoxAddTextToList(comboBoxBatchRunImageAppId);
            ComboBoxAddTextToList(comboBoxBatchRunVideoAppId);

            ComboBoxAddTextToList(comboBoxBatchRunImageVerb);
            ComboBoxAddTextToList(comboBoxBatchRunVideoVerb);

            string errors = "";
            foreach (Metadata metadata in Metadatas)
            {
                
                try
                {
                    string imageCommandWithArguments = metadata.ReplaceVariables(comboBoxBatchRunImageCommand.Text, AllowedFileNameDateTimeFormats);
                    string videoCommandWithArguments = metadata.ReplaceVariables(comboBoxBatchRunVideoCommand.Text, AllowedFileNameDateTimeFormats);

                    if (tabControlBatchRunImage.SelectedTab.Tag.ToString() == "Command")
                        ApplicationActivation.ProcessRun(imageCommandWithArguments, checkBoxBatchRunImageWaitForCommandExit.Checked);
                    else
                        ApplicationActivation.ActivateForFile(textBoxOpenImageWithAppId.Text, metadata.FileFullPath, comboBoxBatchRunImageVerb.Text, checkBoxBatchRunImageWaitForAppExit.Checked);
                    
                    if (tabControlBatchRunVideo.SelectedTab.Tag.ToString() == "Command")
                        ApplicationActivation.ProcessRun(videoCommandWithArguments, checkBoxBatchRunVideoWaitForCommandExit.Checked);
                    else
                        ApplicationActivation.ActivateForFile(textBoxOpenVideoWithAppId.Text, metadata.FileFullPath, comboBoxBatchRunImageVerb.Text, checkBoxBatchRunVideoWaitForAppExit.Checked);
                }
                catch (Exception ex)
                {
                    errors += (errors == "" ? "" : "\r\n") + "File: " + metadata.FileFullPath + "\r\nError message: " + ex.Message;
                }
            }
            if (errors != "") MessageBox.Show(errors);
        }
        #endregion

        #region Batch run - Image - Variable selected
        private void comboBoxBatchRunImageVariables_SelectionChangeCommitted(object sender, EventArgs e)
        {
            SelectionChangeCommitted(comboBoxBatchRunImageCommand, comboBoxBatchRunImageVariables.Text);
        }
        #endregion

        #region Batch run - Video - Variable selected
        private void comboBoxBatchRunVideoVariables_SelectionChangeCommitted(object sender, EventArgs e)
        {
            SelectionChangeCommitted(comboBoxBatchRunVideoCommand, comboBoxBatchRunVideoVariables.Text);
        }
        #endregion 

        #region Batch run - Image - Browser
        private void buttonBatchRunImageBrowser_Click(object sender, EventArgs e)
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
                comboBoxBatchRunImageCommand.Text = openFileDialog.FileName + " \"{FileFullPath}\"";
            }
        }
        #endregion

        #region Batch run - Video - Browser

        private void buttonBatchRunVideoBrowser_Click(object sender, EventArgs e)
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
                comboBoxBatchRunVideoCommand.Text = openFileDialog.FileName + " \"{FileFullPath}\"";
            }
        }
        #endregion

        #region Bach run - Image - Example App Selected
        private void comboBoxBatchCommandImageApp_SelectionChangeCommitted(object sender, EventArgs e)
        {
            comboBoxBatchRunImageAppId.Text = applicationDatas.Values[comboBoxBatchRunImageAppExample.SelectedIndex].ApplicationId;
        }
        #endregion 

        #region Bach run - Image - Example App Selected

        private void comboBoxBatchRunVideoApp_SelectionChangeCommitted(object sender, EventArgs e)
        {
            comboBoxBatchRunVideoAppId.Text = applicationDatas.Values[comboBoxBatchRunVideoAppExample.SelectedIndex].ApplicationId;
        }
        #endregion


        #region ArumentFile run - Click 
        private void buttonArgumentFileRun_Click(object sender, EventArgs e)
        {
            try
            {
                ComboBoxAddTextToList(comboBoxArgumentFileCommand);

                string tempArguFileFullPath = ExiftoolWriter.GetTempArguFileFullPath();
                string commandWithArguments = comboBoxArgumentFileCommand.Text.Replace("{TempFileArgumentFullPath}", tempArguFileFullPath);

                switch (tabControlArgumentFile.SelectedTab.Tag.ToString())
                {
                    case "ArgumentFile":
                        System.IO.File.WriteAllText(tempArguFileFullPath, fastColoredTextBoxArgumentFileArgumentFile.Text);
                        ApplicationActivation.ProcessRun(commandWithArguments, false);
                        break;
                    case "AutoCorrect":
                        System.IO.File.WriteAllText(tempArguFileFullPath, fastColoredTextBoxArgumentFileArgumentFileAutoCorrect.Text);
                        ApplicationActivation.ProcessRun(commandWithArguments, false);
                        break;
                    default:
                        throw new Exception("Has not been implemedted");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ArgumentFile run - Variable selected
        private void comboBoxArgumentFileCommandVariables_SelectionChangeCommitted(object sender, EventArgs e)
        {
            SelectionChangeCommitted(comboBoxArgumentFileCommand, comboBoxArgumentFileCommandVariables.Text);
        }
        #endregion

        #region ComboBox - Settings - Convert String add to List
        private void ComboBoxPopulate(ComboBox comboBox, string valueListString, string defaultValue)
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

        #region ComboBox - Settings - Convert List to string
        private string ComboBoxStringCollection(ComboBox comboBox)
        {
            string resultListString = "";
            foreach (object item in comboBox.Items)
            {
                resultListString += (resultListString == "" ? "" : "\r\n") + item.ToString();
            }
            return resultListString;
        }
        #endregion 

        #region ComboBox - Insert selected and cmomitted selection to Textbox
        private void SelectionChangeCommitted(ComboBox textBox, string insertText)
        {
            if (!isPopulation)
            {
                //textBox.Focus();
                var selectionIndex = textBox.SelectionStart;
                textBox.Text = textBox.Text.Remove(selectionIndex, textBox.SelectionLength);
                textBox.Text = textBox.Text.Insert(selectionIndex, insertText);
                textBox.SelectionStart = selectionIndex + insertText.Length;
            }
        }
        #endregion 

        #region ComboBox - Remeber last text and Add Text to list
        private void ComboBoxAddTextToList(ComboBox comboBox)
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

        #region ComboBox - Paint
        private void PaintComboBoxItem (ComboBox comboBox, DrawItemEventArgs e)
        {
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
                using (Brush backgbrush = new SolidBrush(comboBox.BackColor))
                {
                    e.Graphics.FillRectangle(backgbrush, e.Bounds);
                    foreColor = comboBox.ForeColor;
                }
            }
            using (Brush textbrush = new SolidBrush(foreColor))
            {
                e.Graphics.DrawString(comboBox.Items[e.Index].ToString(), e.Font, textbrush, e.Bounds.Height + 10, e.Bounds.Y, StringFormat.GenericTypographic);
            }

            e.Graphics.DrawImage(applicationDatas.Values[e.Index].Icon.ToBitmap(), new Rectangle(e.Bounds.Location, new Size(e.Bounds.Height - 2, e.Bounds.Height - 2)));

        }

        private void comboBoxBatchRunVideoAppExample_DrawItem(object sender, DrawItemEventArgs e)
        {
            PaintComboBoxItem ((ComboBox)sender, e);
        }

        private void comboBoxBatchCommandImageApp_DrawItem(object sender, DrawItemEventArgs e)
        {
            PaintComboBoxItem ((ComboBox)sender, e);             
        }
        #endregion

        #region ComboBox - Text Selection Hack - Remember Selection
        private void RemeberComboBoxSelection(ComboBox comboBox)
        {
            comboBox.Tag = new ComboBoxSelection(comboBox);
        }

        private void SetComboBoxSelection(ComboBox comboBox)
        {
            if (comboBox.Tag is ComboBoxSelection comboBoxSelection)
            {
                comboBox.SelectionStart = comboBoxSelection.SelectionStart;
                comboBox.SelectionLength = comboBoxSelection.SelectionLength;
            }
        }

        private void comboBox_MouseMove(object sender, MouseEventArgs e)
        {
            RemeberComboBoxSelection((ComboBox)sender);
        }

        private void comboBox_KeyUp(object sender, KeyEventArgs e)
        {
            RemeberComboBoxSelection((ComboBox)sender);
        }

        private void comboBox_Leave(object sender, EventArgs e)
        {
            SetComboBoxSelection((ComboBox)sender);            
        }


        #endregion

        #region FastColoredTextBox - Events handling
        private void fastColoredTextBoxArgumentFileArgumentFile_KeyDown(object sender, KeyEventArgs e)
        {
            if (fastColoredTextBoxHandlerRunArgumentFile != null) fastColoredTextBoxHandlerRunArgumentFile.KeyDown(sender, e);
        }

        private void fastColoredTextBoxArgumentFileArgumentFile_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            if (fastColoredTextBoxHandlerRunArgumentFile != null) fastColoredTextBoxHandlerRunArgumentFile.SyntaxHighlightProperties(sender, e);
        }

        private void fastColoredTextBoxArgumentFileArgumentFileAutoCorrect_KeyDown(object sender, KeyEventArgs e)
        {           
            if (fastColoredTextBoxHandlerRunArgumentFileAutoCorrect != null) fastColoredTextBoxHandlerRunArgumentFileAutoCorrect.KeyDown(sender, e);
        }

        private void fastColoredTextBoxArgumentFileArgumentFileAutoCorrect_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            if (fastColoredTextBoxHandlerRunArgumentFileAutoCorrect != null) fastColoredTextBoxHandlerRunArgumentFileAutoCorrect.SyntaxHighlightProperties(sender, e);
        }
        #endregion

        #region ArumentFile run - Save click
        private void buttonArgumentFileSave_Click(object sender, EventArgs e)
        {
            try {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Title = "Save aurgument text file";
                saveFileDialog1.CheckFileExists = false;
                saveFileDialog1.CheckPathExists = true;
                saveFileDialog1.DefaultExt = "txt";
                saveFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFileDialog1.FilterIndex = 2;
                saveFileDialog1.RestoreDirectory = true;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    switch (tabControlArgumentFile.SelectedTab.Tag.ToString())
                    {
                        case "ArgumentFile":                            
                            System.IO.File.WriteAllText(saveFileDialog1.FileName, fastColoredTextBoxArgumentFileArgumentFile.Text);
                            break;
                    case "AutoCorrect":
                            System.IO.File.WriteAllText(saveFileDialog1.FileName, fastColoredTextBoxArgumentFileArgumentFileAutoCorrect.Text);
                            break;
                        default:
                            throw new Exception("Has not been implemedted");
                    }
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion clic

        #region ArumentFile run - Load click
        private void buttonArgumentFileLoad_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog
                {
                    InitialDirectory = @"D:\",
                    Title = "Browse Text Files",

                    CheckFileExists = true,
                    CheckPathExists = true,

                    DefaultExt = "txt",
                    Filter = "txt files (*.txt)|*.txt",
                    FilterIndex = 2,
                    RestoreDirectory = true,

                    ReadOnlyChecked = true,
                    ShowReadOnly = true
                };

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                   
                    switch (tabControlArgumentFile.SelectedTab.Tag.ToString())
                    {
                        case "ArgumentFile":
                            fastColoredTextBoxArgumentFileArgumentFile.Text = System.IO.File.ReadAllText(openFileDialog1.FileName);
                            break;
                        case "AutoCorrect":
                            fastColoredTextBoxArgumentFileArgumentFileAutoCorrect.Text = System.IO.File.ReadAllText(openFileDialog1.FileName);
                            break;
                        default:
                            throw new Exception("Has not been implemedted");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ArumentFile run - Compare click
        private void buttonArgumentFileCompare_Click(object sender, EventArgs e)
        {
            FormCompareText formCompareText = new FormCompareText();
            formCompareText.Compare(fastColoredTextBoxArgumentFileArgumentFile.Text, fastColoredTextBoxArgumentFileArgumentFileAutoCorrect.Text);
            formCompareText.ShowDialog();
        }
        #endregion 
    }

    #region ComboBox - Text Selction Hack - Remember ComboBoxSelection
    public class ComboBoxSelection
    {
        public int SelectionStart { get; set; }
        public int SelectionLength { get; set; }

        public ComboBoxSelection (ComboBox comboBox)
        {
            SelectionStart = comboBox.SelectionStart;
            SelectionLength = comboBox.SelectionLength;
        }
    }
    #endregion 

}
