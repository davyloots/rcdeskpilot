namespace RCSim.AircraftEditor.Dialogs
{
    partial class ToolboxForm
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
            this.components = new System.ComponentModel.Container();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageGraphics = new System.Windows.Forms.TabPage();
            this.groupBoxHierarchy = new System.Windows.Forms.GroupBox();
            this.TreeViewHierarchy = new System.Windows.Forms.TreeView();
            this.contextMenuStripHierarchy = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addChildItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPagePhysics = new System.Windows.Forms.TabPage();
            this.checkBoxFlying = new System.Windows.Forms.CheckBox();
            this.tabPageSound = new System.Windows.Forms.TabPage();
            this.tabPageCollision = new System.Windows.Forms.TabPage();
            this.tabPageMiscellaneous = new System.Windows.Forms.TabPage();
            this.surfacePropertiesControl = new RCSim.AircraftEditor.Dialogs.SurfacePropertiesControl();
            this.aircraftPanelControl = new RCSim.AircraftEditor.Dialogs.AircraftPanelControl();
            this.soundControl = new RCSim.AircraftEditor.Dialogs.SoundControl();
            this.collisionControl = new RCSim.AircraftEditor.Dialogs.CollisionControl();
            this.miscellaneousPanelControl = new RCSim.AircraftEditor.Dialogs.MiscellaneousPanelControl();
            this.tabControl.SuspendLayout();
            this.tabPageGraphics.SuspendLayout();
            this.groupBoxHierarchy.SuspendLayout();
            this.contextMenuStripHierarchy.SuspendLayout();
            this.tabPagePhysics.SuspendLayout();
            this.tabPageSound.SuspendLayout();
            this.tabPageCollision.SuspendLayout();
            this.tabPageMiscellaneous.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageGraphics);
            this.tabControl.Controls.Add(this.tabPagePhysics);
            this.tabControl.Controls.Add(this.tabPageSound);
            this.tabControl.Controls.Add(this.tabPageCollision);
            this.tabControl.Controls.Add(this.tabPageMiscellaneous);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(271, 516);
            this.tabControl.TabIndex = 0;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // tabPageGraphics
            // 
            this.tabPageGraphics.AutoScroll = true;
            this.tabPageGraphics.Controls.Add(this.groupBoxHierarchy);
            this.tabPageGraphics.Controls.Add(this.surfacePropertiesControl);
            this.tabPageGraphics.Location = new System.Drawing.Point(4, 22);
            this.tabPageGraphics.Name = "tabPageGraphics";
            this.tabPageGraphics.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGraphics.Size = new System.Drawing.Size(263, 490);
            this.tabPageGraphics.TabIndex = 0;
            this.tabPageGraphics.Text = "Graphics";
            this.tabPageGraphics.UseVisualStyleBackColor = true;
            // 
            // groupBoxHierarchy
            // 
            this.groupBoxHierarchy.Controls.Add(this.TreeViewHierarchy);
            this.groupBoxHierarchy.Location = new System.Drawing.Point(3, 6);
            this.groupBoxHierarchy.Name = "groupBoxHierarchy";
            this.groupBoxHierarchy.Size = new System.Drawing.Size(256, 198);
            this.groupBoxHierarchy.TabIndex = 3;
            this.groupBoxHierarchy.TabStop = false;
            this.groupBoxHierarchy.Text = "Hierarchy";
            // 
            // TreeViewHierarchy
            // 
            this.TreeViewHierarchy.ContextMenuStrip = this.contextMenuStripHierarchy;
            this.TreeViewHierarchy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TreeViewHierarchy.Location = new System.Drawing.Point(3, 16);
            this.TreeViewHierarchy.Name = "TreeViewHierarchy";
            this.TreeViewHierarchy.Size = new System.Drawing.Size(250, 179);
            this.TreeViewHierarchy.TabIndex = 0;
            this.TreeViewHierarchy.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TreeViewHierarchy_NodeMouseClick);
            // 
            // contextMenuStripHierarchy
            // 
            this.contextMenuStripHierarchy.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addChildItemToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.contextMenuStripHierarchy.Name = "contextMenuStripHierarchy";
            this.contextMenuStripHierarchy.Size = new System.Drawing.Size(153, 48);
            // 
            // addChildItemToolStripMenuItem
            // 
            this.addChildItemToolStripMenuItem.Name = "addChildItemToolStripMenuItem";
            this.addChildItemToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.addChildItemToolStripMenuItem.Text = "Add child item";
            this.addChildItemToolStripMenuItem.Click += new System.EventHandler(this.addChildItemToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // tabPagePhysics
            // 
            this.tabPagePhysics.Controls.Add(this.aircraftPanelControl);
            this.tabPagePhysics.Controls.Add(this.checkBoxFlying);
            this.tabPagePhysics.Location = new System.Drawing.Point(4, 22);
            this.tabPagePhysics.Name = "tabPagePhysics";
            this.tabPagePhysics.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePhysics.Size = new System.Drawing.Size(263, 490);
            this.tabPagePhysics.TabIndex = 1;
            this.tabPagePhysics.Text = "Physics";
            this.tabPagePhysics.UseVisualStyleBackColor = true;
            // 
            // checkBoxFlying
            // 
            this.checkBoxFlying.AutoSize = true;
            this.checkBoxFlying.Location = new System.Drawing.Point(9, 288);
            this.checkBoxFlying.Name = "checkBoxFlying";
            this.checkBoxFlying.Size = new System.Drawing.Size(108, 17);
            this.checkBoxFlying.TabIndex = 1;
            this.checkBoxFlying.Text = "Enable test mode";
            this.checkBoxFlying.UseVisualStyleBackColor = true;
            this.checkBoxFlying.CheckedChanged += new System.EventHandler(this.checkBoxFlying_CheckedChanged);
            // 
            // tabPageSound
            // 
            this.tabPageSound.Controls.Add(this.soundControl);
            this.tabPageSound.Location = new System.Drawing.Point(4, 22);
            this.tabPageSound.Name = "tabPageSound";
            this.tabPageSound.Size = new System.Drawing.Size(263, 490);
            this.tabPageSound.TabIndex = 2;
            this.tabPageSound.Text = "Sound";
            this.tabPageSound.UseVisualStyleBackColor = true;
            // 
            // tabPageCollision
            // 
            this.tabPageCollision.Controls.Add(this.collisionControl);
            this.tabPageCollision.Location = new System.Drawing.Point(4, 22);
            this.tabPageCollision.Name = "tabPageCollision";
            this.tabPageCollision.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCollision.Size = new System.Drawing.Size(263, 490);
            this.tabPageCollision.TabIndex = 3;
            this.tabPageCollision.Text = "Collision";
            this.tabPageCollision.UseVisualStyleBackColor = true;
            // 
            // tabPageMiscellaneous
            // 
            this.tabPageMiscellaneous.Controls.Add(this.miscellaneousPanelControl);
            this.tabPageMiscellaneous.Location = new System.Drawing.Point(4, 22);
            this.tabPageMiscellaneous.Name = "tabPageMiscellaneous";
            this.tabPageMiscellaneous.Size = new System.Drawing.Size(263, 490);
            this.tabPageMiscellaneous.TabIndex = 4;
            this.tabPageMiscellaneous.Text = "Misc";
            this.tabPageMiscellaneous.UseVisualStyleBackColor = true;
            // 
            // surfacePropertiesControl
            // 
            this.surfacePropertiesControl.Location = new System.Drawing.Point(3, 210);
            this.surfacePropertiesControl.Name = "surfacePropertiesControl";
            this.surfacePropertiesControl.Size = new System.Drawing.Size(256, 236);
            this.surfacePropertiesControl.TabIndex = 2;
            this.surfacePropertiesControl.FileChanged += new System.EventHandler(this.surfacePropertiesControl_FileChanged);
            // 
            // aircraftPanelControl
            // 
            this.aircraftPanelControl.Location = new System.Drawing.Point(3, 6);
            this.aircraftPanelControl.ModelControl = null;
            this.aircraftPanelControl.Name = "aircraftPanelControl";
            this.aircraftPanelControl.Size = new System.Drawing.Size(251, 481);
            this.aircraftPanelControl.TabIndex = 3;
            this.aircraftPanelControl.TestFlyChanged += new System.EventHandler(this.aircraftPanelControl_TestFlyChanged);
            // 
            // soundControl
            // 
            this.soundControl.EngineMaxFrequency = 22100;
            this.soundControl.EngineMinFrequency = 22100;
            this.soundControl.EngineSoundFile = "";
            this.soundControl.Location = new System.Drawing.Point(3, 3);
            this.soundControl.ModelControl = null;
            this.soundControl.Name = "soundControl";
            this.soundControl.Size = new System.Drawing.Size(256, 115);
            this.soundControl.TabIndex = 0;
            // 
            // collisionControl
            // 
            this.collisionControl.Location = new System.Drawing.Point(4, 7);
            this.collisionControl.ModelControl = null;
            this.collisionControl.Name = "collisionControl";
            this.collisionControl.Size = new System.Drawing.Size(256, 477);
            this.collisionControl.TabIndex = 0;
            // 
            // miscellaneousPanelControl
            // 
            this.miscellaneousPanelControl.Location = new System.Drawing.Point(4, 3);
            this.miscellaneousPanelControl.ModelControl = null;
            this.miscellaneousPanelControl.Name = "miscellaneousPanelControl";
            this.miscellaneousPanelControl.Size = new System.Drawing.Size(251, 450);
            this.miscellaneousPanelControl.TabIndex = 0;
            // 
            // ToolboxForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(271, 516);
            this.ControlBox = false;
            this.Controls.Add(this.tabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ToolboxForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Toolbox";
            this.tabControl.ResumeLayout(false);
            this.tabPageGraphics.ResumeLayout(false);
            this.groupBoxHierarchy.ResumeLayout(false);
            this.contextMenuStripHierarchy.ResumeLayout(false);
            this.tabPagePhysics.ResumeLayout(false);
            this.tabPagePhysics.PerformLayout();
            this.tabPageSound.ResumeLayout(false);
            this.tabPageCollision.ResumeLayout(false);
            this.tabPageMiscellaneous.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageGraphics;
        private System.Windows.Forms.TabPage tabPagePhysics;
        public System.Windows.Forms.TreeView TreeViewHierarchy;
        private SurfacePropertiesControl surfacePropertiesControl;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripHierarchy;
        private System.Windows.Forms.ToolStripMenuItem addChildItemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.CheckBox checkBoxFlying;
        private AircraftPanelControl aircraftPanelControl;
        private System.Windows.Forms.TabPage tabPageSound;
        private SoundControl soundControl;
        private System.Windows.Forms.TabPage tabPageCollision;
        private CollisionControl collisionControl;
        private System.Windows.Forms.GroupBox groupBoxHierarchy;
        private System.Windows.Forms.TabPage tabPageMiscellaneous;
        private MiscellaneousPanelControl miscellaneousPanelControl;
    }
}