using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.TSP
{
    public class Parameters : BHoMObject
    {
        public virtual Unit PrototypeUnit { get; set; } = new Unit();

        public virtual PlanParameters PlanParameters { get; set; } = new PlanParameters();

        public virtual VerticalParameters VerticalParameters { get; set; } = new VerticalParameters();

        public virtual FacilitiesParameters FacilitiesParameters { get; set; } = new FacilitiesParameters();

        public virtual SolarAccessParameters SolarAccessParameters { get; set; } = new SolarAccessParameters();

        public virtual CostParameters CostParameters { get; set; } = new CostParameters();

        public virtual object LayoutMethod { get; set; }

    }
}
