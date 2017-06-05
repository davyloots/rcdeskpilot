namespace RCSim.AircraftEditor.Dialogs
{
    partial class InertiaForm
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
            this.labelTitle = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.numericUpDownIxx = new System.Windows.Forms.NumericUpDown();
            this.labelIxx = new System.Windows.Forms.Label();
            this.numericUpDownIyy = new System.Windows.Forms.NumericUpDown();
            this.labelIyy = new System.Windows.Forms.Label();
            this.numericUpDownIzz = new System.Windows.Forms.NumericUpDown();
            this.labelIzz = new System.Windows.Forms.Label();
            this.labelIxxDesc = new System.Windows.Forms.Label();
            this.labelIyyDesc = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxValues = new System.Windows.Forms.GroupBox();
            this.groupBoxWizard = new System.Windows.Forms.GroupBox();
            this.labelInertiaEstimates = new System.Windows.Forms.Label();
            this.labelEstimateValues = new System.Windows.Forms.Label();
            this.buttonUseEstimate = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.labelFuselageWeight = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.labelWingSpan = new System.Windows.Forms.Label();
            this.labelFuselageDiameter = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.numericUpDownWingMass = new System.Windows.Forms.NumericUpDown();
            this.labelLength = new System.Windows.Forms.Label();
            this.numericUpDownChord = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownFuseMass = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownSpan = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownDiameter = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDownLength = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.labelFuseWeightDesc = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelDiameterDesc = new System.Windows.Forms.Label();
            this.labelLengthDesc = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIxx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIyy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIzz)).BeginInit();
            this.groupBoxValues.SuspendLayout();
            this.groupBoxWizard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWingMass)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownChord)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFuseMass)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSpan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDiameter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLength)).BeginInit();
            this.SuspendLayout();
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.Location = new System.Drawing.Point(12, 9);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(149, 18);
            this.labelTitle.TabIndex = 14;
            this.labelTitle.Text = "Moments of Inertia";
            // 
            // pictureBox1
            // 
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(226, 54);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(236, 129);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 15;
            this.pictureBox1.TabStop = false;
            // 
            // numericUpDownIxx
            // 
            this.numericUpDownIxx.DecimalPlaces = 5;
            this.numericUpDownIxx.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDownIxx.Location = new System.Drawing.Point(36, 15);
            this.numericUpDownIxx.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownIxx.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            327680});
            this.numericUpDownIxx.Name = "numericUpDownIxx";
            this.numericUpDownIxx.Size = new System.Drawing.Size(90, 20);
            this.numericUpDownIxx.TabIndex = 0;
            this.numericUpDownIxx.Value = new decimal(new int[] {
            1,
            0,
            0,
            327680});
            this.numericUpDownIxx.ValueChanged += new System.EventHandler(this.numericUpDownIxx_ValueChanged);
            // 
            // labelIxx
            // 
            this.labelIxx.AutoSize = true;
            this.labelIxx.Location = new System.Drawing.Point(7, 17);
            this.labelIxx.Name = "labelIxx";
            this.labelIxx.Size = new System.Drawing.Size(23, 13);
            this.labelIxx.TabIndex = 17;
            this.labelIxx.Text = "Ixx:";
            // 
            // numericUpDownIyy
            // 
            this.numericUpDownIyy.DecimalPlaces = 5;
            this.numericUpDownIyy.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDownIyy.Location = new System.Drawing.Point(36, 41);
            this.numericUpDownIyy.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownIyy.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            327680});
            this.numericUpDownIyy.Name = "numericUpDownIyy";
            this.numericUpDownIyy.Size = new System.Drawing.Size(90, 20);
            this.numericUpDownIyy.TabIndex = 1;
            this.numericUpDownIyy.Value = new decimal(new int[] {
            1,
            0,
            0,
            327680});
            this.numericUpDownIyy.ValueChanged += new System.EventHandler(this.numericUpDownIyy_ValueChanged);
            // 
            // labelIyy
            // 
            this.labelIyy.AutoSize = true;
            this.labelIyy.Location = new System.Drawing.Point(7, 43);
            this.labelIyy.Name = "labelIyy";
            this.labelIyy.Size = new System.Drawing.Size(23, 13);
            this.labelIyy.TabIndex = 17;
            this.labelIyy.Text = "Iyy:";
            // 
            // numericUpDownIzz
            // 
            this.numericUpDownIzz.DecimalPlaces = 5;
            this.numericUpDownIzz.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDownIzz.Location = new System.Drawing.Point(36, 67);
            this.numericUpDownIzz.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownIzz.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            327680});
            this.numericUpDownIzz.Name = "numericUpDownIzz";
            this.numericUpDownIzz.Size = new System.Drawing.Size(90, 20);
            this.numericUpDownIzz.TabIndex = 2;
            this.numericUpDownIzz.Value = new decimal(new int[] {
            1,
            0,
            0,
            327680});
            this.numericUpDownIzz.ValueChanged += new System.EventHandler(this.numericUpDownIzz_ValueChanged);
            // 
            // labelIzz
            // 
            this.labelIzz.AutoSize = true;
            this.labelIzz.Location = new System.Drawing.Point(7, 69);
            this.labelIzz.Name = "labelIzz";
            this.labelIzz.Size = new System.Drawing.Size(23, 13);
            this.labelIzz.TabIndex = 17;
            this.labelIzz.Text = "Izz:";
            // 
            // labelIxxDesc
            // 
            this.labelIxxDesc.AutoSize = true;
            this.labelIxxDesc.Location = new System.Drawing.Point(133, 17);
            this.labelIxxDesc.Name = "labelIxxDesc";
            this.labelIxxDesc.Size = new System.Drawing.Size(207, 13);
            this.labelIxxDesc.TabIndex = 17;
            this.labelIxxDesc.Text = "kg m²   Moment of inertia along the roll axis";
            // 
            // labelIyyDesc
            // 
            this.labelIyyDesc.AutoSize = true;
            this.labelIyyDesc.Location = new System.Drawing.Point(133, 43);
            this.labelIyyDesc.Name = "labelIyyDesc";
            this.labelIyyDesc.Size = new System.Drawing.Size(217, 13);
            this.labelIyyDesc.TabIndex = 17;
            this.labelIyyDesc.Text = "kg m²   Moment of inertia along the pitch axis";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(133, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(213, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "kg m²   Moment of inertia along the yaw axis";
            // 
            // groupBoxValues
            // 
            this.groupBoxValues.Controls.Add(this.numericUpDownIyy);
            this.groupBoxValues.Controls.Add(this.labelIzz);
            this.groupBoxValues.Controls.Add(this.numericUpDownIxx);
            this.groupBoxValues.Controls.Add(this.labelIyy);
            this.groupBoxValues.Controls.Add(this.numericUpDownIzz);
            this.groupBoxValues.Controls.Add(this.label1);
            this.groupBoxValues.Controls.Add(this.labelIxx);
            this.groupBoxValues.Controls.Add(this.labelIyyDesc);
            this.groupBoxValues.Controls.Add(this.labelIxxDesc);
            this.groupBoxValues.Location = new System.Drawing.Point(15, 30);
            this.groupBoxValues.Name = "groupBoxValues";
            this.groupBoxValues.Size = new System.Drawing.Size(469, 93);
            this.groupBoxValues.TabIndex = 18;
            this.groupBoxValues.TabStop = false;
            this.groupBoxValues.Text = "Values";
            // 
            // groupBoxWizard
            // 
            this.groupBoxWizard.Controls.Add(this.labelInertiaEstimates);
            this.groupBoxWizard.Controls.Add(this.labelEstimateValues);
            this.groupBoxWizard.Controls.Add(this.buttonUseEstimate);
            this.groupBoxWizard.Controls.Add(this.label7);
            this.groupBoxWizard.Controls.Add(this.labelFuselageWeight);
            this.groupBoxWizard.Controls.Add(this.pictureBox1);
            this.groupBoxWizard.Controls.Add(this.label6);
            this.groupBoxWizard.Controls.Add(this.labelWingSpan);
            this.groupBoxWizard.Controls.Add(this.labelFuselageDiameter);
            this.groupBoxWizard.Controls.Add(this.label5);
            this.groupBoxWizard.Controls.Add(this.numericUpDownWingMass);
            this.groupBoxWizard.Controls.Add(this.labelLength);
            this.groupBoxWizard.Controls.Add(this.numericUpDownChord);
            this.groupBoxWizard.Controls.Add(this.numericUpDownFuseMass);
            this.groupBoxWizard.Controls.Add(this.numericUpDownSpan);
            this.groupBoxWizard.Controls.Add(this.numericUpDownDiameter);
            this.groupBoxWizard.Controls.Add(this.label4);
            this.groupBoxWizard.Controls.Add(this.numericUpDownLength);
            this.groupBoxWizard.Controls.Add(this.label3);
            this.groupBoxWizard.Controls.Add(this.labelFuseWeightDesc);
            this.groupBoxWizard.Controls.Add(this.label2);
            this.groupBoxWizard.Controls.Add(this.labelDiameterDesc);
            this.groupBoxWizard.Controls.Add(this.labelLengthDesc);
            this.groupBoxWizard.Location = new System.Drawing.Point(15, 130);
            this.groupBoxWizard.Name = "groupBoxWizard";
            this.groupBoxWizard.Size = new System.Drawing.Size(469, 218);
            this.groupBoxWizard.TabIndex = 19;
            this.groupBoxWizard.TabStop = false;
            this.groupBoxWizard.Text = "Calculator";
            // 
            // labelInertiaEstimates
            // 
            this.labelInertiaEstimates.AutoSize = true;
            this.labelInertiaEstimates.Location = new System.Drawing.Point(102, 194);
            this.labelInertiaEstimates.Name = "labelInertiaEstimates";
            this.labelInertiaEstimates.Size = new System.Drawing.Size(88, 13);
            this.labelInertiaEstimates.TabIndex = 20;
            this.labelInertiaEstimates.Text = "Ixx=0;Iyy=0;Izz=0";
            // 
            // labelEstimateValues
            // 
            this.labelEstimateValues.AutoSize = true;
            this.labelEstimateValues.Location = new System.Drawing.Point(7, 194);
            this.labelEstimateValues.Name = "labelEstimateValues";
            this.labelEstimateValues.Size = new System.Drawing.Size(90, 13);
            this.labelEstimateValues.TabIndex = 20;
            this.labelEstimateValues.Text = "Estimated values:";
            // 
            // buttonUseEstimate
            // 
            this.buttonUseEstimate.Location = new System.Drawing.Point(353, 189);
            this.buttonUseEstimate.Name = "buttonUseEstimate";
            this.buttonUseEstimate.Size = new System.Drawing.Size(109, 23);
            this.buttonUseEstimate.TabIndex = 19;
            this.buttonUseEstimate.Text = "Use estimate";
            this.buttonUseEstimate.UseVisualStyleBackColor = true;
            this.buttonUseEstimate.Click += new System.EventHandler(this.buttonUseEstimate_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 165);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(69, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Wing weight:";
            // 
            // labelFuselageWeight
            // 
            this.labelFuselageWeight.AutoSize = true;
            this.labelFuselageWeight.Location = new System.Drawing.Point(12, 99);
            this.labelFuselageWeight.Name = "labelFuselageWeight";
            this.labelFuselageWeight.Size = new System.Drawing.Size(87, 13);
            this.labelFuselageWeight.TabIndex = 18;
            this.labelFuselageWeight.Text = "Fuselage weight:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 143);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Wing chord:";
            // 
            // labelWingSpan
            // 
            this.labelWingSpan.AutoSize = true;
            this.labelWingSpan.Location = new System.Drawing.Point(12, 121);
            this.labelWingSpan.Name = "labelWingSpan";
            this.labelWingSpan.Size = new System.Drawing.Size(58, 13);
            this.labelWingSpan.TabIndex = 18;
            this.labelWingSpan.Text = "Wingspan:";
            // 
            // labelFuselageDiameter
            // 
            this.labelFuselageDiameter.AutoSize = true;
            this.labelFuselageDiameter.Location = new System.Drawing.Point(12, 77);
            this.labelFuselageDiameter.Name = "labelFuselageDiameter";
            this.labelFuselageDiameter.Size = new System.Drawing.Size(96, 13);
            this.labelFuselageDiameter.TabIndex = 18;
            this.labelFuselageDiameter.Text = "Fuselage diameter:";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(7, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(455, 35);
            this.label5.TabIndex = 17;
            this.label5.Text = "Only the values above are used, you can use this calculator the make an estimate " +
                "of these values based on the simplified representation of an airplane depicted b" +
                "elow.";
            // 
            // numericUpDownWingMass
            // 
            this.numericUpDownWingMass.DecimalPlaces = 3;
            this.numericUpDownWingMass.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDownWingMass.Location = new System.Drawing.Point(116, 163);
            this.numericUpDownWingMass.Name = "numericUpDownWingMass";
            this.numericUpDownWingMass.Size = new System.Drawing.Size(58, 20);
            this.numericUpDownWingMass.TabIndex = 8;
            this.numericUpDownWingMass.ValueChanged += new System.EventHandler(this.numericUpDownCalc_ValueChanged);
            // 
            // labelLength
            // 
            this.labelLength.AutoSize = true;
            this.labelLength.Location = new System.Drawing.Point(12, 55);
            this.labelLength.Name = "labelLength";
            this.labelLength.Size = new System.Drawing.Size(85, 13);
            this.labelLength.TabIndex = 18;
            this.labelLength.Text = "Fuselage length:";
            // 
            // numericUpDownChord
            // 
            this.numericUpDownChord.DecimalPlaces = 2;
            this.numericUpDownChord.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDownChord.Location = new System.Drawing.Point(116, 141);
            this.numericUpDownChord.Name = "numericUpDownChord";
            this.numericUpDownChord.Size = new System.Drawing.Size(58, 20);
            this.numericUpDownChord.TabIndex = 7;
            this.numericUpDownChord.ValueChanged += new System.EventHandler(this.numericUpDownCalc_ValueChanged);
            // 
            // numericUpDownFuseMass
            // 
            this.numericUpDownFuseMass.DecimalPlaces = 3;
            this.numericUpDownFuseMass.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDownFuseMass.Location = new System.Drawing.Point(116, 97);
            this.numericUpDownFuseMass.Name = "numericUpDownFuseMass";
            this.numericUpDownFuseMass.Size = new System.Drawing.Size(58, 20);
            this.numericUpDownFuseMass.TabIndex = 5;
            this.numericUpDownFuseMass.ValueChanged += new System.EventHandler(this.numericUpDownCalc_ValueChanged);
            // 
            // numericUpDownSpan
            // 
            this.numericUpDownSpan.DecimalPlaces = 2;
            this.numericUpDownSpan.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownSpan.Location = new System.Drawing.Point(116, 119);
            this.numericUpDownSpan.Name = "numericUpDownSpan";
            this.numericUpDownSpan.Size = new System.Drawing.Size(58, 20);
            this.numericUpDownSpan.TabIndex = 6;
            this.numericUpDownSpan.ValueChanged += new System.EventHandler(this.numericUpDownCalc_ValueChanged);
            // 
            // numericUpDownDiameter
            // 
            this.numericUpDownDiameter.DecimalPlaces = 2;
            this.numericUpDownDiameter.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDownDiameter.Location = new System.Drawing.Point(116, 75);
            this.numericUpDownDiameter.Name = "numericUpDownDiameter";
            this.numericUpDownDiameter.Size = new System.Drawing.Size(58, 20);
            this.numericUpDownDiameter.TabIndex = 4;
            this.numericUpDownDiameter.ValueChanged += new System.EventHandler(this.numericUpDownCalc_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(180, 165);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(19, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "kg";
            // 
            // numericUpDownLength
            // 
            this.numericUpDownLength.DecimalPlaces = 2;
            this.numericUpDownLength.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownLength.Location = new System.Drawing.Point(116, 53);
            this.numericUpDownLength.Name = "numericUpDownLength";
            this.numericUpDownLength.Size = new System.Drawing.Size(58, 20);
            this.numericUpDownLength.TabIndex = 3;
            this.numericUpDownLength.ValueChanged += new System.EventHandler(this.numericUpDownCalc_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(180, 143);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(15, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "m";
            // 
            // labelFuseWeightDesc
            // 
            this.labelFuseWeightDesc.AutoSize = true;
            this.labelFuseWeightDesc.Location = new System.Drawing.Point(180, 99);
            this.labelFuseWeightDesc.Name = "labelFuseWeightDesc";
            this.labelFuseWeightDesc.Size = new System.Drawing.Size(19, 13);
            this.labelFuseWeightDesc.TabIndex = 17;
            this.labelFuseWeightDesc.Text = "kg";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(180, 121);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(15, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "m";
            // 
            // labelDiameterDesc
            // 
            this.labelDiameterDesc.AutoSize = true;
            this.labelDiameterDesc.Location = new System.Drawing.Point(180, 77);
            this.labelDiameterDesc.Name = "labelDiameterDesc";
            this.labelDiameterDesc.Size = new System.Drawing.Size(15, 13);
            this.labelDiameterDesc.TabIndex = 17;
            this.labelDiameterDesc.Text = "m";
            // 
            // labelLengthDesc
            // 
            this.labelLengthDesc.AutoSize = true;
            this.labelLengthDesc.Location = new System.Drawing.Point(180, 55);
            this.labelLengthDesc.Name = "labelLengthDesc";
            this.labelLengthDesc.Size = new System.Drawing.Size(15, 13);
            this.labelLengthDesc.TabIndex = 17;
            this.labelLengthDesc.Text = "m";
            // 
            // InertiaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 358);
            this.Controls.Add(this.groupBoxWizard);
            this.Controls.Add(this.groupBoxValues);
            this.Controls.Add(this.labelTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "InertiaForm";
            this.Text = "Inertia";
            this.Load += new System.EventHandler(this.InertiaForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIxx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIyy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIzz)).EndInit();
            this.groupBoxValues.ResumeLayout(false);
            this.groupBoxValues.PerformLayout();
            this.groupBoxWizard.ResumeLayout(false);
            this.groupBoxWizard.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWingMass)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownChord)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFuseMass)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSpan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDiameter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLength)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.NumericUpDown numericUpDownIxx;
        private System.Windows.Forms.Label labelIxx;
        private System.Windows.Forms.NumericUpDown numericUpDownIyy;
        private System.Windows.Forms.Label labelIyy;
        private System.Windows.Forms.NumericUpDown numericUpDownIzz;
        private System.Windows.Forms.Label labelIzz;
        private System.Windows.Forms.Label labelIxxDesc;
        private System.Windows.Forms.Label labelIyyDesc;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBoxValues;
        private System.Windows.Forms.GroupBox groupBoxWizard;
        private System.Windows.Forms.Label labelFuselageDiameter;
        private System.Windows.Forms.Label labelLength;
        private System.Windows.Forms.NumericUpDown numericUpDownDiameter;
        private System.Windows.Forms.NumericUpDown numericUpDownLength;
        private System.Windows.Forms.Label labelDiameterDesc;
        private System.Windows.Forms.Label labelLengthDesc;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label labelFuselageWeight;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelWingSpan;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numericUpDownWingMass;
        private System.Windows.Forms.NumericUpDown numericUpDownChord;
        private System.Windows.Forms.NumericUpDown numericUpDownFuseMass;
        private System.Windows.Forms.NumericUpDown numericUpDownSpan;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelFuseWeightDesc;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelInertiaEstimates;
        private System.Windows.Forms.Label labelEstimateValues;
        private System.Windows.Forms.Button buttonUseEstimate;
    }
}