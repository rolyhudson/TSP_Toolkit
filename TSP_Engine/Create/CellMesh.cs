using BH.oM.Geometry;
using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Engine.TSP
{
    public static partial class Create
    {
        public static Mesh CellMesh(this Cell cell)
        {
            Mesh mesh = new Mesh();
            List<Point> points = new List<Point>();
            foreach(Point point in cell.Boundary.ControlPoints)
                points.Add(point);

            List<Face> faces = new List<Face>();
            faces.Add(new Face() { A = 0, B = 1, C = 2, D = 3 });
            mesh.Vertices = points;
            mesh.Faces = faces;
            return mesh;
        }
    }
}
