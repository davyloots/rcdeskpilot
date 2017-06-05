using System;
using System.Collections.Generic;
using System.Text;

namespace Bonsai.Core
{
    /// <summary>Base class for sample exceptions</summary>
    public class BonsaiException : System.ApplicationException
    {
        public BonsaiException(string message) : base(message) { }
        public BonsaiException(string message, Exception inner) : base(message, inner) { }
    }

    /// <summary>
    /// The No Direct3D exception.  Something really had to go wrong for this to occur.
    /// </summary>
    public class NoDirect3DException : BonsaiException
    {
        public NoDirect3DException() : base("Could not initialize Direct3D. You may want to check that the latest version of DirectX is correctly installed on your system.") { }
        public NoDirect3DException(Exception inner) : base("Could not initialize Direct3D. You may want to check that the latest version of DirectX is correctly installed on your system.", inner) { }
    }
    /// <summary>
    /// No compatible devices were found for this application.  
    /// </summary>
    public class NoCompatibleDevicesException : BonsaiException
    {
        public NoCompatibleDevicesException() : base("Could not find any compatible Direct3D devices.") { }
        public NoCompatibleDevicesException(Exception inner) : base("Could not find any compatible Direct3D devices.", inner) { }
    }
    /// <summary>
    /// Media couldn't be found
    /// </summary>
    public class MediaNotFoundException : BonsaiException
    {
        public MediaNotFoundException() : base("Could not find required media.") { }
        public MediaNotFoundException(Exception inner) : base("Could not find required media.", inner) { }
        public MediaNotFoundException(string file) : base(string.Format("Could not find required media: {0}", file)) { }
    }
    /// <summary>
    /// Creating the device failed
    /// </summary>
    public class CreatingDeviceException : BonsaiException
    {
        public CreatingDeviceException() : base("Failed creating the Direct3D device.") { }
        public CreatingDeviceException(Exception inner) : base("Failed creating the Direct3D device.", inner) { }
    }
    /// <summary>
    /// Resetting the device failed
    /// </summary>
    public class ResettingDeviceException : BonsaiException
    {
        public ResettingDeviceException() : base("Failed resetting the Direct3D device.") { }
        public ResettingDeviceException(Exception inner) : base("Failed resetting the Direct3D device.", inner) { }
    }
    /// <summary>
    /// Creating the device objects failed
    /// </summary>
    public class CreatingDeviceObjectsException : BonsaiException
    {
        public CreatingDeviceObjectsException() : base("Failed creating Direct3D device objects.") { }
        public CreatingDeviceObjectsException(Exception inner) : base("Failed creating Direct3D device objects.", inner) { }
    }
    /// <summary>
    /// Resetting the device failed
    /// </summary>
    public class ResettingDeviceObjectsException : BonsaiException
    {
        public ResettingDeviceObjectsException() : base("Failed resetting Direct3D device objects.") { }
        public ResettingDeviceObjectsException(Exception inner) : base("Failed resetting Direct3D device objects.", inner) { }
    }
}
