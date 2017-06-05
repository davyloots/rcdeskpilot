using System;
using System.Collections.Generic;
using System.Text;
using RCSim.DataClasses;
using System.Diagnostics;
using Bonsai.Core;
using Microsoft.DirectX;
using Bonsai.Objects.Terrain;

namespace RCSim
{
    class FlightModel
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
        #endregion

        #region Wind
        /// <summary>
        /// Gets/Sets the wind vector [m/s]
        /// </summary>
        public Vector3 Wind { get; set; }
        #endregion
        #endregion

        #region Private fields
        private double rhoSurface = 0.0;
        private double rhoVerticalSurface = 0.0f;
        private double airSpeedSq = 0.0;
        private double gravityMass = 0.0;

        private Heightmap heightmap = null;
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
        #endregion

        #region DEBUG properties
        public Vector3 DebugPosition { get; set; }
        #endregion

        #region Public methods
        public void Initialize()
        {
            rhoSurface = _AirDensity * AircraftParameters.WingArea / 2;
            rhoVerticalSurface = _AirDensity * AircraftParameters.VerticalArea / 2;
            gravityMass = _Gravity * AircraftParameters.Mass;
            OrientationQuat = Quaternion.RotationYawPitchRoll(0.0f, 0.0f, 0.0f);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elapsedTime"></param>
        public void MoveScene(float elapsedTime)
        {
            if (elapsedTime > 1.0f)
                return;
            float elapsedSeconds = elapsedTime;

            // Update the airspeed
            UpdateAirspeed();

            if (Math.Abs(Vx) < 0.001)
            {
                Alpha = 0;
                Beta = 0;
            }
            else
            {
                Alpha = Math.Atan2(Vz, Vx);
                Beta = -Math.Atan2(Vy, Vx);
            }

            // Calculate all forces on the airframe.
            CalculateForces(elapsedSeconds);

            Ax = (float)(Fx / AircraftParameters.Mass);
            Ay = (float)(Fy / AircraftParameters.Mass);
            Az = (float)(Fz / AircraftParameters.Mass);

            float velX0 = Velocity.X; float velY0 = Velocity.Y; float velZ0 = Velocity.Z;
            Vector3 dVelocity = new Vector3(Ax * elapsedSeconds, Ay * elapsedSeconds, Az * elapsedSeconds);
            dVelocity.TransformCoordinate(Matrix.RotationQuaternion(OrientationQuat));
            //Vector3 dVelocity = new Vector3(Ax * elapsedSeconds, Ay * elapsedSeconds, Az * elapsedSeconds).TransformCoordinate(Matrix.RotationQuaternion(OrientationQuat));
            Velocity += dVelocity;
            Vector3 planeVelocity = new Vector3(Velocity.X, Velocity.Y, Velocity.Z);
            planeVelocity.TransformCoordinate(Matrix.RotationQuaternion(Quaternion.Invert(OrientationQuat)));
            Vx = planeVelocity.X;
            Vy = planeVelocity.Y;
            Vz = planeVelocity.Z;

            // Calculate all torques on the airframe
            CalculateTorques(elapsedSeconds);

            AAx = (float)(Tx / AircraftParameters.Ixx);
            AAy = (float)(Ty / AircraftParameters.Iyy);
            AAz = (float)(Tz / AircraftParameters.Izz);

            // Integrate
            float Wx0 = Wx; float Wy0 = Wy; float Wz0 = Wz;
            Wx += AAx * elapsedSeconds;
            Wy += AAy * elapsedSeconds;
            Wz += AAz * elapsedSeconds;

            // Update location
            Vector3 dLoc = new Vector3(
               velX0 * elapsedSeconds + dVelocity.X * elapsedSeconds * 0.5f,
               velY0 * elapsedSeconds + dVelocity.Y * elapsedSeconds * 0.5f,
               velZ0 * elapsedSeconds + dVelocity.Z * elapsedSeconds * 0.5f);

            X += dLoc.X;
            Y += dLoc.Y;
            Z += dLoc.Z;

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

            // collision response
            UpdateCollisions(elapsedSeconds);
        }
        #endregion

        #region Private methods
        private void UpdateAirspeed()
        {
            airSpeedSq = Vx * Vx + Vy * Vy + Vz * Vz;
        }

        /// <summary>
        /// Calculates the total forces on the airframe.
        /// </summary>
        /// <param name="elapsedTime"></param>
        private void CalculateForces(float elapsedTime)
        {
            Lift = GetLiftForce();
            Drag = GetDragForce();
            double sideLift = GetSideLiftForce();
            double sideDrag = GetSideDragForce();
            // verified : gravity, liftX, throttle, liftZ 
            Fx = (float)(+sideLift * Math.Sin(Beta) - sideDrag * Math.Cos(Beta) - Lift * Math.Sin(Alpha) - Drag * Math.Cos(Alpha) + AircraftParameters.MaximumThrust * Throttle - gravityMass * Math.Sin(Pitch));
            Fy = (float)(+sideLift * Math.Cos(Beta) + sideDrag * Math.Sin(Beta) + gravityMass * Math.Sin(Roll) * Math.Cos(Pitch));
            Fz = (float)(-Lift * Math.Cos(Alpha) - Drag * Math.Sin(Alpha) + gravityMass * Math.Cos(Roll) * Math.Cos(Pitch));
        }

        /// <summary>
        /// Calculates the lift force.
        /// </summary>
        /// <returns></returns>
        private double GetLiftForce()
        {
            // F = (1/2)*Cl*V^2*S
            return AircraftParameters.GetLiftCoefficient(Alpha) * (Vx * Vx + Vz * Vz) * rhoSurface;
        }

        /// <summary>
        /// Calculates the drag force.
        /// </summary>
        /// <returns></returns>
        private double GetDragForce()
        {
            // F = (1/2)*Dc*V^2*S
            return AircraftParameters.GetDragCoefficient(Alpha) * (Vx * Vx + Vz * Vz) * rhoSurface;
        }

        /// <summary>
        /// Calculates the lift force generated by the fuselage and vertical tail.
        /// </summary>
        /// <returns></returns>
        private double GetSideLiftForce()
        {
            return AircraftParameters.GetSideLiftCoefficient(Beta) * (Vx * Vx + Vy * Vy) * rhoVerticalSurface;
        }

        /// <summary>
        /// Calculates the drag force generated by the fuselage and vertical tail.
        /// </summary>
        /// <returns></returns>
        private double GetSideDragForce()
        {
            return AircraftParameters.GetSideDragCoefficient(Beta) * (Vx * Vx + Vy * Vy) * rhoVerticalSurface;
        }

        /// <summary>
        /// Calculate the torques along the different axis of the airframe.
        /// </summary>
        /// <param name="elapsedTime"></param>
        private void CalculateTorques(float elapsedTime)
        {
            // Torque = F * r * sin(alpha)
            Tx = (float)((-AircraftParameters.RollDamping * Wx + AircraftParameters.AileronEfficiency * Vx * Ailerons + AircraftParameters.DihedralAngle * AircraftParameters.DihedralEfficiency * Vx * Beta) * Vx);
            Ty = (float)((-AircraftParameters.PitchDamping * Wy - Alpha * Math.Sign(Vx) * AircraftParameters.PitchStability + AircraftParameters.ElevatorEfficiency * Vx * Elevator) * Vx);
            Tz = (float)((-AircraftParameters.YawDamping * Wz - Beta * Math.Sign(Vx) * AircraftParameters.YawStability + AircraftParameters.RudderEfficiency * Vx * Rudder) * Vx);
        }

        private void UpdateCollisions(float elapsedTime)
        {
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
                    contactPoint = (0.5f * (contactList[0].ContactPoint + contactList[1].ContactPoint));
                    normal = (0.5f * (contactList[0].Normal + contactList[1].Normal));
                    normalW = (0.5f * (contactList[0].NormalW + contactList[1].NormalW));
                    depth = (contactList[0].Depth + contactList[1].Depth)/2;
                }
                else
                {
                    // plane
                    
                    //contactPoint = (0.5f * (contactList[0].ContactPoint + contactList[1].ContactPoint));
                    normal = (0.5f * (contactList[0].Normal + contactList[1].Normal));
                    normalW = (0.5f * (contactList[0].NormalW + contactList[1].NormalW));
                    depth = (contactList[0].Depth + contactList[1].Depth) / 2;
                    contactPoint = -depth * normal;
                }
                Vector3 pointVel = planeVelocity + Vector3.Cross(rotationalVelocity, contactPoint);
                float epsilon = 0.7f; // restitution
                float j = ((-1 - epsilon) * Vector3.Dot(planeVelocity, normal)) /
                    ((Vector3.Dot(normal, normal) / (float)AircraftParameters.Mass) +
                     (Vector3.Dot(MultiplyInertiaInverse(Vector3.Cross(Vector3.Cross(contactPoint, normal), contactPoint)), normal)));
                Z -= depth;
                if (Vector3.Dot(normal, pointVel) > 0)
                {
                    
                }
                else
                {
                    //Z -= depth;
#if DEBUG
                    Framework.Instance.DebugString += j + "\n";
                    Framework.Instance.DebugString += normalW + "\n";
                    Framework.Instance.DebugString += contactList.Count;
#endif
                    Velocity += (j / (float)AircraftParameters.Mass) * normalW;
                    //Velocity += (-Velocity * 1.0f* elapsedTime);
                    Vector3 rotationVelDiff = MultiplyInertiaInverse(Vector3.Cross(contactPoint, j * normal));
                    Wx += rotationVelDiff.X;
                    Wy += rotationVelDiff.Y;
                    Wz += rotationVelDiff.Z;
                }
            }       
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

        private Vector3 ToDirectX(Vector3 source)
        {
            return new Vector3(-source.Y, -source.Z, -source.X);
        }

        private Vector3 ToModel(Vector3 source)
        {
            return new Vector3(-source.Z, -source.X, -source.Y);
        }
        #endregion
    }
}
