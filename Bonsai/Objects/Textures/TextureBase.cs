using System;
using System.Collections.Generic;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Bonsai.Core;
using System.IO;

namespace Bonsai.Objects.Textures
{
    public class TextureBase : IDisposable
    {
        #region private struct GlobalTexture
        private struct GlobalTexture
        {
            public BaseTexture BaseTexture;
            public int ReferenceCount;
        }
        #endregion

        #region Protected fields
        protected string fileName = null;
        protected BaseTexture texture = null;
        protected bool transparent = false;
        protected bool lowLevel = false;
        protected bool renderTarget = false;
        protected bool cubeMap = false;
        protected string folder = null;
        protected int width = 256;
        protected int height = 256;
        private static Dictionary<string, GlobalTexture> _textures = new Dictionary<string, GlobalTexture>();
        #endregion

        #region Public properties
        /// <summary>
        /// Gets the lower level texture.
        /// </summary>
        public BaseTexture Texture
        {
            get { return texture; }
        }

        /// <summary>
        /// Gets/Sets whether the texure should be rendered with transparency.
        /// </summary>
        public bool Transparent
        {
            get { return transparent; }
            set { transparent = value; }
        }

        /// <summary>
        /// Gets the filename used to load the texture.
        /// </summary>
        public string FileName
        {
            get { return fileName; }
        }

        /// <summary>
        /// Gets the underlying Direct3D Texture object.
        /// </summary>
        public Texture Direct3DTexture
        {
            get { return texture as Texture; }
        }
        #endregion

        #region Constructor
        public TextureBase(string textureFileName)
        {
            Framework.Instance.DeviceCreated += new Bonsai.Core.EventArgs.DeviceEventHandler(Instance_DeviceCreated);
            Framework.Instance.DeviceLost += new EventHandler(Instance_DeviceLost);
            Framework.Instance.DeviceReset += new Bonsai.Core.EventArgs.DeviceEventHandler(Instance_DeviceReset);

            this.fileName = textureFileName.ToLower();
            LoadTexture(null);
        }

        public TextureBase(string textureFileName, string folder)
        {
            Framework.Instance.DeviceCreated += new Bonsai.Core.EventArgs.DeviceEventHandler(Instance_DeviceCreated);
            Framework.Instance.DeviceLost += new EventHandler(Instance_DeviceLost);
            Framework.Instance.DeviceReset += new Bonsai.Core.EventArgs.DeviceEventHandler(Instance_DeviceReset);

            this.fileName = textureFileName.ToLower();
            this.folder = folder;
            LoadTexture(folder);
        }

        public TextureBase(Texture texture)
        {
            this.texture = texture;
        }

        public TextureBase(int cubeSize, bool renderTarget)
        {
            Framework.Instance.DeviceCreated += new Bonsai.Core.EventArgs.DeviceEventHandler(Instance_DeviceCreated);
            Framework.Instance.DeviceLost += new EventHandler(Instance_DeviceLost);
            Framework.Instance.DeviceReset += new Bonsai.Core.EventArgs.DeviceEventHandler(Instance_DeviceReset);
            this.lowLevel = true;
            this.width = cubeSize;
            this.height = cubeSize;
            this.cubeMap = true;
            this.fileName = null;
            this.renderTarget = renderTarget;
            if (renderTarget)
                this.texture = new CubeTexture(Framework.Instance.Device, cubeSize, 1, Usage.Dynamic,
                    Framework.Instance.Device.PresentationParameters.BackBufferFormat, Pool.Default);
            else
                this.texture = new CubeTexture(Framework.Instance.Device, cubeSize, 1, Usage.Dynamic, Format.A8R8G8B8, Pool.Default);
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Clean up.
        /// </summary>
        public void Dispose()
        {
            // unsubscribe from events
            Framework.Instance.DeviceCreated -= new Bonsai.Core.EventArgs.DeviceEventHandler(Instance_DeviceCreated);
            Framework.Instance.DeviceLost -= new EventHandler(Instance_DeviceLost);
            Framework.Instance.DeviceReset -= new Bonsai.Core.EventArgs.DeviceEventHandler(Instance_DeviceReset);

            if ((fileName != null) && (_textures.ContainsKey(fileName)))
            {
                GlobalTexture globalTexture = _textures[fileName];
                globalTexture.ReferenceCount--;
                if (globalTexture.ReferenceCount == 0)
                {
                    globalTexture.BaseTexture.Dispose();
                    globalTexture.BaseTexture = null;
                    _textures.Remove(fileName);
                }
                else
                {
                    _textures[fileName] = globalTexture;
                }
            }
            else
            {
                if (texture != null)
                {
                    texture.Dispose();
                    texture = null;
                }
            }
        }

        public static void DisposeAll()
        {
            foreach (GlobalTexture texture in _textures.Values)
            {
                texture.BaseTexture.Dispose();                
            }
            _textures.Clear();
        }
        #endregion

        #region Private event handlers
        void Instance_DeviceReset(object sender, Bonsai.Core.EventArgs.DeviceEventArgs e)
        {
            if (lowLevel)
            {
                if (texture != null)
                {
                    texture.Dispose();
                    texture = null;
                }
                if (cubeMap)
                {
                    if (renderTarget)
                        this.texture = new CubeTexture(Framework.Instance.Device, width, 1, Usage.RenderTarget,
                            Framework.Instance.Device.PresentationParameters.BackBufferFormat, Pool.Default);
                    else
                        this.texture = new CubeTexture(Framework.Instance.Device, width, 1, Usage.Dynamic,
                            Format.A8R8G8B8, Pool.Default);
                }
                else
                {
                    if (renderTarget)
                        this.texture = new Texture(Framework.Instance.Device, width, height, 1, Usage.Dynamic, Format.A8R8G8B8, Pool.Default);
                    else
                        this.texture = new Texture(Framework.Instance.Device, width, height, 1, Usage.RenderTarget,
                            Framework.Instance.Device.PresentationParameters.BackBufferFormat, Pool.Default);
                }
            }
            else
                LoadTexture(folder);
            
        }

        void Instance_DeviceLost(object sender, EventArgs e)
        {

        }

        void Instance_DeviceCreated(object sender, Bonsai.Core.EventArgs.DeviceEventArgs e)
        {
            
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Loads the actual texture.
        /// </summary>
        protected virtual void LoadTexture(string folder)
        {
            ImageInformation info = new ImageInformation();

            try
            {
                // First look for the texture in the same folder as the input folder
                if (File.Exists(fileName))
                    info = TextureLoader.ImageInformationFromFile(fileName);
                else
                {
                    fileName = Utility.FindMediaFile(fileName, folder);
                    info = TextureLoader.ImageInformationFromFile(fileName);
                }
            }
            catch (MediaNotFoundException)
            {
                // Couldn't find it anywhere, skip it
                return;
            }
            catch 
            {
                return;
            }
            if (_textures.ContainsKey(fileName))
            {
                GlobalTexture globalTexture = _textures[fileName];
                texture = globalTexture.BaseTexture;
                globalTexture.ReferenceCount = globalTexture.ReferenceCount + 1;
                _textures[fileName] = globalTexture;
            }
            else
            {
                switch (info.ResourceType)
                {
                    case ResourceType.Textures:
                        texture = TextureLoader.FromFile(Framework.Instance.Device, fileName);
                        break;
                    case ResourceType.CubeTexture:
                        texture = TextureLoader.FromCubeFile(Framework.Instance.Device, fileName);
                        break;
                    case ResourceType.VolumeTexture:
                        texture = TextureLoader.FromVolumeFile(Framework.Instance.Device, fileName);
                        break;
                }
                if (texture != null)
                {
                    GlobalTexture globalTexture = new GlobalTexture();
                    globalTexture.BaseTexture = texture;
                    globalTexture.ReferenceCount = 1;
                    _textures.Add(fileName, globalTexture);
                }
            }
        }
        #endregion
    }
}
