using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Bonsai.Core;

namespace RCSim.AircraftEditor.Dialogs
{
    public partial class NewVariationStep2Control : UserControl
    {
        private string baseAircraft = null;

        public event EventHandler FormValidatedChanged;

        public string BaseAircraft
        {
            get { return baseAircraft; }
        }

        public string AircraftName
        {
            get { return textBoxName.Text; }
        }

        public bool FormValid
        {
            get { return (!string.IsNullOrEmpty(baseAircraft)) && (!string.IsNullOrEmpty(textBoxName.Text)); }
        }

        public NewVariationStep2Control()
        {
            InitializeComponent();
        }

        private void textBoxName_KeyPress(object sender, KeyPressEventArgs e)
        {
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ-_1234567890+() \b";
            if (chars.IndexOf(e.KeyChar) == -1)
                e.Handled = true;
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
           if (FormValidatedChanged != null)
                FormValidatedChanged(this, EventArgs.Empty);
        }   

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(openFileDialog.FileName))
                {
                    labelSelected.Text = Utility.GetFileNamePart(openFileDialog.FileName);
                    baseAircraft = openFileDialog.FileName;
                    if (FormValidatedChanged != null)
                        FormValidatedChanged(this, EventArgs.Empty);
                }
            }
        }
    }
}
