using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Core.Interfaces;
using System.IO;
using Microsoft.DirectX.Direct3D;
using RCSim.DataClasses;


namespace RCSim
{
    class FlightRecorder : IFrameworkCallback
    {
        #region Private fields
        private bool recording = false;
        private string fileName = string.Empty;
        private FileStream file = null;
        private BinaryWriter binaryWriter = null;
        private double startTime = 0;
        private double lastRecord = 0;
        private double recorderInterval = 0.05;
        private AirplaneState state = new AirplaneState();
        private Program owner = null;
        #endregion

        #region Public properties
        /// <summary>
        /// Gets/Sets whether the recorder is recording.
        /// </summary>
        public bool Recording
        {
            get { return recording; }
            set 
            {
                if (value)
                    StartRecording();
                else
                    StopRecording();
            }
        }

        /// <summary>
        /// Gets/Sets the airplane.
        /// </summary>
        public AirplaneModel Airplane
        {
            get { return owner.Player.Airplane; }
        }

        /// <summary>
        /// Gets/Sets the recording filename.
        /// </summary>
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }
        #endregion

        #region Constructor
        public FlightRecorder(Program owner)
        {
            this.owner = owner;
        }
        #endregion

        #region Private methods
        private void StartRecording()
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                file = File.Create(fileName);
                binaryWriter = new BinaryWriter(file);
                string aircraftFile = owner.Player.AircraftParameters.FileName.ToLower();
                binaryWriter.Write(aircraftFile.Substring(aircraftFile.LastIndexOf("\\aircraft\\")+1));
                startTime = -1.0;
                lastRecord = -1.0;
                recording = true;
            }
        }

        private void StopRecording()
        {
            recording = false;
            startTime = -1;
            if (binaryWriter != null)
            {
                binaryWriter.Flush();
                binaryWriter.Close();
                binaryWriter = null;
            }
            if (file != null)
            {
                file.Close();
                file.Dispose();
                file = null;
            }
        }
        #endregion

        #region IFrameworkCallback Members
        public void OnFrameMove(Device device, double totalTime, float elapsedTime)
        {
            if (recording)
            {
                if (startTime == -1)
                {
                    startTime = totalTime;
                }
            }
        }

        public void OnFrameRender(Device device, double totalTime, float elapsedTime)
        {
            if (recording && startTime != -1)
            {
                if (totalTime - lastRecord > recorderInterval)
                {
                    lastRecord = totalTime;
                    binaryWriter.Write(totalTime - startTime);
                    state.Position = Airplane.Position;
                    state.Orientation = Airplane.Orientation;
                    state.Rudder = owner.Player.FlightModel.Rudder;
                    state.Throttle = owner.Player.FlightModel.Throttle;
                    state.Elevator = owner.Player.FlightModel.Elevator;
                    state.Ailerons = owner.Player.FlightModel.Ailerons;
                    state.Smoke = owner.Player.Smoking;
                    state.Gear = owner.Player.FlightModel.GearExtended;
                    state.Flaps = owner.Player.FlightModel.FlapsExtended;
                    state.OnWater = owner.Player.FlightModel.OnWater;
                    state.Write(binaryWriter);
                }
            } 
        }
        #endregion
    }
}
