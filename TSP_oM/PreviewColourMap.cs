using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace BH.oM.TSP
{
    public class PreviewColourMap : BHoMObject
    {
        public virtual Dictionary<string, Color> Map { get; set; } = new Dictionary<string, Color>();
    }
}
