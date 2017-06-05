namespace RCSim
{
    partial class ToolBoxForm
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
            this.panelCursor = new System.Windows.Forms.Panel();
            this.labelPosition = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxCursorEnable = new System.Windows.Forms.CheckBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageCursor = new System.Windows.Forms.TabPage();
            this.tabPageDisplay = new System.Windows.Forms.TabPage();
            this.checkBoxGround = new System.Windows.Forms.CheckBox();
            this.tabPageObjects = new System.Windows.Forms.TabPage();
            this.labelCurrentObject = new System.Windows.Forms.Label();
            this.buttonSelectObject = new System.Windows.Forms.Button();
            this.buttonAddObject = new System.Windows.Forms.Button();
            this.buttonWindmill = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonAddSimpleTallTree = new System.Windows.Forms.Button();
            this.buttonAddSimpleTree = new System.Windows.Forms.Button();
            this.buttonAddTree = new System.Windows.Forms.Button();
            this.tabPageMaps = new System.Windows.Forms.TabPage();
            this.buttonLightMap = new System.Windows.Forms.Button();
            this.tabPageRace = new System.Windows.Forms.TabPage();
            this.labelGateRotation = new System.Windows.Forms.Label();
            this.buttonGateAdd = new System.Windows.Forms.Button();
            this.numericGateSequence = new System.Windows.Forms.NumericUpDown();
            this.numericGateRotation = new System.Windows.Forms.NumericUpDown();
            this.labelGateSequence = new System.Windows.Forms.Label();
            this.panelCursor.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPageCursor.SuspendLayout();
            this.tabPageDisplay.SuspendLayout();
            this.tabPageObjects.SuspendLayout();
            this.tabPageMaps.SuspendLayout();
            this.tabPageRace.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericGateSequence)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericGateRotation)).BeginInit();
            this.SuspendLayout();
            // 
            // panelCursor
            // 
            this.panelCursor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelCursor.Controls.Add(this.labelPosition);
            this.panelCursor.Controls.Add(this.label1);
            this.panelCursor.Controls.Add(this.checkBoxCursorEnable);
            this.panelCursor.Location = new System.Drawing.Point(3, 6);
            this.panelCursor.Name = "panelCursor";
            this.panelCursor.Size = new System.Drawing.Size(190, 120);
            this.panelCursor.TabIndex = 0;
            // 
            // labelPosition
            // 
            this.labelPosition.AutoSize = true;
            this.labelPosition.Location = new System.Drawing.Point(58, 28);
            this.labelPosition.Name = "labelPosition";
            this.labelPosition.Size = new System.Drawing.Size(0, 13);
            this.labelPosition.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Position:";
            // 
            // checkBoxCursorEnable
            // 
            this.checkBoxCursorEnable.AutoSize = true;
            this.checkBoxCursorEnable.Location = new System.Drawing.Point(4, 4);
            this.checkBoxCursorEnable.Name = "checkBoxCursorEnable";
            this.checkBoxCursorEnable.Size = new System.Drawing.Size(108, 17);
            this.checkBoxCursorEnable.TabIndex = 0;
            this.checkBoxCursorEnable.Text = "Enable 3D cursor";
            this.checkBoxCursorEnable.UseVisualStyleBackColor = true;
            this.checkBoxCursorEnable.CheckedChanged += new System.EventHandler(this.checkBoxCursorEnable_CheckedChanged);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageCursor);
            this.tabControl.Controls.Add(this.tabPageDisplay);
            this.tabControl.Controls.Add(this.tabPageObjects);
            this.tabControl.Controls.Add(this.tabPageMaps);
            this.tabControl.Controls.Add(this.tabPageRace);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(207, 396);
            this.tabControl.TabIndex = 1;
            // 
            // tabPageCursor
            // 
            this.tabPageCursor.Controls.Add(this.panelCursor);
            this.tabPageCursor.Location = new System.Drawing.Point(4, 22);
            this.tabPageCursor.Name = "tabPageCursor";
            this.tabPageCursor.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCursor.Size = new System.Drawing.Size(199, 370);
            this.tabPageCursor.TabIndex = 0;
            this.tabPageCursor.Text = "Cursor";
            this.tabPageCursor.UseVisualStyleBackColor = true;
            // 
            // tabPageDisplay
            // 
            this.tabPageDisplay.Controls.Add(this.checkBoxGround);
            this.tabPageDisplay.Location = new System.Drawing.Point(4, 22);
            this.tabPageDisplay.Name = "tabPageDisplay";
            this.tabPageDisplay.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDisplay.Size = new System.Drawing.Size(199, 370);
            this.tabPageDisplay.TabIndex = 1;
            this.tabPageDisplay.Text = "Display";
            this.tabPageDisplay.UseVisualStyleBackColor = true;
            // 
            // checkBoxGround
            // 
            this.checkBoxGround.AutoSize = true;
            this.checkBoxGround.Checked = true;
            this.checkBoxGround.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxGround.Location = new System.Drawing.Point(4, 7);
            this.checkBoxGround.Name = "checkBoxGround";
            this.checkBoxGround.Size = new System.Drawing.Size(89, 17);
            this.checkBoxGround.TabIndex = 0;
            this.checkBoxGround.Text = "Show ground";
            this.checkBoxGround.UseVisualStyleBackColor = true;
            this.checkBoxGround.CheckedChanged += new System.EventHandler(this.checkBoxGround_CheckedChanged);
            // 
            // tabPageObjects
            // 
            this.tabPageObjects.Controls.Add(this.labelCurrentObject);
            this.tabPageObjects.Controls.Add(this.buttonSelectObject);
            this.tabPageObjects.Controls.Add(this.buttonAddObject);
            this.tabPageObjects.Controls.Add(this.buttonWindmill);
            this.tabPageObjects.Controls.Add(this.button1);
            this.tabPageObjects.Controls.Add(this.buttonAddSimpleTallTree);
            this.tabPageObjects.Controls.Add(this.buttonAddSimpleTree);
            this.tabPageObjects.Controls.Add(this.buttonAddTree);
            this.tabPageObjects.Location = new System.Drawing.Point(4, 22);
            this.tabPageObjects.Name = "tabPageObjects";
            this.tabPageObjects.Size = new System.Drawing.Size(199, 370);
            this.tabPageObjects.TabIndex = 2;
            this.tabPageObjects.Text = "Objects";
            this.tabPageObjects.UseVisualStyleBackColor = true;
            // 
            // labelCurrentObject
            // 
            this.labelCurrentObject.AutoSize = true;
            this.labelCurrentObject.Location = new System.Drawing.Point(8, 137);
            this.labelCurrentObject.Name = "labelCurrentObject";
            this.labelCurrentObject.Size = new System.Drawing.Size(96, 13);
            this.labelCurrentObject.TabIndex = 1;
            this.labelCurrentObject.Text = "No object selected";
            // 
            // buttonSelectObject
            // 
            this.buttonSelectObject.Location = new System.Drawing.Point(89, 111);
            this.buttonSelectObject.Name = "buttonSelectObject";
            this.buttonSelectObject.Size = new System.Drawing.Size(75, 23);
            this.buttonSelectObject.TabIndex = 0;
            this.buttonSelectObject.Text = "Select...";
            this.buttonSelectObject.UseVisualStyleBackColor = true;
            this.buttonSelectObject.Click += new System.EventHandler(this.buttonSelectObject_Click);
            // 
            // buttonAddObject
            // 
            this.buttonAddObject.Location = new System.Drawing.Point(11, 111);
            this.buttonAddObject.Name = "buttonAddObject";
            this.buttonAddObject.Size = new System.Drawing.Size(75, 23);
            this.buttonAddObject.TabIndex = 0;
            this.buttonAddObject.Text = "Object";
            this.buttonAddObject.UseVisualStyleBackColor = true;
            this.buttonAddObject.Click += new System.EventHandler(this.buttonAddObject_Click);
            // 
            // buttonWindmill
            // 
            this.buttonWindmill.Location = new System.Drawing.Point(8, 165);
            this.buttonWindmill.Name = "buttonWindmill";
            this.buttonWindmill.Size = new System.Drawing.Size(75, 23);
            this.buttonWindmill.TabIndex = 0;
            this.buttonWindmill.Text = "Windmill";
            this.buttonWindmill.UseVisualStyleBackColor = true;
            this.buttonWindmill.Click += new System.EventHandler(this.buttonWindmill_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(89, 32);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Small Tree";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.buttonAddSimpleSmallTree_Click);
            // 
            // buttonAddSimpleTallTree
            // 
            this.buttonAddSimpleTallTree.Location = new System.Drawing.Point(8, 32);
            this.buttonAddSimpleTallTree.Name = "buttonAddSimpleTallTree";
            this.buttonAddSimpleTallTree.Size = new System.Drawing.Size(75, 23);
            this.buttonAddSimpleTallTree.TabIndex = 0;
            this.buttonAddSimpleTallTree.Text = "S. Tall Tree";
            this.buttonAddSimpleTallTree.UseVisualStyleBackColor = true;
            this.buttonAddSimpleTallTree.Click += new System.EventHandler(this.buttonAddSimpleTallTree_Click);
            // 
            // buttonAddSimpleTree
            // 
            this.buttonAddSimpleTree.Location = new System.Drawing.Point(89, 3);
            this.buttonAddSimpleTree.Name = "buttonAddSimpleTree";
            this.buttonAddSimpleTree.Size = new System.Drawing.Size(75, 23);
            this.buttonAddSimpleTree.TabIndex = 0;
            this.buttonAddSimpleTree.Text = "Simple Tree";
            this.buttonAddSimpleTree.UseVisualStyleBackColor = true;
            this.buttonAddSimpleTree.Click += new System.EventHandler(this.buttonAddSimpleTree_Click);
            // 
            // buttonAddTree
            // 
            this.buttonAddTree.Location = new System.Drawing.Point(8, 3);
            this.buttonAddTree.Name = "buttonAddTree";
            this.buttonAddTree.Size = new System.Drawing.Size(75, 23);
            this.buttonAddTree.TabIndex = 0;
            this.buttonAddTree.Text = "Tree";
            this.buttonAddTree.UseVisualStyleBackColor = true;
            this.buttonAddTree.Click += new System.EventHandler(this.buttonAddTree_Click);
            // 
            // tabPageMaps
            // 
            this.tabPageMaps.Controls.Add(this.buttonLightMap);
            this.tabPageMaps.Location = new System.Drawing.Point(4, 22);
            this.tabPageMaps.Name = "tabPageMaps";
            this.tabPageMaps.Size = new System.Drawing.Size(199, 370);
            this.tabPageMaps.TabIndex = 3;
            this.tabPageMaps.Text = "Maps";
            this.tabPageMaps.UseVisualStyleBackColor = true;
            // 
            // buttonLightMap
            // 
            this.buttonLightMap.Location = new System.Drawing.Point(9, 4);
            this.buttonLightMap.Name = "buttonLightMap";
            this.buttonLightMap.Size = new System.Drawing.Size(182, 23);
            this.buttonLightMap.TabIndex = 0;
            this.buttonLightMap.Text = "Generate Lightmap";
            this.buttonLightMap.UseVisualStyleBackColor = true;
            this.buttonLightMap.Click += new System.EventHandler(this.buttonLightMap_Click);
            // 
            // tabPageRace
            // 
            this.tabPageRace.Controls.Add(this.labelGateSequence);
            this.tabPageRace.Controls.Add(this.labelGateRotation);
            this.tabPageRace.Controls.Add(this.buttonGateAdd);
            this.tabPageRace.Controls.Add(this.numericGateSequence);
            this.tabPageRace.Controls.Add(this.numericGateRotation);
            this.tabPageRace.Location = new System.Drawing.Point(4, 22);
            this.tabPageRace.Name = "tabPageRace";
            this.tabPageRace.Size = new System.Drawing.Size(199, 370);
            this.tabPageRace.TabIndex = 4;
            this.tabPageRace.Text = "Race";
            this.tabPageRace.UseVisualStyleBackColor = true;
            // 
            // labelGateRotation
            // 
            this.labelGateRotation.AutoSize = true;
            this.labelGateRotation.Location = new System.Drawing.Point(9, 9);
            this.labelGateRotation.Name = "labelGateRotation";
            this.labelGateRotation.Size = new System.Drawing.Size(50, 13);
            this.labelGateRotation.TabIndex = 2;
            this.labelGateRotation.Text = "Rotation:";
            // 
            // buttonGateAdd
            // 
            this.buttonGateAdd.Location = new System.Drawing.Point(116, 70);
            this.buttonGateAdd.Name = "buttonGateAdd";
            this.buttonGateAdd.Size = new System.Drawing.Size(75, 23);
            this.buttonGateAdd.TabIndex = 1;
            this.buttonGateAdd.Text = "Add gate";
            this.buttonGateAdd.UseVisualStyleBackColor = true;
            this.buttonGateAdd.Click += new System.EventHandler(this.buttonGateAdd_Click);
            // 
            // numericGateSequence
            // 
            this.numericGateSequence.Location = new System.Drawing.Point(142, 33);
            this.numericGateSequence.Name = "numericGateSequence";
            this.numericGateSequence.Size = new System.Drawing.Size(49, 20);
            this.numericGateSequence.TabIndex = 0;
            // 
            // numericGateRotation
            // 
            this.numericGateRotation.Increment = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numericGateRotation.Location = new System.Drawing.Point(142, 7);
            this.numericGateRotation.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.numericGateRotation.Name = "numericGateRotation";
            this.numericGateRotation.Size = new System.Drawing.Size(49, 20);
            this.numericGateRotation.TabIndex = 0;
            // 
            // labelGateSequence
            // 
            this.labelGateSequence.AutoSize = true;
            this.labelGateSequence.Location = new System.Drawing.Point(9, 35);
            this.labelGateSequence.Name = "labelGateSequence";
            this.labelGateSequence.Size = new System.Drawing.Size(73, 13);
            this.labelGateSequence.TabIndex = 2;
            this.labelGateSequence.Text = "Sequence Nr:";
            // 
            // ToolBoxForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(207, 396);
            this.Controls.Add(this.tabControl);
            this.Name = "ToolBoxForm";
            this.Text = "ToolBoxForm";
            this.panelCursor.ResumeLayout(false);
            this.panelCursor.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabPageCursor.ResumeLayout(false);
            this.tabPageDisplay.ResumeLayout(false);
            this.tabPageDisplay.PerformLayout();
            this.tabPageObjects.ResumeLayout(false);
            this.tabPageObjects.PerformLayout();
            this.tabPageMaps.ResumeLayout(false);
            this.tabPageRace.ResumeLayout(false);
            this.tabPageRace.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericGateSequence)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericGateRotation)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelCursor;
        private System.Windows.Forms.CheckBox checkBoxCursorEnable;
        private System.Windows.Forms.Label labelPosition;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageCursor;
        private System.Windows.Forms.TabPage tabPageDisplay;
        private System.Windows.Forms.CheckBox checkBoxGround;
        private System.Windows.Forms.TabPage tabPageObjects;
        private System.Windows.Forms.Button buttonAddTree;
        private System.Windows.Forms.Label labelCurrentObject;
        private System.Windows.Forms.Button buttonAddObject;
        private System.Windows.Forms.Button buttonSelectObject;
        private System.Windows.Forms.Button buttonWindmill;
        private System.Windows.Forms.TabPage tabPageMaps;
        private System.Windows.Forms.Button buttonLightMap;
        private System.Windows.Forms.Button buttonAddSimpleTree;
        private System.Windows.Forms.Button buttonAddSimpleTallTree;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabPage tabPageRace;
        private System.Windows.Forms.Label labelGateRotation;
        private System.Windows.Forms.Button buttonGateAdd;
        private System.Windows.Forms.NumericUpDown numericGateRotation;
        private System.Windows.Forms.NumericUpDown numericGateSequence;
        private System.Windows.Forms.Label labelGateSequence;
    }
}