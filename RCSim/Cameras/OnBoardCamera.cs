using System;
using System.Collections.Generic;
using Bonsai.Core;
using Bonsai.Core.Interfaces;
using Bonsai.Objects.Meshes;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Bonsai.Objects.Cameras;
using Bonsai.Objects;

namespace RCSim.Cameras
{
    internal class OnBoardCamera : CameraBase
    {
        #region Public enums
        public enum CameraModeEnum
        {
            Cockpit,
            Tail,
            Wing
        }
        #endregion

        #region Protected fields
        protected AirplaneModel airplane = null;
        protected float distance = 5.0f;
        protected CameraModeEnum cameraMode = CameraModeEnum.Cockpit;
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
        /// Gets/sets the Airplane to track.
        /// </summary>
        public AirplaneModel Airplane
        {
            get { return airplane; }
            set { airplane = value; }
        }

        public CameraModeEnum CameraMode
        {
            get { return cameraMode; }
            set
            {
                cameraMode = value;
                switch (cameraMode)
                {
                    case CameraModeEnum.Cockpit:
                        this.Near = 0.1f;
                        this.FieldOfView = (float)Math.PI / 4;
                        break;
                    case CameraModeEnum.Tail:
                        this.Near = 0.1f;
                        this.FieldOfView = (float)Math.PI / 3.2f;
                        break;
                    case CameraModeEnum.Wing:
                        this.Near = 0.1f;
                        this.FieldOfView = (float)Math.PI / 4.2f;
                        break;
                }
            }
        }

        public override string CameraModeName
        {
            get
            {
                switch (cameraMode)
                {
                    case CameraModeEnum.Cockpit:
                        return "cockpit";
                    case CameraModeEnum.Tail:
                        return "tail cam";
                    case CameraModeEnum.Wing:
                        return "wing cam";
                }
                return "";
            }
        }
        #endregion

        #region Constructors
        public OnBoardCamera(string name)
            : base(name)
        {
            this.Near = 0.1f;
            this.FieldOfView = (float)Math.PI / 4;
        }

        public OnBoardCamera(string name, AirplaneModel airplane)
            : base(name)
        {
            this.airplane = airplane;
            this.Near = 0.1f;            
            this.FieldOfView = (float)Math.PI / 4;
        }
        #endregion

        #region Public methods
        public override string NextMode()
        {
            switch (cameraMode)
            {
                case CameraModeEnum.Cockpit:                    
                    CameraMode = CameraModeEnum.Tail;
                    break;
                case CameraModeEnum.Tail:
                    CameraMode = CameraModeEnum.Wing;
                    break;
                case CameraModeEnum.Wing:
                    CameraMode = CameraModeEnum.Cockpit;
                    break;
            }
            return CameraModeName;
        }
        #endregion

        #region IFrameworkCallback Members
        public override void OnFrameMove(Device device, double totalTime, float elapsedTime)
        {            
            if (airplane != null)
            {
                switch (cameraMode)
                {
                    case CameraModeEnum.Cockpit:
                        lookFrom = airplane.Position;
                        lookAt = airplane.Position + airplane.Front;
                        this.upDirection = airplane.Up;
                        break;
                    case CameraModeEnum.Tail:
                        lookFrom = airplane.Position - 0.8f * airplane.Front * airplane.BoundingBoxMax.Z + 1.4f * airplane.Up * airplane.BoundingBoxMax.Y;
                        lookAt = airplane.Position + airplane.Front;
                        this.upDirection = airplane.Up;
                        break;
                    case CameraModeEnum.Wing:
                        lookFrom = airplane.Position + 1.1f * airplane.BoundingBoxMax.X * Vector3.Cross(airplane.Front, airplane.Up) - 0.1f * airplane.BoundingBoxMax.Y * airplane.Up;
                        lookAt = airplane.Position;
                        this.upDirection = airplane.Up;
                        break;
                }
            }
            base.OnFrameMove(device, totalTime, elapsedTime);
        }
        #endregion

    }
}
