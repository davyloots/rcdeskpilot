using System;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Bonsai.Core.Controls;
using Bonsai.Core;
using Bonsai.Core.Dialogs;
using System.Collections.Generic;
using Bonsai.Input;
using System.Diagnostics;

namespace RCSim.Dialogs
{
    #region Control Ids
    public enum ControlsDialogControlIds
    {
        Static = -1,
        None,
        OK,
        Back,
        ButtonCalibrate,
        JoystickCombo,
        SliderX,
        SliderY,
        SliderZ,
        SliderRx,
        SliderRy,
        SliderRz,
        SliderSlider1,
        SliderSlider2,
        ComboRoll,
        CheckRollInv,
        CheckRollExp,
        ComboPitch,
        CheckPitchInv,
        CheckPitchExp,
        ComboYaw,
        CheckYawInv,
        CheckYawExp,
        ComboThrottle,
        CheckThrottleInv,
        CheckThrottleExp,
        CheckUseAileron,
        ComboFlaps,
        ComboGear
    }
    #endregion

    /// <summary>
    /// Dialog for selection of device settings 
    /// </summary>
    class ControlsDialog : DialogBase
    {
        #region Private fields
        private Program owner;
        private List<string> joysticks = null;

        private Button buttonBack;
        private Button buttonOk;
        private Button buttonCalibrate;
        private ComboBox comboJoysticks;
        private Slider sliderX;
        private Slider sliderY;
        private Slider sliderZ;
        private Slider sliderRx;
        private Slider sliderRy;
        private Slider sliderRz;
        private Slider sliderSlider1;
        private Slider sliderSlider2;
        private ComboBox comboRoll;
        private Checkbox checkRollInv;
        private Checkbox checkRollExp;
        private ComboBox comboPitch;
        private Checkbox checkPitchInv;
        private Checkbox checkPitchExp;
        private ComboBox comboYaw;
        private Checkbox checkYawInv;
        private Checkbox checkYawExp;
        private ComboBox comboThrottle;
        private Checkbox checkThrottleInv;
        private Checkbox checkThrottleExp;
        private ComboBox comboFlaps;
        private ComboBox comboGear;
        
        private Checkbox checkUseAilerons;

        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        #endregion

        #region Creation
        /// <summary>Creates a new settings dialog</summary>
        public ControlsDialog(Framework framework, Program owner)
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

        #region Private methods
        /// <summary>
        /// Creates the controls for use in the dialog
        /// </summary>
        private void CreateControls()
        {
            // Get the joysticklist.
            joysticks =  owner.InputManager.GetAvailableJocksticks();
            
            // Right justify static controls
            Element e = dialog.GetDefaultElement(ControlType.StaticText, 0);
            e.textFormat = DrawTextFormat.VerticalCenter | DrawTextFormat.Left;

            // Title
            int y = 5;
            StaticText title = dialog.AddStatic((int)ControlsDialogControlIds.Static, "Control settings", 10, 5, 400, 30);
            e = title[0];
            e.FontIndex = 1;
            e.textFormat = DrawTextFormat.Top | DrawTextFormat.Left;

            StaticText windSubTitle = dialog.AddStatic((int)ControlsDialogControlIds.Static, "Controller", 10, y += 30, 400, 30);
            e = windSubTitle[0];
            e.FontIndex = 2;

            comboJoysticks = dialog.AddComboBox((int)ControlsDialogControlIds.JoystickCombo, 10, y += 40, 400, 30);
            foreach (string joystick in joysticks)
                comboJoysticks.AddItem(joystick, joystick);
            comboJoysticks.Changed += new EventHandler(comboJoysticks_Changed);

            buttonCalibrate = dialog.AddButton((int)ControlsDialogControlIds.ButtonCalibrate, "Calibrate", 430, y += 5, 150, 20);
            buttonCalibrate.Click += new EventHandler(buttonCalibrate_Click);

            int y2 = y + 35;

            StaticText textX = dialog.AddStatic((int)ControlsDialogControlIds.Static, "1:", 480, y2, 20, 12);
            e = textX[0]; e.FontIndex = 4;
            sliderX = dialog.AddSlider((int)ControlsDialogControlIds.SliderX, 500, y2, 100, 12);

            StaticText textY = dialog.AddStatic((int)ControlsDialogControlIds.Static, "2:", 480, y2 += 12, 20, 12);
            e = textY[0]; e.FontIndex = 4;
            sliderY = dialog.AddSlider((int)ControlsDialogControlIds.SliderY, 500, y2, 100, 12);

            StaticText textZ = dialog.AddStatic((int)ControlsDialogControlIds.Static, "3:", 480, y2 += 12, 20, 12);
            e = textZ[0]; e.FontIndex = 4;
            sliderZ = dialog.AddSlider((int)ControlsDialogControlIds.SliderZ, 500, y2, 100, 12);

            StaticText textRx = dialog.AddStatic((int)ControlsDialogControlIds.Static, "4:", 480, y2 += 12, 20, 12);
            e = textRx[0]; e.FontIndex = 4;
            sliderRx = dialog.AddSlider((int)ControlsDialogControlIds.SliderRx, 500, y2, 100, 12);

            StaticText textRy = dialog.AddStatic((int)ControlsDialogControlIds.Static, "5:", 480, y2 += 12, 20, 12);
            e = textRy[0]; e.FontIndex = 4;
            sliderRy = dialog.AddSlider((int)ControlsDialogControlIds.SliderRy, 500, y2, 100, 12);

            StaticText textRz = dialog.AddStatic((int)ControlsDialogControlIds.Static, "6:", 480, y2 += 12, 20, 12);
            e = textRz[0]; e.FontIndex = 4;
            sliderRz = dialog.AddSlider((int)ControlsDialogControlIds.SliderRz, 500, y2, 100, 12);

            StaticText textSlider1 = dialog.AddStatic((int)ControlsDialogControlIds.Static, "7:", 480, y2 += 12, 20, 12);
            e = textSlider1[0]; e.FontIndex = 4;
            sliderSlider1 = dialog.AddSlider((int)ControlsDialogControlIds.SliderSlider1, 500, y2, 100, 12);

            StaticText textSlider2 = dialog.AddStatic((int)ControlsDialogControlIds.Static, "8:", 480, y2 += 12, 20, 12);
            e = textSlider2[0]; e.FontIndex = 4;
            sliderSlider2 = dialog.AddSlider((int)ControlsDialogControlIds.SliderSlider2, 500, y2, 100, 12);


            StaticText textInvert = dialog.AddStatic((int)ControlsDialogControlIds.Static, "invert", 350, y += 40, 100, 20);
            StaticText textExpo = dialog.AddStatic((int)ControlsDialogControlIds.Static, "expo", 390, y, 100, 20);
            
            StaticText textRoll = dialog.AddStatic((int)ControlsDialogControlIds.Static, "Roll (ailerons):", 10, y += 20, 150, 20);
            e = textRoll[0]; e.FontIndex = 3;

            comboRoll = dialog.AddComboBox((int)ControlsDialogControlIds.ComboRoll, 150, y, 200, 20);
            FillChannelCombo(comboRoll);
            checkRollInv = dialog.AddCheckBox((int)ControlsDialogControlIds.CheckRollInv, "", 360, y, 30, 20, false);
            checkRollExp = dialog.AddCheckBox((int)ControlsDialogControlIds.CheckRollExp, "", 390, y, 30, 20, 
                Convert.ToBoolean(Bonsai.Utils.Settings.GetValue("RollExpo", "false")));
            SetChannelValue("aileron", comboRoll, checkRollInv, InputManager.JoyStickAxis.Y);
            comboRoll.Changed += new EventHandler(comboRoll_Changed);
            checkRollInv.Changed += new EventHandler(comboRoll_Changed);
            checkRollExp.Changed += new EventHandler(checkRollExp_Changed);
            
            StaticText textPitch = dialog.AddStatic((int)ControlsDialogControlIds.Static, "Pitch (elevator):", 10, y += 20, 150, 20);
            e = textPitch[0]; e.FontIndex = 3;

            comboPitch = dialog.AddComboBox((int)ControlsDialogControlIds.ComboPitch, 150, y, 200, 20);
            FillChannelCombo(comboPitch);
            checkPitchInv = dialog.AddCheckBox((int)ControlsDialogControlIds.CheckPitchInv, "", 360, y, 30, 20, false);
            checkPitchExp = dialog.AddCheckBox((int)ControlsDialogControlIds.CheckPitchExp, "", 390, y, 30, 20,
                Convert.ToBoolean(Bonsai.Utils.Settings.GetValue("PitchExpo", "false")));
            SetChannelValue("elevator", comboPitch, checkPitchInv, InputManager.JoyStickAxis.X);
            comboPitch.Changed += new EventHandler(comboPitch_Changed);
            checkPitchInv.Changed += new EventHandler(comboPitch_Changed);
            checkPitchExp.Changed += new EventHandler(checkPitchExp_Changed);

            StaticText textYaw = dialog.AddStatic((int)ControlsDialogControlIds.Static, "Yaw (rudder):", 10, y += 20, 150, 20);
            e = textYaw[0]; e.FontIndex = 3;

            comboYaw = dialog.AddComboBox((int)ControlsDialogControlIds.ComboYaw, 150, y, 200, 20);
            FillChannelCombo(comboYaw);
            checkYawInv = dialog.AddCheckBox((int)ControlsDialogControlIds.CheckYawInv, "", 360, y, 30, 20, false);
            checkYawExp = dialog.AddCheckBox((int)ControlsDialogControlIds.CheckYawExp, "", 390, y, 30, 20,
                Convert.ToBoolean(Bonsai.Utils.Settings.GetValue("YawExpo", "false")));
            SetChannelValue("rudder", comboYaw, checkYawInv, InputManager.JoyStickAxis.Ry);
            comboYaw.Changed += new EventHandler(comboYaw_Changed);
            checkYawInv.Changed += new EventHandler(comboYaw_Changed);
            checkYawExp.Changed += new EventHandler(checkYawExp_Changed);
            
            StaticText textThrottle = dialog.AddStatic((int)ControlsDialogControlIds.Static, "Throttle:", 10, y += 20, 150, 20);
            e = textThrottle[0]; e.FontIndex = 3;

            comboThrottle = dialog.AddComboBox((int)ControlsDialogControlIds.ComboThrottle, 150, y, 200, 20);
            FillChannelCombo(comboThrottle);
            checkThrottleInv = dialog.AddCheckBox((int)ControlsDialogControlIds.CheckThrottleInv, "", 360, y, 30, 20, false);
            //checkThrottleExp = dialog.AddCheckBox((int)ControlsDialogControlIds.CheckThrottleExp, "", 390, y, 30, 20,
            //    Convert.ToBoolean(Bonsai.Utils.Settings.GetValue("ThrottleExpo", "false")));
            SetChannelValue("throttle", comboThrottle, checkThrottleInv, InputManager.JoyStickAxis.Rx);
            comboThrottle.Changed += new EventHandler(comboThrottle_Changed);
            checkThrottleInv.Changed += new EventHandler(comboThrottle_Changed);
            //checkThrottleExp.Changed += new EventHandler(checkThrottleExp_Changed);

            StaticText textFlaps = dialog.AddStatic((int)ControlsDialogControlIds.Static, "Toggle flaps:", 10, y += 20, 150, 20);
            e = textFlaps[0]; e.FontIndex = 3;

            comboFlaps = dialog.AddComboBox((int)ControlsDialogControlIds.ComboFlaps, 150, y, 200, 20);
            FillOptionalChannelCombo(comboFlaps);
            SetChannelValue("flaps", comboFlaps, checkThrottleInv, InputManager.JoyStickAxis.None);
            comboFlaps.Changed += new EventHandler(comboFlaps_Changed);

            StaticText textGear = dialog.AddStatic((int)ControlsDialogControlIds.Static, "Toggle gear:", 10, y += 20, 150, 20);
            e = textGear[0]; e.FontIndex = 3;

            comboGear = dialog.AddComboBox((int)ControlsDialogControlIds.ComboGear, 150, y, 200, 20);
            FillOptionalChannelCombo(comboGear);
            SetChannelValue("gear", comboGear, checkThrottleInv, InputManager.JoyStickAxis.None);
            comboGear.Changed += new EventHandler(comboGear_Changed);

            checkUseAilerons = dialog.AddCheckBox((int)ControlsDialogControlIds.CheckUseAileron, "Use aileron channel for rudder on 2/3 channel airplanes", 10, y += 30, 350, 20, 
                Convert.ToBoolean(Bonsai.Utils.Settings.GetValue("UseAileronChannel")));
            checkUseAilerons.Changed += new EventHandler(checkUseAilerons_Changed);

            buttonBack = dialog.AddButton((int)ControlsDialogControlIds.Back, "back to menu", 190, 435, 100, 31);
            buttonBack.Click += new EventHandler(buttonBack_Click);

            buttonOk = dialog.AddButton((int)ControlsDialogControlIds.OK, "back to sim", 350, 435, 100, 31);
            buttonOk.Click += new EventHandler(buttonOk_Click);
        }

        void buttonCalibrate_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("control.exe", "Joy.cpl");
            }
            catch
            {
            }
        }

       

        private void FillChannelCombo(ComboBox combo)
        {
            combo.AddItem("channel 1", 1);
            combo.AddItem("channel 2", 2);
            combo.AddItem("channel 3", 3);
            combo.AddItem("channel 4", 4);
            combo.AddItem("channel 5", 5);
            combo.AddItem("channel 6", 6);
            combo.AddItem("channel 7", 7);
            combo.AddItem("channel 8", 8);
        }

        private void FillOptionalChannelCombo(ComboBox combo)
        {
            FillChannelCombo(combo);
            combo.AddItem("none", 9);
        }

        private void SetChannelValue(string function, ComboBox combo, Checkbox check, InputManager.JoyStickAxis defaultAxis)
        {
            bool inverted = false;
            InputManager.JoyStickAxis axis = owner.InputManager.GetAxis(function, out inverted);
            switch (axis)
            {
                case InputManager.JoyStickAxis.X:
                    combo.SetSelectedByData(1);
                    break;
                case InputManager.JoyStickAxis.Y:
                    combo.SetSelectedByData(2);
                    break;
                case InputManager.JoyStickAxis.Z:
                    combo.SetSelectedByData(3);
                    break;
                case InputManager.JoyStickAxis.Rx:
                    combo.SetSelectedByData(4);
                    break;
                case InputManager.JoyStickAxis.Ry:
                    combo.SetSelectedByData(5);
                    break;
                case InputManager.JoyStickAxis.Rz:
                    combo.SetSelectedByData(6);
                    break;
                case InputManager.JoyStickAxis.Slider1:
                    combo.SetSelectedByData(7);
                    break;
                case InputManager.JoyStickAxis.Slider2:
                    combo.SetSelectedByData(8);
                    break;
                case InputManager.JoyStickAxis.None:
                    combo.SetSelectedByData(9);
                    break;
            }
            check.IsChecked = inverted;
        }
        
        private void UpdateSetting(string function, ComboBox combo, Checkbox check)
        {
            switch ((int)(combo.GetSelectedData()))
            {
                case 1:
                    owner.InputManager.SetAxis(function, InputManager.JoyStickAxis.X, check.IsChecked);
                    break;
                case 2:
                    owner.InputManager.SetAxis(function, InputManager.JoyStickAxis.Y, check.IsChecked);
                    break;
                case 3:
                    owner.InputManager.SetAxis(function, InputManager.JoyStickAxis.Z, check.IsChecked);
                    break;
                case 4:
                    owner.InputManager.SetAxis(function, InputManager.JoyStickAxis.Rx, check.IsChecked);
                    break;
                case 5:
                    owner.InputManager.SetAxis(function, InputManager.JoyStickAxis.Ry, check.IsChecked);
                    break;
                case 6:
                    owner.InputManager.SetAxis(function, InputManager.JoyStickAxis.Rz, check.IsChecked);
                    break;
                case 7:
                    owner.InputManager.SetAxis(function, InputManager.JoyStickAxis.Slider1, check.IsChecked);
                    break;
                case 8:
                    owner.InputManager.SetAxis(function, InputManager.JoyStickAxis.Slider2, check.IsChecked);
                    break;
                case 9:
                    owner.InputManager.SetAxis(function, InputManager.JoyStickAxis.None, check.IsChecked);
                    break;
            }
        }
        #endregion

        #region Private event handlers
        /*
        void checkThrottleExp_Changed(object sender, EventArgs e)
        {
            Bonsai.Utils.Settings.SetValue("ThrottleExpo", checkThrottleExp.IsChecked.ToString());
        }
         */

        void checkYawExp_Changed(object sender, EventArgs e)
        {
            Bonsai.Utils.Settings.SetValue("YawExpo", checkYawExp.IsChecked.ToString());
        }

        void checkPitchExp_Changed(object sender, EventArgs e)
        {
            Bonsai.Utils.Settings.SetValue("PitchExpo", checkPitchExp.IsChecked.ToString());
        }

        void checkRollExp_Changed(object sender, EventArgs e)
        {
            Bonsai.Utils.Settings.SetValue("RollExpo", checkRollExp.IsChecked.ToString());
        }

        void checkUseAilerons_Changed(object sender, EventArgs e)
        {
            Bonsai.Utils.Settings.SetValue("UseAileronChannel", checkUseAilerons.IsChecked.ToString());
            Player.UseAileronForRudder = checkUseAilerons.IsChecked;
        }

        void comboThrottle_Changed(object sender, EventArgs e)
        {
            UpdateSetting("throttle", comboThrottle, checkThrottleInv);
        }

        void comboYaw_Changed(object sender, EventArgs e)
        {
            UpdateSetting("rudder", comboYaw, checkYawInv);
        }

        void comboPitch_Changed(object sender, EventArgs e)
        {
            UpdateSetting("elevator", comboPitch, checkPitchInv);
        }

        void comboRoll_Changed(object sender, EventArgs e)
        {
            UpdateSetting("aileron", comboRoll, checkRollInv);
        }

        void comboFlaps_Changed(object sender, EventArgs e)
        {
            UpdateSetting("flaps", comboFlaps, checkRollInv);
        }

        void comboGear_Changed(object sender, EventArgs e)
        {
            UpdateSetting("gear", comboGear, checkRollInv);
        }
        #endregion

        public override void OnShowDialog()
        {
            base.OnShowDialog();

            timer.Interval = 100;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Enabled = true;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            owner.InputManager.Update();
            sliderX.Value = owner.InputManager.JoyStickState.X / 2 + 50;
            sliderY.Value = owner.InputManager.JoyStickState.Y / 2 + 50;
            sliderZ.Value = owner.InputManager.JoyStickState.Z / 2 + 50;
            sliderRx.Value = owner.InputManager.JoyStickState.Rx / 2 + 50;
            sliderRy.Value = owner.InputManager.JoyStickState.Ry / 2 + 50;
            sliderRz.Value = owner.InputManager.JoyStickState.Rz / 2 + 50;
            if (owner.InputManager.JoyStickState.GetSlider() != null && owner.InputManager.JoyStickState.GetSlider().GetLength(0) > 0)
                sliderSlider1.Value = owner.InputManager.JoyStickState.GetSlider()[0] / 2 + 50;
            if (owner.InputManager.JoyStickState.GetSlider() != null && owner.InputManager.JoyStickState.GetSlider().GetLength(0) > 1)
                sliderSlider2.Value = owner.InputManager.JoyStickState.GetSlider()[1] / 2 + 50;
        }

        #region Event handlers for the controls
        void buttonOk_Click(object sender, EventArgs e)
        {
            owner.Player.FlightModel.Paused = false;
            timer.Enabled = false;
            Dialog.SetRefreshTime((float)FrameworkTimer.GetTime());
            parent.HideDialog();
        }

        void buttonBack_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
            parent.HideDialog();
            owner.ShowMenu();
        }

        void comboJoysticks_Changed(object sender, EventArgs e)
        {
            if (comboJoysticks.GetSelectedData() != null)
            {
                owner.InputManager.AcquireJoystick(comboJoysticks.GetSelectedData().ToString());
            }
        }
        #endregion
    }
}
