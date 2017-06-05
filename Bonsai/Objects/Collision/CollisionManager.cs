using System;
using System.Collections.Generic;
using System.Text;

namespace Bonsai.Objects.Collision
{
    /// <summary>
    /// Static class maintaining the currently loaded collisionmeshes.
    /// </summary>
    public static class CollisionManager
    {
        #region Protected fields
        private static List<CollisionMesh> meshList = new List<CollisionMesh>();
        #endregion

        #region Public properties
        public static List<CollisionMesh> MeshList
        {
            get { return meshList; }
        }
        #endregion

        #region Internal collision management methods
        internal static void AddCollisionMesh(CollisionMesh mesh)
        {
            meshList.Add(mesh);
        }
        internal static void RemoveCollisionMesh(CollisionMesh mesh)
        {
            meshList.Remove(mesh);
        }
        #endregion
    }
}
