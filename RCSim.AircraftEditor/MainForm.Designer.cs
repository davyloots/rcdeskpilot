namespace RCSim.AircraftEditor
{
    partial class MainForm
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
            this.panel3D = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // panel3D
            // 
            this.panel3D.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3D.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panel3D.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3D.Location = new System.Drawing.Point(3, 13);
            this.panel3D.Name = "panel3D";
            this.panel3D.Size = new System.Drawing.Size(567, 477);
            this.panel3D.TabIndex = 0;
            this.panel3D.TabStop = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(593, 48);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 89);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 497);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel3D);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel3D;
        private System.Windows.Forms.Button button1;
    }
}