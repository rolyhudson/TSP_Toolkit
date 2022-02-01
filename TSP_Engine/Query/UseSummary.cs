using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BH.Engine.TSP
{
    public static partial class Query
    {
        public static Statistics UseSummary(Field field, List<Bar> bars, Unit prototypeUnit, int numberOfApartments)
        {
            Statistics statistics = new Statistics();
            double unitArea = prototypeUnit.X * prototypeUnit.Y;
            statistics.TotalStudyArea = field.Cells.FindAll(x => !(x.Use is OutsideSiteLandUse)).Count * unitArea;
            statistics.OpenSpaceArea = field.Cells.FindAll(x => x.Use is OpenLandUse).Count * unitArea;
            statistics.NumberOfGroundFloorUnits = field.Cells.FindAll(x => x.Use is OccupiedLandUse).Count;
            statistics.BuiltAreaGroundFloor = statistics.NumberOfGroundFloorUnits * unitArea;
            statistics.CirculationSpaceArea = field.Cells.FindAll(x => x.Use is RoadLandUse).Count * unitArea;
            statistics.NumberOfStructuralUnits = bars.SelectMany(x => x.Units).Count();
            statistics.NumberOfApartments = (statistics.NumberOfStructuralUnits - statistics.NumberOfGroundFloorUnits) * numberOfApartments;
            statistics.TotalFloorArea = statistics.NumberOfStructuralUnits * unitArea;
            statistics.PercentBuiltSpace = Math.Round(statistics.BuiltAreaGroundFloor / statistics.TotalStudyArea * 100);
            statistics.PercentCirculationSpace = Math.Round(statistics.CirculationSpaceArea / statistics.TotalStudyArea * 100);
            statistics.PercentOpenSpace= Math.Round(statistics.OpenSpaceArea/ statistics.TotalStudyArea * 100);
            return statistics;
        }
    }
}
