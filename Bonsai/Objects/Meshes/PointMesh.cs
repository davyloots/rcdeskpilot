using System;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Bonsai.Core;
using Bonsai.Objects.Textures;
using System.Drawing;

namespace Bonsai.Objects.Meshes
{
    public class PointMesh : MeshBase, IDisposable
    {
        #region Private fields
        private VertexBuffer vertexBuffer = null;
        private IndexBuffer indexBuffer = null;
        private TextureBase texture = null;
        private int nVertices;
        private int nIndices;
        private int nPrimitives;
        private float size;
        private Vector3 normalVector = new Vector3(0, 0, -1.0f);
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

        public Vector3 Position
        {
            set
            {
                /*
                CustomVertex.PositionTextured[] vertices = (CustomVertex.PositionTextured[])vertexBuffer.Lock(0, 0);
                vertices[0].Position = value;
                vertexBuffer.Unlock();
                 */
            }
        }
        #endregion

        #region Constructor
        public PointMesh(float size)
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
            this.nVertices = 1;
            this.nIndices = 1;
            this.nPrimitives = 1;

            vertexBuffer = new VertexBuffer(typeof(CustomVertex.PositionColored), nVertices, Framework.Instance.Device, Usage.WriteOnly, CustomVertex.PositionColored.Format, Pool.Default);
            vertexBuffer.Created += new EventHandler(OnCreateVertexBuffer);
            this.OnCreateVertexBuffer(vertexBuffer, null);
        }

        void OnCreateVertexBuffer(object sender, EventArgs e)
        {
            CustomVertex.PositionColored[] vertices = (CustomVertex.PositionColored[])vertexBuffer.Lock(0, 0); // Lock the buffer (which will return our structs)
            indexBuffer = new IndexBuffer(typeof(short), nIndices, Framework.Instance.Device, Usage.WriteOnly, Pool.Managed);
            short[] indices = new short[nIndices];

            vertices[0].Position = new Vector3();
            vertices[0].Color = Color.White.ToArgb();
            indices[0] = 0;
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
                    device.RenderState.SourceBlend = Blend.SourceColor;
                    device.RenderState.DestinationBlend = Blend.One;
                    
                    
                    // Reset states
                    //device.RenderState.AlphaBlendEnable = true;
                    //device.RenderState.SourceBlend = Blend.SourceAlpha;
                    //device.RenderState.DestinationBlend = Blend.InvSourceAlpha;
                    
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
            device.RenderState.Lighting = false;
            // Turn on the ZBuffer
            device.RenderState.ZBufferEnable = false;
            device.RenderState.PointSize = (int)(device.Viewport.Width * size);
            device.RenderState.PointSpriteEnable = true;
            device.SetStreamSource(0, vertexBuffer, 0);
            device.VertexFormat = CustomVertex.PositionColored.Format;
            device.Indices = indexBuffer;            
            device.DrawPrimitives(PrimitiveType.PointList, 0, 1);

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
