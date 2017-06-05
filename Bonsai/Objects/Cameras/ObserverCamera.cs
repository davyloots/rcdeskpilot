using System;
using System.Collections.Generic;
using Bonsai.Core;
using Bonsai.Core.Interfaces;
using Bonsai.Objects.Meshes;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Bonsai.Objects.Cameras
{
    public class ObserverCamera : CameraBase
    {
        #region Public enums
        public enum CameraModeEnum
        {
            Standard,
            AutoZoom,
            HorizonInView
        }
        #endregion

        #region Protected fields
        protected GameObject targetObject = null;
        protected float oldFieldOfView = 1.5f;
        protected CameraModeEnum cameraMode = CameraModeEnum.HorizonInView;
        #endregion

        #region Public properties
        /// <summary>
        /// Gets whether the camera should zoom in on the target object when distance increases.
        /// </summary>
        public bool AutoZoom
        {
            get 
            { 
                return CameraMode == CameraModeEnum.AutoZoom; 
            }           
        }

        public bool HorizonInView
        {
            get 
            { 
                return CameraMode == CameraModeEnum.HorizonInView; 
            }
        }

        public CameraModeEnum CameraMode
        {
            get { return cameraMode; }
            set
            {
                if ((cameraMode == CameraModeEnum.Standard) || (cameraMode == CameraModeEnum.HorizonInView))
                    oldFieldOfView = FieldOfView;
                cameraMode = value;
                if ((cameraMode == CameraModeEnum.Standard) || (cameraMode == CameraModeEnum.HorizonInView))
                    FieldOfView = oldFieldOfView;
            }
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
        /// Gets the human readable name for the current camera mode.
        /// </summary>
        public override string CameraModeName
        {
            get
            {
                switch (cameraMode)
                {
                    case CameraModeEnum.AutoZoom:
                        return "Autozoom";
                    case CameraModeEnum.HorizonInView:
                        return "Ground in view";
                    case CameraModeEnum.Standard:
                        return "Fixed zoom";
                }
                return "";
            }
        }
        #endregion

        #region Constructors
        public ObserverCamera(string name)
            : base(name)
        {
        }

        public ObserverCamera(string name, GameObject targetObject) 
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
            switch (cameraMode)
            {
                case CameraModeEnum.Standard:
                    CameraMode = CameraModeEnum.AutoZoom;
                    break;
                case CameraModeEnum.AutoZoom:
                    zoomFactor = 1.0f;
                    CameraMode = CameraModeEnum.HorizonInView;
                    break;
                case CameraModeEnum.HorizonInView:
                    CameraMode = CameraModeEnum.Standard;
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
                lookAt = targetObject.Position;
                switch (cameraMode)
                {
                    case CameraModeEnum.HorizonInView:
                        {
                            float distance = (lookAt - lookFrom).Length() + Math.Abs(lookAt.Y - lookFrom.Y);
                            float maxZoomFactor = 3f + distance / 30f;
                            lookAt = targetObject.Position + (0.1f / zoomFactor) * (new Vector3(0, -(float)distance, 0));
                            float dot = Math.Min(3, Math.Max(-3, HorizonVisible(lookAt, lookFrom.Y)));
                            zoomFactor += elapsedTime * dot;
                            if (zoomFactor < 0.8f)
                                zoomFactor = 0.8f;
                            else if (zoomFactor > maxZoomFactor)
                                zoomFactor = maxZoomFactor;
                            break;
                        }
                    case CameraModeEnum.AutoZoom:
                        {
                            Vector3 distanceVector = lookAt - lookFrom;
                            FieldOfView = (float)Math.PI / ((4 + distanceVector.Length() / 5.0f) * zoomFactor);
                            break;
                        }
                    default:
                        break;
                }
            }
            base.OnFrameMove(device, totalTime, elapsedTime);
        }
        #endregion

    }
}
