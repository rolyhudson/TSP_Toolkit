using BH.oM.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Engine.TSP
{
    public static partial class Query
    {
        public static bool MeshIntersect(this Mesh mesh, Point start, Vector direction)
        {
            foreach(Face f in mesh.Faces)
            {
                if (TriangleIntersect(mesh.Vertices[f.A], mesh.Vertices[f.B], mesh.Vertices[f.C], start, direction))
                    return true;
                if(f.D > -1)
                    if(TriangleIntersect(mesh.Vertices[f.C], mesh.Vertices[f.D], mesh.Vertices[f.A], start, direction))
                        return true;
            }
            return false;
        }


    }
}
