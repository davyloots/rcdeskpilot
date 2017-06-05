using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Objects;
using Bonsai.Objects.Particles;
using Bonsai.Core;

namespace RCSim
{
    internal class ThermalVisual : GameObject, IDisposable
    {
        #region Private fields
        private ParticleSystem particleSystem = null;
        private Program owner = null;
        private float strength;
        private float size;
        #endregion

        #region Public properties
        public bool Emitting
        {
            get { return particleSystem.Emitting; }
            set { particleSystem.Emitting = value; }
        }

        public float Strength
        {
            get { return strength; }
            set 
            { 
                strength = value;
                if (strength == 0)
                    particleSystem.Visible = false;
                else
                    particleSystem.Visible = Convert.ToBoolean(Bonsai.Utils.Settings.GetValue("EnableThermalVisual"));
            }
        }

        public float Size
        {
            get { return size; }
            set 
            { 
                size = value;
                particleSystem.Variation = new Microsoft.DirectX.Vector3(size / 2, 0, size / 2);
            }
        }
        #endregion

        #region Public constructor
        public ThermalVisual(Program owner, float strength, float size)
        {
            this.owner = owner;
            this.strength = strength;
            this.size = size;
            particleSystem = new ParticleSystem(owner.TransparentObjectManager);
            particleSystem.Texture = new Bonsai.Objects.Textures.TextureBase("data\\bubble.png");
            particleSystem.Texture.Transparent = true;
            particleSystem.Variation = new Microsoft.DirectX.Vector3(size / 2, 0, size / 2);
            particleSystem.Emitting = true;
            UpdateDetail();
            Bonsai.Utils.Settings.SettingsChanged += new Bonsai.Utils.Settings.SettingsEventHandler(Settings_SettingsChanged);
        }
        #endregion

        #region Dispose
        /// <summary>
        /// Clean up.
        /// </summary>
        public override void Dispose()
        {            
            Bonsai.Utils.Settings.SettingsChanged -= new Bonsai.Utils.Settings.SettingsEventHandler(Settings_SettingsChanged);
            if (particleSystem != null)
            {
                particleSystem.Dispose();
                particleSystem.Texture.Dispose();
                particleSystem = null;
            }
            base.Dispose();
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
            UpdateDetail();
        }
        #endregion

        #region Private methods
        private void UpdateDetail()
        {
            if (particleSystem != null)
            {
                if (Convert.ToBoolean(Bonsai.Utils.Settings.GetValue("EnableThermalVisual")) && (strength > 0))
                {
                    particleSystem.Visible = true;
                }
                else
                {
                    particleSystem.Visible = false;
                }
                particleSystem.NMaxParticles = 20;
                particleSystem.LifeTime = 40.0;
            }
        }
        #endregion

        #region Public overridden GameObject methods
        public override void OnFrameMove(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            if (particleSystem.Visible)
            {
                particleSystem.Position = this.Position;
                particleSystem.WindVector = new Microsoft.DirectX.Vector3(0, this.strength, 0);
                particleSystem.OnFrameMove(device, totalTime, elapsedTime);
            }
            base.OnFrameMove(device, totalTime, elapsedTime);
        }

        public override void OnFrameRender(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            if (particleSystem.Visible)
                particleSystem.OnFrameRender(device, totalTime, elapsedTime);
            base.OnFrameRender(device, totalTime, elapsedTime);
        }
        #endregion
    }
}
