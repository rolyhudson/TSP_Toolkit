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
                var refcell = fieldcopy.Cells.Find(x => x.BHoM_Guid.Equals(f));
                //get the unoccupied neighbours
                fieldcopy = fieldcopy.SetCurtilage(refcell, settings, 0);
                
            }
            int countOpen = fieldcopy.Cells.FindAll(x => x.Use is OpenLandUse).Count;
            return fieldcopy;
        }

        public static Field SetCurtilage(this Field field, Cell cell, PlanSettings settings, int depth)
        {
            int countOpen = field.Cells.FindAll(x => x.Use is OpenLandUse).Count;
            depth++;
            foreach (Cell n in cell.EightNeighbourhood.FindAll(x => x.Use is UnoccupiedLandUse || x.Use is RoadLandUse))
            {
                //no change to circulation
                if(!(n.Use is RoadLandUse))
                    n.Use = new OpenLandUse();
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
