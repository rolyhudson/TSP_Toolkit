using BH.oM.Base;
using BH.oM.Geometry;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.TSP
{
    public class PlanParameters : BHoMObject
    {
        public virtual double MinimumUnits { get; set; } = 1;

        public virtual double MaximumUnits { get; set; } = 10;

        public virtual double MinimumUnitsSpace { get; set; } = 2;

        public List<object> LandUses { get; set; } = new List<object>();

        public virtual double FootprintOffset { get; set; } = 1.5;
    }
}
