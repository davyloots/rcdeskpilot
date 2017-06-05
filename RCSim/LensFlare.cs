using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Objects;
using Bonsai.Objects.Meshes;
using Bonsai.Objects.Particles;
using Bonsai.Core.Interfaces;
using Bonsai.Core;
using Microsoft.DirectX;

namespace RCSim
{
    internal class LensFlare : IDisposable, IFrameworkCallback
    {
        protected class Flare
        {
            public GameObject Object;
            public float Position;
        }

        protected List<Flare> flares = new List<Flare>();
        protected bool visible = false;
        public static Vector3 SunPosition = new Vector3(-1587, 3918, -1531);
        public static bool Enabled = true;
        public static bool Visible = true;

        #region Constructor
        public LensFlare()
        {
            AddFlare("data\\flare1.png", 0.4f, 0.2f);
            AddFlare("data\\flare2.png", 0.6f, 0.05f);
            AddFlare("data\\flare3.png", 0.9f, 0.1f);
            AddFlare("data\\flare4.png", 1.1f, 0.1f);
            AddFlare("data\\flare1.png", 1.2f, 0.05f);
            Enabled = (Convert.ToInt32(Bonsai.Utils.Settings.GetValue("LensFlare", "1")) == 1);
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            foreach (Flare flare in flares)
            {
                PointMesh mesh = flare.Object.Mesh as PointMesh;
                mesh.Dispose();
                flare.Object.Mesh = null;
                flare.Object.Dispose();
            }
            flares.Clear();
        }
        #endregion

        #region Private methods
        private void AddFlare(string texture, float position, float size)
        {
            Flare flare = new Flare();
            PointMesh mesh = new PointMesh(size);
            mesh.Texture = new Bonsai.Objects.Textures.TextureBase(texture);
            mesh.Texture.Transparent = true;
            flare.Object = new GameObject();
            flare.Object.Mesh = mesh;
            flare.Position = position;
            flares.Add(flare);
        }
        #endregion

        #region IFrameworkCallback Members

        public void OnFrameMove(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            if (Enabled && Visible && Framework.Instance.CurrentCamera.PointVisible(SunPosition))
            {
                float sunDistance = (SunPosition - Framework.Instance.CurrentCamera.LookFrom).Length(); ;
                Vector3 frontVector = (Framework.Instance.CurrentCamera.LookAt - Framework.Instance.CurrentCamera.LookFrom);
                frontVector.Normalize();
                Vector3 flareEndpoint = Framework.Instance.CurrentCamera.LookFrom + (sunDistance/3) * frontVector;
                Vector3 flareDirection = (flareEndpoint - SunPosition);
                visible = true;
                foreach (Flare flare in flares)
                {
                    flare.Object.Position = SunPosition + flareDirection * flare.Position;
                    flare.Object.OnFrameMove(device, totalTime, elapsedTime);
                }                
            }
            else
                visible = false;
            
        }

        public void OnFrameRender(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            if (visible)
            {
                foreach (Flare flare in flares)
                {
                    flare.Object.OnFrameRender(device, totalTime, elapsedTime);
                }                
            }
        }

        #endregion
    }
}
