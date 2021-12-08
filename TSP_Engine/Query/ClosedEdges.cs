using BH.oM.Geometry;
using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using BH.Engine.Geometry;

namespace BH.Engine.TSP
{
    public static partial class Query
    {
        public static List<Line> ClosedEdges(this Footprint footprint, List<Footprint> footprints)
        {
            List<Line> edges = new List<Line>();

            if (footprint.Neighbours.Count == 0)
                footprint.Neighbours = footprint.Neighbourhood(footprints, 9);


            for (int i = 0; i < footprint.Boundary.ControlPoints.Count - 1; i++)
            {
                Line edge = BH.Engine.Geometry.Create.Line(footprint.Boundary.ControlPoints[i], footprint.Boundary.ControlPoints[i + 1]);
                foreach (Polyline pl in footprints.Select(x => x.Boundary))
                {
                    for (int j = 0; j < pl.ControlPoints.Count - 1; j++)
                    {
                        Line edge2 = Geometry.Create.Line(pl.ControlPoints[j], pl.ControlPoints[j + 1]);

                        if (EdgeMatch(edge, edge2,0.001))
                        {
                            edges.Add(edge);
                        }
                            
                    }
                }
            }
            return edges;

        }

        private static bool EdgeMatch(Line line1, Line line2, double tolerance)
        {
            if (line1.Start.SquareDistance(line2.Start) < tolerance
              && line1.End.SquareDistance(line2.End) < tolerance)
                return true;

            if (line1.Start.SquareDistance(line2.End) < tolerance
              && line1.End.SquareDistance(line2.Start) < tolerance)
                return true;

            return false;
        }
    }
}
