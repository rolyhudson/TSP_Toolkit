using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BH.Engine.TSP
{
    public static partial class Query
    {
        public static Statistics UseSummary(Field field, List<Bar> bars, Unit prototypeUnit)
        {
            Statistics statistics = new Statistics();
            double unitArea = prototypeUnit.X * prototypeUnit.Y;
            statistics.TotalStudyArea = field.Cells.FindAll(x => x.Use != Use.OutsideBoundary).Count * unitArea;
            statistics.OpenSpaceArea = field.Cells.FindAll(x => x.Use == Use.Open).Count * unitArea;
            statistics.BuiltArea = field.Cells.FindAll(x => x.Use == Use.Occupied).Count * unitArea;
            statistics.CirculationSpaceArea = field.Cells.FindAll(x => x.Use == Use.Circulation).Count * unitArea;
            statistics.NumberOfUnits = bars.SelectMany(x => x.Units).Count();
            statistics.FloorArea = statistics.NumberOfUnits * unitArea;
            statistics.PercentBuiltSpace = Math.Round(statistics.BuiltArea / statistics.TotalStudyArea * 100);
            statistics.PercentCirculationSpace = Math.Round(statistics.CirculationSpaceArea / statistics.TotalStudyArea * 100);
            statistics.PercentOpenSpace= Math.Round(statistics.OpenSpaceArea/ statistics.TotalStudyArea * 100);
            return statistics;
        }
    }
}
