using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bonsai.Objects;
using Bonsai.Objects.Meshes;
using Microsoft.DirectX;

namespace RCSim
{
    public class AirplaneModel : GameObject, IDisposable
    {
        public AirplaneModel()
        {
            this.Mesh = new XMesh("/data/cessna/cessna_fixed.x");
            this.Scale = new Vector3(0.11f, 0.11f, 0.11f);
            this.Position = new Vector3(0.0f, 10.0f, 0.0f);

            GameObject elevator = new AirplaneParts.Elevator();
            AddChild(elevator);
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
    }
}
