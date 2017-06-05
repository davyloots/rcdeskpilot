using System;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Bonsai.Core;
using Bonsai.Objects.Textures;
using System.Drawing;

namespace Bonsai.Objects.Meshes
{
    public class CubeMesh : MeshBase, IDisposable
    {
        #region Private fields
        private VertexBuffer vertexBuffer = null;
        private IndexBuffer indexBuffer = null;
        private TextureBase texture = null;
        private int nVertices;
        private int nIndices;
        private int nPrimitives;
        private float size;
        #endregion

        #region Public properties
        /// <summary>
        /// Gets/Sets the texture of the dome.
        /// </summary>
        public TextureBase Texture
        {
            get { return texture; }
            set { texture = value; }
        }
        #endregion

        #region Constructor
        public CubeMesh(float size)
        {
            this.size = size;
            
            GenerateVertices();

            Framework.Instance.DeviceCreated += new Bonsai.Core.EventArgs.DeviceEventHandler(Instance_DeviceCreated);
            Framework.Instance.DeviceLost += new EventHandler(Instance_DeviceLost);
            Framework.Instance.DeviceReset += new Bonsai.Core.EventArgs.DeviceEventHandler(Instance_DeviceReset);
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Clean up.
        /// </summary>
        public override void Dispose()
        {
            Framework.Instance.DeviceCreated -= new Bonsai.Core.EventArgs.DeviceEventHandler(Instance_DeviceCreated);
            Framework.Instance.DeviceLost -= new EventHandler(Instance_DeviceLost);
            Framework.Instance.DeviceReset -= new Bonsai.Core.EventArgs.DeviceEventHandler(Instance_DeviceReset);

            if (vertexBuffer != null)
            {
                vertexBuffer.Dispose();
                vertexBuffer = null;
            }
            if (indexBuffer != null)
            {
                indexBuffer.Dispose();
                indexBuffer = null;
            }
            if (texture != null)
            {
                texture.Dispose();
                texture = null;
            }
        }
        #endregion

        #region Private event handlers
        void Instance_DeviceReset(object sender, Bonsai.Core.EventArgs.DeviceEventArgs e)
        {
            if (vertexBuffer == null)
                GenerateVertices();
        }

        void Instance_DeviceLost(object sender, EventArgs e)
        {
            if (vertexBuffer != null)
            {
                vertexBuffer.Dispose();
                vertexBuffer = null;
            }
            if (indexBuffer != null)
            {
                indexBuffer.Dispose();
                indexBuffer = null;
            }
        }

        void Instance_DeviceCreated(object sender, Bonsai.Core.EventArgs.DeviceEventArgs e)
        {
            GenerateVertices();
        }
        #endregion

        #region Private methods
        private void GenerateVertices()
        {
            // set vertex count and index count 
            this.nVertices = 24;
            this.nIndices = 36;
            this.nPrimitives = nIndices / 3;

            vertexBuffer = new VertexBuffer(typeof(CustomVertex.PositionNormalTextured), nVertices, Framework.Instance.Device, Usage.WriteOnly, CustomVertex.PositionNormalTextured.Format, Pool.Default);
            vertexBuffer.Created += new EventHandler(OnCreateVertexBuffer);
            this.OnCreateVertexBuffer(vertexBuffer, null);
        }

        void OnCreateVertexBuffer(object sender, EventArgs e)
        {
            CustomVertex.PositionNormalTextured[] vertices = (CustomVertex.PositionNormalTextured[])vertexBuffer.Lock(0, 0); // Lock the buffer (which will return our structs)
            indexBuffer = new IndexBuffer(typeof(short), nIndices, Framework.Instance.Device, Usage.WriteOnly, Pool.Managed);
            short[] indices = {
                0 , 1, 2,  1, 3, 2,  4, 5, 6,  5, 7, 6, 
                8 , 9,10,  9,11,10,  12,13,14, 13,15,14,
                16,17,18,  17,19,18, 20,21,22, 21,23,22 };

            Vector3 P1 = new Vector3(-size, size, -size);
            Vector3 P2 = new Vector3(size, size, -size);
            Vector3 P3 = new Vector3(-size, -size, -size);
            Vector3 P4 = new Vector3(size, -size, -size);
            Vector3 P5 = new Vector3(-size, size, size);
            Vector3 P6 = new Vector3(size, size, size);
            Vector3 P7 = new Vector3(-size, -size, size);
            Vector3 P8 = new Vector3(size, -size, size);

            // front
            vertices[0].Position = P1; vertices[0].Normal = new Vector3(0.0f, 0.0f, -1.0f);
            vertices[0].Tu = 0.0f; vertices[0].Tv = 0.0f;

            vertices[1].Position = P2; vertices[1].Normal = new Vector3(0.0f, 0.0f, -1.0f);
            vertices[1].Tu = 1.0f; vertices[1].Tv = 0.0f;

            vertices[2].Position = P3; vertices[2].Normal = new Vector3(0.0f, 0.0f, -1.0f);
            vertices[2].Tu = 0.0f; vertices[2].Tv = 1.0f;

            vertices[3].Position = P4; vertices[3].Normal = new Vector3(0.0f, 0.0f, -1.0f);
            vertices[3].Tu = 1.0f; vertices[3].Tv = 1.0f;

            // back
            vertices[4].Position = P5; vertices[4].Normal = new Vector3(0.0f, 0.0f, 1.0f);
            vertices[4].Tu = 1.0f; vertices[4].Tv = 0.0f;

            vertices[5].Position = P7; vertices[5].Normal = new Vector3(0.0f, 0.0f, 1.0f);
            vertices[5].Tu = 1.0f; vertices[5].Tv = 1.0f;

            vertices[6].Position = P6; vertices[6].Normal = new Vector3(0.0f, 0.0f, 1.0f);
            vertices[6].Tu = 0.0f; vertices[6].Tv = 0.0f;

            vertices[7].Position = P8; vertices[7].Normal = new Vector3(0.0f, 0.0f, 1.0f);
            vertices[7].Tu = 0.0f; vertices[7].Tv = 1.0f;

            // up
            vertices[8].Position = P5; vertices[8].Normal = new Vector3(0.0f, 1.0f, 0.0f);
            vertices[8].Tu = 0.0f; vertices[8].Tv = 0.0f;

            vertices[9].Position = P6; vertices[9].Normal = new Vector3(0.0f, 1.0f, 0.0f);
            vertices[9].Tu = 1.0f; vertices[9].Tv = 0.0f;

            vertices[10].Position = P1; vertices[10].Normal = new Vector3(0.0f, 1.0f, 0.0f);
            vertices[10].Tu = 0.0f; vertices[10].Tv = 1.0f;

            vertices[11].Position = P2; vertices[11].Normal = new Vector3(0.0f, 1.0f, 0.0f);
            vertices[11].Tu = 1.0f; vertices[11].Tv = 1.0f;

            // down
            vertices[12].Position = P7; vertices[12].Normal = new Vector3(0.0f, -1.0f, 0.0f);
            vertices[12].Tu = 0.0f; vertices[12].Tv = 1.0f;

            vertices[13].Position = P3; vertices[13].Normal = new Vector3(0.0f, -1.0f, 0.0f);
            vertices[13].Tu = 0.0f; vertices[13].Tv = 0.0f;

            vertices[14].Position = P8; vertices[14].Normal = new Vector3(0.0f, -1.0f, 0.0f);
            vertices[14].Tu = 1.0f; vertices[14].Tv = 1.0f;

            vertices[15].Position = P4; vertices[15].Normal = new Vector3(0.0f, -1.0f, 0.0f);
            vertices[15].Tu = 1.0f; vertices[15].Tv = 0.0f;

            // right
            vertices[16].Position = P2; vertices[16].Normal = new Vector3(1.0f, 0.0f, 0.0f);
            vertices[16].Tu = 0.0f; vertices[16].Tv = 0.0f;

            vertices[17].Position = P6; vertices[17].Normal = new Vector3(1.0f, 0.0f, 0.0f);
            vertices[17].Tu = 1.0f; vertices[17].Tv = 0.0f;

            vertices[18].Position = P4; vertices[18].Normal = new Vector3(1.0f, 0.0f, 0.0f);
            vertices[18].Tu = 0.0f; vertices[18].Tv = 1.0f;

            vertices[19].Position = P8; vertices[19].Normal = new Vector3(1.0f, 0.0f, 0.0f);
            vertices[19].Tu = 1.0f; vertices[19].Tv = 1.0f;

            // left
            vertices[20].Position = P1; vertices[20].Normal = new Vector3(-1.0f, 0.0f, 0.0f);
            vertices[20].Tu = 1.0f; vertices[20].Tv = 0.0f;

            vertices[21].Position = P3; vertices[21].Normal = new Vector3(-1.0f, 0.0f, 0.0f);
            vertices[21].Tu = 1.0f; vertices[21].Tv = 1.0f;

            vertices[22].Position = P5; vertices[22].Normal = new Vector3(-1.0f, 0.0f, 0.0f);
            vertices[22].Tu = 0.0f; vertices[22].Tv = 0.0f;

            vertices[23].Position = P7; vertices[23].Normal = new Vector3(-1.0f, 0.0f, 0.0f);
            vertices[23].Tu = 0.0f; vertices[23].Tv = 1.0f;

            vertexBuffer.Unlock();
            indexBuffer.SetData(indices, 0, LockFlags.None);
        }
        #endregion

        #region IFrameworkCallback Members
        public override void OnFrameMove(Device device, double totalTime, float elapsedTime)
        {

        }

        public override void OnFrameRender(Device device, double totalTime, float elapsedTime)
        {
            if (texture != null)
            {
                device.SetTexture(0, texture.Texture);
                if (texture.Transparent)
                {
                    // Reset states
                    device.RenderState.AlphaBlendEnable = true;
                    device.RenderState.SourceBlend = Blend.SourceAlpha;
                    device.RenderState.DestinationBlend = Blend.InvSourceAlpha;

                    device.TextureState[0].ColorOperation = TextureOperation.Modulate;
                    device.TextureState[0].ColorArgument0 = TextureArgument.Diffuse;
                    device.TextureState[0].ColorArgument1 = TextureArgument.TextureColor;
                    device.TextureState[0].ColorArgument2 = TextureArgument.Diffuse;
                    device.TextureState[0].AlphaOperation = TextureOperation.Modulate;
                    device.TextureState[0].AlphaArgument0 = TextureArgument.Diffuse;
                    device.TextureState[0].AlphaArgument1 = TextureArgument.TextureColor;
                    device.TextureState[0].AlphaArgument2 = TextureArgument.Diffuse;

                }
                else
                {
                    device.RenderState.AlphaBlendEnable = false;
                    //device.TextureState[0].AlphaOperation = TextureOperation.Disable;
                }
            }

            //device.RenderState.CullMode = Cull.None;
            // Turn off D3D lighting
            device.RenderState.Lighting = true;
            // Turn on the ZBuffer
            device.RenderState.ZBufferEnable = true;

            device.SetStreamSource(0, vertexBuffer, 0);
            device.VertexFormat = CustomVertex.PositionNormalTextured.Format;
            device.Indices = indexBuffer;
            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, nVertices, 0, nPrimitives);

            //device.RenderState.CullMode = Cull.CounterClockwise;
            // Turn on the ZBuffer
            device.RenderState.ZBufferEnable = true;
            // Turn on D3D lighting
            device.RenderState.Lighting = true;
            // Turn off transparency
            device.RenderState.AlphaBlendEnable = false;
        }
        #endregion
    }
}
