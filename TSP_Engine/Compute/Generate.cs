using BH.oM.Geometry;
using BH.oM.Reflection;
using BH.oM.Reflection.Attributes;
using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Engine.TSP
{
    public static partial class Compute
    {
        [MultiOutput(0, "bars", "Linear blocks.")]
        [MultiOutput(1, "field", "Field.")]
        public static Output<List<Bar>, Field> Generate(Vector principleDirection, Polyline siteBoundary, Unit prototypeUnit, Settings settings)
        {
            Field field = Create.Field(principleDirection, siteBoundary, prototypeUnit);
            Output<List<Bar>, Field> results = Create.Bars(field, settings, settings.MaximumPlacementAttempts);
            return new Output<List<Bar>, Field>
            {
                Item1 = results.Item1,
                Item2 = results.Item2,
            };
        }
    }
}
