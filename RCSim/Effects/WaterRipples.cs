using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Objects;
using Microsoft.DirectX;

namespace RCSim.Effects
{
    internal class WaterRipples : GameObject
    {
        #region Private fields
        private List<WaterRipple> ripples = new List<WaterRipple>();
        private int nRipples = 50;
        private int currentRipple = 0;
        private double lastRippleTime = 0;
        private static int referenceCount = 0;
        #endregion

        public WaterRipples()
        {
            referenceCount++;
            for (int i = 0; i < nRipples; i++)
            {
                ripples.Add(new WaterRipple());
            }
            Bonsai.Utils.Settings.SettingsChanged += new Bonsai.Utils.Settings.SettingsEventHandler(Settings_SettingsChanged);
        }

        public override void Dispose()
        {
            foreach (WaterRipple ripple in ripples)
            {
                ripple.Dispose();
            }
            ripples.Clear();
            referenceCount--;
            if (referenceCount == 0)
                WaterRipple.CleanUp();
            base.Dispose();
        }

        #region Private event handlers
        void Settings_SettingsChanged(object sender, Bonsai.Utils.Settings.SettingsEventArgs e)
        {
            int newDetail = Convert.ToInt32(Bonsai.Utils.Settings.GetValue("WaterRipplesDetail", "2"));
            if (WaterRipple.Detail != newDetail)
            {
                WaterRipple.SetDetail(newDetail);
            }                
        }
        #endregion

        public void AddRipple(float speed, Vector3 position, double currentTime)
        {
            if ((currentTime - lastRippleTime) > 1f / (1 + speed))
            {
                lastRippleTime = currentTime;
                ripples[currentRipple].Create(position, Program.Instance.CurrentTime,
                    Math.Min(255, 50 + (int)(10*speed)));
                currentRipple = (currentRipple + 1) % nRipples;
            }
        }

        #region Overridden IFrameworkObject methods
        public override void OnFrameMove(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            foreach (WaterRipple ripple in ripples)
                ripple.OnFrameMove(device, totalTime, elapsedTime);
            base.OnFrameMove(device, totalTime, elapsedTime);
        }

        public override void OnFrameRender(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            for (int i = currentRipple; ; )
            {
                ripples[i].OnFrameRender(device, totalTime, elapsedTime);
                i = (i + 1) % nRipples;
                if (i == currentRipple)
                    break;
            }
            base.OnFrameRender(device, totalTime, elapsedTime);
        }
        #endregion
    }
}
