using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Core.Interfaces;
using RCSim.Interfaces;
using System.Drawing;
using Microsoft.DirectX;
using Bonsai.Core;
using System.Windows.Forms;
using System.IO;
using Bonsai.Objects;
using Bonsai.Objects.Terrain;
using Bonsai.Objects.Cameras;
using RCSim.DataClasses;
using RCSim.Effects;

namespace RCSim.AircraftEditor
{
    class ModelControl : IFrameworkCallback, IDisposable
    {
        #region private fields
        //private FlightModelWind flightModelWind = null;
        private IFlightModel iFlightModel = null;
        private AirplaneModel airplaneModel = null;
        protected Point lastMousePosition = Point.Empty;
        protected Point lastMiddleMousePosition = Point.Empty;
        protected Vector2 mouseDelta;
        protected bool resetCursorAfterMove = false;
        protected float rotationScaler = 0.00001f;
        protected float framesToSmoothMouseData = 2.0f;
        protected Vector2 rotationVelocity;
        protected bool rightMouseDown = false;
        protected bool middleMouseDown = false;
        protected Heightmap heightmap = null;
        protected bool flying = false;
        protected Vector3 pilotPosition = new Vector3(0, -3.7f, 0);
        private bool flapsKeyDown = false;
        private bool gearKeyDown = false;
        private int prevFlapsChannel = 0;
        private int prevGearChannel = 0;
        protected bool useAileronForRudder = false;

        // Effects
        private Reflection reflection = null;

        protected float camDistance = 5.0f;
        protected float camYaw = 0.0f;
        protected float camPitch = 0.0f;
        #endregion

        #region Public properties
        /// <summary>
        /// Gets a reference to the AirplaneModel
        /// </summary>
        public AirplaneModel AirplaneModel
        {
            get { return airplaneModel; }
        }

        /// <summary>
        /// Gets/sets the editor is in fly mode.
        /// </summary>
        public bool Flying
        {
            get { return flying; }
            set
            { 
                flying = value;
                if (flying)
                {
                    airplaneModel.Position = new Vector3(0, -5f, 0);
                    airplaneModel.YawPitchRoll = new Vector3(0, 0, 0);
                    Reset();
                    iFlightModel.Paused = false;
                    Program.Instance.SetPilotCam();
                }
                else
                {
                    iFlightModel.Paused = true;
                    airplaneModel.Position = new Vector3(0, 0, 0);
                    airplaneModel.YawPitchRoll = new Vector3(0, 0, 0);
                    airplaneModel.KillEngine();
                    Program.Instance.SetEditorCam();
                }
            }
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
        public ModelControl()
        {
            heightmap = new Heightmap(1000);
            heightmap.MinHeight = -5f;
            heightmap.MaxHeight = 5f;
        }

        public ModelControl(string filename, bool create)
        {
            reflection = new Reflection();

            heightmap = new Heightmap(1000);
            heightmap.MinHeight = -5f;
            heightmap.MaxHeight = 5f;

            useAileronForRudder = Convert.ToBoolean(Bonsai.Utils.Settings.GetValue("UseAileronChannel"));

            AircraftParameters parameters = new AircraftParameters();
            if (create)
                parameters.CreateDefault(filename);
            else
                parameters.File = filename;
            if (parameters.Version == 2)
                iFlightModel = new FlightModelWind2();
            else
                iFlightModel = new FlightModelWind();
            iFlightModel.AircraftParameters = parameters;
            iFlightModel.Heightmap = heightmap;
            iFlightModel.Paused = true;
            iFlightModel.Initialize();
            airplaneModel = new AirplaneModel(iFlightModel);
            airplaneModel.Position = new Vector3(0, 0, 0);
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Clean up.
        /// </summary>
        public void Dispose()
        {
            if (airplaneModel != null)
            {
                airplaneModel.Dispose();
                airplaneModel = null;
            }

            if (iFlightModel != null)
            {
                iFlightModel.Dispose();
                iFlightModel = null;
            }

            if (reflection != null)
            {
                reflection.Dispose();
                reflection = null;
            }
        }

        #endregion

        #region Protected methods
        protected void UpdateMouseDelta(float elapsedTime)
        {
            Point current = System.Windows.Forms.Cursor.Position;
            if (lastMousePosition == Point.Empty)
                lastMousePosition = current;
            if (lastMiddleMousePosition == Point.Empty)
                lastMiddleMousePosition = current;
            Point delta = new Point(current.X - lastMousePosition.X, current.Y - lastMousePosition.Y);
            float percentOfNew = 1.0f / framesToSmoothMouseData;
            float percentOfOld = 1.0f - percentOfNew;
            mouseDelta.X = mouseDelta.X * percentOfOld + delta.X * percentOfNew;
            mouseDelta.Y = mouseDelta.Y * percentOfOld + delta.Y * percentOfNew;
            rotationVelocity = mouseDelta * rotationScaler;
            lastMousePosition = current;
            lastMiddleMousePosition = current;

            if (resetCursorAfterMove)
            {
                System.Windows.Forms.Screen activeScreen = System.Windows.Forms.Screen.PrimaryScreen;
                Point center = new Point(activeScreen.Bounds.Width / 2, activeScreen.Bounds.Height / 2);
                System.Windows.Forms.Cursor.Position = center;
                lastMousePosition = center;
            }
        }
        /*
        protected void AddChildren(GameObject gameObject, TreeNode treeNode)
        {
            foreach (GameObject child in gameObject.Children)
            {
                ControlSurface surface = child as ControlSurface;
                if (surface != null)
                {
                    TreeNode node = new TreeNode(surface.Name);
                    node.Tag = surface;
                    AddChildren(surface, node);
                    treeNode.Nodes.Add(node);
                }
            }
        }
         */
        #endregion

        #region Public methods
        public void Reset()
        {
            iFlightModel.Reset();
            iFlightModel.Z = 5f;
            if (iFlightModel.AircraftParameters.HandLaunched)
            {                
                iFlightModel.HandLaunch(-pilotPosition.Z, -pilotPosition.X, -pilotPosition.Y);
            }
            airplaneModel.StartEngine();
        }

        public void UpdateConstants()
        {
            if (iFlightModel != null)
                iFlightModel.UpdateConstants();
        }

        public void HandleMessages(IntPtr hWnd, NativeMethods.WindowMessage msg, IntPtr wParam, IntPtr lParam)
        {
            switch (msg)
            {                
                case NativeMethods.WindowMessage.RightButtonDown:
                    if (!rightMouseDown)
                        lastMousePosition = System.Windows.Forms.Cursor.Position;
                    rightMouseDown = true;
                    break;
                case NativeMethods.WindowMessage.RightButtonUp:
                    rightMouseDown = false;
                    break;
                case NativeMethods.WindowMessage.MiddleButtonDown:
                    if (!middleMouseDown)
                        lastMousePosition = System.Windows.Forms.Cursor.Position;
                    middleMouseDown = true;
                    break;
                case NativeMethods.WindowMessage.MiddleButtonUp:
                    middleMouseDown = false;
                    break;
            }
        }

        public void OnKeyEvent(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (!(Framework.Instance.CurrentCamera is ObserverCamera))
            {
                switch (e.KeyCode)
                {
                    case System.Windows.Forms.Keys.F1:
                        camYaw = 0f;
                        camPitch = 0f;
                        Framework.Instance.CurrentCamera.LookAt = new Vector3(0, 0, 0);
                        Framework.Instance.CurrentCamera.Up = new Vector3(0, 1, 0);
                        break;
                    case System.Windows.Forms.Keys.F2:
                        camYaw = (float)Math.PI / 2;
                        camPitch = (float)Math.PI / 2;
                        Framework.Instance.CurrentCamera.LookAt = new Vector3(0, 0, 0);
                        Framework.Instance.CurrentCamera.Up = new Vector3(0, 0, -1);
                        break;
                    case System.Windows.Forms.Keys.F3:
                        camYaw = (float)-Math.PI / 2;
                        camPitch = 0;
                        Framework.Instance.CurrentCamera.LookAt = new Vector3(0, 0, 0);
                        Framework.Instance.CurrentCamera.Up = new Vector3(0, 1, 0);
                        break;
                }
            }
        }

        public void OnMouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!(Framework.Instance.CurrentCamera is ObserverCamera))
            {
                float cameraDelta = (camDistance * e.Delta) / 1000f;
                camDistance -= cameraDelta;
                if (camDistance < 0.1f)
                    camDistance = 0.1f;
            }
        }
        /*
        public void BuildTreeView(TreeView treeview)
        {
            treeview.Nodes.Clear();
            if (airplaneModel != null)
            {
                TreeNode rootNode = new TreeNode(airplaneModel.AirplaneControl.AircraftParameters.FixedMesh);
                rootNode.Tag = airplaneModel;
                treeview.Nodes.Add(rootNode);
                AddChildren(airplaneModel, rootNode);
            }
        }
         */
        #endregion

        #region IFrameworkCallback Members

        public void OnFrameMove(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            if (!(Framework.Instance.CurrentCamera is ObserverCamera))
            {
                if (rightMouseDown || middleMouseDown)
                {
                    UpdateMouseDelta(elapsedTime);
                    if (rightMouseDown)
                    {
                        camYaw += mouseDelta.X / 40f;
                        camPitch += mouseDelta.Y / 40f;
                        Framework.Instance.CurrentCamera.Up = new Vector3(0, 1, 0);
                    }

                    if (middleMouseDown)
                    {
                        Framework.Instance.CurrentCamera.LookAt +=
                            Framework.Instance.CurrentCamera.Left * (mouseDelta.X / 160f) +
                            Framework.Instance.CurrentCamera.Up * (mouseDelta.Y / 160f);
                    }
                }
                Framework.Instance.CurrentCamera.LookFrom = Framework.Instance.CurrentCamera.LookAt + new Vector3(camDistance * (float)Math.Cos(camYaw) * (float)Math.Cos(camPitch),
                        camDistance * (float)Math.Sin(camPitch), camDistance * (float)Math.Sin(camYaw) * (float)Math.Cos(camPitch));
            }
            if (iFlightModel != null)
            {
                int throttle = 0;
                int rudder = 0;
                int elevator = 0;
                int aileron = 0;
                if ((Program.Instance.InputManager != null) &&
                    (Program.Instance.InputManager.IsJoyStickAvailable))
                {
                    throttle = Program.Instance.InputManager.GetAxisValue("throttle");
                    rudder = Program.Instance.InputManager.GetAxisValue("rudder");
                    elevator = Program.Instance.InputManager.GetAxisValue("elevator");
                    aileron = Program.Instance.InputManager.GetAxisValue("aileron");
                }
                // Flaps & Gear
                if (Program.Instance.InputManager.KeyBoardState != null)
                {
                    if (airplaneModel.AirplaneControl.AircraftParameters.HasFlaps)
                    {
                        if (Program.Instance.InputManager.KeyBoardState[Microsoft.DirectX.DirectInput.Key.F])
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
                    if (airplaneModel.AirplaneControl.AircraftParameters.HasRetracts)
                    {
                        if (Program.Instance.InputManager.KeyBoardState[Microsoft.DirectX.DirectInput.Key.G])
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
                if (airplaneModel.AirplaneControl.AircraftParameters.HasFlaps)
                {
                    int flapsChannel = Program.Instance.InputManager.GetAxisValue("flaps");
                    if (Math.Abs(flapsChannel - prevFlapsChannel) > 50)
                    {
                        prevFlapsChannel = flapsChannel;
                        iFlightModel.FlapsExtended = !iFlightModel.FlapsExtended;
                    }
                }
                if (airplaneModel.AirplaneControl.AircraftParameters.HasRetracts)
                {
                    int gearChannel = Program.Instance.InputManager.GetAxisValue("gear");
                    if (Math.Abs(gearChannel - prevGearChannel) > 50)
                    {
                        prevGearChannel = gearChannel;
                        iFlightModel.GearExtended = !iFlightModel.GearExtended;
                    }
                }
                if ((iFlightModel.AircraftParameters.Channels < 4) && (useAileronForRudder))
                    rudder = aileron;
                iFlightModel.Throttle = throttle / 100.0;
                iFlightModel.Rudder = rudder / 100.0;
                iFlightModel.Elevator = elevator / 100.0;
                iFlightModel.Ailerons = aileron / 100.0;

                iFlightModel.UpdateControls(elapsedTime);

                if (flying)
                {
                    airplaneModel.Position = new Vector3(-iFlightModel.Y, -iFlightModel.Z, -iFlightModel.X);
                    Vector3 angles = iFlightModel.Angles;
                    airplaneModel.YawPitchRoll = new Vector3(angles.Z, angles.Y, angles.X);

                    if (iFlightModel.Crashed)
                    {
                        iFlightModel.Reset();
                        iFlightModel.Z = 5f;
                        if (iFlightModel.AircraftParameters.HandLaunched)
                        {
                            iFlightModel.HandLaunch(-pilotPosition.Z, -pilotPosition.X, -pilotPosition.Y);
                        }
                        airplaneModel.Position = new Vector3(0, -5f, 0);
                        airplaneModel.YawPitchRoll = new Vector3(0, 0, 0);
                    }
                }
                if (airplaneModel != null)
                    airplaneModel.OnFrameMove(device, totalTime, elapsedTime);
            }
        }

        public void OnFrameRender(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {            
            if (airplaneModel != null)
            {
                // Render the shadow
                Vector3 p1;
                Vector3 p2;
                Vector3 p3;
                Vector3 p4 = new Vector3(0, 0.005f, 0);
                heightmap.GetPoints(airplaneModel.Position.X, airplaneModel.Position.Z, out p1, out p2, out p3);
                airplaneModel.OnRenderShadow(device, p1 + p4, p2 + p4, p3 + p4, new Vector3(0, -1, 0));
                // Render the airplane
                airplaneModel.OnFrameRender(device, totalTime, elapsedTime);
            }
        }
        #endregion
    }
}
