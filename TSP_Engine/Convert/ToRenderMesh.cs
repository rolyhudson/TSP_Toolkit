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

        public static RenderMesh ToRenderMesh(this CommunalBlock communalBlock, Color color)
        {
            Mesh m = new Mesh();
            List<oM.Geometry.Point> vertices = new List<oM.Geometry.Point>();
            vertices.AddRange(communalBlock.Floors[0].ControlPoints);
            vertices.AddRange(communalBlock.Floors.Last().ControlPoints);

            List<Face> faces = new List<Face>();
            faces.Add(Geometry.Create.Face(0, 1, 2, 3));
            faces.Add(Geometry.Create.Face(5,6,7,8));
            faces.Add(Geometry.Create.Face(0, 5, 6, 1));
            faces.Add(Geometry.Create.Face(1,6,7,2));
            faces.Add(Geometry.Create.Face(2,7,8,3));
            faces.Add(Geometry.Create.Face(3,8,9,4));
            m.Vertices = vertices;
            m.Faces = faces;
            return ToRenderMesh(m, color);
        }
    }
}
