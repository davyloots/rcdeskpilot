using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Core.Interfaces;

namespace RCSim
{
    internal class ComputerPilots : IFrameworkCallback, IDisposable
    {
        #region Private fields
        private Program owner = null;
        private Random random = new Random();
        private List<RecordedFlight> recordedFlights = new List<RecordedFlight>();
        #endregion

        #region Public properties
        public int NumberOfPilots
        {
            get { return recordedFlights.Count; }
            set
            {
                while (recordedFlights.Count > value)
                    RemovePilot();
                while (recordedFlights.Count < value)
                    AddPilot();
            }
        }
        #endregion

        #region Constructor
        public ComputerPilots(Program owner)
        {
            this.owner = owner;
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Clean up.
        /// </summary>
        public void Dispose()
        {
            while (recordedFlights.Count > 0)
                RemovePilot();
        }
        #endregion

        #region Private methods
        private void AddPilot()
        {
            RecordedFlight recordedFlight = CreateFlight();
            recordedFlights.Add(recordedFlight);
        }

        private RecordedFlight CreateFlight()
        {
            RecordedFlight recordedFlight = new RecordedFlight(owner);
            string filename = "";
            bool done = false;
            while (!done)
            {
                filename = string.Format("flight{0}.dat", random.Next(4).ToString());
                done = true;
                foreach (RecordedFlight flight in recordedFlights)
                {
                    if (flight.FileName.Equals(filename))
                        done = false;
                }
            }
            recordedFlight.FileName = filename;
            recordedFlight.Stopped += new EventHandler(recordedFlight_Stopped);
            recordedFlight.Playing = true;
            return recordedFlight;
        }

        private void DisposeFlight(RecordedFlight flight)
        {
            flight.Stopped -= new EventHandler(recordedFlight_Stopped);
            flight.Dispose();
        }

        private void RemovePilot()
        {
            RecordedFlight recordedFlight = recordedFlights[recordedFlights.Count - 1];
            recordedFlights.Remove(recordedFlight);
            recordedFlight.Stopped -= new EventHandler(recordedFlight_Stopped);
            recordedFlight.Dispose();
        }

        private void RemovePilot(RecordedFlight flight)
        {
            recordedFlights.Remove(flight);
            DisposeFlight(flight);
        }
        #endregion

        #region Private event handlers
        /// <summary>
        /// Handles the Stopped event of a recorded flight
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void recordedFlight_Stopped(object sender, EventArgs e)
        {
            RecordedFlight flight = sender as RecordedFlight;
            if (flight != null)
            {
                int index = recordedFlights.IndexOf(flight);
                if (index >= 0)
                {
                    recordedFlights[index] = CreateFlight();
                }
                DisposeFlight(flight);
            }
        }
        #endregion

        #region IFrameworkCallback Members
        public void OnFrameMove(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            for (int i = 0; i < recordedFlights.Count; i++)
            {
                RecordedFlight flight = recordedFlights[i];
                flight.OnFrameMove(device, totalTime, elapsedTime);
            }
        }

        public void OnFrameRender(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            foreach (RecordedFlight flight in recordedFlights)
                flight.OnFrameRender(device, totalTime, elapsedTime);

        }
        #endregion
    }
}
