using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Objects;
using Bonsai.Core.Interfaces;
using Microsoft.DirectX;
using Bonsai.Objects.Meshes;

namespace RCSim.AircraftEditor
{
    internal class CollisionPoints : IFrameworkCallback
    {
        protected List<GameObject> points = new List<GameObject>();
        protected List<GameObject> gearPoints = new List<GameObject>();
        protected List<GameObject> floatPoints = new List<GameObject>();
        protected ModelControl modelControl;
        protected XMesh cursorMesh = null;
        protected XMesh cursorMesh2 = null;
        protected XMesh cursorMesh3 = null;

        public static int SelectedCollisionPoint = -1;
        public static int SelectedGearPoint = -1;
        public static int SelectedFloatPoint = -1;

        public ModelControl ModelControl
        {
            get { return modelControl; }
            set
            {
                modelControl = value;
                if (modelControl != null)
                {
                    UpdatePoints();
                }
            }
        }

        public CollisionPoints()
        {
            cursorMesh = new XMesh("data/cursor.x");
            cursorMesh2 = new XMesh("data/cursor2.x");
            cursorMesh3 = new XMesh("data/cursor3.x");
        }

        public void UpdatePointPositions()
        {
            for (int i = 0; i < ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.UnscaledCollisionPoints.Count; i++)
            {
                if (i < points.Count)
                {
                    points[i].Position = FlightModelWind.ToDirectX(ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.UnscaledCollisionPoints[i]) *
                        ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.Scale;
                }
            }

            for (int i = 0; i < ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.UnscaledGearPoints.Count; i++)
            {
                if (i < gearPoints.Count)
                {
                    gearPoints[i].Position = FlightModelWind.ToDirectX(ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.UnscaledGearPoints[i]) *
                        ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.Scale;
                }
            }

            for (int i = 0; i < ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.UnscaledFloatPoints.Count; i++)
            {
                if (i < floatPoints.Count)
                {
                    floatPoints[i].Position = FlightModelWind.ToDirectX(ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.UnscaledFloatPoints[i]) *
                        ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.Scale;
                }
            }
        }

        public void UpdatePoints()
        {
            if (modelControl != null)
            {
                foreach (GameObject point in points)
                {
                    point.Dispose();
                }
                points.Clear();
                for (int i = 0; i < ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.UnscaledCollisionPoints.Count; i++)
                {
                    Vector3 point = ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.UnscaledCollisionPoints[i];
                    GameObject cursor = new GameObject();
                    cursor.Position = FlightModelWind.ToDirectX(point) * ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.Scale;
                    cursor.Mesh = cursorMesh;
                    if (i == SelectedCollisionPoint)
                        cursor.Scale = new Vector3(0.03f, 0.03f, 0.03f);
                    else
                        cursor.Scale = new Vector3(0.015f, 0.015f, 0.015f);
                    points.Add(cursor);
                }

                foreach (GameObject gearPoint in gearPoints)
                {
                    gearPoint.Dispose();
                }
                gearPoints.Clear();
                for (int i = 0; i < ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.UnscaledGearPoints.Count; i++)
                {
                    Vector3 point = ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.UnscaledGearPoints[i];
                    GameObject cursor = new GameObject();
                    cursor.Position = FlightModelWind.ToDirectX(point) * ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.Scale;
                    cursor.Mesh = cursorMesh2;
                    if (i == SelectedGearPoint)
                        cursor.Scale = new Vector3(0.03f, 0.03f, 0.03f);
                    else
                        cursor.Scale = new Vector3(0.015f, 0.015f, 0.015f);
                    gearPoints.Add(cursor);
                }

                foreach (GameObject floatPoint in floatPoints)
                {
                    floatPoint.Dispose();
                }
                floatPoints.Clear();
                for (int i = 0; i < ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.UnscaledFloatPoints.Count; i++)
                {
                    Vector3 point = ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.UnscaledFloatPoints[i];
                    GameObject cursor = new GameObject();
                    cursor.Position = FlightModelWind.ToDirectX(point) * ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.Scale;
                    cursor.Mesh = cursorMesh3;
                    if (i == SelectedFloatPoint)
                        cursor.Scale = new Vector3(0.03f, 0.03f, 0.03f);
                    else
                        cursor.Scale = new Vector3(0.015f, 0.015f, 0.015f);
                    floatPoints.Add(cursor);
                }
            }
        }

        #region IFrameworkCallback Members
        public void OnFrameMove(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            foreach (GameObject point in points)
                point.OnFrameMove(device, totalTime, elapsedTime);
            foreach (GameObject point in gearPoints)
                point.OnFrameMove(device, totalTime, elapsedTime);
            foreach (GameObject point in floatPoints)
                point.OnFrameMove(device, totalTime, elapsedTime);
        }

        public void OnFrameRender(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            foreach (GameObject point in points)
                point.OnFrameRender(device, totalTime, elapsedTime);
            foreach (GameObject point in gearPoints)
                point.OnFrameRender(device, totalTime, elapsedTime);
            foreach (GameObject point in floatPoints)
                point.OnFrameRender(device, totalTime, elapsedTime);
        }
        #endregion
    }
}
