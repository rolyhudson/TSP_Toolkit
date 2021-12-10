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
        public static Output<List<Bar>, Field> Generate(Unit prototypeUnit, PlanSettings settings)
        {
            Field field = Create.Field(settings.PrincipleDirection, settings.SiteBoundary, prototypeUnit);

            //modify for open spaces
            if (settings.OpenSpaces.Count > 0)
                field = field.OpenSpace(settings.OpenSpaces);
            //modify for circulation
            if (settings.CirculationRoutes.Count > 0)
                field = field.Circulation(settings.CirculationRoutes);

            Output<List<Bar>, Field> results = Create.Bars(field, settings, settings.MaximumPlacementAttempts);
            return new Output<List<Bar>, Field>
            {
                Item1 = results.Item1,
                Item2 = results.Item2,
            };
        }
    }
}
