using System;
using System.Collections.Generic;
using System.Drawing;
using Bonsai.Core;
using Bonsai.Core.Interfaces;
using Bonsai.Objects.Meshes;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Bonsai.Objects.Cameras
{
    public class FirstPersonCamera : CameraBase
    {
        #region Protected fields
        protected Point lastMousePosition = Point.Empty;
        protected Vector2 mouseDelta;
        protected bool resetCursorAfterMove = false;
        protected float framesToSmoothMouseData = 2.0f;
        protected float rotationScaler = 0.01f;
        protected float moveScaler = 5.0f; 
        protected Vector2 rotationVelocity;
        protected Vector3 velocity; 
        protected bool[] keys;
        protected bool invertPitch = false;
        protected float cameraYawAngle;      // Yaw angle of camera
        protected float cameraPitchAngle;    // Pitch angle of camera
        protected bool rightMouseDown = false;
        #endregion

        #region Public enums
        public enum CameraKeys : byte
        {
            StrafeLeft,
            StrafeRight,
            MoveForward,
            MoveBackward,
            MoveUp,
            MoveDown,
            MoveSlowly,
            Reset,
            MaxKeys,
            Unknown = 0xff
        }
        #endregion

        #region Public properties
        /// <summary>
        /// Gets/sets whether the cursor should be reset every frame.
        /// </summary>
        public bool ResetCursorAfterMove
        {
            get { return resetCursorAfterMove; }
            set { resetCursorAfterMove = value; }
        }

        /// <summary>
        /// Gets/Sets the maximum speed.
        /// </summary>
        public float MoveScaler
        {
            get { return moveScaler; }
            set { moveScaler = value; }
        }
        #endregion

        #region Constructors
        public FirstPersonCamera(string name)
            : base(name)
        {
            keys = new bool[(int)CameraKeys.MaxKeys];
        }       
        #endregion

        #region IFrameworkCallback Members
        public override void OnFrameMove(Device device, double totalTime, float elapsedTime)
        {
            if (!rightMouseDown)
            {
                base.OnFrameMove(device, totalTime, elapsedTime);
                return;
            }
            if (keys[(int)CameraKeys.Reset])
                SetViewParameters(lookFrom, new Vector3(0,0,0));
            UpdateMouseDelta(elapsedTime);
            UpdateVelocity(elapsedTime);

            // Simple euler method to calculate position delta
            Vector3 posDelta = velocity * elapsedTime;
                     

            // Update the pitch & yaw angle based on mouse movement
            float yawDelta = rotationVelocity.X;
            float pitchDelta = rotationVelocity.Y;

            // Invert pitch if requested
            if (invertPitch)
                pitchDelta = -pitchDelta;

            cameraPitchAngle += pitchDelta;
            cameraYawAngle += yawDelta;

           

            // Limit pitch to straight up or straight down
            cameraPitchAngle = Math.Max(-(float)Math.PI / 2.0f, cameraPitchAngle);
            cameraPitchAngle = Math.Min(+(float)Math.PI / 2.0f, cameraPitchAngle);

            // Make a rotation matrix based on the camera's yaw & pitch
            Matrix cameraRotation = Matrix.RotationYawPitchRoll(cameraYawAngle, cameraPitchAngle, 0);

            // Transform vectors based on camera's rotation matrix
            Vector3 localUp = new Vector3(0, 1, 0);
            Vector3 localAhead = new Vector3(0, 0, 1);
            Vector3 worldUp = Vector3.TransformCoordinate(localUp, cameraRotation);
            Vector3 worldAhead = Vector3.TransformCoordinate(localAhead, cameraRotation);

            // Transform the position delta by the camera's rotation 
            Vector3 posDeltaWorld = Vector3.TransformCoordinate(posDelta, cameraRotation);

            // Move the eye position 
            lookFrom += posDeltaWorld;

            // Update the lookAt position based on the eye position 
            lookAt = lookFrom + worldAhead;
            /*
            // Update the view matrix
            viewMatrix = Matrix.LookAtLH(eye, lookAt, worldUp);
            cameraWorld = Matrix.Invert(viewMatrix);
            */
            base.OnFrameMove(device, totalTime, elapsedTime);
        }
        #endregion

        #region Protected methods
        protected void UpdateMouseDelta(float elapsedTime)
        {
            Point current = System.Windows.Forms.Cursor.Position;
            if (lastMousePosition == Point.Empty)
                lastMousePosition = current;
            Point delta = new Point(current.X - lastMousePosition.X, current.Y - lastMousePosition.Y);
            if (resetCursorAfterMove)
            {
                System.Windows.Forms.Screen activeScreen = System.Windows.Forms.Screen.PrimaryScreen;
                Point center = new Point(activeScreen.Bounds.Width / 2, activeScreen.Bounds.Height / 2);
                System.Windows.Forms.Cursor.Position = center;
                lastMousePosition = center;
            }

            float percentOfNew = 1.0f / framesToSmoothMouseData;
            float percentOfOld = 1.0f - percentOfNew;
            mouseDelta.X = mouseDelta.X * percentOfOld + delta.X * percentOfNew;
            mouseDelta.Y = mouseDelta.Y * percentOfOld + delta.Y * percentOfNew;
            rotationVelocity = mouseDelta * rotationScaler;
            lastMousePosition = current;
        }

        protected void UpdateVelocity(float elapsedTime)
        {
            Vector3 accel = Vector3.Empty;

            // Update acceleration vector based on keyboard state
            if (keys[(int)CameraKeys.MoveForward])
                accel.Z += 1.0f;
            if (keys[(int)CameraKeys.MoveBackward])
                accel.Z -= 1.0f;
            if (keys[(int)CameraKeys.MoveUp])
                accel.Y += 1.0f;
            if (keys[(int)CameraKeys.MoveDown])
                accel.Y -= 1.0f;
            if (keys[(int)CameraKeys.StrafeRight])
                accel.X += 1.0f;
            if (keys[(int)CameraKeys.StrafeLeft])
                accel.X -= 1.0f;
            // Normalize vector so if moving 2 dirs (left & forward), 
            // the camera doesn't move faster than if moving in 1 dir
            accel.Normalize();
            // Scale the acceleration vector
            if (keys[(int)CameraKeys.MoveSlowly])
                accel *= moveScaler * 0.2f;
            else
                accel *= moveScaler;
            velocity = accel;
        }

        /// <summary>
        /// Maps NativeMethods.WindowMessage.Key* msg to a camera key
        /// </summary>
        protected static CameraKeys MapKey(IntPtr param)
        {
            System.Windows.Forms.Keys key = (System.Windows.Forms.Keys)param.ToInt32();
            switch (key)
            {
                case System.Windows.Forms.Keys.Left: return CameraKeys.StrafeLeft;
                case System.Windows.Forms.Keys.Right: return CameraKeys.StrafeRight;
                case System.Windows.Forms.Keys.Up: return CameraKeys.MoveForward;
                case System.Windows.Forms.Keys.Down: return CameraKeys.MoveBackward;
                case System.Windows.Forms.Keys.Prior: return CameraKeys.MoveUp; // pgup
                case System.Windows.Forms.Keys.Next: return CameraKeys.MoveDown; // pgdn
                case System.Windows.Forms.Keys.Home: return CameraKeys.Reset;

                case System.Windows.Forms.Keys.Q: return CameraKeys.StrafeLeft;
                case System.Windows.Forms.Keys.D: return CameraKeys.StrafeRight;
                case System.Windows.Forms.Keys.Z: return CameraKeys.MoveForward;
                case System.Windows.Forms.Keys.S: return CameraKeys.MoveBackward;
                case System.Windows.Forms.Keys.A: return CameraKeys.MoveUp;
                case System.Windows.Forms.Keys.E: return CameraKeys.MoveDown;

                case System.Windows.Forms.Keys.ShiftKey: return CameraKeys.MoveSlowly;

                case System.Windows.Forms.Keys.NumPad4: return CameraKeys.StrafeLeft;
                case System.Windows.Forms.Keys.NumPad6: return CameraKeys.StrafeRight;
                case System.Windows.Forms.Keys.NumPad8: return CameraKeys.MoveForward;
                case System.Windows.Forms.Keys.NumPad2: return CameraKeys.MoveBackward;
                case System.Windows.Forms.Keys.NumPad9: return CameraKeys.MoveUp;
                case System.Windows.Forms.Keys.NumPad3: return CameraKeys.MoveDown;
            }
            // No idea
            return (CameraKeys)byte.MaxValue;
        }
        #endregion

        public void HandleMessages(IntPtr hWnd, NativeMethods.WindowMessage msg, IntPtr wParam, IntPtr lParam)
        {
            switch (msg)
            {
                // Handle the keyboard
                case NativeMethods.WindowMessage.KeyDown:
                    CameraKeys mappedKeyDown = MapKey(wParam);
                    if (mappedKeyDown != (CameraKeys)byte.MaxValue)
                    {
                        // Valid key was pressed, mark it as 'down'
                        keys[(int)mappedKeyDown] = true;
                    }
                    break;
                case NativeMethods.WindowMessage.KeyUp:
                    CameraKeys mappedKeyUp = MapKey(wParam);
                    if (mappedKeyUp != (CameraKeys)byte.MaxValue)
                    {
                        // Valid key was let go, mark it as 'up'
                        keys[(int)mappedKeyUp] = false;
                    }
                    break;
                case NativeMethods.WindowMessage.RightButtonDown:
                    if (!rightMouseDown)
                        lastMousePosition = System.Windows.Forms.Cursor.Position;
                    rightMouseDown = true;
                    break;
                case NativeMethods.WindowMessage.RightButtonUp:
                    rightMouseDown = false;
                    break;
            }
        }

        /// <summary>
        /// Client can call this to change the position and direction of camera
        /// </summary>
        public virtual void SetViewParameters(Vector3 eyePt, Vector3 lookAtPt)
        {
            // Store the data
            lookAt = lookAtPt;

            // Calculate the view matrix
            viewMatrix = Matrix.LookAtLH(lookFrom, lookAt, upDirection);

            // Get the inverted matrix
            Matrix inverseView = Matrix.Invert(viewMatrix);

            // The axis basis vectors and camera position are stored inside the 
            // position matrix in the 4 rows of the camera's world matrix.
            // To figure out the yaw/pitch of the camera, we just need the Z basis vector
            Vector3 pZBasis = new Vector3(inverseView.M31, inverseView.M32, inverseView.M33);
            cameraYawAngle = (float)Math.Atan2(pZBasis.X, pZBasis.Z);
            float len = (float)Math.Sqrt(pZBasis.Z * pZBasis.Z + pZBasis.X * pZBasis.X);
            cameraPitchAngle = -(float)Math.Atan2(pZBasis.Y, len);
        }
    }
}
