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
using FileHandeling;
using PhotoTagsCommonComponets;
using Krypton.Toolkit;

namespace PhotoTagsSynchronizer
{
    public partial class FormRunCommand : KryptonForm
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public string ArguFile { get; set; } = "";
        public string ArguFileAutoCorrect { get; set; } = "";

        private bool isPopulation = false;
        public MetadataReadPrioity MetadataPrioity { get; set; }
        public List<Metadata> MetadatasGridView { get; set; } = new List<Metadata>();
        public List<Metadata> MetadatasOriginal { get; set; } = new List<Metadata>();
        public List<Metadata> MetadatasEmpty { get; set; } = new List<Metadata>();

        public List<string> AllowedFileNameDateTimeFormats { get; set; } = new List<string>();

        private FastColoredTextBoxHandler fastColoredTextBoxHandlerRunArgumentFile = null;
        private FastColoredTextBoxHandler fastColoredTextBoxHandlerRunArgumentFileAutoCorrect = null;

        private FastColoredTextBoxHandler fastColoredTextBoxHandlerKeywordAdd = null;
        private FastColoredTextBoxHandler fastColoredTextBoxHandlerKeywordWriteTags = null;
        private FastColoredTextBoxHandler fastColoredTextBoxHandlerBuildResult = null;

        private ApplicationAssociationsHandler applicationAssociationsHandler = new ApplicationAssociationsHandler();
        private SortedList<string, ApplicationData> applicationDatas;

        public FormRunCommand()
        {
            InitializeComponent();

        }

        #region Common

        #region Common - Init - Load Config
        public void Init()
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action(Init));
                return;
            }

            isPopulation = true;
            #region Tab - Run batch - Command
            comboBoxBatchRunImageVariables.Items.AddRange(Metadata.ListOfProperties(false));
            comboBoxBatchRunVideoVariables.Items.AddRange(Metadata.ListOfProperties(false));
            ComboBoxHandler.ComboBoxPopulateClear(comboBoxBatchRunImageCommand, Properties.Settings.Default.RunBatchImageCommandList, Properties.Settings.Default.RunBatchImageCommand);
            ComboBoxHandler.ComboBoxPopulateClear(comboBoxBatchRunVideoCommand, Properties.Settings.Default.RunBatchVideoCommandList, Properties.Settings.Default.RunBatchVideoCommand);
            checkBoxBatchRunImageWaitForCommandExit.Checked = Properties.Settings.Default.RunBatchImageWaitForCommand;
            checkBoxBatchRunVideoWaitForCommandExit.Checked = Properties.Settings.Default.RunBatchVideoWaitForCommand;
            #endregion

            #region Tab - Run batch - App
            foreach (Metadata metadata in MetadatasGridView) textBoxBatchCommandSelectedFiles.Text += (textBoxBatchCommandSelectedFiles.Text == "" ? "" : "\r\n") + metadata.FileFullPath;

            applicationDatas = ApplicationInstalled.ListInstalledApps();
            for (int index = 0; index < applicationDatas.Count; index++)
            {
                comboBoxBatchRunImageAppExample.Items.Add(applicationDatas.Values[index].FriendlyAppName);
                comboBoxBatchRunVideoAppExample.Items.Add(applicationDatas.Values[index].FriendlyAppName);
            }
            ComboBoxHandler.ComboBoxPopulateClear(comboBoxBatchRunImageAppId, Properties.Settings.Default.RunBatchImageAppIdList, Properties.Settings.Default.RunBatchImageAppId);
            ComboBoxHandler.ComboBoxPopulateClear(comboBoxBatchRunVideoAppId, Properties.Settings.Default.RunBatchVideoAppIdList, Properties.Settings.Default.RunBatchVideoAppId);
            ComboBoxHandler.ComboBoxPopulateClear(comboBoxBatchRunImageVerb, Properties.Settings.Default.RunBatchImageVerbList, Properties.Settings.Default.RunBatchImageVerb);
            ComboBoxHandler.ComboBoxPopulateClear(comboBoxBatchRunVideoVerb, Properties.Settings.Default.RunBatchVideoVerbList, Properties.Settings.Default.RunBatchVideoVerb);
            checkBoxBatchRunImageWaitForAppExit.Checked = Properties.Settings.Default.RunBatchImageWaitForApp;
            checkBoxBatchRunVideoWaitForAppExit.Checked = Properties.Settings.Default.RunBatchVideoWaitForApp;
            checkBoxRunBatchRedirectToTerminalWindows.Checked = Properties.Settings.Default.RunBatchInTerminalWindow;

            #endregion

            #region Tab - Open with
            foreach (Metadata metadata in MetadatasGridView) textBoxOpenWithSelectedFiles.Text += (textBoxOpenWithSelectedFiles.Text == "" ? "" : "\r\n") + metadata.FileFullPath;
            //DataGridView video Open With...
            List<string> videoExtensions = ImageAndMovieFileExtentionsUtility.GetVideoExtension(MetadatasGridView);
            List<string> imageExtensions = ImageAndMovieFileExtentionsUtility.GetImageExtension(MetadatasGridView);

            PopulateOpenWithDataGridView(dataGridViewImages, imageExtensions);
            PopulateOpenWithDataGridView(dataGridViewVideos, videoExtensions);

            SelectApplicationRow(dataGridViewImages, Properties.Settings.Default.OpenWithImageProgId);
            SelectApplicationRow(dataGridViewVideos, Properties.Settings.Default.OpenWithVideoProgId);

            comboBoxBatchRunImageVerb.Text = Properties.Settings.Default.OpenWithImageVerb;
            comboBoxBatchRunVideoVerb.Text = Properties.Settings.Default.OpenWithVideoVerb;

            checkBoxOpenImageWithWaitForExit.Checked = Properties.Settings.Default.OpenWithImageWaitForExit;
            checkBoxOpenVideoWithWaitForExit.Checked = Properties.Settings.Default.OpenWithVideoWaitForExit;
            #endregion

            #region Tab - Argument file
            fastColoredTextBoxHandlerRunArgumentFile = new FastColoredTextBoxHandler(fastColoredTextBoxArgumentFileArgumentFile, false, MetadataPrioity.MetadataPrioityDictionary);
            fastColoredTextBoxHandlerRunArgumentFileAutoCorrect = new FastColoredTextBoxHandler(fastColoredTextBoxArgumentFileArgumentFileAutoCorrect, false, MetadataPrioity.MetadataPrioityDictionary);

            comboBoxArgumentFileCommandVariables.Items.Add("{TempFileArgumentFullPath}");
            ComboBoxHandler.ComboBoxPopulateClear(comboBoxArgumentFileCommand, Properties.Settings.Default.RunArgumentCommandList, Properties.Settings.Default.RunArgumentCommand);
            fastColoredTextBoxArgumentFileArgumentFile.Text = ArguFile;
            fastColoredTextBoxArgumentFileArgumentFileAutoCorrect.Text = ArguFileAutoCorrect;

            #endregion

            #region Tab - Build
            fastColoredTextBoxHandlerKeywordAdd = new FastColoredTextBoxHandler(fastColoredTextBoxMetadataWriteKeywordAdd, true, MetadataPrioity.MetadataPrioityDictionary);
            fastColoredTextBoxHandlerKeywordWriteTags = new FastColoredTextBoxHandler(fastColoredTextBoxMetadataWriteTags, false, MetadataPrioity.MetadataPrioityDictionary);
            fastColoredTextBoxHandlerBuildResult = new FastColoredTextBoxHandler(fastColoredTextBoxBuildResult, false, MetadataPrioity.MetadataPrioityDictionary);

            comboBoxArgumentFileBuilderCommandVariables.Items.Add("{TempFileArgumentFullPath}");
            ComboBoxHandler.ComboBoxPopulateClear(comboBoxArgumentFileBuilderCommand, Properties.Settings.Default.RunArgumentBuildCommandList, Properties.Settings.Default.RunArgumentBuildCommand);

            comboBoxMetadataWriteStandardTags.Items.AddRange(Metadata.ListOfProperties(false));
            comboBoxMetadataWriteKeywordAdd.Items.AddRange(Metadata.ListOfProperties(true));

            fastColoredTextBoxMetadataWriteKeywordAdd.Text = Properties.Settings.Default.RunArgumentBuildKeywordAdd;
            fastColoredTextBoxMetadataWriteTags.Text = Properties.Settings.Default.RunArgumentBuildKeywordTags;
            //fastColoredTextBoxBuildResult.Text = ;
            UpdateBuildResult();
            #endregion

            isPopulation = false;

        }
        #endregion 

        #region Common - FormClosing - Save Config
        private void RunCommand_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                #region Tab - Argument file
                Properties.Settings.Default.RunArgumentCommandList = ComboBoxHandler.ComboBoxStringCollection(comboBoxArgumentFileCommand);
                Properties.Settings.Default.RunArgumentCommand = comboBoxArgumentFileCommand.Text;
                #endregion

                #region Tab - Run batch - Command
                Properties.Settings.Default.RunBatchImageCommandList = ComboBoxHandler.ComboBoxStringCollection(comboBoxBatchRunImageCommand);
                Properties.Settings.Default.RunBatchImageCommand = comboBoxBatchRunImageCommand.Text;

                Properties.Settings.Default.RunBatchVideoCommandList = ComboBoxHandler.ComboBoxStringCollection(comboBoxBatchRunVideoCommand);
                Properties.Settings.Default.RunBatchVideoCommand = comboBoxBatchRunVideoCommand.Text;


                Properties.Settings.Default.RunBatchImageWaitForCommand = checkBoxBatchRunImageWaitForCommandExit.Checked;
                Properties.Settings.Default.RunBatchVideoWaitForCommand = checkBoxBatchRunVideoWaitForCommandExit.Checked;

                Properties.Settings.Default.RunBatchInTerminalWindow = checkBoxRunBatchRedirectToTerminalWindows.Checked;
                #endregion

                #region Tab - Run batch - App

                Properties.Settings.Default.RunBatchImageAppIdList = ComboBoxHandler.ComboBoxStringCollection(comboBoxBatchRunImageAppId);
                Properties.Settings.Default.RunBatchImageAppId = comboBoxBatchRunImageAppId.Text;

                Properties.Settings.Default.RunBatchVideoAppIdList = ComboBoxHandler.ComboBoxStringCollection(comboBoxBatchRunVideoAppId);
                Properties.Settings.Default.RunBatchVideoAppId = comboBoxBatchRunVideoAppId.Text;

                Properties.Settings.Default.RunBatchImageVerbList = ComboBoxHandler.ComboBoxStringCollection(comboBoxBatchRunImageVerb);
                Properties.Settings.Default.RunBatchImageVerb = comboBoxBatchRunImageVerb.Text;

                Properties.Settings.Default.RunBatchVideoVerbList = ComboBoxHandler.ComboBoxStringCollection(comboBoxBatchRunVideoVerb);
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

                #region Tab - Builder
                Properties.Settings.Default.RunArgumentBuildCommandList = ComboBoxHandler.ComboBoxStringCollection(comboBoxArgumentFileBuilderCommand);
                Properties.Settings.Default.RunArgumentBuildCommand = comboBoxArgumentFileBuilderCommand.Text;

                Properties.Settings.Default.RunArgumentBuildKeywordAdd = fastColoredTextBoxMetadataWriteKeywordAdd.Text;
                Properties.Settings.Default.RunArgumentBuildKeywordTags = fastColoredTextBoxMetadataWriteTags.Text;
                #endregion

                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show("Can't save the settings.\r\n\r\n" + ex.Message, "Save settings failed...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ComboBox - Insert selected and cmomitted selection to Textbox
        private void SelectionChangeCommitted(KryptonComboBox textBox, string insertText)
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

        #region ComboBox - Paint
        private void PaintComboBoxItem(ComboBox comboBox, DrawItemEventArgs e)
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
            PaintComboBoxItem((ComboBox)sender, e);
        }

        private void comboBoxBatchCommandImageApp_DrawItem(object sender, DrawItemEventArgs e)
        {
            PaintComboBoxItem((ComboBox)sender, e);
        }
        #endregion

        #region ComboBox - Text Selection Hack - Remember Selection

        private void comboBox_MouseMove(object sender, MouseEventArgs e)
        {
            ComboBoxHandler.RemeberComboBoxSelection((KryptonComboBox)sender);
        }

        private void comboBox_KeyUp(object sender, KeyEventArgs e)
        {
            ComboBoxHandler.RemeberComboBoxSelection((KryptonComboBox)sender);
        }

        private void comboBox_Leave(object sender, EventArgs e)
        {
            ComboBoxHandler.SetComboBoxSelection((KryptonComboBox)sender);
        }
        #endregion

        #endregion

        #region OpenWith

        #region OpenWith - Common - SelectApplicationRow
        private void SelectApplicationRow(DataGridView dataGridView, string progId)
        {
            for (int rowIndex = 0; rowIndex < dataGridView.Rows.Count; rowIndex++)
            {
                if (ConvertValueNullToBlank(dataGridView.Rows[rowIndex].Cells["ProgId"].Value) == progId)
                {
                    dataGridView.CurrentCell = dataGridView[1, rowIndex];
                    dataGridView.Rows[rowIndex].Selected = true;
                }
            }
        }
        #endregion

        #region OpenWith - Common - PopulateOpenWithDataGridView
        private bool isPopulateOpenWithDataGridView = false;
        private void PopulateOpenWithDataGridView(DataGridView dataGridView, List<string> extensions)
        {
            isPopulateOpenWithDataGridView = true;
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

                dataGridView.AllowUserToAddRows = false;
                dataGridView.Columns.Clear();
                //for (int columnIndex = 0; columnIndex < dtApps.Columns.Count; columnIndex++) dataGridView.Columns.Add(dtApps.Columns[columnIndex].ColumnName, dtApps.Columns[columnIndex].ColumnName);                
                dataGridView.DataSource = dtApps;
                dataGridView.RowHeadersWidth = 10;
                dataGridView.Columns["Icon"].Width = 50;
                dataGridView.Columns["Name"].Width = 200;
                dataGridView.Columns["Command"].Visible = false;
                dataGridView.Columns["ProgId"].Visible = false;
                dataGridView.Columns["AppId"].Visible = false;
                dataGridView.RowHeadersWidth = 10;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Populate OpenWith failed...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            isPopulateOpenWithDataGridView = false;
        }
        #endregion

        #region OpenWith - Common - FillComboBoxWithVerbs
        private void FillComboBoxWithVerbs(KryptonComboBox comboBox, string progId)
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
        private void SelectVerb(KryptonComboBox comboBox, string verb)
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
            if (isPopulateOpenWithDataGridView) return "";
            int rowIndex = GetSelectedRowIndex(dataGridView);
            if (rowIndex > -1)
            {
                return ConvertValueNullToBlank(dataGridView.Rows[rowIndex].Cells["ProgId"].Value);
            }
            return "";
        }
        #endregion 

        #region OpenWith - CellEnter - Select row
        private void dataGridViewImages_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (isPopulateOpenWithDataGridView) return;
            DataGridView dataGridView = dataGridViewImages;
            dataGridView.Rows[e.RowIndex].Selected = true;
        }

        private void dataGridViewVideos_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (isPopulateOpenWithDataGridView) return;
            DataGridView dataGridView = dataGridViewVideos;
            dataGridView.Rows[e.RowIndex].Selected = true;
        }
        #endregion 

        #region OpenWith - Image SelectionChanged
        private string ConvertValueNullToBlank(object value)
        {
            return (value == null ? "" : value.ToString());
        }
        private void dataGridViewImages_SelectionChanged(object sender, EventArgs e)
        {
            if (isPopulateOpenWithDataGridView) return;
            DataGridView dataGridView = dataGridViewImages;

            if (dataGridView.Columns.Count > 0 && dataGridView.Rows.Count > 0 && dataGridView.SelectedRows.Count == 1)
            {
                textBoxOpenImageWithApplication.Text = ConvertValueNullToBlank(dataGridView.Rows[dataGridView.SelectedRows[0].Index].Cells["Name"].Value);
                textBoxOpenImageWithCommand.Text = ConvertValueNullToBlank(dataGridView.Rows[dataGridView.SelectedRows[0].Index].Cells["Command"].Value);
                textBoxOpenImageWithAppId.Text = ConvertValueNullToBlank(dataGridView.Rows[dataGridView.SelectedRows[0].Index].Cells["AppId"].Value);

                KryptonComboBox comboBox = comboBoxOpenImageWithVerbs;
                FillComboBoxWithVerbs(comboBox, ConvertValueNullToBlank(dataGridView.Rows[dataGridView.SelectedRows[0].Index].Cells["ProgId"].Value));
                SelectVerb(comboBox, "Open");
                if (string.IsNullOrWhiteSpace(comboBox.Text)) SelectVerb(comboBox, "Edit");
                if (string.IsNullOrWhiteSpace(comboBox.Text)) SelectVerb(comboBox, "Play");
            }
        }
        #endregion

        #region OpenWith - Video SelectionChanged
        private void dataGridViewVideos_SelectionChanged(object sender, EventArgs e)
        {
            if (isPopulateOpenWithDataGridView) return;
            DataGridView dataGridView = dataGridViewVideos;

            if (dataGridView.Columns.Count > 0 && dataGridView.Rows.Count > 0 && dataGridView.SelectedRows.Count == 1)
            {
                textBoxOpenVideoWithApplication.Text = ConvertValueNullToBlank(dataGridView.Rows[dataGridView.SelectedRows[0].Index].Cells["Name"].Value);
                textBoxOpenVideoWithCommand.Text = ConvertValueNullToBlank(dataGridView.Rows[dataGridView.SelectedRows[0].Index].Cells["Command"].Value);
                textBoxOpenVideoWithAppId.Text = ConvertValueNullToBlank(dataGridView.Rows[dataGridView.SelectedRows[0].Index].Cells["AppId"].Value);
                FillComboBoxWithVerbs(comboBoxOpenVideoWithVerbs, ConvertValueNullToBlank(dataGridView.Rows[dataGridView.SelectedRows[0].Index].Cells["ProgId"].Value));
                KryptonComboBox comboBox = comboBoxOpenVideoWithVerbs;
                FillComboBoxWithVerbs(comboBox, ConvertValueNullToBlank(dataGridView.Rows[dataGridView.SelectedRows[0].Index].Cells["ProgId"].Value));
                SelectVerb(comboBox, "Open");
                if (string.IsNullOrWhiteSpace(comboBox.Text)) SelectVerb(comboBox, "Edit");
                if (string.IsNullOrWhiteSpace(comboBox.Text)) SelectVerb(comboBox, "Play");
            }
        }
        #endregion

        #region OpenWith - Image - New verb selected
        #endregion

        #region OpenWith - Video - New verb selected
        #endregion

        #region OpenWith - Click
        private void buttonOpenWithOpenWith_Click(object sender, EventArgs e)
        {
            string errors = "";
            foreach (Metadata metadata in MetadatasGridView)
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
            if (errors != "") KryptonMessageBox.Show(errors, "Was not able to open files...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true); 
        }
        #endregion

        #endregion

        #region Batch run

        #region Batch run - Show RunBatch Eaxmple when - TextChanged

        private void ShowRunBatchExample()
        {

            textBoxRunBatchImageExample.Text = "";

            if (kryptonWorkspaceCellRunBatchImage.SelectedPage.Tag.ToString() == "Command")
                foreach (Metadata metadata in MetadatasGridView) textBoxRunBatchImageExample.Text += (textBoxRunBatchImageExample.Text == "" ? "" : "\r\n") +
                        metadata.ReplaceVariablesWrittenByUser(comboBoxBatchRunImageCommand.Text, AllowedFileNameDateTimeFormats);
            else
                foreach (Metadata metadata in MetadatasGridView) textBoxRunBatchImageExample.Text += (textBoxRunBatchImageExample.Text == "" ? "" : "\r\n") +
                        comboBoxBatchRunImageVerb.Text + " " + comboBoxBatchRunImageAppId.Text + " " + metadata.FileFullPath;

            textBoxRunBatchVideoExample.Text = "";
            if (kryptonWorkspaceCellRunBatchVideo.SelectedPage.Tag.ToString() == "Command")
                foreach (Metadata metadata in MetadatasGridView) textBoxRunBatchVideoExample.Text += (textBoxRunBatchVideoExample.Text == "" ? "" : "\r\n") +
                        metadata.ReplaceVariablesWrittenByUser(comboBoxBatchRunVideoCommand.Text, AllowedFileNameDateTimeFormats);
            else
                foreach (Metadata metadata in MetadatasGridView) textBoxRunBatchVideoExample.Text += (textBoxRunBatchVideoExample.Text == "" ? "" : "\r\n") +
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
            ComboBoxHandler.ComboBoxAddLastTextFirstInList(comboBoxBatchRunImageCommand);
            ComboBoxHandler.ComboBoxAddLastTextFirstInList(comboBoxBatchRunVideoCommand);

            ComboBoxHandler.ComboBoxAddLastTextFirstInList(comboBoxBatchRunImageAppId);
            ComboBoxHandler.ComboBoxAddLastTextFirstInList(comboBoxBatchRunVideoAppId);

            ComboBoxHandler.ComboBoxAddLastTextFirstInList(comboBoxBatchRunImageVerb);
            ComboBoxHandler.ComboBoxAddLastTextFirstInList(comboBoxBatchRunVideoVerb);

            PhotoTagsCommonComponets.FormTerminalWindow formTerminalWindow = null;
            if (checkBoxRunBatchRedirectToTerminalWindows.Checked)
            {
                formTerminalWindow = new PhotoTagsCommonComponets.FormTerminalWindow();
                formTerminalWindow.Show();
            }


            string errors = "";
            foreach (Metadata metadata in MetadatasGridView)
            {

                try
                {
                    string imageCommandWithArguments = metadata.ReplaceVariablesWrittenByUser(comboBoxBatchRunImageCommand.Text, AllowedFileNameDateTimeFormats);
                    string videoCommandWithArguments = metadata.ReplaceVariablesWrittenByUser(comboBoxBatchRunVideoCommand.Text, AllowedFileNameDateTimeFormats);

                    if (ImageAndMovieFileExtentions.ImageAndMovieFileExtentionsUtility.IsImageFormat(metadata.FileFullPath))
                    {
                        if (kryptonWorkspaceCellRunBatchImage.SelectedPage.Tag.ToString() == "Command")
                        {

                            if (checkBoxRunBatchRedirectToTerminalWindows.Checked)
                                ApplicationActivation.ProcessRun(formTerminalWindow, imageCommandWithArguments, checkBoxBatchRunImageWaitForCommandExit.Checked);
                            else
                                ApplicationActivation.ProcessRun(imageCommandWithArguments, checkBoxBatchRunImageWaitForCommandExit.Checked);
                        }
                        else ApplicationActivation.ActivateForFile(textBoxOpenImageWithAppId.Text, metadata.FileFullPath, comboBoxBatchRunImageVerb.Text, checkBoxBatchRunImageWaitForAppExit.Checked);
                    }

                    if (ImageAndMovieFileExtentions.ImageAndMovieFileExtentionsUtility.IsVideoFormat(metadata.FileFullPath))
                    {

                        if (kryptonWorkspaceCellRunBatchVideo.SelectedPage.Tag.ToString() == "Command")
                        {
                            if (checkBoxRunBatchRedirectToTerminalWindows.Checked)
                                ApplicationActivation.ProcessRun(formTerminalWindow, videoCommandWithArguments, checkBoxBatchRunVideoWaitForCommandExit.Checked);
                            else
                                ApplicationActivation.ProcessRun(videoCommandWithArguments, checkBoxBatchRunVideoWaitForCommandExit.Checked);
                        }
                        else ApplicationActivation.ActivateForFile(textBoxOpenVideoWithAppId.Text, metadata.FileFullPath, comboBoxBatchRunImageVerb.Text, checkBoxBatchRunVideoWaitForAppExit.Checked);
                    }
                }
                catch (Exception ex)
                {
                    errors += (errors == "" ? "" : "\r\n") + "File: " + metadata.FileFullPath + "\r\nError message: " + ex.Message;
                }
                if (formTerminalWindow != null && formTerminalWindow.GetWasProcessKilled()) break;
            }
            if (errors != "") KryptonMessageBox.Show(errors, "Was not able to open files...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);

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

        #region Batch run - Image - Browse
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

        #region Batch run - Video - Browse

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

        #endregion

        #region Argument run

        #region ArumentFile run - Click 
        private void buttonArgumentFileRun_Click(object sender, EventArgs e)
        {
            try
            {
                ComboBoxHandler.ComboBoxAddLastTextFirstInList(comboBoxArgumentFileCommand);

                string tempArguFileFullPath = FileHandler.GetLocalApplicationDataPath("exiftool_arg_run.txt", true, this);
                string commandWithArguments = comboBoxArgumentFileCommand.Text.Replace("{TempFileArgumentFullPath}", tempArguFileFullPath);

                switch (kryptonWorkspaceCellRunArgumentFileAutoCorrect.SelectedPage.Tag.ToString())
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
                KryptonMessageBox.Show(ex.Message, "Was not able to run files...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ArgumentFile run - Variable selected
        private void comboBoxArgumentFileCommandVariables_SelectionChangeCommitted(object sender, EventArgs e)
        {
            SelectionChangeCommitted(comboBoxArgumentFileCommand, comboBoxArgumentFileCommandVariables.Text);            
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
                    switch (kryptonWorkspaceCellRunArgumentFileAutoCorrect.Tag.ToString())
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
                KryptonMessageBox.Show(ex.Message, "Was not able to save file...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                   
                    switch (kryptonWorkspaceCellRunArgumentFileAutoCorrect.Tag.ToString())
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
                KryptonMessageBox.Show(ex.Message, "Was not able to load file...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ArumentFile run - Compare click
        private void buttonArgumentFileCompare_Click(object sender, EventArgs e)
        {
            FormCompareText formCompareText = new FormCompareText();
            formCompareText.Compare("Source: Edit by user", "Source: Created by AutoCorrect", fastColoredTextBoxArgumentFileArgumentFile.Text, fastColoredTextBoxArgumentFileArgumentFileAutoCorrect.Text);
            formCompareText.ShowDialog();
        }
        #endregion

        #region Argument file tab - FastColoredTextBox - Events handling
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
        
        #endregion

        #region Build tab 

        #region Build tab - FastColoredTextBox - Events handling
        private void fastColoredTextBoxMetadataWriteKeywordAdd_KeyDown(object sender, KeyEventArgs e)
        {
            if (fastColoredTextBoxHandlerKeywordAdd != null) fastColoredTextBoxHandlerKeywordAdd.KeyDown(sender, e);
        }

        private void fastColoredTextBoxMetadataWriteKeywordAdd_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            if (fastColoredTextBoxHandlerKeywordAdd != null) fastColoredTextBoxHandlerKeywordAdd.SyntaxHighlightProperties(sender, e);
            
        }

        private void fastColoredTextBoxMetadataWriteTags_KeyDown(object sender, KeyEventArgs e)
        {
            if (fastColoredTextBoxHandlerKeywordWriteTags != null) fastColoredTextBoxHandlerKeywordWriteTags.KeyDown(sender, e);
        }
        
        private void fastColoredTextBoxMetadataWriteTags_TextChanging(object sender, FastColoredTextBoxNS.TextChangingEventArgs e)
        {
        }
        private void fastColoredTextBoxMetadataWriteTags_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            if (fastColoredTextBoxHandlerKeywordWriteTags != null) fastColoredTextBoxHandlerKeywordWriteTags.SyntaxHighlightProperties(sender, e);
        }


        private void fastColoredTextBoxBuildResult_KeyDown(object sender, KeyEventArgs e)
        {
            if (fastColoredTextBoxHandlerBuildResult != null) fastColoredTextBoxHandlerBuildResult.KeyDown(sender, e);
        }

        private void fastColoredTextBoxBuildResult_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            if (fastColoredTextBoxHandlerBuildResult != null) fastColoredTextBoxHandlerBuildResult.SyntaxHighlightProperties(sender, e);
        }
        #endregion

        #region Build tab - ComboBox Handler 
        private void comboBoxArgumentFileBuilderCommandVariables_SelectionChangeCommitted(object sender, EventArgs e)
        {
            SelectionChangeCommitted(comboBoxArgumentFileBuilderCommand, comboBoxArgumentFileBuilderCommandVariables.Text);
        }

        private void comboBoxMetadataWriteKeywordAdd_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (!isPopulation) ComboBoxHandler.SelectionChangeCommitted(fastColoredTextBoxMetadataWriteKeywordAdd, comboBoxMetadataWriteKeywordAdd.Text);
        }

        private void comboBoxMetadataWriteStandardTags_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (!isPopulation) ComboBoxHandler.SelectionChangeCommitted(fastColoredTextBoxMetadataWriteTags, comboBoxMetadataWriteStandardTags.Text);
        }
        #endregion

        #region Build tab - Input Add, Delete, Tags - Load and Save click
        
        private void buttonMetadataWriteKeywordAddLoad_Click(object sender, EventArgs e)
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

                if (openFileDialog1.ShowDialog() == DialogResult.OK) fastColoredTextBoxMetadataWriteKeywordAdd.Text = System.IO.File.ReadAllText(openFileDialog1.FileName);
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show(ex.Message, "Was not able to read file...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }

        private void buttonMetadataWriteKeywordAddSave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Title = "Save aurgument text file";
                saveFileDialog1.CheckFileExists = false;
                saveFileDialog1.CheckPathExists = true;
                saveFileDialog1.DefaultExt = "txt";
                saveFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFileDialog1.FilterIndex = 2;
                saveFileDialog1.RestoreDirectory = true;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK) System.IO.File.WriteAllText(saveFileDialog1.FileName, fastColoredTextBoxMetadataWriteKeywordAdd.Text);
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show(ex.Message, "Was not able to save the file...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }

        private void buttonMetadataWriteKeywordsLoad_Click(object sender, EventArgs e)
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

                if (openFileDialog1.ShowDialog() == DialogResult.OK) fastColoredTextBoxMetadataWriteTags.Text = System.IO.File.ReadAllText(openFileDialog1.FileName);
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show(ex.Message, "Was not able to read the file...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }

        private void buttonMetadataWriteKeywordsSave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Title = "Save aurgument text file";
                saveFileDialog1.CheckFileExists = false;
                saveFileDialog1.CheckPathExists = true;
                saveFileDialog1.DefaultExt = "txt";
                saveFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFileDialog1.FilterIndex = 2;
                saveFileDialog1.RestoreDirectory = true;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK) System.IO.File.WriteAllText(saveFileDialog1.FileName, fastColoredTextBoxMetadataWriteTags.Text);
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show(ex.Message, "Was not able to read the file...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region Build tab - Result - Save click
        private void buttonArgumentFileBuilderSave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Title = "Save aurgument text file";
                saveFileDialog1.CheckFileExists = false;
                saveFileDialog1.CheckPathExists = true;
                saveFileDialog1.DefaultExt = "txt";
                saveFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFileDialog1.FilterIndex = 2;
                saveFileDialog1.RestoreDirectory = true;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK) System.IO.File.WriteAllText(saveFileDialog1.FileName, fastColoredTextBoxBuildResult.Text);
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show(ex.Message, "Was not able to write to the file...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion 

        #region Build tab - Run click
        private void buttonArgumentFileBuilderRun_Click(object sender, EventArgs e)
        {
            try
            {
                ComboBoxHandler.ComboBoxAddLastTextFirstInList(comboBoxArgumentFileBuilderCommand);

                string tempArguFileFullPath = FileHandler.GetLocalApplicationDataPath("exiftool_arg_run.txt", true, this);
                string commandWithArguments = comboBoxArgumentFileBuilderCommand.Text.Replace("{TempFileArgumentFullPath}", tempArguFileFullPath);

                System.IO.File.WriteAllText(tempArguFileFullPath, fastColoredTextBoxBuildResult.Text);
                ApplicationActivation.ProcessRun(commandWithArguments, false);
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show(ex.Message, "Was not able to write to the file...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region Build tab - Updated Build result
        private void UpdateBuildResult()
        {
            ExiftoolWriter.CreateExiftoolArguFileText(
                    MetadatasOriginal, MetadatasEmpty,
                    AllowedFileNameDateTimeFormats,
                    fastColoredTextBoxMetadataWriteTags.Text, fastColoredTextBoxMetadataWriteKeywordAdd.Text,
                    true, out string exiftoolFileTextGridView);

            fastColoredTextBoxBuildResult.Text = exiftoolFileTextGridView;
        }
        private void buttonArgumentFileBuilderBuild_Click(object sender, EventArgs e)
        {
            UpdateBuildResult();
        }
        #endregion

        #region Build tab - Comapre click
        private void buttonArgumentFileBuilderCompare_Click(object sender, EventArgs e)
        {
            ExiftoolWriter.CreateExiftoolArguFileText(
                    MetadatasGridView, MetadatasEmpty,
                    AllowedFileNameDateTimeFormats,
                    fastColoredTextBoxMetadataWriteTags.Text, fastColoredTextBoxMetadataWriteKeywordAdd.Text,
                    true, out string exiftoolFileTextOriginal);

            FormCompareText formCompareText = new FormCompareText();
            formCompareText.Compare("Source: Original from media files", "Source: Edit by users", exiftoolFileTextOriginal, fastColoredTextBoxBuildResult.Text);
            formCompareText.ShowDialog();            
        }

        #endregion

        #endregion
    }
}
