using System;
using System.Collections.Generic;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Bonsai.Core;
using Bonsai.Objects.Textures;

namespace Bonsai.Objects.Shaders
{
    public class ShaderBase : IDisposable
    {
        #region Protected fields
        protected Effect effect = null;
        protected Dictionary<EffectHandle, ShaderParameters> variables = new Dictionary<EffectHandle, ShaderParameters>();
        protected string name;
        protected string fileName;
        protected Matrix worldMatrix = Matrix.Identity;
        protected Matrix transformMatrix = Matrix.Identity;
        protected Plane reflectionPlane;
        protected int instanceId = 0;

        protected static int instanceIndex = 1;
        #endregion

        #region Public enumerations
        public enum ShaderParameters
        {
            CameraProjection,
            World,
            WorldInvertTranspose,
            WorldProjection,
            CameraPosition,
            CameraReflectionPosition,
            View,
            Projection,
            Reflection,
            ViewProjection            
        }
        #endregion

        #region Public properties
        /// <summary>
        /// Gets the internal effect object.
        /// </summary>
        public Effect Effect
        {
            get { return effect; }
        }

        /// <summary>
        /// Gets/Sets the WorldMatrix.
        /// </summary>
        public Matrix WorldMatrix
        {
            get { return worldMatrix; }
            set { worldMatrix = value; }
        }

        /// <summary>
        /// Gets/Sets the TransformMatrix.
        /// </summary>
        public Matrix TransformMatrix
        {
            get { return transformMatrix; }
            set { transformMatrix = value; }
        }

        /// <summary>
        /// Gets/Sets the reflection plane.
        /// </summary>
        public Plane ReflectionPlane
        {
            get { return reflectionPlane; }
            set { reflectionPlane = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fileName"></param>
        public ShaderBase(string name, string fileName)
        {
            this.name = name;
            this.fileName = fileName;
            LoadEffect(0);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fileName"></param>
        public ShaderBase(string name, string fileName, bool reuse)
        {
            this.name = name;
            this.fileName = fileName;
            if (reuse)
                LoadEffect(0);
            else
                LoadEffect(instanceIndex++);
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Clean up.
        /// </summary>
        public void Dispose()
        {
            ResourceCache.GetGlobalInstance().RemoveEffect(fileName, instanceId);
            if (effect != null)
                effect.Dispose();
        }
        #endregion

        #region Public methods
        public void SetVariable(string variableName, string value)
        {
            if (effect != null)
                effect.SetValue(variableName, value);
        }

        public void SetVariable(EffectHandle variableHandle, string value)
        {
            if (effect != null)
                effect.SetValue(variableHandle, value);
        }

        public void SetVariable(string variableName, ColorValue value)
        {
            if (effect != null)
                effect.SetValue(variableName, value);
        }

        public void SetVariable(EffectHandle variableHandle, ColorValue value)
        {
            if (effect != null)
                effect.SetValue(variableHandle, value);
        }

        public void SetVariable(string variableName, TextureBase value)
        {
            if (effect != null)
                effect.SetValue(variableName, value.Texture);
        }

        public void SetVariable(EffectHandle variableHandle, TextureBase value)
        {
            if (effect != null)
                effect.SetValue(variableHandle, value.Texture);
        }

        public void SetVariable(EffectHandle variableHandle, BaseTexture value)
        {
            if (effect != null)
                effect.SetValue(variableHandle, value);
        }

        public void SetVariable(string variableName, Vector3 value)
        {
            if (effect != null)
                effect.SetValue(variableName, new Vector4(value.X, value.Y, value.Z, 1f));
        }

        public void SetVariable(EffectHandle variableHandle, Vector3 value)
        {
            if (effect != null)
                effect.SetValue(variableHandle, new Vector4(value.X, value.Y, value.Z, 1f));
        }

        public void SetVariable(string variableName, Vector4 value)
        {
            if (effect != null)
                effect.SetValue(variableName, value);
        }

        public void SetVariable(EffectHandle variableHandle, Vector4 value)
        {
            if (effect != null)
                effect.SetValue(variableHandle, value);
        }

        public void SetVariable(string variableName, Matrix value)
        {
            if (effect != null)
                effect.SetValue(variableName, value);
        }

        public void SetVariable(EffectHandle variableHandle, Matrix value)
        {
            if (effect != null)
                effect.SetValue(variableHandle, value);
        }

        public void SetVariable(string variableName, float value)
        {
            if (effect != null)
                effect.SetValue(variableName, value);
        }

        public void SetVariable(EffectHandle variableHandle, float value)
        {
            if (effect != null)
                effect.SetValue(variableHandle, value);
        }

        public void SetVariable(string variableName, int value)
        {
            if (effect != null)
                effect.SetValue(variableName, value);
        }

        public void SetVariable(EffectHandle variableHandle, int value)
        {
            if (effect != null)
                effect.SetValue(variableHandle, value);
        }

        public void SetVariable(string variableName, ShaderParameters parameter)
        {
            EffectHandle effectHandle = EffectHandle.FromString(variableName);
            if (variables.ContainsKey(effectHandle))
                variables[effectHandle] = parameter;
            else
                variables.Add(effectHandle, parameter);
        }

        public void SetTechnique(string techniqueName)
        {
            if (effect != null)
            {
                EffectHandle techniqueHandle = effect.GetTechnique(techniqueName);
                if (techniqueHandle != null)
                {
                    effect.Technique = techniqueHandle;
                }
            }
        }
        
        public void SetGlobalParameters()
        {
            if (effect != null)
            {
                foreach (KeyValuePair<EffectHandle, ShaderParameters> variable in variables)
                {
                    switch (variable.Value)
                    {
                        case ShaderParameters.CameraPosition:
                            SetVariable(variable.Key, Framework.Instance.CurrentCamera.LookFrom);
                            break;
                        case ShaderParameters.CameraReflectionPosition:
                            SetVariable(variable.Key, new Vector3(Framework.Instance.CurrentCamera.LookFrom.X, -Framework.Instance.CurrentCamera.LookFrom.Y, Framework.Instance.CurrentCamera.LookFrom.Z));
                            break;
                        case ShaderParameters.CameraProjection:
                            SetVariable(variable.Key, Framework.Instance.CurrentCamera.ViewMatrix*Framework.Instance.CurrentCamera.ProjectionMatrix);
                            break;
                        case ShaderParameters.World:
                            SetVariable(variable.Key, worldMatrix);
                            break;
                        case ShaderParameters.WorldInvertTranspose:
                            Matrix invWorld = worldMatrix;
                            invWorld.Invert();
                            invWorld.Transpose(invWorld);
                            SetVariable(variable.Key, invWorld);
                            break;
                        
                        case ShaderParameters.WorldProjection:
                            SetVariable(variable.Key, worldMatrix * Framework.Instance.CurrentCamera.ViewMatrix * Framework.Instance.CurrentCamera.ProjectionMatrix);
                            break;                        
                        case ShaderParameters.View:
                            SetVariable(variable.Key, Framework.Instance.CurrentCamera.ViewMatrix);
                            break;
                        case ShaderParameters.Projection:
                            SetVariable(variable.Key, Framework.Instance.CurrentCamera.ProjectionMatrix);
                            break;
                        case ShaderParameters.ViewProjection:
                            SetVariable(variable.Key, Framework.Instance.CurrentCamera.ViewMatrix * Framework.Instance.CurrentCamera.ProjectionMatrix);
                            break;
                        case ShaderParameters.Reflection:
                            SetVariable(variable.Key, Framework.Instance.CurrentCamera.ReflectionMatrix);
                            break;
                    }
                }
            }
        }
        #endregion

        #region Internal methods
        #endregion

        #region Protected methods
        protected void LoadEffect(int instanceId)
        {
            try
            {
                this.instanceId = instanceId;                
                fileName = Utility.FindMediaFile(fileName);
                effect = ResourceCache.GetGlobalInstance().CreateEffectFromFile(
                    Framework.Instance.Device, fileName, null, null, ShaderFlags.NotCloneable, null, instanceId);

                //using (GraphicsStream code = ShaderLoader.CompileShaderFromFile(fileName, 
            }
            catch (MediaNotFoundException)
            {
                // Couldn't find it anywhere, skip it
                return;
            }
        }
        #endregion

        
    }
}
