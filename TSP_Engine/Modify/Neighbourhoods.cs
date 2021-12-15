using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Engine.TSP
{
    public static partial class Modify
    {
        public static Cell Neighbourhoods(this Cell cell, Field field)
        {
            foreach (Guid guid in field.Adjacency[cell.BHoM_Guid])
                cell.EightNeighbourhood.Add(field.Cells.Find(x => x.BHoM_Guid.Equals(guid)));
            return cell;
        }
    }
}
