using BH.oM.Geometry;
using BH.Engine.Geometry;
using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Engine.TSP
{
    public static partial class Create
    {
        public static List<Mesh> UnitFaces(this Unit unit)
        {
            List<Mesh> meshes = new List<Mesh>();
            TransformMatrix transform = BH.Engine.Geometry.Create.OrientationMatrixGlobalToLocal(unit.CoordinateSystem);
            List<Point> vertices1 = new List<Point>()
            {
                Geometry.Create.Point(0, 0, 0),
                Geometry.Create.Point(0, unit.Y, 0),
                Geometry.Create.Point(0, unit.Y, unit.Z),
                Geometry.Create.Point(0, 0, unit.Z),
                
            };
            Mesh mesh1 = new Mesh();
            foreach (Point p in vertices1)
            {
                mesh1.Vertices.Add(p.Transform(transform));
            }
            List<Face> faces = new List<Face>();
            faces.Add(Geometry.Create.Face(0, 1, 2, 3));
            mesh1.Faces = faces;
            
            meshes.Add(mesh1);

            List<Point> vertices2 = new List<Point>()
            {
                Geometry.Create.Point(unit.X, 0, 0),
                Geometry.Create.Point(unit.X, unit.Y, 0),
                Geometry.Create.Point(unit.X, unit.Y, unit.Z),
                Geometry.Create.Point(unit.X, 0, unit.Z),
                
            };
            Mesh mesh2 = new Mesh();
            foreach (Point p in vertices2)
            {
                mesh2.Vertices.Add(p.Transform(transform));
            }
            List<Face> faces2 = new List<Face>();
            faces2.Add(Geometry.Create.Face(0, 1, 2, 3));
            mesh2.Faces = faces2;

            meshes.Add(mesh2);


            return meshes;
        }
    }
}
