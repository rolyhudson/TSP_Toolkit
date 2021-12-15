using BH.Engine.Geometry;
using BH.oM.Geometry;
using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BH.Engine.TSP
{
    public static partial class Query
    {
        public static List<Cell> AlignedNeighbours(this Cell footprint, Vector direction, Field field)
        {
            List<Cell> aligned = new List<Cell>();
            
            foreach(Cell n in footprint.EightNeighbourhood)
            {
                Vector toNeighbour = n.Boundary.Centroid() - footprint.Boundary.Centroid();
                if (Math.Abs(toNeighbour.IsParallel(direction)) == 1)
                    aligned.Add(n);
            }
            return aligned;
        }

        public static List<Cell> AlignedNeighbours(this Cell footprint, Vector direction, Field field, Use use)
        {
            List<Cell> aligned = new List<Cell>();

            foreach (Cell n in field.Cells.FindAll(x => x.Use == use))
            {
                Vector toNeighbour = n.Centre- footprint.Centre;
                if (Math.Abs(toNeighbour.IsParallel(direction)) == 1)
                    aligned.Add(n);
            }
            //order by distance
            aligned = aligned.OrderBy(x => x.Centre.SquareDistance(footprint.Centre)).ToList();
            //remove the original
            //aligned.RemoveAt(0);
            //take nearest two
            return aligned.Take(2).ToList();
        }
    }
}
