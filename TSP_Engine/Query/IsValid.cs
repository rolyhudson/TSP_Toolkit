using BH.oM.TSP;
using BH.Engine.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BH.Engine.TSP
{
    public static partial class Query
    {
        public static bool IsValid(this Development development)
        {
            //is the communal inside the boundary
            if (development.Field.Boundary.ICurveIntersections(development.CommunalBlock.Floors[0]).Count > 0)
                return false;
            foreach (Guid guid in development.Bars.SelectMany(x => x.Cells))
            {
                //check each cell for intersect
                Cell cell = development.Field.Cells.Find(x => x.BHoM_Guid.Equals(guid));
                if (cell.Boundary.ICurveIntersections(development.CommunalBlock.Floors[0]).Count > 0)
                    return false;
            }
            return true;
        }
    }
}
