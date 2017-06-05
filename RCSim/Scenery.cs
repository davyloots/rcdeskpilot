using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Core.Interfaces;
using Bonsai.Objects;
using Bonsai.Objects.Meshes;
using Bonsai.Objects.Shaders;
using Bonsai.Objects.Textures;
using Microsoft.DirectX;
using Bonsai.Core;
using Bonsai.Objects.Terrain;
using System.Data;
using System.Drawing;
using System.IO;
using RCSim.DataClasses;

namespace RCSim
{
    internal class Scenery : IFrameworkCallback, IDisposable
    {
        #region Private fields
        private GameObject sky = null;
        private GameObject ground = null;
        private GameObject ground00 = null;
        private GameObject ground01 = null;
        private GameObject ground02 = null;
        private GameObject ground10 = null;
        private GameObject ground12 = null;
        private GameObject ground20 = null;
        private GameObject ground21 = null;
        private GameObject ground22 = null;
        private Program owner = null;
        private ShaderBase splatShader = null;
        private Heightmap heightMap = null;
        private Vegetation vegetation = null;
        private TerrainDefinition definition = null;
        private List<SceneryObject> sceneryObjects = new List<SceneryObject>();
        private List<Windmill> windmills = new List<Windmill>();
        private List<Flag> flags = new List<Flag>();
        private List<Water> waters = new List<Water>();
        private Plane waterReflectionClipPlane = new Plane(0f, 1f, 0f, 0.1f);
        private Tractor tractor = null;
        
#if EDITOR
        private List<Gate> gates = new List<Gate>();
#endif
        private string currentDetail = string.Empty;
        private SceneryParameters parameters = null;
        private LensFlare lensFlare = null;
        private int dynamicSceneryDetail = 1;
        private float terrainAmbient = 0;
        private float terrainSun = 0;

        private PhotoScenery photoScenery = null;
        #endregion

        #region Public properties
        /// <summary>
        /// Gets the heightmap of the scenery.
        /// </summary>
        public Heightmap Heightmap
        {
            get { return heightMap; }
        }

        /// <summary>
        /// Gets a reference to the terrain definition.
        /// </summary>
        public TerrainDefinition Definition
        {
            get { return definition; }
        }

        /// <summary>
        /// Returns a reference to the SceneryParameters object.
        /// </summary>
        public SceneryParameters Parameters
        {
            get { return parameters; }
        }

        /// <summary>
        /// Returns the clipping plane for water reflections.
        /// </summary>
        public Plane WaterReflectionClipPlane
        {
            get { return waterReflectionClipPlane; }
        }

        /// <summary>
        /// Returns the pilot position for floatplanes.
        /// </summary>
        public Vector3 PilotWaterPosition
        {
            get
            {
                if (definition != null)
                {
                    Vector3 position = new Vector3(0,1.7f,0);
                    foreach (DataRow row in definition.PilotPositionTable.Rows)
                    {                        
                        if (Convert.ToBoolean(row["Water"]))
                        {
                            return new Vector3(((Vector3)(row["Position"])).X,
                                Heightmap.GetHeightAt(((Vector3)(row["Position"])).X, ((Vector3)(row["Position"])).Z) + 1.7f,
                                ((Vector3)(row["Position"])).Z);
                        }
                        position = new Vector3(((Vector3)(row["Position"])).X,
                                Heightmap.GetHeightAt(((Vector3)(row["Position"])).X, ((Vector3)(row["Position"])).Z) + 1.7f,
                                ((Vector3)(row["Position"])).Z); ;
                    }
                    return position;
                }
                return new Vector3(0, 1.7f, 0);
            }
        }

        /// <summary>
        /// Gets the map for water.
        /// </summary>
        public string WaterMap
        {
            get
            {
                if (definition != null)
                {
                    string map = null;
                    foreach (DataRow row in definition.PilotPositionTable.Rows)
                    {
                        if (Convert.ToBoolean(row["Water"]))
                        {
                            return parameters.SceneryFolder + row["Map"].ToString();
                        }
                        map = parameters.SceneryFolder + row["Map"].ToString();
                    }
                    return map;
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the list of water surfaces.
        /// </summary>
        public List<Water> Water
        {
            get { return waters; }
        }
                
        /// <summary>
        /// Gets/Sets the texture of the sky
        /// </summary>
        public string SkyTexture { get; set; }

        /// <summary>
        /// Gets/Sets the containing folder of the current scenery.
        /// </summary>
        public string SceneryFolder 
        {
            get
            {
                if (parameters != null)
                    return parameters.SceneryFolder;
                else
                    return null;
            }        
        }
        #endregion

        #region Constructor
        public Scenery(Program owner)
        {
            this.owner = owner;
            dynamicSceneryDetail = Convert.ToInt32(Bonsai.Utils.Settings.GetValue("DynamicScenery", "1"));
            Bonsai.Utils.Settings.SettingsChanged += new Bonsai.Utils.Settings.SettingsEventHandler(Settings_SettingsChanged);
        }        
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Clean up.
        /// </summary>
        public void Dispose()
        {
            Bonsai.Utils.Settings.SettingsChanged -= new Bonsai.Utils.Settings.SettingsEventHandler(Settings_SettingsChanged);
            FinalizeScenery();            
        }
        #endregion

        #region Public methods
        public void LoadDefinition(string fileName)
        {            
            FinalizeScenery();
            parameters = new SceneryParameters();
            parameters.ReadParameters(fileName);
            if (parameters.DefinitionFile != null)
            {
                definition = new TerrainDefinition();
                definition.Load(parameters.SceneryFolder + parameters.DefinitionFile);               
            }
            Initialize();
        }

        public void SaveDefinition(string fileName)
        {
            definition.Save(fileName);
        }

        public void ApplyAds(string oldTexture, string newTexture)
        {
            try
            {
                foreach (SceneryObject o in sceneryObjects)
                {
                    try
                    {
                        if (o.Mesh is XMesh)
                        {
                            XMesh xMesh = o.Mesh as XMesh;
                            if (xMesh != null)
                            {
                                xMesh.ReplaceTexture(oldTexture, newTexture, true);
                            }
                        }
                    }
                    catch
                    {
                    }
                }
                Flag.ApplyAds(oldTexture, newTexture);
#if EDITOR
                foreach (Gate o in gates)
                {
                    if (o.Mesh is XMesh)
                    {
                        XMesh xMesh = o.Mesh as XMesh;
                        if (xMesh != null)
                        {
                            xMesh.ReplaceTexture(oldTexture, newTexture, true);
                        }
                    }
                }
#endif
            }
            catch
            {
            }
        }

        public void SetSkyTexture(string fileName)
        {
            if ((sky != null) && (sky.Mesh != null) && (fileName != null))
            {
                DomeMesh skyDome = sky.Mesh as DomeMesh;
                if (skyDome != null)
                {
                    if (skyDome.Texture != null)
                        skyDome.Texture.Dispose();
                    string path = Path.GetDirectoryName(parameters.FileName);
                    skyDome.Texture = new Bonsai.Objects.Textures.TextureBase(string.Concat(path, "\\", fileName));
                    SkyTexture = fileName;
                }
            }

        }

        public void SetSky(string texture, Vector3 sunPosition, Color ambientColor, Color sunlightColor, float terrainAmbient, float terrainSun)
        {
            SetSkyTexture(texture);
            Program.Instance.SunLightColor = sunlightColor;
            Program.Instance.AmbientLightColor = ambientColor;
            Program.Instance.SunPosition = sunPosition;
            LensFlare.SunPosition = sunPosition;
            LensFlare.Visible = sunPosition.Length() > 1f;
            Vector3 sunPosNorm = sunPosition;
            this.terrainAmbient = terrainAmbient;
            this.terrainSun = terrainSun;
            sunPosNorm.Normalize();
            if (splatShader != null)
            {
                splatShader.SetVariable("sunPosition", sunPosNorm);
                splatShader.SetVariable("ambientFactor", terrainAmbient);
                splatShader.SetVariable("sunFactor", terrainSun);
            }
            Flag.SetSky(sunPosition, ambientColor, sunlightColor, terrainAmbient, terrainSun);
            foreach (Water water in waters)
                water.SetSunPosition(sunPosition);
        }

        public void SetWaterCallback(Bonsai.Objects.Terrain.Water.OnFrameRenderDelegate callback)
        {
            foreach (Water water in waters)
            {
                water.SetCallback(callback);
            }
        }

        public void RenderTextures(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            foreach (Water water in waters)
            {
                water.RenderTextures(device, totalTime, elapsedTime);
            }
        }
        #endregion

        #region Private methods
        private void Initialize()
        {

            if (parameters.SceneryType == SceneryParameters.SceneryTypeEnum.Photofield)
            {
                if (parameters.HeightMapFile != null)
                {
                    heightMap = new Heightmap(parameters.SceneryFolder + parameters.HeightMapFile, 
                        (float)parameters.HeightMapSize, parameters.HeightMapResolution, parameters.HeightMapResolution);
                    heightMap.MinHeight = parameters.MinimumHeight;
                    heightMap.MaxHeight = parameters.MaximumHeight;
                }
                else
                    heightMap = new Heightmap(1000);
                photoScenery = new PhotoScenery(parameters);   
                // Set lighting conditions
                if (definition != null)
                {
                    if (definition.SkyTable.Rows.Count > 0)
                    {
                        DataRow dataRow = definition.SkyTable.Rows[0];
                        Vector3 ambientVector = (Vector3)dataRow["AmbientLight"];
                        Vector3 sunVector = (Vector3)dataRow["SunLight"];
                        SetSky(null, (Vector3)dataRow["SunPosition"],
                            Color.FromArgb((int)(255 * ambientVector.X), (int)(255 * ambientVector.Y), (int)(255 * ambientVector.Z)),
                            Color.FromArgb((int)(255 * sunVector.X), (int)(255 * sunVector.Y), (int)(255 * sunVector.Z)),
                            (float)(dataRow["TerrainAmbient"]), (float)(dataRow["TerrainSun"]));
                        lensFlare = new LensFlare();
                    }
                }
            }
            else
            {
                vegetation = new Vegetation(owner);

                ground = new GameObject();
                heightMap = new Heightmap(parameters.SceneryFolder + parameters.HeightMapFile, 1000f, 100, 100);
                heightMap.MinHeight = parameters.MinimumHeight;
                heightMap.MaxHeight = parameters.MaximumHeight;
                TerrainMesh sceneryMesh = new TerrainMesh(1.0f, heightMap);
                ground.Mesh = sceneryMesh;

                // Load the detail settings
                currentDetail = Bonsai.Utils.Settings.GetValue("SceneryDetail", "2");
                dynamicSceneryDetail = Convert.ToInt32(Bonsai.Utils.Settings.GetValue("DynamicScenery", "1"));

                if (currentDetail.Equals("1"))
                    sceneryMesh.Texture = new Bonsai.Objects.Textures.TextureBase(parameters.SplatLowFile);
                else
                {
                    //ground.RotateXAngle = (float)Math.PI / 2;

                    //splatShader = new ShaderBase("splatting", "splatting3.fx");
                    //if (Framework.Instance.DeviceCaps.PixelShaderVersion.Major >= 3)
                    //    splatShader.SetTechnique("TextureSplatting_fx30");
                    //else
                    //    splatShader.SetTechnique("TextureSplatting");

                    splatShader = new ShaderBase("splatting", "splat.fx");
                    splatShader.SetTechnique("TextureSplatting");
                    splatShader.SetVariable("matViewProj", ShaderBase.ShaderParameters.CameraProjection);
                    splatShader.SetVariable("sunPosition", new Vector3(1, 1, 1));
                    splatShader.SetVariable("underwater", 0.0f);
                    splatShader.SetVariable("matWorld", ShaderBase.ShaderParameters.World);
                    splatShader.SetVariable("matInvertTransposeWorld", ShaderBase.ShaderParameters.WorldInvertTranspose);
                    splatShader.SetVariable("cameraPos", ShaderBase.ShaderParameters.CameraPosition);
                    splatShader.SetVariable("nearRepeat", 64.0f);
                    splatShader.SetVariable("farRepeat", 16.0f);
                    splatShader.SetVariable("nearFactor2", 1.0f);
                    splatShader.SetVariable("farFactor2", 1.0f);
                    splatShader.SetVariable("nearFactor3", 1.0f);
                    splatShader.SetVariable("farFactor3", 1.0f);
                    splatShader.SetVariable("nearFactor4", 5.0f);
                    splatShader.SetVariable("farFactor4", 5.0f);
                    splatShader.SetVariable("blendSqDistance", 50f * 50f);
                    splatShader.SetVariable("blendSqWidth", 1.0f / (200f * 200f));
                    TextureBase normalMap = new TextureBase(parameters.NormalMapFile);
                    TextureBase alphaMap = new TextureBase(parameters.SplatHighFile);
                    //TextureBase grassTexture = new TextureBase("data/grass_zoomout.png");
                    TextureBase grassTexture = new TextureBase(parameters.Texture1File);
                    TextureBase rockTexture = new TextureBase(parameters.Texture2File);
                    //TextureBase rockNormal = new TextureBase("data/splat_rock_normal.jpg");
                    TextureBase sandTexture = new TextureBase(parameters.Texture3File);
                    TextureBase concreteTexture = new TextureBase(parameters.Texture4File);

                    splatShader.SetVariable("NormalMapTexture", normalMap);
                    splatShader.SetVariable("AlphaTexture", alphaMap);
                    splatShader.SetVariable("DetailTexture1", grassTexture);
                    splatShader.SetVariable("DetailTexture2", rockTexture);
                    //splatShader.SetVariable("DetailTexture2NormalMap", rockNormal);
                    splatShader.SetVariable("DetailTexture3", sandTexture);
                    splatShader.SetVariable("DetailTexture4", concreteTexture);

                    ground.Shader = splatShader;
                }

                CreateSurroundings();

                // Load the trees
                vegetation.LoadTrees(heightMap, definition);

                // Load the simple trees
                LoadSimpleTrees(definition);

                // Load the simple tall trees
                LoadSimpleTallTrees(definition);

                // Load the simple small trees
                LoadSimpleSmallTrees(definition);

                // Load the scenery objects
                LoadSceneryObjects(definition);

                // Load the windmills
                LoadWindmills(definition);

                // Load the weather
                LoadWeather(definition);

                // Load the dynamic scenery
                if (dynamicSceneryDetail > 0)
                {
                    LoadDynamicScenery(definition);
                }

                // Load the sky
                sky = new GameObject();
                DomeMesh skyDome = new DomeMesh(4500, 16, 16);
                sky.Mesh = skyDome;
                DataRow dataRow = definition.SkyTable.Rows[0];
                foreach (DataRow skyRow in definition.SkyTable.Rows)
                {
                    string skyName = skyRow["Name"].ToString();
                    if (skyName.Equals(Bonsai.Utils.Settings.GetValue("Sky")))
                    {
                        dataRow = skyRow;
                        break;
                    }
                }

                // Load the water
                LoadWater(definition);
                //water = new Water(new Vector3(90, 0, 180), 100f, Bonsai.Objects.Terrain.Water.QualityLevelEnum.Low);

                if (dataRow != null)
                {
                    Vector3 ambientVector = (Vector3)dataRow["AmbientLight"];
                    Vector3 sunVector = (Vector3)dataRow["SunLight"];
                    SetSky(dataRow["Texture"].ToString(), (Vector3)dataRow["SunPosition"],
                        Color.FromArgb((int)(255 * ambientVector.X), (int)(255 * ambientVector.Y), (int)(255 * ambientVector.Z)),
                        Color.FromArgb((int)(255 * sunVector.X), (int)(255 * sunVector.Y), (int)(255 * sunVector.Z)),
                        (float)(dataRow["TerrainAmbient"]), (float)(dataRow["TerrainSun"]));
                }

                // Apply the ads
                if (Utility.MediaExists("ads/ad1.jpg"))
                    ApplyAds("ad.jpg", "ads/ad1.jpg");
                if (Utility.MediaExists("ads/ad2.jpg"))
                    ApplyAds("ad2.jpg", "ads/ad2.jpg");

                lensFlare = new LensFlare();
            }

#if EDITOR
            // Load the racing pylons
            LoadGates(definition);
#else
            if (Program.Instance.Player != null)
            {
                Program.Instance.Player.Heightmap = heightMap;
                Program.Instance.Player.DefaultStartPosition = parameters.DefaultStartPosition;
                Program.Instance.Player.WaterStartPosition = parameters.WaterStartPosition;
            }

            SetWaterCallback(Program.Instance.OnFrameRenderWater);
            // Rerender the reflection
            if (Effects.Reflection.Instance != null)
                Effects.Reflection.Instance.Invalidate();
#endif
        }

        private void FinalizeScenery()
        {
            if ((parameters != null) && (parameters.SceneryType == SceneryParameters.SceneryTypeEnum.Photofield))
            {
                if (photoScenery != null)
                {
                    photoScenery.Dispose();
                    photoScenery = null;
                }                
            }

            owner.Weather.Wind.ClearThermalSources();

            DisposeWater();

            if (sky != null)
            {
                sky.Dispose();
                sky = null;
            }
            if (ground != null)
            {
                ground.Dispose();
                ground = null;
            }
            DisposeSceneryObjects();

            DisposeSurroundings();

            if (vegetation != null)
            {
                vegetation.Dispose();
                vegetation = null;
            }

            DisposeWindmills();

            DisposeDynamicScenery();

            if (lensFlare != null)
            {
                lensFlare.Dispose();
                lensFlare = null;
            }
            /*
            if (windsock != null)
            {
                windsock.Dispose();
                windsock = null;
            }
             */

            if (photoScenery != null)
            {
                photoScenery.Dispose();
                photoScenery = null;
            }

            /*
            if (definition != null)
            {
                definition = null;
            }
             */
#if EDITOR
            DisposeGates();
#endif
        }

        private void CreateSurroundings()
        {
            SquareMesh groundMesh = new SquareMesh(2500, 1, 1, 1.0f);
            TextureBase grassTexture = new TextureBase("data\\surrounding.png");
            groundMesh.Texture = grassTexture;
            CreateGround(ref ground00, groundMesh, 1, 1);
            CreateGround(ref ground01, groundMesh, 1, 0);
            CreateGround(ref ground02, groundMesh, 1, -1);
            CreateGround(ref ground10, groundMesh, 0, 1);
            CreateGround(ref ground12, groundMesh, 0, -1);
            CreateGround(ref ground20, groundMesh, -1, 1);
            CreateGround(ref ground21, groundMesh, -1, 0);
            CreateGround(ref ground22, groundMesh, -1, -1);
        }

        private void DisposeSurroundings()
        {
            DisposeGround(ref ground00);
            DisposeGround(ref ground01);
            DisposeGround(ref ground02);
            DisposeGround(ref ground10);
            DisposeGround(ref ground12);
            DisposeGround(ref ground20);
            DisposeGround(ref ground21);
            DisposeGround(ref ground22);
        }

        private void CreateGround(ref GameObject gameObject, SquareMesh mesh, int x, int y)
        {
            gameObject = new GameObject();
            gameObject.Mesh = mesh;
            gameObject.RotateXAngle = (float)Math.PI / 2;
            gameObject.Position = new Vector3(3000f * x, 0f, 3000f * y);
        }

        private void DisposeGround(ref GameObject gameObject)
        {
            if (gameObject != null)
            {
                gameObject.Dispose();
                gameObject = null;
            }
        }

        private void LoadSceneryObjects(TerrainDefinition definition)
        {
            foreach (DataRow objectRow in definition.ObjectTable.Rows)
            {
                AddObject(
                    TerrainDefinition.ObjectTypeEnum.SceneryObject, 
                    (Vector3)objectRow["Position"], 
                    (Vector3)objectRow["Orientation"],
                    (string)objectRow["FileName"]);
            }
        }

        private void DisposeSceneryObjects()
        {
            foreach (SceneryObject sceneryObject in sceneryObjects)
            {
                sceneryObject.Dispose();
            }
            sceneryObjects.Clear();
        }

        private void LoadSimpleTrees(TerrainDefinition definition)
        {
            foreach (DataRow treeRow in definition.SimpleTreeTable.Rows)
            {
                AddObject(TerrainDefinition.ObjectTypeEnum.SimpleTree,
                    (Vector3)treeRow["Position"], new Vector3(), null);
            }
        }

        private void LoadSimpleTallTrees(TerrainDefinition definition)
        {
            foreach (DataRow treeRow in definition.SimpleTallTreeTable.Rows)
            {
                AddObject(TerrainDefinition.ObjectTypeEnum.SimpleTallTree,
                    (Vector3)treeRow["Position"], new Vector3(), null);
            }
        }

        private void LoadSimpleSmallTrees(TerrainDefinition definition)
        {
            foreach (DataRow treeRow in definition.SimpleSmallTreeTable.Rows)
            {
                AddObject(TerrainDefinition.ObjectTypeEnum.SimpleSmallTree,
                    (Vector3)treeRow["Position"], new Vector3(), null);
            }
        }

        private void LoadWindmills(TerrainDefinition definition)
        {
            foreach (DataRow millRow in definition.WindmillTable.Rows)
            {
                AddObject(TerrainDefinition.ObjectTypeEnum.Windmill,
                    (Vector3)millRow["Position"], new Vector3(), null);
            }
        }

        private void DisposeWindmills()
        {
            foreach (Windmill windmill in windmills)
            {
                windmill.Dispose();
            }
            windmills.Clear();
        }

        private void LoadDynamicScenery(TerrainDefinition definition)
        {
            tractor = new Tractor();
            foreach (DataRow flagRow in definition.FlagTable.Rows)
            {
                AddObject(TerrainDefinition.ObjectTypeEnum.Flag,
                    (Vector3)flagRow["Position"], new Vector3(), null);
            }
            Flag.SetSky(Program.Instance.SunPosition, Program.Instance.AmbientLightColor, Program.Instance.SunLightColor, terrainAmbient, terrainSun);
        }

        private void DisposeDynamicScenery()
        {
            if (tractor != null)
            {
                tractor.Dispose();
                tractor = null;
            }
            foreach (Flag flag in flags)
            {
                flag.Dispose();
            }
            flags.Clear();
        }

        private void LoadWeather(TerrainDefinition definition)
        {
            foreach (DataRow row in definition.ThermalTable.Rows)
            {
                owner.Weather.Wind.AddThermalSource((Vector3)row["Position"], (float)row["Strength"], (float)row["Size"]);
            }
        }

        private void LoadWater(TerrainDefinition definition)
        {
            foreach (DataRow row in definition.WaterTable.Rows)
            {
                waters.Add(new Water((Vector3)row["Position"], (float)row["Size"]));
            }
        }

        private void DisposeWater()
        {
            foreach (Water water in waters)
            {
                water.Dispose();
            }
            waters.Clear();
        }

#if EDITOR
        private void LoadGates(TerrainDefinition definition)
        {
            foreach (DataRow row in definition.GateTable.Rows)
            {
                AddGate((Vector3)row["Position"], (Vector3)row["Orientation"], (int)row["SequenceNr"], (int)row["Type"]);
            }
        }

        private void DisposeGates()
        {
            foreach (Gate gate in gates)
            {
                gate.Dispose();
            }
            gates.Clear();
        }
#endif
        #endregion

        #region Internal objects
        internal void AddObject(TerrainDefinition.ObjectTypeEnum objectTypeEnum, Vector3 position, Vector3 orientation, string fileName)
        {
            AddObject(objectTypeEnum, position, orientation, fileName, false);
        }

        internal void AddObject(TerrainDefinition.ObjectTypeEnum objectTypeEnum, Vector3 position, Vector3 orientation, string fileName, bool addToDefinition)
        {
            switch (objectTypeEnum)
            {
                case TerrainDefinition.ObjectTypeEnum.SceneryObject:
                    SceneryObject sceneryObject = new SceneryObject(fileName);
                    sceneryObject.Position = position;
                    sceneryObject.RotateXAngle = orientation.X;
                    sceneryObject.RotateYAngle = orientation.Y;
                    sceneryObject.RotateZAngle = orientation.Z;
                    sceneryObjects.Add(sceneryObject);
                    if (addToDefinition)
                        definition.AddObject(fileName, position, orientation);
                    break;
                case TerrainDefinition.ObjectTypeEnum.Tree:
                    vegetation.AddTree(position.X, heightMap.GetHeightAt(position.X, position.Z), position.Z);
                    if (addToDefinition)
                        definition.AddTree(position);
                    break;
                case TerrainDefinition.ObjectTypeEnum.SimpleTree:
                    vegetation.AddSimpleTree(position.X, heightMap.GetHeightAt(position.X, position.Z), position.Z);
                    if (addToDefinition)
                        definition.AddSimpleTree(position);
                    break;
                case TerrainDefinition.ObjectTypeEnum.SimpleTallTree:
                    vegetation.AddSimpleTallTree(position.X, heightMap.GetHeightAt(position.X, position.Z), position.Z);
                    if (addToDefinition)
                        definition.AddSimpleTallTree(position);
                    break;
                case TerrainDefinition.ObjectTypeEnum.SimpleSmallTree:
                    vegetation.AddSimpleSmallTree(position.X, heightMap.GetHeightAt(position.X, position.Z), position.Z);
                    if (addToDefinition)
                        definition.AddSimpleSmallTree(position);
                    break;
                case TerrainDefinition.ObjectTypeEnum.Windmill:
                    Windmill windmill = new Windmill();
                    windmill.Position = position;
                    windmills.Add(windmill);
                    if (addToDefinition)
                        definition.AddWindmill(position);
                    break;
                case TerrainDefinition.ObjectTypeEnum.Flag:
                    Flag flag = new Flag(position);
                    flags.Add(flag);
                    if (addToDefinition)
                        definition.AddFlag(position);
                    break;
            }
        }

#if EDITOR
        internal void AddGate(Vector3 position, Vector3 orientation, int sequenceNr, int type)
        {
            AddGate(position, orientation, sequenceNr, type, false);
        }

        internal void AddGate(Vector3 position, Vector3 orientation, int sequenceNr, int type, bool addToDefinition)
        {
            gates.Add(new Gate(owner, position, orientation, sequenceNr, type));
            if (addToDefinition)
                definition.AddGate(position, orientation, sequenceNr, type);
        }
#endif
        internal void CreateLightMap(string fileName)
        {
            int lightmapResolution = 1024;
            Bitmap lightmap = new Bitmap(lightmapResolution,lightmapResolution);
            
            for (int x = 0; x < lightmapResolution; x++)
            {
                for (int y = 0; y < lightmapResolution; y++)
                {
                    float nx = 1000f * ((x - lightmapResolution/2) / (float)lightmapResolution);
                    float ny = 1000f * ((lightmapResolution/2 - y) / (float)lightmapResolution);
                    Vector3 normal = heightMap.GetSmoothNormalAt(nx, ny);
                    lightmap.SetPixel(x, y, Color.FromArgb((int)(127 + 127*normal.X), (int)(127 + 127*normal.Y), (int)(127 + 127*normal.Z)));
                }
            }
            Graphics g = Graphics.FromImage(lightmap);
            // draw shadows of trees
            Bitmap shadowTree = new Bitmap("data\\shadows\\tree.png");
            Bitmap shadowTallTree = new Bitmap("data\\shadows\\tall_tree.png");
            Bitmap shadowSmallTree = new Bitmap("data\\shadows\\small_tree.png");
            Bitmap shadowWindMill = new Bitmap("data\\shadows\\windmill.png");
            foreach (DataRow treeRow in definition.TreeTable.Rows)
            {
                Vector3 position = (Vector3)treeRow["Position"];
                int x = (int)(Math.Round(position.X * 1.024)) + lightmap.Width / 2 - shadowTree.Width / 2;
                int y = (int)(Math.Round(-position.Z * 1.024)) + lightmap.Height / 2 - shadowTree.Height / 2;
                g.DrawImage(shadowTree, new Rectangle(x, y, shadowTree.Width, shadowTree.Height), 
                    new Rectangle(0, 0, shadowTree.Width, shadowTree.Height), GraphicsUnit.Pixel);
            }
            // draw shadows of simple trees
            foreach (DataRow treeRow in definition.SimpleTreeTable.Rows)
            {
                Vector3 position = (Vector3)treeRow["Position"];
                int x = (int)(Math.Round(position.X * 1.024)) + lightmap.Width / 2 - shadowTree.Width / 2;
                int y = (int)(Math.Round(-position.Z * 1.024)) + lightmap.Height / 2 - shadowTree.Height / 2;
                g.DrawImage(shadowTree, new Rectangle(x, y, shadowTree.Width, shadowTree.Height),
                     new Rectangle(0, 0, shadowTree.Width, shadowTree.Height), GraphicsUnit.Pixel);
            }
            // draw shadows of simple tall trees
            foreach (DataRow treeRow in definition.SimpleTallTreeTable.Rows)
            {
                Vector3 position = (Vector3)treeRow["Position"];
                int x = (int)(Math.Round(position.X * 1.024)) + lightmap.Width / 2 - shadowTallTree.Width / 2;
                int y = (int)(Math.Round(-position.Z * 1.024)) + lightmap.Height / 2 - shadowTallTree.Height / 2;
                g.DrawImage(shadowTallTree, new Rectangle(x, y, shadowTallTree.Width, shadowTallTree.Height),
                     new Rectangle(0, 0, shadowTallTree.Width, shadowTallTree.Height), GraphicsUnit.Pixel);
            }
            // draw shadows of simple small trees
            foreach (DataRow treeRow in definition.SimpleSmallTreeTable.Rows)
            {
                Vector3 position = (Vector3)treeRow["Position"];
                int x = (int)(Math.Round(position.X * 1.024)) + lightmap.Width / 2 - shadowSmallTree.Width / 2;
                int y = (int)(Math.Round(-position.Z * 1.024)) + lightmap.Height / 2 - shadowSmallTree.Height / 2;
                g.DrawImage(shadowSmallTree, new Rectangle(x, y, shadowSmallTree.Width, shadowSmallTree.Height),
                     new Rectangle(0, 0, shadowSmallTree.Width, shadowSmallTree.Height), GraphicsUnit.Pixel);
            }
            // draw shadows of windmills
            foreach (DataRow millRow in definition.WindmillTable.Rows)
            {
                Vector3 position = (Vector3)millRow["Position"];
                int x = (int)(Math.Round(position.X * 1.024)) + lightmap.Width / 2 - shadowWindMill.Width / 2;
                int y = (int)(Math.Round(-position.Z * 1.024)) + lightmap.Height / 2 - shadowWindMill.Height / 2;
                g.DrawImage(shadowWindMill, new Rectangle(x, y, shadowWindMill.Width, shadowWindMill.Height),
                     new Rectangle(0, 0, shadowWindMill.Width, shadowWindMill.Height), GraphicsUnit.Pixel);
            }
            // draw shadows of objects
            foreach (DataRow objectRow in definition.ObjectTable.Rows)
            {
                using (Bitmap shadow = new Bitmap("data\\shadows\\" + Path.GetFileNameWithoutExtension(objectRow["FileName"].ToString()) + ".png"))
                {
                    Vector3 position = (Vector3)objectRow["Position"];
                    int x = (int)(Math.Round(position.X * 1.024)) + lightmap.Width / 2 - shadow.Width / 2;
                    int y = (int)(Math.Round(-position.Z * 1.024)) + lightmap.Height / 2 - shadow.Height / 2;
                    g.DrawImage(shadow, new Rectangle(x, y, shadow.Width, shadow.Height),
                        new Rectangle(0, 0, shadow.Width, shadow.Height), GraphicsUnit.Pixel);
                }
            }
            lightmap.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
        }

        internal void CreateLightMapOld(string fileName)
        {
            Bitmap lightmap = new Bitmap("data\\shadows\\empty.png");
            Graphics g = Graphics.FromImage(lightmap);
            // draw shadows of trees
            Bitmap shadowTree = new Bitmap("data\\shadows\\tree.png");
            Bitmap shadowTallTree = new Bitmap("data\\shadows\\tall_tree.png");
            Bitmap shadowSmallTree = new Bitmap("data\\shadows\\small_tree.png");
            Bitmap shadowWindMill = new Bitmap("data\\shadows\\windmill.png");
            foreach (DataRow treeRow in definition.TreeTable.Rows)
            {
                Vector3 position = (Vector3)treeRow["Position"];
                int x = (int)(Math.Round(position.X*1.023)) + lightmap.Width / 2 - shadowTree.Width/2;
                int y = (int)(Math.Round(-position.Z*1.023)) + lightmap.Height / 2 - shadowTree.Height/2;
                g.DrawImage(shadowTree, new Point(x, y));
            }
            // draw shadows of simple trees
            foreach (DataRow treeRow in definition.SimpleTreeTable.Rows)
            {
                Vector3 position = (Vector3)treeRow["Position"];
                int x = (int)(Math.Round(position.X * 1.023)) + lightmap.Width / 2 - shadowTree.Width / 2;
                int y = (int)(Math.Round(-position.Z * 1.023)) + lightmap.Height / 2 - shadowTree.Height / 2;
                g.DrawImage(shadowTree, new Point(x, y));
            }
            // draw shadows of simple tall trees
            foreach (DataRow treeRow in definition.SimpleTallTreeTable.Rows)
            {
                Vector3 position = (Vector3)treeRow["Position"];
                int x = (int)(Math.Round(position.X * 1.023)) + lightmap.Width / 2 - shadowTallTree.Width / 2;
                int y = (int)(Math.Round(-position.Z * 1.023)) + lightmap.Height / 2 - shadowTallTree.Height / 2;
                g.DrawImage(shadowTallTree, new Point(x, y));
            }
            // draw shadows of simple small trees
            foreach (DataRow treeRow in definition.SimpleSmallTreeTable.Rows)
            {
                Vector3 position = (Vector3)treeRow["Position"];
                int x = (int)(Math.Round(position.X * 1.023)) + lightmap.Width / 2 - shadowSmallTree.Width / 2;
                int y = (int)(Math.Round(-position.Z * 1.023)) + lightmap.Height / 2 - shadowSmallTree.Height / 2;
                g.DrawImage(shadowSmallTree, new Point(x, y));
            }
            // draw shadows of windmills
            foreach (DataRow millRow in definition.WindmillTable.Rows)
            {
                Vector3 position = (Vector3)millRow["Position"];
                int x = (int)(Math.Round(position.X * 1.023)) + lightmap.Width / 2 - shadowWindMill.Width / 2;
                int y = (int)(Math.Round(-position.Z * 1.023)) + lightmap.Height / 2 - shadowWindMill.Height / 2;
                g.DrawImage(shadowWindMill, new Point(x, y));
            }
            // draw shadows of objects
            foreach (DataRow objectRow in definition.ObjectTable.Rows)
            {
                using (Bitmap shadow = new Bitmap("data\\shadows\\" + Path.GetFileNameWithoutExtension(objectRow["FileName"].ToString()) + ".png"))
                {
                    Vector3 position = (Vector3)objectRow["Position"];
                    int x = (int)(Math.Round(position.X * 1.023)) + lightmap.Width / 2 - shadow.Width / 2;
                    int y = (int)(Math.Round(-position.Z * 1.023)) + lightmap.Height / 2 - shadow.Height / 2;
                    g.DrawImage(shadow, new Point(x, y));
                }
            }
            lightmap.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
        }
        #endregion

        #region Private event handlers
        void Settings_SettingsChanged(object sender, Bonsai.Utils.Settings.SettingsEventArgs e)
        {
            if (!currentDetail.Equals(Bonsai.Utils.Settings.GetValue("SceneryDetail")))
            {
                FinalizeScenery();
                Initialize();
            }
            if (!dynamicSceneryDetail.Equals(Bonsai.Utils.Settings.GetValue("DynamicScenery", "1")))
            {
                dynamicSceneryDetail = Convert.ToInt32(Bonsai.Utils.Settings.GetValue("DynamicScenery", "1"));
                DisposeDynamicScenery();
                if ((definition != null) && (dynamicSceneryDetail > 0))
                {
                    LoadDynamicScenery(definition);
                }
            }
        }
        #endregion


        #region IFrameworkCallback Members

        public void OnFrameMove(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            if (parameters.SceneryType == SceneryParameters.SceneryTypeEnum.Full3D)
            {
                sky.OnFrameMove(device, totalTime, elapsedTime);
                foreach (SceneryObject sceneryObject in sceneryObjects)
                {
                    sceneryObject.OnFrameMove(device, totalTime, elapsedTime);
                }
                foreach (Windmill mill in windmills)
                {
                    mill.OnFrameMove(device, totalTime, elapsedTime);
                }
                foreach (Flag flag in flags)
                {
                    flag.OnFrameMove(device, totalTime, elapsedTime);
                }
#if EDITOR
            foreach (Gate gate in gates)
            {
               gate.OnFrameMove(device, totalTime, elapsedTime);
            }
#endif
                vegetation.OnFrameMove(device, totalTime, elapsedTime);
                ground.OnFrameMove(device, totalTime, elapsedTime);
                ground00.OnFrameMove(device, totalTime, elapsedTime);
                ground01.OnFrameMove(device, totalTime, elapsedTime);
                ground02.OnFrameMove(device, totalTime, elapsedTime);
                ground10.OnFrameMove(device, totalTime, elapsedTime);
                ground12.OnFrameMove(device, totalTime, elapsedTime);
                ground20.OnFrameMove(device, totalTime, elapsedTime);
                ground21.OnFrameMove(device, totalTime, elapsedTime);
                ground22.OnFrameMove(device, totalTime, elapsedTime);
                //windsock.OnFrameMove(device, totalTime, elapsedTime);
                if (tractor != null)
                    tractor.OnFrameMove(device, totalTime, elapsedTime);
                lensFlare.OnFrameMove(device, totalTime, elapsedTime);
                foreach (Water water in waters)
                    water.OnFrameMove(device, totalTime, elapsedTime);
#if !EDITOR
                if ((Effects.Reflection.Instance != null) &&
                    (Effects.Reflection.ReflectionDetail == RCSim.Effects.Reflection.ReflectionDetailEnum.High))
                        Effects.Reflection.Instance.Invalidate();
#endif
            }
            else
            {
                if (photoScenery != null)
                    photoScenery.OnFrameMove(device, totalTime, elapsedTime);
                if (lensFlare != null)
                    lensFlare.OnFrameMove(device, totalTime, elapsedTime);
            }
        }

        public void OnFrameRender(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            if (parameters.SceneryType == SceneryParameters.SceneryTypeEnum.Full3D)
            {
                sky.OnFrameRender(device, totalTime, elapsedTime);

                foreach (Windmill mill in windmills)
                {
                    mill.OnFrameRender(device, totalTime, elapsedTime);
                }
                foreach (Flag flag in flags)
                {
                    flag.OnFrameRender(device, totalTime, elapsedTime);
                }

#if EDITOR            
            foreach (Gate gate in gates)
            {
                gate.OnFrameRender(device, totalTime, elapsedTime);
            }
#endif
                vegetation.OnFrameRender(device, totalTime, elapsedTime);
                ground.OnFrameRender(device, totalTime, elapsedTime);
                ground00.OnFrameRender(device, totalTime, elapsedTime);
                ground01.OnFrameRender(device, totalTime, elapsedTime);
                ground02.OnFrameRender(device, totalTime, elapsedTime);
                ground10.OnFrameRender(device, totalTime, elapsedTime);
                ground12.OnFrameRender(device, totalTime, elapsedTime);
                ground20.OnFrameRender(device, totalTime, elapsedTime);
                ground21.OnFrameRender(device, totalTime, elapsedTime);
                ground22.OnFrameRender(device, totalTime, elapsedTime);
                
                if (tractor != null)
                    tractor.OnFrameRender(device, totalTime, elapsedTime);

#if EDITOR
                foreach (Water water in waters)
                   water.OnFrameRender(device, totalTime, elapsedTime);
#else

                if ((RCSim.Effects.Reflection.Instance != null) && (!RCSim.Effects.Reflection.Instance.Rendering))
                {
                    foreach (Water water in waters)
                        water.OnFrameRender(device, totalTime, elapsedTime);
                }
#endif
                foreach (SceneryObject sceneryObject in sceneryObjects)
                {
                    sceneryObject.OnFrameRender(device, totalTime, elapsedTime);
                }
                //windsock.OnFrameRender(device, totalTime, elapsedTime);
            }
            else
            {
                if (photoScenery != null)
                    photoScenery.OnFrameRender(device, totalTime, elapsedTime);
            }
        }

        public void OnFrameRenderFinal(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            if (lensFlare != null)
                lensFlare.OnFrameRender(device, totalTime, elapsedTime);
            if (photoScenery != null)
                photoScenery.OnFrameRenderFinal(device, totalTime, elapsedTime);
        }
        #endregion        
    }
}
