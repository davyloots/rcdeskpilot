using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;

namespace RCDeskPilot.API
{
    public class FlightModelSimple : IDisposable
    {
        #region Public properties
        #region Inertial coordinate system
        /// <summary>
        /// The X position in the inertial coordinate system (X positive north)
        /// </summary>
        public float X { get; set; }
        /// <summary>
        /// The Y position in the inertial coordinate system (Y positive east)
        /// </summary>
        public float Y { get; set; }
        /// <summary>
        /// The Z position in the inertial coordinate system (Z positive down)
        /// </summary>
        public float Z { get; set; }
        /// <summary>
        /// The yaw angle (positive right)
        /// </summary>
        public float Yaw { get; set; }
        /// <summary>
        /// The pitch angle (positive up)
        /// </summary>
        public float Pitch { get; set; }
        /// <summary>
        /// The roll angle (positive right)
        /// </summary>
        public float Roll { get; set; }

        /// <summary>
        /// Gets/Sets the orientation Quaternion.
        /// </summary>
        public Quaternion OrientationQuat;

        /// <summary>
        /// Gets/Sets the velocity vector of the aircraft.
        /// </summary>
        public Vector3 Velocity = new Vector3();
        #endregion

        #region Body coordinate system
        /// <summary>
        /// The X component of the velocity in body coordindates (forward)
        /// </summary>
        public float Vx { get; set; }
        /// <summary>
        /// The Y component of the velocity in body coordindates (right positive)
        /// </summary>
        public float Vy { get; set; }
        /// <summary>
        /// The Z component of the velocity in body coordindates (down positive)
        /// </summary>
        public float Vz { get; set; }

        /// <summary>
        /// The angular velocity component along the X axis (right roll positive)
        /// </summary>
        public float Wx { get; set; }
        /// <summary>
        /// The angular velocity component along the Y axis (pitch up positive)
        /// </summary>
        public float Wy { get; set; }
        /// <summary>
        /// The angular velocity component along the Z axis (yaw right positive)
        /// </summary>
        public float Wz { get; set; }

        /// <summary>
        /// The force along the X axis (forward positive)
        /// </summary>
        public float Fx { get; set; }
        /// <summary>
        /// The force along the Y axis (right positive)
        /// </summary>
        public float Fy { get; set; }
        /// <summary>
        /// The force along the Z axis (down positive)
        /// </summary>
        public float Fz { get; set; }

        /// <summary>
        /// The torque along the X axis (right roll positive)
        /// </summary>
        public float Tx { get; set; }
        /// <summary>
        /// The torque along the Y axis (pitch up positive)
        /// </summary>
        public float Ty { get; set; }
        /// <summary>
        /// The torque along the Z axis (yaw right positive)
        /// </summary>
        public float Tz { get; set; }

        /// <summary>
        /// The acceleration along the X axis (forward positive)
        /// </summary>
        public float Ax { get; set; }
        /// <summary>
        /// The acceleration along the Y axis (right positive)
        /// </summary>
        public float Ay { get; set; }
        /// <summary>
        /// The acceleration along the Z axis (down positive)
        /// </summary>
        public float Az { get; set; }

        /// <summary>
        /// The angular acceleration along the X axis (right roll positive)
        /// </summary>
        public float AAx { get; set; }
        /// <summary>
        /// The angular acceleration along the Y axis (pitch up positive)
        /// </summary>
        public float AAy { get; set; }
        /// <summary>
        /// The angular acceleration along the Z axis (yaw right positive)
        /// </summary>
        public float AAz { get; set; }

        /// <summary>
        /// The angle of attack
        /// </summary>
        public double Alpha { get; set; }
        /// <summary>
        /// The sideslip angle
        /// </summary>
        public double Beta { get; set; }
        #endregion

        #region Aircraft
        /// <summary>
        /// Gets/sets the aircraft parameters.
        /// </summary>
        public AircraftParameters AircraftParameters { get; set; }
        #endregion

        #region Controls
        /// <summary>
        /// Gets/sets the throttle [-1, 1].
        /// </summary>
        public double Throttle { get; set; }

        /// <summary>
        /// Gets/sets the elevator deflection [-1, 1].
        /// </summary>
        public double Elevator { get; set; }

        /// <summary>
        /// Gets/sets the aileron deflection [-1, 1].
        /// </summary>
        public double Ailerons { get; set; }

        /// <summary>
        /// Gets/sets the rudder deflection [-1, 1].
        /// </summary>
        public double Rudder { get; set; }
        #endregion

        #region Wind
        /// <summary>
        /// Gets/Sets the wind vector [m/s]
        /// </summary>
        public Vector3 Wind { get; set; }
        #endregion

        #region Misc
        /// <summary>
        /// Gets whether the aircraft is in a crashed state.
        /// </summary>
        public bool Crashed
        {
            get;
            set;
        }

        /// <summary>
        /// Gets/sets whether the simulation is paused.
        /// </summary>
        public bool Paused
        {
            get;
            set;
        }

        /// <summary>
        /// Gets whether the aircraft is touching the ground.
        /// </summary>
        public bool TouchedDown { get; set; }
        #endregion
        #endregion

        #region Protected constants
        /// <summary>
        /// The density of air (at sea level and standard pressure).
        /// </summary>
        protected const double _AirDensity = 1.2041; // [kg/m3]
        /// <summary>
        /// Gravitational acceleration.
        /// </summary>
        protected const double _Gravity = 9.81; // [m/s2]
        #endregion

        #region Virtual public methods
        /// <summary>
        /// Initialize the flightmodel.
        /// </summary>
        public virtual void Initialize()
        {

        }

        /// <summary>
        /// Shutdown the flightmodel.
        /// </summary>
        public virtual void ShutDown()
        {

        }

        /// <summary>
        /// Filters the control inputs before using them in the actual flightmodel.
        /// </summary>
        /// <param name="elapsedTime">The elapsed time in seconds since the last time this method was called.</param>
        /// <returns></returns>
        public virtual bool UpdateControls(float elapsedTime)
        {
            return false;
        }

        /// <summary>
        /// Calculates the total forces on the airframe.
        /// </summary>
        /// <param name="elapsedTime">The elapsed time in seconds since the last time this method was called.</param>
        public virtual bool CalculateForces(float elapsedTime, Vector3 wind)
        {
            return false;            
        }

        /// <summary>
        /// Calculates the total torque on the airframe.
        /// </summary>
        /// <param name="elapsedTime">The elapsed time in seconds since the last time this method was called.</param>
        /// <param name="wind">The current wind vector at the location of the aircraft.</param>
        /// <returns></returns>
        public virtual bool CalculateTorques(float elapsedTime, Vector3 wind)
        {
            return false;
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Clean up.
        /// </summary>
        public void Dispose()
        {
            
        }
        #endregion
    }
}
