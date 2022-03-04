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
            
            double length = communalParameters.MinimumLength;
            double socialAreaPerFloor = summary.NumberOfApartments * unitArea * communalParameters.SocialAreaAsPercentOfTotalAparmentsArea / 100 / communalParameters.TotalStories;
            double lengthSocial = socialAreaPerFloor / communalParameters.Depth;
            length += lengthSocial;

            double commercialAreaPerFloor = summary.NumberOfApartments * unitArea * communalParameters.CommercialAreaAsPercentOfTotalApartmentsArea / 100 / communalParameters.TotalStories;
            double lengthCommercial = commercialAreaPerFloor / communalParameters.Depth;
            length += lengthCommercial;

            int parkingSpacesRequired = (int)Math.Ceiling(summary.NumberOfApartments * communalParameters.ParkingSpacesPerApartment);
            int spacesInBuilidng = communalParameters.SpacesPerFloorForMinimumLength * communalParameters.TotalStories;
            int addtionalSpaces = parkingSpacesRequired - spacesInBuilidng;
            
            if (addtionalSpaces > 0)
            {
                int additionalSpacesPerFloor = (int)Math.Ceiling(addtionalSpaces / communalParameters.TotalStories * 1.0);
                int addtionalBaysPerFloor = (int)Math.Ceiling(additionalSpacesPerFloor / 4.0);
                double lengthParking = addtionalBaysPerFloor * 2.5;
                length += lengthParking;
            }
            List<Point> corners = new List<Point>()
            { 
                Geometry.Create.Point(0,0,0),
                Geometry.Create.Point(length,0,0),
                Geometry.Create.Point(length,communalParameters.Depth,0),
                Geometry.Create.Point(0,communalParameters.Depth,0),
                
            };

            Point average = Geometry.Query.Average(corners);
            Vector xvect = corners[1] - corners[0];
            Vector yvect = Geometry.Query.CrossProduct(Vector.ZAxis, xvect);
            Cartesian cartesian = Geometry.Create.CartesianCoordinateSystem(average, xvect, yvect);

            TransformMatrix transform = Geometry.Create.OrientationMatrix(cartesian, communalLand.CoordinateSystem);
            corners.Add(corners[0]);
            Polyline boundary = new Polyline()
            {
                ControlPoints = corners
            };
            boundary = boundary.Transform(transform);
            List<Polyline> floors = new List<Polyline>();
            for(int i = 0;i< communalParameters.TotalStories;i++)
            {
                Polyline floor = boundary.Translate(Vector.ZAxis * i * communalParameters.FloorToFloor);
                floors.Add(floor);
            }

            return new CommunalBlock() { Floors = floors };
        }
    }
}
