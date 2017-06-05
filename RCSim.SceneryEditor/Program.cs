using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using Bonsai.Core;
using Bonsai.Core.Interfaces;
using Bonsai.Core.Controls;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Bonsai.Core.EventArgs;
using Bonsai.Objects.Meshes;
using Bonsai.Objects.Cameras;
using Bonsai.Objects;
using Bonsai.Objects.Lights;
using Bonsai.Objects.Terrain;
using System.Drawing;

namespace RCSim
{
    class Program : IFrameworkCallback, IDeviceCreation
    {
        #region Private fields
        private Framework framework = null; // Framework for samples
        private Dialog hud = null; // dialog for standard controls
        private XMesh airplaneMesh = null;
        private Scenery scenery = null;
        private Birds birds = null;
        private FirstPersonCamera camera = null;
        private DirectionalLight sun = null;
        private Cursor3D cursor3d = null;
        private ToolBoxForm toolBox = null;
        private TransparentObjectManager transparentObjectManager = null;
        private Weather weather = null;
        private SelectedObject selectedObject = null;
        private double currentTime = 0;

        // HUD Ui Control constants
        private const int ToggleFullscreen = 1;
        private const int ToggleReference = 3;
        private const int ChangeDevice = 4;
        private const int Detail = 5;
        private const int DetailLabel = 6;
        private const int UseOptimizedCheckBox = 7;

        // Private static method
        private static Program instance = null;
        #endregion

        #region Public properties
        /// <summary>
        /// Gets the Transparent Object Manager.
        /// </summary>
        public TransparentObjectManager TransparentObjectManager
        {
            get { return transparentObjectManager; }
        }

        /// <summary>
        /// Gets the weather.
        /// </summary>
        public Weather Weather
        {
            get { return weather; }
        }

        /// <summary>
        /// Get a reference to the running program.
        /// </summary>
        public static Program Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// Gets the heightmap of the current scenery.
        /// </summary>
        public Heightmap Heightmap
        {
            get { return scenery.Heightmap; }
        }

        /// <summary>
        /// Gets the current time.
        /// </summary>
        public double CurrentTime
        {
            get { return currentTime; }
        }

        /// <summary>
        /// Gets/Sets the ambient light color.
        /// </summary>
        public Color AmbientLightColor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets/Sets the sunlight color.
        /// </summary>
        public Color SunLightColor
        {
            get;
            set;
        }

        /// <summary>
        /// Sets the position of the sun.
        /// </summary>
        public Vector3 SunPosition
        {
            get
            {
                if (sun != null)
                    return sun.Direction;
                else
                    return new Vector3();
            }
            set
            {
                if (sun != null)
                    sun.Direction = new Vector3(-value.X, -value.Y, -value.Z);
            }
        }


        /// <summary>
        /// Stub for the Player property.
        /// </summary>
        public GameObject Player
        {
            get
            {
                return null;
            }
        }
        
        #endregion

        #region Constructor
        public Program(Framework f)
        {
            instance = this;
            framework = f;
            hud = new Dialog(framework);
        }
        #endregion

        #region Main
        /// <summary>
        /// Entry point to the program. Initializes everything and goes into a message processing 
        /// loop. Idle time is used to render the scene.
        /// </summary>
        [STAThread]
        static int Main()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            using (Framework framework = new Framework())
            {
                Program sample = new Program(framework);
                // Set the callback functions. These functions allow the sample framework to notify
                // the application about device changes, user input, and windows messages.  The 
                // callbacks are optional so you need only set callbacks for events you're interested 
                // in. However, if you don't handle the device reset/lost callbacks then the sample 
                // framework won't be able to reset your device since the application must first 
                // release all device resources before resetting.  Likewise, if you don't handle the 
                // device created/destroyed callbacks then the sample framework won't be able to 
                // recreate your device resources.
                framework.Disposing += new EventHandler(sample.OnDestroyDevice);
                framework.DeviceLost += new EventHandler(sample.OnLostDevice);
                framework.DeviceCreated += new DeviceEventHandler(sample.OnCreateDevice);
                framework.DeviceReset += new DeviceEventHandler(sample.OnResetDevice);

                framework.SetWndProcCallback(new WndProcCallback(sample.OnMsgProc));

                framework.SetCallbackInterface(sample);
                try
                {

                    // Show the cursor and clip it when in full screen
                    framework.SetCursorSettings(true, true);

                    // Initialize
                    sample.InitializeApplication();

                    // Initialize the sample framework and create the desired window and Direct3D 
                    // device for the application. Calling each of these functions is optional, but they
                    // allow you to set several options which control the behavior of the sampleFramework.
                    framework.Initialize(true, true, true); // Parse the command line, handle the default hotkeys, and show msgboxes
                    framework.CreateWindow("RCSim: Scenery Editor");
                    // Hook the keyboard event
                    framework.Window.KeyDown += new System.Windows.Forms.KeyEventHandler(sample.OnKeyEvent);
                    framework.CreateDevice(0, true, Framework.DefaultSizeWidth, Framework.DefaultSizeHeight,
                        sample);

                    sample.InitializeMenu();

                    // Pass control to the sample framework for handling the message pump and 
                    // dispatching render calls. The sample framework will call your FrameMove 
                    // and FrameRender callback when there is idle time between handling window messages.
                    framework.MainLoop();

                }
#if(DEBUG)
                catch (Exception e)
                {
                    // In debug mode show this error (maybe - depending on settings)
                    framework.DisplayErrorMessage(e);
#else
            catch
            {
                // In release mode fail silently
#endif
                    // Ignore any exceptions here, they would have been handled by other areas
                    return (framework.ExitCode == 0) ? 1 : framework.ExitCode; // Return an error code here
                }

                // Perform any application-level cleanup here. Direct3D device resources are released within the
                // appropriate callback functions and therefore don't require any cleanup code here.
                return framework.ExitCode;
            }
        }
        #endregion

        #region Public methods
        public void SwitchToObserverCamera()
        {
            // do nothing
        }

        /// <summary>
        /// Initializes the application
        /// </summary>
        public void InitializeApplication()
        {
            int y = 10;
            // Initialize the HUD
            Bonsai.Core.Controls.Button fullScreen = hud.AddButton(ToggleFullscreen, "Toggle full screen", 35, y, 125, 22);
            Bonsai.Core.Controls.Button changeDevice = hud.AddButton(ChangeDevice, "Change Device (F2)", 35, y += 24, 125, 22);
            //fullScreen.IsVisible = false;
            //changeDevice.IsVisible = false;
            // Hook the button events for when these items are clicked
            fullScreen.Click += new EventHandler(OnFullscreenClicked);
            changeDevice.Click += new EventHandler(OnChangeDeviceClicked);            
        }
        #endregion

        #region private event handlers
        /// <summary>
        /// As a convenience, the sample framework inspects the incoming windows messages for
        /// keystroke messages and decodes the message parameters to pass relevant keyboard
        /// messages to the application.  The framework does not remove the underlying keystroke 
        /// messages, which are still passed to the application's MsgProc callback.
        /// </summary>
        private void OnKeyEvent(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case System.Windows.Forms.Keys.F1:
                    //isHelpShowing = !isHelpShowing;
                    break;
                case Keys.Tab:
                    RCSim.TerrainDefinition.ObjectTypeEnum objectType;
                    selectedObject.SetSelectedObject(scenery.Definition.GetNearestObject(cursor3d.Position, out objectType));
                    break;
            }
        }

        /// <summary>Called when the change device button is clicked</summary>
        private void OnChangeDeviceClicked(object sender, EventArgs e)
        {
            framework.ShowSettingsDialog(!framework.IsD3DSettingsDialogShowing);
        }

        /// <summary>Called when the full screen button is clicked</summary>
        private void OnFullscreenClicked(object sender, EventArgs e)
        {
            framework.ToggleFullscreen();
        }

        /// <summary>
        /// This event will be fired immediately after the Direct3D device has been 
        /// created, which will happen during application initialization and windowed/full screen 
        /// toggles. This is the best location to create Pool.Managed resources since these 
        /// resources need to be reloaded whenever the device is destroyed. Resources created  
        /// here should be released in the Disposing event. 
        /// </summary>
        private void OnCreateDevice(object sender, DeviceEventArgs e)
        {
            transparentObjectManager = new TransparentObjectManager();

            // Initialize the stats font
            //statsFont = ResourceCache.GetGlobalInstance().CreateFont(e.Device, 15, 0, FontWeight.Bold, 1, false, CharacterSet.Default,
            //    Precision.Default, FontQuality.Default, PitchAndFamily.FamilyDoNotCare | PitchAndFamily.DefaultPitch
            //    , "Arial");  
            // Create the camera
            camera = new FirstPersonCamera("FPCamera");
            camera.AspectRatio = (float)(e.BackBufferDescription.Width) / e.BackBufferDescription.Height;
            camera.MoveScaler = 50.0f;

            weather = new Weather(this);

            // Create the ground
            scenery = new Scenery(this);
            scenery.LoadDefinition("data/scenery/default/default.par");
            //scenery.LoadDefinition(@"C:\Users\Gebruiker\Documents\RC Desk Pilot\Scenery\Aero Club Bad Oldesloe\Aero Club Bad Oldesloe.par");
            scenery.SetWaterCallback(this.OnFrameRenderWater);

            // Create birds
            birds = new Birds(100);
            birds.Random = true;

            // Create the 3Dcursor
            cursor3d = new Cursor3D();

            selectedObject = new SelectedObject();
            RCSim.TerrainDefinition.ObjectTypeEnum objectType;
            selectedObject.SetSelectedObject(scenery.Definition.GetNearestObject(cursor3d.Position, out objectType));

            sun = new DirectionalLight(new Vector3(0.5f, -0.707f, 0.5f));
            sun.Color = System.Drawing.Color.FromArgb(148, 148, 148);
 
            framework.CurrentCamera = camera;
        }

        /// <summary>
        /// This event will be fired immediately after the Direct3D device has been 
        /// reset, which will happen after a lost device scenario. This is the best location to 
        /// create Pool.Default resources since these resources need to be reloaded whenever 
        /// the device is lost. Resources created here should be released in the OnLostDevice 
        /// event. 
        /// </summary>
        private void OnResetDevice(object sender, DeviceEventArgs e)
        {
            SurfaceDescription desc = e.BackBufferDescription;
            camera.AspectRatio = (float)(e.BackBufferDescription.Width) / e.BackBufferDescription.Height;
            System.Console.Out.WriteLine(camera.AspectRatio);
            // Setup UI locations
            hud.SetLocation(desc.Width - 170, 0);
            hud.SetSize(170, 170);            
        }

        /// <summary>
        /// This event function will be called fired after the Direct3D device has 
        /// entered a lost state and before Device.Reset() is called. Resources created
        /// in the OnResetDevice callback should be released here, which generally includes all 
        /// Pool.Default resources. See the "Lost Devices" section of the documentation for 
        /// information about lost devices.
        /// </summary>
        private void OnLostDevice(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// This callback function will be called immediately after the Direct3D device has 
        /// been destroyed, which generally happens as a result of application termination or 
        /// windowed/full screen toggles. Resources created in the OnCreateDevice callback 
        /// should be released here, which generally includes all Pool.Managed resources. 
        /// </summary>
        private void OnDestroyDevice(object sender, EventArgs e)
        {
           
        }

        /// <summary>
        /// Before handling window messages, the sample framework passes incoming windows 
        /// messages to the application through this callback function. If the application sets 
        /// noFurtherProcessing to true, the sample framework will not process the message
        /// </summary>
        public IntPtr OnMsgProc(IntPtr hWnd, NativeMethods.WindowMessage msg, IntPtr wParam, IntPtr lParam, ref bool noFurtherProcessing)
        {
            // Give the dialog a chance to handle the message first
            noFurtherProcessing = hud.MessageProc(hWnd, msg, wParam, lParam);
            if (noFurtherProcessing)
                return IntPtr.Zero;
            if (camera != null)
            {
                camera.HandleMessages(hWnd, msg, wParam, lParam);
            }
            if (cursor3d != null)
            {
                cursor3d.HandleMessages(hWnd, msg, wParam, lParam);
            }
            
            return IntPtr.Zero;
        }
        #endregion

        #region IFrameworkCallback Members

        public void OnFrameMove(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            if (airplaneMesh != null)
                airplaneMesh.OnFrameMove(device, totalTime, elapsedTime);
            scenery.OnFrameMove(device, totalTime, elapsedTime);
            birds.OnFrameMove(device, totalTime, elapsedTime);
            cursor3d.OnFrameMove(device, totalTime, elapsedTime);
            selectedObject.OnFrameMove(device, totalTime, elapsedTime);
            transparentObjectManager.OnFrameMove(device, totalTime, elapsedTime);
            toolBox.CursorPosition = cursor3d.Position.ToString();
        }

        private void OnFrameRenderWater(Device device, double totalTime, float elapsedTime, bool reflection)
        {
            device.Clear(ClearFlags.ZBuffer | ClearFlags.Target, Color.Blue, 1.0f, 0);
            device.Transform.World = Matrix.Identity;
            scenery.OnFrameRender(device, totalTime, elapsedTime);
            transparentObjectManager.OnFrameRender(device, totalTime, elapsedTime);
        }

        public void OnFrameRender(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            currentTime = totalTime;
            bool beginSceneCalled = false;

            if (scenery != null)
            {
                scenery.RenderTextures(device, totalTime, elapsedTime);
            }

            // Clear the render target and the zbuffer 
            device.Clear(ClearFlags.ZBuffer | ClearFlags.Target, 0x00424B79, 1.0f, 0);
            try
            {
                device.BeginScene();
                beginSceneCalled = true;
                
                // Get the world matrix
                //Matrix worldMatrix = worldCenter * camera.WorldMatrix;
                Framework.Instance.Device.RenderState.Ambient = System.Drawing.Color.FromArgb(148, 148, 148);
                scenery.OnFrameRender(device, totalTime, elapsedTime);
                birds.OnFrameRender(device, totalTime, elapsedTime);
                if (airplaneMesh != null)
                    airplaneMesh.OnFrameRender(device, totalTime, elapsedTime);
                transparentObjectManager.OnFrameRender(device, totalTime, elapsedTime);
                cursor3d.OnFrameRender(device, totalTime, elapsedTime);
                selectedObject.OnFrameRender(device, totalTime, elapsedTime);
                // Show UI
                hud.OnRender(elapsedTime);                
            }
            finally
            {
                if (beginSceneCalled)
                    device.EndScene();
            }
        }

        #endregion

        #region IDeviceCreation Members

        public bool IsDeviceAcceptable(Microsoft.DirectX.Direct3D.Caps caps, Microsoft.DirectX.Direct3D.Format adapterFormat, Microsoft.DirectX.Direct3D.Format backBufferFormat, bool isWindowed)
        {
            // No fallback, need at least PS1.1
            if (caps.PixelShaderVersion < new Version(1, 1))
                return false;

            // Skip back buffer formats that don't support alpha blending
            if (!Manager.CheckDeviceFormat(caps.AdapterOrdinal, caps.DeviceType, adapterFormat,
                Usage.QueryPostPixelShaderBlending, ResourceType.Textures, backBufferFormat))
                return false;

            return true;
        }

        public void ModifyDeviceSettings(DeviceSettings settings, Microsoft.DirectX.Direct3D.Caps caps)
        {
            // If device doesn't support HW T&L or doesn't support 1.1 vertex shaders in HW 
            // then switch to SWVP.
            if ((!caps.DeviceCaps.SupportsHardwareTransformAndLight) ||
                (caps.VertexShaderVersion < new Version(1, 1)))
            {
                settings.BehaviorFlags = CreateFlags.SoftwareVertexProcessing;
            }
            else
            {
                settings.BehaviorFlags = CreateFlags.HardwareVertexProcessing;
            }

            // This application is designed to work on a pure device by not using 
            // any get methods, so create a pure device if supported and using HWVP.
            if ((caps.DeviceCaps.SupportsPureDevice) &&
                ((settings.BehaviorFlags & CreateFlags.HardwareVertexProcessing) != 0))
                settings.BehaviorFlags |= CreateFlags.PureDevice;

            // Debugging vertex shaders requires either REF or software vertex processing 
            // and debugging pixel shaders requires REF.  
#if(DEBUG_VS)
            if (settings.DeviceType != DeviceType.Reference )
            {
                settings.BehaviorFlags &= ~CreateFlags.HardwareVertexProcessing;
                settings.BehaviorFlags |= CreateFlags.SoftwareVertexProcessing;
            }
#endif
#if(DEBUG_PS)
            settings.DeviceType = DeviceType.Reference;
#endif

            // For the first device created if its a REF device, optionally display a warning dialog box
            if (settings.DeviceType == DeviceType.Reference)
            {
                Utility.DisplaySwitchingToRefWarning(framework, "ProgressiveMesh");
            }
        }

        #endregion

        #region Private members
        private void InitializeMenu()
        {
            MainMenu menu = new MainMenu();
            MenuItem fileItem = new MenuItem("&File");
            MenuItem fileOpenItem = new MenuItem("&Open", new EventHandler(OnFileOpenClicked));
            MenuItem fileSaveItem = new MenuItem("&Save", new EventHandler(OnFileSaveClicked));
            menu.MenuItems.Add(fileItem);
            fileItem.MenuItems.Add(fileOpenItem);
            fileItem.MenuItems.Add(fileSaveItem);
            framework.WindowForm.Menu = menu;

            toolBox = new ToolBoxForm();
            //framework.WindowForm.Controls.Add(toolBox);
            toolBox.Show(framework.WindowForm);
            toolBox.CursorEnabledChanged += new EventHandler(toolBox_CursorEnabledChanged);
            toolBox.ObjectAdded += new ToolBoxForm.ObjectEventHandler(toolBox_ObjectAdded);
            toolBox.CreateLightMap += new EventHandler(toolBox_CreateLightMap);
            toolBox.GateAdded += new ToolBoxForm.GateEventHandler(toolBox_GateAdded);
        }

        void toolBox_GateAdded(object sender, ToolBoxForm.GateEventArgs e)
        {
            if (cursor3d != null)
            {
                scenery.AddGate(cursor3d.Position, e.Orientation, e.SequenceNr, e.GateType, true);
            }
        }

        void toolBox_CreateLightMap(object sender, EventArgs e)
        {
            scenery.CreateLightMap("data\\lightmap.png");
        }

        void toolBox_ObjectAdded(object sender, ToolBoxForm.ObjectEventArgs e)
        {
            if (cursor3d != null)
            {
                scenery.AddObject(e.ObjectType, cursor3d.Position, e.Orientation, e.FileName, true);
            }
        }

        void toolBox_CursorEnabledChanged(object sender, EventArgs e)
        {
            if (cursor3d != null)
                cursor3d.Enabled = toolBox.CursorEnabled;
        }
        #endregion

        #region Private Menu eventhandlers
        private void OnFileOpenClicked(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = ".x";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                scenery.LoadDefinition(dialog.FileName);
            }
        }

        private void OnFileSaveClicked(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = ".def";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                scenery.SaveDefinition(dialog.FileName);
            }
        }
        #endregion
    }
}
