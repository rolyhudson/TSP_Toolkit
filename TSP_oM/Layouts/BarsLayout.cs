using BH.oM.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.TSP
{
    public class BarsLayout : ILayout
    {
        public virtual Vector PrincipleDirection { get; set; } = new Vector();

        public virtual int MaximumPlacementAttempts { get; set; } = 10;
    }
}
