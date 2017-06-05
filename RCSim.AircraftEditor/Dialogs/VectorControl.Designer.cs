namespace RCSim.AircraftEditor.Dialogs
{
    partial class VectorControl
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
            this.buttonY = new System.Windows.Forms.Button();
            this.buttonXZ = new System.Windows.Forms.Button();
            this.labelVector = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonY
            // 
            this.buttonY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonY.Location = new System.Drawing.Point(152, 0);
            this.buttonY.Name = "buttonY";
            this.buttonY.Size = new System.Drawing.Size(28, 28);
            this.buttonY.TabIndex = 0;
            this.buttonY.UseVisualStyleBackColor = true;
            this.buttonY.MouseMove += new System.Windows.Forms.MouseEventHandler(this.buttonY_MouseMove);
            this.buttonY.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonY_MouseDown);
            this.buttonY.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonY_MouseUp);
            // 
            // buttonXZ
            // 
            this.buttonXZ.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonXZ.Location = new System.Drawing.Point(118, 0);
            this.buttonXZ.Name = "buttonXZ";
            this.buttonXZ.Size = new System.Drawing.Size(28, 28);
            this.buttonXZ.TabIndex = 0;
            this.buttonXZ.UseVisualStyleBackColor = true;
            this.buttonXZ.MouseMove += new System.Windows.Forms.MouseEventHandler(this.buttonXZ_MouseMove);
            this.buttonXZ.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonXZ_MouseDown);
            this.buttonXZ.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonXZ_MouseUp);
            // 
            // labelVector
            // 
            this.labelVector.Location = new System.Drawing.Point(3, 8);
            this.labelVector.Name = "labelVector";
            this.labelVector.Size = new System.Drawing.Size(114, 18);
            this.labelVector.TabIndex = 1;
            this.labelVector.Text = "(0;0;0)";
            // 
            // VectorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelVector);
            this.Controls.Add(this.buttonY);
            this.Controls.Add(this.buttonXZ);
            this.Name = "VectorControl";
            this.Size = new System.Drawing.Size(183, 29);
            this.Load += new System.EventHandler(this.VectorControl_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonXZ;
        private System.Windows.Forms.Button buttonY;
        private System.Windows.Forms.Label labelVector;
    }
}
