using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Objects;
using Bonsai.Objects.Meshes;
using Microsoft.DirectX;
using Bonsai.Sound;
using Bonsai.Core;

namespace RCSim
{
    internal class Gate : GameObject, IDisposable
    {
        #region protected struct GlobalMesh
        protected struct GlobalMesh
        {
            public XMesh Mesh;
            public int ReferenceCount;
        }
        #endregion

        #region public enums
        public enum GateType
        {
            Pylon = 1
        }
        #endregion

        #region Protected fields
        protected string fileName = null;
        protected static Dictionary<string, GlobalMesh> _Meshes = new Dictionary<string, GlobalMesh>();
        protected int sequenceNr = 0;
        protected GateType gateType = GateType.Pylon;
        protected LineMesh lineMesh1 = new LineMesh();
        protected LineMesh lineMesh2 = new LineMesh();
        protected float x1, y1, x2, y2;
        protected float gateSize = 3.0f;
        protected float gateHeight = 6.0f;
        protected bool active = false;
        protected Vector3 previousPosition;
        protected Program owner = null;
        #endregion

        #region Public events
        public event EventHandler GatePassed;
        #endregion

        #region Public properties
        public int SequenceNumber
        {
            get { return sequenceNr; }
        }

        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        public float Height
        {
            get { return gateHeight; }
            set { gateHeight = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a Gate.
        /// </summary>
        /// <param name="fileName"></param>
        public Gate(Program owner, Vector3 position, Vector3 orientation, int sequenceNr, int type)
        {
            this.owner = owner;
            this.Position = position;
            this.Orientation = orientation;
            this.sequenceNr = sequenceNr;
            this.gateType = (GateType)type;
            this.Scale = new Vector3(2, 2, 2);
            LoadMesh("gate1.x");
            x1 = position.X + gateSize * (float)Math.Cos((double)orientation.Y);
            y1 = position.Z - gateSize * (float)Math.Sin((double)orientation.Y);
            x2 = position.X - gateSize * (float)Math.Cos((double)orientation.Y);
            y2 = position.Z + gateSize * (float)Math.Sin((double)orientation.Y);
            lineMesh1.Vertex1 = new Vector3(x1, position.Y, y1);
            lineMesh1.Vertex2 = new Vector3(x1, position.Y + gateHeight, y1);
            lineMesh2.Vertex1 = new Vector3(x2, position.Y, y2);
            lineMesh2.Vertex2 = new Vector3(x2, position.Y + gateHeight, y2);
            /*
            lineMesh1.Vertex1 = this.Position + new Vector3(3.0f * (float)Math.Cos((double)orientation.Y), 0, -3.0f * (float)Math.Sin((double)orientation.Y));
            lineMesh1.Vertex2 = this.Position + new Vector3(3.0f * (float)Math.Cos((double)orientation.Y), 10, -3.0f * (float)Math.Sin((double)orientation.Y));
            lineMesh2.Vertex1 = this.Position + new Vector3(-3.0f * (float)Math.Cos((double)orientation.Y), 0, 3.0f * (float)Math.Sin((double)orientation.Y));
            lineMesh2.Vertex2 = this.Position + new Vector3(-3.0f * (float)Math.Cos((double)orientation.Y), 10, 3.0f * (float)Math.Sin((double)orientation.Y));
             */
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Clean up.
        /// </summary>
        void IDisposable.Dispose()
        {
            if (_Meshes.ContainsKey(fileName))
            {
                GlobalMesh globalMesh = _Meshes[fileName];
                globalMesh.ReferenceCount--;
                if (globalMesh.ReferenceCount == 0)
                {
                    globalMesh.Mesh.Dispose();
                    globalMesh.Mesh = null;
                    _Meshes.Remove(fileName);
                }
                else
                {
                    _Meshes[fileName] = globalMesh;
                }
            }
        }
        #endregion

        #region Private methods
        private void LoadMesh(string filename)
        {
            this.fileName = filename;
            if (_Meshes.ContainsKey(fileName))
            {
                GlobalMesh globalMesh = _Meshes[fileName];
                this.Mesh = globalMesh.Mesh;
                globalMesh.ReferenceCount = globalMesh.ReferenceCount + 1;
                _Meshes[fileName] = globalMesh;
            }
            else
            {
                XMesh xMesh = new XMesh(fileName);
                if (Utility.MediaExists("ads/ad2.jpg"))
                    xMesh.ReplaceTexture("ad2.jpg", "ads/ad2.jpg", true);                
                this.Mesh = xMesh;
                GlobalMesh globalMesh = new GlobalMesh();
                globalMesh.Mesh = this.Mesh as XMesh;
                globalMesh.ReferenceCount = 1;
                _Meshes.Add(fileName, globalMesh);
            }
        }
       
        private bool CCW(float xa, float ya, float xb, float yb, float xc, float yc)
        {
            return (yc - ya) * (xb - xa) > (yb - ya) * (xc - xa);
        }
        #endregion

        #region Overridden GameObject methods
        public override void OnFrameMove(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
#if !EDITOR            
            float xa = owner.Player.Position.X;
            float ya = owner.Player.Position.Z;
            float xb = previousPosition.X;
            float yb = previousPosition.Z;

            if ((sequenceNr == 0) || active)
            {
                if ((CCW(x1, y1, xa, ya, xb, yb) != CCW(x2, y2, xa, ya, xb, yb)) &&
                    (CCW(x1, y1, x2, y2, xa, ya) != CCW(x1, y1, x2, y2, xb, yb)) &&
                    (owner.Player.Position.Y < (Position.Y + gateHeight)))
                {
                    if (GatePassed != null)
                    {
                        GatePassed(this, EventArgs.Empty);
                    }
                }
            }
            /*
            float denom = (yb - ya) * (x2 - x1) - (xb - xa) * (y2 - y1);
            if (denom > 0.001)
            {
                float nom1 = (xb - xa) * (y1 - ya) - (yb - ya) * (x1 - xa);
                float nom2 = (x2 - x1) * (y1 - ya) - (y2 - y1) * (x1 - xa);
                float u1 = nom1 / denom;
                float u2 = nom2 / denom;
                if ((u1 > 0) && (u2 > 0) && (u1 < 1) && (u2 < 1))
                {
                    // gate passed!
                    passSound.Play(false);
                }
            }
            */
            previousPosition = owner.Player.Position;
#endif
            base.OnFrameMove(device, totalTime, elapsedTime);
        }

        public override void OnFrameRender(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            //lineMesh1.OnFrameRender(device, totalTime, elapsedTime);
            //lineMesh2.OnFrameRender(device, totalTime, elapsedTime);
            base.OnFrameRender(device, totalTime, elapsedTime);
        }
        #endregion
    }
}
