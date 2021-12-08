using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.TSP
{
    public class BarSettings : BHoMObject
    {
        public virtual double MinimumUnits { get; set; } = 1;

        public virtual double MaximumUnits { get; set; } = 10;

        public virtual int MaximumLevel { get; set; } = 10;

        public virtual double GapToHeightRatio { get; set; } = 1.0;

    }
}
