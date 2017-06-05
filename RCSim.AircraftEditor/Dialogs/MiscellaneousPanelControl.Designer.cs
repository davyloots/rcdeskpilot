namespace RCSim.AircraftEditor.Dialogs
{
    partial class MiscellaneousPanelControl
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
            this.groupBoxOptions = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDownGearDelay = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownFlapsDelay = new System.Windows.Forms.NumericUpDown();
            this.checkBoxAllowsTowing = new System.Windows.Forms.CheckBox();
            this.checkBoxAilerons = new System.Windows.Forms.CheckBox();
            this.checkBoxHandLaunched = new System.Windows.Forms.CheckBox();
            this.checkBoxHasRetracts = new System.Windows.Forms.CheckBox();
            this.checkBoxHasFlaps = new System.Windows.Forms.CheckBox();
            this.checkBoxVariometer = new System.Windows.Forms.CheckBox();
            this.groupBoxDescription = new System.Windows.Forms.GroupBox();
            this.buttonFile = new System.Windows.Forms.Button();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.labelIconFile = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.labelDescTitle = new System.Windows.Forms.Label();
            this.checkBoxHasFloats = new System.Windows.Forms.CheckBox();
            this.groupBoxOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownGearDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFlapsDelay)).BeginInit();
            this.groupBoxDescription.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxOptions
            // 
            this.groupBoxOptions.Controls.Add(this.checkBoxHasFloats);
            this.groupBoxOptions.Controls.Add(this.label3);
            this.groupBoxOptions.Controls.Add(this.label5);
            this.groupBoxOptions.Controls.Add(this.label4);
            this.groupBoxOptions.Controls.Add(this.label2);
            this.groupBoxOptions.Controls.Add(this.numericUpDownGearDelay);
            this.groupBoxOptions.Controls.Add(this.numericUpDownFlapsDelay);
            this.groupBoxOptions.Controls.Add(this.checkBoxAllowsTowing);
            this.groupBoxOptions.Controls.Add(this.checkBoxAilerons);
            this.groupBoxOptions.Controls.Add(this.checkBoxHandLaunched);
            this.groupBoxOptions.Controls.Add(this.checkBoxHasRetracts);
            this.groupBoxOptions.Controls.Add(this.checkBoxHasFlaps);
            this.groupBoxOptions.Controls.Add(this.checkBoxVariometer);
            this.groupBoxOptions.Location = new System.Drawing.Point(3, 3);
            this.groupBoxOptions.Name = "groupBoxOptions";
            this.groupBoxOptions.Size = new System.Drawing.Size(245, 231);
            this.groupBoxOptions.TabIndex = 0;
            this.groupBoxOptions.TabStop = false;
            this.groupBoxOptions.Text = "Options";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 111);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "delay:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(137, 111);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "seconds";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(136, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "seconds";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "delay:";
            // 
            // numericUpDownGearDelay
            // 
            this.numericUpDownGearDelay.DecimalPlaces = 1;
            this.numericUpDownGearDelay.Location = new System.Drawing.Point(75, 109);
            this.numericUpDownGearDelay.Name = "numericUpDownGearDelay";
            this.numericUpDownGearDelay.Size = new System.Drawing.Size(56, 20);
            this.numericUpDownGearDelay.TabIndex = 1;
            this.numericUpDownGearDelay.ValueChanged += new System.EventHandler(this.numericUpDownGearDelay_ValueChanged);
            // 
            // numericUpDownFlapsDelay
            // 
            this.numericUpDownFlapsDelay.DecimalPlaces = 1;
            this.numericUpDownFlapsDelay.Location = new System.Drawing.Point(75, 65);
            this.numericUpDownFlapsDelay.Name = "numericUpDownFlapsDelay";
            this.numericUpDownFlapsDelay.Size = new System.Drawing.Size(56, 20);
            this.numericUpDownFlapsDelay.TabIndex = 1;
            this.numericUpDownFlapsDelay.ValueChanged += new System.EventHandler(this.numericUpDownFlapsDelay_ValueChanged);
            // 
            // checkBoxAllowsTowing
            // 
            this.checkBoxAllowsTowing.AutoSize = true;
            this.checkBoxAllowsTowing.Location = new System.Drawing.Point(7, 182);
            this.checkBoxAllowsTowing.Name = "checkBoxAllowsTowing";
            this.checkBoxAllowsTowing.Size = new System.Drawing.Size(89, 17);
            this.checkBoxAllowsTowing.TabIndex = 0;
            this.checkBoxAllowsTowing.Text = "allows towing";
            this.checkBoxAllowsTowing.UseVisualStyleBackColor = true;
            this.checkBoxAllowsTowing.CheckedChanged += new System.EventHandler(this.checkBoxAllowsTowing_CheckedChanged);
            // 
            // checkBoxAilerons
            // 
            this.checkBoxAilerons.AutoSize = true;
            this.checkBoxAilerons.Location = new System.Drawing.Point(7, 159);
            this.checkBoxAilerons.Name = "checkBoxAilerons";
            this.checkBoxAilerons.Size = new System.Drawing.Size(82, 17);
            this.checkBoxAilerons.TabIndex = 0;
            this.checkBoxAilerons.Text = "has ailerons";
            this.checkBoxAilerons.UseVisualStyleBackColor = true;
            this.checkBoxAilerons.CheckedChanged += new System.EventHandler(this.checkBoxAilerons_CheckedChanged);
            // 
            // checkBoxHandLaunched
            // 
            this.checkBoxHandLaunched.AutoSize = true;
            this.checkBoxHandLaunched.Location = new System.Drawing.Point(7, 136);
            this.checkBoxHandLaunched.Name = "checkBoxHandLaunched";
            this.checkBoxHandLaunched.Size = new System.Drawing.Size(97, 17);
            this.checkBoxHandLaunched.TabIndex = 0;
            this.checkBoxHandLaunched.Text = "hand launched";
            this.checkBoxHandLaunched.UseVisualStyleBackColor = true;
            this.checkBoxHandLaunched.CheckedChanged += new System.EventHandler(this.checkBoxHandLaunched_CheckedChanged);
            // 
            // checkBoxHasRetracts
            // 
            this.checkBoxHasRetracts.AutoSize = true;
            this.checkBoxHasRetracts.Location = new System.Drawing.Point(7, 91);
            this.checkBoxHasRetracts.Name = "checkBoxHasRetracts";
            this.checkBoxHasRetracts.Size = new System.Drawing.Size(164, 17);
            this.checkBoxHasRetracts.TabIndex = 0;
            this.checkBoxHasRetracts.Text = "has retractable undercarriage";
            this.checkBoxHasRetracts.UseVisualStyleBackColor = true;
            this.checkBoxHasRetracts.CheckedChanged += new System.EventHandler(this.checkBoxHasRetracts_CheckedChanged);
            // 
            // checkBoxHasFlaps
            // 
            this.checkBoxHasFlaps.AutoSize = true;
            this.checkBoxHasFlaps.Location = new System.Drawing.Point(7, 43);
            this.checkBoxHasFlaps.Name = "checkBoxHasFlaps";
            this.checkBoxHasFlaps.Size = new System.Drawing.Size(68, 17);
            this.checkBoxHasFlaps.TabIndex = 0;
            this.checkBoxHasFlaps.Text = "has flaps";
            this.checkBoxHasFlaps.UseVisualStyleBackColor = true;
            this.checkBoxHasFlaps.CheckedChanged += new System.EventHandler(this.checkBoxHasFlaps_CheckedChanged);
            // 
            // checkBoxVariometer
            // 
            this.checkBoxVariometer.AutoSize = true;
            this.checkBoxVariometer.Location = new System.Drawing.Point(7, 20);
            this.checkBoxVariometer.Name = "checkBoxVariometer";
            this.checkBoxVariometer.Size = new System.Drawing.Size(95, 17);
            this.checkBoxVariometer.TabIndex = 0;
            this.checkBoxVariometer.Text = "has variometer";
            this.checkBoxVariometer.UseVisualStyleBackColor = true;
            this.checkBoxVariometer.CheckedChanged += new System.EventHandler(this.checkBoxVariometer_CheckedChanged);
            // 
            // groupBoxDescription
            // 
            this.groupBoxDescription.Controls.Add(this.buttonFile);
            this.groupBoxDescription.Controls.Add(this.textBoxDescription);
            this.groupBoxDescription.Controls.Add(this.labelIconFile);
            this.groupBoxDescription.Controls.Add(this.label1);
            this.groupBoxDescription.Controls.Add(this.labelDescTitle);
            this.groupBoxDescription.Location = new System.Drawing.Point(3, 240);
            this.groupBoxDescription.Name = "groupBoxDescription";
            this.groupBoxDescription.Size = new System.Drawing.Size(245, 159);
            this.groupBoxDescription.TabIndex = 2;
            this.groupBoxDescription.TabStop = false;
            this.groupBoxDescription.Text = "Description";
            // 
            // buttonFile
            // 
            this.buttonFile.Location = new System.Drawing.Point(10, 130);
            this.buttonFile.Name = "buttonFile";
            this.buttonFile.Size = new System.Drawing.Size(53, 23);
            this.buttonFile.TabIndex = 2;
            this.buttonFile.Text = "file...";
            this.buttonFile.UseVisualStyleBackColor = true;
            this.buttonFile.Click += new System.EventHandler(this.buttonFile_Click);
            // 
            // textBoxDescription
            // 
            this.textBoxDescription.Location = new System.Drawing.Point(10, 36);
            this.textBoxDescription.Multiline = true;
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.Size = new System.Drawing.Size(229, 75);
            this.textBoxDescription.TabIndex = 1;
            this.textBoxDescription.TextChanged += new System.EventHandler(this.textBoxDescription_TextChanged);
            // 
            // labelIconFile
            // 
            this.labelIconFile.Location = new System.Drawing.Point(69, 135);
            this.labelIconFile.Name = "labelIconFile";
            this.labelIconFile.Size = new System.Drawing.Size(170, 13);
            this.labelIconFile.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 114);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "In menu icon:";
            // 
            // labelDescTitle
            // 
            this.labelDescTitle.AutoSize = true;
            this.labelDescTitle.Location = new System.Drawing.Point(7, 20);
            this.labelDescTitle.Name = "labelDescTitle";
            this.labelDescTitle.Size = new System.Drawing.Size(102, 13);
            this.labelDescTitle.TabIndex = 0;
            this.labelDescTitle.Text = "In menu description:";
            // 
            // checkBoxHasFloats
            // 
            this.checkBoxHasFloats.AutoSize = true;
            this.checkBoxHasFloats.Location = new System.Drawing.Point(6, 205);
            this.checkBoxHasFloats.Name = "checkBoxHasFloats";
            this.checkBoxHasFloats.Size = new System.Drawing.Size(71, 17);
            this.checkBoxHasFloats.TabIndex = 3;
            this.checkBoxHasFloats.Text = "has floats";
            this.checkBoxHasFloats.UseVisualStyleBackColor = true;
            this.checkBoxHasFloats.CheckedChanged += new System.EventHandler(this.checkBoxHasFloats_CheckedChanged);
            // 
            // MiscellaneousPanelControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxDescription);
            this.Controls.Add(this.groupBoxOptions);
            this.Name = "MiscellaneousPanelControl";
            this.Size = new System.Drawing.Size(251, 450);
            this.groupBoxOptions.ResumeLayout(false);
            this.groupBoxOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownGearDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFlapsDelay)).EndInit();
            this.groupBoxDescription.ResumeLayout(false);
            this.groupBoxDescription.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxOptions;
        private System.Windows.Forms.CheckBox checkBoxHasRetracts;
        private System.Windows.Forms.CheckBox checkBoxHasFlaps;
        private System.Windows.Forms.CheckBox checkBoxVariometer;
        private System.Windows.Forms.CheckBox checkBoxHandLaunched;
        private System.Windows.Forms.GroupBox groupBoxDescription;
        private System.Windows.Forms.Label labelDescTitle;
        private System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.CheckBox checkBoxAilerons;
        private System.Windows.Forms.Button buttonFile;
        private System.Windows.Forms.Label labelIconFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxAllowsTowing;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDownFlapsDelay;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDownGearDelay;
        private System.Windows.Forms.CheckBox checkBoxHasFloats;
    }
}
