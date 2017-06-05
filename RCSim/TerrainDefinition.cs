using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Microsoft.DirectX;
using System.Windows.Forms;
using System.Drawing;

namespace RCSim
{
    internal class TerrainDefinition
    {
        #region Private fields
        private DataSet dataSet;
        private DataTable treeTable;
        private DataTable simpleTreeTable;
        private DataTable simpleTallTreeTable;
        private DataTable simpleSmallTreeTable;
        private DataTable objectTable;
        private DataTable windmillTable;
        private DataTable flagTable;
        private DataTable thermalTable;
        private DataTable gateTable;
        private DataTable pilotPositionTable;
        private DataTable skyTable;
        private DataTable waterTable;
        #endregion

        #region Public enums
        public enum ObjectTypeEnum
        {
            Tree,
            SimpleTree,
            SimpleTallTree,
            SimpleSmallTree,
            SceneryObject,
            Windmill,
            Flag,
            Gate
        }
        #endregion

        #region Public properties
        public DataTable TreeTable
        {
            get { return treeTable; }
        }

        public DataTable SimpleTreeTable
        {
            get { return simpleTreeTable; }
        }

        public DataTable SimpleTallTreeTable
        {
            get { return simpleTallTreeTable; }
        }

        public DataTable SimpleSmallTreeTable
        {
            get { return simpleSmallTreeTable; }
        }

        public DataTable ObjectTable
        {
            get { return objectTable; }
        }

        public DataTable WindmillTable
        {
            get { return windmillTable; }
        }

        public DataTable FlagTable
        {
            get { return flagTable; }
        }

        public DataTable ThermalTable
        {
            get { return thermalTable; }
        }

        public DataTable GateTable
        {
            get { return gateTable; }
        }

        public DataTable PilotPositionTable
        {
            get { return pilotPositionTable; }
        }

        public DataTable SkyTable
        {
            get { return skyTable; }
        }

        public DataTable WaterTable
        {
            get { return waterTable; }
        }
        #endregion

        #region Constructor
        public TerrainDefinition()
        {
            CreateDefinition();   
        }
        #endregion

        #region Private methods
        private void CreateDefinition()
        {
            // Create the dataset
            dataSet = new DataSet("Terrain");

            // Create the tree table
            treeTable = new DataTable("Trees");
            treeTable.Columns.Add(new DataColumn("Position", typeof(Vector3)));
            dataSet.Tables.Add(treeTable);

            simpleTreeTable = new DataTable("SimpleTrees");
            simpleTreeTable.Columns.Add(new DataColumn("Position", typeof(Vector3)));
            dataSet.Tables.Add(simpleTreeTable);

            simpleTallTreeTable = new DataTable("SimpleTallTrees");
            simpleTallTreeTable.Columns.Add(new DataColumn("Position", typeof(Vector3)));
            dataSet.Tables.Add(simpleTallTreeTable);

            simpleSmallTreeTable = new DataTable("SimpleSmallTrees");
            simpleSmallTreeTable.Columns.Add(new DataColumn("Position", typeof(Vector3)));
            dataSet.Tables.Add(simpleSmallTreeTable);

            // Create the object table
            objectTable = new DataTable("Objects");
            objectTable.Columns.Add(new DataColumn("FileName", typeof(string)));
            objectTable.Columns.Add(new DataColumn("Position", typeof(Vector3)));
            objectTable.Columns.Add(new DataColumn("Orientation", typeof(Vector3)));
            dataSet.Tables.Add(objectTable);

            // Create the windmill table
            windmillTable = new DataTable("Windmills");
            windmillTable.Columns.Add(new DataColumn("Position", typeof(Vector3)));
            dataSet.Tables.Add(windmillTable);

            // Create the flag table
            flagTable = new DataTable("Flags");
            flagTable.Columns.Add(new DataColumn("Position", typeof(Vector3)));
            dataSet.Tables.Add(flagTable);

            // Create the thermal table
            thermalTable = new DataTable("Thermals");
            thermalTable.Columns.Add(new DataColumn("Position", typeof(Vector3)));
            thermalTable.Columns.Add(new DataColumn("Strength", typeof(float)));
            thermalTable.Columns.Add(new DataColumn("Size", typeof(float)));
            dataSet.Tables.Add(thermalTable);

            gateTable = new DataTable("Gates");
            gateTable.Columns.Add(new DataColumn("Position", typeof(Vector3)));
            gateTable.Columns.Add(new DataColumn("Orientation", typeof(Vector3)));
            gateTable.Columns.Add(new DataColumn("SequenceNr", typeof(int)));
            gateTable.Columns.Add(new DataColumn("Type", typeof(int)));
            dataSet.Tables.Add(gateTable);

            pilotPositionTable = new DataTable("PilotPositions");
            pilotPositionTable.Columns.Add(new DataColumn("Name", typeof(string)));
            pilotPositionTable.Columns.Add(new DataColumn("Position", typeof(Vector3)));
            pilotPositionTable.Columns.Add(new DataColumn("Water", typeof(bool)));
            pilotPositionTable.Columns.Add(new DataColumn("Map", typeof(string)));
            dataSet.Tables.Add(pilotPositionTable);

            // Create sky table
            skyTable = new DataTable("Skies");
            skyTable.Columns.Add(new DataColumn("Name", typeof(string)));
            skyTable.Columns.Add(new DataColumn("Texture", typeof(string)));
            skyTable.Columns.Add(new DataColumn("SunPosition", typeof(Vector3)));
            skyTable.Columns.Add(new DataColumn("AmbientLight", typeof(Vector3)));
            skyTable.Columns.Add(new DataColumn("SunLight", typeof(Vector3)));
            skyTable.Columns.Add(new DataColumn("TerrainAmbient", typeof(float)));
            skyTable.Columns.Add(new DataColumn("TerrainSun", typeof(float)));
            dataSet.Tables.Add(skyTable);

            // Create water table
            waterTable = new DataTable("Water");
            waterTable.Columns.Add(new DataColumn("Position", typeof(Vector3)));
            waterTable.Columns.Add(new DataColumn("Size", typeof(float)));
            dataSet.Tables.Add(waterTable);
        }
        #endregion

        #region Public methods
        public void AddTree(Vector3 position)
        {
            treeTable.Rows.Add(position);
        }

        public void AddSimpleTree(Vector3 position)
        {
            simpleTreeTable.Rows.Add(position);
        }

        public void AddSimpleTallTree(Vector3 position)
        {
            simpleTallTreeTable.Rows.Add(position);
        }

        public void AddSimpleSmallTree(Vector3 position)
        {
            simpleSmallTreeTable.Rows.Add(position);
        }

        public void AddObject(string fileName, Vector3 position, Vector3 orientation)
        {
            objectTable.Rows.Add(fileName, position, orientation);
        }

        public void AddWindmill(Vector3 position)
        {
            windmillTable.Rows.Add(position);
        }

        public void AddFlag(Vector3 position)
        {
            flagTable.Rows.Add(position);
        }

        public void AddGate(Vector3 position, Vector3 orientation, int sequenceNr, int type)
        {
            gateTable.Rows.Add(position, orientation, sequenceNr, type);
        }

        public void AddPilotPosition(string name, Vector3 position, bool water, string map)
        {
            pilotPositionTable.Rows.Add(name, position, water, map);
        }

        public void AddSky(string name, string texture, Vector3 sunPosition, Color ambientLight, Color sunLight, float terrainLight)
        {
            skyTable.Rows.Add(name, texture, sunPosition, 
                new Vector3(ambientLight.R/255f, ambientLight.G/255f, ambientLight.B/255f),
                new Vector3(sunLight.R/255f, sunLight.G/255f, sunLight.B/255f),
                terrainLight);
        }

        public void AddWater(Vector3 position, float size)
        {
            waterTable.Rows.Add(position, size);
        }

        public void Load(string filename)
        {
            try
            {
                dataSet.ReadXml(filename);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void Save(string filename)
        {
            try
            {
                dataSet.WriteXml(filename);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public DataRow GetNearestObject(Vector3 position, out ObjectTypeEnum objectType)
        {
            DataRow currentRow = null;
            float currentDistance = 10000f;
            objectType = ObjectTypeEnum.Tree;
            foreach (DataRow row in TreeTable.Rows)
            {
                float distance = ((Vector3)(row["Position"]) - position).Length();
                if (distance < currentDistance)
                {
                    currentDistance = distance;
                    currentRow = row;
                    objectType = ObjectTypeEnum.Tree;
                }
            }
            foreach (DataRow row in simpleTreeTable.Rows)
            {
                float distance = ((Vector3)(row["Position"]) - position).Length();
                if (distance < currentDistance)
                {
                    currentDistance = distance;
                    currentRow = row;
                    objectType = ObjectTypeEnum.SimpleTree;
                }
            }
            foreach (DataRow row in simpleTallTreeTable.Rows)
            {
                float distance = ((Vector3)(row["Position"]) - position).Length();
                if (distance < currentDistance)
                {
                    currentDistance = distance;
                    currentRow = row;
                    objectType = ObjectTypeEnum.SimpleTallTree;
                }
            }
            foreach (DataRow row in simpleSmallTreeTable.Rows)
            {
                float distance = ((Vector3)(row["Position"]) - position).Length();
                if (distance < currentDistance)
                {
                    currentDistance = distance;
                    currentRow = row;
                    objectType = ObjectTypeEnum.SimpleSmallTree;
                }
            }
            foreach (DataRow row in objectTable.Rows)
            {
                float distance = ((Vector3)(row["Position"]) - position).Length();
                if (distance < currentDistance)
                {
                    currentDistance = distance;
                    currentRow = row;
                    objectType = ObjectTypeEnum.SceneryObject;
                }
            }
            foreach (DataRow row in windmillTable.Rows)
            {
                float distance = ((Vector3)(row["Position"]) - position).Length();
                if (distance < currentDistance)
                {
                    currentDistance = distance;
                    currentRow = row;
                    objectType = ObjectTypeEnum.Windmill;
                }
            }
            foreach (DataRow row in flagTable.Rows)
            {
                float distance = ((Vector3)(row["Position"]) - position).Length();
                if (distance < currentDistance)
                {
                    currentDistance = distance;
                    currentRow = row;
                    objectType = ObjectTypeEnum.Flag;
                }
            }
            foreach (DataRow row in gateTable.Rows)
            {
                float distance = ((Vector3)(row["Position"]) - position).Length();
                if (distance < currentDistance)
                {
                    currentDistance = distance;
                    currentRow = row;
                    objectType = ObjectTypeEnum.Gate;
                }
            }

            return currentRow;
        }


        public void RemoveObject(ObjectTypeEnum objectType, DataRow row)
        {
            switch (objectType)
            {
                case ObjectTypeEnum.Tree:
                    treeTable.Rows.Remove(row);
                    break;
                case ObjectTypeEnum.SimpleTree:
                    simpleTreeTable.Rows.Remove(row);
                    break;
                case ObjectTypeEnum.SimpleTallTree:
                    simpleTallTreeTable.Rows.Remove(row);
                    break;
                case ObjectTypeEnum.SimpleSmallTree:
                    simpleSmallTreeTable.Rows.Remove(row);
                    break;
                case ObjectTypeEnum.SceneryObject:
                    objectTable.Rows.Remove(row);
                    break;
                case ObjectTypeEnum.Windmill:
                    windmillTable.Rows.Remove(row);
                    break;
                case ObjectTypeEnum.Flag:
                    flagTable.Rows.Remove(row);
                    break;
                case ObjectTypeEnum.Gate:
                    gateTable.Rows.Remove(row);
                    break;
            }
        }
        #endregion
    }
}
