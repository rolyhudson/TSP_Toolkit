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

        public virtual CommunalParameters CommunalParameters { get; set; } = new CommunalParameters();

        public virtual SolarAccessParameters SolarAccessParameters { get; set; } = new SolarAccessParameters();
        public virtual object LayoutMethod { get; set; }

    }
}
