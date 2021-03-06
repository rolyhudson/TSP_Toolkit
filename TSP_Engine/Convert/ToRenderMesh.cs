using BH.Engine.Geometry;
using BH.oM.Geometry;
using BH.oM.Graphics;
using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace BH.Engine.TSP
{
    public static partial class Convert
    {
        public static RenderMesh ToRenderMesh(this Cell cell, Color colour)
        {
            return ToRenderMesh(cell.CellMesh(), colour);
        }

        public static List<RenderMesh> ToRenderMesh(this List<Bar> bars, Unit prototypeUnit, Color colour)
        {
            List<Mesh> meshes = new List<Mesh>();
            foreach (Bar bar in bars)
                meshes.AddRange(Create.UnitMesh(bars));

            List<RenderMesh> renderMeshes = new List<RenderMesh>();
            foreach (Mesh mesh in meshes)
                renderMeshes.Add(ToRenderMesh(mesh, colour));
            return renderMeshes;
        }

        public static RenderMesh ToRenderMesh(this Unit unit, Color colour)
        {
            return ToRenderMesh(unit.UnitMesh(), colour); 
        }

        public static RenderMesh ToRenderMesh(Mesh mesh, Color colour)

        {
            RenderMesh renderMesh = new RenderMesh();
            List<RenderPoint> renderPts = new List<RenderPoint>();
            foreach (BH.oM.Geometry.Point point in mesh.Vertices)
            {
                renderPts.Add(new RenderPoint()
                {
                    Colour = colour,
                    Point = point
                });
            }
            renderMesh.Vertices = renderPts;
            renderMesh.Faces = mesh.Faces;
            return renderMesh;
        }

        public static List<RenderMesh> ToRenderMesh(this FacilitiesBlock FacilitiesBlock, PreviewColourMap previewColourMap)
        {
            List<RenderMesh> renderMeshes = new List<RenderMesh>();
            Mesh m = new Mesh();
            List<oM.Geometry.Point> vertices = new List<oM.Geometry.Point>();
            List<Face> faces = new List<Face>();
            int v = 0;

            foreach (Polyline floor in FacilitiesBlock.Parking)
            {
                vertices.AddRange(floor.ControlPoints);
                faces.Add(Geometry.Create.Face(v, v + 1, v + 2, v + 3));
                v += 5;
            }
            m.Vertices = vertices;
            m.Faces = faces;
            renderMeshes.Add(ToRenderMesh(m, previewColourMap.Map["Parking"]));

            m = new Mesh();
            vertices = new List<oM.Geometry.Point>();
            faces = new List<Face>();
            v = 0;
            foreach (Polyline floor in FacilitiesBlock.Communal)
            {
                vertices.AddRange(floor.ControlPoints);
                faces.Add(Geometry.Create.Face(v, v + 1, v + 2, v + 3));
                v += 5;
            }
            m.Vertices = vertices;
            m.Faces = faces;
            renderMeshes.Add(ToRenderMesh(m, previewColourMap.Map["Communal"]));

            m = new Mesh();
            vertices = new List<oM.Geometry.Point>();
            faces = new List<Face>();
            v = 0;
            foreach (Polyline floor in FacilitiesBlock.Commercial)
            {
                vertices.AddRange(floor.ControlPoints);
                faces.Add(Geometry.Create.Face(v, v + 1, v + 2, v + 3));
                v += 5;
            }
            m.Vertices = vertices;
            m.Faces = faces;
            renderMeshes.Add(ToRenderMesh(m, previewColourMap.Map["Commercial"]));
            

            return renderMeshes;
        }

        public static RenderMesh ToRenderMesh(this Polyline polyline, Color color)
        {
            PlanarSurface planarSurface = BH.Engine.Geometry.Create.PlanarSurface(polyline);
            RenderMesh mesh = BH.Engine.Representation.Compute.RenderMesh(planarSurface);
            foreach (RenderPoint point in mesh.Vertices)
                point.Colour = color;

            return mesh;
        }

        public static RenderMesh ToRenderMesh(this BH.oM.Geometry.Point point, Color color)
        {
            //convert solar point to a mesh
            Vector vector = BH.Engine.Geometry.Create.Vector(point);
            vector = vector.Normalise();
            vector = vector.Reverse();
            Mesh mesh = new Mesh();
            Circle circle = Geometry.Create.Circle(point, vector, 1);
            mesh .Vertices.Add(circle.PointAtParameter(0));
            mesh.Vertices.Add(circle.PointAtParameter(0.33));
            mesh.Vertices.Add(circle.PointAtParameter(0.67));
            mesh.Vertices.Add(point + vector * 20);

            mesh.Faces.Add(Geometry.Create.Face(0, 1, 3));
            mesh.Faces.Add(Geometry.Create.Face(1, 2, 3));
            mesh.Faces.Add(Geometry.Create.Face(2, 0, 3));

            return ToRenderMesh(mesh, Color.Black);
        }
    }
}
