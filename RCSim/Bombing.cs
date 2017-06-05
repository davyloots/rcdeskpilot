using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Core.Interfaces;
using Bonsai.Objects.Meshes;
using Bonsai.Objects.Textures;
using Bonsai.Objects;

namespace RCSim
{
    internal class Bombing : IFrameworkCallback, IDisposable
    {
        #region Protected fields
        protected Program owner = null;
        protected GameObject target = null;
        protected SquareMesh targetMesh = null;
        #endregion

        #region Constructor
        public Bombing(Program owner)
        {
            this.owner = owner;
            targetMesh = new SquareMesh(5.0f, 1, 1, 1.0f);
            targetMesh.Texture = new TextureBase("data\\target1.png");
            targetMesh.Texture.Transparent = true;
            target = new GameObject();
            target.Mesh = targetMesh;
            target.Position = new Microsoft.DirectX.Vector3(0, 0.004f, 0);
            target.RotateXAngle = (float)(Math.PI / 2);
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Clean up.
        /// </summary>
        public void Dispose()
        {
            if (targetMesh != null)
            {
                targetMesh.Dispose();
                targetMesh = null;
                target.Mesh = null;
            }
            if (target != null)
            {
                target.Dispose();
                target = null;
            }
        }

        #endregion

        #region IFrameworkCallback Members
        public void OnFrameMove(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            target.OnFrameMove(device, totalTime, elapsedTime);
        }

        public void OnFrameRender(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            target.OnFrameRender(device, totalTime, elapsedTime);
        }
        #endregion
    }
}
