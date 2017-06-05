namespace RCSim.AircraftEditor.Dialogs
{
    partial class NewAircraftStep1Control
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
            this.radioButtonNew = new System.Windows.Forms.RadioButton();
            this.radioButtonVariation = new System.Windows.Forms.RadioButton();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.labelTitle = new System.Windows.Forms.Label();
            this.labelDescription = new System.Windows.Forms.Label();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioButtonNew
            // 
            this.radioButtonNew.AutoSize = true;
            this.radioButtonNew.Checked = true;
            this.radioButtonNew.Location = new System.Drawing.Point(9, 44);
            this.radioButtonNew.Name = "radioButtonNew";
            this.radioButtonNew.Size = new System.Drawing.Size(82, 17);
            this.radioButtonNew.TabIndex = 0;
            this.radioButtonNew.TabStop = true;
            this.radioButtonNew.Text = "New aircraft";
            this.radioButtonNew.UseVisualStyleBackColor = true;
            // 
            // radioButtonVariation
            // 
            this.radioButtonVariation.AutoSize = true;
            this.radioButtonVariation.Location = new System.Drawing.Point(9, 68);
            this.radioButtonVariation.Name = "radioButtonVariation";
            this.radioButtonVariation.Size = new System.Drawing.Size(166, 17);
            this.radioButtonVariation.TabIndex = 1;
            this.radioButtonVariation.Text = "Variation of an existing aircraft";
            this.radioButtonVariation.UseVisualStyleBackColor = true;
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.labelDescription);
            this.groupBox.Controls.Add(this.labelTitle);
            this.groupBox.Controls.Add(this.radioButtonNew);
            this.groupBox.Controls.Add(this.radioButtonVariation);
            this.groupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox.Location = new System.Drawing.Point(0, 0);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(466, 259);
            this.groupBox.TabIndex = 2;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Step 1 of 2";
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(6, 20);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(179, 15);
            this.labelTitle.TabIndex = 2;
            this.labelTitle.Text = "What do you want to build?";
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(6, 101);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(306, 56);
            this.labelDescription.TabIndex = 2;
            this.labelDescription.Text = "When creating a variation, you\'ll start with an existing aircraft. You can then m" +
                "odify this. If you plan to make an aircraft from scratch, it is advised to selec" +
                "t the \'New aircraft\' option.";
            // 
            // NewAircraftStep1Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox);
            this.Name = "NewAircraftStep1Control";
            this.Size = new System.Drawing.Size(466, 259);
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButtonNew;
        private System.Windows.Forms.RadioButton radioButtonVariation;
        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelDescription;
    }
}
