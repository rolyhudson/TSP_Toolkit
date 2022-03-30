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
        public static FacilitiesLandUse FacilitiesLandUse(Polyline boundary, double boundaryOffset, double communalDepth)
        {
            boundary = boundary.ForceClockwise();
            Polyline offsetA = boundary.Offset(boundaryOffset+0.1, Vector.ZAxis);
            offsetA = offsetA.ForceClockwise();
            List<Line> outeredges = offsetA.SubParts().OrderByDescending(x => x.Length()).ToList();

            Polyline offsetB = offsetA.Offset(communalDepth, Vector.ZAxis);
            List<Line> inneredges = offsetB.SubParts().OrderByDescending(x => x.Length()).ToList();

            int imax = 0;
            
            Plane edgePlane = Geometry.Create.Plane(inneredges[imax].Start, Vector.ZAxis.CrossProduct(inneredges[imax].Direction()));
            //should be 2 
            List<Point> pts = offsetA.PlaneIntersections(edgePlane);
            

            List<Point> corners = new List<Point>()
            {
                outeredges[imax].Start,
                outeredges[imax].End,
                pts[0],
                pts[1],
            };
            
            Vector xvect = outeredges[imax].Direction();
            xvect = xvect.Normalise();
            Vector yvect = Geometry.Query.CrossProduct(Vector.ZAxis, xvect);
            Cartesian cartesian = Geometry.Create.CartesianCoordinateSystem(outeredges[imax].PointAtParameter(0.5), xvect, yvect);

            corners.Add(corners[0]);
            Polyline parking = new Polyline() { ControlPoints = corners };
            
            FacilitiesLandUse parkingLand = new FacilitiesLandUse()
            {
                Boundary = parking,
                CoordinateSystem = cartesian
            };
            return parkingLand;
        }

        public static FacilitiesLandUse IFacilitiesLandUse(ILayout layout, FacilitiesParameters parameters, SiteLandUse siteLand)
        {
            return FacilitiesLandUse(layout as dynamic, parameters, siteLand);
        }

        public static FacilitiesLandUse FacilitiesLandUse(ILayout layout, FacilitiesParameters parameters, SiteLandUse siteLand)
        {
            return null;
        }

        public static FacilitiesLandUse FacilitiesLandUse(BarsLayout layout, FacilitiesParameters parameters, SiteLandUse siteLand)
        {
            double depth = parameters.NumberOfRows * parameters.RowDepth;
            return FacilitiesLandUse(siteLand.Boundary, layout.BoundaryOffset, depth);
        }

        public static FacilitiesLandUse FacilitiesLandUse(PerimeterLayout layout, FacilitiesParameters parameters, SiteLandUse siteLand)
        {
            double depth = parameters.NumberOfRows * parameters.RowDepth;
            return FacilitiesLandUse(siteLand.Boundary, layout.BoundaryOffset, depth);
        }

        public static FacilitiesLandUse FacilitiesLandUse(HybridLayout layout, FacilitiesParameters parameters, SiteLandUse siteLand)
        {
            double depth = parameters.NumberOfRows * parameters.RowDepth;
            return FacilitiesLandUse(siteLand.Boundary, layout.PerimeterLayout.BoundaryOffset, depth);
        }
    }
}
