using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Objects;
using Bonsai.Objects.Meshes;
using Bonsai.Core;
using Microsoft.DirectX;
using System.Data;

namespace RCSim
{
    internal class SelectedObject : GameObject
    {
        #region Private fields
        private DataRow currentObject = null;
        #endregion

        #region Constructor
        public SelectedObject()
        {
            XMesh mesh = new XMesh("data\\arrow.x");
            this.Mesh = mesh;
        }
        #endregion

        #region Public methods
        public void SetSelectedObject(DataRow currentObject)
        {
            this.currentObject = currentObject;
        }
        #endregion

        #region Overridden GameObject methods
        public override void OnFrameMove(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            if (currentObject != null)
            {
                this.Position = (Vector3)(currentObject["Position"]) + new Vector3(0,4f,0);
                this.Orientation = new Vector3(0, (float)totalTime, 0);
                base.OnFrameMove(device, totalTime, elapsedTime);
            }
        }

        public override void OnFrameRender(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            if (currentObject != null)
            {
                base.OnFrameRender(device, totalTime, elapsedTime);
            }
        }
        #endregion
    }
}
