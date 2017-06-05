using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Objects;
using Bonsai.Objects.Meshes;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using Bonsai.Objects.Textures;
using System.Drawing;

namespace RCSim.Effects
{
    internal class WaterRipple : GameObject
    {
        #region Private fields
        private double creationTime = 0;
        private float maxLifeTime = 3.0f;
        private Vector3 creationPosition;
        private int startAlpha = 255;
        #endregion

        #region Private static fields
        private static SquareMesh rippleMesh = null;
        private static Material material = new Material();
        private static Material defaultMaterial = new Material();
        private static int detail = 2;
        #endregion

        #region Public properties
        public static int Detail
        {
            get { return detail; }
        }
        #endregion

        #region Constructor
        public WaterRipple()
        {
            defaultMaterial.Diffuse = System.Drawing.Color.White;
            defaultMaterial.Ambient = System.Drawing.Color.White;
            if (rippleMesh == null)
            {
                detail = Convert.ToInt32(Bonsai.Utils.Settings.GetValue("WaterRipplesDetail", "2"));
                CreateMesh();
            }
            this.Mesh = rippleMesh;
            this.RotateXAngle = (float) Math.PI / 2;
        }
        #endregion

        #region Public methods
        public void Create(Vector3 position, double creationTime, int startAlpha)
        {
            this.creationPosition = position;
            this.creationTime = creationTime;
            this.RotateYAngle = (float)(creationTime);
            this.startAlpha = startAlpha;
        }
        #endregion

        #region Overridden IFrameworkObject methods
        public override void OnFrameMove(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            base.OnFrameMove(device, totalTime, elapsedTime);
        }

        public override void OnFrameRender(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            if (detail > 0)
            {
                float lifeTime = (float)(totalTime - creationTime);
                if (lifeTime < maxLifeTime)
                {
                    this.Scale = new Vector3(lifeTime, lifeTime, lifeTime);
                    this.Position = new Vector3(creationPosition.X, (maxLifeTime - lifeTime) * 0.001f, creationPosition.Z);
                    int alpha = (int)(startAlpha - lifeTime * (startAlpha / maxLifeTime));
                    material.Diffuse = material.Ambient = Color.FromArgb(alpha, Color.White);
                    material.Emissive = Color.Gray;
                    device.Material = material;
                    // Render
                    base.OnFrameRender(device, totalTime, elapsedTime);
                    // Reset the material
                    device.Material = defaultMaterial;
                }
            }
        }
        #endregion

        internal static void CleanUp()
        {
            if (rippleMesh != null)
            {
                if (rippleMesh.Texture != null)
                {
                    rippleMesh.Texture.Dispose();
                    rippleMesh.Texture = null;
                }
                rippleMesh.Dispose();
                rippleMesh = null;
            }            
        }

        internal static void SetDetail(int newDetail)
        {
            if (detail != newDetail)
            {
                detail = newDetail;
                if (rippleMesh != null)
                {
                    if (rippleMesh.Texture != null)
                    {
                        rippleMesh.Texture.Dispose();
                        rippleMesh.Texture = null;
                    }
                    if (detail == 2)
                    {
                        rippleMesh.Texture = new TextureBase("//data//wave128.png");
                        rippleMesh.Texture.Transparent = true;
                    }
                }
            }
        }

        private static void CreateMesh()
        {
            if (detail > 0)
            {
                rippleMesh = new SquareMesh(1.0f, 1, 1, 1.0f, new Vector3(0, 1, 0));
                if (detail == 2)                   
                    rippleMesh.Texture = new TextureBase("//data//wave128.png");
                rippleMesh.Texture.Transparent = true;
            }
        }
    }
}
