using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Core.Interfaces;
using Bonsai.Objects;
using Bonsai.Objects.Meshes;
using System.Collections;
using Bonsai.Objects.Cameras;
using Bonsai.Core;
using Microsoft.DirectX;
using Bonsai.Objects.Textures;
using System.Data;
using Bonsai.Objects.Terrain;

namespace RCSim
{
    internal class Vegetation : IFrameworkCallback, IDisposable
    {
        #region Private fields
        List<GameObject> plants = new List<GameObject>();
        TextureBase simpleTreeTexture1;
        TextureBase simpleTreeTexture2;
        TextureBase simpleTallTreeTexture1;
        TextureBase simpleTallTreeTexture2;
        TextureBase simpleSmallTreeTexture1;
        SquareMesh simpleTreeMesh1;
        SquareMesh simpleTreeMesh2;
        SquareMesh simpleTallTreeMesh1;
        SquareMesh simpleTallTreeMesh2;
        SquareMesh simpleSmallTreeMesh1;
        TextureBase grassTexture;
        SquareMesh grassMesh;
        Program owner = null;
        private List<Tree> trees = new List<Tree>();
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="owner"></param>
        public Vegetation(Program owner)
        {
            this.owner = owner;
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Clean up.
        /// </summary>
        public void Dispose()
        {
            foreach (GameObject plant in plants)
            {
                owner.TransparentObjectManager.Objects.Remove(plant);
                plant.Dispose();
            }
            plants.Clear();
            foreach (Tree tree in trees)
            {
                tree.Dispose();
            }
            trees.Clear();
            if (simpleTreeTexture1 != null)
            {
                simpleTreeTexture1.Dispose();
                simpleTreeTexture1 = null;
            }
            if (simpleTreeTexture2 != null)
            {
                simpleTreeTexture2.Dispose();
                simpleTreeTexture2 = null;
            }
            if (simpleTallTreeTexture1 != null)
            {
                simpleTallTreeTexture1.Dispose();
                simpleTallTreeTexture1 = null;
            }
            if (simpleTallTreeTexture2 != null)
            {
                simpleTallTreeTexture2.Dispose();
                simpleTallTreeTexture2 = null;
            }
            if (simpleSmallTreeTexture1 != null)
            {
                simpleSmallTreeTexture1.Dispose();
                simpleSmallTreeTexture1 = null;
            }
            if (simpleTreeMesh1 != null)
            {
                simpleTreeMesh1.Dispose();
                simpleTreeMesh1 = null;
            }
            if (simpleTreeMesh2 != null)
            {
                simpleTreeMesh2.Dispose();
                simpleTreeMesh2 = null;
            }
            if (simpleTallTreeMesh1 != null)
            {
                simpleTallTreeMesh1.Dispose();
                simpleTallTreeMesh1 = null;
            }
            if (simpleTallTreeMesh2 != null)
            {
                simpleTallTreeMesh2.Dispose();
                simpleTallTreeMesh2 = null;
            }
            if (simpleSmallTreeMesh1 != null)
            {
                simpleSmallTreeMesh1.Dispose();
                simpleSmallTreeMesh1 = null;
            }
            if (grassTexture != null)
            {
                grassTexture.Dispose();
                grassTexture = null;
            }
            if (grassMesh != null)
            {
                grassMesh.Dispose();
                grassMesh = null;
            }
        }
        #endregion

        #region Public methods
        public void CreateTrees()
        {
            AddGrass(5f, 5f);
            AddGrass(5f, 6f);
            AddGrass(6f, 5f);
        }

        public void LoadTrees(Heightmap heightMap, TerrainDefinition definition)
        {
            foreach (DataRow treeRow in definition.TreeTable.Rows)
            {
                Vector3 position = (Vector3)treeRow["Position"];
                AddTree(position.X, heightMap.GetHeightAt(position.X, position.Z), position.Z);
            }
        }

        public void AddSimpleTree(float x, float y, float z)
        {
            if (simpleTreeTexture1 == null)
            {
                simpleTreeTexture1 = new TextureBase("/data/tree1_256.png");
            }
            if (simpleTreeTexture2 == null)
            {
                simpleTreeTexture2 = new TextureBase("/data/tree2_256.png");
            }
            if (simpleTreeMesh1 == null)
            {
                simpleTreeMesh1 = new SquareMesh(2.5f, 1, 1, 1.0f);
                simpleTreeMesh1.Texture = simpleTreeTexture1;
                simpleTreeMesh1.Texture.Transparent = true; 
            }
            if (simpleTreeMesh2 == null)
            {
                simpleTreeMesh2 = new SquareMesh(2.5f, 1, 1, 1.0f);
                simpleTreeMesh2.Texture = simpleTreeTexture2;
                simpleTreeMesh2.Texture.Transparent = true;
            }
            
            GameObject tree = new GameObject();
            if (plants.Count % 2 == 0)
                tree.Mesh = simpleTreeMesh1;
            else
                tree.Mesh = simpleTreeMesh2;
            tree.Position = new Vector3(x, y + 2.5f, z);
            tree.IsBillboard = true;
            plants.Add(tree);
            owner.TransparentObjectManager.Objects.Add(tree);
        }

        public void AddSimpleTallTree(float x, float y, float z)
        {
            if (simpleTallTreeTexture1 == null)
            {
                simpleTallTreeTexture1 = new TextureBase("/data/tall_tree1_256.png");
            }
            if (simpleTallTreeTexture2 == null)
            {
                simpleTallTreeTexture2 = new TextureBase("/data/tall_tree2_256.png");
            }
            if (simpleTallTreeMesh1 == null)
            {
                simpleTallTreeMesh1 = new SquareMesh(5.0f, 1, 1, 1.0f);
                simpleTallTreeMesh1.Texture = simpleTallTreeTexture1;
                simpleTallTreeMesh1.Texture.Transparent = true;
            }
            if (simpleTallTreeMesh2 == null)
            {
                simpleTallTreeMesh2 = new SquareMesh(5.0f, 1, 1, 1.0f);
                simpleTallTreeMesh2.Texture = simpleTallTreeTexture2;
                simpleTallTreeMesh2.Texture.Transparent = true;
            }

            GameObject tree = new GameObject();
            if (plants.Count % 2 == 0)
            {
                tree.Mesh = simpleTallTreeMesh1;
                tree.Scale = new Vector3(1.0f, 1.4f, 1.0f);
                tree.Position = new Vector3(x, y + 7.0f, z);
            }
            else
            {
                tree.Mesh = simpleTallTreeMesh2;
                tree.Scale = new Vector3(0.8f, 1.0f, 1.0f);
                tree.Position = new Vector3(x, y + 5.0f, z);
            }                        
            tree.IsBillboard = true;
            plants.Add(tree);
            owner.TransparentObjectManager.Objects.Add(tree);
        }

        public void AddSimpleSmallTree(float x, float y, float z)
        {
            if (simpleSmallTreeTexture1 == null)
            {
                simpleSmallTreeTexture1 = new TextureBase("/data/small_tree1_256.png");
            }            
            if (simpleSmallTreeMesh1 == null)
            {
                simpleSmallTreeMesh1 = new SquareMesh(1.5f, 1, 1, 1.0f);
                simpleSmallTreeMesh1.Texture = simpleSmallTreeTexture1;
                simpleSmallTreeMesh1.Texture.Transparent = true;
            }          
            
            GameObject tree = new GameObject();
            tree.Mesh = simpleSmallTreeMesh1;
            tree.Scale = new Vector3(1.0f, 1.5f, 1.0f);
            tree.Position = new Vector3(x, y + 2.25f, z);
            tree.IsBillboard = true;
            plants.Add(tree);
            owner.TransparentObjectManager.Objects.Add(tree);
        }

        public void AddTree(float x, float y, float z)
        {
            Tree tree = new Tree(owner);
            tree.Position = new Vector3(x,y,z);
            tree.RotationAngle = x + y + z;
            trees.Add(tree);
        }

        public void AddGrass(float x, float z)
        {
            if (grassTexture == null)
            {
                grassTexture = new TextureBase("/data/grassbillboard.png");
            }
            if (grassMesh == null)
            {
                grassMesh = new SquareMesh(1f, 1, 1, 1.0f);
                grassMesh.Texture = grassTexture;
                grassMesh.Texture.Transparent = true;
            }
            
            GameObject grass = new GameObject();
            grass.Mesh = grassMesh;
            grass.Scale = new Vector3(1f, 1f, 5f);
            grass.Position = new Vector3(x, 0.5f, z);
            grass.IsBillboard = true;
            plants.Add(grass);
            owner.TransparentObjectManager.Objects.Add(grass);
        }
        #endregion

        #region IFrameworkCallback Members
        public void OnFrameMove(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            foreach (Tree tree in trees)
                tree.OnFrameMove(device, totalTime, elapsedTime);
            // Do nothing (transparentobjectmanager will do all)
        }

        public void OnFrameRender(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            foreach (Tree tree in trees)
                tree.OnFrameRender(device, totalTime, elapsedTime);
            // Do nothing (transparentobjectmanager will do all)
        }
        #endregion        
    }
}
