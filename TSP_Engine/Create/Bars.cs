using BH.Engine.Base;
using BH.Engine.Geometry;
using BH.oM.Base;

using BH.oM.Geometry;
using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Engine.TSP
{
    public static partial class Create
    {
        public static Development IBars(Field field, PlanParameters parameters)
        {
            return Bars(field.Layout as dynamic, field, parameters); 
        }

        public static Development Bars(ILayout layout, Field field, PlanParameters parameters)
        {
            return null;
        }

        public static Development Bars(HybridLayout layout, Field field, PlanParameters parameters)
        {
            Field pField= new Field();
            pField.Boundary = field.Boundary;
            pField.Cells = field.Cells.FindAll(x => x.Tags.Contains("perimeter"));
            
            Development pBars = Bars(layout.PerimeterLayout, pField, parameters);

            Field bField = new Field();
            bField.Cells = field.Cells.FindAll(x => x.Tags.Contains("internal"));
            
            Development bBars = Bars(layout.BarsLayout, bField, parameters);

            Development combined = pBars;
            combined.Bars.AddRange(bBars.Bars);
            combined.Field.Cells.AddRange(bBars.Field.Cells);
            combined.Field.Layout = layout;

            return combined;

        }
        public static Development Bars(BarsLayout layout, Field field, PlanParameters parameters)
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
            return new Development
            {
                Bars = bars,
                Field = fieldCopy,
            };
        }
        public static Development Bars(PerimeterLayout layout, Field field, PlanParameters parameters)
        {
            SiteLandUse siteLandUse = Query.FindSiteUse(parameters.LandUses);
            if (siteLandUse == null)
            {
                //Base.Compute.RecordError("No site land use was found. A site land use is required.");
                return new Development();
            }
            
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

            return new Development
            {
                Bars = bars,
                Field = fieldCopy,
            };
        }
    }
}
