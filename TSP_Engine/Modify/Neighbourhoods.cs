using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Engine.TSP
{
    public static partial class Modify
    {
        public static Cell Neighbourhoods(this Cell footprint, Field field)
        {
            footprint.FourNeighbourhood = footprint.Neighbourhood(field.Cells, 4);
            footprint.EightNeighbourhood = footprint.Neighbourhood(field.Cells, 8);
            return footprint;
        }
    }
}
