using BH.Engine.Base;
using BH.Engine.Geometry;
using BH.oM.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Engine.TSP
{
    public static partial class Query
    {
        public static Polyline AlignedBoundingBox(Vector principleDirection, Polyline siteBoundary)
        {
           
            double angle = principleDirection.Angle(Vector.YAxis, new Plane() { Normal = Vector.ZAxis, Origin = new Point() });
            Polyline boundaryCopy = siteBoundary.ShallowClone();
            //align with principle direction
            boundaryCopy = boundaryCopy.Rotate(boundaryCopy.ControlPoints[0], Vector.ZAxis, angle);


            //get the boundign box
            BoundingBox boundingBox = boundaryCopy.Bounds();
            List<Point> points = new List<Point>()
            {
                boundingBox.Min,
                new Point() { X = boundingBox.Min.X, Y = boundingBox.Max.Y, Z = 0 },
                boundingBox.Max,
                new Point() { X = boundingBox.Max.X, Y = boundingBox.Min.Y, Z = 0 },
                boundingBox.Min
            };
            Polyline boxOutline = Geometry.Create.Polyline(points);

            //rotate back
            boxOutline = boxOutline.Rotate(boundaryCopy.ControlPoints[0], Vector.ZAxis, -angle);
            return boxOutline;
        }
    }
}
