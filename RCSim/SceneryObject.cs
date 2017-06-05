using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Objects;
using Bonsai.Objects.Meshes;

namespace RCSim
{
    internal class SceneryObject : GameObject, IDisposable
    {
        #region protected struct GlobalMesh
        protected struct GlobalMesh
        {
            public XMesh Mesh;
            public int ReferenceCount;
        }
        #endregion

        #region Protected fields
        protected string fileName = null;
        protected static Dictionary<string, GlobalMesh> _Meshes = new Dictionary<string, GlobalMesh>();
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a Scenery Object.
        /// </summary>
        /// <param name="fileName"></param>
        public SceneryObject(string fileName)
        {
            this.fileName = fileName;

            LoadMesh();
        }
        #endregion
        
        #region IDisposable Members
        /// <summary>
        /// Clean up.
        /// </summary>
        public override void Dispose()
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
        private void LoadMesh()
        {
            if (_Meshes.ContainsKey(fileName))
            {
                GlobalMesh globalMesh = _Meshes[fileName];
                this.Mesh = globalMesh.Mesh;
                globalMesh.ReferenceCount = globalMesh.ReferenceCount + 1;
                _Meshes[fileName] = globalMesh;
            }
            else
            {
                this.Mesh = new XMesh(fileName);
                GlobalMesh globalMesh = new GlobalMesh();
                globalMesh.Mesh = this.Mesh as XMesh;
                globalMesh.ReferenceCount = 1;
                _Meshes.Add(fileName, globalMesh);
            }
        }
        #endregion

        
    }
}
