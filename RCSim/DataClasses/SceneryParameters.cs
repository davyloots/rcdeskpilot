using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using Bonsai.Core;
using Microsoft.DirectX;
using System.Xml.XPath;
using System.Globalization;

namespace RCSim.DataClasses
{
    class SceneryParameters
    {
        #region Protected fields
        protected SceneryTypeEnum sceneryType = SceneryTypeEnum.Full3D;
        protected string sceneryFolder = null;
        #endregion

        #region Public enums
        public enum SceneryTypeEnum
        {
            Full3D,
            Photofield
        }

        #endregion

        #region Public properties
        /// <summary>
        /// Gets/sets the filename containing the aircraft parameters.
        /// </summary>
        public string FileName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the type of scenery.
        /// </summary>
        public SceneryTypeEnum SceneryType
        {
            get { return sceneryType; }
        }

        public string DefinitionFile
        {
            get;
            set;
        }
        public string HeightMapFile
        {
            get;
            set;
        }
        public string SplatHighFile
        {
            get;
            set;
        }
        public string SplatLowFile
        {
            get;
            set;
        }
        public string NormalMapFile
        {
            get;
            set;
        }
        public string Texture1File
        {
            get;
            set;
        }
        public string Texture2File
        {
            get;
            set;
        }
        public string Texture3File
        {
            get;
            set;
        }
        public string Texture4File
        {
            get;
            set;
        }
        public float MinimumHeight
        {
            get;
            set;
        }
        public float MaximumHeight
        {
            get;
            set;
        }
        public int HeightMapSize
        {
            get;
            set;
        }
        public int HeightMapResolution
        {
            get;
            set;
        }
        public Vector3 DefaultStartPosition
        {
            get;
            set;
        }
        public Vector3 WaterStartPosition
        {
            get;
            set;
        }
        public string LeftPhoto
        {
            get;
            set;
        }
        public string LeftDepthMap
        {
            get;
            set;
        }
        public string RightPhoto
        {
            get;
            set;
        }
        public string RightDepthMap
        {
            get;
            set;
        }
        public string FrontPhoto
        {
            get;
            set;
        }
        public string FrontDepthMap
        {
            get;
            set;
        }
        public string BackPhoto
        {
            get;
            set;
        }
        public string BackDepthMap
        {
            get;
            set;
        }
        public string TopPhoto
        {
            get;
            set;
        }
        public string TopDepthMap
        {
            get;
            set;
        }
        public string BottomPhoto
        {
            get;
            set;
        }
        public string BottomDepthMap
        {
            get;
            set;
        }
        /// <summary>
        /// Sets the file containing the parameters.
        /// </summary>
        public string File
        {
            set { ReadParameters(value); }
        }

        public string SceneryFolder
        {
            get { return sceneryFolder; }
        }
        #endregion

        #region Public methods
        public void ReadParameters(string filename)
        {
            FileName = filename;
            sceneryFolder = filename.Substring(0, filename.Length - Utility.GetFileNamePart(filename).Length - 4);
            XPathDocument doc = new XPathDocument(filename);
            XPathNavigator nav = doc.CreateNavigator();
            // Read in the parameters
            this.sceneryType = ReadSceneryType(nav, "//definition//type", SceneryTypeEnum.Full3D);
            this.DefinitionFile = ReadString(nav, "//definition//definition", null);
            this.HeightMapFile = ReadString(nav, "//definition//heightmap", null);
            this.DefaultStartPosition = ReadVector(nav, "//definition//defaultstartposition", new Vector3(0, 0, 0));
            this.WaterStartPosition = ReadVector(nav, "//definition//waterstartposition", new Vector3(0, 0, 0));
            this.MinimumHeight = (float) ReadDouble(nav, "//definition//minimumheight", 0.0);
            this.MaximumHeight = (float) ReadDouble(nav, "//definition//maximumheight", 0.0);
            switch (sceneryType)
            {
                case SceneryTypeEnum.Full3D:
                    this.SplatHighFile = nav.SelectSingleNode("//definition//splathigh").Value;
                    this.SplatLowFile = nav.SelectSingleNode("//definition//splatlow").Value;
                    this.NormalMapFile = nav.SelectSingleNode("//definition//normalmap").Value;
                    this.Texture1File = nav.SelectSingleNode("//definition//texture1").Value;
                    this.Texture2File = nav.SelectSingleNode("//definition//texture2").Value;
                    this.Texture3File = nav.SelectSingleNode("//definition//texture3").Value;
                    this.Texture4File = nav.SelectSingleNode("//definition//texture4").Value;                    
                    break;
                case SceneryTypeEnum.Photofield:
                    this.LeftPhoto = ReadString(nav, "//definition//left", null);
                    this.LeftDepthMap = ReadString(nav, "//definition//leftdepthmap", null);
                    this.RightPhoto = ReadString(nav, "//definition//right", null);
                    this.RightDepthMap = ReadString(nav, "//definition//rightdepthmap", null);
                    this.FrontPhoto = ReadString(nav, "//definition//front", null);
                    this.FrontDepthMap = ReadString(nav, "//definition//frontdepthmap", null);
                    this.BackPhoto = ReadString(nav, "//definition//back", null);
                    this.BackDepthMap = ReadString(nav, "//definition//backdepthmap", null);
                    this.TopPhoto = ReadString(nav, "//definition//top", null);
                    this.TopDepthMap = ReadString(nav, "//definition//topdepthmap", null);
                    this.BottomPhoto = ReadString(nav, "//definition//bottom", null);
                    this.BottomDepthMap = ReadString(nav, "//definition//bottomdepthmap", null);
                    this.HeightMapSize = ReadInt(nav, "//definition//heightmapsize", 1000);
                    this.HeightMapResolution = ReadInt(nav, "//definition//heightmapresolution", 100);
                    break;
            }
        }

       
        #endregion

        #region Private methods
        /// <summary>
        /// Reads a vector.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        private Vector3 ReadVector(string str)
        {
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            provider.NumberGroupSeparator = ",";

            string[] coords = str.Split(',');
            if (coords.Length != 3)
                return new Vector3(0, 0, 0);
            else
                return new Vector3(
                    (float)Convert.ToDouble(coords[0], provider),
                    (float)Convert.ToDouble(coords[1], provider),
                    (float)Convert.ToDouble(coords[2], provider));
        }

        private Vector3 ReadVector(XPathNavigator nav, string tag, Vector3 defaultValue)
        {
            string str = ReadString(nav, tag, null);
            if (string.IsNullOrEmpty(str))
            {
                return defaultValue;
            }
            else
            {
                return ReadVector(str);
            }
        }

        private string ReadString(XPathNavigator nav, string tag, string defaultValue)
        {
            try
            {
                XPathNavigator node = nav.SelectSingleNode(tag);
                if (node != null)
                    return (string)node.Value;
                else
                    return defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        private double ReadDouble(XPathNavigator nav, string tag, double defaultValue)
        {
            try
            {
                XPathNavigator node = nav.SelectSingleNode(tag);
                if (node != null)
                    return Convert.ToDouble(node.Value);
                else
                    return defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        private int ReadInt(XPathNavigator nav, string tag, int defaultValue)
        {
            try
            {
                XPathNavigator node = nav.SelectSingleNode(tag);
                if (node != null)
                    return Convert.ToInt32(node.Value);
                else
                    return defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }


        private SceneryTypeEnum ReadSceneryType(XPathNavigator nav, string tag, SceneryTypeEnum defaultValue)
        {
            try
            {
                XPathNavigator node = nav.SelectSingleNode(tag);
                if (node.Value.ToLower().Equals("photo"))
                    return SceneryTypeEnum.Photofield;
                else
                    return SceneryTypeEnum.Full3D;
            }
            catch
            {
                return defaultValue;
            }
        }
        #endregion
    }
}
