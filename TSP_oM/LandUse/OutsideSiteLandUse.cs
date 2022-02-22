using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.TSP
{
    public class OutsideSiteLandUse : BHoMObject, ILandUse
    {
        override public string Name { get; set; } = "OutsideSiteLandUse";
    }
}
