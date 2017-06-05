using System;
using System.Collections.Generic;
using Bonsai.Core;
using Bonsai.Core.Interfaces;
using Bonsai.Objects.Meshes;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Bonsai.Objects.Cameras
{
    public class CameraBase : IFrameworkCallback
    {
        #region Protected fields
        protected string name = string.Empty;
        protected Vector3 lookFrom = new Vector3(10f,10f,10f);
        protected Vector3 lookAt = new Vector3(0f,0f,0f);
        protected Matrix viewMatrix = Matrix.Identity;
        protected Matrix projectionMatrix = Matrix.Identity;
        protected Matrix reflectionMatrix = Matrix.Identity;
        protected float near = 1.0f;
        protected float far = 10000.0f;
        protected float fieldOfView = (float)Math.PI / 4;
        protected float zoomFactor = 1.0f; 
        protected float aspectRatio = 1.0f;
        protected Vector3 upDirection = new Vector3(0, 1, 0);
        protected Plane[] frustumPlanes = new Plane[6];
        protected float eyeSeperation = 0.05f;
        protected bool shake = false;
        protected double shakeStart = 0;
        protected double shakeDuration = 0;
        protected double shakeFrequency = 10.0;
        protected float shakeDisplacement = 0;
        protected bool reflected = false;
        #endregion

        #region Public properties
        /// <summary>
        /// Gets/Sets the name of the camera.
        /// </summary>
        public virtual string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Gets/Sets the vector from where to look.
        /// </summary>
        public Vector3 LookFrom
        {
            get { return lookFrom; }
            set { lookFrom = value; }
        }

        /// <summary>
        /// Gets/Sets the vector at which to look.
        /// </summary>
        public Vector3 LookAt
        {
            get { return lookAt; }
            set { lookAt = value; }
        }

        /// <summary>
        /// Gets the up direction.
        /// </summary>
        public Vector3 Up
        {
            get { return upDirection; }
            set { upDirection = value; }
        }

        /// <summary>
        /// Gets the direction to the left.
        /// </summary>
        public Vector3 Left
        {
            get 
            { 
                Vector3 left = Vector3.Cross(LookAt - LookFrom, Up);
                left.Normalize();
                return left; 
            }
        }

        /// <summary>
        /// Gets the current front facing unit vector.
        /// </summary>
        public Vector3 Front
        {
            get
            {
                Vector3 front = LookAt - LookFrom;
                front.Normalize();
                return front;
            }
        }

        /// <summary>
        /// Gets/Sets the aspectratio.
        /// </summary>
        public float AspectRatio
        {
            get { return aspectRatio; }
            set
            {
                aspectRatio = value;
                SetProjectionParameters(fieldOfView, aspectRatio, near, far);
            }
        }

        /// <summary>
        /// Gets/Sets the field of view.
        /// </summary>
        public float FieldOfView
        {
            get { return fieldOfView; }
            set
            {
                fieldOfView = value;
                SetProjectionParameters(fieldOfView, aspectRatio, near, far);
            }
        }

        /// <summary>
        /// Gets the ViewMatrix.
        /// </summary>
        public Matrix ViewMatrix
        {
            get 
            {
                if (reflected)
                    return reflectionMatrix;
                else
                    return viewMatrix; 
            }
        }

        /// <summary>
        /// Gets the ProjectionMatrix.
        /// </summary>
        public Matrix ProjectionMatrix
        {
            get { return projectionMatrix; }
        }

        /// <summary>
        /// Gets the reflection matrix.
        /// </summary>
        public Matrix ReflectionMatrix
        {
            get { return reflectionMatrix; }
        }
        

        /// <summary>
        /// Gets/Sets the zoom factor (default 1)
        /// </summary>
        public float ZoomFactor
        {
            get { return zoomFactor; }
            set 
            {
                if (value > 0.5f)
                {
                    zoomFactor = value;
                    SetProjectionParameters(fieldOfView, aspectRatio, near, far);
                }
                else
                {
                    zoomFactor = 0.5f;
                    SetProjectionParameters(fieldOfView, aspectRatio, near, far);
                }
            }
        }

        /// <summary>
        /// Gets the Near plane of the camera.
        /// </summary>
        public float Near
        {
            get { return near; }
            set
            {
                near = value;
                SetProjectionParameters(fieldOfView, aspectRatio, near, far);
            }
        }

        /// <summary>
        /// Gets the Far plane of the camera.
        /// </summary>
        public float Far
        {
            get { return far; }
            set
            {
                far = value;
                SetProjectionParameters(fieldOfView, aspectRatio, near, far);
            }
        }

        /// <summary>
        /// Gets the human readable name of the current camera mode.
        /// </summary>
        public virtual string CameraModeName
        {
            get { return ""; }
        }

        /// <summary>
        /// Gets/Sets the camera is used to calculate reflection
        /// </summary>
        public bool Reflected
        {
            get { return reflected; }
            set
            {
                if (value && !reflected)
                {
                    // recalculate reflection matrix
                    reflectionMatrix = CalculateReflectionMatrix();
                }
                reflected = value;
            }
        }

        /// <summary>
        /// Gets the direction in the horizontal plane in which the camera is pointing.
        /// </summary>
        public float Direction
        {
            get
            {
                return (float)Math.Atan2(Front.Z, Front.X);
            }
        }
        #endregion

        #region constructor
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name"></param>
        public CameraBase(string name)
        {
            this.name = name;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Sets the projection parameters.
        /// </summary>
        /// <param name="fieldOfView"></param>
        /// <param name="aspectRatio"></param>
        /// <param name="near"></param>
        /// <param name="far"></param>
        public virtual void SetProjectionParameters(
            float fieldOfView,
            float aspectRatio,
            float near,
            float far)
        {
            this.fieldOfView = fieldOfView;
            this.aspectRatio = aspectRatio;
            this.near = near;
            this.far = far;

            projectionMatrix = Matrix.PerspectiveFovLH(this.fieldOfView/zoomFactor, this.aspectRatio, this.near, this.far);
        }

        /// <summary>
        /// Switches the camera to the next mode.
        /// </summary>
        /// <returns></returns>
        public virtual string NextMode()
        {
            return "";
        }

        /// <summary>
        /// Determines whether a vector is within the view frustum.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool PointVisible(Vector3 point)
        {
            for (int i = 0; i < 6; i++)
            {
                if (frustumPlanes[i].Dot(point) < 0)
                    return false;
            }
            return true;
        }

        public float HorizonVisible(Vector3 location, float height)
        {
            Vector3 horizonPoint = new Vector3(location.X, height, location.Z);
            return frustumPlanes[5].Dot(horizonPoint); 
            float dot1 = frustumPlanes[4].Dot(horizonPoint);
            float dot2 = frustumPlanes[5].Dot(horizonPoint);
            if ((dot1 < 0) || (dot2 < 0))
                return Math.Min(dot1, dot2);
            else
                return Math.Max(dot1, dot2);
        }

        public void Shake(double currentTime, double duration, float displacement, double frequency)
        {
            shake = true;
            shakeStart = currentTime;
            shakeDuration = duration;
            shakeDisplacement = displacement;
            shakeFrequency = frequency;
        }

        public Matrix CalculateReflectionMatrix()
        {
            Vector3 reflectionLookFrom = new Vector3(lookFrom.X, -lookFrom.Y, lookFrom.Z);
            Vector3 reflectionLookAt = new Vector3(lookAt.X, -lookAt.Y, lookAt.Z);
            Vector3 reflectionLeft = new Vector3(Left.X, -Left.Y, Left.Z);
            Vector3 reflectionUp = Vector3.Cross(reflectionLeft, reflectionLookAt - reflectionLookFrom);
            reflectionUp.Normalize();
            return Matrix.LookAtLH(reflectionLookFrom, reflectionLookAt, reflectionUp);            
        }
        #endregion

        #region Protected methods
        /// <summary>
        /// Updates the view frustum
        /// </summary>
        /// <param name="device"></param>
        protected void UpdateFrustum( )
        {
            Matrix matrix;
            matrix = Matrix.Multiply(viewMatrix, projectionMatrix);
            // Near plane
            frustumPlanes[0].A = matrix.M14 + matrix.M13;
            frustumPlanes[0].B = matrix.M24 + matrix.M23;
            frustumPlanes[0].C = matrix.M34 + matrix.M33;
            frustumPlanes[0].D = matrix.M44 + matrix.M43;
            frustumPlanes[0].Normalize();
            // Far plane
            frustumPlanes[1].A = matrix.M14 - matrix.M13;
            frustumPlanes[1].B = matrix.M24 - matrix.M23;
            frustumPlanes[1].C = matrix.M34 - matrix.M33;
            frustumPlanes[1].D = matrix.M44 - matrix.M43;
            frustumPlanes[1].Normalize();
            // Left plane
            frustumPlanes[2].A = matrix.M14 + matrix.M11;
            frustumPlanes[2].B = matrix.M24 + matrix.M21;
            frustumPlanes[2].C = matrix.M34 + matrix.M31;
            frustumPlanes[2].D = matrix.M44 + matrix.M41;
            frustumPlanes[2].Normalize();
            // Right plane
            frustumPlanes[3].A = matrix.M14 - matrix.M11;
            frustumPlanes[3].B = matrix.M24 - matrix.M21;
            frustumPlanes[3].C = matrix.M34 - matrix.M31;
            frustumPlanes[3].D = matrix.M44 - matrix.M41;
            frustumPlanes[3].Normalize();
            // Top plane
            frustumPlanes[4].A = matrix.M14 - matrix.M12;
            frustumPlanes[4].B = matrix.M24 - matrix.M22;
            frustumPlanes[4].C = matrix.M34 - matrix.M32;
            frustumPlanes[4].D = matrix.M44 - matrix.M42;
            frustumPlanes[4].Normalize();
            // Bottom plane
            frustumPlanes[5].A = matrix.M14 + matrix.M12;
            frustumPlanes[5].B = matrix.M24 + matrix.M22;
            frustumPlanes[5].C = matrix.M34 + matrix.M32;
            frustumPlanes[5].D = matrix.M44 + matrix.M42;
            frustumPlanes[5].Normalize();
        }
        #endregion

        #region IFrameworkCallback Members
        public virtual void OnFrameMove(Device device, double totalTime, float elapsedTime)
        {
            if (shake)
            {
                double sinceStart = totalTime - shakeStart;
                if (sinceStart > shakeDuration)
                    shake = false;
                else
                {
                    LookAt += new Vector3(
                        shakeDisplacement * (float)Math.Sin(shakeFrequency * sinceStart),
                        shakeDisplacement * (float)Math.Sin(shakeFrequency*1.1 * sinceStart),
                        shakeDisplacement * (float)Math.Sin(shakeFrequency*1.2 * sinceStart));
                }
            }
            this.viewMatrix = Matrix.LookAtLH(lookFrom, lookAt, upDirection);
            this.projectionMatrix = Matrix.PerspectiveFovLH(this.fieldOfView/zoomFactor, this.aspectRatio, this.near, this.far);
            UpdateFrustum();
        }

        public virtual void OnFrameRender(Device device, double totalTime, float elapsedTime)
        {
            device.Transform.View = Matrix.LookAtLH(lookFrom, lookAt, upDirection);
            device.Transform.Projection = Matrix.PerspectiveFovLH(this.fieldOfView/zoomFactor, this.aspectRatio, this.near, this.far);
        }
        #endregion

        #region Anaglyph related methods
        public virtual void OnFrameRenderLeft(Device device, double totalTime, float elapsedTime)
        {
            this.viewMatrix = Matrix.LookAtLH(lookFrom + (eyeSeperation/2f) * Left, lookAt, upDirection);
            device.Transform.View = Matrix.LookAtLH(lookFrom + (eyeSeperation / 2f) * Left, lookAt, upDirection);
            device.Transform.Projection = Matrix.PerspectiveFovLH(this.fieldOfView / zoomFactor, this.aspectRatio, this.near, this.far);
        }

        public virtual void OnFrameRenderRight(Device device, double totalTime, float elapsedTime)
        {
            this.viewMatrix = Matrix.LookAtLH(lookFrom - (eyeSeperation / 2f) * Left, lookAt, upDirection);
            device.Transform.View = Matrix.LookAtLH(lookFrom - (eyeSeperation / 2f) * Left, lookAt, upDirection);
            device.Transform.Projection = Matrix.PerspectiveFovLH(this.fieldOfView / zoomFactor, this.aspectRatio, this.near, this.far);
        }
        #endregion
    }
}
