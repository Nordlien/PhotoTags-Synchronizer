using System;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;

namespace PhotoTagsSynchronizer
{
    public partial class FormMessageBox : KryptonForm
    {
        public FormMessageBox(string title, string message)
        {
            InitializeComponent();
            this.Text = title;
            textBoxMessage.Text = message;
        }

        public void AppendMessage (string message)
        {
            textBoxMessage.Text += "\r\n" + message;
        }

        public void UpdateMessage(string message)
        {
            textBoxMessage.Text = message;
        }
    }
}
