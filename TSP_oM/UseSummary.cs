using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.TSP
{
    public class UseSummary : BHoMObject
    {
        public virtual double PlotArea { get; set; } = 0;

        public virtual double NetPlotArea { get; set; } = 0;

        public virtual double UsedArea { get; set; } = 0;

        public virtual double Occupation { get; set; } = 0;

        public virtual double InternalCirculation { get; set; } = 0;

        public virtual double ExternalCirculation { get; set; } = 0;

        public virtual double TotalCirculation { get; set; } = 0;

        public virtual int HousingUnitsNumber { get; set; } = 0;

        public virtual double HousingArea { get; set; } = 0;

        public virtual double ParkingArea { get; set; } = 0;

        public virtual int ParkingSpaces { get; set; } = 0;

        public virtual double InternalCommercialArea { get; set; } = 0;

        public virtual double ExternalCommercialArea { get; set; } = 0;

        public virtual double TotalCommercialArea { get; set; } = 0;

        public virtual double CommunalArea { get; set; } = 0;

        public virtual double GreenArea { get; set; } = 0;

        public virtual double SellableArea { get; set; } = 0;

        public virtual double EstimatedConstructionCost { get; set; } = 0;


    }
}
