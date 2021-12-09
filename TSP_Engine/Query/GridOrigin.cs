using BH.Engine.Base;
using BH.Engine.Geometry;
using BH.oM.Geometry;
using BH.oM.Geometry.CoordinateSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Engine.TSP
{
    public static partial class Query
    {
        public static Cartesian GridCartesian(Vector principleDirection, Polyline siteBoundary)
        {
            Polyline box = Query.AlignedBoundingBox(principleDirection, siteBoundary);
            Cartesian origin = BH.Engine.Geometry.Create.CartesianCoordinateSystem(box.ControlPoints[0], box.ControlPoints[3] - box.ControlPoints[0], box.ControlPoints[1] - box.ControlPoints[0]);
            return origin;
        }
    }
}
