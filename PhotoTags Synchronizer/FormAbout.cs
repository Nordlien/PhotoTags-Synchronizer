using System;
using System.Windows.Forms;
using Krypton.Toolkit;
using NLog;

namespace PhotoTagsSynchronizer
{
    public partial class FormAbout : KryptonForm
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public FormAbout()
        {
            InitializeComponent();
            try
            {
                richTextBox1.LoadFile(FileHandeling.FileHandler.CombineApplicationPathWithFilename("About.rtf"));
            }
            catch { }
        }

        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(e.LinkText);
            } catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
    }
}
