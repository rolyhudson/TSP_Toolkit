using BH.oM.Geometry;
using BH.Engine.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Engine.TSP
{
    public static partial class Query
    {
        public static bool TriangleIntersect(Point v0, Point v1, Point v2, Point start, Vector direction )
        {
            Vector e1 = v1 - v0;
            Vector e2 = v2 - v0;
            Vector p = direction.CrossProduct(e2);
            double a = e1.DotProduct(p);
            //if p is 0 line is parallel to triangle
            if (a == 0)
                return false;

            //compute denominator
            double f = 1 / a;

            //compute barycentric coords
            Vector s = start - v0;
            double u = f * s.DotProduct(p);
            if (u < 0 || u > 1)
                return false;

            Vector q = s.CrossProduct(e1);
            double v = f * direction.DotProduct(q);
            if (v < 0 || u + v > 1)
                return false;

            //compute line parameter
            double t = f * e2.DotProduct(q);

            return (t >= 0);
        }
    }
}
