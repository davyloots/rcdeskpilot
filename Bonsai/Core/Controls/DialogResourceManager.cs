using System;
using System.Collections;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Bonsai.Core.Controls
{
    /// <summary>
    /// Structure for shared textures
    /// </summary>
    public class TextureNode
    {
        public string Filename;
        public Texture Texture;
        public uint Width;
        public uint Height;
    }

    /// <summary>
    /// Structure for shared fonts
    /// </summary>
    public class FontNode
    {
        public string FaceName;
        public Font Font;
        public uint Height;
        public FontWeight Weight;
    }

    /// <summary>
    /// Manages shared resources of dialogs
    /// </summary>
    public sealed class DialogResourceManager
    {
        private StateBlock dialogStateBlock;  // Stateblock shared amongst all dialogs
        private Sprite dialogSprite; // Sprite used for drawing
        public StateBlock StateBlock { get { return dialogStateBlock; } }
        public Sprite Sprite { get { return dialogSprite; } }
        private Device device; // Device

        // Lists of textures/fonts
        private ArrayList textureCache = new ArrayList();
        private ArrayList fontCache = new ArrayList();

        #region Creation
        /// <summary>Do not allow creation</summary>
        private DialogResourceManager()
        {
            device = null;
            dialogSprite = null;
            dialogStateBlock = null;
        }

        private static DialogResourceManager localObject = null;
        public static DialogResourceManager GetGlobalInstance()
        {
            if (localObject == null)
                localObject = new DialogResourceManager();

            return localObject;
        }
        #endregion

        /// <summary>Gets a font node from the cache</summary>
        public FontNode GetFontNode(int index) { return fontCache[index] as FontNode; }
        /// <summary>Gets a texture node from the cache</summary>
        public TextureNode GetTextureNode(int index) { return textureCache[index] as TextureNode; }
        /// <summary>Gets the device</summary>
        public Device Device { get { return device; } }

        /// <summary>
        /// Adds a font to the resource manager
        /// </summary>
        public int AddFont(string faceName, uint height, FontWeight weight)
        {
            // See if this font exists
            for (int i = 0; i < fontCache.Count; i++)
            {
                FontNode fn = fontCache[i] as FontNode;
                if ((string.Compare(fn.FaceName, faceName, true) == 0) &&
                    fn.Height == height &&
                    fn.Weight == weight)
                {
                    // Found it
                    return i;
                }
            }

            // Doesn't exist, add a new one and try to create it
            FontNode newNode = new FontNode();
            newNode.FaceName = faceName;
            newNode.Height = height;
            newNode.Weight = weight;
            fontCache.Add(newNode);

            int fontIndex = fontCache.Count - 1;
            // If a device is available, try to create immediately
            if (device != null)
                CreateFont(fontIndex);

            return fontIndex;
        }
        /// <summary>
        /// Adds a texture to the resource manager
        /// </summary>
        public int AddTexture(string filename)
        {
            // See if this font exists
            for (int i = 0; i < textureCache.Count; i++)
            {
                TextureNode tn = textureCache[i] as TextureNode;
                if (string.Compare(tn.Filename, filename, true) == 0)
                {
                    // Found it
                    return i;
                }
            }
            // Doesn't exist, add a new one and try to create it
            TextureNode newNode = new TextureNode();
            newNode.Filename = filename;
            textureCache.Add(newNode);

            int texIndex = textureCache.Count - 1;

            // If a device is available, try to create immediately
            if (device != null)
                CreateTexture(texIndex);

            return texIndex;

        }

        /// <summary>
        /// Creates a font
        /// </summary>
        public void CreateFont(int font)
        {
            // Get the font node here
            FontNode fn = GetFontNode(font);
            if (fn.Font != null)
                fn.Font.Dispose(); // Get rid of this

            // Create the new font
            fn.Font = new Font(device, (int)fn.Height, 0, fn.Weight, 1, false, CharacterSet.Default,
                Precision.Default, FontQuality.Default, PitchAndFamily.DefaultPitch | PitchAndFamily.FamilyDoNotCare,
                fn.FaceName);
        }

        /// <summary>
        /// Creates a texture
        /// </summary>
        public void CreateTexture(int tex)
        {
            // Get the texture node here
            TextureNode tn = GetTextureNode(tex);

            // Make sure there's a texture to create
            if ((tn.Filename == null) || (tn.Filename.Length == 0))
                return;

            // Find the texture
            string path = Utility.FindMediaFile(tn.Filename);

            // Create the new texture
            ImageInformation info = new ImageInformation();
            tn.Texture = TextureLoader.FromFile(device, path, D3DX.Default, D3DX.Default, D3DX.Default, Usage.None,
                Format.Unknown, Pool.Managed, (Filter)D3DX.Default, (Filter)D3DX.Default, 0, ref info);

            // Store dimensions
            tn.Width = (uint)info.Width;
            tn.Height = (uint)info.Height;

        }

        #region Device event callbacks
        /// <summary>
        /// Called when the device is created
        /// </summary>
        public void OnCreateDevice(Device d)
        {
            // Store device
            device = d;

            // create fonts and textures
            for (int i = 0; i < fontCache.Count; i++)
                CreateFont(i);

            for (int i = 0; i < textureCache.Count; i++)
                CreateTexture(i);

            dialogSprite = new Sprite(d); // Create the sprite
        }
        /// <summary>
        /// Called when the device is reset
        /// </summary>
        public void OnResetDevice(Device device)
        {
            foreach (FontNode fn in fontCache)
                fn.Font.OnResetDevice();

            if (dialogSprite != null)
                dialogSprite.OnResetDevice();

            // Create new state block
            dialogStateBlock = new StateBlock(device, StateBlockType.All);
        }

        /// <summary>
        /// Clear any resources that need to be lost
        /// </summary>
        public void OnLostDevice()
        {
            foreach (FontNode fn in fontCache)
            {
                if ((fn.Font != null) && (!fn.Font.Disposed))
                    fn.Font.OnLostDevice();
            }

            if (dialogSprite != null)
                dialogSprite.OnLostDevice();

            if (dialogStateBlock != null)
            {
                dialogStateBlock.Dispose();
                dialogStateBlock = null;
            }
        }

        /// <summary>
        /// Destroy any resources and clear the caches
        /// </summary>
        public void OnDestroyDevice()
        {
            foreach (FontNode fn in fontCache)
            {
                if (fn.Font != null)
                    fn.Font.Dispose();
            }

            foreach (TextureNode tn in textureCache)
            {
                if (tn.Texture != null)
                    tn.Texture.Dispose();
            }

            if (dialogSprite != null)
            {
                dialogSprite.Dispose();
                dialogSprite = null;
            }

            if (dialogStateBlock != null)
            {
                dialogStateBlock.Dispose();
                dialogStateBlock = null;
            }
        }

        #endregion
    }
}
