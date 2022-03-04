using BH.oM.Base;
using BH.oM.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.TSP
{
    public class CommunalBlock : BHoMObject
    {
        public List<Polyline> Floors { get; set; } = new List<Polyline>();
    }
}
