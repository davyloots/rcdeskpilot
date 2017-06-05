using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace RCSim.AircraftEditor.Dialogs
{
    public partial class SliderControl : UserControl
    {
        protected double factor = 1.0;
        protected double parameterValue = 0;
        protected string toolTip = string.Empty;

        #region Public properties
        /// <summary>
        /// Gets/sets the multiplication factor of the slider with respect to the original value.
        /// </summary>
        public double Factor
        {
            get { return factor; }
            set { factor = value; }
        }
        /// <summary>
        /// Gets/Sets the name of the parameter.
        /// </summary>
        public string ParameterName
        {
            get { return labelParameter.Text; }
            set { labelParameter.Text = value; }
        }

        /// <summary>
        /// Gets/Sets the value of the parameter.
        /// </summary>
        public double ParameterValue
        {
            get { return parameterValue; }
            set
            {
                parameterValue = value;
                UpdateSliderPosition();
                labelValue.Text = parameterValue.ToString("F03");
            }
        }

        /// <summary>
        /// Gets/Sets the tooltip.
        /// </summary>
        public string ToolTip
        {
            get { return toolTip; }
            set
            {
                string tipText = value.Replace("\\n", "\n");
                toolTip = tipText;
                ToolTip tip = new ToolTip();
                tip.SetToolTip(trackBar, toolTip);
            }
        }

        /// <summary>
        /// Gets the minimum value of the slider (default 0).
        /// </summary>
        public int MinimumValue
        {
            get { return trackBar.Minimum; }
            set 
            { 
                trackBar.Minimum = value;
                UpdateSliderPosition();
            }
        }

        /// <summary>
        /// Gets the maximum value of the slider (default 100).
        /// </summary>
        public int MaximumValue
        {
            get { return trackBar.Maximum; }
            set
            {
                trackBar.Maximum = value;
                UpdateSliderPosition();
            }
        }
        #endregion

        #region Public events
        public event EventHandler ValueChanged;
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor.
        /// </summary>
        public SliderControl() :
            this("parameter", .5, 100, "this is the tooltip for the parameter")
        {
            //InitializeComponent();
        }

        /// <summary>
        /// Initializes a slidercontrol.
        /// </summary>
        /// <param name="parName"></param>
        /// <param name="parValue"></param>
        /// <param name="toolTip"></param>
        public SliderControl(string parName, double parValue, double parFactor, string toolTip)
        {
            InitializeComponent();

            ParameterName = parName;
            Factor = parFactor;
            ParameterValue = parValue;

            ToolTip tip = new ToolTip();
            tip.SetToolTip(trackBar, toolTip);
        }
        #endregion

        #region Protected methods
        /// <summary>
        /// Updates the slider position.
        /// </summary>
        protected void UpdateSliderPosition()
        {
            trackBar.Value = (int)Math.Min(MaximumValue, Math.Max(ParameterValue * Factor, MinimumValue));
        }
        #endregion

        #region Private event handlers
        /// <summary>
        /// Handles the Scroll event when the trackbar position changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackBar_Scroll(object sender, EventArgs e)
        {
            ParameterValue = trackBar.Value / Factor;
            if (ValueChanged != null)
                ValueChanged(this, EventArgs.Empty);
        }
        #endregion
    }
}
