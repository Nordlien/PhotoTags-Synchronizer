using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhotoTagsSynchronizer
{
    public partial class FormMessageBox : Form
    {
        public FormMessageBox(string message)
        {
            InitializeComponent();
            textBoxMessage.Text = message;
        }

        public void AppendMessage (string message)
        {
            textBoxMessage.Text += "\r\n" + message;
        }
    }
}
