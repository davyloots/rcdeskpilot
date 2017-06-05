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
    internal partial class SoundControl : UserControl
    {
        protected ModelControl modelControl;

        public ModelControl ModelControl
        {
            get { return modelControl; }
            set
            {
                modelControl = value;
                if (modelControl != null)
                {
                    EngineSoundFile = modelControl.AirplaneModel.AirplaneControl.AircraftParameters.EngineSound;
                    EngineMinFrequency = modelControl.AirplaneModel.AirplaneControl.AircraftParameters.EngineMinFrequency;
                    EngineMaxFrequency = modelControl.AirplaneModel.AirplaneControl.AircraftParameters.EngineMaxFrequency;
                }
            }
        }

        public string EngineSoundFile
        {
            get { return labelFileName.Text; }
            set { labelFileName.Text = value; }
        }

        public int EngineMinFrequency
        {
            get { return (int)numericUpDownMinFreq.Value; }
            set { numericUpDownMinFreq.Value = value; }
        }

        public int EngineMaxFrequency
        {
            get { return (int)numericUpDownMaxFreq.Value; }
            set { numericUpDownMaxFreq.Value = value; }
        }

        public SoundControl()
        {
            InitializeComponent();
        }

        private void buttonFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Sound files (.wav)|*.wav";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string newFile = AddFileToProject(fileDialog.FileName);
                if (newFile != null)
                {
                    string newFilePart = new FileInfo(newFile).Name;
                    EngineSoundFile = newFilePart;
                    modelControl.AirplaneModel.EngineSound = newFilePart;
                }
            }
        }

        private void numericUpDownMinFreq_ValueChanged(object sender, EventArgs e)
        {
            if (modelControl != null)
            {
                modelControl.AirplaneModel.AirplaneControl.AircraftParameters.EngineMinFrequency = (int)numericUpDownMinFreq.Value;
                modelControl.AirplaneModel.ReloadEngineFrequencies();
            }
        }

        private void numericUpDownMaxFreq_ValueChanged(object sender, EventArgs e)
        {
            if (modelControl != null)
            {
                modelControl.AirplaneModel.AirplaneControl.AircraftParameters.EngineMaxFrequency = (int)numericUpDownMaxFreq.Value;
                modelControl.AirplaneModel.ReloadEngineFrequencies();
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

        private void buttonClear_Click(object sender, EventArgs e)
        {
            modelControl.AirplaneModel.EngineSound = null;
            labelFileName.Text = string.Empty;
        }
    }
}
