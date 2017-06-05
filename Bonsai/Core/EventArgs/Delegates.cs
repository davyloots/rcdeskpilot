using System;
using System.Collections.Generic;
using System.Text;

namespace Bonsai.Core.EventArgs
{
    /// <summary>Event Handler delegate for device creation/reset</summary>
    public delegate void DeviceEventHandler(object sender, DeviceEventArgs e);
    public delegate void TimerCallback(uint eventId);
    public delegate IntPtr WndProcCallback(IntPtr hWnd, NativeMethods.WindowMessage msg, IntPtr wParam, IntPtr lParam, ref bool NoFurtherProcessing);
}
