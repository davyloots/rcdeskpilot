using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace RCSim.AircraftEditor.Dialogs
{
    public partial class Graph2Control : Control
    {
        public class KeyValueComparer : IComparer<KeyValuePair<double, double>>
        {
            #region IComparer<KeyValuePair<double, double>> Members
            /// <summary>
            /// Compares two GameObjects 
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public int Compare(KeyValuePair<double, double> x, KeyValuePair<double, double> y)
            {
                if (x.Key < y.Key)
                    return -1;
                else if (x.Key > y.Key)
                    return 1;
                else
                    return 0;
            }
            #endregion
        }

        #region Protected fields
        protected Bitmap backBuffer = null;
        protected List<KeyValuePair<double, double>> valueList = new List<KeyValuePair<double, double>>();
        protected Color backColor = Color.Black;
        protected Color graphColor = Color.Yellow;
        protected Color selectedColor = Color.Red;
        protected Color gridColor = Color.White;
        protected Font gridFont = new Font(FontFamily.GenericSansSerif, 10f);
        protected double maxValue = 6.0;
        protected int selectedValue = -1;
        protected bool mouseDown = false;
        protected Point mouseDownPoint = new Point();
        protected int margin = 35;
        #endregion

        #region Public properties
        /// <summary>
        /// Gets/Sets the list of values.
        /// </summary>
        public List<KeyValuePair<double, double>> ValueList
        {
            get { return valueList; }
            set
            {
                valueList = value;
                this.Invalidate();
            }
        }

        public Color BackgroundColor
        {
            get { return backColor; }
            set { backColor = value; }
        }

        public Color GraphColor
        {
            get { return graphColor; }
            set { graphColor = value; }
        }

        public Color GridColor
        {
            get { return gridColor; }
            set { gridColor = value; }
        }

        public Color SelectedColor
        {
            get { return selectedColor; }
            set { selectedColor = value; }
        }

        public double MaxValue
        {
            get { return maxValue; }
            set { maxValue = value; }
        }
        #endregion

        #region Constructor
        public Graph2Control()
        {
            InitializeComponent();
            this.Resize += new EventHandler(GraphControl_Resize);
        }
        #endregion

        #region Private event handlers
        /// <summary>
        /// Handles the Resize event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GraphControl_Resize(object sender, EventArgs e)
        {
            this.Invalidate();
        }
        #endregion

        #region Overridden Control methods
        /// <summary>
        /// OnPaint
        /// </summary>
        /// <param name="pe"></param>
        protected override void OnPaint(PaintEventArgs pe)
        {
            CultureInfo cultureUs = new CultureInfo("en-US");

            if ((backBuffer == null) || (backBuffer.Size != this.Size))
            {
                if (backBuffer != null)
                {
                    backBuffer.Dispose();
                }
                backBuffer = new Bitmap(this.Width, this.Height);
            }
            Graphics g = Graphics.FromImage(backBuffer);
            g.Clear(BackgroundColor);

            // calculate scale
            double absMaxValue = 100;
            maxValue = absMaxValue*1.2;

            // Draw grid
            Pen gridPen = new Pen(GridColor);
            SolidBrush textBrush = new SolidBrush(GridColor);
            g.DrawLine(gridPen, 0, Height - margin, Width, Height - margin);
            g.DrawLine(gridPen, margin, 0, margin, Height);
            g.DrawString("(0,0)", gridFont, textBrush, (float)0, (float)Height - margin);
            g.DrawLine(gridPen, margin - 2, margin, margin+2, margin);
            g.DrawString("100%", gridFont, textBrush, 0, margin);
            g.DrawLine(gridPen, (float)Width - margin, (float)Height - margin - 2, (float)Width - margin, (float)Height - margin + 2);
            g.DrawString("100m/s", gridFont, textBrush, (float)Width - margin - 15, (float)Height - margin);
            
            // Draw coefficients
            Pen graphPen = new Pen(GraphColor);
            Pen selectedPen = new Pen(selectedColor);
            if (valueList.Count > 0)
            {
                float hMinM = Height - margin;
                g.DrawLine(graphPen, 0, (int)(hMinM - (valueList[0].Value * (Height - 2 * margin))),
                    (int)(valueList[0].Key * (Width - 2 * margin))/100 + margin,
                    (int)(hMinM - (valueList[0].Value * (Height - 2 * margin))));
                
                for (int i = 0; i < valueList.Count - 1; i++)
                {
                    Point p1 = new Point((int)((valueList[i].Key * (Width - 2 * margin)) / 100 + margin),
                        (int)(hMinM - (valueList[i].Value * (Height - 2 * margin))));
                    Point p2 = new Point((int)((valueList[i + 1].Key * (Width - 2 * margin)) / 100 + margin), 
                        (int)(hMinM - (valueList[i + 1].Value * (Height - 2 * margin))));
                    g.DrawLine(graphPen, p1, p2);
                    if (i == selectedValue)
                    {
                        g.DrawRectangle(selectedPen, new Rectangle(p1.X - 2, p1.Y - 2, 4, 4));
                        string label = string.Format("({0},{1}%)", (valueList[i].Key).ToString("F00", cultureUs), 
                            (valueList[i].Value*100f).ToString("F00", cultureUs));
                        Size labelSize = g.MeasureString(label, gridFont).ToSize();
                        g.DrawString(label, gridFont, textBrush, p1.X - labelSize.Width/2, p1.Y - labelSize.Height);
                    }
                    else
                        g.DrawRectangle(graphPen, new Rectangle(p1.X - 2, p1.Y - 2, 4, 4));

                }
                Point p3 = new Point((int)((valueList[valueList.Count - 1].Key * (Width - 2 * margin)) / 100 + margin),
                    (int)(hMinM - (valueList[valueList.Count - 1].Value * (Height - 2 * margin))));
                g.DrawLine(graphPen, p3.X, p3.Y,
                    Width, p3.Y);
                if (selectedValue == valueList.Count - 1)
                {
                    g.DrawRectangle(selectedPen, new Rectangle(p3.X - 2, p3.Y - 2, 4, 4));
                    string label = string.Format("({0},{1}%)", (valueList[selectedValue].Key).ToString("F00", cultureUs),
                            (valueList[selectedValue].Value*100f).ToString("F00", cultureUs));
                    Size labelSize = g.MeasureString(label, gridFont).ToSize();
                    g.DrawString(label, gridFont, textBrush, p3.X - labelSize.Width / 2, p3.Y - labelSize.Height);
                }
                else
                    g.DrawRectangle(graphPen, new Rectangle(p3.X - 2, p3.Y - 2, 4, 4));                 
            }
            gridPen.Dispose();
            textBrush.Dispose();
            graphPen.Dispose();
            selectedPen.Dispose();

            pe.Graphics.DrawImage(backBuffer, 0,0);
            //base.OnPaint(pe);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            //base.OnPaintBackground(pevent);
        }
        #endregion

        private void GraphControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (valueList.Count > 2)
                {
                    // Remove a point?
                    for (int i = 0; i < valueList.Count; i++)
                    {
                        float hMinM = Height - margin;
                        Point p = new Point((int)((valueList[i].Key * (Width - 2 * margin)) / 100 + margin),
                            (int)(hMinM - (valueList[i].Value * (Height - 2 * margin))));
                        if ((Math.Abs(e.X - p.X) < 3) && (Math.Abs(e.Y - p.Y) < 3))
                        {
                            valueList.RemoveAt(i);
                            Invalidate();
                            return;
                        }
                    }
                }
                for (int i = 0; i < valueList.Count - 1; i++)
                {
                    float hMinM = Height - margin;
                    Point p1 = new Point((int)((valueList[i].Key * (Width - 2 * margin)) / 100 + margin),
                        (int)(hMinM - (valueList[i].Value * (Height - 2 * margin))));
                    Point p2 = new Point((int)((valueList[i+1].Key * (Width - 2 * margin)) / 100 + margin),
                        (int)(hMinM - (valueList[i+1].Value * (Height - 2 * margin)))); 
                    if ((e.X >= p1.X) && (e.X < p2.X))
                    {
                        valueList.Insert(i + 1, new KeyValuePair<double, double>((valueList[i].Key + valueList[i + 1].Key) / 2,
                            (valueList[i].Value + valueList[i + 1].Value) / 2));
                        return;
                    }
                }
            }
            else if (e.Button == MouseButtons.Left)
            {
                selectedValue = -1;
                if (valueList.Count > 0)
                {
                    for (int i = 0; i < valueList.Count; i++)
                    {
                        float hMinM = Height - margin;
                        Point p = new Point((int)((valueList[i].Key * (Width - 2 * margin)) / 100 + margin),
                            (int)(hMinM - (valueList[i].Value * (Height - 2 * margin))));
                        if ((Math.Abs(e.X - p.X) < 3) && (Math.Abs(e.Y - p.Y) < 3))
                        {
                            selectedValue = i;
                            mouseDown = true;
                            mouseDownPoint = e.Location;
                            Invalidate();
                            break;
                        }
                    }
                }
            }
        }

        private void GraphControl_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
            valueList.Sort(new KeyValueComparer());
            this.Invalidate();
        }

        private void GraphControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                if ((selectedValue < valueList.Count) && (selectedValue >= 0))
                {
                    double newKey = Math.Max(0, Math.Min(100,
                        valueList[selectedValue].Key + (e.Location.X - mouseDownPoint.X) * (100f/(Width - 2*margin))));
                    double newValue = Math.Max(0, Math.Min(1.05f,
                        valueList[selectedValue].Value - (e.Location.Y - mouseDownPoint.Y) /(Height - 2f*margin)));
                    valueList[selectedValue] = new KeyValuePair<double,double>(newKey, newValue);
                    mouseDownPoint = e.Location;
                    this.Invalidate();
                }
            }
        }
    }
}
