using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Core.Interfaces;
using Microsoft.DirectX;

namespace RCSim
{
    internal class Towing : IFrameworkCallback, IDisposable
    {
        #region Protected members
        protected RecordedFlight towplane = null;
        #endregion

        #region Public properties
        public Vector3 Position
        {
            get
            {
                if (towplane != null)
                    return towplane.AirplaneModel.Position;
                else
                    return Vector3.Empty;
            }
        }
        public Vector3 Velocity
        {
            get
            {
                if (towplane != null)
                    return towplane.Velocity;
                else
                    return Vector3.Empty;
            }
        }

        public double Time
        {
            get 
            {
                if (towplane != null)
                    return towplane.Time;
                else
                    return 0;
            }
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            if (towplane != null)
                towplane.Playing = false;
            if (towplane != null)
            {
                towplane.Dispose();
                towplane = null;
            }
        }
        #endregion

        #region Public methods
        public void Start()
        {
            if (towplane != null)
            {
                towplane.Playing = false;
            }
            if (towplane == null)
            {
                towplane = new RecordedFlight(Program.Instance);
                towplane.FileName = "towing.dat";
                towplane.Stopped += new EventHandler(towplane_Stopped);
                towplane.Playing = true;
            }
        }

        void towplane_Stopped(object sender, EventArgs e)
        {
            if (towplane != null)
            {
                towplane.Dispose();
                towplane = null;
            }
        }
        #endregion

        #region IFrameworkCallback Members
        public void OnFrameMove(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            if (towplane != null)
                towplane.OnFrameMove(device, totalTime, elapsedTime);
        }

        public void OnFrameRender(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            if (towplane != null)
                towplane.OnFrameRender(device, totalTime, elapsedTime);
        }
        #endregion
    }
}
