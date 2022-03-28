using BH.oM.Base;
using BH.oM.Geometry;
using BH.oM.Geometry.CoordinateSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.TSP
{
    public class FacilitiesLandUse : BHoMObject, ILandUse
    {
        public virtual Polyline Boundary { get; set; } = new Polyline();

        override public string Name { get; set; } = "FacilitiesLandUse";

        public virtual Cartesian CoordinateSystem { get; set; } = new Cartesian();
    }
}
