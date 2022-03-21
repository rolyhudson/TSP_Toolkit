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
        
        public static Result Solution(Parameters parameters, bool runSolarAnalysis, bool run, int maxIterations = 10)
        {
            int runs = 0;
            Development option = new Development();
            option.CommunalBlock = null;
            Parameters parametersClone = parameters.DeepClone();
            int bestScore = 0;
            Development development = new Development();
            while (runs < maxIterations)
            {
                option = Generate(parameters.PrototypeUnit, parametersClone.PlanParameters, parametersClone.CommunalParameters, parametersClone.LayoutMethod as ILayout, option.CommunalBlock);
                option.Field = Modify.ILevels(option.Field, parametersClone.PrototypeUnit, parametersClone.VerticalParameters);
                option.Bars = Massing(option.Bars, option.Field, parametersClone.PrototypeUnit);
                CommunalLandUse communalLand = (CommunalLandUse)parametersClone.PlanParameters.LandUses.Find(x => x is CommunalLandUse);
                option.CommunalBlock = Create.CommunalBlock(option.Field, option.Bars, parametersClone.PrototypeUnit, parametersClone.CommunalParameters, communalLand);
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
            

            List<SolarResult> solarResults = new List<SolarResult>();
            SolarAccessParameters copySolarParameters = parameters.SolarAccessParameters.DeepClone();
            if (runSolarAnalysis)
            {
                //add the units generated to obstructions
                List<Mesh> units = Create.UnitMesh(development.Bars);
                copySolarParameters.Obstructions.AddRange(units);
                copySolarParameters.Obstructions.Add(development.CommunalBlock.UnitMesh());

                solarResults = SolarAnalysis(development.Bars, copySolarParameters.SunPoints, copySolarParameters.Obstructions);
            }
            
            return new Result(parameters.BHoM_Guid, "", 0, development, solarResults);
        }

        //private static void CheckPlan(CommunalBlock communalBlock, List<Bar> bars, Field  Unit unit)
        //{
        //    double barArea = bars.Select(x => x.)
        //}
    }
}
