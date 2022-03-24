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

        public static List<RenderMesh> ToRenderMesh(this CommunalBlock communalBlock, PreviewColourMap previewColourMap)
        {
            List<RenderMesh> renderMeshes = new List<RenderMesh>();
            Mesh m = new Mesh();
            List<oM.Geometry.Point> vertices = new List<oM.Geometry.Point>();
            List<Face> faces = new List<Face>();
            int v = 0;

            foreach (Polyline floor in communalBlock.Parking)
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
            foreach (Polyline floor in communalBlock.Social)
            {
                vertices.AddRange(floor.ControlPoints);
                faces.Add(Geometry.Create.Face(v, v + 1, v + 2, v + 3));
                v += 5;
            }
            m.Vertices = vertices;
            m.Faces = faces;
            renderMeshes.Add(ToRenderMesh(m, previewColourMap.Map["Social"]));

            m = new Mesh();
            vertices = new List<oM.Geometry.Point>();
            faces = new List<Face>();
            v = 0;
            foreach (Polyline floor in communalBlock.Commercial)
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
    }
}
