using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX;

namespace RCSim.AircraftEditor.Dialogs
{
    internal partial class CollisionControl : UserControl
    {
        public class CollisionPoint
        {
            public int Index = 0;
            public Vector3 Vector;

            public override string ToString()
            {
                return string.Format("({0};{1};{2})",
                    Vector.X.ToString("F03"), Vector.Y.ToString("F03"), Vector.Z.ToString("F03"));
            }
        }

        protected ModelControl modelControl;
        protected List<Vector3> gearPoints;
        protected List<Vector3> collisionPoints;
        protected List<Vector3> floatPoints;
        protected CollisionParametersForm parametersForm;

        public ModelControl ModelControl
        {
            get { return modelControl; }
            set
            {
                modelControl = value;
                if (modelControl != null)
                {
                    collisionPoints = new List<Vector3>();
                    foreach (Vector3 point in modelControl.AirplaneModel.AirplaneControl.AircraftParameters.CollisionPoints)
                    {
                        collisionPoints.Add(FlightModelWind.ToDirectX(point));
                    }
                    UpdateListBoxCollisionPoints();
                    gearPoints = new List<Vector3>();
                    foreach (Vector3 point in modelControl.AirplaneModel.AirplaneControl.AircraftParameters.GearPoints)
                    {
                        gearPoints.Add(FlightModelWind.ToDirectX(point));
                    }
                    UpdateListBoxGearPoints();
                    floatPoints = new List<Vector3>();
                    foreach (Vector3 point in modelControl.AirplaneModel.AirplaneControl.AircraftParameters.FloatPoints)
                    {
                        floatPoints.Add(FlightModelWind.ToDirectX(point));
                    }
                    UpdateListBoxFloatPoints();
                   
                    if (parametersForm != null)
                        parametersForm.Close();
                }
            }
        }

        public CollisionControl()
        {
            InitializeComponent();
        }

        protected void UpdateListBoxCollisionPoints()
        {
            listBoxPoints.Items.Clear();
            if (collisionPoints != null)
            {
                for (int i = 0; i < collisionPoints.Count; i++)
                {
                    CollisionPoint colPt = new CollisionPoint();
                    colPt.Index = i;
                    colPt.Vector = collisionPoints[i];
                    listBoxPoints.Items.Add(colPt);
                }
            }
        }

        protected void UpdateListBoxGearPoints()
        {
            listBoxGearPoints.Items.Clear();
            if (gearPoints != null)
            {
                for (int i = 0; i < gearPoints.Count; i++)
                {
                    CollisionPoint colPt = new CollisionPoint();
                    colPt.Index = i;
                    colPt.Vector = gearPoints[i];
                    listBoxGearPoints.Items.Add(colPt);
                }
            }
        }

        protected void UpdateListBoxFloatPoints()
        {
            listBoxFloatPoints.Items.Clear();
            if (floatPoints != null)
            {
                for (int i = 0; i < floatPoints.Count; i++)
                {
                    CollisionPoint colPt = new CollisionPoint();
                    colPt.Index = i;
                    colPt.Vector = floatPoints[i];
                    listBoxFloatPoints.Items.Add(colPt);
                }
            }
        }

        private void listBoxFloatPoints_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxFloatPoints.SelectedIndex > -1)
            {
                vectorControlFloatPoint.Vector = floatPoints[listBoxFloatPoints.SelectedIndex];
                CollisionPoints.SelectedCollisionPoint = -1;
                CollisionPoints.SelectedGearPoint = -1;
                CollisionPoints.SelectedFloatPoint = listBoxFloatPoints.SelectedIndex;
                Program.Instance.CollisionPointsUpdated();
            }
            else
            {
                CollisionPoints.SelectedCollisionPoint = -1;
                CollisionPoints.SelectedGearPoint = -1;
                CollisionPoints.SelectedFloatPoint = -1;
                Program.Instance.CollisionPointsUpdated();
            }
        }

        private void listBoxGearPoints_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxGearPoints.SelectedIndex > -1)
            {
                vectorControlGearPoint.Vector = gearPoints[listBoxGearPoints.SelectedIndex];
                CollisionPoints.SelectedCollisionPoint = -1;
                CollisionPoints.SelectedFloatPoint = -1;
                CollisionPoints.SelectedGearPoint = listBoxGearPoints.SelectedIndex;
                Program.Instance.CollisionPointsUpdated();
            }
            else
            {
                CollisionPoints.SelectedCollisionPoint = -1;
                CollisionPoints.SelectedGearPoint = -1;
                CollisionPoints.SelectedFloatPoint = -1;
                Program.Instance.CollisionPointsUpdated();
            }
        }

        private void listBoxPoints_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxPoints.SelectedIndex > -1)
            {
                vectorControlCollisionPoint.Vector = collisionPoints[listBoxPoints.SelectedIndex];
                CollisionPoints.SelectedCollisionPoint = listBoxPoints.SelectedIndex;
                CollisionPoints.SelectedGearPoint = -1;
                CollisionPoints.SelectedFloatPoint = -1;
                Program.Instance.CollisionPointsUpdated();
            }
            else
            {
                CollisionPoints.SelectedCollisionPoint = -1;
                CollisionPoints.SelectedGearPoint = -1;
                CollisionPoints.SelectedFloatPoint = -1;
                Program.Instance.CollisionPointsUpdated();
            }
        }

        private void vectorControlFloatPoint_VectorChanged(object sender, EventArgs e)
        {
            if (listBoxFloatPoints.SelectedIndex > -1)
            {
                floatPoints[listBoxFloatPoints.SelectedIndex] = vectorControlFloatPoint.Vector;
                CollisionPoint colPt = listBoxFloatPoints.Items[listBoxFloatPoints.SelectedIndex] as CollisionPoint;
                if (colPt != null)
                {
                    colPt.Vector = vectorControlFloatPoint.Vector;
                    listBoxFloatPoints.Items[listBoxFloatPoints.SelectedIndex] = colPt;
                }
                ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.FloatPoints[listBoxFloatPoints.SelectedIndex] =
                    FlightModelWind.ToModel(vectorControlFloatPoint.Vector);
                ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.UnscaledFloatPoints[listBoxFloatPoints.SelectedIndex] =
                    FlightModelWind.ToModel(vectorControlFloatPoint.Vector) * (1f / ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.Scale);
                Program.Instance.CollisionPointsMoved();
            }
        }

        private void vectorControlGearPoint_VectorChanged(object sender, EventArgs e)
        {
            if (listBoxGearPoints.SelectedIndex > -1)
            {
                gearPoints[listBoxGearPoints.SelectedIndex] = vectorControlGearPoint.Vector;
                CollisionPoint colPt = listBoxGearPoints.Items[listBoxGearPoints.SelectedIndex] as CollisionPoint;
                if (colPt != null)
                {
                    colPt.Vector = vectorControlGearPoint.Vector;
                    listBoxGearPoints.Items[listBoxGearPoints.SelectedIndex] = colPt;
                }
                ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.GearPoints[listBoxGearPoints.SelectedIndex] =
                    FlightModelWind.ToModel(vectorControlGearPoint.Vector);
                ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.UnscaledGearPoints[listBoxGearPoints.SelectedIndex] =
                    FlightModelWind.ToModel(vectorControlGearPoint.Vector) * (1f / ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.Scale);
                Program.Instance.CollisionPointsMoved();
            }
        }


        private void vectorControlCollisionPoint_VectorChanged(object sender, EventArgs e)
        {
            if (listBoxPoints.SelectedIndex > -1)
            {
                collisionPoints[listBoxPoints.SelectedIndex] = vectorControlCollisionPoint.Vector;
                CollisionPoint colPt = listBoxPoints.Items[listBoxPoints.SelectedIndex] as CollisionPoint;
                if (colPt != null)
                {
                    colPt.Vector = vectorControlCollisionPoint.Vector;
                    listBoxPoints.Items[listBoxPoints.SelectedIndex] = colPt;
                }
                ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.CollisionPoints[listBoxPoints.SelectedIndex] =
                    FlightModelWind.ToModel(vectorControlCollisionPoint.Vector);
                ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.UnscaledCollisionPoints[listBoxPoints.SelectedIndex] =
                    FlightModelWind.ToModel(vectorControlCollisionPoint.Vector) * (1f/ ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.Scale);
                Program.Instance.CollisionPointsMoved();
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            collisionPoints.Add(new Vector3(0,0,0));
            ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.CollisionPoints.Add(new Vector3(0,0,0));
            ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.UnscaledCollisionPoints.Add(new Vector3(0,0,0));
            Program.Instance.CollisionPointsUpdated();
            UpdateListBoxCollisionPoints();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (listBoxPoints.SelectedIndex > -1)
            {
                CollisionPoint point = listBoxPoints.Items[listBoxPoints.SelectedIndex] as CollisionPoint;
                if (point != null)
                {
                    collisionPoints.RemoveAt(point.Index);
                    ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.CollisionPoints.RemoveAt(point.Index);
                    ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.UnscaledCollisionPoints.RemoveAt(point.Index);
                    Program.Instance.CollisionPointsUpdated();
                    UpdateListBoxCollisionPoints();
                }
            }
        }

        private void buttonClone_Click(object sender, EventArgs e)
        {
            if (listBoxPoints.SelectedIndex > -1)
            {
                CollisionPoint point = listBoxPoints.Items[listBoxPoints.SelectedIndex] as CollisionPoint;
                if (point != null)
                {
                    collisionPoints.Add(new Vector3(-point.Vector.X, point.Vector.Y, point.Vector.Z));
                    ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.CollisionPoints.Add(
                        FlightModelWind.ToModel(new Vector3(-point.Vector.X, point.Vector.Y, point.Vector.Z)));
                    ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.UnscaledCollisionPoints.Add(
                        FlightModelWind.ToModel(new Vector3(-point.Vector.X, point.Vector.Y, point.Vector.Z) * 
                            (1f / ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.Scale)));
                    Program.Instance.CollisionPointsUpdated();
                    UpdateListBoxCollisionPoints();
                }
            }
        }


        private void buttonAddGearPoint_Click(object sender, EventArgs e)
        {
            gearPoints.Add(new Vector3(0, 0, 0));
            ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.GearPoints.Add(new Vector3(0, 0, 0));
            ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.UnscaledGearPoints.Add(new Vector3(0, 0, 0));
            Program.Instance.CollisionPointsUpdated();
            UpdateListBoxGearPoints();
        }

        private void buttonDeleteGearPoint_Click(object sender, EventArgs e)
        {
            if (listBoxGearPoints.SelectedIndex > -1)
            {
                CollisionPoint point = listBoxGearPoints.Items[listBoxGearPoints.SelectedIndex] as CollisionPoint;
                if (point != null)
                {
                    gearPoints.RemoveAt(point.Index);
                    ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.GearPoints.RemoveAt(point.Index);
                    ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.UnscaledGearPoints.RemoveAt(point.Index);
                    Program.Instance.CollisionPointsUpdated();
                    UpdateListBoxGearPoints();
                }
            }
        }

        private void buttonCloneGearPoint_Click(object sender, EventArgs e)
        {
            if (listBoxGearPoints.SelectedIndex > -1)
            {
                CollisionPoint point = listBoxGearPoints.Items[listBoxGearPoints.SelectedIndex] as CollisionPoint;
                if (point != null)
                {
                    gearPoints.Add(new Vector3(-point.Vector.X, point.Vector.Y, point.Vector.Z));
                    ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.GearPoints.Add(
                        FlightModelWind.ToModel(new Vector3(-point.Vector.X, point.Vector.Y, point.Vector.Z)));
                    ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.UnscaledGearPoints.Add(
                        FlightModelWind.ToModel(new Vector3(-point.Vector.X, point.Vector.Y, point.Vector.Z) *
                            (1f / ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.Scale)));
                    Program.Instance.CollisionPointsUpdated();
                    UpdateListBoxGearPoints();
                }
            }
        }
                
        private void buttonCollisionParameters_Click(object sender, EventArgs e)
        {
            if ((parametersForm == null) && (ModelControl != null))
            {
                parametersForm = new CollisionParametersForm(ModelControl);
                parametersForm.FormClosed += new FormClosedEventHandler(parametersForm_FormClosed);
            }
            if (parametersForm != null)
            {
                parametersForm.Show();
                parametersForm.BringToFront();
            }
        }

        void parametersForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (parametersForm != null)
            {
                parametersForm.Dispose();
                parametersForm = null;
            }
        }

        private void buttonAddFloatPoint_Click(object sender, EventArgs e)
        {
            floatPoints.Add(new Vector3(0, 0, 0));
            ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.FloatPoints.Add(new Vector3(0, 0, 0));
            ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.UnscaledFloatPoints.Add(new Vector3(0, 0, 0));
            Program.Instance.CollisionPointsUpdated();
            UpdateListBoxFloatPoints();
        }

        private void buttonDeleteFloatPoint_Click(object sender, EventArgs e)
        {
            if (listBoxFloatPoints.SelectedIndex > -1)
            {
                CollisionPoint point = listBoxFloatPoints.Items[listBoxFloatPoints.SelectedIndex] as CollisionPoint;
                if (point != null)
                {
                    floatPoints.RemoveAt(point.Index);
                    ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.FloatPoints.RemoveAt(point.Index);
                    ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.UnscaledFloatPoints.RemoveAt(point.Index);
                    Program.Instance.CollisionPointsUpdated();
                    UpdateListBoxFloatPoints();
                }
            }
        }

        private void buttonCloneFloatPoint_Click(object sender, EventArgs e)
        {
            if (listBoxFloatPoints.SelectedIndex > -1)
            {
                CollisionPoint point = listBoxFloatPoints.Items[listBoxFloatPoints.SelectedIndex] as CollisionPoint;
                if (point != null)
                {
                    floatPoints.Add(new Vector3(-point.Vector.X, point.Vector.Y, point.Vector.Z));
                    ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.FloatPoints.Add(
                        FlightModelWind.ToModel(new Vector3(-point.Vector.X, point.Vector.Y, point.Vector.Z)));
                    ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.UnscaledFloatPoints.Add(
                        FlightModelWind.ToModel(new Vector3(-point.Vector.X, point.Vector.Y, point.Vector.Z) *
                            (1f / ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.Scale)));
                    Program.Instance.CollisionPointsUpdated();
                    UpdateListBoxFloatPoints();
                }
            }
        }

      
        
    }
}
