using BH.Engine.Base;
using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BH.Engine.TSP
{
    public static partial class Modify
    {
        public static Field SetCurtilage(this Field field, Bar bar, PlanSettings settings)
        {
            Field fieldcopy = field.ShallowClone();
            foreach (Guid f in bar.Cells)
            {
                var refFootprint = fieldcopy.Cells.Find(x => x.BHoM_Guid.Equals(f));
                //get the unoccupied neighbours
                fieldcopy = fieldcopy.SetCurtilage(refFootprint, settings, 0);
                
            }
            int countOpen = fieldcopy.Cells.FindAll(x => x.Use == Use.Open).Count;
            return fieldcopy;
        }

        public static Field SetCurtilage(this Field field, Cell footprint, PlanSettings settings, int depth)
        {
            int countOpen = field.Cells.FindAll(x => x.Use == Use.Open).Count;
            depth++;
            foreach (Cell n in footprint.EightNeighbourhood.FindAll(x => x.Use == Use.Unoccupied || x.Use == Use.Circulation))
            {
                //no change to circulation
                if(n.Use != Use.Circulation)
                    n.Use = Use.Open;
                Cell check = field.Cells.Find(x => x.BHoM_Guid.Equals(n.BHoM_Guid));
                if(check!=null)
                {

                }
                if (depth < settings.MinimumUnitsSpace)
                    field = field.SetCurtilage(n, settings, depth);
            }
            return field;
        }
     }
}
