using BH.oM.Base;
using BH.oM.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.TSP
{
    public class Development : BHoMObject
    {
        public virtual List<Bar> Bars { get; set; } = new List<Bar>();

        public virtual Field Field { get; set;  }

        public virtual FacilitiesBlock FacilitiesBlock { get; set; } = null;

        public virtual Polyline Boundary { get; set; } = new Polyline();

        public virtual List<Polyline> OpenSpace { get; set; } = new List<Polyline>();

        public virtual List<Polyline> Roads { get; set; } = new List<Polyline>();

        public virtual List<Point> Sunpoints { get; set; } = new List<Point>();
    }
}
