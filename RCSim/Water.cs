using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Objects;
using Microsoft.DirectX;

namespace RCSim
{
    internal class Water : GameObject, IDisposable
    {
        #region protected fields
        protected Bonsai.Objects.Terrain.Water water = null;
        protected int waterDetail = 2;
        protected Bonsai.Objects.Terrain.Water.QualityLevelEnum qualityLevel = Bonsai.Objects.Terrain.Water.QualityLevelEnum.Medium;
        protected Vector3 waterPosition;
        protected float waterSize;
        protected Vector3 sunPosition;
        protected Bonsai.Objects.Terrain.Water.OnFrameRenderDelegate waterCallback;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="size"></param>
        public Water(Vector3 position, float size)
        {
            waterPosition = position;
            waterSize = size;
            waterDetail = Convert.ToInt32(Bonsai.Utils.Settings.GetValue("WaterDetail", "2"));
            switch (waterDetail)
            {
                case 2:
                    qualityLevel = Bonsai.Objects.Terrain.Water.QualityLevelEnum.Medium;
                    break;
                case 3:
                    qualityLevel = Bonsai.Objects.Terrain.Water.QualityLevelEnum.High;
                    break;
                default:
                    qualityLevel = Bonsai.Objects.Terrain.Water.QualityLevelEnum.Low;
                    break;
            }
            water = new Bonsai.Objects.Terrain.Water(position, size, qualityLevel);
            if (water != null)
            {
                water.WindDirection = (float)Program.Instance.Weather.Wind.Direction;
                water.WindSpeed = (float)Program.Instance.Weather.Wind.ConstantWindSpeed;
            }
            Bonsai.Utils.Settings.SettingsChanged += new Bonsai.Utils.Settings.SettingsEventHandler(Settings_SettingsChanged);
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Clean up.
        /// </summary>
        void IDisposable.Dispose()
        {
            if (water != null)
            {
                water.Dispose();
            }
        }
        #endregion

        #region Private event handlers
        /// <summary>
        /// Handles the SettingsChanged event for Bonsai settings.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Settings_SettingsChanged(object sender, Bonsai.Utils.Settings.SettingsEventArgs e)
        {
            int newWaterDetail = Convert.ToInt32(Bonsai.Utils.Settings.GetValue("WaterDetail", "2"));
            if (newWaterDetail != waterDetail)
            {
                waterDetail = newWaterDetail;
                switch (waterDetail)
                {
                    case 2:
                        qualityLevel = Bonsai.Objects.Terrain.Water.QualityLevelEnum.Medium;
                        break;
                    case 3:
                        qualityLevel = Bonsai.Objects.Terrain.Water.QualityLevelEnum.High;
                        break;
                    default:
                        qualityLevel = Bonsai.Objects.Terrain.Water.QualityLevelEnum.Low;
                        break;
                }
                if (water != null)
                {
                    water.Dispose();
                    water = null;
                }
                water = new Bonsai.Objects.Terrain.Water(waterPosition, waterSize, qualityLevel);
                water.SunPosition = sunPosition;
                water.OnFrameRenderMethod = waterCallback;
            }

            if (water != null)
            {
                water.WindDirection = (float)Program.Instance.Weather.Wind.Direction;
                water.WindSpeed = (float)Program.Instance.Weather.Wind.ConstantWindSpeed;
            }
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Sets the position of the sun.
        /// </summary>
        /// <param name="sunPosition"></param>
        public void SetSunPosition(Vector3 sunPosition)
        {
            this.sunPosition = sunPosition;
            if (water != null)
                water.SunPosition = sunPosition;
        }

        /// <summary>
        /// Sets the callback to use during rendering.
        /// </summary>
        /// <param name="callback"></param>
        public void SetCallback(Bonsai.Objects.Terrain.Water.OnFrameRenderDelegate callback)
        {
            this.waterCallback = callback;
            if (water != null)
                water.OnFrameRenderMethod = callback;
        }

        /// <summary>
        /// Performs the render step for the textures.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="totalTime"></param>
        /// <param name="elapsedTime"></param>
        public void RenderTextures(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            if (water != null)
                water.RenderTextures(device, totalTime, elapsedTime);
        }

        public bool OverWater(Vector3 position)
        {
            float diffX = position.X - waterPosition.X;
            float diffZ = position.Z - waterPosition.Z;
            if ((diffX < waterSize) && (diffX > -waterSize) && (diffZ < waterSize) && (diffZ > -waterSize))
                return true;
            else
                return false;
        }
        #endregion

        #region Overridden GameObject methods
        public override void OnFrameMove(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            if (water != null)
                water.OnFrameMove(device, totalTime, elapsedTime);
            base.OnFrameMove(device, totalTime, elapsedTime);
        }

        public override void OnFrameRender(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            if (water != null)
                water.OnFrameRender(device, totalTime, elapsedTime);
            base.OnFrameRender(device, totalTime, elapsedTime);
        }
        #endregion
    }
}
