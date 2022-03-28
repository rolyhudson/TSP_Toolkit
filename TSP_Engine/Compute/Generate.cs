using BH.Engine.Geometry;
using BH.oM.Base;
using BH.oM.Geometry;
using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BH.Engine.TSP
{
    public static partial class Compute
    {
        public static Development Generate(Unit prototypeUnit, PlanParameters parameters, FacilitiesParameters communalParameters, ILayout layout, FacilitiesBlock communalBlock = null )
        {
            SiteLandUse siteLandUse = Query.FindSiteUse( parameters.LandUses);
            if(siteLandUse == null)
            {
                //Base.Compute.RecordError("No site land use was found. A site land use is required.");
                return new Development();
            }

            Field field = Create.IField(layout, siteLandUse , prototypeUnit);

            object communalLand = parameters.LandUses.Find(x => x is FacilitiesLandUse);
            //Add communal use
            if(communalLand == null && communalBlock == null)
            {
                communalLand = Create.IFacilitiesLandUse(layout, communalParameters, siteLandUse as SiteLandUse);
                parameters.LandUses.Add(communalLand);
            }
            else
            {
                //replace with block
                parameters.LandUses.Remove(communalLand);
                FacilitiesLandUse communal = communalLand as FacilitiesLandUse;
                communal.Boundary = communalBlock.Boundary.Offset(communalParameters.BaseOffset, Vector.ZAxis);
                parameters.LandUses.Add(communal);
            }
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

            Development results = Create.IBars(field, parameters);
            return results;
        }
    }
}
