using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using BH.oM.Geometry;
using BH.Engine.Base;
using BH.oM.Base;

namespace BH.Engine.TSP
{
    public static partial class Compute
    {
        
        public static Result Solution(Parameters parameters, bool run, int maxIterations = 10)
        {
            if (!run)
                return null;
            int runs = 0;
            Development option = new Development();
            option.FacilitiesBlock = null;
            Parameters parametersClone = parameters.DeepClone();
            int bestScore = 0;
            Development development = new Development();
            while (runs < maxIterations)
            {
                option = Generate(parameters.PrototypeUnit, parametersClone.PlanParameters, parametersClone.FacilitiesParameters, parametersClone.LayoutMethod as ILayout, option.FacilitiesBlock);
                option.Field = Modify.ILevels(option.Field, parametersClone.PrototypeUnit, parametersClone.VerticalParameters);
                option.Bars = Massing(option.Bars, option.Field, parametersClone.PrototypeUnit);
                FacilitiesLandUse communalLand = (FacilitiesLandUse)parametersClone.PlanParameters.LandUses.Find(x => x is FacilitiesLandUse);
                option.FacilitiesBlock = Create.FacilitiesBlock(option.Field, option.Bars, parametersClone.PrototypeUnit, parametersClone.FacilitiesParameters, communalLand);
                if(option.IsValid())
                {
                    int score = option.Bars.SelectMany(x => x.Units).Count();
                    if (score > bestScore)
                    {
                        development = option;
                        bestScore = score;
                    }
                    runs++;
                }
                
            }
            UseSummary useSummary = Query.UseSummary(development, parameters);

            List<SolarResult> solarResults = new List<SolarResult>();
            
            if (parameters.SolarAccessParameters.Run)
            {
                SolarAccessParameters copySolarParameters = parameters.SolarAccessParameters.DeepClone();
                //add the units generated to obstructions
                List<Mesh> units = Create.UnitMesh(development.Bars);
                copySolarParameters.Obstructions.AddRange(units);
                copySolarParameters.Obstructions.Add(development.FacilitiesBlock.UnitMesh());

                solarResults = SolarAnalysis(development.Bars, copySolarParameters.SunPoints, copySolarParameters.Obstructions);
            }
            
            return new Result(parameters.BHoM_Guid, "", 0, development, solarResults, useSummary);
        }

    }
}
