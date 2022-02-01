using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.TSP
{
    public class PerimeterLayout : ILayout
    {
        public virtual PerimeterLevel PerimeterLevelMethod { get; set; } = PerimeterLevel.Random;

        public virtual int MinimumLevel { get; set; } = 1;
    }
}
