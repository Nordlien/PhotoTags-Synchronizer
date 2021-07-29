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

namespace FileHandeling
{
    public partial class FormWaitLockedFile : Form
    {
        public FormWaitLockedFile()
        {
            InitializeComponent();
        }

        public bool IgnoredClicked { get; set; } = false;
        public bool RetryIsClicked { get; set; } = false;
        public bool IsFormVisible { get; set; } = false;

        private List<string> ListOfFiles = new List<string>();
        private static readonly Object _LisFilesLock = new Object();

        public void AddFiles(List<string> listOfFiles)
        {
            lock (_LisFilesLock) ListOfFiles = listOfFiles;
        }

        public string TextBoxFiles
        {
            set
            {
                textBoxFiles.Text = value;
            }
        }

        public void SetTextboxFiles(string text)
        {
            TextBoxFiles = text;
        }

        private void buttonIgnor_Click(object sender, EventArgs e)
        {
            IgnoredClicked = true;
            RetryIsClicked = false;
            this.Close();
        }

        private void buttonRetry_Click(object sender, EventArgs e)
        {
            IgnoredClicked = false;
            RetryIsClicked = true;
            this.Close();
        }

        private void FormWaitLockedFile_Shown(object sender, EventArgs e)
        {
            IsFormVisible = true;
        }

        private void FormWaitLockedFile_FormClosed(object sender, FormClosedEventArgs e)
        {
            IsFormVisible = false;
        }

        private void buttonCheck_Click(object sender, EventArgs e)
        {            
            buttonCheck.Enabled = false;
            try
            {
                textBoxFilesLockedByProcess.Text = "Files locked by process...\r\n";
                List<string> copyListOfFiles = new List<string>();
                lock (_LisFilesLock)
                {
                    copyListOfFiles.AddRange(ListOfFiles);
                }

                foreach (string path in copyListOfFiles)
                {
                    List<Process> processes = FileHandler.FindLockProcesses(path);
                    textBoxFilesLockedByProcess.Text += "Checking: " + path + "\r\n";
                    if (processes.Count == 0)
                    {
                        textBoxFilesLockedByProcess.Text += "  Locked by: Nothing found\r\n";
                    }
                    else
                    {
                        foreach (Process process in processes)
                        {
                            textBoxFilesLockedByProcess.Text += "  Locked by: " + process + "\r\n";
                        }
                    }
                }
                textBoxFilesLockedByProcess.Text += "\r\nDone...\r\nAll files checked\r\n";
            }
            catch
            {
            }

            buttonCheck.Enabled = true;
        }
    }
}
