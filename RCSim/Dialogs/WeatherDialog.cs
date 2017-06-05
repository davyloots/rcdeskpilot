using System;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Bonsai.Core.Controls;
using Bonsai.Core;
using Bonsai.Core.Dialogs;
using System.Data;
using System.Drawing;

namespace RCSim.Dialogs
{
    #region Control Ids
    public enum WeatherDialogControlIds
    {
        Static = -1,
        None,
        OK,
        Back,
        WeatherType,
        Custom,
        WindSpeed,
        WindSpeedName,
        GustType,
        GustStrength,
        GustStrengthName,
        GustVariability,
        GustVariabilityName,
        GustFrequency,
        GustFrequencyName,
        Turbulence,
        ThermalSize,
        ThermalStrength,
        DownDrafts,
        WindDirection,
        WindDirectionName,
        Sky,
        FredButton
    }
    #endregion

    /// <summary>
    /// Dialog for selection of device settings 
    /// </summary>
    class WeatherDialog : DialogBase
    {
        #region Private fields
        private Program owner;

        private Button buttonBack;
        private Button buttonOk;
        private ComboBox comboWeatherType;
        private Checkbox checkCustom;
        private Slider sliderWindSpeed;
        private StaticText textWindSpeed;
        private Slider sliderWindDirection;
        private StaticText currentWindDirText;
        private Slider sliderGustStrength;
        private StaticText textGustStrength;
        private Slider sliderGustVariability;
        private StaticText textGustVariability;
        private Slider sliderGustFrequency;
        private StaticText textGustFrequency;
        private Slider sliderTurbulence;
        private StaticText textTurbulence;
        private Slider sliderThermalSize;
        private StaticText textThermalSize;
        private Slider sliderThermalStrength;
        private StaticText textThermalStrength;
        private Slider sliderDownDrafts;
        private StaticText textSky;
        private ComboBox comboSky;
        private StaticText textDownDrafts;
        private StaticText windSpeedText;
        private StaticText windDirText;
        //private StaticText gustTypeText;
        private StaticText gustText;
        private StaticText gustVarText;
        private StaticText gustFreqText;
        private StaticText turbulenceText;
        private StaticText thermalSizeText;
        private StaticText thermalStrengthText;
        private StaticText downDraftsText;
        private Button buttonFred;
        #endregion

        #region Creation
        /// <summary>Creates a new settings dialog</summary>
        public WeatherDialog(Framework framework, Program owner)
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
            StaticText title = dialog.AddStatic((int)WeatherDialogControlIds.Static, "Flight Conditions", 10, 5, 400, 30);
            e = title[0];
            e.FontIndex = 1;
            e.textFormat = DrawTextFormat.Top | DrawTextFormat.Left;

            StaticText weatherTypeText = dialog.AddStatic((int)WeatherDialogControlIds.Static, "Presets", 10, y += 30, 160, 30);
            e = weatherTypeText[0];
            e.FontIndex = 2;

            comboWeatherType = dialog.AddComboBox((int)WeatherDialogControlIds.WeatherType, 180, y, 250, 30);
            comboWeatherType.AddItem("No weather", "none");
            comboWeatherType.AddItem("Sunrise, light airs", "sunriselightairs");
            comboWeatherType.AddItem("Sunrise, breezy", "sunrisebreezy");
            comboWeatherType.AddItem("Sunrise, windy", "sunrisewindy");
            comboWeatherType.AddItem("Late morning, light airs", "latemorninglightairs");
            comboWeatherType.AddItem("Late morning, breezy", "latemorningbreezy");
            comboWeatherType.AddItem("Late morning, windy", "latemorningwindy");
            comboWeatherType.AddItem("Mid afternoon, light airs", "midafternoonlightairs");
            comboWeatherType.AddItem("Mid afternoon, breezy", "midafternoonbreezy");
            comboWeatherType.AddItem("Mid afternoon, windy", "midafternoonwindy");
            comboWeatherType.AddItem("Sunset, light airs", "eveninglightairs");
            comboWeatherType.AddItem("Sunset, breezy", "eveningbreezy");
            comboWeatherType.AddItem("Sunset, windy", "eveningwindy");
            comboWeatherType.AddItem("Moonlight, light airs", "midnightlightairs");
            comboWeatherType.AddItem("Moonlight, breezy", "midnightbreezy");
            comboWeatherType.AddItem("Moonlight, windy", "midnightwindy");
            comboWeatherType.Changed += new EventHandler(comboWeatherType_Changed);

            checkCustom = dialog.AddCheckBox((int)WeatherDialogControlIds.Custom, "Customize", 10, y += 30, 160, 20, false);
            checkCustom.Changed += new EventHandler(checkCustom_Changed);

            int ySpace = 24;

            // Wind speed
            windSpeedText = dialog.AddStatic((int)WeatherDialogControlIds.Static, "Wind speed", 40, y += ySpace, 160, ySpace);
            e = windSpeedText[0];
            e.FontIndex = 3;
     
            sliderWindSpeed = dialog.AddSlider((int)WeatherDialogControlIds.WindSpeed, 200, y + 5, 100, 20);
            sliderWindSpeed.ValueChanged += new EventHandler(sliderWindSpeed_ValueChanged);

            textWindSpeed = dialog.AddStatic((int)WeatherDialogControlIds.WindSpeedName, "0 kph", 320, y, 200, ySpace);
            e = textWindSpeed[0];
            e.FontIndex = 3;
            
            // Wind direction
            windDirText = dialog.AddStatic((int)WeatherDialogControlIds.Static, "Wind direction", 40, y += ySpace, 160, ySpace);
            e = windDirText[0];
            e.FontIndex = 3;
            
            sliderWindDirection = dialog.AddSlider((int)WeatherDialogControlIds.WindDirection, 200, y + 5, 100, 20);
            sliderWindDirection.ValueChanged += new EventHandler(sliderWindDirection_ValueChanged);

            currentWindDirText = dialog.AddStatic((int)WeatherDialogControlIds.WindDirectionName, "west", 320, y, 200, ySpace);
            e = windDirText[0];
            e.FontIndex = 3;

            gustText = dialog.AddStatic((int)WeatherDialogControlIds.Static, "Gust strength", 40, y += ySpace, 160, ySpace);
            e = gustText[0];
            e.FontIndex = 3;

            sliderGustStrength = dialog.AddSlider((int)WeatherDialogControlIds.GustStrength, 200, y + 5, 100, 20);
            sliderGustStrength.ValueChanged += new EventHandler(sliderGustStrength_ValueChanged);

            textGustStrength = dialog.AddStatic((int)WeatherDialogControlIds.Static, "0 kph", 320, y, 200, ySpace);
            e = textGustStrength[0];
            e.FontIndex = 3;

            gustVarText = dialog.AddStatic((int)WeatherDialogControlIds.Static, "Gust variability", 40, y += ySpace, 160, ySpace);
            e = gustVarText[0];
            e.FontIndex = 3;

            sliderGustVariability = dialog.AddSlider((int)WeatherDialogControlIds.GustVariability, 200, y + 5, 100, 20);
            sliderGustVariability.ValueChanged += new EventHandler(sliderGustVariability_ValueChanged);

            textGustVariability = dialog.AddStatic((int)WeatherDialogControlIds.Static, "very predictable", 320, y, 200, ySpace);
            e = textGustVariability[0];
            e.FontIndex = 3;

            gustFreqText = dialog.AddStatic((int)WeatherDialogControlIds.Static, "Gust frequency", 40, y += ySpace, 160, ySpace);
            e = gustFreqText[0];
            e.FontIndex = 3;

            sliderGustFrequency = dialog.AddSlider((int)WeatherDialogControlIds.GustFrequency, 200, y + 5, 100, 20);
            sliderGustFrequency.ValueChanged += new EventHandler(sliderGustFrequency_ValueChanged);

            textGustFrequency = dialog.AddStatic((int)WeatherDialogControlIds.Static, "low", 320, y, 200, ySpace);
            e = textGustFrequency[0];
            e.FontIndex = 3;

            turbulenceText = dialog.AddStatic((int)WeatherDialogControlIds.Static, "Turbulence", 40, y += ySpace, 160, ySpace);
            e = gustFreqText[0];
            e.FontIndex = 3;

            sliderTurbulence = dialog.AddSlider((int)WeatherDialogControlIds.Turbulence, 200, y + 5, 100, 20);
            sliderTurbulence.ValueChanged += new EventHandler(sliderTurbulence_ValueChanged);

            textTurbulence = dialog.AddStatic((int)WeatherDialogControlIds.Static, "none", 320, y, 200, ySpace);
            e = textTurbulence[0];
            e.FontIndex = 3;

            thermalSizeText = dialog.AddStatic((int)WeatherDialogControlIds.Static, "Thermal size", 40, y += ySpace, 160, ySpace);
            e = thermalSizeText[0];
            e.FontIndex = 3;

            sliderThermalSize = dialog.AddSlider((int)WeatherDialogControlIds.ThermalSize, 200, y + 5, 100, 20);
            sliderThermalSize.ValueChanged += new EventHandler(sliderThermalSize_ValueChanged);
            
            textThermalSize = dialog.AddStatic((int)WeatherDialogControlIds.ThermalSize, "small", 320, y, 200, ySpace);
            e = textThermalSize[0];
            e.FontIndex = 3;

            thermalStrengthText = dialog.AddStatic((int)WeatherDialogControlIds.Static, "Thermal strength", 40, y += ySpace, 160, ySpace);
            e = thermalStrengthText[0];
            e.FontIndex = 3;

            sliderThermalStrength = dialog.AddSlider((int)WeatherDialogControlIds.ThermalSize, 200, y + 5, 100, 20);
            sliderThermalStrength.ValueChanged += new EventHandler(sliderThermalStrength_ValueChanged);

            textThermalStrength = dialog.AddStatic((int)WeatherDialogControlIds.Static, "off", 320, y, 200, ySpace);
            e = textThermalStrength[0];
            e.FontIndex = 3;

            downDraftsText = dialog.AddStatic((int)WeatherDialogControlIds.Static, "Downdrafts", 40, y += ySpace, 160, ySpace);
            e = downDraftsText[0];
            e.FontIndex = 3;

            sliderDownDrafts = dialog.AddSlider((int)WeatherDialogControlIds.DownDrafts, 200, y + 5, 100, 20);
            sliderDownDrafts.ValueChanged += new EventHandler(sliderDownDrafts_ValueChanged);

            textDownDrafts = dialog.AddStatic((int)WeatherDialogControlIds.Static, "off", 320, y, 200, ySpace);
            e = textDownDrafts[0];
            e.FontIndex = 3;

            textSky = dialog.AddStatic((int)WeatherDialogControlIds.Static, "Sky:", 40, y += 30, 180, ySpace);
            e = textSky[0];
            e.FontIndex = 3;

            comboSky = dialog.AddComboBox((int)WeatherDialogControlIds.Sky, 200, y, 200, ySpace);
            comboSky.Changed += new EventHandler(comboSky_Changed);

            //buttonFred = dialog.AddButton((int)WeatherDialogControlIds.FredButton, "Fred button", 10, y += 30, 120, 30);
            //buttonFred.Click += new EventHandler(buttonFred_Click);

            sliderWindSpeed.Value = Convert.ToInt32(Bonsai.Utils.Settings.GetValue("WindSpeed", "0"));
            sliderWindSpeed_ValueChanged(this, EventArgs.Empty);
            sliderWindDirection.Value = Convert.ToInt32(Bonsai.Utils.Settings.GetValue("WindDirection", "0"));
            sliderWindDirection_ValueChanged(this, EventArgs.Empty);
            sliderGustStrength.Value = Convert.ToInt32(Bonsai.Utils.Settings.GetValue("GustStrength", "0"));
            sliderGustStrength_ValueChanged(this, EventArgs.Empty);
            sliderGustVariability.Value = Convert.ToInt32(Bonsai.Utils.Settings.GetValue("GustVariability", "0"));
            sliderGustVariability_ValueChanged(this, EventArgs.Empty);
            sliderGustFrequency.Value = Convert.ToInt32(Bonsai.Utils.Settings.GetValue("GustFrequency", "0"));
            sliderGustFrequency_ValueChanged(this, EventArgs.Empty);
            sliderTurbulence.Value = Convert.ToInt32(Bonsai.Utils.Settings.GetValue("Turbulence", "0"));
            sliderTurbulence_ValueChanged(this, EventArgs.Empty);
            sliderThermalSize.Value = Convert.ToInt32(Bonsai.Utils.Settings.GetValue("ThermalSize", "50"));
            sliderThermalSize_ValueChanged(this, EventArgs.Empty);
            sliderThermalStrength.Value = Convert.ToInt32(Bonsai.Utils.Settings.GetValue("ThermalStrength", "50"));
            sliderThermalStrength_ValueChanged(this, EventArgs.Empty);
            sliderDownDrafts.Value = Convert.ToInt32(Bonsai.Utils.Settings.GetValue("DownDrafts", "0"));
            sliderDownDrafts_ValueChanged(this, EventArgs.Empty);
                 
            try
            {
                checkCustom.IsChecked = Convert.ToBoolean(Bonsai.Utils.Settings.GetValue("customweather", "false"));
                comboWeatherType.SetSelectedByData(Bonsai.Utils.Settings.GetValue("weathertype", "none"));
            }
            catch
            {
            }

            SetControls(checkCustom.IsChecked);

            buttonBack = dialog.AddButton((int)WeatherDialogControlIds.Back, "back to menu", 190, 435, 100, 31);
            buttonBack.Click += new EventHandler(buttonBack_Click);

            buttonOk = dialog.AddButton((int)WeatherDialogControlIds.OK, "back to sim", 350, 435, 100, 31);
            buttonOk.Click += new EventHandler(buttonOk_Click);            
        }

        

        void buttonFred_Click(object sender, EventArgs e)
        {
            FredDialog dialog = new FredDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filename = dialog.NameResult;
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filename+".weather"))
                {
                    file.WriteLine("windspeed=" + sliderWindSpeed.Value);
                    file.WriteLine("direction=" + sliderWindDirection.Value);
                    //file.WriteLine("gusttype="+comboGustType.GetSelectedData().ToString());
                    file.WriteLine("guststrength="+sliderGustStrength.Value);
                    file.WriteLine("gustvariability=" + sliderGustVariability.Value);
                    file.WriteLine("gustfrequency=" + sliderGustFrequency.Value);
                    file.WriteLine("thermalsize="+sliderThermalSize.Value);
                    file.WriteLine("thermalstrength="+sliderThermalStrength.Value);
                    file.WriteLine("downdrafts="+sliderDownDrafts.Value);
                    file.WriteLine("turbulence=" + sliderTurbulence.Value);
                }
                System.Windows.Forms.MessageBox.Show("The preset has been saved to your sim folder!");
            }
        }

        
        void checkCustom_Changed(object sender, EventArgs e)
        {
            Bonsai.Utils.Settings.SetValue("customweather", checkCustom.IsChecked.ToString());
            SetControls(checkCustom.IsChecked);
        }

        private void SetControls(bool enabled)
        {
            windSpeedText.IsEnabled = enabled;
            textWindSpeed.IsEnabled = enabled;
            sliderWindSpeed.IsEnabled = enabled;
            windDirText.IsEnabled = enabled;
            sliderWindDirection.IsEnabled = enabled;
            currentWindDirText.IsEnabled = enabled;
            //gustTypeText.IsEnabled = enabled;
            //comboGustType.IsEnabled = enabled;
            gustText.IsEnabled = enabled;
            sliderGustStrength.IsEnabled = enabled;
            textGustStrength.IsEnabled = enabled;
            gustVarText.IsEnabled = enabled;
            sliderGustVariability.IsEnabled = enabled;
            textGustVariability.IsEnabled = enabled;
            gustFreqText.IsEnabled = enabled;
            sliderGustFrequency.IsEnabled = enabled;
            textGustFrequency.IsEnabled = enabled;
            turbulenceText.IsEnabled = enabled;
            sliderTurbulence.IsEnabled = enabled;
            textTurbulence.IsEnabled = enabled;
            thermalSizeText.IsEnabled = enabled;
            sliderThermalSize.IsEnabled = enabled;
            textThermalSize.IsEnabled = enabled;
            thermalStrengthText.IsEnabled = enabled;
            sliderThermalStrength.IsEnabled = enabled;
            textThermalStrength.IsEnabled = enabled;
            downDraftsText.IsEnabled = enabled;
            sliderDownDrafts.IsEnabled = enabled;
            textDownDrafts.IsEnabled = enabled;
            textSky.IsEnabled = enabled;
            comboSky.IsEnabled = enabled;
        }

        void comboSky_Changed(object sender, EventArgs e)
        {
            DataRow dataRow = comboSky.GetSelectedData() as DataRow;
            if (dataRow != null)
            {
                string skyName = dataRow["Name"].ToString();
                Bonsai.Utils.Settings.SetValue("Sky", skyName);
                Vector3 ambientVector = (Vector3)dataRow["AmbientLight"];
                Vector3 sunVector = (Vector3)dataRow["SunLight"];
                Program.Instance.Scenery.SetSky(dataRow["Texture"].ToString(), (Vector3)dataRow["SunPosition"],
                    Color.FromArgb((int)(255 * ambientVector.X), (int)(255 * ambientVector.Y), (int)(255 * ambientVector.Z)),
                    Color.FromArgb((int)(255 * sunVector.X), (int)(255 * sunVector.Y), (int)(255 * sunVector.Z)),
                    (float)(dataRow["TerrainAmbient"]), (float)(dataRow["TerrainSun"]));                
            }
        }

        void sliderDownDrafts_ValueChanged(object sender, EventArgs e)
        {
            owner.Weather.Wind.DownDrafts = sliderDownDrafts.Value / 100.0;
            if (sliderDownDrafts.Value == 0)
                textDownDrafts.SetText("off");
            else if (sliderDownDrafts.Value < 60)
                textDownDrafts.SetText("less than thermals");
            else 
                textDownDrafts.SetText("realistic");
        }        

        void comboWeatherType_Changed(object sender, EventArgs e)
        {
            string weatherType = comboWeatherType.GetSelectedData() as string;
            Bonsai.Utils.Settings.SetValue("weathertype", weatherType);
            switch (weatherType)
            {
                case "none":
                    sliderWindSpeed.Value = 0;
                    //sliderWindDirection.Value = 0;                    
                    sliderGustStrength.Value = 0;
                    sliderGustVariability.Value = 0;
                    sliderGustFrequency.Value = 0;
                    sliderTurbulence.Value = 0;
                    sliderThermalSize.Value = 0;
                    sliderThermalStrength.Value = 0;
                    sliderDownDrafts.Value = 0;
                    SetSkyCombo("Sunny afternoon");
                    break;
                case "sunriselightairs":
                    sliderWindSpeed.Value = 0;
                    //sliderWindDirection.Value = 0;
                    sliderGustStrength.Value = 13;
                    sliderGustVariability.Value = 14;
                    sliderGustFrequency.Value = 14;
                    sliderTurbulence.Value = 8;
                    sliderThermalSize.Value = 0;
                    sliderThermalStrength.Value = 0;
                    sliderDownDrafts.Value = 0;
                    SetSkyCombo("Sunrise");
                    break;
                case "sunrisebreezy":
                    sliderWindSpeed.Value = 29;
                    //sliderWindDirection.Value = 0;
                    sliderGustStrength.Value = 25;
                    sliderGustVariability.Value = 22;
                    sliderGustFrequency.Value = 26;
                    sliderTurbulence.Value = 8;
                    sliderThermalSize.Value = 0;
                    sliderThermalStrength.Value = 0;
                    sliderDownDrafts.Value = 0;
                    SetSkyCombo("Sunrise");
                    break;
                case "sunrisewindy":
                    sliderWindSpeed.Value = 37;
                    //sliderWindDirection.Value = 0;
                    sliderGustStrength.Value = 36;
                    sliderGustVariability.Value = 56;
                    sliderGustFrequency.Value = 71;
                    sliderTurbulence.Value = 25;
                    sliderThermalSize.Value = 0;
                    sliderThermalStrength.Value = 0;
                    sliderDownDrafts.Value = 0;
                    SetSkyCombo("Sunrise");
                    break;
                case "latemorninglightairs":
                    sliderWindSpeed.Value = 5;
                    //sliderWindDirection.Value = 0;
                    sliderGustStrength.Value = 24;
                    sliderGustVariability.Value = 60;
                    sliderGustFrequency.Value = 30;
                    sliderTurbulence.Value = 5;
                    sliderThermalSize.Value = 35;
                    sliderThermalStrength.Value = 80;
                    sliderDownDrafts.Value = 25;
                    SetSkyCombo("Late morning");
                    break;
                case "latemorningbreezy":
                    sliderWindSpeed.Value = 35;
                    //sliderWindDirection.Value = 0;
                    sliderGustStrength.Value = 48;
                    sliderGustVariability.Value = 54;
                    sliderGustFrequency.Value = 57;
                    sliderTurbulence.Value = 50;
                    sliderThermalSize.Value = 41;
                    sliderThermalStrength.Value = 76;
                    sliderDownDrafts.Value = 50;
                    SetSkyCombo("Late morning");
                    break;
                case "latemorningwindy":
                    sliderWindSpeed.Value = 47;
                    //sliderWindDirection.Value = 0;
                    sliderGustStrength.Value = 48;
                    sliderGustVariability.Value = 34;
                    sliderGustFrequency.Value = 80;
                    sliderTurbulence.Value = 60;
                    sliderThermalSize.Value = 33;
                    sliderThermalStrength.Value = 36;
                    sliderDownDrafts.Value = 50;
                    SetSkyCombo("Late morning");
                    break;
                case "midafternoonlightairs":
                    sliderWindSpeed.Value = 5;
                    //sliderWindDirection.Value = 0;
                    sliderGustStrength.Value = 47;
                    sliderGustVariability.Value = 100;
                    sliderGustFrequency.Value = 72;
                    sliderTurbulence.Value = 23;
                    sliderThermalSize.Value = 100;
                    sliderThermalStrength.Value = 100;
                    sliderDownDrafts.Value = 100;
                    SetSkyCombo("Sunny afternoon");
                    break;
                case "midafternoonbreezy":
                    sliderWindSpeed.Value = 35;
                    //sliderWindDirection.Value = 0;
                    sliderGustStrength.Value = 70;
                    sliderGustVariability.Value = 95;
                    sliderGustFrequency.Value = 97;
                    sliderTurbulence.Value = 75;
                    sliderThermalSize.Value = 100;
                    sliderThermalStrength.Value = 100;
                    sliderDownDrafts.Value = 100;
                    SetSkyCombo("Sunny afternoon");
                    break;
                case "midafternoonwindy":
                    sliderWindSpeed.Value = 59;
                    //sliderWindDirection.Value = 0;
                    sliderGustStrength.Value = 100;
                    sliderGustVariability.Value = 100;
                    sliderGustFrequency.Value = 100;
                    sliderTurbulence.Value = 90;
                    sliderThermalSize.Value = 100;
                    sliderThermalStrength.Value = 61;
                    sliderDownDrafts.Value = 100;
                    SetSkyCombo("Sunny afternoon");
                    break;
                case "eveninglightairs":
                    sliderWindSpeed.Value = 5;
                    //sliderWindDirection.Value = 0;
                    sliderGustStrength.Value = 25;
                    sliderGustVariability.Value = 31;
                    sliderGustFrequency.Value = 41;
                    sliderTurbulence.Value = 0;
                    sliderThermalSize.Value = 69;
                    sliderThermalStrength.Value = 32;
                    sliderDownDrafts.Value = 100;
                    SetSkyCombo("Sunset");
                    break;
                case "eveningbreezy":
                    sliderWindSpeed.Value = 35;
                    //sliderWindDirection.Value = 0;
                    sliderGustStrength.Value = 47;
                    sliderGustVariability.Value = 95;
                    sliderGustFrequency.Value = 54;
                    sliderTurbulence.Value = 23;
                    sliderThermalSize.Value = 100;
                    sliderThermalStrength.Value = 31;
                    sliderDownDrafts.Value = 100;
                    SetSkyCombo("Sunset");
                    break;
                case "eveningwindy":
                    sliderWindSpeed.Value = 47;
                    //sliderWindDirection.Value = 0;
                    sliderGustStrength.Value = 48;
                    sliderGustVariability.Value = 48;
                    sliderGustFrequency.Value = 34;
                    sliderTurbulence.Value = 25;
                    sliderThermalSize.Value = 100;
                    sliderThermalStrength.Value = 26;
                    sliderDownDrafts.Value = 100;
                    SetSkyCombo("Sunset");
                    break;
                case "midnightlightairs":
                    sliderWindSpeed.Value = 0;
                    //sliderWindDirection.Value = 0;
                    sliderGustStrength.Value = 13;
                    sliderGustVariability.Value = 5;
                    sliderGustFrequency.Value = 26;
                    sliderTurbulence.Value = 0;
                    sliderThermalSize.Value = 0;
                    sliderThermalStrength.Value = 0;
                    sliderDownDrafts.Value = 0;
                    SetSkyCombo("Night");
                    break;
                case "midnightbreezy":
                    sliderWindSpeed.Value = 25;
                    //sliderWindDirection.Value = 0;
                    sliderGustStrength.Value = 37;
                    sliderGustVariability.Value = 5;
                    sliderGustFrequency.Value = 21;
                    sliderTurbulence.Value = 0;
                    sliderThermalSize.Value = 0;
                    sliderThermalStrength.Value = 0;
                    sliderDownDrafts.Value = 0;
                    SetSkyCombo("Night");
                    break;
                case "midnightwindy":
                    sliderWindSpeed.Value = 47;
                    //sliderWindDirection.Value = 0;
                    sliderGustStrength.Value = 35;
                    sliderGustVariability.Value = 31;
                    sliderGustFrequency.Value = 34;
                    sliderTurbulence.Value = 0;
                    sliderThermalSize.Value = 0;
                    sliderThermalStrength.Value = 0;
                    sliderDownDrafts.Value = 0;
                    SetSkyCombo("Night");
                    break;               
            }
            sliderWindSpeed_ValueChanged(this, EventArgs.Empty);
            sliderWindDirection_ValueChanged(this, EventArgs.Empty);
            sliderTurbulence_ValueChanged(this, EventArgs.Empty);
            sliderGustStrength_ValueChanged(this, EventArgs.Empty);
            sliderGustFrequency_ValueChanged(this, EventArgs.Empty);
            sliderGustVariability_ValueChanged(this, EventArgs.Empty);
            sliderThermalSize_ValueChanged(this, EventArgs.Empty);
            sliderThermalStrength_ValueChanged(this, EventArgs.Empty);
            sliderDownDrafts_ValueChanged(this, EventArgs.Empty);
            comboSky_Changed(this, EventArgs.Empty);
        }

        public override void OnShowDialog()
        {
            base.OnShowDialog();
            sliderWindSpeed.Value = (int)(100 * owner.Weather.Wind.ConstantWindSpeed / owner.Weather.Wind.MaximumConstantWindSpeed);
            sliderGustStrength.Value = (int)(100 * owner.Weather.Wind.GustSpeed / owner.Weather.Wind.MaximumGustSpeed);
            //sliderWindDirection.Value = (int)(100.0 * owner.Weather.Wind.Direction / (2 * Math.PI)) - 100;
            sliderTurbulence.Value = (int)(100 * owner.Weather.Wind.Turbulence);
            sliderGustFrequency.Value = (int)(100 * owner.Weather.Wind.GustFrequency);
            sliderGustVariability.Value = (int)(100 * owner.Weather.Wind.GustVariability);
            sliderDownDrafts.Value = (int)(100 * owner.Weather.Wind.DownDrafts);
            comboSky.Clear();
            if ((Program.Instance.Scenery != null) && (Program.Instance.Scenery.Definition != null))
            {
                foreach (DataRow dataRow in Program.Instance.Scenery.Definition.SkyTable.Rows)
                {
                    comboSky.AddItem(dataRow["Name"].ToString(), dataRow);
                    if (dataRow["Texture"].Equals(Program.Instance.Scenery.SkyTexture))
                        comboSky.SetSelectedByData(dataRow);
                }
            }
        }

        private void SetSkyCombo(string skyName)
        {
            try
            {
                comboSky.SetSelected(skyName);
            }
            catch
            {
            }
            /*
            foreach (DataRow dataRow in Program.Instance.Scenery.Definition.SkyTable.Rows)
            {
                if (dataRow["Name"].Equals(skyName))
                {
                    comboSky.SetSelectedByData(dataRow);
                }
            }
             */
        }

                
        #region Event handlers for the controls
        void buttonOk_Click(object sender, EventArgs e)
        {
            SaveSettings();
            owner.Player.FlightModel.Paused = false;
            Dialog.SetRefreshTime((float)FrameworkTimer.GetTime());
            parent.HideDialog();
        }

        void buttonBack_Click(object sender, EventArgs e)
        {
            SaveSettings();
            parent.HideDialog();
            owner.ShowMenu();
        }

        private void SaveSettings()
        {
            Bonsai.Utils.Settings.SetValue("WindSpeed", sliderWindSpeed.Value.ToString());
            Bonsai.Utils.Settings.SetValue("WindDirection", sliderWindDirection.Value.ToString());
            Bonsai.Utils.Settings.SetValue("GustStrength", sliderGustStrength.Value.ToString());
            Bonsai.Utils.Settings.SetValue("GustVariability", sliderGustVariability.Value.ToString());
            Bonsai.Utils.Settings.SetValue("GustFrequency", sliderGustFrequency.Value.ToString());
            Bonsai.Utils.Settings.SetValue("Turbulence", sliderTurbulence.Value.ToString());
            Bonsai.Utils.Settings.SetValue("ThermalSize", sliderThermalSize.Value.ToString());
            Bonsai.Utils.Settings.SetValue("ThermalStrength", sliderThermalStrength.Value.ToString());
            Bonsai.Utils.Settings.SetValue("DownDrafts", sliderDownDrafts.Value.ToString());
        }


        void sliderWindSpeed_ValueChanged(object sender, EventArgs e)
        {
            owner.Weather.Wind.ConstantWindSpeed = (sliderWindSpeed.Value * owner.Weather.Wind.MaximumConstantWindSpeed) / 100;
            textWindSpeed.SetText(string.Format("{0} kph", (int)(owner.Weather.Wind.ConstantWindSpeed * 3.6)));
        }

        void sliderWindDirection_ValueChanged(object sender, EventArgs e)
        {
            owner.Weather.Wind.Direction = ((100 - sliderWindDirection.Value) * 2 * Math.PI) / 100.0;
            double dir = 360.0 - owner.Weather.Wind.Direction * 180 / Math.PI;
            string name = "West";
            if (dir > 337.5)
                name = "west";
            else if (dir > 292.5)
                name = "south-west";
            else if (dir > 247.5)
                name = "south";
            else if (dir > 202.5)
                name = "south-east";
            else if (dir > 167.5)
                name = "east";
            else if (dir > 122.5)
                name = "north-east";
            else if (dir > 67.5)
                name = "north";
            else if (dir > 22.5)
                name = "north-west";
            else
                name = "west";
            currentWindDirText.SetText(name);
        }

        void sliderGustStrength_ValueChanged(object sender, EventArgs e)
        {
            owner.Weather.Wind.GustSpeed = (sliderGustStrength.Value * owner.Weather.Wind.MaximumGustSpeed) / 100;
            textGustStrength.SetText(string.Format("{0} kph", (int)(owner.Weather.Wind.GustSpeed * 3.6)));
        }


        void sliderTurbulence_ValueChanged(object sender, EventArgs e)
        {
            owner.Weather.Wind.Turbulence = sliderTurbulence.Value / 100.0;
            if (sliderTurbulence.Value == 0)
                textTurbulence.SetText("none");
            else if (sliderTurbulence.Value < 33)
                textTurbulence.SetText("light");
            else if (sliderTurbulence.Value < 67)
                textTurbulence.SetText("medium");
            else
                textTurbulence.SetText("heavy");
        }

        void sliderGustFrequency_ValueChanged(object sender, EventArgs e)
        {
            owner.Weather.Wind.GustFrequency = sliderGustFrequency.Value / 100.0;
            if (sliderGustFrequency.Value < 33)
                textGustFrequency.SetText("low");
            else if (sliderGustFrequency.Value < 67)
                textGustFrequency.SetText("medium");
            else
                textGustFrequency.SetText("high");
        }

        void sliderGustVariability_ValueChanged(object sender, EventArgs e)
        {
            owner.Weather.Wind.GustVariability = sliderGustVariability.Value / 100.0;
            if (sliderGustVariability.Value < 20)
                textGustVariability.SetText("very predictable");
            else if (sliderGustVariability.Value < 40)
                textGustVariability.SetText("predictable");
            else if (sliderGustVariability.Value < 60)
                textGustVariability.SetText("variable");
            else if (sliderGustVariability.Value < 80)
                textGustVariability.SetText("unpredictable");
            else
                textGustVariability.SetText("very unpredictable");
        }


        void sliderThermalSize_ValueChanged(object sender, EventArgs e)
        {            
            if (sliderThermalSize.Value < 33)
                textThermalSize.SetText("small");
            else if (sliderThermalSize.Value < 67)
                textThermalSize.SetText("medium");
            else
                textThermalSize.SetText("large");
        }       

        void sliderThermalStrength_ValueChanged(object sender, EventArgs e)
        {            
            if (sliderThermalStrength.Value == 0)
                textThermalStrength.SetText("off");
            else if (sliderThermalStrength.Value < 33)
                textThermalStrength.SetText("weak");
            else if (sliderThermalStrength.Value < 67)
                textThermalStrength.SetText("medium");
            else
                textThermalStrength.SetText("strong");
        }
        #endregion
    }
}
