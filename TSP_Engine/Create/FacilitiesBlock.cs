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
        public static FacilitiesBlock FacilitiesBlock(Field field, List<Bar> bars, Unit prototypeUnit, FacilitiesParameters communalParameters, FacilitiesLandUse communalLand)
        {
            int numberApartments = bars.NumberOfApartments(prototypeUnit);
            double areaApartments = numberApartments * prototypeUnit.ApartmentArea;
            
            double lengthParking = communalParameters.MinimumLength;
            double totalCommunalArea = areaApartments * communalParameters.CommunalAreaAsPercentOfTotalApartmentsArea / 100;
            double socialAreaPerFloor = totalCommunalArea / communalParameters.TotalStories;
            double lengthCommunal = socialAreaPerFloor / communalParameters.Depth;


            double totalCommercialArea = areaApartments * communalParameters.CommercialAreaAsPercentOfTotalApartmentsArea / 100;
            double commercialAreaPerFloor = totalCommercialArea / communalParameters.TotalStories;
            double lengthCommercial = commercialAreaPerFloor / communalParameters.Depth;

            int parkingSpacesRequired = (int)Math.Ceiling(numberApartments * communalParameters.ParkingSpacesPerApartment);
            int spacesInBuilidng = communalParameters.SpacesPerFloorForMinimumLength * communalParameters.TotalStories;
            int addtionalSpaces = parkingSpacesRequired - spacesInBuilidng;
            
            if (addtionalSpaces > 0)
            {
                int additionalSpacesPerFloor = (int)Math.Ceiling(addtionalSpaces / communalParameters.TotalStories * 1.0);
                int addtionalBaysPerFloor = (int)Math.Ceiling(additionalSpacesPerFloor / 4.0);
                lengthParking += addtionalBaysPerFloor * 2.5;
            }
            List<Point> cornersSocial = new List<Point>()
            { 
                Geometry.Create.Point(0,0,0),
                Geometry.Create.Point(lengthCommunal,0,0),
                Geometry.Create.Point(lengthCommunal,communalParameters.Depth,0),
                Geometry.Create.Point(0,communalParameters.Depth,0),
                Geometry.Create.Point(0,0,0),

            };
            List<Point> cornersCommercial = new List<Point>()
            {
                Geometry.Create.Point(lengthCommunal,0,0),
                Geometry.Create.Point(lengthCommunal+lengthCommercial,0,0),
                Geometry.Create.Point(lengthCommunal+lengthCommercial,communalParameters.Depth,0),
                Geometry.Create.Point(lengthCommunal,communalParameters.Depth,0),
                Geometry.Create.Point(lengthCommunal,0,0),

            };
            List<Point> cornersParking = new List<Point>()
            {
                Geometry.Create.Point(lengthCommunal+lengthCommercial,0,0),
                Geometry.Create.Point(lengthCommunal+lengthCommercial+lengthParking,0,0),
                Geometry.Create.Point(lengthCommunal+lengthCommercial+lengthParking,communalParameters.Depth,0),
                Geometry.Create.Point(lengthCommunal+lengthCommercial,communalParameters.Depth,0),
                 Geometry.Create.Point(lengthCommunal+lengthCommercial,0,0),
            };
            List<Point> corners = new List<Point>()
            {
                Geometry.Create.Point(0,0,0),
                Geometry.Create.Point(lengthCommunal+lengthCommercial+lengthParking,0,0),
                Geometry.Create.Point(lengthCommunal+lengthCommercial+lengthParking,communalParameters.Depth,0),
                Geometry.Create.Point(0,communalParameters.Depth,0),
                Geometry.Create.Point(0,0,0),

            };
            Point average = Geometry.Query.Average(corners);
            Vector xvect = corners[1] - corners[0];
            Vector yvect = Geometry.Query.CrossProduct(Vector.ZAxis, xvect);
            Cartesian cartesian = Geometry.Create.CartesianCoordinateSystem(average, xvect, yvect);

            TransformMatrix transform = Geometry.Create.OrientationMatrix(cartesian, communalLand.CoordinateSystem);

            Polyline boundary = new Polyline() { ControlPoints = corners};
            boundary = boundary.Transform(transform).Offset(communalParameters.BaseOffset);

            Polyline parkingBoundary = new Polyline(){ ControlPoints = cornersParking};
            parkingBoundary = parkingBoundary.Transform(transform);

            Polyline communalBoundary = new Polyline() { ControlPoints = cornersSocial };
            communalBoundary = communalBoundary.Transform(transform);

            Polyline commercialBoundary = new Polyline() { ControlPoints = cornersCommercial };
            commercialBoundary = commercialBoundary.Transform(transform);

            FacilitiesBlock FacilitiesBlock = new FacilitiesBlock();
            for(int i = 0;i< communalParameters.TotalStories;i++)
            {
                Polyline floor = parkingBoundary.Translate(Vector.ZAxis * i * communalParameters.FloorToFloor);
                FacilitiesBlock.Parking.Add(floor);

                floor = communalBoundary.Translate(Vector.ZAxis * i * communalParameters.FloorToFloor);
                FacilitiesBlock.Communal.Add(floor);

                floor = commercialBoundary.Translate(Vector.ZAxis * i * communalParameters.FloorToFloor);
                FacilitiesBlock.Commercial.Add(floor);
            }
            FacilitiesBlock.Boundary = boundary;
            FacilitiesBlock.ParkingSpaces = parkingSpacesRequired;
            FacilitiesBlock.CommercialArea = totalCommercialArea;
            FacilitiesBlock.CommunalArea = totalCommunalArea;
            return FacilitiesBlock;
        }
    }
}
