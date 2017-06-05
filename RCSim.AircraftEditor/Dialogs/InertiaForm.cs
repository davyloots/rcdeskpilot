using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RCSim.AircraftEditor.Dialogs
{
    internal partial class InertiaForm : Form
    {
        protected ModelControl modelControl = null;
        protected double estIxx = 0;
        protected double estIyy = 0;
        protected double estIzz = 0;

        public event EventHandler InertiaChanged;

        public ModelControl ModelControl
        {
            get { return modelControl; }
            set
            {
                modelControl = value;
                numericUpDownIxx.Value = (decimal)
                    modelControl.AirplaneModel.AirplaneControl.AircraftParameters.Ixx;
                numericUpDownIyy.Value = (decimal)
                    modelControl.AirplaneModel.AirplaneControl.AircraftParameters.Iyy;
                numericUpDownIzz.Value = (decimal)
                    modelControl.AirplaneModel.AirplaneControl.AircraftParameters.Izz;
            }
        }

        public InertiaForm()
        {
            InitializeComponent();
        }

        private void numericUpDownIxx_ValueChanged(object sender, EventArgs e)
        {
            if (modelControl != null)
            {
                modelControl.AirplaneModel.AirplaneControl.AircraftParameters.Ixx = (double)numericUpDownIxx.Value;
                OnInertiaChanged();
            }
        }

        private void numericUpDownIyy_ValueChanged(object sender, EventArgs e)
        {
            if (modelControl != null)
            {
                modelControl.AirplaneModel.AirplaneControl.AircraftParameters.Iyy = (double)numericUpDownIyy.Value;
                OnInertiaChanged();
            }
        }

        private void numericUpDownIzz_ValueChanged(object sender, EventArgs e)
        {
            if (modelControl != null)
            {
                modelControl.AirplaneModel.AirplaneControl.AircraftParameters.Izz = (double)numericUpDownIzz.Value;
                OnInertiaChanged();
            }
        }

        private void OnInertiaChanged()
        {
            if (InertiaChanged != null)
                InertiaChanged(this, EventArgs.Empty);
        }

        private void UpdateEstimates()
        {
            double l = (double)numericUpDownLength.Value;
            double d = (double)numericUpDownLength.Value;
            double mf = (double)numericUpDownFuseMass.Value;
            double span = (double)numericUpDownSpan.Value;
            double chord = (double)numericUpDownChord.Value;
            double mw = (double)numericUpDownWingMass.Value;
            estIxx = (mf * d * d / 8) + (mw * span * span / 12);
            estIyy = (mf/12.0)*(3*d*d / 4 + l * l) + (mw * chord * chord / 12);
            estIzz = (mf/12.0)*(3*d*d / 4 + l * l) + ((mw/12.0)*(chord*chord + span*span));
            labelInertiaEstimates.Text = string.Format("Ixx={0};Iyy={1};Izz={2}", 
                estIxx.ToString("F05"), estIyy.ToString("F05"), estIzz.ToString("F05"));
        }

        private void numericUpDownCalc_ValueChanged(object sender, EventArgs e)
        {
            UpdateEstimates();
        }

        private void buttonUseEstimate_Click(object sender, EventArgs e)
        {
            if (estIxx > 0 && estIyy > 0 && estIzz > 0)
            {
                try
                {
                    numericUpDownIxx.Value = (decimal)estIxx;
                    numericUpDownIyy.Value = (decimal)estIyy;
                    numericUpDownIzz.Value = (decimal)estIzz;
                }
                catch
                {
                }
            }
            else
                MessageBox.Show("The moments of inertia must be larger than zero.");
        }

        private void InertiaForm_Load(object sender, EventArgs e)
        {
            try
            {
                pictureBox1.Image = new Bitmap("data\\inertia.png");
            }
            catch
            {
            }
        }
    }
}
