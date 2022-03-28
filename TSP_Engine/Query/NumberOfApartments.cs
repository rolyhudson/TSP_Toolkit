using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Engine.TSP
{
    public static partial class Query
    {
        public static int NumberOfApartments(this List<Bar> bars, Unit prototypeUnit)
        {   
            return (bars.NumberOfUnits() - bars.NumberOfGroundFloorUnits()) * prototypeUnit.NumberOfApartments;
        }
    }
}
