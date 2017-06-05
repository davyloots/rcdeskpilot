namespace RCSim.AircraftEditor.Dialogs
{
    partial class NewVariationStep2Control
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
            this.labelNameHint = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.buttonSelect = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.labelSelected = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelSelected);
            this.groupBox1.Controls.Add(this.buttonSelect);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.labelNameHint);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBoxName);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(466, 259);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Step 2 of 2";
            // 
            // labelNameHint
            // 
            this.labelNameHint.Location = new System.Drawing.Point(129, 38);
            this.labelNameHint.Name = "labelNameHint";
            this.labelNameHint.Size = new System.Drawing.Size(315, 43);
            this.labelNameHint.TabIndex = 16;
            this.labelNameHint.Text = "This will be the name of your aircraft as it appears in the menu. This cannot be " +
                "changed!";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 15);
            this.label1.TabIndex = 15;
            this.label1.Text = "Aircraft name:";
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(132, 15);
            this.textBoxName.MaxLength = 32;
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(188, 20);
            this.textBoxName.TabIndex = 14;
            this.textBoxName.Text = "New aircraft";
            this.textBoxName.TextChanged += new System.EventHandler(this.textBoxName_TextChanged);
            this.textBoxName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxName_KeyPress);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Aircraft files|*.par";
            this.openFileDialog.InitialDirectory = "Aircraft/";
            // 
            // buttonSelect
            // 
            this.buttonSelect.Location = new System.Drawing.Point(367, 75);
            this.buttonSelect.Name = "buttonSelect";
            this.buttonSelect.Size = new System.Drawing.Size(93, 23);
            this.buttonSelect.TabIndex = 17;
            this.buttonSelect.Text = "Select base...";
            this.buttonSelect.UseVisualStyleBackColor = true;
            this.buttonSelect.Click += new System.EventHandler(this.buttonSelect_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 15);
            this.label2.TabIndex = 15;
            this.label2.Text = "Base aircraft:";
            // 
            // labelSelected
            // 
            this.labelSelected.AutoSize = true;
            this.labelSelected.Location = new System.Drawing.Point(129, 80);
            this.labelSelected.Name = "labelSelected";
            this.labelSelected.Size = new System.Drawing.Size(97, 13);
            this.labelSelected.TabIndex = 18;
            this.labelSelected.Text = "no aircraft selected";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(129, 101);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(315, 43);
            this.label3.TabIndex = 16;
            this.label3.Text = "Click the button above to select the aircraft for which you will be creating a va" +
                "riation. Your version will be created in the same folder.";
            // 
            // NewVariationStep2Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "NewVariationStep2Control";
            this.Size = new System.Drawing.Size(466, 259);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labelNameHint;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label labelSelected;
        private System.Windows.Forms.Button buttonSelect;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
    }
}
