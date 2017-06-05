namespace RCSim.AircraftEditor.Dialogs
{
    partial class CoefficientsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CoefficientsForm));
            this.panelTop = new System.Windows.Forms.Panel();
            this.buttonShiftDown = new System.Windows.Forms.Button();
            this.buttonShiftUp = new System.Windows.Forms.Button();
            this.labelHelp = new System.Windows.Forms.Label();
            this.labelDescription = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            this.panelGraph = new System.Windows.Forms.Panel();
            this.graphControl = new RCSim.AircraftEditor.Dialogs.GraphControl();
            this.buttonToggleZoom = new System.Windows.Forms.Button();
            this.panelTop.SuspendLayout();
            this.panelGraph.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.buttonShiftDown);
            this.panelTop.Controls.Add(this.buttonToggleZoom);
            this.panelTop.Controls.Add(this.buttonShiftUp);
            this.panelTop.Controls.Add(this.labelHelp);
            this.panelTop.Controls.Add(this.labelDescription);
            this.panelTop.Controls.Add(this.labelTitle);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(671, 101);
            this.panelTop.TabIndex = 1;
            // 
            // buttonShiftDown
            // 
            this.buttonShiftDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonShiftDown.Location = new System.Drawing.Point(650, 80);
            this.buttonShiftDown.Name = "buttonShiftDown";
            this.buttonShiftDown.Size = new System.Drawing.Size(21, 21);
            this.buttonShiftDown.TabIndex = 2;
            this.buttonShiftDown.Text = "-";
            this.buttonShiftDown.UseVisualStyleBackColor = true;
            this.buttonShiftDown.Click += new System.EventHandler(this.buttonShiftDown_Click);
            // 
            // buttonShiftUp
            // 
            this.buttonShiftUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonShiftUp.Location = new System.Drawing.Point(623, 80);
            this.buttonShiftUp.Name = "buttonShiftUp";
            this.buttonShiftUp.Size = new System.Drawing.Size(21, 21);
            this.buttonShiftUp.TabIndex = 2;
            this.buttonShiftUp.Text = "+";
            this.buttonShiftUp.UseVisualStyleBackColor = true;
            this.buttonShiftUp.Click += new System.EventHandler(this.buttonShiftUp_Click);
            // 
            // labelHelp
            // 
            this.labelHelp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelHelp.Location = new System.Drawing.Point(3, 80);
            this.labelHelp.Name = "labelHelp";
            this.labelHelp.Size = new System.Drawing.Size(665, 21);
            this.labelHelp.TabIndex = 1;
            this.labelHelp.Text = "Click and drag existing points or right-click to delete/add points; click \'+\'/\'-\'" +
                " buttons to shift all values up or down";
            this.labelHelp.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(6, 22);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(638, 58);
            this.labelDescription.TabIndex = 1;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.Location = new System.Drawing.Point(3, 0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(85, 18);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "Coefficent";
            // 
            // panelGraph
            // 
            this.panelGraph.Controls.Add(this.graphControl);
            this.panelGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGraph.Location = new System.Drawing.Point(0, 101);
            this.panelGraph.Name = "panelGraph";
            this.panelGraph.Size = new System.Drawing.Size(671, 311);
            this.panelGraph.TabIndex = 2;
            // 
            // graphControl
            // 
            this.graphControl.BackgroundColor = System.Drawing.Color.Black;
            this.graphControl.BackgroundValueList = null;
            this.graphControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.graphControl.GraphColor = System.Drawing.Color.Yellow;
            this.graphControl.GridColor = System.Drawing.Color.White;
            this.graphControl.Location = new System.Drawing.Point(0, 0);
            this.graphControl.MaxValue = 2.4;
            this.graphControl.Name = "graphControl";
            this.graphControl.SelectedColor = System.Drawing.Color.Red;
            this.graphControl.Size = new System.Drawing.Size(671, 311);
            this.graphControl.TabIndex = 0;
            this.graphControl.Text = "graphControl1";
            this.graphControl.ValueList = ((System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<double, double>>)(resources.GetObject("graphControl.ValueList")));
            // 
            // buttonToggleZoom
            // 
            this.buttonToggleZoom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonToggleZoom.Location = new System.Drawing.Point(623, 56);
            this.buttonToggleZoom.Name = "buttonToggleZoom";
            this.buttonToggleZoom.Size = new System.Drawing.Size(45, 21);
            this.buttonToggleZoom.TabIndex = 2;
            this.buttonToggleZoom.Text = "zoom";
            this.buttonToggleZoom.UseVisualStyleBackColor = true;
            this.buttonToggleZoom.Click += new System.EventHandler(this.buttonToggleZoom_Click);
            // 
            // CoefficientsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(671, 412);
            this.Controls.Add(this.panelGraph);
            this.Controls.Add(this.panelTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "CoefficientsForm";
            this.Text = "CoefficientsForm";
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelGraph.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private GraphControl graphControl;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Panel panelGraph;
        private System.Windows.Forms.Label labelHelp;
        private System.Windows.Forms.Button buttonShiftDown;
        private System.Windows.Forms.Button buttonShiftUp;
        private System.Windows.Forms.Button buttonToggleZoom;
    }
}