using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace RCSim.AircraftEditor.Dialogs
{
    public partial class NewAircraftStep2Control : UserControl
    {
        private bool formValid = true;
        private bool folderEdited = false;

        public bool FormValid
        {
            get { return (!string.IsNullOrEmpty(textBoxName.Text)) && (!string.IsNullOrEmpty(textBoxFolder.Text)); }
        }

        public string AircraftName
        {
            get { return textBoxName.Text; }
        }

        public string FolderName
        {
            get { return textBoxFolder.Text; }
        }

        public event EventHandler FormValidatedChanged;

        public NewAircraftStep2Control()
        {
            InitializeComponent();

            comboBoxFlightModelType.SelectedIndex = 0;
        }

        private void textBoxName_KeyPress(object sender, KeyPressEventArgs e)
        {
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ-_1234567890+() \b";
            if (chars.IndexOf(e.KeyChar) == -1)
                e.Handled = true;
        }

        private void textBoxFolder_KeyPress(object sender, KeyPressEventArgs e)
        {
            folderEdited = true;
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ-_1234567890+() \b";
            if (chars.IndexOf(e.KeyChar) == -1)
                e.Handled = true;
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            if (!folderEdited)
                textBoxFolder.Text = textBoxName.Text;
            //bool prevValid = formValid;
            //formValid = FormValid;
            //if (prevValid != formValid)
            //{
                if (FormValidatedChanged != null)
                    FormValidatedChanged(this, EventArgs.Empty);
            //}
        }
    }
}
