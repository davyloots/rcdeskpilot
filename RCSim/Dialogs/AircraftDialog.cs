using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Bonsai.Core.Controls;
using Bonsai.Core;
using Bonsai.Core.Dialogs;
using Bonsai.Objects.Textures;
using RCSim.DataClasses;
using System.Threading;

namespace RCSim.Dialogs
{
    #region Control Ids
    public enum AircraftDialogControlIds
    {
        Static = -1,
        None,
        OK,
        Back,
        Buy,
        ComboAircraft,
        PictureAircraft,
        TextAircraft,
        CheckStartOnWater
    }
    #endregion

    /// <summary>
    /// Dialog for selection of device settings 
    /// </summary>
    class AircraftDialog : DialogBase
    {
        #region Private classes
        private class AircraftInfo
        {
            public string Name = string.Empty;
            public string ParFile = string.Empty;
            public string Folder = string.Empty;
        }
        #endregion

        #region Private fields
        private Program owner;
        private ComboBox aircraftCombo;
        private Button buttonOk;
        private Button buttonBack;
        private Button buttonBuy;
        private Checkbox checkboxStartOnWater;

        private Picture aircraftPicture;
        private StaticText aircraftText;
        private bool changed = false;
        //private string url = "http://rcdeskpilot.com/aircraft/bmi_allegro";µ
        private string url = "";
        private List<AircraftInfo> aircraftList = new List<AircraftInfo>();
        #endregion

        #region Creation
        /// <summary>Creates a new settings dialog</summary>
        public AircraftDialog(Framework framework, Program owner)
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
            StaticText title = dialog.AddStatic((int)AircraftDialogControlIds.Static, "Aircraft", 10, 5, 400, 30);
            e = title[0];
            e.FontIndex = 1;
            e.textFormat = DrawTextFormat.Top | DrawTextFormat.Left;

            ReadAircraftList();

            aircraftCombo = dialog.AddComboBox((int)AircraftDialogControlIds.ComboAircraft, 10, y += 30, 200, 24);
            foreach (AircraftInfo aircraft in aircraftList)
            {
                aircraftCombo.AddItem(aircraft.Name, aircraft);
            }
            //aircraftCombo.SetSelected("BMI Allegro 1500");
            aircraftCombo.SetSelected("El Trainer");
            /*
            aircraftCombo.AddItem("Cessna", "aircraft\\cessna\\cessna.par");
            aircraftCombo.AddItem("Extra", "aircraft\\extra\\extra.par");
            aircraftCombo.AddItem("Thermal sailplane", "aircraft\\glider\\glider.par");
            aircraftCombo.AddItem("BMKDesigns P-51", "aircraft\\BMK_P-51\\BMK_P-51.par");
            aircraftCombo.AddItem("Eurocopter Tiger", "aircraft\\tiger\\tiger.par");
             */
            aircraftCombo.Changed += new EventHandler(aircraftCombo_Changed);

            //aircraftPicture = dialog.AddPicture((int)AircraftDialogControlIds.PictureAircraft, "aircraft\\BMI Allegro 1500\\allegro_icon.png", 340, 30, 256, 256);
            aircraftPicture = dialog.AddPicture((int)AircraftDialogControlIds.PictureAircraft, "aircraft\\cessna\\icon.png", 340, 30, 256, 256);
            aircraftPicture.SourceRectangle = new System.Drawing.Rectangle(0, 0, 256, 256);

            aircraftText = dialog.AddStatic((int)AircraftDialogControlIds.TextAircraft, "", 10, y += 30, 320, 240);
            e = aircraftText[0];
            e.FontIndex = 0;
            e.textFormat = DrawTextFormat.Top | DrawTextFormat.Left | DrawTextFormat.WordBreak;
            UpdateText();

            checkboxStartOnWater = dialog.AddCheckBox((int)AircraftDialogControlIds.CheckStartOnWater, "Start on water", 10, 320, 200, 24, true);
            checkboxStartOnWater.Changed += new EventHandler(checkboxStartOnWater_Changed);
            checkboxStartOnWater.IsVisible = false;

            buttonBuy = dialog.AddButton((int)AircraftDialogControlIds.Buy, "buy/more info", 370, 300, 196, 32);
            buttonBuy.Click += new EventHandler(buttonBuy_Click);
            buttonBuy.IsVisible = false;
            //buttonBuy.IsVisible = true;

            buttonBack = dialog.AddButton((int)AircraftDialogControlIds.Back, "back to menu", 190, 435, 100, 31);
            buttonBack.Click += new EventHandler(buttonBack_Click);

            buttonOk = dialog.AddButton((int)AircraftDialogControlIds.OK, "back to sim", 350, 435, 100, 31);
            buttonOk.Click += new EventHandler(buttonOk_Click);
        }

        void checkboxStartOnWater_Changed(object sender, EventArgs e)
        {
            changed = true;
        }

        private void ReadAircraftList()
        {
            aircraftList.Clear();
            DirectoryInfo dirInfo = new DirectoryInfo("aircraft");
            DirectoryInfo[] subDirs = dirInfo.GetDirectories();
            foreach (DirectoryInfo subDir in subDirs)
            {
                FileInfo[] parFiles = subDir.GetFiles("*.par");
                foreach (FileInfo parFile in parFiles)
                {
                    AircraftInfo aircraftInfo = new AircraftInfo();
                    aircraftInfo.Name = Utility.GetFileNamePart(parFile.FullName);
                    aircraftInfo.ParFile = parFile.FullName;
                    aircraftInfo.Folder = Utility.AppendDirectorySeparator(parFile.DirectoryName);
                    aircraftList.Add(aircraftInfo);
                }
            }

            try
            {
                dirInfo = new DirectoryInfo(string.Concat(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "\\RC Desk Pilot\\Aircraft\\"));
                subDirs = dirInfo.GetDirectories();
                foreach (DirectoryInfo subDir in subDirs)
                {
                    FileInfo[] parFiles = subDir.GetFiles("*.par");
                    foreach (FileInfo parFile in parFiles)
                    {
                        AircraftInfo aircraftInfo = new AircraftInfo();
                        aircraftInfo.Name = Utility.GetFileNamePart(parFile.FullName);
                        aircraftInfo.ParFile = parFile.FullName;
                        aircraftInfo.Folder = Utility.AppendDirectorySeparator(parFile.DirectoryName);
                        aircraftList.Add(aircraftInfo);
                    }
                }
            }
            catch
            { }
        }

        private void LaunchWebSiteRun()
        {
            try
            {
                System.Diagnostics.Process.Start(url);
            }
            catch 
            {

            }
        }

        void buttonBuy_Click(object sender, EventArgs e)
        {
            Dialog.SetRefreshTime((float)FrameworkTimer.GetTime());
            Thread thread = new Thread(new ThreadStart(LaunchWebSiteRun));
            thread.Start();
        }

        private void UpdateText()
        {
            AircraftInfo ai = aircraftCombo.GetSelectedData() as AircraftInfo;
            if (ai != null)
            {
                AircraftParameters parameters = new AircraftParameters();
                parameters.File = ai.ParFile;
                aircraftText.SetText(parameters.Description);
            }
        }
        

        public override void OnShowDialog()
        {
            base.OnShowDialog();
            changed = false;
            aircraftPicture.FadeIn();
        }


        #region Event handlers for the controls
        void aircraftCombo_Changed(object sender, EventArgs e)
        {
            url = null;
            AircraftInfo ai = aircraftCombo.GetSelectedData() as AircraftInfo;
            if (ai != null)
            {
                AircraftParameters parameters = new AircraftParameters();
                parameters.File = ai.ParFile;
                aircraftText.SetText(parameters.Description);
                if (!string.IsNullOrEmpty(parameters.IconFile))
                {
                    aircraftPicture.TextureFile = string.Concat(ai.Folder, parameters.IconFile);
                    aircraftPicture.IsVisible = true;
                }
                else
                    aircraftPicture.IsVisible = false;
                checkboxStartOnWater.IsVisible = parameters.HasFloats;
                checkboxStartOnWater.IsChecked = true;
                if (!string.IsNullOrEmpty(parameters.BuyUrl))
                {
                    url = string.Format("http://rcdeskpilot.com/aircraft/{0}", parameters.BuyUrl);
                    buttonBuy.IsVisible = true;
                }
                else
                    buttonBuy.IsVisible = false;
            }
           
            changed = true;
            //UpdateText();
        }

        void buttonOk_Click(object sender, EventArgs e)
        {
            if (changed)
            {
                AircraftInfo ai = aircraftCombo.GetSelectedData() as AircraftInfo;
                if (ai != null)
                {
                    if (checkboxStartOnWater.IsChecked && checkboxStartOnWater.IsVisible)
                        owner.Player.TakeOffFromWater = true;
                    else
                        owner.Player.TakeOffFromWater = false;
                    owner.Player.LoadModel(ai.ParFile);
                    if (owner.Player.AircraftParameters.AllowsTowing)
                        owner.CenterHud.ShowGameText("Press 'T' to start towing", 30f);
                    else
                        owner.CenterHud.ShowGameText("", 0f);
                }
                Program.Instance.SetWaterCamera(owner.Player.TakeOffFromWater);
            }
            owner.Player.FlightModel.Paused = false;
            Dialog.SetRefreshTime((float)FrameworkTimer.GetTime());
            owner.CenterHud.SetCrashPicture(owner.Player.AircraftParameters.AdLocation, url, owner.Player.AircraftParameters.FolderName);
            parent.HideDialog();
        }


        void buttonBack_Click(object sender, EventArgs e)
        {
            if (changed)
            {
                AircraftInfo ai = aircraftCombo.GetSelectedData() as AircraftInfo;
                if (ai != null)
                {
                    if (checkboxStartOnWater.IsChecked && checkboxStartOnWater.IsVisible)
                        owner.Player.TakeOffFromWater = true;
                    else
                        owner.Player.TakeOffFromWater = false;
                    owner.Player.LoadModel(ai.ParFile);
                    if (owner.Player.AircraftParameters.AllowsTowing)
                        owner.CenterHud.ShowGameText("Press 'T' to start towing", 30f);
                    else
                        owner.CenterHud.ShowGameText("", 0f);
                }
                Program.Instance.SetWaterCamera(owner.Player.TakeOffFromWater);
            }
            parent.HideDialog();
            owner.ShowMenu();
        }
        #endregion
    }
}
