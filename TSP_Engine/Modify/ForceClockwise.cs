using BH.Engine.Base;
using BH.Engine.Geometry;
using BH.oM.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Engine.TSP
{
    public static partial class Modify
    {
        public static Polyline ForceClockwise(this Polyline polyline)
        {
            Polyline clone = polyline.ShallowClone();
            if (clone.IsClockwise(Vector.ZAxis))
                clone = clone.Flip();
            return clone;
        }
    }
}
