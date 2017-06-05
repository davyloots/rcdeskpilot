using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Objects;
using Bonsai.Objects.Particles;
using Bonsai.Core;
using RCSim.Interfaces;

namespace RCSim
{
    internal class Smoke : GameObject, IDisposable
    {
        #region Private fields
        private ParticleSystem particleSystem = null;
        private Program owner = null;
        private IAirplaneControl airplaneControl = null;
        #endregion

        #region Public properties
        public bool Emitting
        {
            get { return particleSystem.Emitting; }
            set { particleSystem.Emitting = value; }
        }
        #endregion

        #region Public constructor
        public Smoke(Program owner, IAirplaneControl airplaneControl)
        {
            this.owner = owner;
            this.airplaneControl = airplaneControl;
            Bonsai.Utils.Settings.SettingsChanged += new Bonsai.Utils.Settings.SettingsEventHandler(Settings_SettingsChanged);
            particleSystem = new ParticleSystem(owner.TransparentObjectManager);
            UpdateDetail();
            particleSystem.Texture = new Bonsai.Objects.Textures.TextureBase("data\\smokepuff.png");
            particleSystem.Texture.Transparent = true;
            particleSystem.Emitting = false;
            particleSystem.Variation = new Microsoft.DirectX.Vector3(0.05f, 0.05f, 0.05f);
        }
        #endregion

        #region Dispose
        /// <summary>
        /// Clean up.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            Bonsai.Utils.Settings.SettingsChanged -= new Bonsai.Utils.Settings.SettingsEventHandler(Settings_SettingsChanged);
            if (particleSystem != null)
            {
                particleSystem.Dispose();
                particleSystem = null;
            }
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
            if (e.Key == "SmokeDetail")
            {
                UpdateDetail();
            }
        }
        #endregion

        #region Private methods
        private void UpdateDetail()
        {
            string smokeDetail = Bonsai.Utils.Settings.GetValue("SmokeDetail");
            switch (smokeDetail)
            {
                case "2":
                    particleSystem.NMaxParticles = 300;
                    particleSystem.LifeTime = 3.0;
                    break;
                case "3":
                    particleSystem.NMaxParticles = 450;
                    particleSystem.LifeTime = 4.5;
                    break;
                case "1":
                default:
                    particleSystem.NMaxParticles = 150;
                    particleSystem.LifeTime = 1.5;
                    break;
            }
        }
        #endregion

        #region Public overridden GameObject methods
        public override void OnFrameMove(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            particleSystem.Position = this.Position;
            particleSystem.Intensity = Math.Max(5, Convert.ToInt32(airplaneControl.Throttle * 100.0));
            particleSystem.WindVector = owner.Weather.Wind.CurrentWind;
            particleSystem.OnFrameMove(device, totalTime, elapsedTime);
            base.OnFrameMove(device, totalTime, elapsedTime);
        }

        public override void OnFrameRender(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            particleSystem.OnFrameRender(device, totalTime, elapsedTime);
            base.OnFrameRender(device, totalTime, elapsedTime);
        }
        #endregion
    }
}
