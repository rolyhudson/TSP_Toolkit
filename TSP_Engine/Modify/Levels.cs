using BH.Engine.Base;
using BH.Engine.Geometry;
using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Engine.TSP
{
    public static partial class Modify
    {
        public static Field Levels(this Field field, Unit unit, VerticalSettings settings)
        {
            Field fieldcopy = field.ShallowClone();
            foreach (Footprint f in fieldcopy.Footprints.FindAll(x => x.Use == Use.Occupied))
            {
                f.Levels(fieldcopy, unit, settings);
            }
            return fieldcopy;
        }
        public static Footprint Levels(this Footprint footprint, Field field, Unit unit, VerticalSettings settings)
        {
            List<Footprint> nearestOthers = Query.AlignedNeighbours(footprint, footprint.CoordinateSystem.X, field, Use.Occupied);
            double mindist = double.MaxValue;
            if (nearestOthers.Count == 0)
            {
                footprint.Levels = settings.MaximumLevel;
                return footprint;
            }
                
            foreach (Footprint n in nearestOthers)
            {
                double d = Math.Round(n.Centre.Distance(footprint.Centre),1);
                if (d < mindist)
                    mindist = d;
            }
            double nUnitsGap = (mindist / unit.X)-1 ;
            footprint.Levels = (int)(nUnitsGap / settings.GapToHeightRatio);
            if (footprint.Levels > settings.MaximumLevel)
                footprint.Levels = settings.MaximumLevel;
            return footprint;
        }
    }
}
