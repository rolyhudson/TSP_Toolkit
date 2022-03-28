using BH.oM.Base;
using BH.oM.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.TSP
{
    public class Bar : BHoMObject
    {
        public virtual List<Unit> Units { get; set; } = new List<Unit>();
        public virtual List<Guid> Cells { get; set; } = new List<Guid>();
        public virtual Polyline ExternalCirculation { get; set; } = new Polyline();
    }
}
