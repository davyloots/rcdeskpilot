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
    internal partial class CollisionParametersForm : Form
    {
        protected ModelControl modelControl = null;

        public CollisionParametersForm(ModelControl modelControl)
        {
            this.modelControl = modelControl;
            InitializeComponent();

            if (modelControl != null)
            {
                numericUpDownRestitution.Value = (int)(100 * modelControl.AirplaneModel.AirplaneControl.AircraftParameters.Restitution);
                numericUpDownCrashResistance.Value = (decimal)modelControl.AirplaneModel.AirplaneControl.AircraftParameters.CrashResistance;
                numericUpDownCrashResistanceGear.Value = (decimal)modelControl.AirplaneModel.AirplaneControl.AircraftParameters.CrashResistanceGear;
                numericUpDownGroundDrag.Value = (decimal)modelControl.AirplaneModel.AirplaneControl.AircraftParameters.GroundDrag;
                numericUpDownWaterDrag.Value = (decimal)modelControl.AirplaneModel.AirplaneControl.AircraftParameters.WaterDrag;
            }
        }

        private void numericUpDownRestitution_ValueChanged(object sender, EventArgs e)
        {
            if (modelControl != null)
            {
                modelControl.AirplaneModel.AirplaneControl.AircraftParameters.Restitution =
                    ((float)numericUpDownRestitution.Value / 100f);
            }
        }


        private void numericUpDownCrashResistanceGear_ValueChanged(object sender, EventArgs e)
        {
            if (modelControl != null)
            {
                modelControl.AirplaneModel.AirplaneControl.AircraftParameters.CrashResistanceGear =
                    ((double)numericUpDownCrashResistanceGear.Value);
            }
        }

        private void numericUpDownCrashResistance_ValueChanged(object sender, EventArgs e)
        {
            if (modelControl != null)
            {
                modelControl.AirplaneModel.AirplaneControl.AircraftParameters.CrashResistance =
                    ((double)numericUpDownCrashResistance.Value);
            }
        }


        private void numericUpDownGroundDrag_ValueChanged(object sender, EventArgs e)
        {
            if (modelControl != null)
            {
                modelControl.AirplaneModel.AirplaneControl.AircraftParameters.GroundDrag =
                    ((float)numericUpDownGroundDrag.Value);
            }
        }

        private void numericUpDownWaterDrag_ValueChanged(object sender, EventArgs e)
        {
            if (modelControl != null)
            {
                modelControl.AirplaneModel.AirplaneControl.AircraftParameters.WaterDrag =
                    ((float)numericUpDownWaterDrag.Value);
            }
        }
    }
}
