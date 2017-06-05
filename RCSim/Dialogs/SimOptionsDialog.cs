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
    public enum SimOptionsDialogControlIds
    {
        Static = -1,
        None,
        OK,
        Back,
        GameType,
        ComputerPilots
    }
    #endregion

    /// <summary>
    /// Dialog for selection of sim settings 
    /// </summary>
    class SimOptionsDialog : DialogBase
    {
        #region Private fields
        private Program owner;

        private Button buttonOk;
        private Button buttonBack;
        private ComboBox comboGameType;
        private ComboBox comboComputerPilots;
        #endregion

        #region Creation
        /// <summary>Creates a new settings dialog</summary>
        public SimOptionsDialog(Framework framework, Program owner)
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
            StaticText title = dialog.AddStatic((int)SimOptionsDialogControlIds.Static, "Simulation options", 10, 5, 400, 30);
            e = title[0];
            e.FontIndex = 1;
            e.textFormat = DrawTextFormat.Top | DrawTextFormat.Left;

            StaticText gameTypeTitle = dialog.AddStatic((int)SimOptionsDialogControlIds.Static, "Challenges", 10, y += 30, 200, 24);
            
            comboGameType = dialog.AddComboBox((int)SimOptionsDialogControlIds.GameType, 200, y, 200, 24);
            comboGameType.AddItem("None", "none");
            comboGameType.AddItem("Pylon racing", "racing");
            comboGameType.AddItem("Scarecrow", "scarecrow");
            //comboGameType.AddItem("Flour bombing", "bombing");
            comboGameType.Changed += new EventHandler(comboGameType_Changed);

            StaticText pilotsText = dialog.AddStatic((int)SimOptionsDialogControlIds.Static, "Number of other pilots", 10, y += 30, 200, 24);

            comboComputerPilots = dialog.AddComboBox((int)SimOptionsDialogControlIds.ComputerPilots, 200, y, 160, 24);
            comboComputerPilots.AddItem("None", 0);
            comboComputerPilots.AddItem("1", 1);
            comboComputerPilots.AddItem("2", 2);
            comboComputerPilots.Changed += new EventHandler(comboComputerPilots_Changed);

            buttonBack = dialog.AddButton((int)SimOptionsDialogControlIds.Back, "back to menu", 190, 435, 100, 31);
            buttonBack.Click += new EventHandler(buttonBack_Click);

            buttonOk = dialog.AddButton((int)SimOptionsDialogControlIds.OK, "back to sim", 350, 435, 100, 31);
            buttonOk.Click += new EventHandler(buttonOk_Click);
        }

              
        public override void OnShowDialog()
        {
            base.OnShowDialog();            
        }


        #region Event handlers for the controls
        void comboGameType_Changed(object sender, EventArgs e)
        {
            switch (comboGameType.GetSelectedData().ToString())
            {
                case "racing":
                    owner.CurrentGameType = Game.GameType.Racing;
                    break;
                case "scarecrow":
                    owner.CurrentGameType = Game.GameType.ScareCrow;
                    break;
                case "bombing":
                    owner.CurrentGameType = Game.GameType.Bombing;
                    break;
                default:
                    owner.CurrentGameType = Game.GameType.None;
                    break;
            }
        }

        void comboComputerPilots_Changed(object sender, EventArgs e)
        {
            owner.NumberOfComputerPilots = (int)comboComputerPilots.GetSelectedData();
        }

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
        #endregion
    }
}
