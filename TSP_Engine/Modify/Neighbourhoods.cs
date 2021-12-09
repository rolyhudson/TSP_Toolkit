using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Engine.TSP
{
    public static partial class Modify
    {
        public static Footprint Neighbourhoods(this Footprint footprint, Field field)
        {
            footprint.FourNeighbourhood = footprint.Neighbourhood(field.Footprints, 4);
            footprint.EightNeighbourhood = footprint.Neighbourhood(field.Footprints, 8);
            return footprint;
        }
    }
}
