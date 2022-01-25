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
        public static Field Levels(CloisterLayout layout, Field field, Unit unit, VerticalSettings settings)
        {
            Field fieldcopy = field.ShallowClone();
            foreach (Cell f in fieldcopy.Cells.FindAll(x => x.Use == Use.Occupied))
            {
                f.Levels = settings.MaximumLevel;
            }
            return fieldcopy;
        }


        public static Field Levels(this Field field, Unit unit, VerticalSettings settings)
        {
            return Levels(field.Layout as dynamic, field, unit, settings);
        }
        public static Field Levels(BarsLayout layout, Field field, Unit unit, VerticalSettings settings)
        {
            Field fieldcopy = field.ShallowClone();
            foreach (Cell f in fieldcopy.Cells.FindAll(x => x.Use == Use.Occupied))
            {
                f.Levels(fieldcopy, unit, settings);
            }
            return fieldcopy;
        }
        public static Cell Levels(this Cell cell, Field field, Unit unit, VerticalSettings settings)
        {
            List<Cell> nearestOthers = Query.AlignedNeighbours(cell, cell.CoordinateSystem.X, field, Use.Occupied);
            double mindist = double.MaxValue;
            if (nearestOthers.Count == 0)
            {
                cell.Levels = settings.MaximumLevel;
                return cell;
            }
                
            foreach (Cell n in nearestOthers)
            {
                double d = Math.Round(n.Centre.Distance(cell.Centre),1);
                if (d < mindist)
                    mindist = d;
            }
            double nUnitsGap = (mindist / unit.X)-1 ;
            cell.Levels = (int)(nUnitsGap / settings.GapToHeightRatio);
            if (cell.Levels > settings.MaximumLevel)
                cell.Levels = settings.MaximumLevel;
            return cell;
        }
        public static Field ILevels(ILayout layout, Field field, Unit unit, VerticalSettings settings)
        {
            return null;
        }
    }
}
