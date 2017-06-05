using System;
using System.Windows.Forms;
using System.Collections;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Bonsai.Core
{
    #region Device Settings
    /// <summary>
    /// Holds the settings for creating a device
    /// </summary>
    public class DeviceSettings : ICloneable
    {
        public uint AdapterOrdinal;
        public DeviceType DeviceType;
        public Format AdapterFormat;
        public CreateFlags BehaviorFlags;
        public TextureFilter TextureFilter = TextureFilter.Anisotropic;
        public PresentParameters presentParams;
        
        #region ICloneable Members
        /// <summary>Clone this object</summary>
        public DeviceSettings Clone()
        {
            DeviceSettings clonedObject = new DeviceSettings();
            clonedObject.presentParams = (PresentParameters)this.presentParams.Clone();
            clonedObject.AdapterFormat = this.AdapterFormat;
            clonedObject.AdapterOrdinal = this.AdapterOrdinal;
            clonedObject.BehaviorFlags = this.BehaviorFlags;
            clonedObject.DeviceType = this.DeviceType;
            clonedObject.TextureFilter = this.TextureFilter;

            return clonedObject;
        }
        /// <summary>Clone this object</summary>
        object ICloneable.Clone() { throw new NotSupportedException("Use the strongly typed overload instead."); }
        #endregion
    }
    #endregion
}
