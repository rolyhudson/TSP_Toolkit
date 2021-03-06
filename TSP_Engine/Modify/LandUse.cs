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

        public static Field LandUse(this Field field, FacilitiesLandUse landUse)
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

            return LandUseCrossing(field, landUse.Boundary, landUse);
        }

        /***************************************************/

        public static Field LandUse(this Field field, RoadLandUse landUse)
        {
            return LandUseCrossing(field, landUse.Boundary, landUse);
        }

        /***************************************************/

        public static Field LandUse(this Field field, ILandUse landUse)
        {
            //Base.Compute.RecordError("LandUse is unknown.");
            return field;
        }

        /***************************************************/

        private static Field LandUse(this Field field, Polyline polyline, ILandUse landUse)
        {
            if (polyline == null)
                return field;
            if (!polyline.IsClosed() && !polyline.IsPlanar())
            {
                //Base.Compute.RecordWarning("One or more of the open land use polylines provided was not closed or not planar.");
                return field;
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
                return field;
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
