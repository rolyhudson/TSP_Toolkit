using BH.oM.Base.Attributes;
using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using BH.oM.Geometry;
using BH.Engine.Base;

namespace BH.Engine.TSP
{
    public static partial class Compute
    {
        
        public static Result Solution(Parameters parameters, bool runSolarAnalysis)
        {
            var option = Generate(parameters.PrototypeUnit, parameters.PlanParameters, parameters.LayoutMethod as ILayout);
            option.Item2 = Modify.ILevels(option.Item2, parameters.PrototypeUnit, parameters.VerticalParameters);
            option.Item1 = Massing(option.Item1, option.Item2, parameters.PrototypeUnit);
            List<SolarResult> solarResults = new List<SolarResult>();
            SolarAccessParameters copySolarParameters = parameters.SolarAccessParameters.DeepClone();
            if (runSolarAnalysis)
            {
                //add the units generated to obstructions
                List<Mesh> units = Create.UnitMesh(option.Item1);
                copySolarParameters.Obstructions.AddRange(units);

                solarResults = SolarAnalysis(option.Item1, copySolarParameters.SunPoints, copySolarParameters.Obstructions);
            }
            return new Result(parameters.BHoM_Guid,"",0,option.Item2,option.Item1,solarResults);
        }
    }
}
