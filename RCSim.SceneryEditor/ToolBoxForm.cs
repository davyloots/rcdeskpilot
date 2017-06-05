using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.DirectX;

namespace RCSim
{
    internal partial class ToolBoxForm : Form
    {
        #region Constructor
        public ToolBoxForm()
        {
            InitializeComponent();
        }
        #endregion

        #region Cursor
        public event EventHandler CursorEnabledChanged;

        public bool CursorEnabled
        {
            get { return checkBoxCursorEnable.Checked; }
        }

        public string CursorPosition
        {
            set { labelPosition.Text = value; }
        }

        private void checkBoxCursorEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (CursorEnabledChanged != null)
                CursorEnabledChanged(this, EventArgs.Empty);
        }
        #endregion

        #region Display
        public event EventHandler GroundEnabledChanged;

        public bool GroundEnabled
        {
            get { return checkBoxGround.Checked; }
        }

        private void checkBoxGround_CheckedChanged(object sender, EventArgs e)
        {

        }
        #endregion

        #region Maps
        public event EventHandler CreateLightMap;
        #endregion

        #region Objects
        public class ObjectEventArgs : EventArgs
        {
            public TerrainDefinition.ObjectTypeEnum ObjectType;
            public string FileName;
            public Vector3 Orientation;

            public ObjectEventArgs(TerrainDefinition.ObjectTypeEnum objectType)
            {
                this.ObjectType = objectType;
            }

            public ObjectEventArgs(TerrainDefinition.ObjectTypeEnum objectType, string fileName, Vector3 orientation)
            {
                this.ObjectType = objectType;
                this.FileName = fileName;
                this.Orientation = orientation;
            }
        }

        public delegate void ObjectEventHandler(object sender, ObjectEventArgs e);

        public event ObjectEventHandler ObjectAdded;

        private void buttonAddTree_Click(object sender, EventArgs e)
        {
            if (ObjectAdded != null)
                ObjectAdded(this, new ObjectEventArgs(TerrainDefinition.ObjectTypeEnum.Tree));
        }

        private void buttonAddSimpleTree_Click(object sender, EventArgs e)
        {
            if (ObjectAdded != null)
                ObjectAdded(this, new ObjectEventArgs(TerrainDefinition.ObjectTypeEnum.SimpleTree));
        }

        private void buttonAddSimpleTallTree_Click(object sender, EventArgs e)
        {
            if (ObjectAdded != null)
                ObjectAdded(this, new ObjectEventArgs(TerrainDefinition.ObjectTypeEnum.SimpleTallTree));
        }

        private void buttonAddSimpleSmallTree_Click(object sender, EventArgs e)
        {
            if (ObjectAdded != null)
                ObjectAdded(this, new ObjectEventArgs(TerrainDefinition.ObjectTypeEnum.SimpleSmallTree));
        }

        private void buttonWindmill_Click(object sender, EventArgs e)
        {
            if (ObjectAdded != null)
                ObjectAdded(this, new ObjectEventArgs(TerrainDefinition.ObjectTypeEnum.Windmill));
        }

        private void buttonAddObject_Click(object sender, EventArgs e)
        {
            if (ObjectAdded != null)
            {
                ObjectAdded(this, new ObjectEventArgs(TerrainDefinition.ObjectTypeEnum.SceneryObject, labelCurrentObject.Text, new Vector3()));
            }
        }

        #endregion

        private void buttonSelectObject_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "DirectX Meshes (.x)|*.x";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                labelCurrentObject.Text = Path.GetFileName(fileDialog.FileName);
            }
        }

        

        private void buttonLightMap_Click(object sender, EventArgs e)
        {
            if (CreateLightMap != null)
                CreateLightMap(this, EventArgs.Empty);
        }

        #region Race
        public class GateEventArgs : EventArgs
        {
            public Vector3 Orientation;
            public int SequenceNr;
            public int GateType;

            public GateEventArgs(Vector3 orientation, int sequenceNr, int gateType)
            {
                this.Orientation = orientation;
                this.SequenceNr = sequenceNr;
                this.GateType = gateType;
            }
        }

        public delegate void GateEventHandler(object sender, GateEventArgs e);

        public event GateEventHandler GateAdded;

        private void buttonGateAdd_Click(object sender, EventArgs e)
        {
            if (GateAdded != null)
                GateAdded(this, new GateEventArgs(new Vector3(0, (float)((float)(numericGateRotation.Value) * Math.PI * 2)/360, 0), (int)(numericGateSequence.Value), 1)); 
        }
        #endregion



    }
}
