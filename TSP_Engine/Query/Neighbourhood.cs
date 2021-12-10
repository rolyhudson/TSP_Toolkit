﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BH.oM.TSP;
using BH.Engine.Geometry;

namespace BH.Engine.TSP
{
    public static partial class Query
    {
        public static List<Cell> Neighbourhood(this Cell footprint, List<Cell> footprints, int num)
        {
            List<Cell> ordered = footprints.OrderBy(p => p.Centre.SquareDistance(footprint.Centre)).Take(num + 1).ToList();
            ordered.RemoveAt(0);
            return ordered;
        }
    }
}
