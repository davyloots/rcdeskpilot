using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Objects;
using Bonsai.Objects.Meshes;
using Bonsai.Objects.Shaders;
using Bonsai.Objects.Textures;
using Microsoft.DirectX;
using System.Drawing;
using Bonsai.Core;

namespace RCSim
{
    internal class Flag : GameObject, IDisposable
    {
        #region private fields
        private GameObject pole = null;
        private GameObject flag = null;
        private static XMesh meshPole = null;
        private static XMesh meshFlag = null;
        private static ShaderBase flagShader = null;
        private static int count = 0;
        private static string oldTexture = null;
        private static string newTexture = null;
        private float flagTime = 0;
        #endregion

        #region Constructor
        public Flag(Vector3 position)
        {
            count++;
            pole = new GameObject();
            flag = new GameObject();
            if (meshPole == null)
            {
                meshPole = new XMesh("data/flagpole.x");
            }
            if (meshFlag == null)
            {
                meshFlag = new XMesh("data/flag.x");
                flagShader = new ShaderBase("FlagShader", "data/flag.fx");
                flagShader.SetVariable("matWorldViewProj", ShaderBase.ShaderParameters.WorldProjection);
                flagShader.SetVariable("matWorld", ShaderBase.ShaderParameters.World);
                flagShader.SetVariable("vecLightDir", new Vector3(-0.3529871f, 0.8714578f, -0.3405313f));                
                flagShader.SetTechnique("TFlag");
            }
            //this.Position = new Microsoft.DirectX.Vector3((count % 2 == 0)?5f:-5f, 0, (count % 2 == 0)?50f:20f);            
            this.Position = position;
            pole.Mesh = meshPole;
            pole.Scale = new Vector3(2, 2, 2);
            flag.Mesh = meshFlag;
            flag.Position = new Vector3(0, 2.3f, 0);            
            flag.Shader = flagShader;
            this.AddChild(pole);
            this.AddChild(flag);
            this.Scale = new Microsoft.DirectX.Vector3(2f, 2f, 2f);
            if (!string.IsNullOrEmpty(oldTexture) && !string.IsNullOrEmpty(newTexture))
            {
                meshFlag.ReplaceTexture(oldTexture, newTexture, true);
                flagShader.SetVariable("Texture", meshFlag.GetTexture(0));
            }
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Clean up.
        /// </summary>
        public override void Dispose()
        {
            count--;
            if (count == 0)
            {
                meshPole.Dispose();
                meshPole = null;
                meshFlag.Dispose();
                meshFlag = null;
                flagShader.Dispose();
                flagShader = null;
            }
            pole.Dispose();
            flag.Dispose();
        }
        #endregion

        #region Overridden GameObject methods
        public override void OnFrameMove(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            /*
            Bonsai.Objects.Cameras.ObserverCamera camera = Framework.Instance.CurrentCamera as Bonsai.Objects.Cameras.ObserverCamera;
            if (camera != null)
                camera.TargetObject = flag;
            Bonsai.Objects.Cameras.SpotCamera spotCamera = Framework.Instance.CurrentCamera as Bonsai.Objects.Cameras.SpotCamera;
            if (spotCamera != null)
                spotCamera.TargetObject = flag;
            */
            //flagShader.SetVariable("matWorldViewProj", ShaderBase.ShaderParameters.WorldProjection);
            //flagShader.SetVariable("matWorld", ShaderBase.ShaderParameters.World);
            Vector3 currentWind = Program.Instance.Weather.Wind.GetWindAt(Position, false);
            float windSpeed = currentWind.Length();
            if (windSpeed > 0.01f)
                flag.RotateYAngle = -(float)Math.Atan2(currentWind.Z,currentWind.X);
            flagTime += elapsedTime*(windSpeed);
            flagShader.SetVariable("time", (float)-flagTime);
            flagShader.SetVariable("windSpeed", windSpeed);
            base.OnFrameMove(device, totalTime, elapsedTime);
        }

        public override void OnFrameRender(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            base.OnFrameRender(device, totalTime, elapsedTime);
        }
        #endregion

        #region Public static methods
        public static void SetSky(Vector3 sunPosition, Color ambientColor, Color sunlightColor, float terrainAmbient, float terrainSun)
        {
            float light = (terrainAmbient * (ambientColor.R + ambientColor.G + ambientColor.B) +
                terrainSun * (sunlightColor.R + sunlightColor.G + sunlightColor.B))/1530f;

            Vector3 sunPosNorm = sunPosition;
            sunPosNorm.Normalize();
            Vector3 sunColor = new Vector3(sunlightColor.R, sunlightColor.G, sunlightColor.B);
            sunColor.Normalize();
            if (flagShader != null)
            {
                flagShader.SetVariable("vecLightDir", -sunPosNorm);
                flagShader.SetVariable("sunColor", sunColor);
                flagShader.SetVariable("light", 1.0f);
            }

        }

        public static void ApplyAds(string oldTexture, string newTexture)
        {
            try
            {
                if (oldTexture == "ad2.jpg")
                {
                    Flag.oldTexture = oldTexture;
                    Flag.newTexture = newTexture;                 
                }
                if (meshFlag != null)
                {
                    if (meshFlag.ReplaceTexture(oldTexture, newTexture, true))
                    {
                        Flag.oldTexture = oldTexture;
                        Flag.newTexture = newTexture;
                    }
                }
            }
            catch
            {
            }
        }
        #endregion
    }
}
