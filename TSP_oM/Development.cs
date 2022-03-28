using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.TSP
{
    public class Development : BHoMObject
    {
        public virtual List<Bar> Bars { get; set; } = new List<Bar>();

        public virtual Field Field { get; set;  }

        public virtual FacilitiesBlock CommunalBlock { get; set; } = null;

        
    }
}
