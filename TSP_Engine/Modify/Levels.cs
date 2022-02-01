using BH.Engine.Base;
using BH.Engine.Geometry;
using BH.oM.Geometry;
using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BH.Engine.TSP
{
    public static partial class Modify
    {
        public static Field ILevels(Field field, Unit unit, VerticalSettings settings)
        {
            return Levels(field.Layout as dynamic, field, unit, settings);
        }
        public static Field Levels(PerimeterLayout layout, Field field, Unit unit, VerticalSettings settings)
        {
            Field fieldcopy = field.ShallowClone();
            
            foreach (Cell f in fieldcopy.Cells.FindAll(x => x.Use is OccupiedLandUse))
            {
                //TODO change this approach to use IPerimeteLevelMethod
                switch (layout.PerimeterLevelMethod)
                {
                    case PerimeterLevel.Random:
                        f.Levels = m_Random.Next(layout.MinimumLevel, settings.MaximumLevel + 1);
                        break;
                    case PerimeterLevel.Maximum:
                        f.Levels = settings.MaximumLevel;
                        break;
                    case PerimeterLevel.Minimum:
                        f.Levels = layout.MinimumLevel;
                        break;
                    case PerimeterLevel.MaxCentre:
                        f.Levels = CellLevel(f, field, layout.PerimeterLevelMethod, settings.MaximumLevel , layout.MinimumLevel);
                        break;
                    case PerimeterLevel.MaxEnds:
                        f.Levels = CellLevel(f, field, layout.PerimeterLevelMethod, settings.MaximumLevel , layout.MinimumLevel);
                        break;
                    case PerimeterLevel.StartEnd:
                        f.Levels = CellLevel(f, field, layout.PerimeterLevelMethod, settings.MaximumLevel, layout.MinimumLevel);
                        break;
                    case PerimeterLevel.EndStart:
                        f.Levels = CellLevel(f, field, layout.PerimeterLevelMethod, settings.MaximumLevel, layout.MinimumLevel);
                        break;
                }

                
            }
            return fieldcopy;
        }

        private static int CellLevel(Cell cell, Field field, PerimeterLevel method, int levelMax, int levelMin)
        {
            List<Cell> nearestOthers = Query.AlignedNeighbours(cell, cell.CoordinateSystem.Y, field, new OccupiedLandUse());
            SortedList<double, Guid> distDict = new SortedList<double, Guid>();
            nearestOthers.Add(cell);
            foreach(Cell c in nearestOthers)
            {
                Vector v = Geometry.Create.Vector(cell.Centre, c.Centre);
                int parallel = v.IsParallel(cell.CoordinateSystem.Y);
                distDict.Add(v.Length() * parallel, c.BHoM_Guid);
            }


            double halfBarLength = distDict.Count;
            if (method == PerimeterLevel.MaxCentre || method == PerimeterLevel.MaxEnds)
                halfBarLength = halfBarLength / 2;
            int cellPos = distDict.IndexOfValue(cell.BHoM_Guid);
           
            if (cellPos > halfBarLength)
                cellPos = distDict.Count - cellPos;

            double p = cellPos / halfBarLength;
            int levelRange = levelMax - levelMin;
            int levels = 0;
            if (method == PerimeterLevel.MaxCentre || method == PerimeterLevel.StartEnd)
                levels = (int)(levelRange * p) + levelMin ;
            else
                levels = (int)(levelRange * (1 -p)) + levelMin ;

            return levels;
            
        }

        public static Field Levels(BarsLayout layout, Field field, Unit unit, VerticalSettings settings)
        {
            Field fieldcopy = field.ShallowClone();
            foreach (Cell f in fieldcopy.Cells.FindAll(x => x.Use is OccupiedLandUse))
            {
                f.Levels(fieldcopy, unit, settings);
            }
            return fieldcopy;
        }
        public static Cell Levels(this Cell cell, Field field, Unit unit, VerticalSettings settings)
        {
            List<Cell> nearestOthers = Query.AlignedNeighbours(cell, cell.CoordinateSystem.X, field, new OccupiedLandUse());
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
            BarsLayout barsLayout = settings.LayoutMethod as BarsLayout;
            cell.Levels = (int)(nUnitsGap / barsLayout.GapToHeightRatio);
            if (cell.Levels > settings.MaximumLevel)
                cell.Levels = settings.MaximumLevel;
            return cell;
        }
        public static Field ILevels(ILayout layout, Field field, Unit unit, VerticalSettings settings)
        {
            return null;
        }

        private static Random m_Random = new Random();
    }
}
