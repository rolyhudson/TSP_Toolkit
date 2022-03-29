using BH.Engine.Base;
using BH.Engine.Geometry;
using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Engine.TSP
{
    public static partial class Modify
    {
        public static Development AddLandUseBoundaries(this Development development, List<object> landuses)
        {
            Development clone = development.ShallowClone();
            foreach(ILandUse landUse in landuses)
            {
                if(landUse is OpenLandUse)
                {
                    OpenLandUse openLandUse = landUse as OpenLandUse;
                    if (openLandUse.Boundary.IsClosed() && openLandUse.Boundary.IsPlanar())
                        clone.OpenSpace.Add(openLandUse.Boundary);
                  
                }
                if (landUse is RoadLandUse)
                {
                    RoadLandUse roadLandUse = landUse as RoadLandUse;
                    if (roadLandUse.Boundary.IsClosed() && roadLandUse.Boundary.IsPlanar())
                        clone.Roads.Add(roadLandUse.Boundary);

                }
            }
            return clone;
        }
    }
}
