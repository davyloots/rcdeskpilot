using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Bonsai.Core.Interfaces;

namespace Bonsai.Objects
{
    public class TransparentObjectManager : IFrameworkCallback
    {
        #region Private fields
        private List<ITransparentObject> objects = new List<ITransparentObject>();
        private TransparentObjectDistanceComparer comparer = new TransparentObjectDistanceComparer();
        #endregion

        #region Properties
        /// <summary>
        /// Gets the list of transparent objects.
        /// </summary>
        public List<ITransparentObject> Objects
        {
            get { return objects; }
        }

        /// <summary>
        /// Indicates whether the transparentobjectmanager is currently rendering.
        /// </summary>
        public bool Rendering
        {
            get;
            set;
        }
        #endregion

        #region IFrameworkCallback Members
        /// <summary>
        /// Prepares the next frame of all transparent objects.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="totalTime"></param>
        /// <param name="elapsedTime"></param>
        public void OnFrameMove(Device device, double totalTime, float elapsedTime)
        {
            Rendering = true;
            for (int i = objects.Count - 1; i >= 0; i--)
            {
                objects[i].OnFrameMove(device, totalTime, elapsedTime);
            }
            objects.Sort(comparer);
            Rendering = false;
        }

        /// <summary>
        /// Renders all transparent objects from back to front.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="totalTime"></param>
        /// <param name="elapsedTime"></param>
        public void OnFrameRender(Device device, double totalTime, float elapsedTime)
        {
            Rendering = true;
            device.RenderState.NormalizeNormals = true;
            for (int i = objects.Count - 1; i >= 0; i--)
            {
                objects[i].OnFrameRender(device, totalTime, elapsedTime);
            }
            device.RenderState.NormalizeNormals = false;
            Rendering = false;
        }
        #endregion
    }
}
