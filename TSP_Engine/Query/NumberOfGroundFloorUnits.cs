using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BH.Engine.TSP
{
    public static partial class Query
    {
        public static int NumberOfGroundFloorUnits(this List<Bar> bars)
        {
            var a = bars.SelectMany(x => x.Units).ToList();//.FindAll(x => x.CoordinateSystem.Origin.Z == 0).Count;
            return bars.SelectMany(x => x.Units).ToList().FindAll(x => x.CoordinateSystem.Origin.Z == 0).Count;
        }
    }
}
