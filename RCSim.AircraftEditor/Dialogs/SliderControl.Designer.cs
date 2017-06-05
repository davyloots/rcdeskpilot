namespace RCSim.AircraftEditor.Dialogs
{
    partial class SliderControl
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
            this.trackBar = new System.Windows.Forms.TrackBar();
            this.labelParameter = new System.Windows.Forms.Label();
            this.labelValue = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // trackBar
            // 
            this.trackBar.AutoSize = false;
            this.trackBar.Location = new System.Drawing.Point(96, 0);
            this.trackBar.Maximum = 100;
            this.trackBar.Name = "trackBar";
            this.trackBar.Size = new System.Drawing.Size(196, 20);
            this.trackBar.TabIndex = 0;
            this.trackBar.TickFrequency = 10;
            this.trackBar.Scroll += new System.EventHandler(this.trackBar_Scroll);
            // 
            // labelParameter
            // 
            this.labelParameter.AutoSize = true;
            this.labelParameter.Location = new System.Drawing.Point(3, 0);
            this.labelParameter.Name = "labelParameter";
            this.labelParameter.Size = new System.Drawing.Size(55, 13);
            this.labelParameter.TabIndex = 1;
            this.labelParameter.Text = "Parameter";
            // 
            // labelValue
            // 
            this.labelValue.AutoSize = true;
            this.labelValue.Location = new System.Drawing.Point(303, 0);
            this.labelValue.Name = "labelValue";
            this.labelValue.Size = new System.Drawing.Size(22, 13);
            this.labelValue.TabIndex = 1;
            this.labelValue.Text = "0.0";
            // 
            // SliderControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelValue);
            this.Controls.Add(this.labelParameter);
            this.Controls.Add(this.trackBar);
            this.Name = "SliderControl";
            this.Size = new System.Drawing.Size(356, 20);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar trackBar;
        private System.Windows.Forms.Label labelParameter;
        private System.Windows.Forms.Label labelValue;
    }
}
