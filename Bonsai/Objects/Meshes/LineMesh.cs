using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Bonsai.Core;
using System.Drawing;

namespace Bonsai.Objects.Meshes
{
    public class LineMesh : MeshBase, IDisposable
    {
        #region Private fields
        private VertexBuffer vertexBuffer = null;
        private Vector3 vertex1 = new Vector3();
        private Vector3 vertex2 = new Vector3();
        private Material material = new Material();
        #endregion

        #region Public properties
        public Vector3 Vertex1
        {
            get { return vertex1; }
            set
            {
                vertex1 = value;
                if (vertexBuffer != null)
                {
                    CustomVertex.PositionColored[] vertices = (CustomVertex.PositionColored[])vertexBuffer.Lock(0, 0); // Lock the buffer (which will return our structs)
                    vertices[0].Position = vertex1;
                    vertices[1].Position = vertex2;
                    vertexBuffer.Unlock();
                }
            }            
        }

        public Vector3 Vertex2
        {
            get { return vertex2; }
            set
            {
                vertex2 = value;
                if (vertexBuffer != null)
                {
                    CustomVertex.PositionColored[] vertices = (CustomVertex.PositionColored[])vertexBuffer.Lock(0, 0); // Lock the buffer (which will return our structs)
                    vertices[0].Position = vertex1;
                    vertices[1].Position = vertex2;
                    vertexBuffer.Unlock();
                }
            }
        }
        #endregion

        #region Constructor
        public LineMesh()
        {
            GenerateVertices();
            material.Ambient = Color.White;
            material.Diffuse = Color.White;
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
        }

        void Instance_DeviceCreated(object sender, Bonsai.Core.EventArgs.DeviceEventArgs e)
        {
            GenerateVertices();
        }
        #endregion

        #region Private methods
        private void GenerateVertices()
        {
            vertexBuffer = new VertexBuffer(typeof(CustomVertex.PositionColored), 2, Framework.Instance.Device, Usage.WriteOnly | Usage.Dynamic, CustomVertex.PositionColored.Format, Pool.Default);
            vertexBuffer.Created += new EventHandler(OnCreateVertexBuffer);
            this.OnCreateVertexBuffer(vertexBuffer, null);
        }

        void OnCreateVertexBuffer(object sender, EventArgs e)
        {
            CustomVertex.PositionColored[] vertices = (CustomVertex.PositionColored[])vertexBuffer.Lock(0, 0); // Lock the buffer (which will return our structs)
            vertices[0].Position = vertex1;
            vertices[0].Color = (int)(0x6FFFFFFF);
            vertices[1].Position = vertex2; 
            vertices[1].Color = (int)(0x6FFFFFFF);
            vertexBuffer.Unlock();
        }
        #endregion

        #region Public methods
        public void OnParticleFrameRender(Device device, double totalTime, float elapsedTime)
        {
            device.SetTexture(0, null);

            //device.RenderState.CullMode = Cull.None;
            // Turn off D3D lighting
            device.RenderState.Lighting = false;
            // Turn on the ZBuffer
            device.RenderState.ZBufferEnable = true;

            device.SetStreamSource(0, vertexBuffer, 0);
            device.VertexFormat = CustomVertex.PositionColored.Format;
            device.Material = material;
            device.DrawPrimitives(PrimitiveType.LineList, 0, 1);

            //device.RenderState.CullMode = Cull.CounterClockwise;
            // Turn on the ZBuffer
            device.RenderState.ZBufferEnable = true;
            // Turn on D3D lighting
            device.RenderState.Lighting = true;
            // Turn off transparency
            device.RenderState.AlphaBlendEnable = false;
        }
        #endregion

        #region IFrameworkCallback Members
        public override void OnFrameMove(Device device, double totalTime, float elapsedTime)
        {

        }

        public override void OnFrameRender(Device device, double totalTime, float elapsedTime)
        {            
            device.SetTexture(0, null);

            //device.RenderState.CullMode = Cull.None;
            // Turn off D3D lighting
            device.RenderState.Lighting = false;
            // Turn on the ZBuffer
            device.RenderState.ZBufferEnable = true;

            device.VertexFormat = CustomVertex.PositionColored.Format;
            device.SetStreamSource(0, vertexBuffer, 0);            
            device.DrawPrimitives(PrimitiveType.LineList, 0, 1);

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
