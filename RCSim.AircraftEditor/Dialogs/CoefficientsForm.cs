using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RCSim.AircraftEditor.Dialogs
{
    internal partial class CoefficientsForm : Form
    {
        protected ModelControl modelControl = null;
        protected CoefficientTypeEnum coefficientType = CoefficientTypeEnum.Lift;

        public enum CoefficientTypeEnum
        {
            Lift,
            Drag,
            SideLift,
            SideDrag,
            LiftFlaps,
            DragFlaps
        }

        public ModelControl ModelControl
        {
            get { return modelControl; }
            set
            {
                modelControl = value;
                switch (this.coefficientType)
                {
                    case CoefficientTypeEnum.Lift:
                        graphControl.ValueList = modelControl.AirplaneModel.AirplaneControl.AircraftParameters.LiftCoefficientPoints;
                        graphControl.BackgroundValueList = null;
                        break;
                    case CoefficientTypeEnum.Drag:
                        graphControl.ValueList = modelControl.AirplaneModel.AirplaneControl.AircraftParameters.DragCoefficientPoints;
                        graphControl.BackgroundValueList = null;
                        /*
                        graphControl.ShowInduced = true;
                        graphControl.Lift = modelControl.AirplaneModel.AirplaneControl.AircraftParameters.LiftCoefficientPoints;
                        graphControl.Drag = modelControl.AirplaneModel.AirplaneControl.AircraftParameters.DragCoefficientPoints;
                         */
                        break;
                    case CoefficientTypeEnum.SideLift:
                        graphControl.ValueList = modelControl.AirplaneModel.AirplaneControl.AircraftParameters.SideLiftCoefficientPoints;
                        graphControl.BackgroundValueList = null;
                        break;
                    case CoefficientTypeEnum.SideDrag:
                        graphControl.ValueList = modelControl.AirplaneModel.AirplaneControl.AircraftParameters.SideDragCoefficientPoints;
                        graphControl.BackgroundValueList = null;
                        break;
                    case CoefficientTypeEnum.LiftFlaps:
                        graphControl.ValueList = modelControl.AirplaneModel.AirplaneControl.AircraftParameters.LiftFlapsCoefficientPoints;
                        graphControl.BackgroundValueList = modelControl.AirplaneModel.AirplaneControl.AircraftParameters.LiftCoefficientPoints;
                        break;
                    case CoefficientTypeEnum.DragFlaps:
                        graphControl.ValueList = modelControl.AirplaneModel.AirplaneControl.AircraftParameters.DragFlapsCoefficientPoints;
                        graphControl.BackgroundValueList = modelControl.AirplaneModel.AirplaneControl.AircraftParameters.DragCoefficientPoints; 
                        break;
                }
            }
        }



        public CoefficientsForm(CoefficientTypeEnum type)
        {
            InitializeComponent();
            this.coefficientType = type;
            ToolTip tip = new ToolTip();
            tip.SetToolTip(buttonShiftUp, "shift all values up by .01");
            tip.SetToolTip(buttonShiftDown, "shift all values down by .01");
            switch (type)
            {
                case CoefficientTypeEnum.Lift:
                    this.labelTitle.Text = "Lift Coefficient vs Alpha";
                    this.Text = "Lift Coefficient";
                    this.labelDescription.Text = "This graph represents the lift coefficient (Y-axis) in relation to the angle of attack or alpha (X-axis). The more efficient the wing, the higher this curve will be. " +
                        "Usually the curve reaches a maximum between 20° and 30° (the 'stall angle') and then drops lower when the wing looses lift after a stall";
                    break;
                case CoefficientTypeEnum.Drag:
                    this.labelTitle.Text = "Drag Coefficient vs Alpha";
                    this.Text = "Drag Coefficient";
                    this.labelDescription.Text = "This graph represents the drag coefficient (Y-axis) in relation to the angle of attack or alpha (X-axis). The more aerodynamic the aircraft and wing, the lower this curve will be. " +
                        "Note that this curve descibes the drag of the entire aircraft, not just the wing.";
                    break;
                case CoefficientTypeEnum.SideLift:
                    this.labelTitle.Text = "Sideways Lift Coefficient vs Beta";
                    this.Text = "Sideways Lift Coefficient";
                    this.labelDescription.Text = "This graph represents the lift coefficient (Y-axis) in relation to the sideways angle of attack or beta (X-axis). The beta angle corresponds to the yaw angle. " +
                        "The higher this curve, the more lift the airframe generates when flying on its side.";
                    break;
                case CoefficientTypeEnum.SideDrag:
                    this.labelTitle.Text = "Sideways Drag Coefficient vs Beta";
                    this.Text = "Sideways Drag Coefficient";
                    this.labelDescription.Text = "This graph represents the sideways drag coefficient (Y-axis) in relation to the sideways angle of attack or beta (X-axis). The beta angle corresponds to the yaw angle. " +
                        "Ths higher this curve, the more drag the airframe generates when flying on its side.";
                    break;
                case CoefficientTypeEnum.LiftFlaps:
                    this.labelTitle.Text = "Lift Coefficient vs Alpha with flaps";
                    this.Text = "Lift Coefficient (flaps)";
                    this.labelDescription.Text = "This graph represents the lift coefficient (Y-axis) in relation to the angle of attack or alpha (X-axis) with flaps fully extendend. The more efficient the wing, the higher this curve will be.";
                    break;
                case CoefficientTypeEnum.DragFlaps:
                    this.labelTitle.Text = "Drag Coefficient vs Alpha with flaps";
                    this.Text = "Drag Coefficient (flaps)";
                    this.labelDescription.Text = "This graph represents the drag coefficient (Y-axis) in relation to the angle of attack or alpha (X-axis) with flaps fully extended. The more aerodynamic the aircraft and wing, the lower this curve will be.";
                    break;
            }
        }

        private void buttonShiftUp_Click(object sender, EventArgs e)
        {
            graphControl.ShiftVertical(0.01);
        }

        private void buttonShiftDown_Click(object sender, EventArgs e)
        {
            graphControl.ShiftVertical(-0.01);
        }

        private void buttonToggleZoom_Click(object sender, EventArgs e)
        {
            if (graphControl.VerticalZoom > 1.1)
                graphControl.VerticalZoom = 1.0;
            else
                graphControl.VerticalZoom = 2.0;
        }
    }
}
