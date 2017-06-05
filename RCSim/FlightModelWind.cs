using System;
using System.Collections.Generic;
using System.Text;
using RCSim.DataClasses;
using System.Diagnostics;
using Bonsai.Core;
using Microsoft.DirectX;
using Bonsai.Objects.Terrain;
using RCSim.Interfaces;
using System.Threading;

namespace RCSim
{
    internal class FlightModelWind : IAirplaneControl, IDisposable, IFlightModel
    {
        #region Private structs
        private struct CollisionPoint
        {
            public Vector3 ContactPoint;
            public Vector3 Normal;
            public Vector3 NormalW;
            public float Depth;
        };
        #endregion

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

        public Quaternion OrientationQuat;

        /// <summary>
        /// Returns the speed.
        /// </summary>
        public double Speed { get { return Velocity.Length(); } }

        /// <summary>
        /// Gets/sets the velocity in world coordinates.
        /// </summary>
        public Vector3 Velocity
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the Euler angles in world coordinates.
        /// </summary>
        public Vector3 Angles 
        {
            get { return Utility.EulerAnglesFromQuaternion(OrientationQuat); }
        }
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

        public double Lift { get; set; }
        public double Drag { get; set; }
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

        /// <summary>
        /// Gets/sets the flaps deflection [-1, 1].
        /// </summary>
        public double Flaps { get; set; }

        /// <summary>
        /// Gets/sets the gear position [-1, 1].
        /// </summary>
        public double Gear { get; set; }
        #endregion

        #region Wind
        /// <summary>
        /// Gets/Sets the wind vector [m/s]
        /// </summary>
        public Vector3 Wind { get; set; }
        #endregion

        #region Helicopter specific
        /// <summary>
        /// Gets/Sets the RPM of the main rotor.
        /// </summary>
        public float RotorRPM { get; set; }
        /// <summary>
        /// Gets/Sets the relative force the rotor is exercising.
        /// </summary>
        public float RelativeRotorForce { get; set; }
        #endregion

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
        /// Gets whether the flaps have been extended.
        /// </summary>
        public bool FlapsExtended 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Gets whether the gear has been extended.
        /// </summary>
        public bool GearExtended 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Gets the list of collision points in World coordinates.
        /// </summary>
        public List<Vector3> CollisionPoints
        {
            get
            {
                Vector3 position = new Vector3(X, Y, Z);
                List<Vector3> collisionList = new List<Vector3>();
                foreach (Vector3 collisionPoint in AircraftParameters.CollisionPoints)
                {
                    // Check for collisions
                    collisionList.Add(ToDirectX(position + Vector3.TransformCoordinate(collisionPoint, Matrix.RotationQuaternion(OrientationQuat))));
                }
                return collisionList;
            }
        }

        /// <summary>
        /// Gets the list of gear points in World coordinates.
        /// </summary>
        public List<Vector3> GearPoints
        {
            get
            {
                Vector3 position = new Vector3(X, Y, Z);
                List<Vector3> gearList = new List<Vector3>();
                foreach (Vector3 gearPoint in AircraftParameters.GearPoints)
                {
                    // Check for collisions
                    gearList.Add(ToDirectX(position + Vector3.TransformCoordinate(gearPoint, Matrix.RotationQuaternion(OrientationQuat))));
                }
                return gearList;
            }
        }
        #endregion

        #region Private fields
        private double rhoSurface = 0.0;
        private double rhoVerticalSurface = 0.0f;
        private double airSpeedSq = 0.0;
        private double gravityMass = 0.0;

        private Heightmap heightmap = null;

        private float sinceLastTouchdown = 1f;
        private double prevThrottle = 0;

        private float totalSeconds = 0;

        private List<Water> water = new List<Water>();
        #endregion

        #region Private constants
        /// <summary>
        /// The density of air (at sea level and standard pressure).
        /// </summary>
        private const double _AirDensity = 1.2041; // [kg/m3]
        /// <summary>
        /// Gravitational acceleration.
        /// </summary>
        private const double _Gravity = 9.81; // [m/s2]
        #endregion

        #region Public properties
        /// <summary>
        /// Gets/Sets the heightmap.
        /// </summary>
        public Heightmap Heightmap
        {
            get { return heightmap; }
            set { heightmap = value; }
        }

        /// <summary>
        /// Gets/Sets a reference to the water.
        /// </summary>
        public List<Water> Water
        {
            get { return water; }
            set { water = value; }
        }

        /// <summary>
        /// Gets whether the aircraft is on the ground.
        /// </summary>
        public bool TouchedDown
        {
            get
            {
                return (sinceLastTouchdown < 0.5f);
            }
        }

        /// <summary>
        /// Gets whether the aircraft touching water.
        /// </summary>
        public bool OnWater
        {
            get;
            set;
        }
        #endregion

        #region Public Towing proprties
        /// <summary>
        /// Gets/sets the origin point of the towing cable.
        /// </summary>
        public Vector3 CableOrigin
        {
            get;
            set;
        }
        /// <summary>
        /// Gets/sets the velocity of the towing cable.
        /// </summary>
        public Vector3 CableVelocity
        {
            get;
            set;
        }
        /// <summary>
        /// Gets/sets the length of the towing cable.
        /// </summary>
        public float CableLength
        {
            get;
            set;
        }
        /// <summary>
        /// Gets/sets whether the towing cable is currently attached.
        /// </summary>
        public bool CableEnabled
        {
            get;
            set;
        }
        #endregion

        #region DEBUG properties
        public Vector3 DebugPosition { get; set; }
        public Vector3 Debug1 { get; set; }
        public Vector3 Debug2 { get; set; }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Clean up.
        /// </summary>
        public void Dispose()
        {
            StopModel();
        }

        #endregion

        #region Public methods
        public void Initialize()
        {
            rhoSurface = _AirDensity * AircraftParameters.WingArea / 2;
            rhoVerticalSurface = _AirDensity * AircraftParameters.VerticalArea / 2;
            gravityMass = _Gravity * AircraftParameters.Mass;
            OrientationQuat = Quaternion.RotationYawPitchRoll(0.0f, 0.0f, 0.0f);
            GearExtended = true;
            FlapsExtended = false;
            StartModel();
        }

        public void Reset()
        {
            X = 0.0f;
            Y = 0.0f;
            Z = 0.0f;
            OrientationQuat = Quaternion.RotationYawPitchRoll(0f, 0f, (float)Math.PI / 2f);
            //OrientationQuat = Quaternion.RotationYawPitchRoll(0f, 0f, 0f);
            Vx = 0;
            Vy = 0;
            Vz = 0;
            Wx = 0;
            Wy = 0;
            Wz = 0;
            Alpha = 14.0f;
            Velocity = new Vector3(0, 0, 0);
            Crashed = false;
            prevThrottle = 0;
            RotorRPM = 0;
            GearExtended = true;
            Gear = 1;
            FlapsExtended = false;
            Flaps = 0;
            sinceLastTouchdown = 1.0f;
        }

        public void UpdateControls(float elapsedTime)
        {
            double delay = AircraftParameters.ThrottleDelay;
            double targetThrottle = Math.Max(0, (Throttle + 0.97) / 1.97);
            if (AircraftParameters.FlightModelType == AircraftParameters.FlightModelTypeEnum.Helicopter)
                targetThrottle = Throttle;
            if (Crashed)
                Throttle = 0;
            else
                Throttle = Math.Min((1 - delay * elapsedTime) * prevThrottle + elapsedTime * delay * targetThrottle, 1);
            prevThrottle = Math.Min(Throttle, 1.0);
            if ((FlapsExtended) && (Flaps < 0.9999))
                Flaps = Math.Min(1.0, Flaps + elapsedTime / AircraftParameters.FlapsDelay);
            else if ((Flaps > 0.0001) && (!FlapsExtended))
                Flaps = Math.Max(0, Flaps - elapsedTime / AircraftParameters.FlapsDelay);
            if ((GearExtended) && (Gear < 0.9999))
                Gear = Math.Min(1.0, Gear + elapsedTime / AircraftParameters.GearDelay);
            else if ((Gear > 0.0001) && (!GearExtended))
                Gear = Math.Max(0, Gear - elapsedTime / AircraftParameters.GearDelay);
        }

        /// <summary>
        /// Handlaunches an airplane.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public void HandLaunch(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
            double dir = Math.PI - (Math.PI / 2f + Program.Instance.Weather.Wind.CurrentDirection);
            OrientationQuat = Quaternion.RotationYawPitchRoll(0f, 0f, (float)(dir));
            Vx = 15;
            Velocity = new Vector3((float)Math.Cos(dir) * 10f, (float)Math.Sin(dir) * 10f, -2);
        }

        /// <summary>
        /// Updates the semi-constants after a change in Wingarea/Verticalarea/Mass.
        /// </summary>
        public void UpdateConstants()
        {
            rhoSurface = _AirDensity * AircraftParameters.WingArea / 2;
            rhoVerticalSurface = _AirDensity * AircraftParameters.VerticalArea / 2;
            gravityMass = _Gravity * AircraftParameters.Mass;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elapsedTime"></param>
        public void MoveScene(float elapsedTime)
        {
            lock (this)
            {
                //elapsedTime *= 0.5f;
                //if ((elapsedTime > 0.5f) || Crashed || Paused)
                if ((elapsedTime > 0.5f) || Paused)
                    return;
                float elapsedSeconds = elapsedTime;

                // Update the wind.
                totalSeconds += elapsedSeconds;
                Program.Instance.Weather.Wind.WindTime = totalSeconds;

                // Update the airspeed
                UpdateAirspeed();

                // Convert the wind vector to plane coordinates
                Vector3 wind = Vector3.TransformCoordinate(ToModel(-Wind), Matrix.RotationQuaternion(Quaternion.Invert(OrientationQuat)));

                if (Math.Abs(Vx + wind.X) < 0.001)
                {
                    Alpha = 0;
                    Beta = 0;
                }
                else
                {
                    Alpha = Math.Atan2(Vz + wind.Z, Vx + wind.X);
                    Beta = -Math.Atan2(Vy + wind.Y, Vx + wind.X);
                }

                // Calculate all forces on the airframe.
                if (AircraftParameters.FlightModelType == AircraftParameters.FlightModelTypeEnum.Aircraft)
                    CalculateForces(elapsedSeconds, wind);
                else if (AircraftParameters.FlightModelType == AircraftParameters.FlightModelTypeEnum.Helicopter)
                    CalculateForcesHelicopter(elapsedSeconds, wind);
                else if (AircraftParameters.FlightModelType == AircraftParameters.FlightModelTypeEnum.HelicopterCoax)
                    CalculateForcesCoaxHelicopter(elapsedSeconds, wind);                    
                else
                    CalculateForcesSailboat(elapsedSeconds, wind);

                Ax = (float)(Fx / AircraftParameters.Mass);
                Ay = (float)(Fy / AircraftParameters.Mass);
                Az = (float)(Fz / AircraftParameters.Mass);

                float velX0 = Velocity.X; float velY0 = Velocity.Y; float velZ0 = Velocity.Z;
                Vector3 dVelocity = new Vector3(Ax * elapsedSeconds, Ay * elapsedSeconds, Az * elapsedSeconds);
                dVelocity.TransformCoordinate(Matrix.RotationQuaternion(OrientationQuat));
                //Vector3 dVelocity = new Vector3(Ax * elapsedSeconds, Ay * elapsedSeconds, Az * elapsedSeconds).TransformCoordinate(Matrix.RotationQuaternion(OrientationQuat));
                Velocity += dVelocity;

                // collision response
                //UpdateCollisions2(elapsedSeconds);

                Vector3 planeVelocity = new Vector3(Velocity.X, Velocity.Y, Velocity.Z);
                planeVelocity.TransformCoordinate(Matrix.RotationQuaternion(Quaternion.Invert(OrientationQuat)));
                Vx = planeVelocity.X;
                Vy = planeVelocity.Y;
                Vz = planeVelocity.Z;
                
                // Calculate all torques on the airframe
                if (AircraftParameters.FlightModelType == AircraftParameters.FlightModelTypeEnum.Aircraft)
                    CalculateTorques(elapsedSeconds, wind);
                else if (AircraftParameters.FlightModelType == AircraftParameters.FlightModelTypeEnum.Helicopter)
                    CalculateTorquesHelicopter(elapsedSeconds, wind);
                else if (AircraftParameters.FlightModelType == AircraftParameters.FlightModelTypeEnum.HelicopterCoax)
                    CalculateTorquesCoaxHelicopter(elapsedSeconds, wind);                    
                else
                    CalculateTorquesSailboat(elapsedSeconds, wind);

                AAx = (float)(Tx / AircraftParameters.Ixx);
                AAy = (float)(Ty / AircraftParameters.Iyy);
                AAz = (float)(Tz / AircraftParameters.Izz);
                                
                if (TouchedDown && (Vx < 10f))
                {
                    if (Vx > 5f)
                        AAz += (float)Rudder * (10f - Vx) * (20f/(float)AircraftParameters.Mass);
                    else
                        AAz += (float)(Rudder * Vx - Wz) * (20f /(float)AircraftParameters.Mass);
                }                

                // Integrate
                float Wx0 = Wx; float Wy0 = Wy; float Wz0 = Wz;
                Wx += AAx * elapsedSeconds;
                Wy += AAy * elapsedSeconds;
                Wz += AAz * elapsedSeconds;

                /*
                if ((Math.Abs(Wx) > 10) && (Math.Abs(Wx) > Math.Abs(Wx0)) && (Math.Sign(Wx) != Math.Sign(Wx0)))
                    Wx = -Wx0;
                if ((Math.Abs(Wy) > 10) && (Math.Abs(Wy) > Math.Abs(Wy0)) && (Math.Sign(Wy) != Math.Sign(Wy0)))
                    Wy = -Wy0;
                if (Math.Abs(Wz) > 10)
                {
                    if ((Math.Abs(Wz) > Math.Abs(Wz0)) && (Math.Sign(Wz) != Math.Sign(Wz0)))
                        Wz = -Wz0;
                }
                 */

                if ((planeVelocity.LengthSq() > 40000) || (Wx > 100) || (Wy > 100) || (Wz > 100))
                {
                    // Flight model out of control
                    Reset();
                }

                // Update location
                Vector3 dLoc = new Vector3(
                   velX0 * elapsedSeconds + dVelocity.X * elapsedSeconds * 0.5f,
                   velY0 * elapsedSeconds + dVelocity.Y * elapsedSeconds * 0.5f,
                   velZ0 * elapsedSeconds + dVelocity.Z * elapsedSeconds * 0.5f);

                X += dLoc.X;
                Y += dLoc.Y;
                Z += dLoc.Z;

                // collision response
                UpdateCollisions2(elapsedSeconds);

                // cable response
                if (CableEnabled)
                    UpdateCable(elapsedSeconds);

                // Recoordinate the orientation
                Quaternion newOrientation = new Quaternion(
                        OrientationQuat.X + (OrientationQuat.W * Wx + OrientationQuat.Y * Wz - OrientationQuat.Z * Wy) * 0.5f * elapsedSeconds,
                        OrientationQuat.Y + (OrientationQuat.W * Wy + OrientationQuat.Z * Wx - OrientationQuat.X * Wz) * 0.5f * elapsedSeconds,
                        OrientationQuat.Z + (OrientationQuat.W * Wz + OrientationQuat.X * Wy - OrientationQuat.Y * Wx) * 0.5f * elapsedSeconds,
                        OrientationQuat.W - (OrientationQuat.X * Wx + OrientationQuat.Y * Wy + OrientationQuat.Z * Wz) * 0.5f * elapsedSeconds
                        );
                //OrientationQuat += OrientationQuat * (new Vector3(Wx, Wy, Wz)) * 0.5f * elapsedSeconds;
                newOrientation.Normalize();
                OrientationQuat = newOrientation;
                Vector3 ypr = Utility.EulerAnglesFromQuaternion(OrientationQuat);
                Roll = ypr.X;
                Pitch = ypr.Y;
                Yaw = ypr.Z;
            }            
        }
        #endregion

        #region Private methods
        private void UpdateAirspeed()
        {
            airSpeedSq = Vx * Vx + Vy * Vy + Vz * Vz;
        }

        protected double LiftLeft { get; set; }
        protected double LiftRight { get; set; }

        /// <summary>
        /// Calculates the total forces on the airframe.
        /// </summary>
        /// <param name="elapsedTime"></param>
        private void CalculateForces(float elapsedTime, Vector3 wind)
        {
            
            Drag = GetDragForce(wind);
            Lift = GetLiftForce(wind);
            double sideLift = GetSideLiftForce(wind);
            double sideDrag = GetSideDragForce(wind);
            double sideGroundResistance = 0;
            double groundResistance = 0;
            // verified : gravity, liftX, throttle, liftZ 
            if (TouchedDown)
            {
                if ((Vx < 10f) && (Vx > 0.01))
                {
                    double realBeta = -Math.Atan2(Vy, Vx);
                    sideGroundResistance = Math.Sin(realBeta) * gravityMass;
                }
                //groundResistance = Vx/10.0;
            }

            Fx = (float)(+sideLift * Math.Sin(Beta) 
                         - sideDrag * Math.Cos(Beta) 
                         - Lift * Math.Sin(Alpha) 
                         - Drag * Math.Cos(Alpha) 
                         + AircraftParameters.MaximumThrust * Throttle * AircraftParameters.GetThrustCoefficient((double)Vx)
                         - gravityMass * Math.Sin(Pitch)
                         - groundResistance);
            Fy = (float)(+sideLift * Math.Cos(Beta) + sideDrag * Math.Sin(Beta) + gravityMass * Math.Sin(Roll) * Math.Cos(Pitch) + sideGroundResistance);
            Fz = (float)(-Lift * Math.Cos(Alpha) - Drag * Math.Sin(Alpha) + gravityMass * Math.Cos(Roll) * Math.Cos(Pitch));
        }

        /// <summary>
        /// Calculates the total forces on the airframe for helicopters.
        /// </summary>
        /// <param name="elapsedTime"></param>
        private void CalculateForcesHelicopter(float elapsedTime, Vector3 wind)
        {            
            double sideGroundResistance = 0;
            double groundResistance = 0;
            // verified : gravity, liftX, throttle, liftZ 
            if (TouchedDown)
            {
                if ((Vx < 10f) && (Vx > 0.01))
                {
                    double realBeta = -Math.Atan2(Vy, Vx);
                    sideGroundResistance = Math.Sin(realBeta) * gravityMass;
                }
                groundResistance = AircraftParameters.GroundDrag * Vx;
            }

            // Ground effect
            float groundEffect = 0;
            if (heightmap != null)
            {
                float height = heightmap.GetHeightAt(-Y, -X);
                float altitude = -Z - height;
                if (altitude < 2)
                {
                    groundEffect = -(float) Throttle * (float) AircraftParameters.MaximumThrust * (2 - altitude) / 4;
                }
            }
            float vairX = Vx + wind.X;
            float vairY = Vy + wind.Y;
            float vairZ = Vz + wind.Z;

            double dragX = AircraftParameters.FuselageDragX * vairX * vairX;
            double dragY = AircraftParameters.FuselageDragY * vairY * vairY;
            double dragZ = AircraftParameters.FuselageDragZ * vairZ * vairZ;
            
            float defaultRPM = 1000;
            float Frotor = (float)(((float)Throttle + 0.1 * vairZ) * (RotorRPM / defaultRPM) * AircraftParameters.MaximumThrust);
            //float Frotor = (Throttle * AircraftParameters.MaximumThrust * (RotorRPM / 1000));
            Fx = (float)((groundEffect - gravityMass) * Math.Sin(Pitch) 
                          - Frotor*Math.Sin(Elevator*0.1f)
                          - Math.Sign(vairX) * dragX
                          - groundResistance);
            Fy = (float)((groundEffect + gravityMass) * Math.Sin(Roll) * Math.Cos(Pitch)
                          + Frotor*Math.Sin(Ailerons*0.1f)
                          - Math.Sign(vairY) * dragY
                          + sideGroundResistance);
            Fz = (float)((groundEffect + gravityMass) * Math.Cos(Roll) * Math.Cos(Pitch)
                          - Frotor*Math.Cos(Elevator*0.1f)*Math.Cos(Ailerons*0.1f)
                          - Math.Sign(vairZ) * dragZ);

            // Update the rotor RPM
            float rotorSpeed = RotorRPM / defaultRPM;
            float drag = Math.Abs(Frotor) / (float)AircraftParameters.MaximumThrust + rotorSpeed * rotorSpeed;
            RelativeRotorForce = Math.Abs(Frotor) / (float)AircraftParameters.MaximumThrust;
            if (RotorRPM < 200)
                RotorRPM += elapsedTime * (RotorRPM + 1);
            else
                RotorRPM += (2 - drag) * 1500 * elapsedTime;
        }

        /// <summary>
        /// Calculates the total forces on the airframe for coaxial helicopters.
        /// </summary>
        /// <param name="elapsedTime"></param>
        private void CalculateForcesCoaxHelicopter(float elapsedTime, Vector3 wind)
        {
            double sideGroundResistance = 0;
            double groundResistance = 0;
            // verified : gravity, liftX, throttle, liftZ 
            if (TouchedDown)
            {
                if ((Vx < 10f) && (Vx > 0.01))
                {
                    double realBeta = -Math.Atan2(Vy, Vx);
                    sideGroundResistance = Math.Sin(realBeta) * gravityMass;
                }
                groundResistance = AircraftParameters.GroundDrag * Vx;
            }

            // Ground effect
            float groundEffect = 0;
            if (heightmap != null)
            {
                float height = heightmap.GetHeightAt(-Y, -X);
                float altitude = -Z - height;
                if (altitude < 2)
                {
                    groundEffect = -(float)Throttle * (float)AircraftParameters.MaximumThrust * (2 - altitude) / 4;
                }
            }
            float vairX = Vx + wind.X;
            float vairY = Vy + wind.Y;
            float vairZ = Vz + wind.Z;

            double dragX = AircraftParameters.FuselageDragX * vairX * vairX;
            double dragY = AircraftParameters.FuselageDragY * vairY * vairY;
            double dragZ = AircraftParameters.FuselageDragZ * vairZ * vairZ;

            float defaultRPM = 1000;
            float Frotor = (float)(((float)Throttle + 0.1 * vairZ) * (RotorRPM / defaultRPM) * AircraftParameters.MaximumThrust);
            //float Frotor = (Throttle * AircraftParameters.MaximumThrust * (RotorRPM / 1000));
            Fx = (float)((groundEffect - gravityMass) * Math.Sin(Pitch)
                          - Frotor * Math.Sin(Elevator * 0.1f)
                          - Math.Sign(vairX) * dragX
                          - groundResistance);
            Fy = (float)((groundEffect + gravityMass) * Math.Sin(Roll) * Math.Cos(Pitch)
                          + Frotor * Math.Sin(Ailerons * 0.1f)
                          - Math.Sign(vairY) * dragY
                          + sideGroundResistance);
            Fz = (float)((groundEffect + gravityMass) * Math.Cos(Roll) * Math.Cos(Pitch)
                          - Frotor * Math.Cos(Elevator * 0.1f) * Math.Cos(Ailerons * 0.1f)
                          - Math.Sign(vairZ) * dragZ);

            // Update the rotor RPM
            float rotorSpeed = RotorRPM / defaultRPM;
            float drag = Math.Abs(Frotor) / (float)AircraftParameters.MaximumThrust + rotorSpeed * rotorSpeed;
            RelativeRotorForce = Math.Abs(Frotor) / (float)AircraftParameters.MaximumThrust;
            RotorRPM += (2 - drag) * 1500 * elapsedTime;
        }

        /// <summary>
        /// Calculates the total forces on the airframe.
        /// </summary>
        /// <param name="elapsedTime"></param>
        private void CalculateForcesSailboat(float elapsedTime, Vector3 wind)
        {
            Beta += Ailerons;
            Drag = 0;
            Lift = 0;
            double sideLift = GetSideLiftForce(wind);
            double sideDrag = GetSideDragForce(wind);
            double sideGroundResistance = 0;
            double groundResistance = 0;
            // verified : gravity, liftX, throttle, liftZ 
            double realBeta = -Math.Atan2(Vy, Vx);
            sideGroundResistance = Math.Sin(realBeta) * gravityMass;
  
            Fx = (float)(+sideLift * Math.Sin(Beta)
                         - sideDrag * Math.Cos(Beta)
                         - gravityMass * Math.Sin(Pitch)
                         - groundResistance);
            Fy = (float)(+sideLift * Math.Cos(Beta) + sideDrag * Math.Sin(Beta) + gravityMass * Math.Sin(Roll) * Math.Cos(Pitch) + sideGroundResistance);
            Fz = (float)(gravityMass * Math.Cos(Roll) * Math.Cos(Pitch));
        }

        /// <summary>
        /// Calculates the lift force.
        /// </summary>
        /// <returns></returns>
        private double GetLiftForce(Vector3 wind)
        {
            // F = (1/2)*Cl*V^2*S
            if (AircraftParameters.HasFlaps)
            {
                double noFlaps = AircraftParameters.GetLiftCoefficient(Alpha);
                double withFlaps = AircraftParameters.GetLiftCoefficientWithFlaps(Alpha);
                return (Flaps * withFlaps + (1 - Flaps) * noFlaps) * ((Vx + wind.X) * (Vx + wind.X) + (Vz + wind.Z) * (Vz + wind.Z)) * rhoSurface; 
            }
            else
                return AircraftParameters.GetLiftCoefficient(Alpha) * ((Vx + wind.X) * (Vx + wind.X) + (Vz + wind.Z) * (Vz + wind.Z)) * rhoSurface;
        }
              

        /// <summary>
        /// Calculates the drag force.
        /// </summary>
        /// <returns></returns>
        private double GetDragForce(Vector3 wind)
        {
            // F = (1/2)*Dc*V^2*S
            if (AircraftParameters.HasFlaps)
            {
                double noFlaps = AircraftParameters.GetDragCoefficient(Alpha);
                double withFlaps = AircraftParameters.GetDragCoefficientWithFlaps(Alpha);
                return (Flaps * withFlaps + (1 - Flaps) * noFlaps) * ((Vx + wind.X) * (Vx + wind.X) + (Vz + wind.Z) * (Vz + wind.Z)) * rhoSurface;
            }
            else
                return AircraftParameters.GetDragCoefficient(Alpha) * ((Vx + wind.X) * (Vx + wind.X) + (Vz + wind.Z) * (Vz + wind.Z)) * rhoSurface;
        }

        /// <summary>
        /// Calculates the lift force generated by the fuselage and vertical tail.
        /// </summary>
        /// <returns></returns>
        private double GetSideLiftForce(Vector3 wind)
        {
            return AircraftParameters.GetSideLiftCoefficient(Beta) * ((Vx + wind.X) * (Vx + wind.X) + (Vy + wind.Y) * (Vy + wind.Y)) * rhoVerticalSurface;
        }

        /// <summary>
        /// Calculates the drag force generated by the fuselage and vertical tail.
        /// </summary>
        /// <returns></returns>
        private double GetSideDragForce(Vector3 wind)
        {
            return AircraftParameters.GetSideDragCoefficient(Beta) * ((Vx + wind.X) * (Vx + wind.X) + (Vy + wind.Y) * (Vy + wind.Y)) * rhoVerticalSurface;
        }

        /// <summary>
        /// Calculate the torques along the different axis of the airframe.
        /// </summary>
        /// <param name="elapsedTime"></param>
        private void CalculateTorques(float elapsedTime, Vector3 wind)
        {
            // Torque = F * r * sin(alpha)
            // TODO differential lift
            float airspeedX = Vx + wind.X; 
            float airspeedY = Vy + wind.Y;
            float airspeedZ = Vz + wind.Z;
            float airspeedXY = (float)Math.Sqrt(airspeedX * airspeedX + airspeedY * airspeedY);
            float airspeedXZ = (float)Math.Sqrt(airspeedX * airspeedX + airspeedZ * airspeedZ);
            Tx = (float)((-AircraftParameters.RollDamping * Math.Sign(airspeedX) * Wx +
                           AircraftParameters.AileronEfficiency * airspeedX * Ailerons +
                           AircraftParameters.DihedralAngle * AircraftParameters.DihedralEfficiency * airspeedX * Beta) * airspeedX +
                           (Lift * AircraftParameters.SpinFactor * Wz) / (Math.Abs(airspeedX) + 1.0f) +
                           AircraftParameters.AileronEfficiency * AircraftParameters.PropWashAilerons * Throttle * Ailerons);
            Ty = (float)((-AircraftParameters.PitchDamping * Wy -
                           Math.Sin(Alpha) * AircraftParameters.PitchStability +
                           AircraftParameters.ElevatorEfficiency * airspeedX * Elevator + AircraftParameters.PitchTrim) * airspeedXY +
                           AircraftParameters.CenterOfGravity * Lift + 
                           AircraftParameters.ElevatorEfficiency * AircraftParameters.PropWashElevator * Throttle * Elevator);
            Tz = (float)((-AircraftParameters.YawDamping *  Wz -
                           Math.Sin(Beta) * Math.Sign(airspeedXZ) * AircraftParameters.YawStability +
                           AircraftParameters.RudderEfficiency * airspeedX * Rudder) * airspeedXZ +
                           AircraftParameters.RudderEfficiency * AircraftParameters.PropWashRudder * Throttle* Rudder);
        }

        /// <summary>
        /// Calculate the torques along the different axis of the airframe for a helicopter.
        /// </summary>
        /// <param name="elapsedTime"></param>
        private void CalculateTorquesHelicopter(float elapsedTime, Vector3 wind)
        {
            // Torque = F * r * sin(alpha)
            float airspeedX = Vx + wind.X;
            float airspeedY = Vy + wind.Y;
            float airspeedZ = Vz + wind.Z;
            float airspeedXY = (float)Math.Sqrt(airspeedX * airspeedX + airspeedY * airspeedY);
            float airspeedXZ = (float)Math.Sqrt(airspeedX * airspeedX + airspeedZ * airspeedZ);

            Tx = (float)(-AircraftParameters.RollDamping * Wx
                         - AircraftParameters.DihedralEfficiency * Roll +
                           AircraftParameters.AileronEfficiency * Ailerons * RelativeRotorForce);
            Ty = (float)(-AircraftParameters.PitchDamping * Wy +
                         -AircraftParameters.PitchStability * Pitch +
                           AircraftParameters.ElevatorEfficiency * Elevator * RelativeRotorForce);
            Tz = (float)(-AircraftParameters.YawDamping * Wz -
                         AircraftParameters.YawStability * Math.Sin(Beta)* airspeedXY +
                            AircraftParameters.RudderEfficiency * Rudder * Math.Abs(Throttle));

        }

        /// <summary>
        /// Calculate the torques along the different axis of the airframe for a coaxial helicopter.
        /// </summary>
        /// <param name="elapsedTime"></param>
        private void CalculateTorquesCoaxHelicopter(float elapsedTime, Vector3 wind)
        {
            // Torque = F * r * sin(alpha)
            float airspeedX = Vx + wind.X;
            float airspeedY = Vy + wind.Y;
            float airspeedZ = Vz + wind.Z;
            float airspeedXY = (float)Math.Sqrt(airspeedX * airspeedX + airspeedY * airspeedY);
            float airspeedXZ = (float)Math.Sqrt(airspeedX * airspeedX + airspeedZ * airspeedZ);

            Vector3 gravityPlane = ToWorldCoords(new Vector3(0, 0, (float)gravityMass)); // ToWorldCoords and ToPlaneCoords should be inverted.

            Tx = (float)(-AircraftParameters.RollDamping * Wx
                         + AircraftParameters.AileronEfficiency * Ailerons * RelativeRotorForce
                         - gravityPlane.Y * 1.0);
            Ty = (float)(- AircraftParameters.PitchDamping * Wy 
                         + AircraftParameters.ElevatorEfficiency * Elevator * RelativeRotorForce 
                         + gravityPlane.X * 1.0);
            Tz = (float)(-AircraftParameters.YawDamping * Wz -
                         AircraftParameters.YawStability * Math.Sin(Beta) * airspeedXY +
                            AircraftParameters.RudderEfficiency * Rudder * Math.Abs(Throttle));

        }

        /// <summary>
        /// Calculate the torques along the different axis of the airframe.
        /// </summary>
        /// <param name="elapsedTime"></param>
        private void CalculateTorquesSailboat(float elapsedTime, Vector3 wind)
        {
            // Torque = F * r * sin(alpha)
            // TODO differential lift
            float airspeedX = Vx + wind.X; 
            float airspeedY = Vy + wind.Y;
            float airspeedZ = Vz + wind.Z;
            float airspeedXY = (float)Math.Sqrt(airspeedX * airspeedX + airspeedY * airspeedY);
            float airspeedXZ = (float)Math.Sqrt(airspeedX * airspeedX + airspeedZ * airspeedZ);

            Vector3 gravityPlane = ToWorldCoords(new Vector3(0, 0, (float)gravityMass)); // ToWorldCoords and ToPlaneCoords should be inverted.

            Tx = (float)((-AircraftParameters.RollDamping * Math.Sign(airspeedX) * Wx +
                           AircraftParameters.DihedralAngle * AircraftParameters.DihedralEfficiency * airspeedX * Beta) * airspeedX
                           + gravityPlane.Y * 1.0);
            Ty = (float)((-AircraftParameters.PitchDamping * Wy -
                           Math.Sin(Alpha) * AircraftParameters.PitchStability) * airspeedXY);
            Tz = (float)((-AircraftParameters.YawDamping *  Wz -
                           Math.Sin(Beta) * Math.Sign(airspeedXZ) * AircraftParameters.YawStability +
                           AircraftParameters.RudderEfficiency * airspeedX * Rudder) * airspeedXZ);                           
        }

        private Vector3 ToWorldCoords(Vector3 vector)
        {
            return Vector3.TransformCoordinate(vector, Matrix.RotationQuaternion(Quaternion.Invert(OrientationQuat)));
        }

        private Vector3 ToPlaneCoords(Vector3 vector)
        {
            return Vector3.TransformCoordinate(vector, Matrix.RotationQuaternion(OrientationQuat));
        }

        private Matrix InertiaInvertInWorldCoords()
        {
            Matrix m = new Matrix();
            m.M11 = (float)AircraftParameters.Ixx;
            m.M22 = (float)AircraftParameters.Iyy;
            m.M33 = (float)AircraftParameters.Izz;
            m.M44 = 1;
            m.Invert();
            
            Matrix r = Matrix.RotationQuaternion(OrientationQuat);
            Matrix rt = Matrix.TransposeMatrix(r);
            Matrix result = r * m * rt;
            return result;
        }

        private Vector3 MultiplyInertiaInverse2(Vector3 vector)
        {
            Matrix m = InertiaInvertInWorldCoords();

            return new Vector3(
                vector.X * m.M11 + vector.Y * m.M21 + vector.Z * m.M31,
                vector.X * m.M12 + vector.Y * m.M22 + vector.Z * m.M32,
                vector.X * m.M13 + vector.Y * m.M23 + vector.Z * m.M33);
        }


        private void UpdateCable(float elapsedTime)
        {
            Vector3 distVector = CableOrigin - new Vector3(X, Y, Z);
            float tension = distVector.Length() - CableLength;
            if (tension < -0.05f)
                return; // no tension on the cable
            if (tension > 0.05f)
                tension = 0.05f;
            distVector.Normalize();
            float convergence = Vector3.Dot(distVector, Velocity);
            if (convergence < 0)
            {
                // diverging
                Velocity = CableVelocity.Length()*distVector;
            }
            else if (convergence < CableVelocity.Length())
            {
                // converging slower than the towplane is flying
                if (tension < 0)
                    Velocity = (0.05f + tension)*20f*CableVelocity.Length() * distVector +
                        tension*20f*Velocity;
                else
                    Velocity = CableVelocity.Length() * distVector;
            }            
        }

        private void UpdateCollisions2(float elapsedTime)
        {
            //Vector3 velocityW = ToWorldCoords(Velocity);
            Vector3 rotVelW = ToPlaneCoords(new Vector3(Wx, Wy, Wz));
            Vector3 positionW = new Vector3(X, Y, Z);
            //this.Debug1 = ToDirectX(positionW + rotVelW);

            UpdateWaterCollisions(elapsedTime, rotVelW, positionW);

            List<CollisionPoint> contactList = new List<CollisionPoint>();

            double crashRes = AircraftParameters.CrashResistanceGear;
            // Check the gear points
            if (GearExtended)
            {
                foreach (Vector3 gearPoint in AircraftParameters.GearPoints)
                {
                    // Check for collisions
                    Vector3 gearPointW = positionW + Vector3.TransformCoordinate(gearPoint, Matrix.RotationQuaternion(OrientationQuat));
                    Vector3 normalW;
                    float depth = 0f;
                    if (IsColliding(gearPointW, out normalW, out depth))
                    {
                        Vector3 normal = Vector3.TransformCoordinate(ToModel(normalW), Matrix.RotationQuaternion(Quaternion.Invert(OrientationQuat)));
                        CollisionPoint point = new CollisionPoint();
                        point.ContactPoint = Vector3.TransformCoordinate(gearPoint, Matrix.RotationQuaternion(OrientationQuat));
                        point.Normal = normal;
                        point.NormalW = ToModel(normalW);
                        point.Depth = depth;
                        contactList.Add(point);
                    }
                }
            }
            // Check the collision points
            foreach (Vector3 collisionPoint in AircraftParameters.CollisionPoints)
            {
                // Check for collisions
                Vector3 collisionPointW = positionW + Vector3.TransformCoordinate(collisionPoint, Matrix.RotationQuaternion(OrientationQuat));
                Vector3 normalW;
                float depth = 0f;
                if (IsColliding(collisionPointW, out normalW, out depth))
                {
                    Vector3 normal = Vector3.TransformCoordinate(ToModel(normalW), Matrix.RotationQuaternion(Quaternion.Invert(OrientationQuat)));
                    CollisionPoint point = new CollisionPoint();
                    point.ContactPoint = Vector3.TransformCoordinate(collisionPoint, Matrix.RotationQuaternion(OrientationQuat));
                    point.Normal = normal;
                    point.NormalW = ToModel(normalW);
                    point.Depth = depth;
                    contactList.Add(point);
                    crashRes = AircraftParameters.CrashResistance;
                }
            }

            if (contactList.Count > 0)
            {
                Vector3 contactPoint;
                Vector3 normal;
                Vector3 normalW;
                float depth;
                if (contactList.Count == 1)
                {
                    // vertex
                    contactPoint = contactList[0].ContactPoint;
                    normal = contactList[0].Normal;
                    normalW = contactList[0].NormalW;
                    depth = contactList[0].Depth;
                }
                else if (contactList.Count == 2)
                {
                    // segment
                    float b = 0;
                    contactPoint = PointToSegment(new Vector3(), contactList[0].ContactPoint, contactList[1].ContactPoint, out b);
                    //contactPoint = (0.5f * (contactList[0].ContactPoint + contactList[1].ContactPoint));
                    normal = (b * contactList[0].Normal + (1 - b) * contactList[1].Normal);
                    normal.Normalize();
                    normalW = (b * contactList[0].NormalW + (1 - b) * contactList[1].NormalW);
                    normalW.Normalize();
                    depth = (b * contactList[0].Depth + (1 - b) * contactList[1].Depth);
                }
                else
                {
                    // plane
                    normal = new Vector3();
                    normalW = new Vector3();
                    depth = 0;
                    contactPoint = new Vector3(0, 0, 0);
                    foreach (CollisionPoint collisionPoint in contactList)
                    {
                        normal += collisionPoint.Normal;
                        normalW += collisionPoint.NormalW;
                        depth += collisionPoint.Depth;
                        contactPoint += collisionPoint.ContactPoint;
                    }
                   
                    normal *= (1f / contactList.Count);
                    normal.Normalize();
                    normalW *= (1f / contactList.Count);
                    normalW.Normalize();
                    depth *= (1f / contactList.Count);

                    Plane plane = Plane.FromPoints(contactList[0].ContactPoint, contactList[1].ContactPoint, contactList[2].ContactPoint);
                    //Vector3 planeNormal = Vector3.Cross(contactList[1].ContactPoint - contactList[0].ContactPoint,
                    //    contactList[2].ContactPoint - contactList[0].ContactPoint);
                    //planeNormal.Normalize();
                    plane.Normalize();
                    float distance = plane.D;
                    contactPoint = -distance * normalW;
                    //contactPoint = -contactList[0].ContactPoint.Z * normal;//-depth * normal;
                }
                
                float epsilon = AircraftParameters.Restitution;
                Vector3 contactVel = Velocity + Vector3.Cross(rotVelW, contactPoint);
                float jpoint = ((- 1 - epsilon) * Vector3.Dot(contactVel, normalW)) /
                    ((1.0f / (float)AircraftParameters.Mass) +
                    Vector3.Dot(normalW, Vector3.Cross(MultiplyInertiaInverse2(Vector3.Cross(contactPoint, normalW)), contactPoint)));
                float jcg = ((- 1 - epsilon) * Vector3.Dot(Velocity, normalW)) /
                    ((1.0f / (float)AircraftParameters.Mass));

                if (Math.Abs(jpoint) > crashRes)
                    Crashed = true;

                if (depth > .002)
                    Z -= depth/10;
                
                if (Vector3.Dot(normalW, contactVel) < 0)
                {
                    Vector3 colTangent = Vector3.Cross(Vector3.Cross(normalW, contactVel), normalW);
                    colTangent.Normalize();
                    //this.DebugPosition = ToDirectX(positionW + contactPoint);
                    //this.Debug1 = ToDirectX(positionW + colTangent);
                    float vrt = Vector3.Dot(contactVel, colTangent);
                    Vector3 rotationVelDiffW;
                    if (vrt > 0)
                    {
                        Velocity += (jcg / (float)AircraftParameters.Mass) * normalW
                            - ((jcg * AircraftParameters.GroundDrag / (float)AircraftParameters.Mass) * colTangent);
                        rotationVelDiffW = MultiplyInertiaInverse2(
                            Vector3.Cross(contactPoint, (jpoint * normalW - jcg * AircraftParameters.GroundDrag * colTangent)));
                    }
                    else
                    {
                        Velocity += (jcg / (float)AircraftParameters.Mass) * normalW;
                        rotationVelDiffW = MultiplyInertiaInverse2(Vector3.Cross(contactPoint, jpoint * normalW));
                    }
                    if (Velocity.LengthSq() < 2f)
                    {
                        if (elapsedTime < 0.2f)
                        {
                            if (Math.Sign(rotVelW.X) != Math.Sign(rotationVelDiffW.X))
                                rotVelW.X += rotationVelDiffW.X;
                            else
                                rotVelW.X += 0.5f * rotationVelDiffW.X;
                            if (Math.Sign(rotVelW.Y) != Math.Sign(rotationVelDiffW.Y))
                                rotVelW.Y += rotationVelDiffW.Y;
                            else
                                rotVelW.Y += 0.5f * rotationVelDiffW.Y;
                            if (Math.Sign(rotVelW.Z) != Math.Sign(rotationVelDiffW.Z))
                                rotVelW.Z += rotationVelDiffW.Z;
                            else
                                rotVelW.Z += 0.5f * rotationVelDiffW.Z;
                        }
                    }
                    else
                        rotVelW += rotationVelDiffW;
                    //+ (jpoint*0.3f*colTangent));
                    //Vector3 rotationVelDiffW = MultiplyInertiaInverse2(j * normalW);
                    
                    Vector3 newRotVels = ToWorldCoords(rotVelW);
                    Wx = newRotVels.X; Wy = newRotVels.Y; Wz = newRotVels.Z;
                }
                
                sinceLastTouchdown = 0.0f;
            }
            sinceLastTouchdown += elapsedTime;
        }


        private void UpdateCollisions(float elapsedTime)
        {
            //Framework.Instance.DebugString += "\nvel: " + Velocity;
            Vector3 planeVelocity = new Vector3(Velocity.X, Velocity.Y, Velocity.Z);
            Vector3 rotationalVelocity = new Vector3(Wx, Wy, Wz);
            planeVelocity.TransformCoordinate(Matrix.RotationQuaternion(Quaternion.Invert(OrientationQuat)));
            //Vx = planeVelocity.X;
            //Vy = planeVelocity.Y;
            //Vz = planeVelocity.Z;                        
            Vector3 position = new Vector3(X, Y, Z);

            List<CollisionPoint> contactList = new List<CollisionPoint>();
            foreach (Vector3 gearPoint in AircraftParameters.GearPoints)
            {
                // Check for collisions
                Vector3 gearPointW = position + Vector3.TransformCoordinate(gearPoint, Matrix.RotationQuaternion(OrientationQuat));
                Vector3 normalW;
                float depth = 0f;
                if (IsColliding(gearPointW, out normalW, out depth))
                {
                    Vector3 normal = Vector3.TransformCoordinate(ToModel(normalW), Matrix.RotationQuaternion(Quaternion.Invert(OrientationQuat)));
                    CollisionPoint point = new CollisionPoint();
                    point.ContactPoint = gearPoint;
                    point.Normal = normal;
                    point.NormalW = ToModel(normalW);
                    point.Depth = depth;
                    contactList.Add(point);
                }
            }
            if (contactList.Count > 0)
            {
                Vector3 contactPoint;
                Vector3 normal;
                Vector3 normalW;
                float depth;
                if (contactList.Count == 1)
                {
                    // vertex
                    contactPoint = contactList[0].ContactPoint;
                    normal = contactList[0].Normal;
                    normalW = contactList[0].NormalW;
                    depth = contactList[0].Depth;
                }
                else if (contactList.Count == 2)
                {
                    // segment
                    float b = 0;
                    contactPoint = PointToSegment(new Vector3(), contactList[0].ContactPoint, contactList[1].ContactPoint, out b);
                    //contactPoint = (0.5f * (contactList[0].ContactPoint + contactList[1].ContactPoint));
                    normal = (b * contactList[0].Normal + (1 - b) * contactList[1].Normal);
                    normal.Normalize();
                    normalW = (b * contactList[0].NormalW + (1 - b) * contactList[1].NormalW);
                    normalW.Normalize();
                    depth = (b * contactList[0].Depth + (1 - b) * contactList[1].Depth);
                }
                else
                {
                    // plane
                    normal = new Vector3();
                    normalW = new Vector3();
                    depth = 0;
                    foreach (CollisionPoint collisionPoint in contactList)
                    {
                        normal += collisionPoint.Normal;
                        normalW += collisionPoint.NormalW;
                        depth += collisionPoint.Depth;
                    }
                    normal *= (1f / contactList.Count);
                    normal.Normalize();
                    normalW *= (1f / contactList.Count);
                    normalW.Normalize();
                    depth *= (1f / contactList.Count);
                    contactPoint = -contactList[0].ContactPoint.Z * normal;//-depth * normal;
                }
                Vector3 pointVel = planeVelocity + Vector3.Cross(rotationalVelocity, contactPoint);
                float epsilon = AircraftParameters.Restitution;
                float j = ((-1 - epsilon) * Vector3.Dot(Velocity, normalW)) /
                    ((1.0f / (float)AircraftParameters.Mass) +
                     (Vector3.Dot(MultiplyInertiaInverse(Vector3.Cross(Vector3.Cross(contactPoint, normal), contactPoint)), normal)));

                if (Math.Abs(j) > AircraftParameters.CrashResistance)
                    Crashed = true;
                //Framework.Instance.DebugString += "\nj=" + j;
                Z -= depth;
                if (Vector3.Dot(normal, pointVel) > 0)
                {

                }
                else
                {
                    //Z -= depth;
                    //Framework.Instance.DebugString += j + "\n";
                    //Framework.Instance.DebugString += normalW + "\n";
                    //Framework.Instance.DebugString += contactList.Count;
                    Velocity += (j / (float)AircraftParameters.Mass) * normalW;

                    //Z += Velocity.Z * elapsedTime;

                    /* 
                    // Slow down on the ground.
                    if (sinceLastTouchdown < 0.5f)
                    {
                        Velocity -= elapsedTime * Velocity * 0.5f;
                    }
                     */
                    //float verticalVel = Vector3.Dot(Velocity, normalW);

                    //Velocity += (-Velocity * 1.0f* elapsedTime);
                    Vector3 rotationVelDiff = MultiplyInertiaInverse(Vector3.Cross(contactPoint, j * normal));
#if DEBUG
                    if (sinceLastTouchdown < 0.5f)
                    {
                        Framework.Instance.DebugString += string.Format("touchdown with {0} points!", contactList.Count);
                    }
#endif
                    if (Velocity.LengthSq() < 5.0f)
                    {
                        if (Math.Sign(Wx) != Math.Sign(rotationVelDiff.X))
                            Wx += rotationVelDiff.X;
                        else
                            Wx += rotationVelDiff.X / (3 * (6.0f - Velocity.LengthSq()));
                        if (Math.Sign(Wy) != Math.Sign(rotationVelDiff.Y))
                            Wy += rotationVelDiff.Y;
                        else
                            Wy += rotationVelDiff.Y / (3 * (6.0f - Velocity.LengthSq()));
                        if (Math.Sign(Wz) != Math.Sign(rotationVelDiff.Z))
                            Wz += rotationVelDiff.Z;
                        else
                            Wz += rotationVelDiff.Z / (3 * (6.0f - Velocity.LengthSq()));
                    }
                    else
                    {
                        Wx += rotationVelDiff.X;
                        Wy += rotationVelDiff.Y;
                        Wz += rotationVelDiff.Z;
                    }
                    /*
                    if (Velocity.LengthSq() < 5.0f)
                    {
                        if (Math.Sign(Roll) == Math.Sign(rotationVelDiff.X))
                            Wx = 0;
                        if (Math.Sign(Pitch) == Math.Sign(rotationVelDiff.Y))
                            Wy = 0;
                        if (Math.Sign(Yaw) == Math.Sign(rotationVelDiff.Z))
                            Wz = 0;                        
                    }
                    */

                }
                sinceLastTouchdown = 0.0f;
            }
            sinceLastTouchdown += elapsedTime;
            //DebugPosition = ToDirectX(gearPointW);
            //pointVel = Velocity + Vector3.Cross(rotationalVelocity, gearPoint);
        }


        private void UpdateWaterCollisions(float elapsedTime, Vector3 rotVelW, Vector3 positionW)
        {
            OnWater = false;
            bool overWater = false;
            foreach (Water water in Water)
            {
                if (water.OverWater(ToDirectX(positionW)))
                {
                    overWater = true;
                    break;
                }
            }
            if (!overWater)
                return;

            if (positionW.Z > 0)
            {
                Z -= positionW.Z / 5;
                Velocity -= 2 * elapsedTime * Velocity;
                Wx = Wy = Wz = 0;
                Crashed = true;
            }

            List<CollisionPoint> contactList = new List<CollisionPoint>();

            double crashRes = AircraftParameters.CrashResistanceGear;

            // Check collision points to see if we're touching water            
            foreach (Vector3 collisionPoint in AircraftParameters.CollisionPoints)
            {
                // Check for collisions
                Vector3 collisionPointW = positionW + Vector3.TransformCoordinate(collisionPoint, Matrix.RotationQuaternion(OrientationQuat));
                if (collisionPointW.Z > 0)
                    OnWater = true;
            }
            // Check the gear points
            if (GearExtended)
            {
                foreach (Vector3 gearPoint in AircraftParameters.GearPoints)
                {
                    // Check for collisions
                    Vector3 gearPointW = positionW + Vector3.TransformCoordinate(gearPoint, Matrix.RotationQuaternion(OrientationQuat));
                    if (gearPointW.Z > 0)
                        OnWater = true;
                }
            }

            // Check for floatation
            if (AircraftParameters.HasFloats)
            {
                foreach (Vector3 floatPoint in AircraftParameters.FloatPoints)
                {
                    // Check for collisions
                    Vector3 floatPointW = positionW + Vector3.TransformCoordinate(floatPoint, Matrix.RotationQuaternion(OrientationQuat));
                    Vector3 normalW;
                    float waterDepth = (float)(0.005f * (Math.Sin(3 * totalSeconds + 2 * floatPointW.X) + Math.Cos(3 * totalSeconds + 2 * floatPointW.Y)));
                    if (floatPointW.Z > waterDepth)
                    {
                        Vector3 normal = Vector3.TransformCoordinate(ToModel(new Vector3(0, 1, 0)), Matrix.RotationQuaternion(Quaternion.Invert(OrientationQuat)));
                        CollisionPoint point = new CollisionPoint();
                        point.ContactPoint = Vector3.TransformCoordinate(floatPoint, Matrix.RotationQuaternion(OrientationQuat));
                        point.Normal = normal;
                        point.NormalW = ToModel(new Vector3(0, 1, 0));
                        point.Depth = floatPointW.Z - waterDepth;
                        contactList.Add(point);
                    }
                }
            }

            if (contactList.Count > 0)
            {
                Vector3 contactPoint;
                Vector3 normal;
                Vector3 normalW;
                float depth;
                if (contactList.Count == 1)
                {
                    // vertex
                    contactPoint = contactList[0].ContactPoint;
                    normal = contactList[0].Normal;
                    normalW = contactList[0].NormalW;
                    depth = contactList[0].Depth;
                }
                else if (contactList.Count == 2)
                {
                    // segment
                    float b = 0;
                    contactPoint = PointToSegment(new Vector3(), contactList[0].ContactPoint, contactList[1].ContactPoint, out b);
                    //contactPoint = (0.5f * (contactList[0].ContactPoint + contactList[1].ContactPoint));
                    normal = (b * contactList[0].Normal + (1 - b) * contactList[1].Normal);
                    normal.Normalize();
                    normalW = (b * contactList[0].NormalW + (1 - b) * contactList[1].NormalW);
                    normalW.Normalize();
                    depth = (b * contactList[0].Depth + (1 - b) * contactList[1].Depth);
                }
                else
                {
                    // plane
                    normal = new Vector3();
                    normalW = new Vector3();
                    depth = 0;
                    contactPoint = new Vector3(0, 0, 0);
                    foreach (CollisionPoint collisionPoint in contactList)
                    {
                        normal += collisionPoint.Normal;
                        normalW += collisionPoint.NormalW;
                        depth += collisionPoint.Depth;
                        contactPoint += collisionPoint.ContactPoint;
                    }

                    normal *= (1f / contactList.Count);
                    normal.Normalize();
                    normalW *= (1f / contactList.Count);
                    normalW.Normalize();
                    depth *= (1f / contactList.Count);

                    Plane plane = Plane.FromPoints(contactList[0].ContactPoint, contactList[1].ContactPoint, contactList[2].ContactPoint);
                    //Vector3 planeNormal = Vector3.Cross(contactList[1].ContactPoint - contactList[0].ContactPoint,
                    //    contactList[2].ContactPoint - contactList[0].ContactPoint);
                    //planeNormal.Normalize();
                    plane.Normalize();
                    float distance = plane.D;
                    contactPoint = -distance * normalW;
                    //contactPoint = -contactList[0].ContactPoint.Z * normal;//-depth * normal;
                }

                float epsilon = AircraftParameters.Restitution;
                Vector3 contactVel = Velocity + Vector3.Cross(rotVelW, contactPoint);
                float jpoint = ((-1 - epsilon) * Vector3.Dot(contactVel, normalW)) /
                    ((1.0f / (float)AircraftParameters.Mass) +
                    Vector3.Dot(normalW, Vector3.Cross(MultiplyInertiaInverse2(Vector3.Cross(contactPoint, normalW)), contactPoint)));
                float jcg = ((-1 - epsilon) * Vector3.Dot(Velocity, normalW)) /
                    ((1.0f / (float)AircraftParameters.Mass));

                if (Math.Abs(jpoint) > crashRes)
                    Crashed = true;

                if (depth > 0)
                    Z -= depth / 20;

                //if (Vector3.Dot(normalW, contactVel) < 0)
                {
                    Vector3 colTangent = Vector3.Cross(Vector3.Cross(normalW, contactVel), normalW);
                    colTangent.Normalize();
                    //this.DebugPosition = ToDirectX(positionW + contactPoint);
                    //this.Debug1 = ToDirectX(positionW + colTangent);
                    float vrt = Vector3.Dot(contactVel, colTangent);
                    Vector3 rotationVelDiffW;
                    if (vrt > 0)
                    {
                        Velocity += (jcg / (float)AircraftParameters.Mass) * normalW
                            - ((jcg * AircraftParameters.WaterDrag / (float)AircraftParameters.Mass) * colTangent);
                        rotationVelDiffW = MultiplyInertiaInverse2(
                            Vector3.Cross(contactPoint, (jpoint * normalW - jcg * AircraftParameters.WaterDrag * colTangent)));
                    }
                    else
                    {
                        Velocity += (jcg / (float)AircraftParameters.Mass) * normalW;
                        rotationVelDiffW = MultiplyInertiaInverse2(Vector3.Cross(contactPoint, jpoint * normalW));
                    }
                    rotVelW += rotationVelDiffW;
                    /*
                    if (Velocity.LengthSq() < 2f)
                    {
                        if (elapsedTime < 0.2f)
                        {
                            if (Math.Sign(rotVelW.X) != Math.Sign(rotationVelDiffW.X))
                                rotVelW.X += rotationVelDiffW.X;
                            else
                                rotVelW.X += 0.5f * rotationVelDiffW.X;
                            if (Math.Sign(rotVelW.Y) != Math.Sign(rotationVelDiffW.Y))
                                rotVelW.Y += rotationVelDiffW.Y;
                            else
                                rotVelW.Y += 0.5f * rotationVelDiffW.Y;
                            if (Math.Sign(rotVelW.Z) != Math.Sign(rotationVelDiffW.Z))
                                rotVelW.Z += rotationVelDiffW.Z;
                            else
                                rotVelW.Z += 0.5f * rotationVelDiffW.Z;
                        }
                    }
                    else
                        rotVelW += rotationVelDiffW;
                     */
                    //+ (jpoint*0.3f*colTangent));
                    //Vector3 rotationVelDiffW = MultiplyInertiaInverse2(j * normalW);

                    Vector3 newRotVels = ToPlaneCoords(rotVelW);
                    Wx = newRotVels.X; Wy = newRotVels.Y; Wz = newRotVels.Z;
                }

                sinceLastTouchdown = 0.0f;
            }
            sinceLastTouchdown += elapsedTime;
        }

        private bool IsColliding(Vector3 point, out Vector3 normal, out float depth)
        {
            if (heightmap != null)
            {
                float height = heightmap.GetHeightAt(-point.Y, -point.X);
                if (-point.Z < height)
                {
                    normal = heightmap.GetNormalAt(-point.Y, -point.X);
                    depth = height + point.Z;
                    return true;
                }
            }
            normal = new Vector3(0,1,0);
            depth = 0f;
            return false;
        }

        private Vector3 MultiplyInertiaInverse(Vector3 vector)
        {
            return new Vector3(
                vector.X / (float)AircraftParameters.Ixx,
                vector.Y / (float)AircraftParameters.Iyy,
                vector.Z / (float)AircraftParameters.Izz);
        }

        public static Vector3 ToDirectX(Vector3 source)
        {
            return new Vector3(-source.Y, -source.Z, -source.X);
        }

        public static Vector3 ToModel(Vector3 source)
        {
            return new Vector3(-source.Z, -source.X, -source.Y);
        }

        private Vector3 PointToSegment(Vector3 p, Vector3 s1, Vector3 s2, out float b)
        {
            Vector3 v = s2 - s1;
            Vector3 w = p - s1;
            float c1 = Vector3.Dot(w, v);
            float c2 = Vector3.Dot(v, v);
            if (c1 <= 0)
            {
                b = 0;
                return s1;
            }
            if (c2 <= c1)
            {
                b = 1;
                return s2;
            }
            b = c1 / c2;
            return s1 + b * v;
        }
        #endregion

        #region Thread related stuff
        private Thread thread;
        private bool running = false;

        private void StartModel()
        {
            thread = new Thread(new ThreadStart(this.ModelRun));
            thread.Priority = ThreadPriority.Highest;
            running = true;
            thread.Start();
        }

        private void StopModel()
        {
            running = false;
            thread.Join();
        }

        private void ModelRun()
        {
            double currentTime = FrameworkTimer.GetTime();
            double previousTime = currentTime;
            while (running)
            {
                float elapsedTime = (float)(currentTime - previousTime);
                MoveScene(elapsedTime);
                previousTime = currentTime;
                Thread.Sleep(2);
                currentTime = FrameworkTimer.GetTime();
            }
        }
        #endregion


    }
}
