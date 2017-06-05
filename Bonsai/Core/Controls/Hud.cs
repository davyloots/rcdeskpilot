using System;
using System.Collections.Generic;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Bonsai.Core;
using Bonsai.Core.Interfaces;

namespace Bonsai.Core.Controls
{
    public class Hud : IDisposable, IFrameworkCallback
    {
        #region Private fields
        private Font font = null; // Font for drawing text
        private Sprite textSprite = null; // Sprite for batching text calls
        private bool showFPS = false;
        private TextHelper textHelper;
        private List<string> lines = new List<string>();
        #endregion

        #region Public properties
        /// <summary>
        /// Gets/Sets whether the framerate should be shown.
        /// </summary>
        public bool ShowFPS
        {
            get { return showFPS; }
            set { showFPS = value; }
        }

        /// <summary>
        /// Gets/sets the lines of text that will be displayed.
        /// </summary>
        public List<string> Lines
        {
            get { return lines; }
            set { lines = value; }
        }
        #endregion


        #region Constructor
        public Hud()
        {
            Device device = Framework.Instance.Device;

            // Initialize the stats font
            font = ResourceCache.GetGlobalInstance().CreateFont(device, 15, 0, FontWeight.Bold, 1, false, CharacterSet.Default,
                Precision.Default, FontQuality.Default, PitchAndFamily.FamilyDoNotCare | PitchAndFamily.DefaultPitch
                , "Arial");


            // Hook the device events
            device.DeviceLost += new EventHandler(OnLostDevice);
            device.DeviceReset += new EventHandler(OnResetDevice);
            device.Disposing += new EventHandler(OnDeviceDisposing);

            // Finally call reset
            OnResetDevice(device, System.EventArgs.Empty);
        }
        #endregion


        #region IDisposable members
        /// <summary>
        /// Clean up.
        /// </summary>
        public void Dispose()
        {
            if (textSprite != null)
            {
                textSprite.Dispose();
                textSprite = null;
            }
            if (font != null)
            {
                font.Dispose();
                font = null;
            }
        }
        #endregion
        
        #region Private event handlers
        /// <summary>Occurs after the device has been reset</summary>
        private void OnResetDevice(object sender, System.EventArgs e)
        {
            Device device = sender as Device;

            // Create a sprite to help batch calls when drawing many lines of text
            textSprite = new Sprite(device);
            textHelper = new TextHelper(font, textSprite, 15); 
        }

        /// <summary>Occurs before the device is going to be reset</summary>
        private void OnLostDevice(object sender, System.EventArgs e)
        {
            if (textSprite != null)
            {
                textSprite.Dispose();
                textSprite = null;
            }
        }

        /// <summary>Cleans up any resources required when this object is disposed</summary>
        private void OnDeviceDisposing(object sender, System.EventArgs e)
        {
            // Just dispose of our class
            Dispose();
        }
        #endregion

        #region IFrameworkCallback Members
        public void OnFrameMove(Device device, double totalTime, float elapsedTime)
        {
            
        }

        public void OnFrameRender(Device device, double totalTime, float elapsedTime)
        {            
            // Output statistics
            textHelper.Begin();
            textHelper.SetInsertionPoint(5, 5);
            textHelper.SetForegroundColor(System.Drawing.Color.Yellow);
            if (showFPS)
                textHelper.DrawTextLine(Framework.Instance.FPS.ToString("F"));
            foreach (string line in Lines)
                textHelper.DrawTextLine(line);
            textHelper.End();
        }
        #endregion
    }
}
