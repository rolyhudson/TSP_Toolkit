using BH.oM.Geometry;
using BH.Engine.Geometry;
using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Engine.TSP
{
    public static partial class Query
    {
        public static List<Point> BoundaryPoints(this Unit unit)
        {
            List<Point> points = new List<Point>()
            {
                new Point(),
                Engine.Geometry.Create.Point(unit.X,0,0),
                Engine.Geometry.Create.Point(unit.X,unit.Y,0),
                Engine.Geometry.Create.Point(0,unit.Y,0),
                new Point(),
            };
            return points;
        }
    }
}
