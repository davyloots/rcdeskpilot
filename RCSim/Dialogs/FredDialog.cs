using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RCSim.Dialogs
{
    public partial class FredDialog : Form
    {
        public string NameResult
        {
            get { return textBoxName.Text; }
        }

        public FredDialog()
        {
            InitializeComponent();
        }
    }
}
