using System;
using System.Collections.Generic;
using System.Text;
using RCSim.DataClasses;

namespace RCSim.Interfaces
{
    interface IAirplaneControl
    {
        double Throttle { get; set; }
        double Ailerons { get; set; }
        double Elevator { get; set; }
        double Rudder { get; set; }
        double Flaps { get; set; }
        double Gear { get; set; }
        AircraftParameters AircraftParameters { get; set; }
        float RotorRPM { get; set; }
        /// <summary>
        /// Gets/Sets the relative force the rotor is exercising.
        /// </summary>
        float RelativeRotorForce { get; set; }
    }
}
