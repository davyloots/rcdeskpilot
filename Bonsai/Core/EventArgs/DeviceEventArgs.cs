using System;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Bonsai.Core.EventArgs
{
    /// <summary>Event arguments for device creation/reset</summary>
    public class DeviceEventArgs : System.EventArgs
    {
        // Class data
        public Device Device;
        public SurfaceDescription BackBufferDescription;

        public DeviceEventArgs(Device d, SurfaceDescription desc)
        {
            Device = d;
            BackBufferDescription = desc;
        }
    }
}
