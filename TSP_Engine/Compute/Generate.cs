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
        public static Development Generate(Unit prototypeUnit, PlanParameters parameters, FacilitiesParameters facilitiesParameters, ILayout layout, FacilitiesBlock FacilitiesBlock = null )
        {
            SiteLandUse siteLandUse = Query.FindSiteUse( parameters.LandUses);
            if(siteLandUse == null)
            {
                //Base.Compute.RecordError("No site land use was found. A site land use is required.");
                return new Development();
            }

            Field field = Create.IField(layout, siteLandUse , prototypeUnit);

            object facilitiesLand = parameters.LandUses.Find(x => x is FacilitiesLandUse);
            //Add facilities use
            if(facilitiesLand == null && FacilitiesBlock == null)
            {
                facilitiesLand = Create.IFacilitiesLandUse(layout, facilitiesParameters, siteLandUse as SiteLandUse);
                parameters.LandUses.Add(facilitiesLand);
            }
            else
            {
                //replace with block
                parameters.LandUses.Remove(facilitiesLand);
                FacilitiesLandUse facilities = facilitiesLand as FacilitiesLandUse;
                facilities.Boundary = FacilitiesBlock.Boundary.Offset(facilitiesParameters.BaseOffset, Vector.ZAxis);
                parameters.LandUses.Add(facilities);
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
