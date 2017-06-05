using System;
using System.Collections.Generic;
using System.Drawing;
using Bonsai.Core;
using Bonsai.Core.Interfaces;
using Bonsai.Objects.Meshes;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Bonsai.Objects.Lights
{
    public class LightBase : IFrameworkCallback, IDisposable
    {
        #region Protected fields
        protected Color color = Color.Gray;
        protected bool enabled = true;
        #endregion

        #region internal fields
        internal int lightNumber = 0;
        #endregion

        #region Public properties
        /// <summary>
        /// Gets/Sets the color of the light.
        /// </summary>
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        /// <summary>
        /// Gets/Sets whether the light is enabled.
        /// </summary>
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }
        #endregion

        #region Constructor
        public LightBase()
        {
            Framework.Instance.AddLight(this);
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            Framework.Instance.RemoveLight(this);
        }
        #endregion

        #region IFrameworkCallback Members
        /// <summary>
        /// Prepares a new frame.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="totalTime"></param>
        /// <param name="elapsedTime"></param>
        public virtual void OnFrameMove(Device device, double totalTime, float elapsedTime)
        {

        }

        /// <summary>
        /// Renders a new frame.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="totalTime"></param>
        /// <param name="elapsedTime"></param>
        public virtual void OnFrameRender(Device device, double totalTime, float elapsedTime)
        {
            
        }
        #endregion

        
    }
}
