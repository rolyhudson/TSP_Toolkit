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
        public static Output<List<Bar>, Field> IBars(Field field, PlanParameters parameters)
        {
            return Bars(field.Layout as dynamic, field, parameters); 
        }

        [MultiOutput(0, "bars", "Linear blocks.")]
        [MultiOutput(1, "field", "Updated field.")]
        public static Output<List<Bar>, Field> Bars(ILayout layout, Field field, PlanParameters parameters)
        {
            return null;
        }

        [MultiOutput(0, "bars", "Linear blocks.")]
        [MultiOutput(1, "field", "Updated field.")]
        public static Output<List<Bar>, Field> Bars(HybridLayout layout, Field field, PlanParameters parameters)
        {
            Field pField= new Field();
            pField.Cells = field.Cells.FindAll(x => x.Tags.Contains("perimeter"));
            PerimeterLayout perimeterLayout = (PerimeterLayout)layout.Layouts.Find(x => x is PerimeterLayout);
            Output<List<Bar>, Field> pBars = Bars(perimeterLayout, pField, parameters);

            Field bField = new Field();
            bField.Cells = field.Cells.FindAll(x => x.Tags.Contains("internal"));
            BarsLayout barsLayout = (BarsLayout)layout.Layouts.Find(x => x is BarsLayout);
            Output<List<Bar>, Field> bBars = Bars(barsLayout, bField, parameters);

            Output<List<Bar>, Field> combined = pBars;
            combined.Item1.AddRange(bBars.Item1);
            combined.Item2.Cells.AddRange(bBars.Item2.Cells);
            combined.Item2.Layout = layout;

            return combined;

        }

        [MultiOutput(0, "bars", "Linear blocks.")]
        [MultiOutput(1, "field", "Updated field.")]
        public static Output<List<Bar>, Field> Bars(BarsLayout layout, Field field, PlanParameters parameters)
        {
            List<Bar> bars = new List<Bar>();
            Field fieldCopy = field.ShallowClone();
            int fails = 0;
            while (fails < layout.MaximumPlacementAttempts)
            {
                int occupied = fieldCopy.Cells.FindAll(x => x.Use is OccupiedLandUse).Count;
                int open = fieldCopy.Cells.FindAll(x => x.Use is OpenLandUse).Count;
                Bar bar = Create.Bar(ref fieldCopy, parameters);
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
        public static Output<List<Bar>, Field> Bars(PerimeterLayout layout, Field field, PlanParameters parameters)
        {
            ILandUse landUse = Query.FindSiteUse(parameters.LandUses);
            if (landUse == null)
            {
                Base.Compute.RecordError("No site land use was found. A site land use is required.");
                return new Output<List<Bar>, Field>();
            }
            SiteLandUse siteLandUse = landUse as SiteLandUse;
            List<Bar> bars = new List<Bar>();
            Field fieldCopy = field.ShallowClone();
            foreach (Line line in siteLandUse.Boundary.SubParts())
            {
                Vector d = line.Direction();
                if (siteLandUse.Boundary.IsClockwise(Vector.ZAxis))
                    d = d.Reverse();
                List<Cell> cells = fieldCopy.Cells.FindAll(x => x.CoordinateSystem.Y.IsParallel(d) == 1 && x.Use is UnoccupiedLandUse);
                if(cells.Count>0)
                {
                    cells.ForEach(x => x.Use = new OccupiedLandUse());
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
