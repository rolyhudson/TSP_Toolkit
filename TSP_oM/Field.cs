﻿using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.TSP
{
    public class Field : BHoMObject
    {
        public virtual List<Footprint> Footprints { get; set; } = new List<Footprint>();
    }
}