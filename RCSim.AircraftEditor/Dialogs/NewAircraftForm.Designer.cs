namespace RCSim.AircraftEditor.Dialogs
{
    partial class NewAircraftForm
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
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonBack = new System.Windows.Forms.Button();
            this.newVariationStep2Control = new RCSim.AircraftEditor.Dialogs.NewVariationStep2Control();
            this.newAircraftStep2Control = new RCSim.AircraftEditor.Dialogs.NewAircraftStep2Control();
            this.newAircraftStep1Control = new RCSim.AircraftEditor.Dialogs.NewAircraftStep1Control();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(387, 271);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(306, 271);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "&Next >";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonBack
            // 
            this.buttonBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBack.Enabled = false;
            this.buttonBack.Location = new System.Drawing.Point(225, 271);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(75, 23);
            this.buttonBack.TabIndex = 0;
            this.buttonBack.Text = "< &Back";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // newVariationStep2Control
            // 
            this.newVariationStep2Control.Location = new System.Drawing.Point(3, 3);
            this.newVariationStep2Control.Name = "newVariationStep2Control";
            this.newVariationStep2Control.Size = new System.Drawing.Size(466, 259);
            this.newVariationStep2Control.TabIndex = 4;
            this.newVariationStep2Control.Visible = false;
            this.newVariationStep2Control.FormValidatedChanged += new System.EventHandler(this.newVariationStep2Control_FormValidatedChanged);
            // 
            // newAircraftStep2Control
            // 
            this.newAircraftStep2Control.Location = new System.Drawing.Point(3, 3);
            this.newAircraftStep2Control.Name = "newAircraftStep2Control";
            this.newAircraftStep2Control.Size = new System.Drawing.Size(466, 259);
            this.newAircraftStep2Control.TabIndex = 3;
            this.newAircraftStep2Control.Visible = false;
            this.newAircraftStep2Control.FormValidatedChanged += new System.EventHandler(this.newAircraftStep2Control_FormValidatedChanged);
            // 
            // newAircraftStep1Control
            // 
            this.newAircraftStep1Control.Location = new System.Drawing.Point(3, 3);
            this.newAircraftStep1Control.Name = "newAircraftStep1Control";
            this.newAircraftStep1Control.Size = new System.Drawing.Size(466, 259);
            this.newAircraftStep1Control.TabIndex = 2;
            // 
            // NewAircraftForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(473, 306);
            this.ControlBox = false;
            this.Controls.Add(this.newVariationStep2Control);
            this.Controls.Add(this.newAircraftStep2Control);
            this.Controls.Add(this.newAircraftStep1Control);
            this.Controls.Add(this.buttonBack);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Name = "NewAircraftForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Create a new aircraft";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private NewAircraftStep1Control newAircraftStep1Control;
        private System.Windows.Forms.Button buttonBack;
        private NewAircraftStep2Control newAircraftStep2Control;
        private NewVariationStep2Control newVariationStep2Control;
    }
}