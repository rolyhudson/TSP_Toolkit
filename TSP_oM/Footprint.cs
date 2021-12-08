using BH.oM.Base;
using BH.oM.Geometry;
using BH.oM.Geometry.CoordinateSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.TSP
{
    public class Footprint : BHoMObject
    {
        public virtual bool Occupied { get; set; } = false;
        public virtual int Levels { get; set; } = 0;
        public List<Footprint> Neighbours { get; set; } = new List<Footprint>();

        public Polyline Boundary { get; set; } = new Polyline();
        public virtual Cartesian CoordinateSystem { get; set; } = new Cartesian();
    }
}
