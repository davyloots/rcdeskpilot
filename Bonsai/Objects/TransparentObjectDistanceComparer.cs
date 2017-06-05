using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Bonsai.Core;
using Bonsai.Core.Interfaces;

namespace Bonsai.Objects
{
    public class TransparentObjectDistanceComparer : IComparer<ITransparentObject>
    {
        #region IComparer<GameObject> Members
        /// <summary>
        /// Compares two GameObjects 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(ITransparentObject x, ITransparentObject y)        
        {
            Vector3 camFrom = Framework.Instance.CurrentCamera.LookFrom;
            Vector3 diffX = x.WorldPosition - camFrom;
            Vector3 diffY = y.WorldPosition - camFrom;
            if (Math.Abs(diffX.LengthSq() - diffY.LengthSq()) < 0.01f)
                return 0;
            else if (diffX.LengthSq() > diffY.LengthSq())
                return 1;
            else
                return -1;
        }
        #endregion
    }
}
