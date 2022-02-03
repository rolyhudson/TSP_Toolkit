using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Engine.TSP
{
    public static partial class Query
    {
        public static SiteLandUse FindSiteUse(List<object> landUses)
        {
            foreach (object use in landUses)
            {
                if (use is SiteLandUse)
                    return use as SiteLandUse;
            }
            return null;
        }
    }
}
