using BH.Engine.Geometry;
using BH.oM.Geometry;
using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using BH.oM.Geometry.CoordinateSystem;

namespace BH.Engine.TSP
{
    public static partial class Create 
    {
        public static CommunalLandUse CommunalLandUse(Polyline boundary, double boundaryOffset, double communalDepth)
        {
            Polyline offsetA = boundary.Offset(boundaryOffset, Vector.ZAxis);
            offsetA = offsetA.ForceClockwise();
            List<Line> outeredges = offsetA.SubParts().OrderByDescending(x => x.Length()).ToList();

            Polyline offsetB = offsetA.Offset(communalDepth, Vector.ZAxis);
            List<Line> inneredges = offsetB.SubParts().OrderByDescending(x => x.Length()).ToList();

            //extend inner
            Vector normal = Geometry.Query.CrossProduct(Vector.ZAxis, inneredges[0].Direction());
            Plane edgePlane = Geometry.Create.Plane(inneredges[0].Start, normal);
            //should be 2 
            List<Point> pts = offsetA.PlaneIntersections(edgePlane);
            Line longEdge = outeredges[0];
            Line shortEdge = Geometry.Create.Line(pts[0],pts[1]);
            if (shortEdge.Length() > longEdge.Length())
            {
                longEdge = shortEdge;
                shortEdge = outeredges[0];
            }

            Point a = longEdge.ClosestPoint(shortEdge.Start);
            Point b = longEdge.ClosestPoint(shortEdge.End);

            List<Point> corners = new List<Point>()
            {
                a,
                shortEdge.Start,
                shortEdge.End,
                b,
            };
            Point average = Geometry.Query.Average(corners);
            Vector xvect = shortEdge.Direction();
            Vector yvect = Geometry.Query.CrossProduct(Vector.ZAxis, xvect);
            Cartesian cartesian = Geometry.Create.CartesianCoordinateSystem(average, xvect, yvect);

            corners.Add(corners[0]);
            Polyline parking = new Polyline() { ControlPoints = corners };
            parking = parking.Offset(2, Vector.ZAxis);
            CommunalLandUse parkingLand = new CommunalLandUse()
            {
                Boundary = parking,
                CoordinateSystem = cartesian
            };
            return parkingLand;
        }

        public static CommunalLandUse ICommunalLandUse(ILayout layout, CommunalParameters parameters, SiteLandUse siteLand)
        {
            return CommunalLandUse(layout as dynamic, parameters, siteLand);
        }

        public static CommunalLandUse CommunalLandUse(ILayout layout, CommunalParameters parameters, SiteLandUse siteLand)
        {
            return null;
        }

        public static CommunalLandUse CommunalLandUse(BarsLayout layout, CommunalParameters parameters, SiteLandUse siteLand)
        {
            return CommunalLandUse(siteLand.Boundary, layout.BoundaryOffset, parameters.Depth);
        }

        public static CommunalLandUse CommunalLandUse(PerimeterLayout layout, CommunalParameters parameters, SiteLandUse siteLand)
        {
            return CommunalLandUse(siteLand.Boundary, layout.BoundaryOffset, parameters.Depth);
        }

        public static CommunalLandUse CommunalLandUse(HybridLayout layout, CommunalParameters parameters, SiteLandUse siteLand)
        {
            
            return CommunalLandUse(siteLand.Boundary, layout.PerimeterLayout.BoundaryOffset, parameters.Depth);
        }
    }
}
