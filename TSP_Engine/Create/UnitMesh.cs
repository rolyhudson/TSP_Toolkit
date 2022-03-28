using BH.Engine.Base;
using BH.Engine.Geometry;
using BH.oM.Geometry;
using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BH.Engine.TSP
{
    public static partial class Create
    {
        public static Mesh UnitMesh(this Unit unit, bool halfUnit = false, int side = 0)
        {
            Mesh mesh = new Mesh();
            List<Point> points = new List<Point>();
            double xDim = unit.X;
            double xStart = 0;
            if (halfUnit)
                xDim = xDim / 2;

            if (side == 1 && halfUnit)
                xStart = xDim;

            points.Add(new Point() { X = xStart, Y = 0 });
            points.Add(new Point() { X = xStart + xDim, Y = 0 });
            points.Add(new Point() { X = xStart + xDim, Y = unit.Y });
            points.Add(new Point() { X = xStart, Y = unit.Y });
            points.Add(new Point() { X = xStart, Y = 0,  Z = unit.Z});
            points.Add(new Point() { X = xStart + xDim, Y = 0, Z = unit.Z });
            points.Add(new Point() { X = xStart + xDim, Y = unit.Y, Z = unit.Z });
            points.Add(new Point() { X = xStart, Y = unit.Y, Z = unit.Z });
            TransformMatrix transform = BH.Engine.Geometry.Create.OrientationMatrixGlobalToLocal(unit.CoordinateSystem);
            List<Point> mapped = new List<Point>();
            foreach(Point point in points)
            {
                mapped.Add(point.Transform(transform));
            }
                

            List<Face> faces = new List<Face>();
            faces.Add(new Face() { A = 0, B = 1, C = 2, D = 3 });
            faces.Add(new Face() { A = 4, B = 5, C = 6, D = 7 });
            faces.Add(new Face() { A = 0, B = 1, C = 5, D = 4 });
            faces.Add(new Face() { A = 1, B = 2, C = 6, D = 5 });
            faces.Add(new Face() { A = 2, B = 3, C = 7, D = 6 });
            faces.Add(new Face() { A = 3, B = 0, C = 4, D = 7 });
            mesh.Vertices = mapped;
            mesh.Faces = faces;
            return mesh;
        }

        public static Mesh UnitMesh(this FacilitiesBlock communalBlock)
        {
            Mesh m = new Mesh();
            List<oM.Geometry.Point> vertices = new List<oM.Geometry.Point>();
            vertices.AddRange(communalBlock.Boundary.ControlPoints);
            double floorToFloor = (communalBlock.Parking.Last().ControlPoints[0].Z - communalBlock.Parking.First().ControlPoints[0].Z) / communalBlock.Parking.Count();

            Polyline roof = communalBlock.Boundary.Translate(new Vector() { X = 0, Y = 0, Z = floorToFloor * (communalBlock.Parking.Count() + 1) });
            vertices.AddRange(roof.ControlPoints);

            List<Face> faces = new List<Face>();
            faces.Add(Geometry.Create.Face(0, 1, 2, 3));
            faces.Add(Geometry.Create.Face(5, 6, 7, 8));
            faces.Add(Geometry.Create.Face(0, 5, 6, 1));
            faces.Add(Geometry.Create.Face(1, 6, 7, 2));
            faces.Add(Geometry.Create.Face(2, 7, 8, 3));
            faces.Add(Geometry.Create.Face(3, 8, 9, 4));
            m.Vertices = vertices;
            m.Faces = faces;

            return m;
            
        }

        public static List<Mesh> UnitMesh(List<Bar> bars)
        {
            List<Mesh> meshes = new List<Mesh>();
            
            foreach (Bar bar in bars)
            {
                foreach(Unit unit in bar.Units)
                {
                   meshes.Add(unit.UnitMesh());
                }
            }
            return meshes;
        }
    }
}
