using BH.Engine.Geometry;
using BH.oM.Geometry;
using BH.oM.Geometry.CoordinateSystem;
using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BH.Engine.TSP
{
    public static partial class Create 
    {
        public static FacilitiesBlock FacilitiesBlock(Field field, List<Bar> bars, Unit prototypeUnit, FacilitiesParameters facilitiesParameters, FacilitiesLandUse facilitiesLand)
        {
            int numberApartments = bars.NumberOfApartments(prototypeUnit);
            double areaApartments = numberApartments * prototypeUnit.ApartmentArea;
            double depth = facilitiesParameters.NumberOfRows * facilitiesParameters.RowDepth;
            
            
            
            FacilitiesBlock FacilitiesBlock = new FacilitiesBlock();
            //check if facilities fit in the boundary
            bool validFacilities = false;

            while (!validFacilities)
            {
                double lengthParking = facilitiesParameters.MinimumLength;
                int spacesPerFloor = (int)(Math.Floor(lengthParking / 2.5) * facilitiesParameters.NumberOfRows * 2);
                double totalCommunalArea = areaApartments * facilitiesParameters.CommunalAreaAsPercentOfTotalApartmentsArea / 100;
                double socialAreaPerFloor = totalCommunalArea / facilitiesParameters.TargetStories;
                double lengthCommunal = socialAreaPerFloor / depth;

                double totalCommercialArea = areaApartments * facilitiesParameters.CommercialAreaAsPercentOfTotalApartmentsArea / 100;
                double commercialAreaPerFloor = totalCommercialArea / facilitiesParameters.TargetStories;
                double lengthCommercial = commercialAreaPerFloor / depth;

                int parkingSpacesRequired = (int)Math.Ceiling(numberApartments * facilitiesParameters.ParkingSpacesPerApartment);
                int spacesInBuilidng = spacesPerFloor * facilitiesParameters.TargetStories;
                int addtionalSpaces = parkingSpacesRequired - spacesInBuilidng;


                int additionalSpacesPerFloor = (int)Math.Ceiling(addtionalSpaces / facilitiesParameters.TargetStories * 1.0);
                int addtionalBaysPerFloor = (int)Math.Ceiling(additionalSpacesPerFloor / facilitiesParameters.NumberOfRows * 2);
                lengthParking += addtionalBaysPerFloor * 2.5;

                List<Point> corners = new List<Point>()
                {
                    Geometry.Create.Point(0,0,0),
                    Geometry.Create.Point(lengthCommunal+lengthCommercial+lengthParking,0,0),
                    Geometry.Create.Point(lengthCommunal+lengthCommercial+lengthParking,depth,0),
                    Geometry.Create.Point(0,depth,0),
                    Geometry.Create.Point(0,0,0),

                };

                List<Point> cornersSocial = new List<Point>()
                {
                    Geometry.Create.Point(0,0,0),
                    Geometry.Create.Point(lengthCommunal,0,0),
                    Geometry.Create.Point(lengthCommunal,depth,0),
                    Geometry.Create.Point(0,depth,0),
                    Geometry.Create.Point(0,0,0),

                };
                List<Point> cornersCommercial = new List<Point>()
                {
                    Geometry.Create.Point(lengthCommunal,0,0),
                    Geometry.Create.Point(lengthCommunal+lengthCommercial,0,0),
                    Geometry.Create.Point(lengthCommunal+lengthCommercial,depth,0),
                    Geometry.Create.Point(lengthCommunal,depth,0),
                    Geometry.Create.Point(lengthCommunal,0,0),

                };
                List<Point> cornersParking = new List<Point>()
                {
                    Geometry.Create.Point(lengthCommunal+lengthCommercial,0,0),
                    Geometry.Create.Point(lengthCommunal+lengthCommercial+lengthParking,0,0),
                    Geometry.Create.Point(lengthCommunal+lengthCommercial+lengthParking,depth,0),
                    Geometry.Create.Point(lengthCommunal+lengthCommercial,depth,0),
                    Geometry.Create.Point(lengthCommunal+lengthCommercial,0,0),
                };
                
                Point average = Geometry.Query.Average(new List<Point>() { corners[1], corners[0] });
                Polyline bound = field.Boundary.ForceClockwise();
                List<Line> edges = bound.SubParts().OrderByDescending(x => x.Length()).ToList();

                Vector xvect = corners[0] - corners[1];
                xvect = xvect.Normalise();
                Vector yvect = Geometry.Query.CrossProduct(Vector.ZAxis, xvect);
                Cartesian cartesian = Geometry.Create.CartesianCoordinateSystem(average, xvect, yvect);

                TransformMatrix transform = Geometry.Create.OrientationMatrix(cartesian, facilitiesLand.CoordinateSystem);

                Polyline boundary = new Polyline() { ControlPoints = corners };
                boundary = boundary.Transform(transform);//.Offset(facilitiesParameters.BaseOffset);

                FacilitiesBlock.Parking = SetLevels(cornersParking, facilitiesParameters, transform);
                FacilitiesBlock.Communal = SetLevels(cornersSocial, facilitiesParameters, transform);
                FacilitiesBlock.Commercial = SetLevels(cornersCommercial, facilitiesParameters, transform);
                
                FacilitiesBlock.Boundary = boundary;
                FacilitiesBlock.ParkingSpaces = parkingSpacesRequired;
                FacilitiesBlock.CommercialArea = totalCommercialArea;
                FacilitiesBlock.CommunalArea = totalCommunalArea;
                
                validFacilities = FacilitiesBlock.IsValid(field.Boundary);
                //if not valid add another floor
                if (!validFacilities)
                    facilitiesParameters.TargetStories++;
            }
            FacilitiesBlock.Boundary = FacilitiesBlock.Boundary.Offset(facilitiesParameters.BaseOffset);
            return FacilitiesBlock;
        }
        private static List<Polyline> SetLevels(List<Point> corners, FacilitiesParameters facilitiesParameters, TransformMatrix transform)
        {
            
            Polyline boundary = new Polyline() { ControlPoints = corners };
            boundary = boundary.Transform(transform);
            List<Polyline> floors = new List<Polyline>();
            for (int i = 0; i < facilitiesParameters.TargetStories; i++)
            {
                floors.Add(boundary.Translate(Vector.ZAxis * i * facilitiesParameters.FloorToFloor));
            }
            return floors;
        }
    }
}
