using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Bonsai.Core.Controls;

namespace Bonsai.Core.Dialogs
{
    public class DialogBase
    {
        #region protected fields
        protected Dialog dialog; // Dialog that will be rendered
        protected StateBlock state; // state block for device
        protected Framework parent; // Parent framework for this dialog
        protected uint windowWidth; // Width of window
        protected uint windowHeight; // Height of window
        protected bool centered = true; // whether the dialog is centered on the screen.
        #endregion

        #region Public properties
        /// <summary>
        /// Gets/Sets wether the dialog is centered on the screen.
        /// </summary>
        public bool Centered
        {
            get { return centered; }
            set { centered = value; }
        }
        #endregion

        #region Constructor
        public DialogBase(Framework parent)
        {
            this.parent = parent;

            dialog = new Dialog(parent);
            dialog.IsUsingKeyboardInput = true;
            dialog.SetFont(0, "Arial", 15, FontWeight.Normal);
            dialog.SetFont(1, "Arial", 28, FontWeight.Bold);
            dialog.SetFont(2, "Arial", 22, FontWeight.Bold);
            dialog.SetFont(3, "Arial", 16, FontWeight.Normal);
            dialog.SetFont(4, "Arial", 12, FontWeight.Normal);

            windowWidth = Framework.DefaultSizeWidth; windowHeight = Framework.DefaultSizeHeight;

            if (centered)
                Center();
        }
        #endregion

        /// <summary>Hand messages off to dialog</summary>
        public void HandleMessages(IntPtr hWnd, NativeMethods.WindowMessage msg, IntPtr wParam, IntPtr lParam)
        {
            dialog.MessageProc(hWnd, msg, wParam, lParam);
        }
        
        /// <summary>Render the dialog</summary>
        public void OnRender(float elapsedTime)
        {
            try
            {
                //state.Capture();
                //parent.Device.RenderState.FillMode = FillMode.Solid;
                dialog.OnRender(elapsedTime);
                //state.Apply();
            }
            catch
            {
            }
        }

        public virtual void OnShowDialog()
        {
            Dialog.SetRefreshTime((float)FrameworkTimer.GetTime());
            if (centered)
                Center();
        }

        /// <summary>
        /// Called when the device is created
        /// </summary>
        public virtual void OnCreateDevice(Device d)
        {

        }



        /// <summary>
        /// Called when the device is reset
        /// </summary>
        public virtual void OnResetDevice()
        {
            SurfaceDescription desc = parent.BackBufferSurfaceDescription;

            // Set up the dialog
            dialog.SetLocation(0, 0);
            dialog.SetSize(desc.Width, desc.Height);
            
            dialog.SetBackgroundColors(new ColorValue((float)98 / 255, (float)138 / 255, (float)206 / 255),
                new ColorValue((float)54 / 255, (float)105 / 255, (float)192 / 255),
                new ColorValue((float)54 / 255, (float)105 / 255, (float)192 / 255),
                new ColorValue((float)10 / 255, (float)73 / 255, (float)179 / 255));

            if (centered)
                Center();

            //Device device = parent.Device;
            //device.BeginStateBlock();
            //device.RenderState.FillMode = FillMode.Solid;
            //state = device.EndStateBlock();
        }

        /// <summary>
        /// Called when the device is lost
        /// </summary>
        public virtual void OnLostDevice()
        {
            if (state != null)
                state.Dispose();

            state = null;
        }

        /// <summary>Destroy any resources</summary>
        public virtual void OnDestroyDevice(object sender, System.EventArgs e)
        {
            // Clear the focus
            Dialog.ClearFocus();
        }

        #region Private methods
        private void Center()
        {
            int ox = (Framework.Instance.BackBufferSurfaceDescription.Width - (int)windowWidth) / 2;
            int oy = (Framework.Instance.BackBufferSurfaceDescription.Height - (int)windowHeight) / 2;
            dialog.SetLocation(ox, oy);
            dialog.SetSize(Framework.DefaultSizeWidth, Framework.DefaultSizeHeight);
        }
        #endregion
    }
}
