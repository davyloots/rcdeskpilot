using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.DirectX;
using System.Windows.Forms;

namespace RCSim.SceneryEditor
{
    internal class TerrainDefinition
    {
        #region Private fields
        private DataSet dataSet;
        private DataTable treeTable;
        #endregion

        #region Public enums
        public enum ObjectTypeEnum
        {
            Tree
        }
        #endregion

        #region Public properties
        public DataTable TreeTable
        {
            get { return treeTable; }
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
            treeTable = new DataTable("Trees");
            treeTable.Columns.Add(new DataColumn("Position", typeof(Vector3)));
            dataSet.Tables.Add(treeTable);
        }
        #endregion

        #region Public methods
        public void AddTree(Vector3 position)
        {
            treeTable.Rows.Add(position);
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
        #endregion
    }
}
