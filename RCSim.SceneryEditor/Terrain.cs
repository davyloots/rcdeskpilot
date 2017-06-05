using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bonsai.Objects;
using Bonsai.Objects.Shaders;
using Bonsai.Objects.Terrain;
using Bonsai.Core;
using Bonsai.Objects.Meshes;
using Bonsai.Objects.Textures;
using Microsoft.DirectX;
using System.Data;

namespace RCSim.SceneryEditor
{
    internal class Terrain : GameObject
    {
        #region Private fields
        private GameObject ground = null;
        private ShaderBase splatShader = null;
        private Heightmap heightMap = null;
        private TerrainDefinition definition = new TerrainDefinition();
        private Vegetation vegetation = null;
        #endregion

        #region Constructor
        public Terrain(Program owner)
        {
            vegetation = new Vegetation(owner);

            ground = new GameObject();
            heightMap = new Heightmap("data/text_height.png", 1000f, 100, 100);
            heightMap.MaxHeight = 100f;
            TerrainMesh sceneryMesh = new TerrainMesh(1.0f, heightMap);
            //sceneryMesh.Texture = new Bonsai.Objects.Textures.TextureBase("data\\grass.png");
            ground.Mesh = sceneryMesh;
            //ground.RotateXAngle = (float)Math.PI / 2;

            splatShader = new ShaderBase("splatting", "splatting3.fx");
            if (Framework.Instance.DeviceCaps.PixelShaderVersion.Major >= 3)
                splatShader.SetTechnique("TextureSplatting_fx30");
            else
                splatShader.SetTechnique("TextureSplatting");
            splatShader.SetVariable("matViewProj", ShaderBase.ShaderParameters.CameraProjection);
            splatShader.SetVariable("sunPosition", new Vector3(1, 1, 1));
            splatShader.SetVariable("underwater", 0.0f);
            splatShader.SetVariable("matWorld", ShaderBase.ShaderParameters.World);
            splatShader.SetVariable("matInvertTransposeWorld", ShaderBase.ShaderParameters.WorldInvertTranspose);
            splatShader.SetVariable("cameraPos", ShaderBase.ShaderParameters.CameraPosition);
            splatShader.SetVariable("nearRepeat", 64.0f);
            splatShader.SetVariable("farRepeat", 20.0f);
            splatShader.SetVariable("nearFactor2", 3.0f);
            splatShader.SetVariable("farFactor2", 2.0f);
            splatShader.SetVariable("nearFactor3", 3.0f);
            splatShader.SetVariable("farFactor3", 2.0f);
            splatShader.SetVariable("nearFactor4", 3.0f);
            splatShader.SetVariable("farFactor4", 2.0f);
            splatShader.SetVariable("blendSqDistance", 50f * 50f);
            splatShader.SetVariable("blendSqWidth", 1.0f / (200f * 200f));
            TextureBase normalMap = new TextureBase("data/text_norm.png");
            TextureBase alphaMap = new TextureBase("data/text.dds");
            TextureBase grassTexture = new TextureBase("data/grass.png");
            TextureBase rockTexture = new TextureBase("data/rock.png");
            TextureBase rockNormal = new TextureBase("data/splat_rock_normal.jpg");
            TextureBase sandTexture = new TextureBase("data/sand.png");
            TextureBase concreteTexture = new TextureBase("data/road.png");

            splatShader.SetVariable("NormalMapTexture", normalMap);
            splatShader.SetVariable("AlphaTexture", alphaMap);
            splatShader.SetVariable("DetailTexture1", grassTexture);
            splatShader.SetVariable("DetailTexture2", rockTexture);
            splatShader.SetVariable("DetailTexture2NormalMap", rockNormal);
            splatShader.SetVariable("DetailTexture3", sandTexture);
            splatShader.SetVariable("DetailTexture4", concreteTexture);

            ground.Shader = splatShader;
        }
        #endregion

        #region Public methods
        public void LoadDefinition(string filename)
        {
            definition.Load(filename);
            foreach (DataRow treeRow in definition.TreeTable.Rows)
            {
                AddObjectMesh(TerrainDefinition.ObjectTypeEnum.Tree, (Vector3)treeRow["Position"], new Vector3(), null);
            }
        }

        public void SaveDefinition(string filename)
        {
            definition.Save(filename);
        }

        public void AddObject(TerrainDefinition.ObjectTypeEnum objectType, Vector3 position, Vector3 orientation, string fileName)
        {
            switch (objectType)
            {
                case TerrainDefinition.ObjectTypeEnum.Tree:
                    Vector3 newPosition = new Vector3(position.X, heightMap.GetHeightAt(position.X, position.Z), position.Z);
                    definition.AddTree(newPosition);
                    AddObjectMesh(TerrainDefinition.ObjectTypeEnum.Tree, newPosition, orientation, fileName);
                    break;
                case TerrainDefinition.ObjectTypeEnum.SceneryObject:
                    definition.AddObject(fileName, position, orientation);
                    break;
            }
        }
        #endregion

        #region Private methods
        private void AddObjectMesh(TerrainDefinition.ObjectTypeEnum objectType, Vector3 position, Vector3 orientation, string fileName)
        {
            switch (objectType)
            {
                case TerrainDefinition.ObjectTypeEnum.Tree:
                    vegetation.AddTree(position.X, position.Y, position.Z);
                    break;
                case TerrainDefinition.ObjectTypeEnum.SceneryObject:
                    break;
            }
        }
        #endregion

        #region IFrameworkCallback Members
        public void OnFrameMove(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            ground.OnFrameMove(device, totalTime, elapsedTime);
            vegetation.OnFrameMove(device, totalTime, elapsedTime);
        }

        public void OnFrameRender(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            ground.OnFrameRender(device, totalTime, elapsedTime);
            vegetation.OnFrameRender(device, totalTime, elapsedTime);
        }
        #endregion
    }
}
