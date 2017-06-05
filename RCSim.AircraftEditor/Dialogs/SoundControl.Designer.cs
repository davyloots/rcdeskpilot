namespace RCSim.AircraftEditor.Dialogs
{
    partial class SoundControl
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
            this.buttonFile = new System.Windows.Forms.Button();
            this.labelFileName = new System.Windows.Forms.Label();
            this.labelMinFrequency = new System.Windows.Forms.Label();
            this.labelMaxFrequency = new System.Windows.Forms.Label();
            this.numericUpDownMinFreq = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownMaxFreq = new System.Windows.Forms.NumericUpDown();
            this.groupBoxEngine = new System.Windows.Forms.GroupBox();
            this.buttonClear = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinFreq)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxFreq)).BeginInit();
            this.groupBoxEngine.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonFile
            // 
            this.buttonFile.Location = new System.Drawing.Point(6, 19);
            this.buttonFile.Name = "buttonFile";
            this.buttonFile.Size = new System.Drawing.Size(53, 23);
            this.buttonFile.TabIndex = 1;
            this.buttonFile.Text = "file...";
            this.buttonFile.UseVisualStyleBackColor = true;
            this.buttonFile.Click += new System.EventHandler(this.buttonFile_Click);
            // 
            // labelFileName
            // 
            this.labelFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFileName.Location = new System.Drawing.Point(65, 24);
            this.labelFileName.Name = "labelFileName";
            this.labelFileName.Size = new System.Drawing.Size(120, 13);
            this.labelFileName.TabIndex = 5;
            // 
            // labelMinFrequency
            // 
            this.labelMinFrequency.AutoSize = true;
            this.labelMinFrequency.Location = new System.Drawing.Point(3, 56);
            this.labelMinFrequency.Name = "labelMinFrequency";
            this.labelMinFrequency.Size = new System.Drawing.Size(83, 13);
            this.labelMinFrequency.TabIndex = 7;
            this.labelMinFrequency.Text = "Min. Frequency:";
            // 
            // labelMaxFrequency
            // 
            this.labelMaxFrequency.AutoSize = true;
            this.labelMaxFrequency.Location = new System.Drawing.Point(3, 82);
            this.labelMaxFrequency.Name = "labelMaxFrequency";
            this.labelMaxFrequency.Size = new System.Drawing.Size(86, 13);
            this.labelMaxFrequency.TabIndex = 7;
            this.labelMaxFrequency.Text = "Max. Frequency:";
            // 
            // numericUpDownMinFreq
            // 
            this.numericUpDownMinFreq.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDownMinFreq.Location = new System.Drawing.Point(92, 54);
            this.numericUpDownMinFreq.Maximum = new decimal(new int[] {
            120000,
            0,
            0,
            0});
            this.numericUpDownMinFreq.Name = "numericUpDownMinFreq";
            this.numericUpDownMinFreq.Size = new System.Drawing.Size(69, 20);
            this.numericUpDownMinFreq.TabIndex = 2;
            this.numericUpDownMinFreq.Value = new decimal(new int[] {
            22100,
            0,
            0,
            0});
            this.numericUpDownMinFreq.ValueChanged += new System.EventHandler(this.numericUpDownMinFreq_ValueChanged);
            // 
            // numericUpDownMaxFreq
            // 
            this.numericUpDownMaxFreq.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDownMaxFreq.Location = new System.Drawing.Point(92, 80);
            this.numericUpDownMaxFreq.Maximum = new decimal(new int[] {
            120000,
            0,
            0,
            0});
            this.numericUpDownMaxFreq.Name = "numericUpDownMaxFreq";
            this.numericUpDownMaxFreq.Size = new System.Drawing.Size(69, 20);
            this.numericUpDownMaxFreq.TabIndex = 3;
            this.numericUpDownMaxFreq.Value = new decimal(new int[] {
            22100,
            0,
            0,
            0});
            this.numericUpDownMaxFreq.ValueChanged += new System.EventHandler(this.numericUpDownMaxFreq_ValueChanged);
            // 
            // groupBoxEngine
            // 
            this.groupBoxEngine.Controls.Add(this.buttonClear);
            this.groupBoxEngine.Controls.Add(this.buttonFile);
            this.groupBoxEngine.Controls.Add(this.labelFileName);
            this.groupBoxEngine.Controls.Add(this.numericUpDownMaxFreq);
            this.groupBoxEngine.Controls.Add(this.labelMinFrequency);
            this.groupBoxEngine.Controls.Add(this.numericUpDownMinFreq);
            this.groupBoxEngine.Controls.Add(this.labelMaxFrequency);
            this.groupBoxEngine.Location = new System.Drawing.Point(3, 3);
            this.groupBoxEngine.Name = "groupBoxEngine";
            this.groupBoxEngine.Size = new System.Drawing.Size(250, 106);
            this.groupBoxEngine.TabIndex = 9;
            this.groupBoxEngine.TabStop = false;
            this.groupBoxEngine.Text = "Engine";
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(191, 19);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(53, 23);
            this.buttonClear.TabIndex = 1;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // SoundControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxEngine);
            this.Name = "SoundControl";
            this.Size = new System.Drawing.Size(256, 179);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinFreq)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxFreq)).EndInit();
            this.groupBoxEngine.ResumeLayout(false);
            this.groupBoxEngine.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonFile;
        private System.Windows.Forms.Label labelFileName;
        private System.Windows.Forms.Label labelMinFrequency;
        private System.Windows.Forms.Label labelMaxFrequency;
        private System.Windows.Forms.NumericUpDown numericUpDownMinFreq;
        private System.Windows.Forms.NumericUpDown numericUpDownMaxFreq;
        private System.Windows.Forms.GroupBox groupBoxEngine;
        private System.Windows.Forms.Button buttonClear;
    }
}
