using BH.oM.Base.Attributes;
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
        
        public static Result Solution(Parameters parameters, bool runSolarAnalysis, int iterations = 1)
        {
            int runs = 0;
            Output<List<Bar>, Field> option = new Output<List<Bar>, Field>();
            CommunalBlock communalBlock = null;
            Parameters parametersClone = parameters.DeepClone();
            while (runs < iterations)
            {
                option = Generate(parameters.PrototypeUnit, parametersClone.PlanParameters, parametersClone.CommunalParameters, parametersClone.LayoutMethod as ILayout, communalBlock);
                option.Item2 = Modify.ILevels(option.Item2, parametersClone.PrototypeUnit, parametersClone.VerticalParameters);
                option.Item1 = Massing(option.Item1, option.Item2, parametersClone.PrototypeUnit);
                CommunalLandUse communalLand = (CommunalLandUse)parametersClone.PlanParameters.LandUses.Find(x => x is CommunalLandUse);
                communalBlock = Create.CommunalBlock(option.Item2, option.Item1, parametersClone.PrototypeUnit, parametersClone.CommunalParameters, communalLand);
                runs++;
            }
            

            List<SolarResult> solarResults = new List<SolarResult>();
            SolarAccessParameters copySolarParameters = parameters.SolarAccessParameters.DeepClone();
            if (runSolarAnalysis)
            {
                //add the units generated to obstructions
                List<Mesh> units = Create.UnitMesh(option.Item1);
                copySolarParameters.Obstructions.AddRange(units);
                copySolarParameters.Obstructions.Add(communalBlock.UniteMesh());

                solarResults = SolarAnalysis(option.Item1, copySolarParameters.SunPoints, copySolarParameters.Obstructions);
            }
            
            return new Result(parameters.BHoM_Guid, "", 0, option.Item2, option.Item1, communalBlock, solarResults);
        }
    }
}
