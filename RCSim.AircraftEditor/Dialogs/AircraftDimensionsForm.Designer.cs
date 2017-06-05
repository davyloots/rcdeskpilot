namespace RCSim.AircraftEditor.Dialogs
{
    partial class AircraftDimensionsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AircraftDimensionsForm));
            this.labelWeight = new System.Windows.Forms.Label();
            this.labelWingArea = new System.Windows.Forms.Label();
            this.labelVertArea = new System.Windows.Forms.Label();
            this.labelKg = new System.Windows.Forms.Label();
            this.labelCm2 = new System.Windows.Forms.Label();
            this.labelCm22 = new System.Windows.Forms.Label();
            this.labelWeightLbs = new System.Windows.Forms.Label();
            this.labelWingAreaInch = new System.Windows.Forms.Label();
            this.labelVertAreaInch = new System.Windows.Forms.Label();
            this.numericUpDownWeight = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownWingArea = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownVertArea = new System.Windows.Forms.NumericUpDown();
            this.vectorControlWingCenter = new RCSim.AircraftEditor.Dialogs.VectorControl();
            this.labelWingCenter = new System.Windows.Forms.Label();
            this.labelPropCenter = new System.Windows.Forms.Label();
            this.vectorControlPropCenter = new RCSim.AircraftEditor.Dialogs.VectorControl();
            this.labelWingSpan = new System.Windows.Forms.Label();
            this.numericUpDownWingSpan = new System.Windows.Forms.NumericUpDown();
            this.labelWingSpanCm = new System.Windows.Forms.Label();
            this.labelWingSpanInch = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWingArea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVertArea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWingSpan)).BeginInit();
            this.SuspendLayout();
            // 
            // labelWeight
            // 
            this.labelWeight.AutoSize = true;
            this.labelWeight.Location = new System.Drawing.Point(8, 9);
            this.labelWeight.Name = "labelWeight";
            this.labelWeight.Size = new System.Drawing.Size(44, 13);
            this.labelWeight.TabIndex = 0;
            this.labelWeight.Text = "Weight:";
            // 
            // labelWingArea
            // 
            this.labelWingArea.AutoSize = true;
            this.labelWingArea.Location = new System.Drawing.Point(7, 35);
            this.labelWingArea.Name = "labelWingArea";
            this.labelWingArea.Size = new System.Drawing.Size(59, 13);
            this.labelWingArea.TabIndex = 0;
            this.labelWingArea.Text = "Wing area:";
            // 
            // labelVertArea
            // 
            this.labelVertArea.AutoSize = true;
            this.labelVertArea.Location = new System.Drawing.Point(8, 61);
            this.labelVertArea.Name = "labelVertArea";
            this.labelVertArea.Size = new System.Drawing.Size(69, 13);
            this.labelVertArea.TabIndex = 0;
            this.labelVertArea.Text = "Vertical area:";
            // 
            // labelKg
            // 
            this.labelKg.AutoSize = true;
            this.labelKg.Location = new System.Drawing.Point(174, 9);
            this.labelKg.Name = "labelKg";
            this.labelKg.Size = new System.Drawing.Size(20, 13);
            this.labelKg.TabIndex = 0;
            this.labelKg.Text = "Kg";
            // 
            // labelCm2
            // 
            this.labelCm2.AutoSize = true;
            this.labelCm2.Location = new System.Drawing.Point(174, 35);
            this.labelCm2.Name = "labelCm2";
            this.labelCm2.Size = new System.Drawing.Size(24, 13);
            this.labelCm2.TabIndex = 0;
            this.labelCm2.Text = "cm²";
            // 
            // labelCm22
            // 
            this.labelCm22.AutoSize = true;
            this.labelCm22.Location = new System.Drawing.Point(174, 61);
            this.labelCm22.Name = "labelCm22";
            this.labelCm22.Size = new System.Drawing.Size(24, 13);
            this.labelCm22.TabIndex = 0;
            this.labelCm22.Text = "cm²";
            // 
            // labelWeightLbs
            // 
            this.labelWeightLbs.Location = new System.Drawing.Point(204, 9);
            this.labelWeightLbs.Name = "labelWeightLbs";
            this.labelWeightLbs.Size = new System.Drawing.Size(81, 13);
            this.labelWeightLbs.TabIndex = 2;
            this.labelWeightLbs.Text = "lbs.";
            this.labelWeightLbs.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelWingAreaInch
            // 
            this.labelWingAreaInch.Location = new System.Drawing.Point(204, 35);
            this.labelWingAreaInch.Name = "labelWingAreaInch";
            this.labelWingAreaInch.Size = new System.Drawing.Size(81, 13);
            this.labelWingAreaInch.TabIndex = 2;
            this.labelWingAreaInch.Text = "inch²";
            this.labelWingAreaInch.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelVertAreaInch
            // 
            this.labelVertAreaInch.Location = new System.Drawing.Point(204, 61);
            this.labelVertAreaInch.Name = "labelVertAreaInch";
            this.labelVertAreaInch.Size = new System.Drawing.Size(81, 13);
            this.labelVertAreaInch.TabIndex = 2;
            this.labelVertAreaInch.Text = "inch²";
            this.labelVertAreaInch.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // numericUpDownWeight
            // 
            this.numericUpDownWeight.DecimalPlaces = 3;
            this.numericUpDownWeight.Location = new System.Drawing.Point(107, 7);
            this.numericUpDownWeight.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            196608});
            this.numericUpDownWeight.Name = "numericUpDownWeight";
            this.numericUpDownWeight.Size = new System.Drawing.Size(61, 20);
            this.numericUpDownWeight.TabIndex = 3;
            this.numericUpDownWeight.Value = new decimal(new int[] {
            10,
            0,
            0,
            196608});
            this.numericUpDownWeight.ValueChanged += new System.EventHandler(this.numericUpDownWeight_ValueChanged);
            // 
            // numericUpDownWingArea
            // 
            this.numericUpDownWingArea.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownWingArea.Location = new System.Drawing.Point(107, 33);
            this.numericUpDownWingArea.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericUpDownWingArea.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownWingArea.Name = "numericUpDownWingArea";
            this.numericUpDownWingArea.Size = new System.Drawing.Size(61, 20);
            this.numericUpDownWingArea.TabIndex = 3;
            this.numericUpDownWingArea.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownWingArea.ValueChanged += new System.EventHandler(this.numericUpDownWingArea_ValueChanged);
            // 
            // numericUpDownVertArea
            // 
            this.numericUpDownVertArea.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownVertArea.Location = new System.Drawing.Point(107, 59);
            this.numericUpDownVertArea.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericUpDownVertArea.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownVertArea.Name = "numericUpDownVertArea";
            this.numericUpDownVertArea.Size = new System.Drawing.Size(61, 20);
            this.numericUpDownVertArea.TabIndex = 3;
            this.numericUpDownVertArea.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownVertArea.ValueChanged += new System.EventHandler(this.numericUpDownVertArea_ValueChanged);
            // 
            // vectorControlWingCenter
            // 
            this.vectorControlWingCenter.Location = new System.Drawing.Point(102, 109);
            this.vectorControlWingCenter.Name = "vectorControlWingCenter";
            this.vectorControlWingCenter.Size = new System.Drawing.Size(183, 29);
            this.vectorControlWingCenter.TabIndex = 4;
            this.vectorControlWingCenter.Vector = ((Microsoft.DirectX.Vector3)(resources.GetObject("vectorControlWingCenter.Vector")));
            this.vectorControlWingCenter.VectorChanged += new System.EventHandler(this.vectorControlWingCenter_VectorChanged);
            // 
            // labelWingCenter
            // 
            this.labelWingCenter.AutoSize = true;
            this.labelWingCenter.Location = new System.Drawing.Point(8, 117);
            this.labelWingCenter.Name = "labelWingCenter";
            this.labelWingCenter.Size = new System.Drawing.Size(93, 13);
            this.labelWingCenter.TabIndex = 0;
            this.labelWingCenter.Text = "Right wing center:";
            // 
            // labelPropCenter
            // 
            this.labelPropCenter.AutoSize = true;
            this.labelPropCenter.Location = new System.Drawing.Point(7, 151);
            this.labelPropCenter.Name = "labelPropCenter";
            this.labelPropCenter.Size = new System.Drawing.Size(65, 13);
            this.labelPropCenter.TabIndex = 0;
            this.labelPropCenter.Text = "Prop center:";
            // 
            // vectorControlPropCenter
            // 
            this.vectorControlPropCenter.Location = new System.Drawing.Point(102, 144);
            this.vectorControlPropCenter.Name = "vectorControlPropCenter";
            this.vectorControlPropCenter.Size = new System.Drawing.Size(183, 29);
            this.vectorControlPropCenter.TabIndex = 4;
            this.vectorControlPropCenter.Vector = ((Microsoft.DirectX.Vector3)(resources.GetObject("vectorControlPropCenter.Vector")));
            this.vectorControlPropCenter.VectorChanged += new System.EventHandler(this.vectorControlPropCenter_VectorChanged);
            // 
            // labelWingSpan
            // 
            this.labelWingSpan.AutoSize = true;
            this.labelWingSpan.Location = new System.Drawing.Point(8, 88);
            this.labelWingSpan.Name = "labelWingSpan";
            this.labelWingSpan.Size = new System.Drawing.Size(61, 13);
            this.labelWingSpan.TabIndex = 0;
            this.labelWingSpan.Text = "Wing span:";
            // 
            // numericUpDownWingSpan
            // 
            this.numericUpDownWingSpan.Location = new System.Drawing.Point(107, 85);
            this.numericUpDownWingSpan.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownWingSpan.Name = "numericUpDownWingSpan";
            this.numericUpDownWingSpan.Size = new System.Drawing.Size(61, 20);
            this.numericUpDownWingSpan.TabIndex = 3;
            this.numericUpDownWingSpan.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownWingSpan.ValueChanged += new System.EventHandler(this.numericUpDownWingSpan_ValueChanged);
            // 
            // labelWingSpanCm
            // 
            this.labelWingSpanCm.AutoSize = true;
            this.labelWingSpanCm.Location = new System.Drawing.Point(174, 87);
            this.labelWingSpanCm.Name = "labelWingSpanCm";
            this.labelWingSpanCm.Size = new System.Drawing.Size(21, 13);
            this.labelWingSpanCm.TabIndex = 0;
            this.labelWingSpanCm.Text = "cm";
            // 
            // labelWingSpanInch
            // 
            this.labelWingSpanInch.Location = new System.Drawing.Point(204, 87);
            this.labelWingSpanInch.Name = "labelWingSpanInch";
            this.labelWingSpanInch.Size = new System.Drawing.Size(81, 13);
            this.labelWingSpanInch.TabIndex = 2;
            this.labelWingSpanInch.Text = "inch";
            this.labelWingSpanInch.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // AircraftDimensionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(316, 187);
            this.Controls.Add(this.vectorControlPropCenter);
            this.Controls.Add(this.vectorControlWingCenter);
            this.Controls.Add(this.numericUpDownWingSpan);
            this.Controls.Add(this.numericUpDownVertArea);
            this.Controls.Add(this.numericUpDownWingArea);
            this.Controls.Add(this.numericUpDownWeight);
            this.Controls.Add(this.labelWeight);
            this.Controls.Add(this.labelWingSpanInch);
            this.Controls.Add(this.labelVertAreaInch);
            this.Controls.Add(this.labelWingArea);
            this.Controls.Add(this.labelWingAreaInch);
            this.Controls.Add(this.labelPropCenter);
            this.Controls.Add(this.labelWingCenter);
            this.Controls.Add(this.labelWingSpan);
            this.Controls.Add(this.labelVertArea);
            this.Controls.Add(this.labelWeightLbs);
            this.Controls.Add(this.labelKg);
            this.Controls.Add(this.labelWingSpanCm);
            this.Controls.Add(this.labelCm22);
            this.Controls.Add(this.labelCm2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AircraftDimensionsForm";
            this.Text = "AircraftDimensions";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AircraftDimensionsForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWingArea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVertArea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWingSpan)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelWeight;
        private System.Windows.Forms.Label labelWingArea;
        private System.Windows.Forms.Label labelVertArea;
        private System.Windows.Forms.Label labelKg;
        private System.Windows.Forms.Label labelCm2;
        private System.Windows.Forms.Label labelCm22;
        private System.Windows.Forms.Label labelWeightLbs;
        private System.Windows.Forms.Label labelWingAreaInch;
        private System.Windows.Forms.Label labelVertAreaInch;
        private System.Windows.Forms.NumericUpDown numericUpDownWeight;
        private System.Windows.Forms.NumericUpDown numericUpDownWingArea;
        private System.Windows.Forms.NumericUpDown numericUpDownVertArea;
        private VectorControl vectorControlWingCenter;
        private System.Windows.Forms.Label labelWingCenter;
        private System.Windows.Forms.Label labelPropCenter;
        private VectorControl vectorControlPropCenter;
        private System.Windows.Forms.Label labelWingSpan;
        private System.Windows.Forms.NumericUpDown numericUpDownWingSpan;
        private System.Windows.Forms.Label labelWingSpanCm;
        private System.Windows.Forms.Label labelWingSpanInch;

    }
}