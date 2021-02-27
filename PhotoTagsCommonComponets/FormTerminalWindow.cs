using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FastColoredTextBoxNS;

namespace PhotoTagsCommonComponets
{
    public partial class FormTerminalWindow : Form
    {
        public FormTerminalWindow()
        {
            InitializeComponent();
        }

        TextStyle infoStyle = new TextStyle(Brushes.Yellow, null, FontStyle.Regular);
        TextStyle warningStyle = new TextStyle(Brushes.BurlyWood, null, FontStyle.Regular);
        TextStyle errorStyle = new TextStyle(Brushes.Red, null, FontStyle.Regular);

        public void LogInfo(string text)
        {
            Log(text, infoStyle); 
        }

        public void LogError(string text)
        {
            Log(text, errorStyle);
        }

        public void LogWarningo(string text)
        {
            Log(text, warningStyle);
        }


        public void Log(string text, Style style)
        {
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
            //
            fastColoredTextBox1.Selection.EndUpdate();
            fastColoredTextBox1.EndUpdate();
        }

        private void buttonScrollToEnd_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.GoEnd();
        }
    }
}
