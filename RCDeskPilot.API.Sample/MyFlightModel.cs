using System;
using System.Windows.Forms;

using RCDeskPilot.API;

namespace RCDeskPilot.API.Sample
{
    /// <summary>
    /// This class serves as a sample flight model implementation.
    /// The class inherits from the RCDeskPilot.API.FlightModelSimple class 
    /// which will give you access to all aircraft parameters and will release
    /// you of all tasks that are the same for all flightmodels so you can
    /// focus on the parts that really matter.
    /// </summary>
    public class MyFlightModel : FlightModelSimple
    {
        private DebugForm debugForm = null;

        /// <summary>
        /// Called when the flightmodel is being initialized.
        /// </summary>
        public override void Initialize()
        {
            debugForm = new DebugForm();
            debugForm.FlightModel = this;
            debugForm.Show();
        }

        /// <summary>
        /// Clean up all resources that we created.
        /// </summary>
        public override void ShutDown()
        {
            if (debugForm != null)
            {
                debugForm.Close();
                debugForm.Dispose();
                debugForm = null;
            }
        }

        /// <summary>
        /// Override the default sim control input if desired.
        /// </summary>
        /// <param name="elapsedTime">The elapsed time in seconds since the last time 
        /// this method was called.</param>
        /// <returns>Return true if you perform the input filtering (e.g. throttle delay, 
        /// servo response,...) yourself.
        /// Return false if you want the default filtering executed.
        /// </returns>
        public override bool UpdateControls(float elapsedTime)
        {
            if (debugForm != null)
            {
                if (debugForm.OverrideControls)
                {
                    this.Ailerons = debugForm.Ailerons;
                    this.Elevator = debugForm.Elevator;
                    this.Rudder = debugForm.Rudder;
                    this.Throttle = debugForm.Throttle;                    
                }
            }
            // If you perform input filtering here, return true

            return false; // Let the normal input filtering perform as usual.
        }

        /// <summary>
        /// Calculate the forces that act on the aircraft in the three axis.
        /// You should only adjust the Fx, Fy and Fz fields in this function, all 
        /// others will be recalculated by the base class.
        /// </summary>
        /// <param name="elapsedTime">The time elapsed since last call (in seconds).</param>
        /// <param name="wind">The windvector in airplane coordincates.</param>
        /// <returns>True if you've implemented this function, false if you want
        /// to use the default implementation.</returns>
        public override bool CalculateForces(float elapsedTime, Microsoft.DirectX.Vector3 wind)
        {            
            // Here you've got access to all parameters from the flightmodel, as well as
            // the aircraft parameters (through the AircraftParameters property).
            
            // Fx = force front/back
            // Fy = force left/right
            // Fz = force up/down

            // Return true if you want to override the default implementation
            // Return false if you don't want to implement this method yourself.
            return false;
        }

        /// <summary>
        /// Calculate the torques that act on the aircraft in the three axis.
        /// You should only adjust the Tx, Ty and Tz fields in this function, all 
        /// others will be recalculated by the base class.
        /// </summary>
        /// <param name="elapsedTime">The time elapsed since last call (in seconds).</param>
        /// <param name="wind">The windvector in airplane coordincates.</param>
        /// <returns>True if you've implemented this function, false if you want
        /// to use the default implementation.</returns>
        public override bool CalculateTorques(float elapsedTime, Microsoft.DirectX.Vector3 wind)
        {
            // Here you've got access to all parameters from the flightmodel, as well as
            // the aircraft parameters (through the AircraftParameters property).

            // Tx = Torque around x-axis.
            // Ty = Torque around y-axis.
            // Tz = Torque around z-axis.

            // Return true if you want to override the default implementation
            // Return false if you don't want to implement this method yourself.
            return false;
        }
    }
}
