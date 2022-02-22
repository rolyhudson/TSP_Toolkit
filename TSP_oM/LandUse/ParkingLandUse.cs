using BH.oM.Base;
using BH.oM.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.TSP
{
    public class ParkingLandUse : BHoMObject, ILandUse
            
    {
        public virtual Polyline Boundary { get; set; } = new Polyline();

        override public string Name { get; set; } = "ParkingLandUse";
    }
}
