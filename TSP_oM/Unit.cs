using BH.oM.Base;
using BH.oM.Geometry.CoordinateSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.TSP
{
    public class Unit : BHoMObject
    {
        public virtual double X { get; set; } = 4.0;
        public virtual double Y { get; set; } = 6.0;
        public virtual double Z { get; set; } = 3.5;
        public virtual Cartesian CoordinateSystem { get; set; } = new Cartesian();
    }
}
