using BH.oM.Geometry;
using BH.oM.Graphics;
using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Drawing;
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
    }
}
