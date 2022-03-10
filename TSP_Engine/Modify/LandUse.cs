using BH.Engine.Base;
using BH.Engine.Geometry;
using BH.oM.Geometry;
using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Engine.TSP
{
    public static partial class Modify
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static Field ILandUse(this Field field, ILandUse landUse)
        {
            return LandUse(field, landUse as dynamic);
        }

        /***************************************************/

        public static Field LandUse(this Field field, CommunalLandUse landUse)
        {
            return LandUseCrossing(field, landUse.Boundary, landUse);
        }

        /***************************************************/


        public static Field LandUse(this Field field, ParkingLandUse landUse)
        {
            return LandUse(field, landUse.Boundary, landUse); ;
        }

        /***************************************************/

        public static Field LandUse(this Field field, OpenLandUse landUse)
        {
            
            return LandUse(field, landUse.Boundary, landUse);
        }

        /***************************************************/

        public static Field LandUse(this Field field, RoadLandUse landUse)
        {
            if (landUse.CentreLine == null)
                return field;
            if (!landUse.CentreLine.IsPlanar())
            {
                //Base.Compute.RecordWarning("One or more of the road land use polylines provided was not planar.");
                return null;
            }
            Field fieldcopy = field.ShallowClone();
            String currentUse = landUse.GetType().ToString();
            foreach (Cell cell in fieldcopy.Cells.FindAll(x => !(x.Use is OutsideSiteLandUse) && !(x.Use is RoadLandUse)))
            {
                List<Point> intersections = cell.Boundary.LineIntersections(landUse.CentreLine);
                if (intersections.Count > 0)
                    cell.Use = landUse;
            }
            return fieldcopy;
        }

        /***************************************************/

        public static Field LandUse(this Field field, ILandUse landUse)
        {
            //Base.Compute.RecordError("LandUse is unknown.");
            return null;
        }

        /***************************************************/

        private static Field LandUse(this Field field, Polyline polyline, ILandUse landUse)
        {
            if (polyline == null)
                return field;
            if (!polyline.IsClosed() && !polyline.IsPlanar())
            {
                //Base.Compute.RecordWarning("One or more of the open land use polylines provided was not closed or not planar.");
                return null;
            }
            Field fieldcopy = field.ShallowClone();
            Type currentUse = landUse.GetType();
            //all not outside and not already tagged with the current use
            foreach (Cell cell in fieldcopy.Cells.FindAll(x => !(x.Use is OutsideSiteLandUse) && !(x.Use.GetType().Equals(currentUse))))
            {
                if (polyline.IIsContaining(new List<Point>() { cell.Centre }))
                    cell.Use = landUse;
            }
            return fieldcopy;
        }

        /***************************************************/

        private static Field LandUseCrossing(this Field field, Polyline polyline, ILandUse landUse)
        {
            if (polyline == null)
                return field;
            if (!polyline.IsClosed() && !polyline.IsPlanar())
            {
                //Base.Compute.RecordWarning("One or more of the open land use polylines provided was not closed or not planar.");
                return null;
            }
            Field fieldcopy = field.ShallowClone();
            Type currentUse = landUse.GetType();
            //all not outside and not already tagged with the current use
            foreach (Cell cell in fieldcopy.Cells.FindAll(x => !(x.Use is OutsideSiteLandUse) && !(x.Use.GetType().Equals(currentUse))))
            {
                if (polyline.IIsContaining(new List<Point>() { cell.Centre }))
                {
                    cell.Use = landUse;
                    continue;
                }
                    
                if (polyline.ICurveIntersections(cell.Boundary).Count > 0)
                    cell.Use = landUse;
            }
            return fieldcopy;
        }



    }
}
