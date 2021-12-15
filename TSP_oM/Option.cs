using BH.oM.Base;
using BH.oM.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.TSP
{
    public class Option : BHoMObject
    {
        public virtual List<RenderMesh> Preview { get; set; } = new List<RenderMesh>();
        public virtual Statistics Statistics { get; set; } = new Statistics();
    }
}
