using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RCDeskPilot.API.Sample
{
    public partial class AttitudeControl : Control
    {
        #region private fields
        private float speed = 0f;
        private float roll = 0f;
        private float pitch = 0f;
        private float altitude = 0f;
        private Bitmap backBuffer = null;
        private SolidBrush skyBrush = null;
        private SolidBrush groundBrush = null;
        private SolidBrush whiteBrush = null;
        private Pen whitePen = null;
        private Pen blackPen = null;
        private Font hudFont = null;
        #endregion

        #region Public properties
        /// <summary>
        /// Gets/Sets the pitch angle.
        /// </summary>
        public float Pitch 
        { 
            get { return pitch; }
            set
            {
                pitch = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets/Sets the roll angle.
        /// </summary>
        public float Roll 
        {
            get { return roll; }
            set
            {
                roll = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets/Sets the speed.
        /// </summary>
        public float Speed
        {
            get { return speed; }
            set
            {
                speed = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets/Sets the altitude.
        /// </summary>
        public float Altitude
        {
            get { return altitude; }
            set
            {
                altitude = value;
                Invalidate();
            }
        }
        #endregion

        public AttitudeControl()
        {
            InitializeComponent();

            skyBrush = new SolidBrush(Color.SkyBlue);
            groundBrush = new SolidBrush(Color.Green);
            whiteBrush = new SolidBrush(Color.White);
            whitePen = new Pen(Color.White);
            blackPen = new Pen(Color.Black);
            hudFont = new Font(FontFamily.GenericMonospace, 12f);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
                if (hudFont != null)
                    hudFont.Dispose();
                if (whitePen != null)
                    whitePen.Dispose();
                if (blackPen != null)
                    blackPen.Dispose();
                if (whiteBrush != null)
                    whiteBrush.Dispose();
                if (groundBrush != null)
                    groundBrush.Dispose();
                if (skyBrush != null)
                    skyBrush.Dispose();
                if (backBuffer != null)
                    backBuffer.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Overrides the OnPaint method.
        /// </summary>
        /// <param name="pe"></param>
        protected override void OnPaint(PaintEventArgs pe)
        {
            if ((backBuffer != null) && (this.Size != backBuffer.Size))
            {
                backBuffer.Dispose();
                backBuffer = null;
            }
            if (backBuffer == null)
                backBuffer = new Bitmap(this.Width, this.Height);
            Graphics g = Graphics.FromImage(backBuffer);
            g.Clear(Color.SkyBlue);
            int vcenter = (int)(Pitch*this.Height);
            if (Roll < -Math.PI / 2 || Roll > Math.PI / 2)
                vcenter = (int)(-Pitch * this.Height);
            g.TranslateTransform(Width/2, vcenter + Height/2);
            g.RotateTransform(-Roll * 57.29f);
            g.FillRectangle(groundBrush, new Rectangle(-2*Width, 0, 4*Width, 2*Height));            
            g.ResetTransform();
            g.DrawLine(whitePen, 2*Width / 5, Height / 2, 3*Width / 5, Height / 2);
            
            g.DrawString(string.Format("{0}", speed.ToString("F1")), hudFont, whiteBrush, 
                new RectangleF(Width - 50, Height / 2 - 7, 50, 14));
            g.DrawString(string.Format("{0}", altitude.ToString("F1")), hudFont, whiteBrush,
                new RectangleF(5, Height / 2 - 7, 50, 14));
            g.DrawRectangle(blackPen, new Rectangle(0, 0, Width - 1, Height - 1));
            pe.Graphics.DrawImage(backBuffer, 0, 0);            
        }

        /// <summary>
        /// Overrides the OnPaintBackground method (to avoid flicker.).
        /// </summary>
        /// <param name="pevent"></param>
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            
        }
    }
}
