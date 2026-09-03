using System;
using System.Collections.Generic;
using System.Text;

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
using Bonsai.Input;
using Bonsai.Sound;
using System.Drawing;
using RCSim.Dialogs;
using RCSim.Cameras;
using Bonsai.Objects.Shaders;
using Bonsai.Objects.Terrain;
using RCSim.DataClasses;

namespace RCSim
{
    class Program : IFrameworkCallback, IDeviceCreation
    {
        #region Private fields
        private Framework framework = null; // Framework for samples
        // Dialogs
        private Dialog hud = null; // dialog for standard controls
        private CenterHud centerHud = null;
        private MenuDialog menuDialog = null;
        private WelcomeDialog welcomeDialog = null;
        
        private Player player = null;
        private Scenery scenery = null;
        private Weather weather = null;
        private FlightRecorder recorder = null;
        private ObserverCamera observerCamera = null;
        private SpotCamera spotCamera = null;
        private OnBoardCamera onBoardCamera = null;
        private CameraBase baseCamera = null;
        private CinematicCamera cinematicCamera = null;
        private DirectionalLight sun = null;
        private InputManager inputManager = null;
        private Hud osd = null;
        private TransparentObjectManager transparentObjectManager = null;
        private AdManager adManager = null;
        private double currentTime = 0;
        //private Vector3 pilotPosition = new Vector3(0.1f, 1.7f, -15.0f);
        private Demo demo = null;
        private ComputerPilots computerPilots = null;
        private bool setFullScreen = false;
        private Game game = null;
        private bool anaglyph = false;
        private Vector3 pilotPosition = new Vector3(0.1f, + 1.7f, -15.0f);
        private Color ambientLightColor = Color.FromArgb(148, 148, 148);
        private Color sunLightColor = Color.FromArgb(148, 148, 148);        
        private string map = null;

        // HUD Ui Control constants
        private const int ToggleFullscreen = 1;
        private const int ToggleReference = 2;
        private const int ToggleMenu = 3;
        private const int ChangeDevice = 4;
        private const int Detail = 5;
        private const int DetailLabel = 6;
        private const int UseOptimizedCheckBox = 7;
        private const int CameraText = 8;

        // Statis fields
        private static Program instance = null;
        #endregion

        #region Public properties
        /// <summary>
        /// Gets the InputManager.
        /// </summary>
        public InputManager InputManager
        {
            get { return inputManager; }
        }
        
        /// <summary>
        /// Gets the Transparent Object Manager.
        /// </summary>
        public TransparentObjectManager TransparentObjectManager
        {
            get { return transparentObjectManager; }
        }

        /// <summary>
        /// Gets the Player.
        /// </summary>
        public Player Player
        {
            get { return player; }
        }

        /// <summary>
        /// Gets the scenery.
        /// </summary>
        public Scenery Scenery
        {
            get { return scenery; }
        }

        /// <summary>
        /// Gets the heightmap of the current scenery.
        /// </summary>
        public Heightmap Heightmap
        {
            get { return scenery.Heightmap; }
        }
        

        /// <summary>
        /// Gets the weather.
        /// </summary>
        public Weather Weather
        {
            get { return weather; }
        }

        /// <summary>
        /// Gets the position of the pilot on the field.
        /// </summary>
        public Vector3 PilotPosition
        {
           // get { return new Vector3(0.1f, scenery.Heightmap.GetHeightAt(0.1f, -15.0f) + 1.7f, -15.0f); }
            get { return pilotPosition; }
            set 
            { 
                pilotPosition = value; 
                if (observerCamera != null)
                    observerCamera.LookFrom = PilotPosition;
            }
        }

        /// <summary>
        /// Gets/sets the map of the current scenery.
        /// </summary>
        public string Map
        {
            get { return map; }
            set
            {
                map = value;
                centerHud.SetMapPicture(map);
            }
        }

        /// <summary>
        /// Gets/Sets the ambient light color.
        /// </summary>
        public Color AmbientLightColor
        {
            get { return ambientLightColor; }
            set { ambientLightColor = value; }
        }

        /// <summary>
        /// Gets/Sets the sunlight color.
        /// </summary>
        public Color SunLightColor
        {
            get { return sunLightColor; }
            set
            {
                sunLightColor = value;
                if (sun != null)
                    sun.Color = value;
            }
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
        /// Gets the current time.
        /// </summary>
        public double CurrentTime
        {
            get { return currentTime; }
        }

        /// <summary>
        /// Gets the current gametype.
        /// </summary>
        public Game.GameType CurrentGameType
        {
            get { return game.CurrentGameType; }
            set { game.CurrentGameType = value; }
        }

        /// <summary>
        /// Gets/Sets the AutoZoom value.
        /// </summary>
        public bool AutoZoom
        {
            get { return observerCamera.AutoZoom; }
            set 
            { 
                //observerCamera.AutoZoom = value; 
            }
        }

        /// <summary>
        /// Gets a reference to the WelcomeDialog.
        /// </summary>
        public WelcomeDialog WelcomeDialog
        {
            get { return welcomeDialog; }
        }

        /// <summary>
        /// Gets a reference to the MenuDialog.
        /// </summary>
        public MenuDialog MenuDialog
        {
            get { return menuDialog; }
        }

        /// <summary>
        /// Gets a reference to the center hud.
        /// </summary>
        public CenterHud CenterHud
        {
            get { return centerHud; }
        }

        /// <summary>
        /// Gets/sets the number of computer pilots.
        /// </summary>
        public int NumberOfComputerPilots
        {
            get { return computerPilots.NumberOfPilots; }
            set { computerPilots.NumberOfPilots = value; }
        }

        /// <summary>
        /// Gets a reference to the observer camera.
        /// </summary>
        public ObserverCamera ObserverCamera
        {
            get { return observerCamera; }
        }

        /// <summary>
        /// Get a reference to the running program.
        /// </summary>
        public static Program Instance
        {
            get { return instance; }
        }
        #endregion

        #region Constructor
        public Program(Framework f)
        {
            instance = this;
            framework = f;
            hud = new Dialog(framework);
            centerHud = new CenterHud(this);
        }
        #endregion

        #region Main
        /// <summary>
        /// Entry point to the program. Initializes everything and goes into a message processing 
        /// loop. Idle time is used to render the scene.
        /// </summary>
        static int Main()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            using (Framework framework = new Framework())
            {
                Program rcSim = new Program(framework);
                // Set the callback functions. These functions allow the sample framework to notify
                // the application about device changes, user input, and windows messages.  The 
                // callbacks are optional so you need only set callbacks for events you're interested 
                // in. However, if you don't handle the device reset/lost callbacks then the sample 
                // framework won't be able to reset your device since the application must first 
                // release all device resources before resetting.  Likewise, if you don't handle the 
                // device created/destroyed callbacks then the sample framework won't be able to 
                // recreate your device resources.
                framework.Disposing += new EventHandler(rcSim.OnDestroyDevice);
                framework.DeviceLost += new EventHandler(rcSim.OnLostDevice);
                framework.DeviceCreated += new DeviceEventHandler(rcSim.OnCreateDevice);
                framework.DeviceReset += new DeviceEventHandler(rcSim.OnResetDevice);

                framework.SetWndProcCallback(new WndProcCallback(rcSim.OnMsgProc));

                framework.SetCallbackInterface(rcSim);
                try
                {

                    // Show the cursor and clip it when in full screen
                    framework.SetCursorSettings(true, true);

                    // Initialize
                    rcSim.InitializeApplication();

                    // Initialize the sample framework and create the desired window and Direct3D 
                    // device for the application. Calling each of these functions is optional, but they
                    // allow you to set several options which control the behavior of the sampleFramework.
                    framework.Initialize(true, true, true); // Parse the command line, handle the default hotkeys, and show msgboxes
                    System.Drawing.Icon icon = new Icon("icon16.ico");
                    framework.CreateWindow("R/C Desk Pilot", icon);
                    // Hook the keyboard event
                    framework.Window.KeyDown += new System.Windows.Forms.KeyEventHandler(rcSim.OnKeyEvent);
                    framework.CreateDevice(0, true, Framework.DefaultSizeWidth, Framework.DefaultSizeHeight, rcSim);
                    

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
                framework.DisplayErrorMessage(e);
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
        /// <summary>
        /// Initializes the application
        /// </summary>
        public void InitializeApplication()
        {
            int y = 10;
            // Initialize the HUD
            //Button fullScreen = hud.AddButton(ToggleFullscreen, "Toggle full screen", 35, y, 125, 22);
            Button changeDevice = hud.AddButton(ChangeDevice, "Change Device (F2)", 35, y += 24, 125, 22);
            Button menuButton = hud.AddButton(ToggleMenu, "Menu", 35, y += 24, 125, 22);
            
            // Hook the button events for when these items are clicked
            //fullScreen.Click += new EventHandler(OnFullscreenClicked);
            changeDevice.Click += new EventHandler(OnChangeDeviceClicked);
            menuButton.Click += new EventHandler(OnToggleMenuClicked);
            try
            {
                SoundManager.Volume = Convert.ToInt32(Bonsai.Utils.Settings.GetValue("Volume"));
            }
            catch
            {
            }
        }

        /// <summary>
        /// Updates the target object of all camera's. Should be called after loading a new aircraft.
        /// </summary>
        public void UpdateCameras()
        {
            if (observerCamera != null)
                observerCamera.TargetObject = player.Airplane;
            if (spotCamera != null)
                spotCamera.TargetObject = player.Airplane;
            if (onBoardCamera != null)
                onBoardCamera.Airplane = player.Airplane;
            if (cinematicCamera != null)
            {
                cinematicCamera.Airplane = player.Airplane;
                cinematicCamera.Scenery = scenery;
            }
        }

        public void HandleCrash()
        {
            if (centerHud != null)
            {
                centerHud.Crash(currentTime);
            }
        }

        /// <summary>
        /// Shows the menu.
        /// </summary>
        public void ShowMenu()
        {
            framework.ShowDialog(menuDialog);
        }

        /// <summary>
        /// Changes the camera.
        /// </summary>
        public void ChangeView()
        {
            if (scenery.Parameters.SceneryType == SceneryParameters.SceneryTypeEnum.Full3D)
            {
                if (framework.CurrentCamera == cinematicCamera)
                {
                    framework.CurrentCamera = observerCamera;
                }
                else if (framework.CurrentCamera == observerCamera)
                {
                    framework.CurrentCamera = onBoardCamera;
                }
                else if (framework.CurrentCamera == onBoardCamera)
                {
                    framework.CurrentCamera = spotCamera;
                }
                else
                {
                    framework.CurrentCamera = cinematicCamera;
                }
            }
            if (!string.IsNullOrEmpty(framework.CurrentCamera.CameraModeName))
                centerHud.ShowCaption(string.Concat(framework.CurrentCamera.Name, " - ", framework.CurrentCamera.CameraModeName), currentTime);
            else
                centerHud.ShowCaption(framework.CurrentCamera.Name, currentTime);
        }

        /// <summary>
        /// Changes the mode of the current camera.
        /// </summary>
        public void ChangeCameraMode()
        {
            centerHud.ShowCaption(string.Concat(framework.CurrentCamera.Name, " - ", framework.CurrentCamera.NextMode()), currentTime);
        }

        public void SwitchToCinematicCamera()
        {
            framework.CurrentCamera = cinematicCamera;
        }

        public void SwitchToObserverCamera()
        {
            framework.CurrentCamera = observerCamera;
        }

        /// <summary>
        /// Zooms in the current camera.
        /// </summary>
        public void ZoomIn()
        {
            framework.CurrentCamera.ZoomFactor *= 1.5f;
        }

        /// <summary>
        /// Zooms out the current camera.
        /// </summary>
        public void ZoomOut()
        {
            framework.CurrentCamera.ZoomFactor *= 2f / 3f;
        }

        /// <summary>
        /// Sets the camera target
        /// </summary>
        /// <param name="target"></param>
        public void SetCameraTarget(GameObject target)
        {
            observerCamera.TargetObject = target;
            cinematicCamera.Airplane = (AirplaneModel)target;            
            onBoardCamera.Airplane = (AirplaneModel)target;
            spotCamera.TargetObject = target;
        }

        /// <summary>
        /// Determines whether or not the camera should switch to the water camera.
        /// </summary>
        /// <param name="atWater"></param>
        public void SetWaterCamera(bool atWater)
        {
            if (atWater)
            {
                if (observerCamera != null)
                {
                    observerCamera.LookFrom = Scenery.PilotWaterPosition;
                    centerHud.SetMapPicture(Scenery.WaterMap);
                }
            }
            else
            {
                if (observerCamera != null)
                {
                    observerCamera.LookFrom = PilotPosition;
                    centerHud.SetMapPicture(Map);
                }
            }
        }

        /// <summary>
        /// Reloads the plane of the player.
        /// </summary>
        public void ReloadPlayer()
        {
            if (Player != null)
                Player.ReloadModel();
        }

        /// <summary>
        /// Restarts the current game
        /// </summary>
        public void RestartGame()
        {
            if (game != null)
                game.Restart();
        }       
        #endregion

        #region Private methods
        private void RestoreResolution()
        {
            // Get some information
            DeviceSettings globalSettings = framework.DeviceSettings.Clone();
            int resX = Convert.ToInt32(Bonsai.Utils.Settings.GetValue("ResolutionWidth"));
            int resY = Convert.ToInt32(Bonsai.Utils.Settings.GetValue("ResolutionHeight"));
            globalSettings.presentParams.Windowed = false;
            globalSettings.presentParams.BackBufferWidth = resX;
            globalSettings.presentParams.BackBufferHeight = resY;
            if (globalSettings.presentParams.MultiSample != MultiSampleType.None)
            {
                globalSettings.presentParams.PresentFlag &= ~PresentFlag.LockableBackBuffer;
            }
            framework.CreateDeviceFromSettings(globalSettings);
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
                    //observerCamera.TargetObject = recorder.RecordedFlight.AirplaneModel;
                    //cinematicCamera.Airplane = recorder.RecordedFlight.AirplaneModel;
                    //isHelpShowing = !isHelpShowing;
                    break;
                case System.Windows.Forms.Keys.V:
                    if (!Framework.Instance.IsDialogShowing)
                        ChangeView();
                    break;
                case System.Windows.Forms.Keys.Enter:
                    if (!Framework.Instance.IsDialogShowing)
                    {
                        if (e.Alt) // Alt was pressed as well
                        {
                            Bonsai.Utils.Settings.SetValue("FullScreen", framework.Device.PresentationParameters.Windowed.ToString());
                            Framework.Instance.ToggleFullscreen();
                        }
                        else
                        {
                            player.Reset();
                        }
                    }
                    break;
                case System.Windows.Forms.Keys.S:
                    if (!Framework.Instance.IsDialogShowing)
                        player.ToggleSmoke();
                    break;
                case System.Windows.Forms.Keys.B:
                    if (!Framework.Instance.IsDialogShowing)
                        ChangeCameraMode();
                    break;
                case System.Windows.Forms.Keys.Add:
                    if (!Framework.Instance.IsDialogShowing)
                        ZoomIn();
                    break;
                case System.Windows.Forms.Keys.Subtract:
                    if (!Framework.Instance.IsDialogShowing)
                        ZoomOut();
                    break;
                case System.Windows.Forms.Keys.I:
                    if (!Framework.Instance.IsDialogShowing)
                        centerHud.ShowInfo = !centerHud.ShowInfo;
                    break;
                case System.Windows.Forms.Keys.F12:
                    if (!Framework.Instance.IsDialogShowing)
                    {
                        Weather.Wind.ShowVectorField = !Weather.Wind.ShowVectorField;
                        if (Weather.Wind.ShowVectorField)
                            centerHud.ShowGameText("Wind vectors: on", 2);
                        else
                            centerHud.ShowGameText("Wind vectors: off", 2);
                    }
                    break;
#if DEBUG                
                case System.Windows.Forms.Keys.R:
                    if (!Framework.Instance.IsDialogShowing)
                    {
                        recorder.Recording = !recorder.Recording;
                        if (recorder.Recording)
                            centerHud.ShowGameText("recording", 1000000);
                        else
                            centerHud.ShowGameText("", 0.0);
                    }
                    break;
#endif
                case System.Windows.Forms.Keys.Escape:
                    if (demo.Playing)
                    {
                        demo.Stop();
                    }
                    /*
                    else
                    {
                        player.Reset();
                        demo.Play();
                    }
                     */
                    // Received key to exit app, check for a form to close
                    else if (Framework.Instance.WindowForm != null)
                    {
                        if (!Framework.Instance.IsDialogShowing)
                            OnToggleMenuClicked(this, EventArgs.Empty);
                        //Framework.Instance.WindowForm.Close();
                    }
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


        void OnToggleMenuClicked(object sender, EventArgs e)
        {
            framework.ShowDialog(menuDialog);
        }


        #region Anaglyph stuff
        private enum EyeEnum
        {
            Left,
            Right,
            Both
        }

        private ShaderBase anaglyphShader = null;
        private Surface leftSurface = null;
        private Surface rightSurface = null;
        private Surface screen = null;
        private Texture leftTexture = null;
        private Texture rightTexture = null;

        public bool Anaglyph
        {
            get { return anaglyph; }
            set
            {
                anaglyph = value;
                if (anaglyph)
                {
                    if (anaglyphShader == null)
                    {
                        anaglyphShader = new ShaderBase("Anaglyph", "data/anaglyph.fx");
                        anaglyphShader.SetTechnique("ShaderModel2");
                    }
                    UpdateAnaglyphSurfaces();
                }
                else
                {
                    CleanupAnaglyphStuff();
                }
            }
        }

        private void CleanupAnaglyphStuff()
        {
            if (leftTexture != null)
            {
                leftSurface.Dispose();
                leftTexture.Dispose();
                leftTexture = null;
            }
            if (rightTexture != null)
            {
                rightSurface.Dispose();
                rightTexture.Dispose();
                rightTexture = null;
            }
            if (screen != null)
            {
                screen.Dispose();
                screen = null;
            }
            if (anaglyphShader != null)
            {
                anaglyphShader.Dispose();
                anaglyphShader = null;
            }
        }

        private void UpdateAnaglyphSurfaces()
        {
            if (leftTexture != null)
            {
                leftSurface.Dispose();
                leftTexture.Dispose();
                leftTexture = null;
            }
            if (rightTexture != null)
            {
                rightSurface.Dispose();
                rightTexture.Dispose();
                rightTexture = null;
            }
                        
            int width = framework.Device.PresentationParameters.BackBufferWidth;
            int height = framework.Device.PresentationParameters.BackBufferHeight;            
            leftTexture = new Texture(framework.Device, width, height, 1, Usage.RenderTarget, Format.A8R8G8B8, Pool.Default);
            rightTexture = new Texture(framework.Device, width, height, 1, Usage.RenderTarget, Format.A8R8G8B8, Pool.Default);
            leftSurface = leftTexture.GetSurfaceLevel(0);
            rightSurface = rightTexture.GetSurfaceLevel(0);
        }

        public void OnFrameRenderIntern(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            bool beginSceneCalled = false;

            // Clear the render target and the zbuffer 
            device.Clear(ClearFlags.ZBuffer | ClearFlags.Target, 0x00000000, 1.0f, 0);

            try
            {
                device.BeginScene();
                beginSceneCalled = true;

                Framework.Instance.Device.RenderState.Ambient = Color.FromArgb(148, 148, 148);

                scenery.OnFrameRender(device, totalTime, elapsedTime);
                weather.OnFrameRender(device, totalTime, elapsedTime);
                game.OnFrameRender(device, totalTime, elapsedTime);
                if (demo.Playing)
                {
                    demo.OnFrameRender(device, totalTime, elapsedTime);
                }
                else if (!welcomeDialog.Visible)
                {
                    player.OnFrameRender(device, totalTime, elapsedTime);
                    recorder.OnFrameRender(device, totalTime, elapsedTime);
                    computerPilots.OnFrameRender(device, totalTime, elapsedTime);
                }
                transparentObjectManager.OnFrameRender(device, totalTime, elapsedTime);
                scenery.OnFrameRenderFinal(device, totalTime, elapsedTime);
                // Show UI
                //hud.OnRender(elapsedTime);
                if (!demo.Playing)
                {
                    centerHud.OnFrameRender(device, totalTime, elapsedTime);
                    welcomeDialog.OnFrameRender(device, totalTime, elapsedTime);
                }
                //osd.OnFrameRender(device, totalTime, elapsedTime);
            }
            finally
            {
                if (beginSceneCalled)
                    device.EndScene();
            }
        }

        internal void OnFrameRenderWater(Device device, double totalTime, float elapsedTime, bool reflection)
        {
            device.Clear(ClearFlags.ZBuffer | ClearFlags.Target, Color.Blue, 1.0f, 0);
            device.Transform.World = Matrix.Identity;
            scenery.OnFrameRender(device, totalTime, elapsedTime);
            if (reflection)
            {
                /*
                // create matrices to render with
                Matrix camMatrix = Framework.Instance.CurrentCamera.ReflectionMatrix *
                    Framework.Instance.CurrentCamera.ProjectionMatrix;
                camMatrix.Invert();
                camMatrix.Transpose(camMatrix);                
                
                Plane reflectionClipPlane = new Plane(0f, 1f, 0f, 0.1f);
                reflectionClipPlane.Transform(camMatrix);
                */

                device.ClipPlanes[0].Plane = scenery.WaterReflectionClipPlane;
                device.ClipPlanes[0].Enabled = true;

                player.OnFrameRender(device, totalTime, elapsedTime);
                if (demo.Playing)
                    demo.OnFrameRender(device, totalTime, elapsedTime);

                device.ClipPlanes.DisableAll();
            }
            transparentObjectManager.OnFrameRender(device, totalTime, elapsedTime);
        }

        internal void OnFrameRenderReflection(Device device, double totalTime, float elapsedTime)
        {
            device.Clear(ClearFlags.ZBuffer | ClearFlags.Target, Color.Blue, 1.0f, 0);
            device.Transform.World = Matrix.Identity;
            scenery.OnFrameRender(device, totalTime, elapsedTime);
            transparentObjectManager.OnFrameRender(device, totalTime, elapsedTime);
        }

        /// <summary>
        /// Performs a straight-forward render-to-texture using whatever states the device has set.
        /// </summary>
        /// <param name="width">The width of the render-target.</param>
        /// <param name="height">The height of the render-target.</param>
        void RenderToTexture(Device device, int width, int height)
        {
            CustomVertex.TransformedTextured[] vertices = new CustomVertex.TransformedTextured[4];

            #region Vertex initialization
            // To correctly map from texels->pixels we offset the coordinates by -0.5
            vertices[0].Position = new Vector4(-0.5f, -0.5f, 0.0f, 1.0f);
            vertices[0].Tu = 0.0f; vertices[0].Tv = 0.0f;

            vertices[1].Position = new Vector4(width - 0.5f, -0.5f, 0.0f, 1.0f);
            vertices[1].Tu = 1.0f; vertices[1].Tv = 0.0f;

            vertices[2].Position = new Vector4(-0.5f, height - 0.5f, 0.0f, 1.0f);
            vertices[2].Tu = 0.0f; vertices[2].Tv = 1.0f;

            vertices[3].Position = new Vector4(width - 0.5f, height - 0.5f, 0.0f, 1.0f);
            vertices[3].Tu = 1.0f; vertices[3].Tv = 1.0f;

            #endregion

            device.VertexShader = null;
            device.VertexFormat = CustomVertex.TransformedTextured.Format;
            device.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, vertices);
        }
 
        private void OnFrameRender(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime, EyeEnum eye)
        {
            bool beginSceneCalled = false;
            
            if (eye == EyeEnum.Left)
            {
                device.Transform.World = Matrix.Identity;
                device.SetRenderTarget(0, leftSurface);
                framework.CurrentCamera.OnFrameRenderLeft(device, totalTime, elapsedTime);
                OnFrameRenderIntern(device, totalTime, elapsedTime);
                return;
            }
            else if (eye == EyeEnum.Right)
            {
                device.Transform.World = Matrix.Identity;
                device.SetRenderTarget(0, rightSurface);
                framework.CurrentCamera.OnFrameRenderRight(device, totalTime, elapsedTime);
                OnFrameRenderIntern(device, totalTime, elapsedTime);
                return;
            }
            // Clear the render target and the zbuffer 
            device.Clear(ClearFlags.ZBuffer | ClearFlags.Target, 0x00000000, 1.0f, 0);

            try
            {
                device.BeginScene();
                beginSceneCalled = true;

                Framework.Instance.Device.RenderState.Ambient = Color.FromArgb(148, 148, 148);

                /*
                if (anaglyphShader != null)
                {
                    anaglyphShader.SetGlobalParameters();
                    anaglyphShader.WorldMatrix = Matrix.Identity;
                    anaglyphShader.SetVariable("LeftTexture", leftTexture);
                    anaglyphShader.SetVariable("RightTexture", rightTexture);
                    int width = framework.Device.PresentationParameters.BackBufferWidth;
                    int height = framework.Device.PresentationParameters.BackBufferHeight;
                    anaglyphShader.SetVariable("TexelSize", new Vector3(1f/width, 1f/height, 1f));
                    int passes = anaglyphShader.Effect.Begin(0);
                    for (int iPass = 0; iPass < passes; iPass++)
                    {
                        anaglyphShader.Effect.BeginPass(iPass);

                        RenderToTexture(device, width, height);
                        anaglyphShader.Effect.EndPass();
                    }
                    anaglyphShader.Effect.End();
                }
                */
                device.StretchRectangle(leftSurface, new Rectangle(0, 0, leftSurface.Description.Width, leftSurface.Description.Height),
                    screen, new Rectangle(0, 0, screen.Description.Width / 2, screen.Description.Height), TextureFilter.None);
                device.StretchRectangle(rightSurface, new Rectangle(0, 0, rightSurface.Description.Width, rightSurface.Description.Height),
                    screen, new Rectangle(screen.Description.Width / 2, 0, screen.Description.Width / 2, screen.Description.Height), TextureFilter.None);
                

                //osd.OnFrameRender(device, totalTime, elapsedTime);
            }
            finally
            {
                if (beginSceneCalled)
                    device.EndScene();
            }
        }

        #endregion


        /// <summary>
        /// This event will be fired immediately after the Direct3D device has been 
        /// created, which will happen during application initialization and windowed/full screen 
        /// toggles. This is the best location to create Pool.Managed resources since these 
        /// resources need to be reloaded whenever the device is destroyed. Resources created  
        /// here should be released in the Disposing event. 
        /// </summary>
        private void OnCreateDevice(object sender, DeviceEventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            // Show the splashscreen
            Image splashBg = new Bitmap(this.GetType().Assembly.GetManifestResourceStream("RCSim.Resources.splash.png"));
            Bonsai.Utils.SplashScreen.StartSplashScreen(splashBg, new Icon("icon16.ico"),
                string.Concat("v", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()));
            //Bonsai.Utils.SplashScreen.SetSplashBackground(splashBg);
            Bonsai.Utils.SplashScreen.SetSplashStatus("Initializing...");
            // Initialize the stats font
            //statsFont = ResourceCache.GetGlobalInstance().CreateFont(e.Device, 15, 0, FontWeight.Bold, 1, false, CharacterSet.Default,
            //    Precision.Default, FontQuality.Default, PitchAndFamily.FamilyDoNotCare | PitchAndFamily.DefaultPitch
            //    , "Arial");   
            SoundManager.Initialize(Framework.Instance.Window);

            if (centerHud == null)
                centerHud = new CenterHud(this);

            transparentObjectManager = new TransparentObjectManager();

            Bonsai.Utils.SplashScreen.SetSplashStatus("Loading weather");
            weather = new Weather(this);

            if (anaglyph)
            {
                if (anaglyphShader == null)
                {
                    anaglyphShader = new ShaderBase("Anaglyph", "data/anaglyph.fx");
                    anaglyphShader.SetTechnique("ShaderModel2");
                }
            }

            sun = new DirectionalLight(new Vector3(0.5f, -0.707f, 0.5f));
            sun.Color = System.Drawing.Color.FromArgb(148, 148, 148);

            Bonsai.Utils.SplashScreen.SetSplashStatus("Loading scenery");
            scenery = new Scenery(this);
            scenery.LoadDefinition("data\\scenery\\default\\default.par");            
            map = scenery.SceneryFolder + "map_1.png";

            Bonsai.Utils.SplashScreen.SetSplashStatus("Loading aircraft");
            player = new Player(this);
            player.LoadModel(Player.CurrentModel);
            player.Heightmap = scenery.Heightmap;
            player.DefaultStartPosition = scenery.Parameters.DefaultStartPosition;
            player.WaterStartPosition = scenery.Parameters.WaterStartPosition;
            player.FlightModel.Paused = true;
            player.Airplane.KillEngine();
            player.Airplane.Visible = false;

            Bonsai.Utils.SplashScreen.SetSplashStatus("Loading cameras");
            observerCamera = new ObserverCamera("Pilot view", player.Airplane);
            observerCamera.LookFrom = PilotPosition;
            observerCamera.ZoomFactor = 1.5f;
            observerCamera.AspectRatio = (float)e.BackBufferDescription.Width / e.BackBufferDescription.Height;

            spotCamera = new SpotCamera("External view", player.Airplane);
            spotCamera.SpotDistance = 3f;
            spotCamera.FollowDistance = 5;
            spotCamera.AspectRatio = e.BackBufferDescription.Width / e.BackBufferDescription.Height;

            onBoardCamera = new OnBoardCamera("On board view", player.Airplane);
            onBoardCamera.AspectRatio = (float)e.BackBufferDescription.Width / e.BackBufferDescription.Height;

            cinematicCamera = new CinematicCamera("Cinematic view");
            cinematicCamera.AspectRatio = (float)e.BackBufferDescription.Width / e.BackBufferDescription.Height;
            cinematicCamera.Airplane = player.Airplane;
            cinematicCamera.Scenery = scenery;

            baseCamera = new CameraBase("base");

            framework.CurrentCamera = observerCamera;
            
            //osd = new Hud();
            //osd.ShowFPS = true;
            //osd.Lines.Add("Model position:");
            //osd.Lines.Add("Terrainheight:");
            //osd.Lines.Add("collision?");

            recorder = new FlightRecorder(this);
            recorder.FileName = "recording.dat";
            
            demo = new Demo(this);
            demo.Stopped += new EventHandler(demo_Stopped);
            //demo.Play();

            computerPilots = new ComputerPilots(this);
                       
            Bonsai.Utils.SplashScreen.SetSplashStatus("Initializing input");
            inputManager = new Bonsai.Input.InputManager(Framework.Instance.Window);
            
            inputManager.SetDefaultAxis("throttle", Bonsai.Input.InputManager.JoyStickAxis.Y, true);
            inputManager.SetDefaultAxis("rudder", Bonsai.Input.InputManager.JoyStickAxis.X, false);
            inputManager.SetDefaultAxis("elevator", Bonsai.Input.InputManager.JoyStickAxis.Slider1, false);
            inputManager.SetDefaultAxis("aileron", Bonsai.Input.InputManager.JoyStickAxis.Rz, false);
            /*
            inputManager.SetDefaultAxis("elevator", Bonsai.Input.InputManager.JoyStickAxis.Y, true);
            inputManager.SetDefaultAxis("aileron", Bonsai.Input.InputManager.JoyStickAxis.X, true);
            inputManager.SetDefaultAxis("throttle", Bonsai.Input.InputManager.JoyStickAxis.Z, false);
            inputManager.SetDefaultAxis("rudder", Bonsai.Input.InputManager.JoyStickAxis.Rz, false);
            */

            Bonsai.Utils.SplashScreen.SetSplashStatus("Initializing menus");
            // Create the menus
            menuDialog = new MenuDialog(framework, this);

            welcomeDialog = new WelcomeDialog(this);

            // Create the hud
            centerHud.Initialize();
            welcomeDialog.Initialize();
            welcomeDialog.StartClicked += new EventHandler(welcomeDialog_StartClicked);
            welcomeDialog.DemoClicked += new EventHandler(welcomeDialog_DemoClicked);
            // TODO: remove after BMI demo.
            //welcomeDialog.Visible = false;
            //demo.Play();
                        
            game = new Game(this);
            game.CurrentGameType = Game.GameType.None;

            if (adManager == null)
            {
                adManager = new AdManager(scenery, this);
            }
            
            Bonsai.Utils.SplashScreen.SetSplashStatus("Almost done");
            // Hide the splashscreen
            Bonsai.Utils.SplashScreen.HideSplashScreen();
            Bonsai.Core.Framework.Instance.WindowForm.BringToFront();
            setFullScreen = Convert.ToBoolean(Bonsai.Utils.Settings.GetValue("FullScreen", "false"));
            //int resX = Convert.ToInt32(Bonsai.Utils.Settings.GetValue("ResolutionWidth", Framework.DefaultSizeWidth.ToString()));
            //int resY = Convert.ToInt32(Bonsai.Utils.Settings.GetValue("ResolutionHeight", Framework.DefaultSizeHeight.ToString()));       
        }

        void demo_Stopped(object sender, EventArgs e)
        {
            SetCameraTarget(null);
            framework.CurrentCamera = observerCamera;
            welcomeDialog.Visible = true;
        }

        void welcomeDialog_DemoClicked(object sender, EventArgs e)
        {
            welcomeDialog.Visible = false;
            demo.Play();
            SetCameraTarget(demo.CameraTarget);
            framework.CurrentCamera = cinematicCamera;
        }

        void welcomeDialog_StartClicked(object sender, EventArgs e)
        {
            welcomeDialog.Visible = false;
            SetCameraTarget(player.Airplane);
            player.FlightModel.Paused = false;
            player.Airplane.StartEngine();
            player.Airplane.Visible = true;
            player.Reset();
            framework.CurrentCamera = observerCamera;
            SetWaterCamera(Player.TakeOffFromWater);
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

            observerCamera.AspectRatio = (float)e.BackBufferDescription.Width / e.BackBufferDescription.Height;
            spotCamera.AspectRatio = (float)e.BackBufferDescription.Width / e.BackBufferDescription.Height;
            onBoardCamera.AspectRatio = (float)e.BackBufferDescription.Width / e.BackBufferDescription.Height;
            cinematicCamera.AspectRatio = (float)e.BackBufferDescription.Width / e.BackBufferDescription.Height;
            //System.Console.Out.WriteLine("Aspect = " + observerCamera.AspectRatio);

            // Setup UI locations
            hud.SetLocation(desc.Width - 170, 0);
            hud.SetSize(170, 170);
            
            centerHud.SetSize(desc.Width, desc.Height);
            welcomeDialog.SetSize(desc.Width, desc.Height);
            if (anaglyph)
            {
                UpdateAnaglyphSurfaces();
            }
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
            if (leftTexture != null)
            {
                leftSurface.Dispose();
                leftTexture.Dispose();
                leftTexture = null;
            }
            if (rightTexture != null)
            {
                rightSurface.Dispose();
                rightTexture.Dispose();
                rightTexture = null;
            }
            if (screen != null)
            {
                screen.Dispose();
                screen = null;
            }
        }

        /// <summary>
        /// This callback function will be called immediately after the Direct3D device has 
        /// been destroyed, which generally happens as a result of application termination or 
        /// windowed/full screen toggles. Resources created in the OnCreateDevice callback 
        /// should be released here, which generally includes all Pool.Managed resources. 
        /// </summary>
        private void OnDestroyDevice(object sender, EventArgs e)
        {   
            if (inputManager != null)
            {
                inputManager.Dispose();
                inputManager = null;
            }

            if (game != null)
            {
                game.Dispose();
                game = null;
            }

            if (computerPilots != null)
            {
                computerPilots.Dispose();
                computerPilots = null;
            }

            if (demo != null)
            {
                demo.Dispose();
                demo = null;
            }

            if (osd != null)
            {
                osd.Dispose();
                osd = null;
            }

            if (player != null)
            {
                player.Dispose();
                player = null;
            }

            if (scenery != null)
            {
                scenery.Dispose();
                scenery = null;
            }

            if (sun != null)
            {
                sun.Dispose();
                sun = null;
            }

            if (weather != null)
            {
                weather.Dispose();
                weather = null;
            }


            if (transparentObjectManager != null)
            {
                transparentObjectManager = null;
            }

            if (centerHud != null)
            {
                centerHud.Dispose();
                centerHud = null;
            }

            // Clear all textures to force a reload.
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
            if ((welcomeDialog != null) && (welcomeDialog.Dialog != null) && welcomeDialog.Visible)
                noFurtherProcessing |= welcomeDialog.Dialog.MessageProc(hWnd, msg, wParam, lParam); 
            else if ((centerHud != null) && (centerHud.Dialog != null))
                noFurtherProcessing |= centerHud.Dialog.MessageProc(hWnd, msg, wParam, lParam);
            noFurtherProcessing |= hud.MessageProc(hWnd, msg, wParam, lParam);
            if (noFurtherProcessing)
                return IntPtr.Zero;

            return IntPtr.Zero;
        }
        #endregion

        #region IFrameworkCallback Members

        public void OnFrameMove(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            currentTime = totalTime;

            if (setFullScreen)
            {
                //framework.ToggleFullscreen();
                RestoreResolution();
                setFullScreen = false;
            }

            inputManager.Update();
            
            scenery.OnFrameMove(device, totalTime, elapsedTime);
            weather.OnFrameMove(device, totalTime, elapsedTime);
            game.OnFrameMove(device, totalTime, elapsedTime);
            if (demo.Playing)
            {
                demo.OnFrameMove(device, totalTime, elapsedTime);
            }
            else 
            {
                player.OnFrameMove(device, totalTime, elapsedTime);
                recorder.OnFrameMove(device, totalTime, elapsedTime);
                computerPilots.OnFrameMove(device, totalTime, elapsedTime);
            }
            transparentObjectManager.OnFrameMove(device, totalTime, elapsedTime);
            
            /*
            osd.OnFrameMove(device, totalTime, elapsedTime);
            osd.Lines[0] = ("Model position:" + player.Position.X.ToString("F") + "," + player.Position.Y.ToString("F") + "," + player.Position.Z.ToString("F"));
            //osd.Lines[1] = ("Terrainheight:" + scenery.Heightmap.GetHeightAt(player.Position.X, player.Position.Z).ToString("F"));
            osd.Lines[1] = ("A:" + player.FlightModel.Alpha.ToString("N") + "B:" + player.FlightModel.Beta.ToString("N") + 
                "AAx:" + player.FlightModel.AAx.ToString("N") + "AAy:" + player.FlightModel.AAy.ToString("N") + "AAz:" + player.FlightModel.AAz.ToString("N"));
            osd.Lines[2] = framework.DebugString;
             */
            if (!demo.Playing)
            {
                centerHud.OnFrameMove(device, totalTime, elapsedTime);
                welcomeDialog.OnFrameMove(device, totalTime, elapsedTime);
            }
        }

        public void OnFrameRender(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            //currentTime = totalTime;

            if (scenery != null)
            {
                scenery.RenderTextures(device, totalTime, elapsedTime);
            }

            if (anaglyph)
            {
                if (screen == null)
                    screen = device.GetRenderTarget(0);
                OnFrameRender(device, totalTime, elapsedTime, EyeEnum.Left);
                OnFrameRender(device, totalTime, elapsedTime, EyeEnum.Right);
                device.SetRenderTarget(0, screen);
                OnFrameRender(device, totalTime, elapsedTime, EyeEnum.Both);
                return;
            }

            if ((Player != null) && (Player.Reflection != null))
            {
                if (scenery.Parameters.SceneryType == SceneryParameters.SceneryTypeEnum.Photofield)
                    Player.Reflection.UpdateCubeMap(device, totalTime, elapsedTime, new Vector3(0, 0, 0));
                else if (Effects.Reflection.ReflectionDetail == RCSim.Effects.Reflection.ReflectionDetailEnum.High)
                    Player.Reflection.UpdateCubeMap(device, totalTime, elapsedTime, Player.Airplane.Position);
                else
                    Player.Reflection.UpdateCubeMap(device, totalTime, elapsedTime, new Vector3(0, 20, 0));
            }

            bool beginSceneCalled = false;

            // Clear the render target and the zbuffer 
            device.Clear(ClearFlags.ZBuffer | ClearFlags.Target, 0x00000000, 1.0f, 0);
            
            try
            {
                device.BeginScene();
                beginSceneCalled = true;

                Framework.Instance.Device.RenderState.Ambient = ambientLightColor;

                scenery.OnFrameRender(device, totalTime, elapsedTime);
                weather.OnFrameRender(device, totalTime, elapsedTime);
                game.OnFrameRender(device, totalTime, elapsedTime);
                if (demo.Playing)
                {
                    demo.OnFrameRender(device, totalTime, elapsedTime);
                }
                else if (!welcomeDialog.Visible)
                {
                    player.OnFrameRender(device, totalTime, elapsedTime);
                    recorder.OnFrameRender(device, totalTime, elapsedTime);
                    computerPilots.OnFrameRender(device, totalTime, elapsedTime);
                }
                
                transparentObjectManager.OnFrameRender(device, totalTime, elapsedTime);
                scenery.OnFrameRenderFinal(device, totalTime, elapsedTime);
                // Show UI
                //hud.OnRender(elapsedTime);
                centerHud.OnFrameRender(device, totalTime, elapsedTime);
                if (!demo.Playing)
                {                    
                    welcomeDialog.OnFrameRender(device, totalTime, elapsedTime);
                }
                //osd.OnFrameRender(device, totalTime, elapsedTime);
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
                //Bonsai.Utils.Settings.SetValue("SceneryDetail", "1");
                //settings.BehaviorFlags = CreateFlags.HardwareVertexProcessing;
                //Utility.DisplaySwitchingToRefWarning(framework, "R/C Desk Pilot", "No recent video adapter was found, the sim can attempt to run in low detail mode. Do you want to continue?");
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
                Utility.DisplaySwitchingToRefWarning(framework, "R/C Desk Pilot");
            }
        }
        #endregion
    }
}
