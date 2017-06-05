using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Objects;
using Bonsai.Objects.Meshes;
using Bonsai.Sound;
using Microsoft.DirectX;
using RCSim.DataClasses;
using Bonsai.Core;
using Bonsai.Objects.Cameras;
using RCSim.Interfaces;
using System.IO;

namespace RCSim
{
    internal class AirplaneModel : GameObject, IDisposable
    {
        #region Private fields
        private SoundControllable engineSound = null;
        private SoundControllable rotorSound = null;
        private SoundControllable crashSound = null;
        private IAirplaneControl airplaneControl = null;
        private float elapsedCumul = 0f;
        private float prevDistance = 0;
        private CameraBase prevCamera = null;
        private int engineMinFreq = 0;
        private int engineMaxFreq = 44100;
        #endregion

        #region Public properties
        public Vector3 BoundingBoxMax
        {
            get
            {
                XMesh xMesh = (XMesh)(this.Mesh);
                return new Vector3(xMesh.BoundingBoxMax.X*Scale.X, xMesh.BoundingBoxMax.Y*Scale.Y, xMesh.BoundingBoxMax.Z*Scale.Z);
            }
        }

        public Vector3 BoundingBoxMin
        {
            get
            {
                XMesh xMesh = (XMesh)(this.Mesh);
                return new Vector3(xMesh.BoundingBoxMin.X * Scale.X, xMesh.BoundingBoxMin.Y * Scale.Y, xMesh.BoundingBoxMin.Z * Scale.Z);
            }
        }

        public IAirplaneControl AirplaneControl
        {
            get
            {
                return airplaneControl;
            }
        }

        public string MeshFileName
        {
            get
            {
                return airplaneControl.AircraftParameters.FixedMesh;
            }
            set
            {
                airplaneControl.AircraftParameters.FixedMesh = value;
                SetMeshFile(value);
            }
        }

        public string EngineSound
        {
            set
            {
                if (engineSound != null)
                {
                    engineSound.Stop();
                    engineSound.Dispose();
                    engineSound = null;
                }
                airplaneControl.AircraftParameters.EngineSound = value;
                if (!string.IsNullOrEmpty(airplaneControl.AircraftParameters.EngineSound))
                    engineSound = new SoundControllable(airplaneControl.AircraftParameters.EngineSound, airplaneControl.AircraftParameters.FolderName);
            }
        }
        #endregion

        public AirplaneModel(IAirplaneControl airplaneControl)
        {
            this.airplaneControl = airplaneControl;
            if (!string.IsNullOrEmpty(airplaneControl.AircraftParameters.FixedMesh))
            {
                this.Mesh = new XMesh(airplaneControl.AircraftParameters.FixedMesh, airplaneControl.AircraftParameters.FolderName);
                XMesh xMesh = (XMesh)(this.Mesh);
                xMesh.ComputeBoundingBox();
            }
            this.Scale = new Vector3(airplaneControl.AircraftParameters.Scale, airplaneControl.AircraftParameters.Scale, airplaneControl.AircraftParameters.Scale);
            this.Position = new Vector3(0.0f, 10.0f, 0.0f);

            foreach (AircraftParameters.ControlSurface controlSurfaceDef in airplaneControl.AircraftParameters.ControlSurfaces)
            {
                ControlSurface surface = new ControlSurface(controlSurfaceDef, airplaneControl);
                AddChild(surface);
            }

            engineMinFreq = airplaneControl.AircraftParameters.EngineMinFrequency;
            engineMaxFreq = airplaneControl.AircraftParameters.EngineMaxFrequency;
            if (!string.IsNullOrEmpty(airplaneControl.AircraftParameters.EngineSound))
                engineSound = new SoundControllable(airplaneControl.AircraftParameters.EngineSound, airplaneControl.AircraftParameters.FolderName);
            if ((airplaneControl.AircraftParameters.FlightModelType == AircraftParameters.FlightModelTypeEnum.Helicopter) 
                && (airplaneControl.AircraftParameters.RotorSound != null))
            {
                if (!string.IsNullOrEmpty(airplaneControl.AircraftParameters.RotorSound))
                    rotorSound = new SoundControllable(airplaneControl.AircraftParameters.RotorSound, airplaneControl.AircraftParameters.FolderName);                
            }
            
            crashSound = new SoundControllable("data\\crash.wav");

            if (!(airplaneControl is RecordedFlight))
                Birds.ScareCrow = this; 
        }

        #region IDisposable Members
        /// <summary>
        /// Clean up.
        /// </summary>
        public override void Dispose()
        {
            if (crashSound != null)
            {
                crashSound.Stop();
                crashSound.Dispose();
                crashSound = null;
            }
            if (engineSound != null)
            {
                engineSound.Stop();
                engineSound.Dispose();
                engineSound = null;
            }
            if (rotorSound != null)
            {
                rotorSound.Stop();
                rotorSound.Dispose();
                rotorSound = null;
            }
            if (Mesh != null)
            {
                Mesh.Dispose();
                Mesh = null;
            }
            base.Dispose();
        }
        #endregion

        #region Public methods
        public void KillEngine()
        {
            if (engineSound != null)
                engineSound.Stop();
            if (rotorSound != null)
                rotorSound.Stop();
        }

        public void StartEngine()
        {
            if (engineSound != null)
            {
                engineSound.Volume = 1;
                engineSound.Play(true);
            }
            if (rotorSound != null)
            {
                rotorSound.Volume = 1;
                rotorSound.Play(true);
            }
        }

        public void Crash()
        {
            Vector3 posDiff = this.Position - Framework.Instance.CurrentCamera.LookFrom;
            float distance = posDiff.Length();
            crashSound.Volume = Math.Max(100 - (int)(distance / 5), 0);
            crashSound.Play(false);
            if (engineSound != null)
                engineSound.Stop();
            if (rotorSound != null)
                rotorSound.Stop();
        }

        public void ReloadEngineFrequencies()
        {
            engineMinFreq = airplaneControl.AircraftParameters.EngineMinFrequency;
            engineMaxFreq = airplaneControl.AircraftParameters.EngineMaxFrequency;
        }
        #endregion

        #region Protected methods
        protected void SetMeshFile(string filename)
        {
            if (Mesh != null)
            {
                Mesh.Dispose();
                Mesh = null;
            }
            if (!string.IsNullOrEmpty(filename))
            {
                Mesh = new XMesh(filename, airplaneControl.AircraftParameters.FolderName);
                XMesh xMesh = (XMesh)(this.Mesh);
                xMesh.ComputeBoundingBox();
            }
        }
        #endregion


        public override void OnFrameMove(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            elapsedCumul += elapsedTime;
            if (elapsedCumul > 0.1f)
            {
                if (Framework.Instance.CurrentCamera != null)
                {
                    Vector3 posDiff = this.Position - Framework.Instance.CurrentCamera.LookFrom;
                    float distance = posDiff.Length();
                    if (engineSound != null)
                    {                        
                        if (airplaneControl.Throttle < 0.01 && engineMinFreq == 0)
                            engineSound.Volume = 0;
                        else
                            engineSound.Volume = Math.Max(100 - (int)(distance / 5), 0);
                        float dopplerCoeff = 1f;
                        if (prevDistance != 0 && (prevCamera == Framework.Instance.CurrentCamera))
                        {
                            dopplerCoeff = 1f + ((prevDistance - distance) / 150f) / elapsedCumul;
                        }
                        
                        //engineSound.Frequency = (int)(44100 * airplaneControl.Throttle * 1.33 * dopplerCoeff);
                        engineSound.Frequency = (int)((engineMinFreq + (int)((engineMaxFreq - engineMinFreq) * airplaneControl.Throttle)) * dopplerCoeff);
                    }
                    prevDistance = distance;
                    prevCamera = Framework.Instance.CurrentCamera;
                    if (rotorSound != null)
                    {
                        rotorSound.Volume = Math.Max(Math.Min((int)((100 - (int)(distance / 5))*Math.Pow(airplaneControl.RelativeRotorForce, 0.5)*0.8f), 100), 0);
                        rotorSound.Frequency = (int)(airplaneControl.RotorRPM * 22100 / 1000f);
                    }
                }
                elapsedCumul = 0f;
            }
            base.OnFrameMove(device, totalTime, elapsedTime);
        }
    }
}
