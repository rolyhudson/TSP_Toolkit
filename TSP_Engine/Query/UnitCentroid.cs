using BH.Engine.Geometry;
using BH.oM.Geometry;
using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Engine.TSP
{
    public static partial class Query
    {
        public static Point UnitCentroid(this Unit unit)
        {
            Point centroid = Engine.Geometry.Create.Point(unit.X / 2, unit.Y / 2, unit.Z / 2);
            TransformMatrix transform = Engine.Geometry.Create.OrientationMatrixGlobalToLocal(unit.CoordinateSystem);
            centroid = centroid.Transform(transform);
            return centroid;
        }
    }
}
