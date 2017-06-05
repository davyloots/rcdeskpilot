using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace RCSim.AircraftEditor.Dialogs
{
    internal partial class AircraftPanelControl : UserControl
    {
        protected AircraftParametersForm parametersForm = null;
        protected CoefficientsForm liftForm = null;
        protected CoefficientsForm dragForm = null;
        protected CoefficientsForm sideLiftForm = null;
        protected CoefficientsForm sideDragForm = null;
        protected CoefficientsForm liftFlapsForm = null;
        protected CoefficientsForm dragFlapsForm = null;
        protected PropulsionForm propulsionForm = null;
        protected AircraftDimensionsForm dimensionsForm = null;
        protected InertiaForm inertiaForm = null;
        protected ModelControl modelControl = null;

        public bool TestFlyEnabled
        {
            get { return checkBoxTestFly.Checked; }
        }

        public event EventHandler TestFlyChanged;

        public ModelControl ModelControl
        {
            get { return modelControl; }
            set 
            { 
                modelControl = value;
                checkBoxTestFly.Checked = false;
                if (modelControl != null)
                {
                    if (modelControl.AirplaneModel.AirplaneControl.AircraftParameters.Version == 1.0)
                        comboBoxVersion.SelectedIndex = 0;
                    else if (modelControl.AirplaneModel.AirplaneControl.AircraftParameters.Version == 2.0)
                        comboBoxVersion.SelectedIndex = 1;
                    else
                        MessageBox.Show("This aircraft was made for a newer version of R/C Desk Pilot");
                }                
                if (liftForm != null)
                    liftForm.Close();
                if (dragForm != null)
                    dragForm.Close();
                if (sideLiftForm != null)
                    sideLiftForm.Close();
                if (sideDragForm != null)
                    sideDragForm.Close();
                if (liftFlapsForm != null)
                    liftFlapsForm.Close();
                if (dragFlapsForm != null)
                    dragFlapsForm.Close();
                if (parametersForm != null)
                    parametersForm.Close();
                if (propulsionForm != null)
                    propulsionForm.Close();
                if (inertiaForm != null)
                    inertiaForm.Close();
                if (dimensionsForm != null)
                    dimensionsForm.Close();
                if (parametersForm != null)
                    parametersForm.Close();
            }
        }

        public AircraftPanelControl()
        {
            InitializeComponent();
        }

        private void buttonParameters_Click(object sender, EventArgs e)
        {
            if ((parametersForm == null) && (modelControl != null))
            {
                parametersForm = new AircraftParametersForm(modelControl.AirplaneModel.AirplaneControl.AircraftParameters);
                parametersForm.FormClosed += new FormClosedEventHandler(parametersForm_FormClosed);
            }
            if (parametersForm != null)
            {
                parametersForm.Show();
                parametersForm.BringToFront();
            }
        }

        void parametersForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (parametersForm != null)
            {
                parametersForm.Dispose();
                parametersForm = null;
            }
        }

        private void buttonLift_Click(object sender, EventArgs e)
        {
            if ((liftForm == null) && (modelControl != null))
            {
                liftForm = new CoefficientsForm(CoefficientsForm.CoefficientTypeEnum.Lift);
                liftForm.ModelControl = modelControl;
                liftForm.FormClosed += new FormClosedEventHandler(liftForm_FormClosed);
            }
            if (liftForm != null)
            {
                liftForm.Show();
                liftForm.BringToFront();
            }
        }

        void liftForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (liftForm != null)
            {
                liftForm.Dispose();
                liftForm = null;
            }
        }

        private void buttonDrag_Click(object sender, EventArgs e)
        {
            if ((dragForm == null) && (modelControl != null))
            {
                dragForm = new CoefficientsForm(CoefficientsForm.CoefficientTypeEnum.Drag);
                dragForm.ModelControl = modelControl;
                dragForm.FormClosed += new FormClosedEventHandler(dragForm_FormClosed);
            }
            if (dragForm != null)
            {
                dragForm.Show();
                dragForm.BringToFront();
            }
        }

        void dragForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (dragForm != null)
            {
                dragForm.Dispose();
                dragForm = null;
            }
        }

        private void buttonSideLift_Click(object sender, EventArgs e)
        {
            if ((sideLiftForm == null) && (modelControl != null))
            {
                sideLiftForm = new CoefficientsForm(CoefficientsForm.CoefficientTypeEnum.SideLift);
                sideLiftForm.ModelControl = modelControl;
                sideLiftForm.FormClosed += new FormClosedEventHandler(sideLiftForm_FormClosed);
            }
            if (sideLiftForm != null)
            {
                sideLiftForm.Show();
                sideLiftForm.BringToFront();
            }
        }

        void sideLiftForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (sideLiftForm != null)
            {
                sideLiftForm.Dispose();
                sideLiftForm = null;
            }
        }

        private void buttonSideDrag_Click(object sender, EventArgs e)
        {
            if ((sideDragForm == null) && (modelControl != null))
            {
                sideDragForm = new CoefficientsForm(CoefficientsForm.CoefficientTypeEnum.SideDrag);
                sideDragForm.ModelControl = modelControl;
                sideDragForm.FormClosed += new FormClosedEventHandler(sideDragForm_FormClosed);
            }
            if (sideDragForm != null)
            {
                sideDragForm.Show();
                sideDragForm.BringToFront();
            }
        }

        void sideDragForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (sideDragForm != null)
            {
                sideDragForm.Dispose();
                sideDragForm = null;
            }
        }

        private void buttonLiftFlaps_Click(object sender, EventArgs e)
        {
            if ((liftFlapsForm == null) && (modelControl != null))
            {
                liftFlapsForm = new CoefficientsForm(CoefficientsForm.CoefficientTypeEnum.LiftFlaps);
                liftFlapsForm.ModelControl = modelControl;
                liftFlapsForm.FormClosed += new FormClosedEventHandler(liftFlapsForm_FormClosed);
            }
            if (liftFlapsForm != null)
            {
                liftFlapsForm.Show();
                liftFlapsForm.BringToFront();
            }
        }

        void liftFlapsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (liftFlapsForm != null)
            {
                liftFlapsForm.Dispose();
                liftFlapsForm = null;
            }
        }

        private void buttonDragFlaps_Click(object sender, EventArgs e)
        {
            if ((dragFlapsForm == null) && (modelControl != null))
            {
                dragFlapsForm = new CoefficientsForm(CoefficientsForm.CoefficientTypeEnum.DragFlaps);
                dragFlapsForm.ModelControl = modelControl;
                dragFlapsForm.FormClosed += new FormClosedEventHandler(dragFlapsForm_FormClosed);
            }
            if (dragFlapsForm != null)
            {
                dragFlapsForm.Show();
                dragFlapsForm.BringToFront();
            }
        }

        void dragFlapsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (dragFlapsForm != null)
            {
                dragFlapsForm.Dispose();
                dragFlapsForm = null;
            }
        }

        private void buttonDimensions_Click(object sender, EventArgs e)
        {
            if ((dimensionsForm == null) && (modelControl != null))
            {
                dimensionsForm = new AircraftDimensionsForm(modelControl);
                dimensionsForm.FormClosed += new FormClosedEventHandler(dimensionsForm_FormClosed);
            }
            if (dimensionsForm != null)
            {
                dimensionsForm.Show();
                dimensionsForm.BringToFront();
            }
        }

        void dimensionsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (dimensionsForm != null)
            {
                dimensionsForm.Dispose();
                dimensionsForm = null;
            }
        }    

        private void buttonPropulsion_Click(object sender, EventArgs e)
        {
            if ((propulsionForm == null) && (modelControl != null))
            {
                propulsionForm = new PropulsionForm();
                propulsionForm.ModelControl = modelControl;
                propulsionForm.FormClosed += new FormClosedEventHandler(propulsionForm_FormClosed);
            }
            if (propulsionForm != null)
            {
                propulsionForm.Show();
                propulsionForm.BringToFront();
            }
        }

        void propulsionForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (propulsionForm != null)
            {
                propulsionForm.Dispose();
                propulsionForm = null;
            }
        }


        private void buttonInertia_Click(object sender, EventArgs e)
        {
            if ((inertiaForm == null) && (modelControl != null))
            {
                inertiaForm = new InertiaForm();
                inertiaForm.ModelControl = modelControl;
                inertiaForm.InertiaChanged += new EventHandler(inertiaForm_InertiaChanged);
                inertiaForm.FormClosed += new FormClosedEventHandler(inertiaForm_FormClosed);
            }
            if (inertiaForm != null)
            {
                inertiaForm.Show();
                inertiaForm.BringToFront();
            }
        }

        void inertiaForm_InertiaChanged(object sender, EventArgs e)
        {
            if (parametersForm != null)
                parametersForm.UpdateValues();
        }

        void inertiaForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (inertiaForm != null)
            {
                inertiaForm.Dispose();
                inertiaForm = null;
            }
        }
          

        private void checkBoxTestFly_CheckedChanged(object sender, EventArgs e)
        {
            if (TestFlyChanged != null)
                TestFlyChanged(this, EventArgs.Empty);
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            if (modelControl != null)
                modelControl.Reset();
        }

        private void comboBoxVersion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (modelControl != null)
            {
                if ((comboBoxVersion.SelectedIndex == 0) && (modelControl.AirplaneModel.AirplaneControl.AircraftParameters.Version != 1.0))
                {
                    if (MessageBox.Show("Are you sure you want to change to the advanced flight model? Some settings will be lost.",
                        "Switch Flight Model", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        modelControl.AirplaneModel.AirplaneControl.AircraftParameters.Version = 1.0;
                        ModelControl = modelControl;
                    }
                    else
                        comboBoxVersion.SelectedIndex = 1;
                }
                else if ((comboBoxVersion.SelectedIndex == 1) && (modelControl.AirplaneModel.AirplaneControl.AircraftParameters.Version != 2.0))
                {
                    if (MessageBox.Show("Are you sure you want to change to the basic flight model? Some settings will be lost.",
                        "Switch Flight Model", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        modelControl.AirplaneModel.AirplaneControl.AircraftParameters.Version = 2.0;
                        ModelControl = modelControl;
                    }
                    else
                        comboBoxVersion.SelectedIndex = 0;
                }
            }
        }        
    }
}
