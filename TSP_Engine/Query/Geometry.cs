﻿using BH.oM.Geometry;
using BH.oM.TSP;
using BH.Engine.Geometry;
using System;
using System.Collections.Generic;
using System.Text;
using BH.oM.Geometry.CoordinateSystem;

namespace BH.Engine.TSP
{
    public static partial class Query
    {
        public static Polyline Geometry(this Cell footprint)
        {
            return footprint.Boundary;
        }

        public static Polyline Geometry(this Unit unit)
        {
            List<Point> points = new List<Point>()
            {
                new Point(),
                BH.Engine.Geometry.Create.Point(unit.X,0,0),
                BH.Engine.Geometry.Create.Point(unit.X,unit.Y,0),
                BH.Engine.Geometry.Create.Point(0,unit.Y,0),
                new Point(),
            };

            Polyline boundary = BH.Engine.Geometry.Create.Polyline(points);
            TransformMatrix transform = BH.Engine.Geometry.Create.OrientationMatrixGlobalToLocal(unit.CoordinateSystem);
            boundary = boundary.Transform(transform);
            return boundary;
        }
    }
}
