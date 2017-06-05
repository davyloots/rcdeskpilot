using System;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Bonsai.Core;
using Bonsai.Objects.Textures;

namespace Bonsai.Objects.Meshes
{
    public class DomeMesh : MeshBase, IDisposable
    {
        #region Private fields
        private VertexBuffer vertexBuffer = null;
        private IndexBuffer indexBuffer = null;
        private TextureBase texture = null;
        private float radius = 1.0f;
        private int rings = 2;
        private int segments = 8;
        private int nVertices;
        private int nIndices;
        private int nPrimitives;
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
        public DomeMesh(float radius, int rings, int segments)
        {
            this.radius = radius;
            this.rings = rings;
            this.segments = segments;
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
            this.nVertices = (rings + 1) * (segments + 1);
            this.nIndices = 2 * rings * (segments + 1);
            this.nPrimitives = nIndices - 2;

            vertexBuffer = new VertexBuffer(typeof(CustomVertex.PositionNormalTextured), nVertices, Framework.Instance.Device, Usage.WriteOnly, CustomVertex.PositionNormalTextured.Format, Pool.Default);
            vertexBuffer.Created += new EventHandler(OnCreateVertexBuffer);
            this.OnCreateVertexBuffer(vertexBuffer, null);
        }

        

        void OnCreateVertexBuffer(object sender, EventArgs e)
        {
            CustomVertex.PositionNormalTextured[] vertices = (CustomVertex.PositionNormalTextured[])vertexBuffer.Lock(0, 0); // Lock the buffer (which will return our structs)
            indexBuffer = new IndexBuffer(typeof(short), nIndices, Framework.Instance.Device, Usage.WriteOnly, Pool.Managed);
            short[] indices = new short[nIndices];
            float fDeltaRingAngle = ((float)Math.PI / 2 / rings);
            float fDeltaSegAngle = (2.0f * (float)Math.PI / segments);

            int vertexPos = 0;
            int indexPos = 0;
            short verticeIndex = 0;
            float r0;
            float y0;
            float x0;
            float z0;

            // Generate the group of rings for the dome
            for (int ring = 0; ring < rings + 1; ring++)
            {
                r0 = (float)Math.Sin(ring * fDeltaRingAngle);
                y0 = (float)Math.Cos(ring * fDeltaRingAngle);

                // Generate the group of segments for the current ring
                for (int seg = 0; seg < segments + 1; seg++)
                {
                    x0 = r0 * (float)Math.Sin(seg * fDeltaSegAngle);
                    z0 = r0 * (float)Math.Cos(seg * fDeltaSegAngle);

                    // Add one vertices to the strip which makes up the dome
                    vertices[vertexPos].Position = new Vector3(x0 * radius, y0 * radius, z0 * radius);
                    vertices[vertexPos].Normal = Vector3.Normalize(vertices[vertexPos].Position);
                    vertices[vertexPos].Tu = (float)seg / (float)segments;
                    vertices[vertexPos].Tv = (float)ring / (float)rings;

                    vertexPos++;
                                       
                    // add two indices except for last ring 
                    if (ring != rings)
                    {
                        indices[indexPos] = verticeIndex;
                        indices[indexPos + 1] = (short)(verticeIndex + (segments + 1));
                        verticeIndex++;
                        indexPos += 2;
                    }
                } // end for seg 
            } // end for ring 
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
                device.TextureState[0].ColorOperation = TextureOperation.Modulate;
                device.TextureState[0].ColorArgument1 = TextureArgument.TextureColor;
                device.TextureState[0].ColorArgument2 = TextureArgument.Diffuse;
                device.TextureState[0].AlphaOperation = TextureOperation.Disable;
            }
            
            device.RenderState.CullMode = Cull.None;
            // Turn off D3D lighting
            device.RenderState.Lighting = false;
            // Turn on the ZBuffer
            device.RenderState.ZBufferEnable = true;

            device.SetStreamSource(0, vertexBuffer, 0);
            device.VertexFormat = CustomVertex.PositionNormalTextured.Format;
            device.Indices = indexBuffer;
            device.DrawIndexedPrimitives(PrimitiveType.TriangleStrip, 0, 0, nVertices, 0, nPrimitives);

            device.RenderState.CullMode = Cull.CounterClockwise;
            // Turn off D3D lighting
            device.RenderState.Lighting = true;
            // Turn on the ZBuffer
            device.RenderState.ZBufferEnable = true;
        }
        #endregion
    }
}
