using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Objects;
using Bonsai.Objects.Meshes;
using Microsoft.DirectX;

namespace RCSim.AircraftEditor
{
    internal class Cursor3d : GameObject
    {
        protected XMesh cursorMesh = null;

        public Cursor3d()
        {
            cursorMesh = new XMesh("data/cursor.x");
            this.Scale = new Vector3(0.1f, 0.1f, 0.1f);
            this.Mesh = cursorMesh;
        }
    }
}
