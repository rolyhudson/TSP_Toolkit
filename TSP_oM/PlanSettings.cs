using BH.oM.Base;
using BH.oM.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.TSP
{
    public class PlanSettings : BHoMObject
    {
        public virtual double MinimumUnits { get; set; } = 1;

        public virtual double MaximumUnits { get; set; } = 10;

        public virtual double MinimumUnitsSpace { get; set; } = 2;

        public virtual int MaximumPlacementAttempts{ get; set; } = 10;

        public virtual Polyline SiteBoundary { get; set; } = new Polyline();

        public virtual List<Polyline> CirculationRoutes { get; set; } = new List<Polyline>();

        public virtual List<Polyline> OpenSpaces { get; set; } = new List<Polyline>();

        public virtual ILayout LayoutMethod { get; set; }

    }
}
