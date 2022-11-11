using System;
using System.Windows.Forms;
using Krypton.Toolkit;

namespace PhotoTagsSynchronizer
{
    public partial class FormAbout : KryptonForm
    {
        public FormAbout()
        {
            InitializeComponent();
            try
            {
                richTextBox1.LoadFile(FileHandeling.FileHandler.CombineApplicationPathWithFilename("About.rtf"));
            }
            catch { }
        }
    }
}
