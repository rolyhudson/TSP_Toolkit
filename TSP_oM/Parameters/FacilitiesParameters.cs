using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.TSP
{
    public class FacilitiesParameters : BHoMObject
    {
        public virtual double ParkingSpacesPerApartment { get; set; } = 1;

        public virtual double RowDepth { get; set; } = 15;

        public virtual double NumberOfRows { get; set; } = 2;

        public virtual double ParkingBayWidth { get; set; } = 2.5;

        public virtual int TargetStories { get; set; } = 5;

        public virtual double CommercialAreaAsPercentOfTotalApartmentsArea { get; set; } = 5;

        public virtual double CommunalAreaAsPercentOfTotalApartmentsArea { get; set; } = 5;

        public virtual double MinimumLength { get; set; } = 40;

        public virtual double FloorToFloor { get; set; } = 4.5;

        public virtual double BaseOffset { get; set; } = 4.5;
    }
}
