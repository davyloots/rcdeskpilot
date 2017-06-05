using System;
using System.Collections.Generic;
using System.Text;

using Bonsai.Objects.Shaders;
using Microsoft.DirectX;
using Bonsai.Objects.Textures;
using Bonsai.Core;
using System.Drawing;
using Microsoft.DirectX.Direct3D;
using Bonsai.Objects.Meshes;
using Bonsai.Core.Interfaces;

namespace Bonsai.Objects.Terrain
{
    public class Water : GameObject, IDisposable
    {
        #region Public enums
        /// <summary>
        /// Enumeration for the level of quality in the water rendering.
        /// </summary>
        public enum QualityLevelEnum
        {
            Low,
            Medium,
            High
        }
        #endregion

        #region Protected fields
        protected ShaderBase waterShader;
        protected RenderToSurface renderHelper = null;
        protected Texture refractionTexture = null;
        protected Surface refractionSurface = null;
        protected Plane refractionClipPlane;
        protected Texture reflectionTexture = null;
        protected Surface reflectionSurface = null;
        protected Plane reflectionClipPlane;
        protected TextureBase waterNormalTexture = null;
        protected SquareMesh squareMesh = null;
        protected OnFrameRenderDelegate onFrameRender = null;
        protected bool rendering = false;
        protected float reflectionRefractionRatio = 1.0f;
        protected int fresnelMode = 0;
        protected int fresnelCount = 3;
        protected int textureSize = 512;
        protected QualityLevelEnum qualityLevel = QualityLevelEnum.Medium;
        #endregion

        #region Public delegate
        public delegate void OnFrameRenderDelegate(Device device, double totalTime, float elapsedTime, bool reflection);
        #endregion

        #region Public properties
        /// <summary>
        /// Gets/sets the terrain heighmap.
        /// </summary>
        public Heightmap Heightmap
        {
            get;
            set;
        }

        /// <summary>
        /// Sets the position of the sun.
        /// </summary>
        public Vector3 SunPosition
        {
            set
            {
                if (waterShader != null)
                    waterShader.SetVariable("SunPosition", new Vector3(value.X, value.Z, value.Y));
            }
        }

        /// <summary>
        /// Sets the direction of the wind.
        /// </summary>
        public float WindDirection
        {
            set
            {
                if (waterShader != null)
                    waterShader.SetVariable("xWindDirection", Matrix.RotationZ(value - (float)Math.PI/2));
            }
        }

        /// <summary>
        /// Sets the direction of the wind.
        /// </summary>
        public float WindSpeed
        {
            set
            {
                if (waterShader != null)
                {
                    waterShader.SetVariable("WaveLength", 0.1f + value/100f);
                    waterShader.SetVariable("xWindForce", 20.0f + 2*value);
                    waterShader.SetVariable("WaveHeight", 0.2f + value/50f);                    
                }
                    
            }
        }

        public OnFrameRenderDelegate OnFrameRenderMethod
        {
            get { return onFrameRender; }
            set { onFrameRender = value; }
        }

        public bool Rendering
        {
            get { return rendering; }
        }

        /// <summary>
        /// Gets the level of quality of the water rendering.
        /// </summary>
        public QualityLevelEnum QualityLevel
        {
            get { return qualityLevel; }
        }
        #endregion

        #region Constructor
        public Water(Vector3 position, float size, QualityLevelEnum quality)
        {
            this.qualityLevel = quality;
            squareMesh = new SquareMesh(size, 1, 1, 1.0f);
            this.Mesh = squareMesh;
            this.RotateXAngle = (float)Math.PI / 2;
            this.Position = position;

            if (quality != QualityLevelEnum.Low)
            {
                Vector3 point1 = new Vector3(-1, 0f, 1);
                Vector3 point2 = new Vector3(1, 0f, 1);
                Vector3 point3 = new Vector3(-1, 0f, -1);
                Plane plane = Plane.FromPoints(point1, point2, point3);

                // reflection
                //reflectionTexture = new TextureBase(256, 256);
                reflectionClipPlane = plane;

                // refraction
                plane = Plane.FromPoints(point1, point2, point3);
                plane.C *= -1;
                refractionClipPlane = plane;

                waterNormalTexture = new TextureBase("waterbump.dds");

                // initialize shader
                waterShader = new ShaderBase("water", "watereffects.fx");
                waterShader.SetVariable("xWorld", ShaderBase.ShaderParameters.World);
                waterShader.SetVariable("xView", ShaderBase.ShaderParameters.View);
                waterShader.SetVariable("xReflectionView", ShaderBase.ShaderParameters.Reflection);
                waterShader.SetVariable("xProjection", ShaderBase.ShaderParameters.Projection);
                //waterShader.SetVariable("xReflectionMap", reflectionTexture);
                //waterShader.SetVariable("xRefractionMap", refractionTexture);
                waterShader.SetVariable("xDrawMode", reflectionRefractionRatio);
                waterShader.SetVariable("fresnelMode", fresnelMode);
                waterShader.SetVariable("xdullBlendFactor", 0.2f);
                waterShader.SetVariable("xEnableTextureBlending", 0);
                waterShader.SetVariable("xWaterBumpMap", waterNormalTexture);
                waterShader.SetVariable("xWaterPos", new Vector3(position.X, position.Z, position.Y));

                // parameteres for the wave
                waterShader.SetVariable("WaveLength", 0.1f);
                waterShader.SetVariable("WaveHeight", 0.4f);
                waterShader.SetVariable("SunPosition", new Vector3(1.0f, 1.0f, 1.0f));
                //waterShader.SetVariable("xCamPos", Framework.Instance.CurrentCamera.LookFrom);

                //waterShader.SetVariable("xTime", totalTime);
                waterShader.SetVariable("xWindForce", 20.0f);
                waterShader.SetVariable("xWindDirection", Matrix.RotationY(1.0f));

                //specular reflection parameters
                waterShader.SetVariable("specPower", 364);
                waterShader.SetVariable("specPerturb", 4);

                waterShader.SetTechnique("Water");

                CreateShaderResources(Framework.Instance.Device);

                this.Shader = waterShader;
            }
            else // low quality
            {
                squareMesh.Texture = new TextureBase("water.jpg");
            }

            Framework.Instance.DeviceReset += new Bonsai.Core.EventArgs.DeviceEventHandler(Instance_DeviceReset);
            Framework.Instance.DeviceLost += new EventHandler(Instance_DeviceLost);
        }

        void Instance_DeviceLost(object sender, EventArgs e)
        {
            DeleteShaderResources();
        }

        void Instance_DeviceReset(object sender, Bonsai.Core.EventArgs.DeviceEventArgs e)
        {
            DeleteShaderResources();

            CreateShaderResources(e.Device);
        }
        #endregion

        #region IDisposable Members
        void IDisposable.Dispose()
        {
            DeleteShaderResources();
            if (waterShader != null)
            {
                waterShader.Dispose();
                waterShader = null;
            }

            if (waterNormalTexture != null)
            {
                waterNormalTexture.Dispose();
                waterNormalTexture = null;
            }

            if (squareMesh.Texture != null)
            {
                squareMesh.Texture.Dispose();
                squareMesh.Texture = null;
            }
            base.Dispose();
        }
        #endregion

        #region Protected methods
        protected void CreateShaderResources(Device device)
        {
            if (qualityLevel != QualityLevelEnum.Low)
            {
                int textureSize = 512; // high quality
                if (qualityLevel == QualityLevelEnum.Medium)
                    textureSize = 256; // medium quality
                if (renderHelper == null)
                    renderHelper = new RenderToSurface(device, textureSize, textureSize, device.PresentationParameters.BackBufferFormat, true, 
                        device.PresentationParameters.AutoDepthStencilFormat);
                if (reflectionTexture == null)
                    reflectionTexture = new Texture(device, textureSize, textureSize, 1, Usage.RenderTarget, device.PresentationParameters.BackBufferFormat, Pool.Default);
                if (reflectionSurface == null)
                    reflectionSurface = reflectionTexture.GetSurfaceLevel(0);
                if (refractionTexture == null)
                    refractionTexture = new Texture(device, textureSize, textureSize, 1, Usage.RenderTarget, device.PresentationParameters.BackBufferFormat, Pool.Default);
                if (refractionSurface == null)
                    refractionSurface = refractionTexture.GetSurfaceLevel(0);
            }
        }

        protected void DeleteShaderResources()
        {
            if (reflectionTexture != null)
            {
                reflectionTexture.Dispose();
                reflectionTexture = null;
            }
            if (reflectionSurface != null)
            {
                reflectionSurface.Dispose();
                reflectionSurface = null;
            }
            if (refractionTexture != null)
            {
                refractionTexture.Dispose();
                refractionTexture = null;
            }
            if (refractionSurface != null)
            {
                refractionSurface.Dispose();
                refractionSurface = null;
            }
            if (renderHelper != null)
            {
                renderHelper.Dispose();
                renderHelper = null;
            }
        }
        #endregion

        #region Public method
        public void RenderTextures(Device device, double totalTime, float elapsedTime)
        {
            if (qualityLevel != QualityLevelEnum.Low)
            {
                rendering = true;
                if (reflectionSurface == null)
                    reflectionSurface = reflectionTexture.GetSurfaceLevel(0);
                if (refractionSurface == null)
                    refractionSurface = refractionTexture.GetSurfaceLevel(0);
                renderHelper.BeginScene(reflectionSurface);
                //Surface screen = device.GetRenderTarget(0);
                //Surface reflectionSurface = reflectionTexture.GetSurfaceLevel(0);
                device.Clear(ClearFlags.ZBuffer | ClearFlags.Target, 0x00000000, 1.0f, 0);
                Framework.Instance.CurrentCamera.Reflected = true;
                
                /*
                // create matrices to render with
                Matrix camMatrix = Framework.Instance.CurrentCamera.ReflectionMatrix *
                    Framework.Instance.CurrentCamera.ProjectionMatrix;
                camMatrix.Invert();
                camMatrix.Transpose(camMatrix);                

                //Plane reflectionClipPlane = Plane.FromPointNormal(Vector3.Empty, planeNormalDirection);
                Plane reflectionClipPlane = new Plane(0f, 1f, 0f, 0.1f);
                reflectionClipPlane.Transform(camMatrix);

                device.ClipPlanes[0].Plane = reflectionClipPlane;
                device.ClipPlanes[0].Enabled = true;
                */
                device.Transform.View = Framework.Instance.CurrentCamera.ReflectionMatrix;
                if (onFrameRender != null)
                    onFrameRender(device, totalTime, elapsedTime, true);
                Framework.Instance.CurrentCamera.Reflected = false;

                waterShader.SetVariable("xReflectionMap", reflectionTexture);
                
                //device.ClipPlanes.DisableAll();

                device.Transform.World = Matrix.Identity;
                device.Transform.View = Framework.Instance.CurrentCamera.ViewMatrix;
                renderHelper.EndScene(Filter.None);


                renderHelper.BeginScene(refractionSurface);
                device.Clear(ClearFlags.ZBuffer | ClearFlags.Target, 0x00000000, 1.0f, 0);

                //device.Transform.View = Framework.Instance.CurrentCamera.ViewMatrix;
                if (onFrameRender != null)
                    onFrameRender(device, totalTime, elapsedTime, false);
                waterShader.SetVariable("xRefractionMap", refractionTexture);
                //device.ClipPlanes.DisableAll();
                //device.Transform.World = Matrix.Identity;
                //device.Transform.View = Framework.Instance.CurrentCamera.ViewMatrix;
                renderHelper.EndScene(Filter.None);

                if (reflectionSurface != null)
                {
                    reflectionSurface.Dispose();
                    reflectionSurface = null;
                }
                if (refractionSurface != null)
                {
                    refractionSurface.Dispose();
                    refractionSurface = null;
                }

                rendering = false;
            }
        }
        #endregion

        #region IFrameworkCallback Members
        public override void OnFrameMove(Device device, double totalTime, float elapsedTime)
        {
            if (waterShader != null)
            {
                waterShader.SetVariable("xCamPos", new Vector3(Framework.Instance.CurrentCamera.LookFrom.X, Framework.Instance.CurrentCamera.LookFrom.Z, Framework.Instance.CurrentCamera.LookFrom.Y));
                waterShader.SetVariable("xTime", (float)totalTime / 1000f);
            }
            base.OnFrameMove(device, totalTime, elapsedTime);
        }

        public override void OnFrameRender(Device device, double totalTime, float elapsedTime)
        {     
            if (!rendering)
                base.OnFrameRender(device, totalTime, elapsedTime);            
        }
        #endregion        
  

    }
}
