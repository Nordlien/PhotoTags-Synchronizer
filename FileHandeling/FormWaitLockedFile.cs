using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
    }
}
