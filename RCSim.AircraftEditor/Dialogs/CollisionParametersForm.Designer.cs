namespace RCSim.AircraftEditor.Dialogs
{
    partial class CollisionParametersForm
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
            this.numericUpDownCrashResistanceGear = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownGroundDrag = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownCrashResistance = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownRestitution = new System.Windows.Forms.NumericUpDown();
            this.labelCrashResistanceGear = new System.Windows.Forms.Label();
            this.labelGroundDrag = new System.Windows.Forms.Label();
            this.labelCrashResistance = new System.Windows.Forms.Label();
            this.labelRestitution = new System.Windows.Forms.Label();
            this.labelWaterDrag = new System.Windows.Forms.Label();
            this.numericUpDownWaterDrag = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCrashResistanceGear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownGroundDrag)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCrashResistance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRestitution)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWaterDrag)).BeginInit();
            this.SuspendLayout();
            // 
            // numericUpDownCrashResistanceGear
            // 
            this.numericUpDownCrashResistanceGear.DecimalPlaces = 1;
            this.numericUpDownCrashResistanceGear.Location = new System.Drawing.Point(188, 32);
            this.numericUpDownCrashResistanceGear.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownCrashResistanceGear.Name = "numericUpDownCrashResistanceGear";
            this.numericUpDownCrashResistanceGear.Size = new System.Drawing.Size(65, 20);
            this.numericUpDownCrashResistanceGear.TabIndex = 10;
            this.numericUpDownCrashResistanceGear.ValueChanged += new System.EventHandler(this.numericUpDownCrashResistanceGear_ValueChanged);
            // 
            // numericUpDownGroundDrag
            // 
            this.numericUpDownGroundDrag.DecimalPlaces = 2;
            this.numericUpDownGroundDrag.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDownGroundDrag.Location = new System.Drawing.Point(188, 84);
            this.numericUpDownGroundDrag.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            131072});
            this.numericUpDownGroundDrag.Name = "numericUpDownGroundDrag";
            this.numericUpDownGroundDrag.Size = new System.Drawing.Size(65, 20);
            this.numericUpDownGroundDrag.TabIndex = 12;
            this.numericUpDownGroundDrag.ValueChanged += new System.EventHandler(this.numericUpDownGroundDrag_ValueChanged);
            // 
            // numericUpDownCrashResistance
            // 
            this.numericUpDownCrashResistance.DecimalPlaces = 1;
            this.numericUpDownCrashResistance.Location = new System.Drawing.Point(188, 58);
            this.numericUpDownCrashResistance.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownCrashResistance.Name = "numericUpDownCrashResistance";
            this.numericUpDownCrashResistance.Size = new System.Drawing.Size(65, 20);
            this.numericUpDownCrashResistance.TabIndex = 11;
            this.numericUpDownCrashResistance.ValueChanged += new System.EventHandler(this.numericUpDownCrashResistance_ValueChanged);
            // 
            // numericUpDownRestitution
            // 
            this.numericUpDownRestitution.Location = new System.Drawing.Point(188, 7);
            this.numericUpDownRestitution.Name = "numericUpDownRestitution";
            this.numericUpDownRestitution.Size = new System.Drawing.Size(65, 20);
            this.numericUpDownRestitution.TabIndex = 9;
            this.numericUpDownRestitution.ValueChanged += new System.EventHandler(this.numericUpDownRestitution_ValueChanged);
            // 
            // labelCrashResistanceGear
            // 
            this.labelCrashResistanceGear.AutoSize = true;
            this.labelCrashResistanceGear.Location = new System.Drawing.Point(12, 34);
            this.labelCrashResistanceGear.Name = "labelCrashResistanceGear";
            this.labelCrashResistanceGear.Size = new System.Drawing.Size(142, 13);
            this.labelCrashResistanceGear.TabIndex = 6;
            this.labelCrashResistanceGear.Text = "Crash resistance gear/floats:";
            // 
            // labelGroundDrag
            // 
            this.labelGroundDrag.AutoSize = true;
            this.labelGroundDrag.Location = new System.Drawing.Point(12, 86);
            this.labelGroundDrag.Name = "labelGroundDrag";
            this.labelGroundDrag.Size = new System.Drawing.Size(69, 13);
            this.labelGroundDrag.TabIndex = 5;
            this.labelGroundDrag.Text = "Ground drag:";
            // 
            // labelCrashResistance
            // 
            this.labelCrashResistance.AutoSize = true;
            this.labelCrashResistance.Location = new System.Drawing.Point(12, 60);
            this.labelCrashResistance.Name = "labelCrashResistance";
            this.labelCrashResistance.Size = new System.Drawing.Size(128, 13);
            this.labelCrashResistance.TabIndex = 8;
            this.labelCrashResistance.Text = "Crash resistance airframe:";
            // 
            // labelRestitution
            // 
            this.labelRestitution.AutoSize = true;
            this.labelRestitution.Location = new System.Drawing.Point(12, 9);
            this.labelRestitution.Name = "labelRestitution";
            this.labelRestitution.Size = new System.Drawing.Size(47, 13);
            this.labelRestitution.TabIndex = 7;
            this.labelRestitution.Text = "Bounce:";
            // 
            // labelWaterDrag
            // 
            this.labelWaterDrag.AutoSize = true;
            this.labelWaterDrag.Location = new System.Drawing.Point(12, 112);
            this.labelWaterDrag.Name = "labelWaterDrag";
            this.labelWaterDrag.Size = new System.Drawing.Size(63, 13);
            this.labelWaterDrag.TabIndex = 5;
            this.labelWaterDrag.Text = "Water drag:";
            // 
            // numericUpDownWaterDrag
            // 
            this.numericUpDownWaterDrag.DecimalPlaces = 2;
            this.numericUpDownWaterDrag.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDownWaterDrag.Location = new System.Drawing.Point(188, 110);
            this.numericUpDownWaterDrag.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            131072});
            this.numericUpDownWaterDrag.Name = "numericUpDownWaterDrag";
            this.numericUpDownWaterDrag.Size = new System.Drawing.Size(65, 20);
            this.numericUpDownWaterDrag.TabIndex = 12;
            this.numericUpDownWaterDrag.ValueChanged += new System.EventHandler(this.numericUpDownWaterDrag_ValueChanged);
            // 
            // CollisionParametersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(265, 178);
            this.Controls.Add(this.numericUpDownCrashResistanceGear);
            this.Controls.Add(this.numericUpDownWaterDrag);
            this.Controls.Add(this.numericUpDownGroundDrag);
            this.Controls.Add(this.numericUpDownCrashResistance);
            this.Controls.Add(this.numericUpDownRestitution);
            this.Controls.Add(this.labelCrashResistanceGear);
            this.Controls.Add(this.labelWaterDrag);
            this.Controls.Add(this.labelGroundDrag);
            this.Controls.Add(this.labelCrashResistance);
            this.Controls.Add(this.labelRestitution);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "CollisionParametersForm";
            this.Text = "Collision Parameters";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCrashResistanceGear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownGroundDrag)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCrashResistance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRestitution)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWaterDrag)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numericUpDownCrashResistanceGear;
        private System.Windows.Forms.NumericUpDown numericUpDownGroundDrag;
        private System.Windows.Forms.NumericUpDown numericUpDownCrashResistance;
        private System.Windows.Forms.NumericUpDown numericUpDownRestitution;
        private System.Windows.Forms.Label labelCrashResistanceGear;
        private System.Windows.Forms.Label labelGroundDrag;
        private System.Windows.Forms.Label labelCrashResistance;
        private System.Windows.Forms.Label labelRestitution;
        private System.Windows.Forms.Label labelWaterDrag;
        private System.Windows.Forms.NumericUpDown numericUpDownWaterDrag;
    }
}