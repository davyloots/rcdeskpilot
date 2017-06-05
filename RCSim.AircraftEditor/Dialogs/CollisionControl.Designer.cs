namespace RCSim.AircraftEditor.Dialogs
{
    partial class CollisionControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CollisionControl));
            this.groupBoxCollision = new System.Windows.Forms.GroupBox();
            this.labelCurrentPoint = new System.Windows.Forms.Label();
            this.vectorControlCollisionPoint = new RCSim.AircraftEditor.Dialogs.VectorControl();
            this.buttonClone = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.listBoxPoints = new System.Windows.Forms.ListBox();
            this.groupBoxParameters = new System.Windows.Forms.GroupBox();
            this.buttonCollisionParameters = new System.Windows.Forms.Button();
            this.groupBoxGearPoints = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.vectorControlGearPoint = new RCSim.AircraftEditor.Dialogs.VectorControl();
            this.buttonCloneGearPoint = new System.Windows.Forms.Button();
            this.buttonDeleteGearPoint = new System.Windows.Forms.Button();
            this.buttonAddGearPoint = new System.Windows.Forms.Button();
            this.listBoxGearPoints = new System.Windows.Forms.ListBox();
            this.groupBoxFloatPoints = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.vectorControlFloatPoint = new RCSim.AircraftEditor.Dialogs.VectorControl();
            this.buttonCloneFloatPoint = new System.Windows.Forms.Button();
            this.buttonDeleteFloatPoint = new System.Windows.Forms.Button();
            this.buttonAddFloatPoint = new System.Windows.Forms.Button();
            this.listBoxFloatPoints = new System.Windows.Forms.ListBox();
            this.groupBoxCollision.SuspendLayout();
            this.groupBoxParameters.SuspendLayout();
            this.groupBoxGearPoints.SuspendLayout();
            this.groupBoxFloatPoints.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxCollision
            // 
            this.groupBoxCollision.Controls.Add(this.labelCurrentPoint);
            this.groupBoxCollision.Controls.Add(this.vectorControlCollisionPoint);
            this.groupBoxCollision.Controls.Add(this.buttonClone);
            this.groupBoxCollision.Controls.Add(this.buttonDelete);
            this.groupBoxCollision.Controls.Add(this.buttonAdd);
            this.groupBoxCollision.Controls.Add(this.listBoxPoints);
            this.groupBoxCollision.Location = new System.Drawing.Point(3, 145);
            this.groupBoxCollision.Name = "groupBoxCollision";
            this.groupBoxCollision.Size = new System.Drawing.Size(253, 140);
            this.groupBoxCollision.TabIndex = 0;
            this.groupBoxCollision.TabStop = false;
            this.groupBoxCollision.Text = "Collision Points";
            // 
            // labelCurrentPoint
            // 
            this.labelCurrentPoint.AutoSize = true;
            this.labelCurrentPoint.Location = new System.Drawing.Point(7, 115);
            this.labelCurrentPoint.Name = "labelCurrentPoint";
            this.labelCurrentPoint.Size = new System.Drawing.Size(44, 13);
            this.labelCurrentPoint.TabIndex = 3;
            this.labelCurrentPoint.Text = "Current:";
            // 
            // vectorControlCollisionPoint
            // 
            this.vectorControlCollisionPoint.Location = new System.Drawing.Point(64, 107);
            this.vectorControlCollisionPoint.Name = "vectorControlCollisionPoint";
            this.vectorControlCollisionPoint.Size = new System.Drawing.Size(183, 29);
            this.vectorControlCollisionPoint.TabIndex = 2;
            this.vectorControlCollisionPoint.Vector = ((Microsoft.DirectX.Vector3)(resources.GetObject("vectorControlCollisionPoint.Vector")));
            this.vectorControlCollisionPoint.VectorChanged += new System.EventHandler(this.vectorControlCollisionPoint_VectorChanged);
            // 
            // buttonClone
            // 
            this.buttonClone.Location = new System.Drawing.Point(169, 77);
            this.buttonClone.Name = "buttonClone";
            this.buttonClone.Size = new System.Drawing.Size(78, 23);
            this.buttonClone.TabIndex = 1;
            this.buttonClone.Text = "Clone point";
            this.buttonClone.UseVisualStyleBackColor = true;
            this.buttonClone.Click += new System.EventHandler(this.buttonClone_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(169, 48);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(78, 23);
            this.buttonDelete.TabIndex = 1;
            this.buttonDelete.Text = "Delete point";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(169, 19);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(78, 23);
            this.buttonAdd.TabIndex = 1;
            this.buttonAdd.Text = "Add point";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // listBoxPoints
            // 
            this.listBoxPoints.FormattingEnabled = true;
            this.listBoxPoints.Location = new System.Drawing.Point(6, 19);
            this.listBoxPoints.Name = "listBoxPoints";
            this.listBoxPoints.Size = new System.Drawing.Size(157, 82);
            this.listBoxPoints.TabIndex = 0;
            this.listBoxPoints.SelectedIndexChanged += new System.EventHandler(this.listBoxPoints_SelectedIndexChanged);
            // 
            // groupBoxParameters
            // 
            this.groupBoxParameters.Controls.Add(this.buttonCollisionParameters);
            this.groupBoxParameters.Location = new System.Drawing.Point(3, 429);
            this.groupBoxParameters.Name = "groupBoxParameters";
            this.groupBoxParameters.Size = new System.Drawing.Size(253, 48);
            this.groupBoxParameters.TabIndex = 1;
            this.groupBoxParameters.TabStop = false;
            this.groupBoxParameters.Text = "Parameters";
            // 
            // buttonCollisionParameters
            // 
            this.buttonCollisionParameters.Location = new System.Drawing.Point(10, 19);
            this.buttonCollisionParameters.Name = "buttonCollisionParameters";
            this.buttonCollisionParameters.Size = new System.Drawing.Size(75, 23);
            this.buttonCollisionParameters.TabIndex = 5;
            this.buttonCollisionParameters.Text = "Parameters";
            this.buttonCollisionParameters.UseVisualStyleBackColor = true;
            this.buttonCollisionParameters.Click += new System.EventHandler(this.buttonCollisionParameters_Click);
            // 
            // groupBoxGearPoints
            // 
            this.groupBoxGearPoints.Controls.Add(this.label1);
            this.groupBoxGearPoints.Controls.Add(this.vectorControlGearPoint);
            this.groupBoxGearPoints.Controls.Add(this.buttonCloneGearPoint);
            this.groupBoxGearPoints.Controls.Add(this.buttonDeleteGearPoint);
            this.groupBoxGearPoints.Controls.Add(this.buttonAddGearPoint);
            this.groupBoxGearPoints.Controls.Add(this.listBoxGearPoints);
            this.groupBoxGearPoints.Location = new System.Drawing.Point(3, 3);
            this.groupBoxGearPoints.Name = "groupBoxGearPoints";
            this.groupBoxGearPoints.Size = new System.Drawing.Size(253, 140);
            this.groupBoxGearPoints.TabIndex = 0;
            this.groupBoxGearPoints.TabStop = false;
            this.groupBoxGearPoints.Text = "Landing Gear Points";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 115);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Current:";
            // 
            // vectorControlGearPoint
            // 
            this.vectorControlGearPoint.Location = new System.Drawing.Point(64, 107);
            this.vectorControlGearPoint.Name = "vectorControlGearPoint";
            this.vectorControlGearPoint.Size = new System.Drawing.Size(183, 29);
            this.vectorControlGearPoint.TabIndex = 2;
            this.vectorControlGearPoint.Vector = ((Microsoft.DirectX.Vector3)(resources.GetObject("vectorControlGearPoint.Vector")));
            this.vectorControlGearPoint.VectorChanged += new System.EventHandler(this.vectorControlGearPoint_VectorChanged);
            // 
            // buttonCloneGearPoint
            // 
            this.buttonCloneGearPoint.Location = new System.Drawing.Point(169, 77);
            this.buttonCloneGearPoint.Name = "buttonCloneGearPoint";
            this.buttonCloneGearPoint.Size = new System.Drawing.Size(78, 23);
            this.buttonCloneGearPoint.TabIndex = 1;
            this.buttonCloneGearPoint.Text = "Clone point";
            this.buttonCloneGearPoint.UseVisualStyleBackColor = true;
            this.buttonCloneGearPoint.Click += new System.EventHandler(this.buttonCloneGearPoint_Click);
            // 
            // buttonDeleteGearPoint
            // 
            this.buttonDeleteGearPoint.Location = new System.Drawing.Point(169, 48);
            this.buttonDeleteGearPoint.Name = "buttonDeleteGearPoint";
            this.buttonDeleteGearPoint.Size = new System.Drawing.Size(78, 23);
            this.buttonDeleteGearPoint.TabIndex = 1;
            this.buttonDeleteGearPoint.Text = "Delete point";
            this.buttonDeleteGearPoint.UseVisualStyleBackColor = true;
            this.buttonDeleteGearPoint.Click += new System.EventHandler(this.buttonDeleteGearPoint_Click);
            // 
            // buttonAddGearPoint
            // 
            this.buttonAddGearPoint.Location = new System.Drawing.Point(169, 19);
            this.buttonAddGearPoint.Name = "buttonAddGearPoint";
            this.buttonAddGearPoint.Size = new System.Drawing.Size(78, 23);
            this.buttonAddGearPoint.TabIndex = 1;
            this.buttonAddGearPoint.Text = "Add point";
            this.buttonAddGearPoint.UseVisualStyleBackColor = true;
            this.buttonAddGearPoint.Click += new System.EventHandler(this.buttonAddGearPoint_Click);
            // 
            // listBoxGearPoints
            // 
            this.listBoxGearPoints.FormattingEnabled = true;
            this.listBoxGearPoints.Location = new System.Drawing.Point(6, 19);
            this.listBoxGearPoints.Name = "listBoxGearPoints";
            this.listBoxGearPoints.Size = new System.Drawing.Size(157, 82);
            this.listBoxGearPoints.TabIndex = 0;
            this.listBoxGearPoints.SelectedIndexChanged += new System.EventHandler(this.listBoxGearPoints_SelectedIndexChanged);
            // 
            // groupBoxFloatPoints
            // 
            this.groupBoxFloatPoints.Controls.Add(this.label2);
            this.groupBoxFloatPoints.Controls.Add(this.vectorControlFloatPoint);
            this.groupBoxFloatPoints.Controls.Add(this.buttonCloneFloatPoint);
            this.groupBoxFloatPoints.Controls.Add(this.buttonDeleteFloatPoint);
            this.groupBoxFloatPoints.Controls.Add(this.buttonAddFloatPoint);
            this.groupBoxFloatPoints.Controls.Add(this.listBoxFloatPoints);
            this.groupBoxFloatPoints.Location = new System.Drawing.Point(3, 287);
            this.groupBoxFloatPoints.Name = "groupBoxFloatPoints";
            this.groupBoxFloatPoints.Size = new System.Drawing.Size(253, 140);
            this.groupBoxFloatPoints.TabIndex = 4;
            this.groupBoxFloatPoints.TabStop = false;
            this.groupBoxFloatPoints.Text = "Flotation Points";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 115);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Current:";
            // 
            // vectorControlFloatPoint
            // 
            this.vectorControlFloatPoint.Location = new System.Drawing.Point(64, 107);
            this.vectorControlFloatPoint.Name = "vectorControlFloatPoint";
            this.vectorControlFloatPoint.Size = new System.Drawing.Size(183, 29);
            this.vectorControlFloatPoint.TabIndex = 2;
            this.vectorControlFloatPoint.Vector = ((Microsoft.DirectX.Vector3)(resources.GetObject("vectorControlFloatPoint.Vector")));
            this.vectorControlFloatPoint.VectorChanged += new System.EventHandler(this.vectorControlFloatPoint_VectorChanged);
            // 
            // buttonCloneFloatPoint
            // 
            this.buttonCloneFloatPoint.Location = new System.Drawing.Point(169, 77);
            this.buttonCloneFloatPoint.Name = "buttonCloneFloatPoint";
            this.buttonCloneFloatPoint.Size = new System.Drawing.Size(78, 23);
            this.buttonCloneFloatPoint.TabIndex = 1;
            this.buttonCloneFloatPoint.Text = "Clone point";
            this.buttonCloneFloatPoint.UseVisualStyleBackColor = true;
            this.buttonCloneFloatPoint.Click += new System.EventHandler(this.buttonCloneFloatPoint_Click);
            // 
            // buttonDeleteFloatPoint
            // 
            this.buttonDeleteFloatPoint.Location = new System.Drawing.Point(169, 48);
            this.buttonDeleteFloatPoint.Name = "buttonDeleteFloatPoint";
            this.buttonDeleteFloatPoint.Size = new System.Drawing.Size(78, 23);
            this.buttonDeleteFloatPoint.TabIndex = 1;
            this.buttonDeleteFloatPoint.Text = "Delete point";
            this.buttonDeleteFloatPoint.UseVisualStyleBackColor = true;
            this.buttonDeleteFloatPoint.Click += new System.EventHandler(this.buttonDeleteFloatPoint_Click);
            // 
            // buttonAddFloatPoint
            // 
            this.buttonAddFloatPoint.Location = new System.Drawing.Point(169, 19);
            this.buttonAddFloatPoint.Name = "buttonAddFloatPoint";
            this.buttonAddFloatPoint.Size = new System.Drawing.Size(78, 23);
            this.buttonAddFloatPoint.TabIndex = 1;
            this.buttonAddFloatPoint.Text = "Add point";
            this.buttonAddFloatPoint.UseVisualStyleBackColor = true;
            this.buttonAddFloatPoint.Click += new System.EventHandler(this.buttonAddFloatPoint_Click);
            // 
            // listBoxFloatPoints
            // 
            this.listBoxFloatPoints.FormattingEnabled = true;
            this.listBoxFloatPoints.Location = new System.Drawing.Point(6, 19);
            this.listBoxFloatPoints.Name = "listBoxFloatPoints";
            this.listBoxFloatPoints.Size = new System.Drawing.Size(157, 82);
            this.listBoxFloatPoints.TabIndex = 0;
            this.listBoxFloatPoints.SelectedIndexChanged += new System.EventHandler(this.listBoxFloatPoints_SelectedIndexChanged);
            // 
            // CollisionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxFloatPoints);
            this.Controls.Add(this.groupBoxParameters);
            this.Controls.Add(this.groupBoxGearPoints);
            this.Controls.Add(this.groupBoxCollision);
            this.Name = "CollisionControl";
            this.Size = new System.Drawing.Size(256, 482);
            this.groupBoxCollision.ResumeLayout(false);
            this.groupBoxCollision.PerformLayout();
            this.groupBoxParameters.ResumeLayout(false);
            this.groupBoxGearPoints.ResumeLayout(false);
            this.groupBoxGearPoints.PerformLayout();
            this.groupBoxFloatPoints.ResumeLayout(false);
            this.groupBoxFloatPoints.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxCollision;
        private System.Windows.Forms.ListBox listBoxPoints;
        private VectorControl vectorControlCollisionPoint;
        private System.Windows.Forms.Button buttonClone;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Label labelCurrentPoint;
        private System.Windows.Forms.GroupBox groupBoxParameters;
        private System.Windows.Forms.GroupBox groupBoxGearPoints;
        private System.Windows.Forms.Label label1;
        private VectorControl vectorControlGearPoint;
        private System.Windows.Forms.Button buttonCloneGearPoint;
        private System.Windows.Forms.Button buttonDeleteGearPoint;
        private System.Windows.Forms.Button buttonAddGearPoint;
        private System.Windows.Forms.ListBox listBoxGearPoints;
        private System.Windows.Forms.Button buttonCollisionParameters;
        private System.Windows.Forms.GroupBox groupBoxFloatPoints;
        private System.Windows.Forms.Label label2;
        private VectorControl vectorControlFloatPoint;
        private System.Windows.Forms.Button buttonCloneFloatPoint;
        private System.Windows.Forms.Button buttonDeleteFloatPoint;
        private System.Windows.Forms.Button buttonAddFloatPoint;
        private System.Windows.Forms.ListBox listBoxFloatPoints;
    }
}
