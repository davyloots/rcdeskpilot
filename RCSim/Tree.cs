using System;
using System.Collections.Generic;
using Bonsai.Core.Interfaces;
using Bonsai.Objects;
using Bonsai.Objects.Meshes;
using Bonsai.Objects.Textures;
using Microsoft.DirectX;
using Bonsai.Core;

namespace RCSim
{
    internal class Tree : IFrameworkCallback, IDisposable
    {
        private class Foliage : ITransparentObject
        {
            #region Private fields
            private Vector3 fixedPosition;
            private Vector3 relPosition;
            private Vector3 position;
            private SquareMesh mesh;
            private Matrix transformMatrix = new Matrix();
            private static Vector3 up = new Vector3(0, 1, 0);
            private static int counter = 0;
            private int number = 0;
            private float rotationAngle = 0;
            #endregion

            #region ITransparentObject Members
            /// <summary>
            /// Gets/Sets the position of the foliage.
            /// </summary>
            public Vector3 WorldPosition
            {
                get { return position; }
            }

            public Vector3 FixedPosition
            {
                get { return fixedPosition; }
                set 
                { 
                    fixedPosition = value;
                    Vector3 tempRelPos = relPosition;
                    tempRelPos.TransformCoordinate(Matrix.RotationY(rotationAngle));
                    position = fixedPosition + tempRelPos;
                }
            }

            public Vector3 RelativePosition
            {
                get { return relPosition; }
                set 
                { 
                    relPosition = value;
                    Vector3 tempRelPos = relPosition;
                    tempRelPos.TransformCoordinate(Matrix.RotationY(rotationAngle));
                    position = fixedPosition + tempRelPos;
                }
            }

            public float RotationAngle
            {
                get { return rotationAngle; }
                set
                {
                    rotationAngle = value;
                    Vector3 tempRelPos = relPosition;
                    tempRelPos.TransformCoordinate(Matrix.RotationY(rotationAngle));
                    position = fixedPosition + tempRelPos;
                }
            }
            #endregion

            #region Public properties
            public SquareMesh Mesh
            {
                get { return mesh; }
                set { mesh = value; }
            }
            #endregion

            #region Constructor
            public Foliage()
            {
                number = counter++;
            }
            #endregion

            #region IFrameworkCallback Members
            void IFrameworkCallback.OnFrameMove(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
            {
                Vector3 diff = Framework.Instance.CurrentCamera.LookFrom - position;
                float lengthSq = diff.LengthSq();
                if (lengthSq < 0.0001f)
                    diff = Framework.Instance.CurrentCamera.LookAt - Framework.Instance.CurrentCamera.LookFrom;
                else
                    diff *= (float)(1.0f / Math.Sqrt(lengthSq));

                //Vector3 crossed = Vector3.Cross(up, diff);
                // Make the leaves wiggle
                Vector3 crossed = Vector3.Cross(new Vector3((float)Math.Sin(2 * totalTime + number) / 30f, 1f, (float)Math.Cos(2 * totalTime + number) / 30f), diff);
                crossed.Normalize();
                Vector3 final = Vector3.Cross(diff, crossed);

                transformMatrix.M11 = final.X;
                transformMatrix.M12 = final.Y;
                transformMatrix.M13 = final.Z;
                transformMatrix.M14 = 0;
                transformMatrix.M21 = crossed.X;
                transformMatrix.M22 = crossed.Y;
                transformMatrix.M23 = crossed.Z;
                transformMatrix.M24 = 0;
                transformMatrix.M31 = diff.X;
                transformMatrix.M32 = diff.Y;
                transformMatrix.M33 = diff.Z;
                transformMatrix.M34 = 0;
                transformMatrix.M41 = position.X;
                transformMatrix.M42 = position.Y;
                transformMatrix.M43 = position.Z;
                transformMatrix.M44 = 1.0f;
            }

            void IFrameworkCallback.OnFrameRender(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
            {
                // Move
                device.Transform.World = transformMatrix;
                // Render
                mesh.OnFrameRender(device, totalTime, elapsedTime);
                // Reset transformation
                device.Transform.World = Matrix.Identity;
            }
            #endregion
        }


        #region Private fields
        private GameObject trunk = null;
        private static TransparentXMesh trunkMesh = null;
        private static SquareMesh foliageMesh = null;
        private List<Foliage> foliages = new List<Foliage>();
        private Program owner = null;
        private static TextureBase foliageTexture = null;
        private Vector3[] foliagePositions = new Vector3[] { new Vector3(0f, 4.155316f, 1.188562f), new Vector3(1.22025f, 3.287096f, 0.078891f),
            new Vector3(-0.0984f, 3.941261f, -2.30767f), new Vector3(-1.384411f,3.64767f,-1.310369f), new Vector3(0,6f,0)};
        private static int counter = 0;
        #endregion

        public Vector3 Position
        {
            get { return trunk.Position; }
            set
            {
                trunk.Position = value;
                foreach (Foliage fol in foliages)
                {
                    fol.FixedPosition = value;
                }
            }
        }

        public float RotationAngle
        {
            set
            {
                trunk.RotateYAngle = value;
                foreach (Foliage fol in foliages)
                {
                    fol.RotationAngle = value;
                }
            }
        }

        #region Constructor
        public Tree(Program owner)
        {
            this.owner = owner;

            // Instantiate statics if needed
            if (trunkMesh == null)
                trunkMesh = new TransparentXMesh("/data/tree_trunk.x");
            if (foliageMesh == null)
                foliageMesh = new SquareMesh(2f, 1, 1, 1.0f, new Vector3(0, 0, 1));
            if (foliageTexture == null)
                foliageTexture = new TextureBase("/data/leaves1.png");

            trunk = new GameObject();            
            trunk.Mesh = trunkMesh;
            trunk.Scale = new Vector3(1f, 1f, 1f);
                        
            foliageMesh.Texture = foliageTexture;
            foliageMesh.Texture.Transparent = true;

            foreach (Vector3 pos in foliagePositions)
            {
                Foliage foliage = new Foliage();
                foliage.RelativePosition = pos;
                foliage.Mesh = foliageMesh;
                foliages.Add(foliage);
                owner.TransparentObjectManager.Objects.Add(foliage);
            }

            counter++;
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Clean up.
        /// </summary>
        public void Dispose()
        {
            foreach (Foliage fol in foliages)
            {
                owner.TransparentObjectManager.Objects.Remove(fol);
            }
            foliages.Clear();

            counter--;
            if (counter == 0)
            {
                if (foliageMesh != null)
                {
                    foliageMesh.Dispose();
                    foliageMesh = null;
                }
                if (trunkMesh != null)
                {
                    trunkMesh.Dispose();
                    trunkMesh = null;
                }
                if (foliageTexture != null)
                {
                    foliageTexture.Dispose();
                    foliageTexture = null;
                }
            }
        }
        #endregion

        #region IFrameworkCallback Members

        public void OnFrameMove(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            trunk.OnFrameMove(device, totalTime, elapsedTime);            
        }

        public void OnFrameRender(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            trunk.OnFrameRender(device, totalTime, elapsedTime);            
        }

        #endregion

        

        
    }
}
