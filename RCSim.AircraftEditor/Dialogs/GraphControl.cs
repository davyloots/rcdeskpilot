using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using Bonsai.Core;

namespace RCSim.AircraftEditor.Dialogs
{
    public partial class GraphControl : Control
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
        protected List<KeyValuePair<double, double>> backgroundValueList = null;
        protected Color backColor = Color.Black;
        protected Color graphColor = Color.Yellow;
        protected Color selectedColor = Color.Red;
        protected Color gridColor = Color.White;
        protected Font gridFont = new Font(FontFamily.GenericSansSerif, 10f);
        protected double maxValue = 6.0;
        protected int selectedValue = -1;
        protected bool mouseDown = false;
        protected Point mouseDownPoint = new Point();
        protected double verticalZoom = 1.0;
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

        public List<KeyValuePair<double, double>> BackgroundValueList
        {
            get { return backgroundValueList; }
            set
            {
                backgroundValueList = value;
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

        public double VerticalZoom
        {
            get { return verticalZoom; }
            set 
            { 
                verticalZoom = value;
                this.Invalidate();
            }
        }

        public bool ShowInduced
        {
            get;
            set;
        }

        public List<KeyValuePair<double, double>> Lift
        {
            get;
            set;
        }

        public List<KeyValuePair<double, double>> Drag
        {
            get;
            set;
        }
        #endregion

        #region Constructor
        public GraphControl()
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
            double absMaxValue = 1;
            foreach (KeyValuePair<double, double> keyValue in valueList)
            {
                if (Math.Abs(keyValue.Value) > absMaxValue)
                    absMaxValue = Math.Abs(keyValue.Value);
            }
            maxValue = absMaxValue*2.4/VerticalZoom;

            // Draw grid
            Pen gridPen = new Pen(GridColor);
            SolidBrush textBrush = new SolidBrush(GridColor);
            g.DrawLine(gridPen, 0, Height/2, Width, Height/2);
            g.DrawLine(gridPen, Width / 2, 0, Width / 2, Height);
            g.DrawString("(0,0)", gridFont, textBrush, (float)Width / 2, (float)Height / 2);
            Point pMaxPos = new Point(Width / 2, (int)((0.5 - absMaxValue / maxValue) * Height));
            Point pMaxNeg = new Point(Width/2, (int)((0.5 + absMaxValue / maxValue)*Height));
            g.DrawLine(gridPen, pMaxPos.X - 2, pMaxPos.Y, pMaxPos.X + 2, pMaxPos.Y);
            g.DrawLine(gridPen, pMaxNeg.X - 2, pMaxNeg.Y, pMaxNeg.X + 2, pMaxNeg.Y);
            Size tagSize = g.MeasureString(maxValue.ToString("F02", new CultureInfo("en-US")), gridFont).ToSize(); ;
            g.DrawString(absMaxValue.ToString("F02", cultureUs), gridFont, textBrush, pMaxPos.X - tagSize.Width, pMaxPos.Y);
            g.DrawString((-absMaxValue).ToString("F02", cultureUs), gridFont, textBrush, pMaxNeg.X, pMaxNeg.Y);
            g.DrawLine(gridPen, Width / 4, Height / 2 - 2, Width / 4, Height / 2 + 2);
            g.DrawLine(gridPen, 3 * Width / 4, Height / 2 - 2, 3 * Width / 4, Height / 2 + 2);
            tagSize = g.MeasureString("-90°", gridFont).ToSize();
            g.DrawString("-90°", gridFont, textBrush, Width / 4 - tagSize.Width / 2, Height/2 + 2);
            g.DrawString("+90°", gridFont, textBrush, 3 * Width / 4 - tagSize.Width / 2, Height/2 + 2);
            
            // Draw induced drag
            if (ShowInduced)
                PaintInduced(g);

            // Draw background coefficients
            if (backgroundValueList != null)
            {
                // Draw coefficients
                Pen bgPen = new Pen(Color.Gray);
                if (backgroundValueList.Count > 0)
                {
                    g.DrawLine(bgPen, 0, (int)((0.5 - backgroundValueList[0].Value / maxValue) * Height),
                        (int)((backgroundValueList[0].Key / (2 * Math.PI) + 0.5) * Width), (int)((0.5 - backgroundValueList[0].Value / maxValue) * Height));
                    for (int i = 0; i < backgroundValueList.Count - 1; i++)
                    {
                        Point p1 = new Point((int)((backgroundValueList[i].Key / (2 * Math.PI) + 0.5) * Width), (int)((0.5 - backgroundValueList[i].Value / maxValue) * Height));
                        Point p2 = new Point((int)((backgroundValueList[i + 1].Key / (2 * Math.PI) + 0.5) * Width), (int)((0.5 - backgroundValueList[i + 1].Value / maxValue) * Height));
                        g.DrawLine(bgPen, p1, p2);
                        g.DrawRectangle(bgPen, new Rectangle(p1.X - 2, p1.Y - 2, 4, 4));
                    }
                    Point p3 = new Point((int)((backgroundValueList[backgroundValueList.Count - 1].Key / (2 * Math.PI) + 0.5) * Width), (int)((0.5 - backgroundValueList[backgroundValueList.Count - 1].Value / maxValue) * Height));
                    g.DrawLine(bgPen, p3.X, p3.Y,
                        Width, (int)((0.5 - backgroundValueList[backgroundValueList.Count - 1].Value / maxValue) * Height));
                    g.DrawRectangle(bgPen, new Rectangle(p3.X - 2, p3.Y - 2, 4, 4));
                }
                bgPen.Dispose();
            }


            // Draw coefficients
            Pen graphPen = new Pen(GraphColor);
            Pen selectedPen = new Pen(selectedColor);
            if (valueList.Count > 0)
            {                
                g.DrawLine(graphPen, 0, (int)((0.5 - valueList[0].Value / maxValue) * Height), 
                    (int)((valueList[0].Key / (2*Math.PI) + 0.5) * Width), (int)((0.5 - valueList[0].Value / maxValue) * Height));
                for (int i = 0; i < valueList.Count - 1; i++)
                {
                    Point p1 = new Point((int)((valueList[i].Key / (2*Math.PI) + 0.5) * Width), (int)((0.5 - valueList[i].Value / maxValue) * Height));
                    Point p2 = new Point((int)((valueList[i+1].Key / (2*Math.PI) + 0.5) * Width), (int)((0.5 - valueList[i+1].Value / maxValue) * Height));
                    g.DrawLine(graphPen, p1, p2);
                    if (i == selectedValue)
                    {
                        g.DrawRectangle(selectedPen, new Rectangle(p1.X - 2, p1.Y - 2, 4, 4));
                        string label = string.Format("({0},{1})", (valueList[i].Key*180/Math.PI).ToString("F02", cultureUs), 
                            valueList[i].Value.ToString("F02", cultureUs));
                        Size labelSize = g.MeasureString(label, gridFont).ToSize();
                        g.DrawString(label, gridFont, textBrush, p1.X - labelSize.Width/2, p1.Y - labelSize.Height);
                    }
                    else
                        g.DrawRectangle(graphPen, new Rectangle(p1.X - 2, p1.Y - 2, 4, 4));

                }
                Point p3 = new Point((int)((valueList[valueList.Count - 1].Key / (2*Math.PI) + 0.5) * Width), (int)((0.5 - valueList[valueList.Count - 1].Value / maxValue) * Height));
                g.DrawLine(graphPen, p3.X, p3.Y,
                    Width, (int)((0.5 - valueList[valueList.Count - 1].Value / maxValue) * Height));
                if (selectedValue == valueList.Count - 1)
                {
                    g.DrawRectangle(selectedPen, new Rectangle(p3.X - 2, p3.Y - 2, 4, 4));
                    string label = string.Format("({0},{1})", (valueList[selectedValue].Key * 180 / Math.PI).ToString("F02", cultureUs),
                            valueList[selectedValue].Value.ToString("F02", cultureUs));
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

        private void PaintInduced(Graphics g)
        {
            List<KeyValuePair<double, double>> inducedValueList = new List<KeyValuePair<double, double>>();

            for (double alpha = -Math.PI; alpha < Math.PI; alpha += 0.05)
            {
                inducedValueList.Add(new KeyValuePair<double,double>(alpha, 
                    - Utility.Interpolate(Lift, alpha) * Math.Sin(alpha) + Utility.Interpolate(Drag, alpha)*Math.Cos(alpha)));
            }

            // Draw coefficients
            Pen bgPen = new Pen(Color.Gray);
            if (inducedValueList.Count > 0)
            {
                g.DrawLine(bgPen, 0, (int)((0.5 - inducedValueList[0].Value / maxValue) * Height),
                    (int)((inducedValueList[0].Key / (2 * Math.PI) + 0.5) * Width), (int)((0.5 - inducedValueList[0].Value / maxValue) * Height));
                for (int i = 0; i < inducedValueList.Count - 1; i++)
                {
                    Point p1 = new Point((int)((inducedValueList[i].Key / (2 * Math.PI) + 0.5) * Width), (int)((0.5 - inducedValueList[i].Value / maxValue) * Height));
                    Point p2 = new Point((int)((inducedValueList[i + 1].Key / (2 * Math.PI) + 0.5) * Width), (int)((0.5 - inducedValueList[i + 1].Value / maxValue) * Height));
                    g.DrawLine(bgPen, p1, p2);                    
                }
                Point p3 = new Point((int)((inducedValueList[inducedValueList.Count - 1].Key / (2 * Math.PI) + 0.5) * Width), (int)((0.5 - inducedValueList[inducedValueList.Count - 1].Value / maxValue) * Height));
                g.DrawLine(bgPen, p3.X, p3.Y,
                    Width, (int)((0.5 - inducedValueList[inducedValueList.Count - 1].Value / maxValue) * Height));
                g.DrawRectangle(bgPen, new Rectangle(p3.X - 2, p3.Y - 2, 4, 4));
            }
            bgPen.Dispose();
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
                        Point p = new Point((int)((valueList[i].Key / (2 * Math.PI) + 0.5) * Width), (int)((0.5 - valueList[i].Value / maxValue) * Height));
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
                    Point p1 = new Point((int)((valueList[i].Key / (2 * Math.PI) + 0.5) * Width), (int)((0.5 - valueList[i].Value / maxValue) * Height));
                    Point p2 = new Point((int)((valueList[i+1].Key / (2 * Math.PI) + 0.5) * Width), (int)((0.5 - valueList[i+1].Value / maxValue) * Height));
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
                        Point p = new Point((int)((valueList[i].Key / (2 * Math.PI) + 0.5) * Width), (int)((0.5 - valueList[i].Value / maxValue) * Height));
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
                    valueList[selectedValue] = new KeyValuePair<double,double>(
                        Math.Max(-Math.PI, Math.Min(Math.PI, 
                        valueList[selectedValue].Key + (e.Location.X - mouseDownPoint.X) * (2 * Math.PI) / Width)),
                        valueList[selectedValue].Value - (e.Location.Y - mouseDownPoint.Y) * maxValue / Height);
                    mouseDownPoint = e.Location;
                    this.Invalidate();
                }
            }
        }

        public void ShiftVertical(double amount)
        {
            for (int i = 0; i < valueList.Count; i++)
            {
                valueList[i] = new KeyValuePair<double, double>(valueList[i].Key, valueList[i].Value + amount);
            }
            Invalidate();
        }
    }
}
