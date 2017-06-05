using System;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Bonsai.Core;
using Bonsai.Objects.Textures;

namespace Bonsai.Objects.Meshes
{
    /// <summary>Class for loading and rendering file-based meshes</summary>
    public sealed class XMesh : MeshBase, IDisposable
    {
        #region Instance Data
        private string meshFileName;
        private string meshFolder = null;
        private Mesh systemMemoryMesh = null; // System Memory mesh, lives through a resize
        private Mesh localMemoryMesh = null; // Local mesh, rebuilt on resize
        private Device device = null;

        private Material[] meshMaterials = null; // Materials for the mesh
        private TextureBase[] meshTextures = null; // Textures for the mesh
        private bool isUsingMeshMaterials = true; // Should the mesh be rendered with the materials

        private Vector3 boundingBoxMin;
        private Vector3 boundingBoxMax;

        private static Material defaultMaterial;
        private static Material shadowMaterial;

        /// <summary>Returns the system memory mesh</summary>
        public Mesh SystemMesh { get { return systemMemoryMesh; } }
        /// <summary>Returns the local memory mesh</summary>
        public Mesh LocalMesh { get { return localMemoryMesh; } }
        /// <summary>Should the mesh be rendered with materials</summary>
        public bool IsUsingMaterials{ get { return isUsingMeshMaterials; } set { isUsingMeshMaterials = value; } }
        /// <summary>Number of materials in mesh</summary>
        public int NumberMaterials { get { return meshMaterials.Length; } }
        /// <summary>Number of textures in mesh</summary>
        public int NumberTextures { get { return meshTextures.Length; } }
        /// <summary>Gets a texture from the mesh</summary>
        public TextureBase GetTexture(int index) { return meshTextures[index]; }
        /// <summary>Gets a material from the mesh</summary>
        public Material GetMaterial(int index) { return meshMaterials[index]; }
        /// <summary>
        /// Gets the Maximum positive bounding point.
        /// </summary>
        public Vector3 BoundingBoxMax { get { return boundingBoxMax; } }
        /// <summary>
        /// Gets the Maximum negative bounding point.
        /// </summary>
        public Vector3 BoundingBoxMin { get { return boundingBoxMin; } }
        /// <summary>
        /// Gets the filename.
        /// </summary>
        public string FileName
        {
            get { return meshFileName; }
        }
        #endregion

        #region Creation
        /// <summary>Create a new mesh using this file</summary>
        public XMesh(string fileName)
        {
            meshFileName = fileName;
            Create(Framework.Instance.Device, meshFileName);
        }

        /// <summary>Create a new mesh using this file</summary>
        public XMesh(string fileName, string folder)
        {
            meshFileName = fileName;
            meshFolder = folder;
            Create(Framework.Instance.Device, meshFileName);
        }
        
        /// <summary>Create the mesh data</summary>
        public void Create(Device device, string name)
        {
            defaultMaterial.Diffuse = System.Drawing.Color.White;
            defaultMaterial.Ambient = System.Drawing.Color.White;

            shadowMaterial.Diffuse = System.Drawing.Color.Black;
            shadowMaterial.Ambient = System.Drawing.Color.Black;
            
            // Hook the device events
            System.Diagnostics.Debug.Assert(device != null, "Device should not be null.");
            this.device = device;
            device.DeviceLost += new EventHandler(OnLostDevice);
            device.DeviceReset += new EventHandler(OnResetDevice);
            device.Disposing += new EventHandler(OnDeviceDisposing);

            GraphicsStream adjacency; // Adjacency information
            ExtendedMaterial[] materials; // Mesh material information

            // First try to find the filename
            string path = string.Empty;
            try
            {
                path = Utility.FindMediaFile(name, meshFolder);
            }
            catch(MediaNotFoundException)
            {
                // The media was not found, maybe a full path was passed in?
                if (System.IO.File.Exists(name))
                {
                    path = name;
                }
                else
                {
                    // No idea what this is trying to find
                    throw new MediaNotFoundException(name);
                }
            }

            // Now load the mesh
            systemMemoryMesh = Mesh.FromFile(path, MeshFlags.SystemMemory, device, out adjacency, 
                out materials);

            using (adjacency)
            {
                // Optimize the mesh for performance
                systemMemoryMesh.OptimizeInPlace(MeshFlags.OptimizeVertexCache | MeshFlags.OptimizeCompact | 
                    MeshFlags.OptimizeAttributeSort, adjacency);

                // Find the folder of where the mesh file is located
                string folder = Utility.AppendDirectorySeparator(new System.IO.FileInfo(path).DirectoryName);

                // Create the materials
                CreateMaterials(folder, device, adjacency, materials);
            }

            // Finally call reset
            OnResetDevice(device, EventArgs.Empty);
        }
        // TODO: Create with XOF

        /// <summary>Create the materials for the mesh</summary>
        public void CreateMaterials(string folder, Device device, GraphicsStream adjacency, ExtendedMaterial[] materials)
        {
            // Does the mesh have materials?
            if ((materials != null) && (materials.Length > 0))
            {
                // Allocate the arrays for the materials
                meshMaterials = new Material[materials.Length];
                meshTextures = new TextureBase[materials.Length];

                // Copy each material and create it's texture
                for(int i = 0; i < materials.Length; i++)
                {
                    // Copy the material first
                    meshMaterials[i] = materials[i].Material3D;
                    meshMaterials[i].Ambient = materials[i].Material3D.Diffuse;
                    // added later
                    meshMaterials[i].Diffuse = materials[i].Material3D.Diffuse;
                    meshMaterials[i].Emissive = materials[i].Material3D.Emissive;
                    meshMaterials[i].Specular = materials[i].Material3D.Specular;
                    meshMaterials[i].SpecularSharpness = materials[i].Material3D.SpecularSharpness;
                    //meshMaterials[i].Ambient = System.Drawing.Color.White;
                    
                    // Is there a texture for this material?
                    if ((materials[i].TextureFilename == null) || (materials[i].TextureFilename.Length == 0) )
                        continue; // No, just continue now

                    meshTextures[i] = new TextureBase(materials[i].TextureFilename, folder);
                    /*
                    ImageInformation info = new ImageInformation();
                    string textureFile = folder + materials[i].TextureFilename;
                    try
                    {
                        // First look for the texture in the same folder as the input folder
                        info = TextureLoader.ImageInformationFromFile(textureFile);
                    }
                    catch
                    {
                        try
                        {
                            // Couldn't find it, look in the media folder
                            textureFile = Utility.FindMediaFile(materials[i].TextureFilename);
                            info = TextureLoader.ImageInformationFromFile(textureFile);
                        }
                        catch (MediaNotFoundException)
                        {
                            // Couldn't find it anywhere, skip it
                            continue;
                        }
                    }
                    switch (info.ResourceType)
                    {
                        case ResourceType.Textures:
                            meshTextures[i] = TextureLoader.FromFile(device, textureFile);
                            break;
                        case ResourceType.CubeTexture:
                            meshTextures[i] = TextureLoader.FromCubeFile(device, textureFile);
                            break;
                        case ResourceType.VolumeTexture:
                            meshTextures[i] = TextureLoader.FromVolumeFile(device, textureFile);
                            break;
                    }
                     */
                }
            }
        }
        #endregion

        #region Class Methods
        /// <summary>Updates the mesh to a new vertex format</summary>
        public void SetVertexFormat(Device device, VertexFormats format)
        {
            Mesh tempSystemMesh = null;
            Mesh tempLocalMesh = null;
            VertexFormats oldFormat = VertexFormats.None;
            using(systemMemoryMesh)
            {
                using (localMemoryMesh)
                {
                    // Clone the meshes
                    if (systemMemoryMesh != null)
                    {
                        oldFormat = systemMemoryMesh.VertexFormat;
                        tempSystemMesh = systemMemoryMesh.Clone(systemMemoryMesh.Options.Value,
                            format, device);
                    }
                    if (localMemoryMesh != null)
                    {
                        tempLocalMesh = localMemoryMesh.Clone(localMemoryMesh.Options.Value,
                            format, device); 
                    }
                }
            }

            // Store the new meshes
            systemMemoryMesh = tempSystemMesh;
            localMemoryMesh = tempLocalMesh;

            // Compute normals if they are being requested and the old mesh didn't have them
            if ( ((oldFormat & VertexFormats.Normal) == 0) && (format != 0) )
            {
                if (systemMemoryMesh != null)
                    systemMemoryMesh.ComputeNormals();
                if (localMemoryMesh != null)
                    localMemoryMesh.ComputeNormals();
            }
        }
        /// <summary>Updates the mesh to a new vertex declaration</summary>
        public void SetVertexDeclaration(Device device, VertexElement[] decl)
        {
            Mesh tempSystemMesh = null;
            Mesh tempLocalMesh = null;
            VertexElement[] oldDecl = null;
            using(systemMemoryMesh)
            {
                using (localMemoryMesh)
                {
                    // Clone the meshes
                    if (systemMemoryMesh != null)
                    {
                        oldDecl = systemMemoryMesh.Declaration;
                        tempSystemMesh = systemMemoryMesh.Clone(systemMemoryMesh.Options.Value,
                            decl, device);
                    }
                    if (localMemoryMesh != null)
                    {
                        tempLocalMesh = localMemoryMesh.Clone(localMemoryMesh.Options.Value,
                            decl, device); 
                    }
                }
            }

            // Store the new meshes
            systemMemoryMesh = tempSystemMesh;
            localMemoryMesh = tempLocalMesh;
            
            bool hadNormal = false;
            // Check if the old declaration contains a normal.
            for(int i = 0; i < oldDecl.Length; i++)
            {
                if (oldDecl[i].DeclarationUsage == DeclarationUsage.Normal)
                {
                    hadNormal = true;
                    break;
                }
            }
            // Check to see if the new declaration has a normal
            bool hasNormalNow = false;
            for(int i = 0; i < decl.Length; i++)
            {
                if (decl[i].DeclarationUsage == DeclarationUsage.Normal)
                {
                    hasNormalNow = true;
                    break;
                }
            }

            // Compute normals if they are being requested and the old mesh didn't have them
            if ( !hadNormal && hasNormalNow )
            {
                if (systemMemoryMesh != null)
                    systemMemoryMesh.ComputeNormals();
                if (localMemoryMesh != null)
                    localMemoryMesh.ComputeNormals();
            }
        }

        /// <summary>Occurs after the device has been reset</summary>
        private void OnResetDevice(object sender, EventArgs e)
        {
            Device device = sender as Device;
            if (systemMemoryMesh == null)
                throw new InvalidOperationException("There is no system memory mesh.  Nothing to do here.");

            // Make a local memory version of the mesh. Note: because we are passing in
            // no flags, the default behavior is to clone into local memory.
            localMemoryMesh = systemMemoryMesh.Clone((systemMemoryMesh.Options.Value & ~MeshFlags.SystemMemory), 
                systemMemoryMesh.VertexFormat, device);
        }

        /// <summary>Occurs before the device is going to be reset</summary>
        private void OnLostDevice(object sender, EventArgs e)
        {
            if (localMemoryMesh != null)
                localMemoryMesh.Dispose();

            localMemoryMesh = null;
        }
        /// <summary>Renders this mesh</summary>
        public void Render(Device device, bool canDrawOpaque, bool canDrawAlpha)
        {
            if (localMemoryMesh == null)
                throw new InvalidOperationException("XMesh: No local memory mesh:" + meshFileName);

            // Only draw the subsets without alpha
            if (canDrawOpaque)
            {
                for (int i = 0; i < meshMaterials.Length; i++)
                {
                    if (isUsingMeshMaterials)
                    {
                        if (meshMaterials[i].DiffuseColor.Alpha < 1.0f)
                            continue; // Only drawing opaque right now

                        // set the device material and texture
                        device.Material = meshMaterials[i];
                        if (meshTextures[i] != null)
                            device.SetTexture(0, meshTextures[i].Texture);
                        else
                            device.SetTexture(0, null);
                    }
                    localMemoryMesh.DrawSubset(i);
                }
            }

            // Then, draw the subsets with alpha
            if (canDrawAlpha)
            {
                for (int i = 0; i < meshMaterials.Length; i++)
                {
                    if (meshMaterials[i].DiffuseColor.Alpha == 1.0f)
                        continue; // Only drawing non-opaque right now
                    
                    device.RenderState.AlphaBlendEnable = true;
                    device.RenderState.SourceBlend = Blend.SourceAlpha;
                    device.RenderState.DestinationBlend = Blend.InvSourceAlpha;

                    // New since 0.1.4
                    device.TextureState[0].ColorOperation = TextureOperation.Modulate;
                    device.TextureState[0].ColorArgument0 = TextureArgument.Diffuse;
                    device.TextureState[0].ColorArgument1 = TextureArgument.TextureColor;
                    device.TextureState[0].ColorArgument2 = TextureArgument.Diffuse;
                    device.TextureState[0].AlphaOperation = TextureOperation.Modulate;
                    device.TextureState[0].AlphaArgument0 = TextureArgument.Diffuse;
                    device.TextureState[0].AlphaArgument1 = TextureArgument.TextureColor;
                    device.TextureState[0].AlphaArgument2 = TextureArgument.Diffuse;
                                       
                   // set the device material and texture
                   device.Material = meshMaterials[i];
                    
                   if (meshTextures[i] != null)
                       device.SetTexture(0, meshTextures[i].Texture);
                   else
                       device.SetTexture(0, null);
                    
                   localMemoryMesh.DrawSubset(i);
                   device.RenderState.AlphaBlendEnable = false;
                   // HACK!!!
                   //device.Material = meshMaterials[0];                   
                   device.Material = defaultMaterial;
                }
            }
            device.Material = defaultMaterial;
        }
        /// <summary>Renders this mesh</summary>
        public void Render(Device device) { Render(device, true, true); }

        public void OnRenderShadow(Device device)
        {
            if (localMemoryMesh == null)
                throw new InvalidOperationException("No local memory mesh.");
            // Only draw the subsets without alpha

            device.Material = shadowMaterial;
            device.SetTexture(0, null);

            for (int i = 0; i < meshMaterials.Length; i++)
            {
                if (isUsingMeshMaterials)
                {
                    if (meshMaterials[i].DiffuseColor.Alpha < 1.0f)
                        continue; // Only drawing opaque right now
                }
                localMemoryMesh.DrawSubset(i);
            }            
            device.Material = defaultMaterial;
        }

        #region IFrameworkCallback Members
        public override void OnFrameMove(Device device, double totalTime, float elapsedTime)
        {

        }

        public override void OnFrameRender(Device device, double totalTime, float elapsedTime)
        {
            device.RenderState.Lighting = true;
            Render(device, true, true);
        }
        #endregion

        // TODO: Render with effect

        /// <summary>Compute a bounding sphere for this mesh</summary>
        public float ComputeBoundingSphere(out Vector3 center)
        {
            if (systemMemoryMesh == null)
                throw new InvalidOperationException("There is no system memory mesh.  Nothing to do here.");

            // Get the object declaration
            int strideSize = VertexInformation.GetFormatSize(systemMemoryMesh.VertexFormat);

            // Lock the vertex buffer
            GraphicsStream data = null;
            try
            {
                data = systemMemoryMesh.LockVertexBuffer(LockFlags.ReadOnly);
                // Now compute the bounding sphere
                return Geometry.ComputeBoundingSphere(data, systemMemoryMesh.NumberVertices, 
                    systemMemoryMesh.VertexFormat, out center);
            }
            finally
            {
                // Make sure to unlock the vertex buffer
                if (data != null)
                    systemMemoryMesh.UnlockVertexBuffer();
            }
        }

        public void ComputeBoundingBox()
        {
            if (systemMemoryMesh == null)
                throw new InvalidOperationException("There is no system memory mesh.  Nothing to do here.");

            // Get the object declaration
            int strideSize = VertexInformation.GetFormatSize(systemMemoryMesh.VertexFormat);

            // Lock the vertex buffer
            GraphicsStream data = null;
            try
            {
                data = systemMemoryMesh.LockVertexBuffer(LockFlags.ReadOnly);
                // Now compute the bounding sphere
                Geometry.ComputeBoundingBox(data, systemMemoryMesh.NumberVertices,
                    systemMemoryMesh.VertexFormat, out boundingBoxMin, out boundingBoxMax);
            }
            finally
            {
                // Make sure to unlock the vertex buffer
                if (data != null)
                    systemMemoryMesh.UnlockVertexBuffer();
            }
        }

        /// <summary>
        /// Replaces a texture with a new one.
        /// </summary>
        /// <param name="oldTexture"></param>
        /// <param name="newTexture"></param>
        public bool ReplaceTexture(string oldTexture, string newTexture, bool update)
        {
            bool textureFound = false;
            if (meshTextures == null)
                return false;
            for (int i = 0; i < meshTextures.Length; i++)
            {
                TextureBase texture = meshTextures[i];
                if (texture != null)
                {
                    if ((texture.FileName.ToLower().EndsWith(oldTexture.ToLower())) ||
                        (update && texture.FileName.LastIndexOf('\\') > 0 &&
                        newTexture.ToLower().EndsWith(texture.FileName.Substring(texture.FileName.LastIndexOf('\\') + 1).ToLower())))
                    {
                        try
                        {
                            textureFound = true;
                            texture.Dispose();
                            texture = new TextureBase(newTexture);
                            meshTextures[i] = texture;
                        }
                        catch
                        {
                        }
                    }
                }
            }
            return textureFound;
        }

        #endregion

        #region IDisposable Members

        /// <summary>Cleans up any resources required when this object is disposed</summary>
        public override void Dispose()
        {
            OnLostDevice(null, EventArgs.Empty);
            if (meshTextures != null)
            {
                for(int i = 0; i < meshTextures.Length; i++)
                {
                    if (meshTextures[i] != null)
                        meshTextures[i].Dispose();
                }
            }
            meshTextures = null;
            meshMaterials = null;

            if (systemMemoryMesh != null)
                systemMemoryMesh.Dispose();

            systemMemoryMesh = null;

            if (device != null)
            {
                device.DeviceLost -= new EventHandler(OnLostDevice);
                device.DeviceReset -= new EventHandler(OnResetDevice);
                device.Disposing -= new EventHandler(OnDeviceDisposing);
            }
        }

        /// <summary>Cleans up any resources required when this object is disposed</summary>
        private void OnDeviceDisposing(object sender, EventArgs e)
        {
            // Just dispose of our class
            Dispose();
        }
        #endregion

    }
}
