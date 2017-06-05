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
    internal class CinematicCamera : CameraBase
    {
        #region Protected fields
        protected AirplaneModel airplane = null;
        protected Scenery scenery = null;
        protected float distance = 5.0f;
        protected CameraType currentCameraType = CameraType.FlyBy;
        protected CameraMode cameraMode = CameraMode.Switching;
        protected double startTime = 0;
        protected double typeStartTime = 0;
        #endregion

        #region protected enums
        public enum CameraType
        {
            FlyBy,
            Spot,
            TailCam,
            WingCam,
            TopCam,
            Last
        }

        public enum CameraMode
        {
            Fixed,
            Switching
        }
        #endregion

        #region Public properties
        /// <summary>
        /// Gets/Sets the current camera type.
        /// </summary>
        public CameraType CurrentCameraType
        {
            get { return currentCameraType; }
            set
            {
                switch (value)
                {
                    case CameraType.FlyBy:
                        StartFlyBy();
                        break;
                    case CameraType.Spot:
                        StartSpot();
                        break;
                    case CameraType.TailCam:
                        StartTailCam();
                        break;
                    case CameraType.WingCam:
                        StartWingCam();
                        break;
                    case CameraType.TopCam:
                        StartTopCam();
                        break;                    
                }
            }
        }

        /// <summary>
        /// Gets/Sets the current camera mode.
        /// </summary>
        public CameraMode CurrentCameraMode
        {
            get { return cameraMode; }
            set { cameraMode = value; }
        }

        /// <summary>
        /// Gets/sets the Airplane to track.
        /// </summary>
        public AirplaneModel Airplane
        {
            get { return airplane; }
            set { airplane = value; }
        }

        /// <summary>
        /// Gets/sets the scenery.
        /// </summary>
        public Scenery Scenery
        {
            get { return scenery; }
            set { scenery = value; }
        }

        /// <summary>
        /// Gets/sets the distance between the targetobject and the camera.
        /// </summary>
        public float Distance
        {
            get { return distance; }
            set { distance = value; }
        }

        /// <summary>
        /// Gets the name of the camera
        /// </summary>
        public override string Name
        {
            get 
            {
                if (cameraMode == CameraMode.Switching)
                    return "Cinematic camera";
                else
                    return "Tail camera";
            }
        }

        /// <summary>
        /// Gets the human readable name for the current camera mode.
        /// </summary>
        public override string CameraModeName
        {
            get
            {
                switch (currentCameraType)
                {
                    case CameraType.FlyBy:
                        return "fly by";
                    case CameraType.Spot:
                        return "spot camera";
                    case CameraType.TailCam:
                        return "tail cam";
                    case CameraType.TopCam:
                        return "top cam";
                    case CameraType.WingCam:
                        return "wing cam";
                }
                return "";
            }
        }
        #endregion

        #region Constructors
        public CinematicCamera(string name)
            : base(name)
        {
        }        
        #endregion

        #region Public methods
        public override string NextMode()
        {
            switch (currentCameraType)
            {
                case CameraType.FlyBy:
                    StartSpot();
                    break;
                case CameraType.Spot:
                    StartTailCam();
                    break;
                case CameraType.TailCam:
                    StartTopCam();
                    break;
                case CameraType.TopCam:
                    StartWingCam();
                    break;
                default:
                    StartFlyBy();
                    break;
            }
            typeStartTime = Program.Instance.CurrentTime;
            return CameraModeName;
        }
        #endregion

        #region IFrameworkCallback Members
        public override void OnFrameMove(Device device, double totalTime, float elapsedTime)
        {
            if (cameraMode == CameraMode.Switching)
            {
                if (startTime == 0)
                {
                    Start(totalTime);
                }
                CheckCameraSwitch(totalTime);
            }
            switch (currentCameraType)
            {
                case CameraType.FlyBy:
                    if (airplane != null)
                    {
                        lookAt = airplane.Position;
                    }
                    break;
                case CameraType.Spot:
                    if (airplane != null)
                    {
                        lookFrom = new Vector3(
                        airplane.Position.X + distance * (float)Math.Sin(totalTime / 4.0),
                        airplane.Position.Y + 1f,
                        airplane.Position.Z + distance * (float)Math.Cos(totalTime / 4.0));
                        lookAt = airplane.Position;
                    }
                    break;
                case CameraType.TailCam:
                    if (airplane != null)
                    {
                        lookFrom = airplane.Position - 0.8f*airplane.Front * airplane.BoundingBoxMax.Z + 1.4f * airplane.Up * airplane.BoundingBoxMax.Y;
                        lookAt = airplane.Position + airplane.Front;
                        this.upDirection = airplane.Up;
                    }
                    break;
                case CameraType.WingCam:
                    if (airplane != null)
                    {
                        lookFrom = airplane.Position + 1.1f * airplane.BoundingBoxMax.X * Vector3.Cross(airplane.Front, airplane.Up) - 0.1f * airplane.BoundingBoxMax.Y * airplane.Up;
                        lookAt = airplane.Position;
                        this.upDirection = airplane.Up;
                    }
                    break;
                case CameraType.TopCam:
                    if (Airplane != null)
                    {
                        lookFrom = airplane.Position + 3.0f*Vector3.Normalize(airplane.Position);
                        lookAt = airplane.Position;
                    }
                    break;
            }           
            
            base.OnFrameMove(device, totalTime, elapsedTime);
        }
        #endregion

        #region private methods
        private void Start(double startTime)
        {
            this.startTime = startTime;
            this.typeStartTime = startTime;
            StartTopCam();
        }

        private void CheckCameraSwitch(double totalTime)
        {
            switch (currentCameraType)
            {
                case CameraType.FlyBy:
                case CameraType.Spot:
                case CameraType.WingCam:
                case CameraType.TopCam:
                    if (totalTime - typeStartTime > 4.0)
                        SwitchCameraType(totalTime);
                    break;
                case CameraType.TailCam:
                    if (totalTime - typeStartTime > 8.0)
                        SwitchCameraType(totalTime);
                    break;
            }
        }

        private void SwitchCameraType(double totalTime)
        {
            Random rnd = new Random();            
            switch (rnd.Next((int)(CameraType.Last)))
            {
                case 0:
                    StartFlyBy();
                    break;
                case 1:
                    StartSpot();
                    break;
                case 2:
                    StartTailCam();
                    break;
                case 3:
                    StartWingCam();
                    break;
                case 4:
                    StartTopCam();
                    break;
                default:
                    StartSpot();
                    break;
            }
            typeStartTime = totalTime;
        }

        private void StartFlyBy()
        {
            this.Near = 0.5f;
            this.FieldOfView = (float)Math.PI / 4.5f;
            currentCameraType = CameraType.FlyBy;
            this.upDirection = new Vector3(0, 1, 0);
            if (airplane != null)
            {
                lookFrom = airplane.Position + 10*airplane.Front + 3*Vector3.Cross(airplane.Front, airplane.Up);
                if (scenery != null)
                {
                    float height = scenery.Heightmap.GetHeightAt(lookFrom.X, lookFrom.Z);
                    if (lookFrom.Y < height)
                    {
                        lookFrom = new Vector3(lookFrom.X, height + 0.2f, lookFrom.Z);
                    }
                }
            }
        }

        private void StartSpot()
        {
            this.Near = 0.5f;
            this.FieldOfView = (float)Math.PI / 4.5f;
            currentCameraType = CameraType.Spot;
            this.upDirection = new Vector3(0, 1, 0);            
        }

        private void StartTailCam()
        {
            this.Near = 0.1f;
            this.FieldOfView = (float)Math.PI / 3.2f;
            currentCameraType = CameraType.TailCam;
        }
        
        private void StartWingCam()
        {
            this.Near = 0.1f;
            this.FieldOfView = (float)Math.PI / 3.2f;
            currentCameraType = CameraType.WingCam;
        }

        private void StartTopCam()
        {
            this.Near = 1.0f;
            this.FieldOfView = (float)Math.PI / 6.0f;
            currentCameraType = CameraType.TopCam;
            this.upDirection = new Vector3(0, 1, 0);
        }
        #endregion

    }
}