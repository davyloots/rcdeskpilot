using System;
using System.Collections.Generic;
using Bonsai.Core;
using Bonsai.Core.Interfaces;
using Bonsai.Objects.Meshes;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Bonsai.Objects.Cameras
{
    public class FollowCamera : CameraBase
    {
        #region Protected fields
        protected GameObject targetObject = null;
        protected float distance = 5.0f;
        #endregion

        #region Public properties
        /// <summary>
        /// Gets/sets the distance between the targetobject and the camera.
        /// </summary>
        public float Distance
        {
            get { return distance; }
            set { distance = value; }
        }

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
        public FollowCamera(string name)
            : base(name)
        {
        }

        public FollowCamera(string name, GameObject targetObject)
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
                float vectorLength = (float)Math.Sqrt((double)(targetObject.Front.X * targetObject.Front.X +
                    targetObject.Front.Z * targetObject.Front.Z));
                lookFrom = new Vector3(targetObject.Position.X - distance*targetObject.Front.X/(vectorLength + 0.01f),
                    targetObject.Position.Y + 1,
                    targetObject.Position.Z - distance * targetObject.Front.Z / (vectorLength + 0.01f));
                lookAt = targetObject.Position;
            }
            base.OnFrameMove(device, totalTime, elapsedTime);
        }
        #endregion

    }
}