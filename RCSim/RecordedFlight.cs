using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Bonsai.Core.Interfaces;
using Microsoft.DirectX.Direct3D;
using RCSim.DataClasses;
using RCSim.Interfaces;
using Microsoft.DirectX;
using RCSim.Effects;
using Bonsai.Core;

namespace RCSim
{
    class RecordedFlight : IFrameworkCallback, IAirplaneControl, IDisposable
    {
        #region Private fields
        private string fileName = null;
        private AirplaneModel airplaneModel;
        private AircraftParameters aircraftParameters = null;
        private FileStream file = null;
        private BinaryReader binaryReader = null;
        private bool playing = false;
        private AirplaneState previousState = null;
        private AirplaneState nextState = null;
        private AirplaneState currentState = new AirplaneState();
        private double startTime = 0;
        private double previousTime = 0;
        private double nextTime = 0;
        private double relativeTime = 0;
        private Program owner = null;
        private Smoke smoke = null;
        private WaterRipples ripples = null;
        #endregion

        #region Public events
        public event EventHandler Stopped;
        #endregion

        #region Public properties
        /// <summary>
        /// Gets the airplanemodel for the recording.
        /// </summary>
        public AirplaneModel AirplaneModel
        {
            get { return airplaneModel; }
        }

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        public bool Playing
        {
            get { return playing; }
            set
            {
                if (value != playing)
                {
                    if (value)
                        StartPlaying();
                    else
                        StopPlaying();
                }
            }
        }

        public Vector3 Velocity
        {
            get
            {
                if (nextState != null && previousState != null)
                {
                    Vector3 distance = nextState.Position - previousState.Position;
                    double time = nextTime - previousTime;
                    if (time > 0)
                    {
                        return (1f / (float)time) * distance;
                    }
                }
                return Vector3.Empty;
            }
        }

        public double Time
        {
            get
            {
                return relativeTime;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="owner"></param>
        public RecordedFlight(Program owner)
        {
            this.owner = owner;
            this.smoke = new Smoke(owner, this);
            this.ripples = new WaterRipples();
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Clean up.
        /// </summary>
        public void Dispose()
        {
            if (smoke != null)
            {
                smoke.Dispose();
                smoke = null;
            }
            if (airplaneModel != null)
            {
                airplaneModel.Dispose();
                airplaneModel = null;
            }
            if (ripples != null)
            {
                ripples.Dispose();
                ripples = null;
            }
        }
        #endregion

        private void StartPlaying()
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                if (playing)
                    StopPlaying();
                
                file = File.OpenRead(fileName);
                binaryReader = new BinaryReader(file);
                aircraftParameters = new AircraftParameters();
                aircraftParameters.ReadParameters(binaryReader.ReadString());
                if (airplaneModel == null)
                    airplaneModel = new AirplaneModel(this);
                playing = true;
                startTime = -1;
                relativeTime = 0;
                previousState = new AirplaneState();
                nextState = new AirplaneState();
                nextTime = binaryReader.ReadDouble();
                nextState.Read(binaryReader);
                currentState.Gear = nextState.Gear;
                currentState.Flaps = nextState.Flaps;
                if (currentState.Gear)
                    Gear = 1.0;
                if (currentState.Flaps)
                    Flaps = 1.0;
                airplaneModel.StartEngine();
            }
        }

        private void StopPlaying()
        {
            playing = false;
            airplaneModel.KillEngine();
            startTime = -1;
            if (binaryReader != null)
            {
                binaryReader.Close();
                binaryReader = null;
            }
            if (file != null)
            {
                file.Close();
                file.Dispose();
                file = null;
            }
            if (airplaneModel != null)
            {
                airplaneModel.Dispose();
                airplaneModel = null;
            }
            previousState = null;
            nextState = null;
            if (Stopped != null)
                Stopped(this, EventArgs.Empty);
        }

        #region IFrameworkCallback Members

        public void OnFrameMove(Device device, double totalTime, float elapsedTime)
        {
            if (playing)
            {
                if (startTime == -1)
                {
                    startTime = totalTime;                    
                }

                relativeTime = totalTime - startTime;
                if (relativeTime >= nextTime)
                {
                    previousTime = nextTime;
                    previousState.Position = nextState.Position;
                    previousState.Orientation = nextState.Orientation;
                    previousState.Rudder = nextState.Rudder;
                    previousState.Throttle = nextState.Throttle;
                    previousState.Elevator = nextState.Elevator;
                    previousState.Ailerons = nextState.Ailerons;
                    previousState.Smoke = nextState.Smoke;
                    previousState.Gear = nextState.Gear;
                    previousState.Flaps = nextState.Flaps;
                    previousState.OnWater = nextState.OnWater;
                    try
                    {
                        nextTime = binaryReader.ReadDouble();
                        nextState.Read(binaryReader);
                    }
                    catch (Exception)
                    {
                        StopPlaying();
                        return;
                    }
                }
                
                double factor = (relativeTime - previousTime) / (nextTime - previousTime);
                float newYaw;
                float newPitch;
                float newRoll;
                if (Math.Abs(previousState.Orientation.X - nextState.Orientation.X) > Math.PI)
                    newYaw = (float)(1 - factor) * ((previousState.Orientation.X + 2 * (float)Math.PI) % (2 * (float)Math.PI)) + (float)factor * ((nextState.Orientation.X + 2 * (float)Math.PI) % (2 * (float)Math.PI));
                else
                    newYaw = (float)(1 - factor) * previousState.Orientation.X + (float)factor * nextState.Orientation.X;
                if (Math.Abs(previousState.Orientation.Y - nextState.Orientation.Y) > Math.PI)
                    newPitch = (float)(1 - factor) * ((previousState.Orientation.Y + 2 * (float)Math.PI) % (2 * (float)Math.PI)) + (float)factor * ((nextState.Orientation.Y + 2 * (float)Math.PI) % (2 * (float)Math.PI));
                else
                    newPitch = (float)(1 - factor) * previousState.Orientation.Y + (float)factor * nextState.Orientation.Y;
                if (Math.Abs(previousState.Orientation.Z - nextState.Orientation.Z) > Math.PI)
                    newRoll = (float)(1 - factor) * ((previousState.Orientation.Z + 2 * (float)Math.PI) % (2 * (float)Math.PI)) + (float)factor * ((nextState.Orientation.Z + 2 * (float)Math.PI) % (2 * (float)Math.PI));
                else
                    newRoll = (float)(1 - factor) * previousState.Orientation.Z + (float)factor * nextState.Orientation.Z;

                if ((currentState.Flaps) && (Flaps < 0.9999))
                    Flaps = Math.Min(1.0, Flaps + elapsedTime / AircraftParameters.FlapsDelay);
                else if ((Flaps > 0.0001) && (!currentState.Flaps))
                    Flaps = Math.Max(0, Flaps - elapsedTime / AircraftParameters.FlapsDelay);
                if ((currentState.Gear) && (Gear < 0.9999))
                    Gear = Math.Min(1.0, Gear + elapsedTime / AircraftParameters.GearDelay);
                else if ((Gear > 0.0001) && (!currentState.Gear))
                    Gear = Math.Max(0, Gear - elapsedTime / AircraftParameters.GearDelay);

                currentState.Position = (float)(1 - factor) * previousState.Position + (float)factor * nextState.Position;
                currentState.Orientation = new Microsoft.DirectX.Vector3(newYaw, newPitch, newRoll);
                currentState.Rudder = (1 - factor) * previousState.Rudder + factor * nextState.Rudder;
                currentState.Throttle = (1 - factor) * previousState.Throttle + factor * nextState.Throttle;
                currentState.Elevator = (1 - factor) * previousState.Elevator + factor * nextState.Elevator;
                currentState.Ailerons = (1 - factor) * previousState.Ailerons + factor * nextState.Ailerons;
                currentState.Smoke = previousState.Smoke;
                currentState.Gear = previousState.Gear;
                currentState.Flaps = previousState.Flaps;
                currentState.OnWater = previousState.OnWater;

                float speed = (float)((currentState.Position - nextState.Position).Length() / (nextTime - previousTime));
                
                airplaneModel.Position = currentState.Position;
                airplaneModel.YawPitchRoll = currentState.Orientation;
                airplaneModel.OnFrameMove(device, totalTime, elapsedTime);

                smoke.Position = airplaneModel.Position - (float)Throttle * this.airplaneModel.Front;
                smoke.Emitting = currentState.Smoke;
                smoke.OnFrameMove(device, totalTime, elapsedTime);
                
                if (currentState.OnWater)
                    ripples.AddRipple(speed, airplaneModel.Position, totalTime);
                ripples.OnFrameMove(device, totalTime, elapsedTime);
            }
        }

        public void OnFrameRender(Device device, double totalTime, float elapsedTime)
        {
            if (playing && startTime != -1)
            {
                if (!Framework.Instance.CurrentCamera.Reflected)
                {
                    Vector3 p1;
                    Vector3 p2;
                    Vector3 p3;
                    Vector3 p4 = new Vector3(0, 0.005f, 0);

                    Program.Instance.Heightmap.GetPoints(airplaneModel.Position.X, airplaneModel.Position.Z, out p1, out p2, out p3);
                    airplaneModel.OnRenderShadow(device, p1 + p4, p2 + p4, p3 + p4, new Vector3(0, -1, 0));
                    ripples.OnFrameRender(device, totalTime, elapsedTime);
                }
                airplaneModel.OnFrameRender(device, totalTime, elapsedTime);
                smoke.OnFrameRender(device, totalTime, elapsedTime);                
            }            
        }
        #endregion

        #region IAirplaneControl Members

        public double Throttle
        {
            get { return currentState.Throttle; }
            set { }
        }

        public double Ailerons
        {
            get { return currentState.Ailerons; }
            set {  }
        }

        public double Elevator
        {
            get { return currentState.Elevator; }
            set {  }
        }

        public double Rudder
        {
            get { return currentState.Rudder; }
            set {   }
        }

        public double Flaps
        {
            get; set;
        }

        public double Gear
        {
            get; set;
        }

        public AircraftParameters AircraftParameters
        {
            get
            {
                return aircraftParameters;
            }
            set
            {
                aircraftParameters = value;
            }
        }

        public float RotorRPM 
        {
            get
            {
                return 1000;
            }
            set
            {
            }
        }

        /// <summary>
        /// Gets/Sets the relative force the rotor is exercising.
        /// </summary>
        public float RelativeRotorForce 
        {
            get
            {
                return 1f;
            }
            set
            {
            }
        }
        
        #endregion

 
    }
}
