using BH.oM.Base;
using BH.oM.Geometry;
using System;
using System.Collections.Generic;
using System.Text;
namespace BH.oM.TSP
{
    public class RoadLandUse : BHoMObject, ILandUse 
    {
        public virtual Polyline CentreLine { get; set; } = new Polyline();

        override public string Name { get; set; } = "RoadLandUse";
    }
}
