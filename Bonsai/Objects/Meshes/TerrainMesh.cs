using System;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Bonsai.Core;
using Bonsai.Objects.Textures;
using Bonsai.Objects.Terrain;

namespace Bonsai.Objects.Meshes
{
    public class TerrainMesh : MeshBase, IDisposable
    {
        #region Private fields
        private VertexBuffer vertexBuffer = null;
        private IndexBuffer indexBuffer = null;
        private TextureBase texture = null;
        private int nVertices;
        private int nIndices;
        private int nPrimitives;
        private float size;
        private int xSubdivisions = 1;
        private int ySubdivisions = 1;
        private float textureScale = 1.0f;
        private Heightmap heightmap = null;
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
        public TerrainMesh(float size, int xSubdivisions, int ySubdivisions, float textureScale)
        {
            this.size = size;
            this.xSubdivisions = xSubdivisions;
            this.ySubdivisions = ySubdivisions;
            if (textureScale > 0)
                this.textureScale = textureScale;
            GenerateVertices();

            Framework.Instance.DeviceCreated += new Bonsai.Core.EventArgs.DeviceEventHandler(Instance_DeviceCreated);
            Framework.Instance.DeviceLost += new EventHandler(Instance_DeviceLost);
            Framework.Instance.DeviceReset += new Bonsai.Core.EventArgs.DeviceEventHandler(Instance_DeviceReset);
        }

        public TerrainMesh(float textureScale, Heightmap heightmap)
        {
            this.heightmap = heightmap;
            this.size = heightmap.Size/2;
            this.xSubdivisions = heightmap.XSubdivisions;
            this.ySubdivisions = heightmap.YSubdivisions;
            if (textureScale > 0)
                this.textureScale = textureScale;
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
            this.nVertices = (xSubdivisions + 1) * (ySubdivisions + 1);
            this.nIndices = 2 * 3 * xSubdivisions * ySubdivisions;
            this.nPrimitives = nIndices / 3;

            vertexBuffer = new VertexBuffer(typeof(CustomVertex.PositionNormalTextured), nVertices, Framework.Instance.Device, Usage.WriteOnly, CustomVertex.PositionNormalTextured.Format, Pool.Default);
            vertexBuffer.Created += new EventHandler(OnCreateVertexBuffer);
            this.OnCreateVertexBuffer(vertexBuffer, null);
        }



        void OnCreateVertexBuffer(object sender, EventArgs e)
        {
            CustomVertex.PositionNormalTextured[] vertices = (CustomVertex.PositionNormalTextured[])vertexBuffer.Lock(0, 0); // Lock the buffer (which will return our structs)
            indexBuffer = new IndexBuffer(typeof(short), nIndices, Framework.Instance.Device, Usage.WriteOnly, Pool.Managed);
            short[] indices = new short[nIndices];

            // Compute vertices
            int pos = 0;
            for (int row = 0; row < 1 + ySubdivisions; row++)
            {
                for (int col = 0; col < 1 + xSubdivisions; col++)
                {
                    float x = -size + (size * 2 / xSubdivisions) * col;
                    float z = size - (size * 2 / ySubdivisions) * row;
                    float y = 0;
                    Vector3 normal = new Vector3(0, 1f, 0);
                    if (heightmap != null)
                    {
                        y = heightmap.GetHeightAt(row, col);
                        normal = heightmap.GetNormalAt(row, col);
                    }
                    vertices[pos].Position = new Vector3(x, y, z);
                    vertices[pos].Normal = normal;
                    vertices[pos].Tu = (textureScale / xSubdivisions) * col;
                    vertices[pos].Tv = (textureScale / ySubdivisions) * row;
                    pos++;
                }
            }
            // Compute indices
            pos = 0;
            for (int row = 0; row < ySubdivisions; row++)
            {
                for (int col = 0; col < xSubdivisions; col++)
                {
                    indices[pos] = (short)((row * (1 + xSubdivisions)) + col);
                    indices[pos + 1] = (short)((row * (1 + xSubdivisions)) + col + 1);
                    indices[pos + 2] = (short)(((row + 1) * (1 + xSubdivisions)) + col);
                    indices[pos + 3] = (short)((row * (1 + xSubdivisions)) + col + 1);
                    indices[pos + 4] = (short)(((row + 1) * (1 + xSubdivisions)) + col + 1);
                    indices[pos + 5] = (short)(((row + 1) * (1 + xSubdivisions)) + col);
                    pos += 6;
                }
            }
            
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

            device.RenderState.CullMode = Cull.CounterClockwise;
            // Turn off D3D lighting
            device.RenderState.Lighting = true;
            // Turn on the ZBuffer
            device.RenderState.ZBufferEnable = true;

            device.SetStreamSource(0, vertexBuffer, 0);
            device.VertexFormat = CustomVertex.PositionNormalTextured.Format;
            device.Indices = indexBuffer;
            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, nVertices, 0, nPrimitives);

            device.RenderState.CullMode = Cull.CounterClockwise;
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
