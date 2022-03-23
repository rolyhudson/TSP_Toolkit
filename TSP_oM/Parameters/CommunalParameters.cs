using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.TSP
{
    public class CommunalParameters : BHoMObject
    {
        public virtual double ParkingSpacesPerApartment { get; set; } = 1;
        public virtual double Depth { get; set; } = 30;

        public virtual int TotalStories { get; set; } = 5;

        public virtual double CommercialAreaAsPercentOfTotalApartmentsArea { get; set; } = 5;

        public virtual double SocialAreaAsPercentOfTotalAparmentsArea { get; set; } = 5;

        public virtual double MinimumLength { get; set; } = 40;

        public virtual int SpacesPerFloorForMinimumLength { get; set; } = 32;

        public virtual double FloorToFloor { get; set; } = 4.5;

        public virtual double BaseOffset { get; set; } = 4.5;
    }
}
