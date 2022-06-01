using Krypton.Toolkit;
using FastColoredTextBoxNS;
using System.Diagnostics;
using System.Windows.Forms;
using System;
using System.Drawing;

namespace PhotoTagsCommonComponets
{
    public partial class FormTerminalWindow : KryptonForm
    {
        public FormTerminalWindow()
        {
            InitializeComponent();
        }

        private FastColoredTextBoxNS.TextStyle infoStyle = new FastColoredTextBoxNS.TextStyle(Brushes.Yellow, null, FontStyle.Regular);
        private FastColoredTextBoxNS.TextStyle warningStyle = new FastColoredTextBoxNS.TextStyle(Brushes.BurlyWood, null, FontStyle.Regular);
        private FastColoredTextBoxNS.TextStyle errorStyle = new FastColoredTextBoxNS.TextStyle(Brushes.Red, null, FontStyle.Regular);
        private Process process = null;
        private bool wasProcessKilled = false;

        public bool GetWasProcessKilled()
        {
            return wasProcessKilled;
        }

        public void SetProcssToFollow(Process processFollowMe)
        {
            process = processFollowMe;
            wasProcessKilled = false;
        }

        public void LogInfo(string text)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<string>(LogInfo), text);
                return;
            }
            Log(text, infoStyle); 
        }

        public void LogError(string text)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<string>(LogError), text);
                return;
            }

            Log(text, errorStyle);
        }

        public void LogWarning(string text)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<string>(LogWarning), text);
                return;
            }
            Log(text, warningStyle);
        }

        public void Log(string text, Style style)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<string, Style>(Log), text, style);
                return;
            }

            //some stuffs for best performance
            fastColoredTextBox1.BeginUpdate();
            fastColoredTextBox1.Selection.BeginUpdate();
            //remember user selection
            var userSelection = fastColoredTextBox1.Selection.Clone();
            //add text with predefined style
            fastColoredTextBox1.TextSource.CurrentTB = fastColoredTextBox1;
            fastColoredTextBox1.AppendText(text, style);
            //restore user selection
            if (!userSelection.IsEmpty || userSelection.Start.iLine < fastColoredTextBox1.LinesCount - 2)
            {
                fastColoredTextBox1.Selection.Start = userSelection.Start;
                fastColoredTextBox1.Selection.End = userSelection.End;
            }
            else
                fastColoredTextBox1.GoEnd();//scroll to end of the text
            fastColoredTextBox1.Selection.EndUpdate();
            fastColoredTextBox1.EndUpdate();            
            fastColoredTextBox1.Refresh();
        }

        public void GoEnd()
        {
            fastColoredTextBox1.GoEnd();
        }

        private void buttonScrollToEnd_Click(object sender, EventArgs e)
        {
            GoEnd();
        }

        private void FormTerminalWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (process != null && !process.HasExited)
            {
                if (MessageBox.Show("Proces is still running, kill the process?", "Process running", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    process.Kill();
                    wasProcessKilled = true;
                }
                else
                    e.Cancel = true;
            }
        }
    }
}
