namespace RCSim.AircraftEditor.Dialogs
{
    partial class NewAircraftStep2Control
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.labelNameHint = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxFlightModelType = new System.Windows.Forms.ComboBox();
            this.textBoxFolder = new System.Windows.Forms.TextBox();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.labelNameHint);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.comboBoxFlightModelType);
            this.groupBox1.Controls.Add(this.textBoxFolder);
            this.groupBox1.Controls.Add(this.textBoxName);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(466, 259);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Step 2 of 2";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(129, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(315, 21);
            this.label3.TabIndex = 11;
            this.label3.Text = "For now, only airplanes are supported.";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(129, 155);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(315, 43);
            this.label5.TabIndex = 12;
            this.label5.Text = "The editor will create a subfolder with this name in the \'Aircraft\' folder of you" +
                "r sim.";
            // 
            // labelNameHint
            // 
            this.labelNameHint.Location = new System.Drawing.Point(129, 38);
            this.labelNameHint.Name = "labelNameHint";
            this.labelNameHint.Size = new System.Drawing.Size(315, 43);
            this.labelNameHint.TabIndex = 13;
            this.labelNameHint.Text = "This will be the name of your aircraft as it appears in the menu. This cannot be " +
                "changed!";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 15);
            this.label2.TabIndex = 10;
            this.label2.Text = "Aircraft type:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(6, 133);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 15);
            this.label4.TabIndex = 9;
            this.label4.Text = "Aircraft folder:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 15);
            this.label1.TabIndex = 8;
            this.label1.Text = "Aircraft name:";
            // 
            // comboBoxFlightModelType
            // 
            this.comboBoxFlightModelType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFlightModelType.FormattingEnabled = true;
            this.comboBoxFlightModelType.Items.AddRange(new object[] {
            "Aircraft"});
            this.comboBoxFlightModelType.Location = new System.Drawing.Point(132, 84);
            this.comboBoxFlightModelType.Name = "comboBoxFlightModelType";
            this.comboBoxFlightModelType.Size = new System.Drawing.Size(121, 21);
            this.comboBoxFlightModelType.TabIndex = 7;
            // 
            // textBoxFolder
            // 
            this.textBoxFolder.Location = new System.Drawing.Point(132, 132);
            this.textBoxFolder.MaxLength = 32;
            this.textBoxFolder.Name = "textBoxFolder";
            this.textBoxFolder.Size = new System.Drawing.Size(188, 20);
            this.textBoxFolder.TabIndex = 8;
            this.textBoxFolder.Text = "New aircraft";
            this.textBoxFolder.TextChanged += new System.EventHandler(this.textBoxName_TextChanged);
            this.textBoxFolder.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxFolder_KeyPress);
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(132, 15);
            this.textBoxName.MaxLength = 32;
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(188, 20);
            this.textBoxName.TabIndex = 6;
            this.textBoxName.Text = "New aircraft";
            this.textBoxName.TextChanged += new System.EventHandler(this.textBoxName_TextChanged);
            this.textBoxName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxName_KeyPress);
            // 
            // NewAircraftStep2Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "NewAircraftStep2Control";
            this.Size = new System.Drawing.Size(466, 259);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelNameHint;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxFlightModelType;
        private System.Windows.Forms.TextBox textBoxFolder;
        private System.Windows.Forms.TextBox textBoxName;
    }
}
