using BH.oM.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.TSP
{
    public class BarsLayout : ILayout
    {
        public virtual double PrincipleDirection { get; set; } = 0;

        public virtual double MinimumUnits { get; set; } = 1;

        public virtual double MaximumUnits { get; set; } = 10;

        public virtual double MinimumUnitsSpace { get; set; } = 2;

        public virtual int MaximumPlacementAttempts { get; set; } = 10;

        public virtual double GapToHeightRatio { get; set; } = 0.2;

        public virtual string Name { get; set; } = "BarsLayout";

        public virtual double BoundaryOffset { get; set; } = 6.0;
    }
}
