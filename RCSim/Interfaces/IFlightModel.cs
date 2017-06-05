using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using RCDeskPilot.API;
using Bonsai.Objects.Terrain;
using RCSim;

namespace RCSim.Interfaces
{
    interface IFlightModel : IAirplaneControl, IDisposable
    {
        #region Properties
        /// <summary>
        /// The X position in the inertial coordinate system (X positive north)
        /// </summary>
        float X { get; set; }
        /// <summary>
        /// The Y position in the inertial coordinate system (Y positive east)
        /// </summary>
        float Y { get; set; }
        /// <summary>
        /// The Z position in the inertial coordinate system (Z positive down)
        /// </summary>
        float Z { get; set; }
        /// <summary>
        /// The yaw angle (positive right)
        /// </summary>
        float Yaw { get; set; }
        /// <summary>
        /// The pitch angle (positive up)
        /// </summary>
        float Pitch { get; set; }
        /// <summary>
        /// The roll angle (positive right)
        /// </summary>
        float Roll { get; set; }
        
        /// <summary>
        /// Gets the speed of the aircraft.
        /// </summary>
        double Speed { get; }
        
        /// <summary>
        /// Gets/sets whether the flightmodel should be paused.
        /// </summary>
        bool Paused { get; set; }

        /// <summary>
        /// Gets/Sets the wind vector [m/s]
        /// </summary>
        Vector3 Wind { get; set; }

        /// <summary>
        /// Gets/Sets the heightmap.
        /// </summary>
        Heightmap Heightmap { get; set; }

        /// <summary>
        /// Gets/Sets a reference to the water.
        /// </summary>
        List<Water> Water { get; set; }

        /// <summary>
        /// Gets whether the aircraft is in a crashed state.
        /// </summary>
        bool Crashed { get; set; }

        /// <summary>
        /// Gets whether the aircraft is on the ground.
        /// </summary>
        bool TouchedDown { get; }

        /// <summary>
        /// Gets whether the aircraft touching water.
        /// </summary>
        bool OnWater { get; }

        /// <summary>
        /// Gets whether the flaps have been extended.
        /// </summary>
        bool FlapsExtended { get; set; }

        /// <summary>
        /// Gets whether the gear has been extended.
        /// </summary>
        bool GearExtended { get; set; }

        /// <summary>
        /// Gets/sets the origin point of the towing cable.
        /// </summary>
        Vector3 CableOrigin
        {
            get;
            set;
        }

        /// <summary>
        /// Gets/sets the velocity of the towing cable.
        /// </summary>
        Vector3 CableVelocity
        {
            get;
            set;
        }

        /// <summary>
        /// Gets/sets the length of the towing cable.
        /// </summary>
        float CableLength
        {
            get;
            set;
        }

        /// <summary>
        /// Gets/sets whether the towing cable is currently attached.
        /// </summary>
        bool CableEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets/Sets the AircraftParameters.
        /// </summary>
        RCSim.DataClasses.AircraftParameters AircraftParameters { get; set; }

        /// <summary>
        /// Gets the Euler angles in world coordinates.
        /// </summary>
        Vector3 Angles { get; }

        /// <summary>
        /// Gets the velocity in world coordinates.
        /// </summary>
        Vector3 Velocity { get; set; }

        /// <summary>
        /// Gets the list of collision points in World coordinates.
        /// </summary>
        List<Vector3> CollisionPoints
        {
            get;
        }

        /// <summary>
        /// Gets the list of gear points in World coordinates.
        /// </summary>
        List<Vector3> GearPoints
        {
            get;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Initializes the flightmodel.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Resets the flightmodel.
        /// </summary>
        void Reset();

        /// <summary>
        /// Handlaunches an airplane.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        void HandLaunch(float x, float y, float z);

        /// <summary>
        /// Allows the flightmodel to override the controls.
        /// </summary>
        void UpdateControls(float elapsedTime);

        /// <summary>
        /// Tells the flight model to recalculate constants.
        /// </summary>
        void UpdateConstants();
        #endregion
    }
}
