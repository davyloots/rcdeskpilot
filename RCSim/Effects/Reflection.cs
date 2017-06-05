using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Objects.Textures;
using Bonsai.Objects.Cameras;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Bonsai.Core;
using Bonsai.Utils;

namespace RCSim.Effects
{
    internal class Reflection : IDisposable
    {
        #region Public enums
        public enum ReflectionDetailEnum
        {
            Off,
            Low,
            High
        }
        #endregion

        #region Protected fields
        protected CameraBase cubeCamera;
        protected OnFrameRenderDelegate onFrameRender = null;
        protected TextureBase cubeTexture = null;
        protected RenderToSurface renderHelper = null;
        protected bool rendering = false;
        protected bool invalidated = true;
        #endregion

        #region Public delegate
        public delegate void OnFrameRenderDelegate(Device device, double totalTime, float elapsedTime);
        #endregion

        #region Public properties
        /// <summary>
        /// Gets/sets the render method
        /// </summary>
        public OnFrameRenderDelegate OnFrameRenderMethod
        {
            get { return onFrameRender; }
            set { onFrameRender = value; }
        }

        /// <summary>
        /// Gets a reference to the cubemap.
        /// </summary>
        public TextureBase CubeMap
        {
            get { return cubeTexture; }
        }

        /// <summary>
        /// Gets whether the Reflection cubemap is currently being rendered.
        /// </summary>
        public bool Rendering
        {
            get { return rendering; }
        }
        #endregion

        #region Static properties
        protected static Reflection instance = null;
        protected static ReflectionDetailEnum reflectionDetail = ReflectionDetailEnum.Off;

        /// <summary>
        /// Get a reference to the running program.
        /// </summary>
        public static Reflection Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// Gets/sets whether reflections are enabled.
        /// </summary>
        public static bool Enabled
        {
            get { return reflectionDetail != ReflectionDetailEnum.Off; }
        }

        public static ReflectionDetailEnum ReflectionDetail
        {
            get { return reflectionDetail; }
            set 
            {
                if (reflectionDetail != value)
                {
                    reflectionDetail = value;
                    if (Instance != null)
                    {
                        if (Enabled)
                            Instance.Initialize();
                        else
                            Instance.CleanUp();
                    }
                    if (Program.Instance != null)
                        Program.Instance.ReloadPlayer();
                }
            }
        }
        #endregion

        #region Constructor
        public Reflection()
        {           
            instance = this;

            cubeCamera = new CameraBase("cubecamera");
            cubeCamera.AspectRatio = 1;
            cubeCamera.FieldOfView = (float)Math.PI / 2;
            cubeCamera.Up = new Vector3(0, 1f, 0);
            cubeCamera.Near = 0.1f;

            int detailInt = Convert.ToInt32(Settings.GetValue("ReflectionDetail", "2"));
            switch (detailInt)
            {
                case 0:
                    reflectionDetail = ReflectionDetailEnum.Off;
                    break;
                case 1:
                    reflectionDetail = ReflectionDetailEnum.Low;
                    break;
                case 2:
                    reflectionDetail = ReflectionDetailEnum.High;
                    break;
            }
            Initialize();
 
            Framework.Instance.DeviceReset += new Bonsai.Core.EventArgs.DeviceEventHandler(Instance_DeviceReset);
            Framework.Instance.DeviceCreated += new Bonsai.Core.EventArgs.DeviceEventHandler(Instance_DeviceCreated);
            Framework.Instance.DeviceLost += new EventHandler(Instance_DeviceLost);
            
            this.OnFrameRenderMethod = Program.Instance.OnFrameRenderReflection;
        }

        
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Clean up.
        /// </summary>
        public void Dispose()
        {
            Framework.Instance.DeviceReset -= new Bonsai.Core.EventArgs.DeviceEventHandler(Instance_DeviceReset);
            Framework.Instance.DeviceCreated -= new Bonsai.Core.EventArgs.DeviceEventHandler(Instance_DeviceCreated);
            Framework.Instance.DeviceLost -= new EventHandler(Instance_DeviceLost);
            CleanUp();
            instance = null;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Renders the CubeMap from the given position.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="totalTime"></param>
        /// <param name="elapsedTime"></param>
        /// <param name="position"></param>
        public void UpdateCubeMap(Device device, double totalTime, float elapsedTime, Vector3 position)
        {
            if (Enabled)
                RenderSceneIntoCubeMap(device, totalTime, elapsedTime, position);
        }

        /// <summary>
        /// Invalidates the reflection CubeMap to re-render in next time.
        /// </summary>
        public void Invalidate()
        {
            invalidated = true;
        }
        #endregion

        #region Private event handlers
        void Instance_DeviceLost(object sender, EventArgs e)
        {
            CleanUp();
        }

        void Instance_DeviceCreated(object sender, Bonsai.Core.EventArgs.DeviceEventArgs e)
        {
            Initialize();
        }

        void Instance_DeviceReset(object sender, Bonsai.Core.EventArgs.DeviceEventArgs e)
        {
            CleanUp();
            Initialize();
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Clean up resources.
        /// </summary>
        private void CleanUp()
        {
            if (renderHelper != null)
            {
                renderHelper.Dispose();
                renderHelper = null;
            }
            if (cubeTexture != null)
            {
                cubeTexture.Dispose();
                cubeTexture = null;
            }
        }

        /// <summary>
        /// Creates resources.
        /// </summary>
        private void Initialize()
        {
            if (Enabled)
            {
                if (cubeTexture == null)
                {
                    cubeTexture = new TextureBase(256, true);
                }
                if (renderHelper == null)
                {
                    renderHelper = new RenderToSurface(Framework.Instance.Device, 256, 256,
                        Framework.Instance.Device.PresentationParameters.BackBufferFormat, true,
                        Framework.Instance.Device.PresentationParameters.AutoDepthStencilFormat);
                }
                Invalidate();
            }
        }
       
        /// <summary>Set up the cube map by rendering the scene into it</summary>
        private void RenderSceneIntoCubeMap(Device device, double totalTime, float elapsedTime, Vector3 position)
        {
            if (invalidated)
            {
                rendering = true;
                CameraBase cam = Framework.Instance.CurrentCamera;
                cubeCamera.LookFrom = position;
                Framework.Instance.CurrentCamera = cubeCamera;
                for (int f = 0; f < 6; ++f)
                {
                    CubeTexture cubeTex = cubeTexture.Texture as CubeTexture;
                    using (Surface s = cubeTex.GetCubeMapSurface((CubeMapFace)f, 0))
                    {
                        SetCubeMapCamera((CubeMapFace)f);

                        renderHelper.BeginScene(s);
                        cubeCamera.OnFrameMove(device, totalTime, elapsedTime);
                        cubeCamera.OnFrameRender(device, totalTime, elapsedTime);
                        if (OnFrameRenderMethod != null)
                            OnFrameRenderMethod(device, totalTime, elapsedTime);
                        device.Transform.World = Matrix.Identity;
                        renderHelper.EndScene(Filter.None);
                    }
                }
                Framework.Instance.CurrentCamera = cam;
                Framework.Instance.CurrentCamera.OnFrameRender(device, totalTime, elapsedTime);
                rendering = false;
                invalidated = false;
            }
        }

        /// <summary>Returns the view matrix for a cube map face</summary>
        private void SetCubeMapCamera(CubeMapFace face)
        {            
            switch (face)
            {
                case CubeMapFace.PositiveX:
                    cubeCamera.LookAt = cubeCamera.LookFrom + new Vector3(1f, 0, 0);
                    cubeCamera.Up = new Vector3(0, 1f, 0);
                    break;
                case CubeMapFace.NegativeX:
                    cubeCamera.LookAt = cubeCamera.LookFrom + new Vector3(-1f, 0, 0);
                    cubeCamera.Up = new Vector3(0, 1f, 0);
                    break;
                case CubeMapFace.PositiveY:
                    cubeCamera.LookAt = cubeCamera.LookFrom + new Vector3(0, 1f, 0);
                    cubeCamera.Up = new Vector3(0, 0, -1f);
                    break;
                case CubeMapFace.NegativeY:
                    cubeCamera.LookAt = cubeCamera.LookFrom + new Vector3(0, -1f, 0);
                    cubeCamera.Up = new Vector3(0, 0, 1f);
                    break;
                case CubeMapFace.PositiveZ:
                    cubeCamera.LookAt = cubeCamera.LookFrom + new Vector3(0, 0, 1f);
                    cubeCamera.Up = new Vector3(0, 1f, 0);
                    break;
                case CubeMapFace.NegativeZ:
                    cubeCamera.LookAt = cubeCamera.LookFrom + new Vector3(0, 0, -1f);
                    cubeCamera.Up = new Vector3(0, 1f, 0);
                    break;
            }
        }
        #endregion
    }
}
