using System;
using System.Collections.Generic;

using Bonsai.Core;
using Bonsai.Core.Interfaces;
using Bonsai.Core.Controls;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Bonsai.Core.EventArgs;
using Bonsai.Objects;
using Bonsai.Objects.Cameras;
using Bonsai.Objects.Meshes;
using Bonsai.Objects.Lights;
using Bonsai.Objects.Terrain;
using Bonsai.Sound;
using Bonsai.Input;
using RCSim.AircraftEditor;
using System.Drawing;
//using System.Windows.Forms;
using RCSim.AircraftEditor.Dialogs;
using System.Windows.Forms;
using System.IO;

namespace RCSim
{
    internal class Program : IFrameworkCallback, IDeviceCreation
    {
        #region Private fields
        private Framework sampleFramework = null; // Framework for samples
        //private Dialog hud = null; // dialog for standard controls
        private GameObject sky = null;
        private GameObject scenery = null;
        private ModelControl modelControl = null;
        private CameraBase cameraEditor = null;
        private ObserverCamera cameraPilot = null;
        private DirectionalLight sun = null;
        private TransparentObjectManager transparentObjectManager = null;
        private Heightmap heightmap = new Heightmap(1000);
        private Weather weather = null;
        private InputManager inputManager = null;
        private LineMesh rotationAxisMesh = null;
        private CollisionPoints collisionPoints = null;
        private Cursor3d cursor3d = null;
        private Cursor3d cursor3d2 = null;
        private string openFile = null;
        private double currentTime = 0;


        // HUD Ui Control constants
        private const int ToggleFullscreen = 1;
        private const int ToggleReference = 3;
        private const int ChangeDevice = 4;
        private const int Detail = 5;
        private const int DetailLabel = 6;
        private const int UseOptimizedCheckBox = 7;

        // Dialogs
        private ToolboxForm toolbox = null;


        // Statis fields
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
        /// Gets the InputManager.
        /// </summary>
        public InputManager InputManager
        {
            get { return inputManager; }
        }

        /// <summary>
        /// Get a reference to the running program.
        /// </summary>
        public static Program Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// Gets the current time.
        /// </summary>
        public double CurrentTime
        {
            get { return currentTime; }
        }

        /// <summary>
        /// Gets the aircraft folder of the sim.
        /// </summary>
        public static string AircraftFolder
        {
            get
            {
                return string.Concat(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "\\RC Desk Pilot\\Aircraft\\");
            }
        }

        /// <summary>
        /// Gets the heightmap of the current scenery.
        /// </summary>
        public Heightmap Heightmap
        {
            get { return heightmap; }
        }

        /// <summary>
        /// Gets the weather.
        /// </summary>
        public Weather Weather
        {
            get { return weather; }
        }

        /// <summary>
        /// Gets the currently opened file.
        /// </summary>
        public string OpenFile
        {
            get { return openFile; }
        }

        /// <summary>
        /// Determines whether the collisionpoints should be rendered.
        /// </summary>
        public bool CollisionPointsVisible
        {
            get;
            set;
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
                return sun.Direction;
            }
            set
            {
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

        /// <summary>
        /// Gets/Sets whether the 3D cursor should be visible.
        /// </summary>
        public bool CursorVisible
        {
            get;
            set;
        }

        /// <summary>
        /// Gets/Sets whether the 2nd 3D cursor should be visible.
        /// </summary>
        public bool Cursor2Visible
        {
            get;
            set;
        }

        /// <summary>
        /// Gets/Sets the position of the 3D cursor.
        /// </summary>
        public Vector3 CursorPosition
        {
            get 
            { 
                if (cursor3d != null)
                    return cursor3d.Position; 
                else
                    return Vector3.Empty;
            }
            set 
            {
                if (cursor3d != null)
                    cursor3d.Position = value;
            }
        }

        /// <summary>
        /// Gets/Sets the position of the 2nd 3D cursor.
        /// </summary>
        public Vector3 Cursor2Position
        {
            get
            {
                if (cursor3d2 != null)
                    return cursor3d2.Position;
                else
                    return Vector3.Empty;
            }
            set
            {
                if (cursor3d2 != null)
                    cursor3d2.Position = value;
            }
        }
        #endregion

        #region Constructor
        public Program(Framework f)
        {
            instance = this;
            sampleFramework = f;
            //hud = new Dialog(sampleFramework);
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
                Program program = new Program(framework);
                // Set the callback functions. These functions allow the sample framework to notify
                // the application about device changes, user input, and windows messages.  The 
                // callbacks are optional so you need only set callbacks for events you're interested 
                // in. However, if you don't handle the device reset/lost callbacks then the sample 
                // framework won't be able to reset your device since the application must first 
                // release all device resources before resetting.  Likewise, if you don't handle the 
                // device created/destroyed callbacks then the sample framework won't be able to 
                // recreate your device resources.
                framework.Disposing += new EventHandler(program.OnDestroyDevice);
                framework.DeviceLost += new EventHandler(program.OnLostDevice);
                framework.DeviceCreated += new DeviceEventHandler(program.OnCreateDevice);
                framework.DeviceReset += new DeviceEventHandler(program.OnResetDevice);

                framework.SetWndProcCallback(new WndProcCallback(program.OnMsgProc));

                framework.SetCallbackInterface(program);
                try
                {

                    // Show the cursor and clip it when in full screen
                    framework.SetCursorSettings(true, true);

                    // Initialize
                    program.InitializeApplication();

                    // Initialize the sample framework and create the desired window and Direct3D 
                    // device for the application. Calling each of these functions is optional, but they
                    // allow you to set several options which control the behavior of the sampleFramework.
                    framework.Initialize(true, true, true); // Parse the command line, handle the default hotkeys, and show msgboxes
                    framework.CreateWindow("R/C Desk Pilot: aircraft editor", new Icon("icon_blueprint_42.ico"));
                    // Hook the keyboard event
                    framework.Window.KeyDown += new System.Windows.Forms.KeyEventHandler(program.OnKeyEvent);
                    framework.Window.MouseWheel += new System.Windows.Forms.MouseEventHandler(program.OnMouseWheel);
                    framework.CreateDevice(0, true, Framework.DefaultSizeWidth, Framework.DefaultSizeHeight,
                        program);

                    program.InitializeMenu();

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
                catch (Exception e)
            {
                // In release mode fail silently
#endif
                framework.DisplayErrorMessage(e);
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
        /// <summary>
        /// Initializes the application
        /// </summary>
        public void InitializeApplication()
        {
            int y = 10;
            // Initialize the HUD
            //Bonsai.Core.Controls.Button fullScreen = hud.AddButton(ToggleFullscreen, "Toggle full screen", 35, y, 125, 22);
            //Bonsai.Core.Controls.Button changeDevice = hud.AddButton(ChangeDevice, "Change Device (F2)", 35, y += 24, 125, 22);
            // Hook the button events for when these items are clicked
            //fullScreen.Click += new EventHandler(OnFullscreenClicked);
            //changeDevice.Click += new EventHandler(OnChangeDeviceClicked);
            
        }

        /// <summary>
        /// Notify the program that the scale has changed.
        /// </summary>
        public void ScaleChanged()
        {
            collisionPoints.UpdatePoints();
        }

        /// <summary>
        /// Notify the program that the position of one or more CollisionPoints has changed.
        /// </summary>
        public void CollisionPointsMoved()
        {
            collisionPoints.UpdatePointPositions();
        }

        /// <summary>
        /// Notify the program that the collection of CollisionPoints has changed.
        /// </summary>
        public void CollisionPointsUpdated()
        {
            collisionPoints.UpdatePoints();
        }

        /// <summary>
        /// Notify the program that the model control has changed.
        /// </summary>
        public void ModelControlChanged()
        {
            Framework.Instance.WindowForm.Text = "R/C Desk Pilot - Aircraft Editor";
            Framework.Instance.CurrentCamera = cameraEditor;
            if (modelControl != null)
            {
                toolbox.Visible = true;
                cameraPilot.TargetObject = modelControl.AirplaneModel;
                if (modelControl.AirplaneModel != null &&
                    modelControl.AirplaneModel.AirplaneControl != null &&
                    modelControl.AirplaneModel.AirplaneControl.AircraftParameters != null &&
                    modelControl.AirplaneModel.AirplaneControl.AircraftParameters.FileName != null)
                {
                    Framework.Instance.WindowForm.Text = "RCDP - " + modelControl.AirplaneModel.AirplaneControl.AircraftParameters.FileName;
                }
            }
        }

        public void SetPilotCam()
        {
            Framework.Instance.CurrentCamera = cameraPilot;
        }

        public void SetEditorCam()
        {
            Framework.Instance.CurrentCamera = cameraEditor;
        }

        /// <summary>
        /// Reloads the plane of the player.
        /// </summary>
        public void ReloadPlayer()
        {
            // Do nothing in the editor.
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
                    break;
                case System.Windows.Forms.Keys.Add:
                    if (Framework.Instance.CurrentCamera == cameraPilot)
                        cameraPilot.ZoomFactor *= 1.5f;
                    break;
                case System.Windows.Forms.Keys.Subtract:
                    if (Framework.Instance.CurrentCamera == cameraPilot)
                        cameraPilot.ZoomFactor /= 1.5f;
                    break;
            }
            if (modelControl != null)
                modelControl.OnKeyEvent(sender, e);
        }


        private void OnMouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (modelControl != null)
                modelControl.OnMouseWheel(sender, e);
        }

        /// <summary>Called when the change device button is clicked</summary>
        private void OnChangeDeviceClicked(object sender, EventArgs e)
        {
            sampleFramework.ShowSettingsDialog(!sampleFramework.IsD3DSettingsDialogShowing);
        }

        /// <summary>Called when the full screen button is clicked</summary>
        private void OnFullscreenClicked(object sender, EventArgs e)
        {
            sampleFramework.ToggleFullscreen();
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
            // Initialize the stats font
            //statsFont = ResourceCache.GetGlobalInstance().CreateFont(e.Device, 15, 0, FontWeight.Bold, 1, false, CharacterSet.Default,
            //    Precision.Default, FontQuality.Default, PitchAndFamily.FamilyDoNotCare | PitchAndFamily.DefaultPitch
            //    , "Arial");   
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            SoundManager.Initialize(Framework.Instance.Window);
            inputManager = new Bonsai.Input.InputManager(Framework.Instance.Window);

            transparentObjectManager = new TransparentObjectManager();
            weather = new Weather(this);

            //modelControl = new ModelControl("Data/cessna/cessna.par");
            modelControl = new ModelControl();

            rotationAxisMesh = new LineMesh();

            sky = new GameObject();
            DomeMesh skyDome = new DomeMesh(5000, 10, 16);
            skyDome.Texture = new Bonsai.Objects.Textures.TextureBase("data\\sky2.JPG");
            sky.Position = new Vector3(0, -5f, 0);
            sky.Mesh = skyDome;

            scenery = new GameObject();
            SquareMesh sceneryMesh = new SquareMesh(5000, 10, 10, 1000f);
            sceneryMesh.Texture = new Bonsai.Objects.Textures.TextureBase("data\\scenery\\default\\grass1.jpg");
            scenery.Mesh = sceneryMesh;
            scenery.Position = new Vector3(0, -5f, 0);
            scenery.RotateXAngle = (float)Math.PI / 2;

            cameraEditor = new CameraBase("ObserverCamera");
            cameraEditor.Near = 0.05f;

            cameraPilot = new ObserverCamera("PilotCamera", modelControl.AirplaneModel);
            cameraPilot.LookFrom = new Vector3(5f, -3.3f, 5f);
            
            //camera.LookAt = modelControl.AirplaneModel.Position;
            //camera.LookFrom = modelControl.AirplaneModel.Position + new Vector3(cameraDistance, 0f, 0f);
            //camera.AutoZoom = true;
            cameraEditor.AspectRatio = (float)(e.BackBufferDescription.Width) / e.BackBufferDescription.Height;
            cameraPilot.AspectRatio = (float)(e.BackBufferDescription.Width) / e.BackBufferDescription.Height;


            Framework.Instance.CurrentCamera = cameraEditor;

            sun = new DirectionalLight(new Vector3(-0.5f, -0.707f, 0.5f));
            sun.Color = System.Drawing.Color.FromArgb(148, 148, 148);

            collisionPoints = new CollisionPoints();

            cursor3d = new Cursor3d();
            cursor3d2 = new Cursor3d();
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

            cameraEditor.AspectRatio = (float)e.BackBufferDescription.Width / e.BackBufferDescription.Height;
            cameraPilot.AspectRatio = (float)e.BackBufferDescription.Width / e.BackBufferDescription.Height;
            System.Console.Out.WriteLine("Aspect = " + cameraEditor.AspectRatio);

            // Setup UI locations
            //hud.SetLocation(desc.Width - 170, 0);
            //hud.SetSize(170, 170);            
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
            if (modelControl != null && modelControl.AirplaneModel != null)
            {
                if (MessageBox.Show("Do you want to save changes?", "Save changes?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (!string.IsNullOrEmpty(openFile))
                        modelControl.AirplaneModel.AirplaneControl.AircraftParameters.Save(openFile);
                }
            }
            if (modelControl != null)
            {
                modelControl.Dispose();
                modelControl = null;
            }
            if (sky != null)
            {
                if (sky.Mesh != null)
                {
                    sky.Mesh.Dispose();
                    sky.Mesh = null;
                }
                sky.Dispose();
                sky = null;
            }

            Bonsai.Objects.Textures.TextureBase.DisposeAll();
        }

        /// <summary>
        /// Before handling window messages, the sample framework passes incoming windows 
        /// messages to the application through this callback function. If the application sets 
        /// noFurtherProcessing to true, the sample framework will not process the message
        /// </summary>
        public IntPtr OnMsgProc(IntPtr hWnd, NativeMethods.WindowMessage msg, IntPtr wParam, IntPtr lParam, ref bool noFurtherProcessing)
        {
            // Give the dialog a chance to handle the message first
            //noFurtherProcessing = hud.MessageProc(hWnd, msg, wParam, lParam);
            if (noFurtherProcessing)
                return IntPtr.Zero;
            
            if (modelControl != null)
            {
                modelControl.HandleMessages(hWnd, msg, wParam, lParam);
            }
            
            return IntPtr.Zero;
        }
        #endregion

        #region Private methods
        private void InitializeMenu()
        {
            MainMenu menu = new MainMenu();
            MenuItem fileItem = new MenuItem("&File");
            MenuItem fileNewItem = new MenuItem("&New", new EventHandler(OnFileNewClicked));
            fileItem.MenuItems.Add(fileNewItem);
            MenuItem fileOpenItem = new MenuItem("&Open", new EventHandler(OnFileOpenClicked));
            fileItem.MenuItems.Add(fileOpenItem);
            MenuItem fileSaveItem = new MenuItem("&Save", new EventHandler(OnFileSaveClicked));
            fileItem.MenuItems.Add(fileSaveItem);

            menu.MenuItems.Add(fileItem);

            MenuItem helpItem = new MenuItem("&Help");
            MenuItem helpLicenseItem = new MenuItem("&License", new EventHandler(OnHelpLicenseClicked));
            helpItem.MenuItems.Add(helpLicenseItem);
            MenuItem helpAboutItem = new MenuItem("&About", new EventHandler(OnHelpAboutClicked));
            helpItem.MenuItems.Add(helpAboutItem);
            menu.MenuItems.Add(helpItem);

            Framework.Instance.WindowForm.Menu = menu;

            toolbox = new ToolboxForm();
            toolbox.Show(Framework.Instance.WindowForm);
            toolbox.Visible = false;
            
            //modelControl.BuildTreeView(toolbox.TreeViewHierarchy);
            //toolbox.CursorEnabledChanged += new EventHandler(toolBox_CursorEnabledChanged);
        }

        private void OnFileNewClicked(object sender, EventArgs e)
        {
            NewAircraftForm newDialog = new NewAircraftForm();
            if (newDialog.ShowDialog() == DialogResult.OK)
            {
                if (newDialog.WizardResult == NewAircraftForm.WizardResultEnum.NewAircraft)
                {
                    DirectoryInfo newFolder = Directory.CreateDirectory(string.Concat(Program.AircraftFolder, newDialog.FolderName));
                    string fullFileName = string.Concat(newFolder.FullName, "\\", newDialog.AircraftName, ".par");
                    ModelControl newModelControl = new ModelControl(fullFileName, true);
                    if (newModelControl != null)
                    {
                        modelControl.Dispose();
                        modelControl = newModelControl;
                        toolbox.ModelControl = newModelControl;
                        collisionPoints.ModelControl = modelControl;
                        openFile = fullFileName;
                        modelControl.AirplaneModel.AirplaneControl.AircraftParameters.Save(fullFileName);
                        ModelControlChanged();
                    }
                }
                else
                {
                    FileInfo mainFile = new FileInfo(newDialog.BaseAircraft);
                    string fullFileName = string.Concat(mainFile.DirectoryName, "\\", newDialog.AircraftName, ".par");
                    if (File.Exists(fullFileName))
                    {
                        MessageBox.Show("This variation already exists, use the File > Open menu to open it", "Aircraft already exists");
                        return;
                    }
                    File.Copy(newDialog.BaseAircraft, fullFileName);
                    ModelControl newModelControl = new ModelControl(fullFileName, false);
                    if (newModelControl != null)
                    {
                        modelControl.Dispose();
                        modelControl = newModelControl;
                        toolbox.ModelControl = newModelControl;
                        collisionPoints.ModelControl = modelControl;
                        openFile = fullFileName;
                        ModelControlChanged();
                    }
                }
            }
        }

        private void OnFileOpenClicked(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "RC Desk Pilot Aircraft (*.par)|*.par";
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ModelControl newModelControl = new ModelControl(openDialog.FileName, false);
                    if (newModelControl != null)
                    {                        
                        modelControl.Dispose();
                        modelControl = newModelControl;
                        toolbox.ModelControl = newModelControl;
                        openFile = openDialog.FileName;
                        collisionPoints.ModelControl = modelControl;
                        ModelControlChanged();
                    }
                }
                catch
                {
                    MessageBox.Show("Not a valid aircraft file");
                }
            }
        }

        private void OnFileSaveClicked(object sender, EventArgs e)
        {
            if (modelControl != null)
            {
                if (!string.IsNullOrEmpty(openFile))
                    modelControl.AirplaneModel.AirplaneControl.AircraftParameters.Save(openFile);
            }
        }

        private void OnHelpLicenseClicked(object sender, EventArgs e)
        {
            RCSim.AircraftEditor.Dialogs.LicenseBox license = new LicenseBox();
            license.ShowDialog();
        }

        private void OnHelpAboutClicked(object sender, EventArgs e)
        {
            RCSim.AircraftEditor.Dialogs.AboutBox about = new AboutBox();
            about.ShowDialog();
        }
        #endregion

        #region IFrameworkCallback Members

        public void OnFrameMove(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            if (inputManager != null)
            {
                if (Framework.Instance.Window.Focused)
                    inputManager.Update();
                else
                    inputManager.UpdateJoystick();
            }
            
            sky.OnFrameMove(device, totalTime, elapsedTime);
            scenery.OnFrameMove(device, totalTime, elapsedTime);
            modelControl.OnFrameMove(device, totalTime, elapsedTime);
            collisionPoints.OnFrameMove(device, totalTime, elapsedTime);
            transparentObjectManager.OnFrameMove(device, totalTime, elapsedTime);
            if (CursorVisible)
                cursor3d.OnFrameMove(device, totalTime, elapsedTime);
            if (Cursor2Visible)
                cursor3d2.OnFrameMove(device, totalTime, elapsedTime);
            Framework.Instance.CurrentCamera.OnFrameMove(device, totalTime, elapsedTime);
        }

        internal void OnFrameRenderReflection(Device device, double totalTime, float elapsedTime)
        {
            device.Clear(ClearFlags.ZBuffer | ClearFlags.Target, Color.Blue, 1.0f, 0);
            device.Transform.World = Matrix.Identity;
            sky.OnFrameRender(device, totalTime, elapsedTime);
            scenery.OnFrameRender(device, totalTime, elapsedTime);
            transparentObjectManager.OnFrameRender(device, totalTime, elapsedTime);
        }

        public void OnFrameRender(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            currentTime = totalTime;

            if ((modelControl != null) && (modelControl.Reflection != null))
            {
                //if (screen == null)
                //    screen = device.GetRenderTarget(0);
                modelControl.Reflection.UpdateCubeMap(device, totalTime, elapsedTime, modelControl.AirplaneModel.Position);
                //device.SetRenderTarget(0, screen);

            }


            bool beginSceneCalled = false;

            // Clear the render target and the zbuffer 
            device.Clear(ClearFlags.ZBuffer | ClearFlags.Target, 0x00424B79, 1.0f, 0);
            try
            {
                device.BeginScene();
                Framework.Instance.Device.RenderState.Ambient = Color.FromArgb(148, 148, 148);
                beginSceneCalled = true;
                Framework.Instance.CurrentCamera.OnFrameRender(device, totalTime, elapsedTime);
                sky.OnFrameRender(device, totalTime, elapsedTime);
                scenery.OnFrameRender(device, totalTime, elapsedTime);
                modelControl.OnFrameRender(device, totalTime, elapsedTime);
                if (CollisionPointsVisible && (!modelControl.Flying))
                    collisionPoints.OnFrameRender(device, totalTime, elapsedTime);
                transparentObjectManager.OnFrameRender(device, totalTime, elapsedTime);
                if ((toolbox != null) && (toolbox.CurrentObject != null) && (!modelControl.Flying))
                {
                    // Render the wireframe
                    device.RenderState.FillMode = FillMode.WireFrame;
                    toolbox.CurrentObject.OnFrameRender(device, totalTime, elapsedTime);
                    // Render the rotationaxis
                    ControlSurface surface = toolbox.CurrentObject as ControlSurface;
                    if (surface != null)
                    {
                        rotationAxisMesh.Vertex1 = surface.WorldPosition * modelControl.AirplaneModel.AirplaneControl.AircraftParameters.Scale;
                        rotationAxisMesh.Vertex2 = (surface.WorldPosition + 2*surface.RotationAxis) * modelControl.AirplaneModel.AirplaneControl.AircraftParameters.Scale;
                        rotationAxisMesh.OnFrameRender(device, totalTime, elapsedTime);
                    }
                    device.RenderState.FillMode = FillMode.Solid;
                }

                if (CursorVisible)
                    cursor3d.OnFrameRender(device, totalTime, elapsedTime);

                if (Cursor2Visible)
                    cursor3d2.OnFrameRender(device, totalTime, elapsedTime);

                // Get the world matrix
                //Matrix worldMatrix = worldCenter * camera.WorldMatrix;
                
                // Show UI
                //hud.OnRender(elapsedTime);                
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
                Utility.DisplaySwitchingToRefWarning(sampleFramework, "ProgressiveMesh");
            }
        }

        #endregion
    }
}
