using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.TSP
{
    public class OccupiedLandUse : BHoMObject, ILandUse
    {
        override public string Name { get; set; } = "OccupiedLandUse";
    }
}
