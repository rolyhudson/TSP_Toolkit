using BH.oM.Base;
using BH.oM.Base.Attributes;
using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BH.Engine.TSP
{
    public static partial class Compute
    {
        [MultiOutput(0, "bars", "Linear blocks.")]
        [MultiOutput(1, "field", "Field.")]
        public static Output<List<Bar>, Field> Generate(Unit prototypeUnit, PlanParameters parameters)
        {
            ILandUse siteLandUse = parameters.LandUses.Find(x => x is SiteLandUse);
            if(siteLandUse == null)
            {
                Base.Compute.RecordError("No site land use was found. A site land use is required.");
                return new Output<List<Bar>, Field>();
            }
                
            Field field = Create.IField(parameters.LayoutMethod, siteLandUse as SiteLandUse, prototypeUnit);

            foreach (var landUseGroup in parameters.LandUses.GroupBy(x => x.GetType()))
            {
                //TODO may need ordering by use priority
                if (landUseGroup.Key.Name == "SiteLandUse")
                    continue;
                else
                {
                    foreach(var landUse in landUseGroup)
                        field.ILandUse(landUse);
                }
                
            }

            Output<List<Bar>, Field> results = Create.IBars(field, parameters);
            return new Output<List<Bar>, Field>
            {
                Item1 = results.Item1,
                Item2 = results.Item2,
            };
        }
    }
}
