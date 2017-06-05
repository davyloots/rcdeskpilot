using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Core.Interfaces;
using Bonsai.Objects.Meshes;
using Bonsai.Objects.Textures;
using RCSim.DataClasses;
using Bonsai.Objects;
using System.IO;
using Bonsai.Objects.Shaders;

namespace RCSim
{
    internal class PhotoScenery : IFrameworkCallback, IDisposable
    {
        #region Protected fields
        protected SquareMesh[] meshes = new SquareMesh[6];
        protected TextureBase[] textures = new TextureBase[6];
        protected TextureBase[] depthMaps = new TextureBase[6];
        protected GameObject[] objects = new GameObject[6];
        protected ShaderBase[] shaders = new ShaderBase[6];
        protected SceneryParameters parameters;
        #endregion

        #region Constructor
        public PhotoScenery(SceneryParameters parameters)
        {
            this.parameters = parameters;
            BuildCube();
            Program.Instance.SwitchToObserverCamera();
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            CleanCube();
        }
        #endregion

        #region Private methods
        private void BuildCube()
        {
            float squareSize = 1000f;
            for (int i = 0; i < 6; i++)
            {
                meshes[i] = new SquareMesh(squareSize+1.0f, 1, 1, 0.999f);
                meshes[i].Lighted = false;
                meshes[i].ZBufferEnabled = false;
                meshes[i].RenderTransparent = true;
                objects[i] = new GameObject();
                FileInfo fileInfo = new FileInfo(parameters.FileName);
                string directory = fileInfo.DirectoryName + "\\";
                switch (i)
                {
                    case 0:
                        textures[i] = new TextureBase(directory + parameters.FrontPhoto);
                        if (!string.IsNullOrEmpty(parameters.FrontDepthMap))
                            depthMaps[i] = new TextureBase(directory + parameters.FrontDepthMap);
                        objects[i].Position = new Microsoft.DirectX.Vector3(0, 0, squareSize);
                        break;                        
                    case 1:
                        textures[i] = new TextureBase(directory + parameters.BackPhoto);
                        if (!string.IsNullOrEmpty(parameters.BackDepthMap))
                            depthMaps[i] = new TextureBase(directory + parameters.BackDepthMap);
                        objects[i].Position = new Microsoft.DirectX.Vector3(0, 0, -squareSize);
                        objects[i].RotateYAngle = (float) Math.PI;
                        break;                        
                    case 2:
                        textures[i] = new TextureBase(directory + parameters.RightPhoto);
                        if (!string.IsNullOrEmpty(parameters.RightDepthMap))
                            depthMaps[i] = new TextureBase(directory + parameters.RightDepthMap);
                        objects[i].Position = new Microsoft.DirectX.Vector3(squareSize, 0, 0);
                        objects[i].RotateYAngle = (float) Math.PI/2;
                        break;
                    case 3:
                        textures[i] = new TextureBase(directory + parameters.LeftPhoto);
                        if (!string.IsNullOrEmpty(parameters.LeftDepthMap))
                            depthMaps[i] = new TextureBase(directory + parameters.LeftDepthMap);
                        objects[i].Position = new Microsoft.DirectX.Vector3(-squareSize, 0, 0);
                        objects[i].RotateYAngle = (float) -Math.PI/2;
                        break;                        
                    case 4:
                        textures[i] = new TextureBase(directory + parameters.BottomPhoto);
                        if (!string.IsNullOrEmpty(parameters.BottomDepthMap))
                            depthMaps[i] = new TextureBase(directory + parameters.BottomDepthMap);
                        objects[i].Position = new Microsoft.DirectX.Vector3(0, -squareSize, 0);
                        objects[i].RotateXAngle = (float) Math.PI/2;
                        break;
                    case 5:
                        textures[i] = new TextureBase(directory + parameters.TopPhoto);
                        if (!string.IsNullOrEmpty(parameters.TopDepthMap))
                            depthMaps[i] = new TextureBase(directory + parameters.TopDepthMap);
                        objects[i].Position = new Microsoft.DirectX.Vector3(0, squareSize, 0);
                        objects[i].RotateXAngle = (float) -Math.PI/2;
                        break;
                }
                objects[i].Mesh = meshes[i];
                //meshes[i].Texture = textures[i];
                shaders[i] = new ShaderBase("photo" + i.ToString(), "data/photo.fx", false);
                objects[i].Shader = shaders[i];
                objects[i].Shader.SetVariable("Texture", textures[i]);
                if (depthMaps[i] != null)
                    objects[i].Shader.SetVariable("DepthMap", depthMaps[i]);
                objects[i].Shader.SetVariable("Distance", 0.5f);
                objects[i].Shader.SetTechnique("TPhoto");
            }            
        }

        private void CleanCube()
        {
            for (int i = 0; i < 6; i++)
            {
                if (textures[i] != null)
                {
                    textures[i].Dispose();
                    textures[i] = null;
                }
                if (depthMaps[i] != null)
                {
                    depthMaps[i].Dispose();
                    depthMaps[i] = null;
                }
                if (meshes[i] != null)
                {
                    meshes[i].Dispose();
                    meshes[i] = null;
                }
                if (shaders[i] != null)
                {
                    shaders[i].Dispose();
                    shaders[i] = null;
                }
                if (objects[i] != null)
                {
                    objects[i].Dispose();
                    objects[i] = null;
                }               
            }
        }
        #endregion

        #region IFrameworkCallback Members
        public void OnFrameMove(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            for (int i = 0; i < 6; i++)
            {
                if (objects[i] != null)
                {
                    objects[i].OnFrameMove(device, totalTime, elapsedTime);
                }
            }
        }

        public void OnFrameRender(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            float distance = 100.0f;
            if (Program.Instance.Player != null)
            {
                distance = Program.Instance.Player.Position.Length();
                if (distance > 128)
                {
                    distance = Math.Max(0.5f - (distance - 128) / 1024, 0.001f);
                }
                else
                {
                    distance = 1f - distance / 255f;
                }
            }                    
            for (int i = 0; i < 6; i++)
            {
                if (objects[i] != null)
                {
                    objects[i].Shader.SetVariable("Background", 1);
                    objects[i].Shader.SetVariable("Distance", distance);
                    objects[i].OnFrameRender(device, totalTime, elapsedTime);
                }
            }
        }

        public void OnFrameRenderFinal(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            for (int i = 0; i < 6; i++)
            {
                if ((objects[i] != null) && (depthMaps[i] != null))
                {
                    objects[i].Shader.SetVariable("Background", 0);
                    objects[i].OnFrameRender(device, totalTime, elapsedTime);
                }
            }
        }
        #endregion
    }
}
