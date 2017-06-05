using System;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Bonsai.Core.Controls;
using Bonsai.Core;
using Bonsai.Core.Dialogs;
using System.Drawing;
using System.Threading;

namespace RCSim.Dialogs
{
    #region Control Ids
    public enum MenuDialogControlIds
    {
        Static = -1,
        None,
        OK,
        Aircraft,
        Scenery,
        Weather,
        Controls,
        Graphics,
        Sound,
        SimOptions,
        Multiplayer,
        Exit,
        AffiliatePicture,
        AffiliateButton
    }
    #endregion

    /// <summary>
    /// Dialog for selection of device settings 
    /// </summary>
    class MenuDialog : DialogBase
    {
        #region Private fields
        private AircraftDialog aircraftDialog;
        private SceneryDialog sceneryDialog;
        private WeatherDialog weatherDialog;
        private ControlsDialog controlsDialog;
        private GraphicsDialog graphicsDialog;
        private SoundDialog soundDialog;
        private SimOptionsDialog simOptionsDialog;
        private Program owner;

        private Button buttonOk;
        private Button buttonAircraft;
        private Button buttonScenery;
        private Button buttonWeather;
        private Button buttonControls;
        private Button buttonGraphics;
        private Button buttonSound;
        private Button buttonSimOptions;
        private Button buttonMultiplayer;
        private Button buttonExit;

        private StaticText affiliateText;
        private Picture pictureAffiliate;
        private Button buttonAffiliate;
        private string affUrl;
        #endregion

        #region Creation
        /// <summary>Creates a new settings dialog</summary>
        public MenuDialog(Framework framework, Program owner) : base(framework)
        {
            this.owner = owner;            
            framework.Device.DeviceLost += new EventHandler(Device_DeviceLost);
            framework.Device.DeviceReset += new EventHandler(Device_DeviceReset);
            framework.Device.Disposing += new EventHandler(Device_Disposing);
            CreateControls();

            aircraftDialog = new AircraftDialog(framework, owner);
            sceneryDialog = new SceneryDialog(framework, owner);
            weatherDialog = new WeatherDialog(framework, owner);
            controlsDialog = new ControlsDialog(framework, owner);
            graphicsDialog = new GraphicsDialog(framework, owner);
            soundDialog = new SoundDialog(framework, owner);
            simOptionsDialog = new SimOptionsDialog(framework, owner);
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

        #region Public methods
        public void SetAffiliate(string affImage, string affUrl, Size affSize)
        {
            this.affUrl = affUrl;
            
            affiliateText = dialog.AddStatic((int)MenuDialogControlIds.Static, "special offer:", 180, 40, affSize.Width, 20);
            Element e = affiliateText[0];
            e.FontIndex = 0;
            
            e.textFormat = DrawTextFormat.Top | DrawTextFormat.Center;
            pictureAffiliate = dialog.AddPicture((int)MenuDialogControlIds.AffiliatePicture, affImage, 180, 60, affSize.Width, affSize.Height);
            pictureAffiliate.SourceRectangle = new System.Drawing.Rectangle(0, 0, 
                (int)Math.Pow(2, ((int)(Math.Log(affSize.Width, 2))+1)), (int)Math.Pow(2, ((int)(Math.Log(affSize.Height, 2))+1)));
            pictureAffiliate.Click += new EventHandler(pictureAffiliate_Click);
            
            buttonAffiliate = dialog.AddButton((int)MenuDialogControlIds.AffiliateButton, "to website", 130 + affSize.Width / 2, 70 + affSize.Height, 100, 22);
            buttonAffiliate.Click += new EventHandler(pictureAffiliate_Click);
        }

        

        private void LaunchWebSiteRun()
        {
            try
            {
                System.Diagnostics.Process.Start(affUrl);
            }
            catch 
            {

            }
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
            StaticText title = dialog.AddStatic((int)MenuDialogControlIds.Static, "RC sim menu", 10, 5, 400, 30);
            e = title[0];
            e.FontIndex = 1;
            e.textFormat = DrawTextFormat.Top | DrawTextFormat.Left;

            buttonOk = dialog.AddButton((int)MenuDialogControlIds.OK, "back to sim", 270, 435, 140, 31);
            buttonOk.Click += new EventHandler(buttonOk_Click);

            buttonAircraft = dialog.AddButton((int)MenuDialogControlIds.Aircraft, "aircraft", 10, y += 35, 140, 31);
            buttonAircraft.Click += new EventHandler(buttonAircraft_Click);

            buttonScenery = dialog.AddButton((int)MenuDialogControlIds.Scenery, "scenery", 10, y += 35, 140, 31);
            buttonScenery.Click += new EventHandler(buttonScenery_Click);

            buttonWeather = dialog.AddButton((int)MenuDialogControlIds.Weather, "flight conditions", 10, y += 35, 140, 31);
            buttonWeather.Click += new EventHandler(buttonWeather_Click);

            buttonControls = dialog.AddButton((int)MenuDialogControlIds.Controls, "controls", 10, y += 35, 140, 31);
            buttonControls.Click += new EventHandler(buttonControls_Click);

            buttonGraphics = dialog.AddButton((int)MenuDialogControlIds.Graphics, "graphics", 10, y += 35, 140, 31);
            buttonGraphics.Click += new EventHandler(buttonGraphics_Click);

            buttonSound = dialog.AddButton((int)MenuDialogControlIds.Sound, "sound", 10, y += 35, 140, 31);
            buttonSound.Click += new EventHandler(buttonSound_Click);

            buttonSimOptions = dialog.AddButton((int)MenuDialogControlIds.SimOptions, "sim options", 10, y += 35, 140, 31);
            buttonSimOptions.Click += new EventHandler(buttonSimOptions_Click);

            //buttonMultiplayer = dialog.AddButton((int)MenuDialogControlIds.Multiplayer, "multiplayer", 10, y += 35, 140, 31);
            //buttonMultiplayer.Click += new EventHandler(buttonMultiplayer_Click);

            buttonExit = dialog.AddButton((int)MenuDialogControlIds.Exit, "exit sim", 10, y += 35, 140, 31);
            buttonExit.Click += new EventHandler(buttonExit_Click);
        }

        

        public override void OnShowDialog()
        {
            owner.Player.FlightModel.Paused = true;
            base.OnShowDialog();            
        }

        #region Event handlers for the controls
        void pictureAffiliate_Click(object sender, EventArgs e)
        {
            Dialog.SetRefreshTime((float)FrameworkTimer.GetTime());
            Thread thread = new Thread(new ThreadStart(LaunchWebSiteRun));
            thread.Start();
        }

        void buttonAircraft_Click(object sender, EventArgs e)
        {
            parent.ShowDialog(aircraftDialog);
        }


        void buttonScenery_Click(object sender, EventArgs e)
        {
            parent.ShowDialog(sceneryDialog);
        }

        void buttonGraphics_Click(object sender, EventArgs e)
        {
            parent.ShowDialog(graphicsDialog);
        }


        void buttonWeather_Click(object sender, EventArgs e)
        {
            parent.ShowDialog(weatherDialog);
        }

        void buttonControls_Click(object sender, EventArgs e)
        {
            parent.ShowDialog(controlsDialog);
        }
        
        void buttonSound_Click(object sender, EventArgs e)
        {
            parent.ShowDialog(soundDialog);
        }


        void buttonSimOptions_Click(object sender, EventArgs e)
        {
            parent.ShowDialog(simOptionsDialog);
        }

        void buttonExit_Click(object sender, EventArgs e)
        {
            if (Framework.Instance.WindowForm != null)
            {
                Framework.Instance.WindowForm.Close();
            }
        }

        void  buttonOk_Click(object sender, EventArgs e)
        {
            Dialog.SetRefreshTime((float)FrameworkTimer.GetTime());
            owner.Player.FlightModel.Paused = false;
            parent.HideDialog();
        }
        #endregion

        
    }
}
