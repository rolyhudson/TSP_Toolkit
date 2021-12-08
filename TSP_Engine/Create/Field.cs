using BH.Engine.Base;
using BH.Engine.Geometry;
using BH.oM.Geometry;
using BH.oM.Geometry.CoordinateSystem;
using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Engine.TSP
{
    public static partial class Create
    {
        public static Field Field(Vector principleDirection, Polyline siteBoundary, Unit prototypeUnit)
        {
            Polyline alignedBoundary = Query.AlignedBoundingBox(principleDirection, siteBoundary);
            Cartesian origin = Query.GridCartesian(principleDirection, siteBoundary);
            int unitsX = (int)Math.Ceiling(alignedBoundary.ControlPoints[0].Distance(alignedBoundary.ControlPoints[1]) / prototypeUnit.Width);
            int unitsY = (int)Math.Ceiling(alignedBoundary.ControlPoints[0].Distance(alignedBoundary.ControlPoints[3]) / prototypeUnit.Length);

            List<Point> points = new List<Point>()
            {
                new Point(),
                Geometry.Create.Point(prototypeUnit.Width,0,0),
                Geometry.Create.Point(prototypeUnit.Width,prototypeUnit.Length,0),
                Geometry.Create.Point(0,prototypeUnit.Length,0),
                new Point(),
            };

            Polyline boundary = Geometry.Create.Polyline(points);
            TransformMatrix transform = Geometry.Create.OrientationMatrixGlobalToLocal(origin);
            boundary = boundary.Transform(transform);

            Footprint basePrint = new Footprint()
            {
                Boundary = boundary,
                CoordinateSystem = origin
            };
            Field field = new Field();
            for(int i = 0;i <= unitsX;i++)
            {
                for(int j = 0;j< unitsY;j++)
                {
                    Footprint copy = basePrint.ShallowClone();
                    Cartesian cartesian = origin.ShallowClone();
                    Vector v = origin.X * i * prototypeUnit.Width + origin.Y * j* prototypeUnit.Length;
                    copy.Boundary = copy.Boundary.Translate(v);
                    copy.CoordinateSystem = cartesian.Translate(v);
                    field.Footprints.Add(copy);
                }
            }
            return field;
        }
    }
}
