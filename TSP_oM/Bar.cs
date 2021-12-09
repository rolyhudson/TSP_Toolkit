using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.TSP
{
    public class Bar : BHoMObject
    {
        public virtual List<Unit> Units { get; set; } = new List<Unit>();
        public virtual List<Guid> Footprints { get; set; } = new List<Guid>();
    }
}
