using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX;

namespace RCSim.AircraftEditor.Dialogs
{
    internal partial class VectorControl : UserControl
    {
        #region Protected fields
        protected Vector3 vector = new Vector3(0,0,0);
        protected bool mouseXZDown = false;
        protected bool mouseYDown = false;
        protected int mouseX = 0;
        protected int mouseY = 0;
        protected string toolTip = string.Empty;
        protected static Bitmap upImage = null;
        protected static Bitmap xzImage = null;
        #endregion

        #region Public events
        public event EventHandler VectorChanged;
        #endregion


        #region Public properties
        public Vector3 Vector
        {
            get { return vector; }
            set
            {
                vector = value;
                updateLabel();
            }
        }
        #endregion

        public VectorControl()
        {
            InitializeComponent();

            ToolTip tip = new ToolTip();
            tip.SetToolTip(buttonXZ, "Move the object in the XZ plane, hold SHIFT for smaller movements");
            tip.SetToolTip(buttonY, "Move the object in the Y axis, hold SHIFT for smaller movements");
        }

        static VectorControl()
        {
            try
            {
                if (upImage == null)
                    upImage = new Bitmap("data\\vector_updown_24.png");
                if (xzImage == null)
                    xzImage = new Bitmap("data\\vector2_24.png");
            }
            catch
            {
            }
        }
        
        private void updateLabel()
        {
            labelVector.Text = string.Format("({0};{1};{2})", 
                vector.X.ToString("F03"), vector.Y.ToString("F03"), vector.Z.ToString("F03"));
        }

        private void buttonXZ_MouseDown(object sender, MouseEventArgs e)
        {
            mouseXZDown = true;
            mouseX = e.X;
            mouseY = e.Y;
        }

        private void buttonXZ_MouseUp(object sender, MouseEventArgs e)
        {
            mouseXZDown = false;            
        }

        private void buttonXZ_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseXZDown)
            {
                int diffX = e.X - mouseX;
                int diffY = e.Y - mouseY;
                mouseX = e.X;
                mouseY = e.Y;
                if ((System.Windows.Forms.Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                {
                    if ((System.Windows.Forms.Control.ModifierKeys & Keys.Control) == Keys.Control)
                    {
                        vector.Z += diffY / 1000f;
                    }
                    else
                    {
                        vector.X -= diffX / 1000f;
                        vector.Z += diffY / 1000f;
                    }
                }
                else
                {
                    if ((System.Windows.Forms.Control.ModifierKeys & Keys.Control) == Keys.Control)
                    {
                        vector.Z += diffY / 100f;
                    }
                    else
                    {
                        vector.X -= diffX / 100f;
                        vector.Z += diffY / 100f;
                    }
                }
                updateLabel();
                if (VectorChanged != null)
                    VectorChanged(this, EventArgs.Empty);
            }
        }

        private void buttonY_MouseDown(object sender, MouseEventArgs e)
        {
            mouseYDown = true;
            mouseX = e.X;
            mouseY = e.Y;
        }

        private void buttonY_MouseUp(object sender, MouseEventArgs e)
        {
            mouseYDown = false;
        }

        private void buttonY_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseYDown)
            {
                int diffY = e.Y - mouseY;
                mouseX = e.X;
                mouseY = e.Y;
                if ((System.Windows.Forms.Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                    vector.Y -= diffY / 1000f;
                else
                    vector.Y -= diffY / 100f;
                updateLabel();
                if (VectorChanged != null)
                    VectorChanged(this, EventArgs.Empty);
            }
        }

        private void VectorControl_Load(object sender, EventArgs e)
        {
            buttonY.Image = upImage;
            buttonXZ.Image = xzImage;
        }
    }
}
