using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RCSim.AircraftEditor.Dialogs
{
    internal partial class NewAircraftForm : Form
    {
        private bool keyValid = false;
        private WizardStageEnum wizardStage = WizardStageEnum.Step1;
        private WizardResultEnum wizardResult = WizardResultEnum.NewAircraft;

        private enum WizardStageEnum
        {
            Step1,
            Step2New,
            Step2Variation
        }

        public enum WizardResultEnum
        {
            NewAircraft,
            NewVariation
        }

        public WizardResultEnum WizardResult
        {
            get { return wizardResult; }
        }

        public string AircraftName
        {
            get 
            {
                if (wizardStage == WizardStageEnum.Step2New)
                    return newAircraftStep2Control.AircraftName;
                else
                    return newVariationStep2Control.AircraftName;
            }
        }

        public string FolderName
        {
            get
            {
                if (wizardStage == WizardStageEnum.Step2New)
                    return newAircraftStep2Control.FolderName;
                else
                    return null;
            }
        }

        public string BaseAircraft
        {
            get
            {
                if (wizardStage == WizardStageEnum.Step2Variation)
                    return newVariationStep2Control.BaseAircraft;
                else
                    return null;
            }
        }

        public NewAircraftForm()
        {
            InitializeComponent();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (wizardStage == WizardStageEnum.Step1)
            {
                if (newAircraftStep1Control.CreationOptions == NewAircraftStep1Control.CreationOptionsEnum.NewAircraft)
                {
                    wizardStage = WizardStageEnum.Step2New;
                    newAircraftStep2Control.Visible = true;
                    buttonOK.Enabled = newAircraftStep2Control.FormValid;
                }
                else
                {
                    wizardStage = WizardStageEnum.Step2Variation;
                    newVariationStep2Control.Visible = true;
                    buttonOK.Enabled = newVariationStep2Control.FormValid;
                }
                newAircraftStep1Control.Visible = false;
                buttonOK.Text = "&Finish";
                buttonBack.Enabled = true;
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                if (wizardStage == WizardStageEnum.Step2New)
                    wizardResult = WizardResultEnum.NewAircraft;
                else
                    wizardResult = WizardResultEnum.NewVariation;
                this.Close();
            }
        }

        private void newAircraftStep2Control_FormValidatedChanged(object sender, EventArgs e)
        {
            if (wizardStage == WizardStageEnum.Step2New)
            {
                if (newAircraftStep2Control.FormValid)
                    buttonOK.Enabled = true;
                else
                    buttonOK.Enabled = false;
            }
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            if ((wizardStage == WizardStageEnum.Step2New) ||
                (wizardStage == WizardStageEnum.Step2Variation))
            {
                wizardStage = WizardStageEnum.Step1;
                newAircraftStep2Control.Visible = false;
                newVariationStep2Control.Visible = false;
                newAircraftStep1Control.Visible = true;
                buttonOK.Text = "&Next >";
                buttonOK.Enabled = true;
                buttonBack.Enabled = false;
            }
        }

        private void newVariationStep2Control_FormValidatedChanged(object sender, EventArgs e)
        {
            if (wizardStage == WizardStageEnum.Step2Variation)
            {
                if (newVariationStep2Control.FormValid)
                    buttonOK.Enabled = true;
                else
                    buttonOK.Enabled = false;
            }
        }
    }
}
