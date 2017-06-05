using System;
using Bonsai.Core.Interfaces;
using Microsoft.DirectX;
using System.Collections.Generic;
using Bonsai.Sound;
using Bonsai.Objects.Meshes;


namespace RCSim
{
    internal class Wind : IFrameworkCallback, IDisposable
    {
        #region Private class ThermalSource
        private class ThermalSource
        {
            public Vector3 Position;
            public float OriginalStrength = 1.5f;
            public float OriginalSize = 30f;
            public float Strength = 1.5f;
            public float SizeSq = 900f;
            public ThermalVisual Visual = null;
        }
        #endregion

        #region Public enums
        public enum GustTypeEnum
        {
            Default,
            HighFrequency,
            LowFreqSharpTransitions,
            LowFreqSmoothTransitions
        }
        #endregion

        #region Private fields
        private Program owner = null;
        private float thermalStrengthFactor = 1.0f;
        private float thermalSizeFactor = 1.0f;
        private List<ThermalSource> thermalSources = new List<ThermalSource>();
        private SoundControllable sound = null;
        private double constantWindSpeed = 0;
        private double gustSpeed = 0;
        private double currentGustSpeed = 0;
        private double currentSpeed = 0;
        private double lastSoundUpdate = 0;
        private GustTypeEnum gustType = GustTypeEnum.LowFreqSharpTransitions;
        private float nextRandomTime = 0;
        private Random random = new Random();
        private double randomFactor = 0;
        // Vector field
        private int nVectors = 9;
        private List<LineMesh> vectors = new List<LineMesh>();
        private bool showVectorField = false;
        #endregion

        #region Public properties
        public double ConstantWindSpeed 
        {
            get { return constantWindSpeed; }
            set
            {
                constantWindSpeed = value;
                CheckSound();
            }
        }

        
        public double GustSpeed 
        {
            get { return gustSpeed; }
            set
            {
                gustSpeed = value;
                CheckSound();
            }
        }

        public GustTypeEnum GustType
        {
            get { return gustType; }
            set { gustType = value; }
        }

        public bool ShowVectorField
        {
            get { return showVectorField; }
            set
            {
                showVectorField = value;
                if (vectors.Count > 0)
                {
                    foreach (LineMesh vector in vectors)
                    {
                        vector.Dispose();
                    }
                    vectors.Clear();
                }
                if (showVectorField)
                {
                    for (int i = 0; i < nVectors; i++)
                    {
                        LineMesh vector = new LineMesh();
                        vectors.Add(vector);
                    }
                }
            }
        }

        public double Direction { get; set; }
        public double DirectionVariance { get; set; }
        public Vector3 CurrentWind { get; set; }
        public double MaximumConstantWindSpeed { get { return 12f; } }
        public double MaximumGustSpeed { get { return 12f; } }
        public double CurrentDirection { get; set; }
        public double DownDrafts { get; set; }
        public double GustFrequency { get; set; }
        public double GustVariability { get; set; }
        public double Turbulence { get; set; }
        public float WindTime { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="owner"></param>
        public Wind(Program owner)
        {
            this.owner = owner;
            GustFrequency = 0.0;
            GustVariability = 0.0;
            Turbulence = 0.0;
            thermalStrengthFactor = (float)Convert.ToInt32(Bonsai.Utils.Settings.GetValue("ThermalStrength")) / 50.0f;
            thermalSizeFactor = (float)Convert.ToInt32(Bonsai.Utils.Settings.GetValue("ThermalSize")) / 50.0f;
            Bonsai.Utils.Settings.SettingsChanged += new Bonsai.Utils.Settings.SettingsEventHandler(Settings_SettingsChanged);            
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Clean up.
        /// </summary>
        public void Dispose()
        {
            Bonsai.Utils.Settings.SettingsChanged -= new Bonsai.Utils.Settings.SettingsEventHandler(Settings_SettingsChanged);
            if (sound != null)
            {
                sound.Stop();
                sound.Dispose();
                sound = null;
            }
        }
        #endregion

        #region Private methods
        private void CheckSound()
        {
            if ((Convert.ToBoolean(Bonsai.Utils.Settings.GetValue("EnableWindSound", "true"))) &&
                (ConstantWindSpeed > 0 || GustSpeed > 0))
            {
                if (sound == null)
                {
                    sound = new SoundControllable("data/wind.wav");
                    UpdateSound();
                    sound.Play(true);
                }  
                else
                    UpdateSound();
            }
            else
            {
                if (sound != null)
                {
                    sound.Stop();
                    sound.Dispose();
                    sound = null;
                }
            }
        }

        private void UpdateSound()
        {            
            if (sound != null)
            {
                sound.Frequency = (int)(22050 + 88200 * CurrentWind.Length() / (MaximumConstantWindSpeed + MaximumGustSpeed));
                sound.Volume = (int)(90 + 10 * CurrentWind.Length() / (MaximumConstantWindSpeed + MaximumGustSpeed));
            }
        }

        private void UpdateGustSpeed(double totalTime)
        {
            switch (gustType)
            {
                case GustTypeEnum.Default:
                    if (Math.Sin(totalTime/3.3) > 0)
                        currentGustSpeed = Math.Abs(GustSpeed * Math.Sin(totalTime/10) * Math.Sin(totalTime / 13.5) * Math.Sin(totalTime / 3.3));
                    else
                        currentGustSpeed = 0;
                    break;
                case GustTypeEnum.HighFrequency:
                    if (Math.Sin(totalTime/1.3) > 0)
                        currentGustSpeed = Math.Abs(GustSpeed * Math.Sin(totalTime/20) * Math.Sin(totalTime / 13.5) * Math.Sin(totalTime / 1.3));
                    else
                        currentGustSpeed = 0;
                    break;
                case GustTypeEnum.LowFreqSharpTransitions:
                    double signedSpeed = GustSpeed * Math.Sin(totalTime / 10.0) * Math.Sin(totalTime / 13.5) * Math.Sin(totalTime / 15.3);
                    if (signedSpeed > 0)
                        currentGustSpeed = Math.Sqrt(signedSpeed);
                    else
                        currentGustSpeed = 0;
                    break;
                case GustTypeEnum.LowFreqSmoothTransitions:
                    signedSpeed = GustSpeed * Math.Sin(totalTime / 10.0) * Math.Sin(totalTime / 13.5) * Math.Sin(totalTime / 15.3);
                    if (signedSpeed > 0)
                        currentGustSpeed = signedSpeed;
                    else
                        currentGustSpeed = 0;
                    break;
            }
        }

        

        private Vector3 GetGustAt(Vector3 position, float totalTime)
        {
            totalTime -= (float)((2.0 / (MaximumConstantWindSpeed + 1)) * (Math.Cos(Direction) * position.X + Math.Sin(Direction) * position.Z));
            //totalTime -= (float)(0.1f * (Math.Cos(Direction) * position.X + Math.Sin(Direction) * position.Z));
            double gustSpeed = 0;
            double frequency = 0.5 + 2*GustFrequency;
            double signedSpeed = GustSpeed * 0.66 * (0.5 +
                Math.Sin(totalTime * frequency / 5.0) *
                 (1.0 - GustVariability + GustVariability * 
                        Math.Sin(totalTime * frequency / 2.3) *
                        Math.Sin(totalTime * frequency / 13.5)));
            if (signedSpeed > 0)
                gustSpeed = signedSpeed;
            double direction = CurrentDirection + GustVariability*Math.Sin(totalTime / 20f);
            return new Vector3(
                (float)(gustSpeed * Math.Cos(direction)),
                (float)(0.3f*gustSpeed*GustVariability*Math.Cos(totalTime)),
                (float)(gustSpeed * Math.Sin(direction)));
        }

        private Vector3 GetGroundInfluence(Vector3 position)
        {
            if (CurrentWind.LengthSq() > 0.01f)
            {
                float maxAlt = 30f;
                float groundLevel = Program.Instance.Heightmap.GetHeightAt(position.X, position.Z);
                float altitude = position.Y - groundLevel;
                if (altitude < groundLevel + maxAlt)
                {
                    Vector3 windPosition = position - (2 * altitude/maxAlt) * CurrentWind;
                    Vector3 groundNormal = Program.Instance.Heightmap.GetSmoothNormalAt(windPosition.X, windPosition.Z);
                    Vector3 normalizedWind = CurrentWind;
                    normalizedWind.Normalize();
                    return (-(maxAlt - altitude) / maxAlt) * Vector3.Dot(normalizedWind, groundNormal) * groundNormal;

                    /*
                    Vector3 groundNormal = Program.Instance.Heightmap.GetSmoothNormalAt(position.X, position.Z);
                    Vector3 normalizedWind = CurrentWind;
                    normalizedWind.Normalize();
                    return (-(maxAlt - altitude) / maxAlt) * Vector3.Dot(normalizedWind, groundNormal) * groundNormal;
                     */
                }
            }

            return Vector3.Empty;
        }

        private Vector3 GetThermalInfluence(Vector3 position)
        {
            Vector3 result = Vector3.Empty;
            float minDistanceSq = 100000f;
            ThermalSource nearestSource = null;
            foreach (ThermalSource thermalSource in thermalSources)
            {
                float distanceSq = (new Vector3(position.X, 0, position.Z) - thermalSource.Position).LengthSq();
                if (distanceSq < minDistanceSq)
                {
                    minDistanceSq = distanceSq;
                    nearestSource = thermalSource;
                    if (distanceSq < thermalSource.SizeSq)
                    {
                        result = new Vector3(0, Math.Min(position.Y,
                                Math.Min(thermalSource.Strength, thermalSource.Strength * (float)Math.Pow(2 * (thermalSource.SizeSq - distanceSq) / thermalSource.SizeSq, 0.25))),
                                0);
                        if ((position.Y > 100) && ((Program.Instance.CurrentTime) % 240 > 200))
                        {
                            result *= 2f;
                        }
                    }
                }
            }
            if ((nearestSource != null) && (DownDrafts > 0) && (result == Vector3.Empty))
            {
                if (minDistanceSq < 2 * nearestSource.SizeSq)
                {
                    if (minDistanceSq < 3*nearestSource.SizeSq / 2)
                    {
                        result = new Vector3(0,
                            (float)(DownDrafts * nearestSource.Strength * (nearestSource.SizeSq - minDistanceSq) / nearestSource.SizeSq), 0);
                    }
                    else
                    {
                        result = new Vector3(0,
                            (float)(DownDrafts * nearestSource.Strength * (minDistanceSq - 2 * nearestSource.SizeSq) / nearestSource.SizeSq), 0);
                    }
                }
            }
            return result;
        }

        private Vector3 GetTurbulence(Vector3 position, float totalTime)
        {
            if (totalTime > nextRandomTime)
            {
                randomFactor = random.NextDouble();
                if (randomFactor > 0.4)
                    nextRandomTime = totalTime + random.Next(2);
                else
                    nextRandomTime = totalTime + random.Next(5);
            }
            totalTime -= (float)((2.0 / (MaximumConstantWindSpeed + 1)) * (Math.Cos(Direction) * position.X + Math.Sin(Direction) * position.Z));
            if (Turbulence > 0)
                return (float)Math.Min(0.5, Math.Max(0, 0.2 + 0.02 * (40 - position.Y))) * (float)(randomFactor*Turbulence) * new Vector3(
                    (float)(Math.Sin(5 * totalTime) * Math.Sin(9.7 * totalTime)),
                    (float)(Math.Sin(5 * (totalTime + 10)) * Math.Sin(9.9 * (totalTime + 10))),
                    (float)(Math.Sin(5 * (totalTime + 20)) * Math.Sin(9.5 * (totalTime + 20))));
            else
                return Vector3.Empty;
        }
        #endregion

        #region Private event handlers
        /// <summary>
        /// Handles the SettingsChanged event from the settings.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Settings_SettingsChanged(object sender, Bonsai.Utils.Settings.SettingsEventArgs e)
        {
            thermalStrengthFactor = (float)Convert.ToInt32(Bonsai.Utils.Settings.GetValue("ThermalStrength"))/50.0f;
            thermalSizeFactor = (float)Convert.ToInt32(Bonsai.Utils.Settings.GetValue("ThermalSize")) / 50.0f;
            foreach (ThermalSource thermalSource in thermalSources)
            {
                thermalSource.SizeSq = thermalSource.OriginalSize * thermalSource.OriginalSize * thermalSizeFactor * thermalSizeFactor;
                thermalSource.Strength = thermalSource.OriginalStrength * thermalStrengthFactor;
                thermalSource.Visual.Size = thermalSource.OriginalSize * thermalSizeFactor;
                thermalSource.Visual.Strength = thermalSource.Strength;
            }
            CheckSound();
        }
        #endregion

        public void AddThermalSource(Vector3 position, float strength, float size)
        {
            ThermalSource source = new ThermalSource();
            source.Position = position;
            source.OriginalSize = size;
            source.OriginalStrength = strength;
            source.SizeSq = size * size * thermalSizeFactor * thermalSizeFactor;
            source.Strength = strength * thermalStrengthFactor;
            source.Visual = new ThermalVisual(owner, strength * thermalStrengthFactor, size * thermalSizeFactor);
            source.Visual.Position = position;
            thermalSources.Add(source);
        }

        /// <summary>
        /// Clears all thermals from the weather.
        /// </summary>
        public void ClearThermalSources()
        {
            foreach (ThermalSource thermalSource in thermalSources)
            {
                thermalSource.Visual.Dispose();                
            }
            thermalSources.Clear();
        }

        /// <summary>
        /// Returns the current windvector at the given position.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        /*
        public Vector3 GetWindAt(Vector3 position)
        {
            return CurrentWind + GetThermalInfluence(position) + (float)currentSpeed * GetGroundInfluence(position);
        }*/

        public Vector3 GetWindAt(Vector3 position, bool includeTurbulence)
        {
            Vector3 windVector = new Vector3(
                (float)(ConstantWindSpeed * Math.Cos(CurrentDirection)),
                0,
                (float)(ConstantWindSpeed * Math.Sin(CurrentDirection))) + GetGustAt(position, WindTime);
            if (includeTurbulence)
                windVector += GetTurbulence(position, WindTime);
            return windVector + GetThermalInfluence(position) + (float)currentSpeed * GetGroundInfluence(position);
        }

        public Vector3 GetWindAt(Vector3 position)
        {
            /*
            float gustSpeed = (float)GetGustSpeedAt(position, WindTime);
            double direction = CurrentDirection - (0.5 + 1.0 * Math.Sin(WindTime / 20f)) * gustSpeed / (2 * MaximumGustSpeed);
            float totalSpeed = (float)ConstantWindSpeed + gustSpeed;
            Vector3 windVector = new Vector3(
                (float)(totalSpeed * Math.Cos(direction)), 
                (float)(0.5f*gustSpeed * Math.Cos(WindTime)), 
                (float)(totalSpeed * Math.Sin(direction)));
             */
            Vector3 windVector = new Vector3(
                (float)(ConstantWindSpeed * Math.Cos(CurrentDirection)),
                0,
                (float)(ConstantWindSpeed * Math.Sin(CurrentDirection))) + GetGustAt(position, WindTime);
            return windVector + GetTurbulence(position, WindTime) + GetThermalInfluence(position) + (float)currentSpeed * GetGroundInfluence(position);
        }


        #region IFrameworkCallback Members
        public void OnFrameMove(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            UpdateGustSpeed(WindTime);
            CurrentDirection = Direction + DirectionVariance * Math.Sin(WindTime / 130.0);
            currentSpeed = ConstantWindSpeed;
            currentSpeed += currentGustSpeed;
            lock (this)
            {
                CurrentWind = new Vector3((float)(currentSpeed * Math.Cos(CurrentDirection)), 0, (float)(currentSpeed * Math.Sin(CurrentDirection)));
            }
            if (totalTime - lastSoundUpdate > 0.1)
            {
                UpdateSound();
                lastSoundUpdate = totalTime;
            }
            foreach (ThermalSource source in thermalSources)
                source.Visual.OnFrameMove(device, totalTime, elapsedTime);
            if (showVectorField)
            {
                int i = 0;
                Vector3 playerPos = Program.Instance.Player.Position;
                foreach (LineMesh vector in vectors)
                {
                    //int j = i / 3;
                    //vector.Vertex1 = new Vector3(playerPos.X + 5 * ((j % 3) - 1), playerPos.Y + 5 * ((i % 3) - 1), playerPos.Z + 5 * ((j / 3) - 1));
                    vector.Vertex1 = new Vector3(playerPos.X + 5 * ((i % 3) - 1), playerPos.Y, playerPos.Z + 5 * ((i / 3) - 1));
                    vector.Vertex2 = vector.Vertex1 + 0.5f*GetWindAt(vector.Vertex1, false);
                    vector.OnFrameMove(device, totalTime, elapsedTime);
                    i++;
                }
            }
        }

        public void OnFrameRender(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            foreach (ThermalSource source in thermalSources)
                source.Visual.OnFrameRender(device, totalTime, elapsedTime);
            if (showVectorField)
            {
                foreach (LineMesh vector in vectors)
                    vector.OnFrameRender(device, totalTime, elapsedTime);
            }
        }
        #endregion
    }
}
