using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BH.Engine.TSP
{
    public static partial class Create
    {
        public static Bar Bar(ref Field field, PlanSettings settings)
        {
            Bar bar = new Bar();
            //find a start point
            List<Cell> unoccupied = field.Cells.FindAll(x => x.Use == Use.Unoccupied).ToList();
            if (unoccupied.Count == 0)
                return bar;
            int r = m_Random.Next(0, unoccupied.Count);
            Cell start = unoccupied[r];


            start.Use = Use.Occupied;
            int occupiedCount = field.Cells.FindAll(x => x.Use == Use.Occupied).Count;
            
            bar.Cells.Add(start.BHoM_Guid);
            //get aligned neighbours
            GrowBar(start, field, ref bar, settings);
            if (bar.Cells.Count < settings.MinimumUnits)
            {
                //reset as unoccupied
                foreach(Guid f in bar.Cells)
                {
                    var refFootprint = field.Cells.Find(x => x.BHoM_Guid.Equals(f));
                    refFootprint.Use = Use.Unoccupied;
                }
                bar.Cells = new List<Guid>();
            }
            field.SetCurtilage(bar, settings);
            return bar;
        }

        private static void GrowBar(Cell start, Field field, ref Bar bar, PlanSettings settings)
        {
            List<Cell> aligned = start.AlignedNeighbours(start.CoordinateSystem.Y, field);
            aligned = aligned.FindAll(x => x.Use == Use.Unoccupied).ToList();
            
            foreach (var f in aligned)
            {
                if (bar.Cells.Count < settings.MaximumUnits)
                {
                    bar.Cells.Add(f.BHoM_Guid);
                    f.Use = Use.Occupied;
                    GrowBar(f, field, ref bar, settings);
                }
            } 
            
        }

        private static Random m_Random { get; set; } = new Random();
    }
}
