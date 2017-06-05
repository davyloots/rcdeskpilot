using System;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Bonsai.Core.Interfaces
{
    /// <summary>Interface that the framework will use to call into samples</summary>
    public interface ITransparentObject : IFrameworkCallback
    {
        Vector3 WorldPosition { get; }
    }
}
