﻿using BH.oM.Geometry;
using BH.oM.TSP;
using BH.Engine.Representation;
using System;
using System.Collections.Generic;
using System.Text;
using BH.oM.Graphics;
using System.Drawing;
using BH.Engine.Geometry;

namespace BH.Engine.TSP
{
    public static partial class Create
    {
        public static RenderMesh SiteMesh(SiteLandUse siteLandUse)
        {
            PlanarSurface planarSurface = BH.Engine.Geometry.Create.PlanarSurface(siteLandUse.Boundary.Translate(new Vector() { X = 0, Y = 0, Z = -0.25 }));
            RenderMesh mesh = BH.Engine.Representation.Compute.RenderMesh(planarSurface);
            foreach (RenderPoint point in mesh.Vertices)
                point.Colour = Color.FromArgb(167, 152, 251, 152);

            return mesh;
        }
    }
}