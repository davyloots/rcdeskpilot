using System;
using System.Collections.Generic;
using Bonsai.Core;
using Bonsai.Core.Interfaces;
using Bonsai.Objects.Meshes;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Bonsai.Objects.Cameras
{
    public class CockpitCamera : CameraBase
    {
        #region Protected fields
        protected GameObject targetObject = null;
        #endregion

        #region Public properties
        /// <summary>
        /// Gets/Sets the object that is tracked.
        /// </summary>
        public GameObject TargetObject
        {
            get { return targetObject; }
            set { targetObject = value; }
        }
        #endregion

        #region Constructors
        public CockpitCamera(string name)
            : base(name)
        {
        }

        public CockpitCamera(string name, GameObject targetObject)
            : base(name)
        {
            this.targetObject = targetObject;
        }
        #endregion

        #region IFrameworkCallback Members
        public override void OnFrameMove(Device device, double totalTime, float elapsedTime)
        {
            if (targetObject != null)
            {
                lookFrom = targetObject.Position;
                lookAt = targetObject.Position + targetObject.Front;
                this.upDirection = targetObject.Up;
            }
            base.OnFrameMove(device, totalTime, elapsedTime);
        }
        #endregion

    }
}