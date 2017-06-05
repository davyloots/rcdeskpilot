using System;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Bonsai.Core;
using Bonsai.Objects.Meshes;

namespace Bonsai.Objects.Collision
{
    public class CollisionMesh : IDisposable
    {
        #region Protected fields
        protected Mesh mesh;
        protected GameObject gameObject;
        #endregion

        #region Constructor
        public CollisionMesh(GameObject gameObject) :
            this(gameObject, null)
        {

        }

        public CollisionMesh(GameObject gameObject, XMesh xmesh)
        {
            this.gameObject = gameObject;
            if (xmesh != null)
                this.mesh = xmesh.SystemMesh;
            else
            {
                this.mesh = (gameObject.Mesh as XMesh).SystemMesh;
            }
            // Register the collision mesh.
            CollisionManager.AddCollisionMesh(this);
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Clean up.
        /// </summary>
        public void Dispose()
        {
            // Unregister the collision mesh.
            CollisionManager.RemoveCollisionMesh(this);
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Checks whether a ray intersects with the collision mesh.
        /// </summary>
        /// <param name="rayPos"></param>
        /// <param name="rayDir"></param>
        /// <returns></returns>
        public bool Intersects(Vector3 rayPos, Vector3 rayDir)
        {
            if (this.mesh != null)
            {
                // Transform the ray to the GameObject coordinates
                Vector3 rayPosModel = rayPos;
                Vector3 rayDirModel = rayDir;
                Matrix inverseTransformMatrix = Matrix.Invert(gameObject.TransformMatrix);
                rayPosModel.TransformCoordinate(inverseTransformMatrix);
                rayDirModel.TransformCoordinate(inverseTransformMatrix);

                IntersectInformation intersectInfo = new IntersectInformation();
                if (this.mesh.Intersect(rayPosModel, rayDirModel, out intersectInfo))
                {
                    if (intersectInfo.Dist >= 0 &&
                        intersectInfo.Dist < rayDir.Length())
                    {
                        return true;
                    }
                }
            }
            // If arrived here, no intersection occurred
            return false;
        }

        /// <summary>
        /// Checks whether a ray intersects with the collision mesh.
        /// </summary>
        /// <param name="rayPos"></param>
        /// <param name="rayDir"></param>
        /// <param name="depth"></param>
        /// <returns></returns>
        public bool Intersects(Vector3 rayPos, Vector3 rayDir, out float depth)
        {
            if (this.mesh != null)
            {
                // Transform the ray to the GameObject coordinates
                Vector3 rayPosModel = rayPos;
                Vector3 rayDirModel = rayDir;
                Matrix inverseTransformMatrix = Matrix.Invert(gameObject.TransformMatrix);
                rayPosModel.TransformCoordinate(inverseTransformMatrix);
                rayDirModel.TransformCoordinate(inverseTransformMatrix);

                IntersectInformation intersectInfo = new IntersectInformation();
                if (this.mesh.Intersect(rayPosModel, rayDirModel, out intersectInfo))
                {
                    if (intersectInfo.Dist >= 0 &&
                        intersectInfo.Dist < rayDir.Length())
                    {
                        depth = intersectInfo.Dist;
                        return true;
                    }
                }
            }
            // If arrived here, no intersection occurred
            depth = 0;
            return false;
        }
        #endregion
    }
}
