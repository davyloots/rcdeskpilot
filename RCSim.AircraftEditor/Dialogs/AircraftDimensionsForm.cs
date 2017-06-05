using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using RCSim.DataClasses;

namespace RCSim.AircraftEditor.Dialogs
{
    internal partial class AircraftDimensionsForm : Form
    {
        protected ModelControl modelControl = null;

        public AircraftDimensionsForm(ModelControl modelControl)
        {
            this.modelControl = modelControl;
            InitializeComponent();

            if (modelControl != null)
            {
                numericUpDownWeight.Value = (decimal)modelControl.AirplaneModel.AirplaneControl.AircraftParameters.Mass;
                numericUpDownWingArea.Value = (decimal)
                    modelControl.AirplaneModel.AirplaneControl.AircraftParameters.WingArea * 10000;
                numericUpDownVertArea.Value = (decimal)
                    modelControl.AirplaneModel.AirplaneControl.AircraftParameters.VerticalArea * 10000;
                numericUpDownWingSpan.Value = (decimal)(200*                    
                    modelControl.AirplaneModel.AirplaneControl.AircraftParameters.WingCenter.Y *
                    modelControl.AirplaneModel.AirplaneControl.AircraftParameters.WingSpanFactor);
            }
            if (modelControl.AirplaneModel.AirplaneControl.AircraftParameters.Version == 1.0)
            {
                labelWingCenter.Visible = false;
                vectorControlWingCenter.Visible = false;
                labelPropCenter.Visible = false;
                vectorControlPropCenter.Visible = false;
                numericUpDownWingSpan.Visible = false;
                labelWingSpan.Visible = false;
                labelWingSpanCm.Visible = false;
                labelWingSpanInch.Visible = false;
            }
            else
            {
                Program.Instance.CursorVisible = true;
                Program.Instance.Cursor2Visible = true;
                vectorControlWingCenter.Vector = FlightModelWind.ToDirectX(modelControl.AirplaneModel.AirplaneControl.AircraftParameters.WingCenter);
                vectorControlPropCenter.Vector = FlightModelWind.ToDirectX(modelControl.AirplaneModel.AirplaneControl.AircraftParameters.PropCenter);
            }
        }

        private void numericUpDownWeight_ValueChanged(object sender, EventArgs e)
        {
            if (modelControl != null)
            {
                modelControl.AirplaneModel.AirplaneControl.AircraftParameters.Mass = (double)numericUpDownWeight.Value;
                modelControl.UpdateConstants();
                labelWeightLbs.Text = string.Format("{0} lbs",
                    (modelControl.AirplaneModel.AirplaneControl.AircraftParameters.Mass * 2.204625).ToString("F03"));
            }
        }

        private void numericUpDownWingArea_ValueChanged(object sender, EventArgs e)
        {
            if (modelControl != null)
            {
                modelControl.AirplaneModel.AirplaneControl.AircraftParameters.WingArea = ((double)(numericUpDownWingArea.Value)) / 10000.0;
                modelControl.UpdateConstants();
                labelWingAreaInch.Text = string.Format("{0} inch²",
                    (modelControl.AirplaneModel.AirplaneControl.AircraftParameters.WingArea * 1550.0031).ToString("F01"));
            }
        }

        private void numericUpDownVertArea_ValueChanged(object sender, EventArgs e)
        {
            if (modelControl != null)
            {
                modelControl.AirplaneModel.AirplaneControl.AircraftParameters.VerticalArea = ((double)(numericUpDownVertArea.Value)) / 10000.0;
                modelControl.UpdateConstants();
                labelVertAreaInch.Text = string.Format("{0} inch²",
                    (modelControl.AirplaneModel.AirplaneControl.AircraftParameters.VerticalArea * 1550.0031).ToString("F01"));
            }
        }

        private void vectorControlWingCenter_VectorChanged(object sender, EventArgs e)
        {
            if (modelControl != null)
            {
                modelControl.AirplaneModel.AirplaneControl.AircraftParameters.WingCenter = FlightModelWind.ToModel(vectorControlWingCenter.Vector);
                Program.Instance.CursorPosition = vectorControlWingCenter.Vector;
                if (modelControl.AirplaneModel.AirplaneControl.AircraftParameters.WingCenter.Y > 0)
                {
                    modelControl.AirplaneModel.AirplaneControl.AircraftParameters.WingSpanFactor =
                      (float)(0.005*(float)numericUpDownWingSpan.Value) / modelControl.AirplaneModel.AirplaneControl.AircraftParameters.WingCenter.Y;
                }
            }
        }
               
        private void numericUpDownWingSpan_ValueChanged(object sender, EventArgs e)
        {
            if (modelControl.AirplaneModel.AirplaneControl.AircraftParameters.WingCenter.Y > 0)
            {
                modelControl.AirplaneModel.AirplaneControl.AircraftParameters.WingSpanFactor =
                (float)(0.005 * (float)numericUpDownWingSpan.Value) / modelControl.AirplaneModel.AirplaneControl.AircraftParameters.WingCenter.Y;
                labelWingSpanInch.Text = string.Format("{0} inch",
                    ((float)numericUpDownWingSpan.Value * 0.393700787).ToString("F01"));
            }
        }

        private void vectorControlPropCenter_VectorChanged(object sender, EventArgs e)
        {
            if (modelControl != null)
            {
                modelControl.AirplaneModel.AirplaneControl.AircraftParameters.PropCenter = FlightModelWind.ToModel(vectorControlPropCenter.Vector);
                Program.Instance.Cursor2Position = vectorControlPropCenter.Vector;
            }
        }

        private void AircraftDimensionsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.Instance.CursorVisible = false;
            Program.Instance.Cursor2Visible = false;
        }

    }
}
