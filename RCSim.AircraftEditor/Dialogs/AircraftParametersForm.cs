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
    internal partial class AircraftParametersForm : Form
    {
        protected AircraftParameters parameters = null;

        public AircraftParametersForm(AircraftParameters parameters)
        {
            this.parameters = parameters;
            InitializeComponent();
            if (parameters.Version >= 2.0)
            {
                sliderControlSpinFactor.Visible = false;
            }
            UpdateValues();
        }

        public void UpdateValues( )
        {
            // Yaw
            sliderControlRudderEfficiency.ParameterValue = parameters.RudderEfficiency / parameters.Izz;
            sliderControlYawDamping.ParameterValue = parameters.YawDamping / parameters.Izz;
            sliderControlYawStability.ParameterValue = parameters.YawStability / parameters.Izz; 

            // Pitch
            sliderControlElevatorEfficiency.ParameterValue = parameters.ElevatorEfficiency / parameters.Iyy;
            sliderControlPitchDamping.ParameterValue = parameters.PitchDamping / parameters.Iyy;
            sliderControlTrim.ParameterValue = parameters.PitchTrim / parameters.Iyy;
            sliderControlPitchStability.ParameterValue = parameters.PitchStability / parameters.Iyy;
            sliderControlCoG.ParameterValue = parameters.CenterOfGravity;

            // Roll
            if (parameters.Version == 1.0)
            {
                sliderControlAileronEfficiency.ParameterValue = parameters.AileronEfficiency / parameters.Ixx;
                sliderControlDihedralEfficiency.ParameterValue = parameters.DihedralEfficiency / parameters.Ixx;
            }
            else
            {
                sliderControlAileronEfficiency.Factor = 100;
                sliderControlAileronEfficiency.ParameterValue = parameters.AileronEfficiency;
                sliderControlDihedralEfficiency.Factor = 10;
                sliderControlDihedralEfficiency.ParameterValue = parameters.DihedralEfficiency;
            }
            sliderControlRollDamping.ParameterValue = parameters.RollDamping / parameters.Ixx;
            sliderControlDihedralAngle.ParameterValue = parameters.DihedralAngle * 180.0 / Math.PI;
            sliderControlSpinFactor.ParameterValue = parameters.SpinFactor / parameters.Ixx;

            // Prop wash
            sliderControlPropWashRudder.ParameterValue = parameters.PropWashRudder;
            sliderControlPropWashElevator.ParameterValue = parameters.PropWashElevator;
            sliderControlPropWashAilerons.ParameterValue = parameters.PropWashAilerons;
        }

        private void sliderControlRudderEfficiency_ValueChanged(object sender, EventArgs e)
        {
            parameters.RudderEfficiency = sliderControlRudderEfficiency.ParameterValue * parameters.Izz;
        }

        private void sliderControlYawDamping_ValueChanged(object sender, EventArgs e)
        {
            parameters.YawDamping = sliderControlYawDamping.ParameterValue * parameters.Izz;
        }

        private void sliderControlYawStability_ValueChanged(object sender, EventArgs e)
        {
            parameters.YawStability = sliderControlYawStability.ParameterValue * parameters.Izz;
        }

        private void sliderControlElevatorEfficiency_ValueChanged(object sender, EventArgs e)
        {
            parameters.ElevatorEfficiency = sliderControlElevatorEfficiency.ParameterValue * parameters.Iyy;
        }

        private void sliderControlPitchDamping_ValueChanged(object sender, EventArgs e)
        {
            parameters.PitchDamping = sliderControlPitchDamping.ParameterValue * parameters.Iyy;
        }

        private void sliderControlTrim_ValueChanged(object sender, EventArgs e)
        {
            parameters.PitchTrim = sliderControlTrim.ParameterValue * parameters.Iyy;
        }

        private void sliderControlPitchStability_ValueChanged(object sender, EventArgs e)
        {
            parameters.PitchStability = sliderControlPitchStability.ParameterValue * parameters.Iyy;
        }

        private void sliderControlCoG_ValueChanged(object sender, EventArgs e)
        {
            parameters.CenterOfGravity = sliderControlCoG.ParameterValue;
        }

        private void sliderControlAileronEfficiency_ValueChanged(object sender, EventArgs e)
        {
            if (parameters.Version == 1.0)
            {
                parameters.AileronEfficiency = sliderControlAileronEfficiency.ParameterValue * parameters.Ixx;
            }
            else
            {
                parameters.AileronEfficiency = sliderControlAileronEfficiency.ParameterValue;
            }
        }

        private void sliderControlRollDamping_ValueChanged(object sender, EventArgs e)
        {
            parameters.RollDamping = sliderControlRollDamping.ParameterValue * parameters.Ixx;
        }

        private void sliderControlDihedralAngle_ValueChanged(object sender, EventArgs e)
        {
            parameters.DihedralAngle = Math.PI * sliderControlDihedralAngle.ParameterValue / 180.0;
        }

        private void sliderControlDihedralEfficiency_ValueChanged(object sender, EventArgs e)
        {
            if (parameters.Version == 1.0)
                parameters.DihedralEfficiency = sliderControlDihedralEfficiency.ParameterValue * parameters.Ixx;
            else
                parameters.DihedralEfficiency = sliderControlDihedralEfficiency.ParameterValue;
        }

        private void sliderControlSpinFactor_ValueChanged(object sender, EventArgs e)
        {
            parameters.SpinFactor = sliderControlSpinFactor.ParameterValue * parameters.Ixx;
        }

        private void sliderControlPropWashRudder_ValueChanged(object sender, EventArgs e)
        {
            parameters.PropWashRudder = sliderControlPropWashRudder.ParameterValue;
        }

        private void sliderControlPropWashElevator_ValueChanged(object sender, EventArgs e)
        {
            parameters.PropWashElevator = sliderControlPropWashElevator.ParameterValue;
        }

        private void sliderControlPropWashAilerons_ValueChanged(object sender, EventArgs e)
        {
            parameters.PropWashAilerons = sliderControlPropWashAilerons.ParameterValue;
        }
    }
}
