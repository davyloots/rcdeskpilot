using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Objects;
using Microsoft.DirectX;
using Bonsai.Objects.Meshes;

using RCSim.DataClasses;
using Microsoft.DirectX.Direct3D;
using RCSim.Interfaces;
using Bonsai.Core;
using System.IO;
using Bonsai.Objects.Textures;
using Bonsai.Objects.Shaders;
using RCSim.Effects;

namespace RCSim
{
    internal class ControlSurface : GameObject, IDisposable
    {
        #region Private fields
        private AircraftParameters.ControlSurface surfaceDefinition;
        private IAirplaneControl airplaneControl;
        private ShaderBase reflectionShader = null;
        #endregion

        #region Public properties
        /// <summary>
        /// Gets a reference to the AirplaneControl.
        /// </summary>
        public IAirplaneControl AirplaneControl
        {
            get { return airplaneControl; }
        }

        public AircraftParameters.ChannelEnum Channel
        {
            get { return surfaceDefinition.Channel; }
            set { surfaceDefinition.Channel = value; }
        }

        public AircraftParameters.ControlSurfaceTypeEnum ControlSurfaceType
        {
            get { return surfaceDefinition.Type; }
            set
            {
                if (surfaceDefinition.Type == AircraftParameters.ControlSurfaceTypeEnum.PropHighRPM ||
                    surfaceDefinition.Type == AircraftParameters.ControlSurfaceTypeEnum.RotorHighRPM ||
                    surfaceDefinition.Type == AircraftParameters.ControlSurfaceTypeEnum.TailrotorHighRPM)
                {
                    Program.Instance.TransparentObjectManager.Objects.Remove(this);
                }
                surfaceDefinition.Type = value;
                if (surfaceDefinition.Type == AircraftParameters.ControlSurfaceTypeEnum.PropHighRPM ||
                    surfaceDefinition.Type == AircraftParameters.ControlSurfaceTypeEnum.RotorHighRPM ||
                    surfaceDefinition.Type == AircraftParameters.ControlSurfaceTypeEnum.TailrotorHighRPM)
                {
                    Program.Instance.TransparentObjectManager.Objects.Add(this);
                    this.Visible = false;
                }
                if (surfaceDefinition.Type == AircraftParameters.ControlSurfaceTypeEnum.Reflective &&
                    Effects.Reflection.Enabled)
                {
                    InitializeReflectionShader();
                    this.Shader = reflectionShader;
                }
                else
                    this.Shader = null;
            }
        }

        /// <summary>
        /// Gets a reference to the defining definition.
        /// </summary>
        public AircraftParameters.ControlSurface SurfaceDefinition
        {
            get { return surfaceDefinition; }
        }

        public bool Reversed
        {
            get { return surfaceDefinition.Reversed; }
            set { surfaceDefinition.Reversed = value; }
        }

        /// <summary>
        /// Gets/Sets the file to use for the mesh.
        /// </summary>
        public string MeshFileName
        {
            get { return surfaceDefinition.Filename; }
            set
            {
                surfaceDefinition.Filename = new FileInfo(value).Name;
                SetMeshFile(value);
            }
        }

        /// <summary>
        /// Gets/Sets the minimum rotation angle.
        /// </summary>
        public float MinimumAngle
        {
            get { return (float)surfaceDefinition.MinimumAngle; }
            set { surfaceDefinition.MinimumAngle = value; }
        }

        /// <summary>
        /// Gets/Sets the maximum rotation angle.
        /// </summary>
        public float MaximumAngle
        {
            get { return (float)surfaceDefinition.MaximumAngle; }
            set { surfaceDefinition.MaximumAngle = value; }
        }

        /// <summary>
        /// Gets/Sets the default angle of the controlsurface.
        /// </summary>
        public float ZeroAngle
        {
            get { return (float)surfaceDefinition.ZeroAngle; }
            set { surfaceDefinition.ZeroAngle = value; }
        }

        /// <summary>
        /// Gets/Sets the scale in all directions.
        /// </summary>
        public float SingleScale
        {
            get { return surfaceDefinition.Scale; }
            set
            {
                surfaceDefinition.Scale = value;
                this.Scale = new Vector3(value, value, value);
            }
        }
        #endregion

        #region Constructor
        public ControlSurface(AircraftParameters.ControlSurface surfaceDefinition, IAirplaneControl airplaneControl)
        {
            this.surfaceDefinition = surfaceDefinition;
            this.airplaneControl = airplaneControl;
            if (!string.IsNullOrEmpty(surfaceDefinition.Filename))
                this.Mesh = new XMesh(surfaceDefinition.Filename, airplaneControl.AircraftParameters.FolderName);  
            this.Name = surfaceDefinition.Filename;
            this.Position = surfaceDefinition.Position;
            this.RotationAxis = surfaceDefinition.RotationAxis;
            if (surfaceDefinition.Scale != 1)
                this.Scale = new Vector3(surfaceDefinition.Scale, surfaceDefinition.Scale, surfaceDefinition.Scale);
            if (surfaceDefinition.Type == AircraftParameters.ControlSurfaceTypeEnum.PropHighRPM ||
                surfaceDefinition.Type == AircraftParameters.ControlSurfaceTypeEnum.RotorHighRPM ||
                surfaceDefinition.Type == AircraftParameters.ControlSurfaceTypeEnum.TailrotorHighRPM)
            {
                Program.Instance.TransparentObjectManager.Objects.Add(this);
                this.Visible = false;
            }
            if (surfaceDefinition.Type == AircraftParameters.ControlSurfaceTypeEnum.Reflective &&
                Effects.Reflection.Enabled)
            {
                InitializeReflectionShader();
                this.Shader = reflectionShader;
            }
            else
                this.Shader = null;
            if (surfaceDefinition.ChildControlSurfaces != null)
            {
                foreach (AircraftParameters.ControlSurface childDef in surfaceDefinition.ChildControlSurfaces)
                {
                    ControlSurface childSurface = new ControlSurface(childDef, airplaneControl);
                    if (childSurface != null)
                        this.AddChild(childSurface);
                }
            }
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Clean up.
        /// </summary>
        public override void Dispose()
        {
            if (surfaceDefinition.Type == AircraftParameters.ControlSurfaceTypeEnum.PropHighRPM ||
                surfaceDefinition.Type == AircraftParameters.ControlSurfaceTypeEnum.RotorHighRPM ||
                surfaceDefinition.Type == AircraftParameters.ControlSurfaceTypeEnum.TailrotorHighRPM)
            {
                Program.Instance.TransparentObjectManager.Objects.Remove(this);
            }

            if (Mesh != null)
            {
                Mesh.Dispose();
                Mesh = null;
            }

            if (reflectionShader != null)
            {
                reflectionShader.Dispose();
                reflectionShader = null;
            }
        }
        #endregion

        #region Protected methods
        protected void InitializeReflectionShader()
        {
            if (reflectionShader == null)
            {
                reflectionShader = new ShaderBase("reflectionshader", @"data\reflection.fx", false);
                //TextureBase skyBox = new TextureBase(@"data\cubemap.dds");
                                
                if (Reflection.Instance.CubeMap != null)
                    reflectionShader.SetVariable("SkyboxTexture", Reflection.Instance.CubeMap);
                reflectionShader.SetVariable("specular", 0.9f);
                reflectionShader.SetVariable("matWorldViewProj", ShaderBase.ShaderParameters.WorldProjection);
                reflectionShader.SetVariable("matWorld", ShaderBase.ShaderParameters.World);
                Vector3 sunVector = new Vector3(0.3f, 0.9f, 0.4f);
                sunVector.Normalize();
                reflectionShader.SetVariable("vecLightDir", sunVector);
                Vector3 sunColor = new Vector3(1.0f, 1.0f, 1.0f);
                sunColor.Normalize();
                reflectionShader.SetVariable("sunColor", sunColor);
                reflectionShader.SetVariable("light", 1.3f);
                reflectionShader.SetVariable("cameraPosition", ShaderBase.ShaderParameters.CameraPosition);
            }
        }

        protected void SetMeshFile(string filename)
        {
            if (Mesh != null)
            {
                Mesh.Dispose();
                Mesh = null;
            }
            Mesh = new XMesh(filename, airplaneControl.AircraftParameters.FolderName);
            this.Name = Utility.GetFileNamePart(filename);
        }
        #endregion

        #region Public methods
        public override void OnFrameMove(Device device, double totalTime, float elapsedTime)
        {
            if (surfaceDefinition.Type == AircraftParameters.ControlSurfaceTypeEnum.PropHighRPM ||
                surfaceDefinition.Type == AircraftParameters.ControlSurfaceTypeEnum.RotorHighRPM ||
                surfaceDefinition.Type == AircraftParameters.ControlSurfaceTypeEnum.TailrotorHighRPM)
            {
                if (Program.Instance.TransparentObjectManager.Rendering == false)
                    return;
                if (surfaceDefinition.Type == AircraftParameters.ControlSurfaceTypeEnum.PropHighRPM)
                {
                    if (airplaneControl.Throttle < 0.1)
                        this.Visible = false;
                    else
                        this.Visible = true;
                }
                else
                    this.Visible = true;
            }

            double controlInput = 0;
            switch (surfaceDefinition.Channel)
            {
                case AircraftParameters.ChannelEnum.None:
                    break;
                case AircraftParameters.ChannelEnum.Elevator:                    
                    controlInput = airplaneControl.Elevator * (surfaceDefinition.Reversed ? -1 : 1);
                    break;
                case AircraftParameters.ChannelEnum.Rudder:
                    controlInput = airplaneControl.Rudder * (surfaceDefinition.Reversed ? -1 : 1);
                    break;
                case AircraftParameters.ChannelEnum.Aileron:
                    controlInput = airplaneControl.Ailerons * (surfaceDefinition.Reversed ? -1 : 1);
                    break;
                case AircraftParameters.ChannelEnum.Throttle:
                    controlInput = airplaneControl.Throttle * (surfaceDefinition.Reversed ? -1 : 1);
                    break;
                case AircraftParameters.ChannelEnum.Flaps:
                    controlInput = airplaneControl.Flaps * (surfaceDefinition.Reversed ? -1 : 1);
                    break;
                case AircraftParameters.ChannelEnum.Gear:
                    controlInput = airplaneControl.Gear * (surfaceDefinition.Reversed ? -1 : 1);
                    break;
            }
            if (surfaceDefinition.Type == AircraftParameters.ControlSurfaceTypeEnum.Normal ||
                surfaceDefinition.Type == AircraftParameters.ControlSurfaceTypeEnum.Reflective)
            {
                if (controlInput < 0)
                    this.RotationAngle = (float)(surfaceDefinition.ZeroAngle - controlInput * (surfaceDefinition.MinimumAngle - surfaceDefinition.ZeroAngle));
                else
                    this.RotationAngle = (float)(surfaceDefinition.ZeroAngle + controlInput * (surfaceDefinition.MaximumAngle - surfaceDefinition.ZeroAngle));
            }
            else if (surfaceDefinition.Type == AircraftParameters.ControlSurfaceTypeEnum.PropHighRPM)
                this.RotationAngle += (8f * elapsedTime * (float)(controlInput - 0.5));
            else if ((surfaceDefinition.Type == AircraftParameters.ControlSurfaceTypeEnum.PropFoldingLowRPM) ||
                     (surfaceDefinition.Type == AircraftParameters.ControlSurfaceTypeEnum.PropLowRPM))
            {
                if (airplaneControl.AircraftParameters.EngineMinFrequency > 0)
                    this.RotationAngle += 1000f * elapsedTime * (float)(controlInput + 0.13);
                else
                    this.RotationAngle += 1000f * elapsedTime * (float)(controlInput);
            }
            else if (surfaceDefinition.Type == AircraftParameters.ControlSurfaceTypeEnum.RotorHighRPM)
            {
                this.RotateXAngle = (float)airplaneControl.Elevator * 0.05f;
                this.RotateZAngle = (float)airplaneControl.Ailerons * 0.05f;
                this.RotateYAngle += elapsedTime * (800 - airplaneControl.RotorRPM) / 100; 
                this.OrientationMode = OrientationModeEnum.YXZ;
            }
            else if (surfaceDefinition.Type == AircraftParameters.ControlSurfaceTypeEnum.RotorLowRPM)
            {
                this.RotationAngle += elapsedTime*airplaneControl.RotorRPM;
            }
            else if (surfaceDefinition.Type == AircraftParameters.ControlSurfaceTypeEnum.TailrotorHighRPM)
            {
                this.RotationAngle += 2 * elapsedTime;
            }
            else if (surfaceDefinition.Type == AircraftParameters.ControlSurfaceTypeEnum.TailrotorLowRPM)
            {
                this.RotationAngle += 2 * elapsedTime * airplaneControl.RotorRPM;
            }
            else
                this.RotationAngle = 0f;

            if ((reflectionShader != null) && (Reflection.Instance.CubeMap != null))
                reflectionShader.SetVariable("SkyboxTexture", Reflection.Instance.CubeMap);
            
            base.OnFrameMove(device, totalTime, elapsedTime);
        }

        public override void OnFrameRender(Device device, double totalTime, float elapsedTime)
        {
            if (surfaceDefinition.Type == AircraftParameters.ControlSurfaceTypeEnum.PropHighRPM ||
                surfaceDefinition.Type == AircraftParameters.ControlSurfaceTypeEnum.RotorHighRPM ||
                surfaceDefinition.Type == AircraftParameters.ControlSurfaceTypeEnum.TailrotorHighRPM)
            {
                if ((Program.Instance.TransparentObjectManager.Rendering == false) ||
                    (Parent.Visible == false))
                    return;
                if (surfaceDefinition.Type == AircraftParameters.ControlSurfaceTypeEnum.RotorHighRPM)
                {
                    if (airplaneControl.RotorRPM < 100)
                        return;
                }
                else if (surfaceDefinition.Type == AircraftParameters.ControlSurfaceTypeEnum.TailrotorHighRPM)
                {
                    if (airplaneControl.RotorRPM < 50)
                        return;
                }                
            }
            else if (surfaceDefinition.Type == AircraftParameters.ControlSurfaceTypeEnum.PropLowRPM)
            {
                if (airplaneControl.Throttle >= 0.1)
                    return;
            }
            else if (surfaceDefinition.Type == AircraftParameters.ControlSurfaceTypeEnum.PropFoldingLowRPM)
            {
                if ((airplaneControl.Throttle >= 0.1) || (airplaneControl.Throttle < 0.01))
                    return;
            }
            else if (surfaceDefinition.Type == AircraftParameters.ControlSurfaceTypeEnum.PropFolded)
            {
                if (airplaneControl.Throttle >= 0.01)
                    return;
            }
            else if (surfaceDefinition.Type == AircraftParameters.ControlSurfaceTypeEnum.RotorLowRPM)
            {
                if (airplaneControl.RotorRPM >= 100)
                    return;
            }
            else if (surfaceDefinition.Type == AircraftParameters.ControlSurfaceTypeEnum.TailrotorLowRPM)
            {
                if (airplaneControl.RotorRPM >= 50)
                    return;
            }

            base.OnFrameRender(device, totalTime, elapsedTime);
        }

        public override void OnRenderShadow(Microsoft.DirectX.Direct3D.Device device, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 lightDir)
        {
            if (surfaceDefinition.Type == AircraftParameters.ControlSurfaceTypeEnum.PropHighRPM || 
                surfaceDefinition.Type == AircraftParameters.ControlSurfaceTypeEnum.RotorHighRPM ||
                surfaceDefinition.Type == AircraftParameters.ControlSurfaceTypeEnum.TailrotorHighRPM)
            {
                return;
            }
            else if (surfaceDefinition.Type == AircraftParameters.ControlSurfaceTypeEnum.PropLowRPM)
            {
                if (airplaneControl.Throttle >= 0.1)
                    return;
            }
            else if (surfaceDefinition.Type == AircraftParameters.ControlSurfaceTypeEnum.PropFoldingLowRPM)
            {
                if ((airplaneControl.Throttle >= 0.1) || (airplaneControl.Throttle < 0.01))
                    return;
            }
            else if (surfaceDefinition.Type == AircraftParameters.ControlSurfaceTypeEnum.PropFolded)
            {
                if (airplaneControl.Throttle >= 0.01)
                    return;
            }
            else if (surfaceDefinition.Type == AircraftParameters.ControlSurfaceTypeEnum.RotorLowRPM)
            {
                if (airplaneControl.RotorRPM >= 100)
                    return;
            }
            else if (surfaceDefinition.Type == AircraftParameters.ControlSurfaceTypeEnum.TailrotorLowRPM)
            {
                if (airplaneControl.RotorRPM >= 50)
                    return;
            }
            base.OnRenderShadow(device, p1, p2, p3, lightDir);
        }
        #endregion
    }
}
