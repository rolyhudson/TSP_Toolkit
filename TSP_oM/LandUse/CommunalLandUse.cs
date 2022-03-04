using BH.oM.Base;
using BH.oM.Geometry;
using BH.oM.Geometry.CoordinateSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.TSP
{
    public class CommunalLandUse : BHoMObject, ILandUse
    {
        public virtual Polyline Boundary { get; set; } = new Polyline();

        override public string Name { get; set; } = "CommunalLandUse";

        public virtual Cartesian CoordinateSystem { get; set; } = new Cartesian();
    }
}
