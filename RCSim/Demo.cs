using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Core.Interfaces;
using Bonsai.Objects;
using System.Data;
using Microsoft.DirectX;
using System.Drawing;
using Bonsai.Core;

namespace RCSim
{
    internal class Demo : IFrameworkCallback, IDisposable
    {
        internal class ScheduledMessage
        {
            public double Time;
            public string Message;
            public float Duration;

            public ScheduledMessage(double time, string message, float duration)
            {
                this.Time = time;
                this.Message = message;
                this.Duration = duration;
            }
        }

        #region Private fields
        private bool playing = false;
        private Program owner = null;
        private RecordedFlight recordedFlight = null;
        private int currentScene = 0;
        private int nScenes = 4;
        private int prevFlapsChannel = 0;
        private int prevGearChannel = 0;
        private List<ScheduledMessage> scheduledMessages = new List<ScheduledMessage>();
        private double previousFlightTime = 0;
        #endregion
        
        #region Public events
        public event EventHandler Stopped;
        #endregion

        #region Public properties
        /// <summary>
        /// Gets whether the demo is currently playing.
        /// </summary>
        public bool Playing
        {
            get 
            {
                if (recordedFlight != null)
                    return recordedFlight.Playing;
                else
                    return false;
            }
        }

        /// <summary>
        /// Gets the object to track with the cameras.
        /// </summary>
        public GameObject CameraTarget
        {
            get { return recordedFlight.AirplaneModel; }
        }
        #endregion

        #region Constructor
        public Demo(Program owner)
        {
            this.owner = owner;
            currentScene = nScenes - 1;            
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Clean up.
        /// </summary>
        public void Dispose()
        {
            if (recordedFlight != null)
            {
                recordedFlight.Dispose();
                recordedFlight = null;
            }
        }
        #endregion

        #region Private methods
        private void NextScene()
        {
            if (recordedFlight != null)
            {
                recordedFlight.Dispose();
                recordedFlight = null;
            }
            currentScene = (currentScene + 1) % nScenes;
            recordedFlight = new RecordedFlight(owner);
            scheduledMessages.Clear();
            switch (currentScene)
            {
                case 0:
                    //Program.Instance.SetWaterCamera(true);
                    recordedFlight.FileName = "flight0.dat";
                    //AddScheduledMessage(3, "BMI Beaver 1300 with floats", 160f);
                    break;
                case 1:
                    //Program.Instance.SetWaterCamera(false);
                    //AddScheduledMessage(3, "BMI Allegro 1500", 160f);
                    recordedFlight.FileName = "flight1.dat";            
                    break;
                case 2:
                    //AddScheduledMessage(3, "BMI Beaver 1300", 160f);
                    recordedFlight.FileName = "flight2.dat";            
                    break;
                case 3:
                    //AddScheduledMessage(3, "BMI Arrow 1400", 160f);
                    recordedFlight.FileName = "flight3.dat";            
                    break;
            }
            Program.Instance.CenterHud.ShowCaption("Flip a switch on the controller\nor press space bar to take control", 100000);
            recordedFlight.Stopped += new EventHandler(recordedFlight_Stopped);
            recordedFlight.Playing = true;
            Program.Instance.SetCameraTarget(recordedFlight.AirplaneModel);
        }

        private void SetSky(string skyTexture)
        {
            foreach (DataRow dataRow in Program.Instance.Scenery.Definition.SkyTable.Rows)
            {
                if (dataRow["Texture"].ToString().ToLower().Equals(skyTexture.ToLower()))
                {
                    string skyName = dataRow["Name"].ToString();
                    Bonsai.Utils.Settings.SetValue("Sky", skyName);
                    Vector3 ambientVector = (Vector3)dataRow["AmbientLight"];
                    Vector3 sunVector = (Vector3)dataRow["SunLight"];
                    Program.Instance.Scenery.SetSky(dataRow["Texture"].ToString(), (Vector3)dataRow["SunPosition"],
                        Color.FromArgb((int)(255 * ambientVector.X), (int)(255 * ambientVector.Y), (int)(255 * ambientVector.Z)),
                        Color.FromArgb((int)(255 * sunVector.X), (int)(255 * sunVector.Y), (int)(255 * sunVector.Z)),
                        (float)(dataRow["TerrainAmbient"]), (float)(dataRow["TerrainSun"]));
                    return;
                }
            }            
        }

        private void AddScheduledMessage(double time, string message, float duration)
        {
            scheduledMessages.Add(new ScheduledMessage(time, message, duration));
        }
        #endregion

        #region Public methods
        public void Play()
        {            
            owner.Player.FlightModel.Paused = true;
            prevFlapsChannel = owner.InputManager.GetAxisValue("flaps");
            prevGearChannel = owner.InputManager.GetAxisValue("gear");            
            Program.Instance.CenterHud.ShowGameText("Demo mode", 3.0);
            Program.Instance.CenterHud.ShowCaption("Flip a switch on the controller\nor press space bar to take control", 100000);
            NextScene();
        }

        public void Stop()
        {
            recordedFlight.Stopped -= new EventHandler(recordedFlight_Stopped);
            recordedFlight.Playing = false;
            Program.Instance.CenterHud.ShowGameText("", 1.0);
            Program.Instance.CenterHud.ShowCaption("You're in control!", Program.Instance.CurrentTime, 3.0);                    
            Program.Instance.Player.LoadModel(recordedFlight.AircraftParameters.FileName);
            if (Stopped != null)
                Stopped(this, EventArgs.Empty);
        }
        #endregion

        #region Private event handlers
        void recordedFlight_Stopped(object sender, EventArgs e)
        {
            Program.Instance.CenterHud.ShowGameText("", 0.1);
            NextScene();
        }
        #endregion

        #region IFrameworkCallback Members
        public void OnFrameMove(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            if (prevFlapsChannel == 0)
                prevFlapsChannel = owner.InputManager.GetAxisValue("flaps");
            if (prevGearChannel == 0)
                prevGearChannel = owner.InputManager.GetAxisValue("gear");
            int flapsChannel = owner.InputManager.GetAxisValue("flaps");
            int gearChannel = owner.InputManager.GetAxisValue("gear");
            if (((owner.InputManager.KeyBoardState != null) && (owner.InputManager.KeyBoardState[Microsoft.DirectX.DirectInput.Key.Space])) || 
                (Math.Abs(gearChannel - prevGearChannel) > 50) || (Math.Abs(flapsChannel - prevFlapsChannel) > 50))
            {
                Stop();
            }
            recordedFlight.OnFrameMove(device, totalTime, elapsedTime);
            if (scheduledMessages.Count > 0)
            {
                double messageTime = scheduledMessages[0].Time;
                if (messageTime < recordedFlight.Time)
                {
                    Program.Instance.CenterHud.ShowGameText(scheduledMessages[0].Message, scheduledMessages[0].Duration);                    
                    scheduledMessages.RemoveAt(0);
                }
            }

            if ((recordedFlight.Time % 40 > 10) && (previousFlightTime % 40 < 10))
            {
                Program.Instance.SwitchToCinematicCamera();
            }
            else if ((recordedFlight.Time % 40 > 30) && (previousFlightTime % 40 < 30))
            {
                Program.Instance.SwitchToObserverCamera();
            }
            previousFlightTime = recordedFlight.Time;
        }

        public void OnFrameRender(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            recordedFlight.OnFrameRender(device, totalTime, elapsedTime);
        }
        #endregion

        
    }
}
