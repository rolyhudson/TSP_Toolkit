using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.TSP
{
    public class HybridLayout : ILayout
    {
        public virtual BarsLayout BarsLayout { get; set; } = new BarsLayout();

        public virtual PerimeterLayout PerimeterLayout { get; set; } = new PerimeterLayout();

        public virtual string Name { get; set; } = "HybridLayout";
    }
}
