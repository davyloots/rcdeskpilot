using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Bonsai.Objects.Meshes;
using System.Reflection;
using Bonsai.Objects;
using Microsoft.DirectX;
using Bonsai.Core;
using System.IO;
using Bonsai.Objects.Textures;

namespace RCSim.AircraftEditor.Dialogs
{
    internal partial class SurfacePropertiesControl : UserControl
    {
        #region Protected fields
        protected ControlSurface surface = null;
        protected AirplaneModel airplane = null;
        #endregion

        #region Public events
        public event EventHandler FileChanged;
        #endregion


        public SurfacePropertiesControl()
        {
            InitializeComponent();
        }

        public void SetControlSurface(object obj)
        {
            surface = null;
            airplane = null;
            surface = obj as ControlSurface;
            if (surface != null)
            {
                comboBoxChannel.Enabled = true;
                comboBoxType.Enabled = true;
                checkBoxReversed.Enabled = true;
                comboBoxChannel.SelectedIndex = (int)surface.Channel;
                comboBoxType.SelectedIndex = (int)surface.ControlSurfaceType;
                checkBoxReversed.Visible = true;
                checkBoxReversed.Checked = surface.Reversed;
                vectorControlPosition.Vector = surface.Position;
                vectorControlRotationAxis.Vector = surface.RotationAxis;
                labelFileName.Text = surface.MeshFileName;
                numericUpDownDefaultAngle.Value = Convert.ToDecimal(surface.ZeroAngle * 180 / Math.PI);
                numericUpDownMinAngle.Value = Convert.ToDecimal(surface.MinimumAngle * 180 / Math.PI);
                numericUpDownMaxAngle.Value = Convert.ToDecimal(surface.MaximumAngle * 180 / Math.PI);
                numericUpDownScale.Value = Convert.ToDecimal(surface.Scale.X);
                comboBoxChannel.Visible = true;
                labelChannel.Visible = true;
                comboBoxType.Visible = true;
                labelType.Visible = true;
                vectorControlPosition.Visible = true;
                labelPosition.Visible = true;
                vectorControlRotationAxis.Visible = true;
                labelRotation.Visible = true;
                numericUpDownDefaultAngle.Visible = true;
                labelDefaultAngle.Visible = true;
                numericUpDownMaxAngle.Visible = true;
                labelMaxAngle.Visible = true;
                numericUpDownMinAngle.Visible = true;
                labelMinAngle.Visible = true;
                labelSpanLength.Visible = false;
            }
            else
            {
                airplane = obj as AirplaneModel;
                if (airplane != null)
                {
                    numericUpDownScale.Value = Convert.ToDecimal(airplane.Scale.X);
                    labelFileName.Text = airplane.MeshFileName;
                    labelSpanLength.Visible = true;
                    UpdateSizeLabel();
                    comboBoxChannel.Visible = false;
                    labelChannel.Visible = false;
                    comboBoxType.Visible = false;
                    labelType.Visible = false;
                    vectorControlPosition.Visible = false;
                    labelPosition.Visible = false;
                    vectorControlRotationAxis.Visible = false;
                    labelRotation.Visible = false;
                    numericUpDownDefaultAngle.Visible = false;
                    labelDefaultAngle.Visible = false;
                    numericUpDownMaxAngle.Visible = false;
                    labelMaxAngle.Visible = false;
                    numericUpDownMinAngle.Visible = false;
                    labelMinAngle.Visible = false;
                    checkBoxReversed.Visible = false;
                }
                else
                {
                    comboBoxType.SelectedIndex = -1;
                    comboBoxChannel.SelectedIndex = -1;
                    checkBoxReversed.Checked = false;
                    comboBoxChannel.Enabled = false;
                    comboBoxType.Enabled = false;
                    checkBoxReversed.Enabled = false;
                }
            }
        }

        private void UpdateSizeLabel()
        {
            if (airplane != null && airplane.Mesh != null)
            {
                Vector3 size = airplane.BoundingBoxMax - airplane.BoundingBoxMin;
                labelSpanLength.Text = string.Format("S:{0}, L:{1}", size.X.ToString("F02"), size.Z.ToString("F02"));
            }
        }

        private void comboBoxChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (surface != null)
            {
                surface.Channel = (RCSim.DataClasses.AircraftParameters.ChannelEnum)comboBoxChannel.SelectedIndex;
                surface.SurfaceDefinition.Channel = surface.Channel;
            }
        }

        private void comboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (surface != null)
            {
                surface.ControlSurfaceType = (RCSim.DataClasses.AircraftParameters.ControlSurfaceTypeEnum)comboBoxType.SelectedIndex;
                surface.SurfaceDefinition.Type = surface.ControlSurfaceType;
            }
        }

        private void checkBoxReversed_CheckedChanged(object sender, EventArgs e)
        {
            if (surface != null)
            {
                surface.Reversed = checkBoxReversed.Checked;
                surface.SurfaceDefinition.Reversed = surface.Reversed;
            }
        }

        private void vectorControlPosition_VectorChanged(object sender, EventArgs e)
        {
            if (surface != null)
            {
                surface.Position = vectorControlPosition.Vector;
                surface.SurfaceDefinition.Position = surface.Position;
            }
        }

        private void vectorControlRotationAxis_VectorChanged(object sender, EventArgs e)
        {
            if (surface != null)
            {
                surface.RotationAxis = vectorControlRotationAxis.Vector;
                surface.SurfaceDefinition.RotationAxis = surface.RotationAxis;
            }
        }

        private void buttonFile_Click(object sender, EventArgs e)
        {
            if (surface != null)
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Filter = "DirectX .x files|*.x";
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    string newFile = AddFileToProject(fileDialog.FileName);
                    if (newFile != null)
                    {
                        string newFilePart = new FileInfo(newFile).Name;
                        surface.MeshFileName = newFilePart;
                        labelFileName.Text = newFilePart;
                    }
                    if (FileChanged != null)
                        FileChanged(this, EventArgs.Empty);
                }
            }
            else if (airplane != null)
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Filter = "DirectX .x files|*.x";
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    string newFile = AddFileToProject(fileDialog.FileName);
                    if (newFile != null)
                    {
                        string newFilePart = new FileInfo(newFile).Name;
                        airplane.MeshFileName = newFilePart;
                        labelFileName.Text = newFilePart;
                    }
                    if (FileChanged != null)
                        FileChanged(this, EventArgs.Empty);
                }
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
                    if (filename.EndsWith(".x"))
                    {
                        XMesh mesh = new XMesh(filename);
                        for (int i = 0; i < mesh.NumberTextures; i++)
                        {
                            TextureBase texture = mesh.GetTexture(i);
                            if ((texture != null) && (!string.IsNullOrEmpty(texture.FileName)))
                                AddFileToProject(texture.FileName);
                        }
                        mesh.Dispose();
                    }
                }
                return copiedName;
            }
            else
                return null;
        }

        private void numericUpDownDefaultAngle_ValueChanged(object sender, EventArgs e)
        {
            if (surface != null)
            {
                surface.ZeroAngle = (float)(Convert.ToDouble(numericUpDownDefaultAngle.Value) * Math.PI / 180);
                surface.SurfaceDefinition.ZeroAngle = surface.ZeroAngle;
            }
        }

        private void numericUpDownMinAngle_ValueChanged(object sender, EventArgs e)
        {
            if (surface != null)
            {
                surface.MinimumAngle = (float)(Convert.ToDouble(numericUpDownMinAngle.Value) * Math.PI / 180);
                surface.SurfaceDefinition.MinimumAngle = surface.MinimumAngle;
            }
        }

        private void numericUpDownMaxAngle_ValueChanged(object sender, EventArgs e)
        {
            if (surface != null)
            {
                surface.MaximumAngle = (float)(Convert.ToDouble(numericUpDownMaxAngle.Value) * Math.PI / 180);
                surface.SurfaceDefinition.MaximumAngle = surface.MaximumAngle;
            }
        }

        private void numericUpDownScale_ValueChanged(object sender, EventArgs e)
        {
            if (surface != null)
            {
                surface.SingleScale = (float)numericUpDownScale.Value;
                surface.SurfaceDefinition.Scale = surface.SingleScale;
                //surface.Scale = new Vector3((float)numericUpDownScale.Value, (float)numericUpDownScale.Value, (float)numericUpDownScale.Value);
            }
            if (airplane != null)
            {
                airplane.Scale = new Vector3((float)numericUpDownScale.Value, (float)numericUpDownScale.Value, (float)numericUpDownScale.Value);
                airplane.AirplaneControl.AircraftParameters.Scale = (float)numericUpDownScale.Value;
                airplane.AirplaneControl.AircraftParameters.UpdateScaledCollisionPoints();
                Program.Instance.ScaleChanged();
                UpdateSizeLabel();
            }
        }
    }
}
