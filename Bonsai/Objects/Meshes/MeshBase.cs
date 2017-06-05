using System;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Bonsai.Core;
using Bonsai.Core.Interfaces;
using Bonsai.Objects.Meshes;

namespace Bonsai.Objects.Meshes
{
    public abstract class MeshBase : IFrameworkCallback, IDisposable
    {
        #region IFrameworkCallback Members
        public virtual void OnFrameMove(Device device, double totalTime, float elapsedTime)
        {
        }

        public virtual void OnFrameRender(Device device, double totalTime, float elapsedTime)
        {
        }
        #endregion

        #region IDisposable Members
        public virtual void Dispose()
        {
        }
        #endregion
    }
}
