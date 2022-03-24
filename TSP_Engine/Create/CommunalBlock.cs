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
        public static CommunalBlock CommunalBlock(Field field, List<Bar> bars, Unit prototypeUnit, CommunalParameters communalParameters, CommunalLandUse communalLand)
        {
            UseSummary summary = Query.UseSummary(field, bars, prototypeUnit);
            double unitArea = prototypeUnit.X * prototypeUnit.Y;
            
            double lengthParking = communalParameters.MinimumLength;
            double totalSocialArea = summary.NumberOfApartments * unitArea * communalParameters.SocialAreaAsPercentOfTotalAparmentsArea / 100;
            double socialAreaPerFloor = totalSocialArea / communalParameters.TotalStories;
            double lengthSocial = socialAreaPerFloor / communalParameters.Depth;


            double totalCommercialArea = summary.NumberOfApartments * unitArea * communalParameters.CommercialAreaAsPercentOfTotalApartmentsArea / 100;
            double commercialAreaPerFloor = totalCommercialArea / communalParameters.TotalStories;
            double lengthCommercial = commercialAreaPerFloor / communalParameters.Depth;

            int parkingSpacesRequired = (int)Math.Ceiling(summary.NumberOfApartments * communalParameters.ParkingSpacesPerApartment);
            int spacesInBuilidng = communalParameters.SpacesPerFloorForMinimumLength * communalParameters.TotalStories;
            int addtionalSpaces = parkingSpacesRequired - spacesInBuilidng;
            
            if (addtionalSpaces > 0)
            {
                int additionalSpacesPerFloor = (int)Math.Ceiling(addtionalSpaces / communalParameters.TotalStories * 1.0);
                int addtionalBaysPerFloor = (int)Math.Ceiling(additionalSpacesPerFloor / 4.0);
                lengthParking = addtionalBaysPerFloor * 2.5;
            }
            List<Point> cornersSocial = new List<Point>()
            { 
                Geometry.Create.Point(0,0,0),
                Geometry.Create.Point(lengthSocial,0,0),
                Geometry.Create.Point(lengthSocial,communalParameters.Depth,0),
                Geometry.Create.Point(0,communalParameters.Depth,0),
                Geometry.Create.Point(0,0,0),

            };
            List<Point> cornersCommercial = new List<Point>()
            {
                Geometry.Create.Point(lengthSocial,0,0),
                Geometry.Create.Point(lengthSocial+lengthCommercial,0,0),
                Geometry.Create.Point(lengthSocial+lengthCommercial,communalParameters.Depth,0),
                Geometry.Create.Point(lengthSocial,communalParameters.Depth,0),
                Geometry.Create.Point(lengthSocial,0,0),

            };
            List<Point> cornersParking = new List<Point>()
            {
                Geometry.Create.Point(lengthSocial+lengthCommercial,0,0),
                Geometry.Create.Point(lengthSocial+lengthCommercial+lengthParking,0,0),
                Geometry.Create.Point(lengthSocial+lengthCommercial+lengthParking,communalParameters.Depth,0),
                Geometry.Create.Point(lengthSocial+lengthCommercial,communalParameters.Depth,0),
                 Geometry.Create.Point(lengthSocial+lengthCommercial,0,0),
            };
            List<Point> corners = new List<Point>()
            {
                Geometry.Create.Point(0,0,0),
                Geometry.Create.Point(lengthSocial+lengthCommercial+lengthParking,0,0),
                Geometry.Create.Point(lengthSocial+lengthCommercial+lengthParking,communalParameters.Depth,0),
                Geometry.Create.Point(0,communalParameters.Depth,0),
                Geometry.Create.Point(0,0,0),

            };
            Point average = Geometry.Query.Average(corners);
            Vector xvect = corners[1] - corners[0];
            Vector yvect = Geometry.Query.CrossProduct(Vector.ZAxis, xvect);
            Cartesian cartesian = Geometry.Create.CartesianCoordinateSystem(average, xvect, yvect);

            TransformMatrix transform = Geometry.Create.OrientationMatrix(cartesian, communalLand.CoordinateSystem);

            Polyline boundary = new Polyline() { ControlPoints = corners};
            boundary = boundary.Transform(transform);

            Polyline parkingBoundary = new Polyline(){ ControlPoints = cornersParking};
            parkingBoundary = parkingBoundary.Transform(transform);

            Polyline socialBoundary = new Polyline() { ControlPoints = cornersSocial };
            socialBoundary = socialBoundary.Transform(transform);

            Polyline commercialBoundary = new Polyline() { ControlPoints = cornersCommercial };
            commercialBoundary = commercialBoundary.Transform(transform);

            CommunalBlock communalBlock = new CommunalBlock();
            for(int i = 0;i< communalParameters.TotalStories;i++)
            {
                Polyline floor = parkingBoundary.Translate(Vector.ZAxis * i * communalParameters.FloorToFloor);
                communalBlock.Parking.Add(floor);

                floor = socialBoundary.Translate(Vector.ZAxis * i * communalParameters.FloorToFloor);
                communalBlock.Social.Add(floor);

                floor = commercialBoundary.Translate(Vector.ZAxis * i * communalParameters.FloorToFloor);
                communalBlock.Commercial.Add(floor);
            }
            communalBlock.Boundary = boundary;
            communalBlock.ParkingSpaces = parkingSpacesRequired;
            communalBlock.CommercialArea = totalCommercialArea;
            communalBlock.SocialArea = totalSocialArea;
            return communalBlock;
        }
    }
}
