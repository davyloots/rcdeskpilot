using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Core.Interfaces;
using Bonsai.Objects;
using Bonsai.Objects.Meshes;
using Microsoft.DirectX;
using Bonsai.Core;

namespace RCSim
{
    internal class ScareCrow : IFrameworkCallback, IDisposable
    {
        #region Private classes
        protected class CornField : IFrameworkCallback, IDisposable
        {
            protected List<GameObject> corns = new List<GameObject>();
            protected List<GameObject> perpCorns = new List<GameObject>();
            public Vector3 Position = new Vector3();

            public CornField(XMesh cornMesh, Vector3 position)
            {
                this.Position = position;
                for (int i = 0; i < 3; i++)
                {
                    GameObject corn = new GameObject();
                    corn.Mesh = cornMesh;
                    corn.Scale = new Vector3(1.6f, 1.6f, 1.6f);
                    Program.Instance.TransparentObjectManager.Objects.Add(corn);
                    corn.Position = new Vector3(position.X, 0, position.Z + 6.6f * i - 6.6f);
                    corns.Add(corn);
                }
                for (int i = 0; i < 2; i++)
                {
                    GameObject corn = new GameObject();
                    corn.Mesh = cornMesh;
                    corn.Scale = new Vector3(1.6f, 1.6f, 1.6f);
                    Program.Instance.TransparentObjectManager.Objects.Add(corn);
                    corn.Position = new Vector3(position.X + 13f * i - 6.5f, 0, position.Z);
                    corn.RotateYAngle = (float)Math.PI / 2;
                    perpCorns.Add(corn);
                }
            }

            #region IDisposable Members
            public void Dispose()
            {
                foreach (GameObject corn in corns)
                {
                    Program.Instance.TransparentObjectManager.Objects.Remove(corn);
                    corn.Dispose();
                }
                foreach (GameObject corn in perpCorns)
                {
                    Program.Instance.TransparentObjectManager.Objects.Remove(corn);
                    corn.Dispose();
                }
                corns.Clear();
            }
            #endregion

            #region IFrameworkCallback Members
            public void OnFrameMove(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
            {
                float lookFromZ = Framework.Instance.CurrentCamera.LookFrom.Z;
                float lookFromX = Framework.Instance.CurrentCamera.LookFrom.X;
                foreach (GameObject corn in corns)
                {
                    if (lookFromZ > corn.Position.Z)
                        corn.RotateYAngle = (float)Math.PI;
                    else
                        corn.RotateYAngle = 0;
                }
                foreach (GameObject corn in perpCorns)
                {
                    if (lookFromX > corn.Position.X)
                        corn.RotateYAngle = (float)-Math.PI/2;
                    else
                        corn.RotateYAngle = (float)Math.PI/2;
                }
            }

            public void OnFrameRender(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
            {

            }
            #endregion            
        }
        #endregion

        #region Private fields
        protected List<CornField> cornfields = new List<CornField>();
        protected XMesh cornMesh;
        protected GameObject arrow = null;
        protected Birds birds = null;
        protected double lastUpdate = -10.0f;
        protected int currentTargetField = -1;
        protected Random rnd = new Random();
        protected double cropsLeft = 100.0;
        protected bool justArrived = true;
        protected double startTime = 0;
        protected double endTime = 0;
        protected int minutes = 0;
        protected int seconds = 0;
        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor.
        /// </summary>
        public ScareCrow()
        {
            cornMesh = new XMesh("data/corn.x");
            cornfields.Add(new CornField(cornMesh, new Vector3(30, 0, 30)));
            cornfields.Add(new CornField(cornMesh, new Vector3(-30, 0, 30)));
            cornfields.Add(new CornField(cornMesh, new Vector3(-30, 0, -30)));

            birds = new Birds(100);
            birds.Random = false;
            birds.SetRandomTarget();

            arrow = new GameObject();
            arrow.Mesh = new XMesh("data\\arrow.x");

            startTime = 0;
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Clean up.
        /// </summary>
        public void Dispose()
        {
            Program.Instance.CenterHud.ShowGameText("", 1000);
            foreach (CornField field in cornfields)
            {
                field.Dispose();
            }
            cornfields.Clear();
            if (birds != null)
            {
                birds.Dispose();
                birds = null;
            }

            if (arrow != null)
            {
                arrow.Dispose();
                arrow = null;
            }
            cornMesh.Dispose();
        }
        #endregion

        #region Public methods
        public void Restart()
        {
            startTime = 0;
            currentTargetField = -1;
            cropsLeft = 100.0;
            currentTargetField = -1;
            lastUpdate = -10.0f;
        }
        #endregion

        #region Protected methods
        protected void Retarget()
        {
            int newTarget = 0;
            do
            {
                newTarget = rnd.Next(cornfields.Count);
            }
            while (newTarget == currentTargetField);
            currentTargetField = newTarget;
            birds.SetTarget(cornfields[newTarget].Position);
            justArrived = false;
        }
        #endregion


        #region IFrameworkCallback Members
        public void OnFrameMove(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            if (startTime != 0)
            {
                if (cropsLeft > 0)
                {
                    minutes = (int)Math.Floor((Program.Instance.CurrentTime - startTime) / 60);
                    seconds = (int)Math.Floor((Program.Instance.CurrentTime - startTime) - minutes * 60);
                }

                if (cropsLeft < 0)
                {
                    Program.Instance.CenterHud.ShowGameText(string.Format("Game over!\nYou defended the crops for {0} minutes and {1} seconds",
                         minutes, seconds), 1000);
                }
                else
                    Program.Instance.CenterHud.ShowGameText(string.Format("Your time : {0}:{1}\nCrops remaining : {2}%", minutes, seconds.ToString("00"), (int)(Math.Floor(cropsLeft))),
                        1000);
            }
            else
                startTime = Program.Instance.CurrentTime;
            
            if (totalTime > lastUpdate + 5)
            {
                if ((birds.TargetReached) || (currentTargetField == -1)) 
                {
                    if ((birds.TargetReached) && (currentTargetField != -1))
                    {
                        if (!justArrived)
                            justArrived = true;
                        else
                            Retarget();
                    }
                    else
                        Retarget();                    
                }                
                lastUpdate = totalTime;
            }
            if (birds.TargetReached)
            {
                if (currentTargetField != -1)
                {
                    bool temp = birds.TargetReached;
                    cropsLeft -= (double)(2 * elapsedTime);
                    arrow.Position = new Vector3(cornfields[currentTargetField].Position.X, 3f, cornfields[currentTargetField].Position.Z);
                    arrow.RotateYAngle = (float)totalTime;
                    arrow.OnFrameMove(device, totalTime, elapsedTime);
                }
            }
            foreach (CornField field in cornfields)
                field.OnFrameMove(device, totalTime, elapsedTime);
            birds.OnFrameMove(device, totalTime, elapsedTime);
            if (birds.Scared)
            {
                currentTargetField = -1;
                birds.SetRandomTarget();
            }
        }

        public void OnFrameRender(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            foreach (CornField field in cornfields)
                field.OnFrameRender(device, totalTime, elapsedTime);
            birds.OnFrameRender(device, totalTime, elapsedTime);
            if (birds.TargetReached)
            {
                if (currentTargetField != -1)
                {
                    arrow.OnFrameRender(device, totalTime, elapsedTime);
                }
            }
        }
        #endregion
    }
}
