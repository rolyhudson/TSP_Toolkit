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

        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static Field ILevels(Field field, Unit unit, VerticalParameters parameters)
        {
            return Levels(field.Layout as dynamic, field, unit, parameters);
        }

        /***************************************************/

        public static Field Levels(HybridLayout layout, Field field, Unit unit, VerticalParameters parameters)
        {
            Field pField = new Field();
            pField.Boundary = field.Boundary;
            pField.Cells = field.Cells.FindAll(x => x.Tags.Contains("perimeter"));
            
            pField = Levels(layout.PerimeterLayout, pField, unit, parameters);

            Field bField = new Field();
            bField.Boundary = field.Boundary;
            bField.Cells = field.Cells.FindAll(x => x.Tags.Contains("internal"));
            
            bField = Levels(layout.BarsLayout, bField, unit, parameters);

            bField.Cells.AddRange(pField.Cells);

            return bField;
        }

        /***************************************************/

        public static Field Levels(PerimeterLayout layout, Field field, Unit unit, VerticalParameters parameters)
        {
            Field fieldcopy = field.ShallowClone();
            
            foreach (Cell f in fieldcopy.Cells.FindAll(x => x.Use is OccupiedLandUse))
            {
                //TODO change this approach to use IPerimeteLevelMethod
                switch (layout.PerimeterLevelMethod)
                {
                    case PerimeterLevel.Random:
                        f.Levels = m_Random.Next(layout.MinimumLevel, parameters.MaximumLevel + 1);
                        break;
                    case PerimeterLevel.Maximum:
                        f.Levels = parameters.MaximumLevel;
                        break;
                    case PerimeterLevel.Minimum:
                        f.Levels = layout.MinimumLevel;
                        break;
                    case PerimeterLevel.MaxCentre:
                        f.Levels = CellLevel(f, field, layout.PerimeterLevelMethod, parameters.MaximumLevel , layout.MinimumLevel);
                        break;
                    case PerimeterLevel.MaxEnds:
                        f.Levels = CellLevel(f, field, layout.PerimeterLevelMethod, parameters.MaximumLevel , layout.MinimumLevel);
                        break;
                    case PerimeterLevel.StartEnd:
                        f.Levels = CellLevel(f, field, layout.PerimeterLevelMethod, parameters.MaximumLevel, layout.MinimumLevel);
                        break;
                    case PerimeterLevel.EndStart:
                        f.Levels = CellLevel(f, field, layout.PerimeterLevelMethod, parameters.MaximumLevel, layout.MinimumLevel);
                        break;
                }

                
            }
            return fieldcopy;
        }

        /***************************************************/

        public static Field Levels(BarsLayout layout, Field field, Unit unit, VerticalParameters parameters)
        {
            Field fieldcopy = field.ShallowClone();
            foreach (Cell f in fieldcopy.Cells.FindAll(x => x.Use is OccupiedLandUse))
            {
                f.Levels(fieldcopy, unit, parameters, layout);
            }
            return fieldcopy;
        }

        /***************************************************/

        public static Cell Levels(this Cell cell, Field field, Unit unit, VerticalParameters parameters, ILayout layout)
        {
            List<Cell> nearestOthers = Query.AlignedNeighbours(cell, cell.CoordinateSystem.X, field, new OccupiedLandUse());
            double mindist = double.MaxValue;
            if (nearestOthers.Count == 0)
            {
                cell.Levels = parameters.MaximumLevel;
                return cell;
            }

            foreach (Cell n in nearestOthers)
            {
                double d = Math.Round(n.Centre.Distance(cell.Centre), 1);
                if (d < mindist)
                    mindist = d;
            }
            double nUnitsGap = (mindist / unit.X) - 1;
            BarsLayout barsLayout = layout as BarsLayout;
            cell.Levels = (int)(nUnitsGap / barsLayout.GapToHeightRatio);
            if (cell.Levels > parameters.MaximumLevel)
                cell.Levels = parameters.MaximumLevel;
            return cell;
        }

        /***************************************************/

        public static Field ILevels(ILayout layout, Field field, Unit unit, VerticalParameters parameters)
        {
            return null;
        }

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

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

        /***************************************************/

        private static Random m_Random = new Random();
    }
}
