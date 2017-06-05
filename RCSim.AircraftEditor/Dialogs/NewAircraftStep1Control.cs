using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace RCSim.AircraftEditor.Dialogs
{
    public partial class NewAircraftStep1Control : UserControl
    {
        public enum CreationOptionsEnum
        {
            NewAircraft,
            Variation
        }

        public CreationOptionsEnum CreationOptions
        {
            get
            {
                if (radioButtonNew.Checked)
                    return CreationOptionsEnum.NewAircraft;
                else
                    return CreationOptionsEnum.Variation;
            }
        }

        public NewAircraftStep1Control()
        {
            InitializeComponent();
        }
    }
}
