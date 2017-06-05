using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Core.Interfaces;

namespace RCSim
{
    internal class Weather : IFrameworkCallback, IDisposable
    {
        #region Private fields
        private Wind wind = null;
        private Program owner = null;
        #endregion

        #region Public properties
        /// <summary>
        /// Gets the wind.
        /// </summary>
        public Wind Wind
        {
            get { return wind; }
        }
        #endregion

        #region Constructor
        public Weather(Program owner)
        {
            this.owner = owner;
            this.wind = new Wind(owner);
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Clean up.
        /// </summary>
        public void Dispose()
        {
            if (wind != null)
                wind.Dispose();
        }
        #endregion

        #region IFrameworkCallback Members
        /// <summary>
        /// Prepares the next frame.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="totalTime"></param>
        /// <param name="elapsedTime"></param>
        public void OnFrameMove(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {            
            wind.OnFrameMove(device, totalTime, elapsedTime);
        }

        /// <summary>
        /// Renders the current frame.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="totalTime"></param>
        /// <param name="elapsedTime"></param>
        public void OnFrameRender(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            wind.OnFrameRender(device, totalTime, elapsedTime);
        }
        #endregion

        
    }
}
