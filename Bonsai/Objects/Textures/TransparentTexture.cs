using System;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Bonsai.Core;
using System.Drawing;

namespace Bonsai.Objects.Textures
{
    public class TransparentTexture : TextureBase
    {
        #region Protected fields
        private Color transparencyKey;
        #endregion

        #region Constructor
        public TransparentTexture(string textureFileName, Color transparencyKey) :
            base(textureFileName)
        {
            this.transparencyKey = transparencyKey;
        }
        #endregion

        protected override void LoadTexture(string folder)
        {
            ImageInformation info = new ImageInformation();

            try
            {
                // First look for the texture in the same folder as the input folder
                info = TextureLoader.ImageInformationFromFile(fileName);
            }
            catch
            {
                try
                {
                    // Couldn't find it, look in the media folder
                    fileName = Utility.FindMediaFile(fileName, folder);
                    info = TextureLoader.ImageInformationFromFile(fileName);
                }
                catch (MediaNotFoundException)
                {
                    // Couldn't find it anywhere, skip it
                    return;
                }
            }
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
        }
    }
}
