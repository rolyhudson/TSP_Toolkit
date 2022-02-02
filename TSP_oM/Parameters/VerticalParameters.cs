using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.TSP
{
    public class VerticalParameters : BHoMObject
    {
        public virtual int MaximumLevel { get; set; } = 10;

        public virtual ILayout LayoutMethod { get; set; }
    }
}
