using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BH.Engine.TSP
{
    public static partial class Modify
    {
        public static Field SetCurtilage(this Field field, Bar bar, Settings settings)
        {
            
            foreach (Guid f in bar.Footprints)
            {
                var refFootprint = field.Footprints.Find(x => x.BHoM_Guid.Equals(f));
                //get the unoccupied neighbours
                field = field.SetCurtilage(refFootprint, settings, 0);
                
            }
            int countOpen = field.Footprints.FindAll(x => x.Use == Use.Open).Count;
            return field;
        }

        public static Field SetCurtilage(this Field field, Footprint footprint, Settings settings, int depth)
        {
            int countOpen = field.Footprints.FindAll(x => x.Use == Use.Open).Count;
            depth++;
            foreach (Footprint n in footprint.EightNeighbourhood.FindAll(x => x.Use == Use.Unoccupied))
            {
                n.Use = Use.Open;
                
                if (depth < settings.MinimumUnitsSpace)
                    field.SetCurtilage(n, settings, depth);
            }
            return field;
        }
     }
}
