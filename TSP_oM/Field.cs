using BH.oM.Base;
using BH.oM.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.TSP
{
    public class Field : BHoMObject
    {
        public virtual List<Cell> Cells { get; set; } = new List<Cell>();
        public virtual Dictionary<Guid, List<Guid>> Adjacency { get; set; } = new Dictionary<Guid, List<Guid>>();

        public virtual ILayout Layout { get; set; }

        public virtual Polyline Boundary { get; set; } = new Polyline();
    }
}
