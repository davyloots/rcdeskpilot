namespace RCSim.AircraftEditor.Dialogs
{
    partial class AircraftParametersForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBoxYaw = new System.Windows.Forms.GroupBox();
            this.sliderControlYawStability = new RCSim.AircraftEditor.Dialogs.SliderControl();
            this.sliderControlYawDamping = new RCSim.AircraftEditor.Dialogs.SliderControl();
            this.sliderControlRudderEfficiency = new RCSim.AircraftEditor.Dialogs.SliderControl();
            this.groupBoxPitch = new System.Windows.Forms.GroupBox();
            this.sliderControlPitchStability = new RCSim.AircraftEditor.Dialogs.SliderControl();
            this.sliderControlCoG = new RCSim.AircraftEditor.Dialogs.SliderControl();
            this.sliderControlTrim = new RCSim.AircraftEditor.Dialogs.SliderControl();
            this.sliderControlPitchDamping = new RCSim.AircraftEditor.Dialogs.SliderControl();
            this.sliderControlElevatorEfficiency = new RCSim.AircraftEditor.Dialogs.SliderControl();
            this.groupBoxRoll = new System.Windows.Forms.GroupBox();
            this.sliderControlSpinFactor = new RCSim.AircraftEditor.Dialogs.SliderControl();
            this.sliderControlDihedralEfficiency = new RCSim.AircraftEditor.Dialogs.SliderControl();
            this.sliderControlDihedralAngle = new RCSim.AircraftEditor.Dialogs.SliderControl();
            this.sliderControlAileronEfficiency = new RCSim.AircraftEditor.Dialogs.SliderControl();
            this.sliderControlRollDamping = new RCSim.AircraftEditor.Dialogs.SliderControl();
            this.groupBoxPropWash = new System.Windows.Forms.GroupBox();
            this.sliderControlPropWashAilerons = new RCSim.AircraftEditor.Dialogs.SliderControl();
            this.sliderControlPropWashElevator = new RCSim.AircraftEditor.Dialogs.SliderControl();
            this.sliderControlPropWashRudder = new RCSim.AircraftEditor.Dialogs.SliderControl();
            this.groupBoxYaw.SuspendLayout();
            this.groupBoxPitch.SuspendLayout();
            this.groupBoxRoll.SuspendLayout();
            this.groupBoxPropWash.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxYaw
            // 
            this.groupBoxYaw.Controls.Add(this.sliderControlYawStability);
            this.groupBoxYaw.Controls.Add(this.sliderControlYawDamping);
            this.groupBoxYaw.Controls.Add(this.sliderControlRudderEfficiency);
            this.groupBoxYaw.Location = new System.Drawing.Point(13, 5);
            this.groupBoxYaw.Name = "groupBoxYaw";
            this.groupBoxYaw.Size = new System.Drawing.Size(366, 98);
            this.groupBoxYaw.TabIndex = 0;
            this.groupBoxYaw.TabStop = false;
            this.groupBoxYaw.Text = "Yaw";
            // 
            // sliderControlYawStability
            // 
            this.sliderControlYawStability.Factor = 5;
            this.sliderControlYawStability.Location = new System.Drawing.Point(7, 71);
            this.sliderControlYawStability.MaximumValue = 100;
            this.sliderControlYawStability.MinimumValue = 0;
            this.sliderControlYawStability.Name = "sliderControlYawStability";
            this.sliderControlYawStability.ParameterName = "Yaw Stability";
            this.sliderControlYawStability.ParameterValue = 4;
            this.sliderControlYawStability.Size = new System.Drawing.Size(356, 20);
            this.sliderControlYawStability.TabIndex = 2;
            this.sliderControlYawStability.ToolTip = "Yaw stability defines the tendency of the airframe to return to zero sideways ang" +
                "le of attack at airspeed.";
            this.sliderControlYawStability.ValueChanged += new System.EventHandler(this.sliderControlYawStability_ValueChanged);
            // 
            // sliderControlYawDamping
            // 
            this.sliderControlYawDamping.Factor = 50;
            this.sliderControlYawDamping.Location = new System.Drawing.Point(6, 45);
            this.sliderControlYawDamping.MaximumValue = 100;
            this.sliderControlYawDamping.MinimumValue = 0;
            this.sliderControlYawDamping.Name = "sliderControlYawDamping";
            this.sliderControlYawDamping.ParameterName = "Yaw Damping";
            this.sliderControlYawDamping.ParameterValue = 0.1;
            this.sliderControlYawDamping.Size = new System.Drawing.Size(356, 20);
            this.sliderControlYawDamping.TabIndex = 2;
            this.sliderControlYawDamping.ToolTip = "Yaw damping specifies the amount of damping on the yaw rotation.\nIn general, larg" +
                "er vertical tailplanes increases yaw damping";
            this.sliderControlYawDamping.ValueChanged += new System.EventHandler(this.sliderControlYawDamping_ValueChanged);
            // 
            // sliderControlRudderEfficiency
            // 
            this.sliderControlRudderEfficiency.Factor = 250;
            this.sliderControlRudderEfficiency.Location = new System.Drawing.Point(6, 19);
            this.sliderControlRudderEfficiency.MaximumValue = 100;
            this.sliderControlRudderEfficiency.MinimumValue = 0;
            this.sliderControlRudderEfficiency.Name = "sliderControlRudderEfficiency";
            this.sliderControlRudderEfficiency.ParameterName = "Rudder Effectiven.";
            this.sliderControlRudderEfficiency.ParameterValue = 0.05;
            this.sliderControlRudderEfficiency.Size = new System.Drawing.Size(356, 20);
            this.sliderControlRudderEfficiency.TabIndex = 2;
            this.sliderControlRudderEfficiency.ToolTip = "This value defines the effect of the rudder.";
            this.sliderControlRudderEfficiency.ValueChanged += new System.EventHandler(this.sliderControlRudderEfficiency_ValueChanged);
            // 
            // groupBoxPitch
            // 
            this.groupBoxPitch.Controls.Add(this.sliderControlPitchStability);
            this.groupBoxPitch.Controls.Add(this.sliderControlCoG);
            this.groupBoxPitch.Controls.Add(this.sliderControlTrim);
            this.groupBoxPitch.Controls.Add(this.sliderControlPitchDamping);
            this.groupBoxPitch.Controls.Add(this.sliderControlElevatorEfficiency);
            this.groupBoxPitch.Location = new System.Drawing.Point(13, 109);
            this.groupBoxPitch.Name = "groupBoxPitch";
            this.groupBoxPitch.Size = new System.Drawing.Size(366, 148);
            this.groupBoxPitch.TabIndex = 2;
            this.groupBoxPitch.TabStop = false;
            this.groupBoxPitch.Text = "Pitch";
            // 
            // sliderControlPitchStability
            // 
            this.sliderControlPitchStability.Factor = 5;
            this.sliderControlPitchStability.Location = new System.Drawing.Point(7, 96);
            this.sliderControlPitchStability.MaximumValue = 100;
            this.sliderControlPitchStability.MinimumValue = 0;
            this.sliderControlPitchStability.Name = "sliderControlPitchStability";
            this.sliderControlPitchStability.ParameterName = "Pitch Stability";
            this.sliderControlPitchStability.ParameterValue = 4;
            this.sliderControlPitchStability.Size = new System.Drawing.Size(356, 20);
            this.sliderControlPitchStability.TabIndex = 2;
            this.sliderControlPitchStability.ToolTip = "Pitch stability defines the tendency of the airframe to return to zero angle of a" +
                "ttack at airspeed.";
            this.sliderControlPitchStability.ValueChanged += new System.EventHandler(this.sliderControlPitchStability_ValueChanged);
            // 
            // sliderControlCoG
            // 
            this.sliderControlCoG.Factor = 250;
            this.sliderControlCoG.Location = new System.Drawing.Point(7, 122);
            this.sliderControlCoG.MaximumValue = 50;
            this.sliderControlCoG.MinimumValue = -50;
            this.sliderControlCoG.Name = "sliderControlCoG";
            this.sliderControlCoG.ParameterName = "CoG Location";
            this.sliderControlCoG.ParameterValue = 0;
            this.sliderControlCoG.Size = new System.Drawing.Size(356, 20);
            this.sliderControlCoG.TabIndex = 1;
            this.sliderControlCoG.ToolTip = "This value determines the location of the center of gravity in relation to the ce" +
                "nter of lift.";
            this.sliderControlCoG.ValueChanged += new System.EventHandler(this.sliderControlCoG_ValueChanged);
            // 
            // sliderControlTrim
            // 
            this.sliderControlTrim.Factor = 50;
            this.sliderControlTrim.Location = new System.Drawing.Point(6, 71);
            this.sliderControlTrim.MaximumValue = 50;
            this.sliderControlTrim.MinimumValue = -50;
            this.sliderControlTrim.Name = "sliderControlTrim";
            this.sliderControlTrim.ParameterName = "Elevator Trim";
            this.sliderControlTrim.ParameterValue = 0;
            this.sliderControlTrim.Size = new System.Drawing.Size(356, 20);
            this.sliderControlTrim.TabIndex = 1;
            this.sliderControlTrim.ToolTip = "This value determines the default trim in the pitch rotation.\nCenter the slider t" +
                "o trim to neutral.\nA negative value (left), means nose down trim.";
            this.sliderControlTrim.ValueChanged += new System.EventHandler(this.sliderControlTrim_ValueChanged);
            // 
            // sliderControlPitchDamping
            // 
            this.sliderControlPitchDamping.Factor = 50;
            this.sliderControlPitchDamping.Location = new System.Drawing.Point(6, 45);
            this.sliderControlPitchDamping.MaximumValue = 100;
            this.sliderControlPitchDamping.MinimumValue = 0;
            this.sliderControlPitchDamping.Name = "sliderControlPitchDamping";
            this.sliderControlPitchDamping.ParameterName = "Pitch Damping";
            this.sliderControlPitchDamping.ParameterValue = 0.5;
            this.sliderControlPitchDamping.Size = new System.Drawing.Size(356, 20);
            this.sliderControlPitchDamping.TabIndex = 0;
            this.sliderControlPitchDamping.ToolTip = "Pitch Damping specifies the amount of damping on the pitch rotation.\nIn general, " +
                "larger horizontal tailplanes and nose heaviness increase pitch damping";
            this.sliderControlPitchDamping.ValueChanged += new System.EventHandler(this.sliderControlPitchDamping_ValueChanged);
            // 
            // sliderControlElevatorEfficiency
            // 
            this.sliderControlElevatorEfficiency.Factor = 250;
            this.sliderControlElevatorEfficiency.Location = new System.Drawing.Point(6, 19);
            this.sliderControlElevatorEfficiency.MaximumValue = 100;
            this.sliderControlElevatorEfficiency.MinimumValue = 0;
            this.sliderControlElevatorEfficiency.Name = "sliderControlElevatorEfficiency";
            this.sliderControlElevatorEfficiency.ParameterName = "Elevator Effectiven.";
            this.sliderControlElevatorEfficiency.ParameterValue = 0.05;
            this.sliderControlElevatorEfficiency.Size = new System.Drawing.Size(356, 20);
            this.sliderControlElevatorEfficiency.TabIndex = 0;
            this.sliderControlElevatorEfficiency.ToolTip = "This value defines the effect of the elevator.";
            this.sliderControlElevatorEfficiency.ValueChanged += new System.EventHandler(this.sliderControlElevatorEfficiency_ValueChanged);
            // 
            // groupBoxRoll
            // 
            this.groupBoxRoll.Controls.Add(this.sliderControlSpinFactor);
            this.groupBoxRoll.Controls.Add(this.sliderControlDihedralEfficiency);
            this.groupBoxRoll.Controls.Add(this.sliderControlDihedralAngle);
            this.groupBoxRoll.Controls.Add(this.sliderControlAileronEfficiency);
            this.groupBoxRoll.Controls.Add(this.sliderControlRollDamping);
            this.groupBoxRoll.Location = new System.Drawing.Point(13, 263);
            this.groupBoxRoll.Name = "groupBoxRoll";
            this.groupBoxRoll.Size = new System.Drawing.Size(366, 157);
            this.groupBoxRoll.TabIndex = 3;
            this.groupBoxRoll.TabStop = false;
            this.groupBoxRoll.Text = "Roll";
            // 
            // sliderControlSpinFactor
            // 
            this.sliderControlSpinFactor.Factor = 20;
            this.sliderControlSpinFactor.Location = new System.Drawing.Point(7, 127);
            this.sliderControlSpinFactor.MaximumValue = 100;
            this.sliderControlSpinFactor.MinimumValue = 0;
            this.sliderControlSpinFactor.Name = "sliderControlSpinFactor";
            this.sliderControlSpinFactor.ParameterName = "Tip Stall Tendency";
            this.sliderControlSpinFactor.ParameterValue = 0.2;
            this.sliderControlSpinFactor.Size = new System.Drawing.Size(356, 20);
            this.sliderControlSpinFactor.TabIndex = 3;
            this.sliderControlSpinFactor.ToolTip = "Indicates the vulnerability to tip stalling.";
            this.sliderControlSpinFactor.ValueChanged += new System.EventHandler(this.sliderControlSpinFactor_ValueChanged);
            // 
            // sliderControlDihedralEfficiency
            // 
            this.sliderControlDihedralEfficiency.Factor = 50;
            this.sliderControlDihedralEfficiency.Location = new System.Drawing.Point(7, 100);
            this.sliderControlDihedralEfficiency.MaximumValue = 100;
            this.sliderControlDihedralEfficiency.MinimumValue = 0;
            this.sliderControlDihedralEfficiency.Name = "sliderControlDihedralEfficiency";
            this.sliderControlDihedralEfficiency.ParameterName = "Dihedral Effectiven.";
            this.sliderControlDihedralEfficiency.ParameterValue = 0.2;
            this.sliderControlDihedralEfficiency.Size = new System.Drawing.Size(356, 20);
            this.sliderControlDihedralEfficiency.TabIndex = 2;
            this.sliderControlDihedralEfficiency.ToolTip = "This number is multiplied with the Dihedral Angle to determine the effect of the " +
                "dihedral.";
            this.sliderControlDihedralEfficiency.ValueChanged += new System.EventHandler(this.sliderControlDihedralEfficiency_ValueChanged);
            // 
            // sliderControlDihedralAngle
            // 
            this.sliderControlDihedralAngle.Factor = 1;
            this.sliderControlDihedralAngle.Location = new System.Drawing.Point(7, 73);
            this.sliderControlDihedralAngle.MaximumValue = 90;
            this.sliderControlDihedralAngle.MinimumValue = 0;
            this.sliderControlDihedralAngle.Name = "sliderControlDihedralAngle";
            this.sliderControlDihedralAngle.ParameterName = "Dihedral Angle";
            this.sliderControlDihedralAngle.ParameterValue = 4;
            this.sliderControlDihedralAngle.Size = new System.Drawing.Size(356, 20);
            this.sliderControlDihedralAngle.TabIndex = 1;
            this.sliderControlDihedralAngle.ToolTip = "The number of degrees of dihedral.";
            this.sliderControlDihedralAngle.ValueChanged += new System.EventHandler(this.sliderControlDihedralAngle_ValueChanged);
            // 
            // sliderControlAileronEfficiency
            // 
            this.sliderControlAileronEfficiency.Factor = 250;
            this.sliderControlAileronEfficiency.Location = new System.Drawing.Point(7, 20);
            this.sliderControlAileronEfficiency.MaximumValue = 100;
            this.sliderControlAileronEfficiency.MinimumValue = 0;
            this.sliderControlAileronEfficiency.Name = "sliderControlAileronEfficiency";
            this.sliderControlAileronEfficiency.ParameterName = "Aileron Effectiven.";
            this.sliderControlAileronEfficiency.ParameterValue = 0.05;
            this.sliderControlAileronEfficiency.Size = new System.Drawing.Size(356, 20);
            this.sliderControlAileronEfficiency.TabIndex = 0;
            this.sliderControlAileronEfficiency.ToolTip = "This value defines the effect of the ailerons.";
            this.sliderControlAileronEfficiency.ValueChanged += new System.EventHandler(this.sliderControlAileronEfficiency_ValueChanged);
            // 
            // sliderControlRollDamping
            // 
            this.sliderControlRollDamping.Factor = 50;
            this.sliderControlRollDamping.Location = new System.Drawing.Point(7, 46);
            this.sliderControlRollDamping.MaximumValue = 100;
            this.sliderControlRollDamping.MinimumValue = 0;
            this.sliderControlRollDamping.Name = "sliderControlRollDamping";
            this.sliderControlRollDamping.ParameterName = "Roll Damping";
            this.sliderControlRollDamping.ParameterValue = 0.5;
            this.sliderControlRollDamping.Size = new System.Drawing.Size(356, 20);
            this.sliderControlRollDamping.TabIndex = 0;
            this.sliderControlRollDamping.ToolTip = "Roll Damping specifies the amount of damping on the roll rotation.\n\nIn general, l" +
                "arger wingspan increase roll damping";
            this.sliderControlRollDamping.ValueChanged += new System.EventHandler(this.sliderControlRollDamping_ValueChanged);
            // 
            // groupBoxPropWash
            // 
            this.groupBoxPropWash.Controls.Add(this.sliderControlPropWashAilerons);
            this.groupBoxPropWash.Controls.Add(this.sliderControlPropWashElevator);
            this.groupBoxPropWash.Controls.Add(this.sliderControlPropWashRudder);
            this.groupBoxPropWash.Location = new System.Drawing.Point(13, 426);
            this.groupBoxPropWash.Name = "groupBoxPropWash";
            this.groupBoxPropWash.Size = new System.Drawing.Size(366, 100);
            this.groupBoxPropWash.TabIndex = 4;
            this.groupBoxPropWash.TabStop = false;
            this.groupBoxPropWash.Text = "Prop Wash";
            // 
            // sliderControlPropWashAilerons
            // 
            this.sliderControlPropWashAilerons.Factor = 1;
            this.sliderControlPropWashAilerons.Location = new System.Drawing.Point(7, 72);
            this.sliderControlPropWashAilerons.MaximumValue = 100;
            this.sliderControlPropWashAilerons.MinimumValue = 0;
            this.sliderControlPropWashAilerons.Name = "sliderControlPropWashAilerons";
            this.sliderControlPropWashAilerons.ParameterName = "Ailerons";
            this.sliderControlPropWashAilerons.ParameterValue = 20;
            this.sliderControlPropWashAilerons.Size = new System.Drawing.Size(356, 20);
            this.sliderControlPropWashAilerons.TabIndex = 0;
            this.sliderControlPropWashAilerons.ToolTip = "The effect the prop wash has on the ailerons.";
            this.sliderControlPropWashAilerons.ValueChanged += new System.EventHandler(this.sliderControlPropWashAilerons_ValueChanged);
            // 
            // sliderControlPropWashElevator
            // 
            this.sliderControlPropWashElevator.Factor = 1;
            this.sliderControlPropWashElevator.Location = new System.Drawing.Point(7, 46);
            this.sliderControlPropWashElevator.MaximumValue = 100;
            this.sliderControlPropWashElevator.MinimumValue = 0;
            this.sliderControlPropWashElevator.Name = "sliderControlPropWashElevator";
            this.sliderControlPropWashElevator.ParameterName = "Elevator";
            this.sliderControlPropWashElevator.ParameterValue = 20;
            this.sliderControlPropWashElevator.Size = new System.Drawing.Size(356, 20);
            this.sliderControlPropWashElevator.TabIndex = 0;
            this.sliderControlPropWashElevator.ToolTip = "The effect the prop wash has on the elevator.";
            this.sliderControlPropWashElevator.ValueChanged += new System.EventHandler(this.sliderControlPropWashElevator_ValueChanged);
            // 
            // sliderControlPropWashRudder
            // 
            this.sliderControlPropWashRudder.Factor = 1;
            this.sliderControlPropWashRudder.Location = new System.Drawing.Point(7, 20);
            this.sliderControlPropWashRudder.MaximumValue = 100;
            this.sliderControlPropWashRudder.MinimumValue = 0;
            this.sliderControlPropWashRudder.Name = "sliderControlPropWashRudder";
            this.sliderControlPropWashRudder.ParameterName = "Rudder";
            this.sliderControlPropWashRudder.ParameterValue = 20;
            this.sliderControlPropWashRudder.Size = new System.Drawing.Size(356, 20);
            this.sliderControlPropWashRudder.TabIndex = 0;
            this.sliderControlPropWashRudder.ToolTip = "The effect the prop wash has on the rudder.";
            this.sliderControlPropWashRudder.ValueChanged += new System.EventHandler(this.sliderControlPropWashRudder_ValueChanged);
            // 
            // AircraftParametersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 528);
            this.Controls.Add(this.groupBoxPropWash);
            this.Controls.Add(this.groupBoxRoll);
            this.Controls.Add(this.groupBoxPitch);
            this.Controls.Add(this.groupBoxYaw);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AircraftParametersForm";
            this.Text = "Flight Parameters";
            this.groupBoxYaw.ResumeLayout(false);
            this.groupBoxPitch.ResumeLayout(false);
            this.groupBoxRoll.ResumeLayout(false);
            this.groupBoxPropWash.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxYaw;
        private SliderControl sliderControlRudderEfficiency;
        private SliderControl sliderControlYawDamping;
        private System.Windows.Forms.GroupBox groupBoxPitch;
        private SliderControl sliderControlPitchDamping;
        private SliderControl sliderControlElevatorEfficiency;
        private SliderControl sliderControlTrim;
        private System.Windows.Forms.GroupBox groupBoxRoll;
        private SliderControl sliderControlAileronEfficiency;
        private SliderControl sliderControlRollDamping;
        private SliderControl sliderControlDihedralAngle;
        private SliderControl sliderControlDihedralEfficiency;
        private SliderControl sliderControlSpinFactor;
        private System.Windows.Forms.GroupBox groupBoxPropWash;
        private SliderControl sliderControlPropWashAilerons;
        private SliderControl sliderControlPropWashElevator;
        private SliderControl sliderControlPropWashRudder;
        private SliderControl sliderControlYawStability;
        private SliderControl sliderControlPitchStability;
        private SliderControl sliderControlCoG;
    }
}