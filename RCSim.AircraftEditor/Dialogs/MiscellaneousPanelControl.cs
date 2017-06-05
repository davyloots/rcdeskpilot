using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace RCSim.AircraftEditor.Dialogs
{
    internal partial class MiscellaneousPanelControl : UserControl
    {
        protected ModelControl modelControl = null;

        public ModelControl ModelControl
        {
            get { return modelControl; }
            set
            {
                modelControl = value;
                if (modelControl != null)
                {
                    checkBoxVariometer.Checked =
                        modelControl.AirplaneModel.AirplaneControl.AircraftParameters.HasVariometer;
                    checkBoxHasFlaps.Checked =
                        modelControl.AirplaneModel.AirplaneControl.AircraftParameters.HasFlaps;
                    checkBoxHasRetracts.Checked =
                        modelControl.AirplaneModel.AirplaneControl.AircraftParameters.HasRetracts;
                    checkBoxHandLaunched.Checked =
                        modelControl.AirplaneModel.AirplaneControl.AircraftParameters.HandLaunched;
                    checkBoxAilerons.Checked =
                        (modelControl.AirplaneModel.AirplaneControl.AircraftParameters.Channels > 3);
                    checkBoxAllowsTowing.Checked =
                        modelControl.AirplaneModel.AirplaneControl.AircraftParameters.AllowsTowing;
                    textBoxDescription.Text =
                        modelControl.AirplaneModel.AirplaneControl.AircraftParameters.Description;
                    labelIconFile.Text =
                        modelControl.AirplaneModel.AirplaneControl.AircraftParameters.IconFile;
                    numericUpDownFlapsDelay.Enabled =
                        modelControl.AirplaneModel.AirplaneControl.AircraftParameters.HasFlaps;
                    numericUpDownFlapsDelay.Value =
                        (decimal)modelControl.AirplaneModel.AirplaneControl.AircraftParameters.FlapsDelay;
                    numericUpDownGearDelay.Enabled =
                        modelControl.AirplaneModel.AirplaneControl.AircraftParameters.HasRetracts;
                    numericUpDownGearDelay.Value =
                        (decimal)modelControl.AirplaneModel.AirplaneControl.AircraftParameters.GearDelay;
                    checkBoxHasFloats.Checked = 
                        modelControl.AirplaneModel.AirplaneControl.AircraftParameters.HasFloats;
                }
            }
        }
        public MiscellaneousPanelControl()
        {
            InitializeComponent();
        }

        private void checkBoxVariometer_CheckedChanged(object sender, EventArgs e)
        {
            if (modelControl != null)
            {
                modelControl.AirplaneModel.AirplaneControl.AircraftParameters.HasVariometer = checkBoxVariometer.Checked;
            }
        }

        private void checkBoxHasFlaps_CheckedChanged(object sender, EventArgs e)
        {
            if (modelControl != null)
            {
                modelControl.AirplaneModel.AirplaneControl.AircraftParameters.HasFlaps = checkBoxHasFlaps.Checked;
                numericUpDownFlapsDelay.Enabled =
                        modelControl.AirplaneModel.AirplaneControl.AircraftParameters.HasFlaps;
            }
        }

        private void checkBoxHasRetracts_CheckedChanged(object sender, EventArgs e)
        {
            if (modelControl != null)
            {
                modelControl.AirplaneModel.AirplaneControl.AircraftParameters.HasRetracts = checkBoxHasRetracts.Checked;
                numericUpDownGearDelay.Enabled =
                    modelControl.AirplaneModel.AirplaneControl.AircraftParameters.HasRetracts;
            }
        }

        private void checkBoxHandLaunched_CheckedChanged(object sender, EventArgs e)
        {
            if (modelControl != null)
            {
                modelControl.AirplaneModel.AirplaneControl.AircraftParameters.HandLaunched = checkBoxHandLaunched.Checked;
            }
        }

        private void textBoxDescription_TextChanged(object sender, EventArgs e)
        {
            if (modelControl != null)
            {
                string description = modelControl.AirplaneModel.AirplaneControl.AircraftParameters.ClearDescriptionString(textBoxDescription.Text);
                if (!description.Equals(textBoxDescription.Text))
                {
                    MessageBox.Show("Sorry, urls are not allowed in the description. Contact info@rcdeskpilot.com for more information on how to link advertising to your aircraft");
                }
                textBoxDescription.Text = description;
                modelControl.AirplaneModel.AirplaneControl.AircraftParameters.Description = textBoxDescription.Text;
            }
        }

        private void checkBoxAilerons_CheckedChanged(object sender, EventArgs e)
        {
            if (modelControl != null)
            {
                modelControl.AirplaneModel.AirplaneControl.AircraftParameters.Channels =
                    checkBoxAilerons.Checked ? 4 : 3;
            }
        }


        private void checkBoxAllowsTowing_CheckedChanged(object sender, EventArgs e)
        {
            if (modelControl != null)
            {
                modelControl.AirplaneModel.AirplaneControl.AircraftParameters.AllowsTowing = checkBoxAllowsTowing.Checked;
            }
        }

        private string AddFileToProject(string filename)
        {
            string openFile = Program.Instance.OpenFile;
            if (openFile != null)
            {
                string projectFolder = string.Concat(new FileInfo(openFile).DirectoryName, "\\");
                string newFileName = new FileInfo(filename).Name;
                string copiedName = string.Concat(projectFolder, newFileName);
                if (!File.Exists(copiedName))
                {
                    File.Copy(filename, copiedName);
                }
                return copiedName;
            }
            else
                return null;
        }

        private bool NormalizeIcon(string file)
        {
            try
            {
                Bitmap icon = new Bitmap(file);
                if ((icon.Width == 256) && (icon.Height == 256))
                {
                    icon.Dispose();
                    return true;
                }
                Bitmap result = new Bitmap(256, 256);
                Graphics g = Graphics.FromImage(result);
                if (icon.Width > icon.Height)
                    g.DrawImage(icon, new Rectangle(0, 0, result.Width, result.Height),
                        new Rectangle((icon.Width - icon.Height) / 2, 0, icon.Height, icon.Height), GraphicsUnit.Pixel);
                else
                    g.DrawImage(icon, new Rectangle(0, 0, result.Width, result.Height),
                        new Rectangle(0, (icon.Height - icon.Width) / 2, icon.Width, icon.Width), GraphicsUnit.Pixel);
                if (file.ToLower().EndsWith("png"))
                    result.Save(file, System.Drawing.Imaging.ImageFormat.Png);
                else
                    result.Save(file, System.Drawing.Imaging.ImageFormat.Jpeg);
                icon.Dispose();
                result.Dispose();
                return true;
            }
            catch
            {
                MessageBox.Show("This is not a valid icon: icons are 256x256 png or jpg files");
                return false;
            }
        }

        private void buttonFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "PNG files (.png)|*.png|JPG files (.jpg)|*.jpg";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string newFile = AddFileToProject(fileDialog.FileName);
                if (NormalizeIcon(newFile))
                {
                    if (newFile != null)
                    {
                        string newFilePart = new FileInfo(newFile).Name;
                        labelIconFile.Text = newFilePart;
                        modelControl.AirplaneModel.AirplaneControl.AircraftParameters.IconFile = newFilePart;
                    }
                }
            }
        }

        private void numericUpDownFlapsDelay_ValueChanged(object sender, EventArgs e)
        {
            if (modelControl != null)
            {
                modelControl.AirplaneModel.AirplaneControl.AircraftParameters.FlapsDelay =
                    (double)numericUpDownFlapsDelay.Value;
            }
        }

        private void numericUpDownGearDelay_ValueChanged(object sender, EventArgs e)
        {
            if (modelControl != null)
            {
                modelControl.AirplaneModel.AirplaneControl.AircraftParameters.GearDelay =
                    (double)numericUpDownGearDelay.Value;
            }
        }

        private void checkBoxHasFloats_CheckedChanged(object sender, EventArgs e)
        {
            if (modelControl != null)
            {
                modelControl.AirplaneModel.AirplaneControl.AircraftParameters.HasFloats = checkBoxHasFloats.Checked;
            }
        }

    }
}
