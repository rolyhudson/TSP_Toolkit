using BH.oM.Geometry;
using BH.Engine.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Engine.TSP
{
    public static partial class Query
    {
        public static Sphere BoundingSphere(this Mesh mesh)
        {
            double rMaxSq = double.MinValue;
            Sphere sphere = new Sphere();
            sphere.Centre = mesh.Vertices.Average();
            foreach (Point p in mesh.Vertices)
            {
                double dSq = p.SquareDistance(sphere.Centre);
                if (dSq > rMaxSq)
                    rMaxSq = dSq;
            }
            sphere.Radius = Math.Sqrt(rMaxSq);
            return sphere;
        }
    }
}
