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

        public List<ILandUse> LandUses { get; set; } = new List<ILandUse>();

        public virtual ILayout LayoutMethod { get; set; }

    }
}
