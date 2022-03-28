using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.TSP
{
    public class CostParameters : BHoMObject
    {
        public virtual double HousingMetreSquared { get; set; } = 1;

        public virtual double ParkingMetreSquared { get; set; } = 1;

        public virtual double CommercialMetreSquared { get; set; } = 1;

        public virtual double GreenAreaMetreSquared { get; set; } = 1;

        public virtual double InternalCirculationMetreSquared { get; set; } = 1;

        public virtual double ExternalCirculationMetreSquared { get; set; } = 1;
    }
}
