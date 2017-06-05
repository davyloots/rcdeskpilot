namespace RCSim.AircraftEditor.Dialogs
{
    partial class SurfacePropertiesControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SurfacePropertiesControl));
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.numericUpDownMaxAngle = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownScale = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownDefaultAngle = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownMinAngle = new System.Windows.Forms.NumericUpDown();
            this.buttonFile = new System.Windows.Forms.Button();
            this.vectorControlRotationAxis = new RCSim.AircraftEditor.Dialogs.VectorControl();
            this.vectorControlPosition = new RCSim.AircraftEditor.Dialogs.VectorControl();
            this.checkBoxReversed = new System.Windows.Forms.CheckBox();
            this.labelFileName = new System.Windows.Forms.Label();
            this.labelRotation = new System.Windows.Forms.Label();
            this.labelMaxAngle = new System.Windows.Forms.Label();
            this.labelSpanLength = new System.Windows.Forms.Label();
            this.labelScale = new System.Windows.Forms.Label();
            this.labelDefaultAngle = new System.Windows.Forms.Label();
            this.labelMinAngle = new System.Windows.Forms.Label();
            this.labelPosition = new System.Windows.Forms.Label();
            this.labelType = new System.Windows.Forms.Label();
            this.labelChannel = new System.Windows.Forms.Label();
            this.comboBoxChannel = new System.Windows.Forms.ComboBox();
            this.comboBoxType = new System.Windows.Forms.ComboBox();
            this.groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxAngle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDefaultAngle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinAngle)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.numericUpDownMaxAngle);
            this.groupBox.Controls.Add(this.numericUpDownScale);
            this.groupBox.Controls.Add(this.numericUpDownDefaultAngle);
            this.groupBox.Controls.Add(this.numericUpDownMinAngle);
            this.groupBox.Controls.Add(this.buttonFile);
            this.groupBox.Controls.Add(this.vectorControlRotationAxis);
            this.groupBox.Controls.Add(this.vectorControlPosition);
            this.groupBox.Controls.Add(this.checkBoxReversed);
            this.groupBox.Controls.Add(this.labelFileName);
            this.groupBox.Controls.Add(this.labelRotation);
            this.groupBox.Controls.Add(this.labelMaxAngle);
            this.groupBox.Controls.Add(this.labelSpanLength);
            this.groupBox.Controls.Add(this.labelScale);
            this.groupBox.Controls.Add(this.labelDefaultAngle);
            this.groupBox.Controls.Add(this.labelMinAngle);
            this.groupBox.Controls.Add(this.labelPosition);
            this.groupBox.Controls.Add(this.labelType);
            this.groupBox.Controls.Add(this.labelChannel);
            this.groupBox.Controls.Add(this.comboBoxChannel);
            this.groupBox.Controls.Add(this.comboBoxType);
            this.groupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox.Location = new System.Drawing.Point(0, 0);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(256, 252);
            this.groupBox.TabIndex = 0;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Properties";
            // 
            // numericUpDownMaxAngle
            // 
            this.numericUpDownMaxAngle.DecimalPlaces = 1;
            this.numericUpDownMaxAngle.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericUpDownMaxAngle.Location = new System.Drawing.Point(195, 173);
            this.numericUpDownMaxAngle.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.numericUpDownMaxAngle.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.numericUpDownMaxAngle.Name = "numericUpDownMaxAngle";
            this.numericUpDownMaxAngle.Size = new System.Drawing.Size(55, 20);
            this.numericUpDownMaxAngle.TabIndex = 7;
            this.numericUpDownMaxAngle.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownMaxAngle.ValueChanged += new System.EventHandler(this.numericUpDownMaxAngle_ValueChanged);
            // 
            // numericUpDownScale
            // 
            this.numericUpDownScale.DecimalPlaces = 3;
            this.numericUpDownScale.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownScale.Location = new System.Drawing.Point(75, 204);
            this.numericUpDownScale.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownScale.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.numericUpDownScale.Name = "numericUpDownScale";
            this.numericUpDownScale.Size = new System.Drawing.Size(55, 20);
            this.numericUpDownScale.TabIndex = 5;
            this.numericUpDownScale.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownScale.Value = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.numericUpDownScale.ValueChanged += new System.EventHandler(this.numericUpDownScale_ValueChanged);
            // 
            // numericUpDownDefaultAngle
            // 
            this.numericUpDownDefaultAngle.DecimalPlaces = 1;
            this.numericUpDownDefaultAngle.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericUpDownDefaultAngle.Location = new System.Drawing.Point(75, 142);
            this.numericUpDownDefaultAngle.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.numericUpDownDefaultAngle.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.numericUpDownDefaultAngle.Name = "numericUpDownDefaultAngle";
            this.numericUpDownDefaultAngle.Size = new System.Drawing.Size(55, 20);
            this.numericUpDownDefaultAngle.TabIndex = 5;
            this.numericUpDownDefaultAngle.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownDefaultAngle.ValueChanged += new System.EventHandler(this.numericUpDownDefaultAngle_ValueChanged);
            // 
            // numericUpDownMinAngle
            // 
            this.numericUpDownMinAngle.DecimalPlaces = 1;
            this.numericUpDownMinAngle.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericUpDownMinAngle.Location = new System.Drawing.Point(195, 142);
            this.numericUpDownMinAngle.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.numericUpDownMinAngle.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.numericUpDownMinAngle.Name = "numericUpDownMinAngle";
            this.numericUpDownMinAngle.Size = new System.Drawing.Size(55, 20);
            this.numericUpDownMinAngle.TabIndex = 6;
            this.numericUpDownMinAngle.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownMinAngle.ValueChanged += new System.EventHandler(this.numericUpDownMinAngle_ValueChanged);
            // 
            // buttonFile
            // 
            this.buttonFile.Location = new System.Drawing.Point(6, 118);
            this.buttonFile.Name = "buttonFile";
            this.buttonFile.Size = new System.Drawing.Size(53, 23);
            this.buttonFile.TabIndex = 4;
            this.buttonFile.Text = "file...";
            this.buttonFile.UseVisualStyleBackColor = true;
            this.buttonFile.Click += new System.EventHandler(this.buttonFile_Click);
            // 
            // vectorControlRotationAxis
            // 
            this.vectorControlRotationAxis.Location = new System.Drawing.Point(65, 91);
            this.vectorControlRotationAxis.Name = "vectorControlRotationAxis";
            this.vectorControlRotationAxis.Size = new System.Drawing.Size(183, 29);
            this.vectorControlRotationAxis.TabIndex = 3;
            this.vectorControlRotationAxis.Vector = ((Microsoft.DirectX.Vector3)(resources.GetObject("vectorControlRotationAxis.Vector")));
            this.vectorControlRotationAxis.VectorChanged += new System.EventHandler(this.vectorControlRotationAxis_VectorChanged);
            // 
            // vectorControlPosition
            // 
            this.vectorControlPosition.Location = new System.Drawing.Point(65, 62);
            this.vectorControlPosition.Name = "vectorControlPosition";
            this.vectorControlPosition.Size = new System.Drawing.Size(183, 29);
            this.vectorControlPosition.TabIndex = 2;
            this.vectorControlPosition.Vector = ((Microsoft.DirectX.Vector3)(resources.GetObject("vectorControlPosition.Vector")));
            this.vectorControlPosition.VectorChanged += new System.EventHandler(this.vectorControlPosition_VectorChanged);
            // 
            // checkBoxReversed
            // 
            this.checkBoxReversed.AutoSize = true;
            this.checkBoxReversed.Location = new System.Drawing.Point(6, 174);
            this.checkBoxReversed.Name = "checkBoxReversed";
            this.checkBoxReversed.Size = new System.Drawing.Size(72, 17);
            this.checkBoxReversed.TabIndex = 8;
            this.checkBoxReversed.Text = "Reversed";
            this.checkBoxReversed.UseVisualStyleBackColor = true;
            this.checkBoxReversed.CheckedChanged += new System.EventHandler(this.checkBoxReversed_CheckedChanged);
            // 
            // labelFileName
            // 
            this.labelFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFileName.Location = new System.Drawing.Point(65, 123);
            this.labelFileName.Name = "labelFileName";
            this.labelFileName.Size = new System.Drawing.Size(183, 13);
            this.labelFileName.TabIndex = 1;
            // 
            // labelRotation
            // 
            this.labelRotation.AutoSize = true;
            this.labelRotation.Location = new System.Drawing.Point(2, 98);
            this.labelRotation.Name = "labelRotation";
            this.labelRotation.Size = new System.Drawing.Size(51, 13);
            this.labelRotation.TabIndex = 1;
            this.labelRotation.Text = "Rot. axis:";
            // 
            // labelMaxAngle
            // 
            this.labelMaxAngle.AutoSize = true;
            this.labelMaxAngle.Location = new System.Drawing.Point(129, 175);
            this.labelMaxAngle.Name = "labelMaxAngle";
            this.labelMaxAngle.Size = new System.Drawing.Size(65, 13);
            this.labelMaxAngle.TabIndex = 1;
            this.labelMaxAngle.Text = "Max. angle: ";
            // 
            // labelSpanLength
            // 
            this.labelSpanLength.Location = new System.Drawing.Point(136, 206);
            this.labelSpanLength.Name = "labelSpanLength";
            this.labelSpanLength.Size = new System.Drawing.Size(117, 13);
            this.labelSpanLength.TabIndex = 1;
            // 
            // labelScale
            // 
            this.labelScale.AutoSize = true;
            this.labelScale.Location = new System.Drawing.Point(3, 206);
            this.labelScale.Name = "labelScale";
            this.labelScale.Size = new System.Drawing.Size(40, 13);
            this.labelScale.TabIndex = 1;
            this.labelScale.Text = "Scale: ";
            // 
            // labelDefaultAngle
            // 
            this.labelDefaultAngle.AutoSize = true;
            this.labelDefaultAngle.Location = new System.Drawing.Point(3, 144);
            this.labelDefaultAngle.Name = "labelDefaultAngle";
            this.labelDefaultAngle.Size = new System.Drawing.Size(76, 13);
            this.labelDefaultAngle.TabIndex = 1;
            this.labelDefaultAngle.Text = "Default angle: ";
            // 
            // labelMinAngle
            // 
            this.labelMinAngle.AutoSize = true;
            this.labelMinAngle.Location = new System.Drawing.Point(129, 144);
            this.labelMinAngle.Name = "labelMinAngle";
            this.labelMinAngle.Size = new System.Drawing.Size(62, 13);
            this.labelMinAngle.TabIndex = 1;
            this.labelMinAngle.Text = "Min. angle: ";
            // 
            // labelPosition
            // 
            this.labelPosition.AutoSize = true;
            this.labelPosition.Location = new System.Drawing.Point(3, 70);
            this.labelPosition.Name = "labelPosition";
            this.labelPosition.Size = new System.Drawing.Size(47, 13);
            this.labelPosition.TabIndex = 1;
            this.labelPosition.Text = "Position:";
            // 
            // labelType
            // 
            this.labelType.AutoSize = true;
            this.labelType.Location = new System.Drawing.Point(3, 42);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(31, 13);
            this.labelType.TabIndex = 1;
            this.labelType.Text = "Type";
            // 
            // labelChannel
            // 
            this.labelChannel.AutoSize = true;
            this.labelChannel.Location = new System.Drawing.Point(2, 20);
            this.labelChannel.Name = "labelChannel";
            this.labelChannel.Size = new System.Drawing.Size(46, 13);
            this.labelChannel.TabIndex = 1;
            this.labelChannel.Text = "Channel";
            // 
            // comboBoxChannel
            // 
            this.comboBoxChannel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxChannel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxChannel.FormattingEnabled = true;
            this.comboBoxChannel.Items.AddRange(new object[] {
            "None",
            "Elevator",
            "Throttle",
            "Aileron",
            "Rudder",
            "Flaps",
            "Gear"});
            this.comboBoxChannel.Location = new System.Drawing.Point(129, 17);
            this.comboBoxChannel.Name = "comboBoxChannel";
            this.comboBoxChannel.Size = new System.Drawing.Size(121, 21);
            this.comboBoxChannel.TabIndex = 0;
            this.comboBoxChannel.SelectedIndexChanged += new System.EventHandler(this.comboBoxChannel_SelectedIndexChanged);
            // 
            // comboBoxType
            // 
            this.comboBoxType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxType.FormattingEnabled = true;
            this.comboBoxType.Items.AddRange(new object[] {
            "Normal",
            "Reflective",
            "Prop (low RPM)",
            "Prop (high RPM)",
            "Prop (folding low RPM)",
            "Prop (folded)",
            "Rotor (low RPM)",
            "Rotor (high RPM)",
            "Tailrotor (low RPM)",
            "Tailrotor (high RPM)"});
            this.comboBoxType.Location = new System.Drawing.Point(129, 39);
            this.comboBoxType.Name = "comboBoxType";
            this.comboBoxType.Size = new System.Drawing.Size(121, 21);
            this.comboBoxType.TabIndex = 1;
            this.comboBoxType.SelectedIndexChanged += new System.EventHandler(this.comboBoxType_SelectedIndexChanged);
            // 
            // SurfacePropertiesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox);
            this.Name = "SurfacePropertiesControl";
            this.Size = new System.Drawing.Size(256, 252);
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxAngle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDefaultAngle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinAngle)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.Label labelChannel;
        private System.Windows.Forms.ComboBox comboBoxType;
        private System.Windows.Forms.ComboBox comboBoxChannel;
        private System.Windows.Forms.Label labelType;
        private System.Windows.Forms.CheckBox checkBoxReversed;
        private VectorControl vectorControlPosition;
        private VectorControl vectorControlRotationAxis;
        private System.Windows.Forms.Label labelRotation;
        private System.Windows.Forms.Label labelPosition;
        private System.Windows.Forms.Label labelFileName;
        private System.Windows.Forms.Button buttonFile;
        private System.Windows.Forms.NumericUpDown numericUpDownMaxAngle;
        private System.Windows.Forms.NumericUpDown numericUpDownMinAngle;
        private System.Windows.Forms.Label labelMaxAngle;
        private System.Windows.Forms.Label labelMinAngle;
        private System.Windows.Forms.NumericUpDown numericUpDownDefaultAngle;
        private System.Windows.Forms.Label labelDefaultAngle;
        private System.Windows.Forms.NumericUpDown numericUpDownScale;
        private System.Windows.Forms.Label labelScale;
        private System.Windows.Forms.Label labelSpanLength;
    }
}
