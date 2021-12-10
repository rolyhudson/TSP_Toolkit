using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.TSP
{
    public class Statistics : BHoMObject
    {
        public virtual double TotalStudyArea { get; set; } = 0;
        public virtual int NumberOfUnits { get; set; } = 0;

        public virtual double FloorArea { get; set; } = 0;
        public virtual double BuiltArea { get; set; } = 0;
        public virtual double OpenSpaceArea { get; set; } = 0;
        public virtual double CirculationSpaceArea { get; set; } = 0;
        
        public virtual double PercentOpenSpace { get; set; } = 0;
        public virtual double PercentBuiltSpace { get; set; } = 0;
        public virtual double PercentCirculationSpace { get; set; } = 0;
    }
}
