using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Engine.TSP
{
    public static partial class Query
    {
        public static List<Cell> CellsByUse(this Field field, Type landUse)
        {
            return field.Cells.FindAll(c => c.Use.GetType().Equals(landUse));
        }
    }
}
