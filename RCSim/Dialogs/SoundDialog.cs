using System;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Bonsai.Core.Controls;
using Bonsai.Core;
using Bonsai.Core.Dialogs;
using Bonsai.Sound;

namespace RCSim.Dialogs
{
    #region Control Ids
    public enum SoundDialogControlIds
    {
        Static = -1,
        None,
        OK,
        Back,
        Volume,
        EnableSound,
        EnableVariometer
    }
    #endregion

    /// <summary>
    /// Dialog for selection of device settings 
    /// </summary>
    class SoundDialog : DialogBase
    {
        #region Private fields
        private Program owner;

        private Button buttonOk;
        private Button buttonBack;
        private Slider sliderVolume;
        private Checkbox checkEnableSound;
        private Checkbox checkEnableVariometer;
        #endregion

        #region Creation
        /// <summary>Creates a new settings dialog</summary>
        public SoundDialog(Framework framework, Program owner)
            : base(framework)
        {
            this.owner = owner;
            framework.Device.DeviceLost += new EventHandler(Device_DeviceLost);
            //framework.Device.DeviceReset += new EventHandler(Device_DeviceReset);
            framework.Device.Disposing += new EventHandler(Device_Disposing);
            CreateControls();
        }

        void Device_Disposing(object sender, EventArgs e)
        {
            base.OnDestroyDevice(sender, e);
        }

        void Device_DeviceReset(object sender, EventArgs e)
        {
            base.OnResetDevice();
            Dialog.SetRefreshTime((float)FrameworkTimer.GetTime());
        }

        void Device_DeviceLost(object sender, EventArgs e)
        {
            base.OnLostDevice();
        }
        #endregion

        /// <summary>
        /// Creates the controls for use in the dialog
        /// </summary>
        private void CreateControls()
        {
            // Right justify static controls
            Element e = dialog.GetDefaultElement(ControlType.StaticText, 0);
            e.textFormat = DrawTextFormat.VerticalCenter | DrawTextFormat.Left;

            // Title
            int y = 5;
            StaticText title = dialog.AddStatic((int)SoundDialogControlIds.Static, "Sound settings", 10, y, 400, 30);
            e = title[0];
            e.FontIndex = 1;
            e.textFormat = DrawTextFormat.Top | DrawTextFormat.Left;

            StaticText volumeSubTitle = dialog.AddStatic((int)SoundDialogControlIds.Static, "Volume", 10, y += 30, 200, 30);
            e = volumeSubTitle[0];
            e.FontIndex = 3;

            sliderVolume = dialog.AddSlider((int)SoundDialogControlIds.Volume, 200, y+5, 100, 20);
            sliderVolume.ValueChanged += new EventHandler(sliderVolume_ValueChanged);

            checkEnableSound = dialog.AddCheckBox((int)SoundDialogControlIds.EnableSound, "Enable wind sound", 10, y += 30, 400, 20,
            Convert.ToBoolean(Bonsai.Utils.Settings.GetValue("EnableWindSound", "true")));
            checkEnableSound.Changed += new EventHandler(checkEnableSound_Changed);

            checkEnableVariometer = dialog.AddCheckBox((int)SoundDialogControlIds.EnableVariometer, "Enable variometer (if equipped)", 10, y += 30, 400, 20,
                Convert.ToBoolean(Bonsai.Utils.Settings.GetValue("EnableVariometer", "false")));
            checkEnableVariometer.Changed += new EventHandler(checkEnableVariometer_Changed);


            buttonBack = dialog.AddButton((int)SoundDialogControlIds.Back, "back to menu", 190, 435, 100, 31);
            buttonBack.Click += new EventHandler(buttonBack_Click);

            buttonOk = dialog.AddButton((int)SoundDialogControlIds.OK, "back to sim", 350, 435, 100, 31);
            buttonOk.Click += new EventHandler(buttonOk_Click);
        }

        public override void OnShowDialog()
        {
            base.OnShowDialog();
            sliderVolume.Value = SoundManager.Volume;
            
        }


        #region Event handlers for the controls
        void buttonOk_Click(object sender, EventArgs e)
        {
            owner.Player.FlightModel.Paused = false;
            Dialog.SetRefreshTime((float)FrameworkTimer.GetTime());
            parent.HideDialog();
        }


        void buttonBack_Click(object sender, EventArgs e)
        {
            parent.HideDialog();
            owner.ShowMenu();
        }


        void sliderVolume_ValueChanged(object sender, EventArgs e)
        {
            SoundManager.Volume = (int)sliderVolume.Value;
            Bonsai.Utils.Settings.SetValue("Volume", SoundManager.Volume.ToString());
        }

        void checkEnableSound_Changed(object sender, EventArgs e)
        {
            Bonsai.Utils.Settings.SetValue("EnableWindSound", checkEnableSound.IsChecked.ToString());
        }

        void checkEnableVariometer_Changed(object sender, EventArgs e)
        {
            Bonsai.Utils.Settings.SetValue("EnableVariometer", checkEnableVariometer.IsChecked.ToString());
        }
        #endregion
    }
}
