using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;


namespace RCDeskPilot.API
{
    public class AircraftParameters
    {
        #region Public enumerations
        /// <summary>
        /// Enumeration of different types of flightmodel
        /// </summary>
        public enum FlightModelTypeEnum
        {
            Aircraft,
            Helicopter
        }
        #endregion

        #region public properties
        /// <summary>
        /// The mass of the airplane in Kg.
        /// </summary>
        public double Mass { get; set; }
        /// <summary>
        /// The inertia tensor value in the X-axis (roll)
        /// </summary>
        public double Ixx { get; set; }
        /// <summary>
        /// The inertia tensor value in the Y-axis (pitch)
        /// </summary>
        public double Iyy { get; set; }
        /// <summary>
        /// The inertia tensor value in the Z-axis (yaw)
        /// </summary>
        public double Izz { get; set; }

        /// <summary>
        /// The wing surface area in square meter.
        /// </summary>
        public double WingArea { get; set; }
        /// <summary>
        /// The total vertical surface area in square meter.
        /// </summary>
        public double VerticalArea { get; set; }
        /// <summary>
        /// The maximum thrust of the airplane in Newton.
        /// </summary>
        public double MaximumThrust { get; set; }
        /// <summary>
        /// Gets/sets the dihedral angle in radians.
        /// </summary>
        public double DihedralAngle { get; set; }
        /// <summary>
        /// Gets/sets the efficiency of the dihedral roll moment.
        /// </summary>
        public double DihedralEfficiency { get; set; }
        /// <summary>
        /// Gets the roll damping factor.
        /// </summary>
        public double RollDamping { get; set; }
        /// <summary>
        /// Gets the efficiency of the ailerons.
        /// </summary>
        public double AileronEfficiency { get; set; }
        /// <summary>
        /// Gets the pitch damping factor.
        /// </summary>
        public double PitchDamping { get; set; }
        /// <summary>
        /// Gets the efficiency of the elevator.
        /// </summary>
        public double ElevatorEfficiency { get; set; }
        /// <summary>
        /// Gets the yaw damping factor.
        /// </summary>
        public double YawDamping { get; set; }
        /// <summary>
        /// Gets the efficiency of the rudder.
        /// </summary>
        public double RudderEfficiency { get; set; }
        /// <summary>
        /// Gets the factor with which the alpha will try to go to zero (default 1.0)
        /// </summary>
        public double PitchStability { get; set; }
        /// <summary>
        /// Gets the pitchtrim: <0 : pitch down, >0 : pitch up.
        /// </summary>
        public double PitchTrim { get; set; }
        /// <summary>
        /// Gets the factor with which the beta will try to go to zero (default 1.0)
        /// </summary>
        public double YawStability { get; set; }
        /// <summary>
        /// Gets the effect of propwash on the ailerons.
        /// </summary>
        public double PropWashAilerons { get; set; }
        /// <summary>
        /// Gets the effect of propwash on the elevator.
        /// </summary>
        public double PropWashElevator { get; set; }
        /// <summary>
        /// Gets the effect of propwash on the rudder.
        /// </summary>
        public double PropWashRudder { get; set; }

        /// <summary>
        /// Gets the lateral instability caused by yaw at low airspeeds.
        /// </summary>
        public double SpinFactor { get; set; }
        /// <summary>
        /// Reference points for the Lift Coefficient for alpha in [-Pi, Pi[.
        /// </summary>
        public List<KeyValuePair<double, double>> LiftCoefficientPoints { get; set; }
        /// <summary>
        /// Reference points for the Drag Coefficient for alpha in [-Pi, Pi[.
        /// </summary>
        public List<KeyValuePair<double, double>> DragCoefficientPoints { get; set; }
        /// <summary>
        /// Reference points for the Side Lift Coefficient for beta in [-Pi, Pi[.
        /// </summary>
        public List<KeyValuePair<double, double>> SideLiftCoefficientPoints { get; set; }
        /// <summary>
        /// Reference points for the Side Drag Coefficient for beta in [-Pi, Pi[.
        /// </summary>
        public List<KeyValuePair<double, double>> SideDragCoefficientPoints { get; set; }
        /// <summary>
        /// The thrustcoefficient in relation to the speed.
        /// </summary>
        public List<KeyValuePair<double, double>> ThrustCoeffecientPoints { get; set; }

        /// <summary>
        /// The touchdown point of the landing gear.
        /// </summary>
        public List<Vector3> GearPoints { get; set; }
        /// <summary>
        /// The scale of the geometry points
        /// </summary>
        public float Scale { get; set; }
        /// <summary>
        /// Gets/sets the filename of the engine sound.
        /// </summary>
        public string EngineSound { get; set; }
        /// <summary>
        /// Gets/sets the minimum frequency of the engine.
        /// </summary>
        public int EngineMinFrequency { get; set; }
        /// <summary>
        /// Gets/sets the maximum frequency of the engine.
        /// </summary>
        public int EngineMaxFrequency { get; set; }
        /// <summary>
        /// Gets/sets the responsiveness of the engine.
        /// </summary>
        public double ThrottleDelay { get; set; }
        /// <summary>
        /// Gets/sets the restitution at collisions.
        /// </summary>
        public float Restitution { get; set; }
        /// <summary>
        /// Gets/sets the resistance against crashes.
        /// </summary>
        public double CrashResistence { get; set; }
        /// <summary>
        /// Gets/sets the number of channels.
        /// </summary>
        public int Channels { get; set; }
        /// <summary>
        /// Gets/sets whether the aircraft should be handlaunched.
        /// </summary>
        public bool HandLaunched { get; set; }
        /// <summary>
        /// Gets/sets the fixed part of the mesh.
        /// </summary>
        public string FixedMesh { get; set; }
        /// <summary>
        /// Gets/sets the description of the airplane.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets the type of flightmodel to use.
        /// </summary>
        public FlightModelTypeEnum FlightModelType { get; set; }
        #endregion

        #region Public methods
        /// <summary>
        /// Gets the LiftCoefficient at given angle of attack
        ///         |
        ///         |
        ///         |     /\
        ///         |    /  \
        ///         |   /   \
        ///         |  /     \_____________
        /// --------+-/----------------------
        /// ----\   |/
        ///      \  /
        ///       \/ 
        /// </summary>
        /// <param name="alpha"></param>
        /// <param name="flapsAngle"></param>
        /// <returns></returns>
        public double GetLiftCoefficient(double alpha, double flapsAngle)
        {
            double modAlpha = NormalizeAngle(alpha);
            return Interpolate(LiftCoefficientPoints, modAlpha);
        }

        /// <summary>
        /// Gets the DragCoeffient at given angle of attack
        /// </summary>
        /// <param name="alpha"></param>
        /// <param name="flapsAngle"></param>
        /// <returns></returns>
        public double GetDragCoefficient(double alpha, double flapsAngle)
        {
            double modAlpha = NormalizeAngle(alpha);
            return Interpolate(DragCoefficientPoints, modAlpha);
        }

        /// <summary>
        /// Gets the LiftCoefficient from the vertical area at given sideslip angle.
        /// </summary>
        /// <param name="beta"></param>
        /// <returns></returns>
        public double GetSideLiftCoefficient(double beta)
        {
            double modBeta = NormalizeAngle(beta);
            return Interpolate(SideLiftCoefficientPoints, modBeta);
        }

        /// <summary>
        /// Gets the DragCoefficient from the vertical area at given sideslip angle.
        /// </summary>
        /// <param name="beta"></param>
        /// <returns></returns>
        public double GetSideDragCoefficient(double beta)
        {
            double modBeta = NormalizeAngle(beta);
            return Interpolate(SideDragCoefficientPoints, modBeta);
        }
        #endregion

        #region Private static methods
        private static double Interpolate(List<KeyValuePair<double, double>> knownSamples, double z)
        {
            for (int i = 0; i < knownSamples.Count - 1; i++)
            {
                if ((z >= knownSamples[i].Key) && (z < knownSamples[i + 1].Key))
                {
                    double factor = (z - knownSamples[i].Key) / (knownSamples[i + 1].Key - knownSamples[i].Key);
                    return factor * knownSamples[i + 1].Value + (1 - factor) * knownSamples[i].Value;
                }
            }
            if (z < knownSamples[0].Key)
                return knownSamples[0].Value;
            else
                return knownSamples[knownSamples.Count - 1].Value;
        }

        private static double NormalizeAngle(double angle)
        {
            if (angle < -Math.PI)
            {
                int nPi = (int)((angle + Math.PI) / (2 * Math.PI)) - 1;
                return angle - 2 * nPi * Math.PI;
            }
            else if (angle >= Math.PI)
            {
                int nPi = (int)((angle + Math.PI) / (2 * Math.PI));
                return angle - 2 * nPi * Math.PI;
            }
            else
                return angle;
        }
        #endregion
    }
}
