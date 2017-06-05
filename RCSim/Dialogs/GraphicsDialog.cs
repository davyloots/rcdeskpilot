using System;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Bonsai.Core.Controls;
using Bonsai.Core;
using Bonsai.Core.Dialogs;
using Bonsai.Utils;


namespace RCSim.Dialogs
{
    #region Control Ids
    public enum GraphicsDialogControlIds
    {
        Static = -1,
        None,
        OK,
        Back,
        SmokeDetail,
        SceneryDetail,
        WaterDetail,
        WaterRipplesDetail,
        ReflectionDetail,
        Resolution,
        Compass,
        Enable3D
    }
    #endregion

    class GraphicsDialog : DialogBase
    {
        #region Private fields
        private Program owner;
        private DeviceSettings globalSettings; // Device settings

        private Button buttonBack;
        private Button buttonOk;
        private ComboBox comboSmoke;
        private ComboBox comboScenery;
        private ComboBox comboWater;
        private ComboBox comboWaterRipples;
        private ComboBox comboReflection;
        private ComboBox comboResolution;
        private Checkbox checkCompass;
        private Checkbox checkEnable3D;
        private bool changed = false;
        private bool fullscreen = false;
        private int resX = 0;
        private int resY = 0;
        #endregion

        #region Creation
        /// <summary>Creates a new settings dialog</summary>
        public GraphicsDialog(Framework framework, Program owner)
            : base(framework)
        {
            this.owner = owner;
            
            // Get some information
            globalSettings = parent.DeviceSettings.Clone();

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
            StaticText title = dialog.AddStatic((int)GraphicsDialogControlIds.Static, "Graphics settings", 10, 5, 400, 30);
            e = title[0];
            e.FontIndex = 1;
            e.textFormat = DrawTextFormat.Top | DrawTextFormat.Left;

            StaticText smokeText = dialog.AddStatic((int)GraphicsDialogControlIds.Static, "Smoke detail", 10, y += 30, 200, 20);
            e = smokeText[0];
            e.FontIndex = 3;

            comboSmoke = dialog.AddComboBox((int)GraphicsDialogControlIds.SmokeDetail, 200, y, 100, 20);
            comboSmoke.AddItem("low", "1"); comboSmoke.AddItem("medium", "2"); comboSmoke.AddItem("high", "3");
            comboSmoke.SetSelectedByData(Bonsai.Utils.Settings.GetValue("SmokeDetail", "2"));
            comboSmoke.Changed += new EventHandler(comboSmoke_Changed);

            StaticText sceneryText = dialog.AddStatic((int)GraphicsDialogControlIds.Static, "Scenery detail", 10, y += 30, 200, 20);
            e = sceneryText[0];
            e.FontIndex = 3;

            comboScenery = dialog.AddComboBox((int)GraphicsDialogControlIds.SceneryDetail, 200, y, 100, 20);
            comboScenery.AddItem("low", "1"); comboScenery.AddItem("high", "2");
            comboScenery.SetSelectedByData(Bonsai.Utils.Settings.GetValue("SceneryDetail", "2"));
            comboScenery.Changed += new EventHandler(comboScenery_Changed);

            StaticText waterText = dialog.AddStatic((int)GraphicsDialogControlIds.Static, "Water detail", 10, y += 30, 200, 20);
            e = waterText[0];
            e.FontIndex = 3;

            comboWater = dialog.AddComboBox((int)GraphicsDialogControlIds.WaterDetail, 200, y, 100, 20);
            comboWater.AddItem("low", "1"); comboWater.AddItem("medium", "2"); comboWater.AddItem("high", "3");
            comboWater.SetSelectedByData(Bonsai.Utils.Settings.GetValue("WaterDetail", "2"));
            comboWater.Changed += new EventHandler(comboWater_Changed);

            StaticText waterRipplesText = dialog.AddStatic((int)GraphicsDialogControlIds.Static, "Plane/Water interaction", 10, y += 30, 200, 20);
            e = waterText[0];
            e.FontIndex = 3;

            comboWaterRipples = dialog.AddComboBox((int)GraphicsDialogControlIds.WaterRipplesDetail, 200, y, 100, 20);
            comboWaterRipples.AddItem("off", "0"); comboWaterRipples.AddItem("on", "2");
            comboWaterRipples.SetSelectedByData(Bonsai.Utils.Settings.GetValue("WaterRipplesDetail", "2"));
            comboWaterRipples.Changed += new EventHandler(comboWaterRipples_Changed);

            StaticText reflectionText = dialog.AddStatic((int)GraphicsDialogControlIds.ReflectionDetail, "Reflection detail", 10, y += 30, 200, 20);
            e = reflectionText[0];
            e.FontIndex = 3;

            comboReflection = dialog.AddComboBox((int)GraphicsDialogControlIds.ReflectionDetail, 200, y, 100, 20);
            comboReflection.AddItem("off", "0"); comboReflection.AddItem("low", "1"); comboReflection.AddItem("high", "2");
            comboReflection.SetSelectedByData(Bonsai.Utils.Settings.GetValue("ReflectionDetail", "1"));
            comboReflection.Changed += new EventHandler(comboReflection_Changed);

            StaticText resolutionText = dialog.AddStatic((int)GraphicsDialogControlIds.Static, "Full screen resolution", 10, y += 30, 200, 20);
            e = resolutionText[0];
            e.FontIndex = 3;

            comboResolution = dialog.AddComboBox((int)GraphicsDialogControlIds.Resolution, 200, y, 100, 20);
            InitResolutions();
            comboResolution.Changed += new EventHandler(comboResolution_Changed);

            checkCompass = dialog.AddCheckBox((int)GraphicsDialogControlIds.Compass,
                "Enable compass", 10, y += 30, 400, 20, Convert.ToBoolean(Settings.GetValue("CompassVisible", "true")));
            checkCompass.Changed += new EventHandler(checkCompass_Changed);

            checkEnable3D = dialog.AddCheckBox((int)GraphicsDialogControlIds.Enable3D, 
                "Enable anaglyph 3D (requires red/cyan glasses)", 10, y += 30, 400, 20, owner.Anaglyph);
            checkEnable3D.Changed += new EventHandler(checkEnable3D_Changed);

            buttonBack = dialog.AddButton((int)GraphicsDialogControlIds.Back, "back to menu", 190, 435, 100, 31);
            buttonBack.Click += new EventHandler(buttonBack_Click);

            buttonOk = dialog.AddButton((int)GraphicsDialogControlIds.OK, "back to sim", 350, 435, 100, 31);
            buttonOk.Click += new EventHandler(buttonOk_Click);            
        }

        void checkCompass_Changed(object sender, EventArgs e)
        {
            Settings.SetValue("CompassVisible", checkCompass.IsChecked.ToString());
            owner.CenterHud.MapVisible = checkCompass.IsChecked;
        }

        void checkEnable3D_Changed(object sender, EventArgs e)
        {
            owner.Anaglyph = checkEnable3D.IsChecked;
        }

        void comboResolution_Changed(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;

            if (cb.GetSelectedData() == null)
            {
                if (!globalSettings.presentParams.Windowed)
                {
                    // If windowed, get the appropriate adapter format from Direct3D
                    globalSettings.presentParams.Windowed = true;
                    DisplayMode mode = Manager.Adapters[(int)globalSettings.AdapterOrdinal].CurrentDisplayMode;
                    globalSettings.AdapterFormat = mode.Format;
                    globalSettings.presentParams.BackBufferWidth = mode.Width;
                    globalSettings.presentParams.BackBufferHeight = mode.Height;
                    globalSettings.presentParams.FullScreenRefreshRateInHz = mode.RefreshRate;
                    fullscreen = false;
                    resX = mode.Width;
                    resY = mode.Height;
                    changed = true;
                }
            }
            else
            {
                globalSettings.presentParams.Windowed = false;
                // Set the resolution
                uint data = (uint)cb.GetSelectedData();
                resX = NativeMethods.LoWord(data);
                resY = NativeMethods.HiWord(data);
                globalSettings.presentParams.BackBufferWidth = resX;
                globalSettings.presentParams.BackBufferHeight = resY;
                fullscreen = true;                
                changed = true;
            }
        }

        public override void OnShowDialog()
        {
            changed = false;
            InitResolutions();
            base.OnShowDialog();
        }


        #region Private methods
        private void InitResolutions()
        {
            // Resolutions
            comboResolution.Clear();

            // Add the windowed option.
            comboResolution.AddItem("Windowed", null);

            EnumAdapterInformation adapterInfo = Enumeration.GetAdapterInformation(globalSettings.AdapterOrdinal);
            foreach (DisplayMode dm in adapterInfo.displayModeList)
            {
                if ((dm.Format == globalSettings.AdapterFormat) && (dm.Height >= 480))
                    AddResolution((short)dm.Width, (short)dm.Height);
            }

            if (globalSettings.presentParams.Windowed)
                comboResolution.SetSelected(0);
            else
                comboResolution.SetSelectedByData(NativeMethods.MakeUInt32(
                    (short)globalSettings.presentParams.BackBufferWidth, (short)globalSettings.presentParams.BackBufferHeight));
        }

        /// <summary>Adds a resolution to the combo box</summary>
        private void AddResolution(short width, short height)
        {
            string itemText = string.Format("{0} x {1}", width, height);
            // Store the resolution in a single variable
            uint resolutionData = NativeMethods.MakeUInt32(width, height);

            // Add this item
            if (!comboResolution.ContainsItem(itemText))
                comboResolution.AddItem(itemText, resolutionData);
        }
        #endregion

        #region Event handlers for the controls
        void buttonOk_Click(object sender, EventArgs e)
        {
            owner.Player.FlightModel.Paused = false;

            Dialog.SetRefreshTime((float)FrameworkTimer.GetTime());

            if (changed)
            {
                // The device needs to be updated
                if (globalSettings.presentParams.Windowed)
                {
                    globalSettings.presentParams.FullScreenRefreshRateInHz = 0;
                    globalSettings.presentParams.BackBufferWidth = (int)windowWidth;
                    globalSettings.presentParams.BackBufferHeight = (int)windowHeight;
                }

                if (globalSettings.presentParams.MultiSample != MultiSampleType.None)
                {
                    globalSettings.presentParams.PresentFlag &= ~PresentFlag.LockableBackBuffer;
                }

                // Save settings
                Bonsai.Utils.Settings.SetValue("FullScreen", fullscreen.ToString());
                if (fullscreen)
                {
                    Bonsai.Utils.Settings.SetValue("ResolutionWidth", resX.ToString());
                    Bonsai.Utils.Settings.SetValue("ResolutionHeight", resY.ToString());
                }

                // Create a device
                parent.CreateDeviceFromSettings(globalSettings);
            }
            parent.HideDialog();
        }


        void buttonBack_Click(object sender, EventArgs e)
        {
            if (changed)
            {
                // The device needs to be updated
                if (globalSettings.presentParams.Windowed)
                {
                    globalSettings.presentParams.FullScreenRefreshRateInHz = 0;
                    globalSettings.presentParams.BackBufferWidth = (int)windowWidth;
                    globalSettings.presentParams.BackBufferHeight = (int)windowHeight;
                }

                if (globalSettings.presentParams.MultiSample != MultiSampleType.None)
                {
                    globalSettings.presentParams.PresentFlag &= ~PresentFlag.LockableBackBuffer;
                }

                // Save settings
                Bonsai.Utils.Settings.SetValue("FullScreen", fullscreen.ToString());
                if (fullscreen)
                {
                    Bonsai.Utils.Settings.SetValue("ResolutionWidth", resX.ToString());
                    Bonsai.Utils.Settings.SetValue("ResolutionHeight", resY.ToString());
                }

                // Create a device
                parent.CreateDeviceFromSettings(globalSettings);
            }
            parent.HideDialog();
            owner.ShowMenu();
        }

        void comboSmoke_Changed(object sender, EventArgs e)
        {
            Bonsai.Utils.Settings.SetValue("SmokeDetail", comboSmoke.GetSelectedData().ToString());
        }

        void comboScenery_Changed(object sender, EventArgs e)
        {
            Bonsai.Utils.Settings.SetValue("SceneryDetail", comboScenery.GetSelectedData().ToString());
        }


        void comboWater_Changed(object sender, EventArgs e)
        {
            Bonsai.Utils.Settings.SetValue("WaterDetail", comboWater.GetSelectedData().ToString());
        }

        void comboWaterRipples_Changed(object sender, EventArgs e)
        {
            Bonsai.Utils.Settings.SetValue("WaterRipplesDetail", comboWaterRipples.GetSelectedData().ToString());
        }


        void comboReflection_Changed(object sender, EventArgs e)
        {
            Bonsai.Utils.Settings.SetValue("ReflectionDetail", comboReflection.GetSelectedData().ToString());
            int detailInt = Convert.ToInt32(Settings.GetValue("ReflectionDetail", "2"));
            switch (detailInt)
            {
                case 0:
                    Effects.Reflection.ReflectionDetail = RCSim.Effects.Reflection.ReflectionDetailEnum.Off;
                    break;
                case 1:
                    Effects.Reflection.ReflectionDetail = RCSim.Effects.Reflection.ReflectionDetailEnum.Low;
                    break;
                case 2:
                    Effects.Reflection.ReflectionDetail = RCSim.Effects.Reflection.ReflectionDetailEnum.High;
                    break;
            }            
        }
        #endregion
    }
}
