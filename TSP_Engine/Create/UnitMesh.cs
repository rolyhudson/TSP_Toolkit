using BH.Engine.Base;
using BH.Engine.Geometry;
using BH.oM.Geometry;
using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Engine.TSP
{
    public static partial class Create
    {
        public static Mesh UnitMesh(this Unit unit)
        {
            Mesh mesh = new Mesh();
            List<Point> points = new List<Point>();
            points.Add(new Point() { X = 0, Y = 0 });
            points.Add(new Point() { X = unit.X, Y = 0 });
            points.Add(new Point() { X = unit.X, Y = unit.Y });
            points.Add(new Point() { X = 0, Y = unit.Y });
            points.Add(new Point() { X = 0, Y = 0,  Z = unit.Z});
            points.Add(new Point() { X = unit.X, Y = 0, Z = unit.Z });
            points.Add(new Point() { X = unit.X, Y = unit.Y, Z = unit.Z });
            points.Add(new Point() { X = 0, Y = unit.Y, Z = unit.Z });
            TransformMatrix transform = BH.Engine.Geometry.Create.OrientationMatrixGlobalToLocal(unit.CoordinateSystem);
            foreach(Point point in points)
                point.Transform(transform);

            List<Face> faces = new List<Face>();
            faces.Add(new Face() { A = 0, B = 1, C = 2, D = 3 });
            faces.Add(new Face() { A = 4, B = 5, C = 6, D = 7 });
            faces.Add(new Face() { A = 0, B = 1, C = 5, D = 4 });
            faces.Add(new Face() { A = 1, B = 2, C = 6, D = 5 });
            faces.Add(new Face() { A = 2, B = 3, C = 7, D = 6 });
            faces.Add(new Face() { A = 3, B = 0, C = 4, D = 7 });
            mesh.Vertices = points;
            mesh.Faces = faces;
            return mesh;
        }

        public static List<Mesh> UnitMesh(List<Bar> bars, Field field, Unit prototypeUnit)
        {
            List<Mesh> meshes = new List<Mesh>();
            Mesh baseMesh = prototypeUnit.UnitMesh();
            foreach (Bar bar in bars)
            {
                foreach(Unit unit in bar.Units)
                {
                    TransformMatrix transform = BH.Engine.Geometry.Create.OrientationMatrixGlobalToLocal(unit.CoordinateSystem);
                    Mesh mesh = baseMesh.ShallowClone();
                    mesh = mesh.Transform(transform);
                    meshes.Add(mesh);
                }
            }
            return meshes;
        }
    }
}
