namespace RCSim.AircraftEditor.Dialogs
{
    partial class AircraftPanelControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonParameters = new System.Windows.Forms.Button();
            this.buttonLift = new System.Windows.Forms.Button();
            this.buttonDrag = new System.Windows.Forms.Button();
            this.buttonSideLift = new System.Windows.Forms.Button();
            this.buttonSideDrag = new System.Windows.Forms.Button();
            this.groupBoxLiftDrag = new System.Windows.Forms.GroupBox();
            this.buttonDragFlaps = new System.Windows.Forms.Button();
            this.buttonLiftFlaps = new System.Windows.Forms.Button();
            this.groupBoxAgility = new System.Windows.Forms.GroupBox();
            this.groupBoxDimensions = new System.Windows.Forms.GroupBox();
            this.buttonDimensions = new System.Windows.Forms.Button();
            this.groupBoxPropulsion = new System.Windows.Forms.GroupBox();
            this.buttonPropulsion = new System.Windows.Forms.Button();
            this.groupBoxFlightTest = new System.Windows.Forms.GroupBox();
            this.buttonReset = new System.Windows.Forms.Button();
            this.checkBoxTestFly = new System.Windows.Forms.CheckBox();
            this.groupBoxInertia = new System.Windows.Forms.GroupBox();
            this.buttonInertia = new System.Windows.Forms.Button();
            this.groupBoxVersion = new System.Windows.Forms.GroupBox();
            this.labelFMVersion = new System.Windows.Forms.Label();
            this.comboBoxVersion = new System.Windows.Forms.ComboBox();
            this.groupBoxLiftDrag.SuspendLayout();
            this.groupBoxAgility.SuspendLayout();
            this.groupBoxDimensions.SuspendLayout();
            this.groupBoxPropulsion.SuspendLayout();
            this.groupBoxFlightTest.SuspendLayout();
            this.groupBoxInertia.SuspendLayout();
            this.groupBoxVersion.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonParameters
            // 
            this.buttonParameters.Location = new System.Drawing.Point(5, 19);
            this.buttonParameters.Name = "buttonParameters";
            this.buttonParameters.Size = new System.Drawing.Size(113, 23);
            this.buttonParameters.TabIndex = 0;
            this.buttonParameters.Text = "Flight Parameters";
            this.buttonParameters.UseVisualStyleBackColor = true;
            this.buttonParameters.Click += new System.EventHandler(this.buttonParameters_Click);
            // 
            // buttonLift
            // 
            this.buttonLift.Location = new System.Drawing.Point(6, 19);
            this.buttonLift.Name = "buttonLift";
            this.buttonLift.Size = new System.Drawing.Size(113, 23);
            this.buttonLift.TabIndex = 0;
            this.buttonLift.Text = "Lift Coefficient";
            this.buttonLift.UseVisualStyleBackColor = true;
            this.buttonLift.Click += new System.EventHandler(this.buttonLift_Click);
            // 
            // buttonDrag
            // 
            this.buttonDrag.Location = new System.Drawing.Point(6, 48);
            this.buttonDrag.Name = "buttonDrag";
            this.buttonDrag.Size = new System.Drawing.Size(113, 23);
            this.buttonDrag.TabIndex = 0;
            this.buttonDrag.Text = "Drag Coefficient";
            this.buttonDrag.UseVisualStyleBackColor = true;
            this.buttonDrag.Click += new System.EventHandler(this.buttonDrag_Click);
            // 
            // buttonSideLift
            // 
            this.buttonSideLift.Location = new System.Drawing.Point(125, 19);
            this.buttonSideLift.Name = "buttonSideLift";
            this.buttonSideLift.Size = new System.Drawing.Size(113, 23);
            this.buttonSideLift.TabIndex = 0;
            this.buttonSideLift.Text = "Side Lift Coefficient";
            this.buttonSideLift.UseVisualStyleBackColor = true;
            this.buttonSideLift.Click += new System.EventHandler(this.buttonSideLift_Click);
            // 
            // buttonSideDrag
            // 
            this.buttonSideDrag.Location = new System.Drawing.Point(125, 48);
            this.buttonSideDrag.Name = "buttonSideDrag";
            this.buttonSideDrag.Size = new System.Drawing.Size(113, 23);
            this.buttonSideDrag.TabIndex = 0;
            this.buttonSideDrag.Text = "Side Drag Coeff.";
            this.buttonSideDrag.UseVisualStyleBackColor = true;
            this.buttonSideDrag.Click += new System.EventHandler(this.buttonSideDrag_Click);
            // 
            // groupBoxLiftDrag
            // 
            this.groupBoxLiftDrag.Controls.Add(this.buttonDragFlaps);
            this.groupBoxLiftDrag.Controls.Add(this.buttonDrag);
            this.groupBoxLiftDrag.Controls.Add(this.buttonLiftFlaps);
            this.groupBoxLiftDrag.Controls.Add(this.buttonLift);
            this.groupBoxLiftDrag.Controls.Add(this.buttonSideDrag);
            this.groupBoxLiftDrag.Controls.Add(this.buttonSideLift);
            this.groupBoxLiftDrag.Location = new System.Drawing.Point(4, 62);
            this.groupBoxLiftDrag.Name = "groupBoxLiftDrag";
            this.groupBoxLiftDrag.Size = new System.Drawing.Size(245, 103);
            this.groupBoxLiftDrag.TabIndex = 1;
            this.groupBoxLiftDrag.TabStop = false;
            this.groupBoxLiftDrag.Text = "Lift && Drag";
            // 
            // buttonDragFlaps
            // 
            this.buttonDragFlaps.Location = new System.Drawing.Point(126, 77);
            this.buttonDragFlaps.Name = "buttonDragFlaps";
            this.buttonDragFlaps.Size = new System.Drawing.Size(113, 23);
            this.buttonDragFlaps.TabIndex = 0;
            this.buttonDragFlaps.Text = "Drag (Flaps)";
            this.buttonDragFlaps.UseVisualStyleBackColor = true;
            this.buttonDragFlaps.Click += new System.EventHandler(this.buttonDragFlaps_Click);
            // 
            // buttonLiftFlaps
            // 
            this.buttonLiftFlaps.Location = new System.Drawing.Point(6, 77);
            this.buttonLiftFlaps.Name = "buttonLiftFlaps";
            this.buttonLiftFlaps.Size = new System.Drawing.Size(113, 23);
            this.buttonLiftFlaps.TabIndex = 0;
            this.buttonLiftFlaps.Text = "Lift (Flaps)";
            this.buttonLiftFlaps.UseVisualStyleBackColor = true;
            this.buttonLiftFlaps.Click += new System.EventHandler(this.buttonLiftFlaps_Click);
            // 
            // groupBoxAgility
            // 
            this.groupBoxAgility.Controls.Add(this.buttonParameters);
            this.groupBoxAgility.Location = new System.Drawing.Point(4, 171);
            this.groupBoxAgility.Name = "groupBoxAgility";
            this.groupBoxAgility.Size = new System.Drawing.Size(245, 48);
            this.groupBoxAgility.TabIndex = 2;
            this.groupBoxAgility.TabStop = false;
            this.groupBoxAgility.Text = "Agility";
            // 
            // groupBoxDimensions
            // 
            this.groupBoxDimensions.Controls.Add(this.buttonDimensions);
            this.groupBoxDimensions.Location = new System.Drawing.Point(4, 225);
            this.groupBoxDimensions.Name = "groupBoxDimensions";
            this.groupBoxDimensions.Size = new System.Drawing.Size(244, 49);
            this.groupBoxDimensions.TabIndex = 3;
            this.groupBoxDimensions.TabStop = false;
            this.groupBoxDimensions.Text = "Dimensions";
            // 
            // buttonDimensions
            // 
            this.buttonDimensions.Location = new System.Drawing.Point(6, 19);
            this.buttonDimensions.Name = "buttonDimensions";
            this.buttonDimensions.Size = new System.Drawing.Size(113, 23);
            this.buttonDimensions.TabIndex = 0;
            this.buttonDimensions.Text = "Dimensions";
            this.buttonDimensions.UseVisualStyleBackColor = true;
            this.buttonDimensions.Click += new System.EventHandler(this.buttonDimensions_Click);
            // 
            // groupBoxPropulsion
            // 
            this.groupBoxPropulsion.Controls.Add(this.buttonPropulsion);
            this.groupBoxPropulsion.Location = new System.Drawing.Point(4, 336);
            this.groupBoxPropulsion.Name = "groupBoxPropulsion";
            this.groupBoxPropulsion.Size = new System.Drawing.Size(244, 50);
            this.groupBoxPropulsion.TabIndex = 4;
            this.groupBoxPropulsion.TabStop = false;
            this.groupBoxPropulsion.Text = "Propulsion";
            // 
            // buttonPropulsion
            // 
            this.buttonPropulsion.Location = new System.Drawing.Point(6, 19);
            this.buttonPropulsion.Name = "buttonPropulsion";
            this.buttonPropulsion.Size = new System.Drawing.Size(151, 23);
            this.buttonPropulsion.TabIndex = 4;
            this.buttonPropulsion.Text = "Propulsion Parameters";
            this.buttonPropulsion.UseVisualStyleBackColor = true;
            this.buttonPropulsion.Click += new System.EventHandler(this.buttonPropulsion_Click);
            // 
            // groupBoxFlightTest
            // 
            this.groupBoxFlightTest.Controls.Add(this.buttonReset);
            this.groupBoxFlightTest.Controls.Add(this.checkBoxTestFly);
            this.groupBoxFlightTest.Location = new System.Drawing.Point(4, 392);
            this.groupBoxFlightTest.Name = "groupBoxFlightTest";
            this.groupBoxFlightTest.Size = new System.Drawing.Size(244, 50);
            this.groupBoxFlightTest.TabIndex = 5;
            this.groupBoxFlightTest.TabStop = false;
            this.groupBoxFlightTest.Text = "Flight test";
            // 
            // buttonReset
            // 
            this.buttonReset.Location = new System.Drawing.Point(162, 15);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(75, 25);
            this.buttonReset.TabIndex = 1;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // checkBoxTestFly
            // 
            this.checkBoxTestFly.AutoSize = true;
            this.checkBoxTestFly.Location = new System.Drawing.Point(10, 20);
            this.checkBoxTestFly.Name = "checkBoxTestFly";
            this.checkBoxTestFly.Size = new System.Drawing.Size(63, 17);
            this.checkBoxTestFly.TabIndex = 0;
            this.checkBoxTestFly.Text = "Test fly!";
            this.checkBoxTestFly.UseVisualStyleBackColor = true;
            this.checkBoxTestFly.CheckedChanged += new System.EventHandler(this.checkBoxTestFly_CheckedChanged);
            // 
            // groupBoxInertia
            // 
            this.groupBoxInertia.Controls.Add(this.buttonInertia);
            this.groupBoxInertia.Location = new System.Drawing.Point(4, 280);
            this.groupBoxInertia.Name = "groupBoxInertia";
            this.groupBoxInertia.Size = new System.Drawing.Size(244, 50);
            this.groupBoxInertia.TabIndex = 4;
            this.groupBoxInertia.TabStop = false;
            this.groupBoxInertia.Text = "Inertia";
            // 
            // buttonInertia
            // 
            this.buttonInertia.Location = new System.Drawing.Point(6, 19);
            this.buttonInertia.Name = "buttonInertia";
            this.buttonInertia.Size = new System.Drawing.Size(151, 23);
            this.buttonInertia.TabIndex = 4;
            this.buttonInertia.Text = "Moments of Inertia";
            this.buttonInertia.UseVisualStyleBackColor = true;
            this.buttonInertia.Click += new System.EventHandler(this.buttonInertia_Click);
            // 
            // groupBoxVersion
            // 
            this.groupBoxVersion.Controls.Add(this.labelFMVersion);
            this.groupBoxVersion.Controls.Add(this.comboBoxVersion);
            this.groupBoxVersion.Location = new System.Drawing.Point(4, 4);
            this.groupBoxVersion.Name = "groupBoxVersion";
            this.groupBoxVersion.Size = new System.Drawing.Size(244, 52);
            this.groupBoxVersion.TabIndex = 6;
            this.groupBoxVersion.TabStop = false;
            this.groupBoxVersion.Text = "Flight Model";
            // 
            // labelFMVersion
            // 
            this.labelFMVersion.AutoSize = true;
            this.labelFMVersion.Location = new System.Drawing.Point(7, 22);
            this.labelFMVersion.Name = "labelFMVersion";
            this.labelFMVersion.Size = new System.Drawing.Size(45, 13);
            this.labelFMVersion.TabIndex = 1;
            this.labelFMVersion.Text = "Version:";
            // 
            // comboBoxVersion
            // 
            this.comboBoxVersion.FormattingEnabled = true;
            this.comboBoxVersion.Items.AddRange(new object[] {
            "Basic",
            "Advanced (3D)"});
            this.comboBoxVersion.Location = new System.Drawing.Point(116, 19);
            this.comboBoxVersion.Name = "comboBoxVersion";
            this.comboBoxVersion.Size = new System.Drawing.Size(121, 21);
            this.comboBoxVersion.TabIndex = 0;
            this.comboBoxVersion.SelectedIndexChanged += new System.EventHandler(this.comboBoxVersion_SelectedIndexChanged);
            // 
            // AircraftPanelControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxVersion);
            this.Controls.Add(this.groupBoxFlightTest);
            this.Controls.Add(this.groupBoxInertia);
            this.Controls.Add(this.groupBoxPropulsion);
            this.Controls.Add(this.groupBoxDimensions);
            this.Controls.Add(this.groupBoxAgility);
            this.Controls.Add(this.groupBoxLiftDrag);
            this.Name = "AircraftPanelControl";
            this.Size = new System.Drawing.Size(251, 450);
            this.groupBoxLiftDrag.ResumeLayout(false);
            this.groupBoxAgility.ResumeLayout(false);
            this.groupBoxDimensions.ResumeLayout(false);
            this.groupBoxPropulsion.ResumeLayout(false);
            this.groupBoxFlightTest.ResumeLayout(false);
            this.groupBoxFlightTest.PerformLayout();
            this.groupBoxInertia.ResumeLayout(false);
            this.groupBoxVersion.ResumeLayout(false);
            this.groupBoxVersion.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonParameters;
        private System.Windows.Forms.Button buttonLift;
        private System.Windows.Forms.Button buttonDrag;
        private System.Windows.Forms.Button buttonSideLift;
        private System.Windows.Forms.Button buttonSideDrag;
        private System.Windows.Forms.GroupBox groupBoxLiftDrag;
        private System.Windows.Forms.GroupBox groupBoxAgility;
        private System.Windows.Forms.GroupBox groupBoxDimensions;
        private System.Windows.Forms.GroupBox groupBoxPropulsion;
        private System.Windows.Forms.GroupBox groupBoxFlightTest;
        private System.Windows.Forms.CheckBox checkBoxTestFly;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Button buttonPropulsion;
        private System.Windows.Forms.GroupBox groupBoxInertia;
        private System.Windows.Forms.Button buttonInertia;
        private System.Windows.Forms.Button buttonDragFlaps;
        private System.Windows.Forms.Button buttonLiftFlaps;
        private System.Windows.Forms.Button buttonDimensions;
        private System.Windows.Forms.GroupBox groupBoxVersion;
        private System.Windows.Forms.Label labelFMVersion;
        private System.Windows.Forms.ComboBox comboBoxVersion;
    }
}
