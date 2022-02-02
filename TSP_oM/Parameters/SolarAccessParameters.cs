using BH.oM.Base;
using BH.oM.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.TSP
{
    public class SolarAccessParameters : BHoMObject
    {
        public virtual List<Point> SunPoints { get; set; } = new List<Point>();

        public virtual List<Mesh> Obstructions { get; set; } = new List<Mesh>();
    }
}
