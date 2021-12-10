using BH.Engine.Base;
using BH.oM.Reflection;
using BH.oM.Reflection.Attributes;
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
        public static Output<List<Bar>, Field> Bars(Field field, PlanSettings settings, int maxFails =10)
        {
            List<Bar> bars = new List<Bar>();
            Field fieldCopy = field.ShallowClone();
            int fails = 0;
            while(fails < maxFails)
            {
                int occupied = fieldCopy.Cells.FindAll(x => x.Use == Use.Occupied).Count;
                int open= fieldCopy.Cells.FindAll(x => x.Use == Use.Open).Count;
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
        
    }
}
