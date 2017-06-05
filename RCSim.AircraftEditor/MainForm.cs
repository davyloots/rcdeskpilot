using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RCSim.AircraftEditor
{
    public partial class MainForm : Form
    {
        public Panel Panel3D
        {
            get { return panel3D; }
        }

        public Button Button3D
        {
            get { return button1; }
        }

        public MainForm()
        {
            InitializeComponent();
        }
    }
}
