using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Bonsai.Objects;

namespace RCSim.AircraftEditor.Dialogs
{
    internal partial class ToolboxForm : Form
    {
        protected ModelControl modelControl = null;

        public AirplaneModel AirplaneModel
        {
            get { return modelControl.AirplaneModel; }
        }

        public ModelControl ModelControl
        {
            get { return modelControl; }
            set 
            { 
                modelControl = value;
                aircraftPanelControl.ModelControl = modelControl;
                soundControl.ModelControl = modelControl;
                collisionControl.ModelControl = modelControl;
                miscellaneousPanelControl.ModelControl = modelControl;
                BuildTreeView(modelControl.AirplaneModel);
                if (modelControl.AirplaneModel.AirplaneControl.AircraftParameters.FlightModelType == RCSim.DataClasses.AircraftParameters.FlightModelTypeEnum.Aircraft)
                {
                    aircraftPanelControl.Visible = true;
                }
                else // helicopter
                {
                    aircraftPanelControl.Visible = false;
                }
            }
        }

        public GameObject CurrentObject
        {
            get
            {
                if (TreeViewHierarchy.SelectedNode != null)
                    return TreeViewHierarchy.SelectedNode.Tag as GameObject;
                return null;
            }
        }

        public ToolboxForm()
        {
            InitializeComponent();
        }

        private void TreeViewHierarchy_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeViewHierarchy.SelectedNode = e.Node;
            surfacePropertiesControl.SetControlSurface(e.Node.Tag);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TreeViewHierarchy.SelectedNode != null)
            {
                ControlSurface surface = TreeViewHierarchy.SelectedNode.Tag as ControlSurface;
                AirplaneModel airplane = TreeViewHierarchy.Nodes[0].Tag as AirplaneModel;
                if ((surface != null) && (airplane != null))
                {
                    ControlSurface parentSurface = surface.Parent as ControlSurface;
                    if (parentSurface != null)
                    {
                        foreach (RCSim.DataClasses.AircraftParameters.ControlSurface child in parentSurface.SurfaceDefinition.ChildControlSurfaces)
                        {
                            if (child == surface.SurfaceDefinition)
                            {
                                parentSurface.SurfaceDefinition.ChildControlSurfaces.Remove(child);
                                if (parentSurface.SurfaceDefinition.ChildControlSurfaces.Count == 0)
                                {
                                    parentSurface.SurfaceDefinition.ChildControlSurfaces = null;
                                }
                                break;
                            }
                        }
                    }
                    else if (surface.Parent == airplane)
                    {
                        foreach (RCSim.DataClasses.AircraftParameters.ControlSurface child in airplane.AirplaneControl.AircraftParameters.ControlSurfaces)
                        {
                            if (child == surface.SurfaceDefinition)
                            {
                                airplane.AirplaneControl.AircraftParameters.ControlSurfaces.Remove(child);
                                break;
                            }                           
                        }
                    }
                    surface.Parent.RemoveChild(surface);
                    surface.Dispose();
                    BuildTreeView(airplane);
                }
            }
        }

        private void addChildItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TreeViewHierarchy.SelectedNode != null)
            {
                ControlSurface surface = TreeViewHierarchy.SelectedNode.Tag as ControlSurface;
                AirplaneModel airplane = TreeViewHierarchy.Nodes[0].Tag as AirplaneModel;
                if (surface != null)
                {
                    RCSim.DataClasses.AircraftParameters.ControlSurface surfaceDef = new RCSim.DataClasses.AircraftParameters.ControlSurface();
                    if (surface.SurfaceDefinition.ChildControlSurfaces == null)
                        surface.SurfaceDefinition.ChildControlSurfaces = new List<RCSim.DataClasses.AircraftParameters.ControlSurface>();
                    surface.SurfaceDefinition.ChildControlSurfaces.Add(surfaceDef);
                    ControlSurface newSurface = new ControlSurface(surfaceDef, surface.AirplaneControl);
                    surface.AddChild(newSurface);
                }
                else if (airplane != null)
                {
                    RCSim.DataClasses.AircraftParameters.ControlSurface surfaceDef = new RCSim.DataClasses.AircraftParameters.ControlSurface();
                    ModelControl.AirplaneModel.AirplaneControl.AircraftParameters.ControlSurfaces.Add(surfaceDef);
                    ControlSurface newSurface = new ControlSurface(surfaceDef, airplane.AirplaneControl);
                    airplane.AddChild(newSurface);
                }
                BuildTreeView(airplane);
            }
        }

        protected void BuildTreeView(AirplaneModel airplaneModel)
        {
            TreeViewHierarchy.Nodes.Clear();
            if (airplaneModel != null)
            {
                TreeNode rootNode = new TreeNode(GetNodeName(airplaneModel.AirplaneControl.AircraftParameters.FixedMesh));
                rootNode.Tag = airplaneModel;
                TreeViewHierarchy.Nodes.Add(rootNode);
                AddChildren(airplaneModel, rootNode);
            }
            TreeViewHierarchy.ExpandAll();
        }

        protected void AddChildren(GameObject gameObject, TreeNode treeNode)
        {
            foreach (GameObject child in gameObject.Children)
            {
                ControlSurface surface = child as ControlSurface;
                if (surface != null)
                {
                    TreeNode node;
                    if (surface.Name != null)
                        node = new TreeNode(GetNodeName(surface.Name));
                    else
                        node = new TreeNode("empty");
                    node.Tag = surface;
                    AddChildren(surface, node);
                    treeNode.Nodes.Add(node);
                }
            }
        }

        private void checkBoxFlying_CheckedChanged(object sender, EventArgs e)
        {
            if (modelControl != null)
                modelControl.Flying = checkBoxFlying.Checked;
        }

        private void surfacePropertiesControl_FileChanged(object sender, EventArgs e)
        {
            if (TreeViewHierarchy.SelectedNode != null)
            {
                ControlSurface surface = TreeViewHierarchy.SelectedNode.Tag as ControlSurface;
                AirplaneModel airplane = TreeViewHierarchy.Nodes[0].Tag as AirplaneModel;
                if (surface != null)
                {
                    TreeViewHierarchy.SelectedNode.Text = GetNodeName(surface.MeshFileName);
                }
                else if (airplane != null)
                {
                    TreeViewHierarchy.SelectedNode.Text = GetNodeName(airplane.MeshFileName);
                }
            }
        }

        private string GetNodeName(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                return "empty";
            string result = filename;
            if (filename.LastIndexOf('/') > -1)
            {
                result = result.Substring(result.LastIndexOf('/')+1);
            }
            if (filename.LastIndexOf('.') > -1)
            {
                result = result.Substring(0, result.LastIndexOf('.'));
            }
            return result;
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.Instance.CollisionPointsVisible = (tabControl.SelectedTab == tabPageCollision);
        }

        private void aircraftPanelControl_TestFlyChanged(object sender, EventArgs e)
        {
            if (modelControl != null)
                modelControl.Flying = aircraftPanelControl.TestFlyEnabled;
        }
    }
}
