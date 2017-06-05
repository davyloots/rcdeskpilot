using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Objects;
using Bonsai.Objects.Meshes;
using Bonsai.Objects.Shaders;
using Bonsai.Objects.Textures;

namespace RCSim
{
    internal class WindSock : GameObject, IDisposable
    {
        #region private fields
        private static XMesh meshWindSock = null;
        private static ShaderBase flagShader = null;
        private static int count = 0;
        private static TextureBase texture = null;
        #endregion

        #region Constructor
        public WindSock()
        {
            count++;
            if (meshWindSock == null)
            {
                meshWindSock = new XMesh("flag.x");
                flagShader = new ShaderBase("FlagShader", "flag.fx");
                flagShader.SetVariable("matWorldViewProj", ShaderBase.ShaderParameters.WorldProjection);
                flagShader.SetVariable("matWorld", ShaderBase.ShaderParameters.World);
                flagShader.SetTechnique("TVertexShaderOnly");
                //texture = new TextureBase("EuropeFlag.jpg");
                //flagShader.SetVariable("Tex0", texture);
            }
            this.Mesh = meshWindSock;
            this.Shader = flagShader;
            this.Scale = new Microsoft.DirectX.Vector3(1f, 1f, 1f);
            this.Position = new Microsoft.DirectX.Vector3(1, 1, 0);
            
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Clean up.
        /// </summary>
        public override void Dispose()
        {
            count--;
            if (count == 0)
            {
                meshWindSock.Dispose();
                meshWindSock = null;
                flagShader.Dispose();
                flagShader = null;
            }
        }
        #endregion

        #region Overridden GameObject methods
        public override void OnFrameMove(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            flagShader.SetVariable("time", (float)-totalTime);
            base.OnFrameMove(device, totalTime, elapsedTime);
        }

        public override void OnFrameRender(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            base.OnFrameRender(device, totalTime, elapsedTime);
        }
        #endregion
    }
}
