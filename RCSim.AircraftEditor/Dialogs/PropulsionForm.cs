using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RCSim.AircraftEditor.Dialogs
{
    internal partial class PropulsionForm : Form
    {
        protected ModelControl modelControl = null;
        
        public ModelControl ModelControl
        {
            get { return modelControl; }
            set
            {
                modelControl = value;
                graph2Control.ValueList = modelControl.AirplaneModel.AirplaneControl.AircraftParameters.ThrustCoeffecientPoints;
                numericUpDownMaxThrust.Value = (decimal)
                        modelControl.AirplaneModel.AirplaneControl.AircraftParameters.MaximumThrust;
                numericUpDownThrottleDelay.Value = (decimal)
                    modelControl.AirplaneModel.AirplaneControl.AircraftParameters.ThrottleDelay;
            }
        }

        public PropulsionForm()
        {
            InitializeComponent();
        }

        private void numericUpDownMaxThrust_ValueChanged(object sender, EventArgs e)
        {
            if (modelControl != null)
            {
                modelControl.AirplaneModel.AirplaneControl.AircraftParameters.MaximumThrust = (double)(numericUpDownMaxThrust.Value);
                labelMaxThrustLbs.Text = string.Format("{0}lbs",
                    (modelControl.AirplaneModel.AirplaneControl.AircraftParameters.MaximumThrust * 0.224808943).ToString("F02"));
            }
        }

        private void numericUpDownThrottleDelay_ValueChanged(object sender, EventArgs e)
        {
            if (modelControl != null)
            {
                modelControl.AirplaneModel.AirplaneControl.AircraftParameters.ThrottleDelay = (double)(numericUpDownThrottleDelay.Value);
            }
        }
    }
}
