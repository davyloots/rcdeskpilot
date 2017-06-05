using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Objects;
using Bonsai.Objects.Meshes;

namespace RCSim
{
    internal class Windmill : GameObject, IDisposable
    {
        #region private fields
        private GameObject turbine = new GameObject();
        private GameObject blades = new GameObject();
        private static XMesh meshFixed = null;
        private static XMesh meshTurbine = null;
        private static XMesh meshBlades = null;
        private static int count = 0;
        #endregion

        #region Constructor
        public Windmill()
        {
            count++;
            if (meshFixed == null)
                meshFixed = new XMesh("data\\windmill_fixed.x");
            if (meshTurbine == null)
                meshTurbine = new XMesh("data\\windmill_turbine.x");
            if (meshBlades == null)
                meshBlades = new XMesh("data\\windmill_blades.x");
            
            this.Mesh = meshFixed;
            this.Scale = new Microsoft.DirectX.Vector3(3f,3f,3f);
            turbine.Mesh = meshTurbine;
            turbine.Position = new Microsoft.DirectX.Vector3(0, 4.6f, 0);
            blades.Mesh = meshBlades;
            this.AddChild(turbine);
            turbine.AddChild(blades);
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
                meshTurbine.Dispose();
                meshTurbine = null;
                meshBlades.Dispose();
                meshBlades = null;
            }
        }
        #endregion

        #region Overridden GameObject methods
        public override void OnFrameMove(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            double windspeed = Program.Instance.Weather.Wind.ConstantWindSpeed;
            double currentDirection = Program.Instance.Weather.Wind.CurrentDirection;
            //turbine.RotateYAngle = (float)(Math.Sin(totalTime/30) + 1);
            if (windspeed > 0.01)
                turbine.RotateYAngle = (float)(Math.PI / 2 - currentDirection);
            else
                turbine.RotateYAngle = (float)(Math.PI / 2);            
            blades.RotateZAngle = (float)(totalTime*windspeed*0.7 + count);
            base.OnFrameMove(device, totalTime, elapsedTime);
        }

        public override void OnFrameRender(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            base.OnFrameRender(device, totalTime, elapsedTime);
        }
        #endregion
    }
}
