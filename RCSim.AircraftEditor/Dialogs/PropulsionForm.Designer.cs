namespace RCSim.AircraftEditor.Dialogs
{
    partial class PropulsionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PropulsionForm));
            this.graph2Control = new RCSim.AircraftEditor.Dialogs.Graph2Control();
            this.numericUpDownThrottleDelay = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownMaxThrust = new System.Windows.Forms.NumericUpDown();
            this.labelThrottleDelay = new System.Windows.Forms.Label();
            this.labelMaxThrust = new System.Windows.Forms.Label();
            this.labelNewton = new System.Windows.Forms.Label();
            this.labelMaxThrustLbs = new System.Windows.Forms.Label();
            this.groupBoxParameters = new System.Windows.Forms.GroupBox();
            this.labelHelp = new System.Windows.Forms.Label();
            this.labelGraph = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownThrottleDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxThrust)).BeginInit();
            this.groupBoxParameters.SuspendLayout();
            this.SuspendLayout();
            // 
            // graph2Control
            // 
            this.graph2Control.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.graph2Control.BackgroundColor = System.Drawing.Color.Black;
            this.graph2Control.GraphColor = System.Drawing.Color.Yellow;
            this.graph2Control.GridColor = System.Drawing.Color.White;
            this.graph2Control.Location = new System.Drawing.Point(12, 144);
            this.graph2Control.MaxValue = 120;
            this.graph2Control.Name = "graph2Control";
            this.graph2Control.SelectedColor = System.Drawing.Color.Red;
            this.graph2Control.Size = new System.Drawing.Size(485, 234);
            this.graph2Control.TabIndex = 0;
            this.graph2Control.Text = "graph2Control1";
            this.graph2Control.ValueList = ((System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<double, double>>)(resources.GetObject("graph2Control.ValueList")));
            // 
            // numericUpDownThrottleDelay
            // 
            this.numericUpDownThrottleDelay.DecimalPlaces = 1;
            this.numericUpDownThrottleDelay.Location = new System.Drawing.Point(93, 45);
            this.numericUpDownThrottleDelay.Name = "numericUpDownThrottleDelay";
            this.numericUpDownThrottleDelay.Size = new System.Drawing.Size(61, 20);
            this.numericUpDownThrottleDelay.TabIndex = 8;
            this.numericUpDownThrottleDelay.ValueChanged += new System.EventHandler(this.numericUpDownThrottleDelay_ValueChanged);
            // 
            // numericUpDownMaxThrust
            // 
            this.numericUpDownMaxThrust.DecimalPlaces = 2;
            this.numericUpDownMaxThrust.Location = new System.Drawing.Point(93, 19);
            this.numericUpDownMaxThrust.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownMaxThrust.Name = "numericUpDownMaxThrust";
            this.numericUpDownMaxThrust.Size = new System.Drawing.Size(61, 20);
            this.numericUpDownMaxThrust.TabIndex = 9;
            this.numericUpDownMaxThrust.ValueChanged += new System.EventHandler(this.numericUpDownMaxThrust_ValueChanged);
            // 
            // labelThrottleDelay
            // 
            this.labelThrottleDelay.AutoSize = true;
            this.labelThrottleDelay.Location = new System.Drawing.Point(9, 47);
            this.labelThrottleDelay.Name = "labelThrottleDelay";
            this.labelThrottleDelay.Size = new System.Drawing.Size(71, 13);
            this.labelThrottleDelay.TabIndex = 6;
            this.labelThrottleDelay.Text = "Thottle delay:";
            // 
            // labelMaxThrust
            // 
            this.labelMaxThrust.AutoSize = true;
            this.labelMaxThrust.Location = new System.Drawing.Point(9, 21);
            this.labelMaxThrust.Name = "labelMaxThrust";
            this.labelMaxThrust.Size = new System.Drawing.Size(62, 13);
            this.labelMaxThrust.TabIndex = 4;
            this.labelMaxThrust.Text = "Max. thrust:";
            // 
            // labelNewton
            // 
            this.labelNewton.AutoSize = true;
            this.labelNewton.Location = new System.Drawing.Point(160, 21);
            this.labelNewton.Name = "labelNewton";
            this.labelNewton.Size = new System.Drawing.Size(15, 13);
            this.labelNewton.TabIndex = 5;
            this.labelNewton.Text = "N";
            // 
            // labelMaxThrustLbs
            // 
            this.labelMaxThrustLbs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelMaxThrustLbs.Location = new System.Drawing.Point(180, 21);
            this.labelMaxThrustLbs.Name = "labelMaxThrustLbs";
            this.labelMaxThrustLbs.Size = new System.Drawing.Size(81, 13);
            this.labelMaxThrustLbs.TabIndex = 7;
            this.labelMaxThrustLbs.Text = "lbs";
            this.labelMaxThrustLbs.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // groupBoxParameters
            // 
            this.groupBoxParameters.Controls.Add(this.numericUpDownMaxThrust);
            this.groupBoxParameters.Controls.Add(this.numericUpDownThrottleDelay);
            this.groupBoxParameters.Controls.Add(this.labelMaxThrustLbs);
            this.groupBoxParameters.Controls.Add(this.labelNewton);
            this.groupBoxParameters.Controls.Add(this.labelThrottleDelay);
            this.groupBoxParameters.Controls.Add(this.labelMaxThrust);
            this.groupBoxParameters.Location = new System.Drawing.Point(12, 31);
            this.groupBoxParameters.Name = "groupBoxParameters";
            this.groupBoxParameters.Size = new System.Drawing.Size(267, 73);
            this.groupBoxParameters.TabIndex = 10;
            this.groupBoxParameters.TabStop = false;
            this.groupBoxParameters.Text = "Parameters";
            // 
            // labelHelp
            // 
            this.labelHelp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelHelp.Location = new System.Drawing.Point(12, 120);
            this.labelHelp.Name = "labelHelp";
            this.labelHelp.Size = new System.Drawing.Size(485, 21);
            this.labelHelp.TabIndex = 11;
            this.labelHelp.Text = "Click and drag existing points or right-click to delete/add points";
            this.labelHelp.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labelGraph
            // 
            this.labelGraph.AutoSize = true;
            this.labelGraph.Location = new System.Drawing.Point(12, 107);
            this.labelGraph.Name = "labelGraph";
            this.labelGraph.Size = new System.Drawing.Size(383, 13);
            this.labelGraph.TabIndex = 12;
            this.labelGraph.Text = "The graph below depicts the amount of thrust (%) reached at given speed (m/s).";
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.Location = new System.Drawing.Point(7, 9);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(150, 18);
            this.labelTitle.TabIndex = 13;
            this.labelTitle.Text = "Propulsion System";
            // 
            // PropulsionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 390);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.labelGraph);
            this.Controls.Add(this.labelHelp);
            this.Controls.Add(this.groupBoxParameters);
            this.Controls.Add(this.graph2Control);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "PropulsionForm";
            this.Text = "Propulsion";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownThrottleDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxThrust)).EndInit();
            this.groupBoxParameters.ResumeLayout(false);
            this.groupBoxParameters.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Graph2Control graph2Control;
        private System.Windows.Forms.NumericUpDown numericUpDownThrottleDelay;
        private System.Windows.Forms.NumericUpDown numericUpDownMaxThrust;
        private System.Windows.Forms.Label labelThrottleDelay;
        private System.Windows.Forms.Label labelMaxThrust;
        private System.Windows.Forms.Label labelNewton;
        private System.Windows.Forms.Label labelMaxThrustLbs;
        private System.Windows.Forms.GroupBox groupBoxParameters;
        private System.Windows.Forms.Label labelHelp;
        private System.Windows.Forms.Label labelGraph;
        private System.Windows.Forms.Label labelTitle;
    }
}