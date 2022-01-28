using BH.Engine.Base;
using BH.Engine.Geometry;
using BH.oM.Base;
using BH.oM.Base.Attributes;
using BH.oM.Geometry;
using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Engine.TSP
{
    public static partial class Create
    {
        [MultiOutput(0, "bars", "Linear blocks.")]
        [MultiOutput(1, "field", "Updated field.")]
        public static Output<List<Bar>, Field> IBars(Field field, PlanSettings settings)
        {
            return Bars(field.Layout as dynamic, field, settings); 
        }

        [MultiOutput(0, "bars", "Linear blocks.")]
        [MultiOutput(1, "field", "Updated field.")]
        public static Output<List<Bar>, Field> Bars(ILayout layout, Field field, PlanSettings settings)
        {
            return null;
        }

        [MultiOutput(0, "bars", "Linear blocks.")]
        [MultiOutput(1, "field", "Updated field.")]
        public static Output<List<Bar>, Field> Bars(BarsLayout layout, Field field, PlanSettings settings)
        {
            List<Bar> bars = new List<Bar>();
            Field fieldCopy = field.ShallowClone();
            int fails = 0;
            while (fails < settings.MaximumPlacementAttempts)
            {
                int occupied = fieldCopy.Cells.FindAll(x => x.Use == Use.Occupied).Count;
                int open = fieldCopy.Cells.FindAll(x => x.Use == Use.Open).Count;
                Bar bar = Create.Bar(ref fieldCopy, settings);
                if (bar.Cells.Count == 0)
                    fails++;
                else
                    bars.Add(bar);
            }
            return new Output<List<Bar>, Field>
            {
                Item1 = bars,
                Item2 = fieldCopy,
            };
        }

        [MultiOutput(0, "bars", "Linear blocks.")]
        [MultiOutput(1, "field", "Updated field.")]
        public static Output<List<Bar>, Field> Bars(PerimeterLayout layout, Field field, PlanSettings settings)
        {
            List<Bar> bars = new List<Bar>();
            Field fieldCopy = field.ShallowClone();
            foreach (Line line in settings.SiteBoundary.SubParts())
            {
                Vector d = line.Direction();
                if (settings.SiteBoundary.IsClockwise(Vector.ZAxis))
                    d = d.Reverse();
                List<Cell> cells = fieldCopy.Cells.FindAll(x => x.CoordinateSystem.Y.IsParallel(d) == 1 && x.Use == Use.Unoccupied);
                if(cells.Count>0)
                {
                    cells.ForEach(x => x.Use = Use.Occupied);
                    Bar bar = new Bar();
                    cells.ForEach(x => bar.Cells.Add(x.BHoM_Guid));
                    bars.Add(bar);
                }
                    
            }

            return new Output<List<Bar>, Field>
            {
                Item1 = bars,
                Item2 = fieldCopy,
            };
        }

    }
}
