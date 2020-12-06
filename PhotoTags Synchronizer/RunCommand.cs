using Exiftool;
using MetadataLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhotoTagsSynchronizer
{
    public partial class RunCommand : Form
    {
        public string ArguFile { get; set; } = "";
        public List<Metadata> Metadatas { get; set; } = new List<Metadata>();
        public List<string> AllowedFileNameDateTimeFormats { get; set; } = new List<string>();
        private bool isPopulation = false;

        private ProcessStart.OpenWith openW;

        public RunCommand()
        {
            InitializeComponent();
        }

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
                textBoxOpenWithSelectedFiles.Text += (textBoxOpenWithSelectedFiles.Text == "" ? "" : "\r\n")  + metadata.FileFullPath;
                textBoxBatchCommandSelectedFiles.Text += (textBoxBatchCommandSelectedFiles.Text == "" ? "" : "\r\n") + metadata.FileFullPath;
            }

            //Argu file
            textBoxArgumentFileArgumentFile.Text = ArguFile;
            isPopulation = false;

            //Commands
            textBoxArgumentFileCommand.Text = Properties.Settings.Default.RunArgumentCommand;
            textBoxBatchCommandCommand.Text = Properties.Settings.Default.RunBatchCommand;
            string s1 = Properties.Settings.Default.RunOpenPictureWith;
            string s2 = Properties.Settings.Default.RunOpenVideoWith;


            //DataGridView
            Pupulate(dataGridViewPictures, ".jpg");
            Pupulate(dataGridViewVideos, ".mp4");
        }

        private void Pupulate(DataGridView dataGridView, string strExtension)
        {
            try
            {
                openW = new ProcessStart.OpenWith(strExtension);
                if (openW != null)
                {
                    DataTable dtApps = new DataTable("Application");
                    dtApps.Columns.Add("Icon", typeof(Icon));
                    dtApps.Columns.Add("Name");
                    dtApps.Columns.Add("Path");

                    foreach (ProcessStart.cApplicationData data in openW.Applicationlist.Values)
                    {
                        if (data.Havefilelinks) dtApps.Rows.Add(new object[] { data.ApplicationIcon, data.Productname, data.Filenamelink });                        
                    }

                    dataGridView.DataSource = dtApps;
                    dataGridView.Columns["Icon"].Width = 50;
                    dataGridView.Columns["Name"].Width = 200;
                    dataGridView.Columns["Path"].Width = 300;

                }
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message.ToString());
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void RunCommand_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.RunArgumentCommand = textBoxArgumentFileCommand.Text;
            Properties.Settings.Default.RunBatchCommand = textBoxBatchCommandCommand.Text;
            //Properties.Settings.Default.RunOpenPictureWith;
            //Properties.Settings.Default.RunOpenVideoWith;
        }

        private void ProcessRun(string command, string arguments, bool waitForExit)
        {
            using (var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = command,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = false,
                    RedirectStandardError = false,
                    RedirectStandardInput = false,
                    CreateNoWindow = false
                }
            })
            {
                bool result = process.Start();
                if (waitForExit)
                {
                    process.WaitForExit();
                    process.Close();
                    process.Dispose();
                }
            }
             
            
        }

        private void ProcessRun(string commandWithArguments, bool waitForExit)
        {
            string command = "";
            string arguments = "";
            if (commandWithArguments.StartsWith("\""))
            {
                int commandEndIndex = commandWithArguments.IndexOf('\"', 1);
                if (commandEndIndex < 0)
                {
                    command = commandWithArguments;
                    arguments = "";
                }
                else
                {
                    command = commandWithArguments.Substring(1, commandEndIndex - 1);
                    int argumentStartIndex = commandEndIndex + 2;
                    arguments = commandWithArguments.Substring(argumentStartIndex, commandWithArguments.Length - argumentStartIndex);
                }
            }
            else
            {
                int commandEndIndex = commandWithArguments.IndexOf(' ', 0);
                if (commandEndIndex < 0)
                {
                    command = commandWithArguments;
                    arguments = "";
                }
                else
                {
                    command = commandWithArguments.Substring(0, commandEndIndex);
                    int argumentStartIndex = commandEndIndex + 1;
                    arguments = commandWithArguments.Substring(argumentStartIndex, commandWithArguments.Length - argumentStartIndex);
                }
            }

            ProcessRun(command, arguments, waitForExit);            
        }

        private void textBoxBatchCommandCommand_TextChanged(object sender, EventArgs e)
        {
            textBoxCommandCommandExample.Text = "";
            foreach (Metadata metadata in Metadatas)
            {
                textBoxCommandCommandExample.Text += (textBoxCommandCommandExample.Text == "" ? "" : "\r\n") + metadata.ReplaceVariables(textBoxBatchCommandCommand.Text, AllowedFileNameDateTimeFormats);
            }
        }

        private void buttonOpenWithOpenWith_Click(object sender, EventArgs e)
        {
            
        }

        private void buttonBatchCommandBatchRun_Click(object sender, EventArgs e)
        {
            string errors = "";
            foreach (Metadata metadata in Metadatas)
            {
                string commandWithArguments = metadata.ReplaceVariables(textBoxBatchCommandCommand.Text, AllowedFileNameDateTimeFormats);
                try
                {
                    ProcessRun(commandWithArguments, checkBoxBatchCommandWaitForExit.Checked);
                } catch (Exception ex)
                {
                    errors += (errors == "" ? "" : "\r\n") + "File: " + metadata.FileFullPath + "\r\nError message: " + ex.Message;
                }
            }
            if (errors != "") MessageBox.Show(errors);
        }

        private void buttonArgumentFileRun_Click(object sender, EventArgs e)
        {
            string tempArguFileFullPath = ExiftoolWriter.GetTempArguFileFullPath();
            string commandWithArguments = textBoxArgumentFileCommand.Text.Replace("{TempFileArgumentFullPath}", tempArguFileFullPath);

            System.IO.File.WriteAllText(@"C:\Users\Public\TestFolder\WriteText.txt", textBoxArgumentFileArgumentFile.Text);
            try
            {
                ProcessRun(commandWithArguments, checkBoxArgumentWaitForExit.Checked);
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

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

        private void comboBoxArgumentFileCommandVariables_SelectionChangeCommitted(object sender, EventArgs e)
        {
            SelectionChangeCommitted(textBoxArgumentFileCommand, comboBoxArgumentFileCommandVariables.Text);
        }

        private void comboBoxBatchCommandCommandVariables_SelectionChangeCommitted(object sender, EventArgs e)
        {
            SelectionChangeCommitted(textBoxBatchCommandCommand, comboBoxBatchCommandCommandVariables.Text);
        }

        
    }
}
