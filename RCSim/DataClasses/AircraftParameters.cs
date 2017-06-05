using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using Bonsai.Core;
using Microsoft.DirectX;
using System.Xml.XPath;
using System.Globalization;
using System.IO;

namespace RCSim.DataClasses
{
    internal class AircraftParameters
    {
        #region Public structs and enums
        public enum ChannelEnum
        {
            None,
            Elevator,
            Throttle,
            Aileron,
            Rudder,
            Flaps,
            Gear
        }

        public enum ControlSurfaceTypeEnum
        {
            Normal = 0,
            Reflective,
            PropLowRPM,
            PropHighRPM,
            PropFoldingLowRPM,
            PropFolded,
            RotorLowRPM,
            RotorHighRPM,
            TailrotorLowRPM,
            TailrotorHighRPM            
        }

        public enum FlightModelTypeEnum
        {
            Aircraft,
            Helicopter,
            HelicopterCoax,
            Sailboat
        }

        public class ControlSurface
        {
            public string Filename;
            public Vector3 Position;
            public Vector3 RotationAxis;
            public double MinimumAngle;
            public double ZeroAngle;
            public double MaximumAngle;
            public ChannelEnum Channel;
            public bool Reversed;
            public ControlSurfaceTypeEnum Type;
            public float Scale = 1;
            public List<ControlSurface> ChildControlSurfaces;
        }
        #endregion

        #region public properties
        /// <summary>
        /// The version of the flightmodel.
        /// </summary>
        public double Version { get; set; }

        #region Flightmodel version 1.0
        /// <summary>
        /// The mass of the airplane in Kg.
        /// </summary>
        public double Mass { get; set; }
        /// <summary>
        /// Moment of inertia around the roll axis.
        /// </summary>
        public double Ixx { get; set; }
        /// <summary>
        /// Moment of inertia around the pitch axis.
        /// </summary>
        public double Iyy { get; set; }
        /// <summary>
        /// Moment of inertia around the yaw axis.
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
        /// Gets/sets the location of the center of gravity in relation to the center of lift.
        /// </summary>
        public double CenterOfGravity { get; set; }
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
        /// Gets the vulnerability to spins.
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
        /// Reference points for the Lift Coefficient for alpha in [-Pi, Pi[ with flaps extended.
        /// </summary>
        public List<KeyValuePair<double, double>> LiftFlapsCoefficientPoints { get; set; }
        /// <summary>
        /// Reference points for the Drag Coefficient for alpha in [-Pi, Pi[ with flaps extended.
        /// </summary>
        public List<KeyValuePair<double, double>> DragFlapsCoefficientPoints { get; set; }
        /// <summary>
        /// The thrustcoefficient in relation to the speed.
        /// </summary>
        public List<KeyValuePair<double, double>> ThrustCoeffecientPoints { get; set; }

        /// <summary>
        /// The touchdown point of the landing gear.
        /// </summary>
        public List<Vector3> GearPoints { get; set; }
        /// <summary>
        /// The unscaled collission points.
        /// </summary>
        public List<Vector3> UnscaledGearPoints { get; set; }
        /// <summary>
        /// The points of the plane that define the float plane.
        /// </summary>
        public List<Vector3> FloatPoints { get; set; }
        /// <summary>
        /// The unscaled float points.
        /// </summary>
        public List<Vector3> UnscaledFloatPoints { get; set; }
        /// <summary>
        /// The collision points of the airframe.
        /// </summary>
        public List<Vector3> CollisionPoints { get; set; }
        /// <summary>
        /// The unscaled collission points.
        /// </summary>
        public List<Vector3> UnscaledCollisionPoints { get; set; }


        /// <summary>
        /// The scale of the geometry points
        /// </summary>
        public float Scale { get; set; }
        /// <summary>
        /// Gets/sets the filename of the engine sound.
        /// </summary>
        public string EngineSound { get; set; }
        /// <summary>
        /// Gets/sets the filename of the rotor sound.
        /// </summary>
        public string RotorSound { get; set; }
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
        /// Gets/sets the number of seconds it takes to extend/retract the flaps.
        /// </summary>
        public double FlapsDelay { get; set; }
        /// <summary>
        /// Gets/sets the number of seconds it takes to extend/retract the gear.
        /// </summary>
        public double GearDelay { get; set; }
        /// <summary>
        /// Gets/sets the restitution at collisions.
        /// </summary>
        public float Restitution { get; set; }
        /// <summary>
        /// Gets/sets the resistance against crashes.
        /// </summary>
        public double CrashResistance { get; set; }
        /// <summary>
        /// Gets/sets the resistance against crashes of the landing gear.
        /// </summary>
        public double CrashResistanceGear { get; set; }
        /// <summary>
        /// Gets/sets the number of channels.
        /// </summary>
        public int Channels { get; set; }
        /// <summary>
        /// Gets/sets whether the aircraft should be handlaunched.
        /// </summary>
        public bool HandLaunched { get; set; }
        /// <summary>
        /// Gets/Sets whether the aircraft is equipped with a variometer.
        /// </summary>
        public bool HasVariometer { get; set; }
        /// <summary>
        /// Gets/Sets whether the aircraft is equipped with flaps.
        /// </summary>
        public bool HasFlaps { get; set; }
        /// <summary>
        /// Gets/Sets whether the aircraft is equipped with retractable landing gear.
        /// </summary>
        public bool HasRetracts { get; set; }
        /// <summary>
        /// Whether or not the aircraft allows to be towed.
        /// </summary>
        public bool AllowsTowing { get; set; }
        /// <summary>
        /// Gets/Sets whether the aircraft has floats.
        /// </summary>
        public bool HasFloats { get; set; }

        /// <summary>
        /// Gets/sets the fixed part of the mesh.
        /// </summary>
        public string FixedMesh { get; set; }
        /// <summary>
        /// Gets/sets the description of the airplane.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets/sets the list of control surfaces.
        /// </summary>
        public List<ControlSurface> ControlSurfaces { get; set; }

        /// <summary>
        /// Sets the file containing the parameters.
        /// </summary>
        public string File
        {
            set { ReadParameters(value); }
        }

        /// <summary>
        /// Gets/sets the filename containing the aircraft parameters.
        /// </summary>
        public string FileName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the folder of the original par file.
        /// </summary>
        public string FolderName
        {
            get { return Utility.AppendDirectorySeparator(new System.IO.FileInfo(FileName).DirectoryName); }
        }

        /// <summary>
        /// Gets/sets the icon to display in the menu.
        /// </summary>
        public string IconFile
        {
            get;
            set;
        }

        /// <summary>
        /// Gets/Sets the location of the ad of this plane.
        /// </summary>
        public string AdLocation { get; set; }

        /// <summary>
        /// Gets/Sets the url where to buy the airplane.
        /// </summary>
        public string BuyUrl { get; set; }

        /// <summary>
        /// Gets/Sets the type of flightmodel to use.
        /// </summary>
        public FlightModelTypeEnum FlightModelType { get; set; }

        /// <summary>
        /// Gets/Sets the drag of the fuselage on the X-axis.
        /// </summary>
        public float FuselageDragX { get; set; }
        /// <summary>
        /// Gets/Sets the drag of the fuselage on the Y-axis.
        /// </summary>
        public float FuselageDragY { get; set; }
        /// <summary>
        /// Gets/Sets the drag of the fuselage on the Z-axis.
        /// </summary>
        public float FuselageDragZ { get; set; }
        /// <summary>
        /// Gets/Sets the drag experienced while on the ground.
        /// </summary>
        public float GroundDrag { get; set; }
        /// <summary>
        /// Gets/Sets the drag experienced while on the water.
        /// </summary>
        public float WaterDrag { get; set; }
        #endregion
        #region Flightmodel version 2.0
        /// <summary>
        /// Gets the vector of the aerodynamic center of the wing from the CoG.
        /// </summary>
        public Vector3 WingCenter { get; set; }
        /// <summary>
        /// Gets/sets the wingspan in relation to the wing center.
        /// </summary>
        public float WingSpanFactor { get; set; }
        /// <summary>
        /// Gets the vector to the center of the prop from the CoG.
        /// </summary>
        public Vector3 PropCenter { get; set; }
        #endregion
        #endregion

        #region Public methods
        /// <summary>
        /// Gets the LiftCoefficient at given angle of attack
        ///         |
        ///         |
        ///         |     /\
        ///         |    /  \
        ///         |   /    \
        ///         |  /      \_____________
        /// --------+-/----------------------
        /// ----\   |/
        ///      \  /
        ///       \/ 
        /// </summary>
        /// <param name="alpha"></param>
        /// <returns></returns>
        public double GetLiftCoefficient(double alpha)
        {
            double modAlpha = Utility.NormalizeAngle(alpha);
            return Utility.Interpolate(LiftCoefficientPoints, modAlpha);
        }

        /// <summary>
        /// Gets the DragCoeffient at given angle of attack
        /// </summary>
        /// <param name="alpha"></param>
        /// <returns></returns>
        public double GetDragCoefficient(double alpha)
        {
            double modAlpha = Utility.NormalizeAngle(alpha);
            return Utility.Interpolate(DragCoefficientPoints, modAlpha);
        }

        /// <summary>
        /// Gets the LiftCoefficient from the vertical area at given sideslip angle.
        /// </summary>
        /// <param name="beta"></param>
        /// <returns></returns>
        public double GetSideLiftCoefficient(double beta)
        {
            double modBeta = Utility.NormalizeAngle(beta);
            return Utility.Interpolate(SideLiftCoefficientPoints, modBeta);
        }

        /// <summary>
        /// Gets the DragCoefficient from the vertical area at given sideslip angle.
        /// </summary>
        /// <param name="beta"></param>
        /// <returns></returns>
        public double GetSideDragCoefficient(double beta)
        {
            double modBeta = Utility.NormalizeAngle(beta);
            return Utility.Interpolate(SideDragCoefficientPoints, modBeta);
        }

        /// <summary>
        /// Gets the LiftCoefficient at given angle of attack with flaps extended
        /// </summary>
        /// <param name="alpha"></param>
        /// <returns></returns>
        public double GetLiftCoefficientWithFlaps(double alpha)
        {
            double modAlpha = Utility.NormalizeAngle(alpha);
            return Utility.Interpolate(LiftFlapsCoefficientPoints, modAlpha);
        }

        /// <summary>
        /// Gets the DragCoeffient at given angle of attack with flaps extended
        /// </summary>
        /// <param name="alpha"></param>
        /// <returns></returns>
        public double GetDragCoefficientWithFlaps(double alpha)
        {
            double modAlpha = Utility.NormalizeAngle(alpha);
            return Utility.Interpolate(DragFlapsCoefficientPoints, modAlpha);
        }

        /// <summary>
        /// Gets the thrustcoefficient at the given speed.
        /// </summary>
        /// <param name="speed"></param>
        /// <returns></returns>
        public double GetThrustCoefficient(double speed)
        {
            return Utility.Interpolate(ThrustCoeffecientPoints, speed);
        }

        public double GetElevatorControl(double alpha, double flapsAngle)
        {
            double modAlpha = Utility.NormalizeAngle(alpha);
            if (modAlpha >= 0)
            {
                double maxAlpha = Utility.GetMaximumKey(LiftCoefficientPoints);
                if (modAlpha > maxAlpha)
                    return 0.0;
                else
                    return 1.0;
            }
            else
            {
                double minAlpha = Utility.GetMinimumKey(LiftCoefficientPoints);
                if (modAlpha < minAlpha)
                    return 0.0;
                else
                    return 1.0;
            }
        }

        public void Save(string filename)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine("<?xml version=\"1.0\" standalone=\"yes\" ?>");
                writer.WriteLine("<aircraft>");
                WriteDouble(writer, "version", Version);
                if (FlightModelType == FlightModelTypeEnum.Aircraft)
                {
                    writer.WriteLine("<type>airplane</type>");
                }
                else if (FlightModelType == FlightModelTypeEnum.Helicopter)
                {
                    writer.WriteLine("<type>helicopter</type>");
                }

                // The FlightModel part
                writer.WriteLine("<flightmodel>");
                WriteDouble(writer, "mass", Mass);
                WriteDouble(writer, "Ixx", Ixx);
                WriteDouble(writer, "Iyy", Iyy);
                WriteDouble(writer, "Izz", Izz);
                WriteDouble(writer, "wingarea", WingArea);
                WriteDouble(writer, "verticalarea", VerticalArea);
                WriteDouble(writer, "maximumthrust", MaximumThrust);
                WriteDouble(writer, "dihedralangle", DihedralAngle);
                WriteDouble(writer, "dihedralefficiency", DihedralEfficiency);
                WriteDouble(writer, "rolldamping", RollDamping);
                WriteDouble(writer, "aileronefficiency", AileronEfficiency);
                WriteDouble(writer, "pitchdamping", PitchDamping);
                WriteDouble(writer, "elevatorefficiency", ElevatorEfficiency);
                WriteDouble(writer, "centerofgravity", CenterOfGravity);
                WriteDouble(writer, "yawdamping", YawDamping);
                WriteDouble(writer, "rudderefficiency", RudderEfficiency);
                WriteDouble(writer, "pitchstability", PitchStability);
                WriteDouble(writer, "pitchtrim", PitchTrim);
                WriteDouble(writer, "yawstability", YawStability);
                WriteDouble(writer, "propwashailerons", PropWashAilerons);
                WriteDouble(writer, "propwashelevator", PropWashElevator);
                WriteDouble(writer, "propwashrudder", PropWashRudder);
                WriteDouble(writer, "spinfactor", SpinFactor);
                WriteDouble(writer, "grounddrag", GroundDrag);
                WriteDouble(writer, "waterdrag", WaterDrag);
                WriteCoefficients(writer, "liftcoefficientpoint", "angle", "coefficient", LiftCoefficientPoints);
                WriteCoefficients(writer, "dragcoefficientpoint", "angle", "coefficient", DragCoefficientPoints);
                WriteCoefficients(writer, "sideliftcoefficientpoint", "angle", "coefficient", SideLiftCoefficientPoints);
                WriteCoefficients(writer, "sidedragcoefficientpoint", "angle", "coefficient", SideDragCoefficientPoints);
                if (HasFlaps)
                {
                    WriteCoefficients(writer, "liftflapscoefficientpoint", "angle", "coefficient", LiftFlapsCoefficientPoints);
                    WriteCoefficients(writer, "dragflapscoefficientpoint", "angle", "coefficient", DragFlapsCoefficientPoints);
                }
                WriteCoefficients(writer, "thrustcoefficientpoint", "speed", "coefficient", ThrustCoeffecientPoints);
                writer.WriteLine("</flightmodel>");

                if (Version == 2)
                {
                    writer.WriteLine("<flightmodel2>");
                    WriteVector(writer, "wingcenter", WingCenter, 1.0f);
                    WriteDouble(writer, "wingspanfactor", WingSpanFactor);
                    WriteVector(writer, "propcenter", PropCenter, 1.0f);
                    WriteDouble(writer, "aileronefficiency", AileronEfficiency);
                    writer.WriteLine("</flightmodel2>");
                }

                // The Model part
                writer.WriteLine("<model>");
                WriteDouble(writer, "scale", (double)Scale);
                WriteString(writer, "enginesound", EngineSound);
                WriteInteger(writer, "engineminfrequency", EngineMinFrequency);
                WriteInteger(writer, "enginemaxfrequency", EngineMaxFrequency);
                WriteDouble(writer, "throttledelay", ThrottleDelay);
                WriteDouble(writer, "flapsdelay", FlapsDelay);
                WriteDouble(writer, "geardelay", GearDelay);
                WriteDouble(writer, "restitution", Restitution);
                WriteDouble(writer, "crashresistance", CrashResistance);
                WriteDouble(writer, "crashresistancegear", CrashResistanceGear);
                WriteInteger(writer, "channels", Channels);
                WriteBoolean(writer, "handlaunched", HandLaunched);
                WriteBoolean(writer, "hasvariometer", HasVariometer);
                WriteBoolean(writer, "hasflaps", HasFlaps);
                WriteBoolean(writer, "hasretracts", HasRetracts);
                WriteBoolean(writer, "hasfloats", HasFloats);
                WriteBoolean(writer, "allowstowing", AllowsTowing);
                WriteString(writer, "fixedmesh", FixedMesh);
                WriteString(writer, "description", Description);
                WriteString(writer, "icon", IconFile);
                WriteVectorList(writer, "gearpoint", UnscaledGearPoints);
                WriteVectorList(writer, "collisionpoint", UnscaledCollisionPoints);
                if (HasFloats)
                    WriteVectorList(writer, "floatpoint", UnscaledFloatPoints);
                WriteControlSurfaceList(writer, ControlSurfaces);
                writer.WriteLine("</model>");
                writer.WriteLine("</aircraft>");
            }
        }

        public void CreateDefault(string filename)
        {
            this.Version = 1.0;
            this.FileName = filename;
            this.Mass = 1.0;
            this.Ixx = 0.3;
            this.Iyy = 0.3;
            this.Izz = 0.6;
            this.WingArea = 0.3;
            this.VerticalArea = 0.2;
            this.MaximumThrust = 10;
            this.DihedralAngle = 0;
            this.DihedralEfficiency = 0;
            this.RollDamping = 0.05;
            this.AileronEfficiency = 0.01;
            this.PitchDamping = 0.05;
            this.ElevatorEfficiency = 0.02;
            this.CenterOfGravity = 0.0;
            this.YawDamping = 0.10;
            this.RudderEfficiency = 0.02;
            this.PitchStability = 1.0;
            this.PitchTrim = 0.0;
            this.YawStability = 1.0;
            this.PropWashAilerons = 0.0;
            this.PropWashElevator = 0.0;
            this.PropWashRudder = 0.0;
            this.SpinFactor = 0.0;
            LiftCoefficientPoints = new List<KeyValuePair<double, double>>();
            LiftCoefficientPoints.Add(new KeyValuePair<double,double>(-Math.PI, 0.0));
            LiftCoefficientPoints.Add(new KeyValuePair<double,double>(-2.35, 1.0));
            LiftCoefficientPoints.Add(new KeyValuePair<double,double>(-0.78, -1.1));
            LiftCoefficientPoints.Add(new KeyValuePair<double,double>(-0.30, -0.6));
            LiftCoefficientPoints.Add(new KeyValuePair<double,double>(-0.26, -1.2));
            LiftCoefficientPoints.Add(new KeyValuePair<double,double>(0.0,0.0));
            LiftCoefficientPoints.Add(new KeyValuePair<double,double>(0.26, 1.2));
            LiftCoefficientPoints.Add(new KeyValuePair<double,double>(0.30, 0.8));
            LiftCoefficientPoints.Add(new KeyValuePair<double,double>(0.78, 1.1));
            LiftCoefficientPoints.Add(new KeyValuePair<double,double>(2.35, -1.0));
            LiftCoefficientPoints.Add(new KeyValuePair<double,double>(Math.PI, 0.0));
            DragCoefficientPoints = new List<KeyValuePair<double, double>>();
            DragCoefficientPoints.Add(new KeyValuePair<double,double>(-Math.PI, 0.06));
            DragCoefficientPoints.Add(new KeyValuePair<double,double>(-1.57, 1.8));
            DragCoefficientPoints.Add(new KeyValuePair<double,double>(-1.04, 1.5));
            DragCoefficientPoints.Add(new KeyValuePair<double,double>(-0.30, 0.15));
            DragCoefficientPoints.Add(new KeyValuePair<double,double>(-0.26, 0.05));
            DragCoefficientPoints.Add(new KeyValuePair<double,double>(0.0, 0.027));
            DragCoefficientPoints.Add(new KeyValuePair<double,double>(0.26, 0.05));
            DragCoefficientPoints.Add(new KeyValuePair<double,double>(0.30, 0.15));
            DragCoefficientPoints.Add(new KeyValuePair<double,double>(1.04, 1.5));
            DragCoefficientPoints.Add(new KeyValuePair<double,double>(1.57, 1.8));
            DragCoefficientPoints.Add(new KeyValuePair<double,double>(Math.PI, 0.06));
            SideLiftCoefficientPoints = new List<KeyValuePair<double, double>>();
            SideLiftCoefficientPoints.Add(new KeyValuePair<double, double>(-Math.PI, 0.0));
            SideLiftCoefficientPoints.Add(new KeyValuePair<double, double>(-1.0, -0.1));
            SideLiftCoefficientPoints.Add(new KeyValuePair<double, double>(-0.5, -0.4));
            SideLiftCoefficientPoints.Add(new KeyValuePair<double, double>(0.0, 0.0));
            SideLiftCoefficientPoints.Add(new KeyValuePair<double, double>(0.5, 0.4));
            SideLiftCoefficientPoints.Add(new KeyValuePair<double, double>(1.0, 0.1));
            SideLiftCoefficientPoints.Add(new KeyValuePair<double, double>(Math.PI, 0.0));
            SideDragCoefficientPoints = new List<KeyValuePair<double, double>>();
            SideDragCoefficientPoints.Add(new KeyValuePair<double, double>(-Math.PI, 0.02));
            SideDragCoefficientPoints.Add(new KeyValuePair<double, double>(-1.57, 1.0));
            SideDragCoefficientPoints.Add(new KeyValuePair<double, double>(0.0, 0.02));
            SideDragCoefficientPoints.Add(new KeyValuePair<double, double>(1.57, 1.0));
            SideDragCoefficientPoints.Add(new KeyValuePair<double, double>(Math.PI, 0.02));
            LiftFlapsCoefficientPoints = new List<KeyValuePair<double, double>>();
            LiftFlapsCoefficientPoints.Add(new KeyValuePair<double, double>(-Math.PI, 0.0));
            LiftFlapsCoefficientPoints.Add(new KeyValuePair<double, double>(-2.35, 1.0));
            LiftFlapsCoefficientPoints.Add(new KeyValuePair<double, double>(-0.78, -0.9));
            LiftFlapsCoefficientPoints.Add(new KeyValuePair<double, double>(-0.36, -0.3));
            LiftFlapsCoefficientPoints.Add(new KeyValuePair<double, double>(-0.30, -0.9));
            LiftFlapsCoefficientPoints.Add(new KeyValuePair<double, double>(-0.1, 0.0));
            LiftFlapsCoefficientPoints.Add(new KeyValuePair<double, double>(0.20, 1.3));
            LiftFlapsCoefficientPoints.Add(new KeyValuePair<double, double>(0.30, 0.8));
            LiftFlapsCoefficientPoints.Add(new KeyValuePair<double, double>(0.78, 1.1));
            LiftFlapsCoefficientPoints.Add(new KeyValuePair<double, double>(2.35, -1.0));
            LiftFlapsCoefficientPoints.Add(new KeyValuePair<double, double>(Math.PI, 0.0));
            DragFlapsCoefficientPoints = new List<KeyValuePair<double, double>>();
            DragFlapsCoefficientPoints.Add(new KeyValuePair<double, double>(-Math.PI, 0.06));
            DragFlapsCoefficientPoints.Add(new KeyValuePair<double, double>(-1.57, 1.8));
            DragFlapsCoefficientPoints.Add(new KeyValuePair<double, double>(-1.04, 1.5));
            DragFlapsCoefficientPoints.Add(new KeyValuePair<double, double>(-0.30, 0.15));
            DragFlapsCoefficientPoints.Add(new KeyValuePair<double, double>(-0.26, 0.05));
            DragFlapsCoefficientPoints.Add(new KeyValuePair<double, double>(0.0, 0.04));
            DragFlapsCoefficientPoints.Add(new KeyValuePair<double, double>(0.26, 0.065));
            DragFlapsCoefficientPoints.Add(new KeyValuePair<double, double>(0.30, 0.25));
            DragFlapsCoefficientPoints.Add(new KeyValuePair<double, double>(1.04, 1.5));
            DragFlapsCoefficientPoints.Add(new KeyValuePair<double, double>(1.57, 1.8));
            DragFlapsCoefficientPoints.Add(new KeyValuePair<double, double>(Math.PI, 0.06));
            ThrustCoeffecientPoints = new List<KeyValuePair<double, double>>();
            ThrustCoeffecientPoints.Add(new KeyValuePair<double, double>(0.0, 1.00));
            ThrustCoeffecientPoints.Add(new KeyValuePair<double, double>(100.0, 0.00));
            this.FlightModelType = FlightModelTypeEnum.Aircraft;
            this.GroundDrag = 0.05f;
            this.WaterDrag = 0.10f;
            this.AdLocation = null;
            this.IconFile = null;
            this.BuyUrl = null;
            this.Scale = 1.0f;
            this.WingSpanFactor = 1.0f;
            this.GearPoints = new List<Vector3>();
            this.FloatPoints = new List<Vector3>();
            this.EngineSound = null;
            this.RotorSound = null;
            this.EngineMinFrequency = 21000;
            this.EngineMaxFrequency = 21000;
            this.ThrottleDelay = 10.0;
            this.FlapsDelay = 2.0;
            this.GearDelay = 2.0;
            this.Restitution = 0.4f;
            this.CrashResistance = 2.0;
            this.CrashResistanceGear = 5.0;
            this.Channels = 4;
            this.HandLaunched = false;
            this.HasVariometer = false;
            this.HasFlaps = false;
            this.HasRetracts = false;
            this.AllowsTowing = false;
            this.FixedMesh = null;
            GearPoints = new List<Vector3>();
            UnscaledGearPoints = new List<Vector3>();
            FloatPoints = new List<Vector3>();
            UnscaledFloatPoints = new List<Vector3>();
            CollisionPoints = new List<Vector3>();
            UnscaledCollisionPoints = new List<Vector3>();
            ControlSurfaces = new List<ControlSurface>();
            this.Description = "This is my airplane";
        }

        public void ReadParameters(string filename)
        {
            FileName = filename;
            XPathDocument doc = new XPathDocument(filename);
            XPathNavigator nav = doc.CreateNavigator();
            // Read in the flightmodel
            this.Version = ReadDouble(nav, "//version", 1.0);
            this.Mass = nav.SelectSingleNode("//flightmodel/mass").ValueAsDouble;
            this.Ixx = nav.SelectSingleNode("//flightmodel/Ixx").ValueAsDouble;
            this.Iyy = nav.SelectSingleNode("//flightmodel/Iyy").ValueAsDouble;
            this.Izz = nav.SelectSingleNode("//flightmodel/Izz").ValueAsDouble;
            this.WingArea = nav.SelectSingleNode("//flightmodel/wingarea").ValueAsDouble;
            this.VerticalArea = nav.SelectSingleNode("//flightmodel/verticalarea").ValueAsDouble;
            this.MaximumThrust = nav.SelectSingleNode("//flightmodel/maximumthrust").ValueAsDouble;
            this.DihedralAngle = nav.SelectSingleNode("//flightmodel/dihedralangle").ValueAsDouble;
            this.DihedralEfficiency = nav.SelectSingleNode("//flightmodel/dihedralefficiency").ValueAsDouble;
            this.RollDamping = nav.SelectSingleNode("//flightmodel/rolldamping").ValueAsDouble;
            this.AileronEfficiency = nav.SelectSingleNode("//flightmodel/aileronefficiency").ValueAsDouble;
            this.PitchDamping = nav.SelectSingleNode("//flightmodel/pitchdamping").ValueAsDouble;
            this.ElevatorEfficiency = nav.SelectSingleNode("//flightmodel/elevatorefficiency").ValueAsDouble;
            this.CenterOfGravity = ReadDouble(nav, "//flightmodel/centerofgravity", 0.0);
            this.YawDamping = nav.SelectSingleNode("//flightmodel/yawdamping").ValueAsDouble;
            this.RudderEfficiency = nav.SelectSingleNode("//flightmodel/rudderefficiency").ValueAsDouble;
            this.PitchStability = nav.SelectSingleNode("//flightmodel/pitchstability").ValueAsDouble;
            this.PitchTrim = nav.SelectSingleNode("//flightmodel/pitchtrim").ValueAsDouble;
            this.YawStability = nav.SelectSingleNode("//flightmodel/yawstability").ValueAsDouble;
            this.PropWashAilerons = nav.SelectSingleNode("//flightmodel/propwashailerons").ValueAsDouble;
            this.PropWashElevator = nav.SelectSingleNode("//flightmodel/propwashelevator").ValueAsDouble;
            this.PropWashRudder = nav.SelectSingleNode("//flightmodel/propwashrudder").ValueAsDouble;
            this.SpinFactor = nav.SelectSingleNode("//flightmodel/spinfactor").ValueAsDouble;
            LiftCoefficientPoints = ReadCoefficientList(nav, "//liftcoefficientpoints/liftcoefficientpoint", "angle");
            DragCoefficientPoints = ReadCoefficientList(nav, "//dragcoefficientpoints/dragcoefficientpoint", "angle");
            SideLiftCoefficientPoints = ReadCoefficientList(nav, "//sideliftcoefficientpoints/sideliftcoefficientpoint", "angle");
            SideDragCoefficientPoints = ReadCoefficientList(nav, "//sidedragcoefficientpoints/sidedragcoefficientpoint", "angle");
            ThrustCoeffecientPoints = ReadCoefficientList(nav, "//thrustcoefficientpoints/thrustcoefficientpoint", "speed");
            this.FlightModelType = ReadFlightModelType(nav, "//flightmodel/flightmodeltype");
            this.FuselageDragX = ReadFloat(nav, "//flightmodel/fuselagedragx", 0f);
            this.FuselageDragY = ReadFloat(nav, "//flightmodel/fuselagedragy", 0f);
            this.FuselageDragZ = ReadFloat(nav, "//flightmodel/fuselagedragz", 0f);
            this.GroundDrag = ReadFloat(nav, "//flightmodel/grounddrag", 0.05f);
            this.WaterDrag = ReadFloat(nav, "//flightmodel/waterdrag", this.GroundDrag);
            this.Scale = (float)nav.SelectSingleNode("//model/scale").ValueAsDouble;

            // Read in version 2 parameters
            if (Version == 2.0)
            {
                this.WingCenter = ReadVector(nav, "//flightmodel2/wingcenter", 1.0f, new Vector3(0, 0, 0));
                this.WingSpanFactor = ReadFloat(nav, "//flightmodel2/wingspanfactor", 1f);
                this.PropCenter = ReadVector(nav, "//flightmodel2/propcenter", 1.0f, new Vector3(0, 0, 0));
                this.AileronEfficiency = ReadDouble(nav, "//flightmodel2/aileronefficiency", this.AileronEfficiency);
            }

            int code = ReadInt(nav, "//model/code", 0);
            this.AdLocation = VerifyCode(code) ? ReadString(nav, "//model/ad", null) : null;
            this.IconFile = ReadString(nav, "//model/icon", "");
            this.BuyUrl = ReadString(nav, "//model/buyurl", null);

            // Read in the mesh            
            this.HasFloats = ReadBool(nav, "//model/hasfloats", false);
            this.GearPoints = ReadVectorList(nav, "//model/gearpoints/gearpoint", Scale);
            this.UnscaledGearPoints = ReadVectorList(nav, "//model/gearpoints/gearpoint", 1);
            if (HasFloats)
            {
                this.FloatPoints = ReadVectorList(nav, "//model/floatpoints/floatpoint", Scale);
                this.UnscaledFloatPoints = ReadVectorList(nav, "//model/floatpoints/floatpoint", 1);
            }
            else
            {
                this.FloatPoints = new List<Vector3>();
                this.UnscaledFloatPoints = new List<Vector3>();
            }
            this.CollisionPoints = ReadVectorList(nav, "//model/collisionpoints/collisionpoint", Scale);
            this.UnscaledCollisionPoints = ReadVectorList(nav, "//model/collisionpoints/collisionpoint", 1);
            this.EngineSound = ReadString(nav, "//model/enginesound", null);
            this.RotorSound = ReadString(nav, "//model/rotorsound", null);
            this.EngineMinFrequency = Convert.ToInt32(nav.SelectSingleNode("//model/engineminfrequency").Value);
            this.EngineMaxFrequency = Convert.ToInt32(nav.SelectSingleNode("//model/enginemaxfrequency").Value);
            this.ThrottleDelay = Convert.ToDouble(nav.SelectSingleNode("//model/throttledelay").Value);
            this.FlapsDelay = ReadDouble(nav, "//model/flapsdelay", 2.0f);
            this.GearDelay = ReadDouble(nav, "//model/geardelay", 2.0f);
            this.Restitution = (float)nav.SelectSingleNode("//model/restitution").ValueAsDouble;
            this.CrashResistance = nav.SelectSingleNode("//model/crashresistance").ValueAsDouble;
            this.CrashResistanceGear = ReadDouble(nav, "//model/crashresistancegear", 3.0);
            this.Channels = nav.SelectSingleNode("//model/channels").ValueAsInt;
            this.HandLaunched = nav.SelectSingleNode("//model/handlaunched").ValueAsBoolean;
            this.HasVariometer = ReadBool(nav, "//model/hasvariometer", false);
            this.HasFlaps = ReadBool(nav, "//model/hasflaps", false);
            this.AllowsTowing = ReadBool(nav, "//model/allowstowing", false);
            if (HasFlaps)
            {
                LiftFlapsCoefficientPoints = ReadCoefficientList(nav, "//liftflapscoefficientpoints/liftflapscoefficientpoint", "angle");
                DragFlapsCoefficientPoints = ReadCoefficientList(nav, "//dragflapscoefficientpoints/dragflapscoefficientpoint", "angle");
            }
            else
            {
                LiftFlapsCoefficientPoints = new List<KeyValuePair<double,double>>();
                foreach (KeyValuePair<double, double> keyValue in LiftCoefficientPoints)
                    LiftFlapsCoefficientPoints.Add(keyValue);
                DragFlapsCoefficientPoints = new List<KeyValuePair<double, double>>();
                foreach (KeyValuePair<double, double> keyValue in DragCoefficientPoints)
                    DragFlapsCoefficientPoints.Add(keyValue);
            }
            this.HasRetracts = ReadBool(nav, "//model/hasretracts", false);
            this.FixedMesh = ReadString(nav, "//model/fixedmesh", null);
            ControlSurfaces = ReadControlSurfaces(nav, "//model/controlsurfaces/controlsurface", Scale);

            // Miscellaneous
            this.Description = ClearDescriptionString(nav.SelectSingleNode("//model/description").ToString());
        }

        private bool VerifyCode(int code)
        {
            char[] chararray = Utility.GetFileNamePart(FileName).ToCharArray();
            int hash = 0;
            int i = 1;
            foreach (char c in chararray)
            {
                int ic = (int)c;
                hash += i * (i++) * ic;
            }
            if (hash == code)
                return true;
            else
                return false;
        }

        public void UpdateScaledCollisionPoints()
        {
            this.GearPoints.Clear();
            foreach (Vector3 point in UnscaledGearPoints)
            {
                GearPoints.Add(Scale * point);
            }
            this.FloatPoints.Clear();
            foreach (Vector3 point in UnscaledFloatPoints)
            {
                FloatPoints.Add(Scale * point);
            }
            this.CollisionPoints.Clear();
            foreach (Vector3 point in UnscaledCollisionPoints)
            {
                CollisionPoints.Add(Scale * point);
            }
        }

        public string ClearDescriptionString(string description)
        {
            description = description.Replace(".com", ".xxx");
            description = description.Replace("http://", "xxxx://");
            description = description.Replace("www", "xxx");
            description = description.Replace("@", "x");
            return description;
        }

        public RCDeskPilot.API.AircraftParameters CloneToApiParameters( )
        {
            RCDeskPilot.API.AircraftParameters apiParameters = new RCDeskPilot.API.AircraftParameters();
            apiParameters.AileronEfficiency = AileronEfficiency;
            apiParameters.Channels = Channels;
            apiParameters.CrashResistence = CrashResistance;
            apiParameters.Description = Description;
            apiParameters.DihedralAngle = DihedralAngle;
            apiParameters.DihedralEfficiency = DihedralEfficiency;
            apiParameters.DragCoefficientPoints = DragCoefficientPoints;
            apiParameters.ElevatorEfficiency = ElevatorEfficiency;
            apiParameters.EngineMaxFrequency = EngineMaxFrequency;
            apiParameters.EngineMinFrequency = EngineMinFrequency;
            apiParameters.EngineSound = EngineSound;
            apiParameters.FixedMesh = FixedMesh;
            apiParameters.FlightModelType = (RCDeskPilot.API.AircraftParameters.FlightModelTypeEnum)FlightModelType;
            apiParameters.GearPoints = GearPoints;
            apiParameters.HandLaunched = HandLaunched;
            apiParameters.Ixx = Ixx;
            apiParameters.Iyy = Iyy;
            apiParameters.Izz = Izz;
            apiParameters.LiftCoefficientPoints = LiftCoefficientPoints;
            apiParameters.Mass = Mass;
            apiParameters.MaximumThrust = MaximumThrust;
            apiParameters.PitchDamping = PitchDamping;
            apiParameters.PitchStability = PitchStability;
            apiParameters.PitchTrim = PitchTrim;
            apiParameters.PropWashAilerons = PropWashAilerons;
            apiParameters.PropWashElevator = PropWashElevator;
            apiParameters.PropWashRudder = PropWashRudder;
            apiParameters.Restitution = Restitution;
            apiParameters.RollDamping = RollDamping;
            apiParameters.RudderEfficiency = RudderEfficiency;
            apiParameters.Scale = Scale;
            apiParameters.SideDragCoefficientPoints = SideDragCoefficientPoints;
            apiParameters.SideLiftCoefficientPoints = SideLiftCoefficientPoints;
            apiParameters.SpinFactor = SpinFactor;
            apiParameters.ThrottleDelay = ThrottleDelay;
            apiParameters.ThrustCoeffecientPoints = ThrustCoeffecientPoints;
            apiParameters.VerticalArea = VerticalArea;
            apiParameters.WingArea = WingArea;
            apiParameters.YawDamping = YawDamping;
            apiParameters.YawStability = YawStability;
            return apiParameters;
        }
        #endregion

        #region Private methods

        /// <summary>
        /// Writes a double value to a file.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="tag"></param>
        /// <param name="value"></param>
        private void WriteDouble(StreamWriter writer, string tag, double value)
        {
            writer.WriteLine("<{0}>{1}</{0}>", tag, value.ToString());
        }

        /// <summary>
        /// Writes an integer value to a file.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="tag"></param>
        /// <param name="value"></param>
        private void WriteInteger(StreamWriter writer, string tag, int value)
        {
            writer.WriteLine("<{0}>{1}</{0}>", tag, value.ToString());
        }

        /// <summary>
        /// Writes a string to a file.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="tag"></param>
        /// <param name="value"></param>
        private void WriteString(StreamWriter writer, string tag, string value)
        {
            if (!string.IsNullOrEmpty(value))
                writer.WriteLine("<{0}>{1}</{0}>", tag, value);
        }

        /// <summary>
        /// Writes a boolean value to a file.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="tag"></param>
        /// <param name="value"></param>
        private void WriteBoolean(StreamWriter writer, string tag, bool value)
        {
            writer.WriteLine("<{0}>{1}</{0}>", tag, value.ToString().ToLower());
        }

        private void WriteVector(StreamWriter writer, string tag, Vector3 vector)
        {
            writer.WriteLine("<{0}>{1},{2},{3}</{0}>",
                    tag, (double)vector.X, (double)vector.Y, (double)vector.Z);
        }

        private void WriteVector(StreamWriter writer, string tag, Vector3 vector, float scale)
        {
            writer.WriteLine("<{0}>{1},{2},{3}</{0}>",
                    tag, (double)(vector.X/scale), (double)(vector.Y/scale), (double)(vector.Z/scale));
        }
        
        /// <summary>
        /// Writes a coefficients list to a file.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="tag"></param>
        /// <param name="tagKey"></param>
        /// <param name="tagValue"></param>
        /// <param name="list"></param>
        private void WriteCoefficients(StreamWriter writer, string tag, string tagKey, string tagValue, List<KeyValuePair<double, double>> list)
        {
            writer.WriteLine(string.Format("<{0}s>", tag));
            foreach (KeyValuePair<double, double> keyValue in list)
            {
                writer.WriteLine("<{0}>", tag);
                writer.WriteLine("<{0}>{1}</{0}>", tagKey, keyValue.Key.ToString());
                writer.WriteLine("<{0}>{1}</{0}>", tagValue, keyValue.Value.ToString());
                writer.WriteLine("</{0}>", tag);
            }
            writer.WriteLine("</{0}s>", tag);
        }

        /// <summary>
        /// Writes a list of vectors to a file.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="tag"></param>
        /// <param name="vectorList"></param>
        private void WriteVectorList(StreamWriter writer, string tag, List<Vector3> vectorList)
        {
            writer.WriteLine("<{0}s>", tag);
            foreach (Vector3 vector in vectorList)
            {
                WriteVector(writer, tag, vector);
            }
            writer.WriteLine("</{0}s>", tag);
        }

        /// <summary>
        /// Writes a list of vectors to a file.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="tag"></param>
        /// <param name="vectorList"></param>
        private void WriteVectorList(StreamWriter writer, string tag, List<Vector3> vectorList, float scale)
        {
            writer.WriteLine("<{0}s>", tag);
            foreach (Vector3 vector in vectorList)
            {
                WriteVector(writer, tag, vector, scale);
            }
            writer.WriteLine("</{0}s>", tag);
        }

        private void WriteControlSurfaceList(StreamWriter writer, List<ControlSurface> surfaceList)
        {
            writer.WriteLine("<controlsurfaces>");
            if (surfaceList != null)
            {
                foreach (ControlSurface surface in surfaceList)
                {
                    WriteControlSurface(writer, surface);
                }
            }
            writer.WriteLine("</controlsurfaces>");
        }

        private void WriteControlSurface(StreamWriter writer, ControlSurface surface)
        {
            writer.WriteLine("<controlsurface>");
            WriteString(writer, "filename", surface.Filename);
            WriteVector(writer, "position", surface.Position);
            WriteVector(writer, "rotationaxis", surface.RotationAxis);
            WriteDouble(writer, "minangle", (double)surface.MinimumAngle);
            WriteDouble(writer, "zeroangle", (double)surface.ZeroAngle);
            WriteDouble(writer, "maxangle", (double)surface.MaximumAngle);
            WriteChannel(writer, "channel", surface.Channel);
            WriteBoolean(writer, "reversed", surface.Reversed);
            if (surface.Scale != 1)
                WriteDouble(writer, "scale", surface.Scale);
            WriteSurfaceType(writer, "type", surface.Type);
            if ((surface.ChildControlSurfaces != null) &&
                (surface.ChildControlSurfaces.Count > 0))
            {
                WriteControlSurfaceList(writer, surface.ChildControlSurfaces);
            }
            writer.WriteLine("</controlsurface>");
        }

        private void WriteChannel(StreamWriter writer, string tag, ChannelEnum channel)
        {
            writer.Write("<{0}>", tag);
            switch (channel)
            {
                case ChannelEnum.None:
                    writer.Write("none");
                    break;
                case ChannelEnum.Aileron:
                    writer.Write("aileron");
                    break;
                case ChannelEnum.Elevator:
                    writer.Write("elevator");
                    break;
                case ChannelEnum.Rudder:
                    writer.Write("rudder");
                    break;
                case ChannelEnum.Throttle:
                    writer.Write("throttle");
                    break;
                case ChannelEnum.Flaps:
                    writer.Write("flaps");
                    break;
                case ChannelEnum.Gear:
                    writer.Write("gear");
                    break;
            }
            writer.WriteLine("</{0}>", tag);
        }

        private void WriteSurfaceType(StreamWriter writer, string tag, ControlSurfaceTypeEnum surfaceType)
        {
            writer.Write("<{0}>", tag);
            switch (surfaceType)
            {
                case ControlSurfaceTypeEnum.Normal:
                    writer.Write("normal");
                    break;
                case ControlSurfaceTypeEnum.Reflective:
                    writer.Write("reflective");
                    break;
                case ControlSurfaceTypeEnum.PropFolded:
                    writer.Write("propfolded");
                    break;
                case ControlSurfaceTypeEnum.PropFoldingLowRPM:
                    writer.Write("propfoldinglowrpm");
                    break;
                case ControlSurfaceTypeEnum.PropHighRPM:
                    writer.Write("prophighrpm");
                    break;
                case ControlSurfaceTypeEnum.PropLowRPM:
                    writer.Write("proplowrpm");
                    break;
                case ControlSurfaceTypeEnum.RotorHighRPM:
                    writer.Write("rotorhighrpm");
                    break;
                case ControlSurfaceTypeEnum.RotorLowRPM:
                    writer.Write("rotorlowrpm");
                    break;
                case ControlSurfaceTypeEnum.TailrotorHighRPM:
                    writer.Write("tailrotorlowrpm");
                    break;
                case ControlSurfaceTypeEnum.TailrotorLowRPM:
                    writer.Write("tailrotorhighrpm");
                    break;
            }
            writer.WriteLine("</{0}>", tag);
        }

        /// <summary>
        /// Reads a list of vectors and multiplies them with the given scale.
        /// </summary>
        /// <param name="nav"></param>
        /// <param name="rootTag"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        private List<Vector3> ReadVectorList(XPathNavigator nav, string rootTag, float scale)
        {
            List<Vector3> result = new List<Vector3>();
            XPathNodeIterator vectorIterator = nav.Select(rootTag);
            foreach (XPathNavigator vectorNav in vectorIterator)
            {
                result.Add(ReadVector(vectorNav.Value, scale));
            }
            return result;
        }

        /// <summary>
        /// Reads a vector and multiplies it with the scale.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        private Vector3 ReadVector(string str, float scale)
        {
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            provider.NumberGroupSeparator = ",";

            string[] coords = str.Split(',');
            if (coords.Length != 3)
                return new Vector3(0,0,0);
            else
                return new Vector3(
                    (float)Convert.ToDouble(coords[0], provider) * scale,
                    (float)Convert.ToDouble(coords[1], provider) * scale,
                    (float)Convert.ToDouble(coords[2], provider) * scale);
        }

        private Vector3 ReadVector(XPathNavigator nav, string tag, float scale, Vector3 defaultValue)
        {
            string str = ReadString(nav, tag, null);
            if (string.IsNullOrEmpty(str))
            {
                return scale * defaultValue;
            }
            else
            {
                return ReadVector(str, scale);
            }
        }

        /// <summary>
        /// Reads a list of key/value pairs.
        /// </summary>
        /// <param name="nav"></param>
        /// <param name="rootTag"></param>
        /// <returns></returns>
        private List<KeyValuePair<double, double>> ReadCoefficientList(XPathNavigator nav, string rootTag, string indexTag)
        {
            List<KeyValuePair<double, double>> result = new List<KeyValuePair<double, double>>();
            XPathNodeIterator coeffsIterator = nav.Select(rootTag);
            foreach (XPathNavigator pointNav in coeffsIterator)
            {
                double angle = 0;
                XPathNavigator angleNav = pointNav.SelectSingleNode(indexTag);
                if (angleNav.Value.Equals("-PI"))
                    angle = -Math.PI;
                else if (angleNav.Value.Equals("PI"))
                    angle = Math.PI;
                else
                    angle = angleNav.ValueAsDouble;
                double coeff = pointNav.SelectSingleNode("coefficient").ValueAsDouble;
                result.Add(new KeyValuePair<double, double>(angle, coeff));
            }
            if (result.Count == 0)
                result.Add(new KeyValuePair<double, double>(0, 1));
            return result;
        }

        private FlightModelTypeEnum ReadFlightModelType(XPathNavigator nav, string tagId)
        {
            try
            {
                XPathNavigator node = nav.SelectSingleNode(tagId);
                if (node == null)
                    return FlightModelTypeEnum.Aircraft;
                string flightModelString = (string)node.Value;
                if (flightModelString.ToLower().Equals("helicopter"))
                    return FlightModelTypeEnum.Helicopter;
                else if (flightModelString.ToLower().Equals("helicoptercoax"))
                    return FlightModelTypeEnum.HelicopterCoax;
                else if (flightModelString.ToLower().Equals("sailboat"))
                    return FlightModelTypeEnum.Sailboat;
                else
                    return FlightModelTypeEnum.Aircraft;
            }
            catch
            {
                return FlightModelTypeEnum.Aircraft;
            }
        }

        private List<ControlSurface> ReadControlSurfaces(XPathNavigator nav, string rootTag, float scale)
        {
            List<ControlSurface> result = new List<ControlSurface>();
            XPathNodeIterator surfaceIterator = nav.Select(rootTag);
            foreach (XPathNavigator surfaceNav in surfaceIterator)
            {
                ControlSurface surface = new ControlSurface();
                surface.Filename = ReadString(surfaceNav, "filename", null);
                surface.Position = ReadVector(surfaceNav.SelectSingleNode("position").Value, 1.0f);
                surface.RotationAxis = ReadVector(surfaceNav.SelectSingleNode("rotationaxis").Value, 1.0f);
                surface.MinimumAngle = surfaceNav.SelectSingleNode("minangle").ValueAsDouble;
                surface.ZeroAngle = surfaceNav.SelectSingleNode("zeroangle").ValueAsDouble;
                surface.MaximumAngle = surfaceNav.SelectSingleNode("maxangle").ValueAsDouble;
                surface.Scale = ReadFloat(surfaceNav, "scale", 1);
                //surface.Channel = (ChannelEnum)Enum.Parse(typeof(ChannelEnum), surfaceNav.SelectSingleNode("channel").Value);
                // For obfuscation, we use the dummy method
                switch (surfaceNav.SelectSingleNode("channel").Value.ToString().ToLower())
                {
                    case "none":
                        surface.Channel = ChannelEnum.None;
                        break;
                    case "elevator":
                        surface.Channel = ChannelEnum.Elevator;
                        break;
                    case "rudder":
                        surface.Channel = ChannelEnum.Rudder;
                        break;
                    case "aileron":
                        surface.Channel = ChannelEnum.Aileron;
                        break;
                    case "throttle":
                        surface.Channel = ChannelEnum.Throttle;
                        break;
                    case "flaps":
                        surface.Channel = ChannelEnum.Flaps;
                        break;
                    case "gear":
                        surface.Channel = ChannelEnum.Gear;
                        break;
                }

                if (surfaceNav.SelectSingleNode("type") != null)
                {
                    //surface.Type = (ControlSurfaceTypeEnum)Enum.Parse(typeof(ControlSurfaceTypeEnum), surfaceNav.SelectSingleNode("type").Value);
                    // For obfuscation, we use the dummy method
                    switch (surfaceNav.SelectSingleNode("type").Value.ToString().ToLower())
                    {
                        case "normal":
                            surface.Type = ControlSurfaceTypeEnum.Normal;
                            break;
                        case "reflective":
                            surface.Type = ControlSurfaceTypeEnum.Reflective;
                            break;
                        case "proplowrpm":
                            surface.Type = ControlSurfaceTypeEnum.PropLowRPM;
                            break;
                        case "prophighrpm":
                            surface.Type = ControlSurfaceTypeEnum.PropHighRPM;
                            break;
                        case "propfoldinglowrpm":
                            surface.Type = ControlSurfaceTypeEnum.PropFoldingLowRPM;
                            break;
                        case "propfolded":
                            surface.Type = ControlSurfaceTypeEnum.PropFolded;
                            break;
                        case "rotorlowrpm":
                            surface.Type = ControlSurfaceTypeEnum.RotorLowRPM;
                            break;
                        case "rotorhighrpm":
                            surface.Type = ControlSurfaceTypeEnum.RotorHighRPM;
                            break;
                        case "tailrotorlowrpm":
                            surface.Type = ControlSurfaceTypeEnum.TailrotorLowRPM;
                            break;
                        case "tailrotorhighrpm":
                            surface.Type = ControlSurfaceTypeEnum.TailrotorHighRPM;
                            break;
                    }
                }
                surface.Reversed = surfaceNav.SelectSingleNode("reversed").ValueAsBoolean;
                if (surfaceNav.SelectSingleNode("controlsurfaces") != null)
                    surface.ChildControlSurfaces = ReadControlSurfaces(surfaceNav.SelectSingleNode("controlsurfaces"), "controlsurface",
                        surface.Scale);
                result.Add(surface);
            }
            return result;
        }

        private string ReadString(XPathNavigator nav, string tag, string defaultValue)
        {
            try
            {
                XPathNavigator node = nav.SelectSingleNode(tag);
                if (node != null)
                    return (string)node.Value;
                else
                    return defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        private float ReadFloat(XPathNavigator nav, string tag, float defaultValue)
        {
            try
            {
                XPathNavigator node = nav.SelectSingleNode(tag);
                if (node != null)
                    return (float)node.ValueAsDouble;
                else
                    return defaultValue;

            }
            catch
            {
                return defaultValue;
            }
        }

        private double ReadDouble(XPathNavigator nav, string tag, double defaultValue)
        {
            try
            {
                XPathNavigator node = nav.SelectSingleNode(tag);
                if (node != null)
                    return node.ValueAsDouble;
                else
                    return defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        private bool ReadBool(XPathNavigator nav, string tag, bool defaultValue)
        {
            try
            {
                return nav.SelectSingleNode(tag).ValueAsBoolean;
            }
            catch
            {
                return defaultValue;
            }
        }

        private int ReadInt(XPathNavigator nav, string tag, int defaultValue)
        {
            try
            {
                XPathNavigator node = nav.SelectSingleNode(tag);
                if (node != null)
                    return node.ValueAsInt;
                else
                    return defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }
        #endregion
    }
}
