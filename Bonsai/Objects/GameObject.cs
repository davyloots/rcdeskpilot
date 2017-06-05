using System;
using System.Collections.Generic;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Bonsai.Core;
using Bonsai.Core.Interfaces;
using Bonsai.Objects.Meshes;
using Bonsai.Objects.Shaders;

namespace Bonsai.Objects
{
    public class GameObject : IFrameworkCallback, IDisposable, ITransparentObject
    {
        #region Public enums
        public enum OrientationModeEnum
        {
            XYZ,
            YXZ,
            Axis,
            YawPitchRoll,
            Quaternion
        }
        #endregion

        #region Private&Protected fields
        private string name = string.Empty;
        private Matrix transformMatrix = Matrix.Identity;
        private GameObject parent = null;
        protected List<GameObject> children = new List<GameObject>();
        private MeshBase mesh = null;
        private ShaderBase shader = null;

        // Location, scale, attitude
        private Vector3 position = new Vector3();
        private Vector3 scale = new Vector3(1f, 1f, 1f);
        private OrientationModeEnum orientationMode = OrientationModeEnum.XYZ;
        private Vector3 orientation = new Vector3();
        private Vector3 front = new Vector3();
        private Vector3 up = new Vector3();
        private float rotationAngle = 0.0f;
        private bool matrixInvalid = true;
        private bool isBillboard = false;
        private bool scaleSet = false;
        protected bool visible = true;
        #endregion

        #region Private constants
        private static Vector3 XAXIS = new Vector3(1, 0, 0);
        private static Vector3 YAXIS = new Vector3(0, 1, 0);
        private static Vector3 ZAXIS = new Vector3(0, 0, 1);
        #endregion

        #region Public events
        /// <summary>
        /// Raised when the visibility has changed.
        /// </summary>
        public event EventHandler VisibleChanged;
        #endregion

        #region Internal properties
        /// <summary>
        /// Gets the transformation matrix.
        /// </summary>
        internal Matrix TransformMatrix
        {
            get { return transformMatrix; }
        }
        #endregion

        #region Public properties
        /// <summary>
        /// Gets/Sets the name of the object.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Gets/Sets the parent of the GameObject.
        /// </summary>
        public GameObject Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        /// <summary>
        /// Gets/Sets the mesh of the GameObject.
        /// </summary>
        public MeshBase Mesh
        {
            get { return mesh; }
            set { mesh = value; }
        }

        /// <summary>
        /// Gets/Sets the Shader to apply to the object.
        /// </summary>
        public ShaderBase Shader
        {
            get { return shader; }
            set { shader = value; }
        }

        /// <summary>
        /// Gets/Sets the location of the object.
        /// </summary>
        public Vector3 Position
        {
            get { return position; }
            set 
            { 
                position = value;
                matrixInvalid = true;
            }
        }

        /// <summary>
        /// Gets/Sets the orientation.
        /// </summary>
        public Vector3 Orientation
        {
            get { return orientation; }
            set { orientation = value; }
        }

        /// <summary>
        /// Gets/sets the orientationmode.
        /// </summary>
        public OrientationModeEnum OrientationMode
        {
            get { return orientationMode; }
            set { orientationMode = value; }
        }

        /// <summary>
        /// Gets/Sets the scale of the object in 3 directions.
        /// </summary>
        public Vector3 Scale
        {
            get { return scale; }
            set 
            { 
                scale = value;
                if ((value.X != 1.0) || (value.Y != 1.0) || (value.Z != 1.0))
                    scaleSet = true;
                matrixInvalid = true;
            }
        }

        /// <summary>
        /// Gets/Sets the rotation angle in Rads around the X axis.
        /// </summary>
        public float RotateXAngle
        {
            get { return orientation.X; }
            set 
            { 
                orientation.X = value;
                orientationMode = OrientationModeEnum.XYZ;
                matrixInvalid = true;
            }
        }

        /// <summary>
        /// Gets/Sets the rotation angle in Rads around the Y axis.
        /// </summary>
        public float RotateYAngle
        {
            get { return orientation.Y; }
            set 
            { 
                orientation.Y = value;
                orientationMode = OrientationModeEnum.XYZ;
                matrixInvalid = true;
            }
        }

        /// <summary>
        /// Gets/Sets the rotation angle in Rads around the X axis.
        /// </summary>
        public float RotateZAngle
        {
            get { return orientation.Y; }
            set 
            { 
                orientation.Z = value;
                orientationMode = OrientationModeEnum.XYZ;
                matrixInvalid = true;
            }
        }

        /// <summary>
        /// Gets/Sets the axis around which the rotation occurs with angle RotationAngle
        /// </summary>
        public Vector3 RotationAxis
        {
            get { return orientation; }
            set
            {
                orientation = value;
                orientationMode = OrientationModeEnum.Axis;
                matrixInvalid = true;
            }
        }

        /// <summary>
        /// Gets/Sets the angle to rotate around the RotationAxis.
        /// </summary>
        public float RotationAngle
        {
            get { return rotationAngle; }
            set
            {
                rotationAngle = value;
                orientationMode = OrientationModeEnum.Axis;
                matrixInvalid = true;
            }
        }

        /// <summary>
        /// Gets/Sets the Yaw/Pitch/Roll angles.
        /// </summary>
        public Vector3 YawPitchRoll
        {
            get { return orientation; }
            set
            {
                orientation = value;
                orientationMode = OrientationModeEnum.YawPitchRoll;
                matrixInvalid = true;
            }
        }

        /// <summary>
        /// Gets the unit vector facing front.
        /// </summary>
        public Vector3 Front
        {
            get { return front; }
        }

        /// <summary>
        /// Gets the unit vector facing up.
        /// </summary>
        public Vector3 Up
        {
            get { return up; }
        }

        /// <summary>
        /// Gets/Sets whether the mesh is a billboard.
        /// </summary>
        public bool IsBillboard
        {
            get { return isBillboard; }
            set { isBillboard = value; }
        }

        /// <summary>
        /// Gets/Sets whether the object is visible (and should be rendered).
        /// </summary>
        public bool Visible
        {
            get { return visible; }
            set 
            {
                if (visible != value)
                {
                    visible = value;
                    if (VisibleChanged != null)
                        VisibleChanged(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets/Sets the list of child GameObjects.
        /// </summary>
        public List<GameObject> Children
        {
            get { return children; }
            set { children = value; }
        }

        #region ITransparentObject Members
        /// <summary>
        /// Gets the (non-relative) position.
        /// </summary>
        public Vector3 WorldPosition
        {
            get 
            {
                if (Parent != null)
                    return Parent.WorldPosition + Position;
                else
                    return Position;
            }
        }

        #endregion
        #endregion

        #region Protected properties
        protected Matrix InheritedTransformMatrix
        {
            get
            {
                Matrix inheritedTransform = transformMatrix;
                if (parent != null)
                    inheritedTransform.Multiply(parent.InheritedTransformMatrix);
                return inheritedTransform;
            }
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Clean up.
        /// </summary>
        public virtual void Dispose()
        {
            foreach (GameObject child in children)
            {
                child.Dispose();
            }
            children.Clear();
        }
        #endregion


        #region IFrameworkCallback Members
        public virtual void OnFrameMove(Device device, double totalTime, float elapsedTime)
        {
            if (isBillboard)
            {
                if (Framework.Instance.CurrentCamera != null)
                {
                    Vector3 diff = this.Position - Framework.Instance.CurrentCamera.LookFrom;
                    float angle = (float)Math.Atan2(diff.X, diff.Z);
                    RotateYAngle = angle;
                }
            }
            Matrix rotationMatrix = Matrix.Identity;
            switch (orientationMode)
            {
                case OrientationModeEnum.XYZ:
                    rotationMatrix.Multiply(Matrix.RotationX(orientation.X));
                    rotationMatrix.Multiply(Matrix.RotationY(orientation.Y));
                    rotationMatrix.Multiply(Matrix.RotationZ(orientation.Z));
                    break;
                case OrientationModeEnum.Axis:
                    rotationMatrix.Multiply(Matrix.RotationAxis(orientation, rotationAngle));
                    break;
                case OrientationModeEnum.YawPitchRoll:
                    rotationMatrix.Multiply(Matrix.RotationYawPitchRoll(orientation.X, orientation.Y, orientation.Z));
                    break;
                case OrientationModeEnum.Quaternion:
                    // TODO
                    break;
                case OrientationModeEnum.YXZ:
                    rotationMatrix.Multiply(Matrix.RotationY(orientation.Y));
                    rotationMatrix.Multiply(Matrix.RotationX(orientation.X));
                    rotationMatrix.Multiply(Matrix.RotationZ(orientation.Z));
                    break;
            }
            front = Vector3.TransformCoordinate(-ZAXIS, rotationMatrix);
            up = Vector3.TransformCoordinate(YAXIS, rotationMatrix);

            // Forward to children
            foreach (GameObject child in children)
            {
                child.OnFrameMove(device, totalTime, elapsedTime);
            }
        }

        public virtual void OnFrameRender(Device device, double totalTime, float elapsedTime)
        {
            if (Visible == false)
                return;
            if (matrixInvalid)
            {
                // scale
                transformMatrix = Matrix.Scaling(scale);

                // Rotate
                switch (orientationMode)
                {
                    case OrientationModeEnum.XYZ:
                        transformMatrix.Multiply(Matrix.RotationX(orientation.X));
                        transformMatrix.Multiply(Matrix.RotationY(orientation.Y));
                        transformMatrix.Multiply(Matrix.RotationZ(orientation.Z));
                        break;
                    case OrientationModeEnum.Axis:                        
                        transformMatrix.Multiply(Matrix.RotationAxis(orientation, rotationAngle));
                        break;
                    case OrientationModeEnum.YawPitchRoll:
                        transformMatrix.Multiply(Matrix.RotationYawPitchRoll(orientation.X, orientation.Y, orientation.Z));
                        break;
                    case OrientationModeEnum.Quaternion:
                        // TODO
                        break;
                    case OrientationModeEnum.YXZ:
                        transformMatrix.Multiply(Matrix.RotationY(orientation.Y));
                        transformMatrix.Multiply(Matrix.RotationX(orientation.X));
                        transformMatrix.Multiply(Matrix.RotationZ(orientation.Z));
                        break;
                }
                // Translate
                transformMatrix.Multiply(Matrix.Translation(position));
                matrixInvalid = false;
            }
            device.Transform.World = InheritedTransformMatrix;
            /*
            if (parent != null)
            {
                Matrix inheritedTransform = transformMatrix;
                inheritedTransform.Multiply(parent.transformMatrix);
                device.Transform.World = inheritedTransform;
            }
            else
            {
                // Move
                device.Transform.World = transformMatrix;
            }
            */

            // Set renderstates
            if (scaleSet)
                device.RenderState.NormalizeNormals = true; // this will make lighting scale invariant

            // Render
            if (mesh != null)
            {
                if (shader != null)
                {
                    shader.WorldMatrix = InheritedTransformMatrix;                    
                    shader.SetGlobalParameters();                    
                    
                    int passes = shader.Effect.Begin(0);
                    for (int iPass = 0; iPass < passes; iPass++)
                    {
                        shader.Effect.BeginPass(iPass);
                        mesh.OnFrameRender(device, totalTime, elapsedTime);
                        shader.Effect.EndPass();
                    }
                    shader.Effect.End();
                }
                else
                    mesh.OnFrameRender(device, totalTime, elapsedTime);
            }
            
            // Reset transformation
            device.Transform.World = Matrix.Identity;

            // Forward to children
            foreach (GameObject child in children)
            {
                child.OnFrameRender(device, totalTime, elapsedTime);
            }

            // Reset renderstates
            if (scaleSet)
                device.RenderState.NormalizeNormals = false;
        }
        #endregion

        #region Public methods
        public virtual void OnRenderShadow(Device device, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 lightDir)
        {
            if (Visible == false)
                return;
            if (matrixInvalid)
            {
                // scale
                transformMatrix = Matrix.Scaling(scale);
                // Rotate
                switch (orientationMode)
                {
                    case OrientationModeEnum.XYZ:
                        transformMatrix.Multiply(Matrix.RotationX(orientation.X));
                        transformMatrix.Multiply(Matrix.RotationY(orientation.Y));
                        transformMatrix.Multiply(Matrix.RotationZ(orientation.Z));
                        break;
                    case OrientationModeEnum.Axis:
                        transformMatrix.Multiply(Matrix.RotationAxis(orientation, rotationAngle));
                        break;
                    case OrientationModeEnum.YawPitchRoll:
                        transformMatrix.Multiply(Matrix.RotationYawPitchRoll(orientation.X, orientation.Y, orientation.Z));
                        break;
                    case OrientationModeEnum.Quaternion:
                        // TODO
                        break;
                    case OrientationModeEnum.YXZ:
                        transformMatrix.Multiply(Matrix.RotationY(orientation.Y));
                        transformMatrix.Multiply(Matrix.RotationX(orientation.X));
                        transformMatrix.Multiply(Matrix.RotationZ(orientation.Z));
                        break;
                }
                // Translate
                transformMatrix.Multiply(Matrix.Translation(position));
                matrixInvalid = false;
            }
            /*
            Matrix newTransformMatrix = transformMatrix;
            if (parent != null)
            {
                newTransformMatrix.Multiply(parent.transformMatrix);                
            }
             */
            Matrix newTransformMatrix = InheritedTransformMatrix;

            Plane plane = Plane.FromPoints(p1, p2, p3);
            Matrix shadowMat = new Matrix();
            shadowMat.Shadow(new Vector4(lightDir.X, lightDir.Y, lightDir.Z, 0f), plane);
            newTransformMatrix.Multiply(shadowMat);

            // Move
            device.Transform.World = newTransformMatrix;
            
            // Render
            if (mesh != null)
            {
                if (mesh is XMesh)
                {
                    XMesh xmesh = mesh as XMesh;
                    xmesh.OnRenderShadow(device);
                }
            }
            // Reset transformation
            device.Transform.World = Matrix.Identity;

            // Forward to children
            foreach (GameObject child in children)
            {
                child.OnRenderShadow(device, p1, p2, p3, lightDir);
            }


            
        }

        /// <summary>
        /// Adds a child object.
        /// </summary>
        /// <param name="child"></param>
        public void AddChild(GameObject child)
        {
            this.children.Add(child);
            child.Parent = this;
        }

        /// <summary>
        /// Removes a child object.
        /// </summary>
        /// <param name="child"></param>
        public void RemoveChild(GameObject child)
        {
            if (this.children.Contains(child))
            {
                child.Parent = null;
                this.children.Remove(child);
            }
        }
        #endregion
    }
}
