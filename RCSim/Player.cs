using System;
using System.Collections.Generic;
using System.Text;

using Bonsai.Core;
using Bonsai.Core.Controls;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Bonsai.Objects;
using Bonsai.Objects.Meshes;
using Bonsai.Core.Interfaces;
using Bonsai.Objects.Shaders;
using Bonsai.Objects.Terrain;
using RCSim.Interfaces;
using RCSim.DataClasses;
using System.Reflection;
using Bonsai.Sound;
using RCSim.Effects;
using Bonsai.Objects.Collision;

namespace RCSim
{
    internal class Player : IFrameworkCallback, IDisposable
    {
        #region Private fields
        private AirplaneModel airplane = null;
        //private FlightModelWind model = null;
        private FlightModelApi modelApi = null;
        private IFlightModel iFlightModel = null;
        private Program owner = null;
        private Heightmap heightmap = null;
        private Smoke smoke = null;
        private WaterRipples ripples = null;
        private GameObject debugObject = null;
        private LineMesh towLine = null;
        private LineMesh windVector = null;
        private bool expoRoll = false;
        private bool expoPitch = false;
        private bool expoYaw = false;
        //private LineMesh terrainNormal = new LineMesh();
        //private GameObject normalObject = null;
        private Wind wind = null;
        private static string currentModel = "aircraft\\cessna\\El Trainer.par";
        private static double crashTime = 0;
        private static bool useAileronForRudder = false;
        private SoundControllable variometer = null;
        private float elapsedCumul = 0f;
        private bool flapsKeyDown = false;
        private bool gearKeyDown = false;
        private bool towKeyDown = false;
        private int prevFlapsChannel = 0;
        private int prevGearChannel = 0;
        private Towing towing = null;
        private Vector3 defaultStartPosition = new Vector3(0, 0, 0);
        private Vector3 waterStartPosition = new Vector3(0, 0, 0);
        private bool takeOffFromWater = false;
        private Vector3 prevPos = new Vector3();
        private List<Vector3> prevColPoints = new List<Vector3>();

        // Effects
        private Reflection reflection = null;

        // Keyboard controls
        private double kbThrottle = -100;
        private double kbRudder = 0;
        private double kbElevator = 0;
        private double kbAileron = 0;
        #endregion

        #region Public properties
        /// <summary>
        /// Gets the position of the airplane.
        /// </summary>
        public Vector3 Position
        {
            get { return airplane.Position; }
        }

        /// <summary>
        /// Gets the orientation of the airplane.
        /// </summary>
        public Vector3 Orientation
        {
            get { return airplane.Orientation; }
        }

        /// <summary>
        /// Gets the Airplane GameObject.
        /// </summary>
        public AirplaneModel Airplane
        {
            get { return airplane; }
        }

        /// <summary>
        /// Gets/sets the current heightmap.
        /// </summary>
        public Heightmap Heightmap
        {
            get { return heightmap; }
            set 
            { 
                heightmap = value;
                iFlightModel.Heightmap = value;
            }
        }

        /// <summary>
        /// Gets the FlightModel.
        /// </summary>
        public IFlightModel FlightModel
        {
            get { return iFlightModel; }
        }

        public AircraftParameters AircraftParameters
        {
            get
            {
                return iFlightModel.AircraftParameters;
            }
        }

        /// <summary>
        /// Gets/Sets the currently loaded model.
        /// </summary>
        public static string CurrentModel
        {
            get { return currentModel; }
            set { currentModel = value; }
        }

        /// <summary>
        /// Gets/Sets whether the aileron channel should be used for rudder control in 2/3 channel aircraft.
        /// </summary>
        public static bool UseAileronForRudder
        {
            get { return useAileronForRudder; }
            set { useAileronForRudder = value; }
        }

        /// <summary>
        /// Gets whether the airplane is currently emitting smoke.
        /// </summary>
        public bool Smoking
        {
            get { return smoke.Emitting; }
        }

        /// <summary>
        /// Gets/sets the default position to reset the aircraft.
        /// </summary>
        public Vector3 DefaultStartPosition
        {
            get { return defaultStartPosition; }
            set { defaultStartPosition = value; }
        }

        /// <summary>
        /// Gets/sets the starting position for seaplanes.
        /// </summary>
        public Vector3 WaterStartPosition
        {
            get { return waterStartPosition; }
            set { waterStartPosition = value; }
        }

        /// <summary>
        /// Gets/Sets whether the plane should start on water.
        /// </summary>
        public bool TakeOffFromWater
        {
            get { return takeOffFromWater; }
            set { takeOffFromWater = value; }
        }

        /// <summary>
        /// Gets/Sets a reference to the reflection controller.
        /// </summary>
        public Reflection Reflection
        {
            get { return reflection; }
        }
        #endregion

        #region Constructor
        public Player(Program owner)
        {
            this.owner = owner;

            this.wind = owner.Weather.Wind;

            // Set the wind
            wind.Direction = 0;
            wind.DirectionVariance = 0.3;

            useAileronForRudder = Convert.ToBoolean(Bonsai.Utils.Settings.GetValue("UseAileronChannel"));

            UpdateExpos();

            Bonsai.Utils.Settings.SettingsChanged += new Bonsai.Utils.Settings.SettingsEventHandler(Settings_SettingsChanged);

            debugObject = new GameObject();
            CubeMesh debugCube = new CubeMesh(0.1f);
            debugCube.Texture = new Bonsai.Objects.Textures.TextureBase("data\\grass.jpg");
            debugObject.Mesh = debugCube;
            towLine = new LineMesh();
            windVector = new LineMesh();
            ripples = new WaterRipples();
            reflection = new Reflection();
            //normalObject = new GameObject();
            //normalObject.Mesh = terrainNormal;            
        }

        
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Clean up.
        /// </summary>
        public void Dispose()
        {
            Bonsai.Utils.Settings.SettingsChanged -= new Bonsai.Utils.Settings.SettingsEventHandler(Settings_SettingsChanged);

            if (iFlightModel != null)
            {
                iFlightModel.Dispose();
                iFlightModel = null;
            }
            if (modelApi != null)
            {
                modelApi.Dispose();
                modelApi = null;
            }
            if (smoke != null)
            {
                smoke.Dispose();
                smoke = null;
            }
            if (airplane != null)
            {
                airplane.Dispose();
                airplane = null;
            }
            if (variometer != null)
            {
                variometer.Dispose();
                variometer = null;
            }
            if (towLine != null)
            {
                towLine.Dispose();
                towLine = null;
            }
            if (windVector != null)
            {
                windVector.Dispose();
                windVector = null;
            }
            if (ripples != null)
            {
                ripples.Dispose();
                ripples = null;
            }
            if (reflection != null)
            {
                reflection.Dispose();
                reflection = null;
            }

        }
        #endregion

        #region Private eventhandlers
        void Settings_SettingsChanged(object sender, Bonsai.Utils.Settings.SettingsEventArgs e)
        {
            UpdateExpos();
            UpdateVariometer();
        }
        #endregion

        #region Public methods
        public void ReloadModel()
        {
            if ((iFlightModel.AircraftParameters != null) &&
                (!string.IsNullOrEmpty(iFlightModel.AircraftParameters.FileName)))
                LoadModel(iFlightModel.AircraftParameters.FileName);
        }

        public void LoadModel(string fileName)
        {
            // First clean up
            if (iFlightModel != null)
            {
                iFlightModel.Dispose();
                iFlightModel = null;
            }
            if (modelApi != null)
            {
                modelApi.Dispose();
                modelApi = null;
            }
            if (airplane != null)
            {
                airplane.Dispose();
                airplane = null;
            }
            if (variometer != null)
            {
                variometer.Dispose();
                variometer = null;
            }

            // Now load the actual model
            //model = new FlightModelWind();
            AircraftParameters parameters = new RCSim.DataClasses.AircraftParameters();
            parameters.File = fileName;

            if (!string.IsNullOrEmpty(Bonsai.Utils.Settings.GetValue("ApiFlightModel")))
            {
                try
                {
                    string assemblyInfo = Bonsai.Utils.Settings.GetValue("ApiFlightModel");
                    string[] assemblyParts = assemblyInfo.Split(',', ';');
                    Assembly assembly = Assembly.LoadFrom(assemblyParts[0]);
                    RCDeskPilot.API.FlightModelSimple flightModelSimple = 
                        assembly.CreateInstance(assemblyParts[1]) as RCDeskPilot.API.FlightModelSimple;
                    modelApi = new FlightModelApi();
                    modelApi.ApiModel = flightModelSimple;
                    iFlightModel = modelApi;
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(
                        string.Format("Failed to load the flightmodel plugin : {0}", ex.ToString()));
                }
            }
            else
            {
                if (parameters.Version == 2)
                    iFlightModel = new FlightModelWind2();
                else
                    iFlightModel = new FlightModelWind();
            }
            
            iFlightModel.AircraftParameters = parameters;
            iFlightModel.Initialize();
            iFlightModel.Reset();
            iFlightModel.Wind = new Vector3(0, 0, 0);
            iFlightModel.Heightmap = Heightmap;
            iFlightModel.Water = Program.Instance.Scenery.Water;
            airplane = new AirplaneModel(iFlightModel);
                        
            CurrentModel = fileName;

            if (smoke != null)
            {
                smoke.Dispose();
                smoke = null;
            }
            smoke = new Smoke(owner, iFlightModel);

            UpdateVariometer();

            this.owner.UpdateCameras();
            this.Reset();            
        }

        /// <summary>
        /// Resets the position of the airplane
        /// </summary>
        public void Reset()
        {
            iFlightModel.Reset();
            iFlightModel.Throttle = 0;
            iFlightModel.CableEnabled = false;
            crashTime = 0;            
            if (AircraftParameters.HandLaunched)
            {
                if (variometer != null)
                    variometer.Play(true);
                iFlightModel.HandLaunch(-owner.PilotPosition.Z, -owner.PilotPosition.X, -owner.PilotPosition.Y);
            }
            else if (AircraftParameters.HasFloats && TakeOffFromWater)
            {
                iFlightModel.X = -waterStartPosition.Z;
                iFlightModel.Y = -waterStartPosition.X;
                iFlightModel.Z = -waterStartPosition.Y;
                if (AircraftParameters.HasRetracts)
                    iFlightModel.GearExtended = false;
            }
            else
            {
                iFlightModel.X = -defaultStartPosition.Z;
                iFlightModel.Y = -defaultStartPosition.X;
                iFlightModel.Z = -defaultStartPosition.Y;
            }
            if ((owner != null) && (owner.InputManager != null))
            {
                prevFlapsChannel = owner.InputManager.GetAxisValue("flaps");
                prevGearChannel = owner.InputManager.GetAxisValue("gear");
            }
            
            airplane.StartEngine();

            prevPos = new Vector3(-iFlightModel.Y, -iFlightModel.Z, -iFlightModel.X);
        }

        /// <summary>
        /// Toggles the smoke
        /// </summary>
        public void ToggleSmoke()
        {
            smoke.Emitting = !smoke.Emitting;
        }
        #endregion

        #region Private methods
        private void HandleCrash()
        {
            airplane.Crash();            
        }

        private void UpdateExpos()
        {
            expoRoll = Convert.ToBoolean(Bonsai.Utils.Settings.GetValue("RollExpo", "false"));
            expoPitch = Convert.ToBoolean(Bonsai.Utils.Settings.GetValue("PitchExpo", "false"));
            expoYaw = Convert.ToBoolean(Bonsai.Utils.Settings.GetValue("YawExpo", "false"));
        }

        private void UpdateVariometer()
        {
            if (variometer != null)
            {
                variometer.Dispose();
                variometer = null;
            }
            if (Convert.ToBoolean(Bonsai.Utils.Settings.GetValue("EnableVariometer", "false")))
            {
                if ((iFlightModel != null) && (iFlightModel.AircraftParameters.HasVariometer))
                {
                    variometer = new SoundControllable("data/variometer.wav");
                    variometer.Volume = 10;
                    variometer.Play(true);
                }
            }
        }
        #endregion

        #region IFrameworkCallback Members
        /// <summary>
        /// Handles the FrameMove.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="totalTime"></param>
        /// <param name="elapsedTime"></param>
        public void OnFrameMove(Device device, double totalTime, float elapsedTime)
        {
            if ((crashTime > 0) && (crashTime + 3.0 < totalTime))
            {
                Reset();
            }
            
            elapsedCumul += elapsedTime;
            if (elapsedCumul > 0.1f)
            {
                elapsedCumul = 0f;
                if (variometer != null)
                {
                    variometer.Frequency = (int)(22100 - Math.Sign(iFlightModel.Velocity.Z) * Math.Sqrt(Math.Abs(iFlightModel.Velocity.Z)) * 1000);
                    variometer.Volume = Math.Min(100, (int)(Math.Abs(iFlightModel.Velocity.Z - 0.3f) * 100));
                }
            }

            int throttle = 0;
            int rudder = 0;
            int elevator = 0;
            int aileron = 0;
            if (owner.InputManager.IsJoyStickAvailable)
            {
                throttle = owner.InputManager.GetAxisValue("throttle");
                rudder = owner.InputManager.GetAxisValue("rudder");
                elevator = owner.InputManager.GetAxisValue("elevator");
                aileron = owner.InputManager.GetAxisValue("aileron");
            }
            else
            {
                if (owner.InputManager.KeyBoardState != null)
                {
                    if (owner.InputManager.KeyBoardState[Microsoft.DirectX.DirectInput.Key.NumPad9] ||
                        owner.InputManager.KeyBoardState[Microsoft.DirectX.DirectInput.Key.PageUp])
                        kbThrottle = Math.Min(100, kbThrottle + 75 * elapsedTime);
                    else if (owner.InputManager.KeyBoardState[Microsoft.DirectX.DirectInput.Key.NumPad7] ||
                        owner.InputManager.KeyBoardState[Microsoft.DirectX.DirectInput.Key.PageDown])
                        kbThrottle = Math.Max(-100, kbThrottle - 75 * elapsedTime);
                    //else throttle = (int)(200 * (iFlightModel.Throttle - 0.5));
                    if (owner.InputManager.KeyBoardState[Microsoft.DirectX.DirectInput.Key.NumPad3] ||
                        owner.InputManager.KeyBoardState[Microsoft.DirectX.DirectInput.Key.End])
                        kbRudder = Math.Min(100, kbRudder + 200 * elapsedTime);
                    else if (owner.InputManager.KeyBoardState[Microsoft.DirectX.DirectInput.Key.NumPad1] ||
                        owner.InputManager.KeyBoardState[Microsoft.DirectX.DirectInput.Key.Home])
                        kbRudder = Math.Max(-100, kbRudder - 200 * elapsedTime);
                    else if (Math.Abs(kbRudder) < 5)
                        kbRudder = 0;
                    else kbRudder = Math.Max(-100, Math.Min(100, kbRudder + (kbRudder > 0 ? -350 * elapsedTime : 350 * elapsedTime)));
                    if (owner.InputManager.KeyBoardState[Microsoft.DirectX.DirectInput.Key.NumPad2] ||
                        owner.InputManager.KeyBoardState[Microsoft.DirectX.DirectInput.Key.DownArrow])
                        kbElevator = Math.Min(100, kbElevator + 300 * elapsedTime);
                    else if (owner.InputManager.KeyBoardState[Microsoft.DirectX.DirectInput.Key.NumPad8] ||
                        owner.InputManager.KeyBoardState[Microsoft.DirectX.DirectInput.Key.UpArrow])
                        kbElevator = Math.Max(-100, kbElevator - 300 * elapsedTime);
                    else if (Math.Abs(kbElevator) < 5)
                        kbElevator = 0;
                    else kbElevator = Math.Max(-100, Math.Min(100, kbElevator + (kbElevator > 0 ? -350 * elapsedTime : 350 * elapsedTime)));
                    if (owner.InputManager.KeyBoardState[Microsoft.DirectX.DirectInput.Key.NumPad4] ||
                        owner.InputManager.KeyBoardState[Microsoft.DirectX.DirectInput.Key.LeftArrow])
                        kbAileron = Math.Max(-100, kbAileron - 75 * elapsedCumul);
                    else if (owner.InputManager.KeyBoardState[Microsoft.DirectX.DirectInput.Key.NumPad6] ||
                        owner.InputManager.KeyBoardState[Microsoft.DirectX.DirectInput.Key.RightArrow])
                        kbAileron = Math.Min(100, kbAileron + 75 * elapsedCumul);
                    else if (Math.Abs(kbAileron) < 5)
                        kbAileron = 0;
                    else kbAileron = Math.Max(-100, Math.Min(100, kbAileron + (kbAileron > 0 ? -450 * elapsedTime : 450 * elapsedTime)));
                    throttle = (int)kbThrottle;
                    rudder = (int)kbRudder;
                    elevator = (int)kbElevator;
                    aileron = (int)kbAileron;
                }
            }

            // Flaps & Gear
            if (owner.InputManager.KeyBoardState != null)
            {
                if (AircraftParameters.HasFlaps)
                {
                    if (owner.InputManager.KeyBoardState[Microsoft.DirectX.DirectInput.Key.F])
                    {
                        if (!flapsKeyDown)
                        {
                            flapsKeyDown = true;
                            iFlightModel.FlapsExtended = !iFlightModel.FlapsExtended;
                        }
                    }
                    else if (flapsKeyDown)
                        flapsKeyDown = false;
                }
                if (AircraftParameters.HasRetracts)
                {
                    if (owner.InputManager.KeyBoardState[Microsoft.DirectX.DirectInput.Key.G])
                    {
                        if (!gearKeyDown)
                        {
                            gearKeyDown = true;
                            iFlightModel.GearExtended = !iFlightModel.GearExtended;
                        }
                    }
                    else if (gearKeyDown)
                        gearKeyDown = false;
                }
            }
            if (AircraftParameters.HasFlaps)
            {
                int flapsChannel = owner.InputManager.GetAxisValue("flaps");
                if (Math.Abs(flapsChannel - prevFlapsChannel) > 50)
                {
                    prevFlapsChannel = flapsChannel;
                    iFlightModel.FlapsExtended = !iFlightModel.FlapsExtended;
                }
            }
            if (AircraftParameters.HasRetracts)
            {
                int gearChannel = owner.InputManager.GetAxisValue("gear");
                if (Math.Abs(gearChannel - prevGearChannel) > 50)
                {
                    prevGearChannel = gearChannel;
                    iFlightModel.GearExtended = !iFlightModel.GearExtended;
                }
            }

            // Towing            
            if (AircraftParameters.AllowsTowing && owner.InputManager.KeyBoardState[Microsoft.DirectX.DirectInput.Key.T])
            {
                if (!towKeyDown)
                {
                    if (!iFlightModel.CableEnabled)
                    {
                        if (towing == null)
                            towing = new Towing();
                        owner.CenterHud.ShowGameText("", 1f);
                        iFlightModel.Reset();
                        crashTime = 0;
                        towing.Start();
                        iFlightModel.CableEnabled = true;
                        iFlightModel.CableLength = 10f;
                    }
                    else
                    {
                        iFlightModel.CableEnabled = false;
                    }
                }
                towKeyDown = true;                
            }
            else
                towKeyDown = false;
            if (iFlightModel.CableEnabled)
            {
                if (towing.Time > 70f)
                    iFlightModel.CableEnabled = false;
            }

            if ((iFlightModel.AircraftParameters.Channels < 4) && (useAileronForRudder))
                rudder = aileron;
            iFlightModel.Throttle = throttle / 100.0;
            iFlightModel.Rudder = rudder / 100.0;
            iFlightModel.Elevator = elevator / 100.0;
            iFlightModel.Ailerons = aileron / 100.0;
            if (expoRoll)
                iFlightModel.Ailerons *= iFlightModel.Ailerons * Math.Sign(aileron);
            if (expoPitch)
                iFlightModel.Elevator *= iFlightModel.Elevator * Math.Sign(elevator);
            if (expoYaw)
                iFlightModel.Rudder *= iFlightModel.Rudder * Math.Sign(rudder);

            float height = heightmap.GetHeightAt(-iFlightModel.Y, -iFlightModel.X);

            iFlightModel.UpdateControls(elapsedTime);
            airplane.Position = new Vector3(-iFlightModel.Y, -iFlightModel.Z, -iFlightModel.X);
            Vector3 angles = iFlightModel.Angles;
            airplane.YawPitchRoll = new Vector3(angles.Z, angles.Y, angles.X);
            
            airplane.OnFrameMove(device, totalTime, elapsedTime);

            iFlightModel.Wind = wind.GetWindAt(airplane.Position);

            //windVector.Vertex1 = airplane.Position;
            //windVector.Vertex2 = airplane.Position + iFlightModel.Wind;
            //windVector.OnFrameMove(device, totalTime, elapsedTime);
            //debugObject.Position = model.DebugPosition;
            //debugObject.OnFrameMove(device, totalTime, elapsedTime);
            
            if (towing != null)
            {
                if (iFlightModel.CableEnabled)
                {
                    towLine.Vertex1 = airplane.Position;
                    towLine.Vertex2 = towing.Position;
                    towLine.OnFrameMove(device, totalTime, elapsedTime);
                }

                iFlightModel.CableOrigin = FlightModelWind.ToModel(towing.Position);
                iFlightModel.CableVelocity = FlightModelWind.ToModel(towing.Velocity);
                towing.OnFrameMove(device, totalTime, elapsedTime);
            }

            smoke.Position = this.Position - (float)iFlightModel.Throttle * this.Airplane.Front;
            smoke.OnFrameMove(device, totalTime, elapsedTime);
            if (iFlightModel.OnWater)
                ripples.AddRipple((float)iFlightModel.Speed, Position, totalTime);
            ripples.OnFrameMove(device, totalTime, elapsedTime);

            //Program.Instance.CenterHud.ShowGameText(model.RelativeRotorForce.ToString(), totalTime, 1.0);
            if ((iFlightModel.Crashed) && (crashTime == 0))
            {
                owner.HandleCrash();
                this.HandleCrash();
                crashTime = totalTime;
                Framework.Instance.CurrentCamera.Shake(totalTime, 1.0, 0.05f, 50);
            }
            //normalObject.Position = new Vector3(airplane.Position.X, height, airplane.Position.Z);
            //terrainNormal.Vertex2 = heightmap.GetNormalAt(airplane.Position.X, airplane.Position.Z);

            // Collisions
            if (!iFlightModel.Crashed)
            {
                // Collisions
                List<Vector3> newColPoints = iFlightModel.CollisionPoints;
                for (int i = 0; i < Math.Min(newColPoints.Count, prevColPoints.Count); i++)
                {
                    Vector3 rayPos = prevColPoints[i];
                    Vector3 rayDir = newColPoints[i] - prevColPoints[i];
                    foreach (CollisionMesh collisionMesh in CollisionManager.MeshList)
                    {
                        if (collisionMesh.Intersects(rayPos, rayDir))
                        {
                            iFlightModel.Crashed = true;
                            iFlightModel.Velocity = new Vector3(0, 0, 0);
                            newColPoints = new List<Vector3>();
                            break;
                        }
                    }
                }
                prevColPoints = newColPoints;
            }
            prevPos = this.Position;
        }

        /// <summary>
        /// Handles the FrameRender.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="totalTime"></param>
        /// <param name="elapsedTime"></param>
        public void OnFrameRender(Device device, double totalTime, float elapsedTime)
        {
            if (!Framework.Instance.CurrentCamera.Reflected)
            {
                // Render the shadow (if not reflecting)
                Vector3 p1;
                Vector3 p2;
                Vector3 p3;
                Vector3 p4 = new Vector3(0, 0.005f, 0);
                heightmap.GetPoints(airplane.Position.X, airplane.Position.Z, out p1, out p2, out p3);

                airplane.OnRenderShadow(device, p1 + p4, p2 + p4, p3 + p4, new Vector3(0, -1, 0));
                ripples.OnFrameRender(device, totalTime, elapsedTime);
            }
            //debugObject.OnFrameRender(device, totalTime, elapsedTime);
            if (iFlightModel.CableEnabled)
                towLine.OnFrameRender(device, totalTime, elapsedTime);
            //windVector.OnFrameRender(device, totalTime, elapsedTime);
            if (Framework.Instance.CurrentCamera.CameraModeName.Equals("cockpit"))
                airplane.Visible = false;
            else
                airplane.Visible = true;
            airplane.OnFrameRender(device, totalTime, elapsedTime);
            if (towing != null)
                towing.OnFrameRender(device, totalTime, elapsedTime);
            smoke.OnFrameRender(device, totalTime, elapsedTime);            
            //normalObject.OnFrameRender(device, totalTime, elapsedTime);
        }
        #endregion
    }
}
