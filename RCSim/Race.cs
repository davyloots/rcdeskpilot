using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Core.Interfaces;
using Bonsai.Objects;
using Bonsai.Objects.Meshes;
using System.Data;
using Microsoft.DirectX;
using Bonsai.Sound;

namespace RCSim
{
    internal class Race: IFrameworkCallback, IDisposable
    {
        #region Protected fields
        protected bool racing = false;
        protected double startTime = 0;
        protected GameObject arrow = null;
        protected Program owner = null;
        protected List<Gate> gates = new List<Gate>();
        protected SoundControllable passSound = null;
        protected int currentGateNr = 0;
        #endregion

        #region Public properties
        public bool Racing
        {
            get { return racing; }
            set { racing = value; }
        }
        #endregion

        #region Constructor
        public Race(Program owner)
        {
            this.owner = owner;

            passSound = new SoundControllable("data\\gate.wav");
            passSound.Volume = 100;

            // Load the racing pylons
            LoadGates();

            arrow = new GameObject();
            arrow.Mesh = new XMesh("data\\arrow.x");
            arrow.Position = new Vector3(gates[0].Position.X, gates[0].Position.Y + 6f, gates[0].Position.Z);

            owner.CenterHud.ShowGameText("Fly through the first gate to start the clock", 1000);
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Clean up.
        /// </summary>
        public void Dispose()
        {
            foreach (Gate gate in gates)
            {
                gate.Dispose();
            }
            gates.Clear();

            if (arrow != null)
            {
                arrow.Dispose();
                arrow = null;
            }
            if (passSound != null)
            {
                passSound.Dispose();
                passSound = null;
            }
            owner.CenterHud.ShowGameText("", 0);
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Restarts the race
        /// </summary>
        public void Restart()
        {
            owner.CenterHud.ShowGameText("Fly through the first gate to start the clock", 1000);
            currentGateNr = 0;
            startTime = 0;
            arrow.Position = new Vector3(gates[0].Position.X, gates[0].Position.Y + 6f, gates[0].Position.Z);
            racing = false;
        }
        #endregion

        #region Private methods
        private void LoadGates( )
        {
            foreach (DataRow row in owner.Scenery.Definition.GateTable.Rows)
            {
                Gate gate = new Gate(owner, (Vector3)row["Position"], (Vector3)row["Orientation"], (int)row["SequenceNr"], (int)row["Type"]);
                gates.Add(gate);
                gate.GatePassed += new EventHandler(gate_GatePassed);
            }
        }

        private Gate GetGate(int sequenceNumber)
        {
            foreach (Gate gate in gates)
            {
                if (gate.SequenceNumber == sequenceNumber)
                    return gate;
            }
            return null;
        }
        #endregion

        #region Private event handlers
        /// <summary>
        /// Handles the GatePassed event for the gates.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void gate_GatePassed(object sender, EventArgs e)
        {
            Gate gate = sender as Gate;
            if (gate != null)
            {
                if (gate.SequenceNumber == 0)
                {
                    if (passSound != null)
                        passSound.Play(false);
                    // Start counting
                    startTime = owner.CurrentTime;
                    racing = true;
                    // set the proper gates
                    Gate oldGate = GetGate(currentGateNr);
                    oldGate.Active = false;
                    currentGateNr = 1;
                    Gate newGate = GetGate(currentGateNr);
                    newGate.Active = true;
                    arrow.Position = new Vector3(newGate.Position.X, newGate.Position.Y + newGate.Height, newGate.Position.Z);
                }
                else if (gate.SequenceNumber == currentGateNr)
                {
                    gate.Active = false;
                    if (passSound != null)
                        passSound.Play(false);
                    if (currentGateNr == gates.Count - 1)
                    {
                        // Finished!!
                        racing = false;
                        TimeSpan ts = TimeSpan.FromSeconds(owner.CurrentTime - startTime);
//                            new TimeSpan(0, 0, 0, (int)Math.Floor(owner.CurrentTime - startTime), (int)(1000 * (owner.CurrentTime - startTime)) % 1000);
                        owner.CenterHud.ShowGameText(string.Format("You finished in {0}:{1}.{2}", (int)(Math.Floor(ts.TotalMinutes)), ts.Seconds.ToString("00"), ts.Milliseconds.ToString("000")), 1000);
                    }
                    // next gate
                    currentGateNr = (currentGateNr + 1) % gates.Count;
                    gate = GetGate(currentGateNr);
                    gate.Active = true;
                    arrow.Position = new Vector3(gate.Position.X, gate.Position.Y + gate.Height, gate.Position.Z);
                }
            }
        }
        #endregion

        #region IFrameworkCallback Members
        public void OnFrameMove(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            foreach (Gate gate in gates)
            {
                gate.OnFrameMove(device, totalTime, elapsedTime);
            }
            arrow.RotateYAngle = (float)(totalTime);
            arrow.OnFrameMove(device, totalTime, elapsedTime);
            if (racing)
            {
                TimeSpan ts = TimeSpan.FromSeconds(totalTime - startTime);
                owner.CenterHud.ShowGameText(string.Format("Your time: {0}:{1}.{2}", (int)(Math.Floor(ts.TotalMinutes)), ts.Seconds.ToString("00"), ts.Milliseconds.ToString("000")), 1);
                //owner.CenterHud.ShowGameText(string.Format("Your time: {0}", totalTime - startTime), totalTime, 1);
            }
        }

        public void OnFrameRender(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            foreach (Gate gate in gates)
            {
                gate.OnFrameRender(device, totalTime, elapsedTime);
            }
            arrow.OnFrameRender(device, totalTime, elapsedTime);
        }
        #endregion

    }
}
