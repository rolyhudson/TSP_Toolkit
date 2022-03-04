using BH.oM.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.TSP
{
    public class BarsLayout : ILayout
    {
        public virtual double PrincipleDirection { get; set; } = 0;

        public virtual int MaximumPlacementAttempts { get; set; } = 10;

        public virtual double GapToHeightRatio { get; set; } = 1.0;

        public virtual string Name { get; set; } = "BarsLayout";

        public virtual double BoundaryOffset { get; set; } = 6.0;
    }
}
