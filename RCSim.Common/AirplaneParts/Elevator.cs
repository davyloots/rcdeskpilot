using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bonsai.Objects;
using Microsoft.DirectX;
using Bonsai.Objects.Meshes;

namespace RCSim.AirplaneParts
{
    public class Elevator : GameObject, IDisposable
    {
        public Elevator()
        {
            this.Mesh = new XMesh("/data/cessna/cessna_elevator.x");
            this.Position = new Vector3(0.0f, 0.0f, 5.7f);
        }

        #region IDisposable Members
        /// <summary>
        /// Clean up.
        /// </summary>
        void IDisposable.Dispose()
        {
            if (Mesh != null)
            {
                Mesh.Dispose();
                Mesh = null;
            }
        }
        #endregion

        public override void OnFrameMove(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            this.RotateXAngle = (float)Math.Sin(totalTime);
            base.OnFrameMove(device, totalTime, elapsedTime);
        }
    }
}
