using System;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Bonsai.Core.Interfaces
{
    /// <summary>Interface that the framework will use to call into samples</summary>
    public interface IFrameworkCallback
    {
        void OnFrameMove(Device device, double totalTime, float elapsedTime);
        void OnFrameRender(Device device, double totalTime, float elapsedTime);
    }
}
