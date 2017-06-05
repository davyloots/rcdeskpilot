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
using System.Data;
using System.Drawing;
using System.Collections.Generic;
using RCSim.DataClasses;

namespace RCSim.Dialogs
{
    #region Control Ids
    public enum SceneryDialogControlIds
    {
        Static = -1,
        None,
        OK,
        Back,
        Sceneries,
        PilotPosition,
        DynamicScenery,
        LensFlare,
        EnableThermalVisual
    }
    #endregion

    /// <summary>
    /// Dialog for selection of device settings 
    /// </summary>
    class SceneryDialog : DialogBase
    {
        #region Private classes
        private class SceneryInfo
        {
            public string Name = string.Empty;
            public string ParFile = string.Empty;
            public string Folder = string.Empty;
        }
        #endregion

        #region Private fields
        private Program owner;

        private Button buttonOk;
        private Button buttonBack;
        private ComboBox comboSceneries;
        private ComboBox comboPilotPos;
        private Checkbox checkDynamicScenery;
        private Checkbox checkLensFlare;
        private Checkbox checkEnableThermalVisual;
        private List<SceneryInfo> sceneryList = new List<SceneryInfo>();
        private bool changed = false;
        #endregion

        #region Creation
        /// <summary>Creates a new settings dialog</summary>
        public SceneryDialog(Framework framework, Program owner)
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
            StaticText title = dialog.AddStatic((int)SoundDialogControlIds.Static, "Scenery settings", 10, 5, 400, 30);
            e = title[0];
            e.FontIndex = 1;
            e.textFormat = DrawTextFormat.Top | DrawTextFormat.Left;

            ReadSceneryList();

            comboSceneries = dialog.AddComboBox((int)SceneryDialogControlIds.Sceneries, 10, y += 30, 200, 24);
            foreach (SceneryInfo scenery in sceneryList)
            {
                comboSceneries.AddItem(scenery.Name, scenery);
            }
            comboSceneries.SetSelected(0);
            comboSceneries.Changed += new EventHandler(comboSceneries_Changed);

            StaticText pilotPositionText = dialog.AddStatic((int)SceneryDialogControlIds.Static, "Pilot position:", 10, y += 30, 180, 30);
            e = pilotPositionText[0];
            e.FontIndex = 3;

            comboPilotPos = dialog.AddComboBox((int)SceneryDialogControlIds.PilotPosition, 200, y, 200, 30);
            comboPilotPos.Changed += new EventHandler(comboPilotPos_Changed);

            checkDynamicScenery = dialog.AddCheckBox((int)SceneryDialogControlIds.DynamicScenery, "Enable dynamic scenery", 10, y += 30, 200, 20,
                Convert.ToInt32(Bonsai.Utils.Settings.GetValue("DynamicScenery", "1")) == 1);
            checkDynamicScenery.Changed += new EventHandler(checkDynamicScenery_Changed);

            checkLensFlare = dialog.AddCheckBox((int)SceneryDialogControlIds.LensFlare, "Enable lens flare", 10, y += 30, 200, 20,
                Convert.ToInt32(Bonsai.Utils.Settings.GetValue("LensFlare", "1")) == 1);
            checkLensFlare.Changed += new EventHandler(checkLensFlare_Changed);

            checkEnableThermalVisual = dialog.AddCheckBox((int)SceneryDialogControlIds.EnableThermalVisual, "Enable visual representation of thermals", 10, y += 30, 400, 20,
                Convert.ToBoolean(Bonsai.Utils.Settings.GetValue("EnableThermalVisual")));
            checkEnableThermalVisual.Changed += new EventHandler(checkEnableThermalVisual_Changed);
 
            buttonBack = dialog.AddButton((int)SoundDialogControlIds.Back, "back to menu", 190, 435, 100, 31);
            buttonBack.Click += new EventHandler(buttonBack_Click);

            buttonOk = dialog.AddButton((int)SoundDialogControlIds.OK, "back to sim", 350, 435, 100, 31);
            buttonOk.Click += new EventHandler(buttonOk_Click);
        }

        void comboSceneries_Changed(object sender, EventArgs e)
        {
            changed = true;
            SceneryInfo sceneryInfo = comboSceneries.GetSelectedData() as SceneryInfo;
            if (sceneryInfo != null)
            {
                SceneryParameters parameters = new SceneryParameters();
                parameters.File = sceneryInfo.ParFile;
                if (parameters.SceneryType == SceneryParameters.SceneryTypeEnum.Photofield)
                {
                    comboPilotPos.IsVisible = false;

                }
                else
                {
                    comboPilotPos.IsVisible = true;                    
                }
                LoadPilotPositions(parameters);
            }
        }

        void checkLensFlare_Changed(object sender, EventArgs e)
        {
            if (checkLensFlare.IsChecked)
                Bonsai.Utils.Settings.SetValue("LensFlare", "1");
            else
                Bonsai.Utils.Settings.SetValue("LensFlare", "0");
            LensFlare.Enabled = checkLensFlare.IsChecked;
        }

        void checkDynamicScenery_Changed(object sender, EventArgs e)
        {
            if (checkDynamicScenery.IsChecked)
                Bonsai.Utils.Settings.SetValue("DynamicScenery", "1");
            else
                Bonsai.Utils.Settings.SetValue("DynamicScenery", "0");
        }

        void checkEnableThermalVisual_Changed(object sender, EventArgs e)
        {
            Bonsai.Utils.Settings.SetValue("EnableThermalVisual", checkEnableThermalVisual.IsChecked.ToString());
        }

        void comboPilotPos_Changed(object sender, EventArgs e)
        {
            DataRow dataRow = comboPilotPos.GetSelectedData() as DataRow;
            SceneryInfo sceneryInfo = comboSceneries.GetSelectedData() as SceneryInfo;
            if (sceneryInfo != null)
            {
                SceneryParameters parameters = new SceneryParameters();
                parameters.File = sceneryInfo.ParFile;
                SetPilotPosition(parameters, dataRow);
            }
        }

        public override void OnShowDialog()
        {
            base.OnShowDialog();
            changed = false;
            comboPilotPos.Clear();
            if ((Program.Instance.Scenery != null) && (Program.Instance.Scenery.Definition != null))
            {
                foreach (DataRow dataRow in Program.Instance.Scenery.Definition.PilotPositionTable.Rows)
                {
                    comboPilotPos.AddItem(dataRow["Name"].ToString(), dataRow);
                    Vector3 pos = (Vector3)dataRow["Position"];
                    if ((pos.X == Program.Instance.PilotPosition.X) && (pos.Z == Program.Instance.PilotPosition.Z))
                        comboPilotPos.SetSelected(dataRow["Name"].ToString());
                }
            }
        }

        #region Private methods
        private void SetPilotPosition(SceneryParameters parameters, DataRow dataRow)
        {            
            if (dataRow != null)
            {
                Vector3 pilotPos = (Vector3)dataRow["Position"];
                string map = dataRow["Map"].ToString();
                Program.Instance.PilotPosition = new Vector3(pilotPos.X, Program.Instance.Heightmap.GetHeightAt(pilotPos.X, pilotPos.Z) + 1.7f,
                    pilotPos.Z);
                Program.Instance.Map = parameters.SceneryFolder + map;
            }
            else
            {
                Program.Instance.PilotPosition = new Vector3(0, 1.7f, 0);
                Program.Instance.Map = null;
            }
        }

        private void ReadSceneryList()
        {
            sceneryList.Clear();
            DirectoryInfo dirInfo = new DirectoryInfo("data\\scenery");
            DirectoryInfo[] subDirs = dirInfo.GetDirectories();
            foreach (DirectoryInfo subDir in subDirs)
            {
                FileInfo[] parFiles = subDir.GetFiles("*.par");
                foreach (FileInfo parFile in parFiles)
                {
                    SceneryInfo sceneryInfo = new SceneryInfo();
                    sceneryInfo.Name = Utility.GetFileNamePart(parFile.FullName);
                    sceneryInfo.ParFile = parFile.FullName;
                    sceneryInfo.Folder = Utility.AppendDirectorySeparator(parFile.DirectoryName);
                    sceneryList.Add(sceneryInfo);
                }
            }

            try
            {
                dirInfo = new DirectoryInfo(string.Concat(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "\\RC Desk Pilot\\Scenery\\"));
                subDirs = dirInfo.GetDirectories();
                foreach (DirectoryInfo subDir in subDirs)
                {
                    FileInfo[] parFiles = subDir.GetFiles("*.par");
                    foreach (FileInfo parFile in parFiles)
                    {
                        SceneryInfo sceneryInfo = new SceneryInfo();
                        sceneryInfo.Name = Utility.GetFileNamePart(parFile.FullName);
                        sceneryInfo.ParFile = parFile.FullName;
                        sceneryInfo.Folder = Utility.AppendDirectorySeparator(parFile.DirectoryName);
                        sceneryList.Add(sceneryInfo);
                    }
                }
            }
            catch
            { }
        }

        private void LoadPilotPositions(SceneryParameters parameters)
        {
            try
            {
                comboPilotPos.Clear();
                if ((parameters != null) && (parameters.DefinitionFile != null) && (parameters.SceneryType == SceneryParameters.SceneryTypeEnum.Full3D))
                {
                    bool posFound = false;
                    TerrainDefinition definition = new TerrainDefinition();
                    definition.Load(parameters.SceneryFolder + parameters.DefinitionFile);
                    foreach (DataRow dataRow in definition.PilotPositionTable.Rows)
                    {
                        comboPilotPos.AddItem(dataRow["Name"].ToString(), dataRow);
                        Vector3 pos = (Vector3)dataRow["Position"];
                        if ((pos.X == Program.Instance.PilotPosition.X) && (pos.Z == Program.Instance.PilotPosition.Z))
                        {
                            comboPilotPos.SetSelected(dataRow["Name"].ToString());
                            posFound = true;
                        }
                    }
                    if ((!posFound) && definition.PilotPositionTable.Rows.Count > 0)
                    {
                        SetPilotPosition(parameters, definition.PilotPositionTable.Rows[0]);
                        comboPilotPos.SetSelected(definition.PilotPositionTable.Rows[0]["Name"].ToString());
                    }
                }
                else
                {
                    comboPilotPos.AddItem("default", null);
                    comboPilotPos.SetSelectedByData(null);
                    SetPilotPosition(parameters, null);
                }
            }
            catch
            {
            }
        }
        #endregion

        #region Event handlers for the controls
        void buttonOk_Click(object sender, EventArgs e)
        {
            if (changed)
            {
                SceneryInfo sceneryInfo = comboSceneries.GetSelectedData() as SceneryInfo;
                if (sceneryInfo != null)
                    Program.Instance.Scenery.LoadDefinition(sceneryInfo.ParFile);
                Program.Instance.Player.Reset();
            }
            owner.Player.FlightModel.Paused = false;
            Dialog.SetRefreshTime((float)FrameworkTimer.GetTime());
            parent.HideDialog();
        }


        void buttonBack_Click(object sender, EventArgs e)
        {
            if (changed)
            {
                SceneryInfo sceneryInfo = comboSceneries.GetSelectedData() as SceneryInfo;
                if (sceneryInfo != null)
                    Program.Instance.Scenery.LoadDefinition(sceneryInfo.ParFile);
                Program.Instance.Player.Reset();
            }
            parent.HideDialog();
            owner.ShowMenu();
        }
        #endregion
    }
}
