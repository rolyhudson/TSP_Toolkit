﻿using BH.oM.Base;
using BH.oM.Reflection;
using BH.oM.Reflection.Attributes;
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
        public static Output<List<Bar>, Field> Generate(Unit prototypeUnit, PlanParameters parameters, ILayout layout)
        {
            ILandUse siteLandUse = Query.FindSiteUse( parameters.LandUses);
            if(siteLandUse == null)
            {
                Reflection.Compute.RecordError("No site land use was found. A site land use is required.");
                return new Output<List<Bar>, Field>();
            }
                
            Field field = Create.IField(layout, siteLandUse as SiteLandUse, prototypeUnit);

            foreach (var landUseGroup in parameters.LandUses.GroupBy(x => x.GetType()))
            {
                //TODO may need ordering by use priority
                if (landUseGroup.Key.Name == "SiteLandUse")
                    continue;
                else
                {
                    foreach(var landUse in landUseGroup)
                        Modify.ILandUse(field, landUse as dynamic);
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
