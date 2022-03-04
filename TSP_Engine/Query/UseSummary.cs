using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BH.Engine.TSP
{
    public static partial class Query
    {
        public static UseSummary UseSummary(Result result, Unit prototypeUnit)
        {
            return UseSummary(result.Field, result.Bars, prototypeUnit);
        }
        public static UseSummary UseSummary(Field field, List<Bar> bars, Unit prototypeUnit)
        {
            UseSummary statistics = new UseSummary();
            double unitArea = prototypeUnit.X * prototypeUnit.Y;
            statistics.TotalStudyArea = field.Cells.FindAll(x => !(x.Use is OutsideSiteLandUse)).Count * unitArea;
            statistics.OpenSpaceArea = field.Cells.FindAll(x => x.Use is OpenLandUse).Count * unitArea;
            statistics.NumberOfGroundFloorUnits = field.Cells.FindAll(x => x.Use is OccupiedLandUse).Count;
            statistics.BuiltAreaGroundFloor = statistics.NumberOfGroundFloorUnits * unitArea;
            statistics.CirculationSpaceArea = field.Cells.FindAll(x => x.Use is RoadLandUse).Count * unitArea;
            statistics.NumberOfStructuralUnits = bars.SelectMany(x => x.Units).Count();
            statistics.NumberOfApartments = (statistics.NumberOfStructuralUnits - statistics.NumberOfGroundFloorUnits) * prototypeUnit.NumberOfApartments;
            statistics.TotalFloorArea = statistics.NumberOfStructuralUnits * unitArea;
            statistics.PercentBuiltSpace = Math.Round(statistics.BuiltAreaGroundFloor / statistics.TotalStudyArea * 100);
            statistics.PercentCirculationSpace = Math.Round(statistics.CirculationSpaceArea / statistics.TotalStudyArea * 100);
            statistics.PercentOpenSpace= Math.Round(statistics.OpenSpaceArea/ statistics.TotalStudyArea * 100);
            return statistics;
        }
    }
}
