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
using RCDeskPilot.API;

namespace RCSim
{
    internal class FlightModelApi : IAirplaneControl, IDisposable, IFlightModel
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

        /// <summary>
        /// The X position in the inertial coordinate system (X positive north)
        /// </summary>
        public float X
        {
            get
            {
                return ApiModel.X;
            }
            set
            {
                ApiModel.X = value;
            }
        }
        /// <summary>
        /// The Y position in the inertial coordinate system (Y positive east)
        /// </summary>
        public float Y
        {
            get
            {
                return ApiModel.Y;
            }
            set
            {
                ApiModel.Y = value;
            }
        }
        /// <summary>
        /// The Z position in the inertial coordinate system (Z positive down)
        /// </summary>
        public float Z
        {
            get
            {
                return ApiModel.Z;
            }
            set
            {
                ApiModel.Z = value;
            }
        }
        /// <summary>
        /// The yaw angle (positive right)
        /// </summary>
        public float Yaw
        {
            get
            {
                return ApiModel.Yaw;
            }
            set
            {
                ApiModel.Yaw = value;
            }
        }
        /// <summary>
        /// The pitch angle (positive up)
        /// </summary>
        public float Pitch
        {
            get
            {
                return ApiModel.Pitch;
            }
            set
            {
                ApiModel.Pitch = value;
            }
        }
        /// <summary>
        /// The roll angle (positive right)
        /// </summary>
        public float Roll
        {
            get
            {
                return ApiModel.Roll;
            }
            set
            {
                ApiModel.Roll = value;
            }
        }
        
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
        
        /// <summary>
        /// Gets/sets the aircraft parameters.
        /// </summary>
        public RCSim.DataClasses.AircraftParameters AircraftParameters { get; set; }
        public RCDeskPilot.API.AircraftParameters ApiAircraftParameters { get; set; }
        public RCDeskPilot.API.FlightModelSimple ApiModel { get; set; }
       
        /// <summary>
        /// Gets/Sets the wind vector [m/s]
        /// </summary>
        public Vector3 Wind { get; set; }
       
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
        public bool FlapsExtended { get; set; }

        /// <summary>
        /// Gets whether the gear has been extended.
        /// </summary>
        public bool GearExtended { get; set; }


        /// <summary>
        /// Returns the speed.
        /// </summary>
        public double Speed { get { return ApiModel.Velocity.Length(); } }
        
        /// <summary>
        /// Gets the Euler angles in world coordinates.
        /// </summary>
        public Vector3 Angles
        {
            get { return Utility.EulerAnglesFromQuaternion(ApiModel.OrientationQuat); }
        }

        /// <summary>
        /// Gets the velocity in world coordinates.
        /// </summary>
        public Vector3 Velocity
        {
            get
            {
                return ApiModel.Velocity;
            }

            set
            {
                ApiModel.Velocity = value;
            }
        }
        #endregion

        #region Private fields
        private double rhoSurface = 0.0;
        private double rhoVerticalSurface = 0.0f;
        private double airSpeedSq = 0.0;
        private double gravityMass = 0.0;

        private Heightmap heightmap = null;

        private float sinceLastTouchdown = 0;
        private double prevThrottle = 0;
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
        public List<Water> Water { get; set; }

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
            get
            {
                return false;
            }
        }

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

        /// <summary>
        /// Gets the list of collision points in World coordinates.
        /// </summary>
        public List<Vector3> CollisionPoints
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the list of gear points in World coordinates.
        /// </summary>
        public List<Vector3> GearPoints
        {
            get;
            set;
        }

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
        #endregion

        #region DEBUG properties
        public Vector3 DebugPosition { get; set; }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Clean up.
        /// </summary>
        public void Dispose()
        {
            ApiModel.ShutDown();
            StopModel();            
        }

        #endregion

        #region Public methods
        public void Initialize()
        {
            ApiAircraftParameters =  AircraftParameters.CloneToApiParameters( );
            ApiModel.AircraftParameters = ApiAircraftParameters;
            rhoSurface = _AirDensity * ApiAircraftParameters.WingArea / 2;
            rhoVerticalSurface = _AirDensity * ApiAircraftParameters.VerticalArea / 2;
            gravityMass = _Gravity * ApiAircraftParameters.Mass;
            ApiModel.OrientationQuat = Quaternion.RotationYawPitchRoll(0.0f, 0.0f, 0.0f);
            ApiModel.Initialize();
            StartModel();
        }

        public void Reset()
        {
            ApiModel.X = 0.0f;
            ApiModel.Y = 0.0f;
            ApiModel.Z = 0.0f;
            ApiModel.OrientationQuat = Quaternion.RotationYawPitchRoll(0f, 0f, (float)Math.PI / 2f);
            ApiModel.Vx = 0;
            ApiModel.Vy = 0;
            ApiModel.Vz = 0;
            ApiModel.Wx = 0;
            ApiModel.Wy = 0;
            ApiModel.Wz = 0;
            ApiModel.Alpha = 14.0f;
            ApiModel.Velocity = new Vector3(0, 0, 0);
            Crashed = false;
            prevThrottle = 0;
        }

        /// <summary>
        /// Handlaunches an airplane.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public void HandLaunch(float x, float y, float z)
        {
            ApiModel.X = x;
            ApiModel.Y = y;
            ApiModel.Z = z;
            ApiModel.Vx = 15;
            ApiModel.Velocity = new Vector3(0, 10, -2);
        }

        /// <summary>
        /// Allows the flightmodel to override the controls.
        /// </summary>
        /// <param name="elapsedTime"></param>
        public void UpdateControls(float elapsedTime)
        {
            if (ApiModel.UpdateControls(elapsedTime))
                return;
            double delay = AircraftParameters.ThrottleDelay;
            double targetThrottle = (Throttle + 1.0) / 2.0;
            Throttle = Math.Min((1 - delay * elapsedTime) * prevThrottle + elapsedTime * delay * targetThrottle, 1);
            prevThrottle = Math.Min(Throttle, 1.0);
        }

        /// <summary>
        /// Recalculate constants
        /// </summary>
        public void UpdateConstants()
        {

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
                if ((elapsedTime > 0.5f) || Crashed || Paused)
                    return;
                float elapsedSeconds = elapsedTime;

                // Update the airspeed
                UpdateAirspeed();

                // Convert the wind vector to plane coordinates
                Vector3 wind = Vector3.TransformCoordinate(ToModel(-Wind), Matrix.RotationQuaternion(Quaternion.Invert(ApiModel.OrientationQuat)));

                if (Math.Abs(ApiModel.Vx + wind.X) < 0.001)
                {
                    ApiModel.Alpha = 0;
                    ApiModel.Beta = 0;
                }
                else
                {
                    ApiModel.Alpha = Math.Atan2(ApiModel.Vz + wind.Z, ApiModel.Vx + wind.X);
                    ApiModel.Beta = -Math.Atan2(ApiModel.Vy + wind.Y, ApiModel.Vx + wind.X);
                }

                // Calculate all forces on the airframe.
                // Calculate all forces on the airframe.
                if (AircraftParameters.FlightModelType == RCSim.DataClasses.AircraftParameters.FlightModelTypeEnum.Helicopter)
                    CalculateForcesHelicopter(elapsedSeconds, wind);
                else
                    CalculateForces(elapsedSeconds, wind);
                
                ApiModel.Ax = (float)(ApiModel.Fx / ApiAircraftParameters.Mass);
                ApiModel.Ay = (float)(ApiModel.Fy / ApiAircraftParameters.Mass);
                ApiModel.Az = (float)(ApiModel.Fz / ApiAircraftParameters.Mass);

                float velX0 = ApiModel.Velocity.X; float velY0 = ApiModel.Velocity.Y; float velZ0 = ApiModel.Velocity.Z;
                Vector3 dVelocity = new Vector3(ApiModel.Ax * elapsedSeconds, ApiModel.Ay * elapsedSeconds, ApiModel.Az * elapsedSeconds);
                dVelocity.TransformCoordinate(Matrix.RotationQuaternion(ApiModel.OrientationQuat));
                //Vector3 dVelocity = new Vector3(Ax * elapsedSeconds, Ay * elapsedSeconds, Az * elapsedSeconds).TransformCoordinate(Matrix.RotationQuaternion(OrientationQuat));
                ApiModel.Velocity += dVelocity;

                // collision response
                UpdateCollisions(elapsedSeconds);

                Vector3 planeVelocity = new Vector3(ApiModel.Velocity.X, ApiModel.Velocity.Y, ApiModel.Velocity.Z);
                planeVelocity.TransformCoordinate(Matrix.RotationQuaternion(Quaternion.Invert(ApiModel.OrientationQuat)));
                ApiModel.Vx = planeVelocity.X;
                ApiModel.Vy = planeVelocity.Y;
                ApiModel.Vz = planeVelocity.Z;

                // Calculate all torques on the airframe
                // Calculate all torques on the airframe
                if (AircraftParameters.FlightModelType == RCSim.DataClasses.AircraftParameters.FlightModelTypeEnum.Helicopter)
                    CalculateTorquesHelicopter(elapsedSeconds, wind);
                else
                    CalculateTorques(elapsedSeconds, wind);

                ApiModel.AAx = (float)(ApiModel.Tx / ApiAircraftParameters.Ixx);
                ApiModel.AAy = (float)(ApiModel.Ty / ApiAircraftParameters.Iyy);
                ApiModel.AAz = (float)(ApiModel.Tz / ApiAircraftParameters.Izz);

                if (TouchedDown && (ApiModel.Vx < 10f))
                {
                    ApiModel.AAz += (float)ApiModel.Rudder * ApiModel.Vx * 2f;
                }

                // Integrate
                float Wx0 = ApiModel.Wx; float Wy0 = ApiModel.Wy; float Wz0 = ApiModel.Wz;
                ApiModel.Wx += ApiModel.AAx * elapsedSeconds;
                ApiModel.Wy += ApiModel.AAy * elapsedSeconds;
                ApiModel.Wz += ApiModel.AAz * elapsedSeconds;

                // Update location
                Vector3 dLoc = new Vector3(
                   velX0 * elapsedSeconds + dVelocity.X * elapsedSeconds * 0.5f,
                   velY0 * elapsedSeconds + dVelocity.Y * elapsedSeconds * 0.5f,
                   velZ0 * elapsedSeconds + dVelocity.Z * elapsedSeconds * 0.5f);

                ApiModel.X += dLoc.X;
                ApiModel.Y += dLoc.Y;
                ApiModel.Z += dLoc.Z;

                // Recoordinate the orientation
                Quaternion newOrientation = new Quaternion(
                        ApiModel.OrientationQuat.X + (ApiModel.OrientationQuat.W * ApiModel.Wx + 
                                                      ApiModel.OrientationQuat.Y * ApiModel.Wz - 
                                                      ApiModel.OrientationQuat.Z * ApiModel.Wy) * 0.5f * elapsedSeconds,
                        ApiModel.OrientationQuat.Y + (ApiModel.OrientationQuat.W * ApiModel.Wy + 
                                                      ApiModel.OrientationQuat.Z * ApiModel.Wx - 
                                                      ApiModel.OrientationQuat.X * ApiModel.Wz) * 0.5f * elapsedSeconds,
                        ApiModel.OrientationQuat.Z + (ApiModel.OrientationQuat.W * ApiModel.Wz + 
                                                      ApiModel.OrientationQuat.X * ApiModel.Wy - 
                                                      ApiModel.OrientationQuat.Y * ApiModel.Wx) * 0.5f * elapsedSeconds,
                        ApiModel.OrientationQuat.W - (ApiModel.OrientationQuat.X * ApiModel.Wx + 
                                                      ApiModel.OrientationQuat.Y * ApiModel.Wy + 
                                                      ApiModel.OrientationQuat.Z * ApiModel.Wz) * 0.5f * elapsedSeconds
                        );
                //OrientationQuat += OrientationQuat * (new Vector3(Wx, Wy, Wz)) * 0.5f * elapsedSeconds;
                newOrientation.Normalize();
                ApiModel.OrientationQuat = newOrientation;
                Vector3 ypr = Utility.EulerAnglesFromQuaternion(ApiModel.OrientationQuat);
                ApiModel.Roll = ypr.X;
                ApiModel.Pitch = ypr.Y;
                ApiModel.Yaw = ypr.Z;
            }
        }
        #endregion

        #region Private methods
        private void UpdateAirspeed()
        {
            airSpeedSq = ApiModel.Vx * ApiModel.Vx + ApiModel.Vy * ApiModel.Vy + ApiModel.Vz * ApiModel.Vz;
        }

        /// <summary>
        /// Calculates the total forces on the airframe.
        /// </summary>
        /// <param name="elapsedTime"></param>
        private void CalculateForces(float elapsedTime, Vector3 wind)
        {
            if (ApiModel.CalculateForces(elapsedTime, wind))
                return;
            Lift = GetLiftForce(wind);
            Drag = GetDragForce(wind);
            double sideLift = GetSideLiftForce(wind);
            double sideDrag = GetSideDragForce(wind);
            double sideGroundResistance = 0;
            double groundResistance = 0;
            // verified : gravity, liftX, throttle, liftZ 
            if (TouchedDown)
            {
                if ((ApiModel.Vx < 10f) && (ApiModel.Vx > 0.01))
                {
                    double realBeta = -Math.Atan2(ApiModel.Vy, ApiModel.Vx);
                    sideGroundResistance = Math.Sin(realBeta) * gravityMass;
                }
                groundResistance = ApiModel.Vx / 10.0;
            }

            ApiModel.Fx = (float)(+sideLift * Math.Sin(ApiModel.Beta)
                         - sideDrag * Math.Cos(ApiModel.Beta)
                         - Lift * Math.Sin(ApiModel.Alpha)
                         - Drag * Math.Cos(ApiModel.Alpha)
                         + AircraftParameters.MaximumThrust * Throttle * AircraftParameters.GetThrustCoefficient((double)ApiModel.Vx)
                         - gravityMass * Math.Sin(ApiModel.Pitch)
                         - groundResistance);
            ApiModel.Fy = (float)(+sideLift * Math.Cos(ApiModel.Beta) + sideDrag * Math.Sin(ApiModel.Beta) + gravityMass * Math.Sin(ApiModel.Roll) * Math.Cos(ApiModel.Pitch) + sideGroundResistance);
            ApiModel.Fz = (float)(-Lift * Math.Cos(ApiModel.Alpha) - Drag * Math.Sin(ApiModel.Alpha) + gravityMass * Math.Cos(ApiModel.Roll) * Math.Cos(ApiModel.Pitch));
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
                if ((ApiModel.Vx < 10f) && (ApiModel.Vx > 0.01))
                {
                    double realBeta = -Math.Atan2(ApiModel.Vy, ApiModel.Vx);
                    sideGroundResistance = Math.Sin(realBeta) * gravityMass;
                }
                groundResistance = ApiModel.Vx / 10.0;
            }

            // Ground effect
            float groundEffect = 0;
            if (heightmap != null)
            {
                float height = heightmap.GetHeightAt(-ApiModel.Y, -ApiModel.X);
                float altitude = -ApiModel.Z - height;
                if (altitude < 2)
                {
                    groundEffect = -(float)Throttle * (float)AircraftParameters.MaximumThrust * (2 - altitude) / 4;
                }
            }

            double dragX = 0.01 * (ApiModel.Vx + wind.X) * (ApiModel.Vx + wind.X);
            double dragY = 0.01 * (ApiModel.Vy + wind.Y) * (ApiModel.Vy + wind.Y);
            double dragZ = 0.05 * (ApiModel.Vz + wind.Z) * (ApiModel.Vz + wind.Z);
            float defaultRPM = 1000;
            float Frotor = (float)(Throttle * AircraftParameters.MaximumThrust * (RotorRPM / 1000));
            ApiModel.Fx = (float)((groundEffect - gravityMass) * Math.Sin(ApiModel.Pitch)
                          - Math.Sign(ApiModel.Vx) * dragX);
            ApiModel.Fy = (float)((groundEffect + gravityMass) * Math.Sin(ApiModel.Roll) * Math.Cos(ApiModel.Pitch)
                          - Math.Sign(ApiModel.Vy) * dragY);
            ApiModel.Fz = (float)((groundEffect + gravityMass) * Math.Cos(ApiModel.Roll) * Math.Cos(ApiModel.Pitch)
                          - Frotor
                          - Math.Sign(ApiModel.Vz) * dragZ);

            // Update the rotor RPM
            float rotorSpeed = RotorRPM / defaultRPM;
            float drag = Frotor / (float)AircraftParameters.MaximumThrust + rotorSpeed * rotorSpeed;
            RotorRPM += (2 - drag) * 150 * elapsedTime;
        }

        /// <summary>
        /// Calculates the lift force.
        /// </summary>
        /// <returns></returns>
        private double GetLiftForce(Vector3 wind)
        {
            // F = (1/2)*Cl*V^2*S
            return ApiAircraftParameters.GetLiftCoefficient(ApiModel.Alpha, 0.0) * 
                ((ApiModel.Vx + wind.X) * (ApiModel.Vx + wind.X) + (ApiModel.Vz + wind.Z) * (ApiModel.Vz + wind.Z)) * rhoSurface;
        }

        /// <summary>
        /// Calculates the drag force.
        /// </summary>
        /// <returns></returns>
        private double GetDragForce(Vector3 wind)
        {
            // F = (1/2)*Dc*V^2*S
            return ApiAircraftParameters.GetDragCoefficient(ApiModel.Alpha, 0.0) * 
                ((ApiModel.Vx + wind.X) * (ApiModel.Vx + wind.X) + (ApiModel.Vz + wind.Z) * (ApiModel.Vz + wind.Z)) * rhoSurface;
        }

        /// <summary>
        /// Calculates the lift force generated by the fuselage and vertical tail.
        /// </summary>
        /// <returns></returns>
        private double GetSideLiftForce(Vector3 wind)
        {
            return ApiAircraftParameters.GetSideLiftCoefficient(ApiModel.Beta) * 
                ((ApiModel.Vx + wind.X) * (ApiModel.Vx + wind.X) + (ApiModel.Vy + wind.Y) * (ApiModel.Vy + wind.Y)) * rhoVerticalSurface;
        }

        /// <summary>
        /// Calculates the drag force generated by the fuselage and vertical tail.
        /// </summary>
        /// <returns></returns>
        private double GetSideDragForce(Vector3 wind)
        {
            return ApiAircraftParameters.GetSideDragCoefficient(Beta) * 
                ((ApiModel.Vx + wind.X) * (ApiModel.Vx + wind.X) + (ApiModel.Vy + wind.Y) * (ApiModel.Vy + wind.Y)) * rhoVerticalSurface;
        }

        /// <summary>
        /// Calculate the torques along the different axis of the airframe.
        /// </summary>
        /// <param name="elapsedTime"></param>
        private void CalculateTorques(float elapsedTime, Vector3 wind)
        {
            if (ApiModel.CalculateTorques(elapsedTime, wind))
                return;
            // Torque = F * r * sin(alpha)
            // TODO differential lift
            float airspeedX = ApiModel.Vx + wind.X;
            float airspeedY = ApiModel.Vy + wind.Y;
            float airspeedZ = ApiModel.Vz + wind.Z;
            float airspeedXY = (float)Math.Sqrt(airspeedX * airspeedX + airspeedY * airspeedY);
            float airspeedXZ = (float)Math.Sqrt(airspeedX * airspeedX + airspeedZ * airspeedZ);
            ApiModel.Tx = (float)((-ApiAircraftParameters.RollDamping * Math.Sign(airspeedX) * ApiModel.Wx +
                           ApiAircraftParameters.AileronEfficiency * airspeedX * ApiModel.Ailerons +
                           ApiAircraftParameters.DihedralAngle * ApiAircraftParameters.DihedralEfficiency * airspeedX * ApiModel.Beta) * airspeedX +
                           (Lift * ApiAircraftParameters.SpinFactor * ApiModel.Wz) / (Math.Abs(airspeedX) + 1.0f) +
                           ApiAircraftParameters.AileronEfficiency * ApiAircraftParameters.PropWashAilerons * ApiModel.Throttle * ApiModel.Ailerons);
            ApiModel.Ty = (float)((-ApiAircraftParameters.PitchDamping * ApiModel.Wy -
                           Math.Sin(ApiModel.Alpha) * ApiAircraftParameters.PitchStability +
                           ApiAircraftParameters.ElevatorEfficiency * airspeedX * Elevator + ApiAircraftParameters.PitchTrim) * airspeedXY +
                           ApiAircraftParameters.ElevatorEfficiency * ApiAircraftParameters.PropWashElevator * ApiModel.Throttle * ApiModel.Elevator);
            ApiModel.Tz = (float)((-ApiAircraftParameters.YawDamping * ApiModel.Wz -
                           Math.Sin(ApiModel.Beta) * ApiAircraftParameters.YawStability +
                           ApiAircraftParameters.RudderEfficiency * airspeedX * ApiModel.Rudder) * airspeedXZ +
                           ApiAircraftParameters.RudderEfficiency * ApiAircraftParameters.PropWashRudder * ApiModel.Throttle * ApiModel.Rudder);
        }

        /// <summary>
        /// Calculate the torques along the different axis of the airframe for a helicopter.
        /// </summary>
        /// <param name="elapsedTime"></param>
        private void CalculateTorquesHelicopter(float elapsedTime, Vector3 wind)
        {
            // Torque = F * r * sin(alpha)
            // TODO differential lift
            float airspeedX = ApiModel.Vx + Wind.X;
            float airspeedY = ApiModel.Vy + Wind.Y;
            float airspeedZ = ApiModel.Vz + Wind.Z;
            float airspeedXY = (float)Math.Sqrt(airspeedX * airspeedX + airspeedY * airspeedY);
            float airspeedXZ = (float)Math.Sqrt(airspeedX * airspeedX + airspeedZ * airspeedZ);

            ApiModel.Tx = (float)(-AircraftParameters.RollDamping * 10 * ApiModel.Wx
                         - AircraftParameters.RollDamping * 10 * ApiModel.Roll +
                           AircraftParameters.AileronEfficiency * 50 * Ailerons);
            ApiModel.Ty = (float)(-AircraftParameters.PitchDamping * 10 * ApiModel.Wy +
                         -AircraftParameters.PitchDamping * 10 * ApiModel.Pitch +
                           AircraftParameters.ElevatorEfficiency * 50 * ApiModel.Elevator);
            ApiModel.Tz = (float)(-AircraftParameters.YawDamping * 10 * ApiModel.Wz +
                            AircraftParameters.RudderEfficiency * 50 * Rudder);

        }

        private void UpdateCollisions(float elapsedTime)
        {
            //Framework.Instance.DebugString += "\nvel: " + Velocity;
            Vector3 planeVelocity = new Vector3(ApiModel.Velocity.X, ApiModel.Velocity.Y, ApiModel.Velocity.Z);
            Vector3 rotationalVelocity = new Vector3(ApiModel.Wx, ApiModel.Wy, ApiModel.Wz);
            planeVelocity.TransformCoordinate(Matrix.RotationQuaternion(Quaternion.Invert(ApiModel.OrientationQuat)));
            //Vx = planeVelocity.X;
            //Vy = planeVelocity.Y;
            //Vz = planeVelocity.Z;                        
            Vector3 position = new Vector3(ApiModel.X, ApiModel.Y, ApiModel.Z);

            List<CollisionPoint> contactList = new List<CollisionPoint>();
            foreach (Vector3 gearPoint in ApiAircraftParameters.GearPoints)
            {
                // Check for collisions
                Vector3 gearPointW = position + Vector3.TransformCoordinate(gearPoint, Matrix.RotationQuaternion(ApiModel.OrientationQuat));
                Vector3 normalW;
                float depth = 0f;
                if (IsColliding(gearPointW, out normalW, out depth))
                {
                    Vector3 normal = Vector3.TransformCoordinate(ToModel(normalW), Matrix.RotationQuaternion(Quaternion.Invert(ApiModel.OrientationQuat)));
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
                float epsilon = ApiAircraftParameters.Restitution;
                float j = ((-1 - epsilon) * Vector3.Dot(ApiModel.Velocity, normalW)) /
                    ((1.0f / (float)ApiAircraftParameters.Mass) +
                     (Vector3.Dot(MultiplyInertiaInverse(Vector3.Cross(Vector3.Cross(contactPoint, normal), contactPoint)), normal)));

                if (Math.Abs(j) > ApiAircraftParameters.CrashResistence)
                    Crashed = true;
                //Framework.Instance.DebugString += "\nj=" + j;
                ApiModel.Z -= depth;
                if (Vector3.Dot(normal, pointVel) > 0)
                {

                }
                else
                {
                    //Z -= depth;
                    //Framework.Instance.DebugString += j + "\n";
                    //Framework.Instance.DebugString += normalW + "\n";
                    //Framework.Instance.DebugString += contactList.Count;
                    ApiModel.Velocity += (j / (float)ApiAircraftParameters.Mass) * normalW;

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
                    if (ApiModel.Velocity.LengthSq() < 5.0f)
                    {
                        if (Math.Sign(ApiModel.Wx) != Math.Sign(rotationVelDiff.X))
                            ApiModel.Wx += rotationVelDiff.X;
                        else
                            ApiModel.Wx += rotationVelDiff.X / (3 * (6.0f - ApiModel.Velocity.LengthSq()));
                        if (Math.Sign(ApiModel.Wy) != Math.Sign(rotationVelDiff.Y))
                            ApiModel.Wy += rotationVelDiff.Y;
                        else
                            ApiModel.Wy += rotationVelDiff.Y / (3 * (6.0f - ApiModel.Velocity.LengthSq()));
                        if (Math.Sign(ApiModel.Wz) != Math.Sign(rotationVelDiff.Z))
                            ApiModel.Wz += rotationVelDiff.Z;
                        else
                            ApiModel.Wz += rotationVelDiff.Z / (3 * (6.0f - ApiModel.Velocity.LengthSq()));
                    }
                    else
                    {
                        ApiModel.Wx += rotationVelDiff.X;
                        ApiModel.Wy += rotationVelDiff.Y;
                        ApiModel.Wz += rotationVelDiff.Z;
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
            ApiModel.TouchedDown = TouchedDown;
            //DebugPosition = ToDirectX(gearPointW);
            //pointVel = Velocity + Vector3.Cross(rotationalVelocity, gearPoint);
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
            normal = new Vector3(0, 1, 0);
            depth = 0f;
            return false;
        }

        private Vector3 MultiplyInertiaInverse(Vector3 vector)
        {
            return new Vector3(
                vector.X / (float)ApiAircraftParameters.Ixx,
                vector.Y / (float)ApiAircraftParameters.Iyy,
                vector.Z / (float)ApiAircraftParameters.Izz);
        }

        private Vector3 ToDirectX(Vector3 source)
        {
            return new Vector3(-source.Y, -source.Z, -source.X);
        }

        private Vector3 ToModel(Vector3 source)
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



        #region IAirplaneControl Members
        public double Throttle
        {
            get { return ApiModel.Throttle; }
            set { ApiModel.Throttle = value; }
        }

        public double Ailerons
        {
            get { return ApiModel.Ailerons; }
            set { ApiModel.Ailerons = value; }
        }

        public double Elevator
        {
            get { return ApiModel.Elevator; }
            set { ApiModel.Elevator = value; }
        }

        public double Rudder
        {
            get { return ApiModel.Rudder; }
            set { ApiModel.Rudder = value; }
        }

        public double Flaps
        {
            get { return 0; }
            set { }
        }

        public double Gear
        {
            get { return 0; }
            set { }
        }

        RCSim.DataClasses.AircraftParameters IAirplaneControl.AircraftParameters
        {
            get
            {
                return AircraftParameters;
            }
            set
            {
                AircraftParameters = value;
            }
        }

        #endregion
    }
}
