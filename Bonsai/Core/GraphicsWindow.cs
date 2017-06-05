using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Bonsai.Core
{
    /// <summary>
    /// The main window that will be used for the sample framework
    /// </summary>
    public class GraphicsWindow : System.Windows.Forms.Form
    {
        private Framework frame = null;
        public GraphicsWindow(Framework f)
        {
            frame = f;
            this.MinimumSize = Framework.MinWindowSize;
        }

        /// <summary>
        /// Will call into the sample framework's window proc
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            frame.WindowsProcedure(ref m);
            base.WndProc(ref m);
        }


    }
}
