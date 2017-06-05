using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Objects;
using Bonsai.Objects.Meshes;
using Microsoft.DirectX;

namespace RCSim
{
    internal class Tractor : GameObject, IDisposable
    {
        #region private fields
        private GameObject frontWheels = new GameObject();
        private GameObject rearWheels = new GameObject();
        private static XMesh meshFixed = null;
        private static XMesh meshFrontWheels = null;
        private static XMesh meshRearWheels = null;
        private static int count = 0;
        private float X1 = -152f;
        private float Z1 = 310f;
        private float X2 = -42f;
        private float Z2 = 188f;
        private float speed = 3f;
        private bool toRight = true;
        private DriveStageEnum driveStage = DriveStageEnum.South;
        private float turnRadius = 3.0f;
        private float turnAngle = 0;
        private float rotX = 0;
        private float rotZ = 0;
        #endregion

        #region Private enums
        private enum DriveStageEnum
        {
            South,
            North,
            TurnToNorth,
            TurnToSouth
        }
        #endregion

        #region Constructor
        public Tractor()
        {
            count++;
            if (meshFixed == null)
                meshFixed = new XMesh("data\\tractor_fixed.x");
            if (meshFrontWheels == null)
                meshFrontWheels = new XMesh("data\\tractor_frontwheels.x");
            if (meshRearWheels == null)
                meshRearWheels = new XMesh("data\\tractor_rearwheels.x");

            this.Mesh = meshFixed;
            this.Scale = new Microsoft.DirectX.Vector3(0.7f, 0.7f, 0.7f);
            frontWheels.Mesh = meshFrontWheels;
            frontWheels.Position = new Microsoft.DirectX.Vector3(0, -1.5503f, 2.3186f);
            rearWheels.Mesh = meshRearWheels;
            rearWheels.Position = new Microsoft.DirectX.Vector3(0, -0.8669f, -1.7969f);
            this.AddChild(frontWheels);
            this.AddChild(rearWheels);
            this.Position = new Vector3(X1, 0, Z1);
            this.OrientationMode = OrientationModeEnum.YXZ;
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Clean up.
        /// </summary>
        public override void Dispose()
        {
            count--;
            if (count == 0)
            {
                meshFixed.Dispose();
                meshFixed = null;
                meshFrontWheels.Dispose();
                meshFrontWheels = null;
                meshRearWheels.Dispose();
                meshRearWheels = null;
            }
        }
        #endregion

        #region Private methods
        private void UpdatePosition(double totalTime, float elapsedTime)
        {
            if (elapsedTime > 1f)
                return;
            float x = this.Position.X;
            float z = this.Position.Z;
            
            switch (driveStage)
            {
                case DriveStageEnum.South:
                    z -= speed * elapsedTime;
                    this.RotateYAngle = (float)Math.PI;
                    if (z < Z2)
                    {
                        if (toRight && x > X2)
                            toRight = false;
                        else if (x < X1)
                            toRight = true;
                        if (toRight)
                        {
                            rotX = x + turnRadius;
                            rotZ = z;
                        }
                        else
                        {
                            rotX = x - turnRadius;
                            rotZ = z;
                        }
                        turnAngle = 0f;
                        driveStage = DriveStageEnum.TurnToNorth;
                    }
                    break;
                case DriveStageEnum.North:
                    z += speed * elapsedTime;
                    this.RotateYAngle = 0f;
                    if (z > Z1)
                    {
                        if (toRight && x > X2)
                            toRight = false;
                        else if (x < X1)
                            toRight = true;
                        if (toRight)
                        {
                            rotX = x + turnRadius;
                            rotZ = z;
                        }
                        else
                        {
                            rotX = x - turnRadius;
                            rotZ = z;
                        }
                        turnAngle = 0f;
                        driveStage = DriveStageEnum.TurnToSouth;
                    }
                    break;
                case DriveStageEnum.TurnToNorth:
                    turnAngle += elapsedTime;
                    if (toRight)
                    {
                        x = rotX - (float)Math.Cos(turnAngle) * turnRadius;
                        z = rotZ - (float)Math.Sin(turnAngle) * turnRadius;
                        this.RotateYAngle = (float)Math.PI - turnAngle;
                    }
                    else
                    {
                        x = rotX + (float)Math.Cos(turnAngle) * turnRadius;
                        z = rotZ - (float)Math.Sin(turnAngle) * turnRadius;
                        this.RotateYAngle = (float)Math.PI + turnAngle;
                    }
                    if (turnAngle > Math.PI)
                        driveStage = DriveStageEnum.North;
                    break;
                case DriveStageEnum.TurnToSouth:
                    turnAngle += elapsedTime;
                    if (toRight)
                    {
                        x = rotX - (float)Math.Cos(turnAngle) * turnRadius;
                        z = rotZ + (float)Math.Sin(turnAngle) * turnRadius;
                        this.RotateYAngle = (float) turnAngle;
                    }
                    else
                    {
                        x = rotX + (float)Math.Cos(turnAngle) * turnRadius;
                        z = rotZ + (float)Math.Sin(turnAngle) * turnRadius;
                        this.RotateYAngle = -(float)turnAngle;
                    }
                    if (turnAngle > Math.PI)
                        driveStage = DriveStageEnum.South;
                    break;
            }
            float terrainHeight = Program.Instance.Heightmap.GetHeightAt(x, z);
            this.Position = new Vector3(x, terrainHeight + 1.5f, z);            
            Vector3 normal = Program.Instance.Heightmap.GetSmoothNormalAt(x, z);
            this.RotateZAngle = -(float) Math.Atan((double)normal.X);
            this.RotateXAngle = (float) Math.Atan((double)normal.Z);
            this.OrientationMode = OrientationModeEnum.YXZ;
        }
        #endregion

        #region Overridden GameObject methods
        public override void OnFrameMove(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            UpdatePosition(totalTime, elapsedTime);
            frontWheels.RotateXAngle = (float)(1.2f*speed*totalTime);
            rearWheels.RotateXAngle = (float)(speed*totalTime / 1.6f);
            base.OnFrameMove(device, totalTime, elapsedTime);
        }

        public override void OnFrameRender(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            base.OnFrameRender(device, totalTime, elapsedTime);
            // Render the shadow
            Vector3 p1;
            Vector3 p2;
            Vector3 p3;
            Vector3 p4 = new Vector3(0, 0.005f, 0);
            Program.Instance.Heightmap.GetPoints(Position.X, Position.Z, out p1, out p2, out p3);

            OnRenderShadow(device, p1 + p4, p2 + p4, p3 + p4, new Vector3(0, -1, 0));
        }
        #endregion
    }
}
