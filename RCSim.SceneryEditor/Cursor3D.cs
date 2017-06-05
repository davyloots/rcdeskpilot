using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Objects;
using Bonsai.Objects.Meshes;
using Bonsai.Core;
using Microsoft.DirectX;

namespace RCSim
{
    internal class Cursor3D : GameObject
    {
        #region Private fields
        private bool[] keys;
        private Vector3 velocity;
        private float moveScaler = 1000.0f;
        private bool rightButtonDown = false;

        #endregion

        #region Public properties
        public bool Enabled { get; set; }
        #endregion

        #region Public enums
        public enum CursorKeys : byte
        {
            StrafeLeft,
            StrafeRight,
            MoveForward,
            MoveBackward,
            MoveUp,
            MoveDown,
            Reset,
            Slow,
            MaxKeys,
            Unknown = 0xff
        }
        #endregion

        #region Constructor
        public Cursor3D()
        {
            CubeMesh mesh = new CubeMesh(10f);
            mesh.Texture = new Bonsai.Objects.Textures.TextureBase("data\\cursor.png");
            mesh.Texture.Transparent = true;
            this.Mesh = mesh;
            keys = new bool[(int)CursorKeys.MaxKeys];
        }
        #endregion

        #region Overridden GameObject methods
        public override void OnFrameMove(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            if (Enabled)
            {
                if (!rightButtonDown)
                {
                    UpdateVelocity(elapsedTime);
                    this.Position += velocity * elapsedTime;
                }
                base.OnFrameMove(device, totalTime, elapsedTime);
            }
        }

        public override void OnFrameRender(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            if (Enabled)
                base.OnFrameRender(device, totalTime, elapsedTime);
        }
        #endregion

        #region Public methods
        public void HandleMessages(IntPtr hWnd, NativeMethods.WindowMessage msg, IntPtr wParam, IntPtr lParam)
        {
            switch (msg)
            {
                // Handle the keyboard
                case NativeMethods.WindowMessage.KeyDown:
                    CursorKeys mappedKeyDown = MapKey(wParam);
                    if (mappedKeyDown != (CursorKeys)byte.MaxValue)
                    {
                        // Valid key was pressed, mark it as 'down'
                        keys[(int)mappedKeyDown] = true;
                    }
                    break;
                case NativeMethods.WindowMessage.KeyUp:
                    CursorKeys mappedKeyUp = MapKey(wParam);
                    if (mappedKeyUp != (CursorKeys)byte.MaxValue)
                    {
                        // Valid key was let go, mark it as 'up'
                        keys[(int)mappedKeyUp] = false;
                    }
                    break;
                case NativeMethods.WindowMessage.RightButtonDown:
                    rightButtonDown = true;
                    break;
                case NativeMethods.WindowMessage.RightButtonUp:
                    rightButtonDown = false;
                    break;
            }
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Maps NativeMethods.WindowMessage.Key* msg to a cursor key
        /// </summary>
        protected static CursorKeys MapKey(IntPtr param)
        {
            System.Windows.Forms.Keys key = (System.Windows.Forms.Keys)param.ToInt32();
            switch (key)
            {
                case System.Windows.Forms.Keys.ShiftKey: return CursorKeys.Slow;
                case System.Windows.Forms.Keys.Left: return CursorKeys.StrafeLeft;
                case System.Windows.Forms.Keys.Right: return CursorKeys.StrafeRight;
                case System.Windows.Forms.Keys.Up: return CursorKeys.MoveForward;
                case System.Windows.Forms.Keys.Down: return CursorKeys.MoveBackward;
                case System.Windows.Forms.Keys.Prior: return CursorKeys.MoveUp; // pgup
                case System.Windows.Forms.Keys.Next: return CursorKeys.MoveDown; // pgdn
                case System.Windows.Forms.Keys.Home: return CursorKeys.Reset;

                case System.Windows.Forms.Keys.Q: return CursorKeys.StrafeLeft;
                case System.Windows.Forms.Keys.D: return CursorKeys.StrafeRight;
                case System.Windows.Forms.Keys.Z: return CursorKeys.MoveForward;
                case System.Windows.Forms.Keys.S: return CursorKeys.MoveBackward;
                case System.Windows.Forms.Keys.A: return CursorKeys.MoveUp;
                case System.Windows.Forms.Keys.E: return CursorKeys.MoveDown;

                case System.Windows.Forms.Keys.NumPad4: return CursorKeys.StrafeLeft;
                case System.Windows.Forms.Keys.NumPad6: return CursorKeys.StrafeRight;
                case System.Windows.Forms.Keys.NumPad8: return CursorKeys.MoveForward;
                case System.Windows.Forms.Keys.NumPad2: return CursorKeys.MoveBackward;
                case System.Windows.Forms.Keys.NumPad9: return CursorKeys.MoveUp;
                case System.Windows.Forms.Keys.NumPad3: return CursorKeys.MoveDown;
            }
            // No idea
            return (CursorKeys)byte.MaxValue;
        }

        protected void UpdateVelocity(float elapsedTime)
        {
            Vector3 accel = Vector3.Empty;

            // Update acceleration vector based on keyboard state
            if (keys[(int)CursorKeys.MoveForward])
                accel.Z += 1.0f;
            if (keys[(int)CursorKeys.MoveBackward])
                accel.Z -= 1.0f;
            if (keys[(int)CursorKeys.MoveUp])
                accel.Y += 1.0f;
            if (keys[(int)CursorKeys.MoveDown])
                accel.Y -= 1.0f;
            if (keys[(int)CursorKeys.StrafeRight])
                accel.X += 1.0f;
            if (keys[(int)CursorKeys.StrafeLeft])
                accel.X -= 1.0f;
            // Normalize vector so if moving 2 dirs (left & forward), 
            // the camera doesn't move faster than if moving in 1 dir
            accel.Normalize();
            // Scale the acceleration vector
            accel *= moveScaler;
            if (keys[(int)CursorKeys.Slow])
                accel *= 0.1f;
            velocity = accel;
        }
        #endregion
    }
}
