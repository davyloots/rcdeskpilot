using System;
using System.Collections.Generic;
using Bonsai.Core;
using Bonsai.Core.Interfaces;
using Bonsai.Objects.Meshes;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Bonsai.Objects.Lights
{
    public class DirectionalLight : LightBase
    {
        #region Protected fields
        protected Vector3 direction = new Vector3(0, -1f, 0);
        #endregion

        #region Public properties
        /// <summary>
        /// Gets/Sets the direction of the light.
        /// </summary>
        public Vector3 Direction
        {
            get { return direction; }
            set { direction = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor, default direction (down).
        /// </summary>
        public DirectionalLight()
        {
        }

        /// <summary>
        /// Constructs a directional light with given direction.
        /// </summary>
        /// <param name="direction"></param>
        public DirectionalLight(Vector3 direction)
        {
            this.Direction = direction;
        }
        #endregion

        #region Overridden DirectionalLight methods
        public override void OnFrameMove(Device device, double totalTime, float elapsedTime)
        {
            base.OnFrameMove(device, totalTime, elapsedTime);
        }

        public override void OnFrameRender(Device device, double totalTime, float elapsedTime)
        {
            device.Lights[lightNumber].Type = LightType.Directional;
            device.Lights[lightNumber].Diffuse = base.color;
            device.Lights[lightNumber].Direction = direction;
            device.Lights[lightNumber].Enabled = enabled;
            device.Lights[lightNumber].Update();
            base.OnFrameRender(device, totalTime, elapsedTime);
        }
        #endregion
    }
}
