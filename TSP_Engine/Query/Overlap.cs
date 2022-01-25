using BH.oM.TSP;
using BH.Engine.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Engine.TSP
{
    public static partial class Query
    {
        public static bool Overlap(this Cell cell, Field field, double searchRadius)
        {
            double searchSq = searchRadius * searchRadius;
            List<Cell> close = field.Cells.FindAll(x => x.Centre.SquareDistance(cell.Centre) <= searchSq);
            foreach(Cell c in close)
            {
                var intersects = c.Boundary.ICurveIntersections(cell.Boundary);
                if (intersects.Count > 0)
                    return true;
            }
            return false;
        }
    }
}
