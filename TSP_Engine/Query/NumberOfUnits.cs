using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BH.Engine.TSP
{
    public static partial class Query
    {
        public static int NumberOfUnits(this List<Bar> bars)
        {
            
            return bars.SelectMany(x => x.Units).Count();
        }
    }
}
