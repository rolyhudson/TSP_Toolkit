using BH.oM.Geometry;
using BH.Engine.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Engine.TSP
{
    public static partial class Query
    {
        public static List<Point> InHalfSpace(this List<Point> points, Plane plane, bool includeInPlane = false)
        {
            List<Point> halfSpace = new List<Point>();
            foreach (Point p in points)
            {
                if (p.InHalfSpace(plane, includeInPlane))
                    halfSpace.Add(p);
            }
            return halfSpace;
        }

        public static bool InHalfSpace(this Point point, Plane plane, bool includeInPlane = false)
        {
            Vector v = point - plane.Origin;
            bool inHalfSpace = false;
            if (includeInPlane)
            {
                if (v.Angle(plane.Normal) <= Math.PI / 2)
                    inHalfSpace = true;

            }
            else 
            {
                if (v.Angle(plane.Normal) < Math.PI / 2)
                    inHalfSpace = true;
            } 
            return inHalfSpace;
        }
    }
}
