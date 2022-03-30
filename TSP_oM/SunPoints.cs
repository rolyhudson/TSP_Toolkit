using BH.oM.Base;
using BH.oM.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.TSP
{
    public class SunPoints : BHoMObject
    {
        public virtual List<Point> Points { get; set; } = new List<Point>();

        public virtual double Latitude { get; set; } = 0;
    }
}
