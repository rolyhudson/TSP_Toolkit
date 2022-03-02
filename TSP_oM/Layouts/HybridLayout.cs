using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.TSP
{
    public class HybridLayout : ILayout
    {
        public virtual List<ILayout> Layouts { get; set; } = new List<ILayout>();

        public virtual double PerimeterOffset { get; set; } = 5.0;
    }
}
