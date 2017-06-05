using System;
using System.IO;
using System.Collections;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Bonsai.Core
{
    /// <summary>Information about a cached texture</summary>
    struct CachedTexture
    {
        public string Source; // Data source
        public int Width;
        public int Height;
        public int Depth;
        public int MipLevels;
        public Usage Usage;
        public Format Format;
        public Pool Pool;
        public ResourceType Type;
    }

    /// <summary>Information about a cached effect</summary>
    struct CachedEffect
    {
        public string Source; // Data source
        public ShaderFlags Flags;
        public int InstanceId;
    }


    /// <summary>
    /// Will be a resource cache for any resources that may be required by a sample
    /// This class will be 'static'
    /// </summary>
    public class ResourceCache
    {
        #region Creation
        private ResourceCache() { } // Don't allow creation
        private static ResourceCache localObject = null;
        public static ResourceCache GetGlobalInstance()
        {
            if (localObject == null)
                localObject = new ResourceCache();

            return localObject;
        }
        #endregion

        protected Hashtable textureCache = new Hashtable(); // Cache of textures
        protected Hashtable effectCache = new Hashtable(); // Cache of effects
        protected Hashtable fontCache = new Hashtable(); // Cache of fonts

        protected static int _instanceCounter = 0; // index for instanceId's

        #region Cache Creation Methods

        /// <summary>Create a texture from a file</summary>
        public Texture CreateTextureFromFile(Device device, string filename)
        {
            return CreateTextureFromFileEx(device, filename, D3DX.Default, D3DX.Default, D3DX.Default, Usage.None,
                Format.Unknown, Pool.Managed, (Filter)D3DX.Default, (Filter)D3DX.Default, 0);
        }
        /// <summary>Create a texture from a file</summary>
        public Texture CreateTextureFromFileEx(Device device, string filename, int w, int h, int mip, Usage usage, Format fmt, Pool pool, Filter filter, Filter mipfilter, int colorkey)
        {
            // Search the cache first
            foreach (CachedTexture ct in textureCache.Keys)
            {
                if ((string.Compare(ct.Source, filename, true) == 0) &&
                    ct.Width == w &&
                    ct.Height == h &&
                    ct.MipLevels == mip &&
                    ct.Usage == usage &&
                    ct.Format == fmt &&
                    ct.Pool == pool &&
                    ct.Type == ResourceType.Textures)
                {
                    // A match was found, return that
                    return textureCache[ct] as Texture;
                }
            }

            // No matching entry, load the resource and add it to the cache
            Texture t = TextureLoader.FromFile(device, filename, w, h, mip, usage, fmt, pool, filter, mipfilter, colorkey);
            CachedTexture entry = new CachedTexture();
            entry.Source = filename;
            entry.Width = w;
            entry.Height = h;
            entry.MipLevels = mip;
            entry.Usage = usage;
            entry.Format = fmt;
            entry.Pool = pool;
            entry.Type = ResourceType.Textures;

            textureCache.Add(entry, t);

            return t;
        }
        /// <summary>Create a cube texture from a file</summary>
        public CubeTexture CreateCubeTextureFromFile(Device device, string filename)
        {
            return CreateCubeTextureFromFileEx(device, filename, D3DX.Default, D3DX.Default, Usage.None,
                Format.Unknown, Pool.Managed, (Filter)D3DX.Default, (Filter)D3DX.Default, 0);
        }
        /// <summary>Create a cube texture from a file</summary>
        public CubeTexture CreateCubeTextureFromFileEx(Device device, string filename, int size, int mip, Usage usage, Format fmt, Pool pool, Filter filter, Filter mipfilter, int colorkey)
        {
            // Search the cache first
            foreach (CachedTexture ct in textureCache.Keys)
            {
                if ((string.Compare(ct.Source, filename, true) == 0) &&
                    ct.Width == size &&
                    ct.MipLevels == mip &&
                    ct.Usage == usage &&
                    ct.Format == fmt &&
                    ct.Pool == pool &&
                    ct.Type == ResourceType.CubeTexture)
                {
                    // A match was found, return that
                    return textureCache[ct] as CubeTexture;
                }
            }

            // No matching entry, load the resource and add it to the cache
            CubeTexture t = TextureLoader.FromCubeFile(device, filename, size, mip, usage, fmt, pool, filter, mipfilter, colorkey);
            CachedTexture entry = new CachedTexture();
            entry.Source = filename;
            entry.Width = size;
            entry.MipLevels = mip;
            entry.Usage = usage;
            entry.Format = fmt;
            entry.Pool = pool;
            entry.Type = ResourceType.CubeTexture;

            textureCache.Add(entry, t);

            return t;
        }
        /// <summary>Create a volume texture from a file</summary>
        public VolumeTexture CreateVolumeTextureFromFile(Device device, string filename)
        {
            return CreateVolumeTextureFromFileEx(device, filename, D3DX.Default, D3DX.Default, D3DX.Default, D3DX.Default, Usage.None,
                Format.Unknown, Pool.Managed, (Filter)D3DX.Default, (Filter)D3DX.Default, 0);
        }
        /// <summary>Create a volume texture from a file</summary>
        public VolumeTexture CreateVolumeTextureFromFileEx(Device device, string filename, int w, int h, int d, int mip, Usage usage, Format fmt, Pool pool, Filter filter, Filter mipfilter, int colorkey)
        {
            // Search the cache first
            foreach (CachedTexture ct in textureCache.Keys)
            {
                if ((string.Compare(ct.Source, filename, true) == 0) &&
                    ct.Width == w &&
                    ct.Height == h &&
                    ct.Depth == d &&
                    ct.MipLevels == mip &&
                    ct.Usage == usage &&
                    ct.Format == fmt &&
                    ct.Pool == pool &&
                    ct.Type == ResourceType.VolumeTexture)
                {
                    // A match was found, return that
                    return textureCache[ct] as VolumeTexture;
                }
            }

            // No matching entry, load the resource and add it to the cache
            VolumeTexture t = TextureLoader.FromVolumeFile(device, filename, w, h, d, mip, usage, fmt, pool, filter, mipfilter, colorkey);
            CachedTexture entry = new CachedTexture();
            entry.Source = filename;
            entry.Width = w;
            entry.Height = h;
            entry.Depth = d;
            entry.MipLevels = mip;
            entry.Usage = usage;
            entry.Format = fmt;
            entry.Pool = pool;
            entry.Type = ResourceType.VolumeTexture;

            textureCache.Add(entry, t);

            return t;
        }

        /// <summary>Create an effect from a file</summary>
        public Effect CreateEffectFromFile(Device device, string filename, Macro[] defines, Include includeFile, 
            ShaderFlags flags, EffectPool effectPool, int instanceId, out string errors)
        {
            // No errors at first!
            errors = string.Empty;
            // Search the cache first
            foreach (CachedEffect ce in effectCache.Keys)
            {
                if ((string.Compare(ce.Source, filename, true) == 0) &&
                    (ce.Flags == flags) && (ce.InstanceId == instanceId))
                {
                    // A match was found, return that
                    return effectCache[ce] as Effect;
                }
            }

            // Nothing found in the cache
            Effect e = Effect.FromFile(device, filename, defines, includeFile, null, flags, effectPool, out errors);
            // Add this to the cache
            CachedEffect entry = new CachedEffect();
            entry.Flags = flags;
            entry.Source = filename;
            entry.InstanceId = instanceId;
            effectCache.Add(entry, e);

            //if (!string.IsNullOrEmpty(errors))
            //    System.Windows.Forms.MessageBox.Show(errors);

            // Return the new effect
            return e;
        }

        /// <summary>Create an effect from a file</summary>
        public Effect CreateEffectFromFile(Device device, string filename, Macro[] defines, Include includeFile, ShaderFlags flags, EffectPool effectPool)
        {
            return CreateEffectFromFile(device, filename, defines, includeFile, flags, effectPool, 0);
        }

        /// <summary>Create an effect from a file</summary>
        public Effect CreateEffectFromFile(Device device, string filename, Macro[] defines, Include includeFile, ShaderFlags flags, EffectPool effectPool, int instanceId)
        {
            string temp; return CreateEffectFromFile(device, filename, defines, includeFile, flags, effectPool, instanceId, out temp);
        }

        public void RemoveEffect(string filename)
        {
            // Search the cache first
            foreach (CachedEffect ce in effectCache.Keys)
            {
                if (string.Compare(ce.Source, filename, true) == 0) 
                {
                    // A match was found, return that
                    effectCache.Remove(ce);
                    return;
                }
            }
        }

        public void RemoveEffect(string filename, int instanceId)
        {
            // Search the cache first
            foreach (CachedEffect ce in effectCache.Keys)
            {
                if ((string.Compare(ce.Source, filename, true) == 0) && (ce.InstanceId == instanceId))
                {
                    // A match was found, return that
                    effectCache.Remove(ce);
                    return;
                }
            }
        }

        /// <summary>Create a font object</summary>
        public Font CreateFont(Device device, int height, int width, FontWeight weight, int mip, bool italic,
            CharacterSet charSet, Precision outputPrecision, FontQuality quality, PitchAndFamily pandf, string fontName)
        {
            // Create the font description
            FontDescription desc = new FontDescription();
            desc.Height = height;
            desc.Width = width;
            desc.Weight = weight;
            desc.MipLevels = mip;
            desc.IsItalic = italic;
            desc.CharSet = charSet;
            desc.OutputPrecision = outputPrecision;
            desc.Quality = quality;
            desc.PitchAndFamily = pandf;
            desc.FaceName = fontName;

            // return the font
            return CreateFont(device, desc);
        }
        /// <summary>Create a font object</summary>
        public Font CreateFont(Device device, FontDescription desc)
        {
            // Search the cache first
            foreach (FontDescription fd in fontCache.Keys)
            {
                if ((string.Compare(fd.FaceName, desc.FaceName, true) == 0) &&
                    fd.CharSet == desc.CharSet &&
                    fd.Height == desc.Height &&
                    fd.IsItalic == desc.IsItalic &&
                    fd.MipLevels == desc.MipLevels &&
                    fd.OutputPrecision == desc.OutputPrecision &&
                    fd.PitchAndFamily == desc.PitchAndFamily &&
                    fd.Quality == desc.Quality &&
                    fd.Weight == desc.Weight &&
                    fd.Width == desc.Width)
                {
                    // A match was found, return that
                    return fontCache[fd] as Font;
                }
            }

            // Couldn't find anything in the cache, create one
            Font f = new Font(device, desc);
            // Create a new entry
            fontCache.Add(desc, f);

            // return the new font
            return f;
        }

        #endregion

        #region Device event callbacks
        /// <summary>
        /// Called when the device is created
        /// </summary>
        public void OnCreateDevice(Device device) { } // Nothing to do on device create
        /// <summary>
        /// Called when the device is reset
        /// </summary>
        public void OnResetDevice(Device device)
        {
            // Call OnResetDevice on all effect and font objects
            foreach (Font f in fontCache.Values)
                f.OnResetDevice();
            foreach (Effect e in effectCache.Values)
                e.OnResetDevice();
        }
        /// <summary>
        /// Clear any resources that need to be lost
        /// </summary>
        public void OnLostDevice()
        {
            foreach (Font f in fontCache.Values)
                f.OnLostDevice();
            foreach (Effect e in effectCache.Values)
                e.OnLostDevice();

            // Search the texture cache 
            foreach (CachedTexture ct in textureCache.Keys)
            {
                if (ct.Pool == Pool.Default)
                {
                    // A match was found, get rid of it
                    switch (ct.Type)
                    {
                        case ResourceType.Textures:
                            (textureCache[ct] as Texture).Dispose(); break;
                        case ResourceType.CubeTexture:
                            (textureCache[ct] as CubeTexture).Dispose(); break;
                        case ResourceType.VolumeTexture:
                            (textureCache[ct] as VolumeTexture).Dispose(); break;
                    }
                }
            }
        }
        /// <summary>
        /// Destroy any resources and clear the caches
        /// </summary>
        public void OnDestroyDevice()
        {
            // Cleanup the fonts
            foreach (Font f in fontCache.Values)
                f.Dispose();

            // Cleanup the effects
            foreach (Effect e in effectCache.Values)
                e.Dispose();

            // Dispose of any items in the caches
            foreach (BaseTexture texture in textureCache.Values)
            {
                if (texture != null)
                    texture.Dispose();
            }

            // Clear all of the caches
            textureCache.Clear();
            fontCache.Clear();
            effectCache.Clear();
        }

        #endregion
    }
}
