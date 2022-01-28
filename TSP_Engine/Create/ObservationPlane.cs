using BH.Engine.Geometry;
using BH.oM.Geometry;
using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Engine.TSP
{
    public static partial class Create
    {
        public static List<Plane> ObservationPlanes(this Unit unit)
        {
            List<Plane> planes = new List<Plane>();
            TransformMatrix transform = BH.Engine.Geometry.Create.OrientationMatrixGlobalToLocal(unit.CoordinateSystem);
            Plane planeA = Geometry.Create.Plane(Geometry.Create.Point(0, unit.Y / 2, unit.Z / 2), Vector.XAxis * -1);
            planeA = planeA.Transform(transform);
            planes.Add(planeA);

            Plane planeB = Geometry.Create.Plane(Geometry.Create.Point(unit.X, unit.Y / 2, unit.Z / 2), Vector.XAxis);
            planeB = planeB.Transform(transform);
            planes.Add(planeB);
            
            
            return planes;
        }
    }
}
