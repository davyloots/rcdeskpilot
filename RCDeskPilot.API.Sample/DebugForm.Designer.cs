namespace RCDeskPilot.API.Sample
{
    partial class DebugForm
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
            this.components = new System.ComponentModel.Container();
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.groupBoxPosition = new System.Windows.Forms.GroupBox();
            this.labelAlpha = new System.Windows.Forms.Label();
            this.labelYawPitchRoll = new System.Windows.Forms.Label();
            this.labelPosition = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxAI = new System.Windows.Forms.GroupBox();
            this.attitudeControl = new RCDeskPilot.API.Sample.AttitudeControl();
            this.groupBoxControls = new System.Windows.Forms.GroupBox();
            this.trackBarRudder = new System.Windows.Forms.TrackBar();
            this.trackBarThrottle = new System.Windows.Forms.TrackBar();
            this.trackBarElevator = new System.Windows.Forms.TrackBar();
            this.trackBarAilerons = new System.Windows.Forms.TrackBar();
            this.checkBoxOverride = new System.Windows.Forms.CheckBox();
            this.groupBoxPosition.SuspendLayout();
            this.groupBoxAI.SuspendLayout();
            this.groupBoxControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRudder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarThrottle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarElevator)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAilerons)).BeginInit();
            this.SuspendLayout();
            // 
            // updateTimer
            // 
            this.updateTimer.Enabled = true;
            this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
            // 
            // groupBoxPosition
            // 
            this.groupBoxPosition.Controls.Add(this.labelAlpha);
            this.groupBoxPosition.Controls.Add(this.labelYawPitchRoll);
            this.groupBoxPosition.Controls.Add(this.labelPosition);
            this.groupBoxPosition.Controls.Add(this.label3);
            this.groupBoxPosition.Controls.Add(this.label2);
            this.groupBoxPosition.Controls.Add(this.label1);
            this.groupBoxPosition.Location = new System.Drawing.Point(212, 12);
            this.groupBoxPosition.Name = "groupBoxPosition";
            this.groupBoxPosition.Size = new System.Drawing.Size(281, 92);
            this.groupBoxPosition.TabIndex = 0;
            this.groupBoxPosition.TabStop = false;
            this.groupBoxPosition.Text = "Position && attitude";
            // 
            // labelAlpha
            // 
            this.labelAlpha.Location = new System.Drawing.Point(98, 66);
            this.labelAlpha.Name = "labelAlpha";
            this.labelAlpha.Size = new System.Drawing.Size(163, 13);
            this.labelAlpha.TabIndex = 0;
            this.labelAlpha.Text = "0.0";
            // 
            // labelYawPitchRoll
            // 
            this.labelYawPitchRoll.Location = new System.Drawing.Point(98, 44);
            this.labelYawPitchRoll.Name = "labelYawPitchRoll";
            this.labelYawPitchRoll.Size = new System.Drawing.Size(163, 13);
            this.labelYawPitchRoll.TabIndex = 0;
            this.labelYawPitchRoll.Text = "(0,0,0)";
            // 
            // labelPosition
            // 
            this.labelPosition.Location = new System.Drawing.Point(98, 20);
            this.labelPosition.Name = "labelPosition";
            this.labelPosition.Size = new System.Drawing.Size(163, 13);
            this.labelPosition.TabIndex = 0;
            this.labelPosition.Text = "(0,0,0)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Alpha:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Yaw, Pitch, Roll:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Position:";
            // 
            // groupBoxAI
            // 
            this.groupBoxAI.Controls.Add(this.attitudeControl);
            this.groupBoxAI.Location = new System.Drawing.Point(212, 110);
            this.groupBoxAI.Name = "groupBoxAI";
            this.groupBoxAI.Size = new System.Drawing.Size(280, 212);
            this.groupBoxAI.TabIndex = 2;
            this.groupBoxAI.TabStop = false;
            this.groupBoxAI.Text = "Attitude Indicator";
            // 
            // attitudeControl
            // 
            this.attitudeControl.Altitude = 0F;
            this.attitudeControl.Location = new System.Drawing.Point(48, 19);
            this.attitudeControl.Name = "attitudeControl";
            this.attitudeControl.Pitch = 0F;
            this.attitudeControl.Roll = 0F;
            this.attitudeControl.Size = new System.Drawing.Size(180, 180);
            this.attitudeControl.Speed = 0F;
            this.attitudeControl.TabIndex = 1;
            this.attitudeControl.Text = "attitudeControl1";
            // 
            // groupBoxControls
            // 
            this.groupBoxControls.Controls.Add(this.trackBarRudder);
            this.groupBoxControls.Controls.Add(this.trackBarThrottle);
            this.groupBoxControls.Controls.Add(this.trackBarElevator);
            this.groupBoxControls.Controls.Add(this.trackBarAilerons);
            this.groupBoxControls.Controls.Add(this.checkBoxOverride);
            this.groupBoxControls.Location = new System.Drawing.Point(12, 12);
            this.groupBoxControls.Name = "groupBoxControls";
            this.groupBoxControls.Size = new System.Drawing.Size(194, 310);
            this.groupBoxControls.TabIndex = 3;
            this.groupBoxControls.TabStop = false;
            this.groupBoxControls.Text = "Controls";
            // 
            // trackBarRudder
            // 
            this.trackBarRudder.Location = new System.Drawing.Point(51, 211);
            this.trackBarRudder.Minimum = -10;
            this.trackBarRudder.Name = "trackBarRudder";
            this.trackBarRudder.Size = new System.Drawing.Size(139, 45);
            this.trackBarRudder.TabIndex = 1;
            // 
            // trackBarThrottle
            // 
            this.trackBarThrottle.Location = new System.Drawing.Point(10, 75);
            this.trackBarThrottle.Minimum = -10;
            this.trackBarThrottle.Name = "trackBarThrottle";
            this.trackBarThrottle.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarThrottle.Size = new System.Drawing.Size(45, 139);
            this.trackBarThrottle.TabIndex = 1;
            this.trackBarThrottle.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            // 
            // trackBarElevator
            // 
            this.trackBarElevator.Location = new System.Drawing.Point(100, 75);
            this.trackBarElevator.Minimum = -10;
            this.trackBarElevator.Name = "trackBarElevator";
            this.trackBarElevator.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarElevator.Size = new System.Drawing.Size(45, 139);
            this.trackBarElevator.TabIndex = 1;
            this.trackBarElevator.TickStyle = System.Windows.Forms.TickStyle.Both;
            // 
            // trackBarAilerons
            // 
            this.trackBarAilerons.Location = new System.Drawing.Point(51, 44);
            this.trackBarAilerons.Minimum = -10;
            this.trackBarAilerons.Name = "trackBarAilerons";
            this.trackBarAilerons.Size = new System.Drawing.Size(139, 45);
            this.trackBarAilerons.TabIndex = 1;
            // 
            // checkBoxOverride
            // 
            this.checkBoxOverride.AutoSize = true;
            this.checkBoxOverride.Location = new System.Drawing.Point(10, 20);
            this.checkBoxOverride.Name = "checkBoxOverride";
            this.checkBoxOverride.Size = new System.Drawing.Size(104, 17);
            this.checkBoxOverride.TabIndex = 0;
            this.checkBoxOverride.Text = "override controls";
            this.checkBoxOverride.UseVisualStyleBackColor = true;
            // 
            // DebugForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 334);
            this.ControlBox = false;
            this.Controls.Add(this.groupBoxControls);
            this.Controls.Add(this.groupBoxAI);
            this.Controls.Add(this.groupBoxPosition);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DebugForm";
            this.Text = "Cockpit";
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DebugForm_MouseMove);
            this.groupBoxPosition.ResumeLayout(false);
            this.groupBoxPosition.PerformLayout();
            this.groupBoxAI.ResumeLayout(false);
            this.groupBoxControls.ResumeLayout(false);
            this.groupBoxControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRudder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarThrottle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarElevator)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAilerons)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer updateTimer;
        private System.Windows.Forms.GroupBox groupBoxPosition;
        private System.Windows.Forms.Label labelPosition;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelYawPitchRoll;
        private AttitudeControl attitudeControl;
        private System.Windows.Forms.GroupBox groupBoxAI;
        private System.Windows.Forms.GroupBox groupBoxControls;
        private System.Windows.Forms.TrackBar trackBarAilerons;
        private System.Windows.Forms.CheckBox checkBoxOverride;
        private System.Windows.Forms.TrackBar trackBarRudder;
        private System.Windows.Forms.TrackBar trackBarThrottle;
        private System.Windows.Forms.TrackBar trackBarElevator;
        private System.Windows.Forms.Label labelAlpha;
        private System.Windows.Forms.Label label3;
    }
}