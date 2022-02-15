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
        public static Field OpenSpace(this Field field, List<Polyline> openSpaces)
        {
            Field fieldcopy = field.ShallowClone();
            //foreach (Polyline polyline in openSpaces)
            //{
            //    if (!polyline.IsClosed() && !polyline.IsPlanar())
            //    {
            //        Reflection.Compute.RecordWarning("One or more of the polylines provided was not closed or not planar.");
            //        continue;
            //    }
            //    foreach(Cell cell in fieldcopy.Cells.FindAll(x => x.Use != Use.OutsideBoundary && x.Use != Use.Open))
            //    {
            //        if (polyline.IIsContaining(new List<Point>() { cell.Centre }))
            //            cell.Use = Use.Open;
            //    }
            //}
            return fieldcopy;
        }
    }
}
