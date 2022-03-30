using BH.oM.TSP;
using BH.Engine.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BH.oM.Geometry;

namespace BH.Engine.TSP
{
    public static partial class Query
    {
        public static bool IsValid(this Development development)
        {
            //is the communal inside the boundary
            //if (development.Boundary.ICurveIntersections(development.FacilitiesBlock.Boundary).Count > 0)
            //    return false;
            foreach (Guid guid in development.Bars.SelectMany(x => x.Cells))
            {
                //check each cell for intersect
                Cell cell = development.Field.Cells.Find(x => x.BHoM_Guid.Equals(guid));
                if (cell.Boundary.ICurveIntersections(development.FacilitiesBlock.Boundary).Count > 0)
                    return false;
            }
            return true;
        }

        public static bool IsValid(this FacilitiesBlock facilitiesBlock, Polyline boundary)
        {
            //is the communal inside the boundary
            if (boundary.IsContaining(facilitiesBlock.Boundary))
                return true;

            return false;
        }
    }
}
