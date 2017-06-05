using System;
using System.Collections.Generic;
using Bonsai.Core;
using Bonsai.Core.Interfaces;
using Bonsai.Objects.Meshes;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Bonsai.Objects.Cameras
{
    public class SpotCamera : CameraBase
    {
        #region Public enums
        public enum CameraModeEnum
        {
            Spot,
            Follow
        }
        #endregion

        #region Protected fields
        protected GameObject targetObject = null;
        protected float spotDistance = 3.0f;
        protected float followDistance = 5.0f;
        protected CameraModeEnum cameraMode = CameraModeEnum.Follow;
        #endregion

        #region Public properties
        /// <summary>
        /// Gets/sets the distance between the targetobject and the camera in spot mode.
        /// </summary>
        public float SpotDistance
        {
            get { return spotDistance; }
            set { spotDistance = value; }
        }

        /// <summary>
        /// Gets/sets the distance between the targetobject and the camera in follow mode.
        /// </summary>
        public float FollowDistance
        {
            get { return followDistance; }
            set { followDistance = value; }
        }

        /// <summary>
        /// Gets/Sets the object that is tracked.
        /// </summary>
        public GameObject TargetObject
        {
            get { return targetObject; }
            set { targetObject = value; }
        }

        /// <summary>
        /// Gets/Sets the current camera mode
        /// </summary>
        public CameraModeEnum CameraMode
        {
            get { return cameraMode; }
            set
            {
                cameraMode = value;
            }
        }

        
        /// <summary>
        /// Gets the human readable name for the current camera mode.
        /// </summary>
        public override string CameraModeName
        {
            get
            {
                switch (cameraMode)
                {
                    case CameraModeEnum.Follow:
                        return "Follow";
                    case CameraModeEnum.Spot:
                        return "Spot";
                }
                return "";
            }
        }
        #endregion

        #region Constructors
        public SpotCamera(string name)
            : base(name)
        {
        }

        public SpotCamera(string name, GameObject targetObject)
            : base(name)
        {
            this.targetObject = targetObject;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Switches the camera to the next mode.
        /// </summary>
        /// <returns></returns>
        public override string NextMode()
        {
            switch (CameraMode)
            {
                case CameraModeEnum.Follow:
                    CameraMode = CameraModeEnum.Spot;
                    break;
                case CameraModeEnum.Spot:
                    CameraMode = CameraModeEnum.Follow;
                    break;
            }
            return CameraModeName;
        }
        #endregion

        #region IFrameworkCallback Members
        public override void OnFrameMove(Device device, double totalTime, float elapsedTime)
        {
            if (targetObject != null)
            {
                switch (cameraMode)
                {
                    case CameraModeEnum.Spot:
                        {
                            lookFrom = new Vector3(
                                targetObject.Position.X + spotDistance * (float)Math.Sin(totalTime / 4.0),
                                targetObject.Position.Y + 1f,
                                targetObject.Position.Z + spotDistance * (float)Math.Cos(totalTime / 4.0));
                            lookAt = targetObject.Position;
                            break;
                        }
                    case CameraModeEnum.Follow:
                        {
                            float vectorLength = (float)Math.Sqrt((double)(targetObject.Front.X * targetObject.Front.X +
                                targetObject.Front.Z * targetObject.Front.Z));
                            lookFrom = new Vector3(targetObject.Position.X - followDistance*targetObject.Front.X/(vectorLength + 0.01f),
                                targetObject.Position.Y + 1,
                                targetObject.Position.Z - followDistance * targetObject.Front.Z / (vectorLength + 0.01f));
                            lookAt = targetObject.Position;
                            break;
                        }
                }
            }
            base.OnFrameMove(device, totalTime, elapsedTime);
        }
        #endregion

    }
}