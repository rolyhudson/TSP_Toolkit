using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BH.Engine.TSP
{
    public static partial class Create
    {
        public static Bar Bar(ref Field field, BarsLayout barsLayout)
        {
            Bar bar = new Bar();
            //find a start point
            List<Cell> unoccupied = field.CellsByUse(typeof(UnoccupiedLandUse));
            if (unoccupied.Count == 0)
                return bar;
            int r = m_Random.Next(0, unoccupied.Count);
            Cell start = unoccupied[r];

            
            start.Use = new OccupiedLandUse();
            var check = field.Cells.Find(x => x.BHoM_Guid.Equals(start.BHoM_Guid));
            if(check.EightNeighbourhood.Any(x => x.Use is OccupiedLandUse))
            {
                return bar;
            }
            int occupiedCount = field.Cells.FindAll(x => x.Use is OccupiedLandUse).Count;
            
            bar.Cells.Add(start.BHoM_Guid);
            //get aligned neighbours
            GrowBar(start, field, ref bar, barsLayout);
            if (bar.Cells.Count < barsLayout.MinimumUnits)
            {
                //reset as unoccupied
                foreach(Guid f in bar.Cells)
                {
                    var refcell = field.Cells.Find(x => x.BHoM_Guid.Equals(f));
                    refcell.Use = new UnoccupiedLandUse();
                }
                bar.Cells = new List<Guid>();
            }
            field = field.SetCurtilage(bar, barsLayout);
            return bar;
        }

        private static void GrowBar(Cell start, Field field, ref Bar bar, BarsLayout barsLayout)
        {
            List<Cell> aligned = start.AlignedNeighbours(start.CoordinateSystem.Y, field);
            aligned = aligned.FindAll(x => x.Use is UnoccupiedLandUse).ToList();
            int occupied = field.Cells.FindAll(x => x.Use is OccupiedLandUse).Count;
            foreach (var f in aligned)
            {
                if (bar.Cells.Count < barsLayout.MaximumUnits)
                {
                    bar.Cells.Add(f.BHoM_Guid);
                    f.Use = new OccupiedLandUse();
                    GrowBar(f, field, ref bar, barsLayout);
                }
            } 
            
        }

        private static Random m_Random { get; set; } = new Random();
    }
}
