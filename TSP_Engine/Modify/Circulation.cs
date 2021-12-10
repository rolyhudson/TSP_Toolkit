﻿using BH.Engine.Base;
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
        public static Field Circulation(this Field field, List<Polyline> circulationRoutes)
        {
            Field fieldcopy = field.ShallowClone();
            foreach (Polyline polyline in circulationRoutes)
            {
                if (!polyline.IsPlanar())
                {
                    Reflection.Compute.RecordWarning("One or more of the polylines provided was not planar.");
                    continue;
                }
                foreach (Cell footprint in fieldcopy.Footprints.FindAll(x => x.Use != Use.OutsideBoundary && x.Use != Use.Circulation))
                {
                    List<Point> intersections = footprint.Boundary.LineIntersections(polyline);
                    if (intersections.Count > 0)
                        footprint.Use = Use.Circulation;
                }
            }
            return fieldcopy;
        }
    }
}