using BH.oM.TSP;
using BH.Engine.Geometry;
using System;
using System.Collections.Generic;
using System.Text;
using BH.oM.Data.Collections;
using System.Linq;
using BH.oM.Geometry;

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

        public static bool Overlap(this Cell cell, Field field)
        {
            List<Polyline> curves = field.Cells.Select(x => x.Boundary).ToList();
            DomainTree<int> indexTree = Data.Create.DomainTree(curves.Select((x, i) => Data.Create.DomainTreeLeaf(i, x.Bounds().DomainBox())));
            foreach (int j in Data.Query.ItemsInRange(indexTree, cell.Boundary.Bounds().Inflate(1).DomainBox()))
            {
                var intersects = curves[j].ICurveIntersections(cell.Boundary);
                if (intersects.Count > 0)
                    return true;
            }
           
            return false;
        }
    }
}
