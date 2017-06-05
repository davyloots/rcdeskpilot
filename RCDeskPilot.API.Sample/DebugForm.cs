using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RCDeskPilot.API.Sample
{
    /// <summary>
    /// A simple form showing how you
    /// </summary>
    public partial class DebugForm : Form
    {
        /// <summary>
        /// Gets/sets the flightmodel.
        /// </summary>
        public MyFlightModel FlightModel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets whether the default sim controls should be overridden with this one.
        /// </summary>
        public bool OverrideControls
        {
            get { return checkBoxOverride.Checked; }
        }

        /// <summary>
        /// Gets the ailerons controller value.
        /// </summary>
        public double Ailerons
        {
            get { return trackBarAilerons.Value / 10.0; }
        }

        /// <summary>
        /// Gets the elevator controller value.
        /// </summary>
        public double Elevator
        {
            get { return -trackBarElevator.Value / 10.0; }
        }

        /// <summary>
        /// Gets the rudder controller value.
        /// </summary>
        public double Rudder
        {
            get { return trackBarRudder.Value / 10.0; }
        }

        /// <summary>
        /// Gets the throttle controller value.
        /// </summary>
        public double Throttle
        {
            get { return trackBarThrottle.Value / 10.0; }
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public DebugForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Tick event from the updateTimer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updateTimer_Tick(object sender, EventArgs e)
        {
            if (FlightModel != null)
            {
                labelPosition.Text = string.Format("({0}, {1}, {2})",
                    FlightModel.X.ToString("F02"),
                    FlightModel.Y.ToString("F02"),
                    FlightModel.Z.ToString("F03"));
                labelYawPitchRoll.Text = string.Format("({0}, {1}, {2})",
                    FlightModel.Yaw.ToString("F02"),
                    FlightModel.Pitch.ToString("F02"),
                    FlightModel.Roll.ToString("F02"));

                labelAlpha.Text = FlightModel.Alpha.ToString("F02");

                attitudeControl.Roll = FlightModel.Roll;
                attitudeControl.Pitch = FlightModel.Pitch;
                attitudeControl.Speed = FlightModel.Velocity.Length();
                attitudeControl.Altitude = -FlightModel.Z;
                if (!checkBoxOverride.Checked)
                {
                    trackBarAilerons.Value = (int)Math.Round(FlightModel.Ailerons * 10);
                    trackBarElevator.Value = (int)Math.Round(-FlightModel.Elevator * 10);
                    trackBarRudder.Value = (int)Math.Round(FlightModel.Rudder * 10);
                    trackBarThrottle.Value = (int)Math.Round(FlightModel.Throttle * 10);
                }
            }
        }

        private void DebugForm_MouseMove(object sender, MouseEventArgs e)
        {
            Cursor.Show();
        }
    }
}
