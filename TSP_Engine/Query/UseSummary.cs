using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using BH.Engine.Geometry;

namespace BH.Engine.TSP
{
    public static partial class Query
    {
        public static UseSummary UseSummary(Development development, Parameters parameters)
        {
            UseSummary summary = new UseSummary();

            SiteLandUse siteland = (SiteLandUse)parameters.PlanParameters.LandUses.Find(x => x is SiteLandUse);
            summary.PlotArea = siteland.Boundary.Area();
            double offsetArea = summary.PlotArea - development.Field.Boundary.Area();
            summary.NetPlotArea = summary.PlotArea - offsetArea;

            double blocksCirculation = 0;
            development.Bars.ForEach(x => blocksCirculation += x.ExternalCirculation.Area());
            double unitArea = parameters.PrototypeUnit.X * parameters.PrototypeUnit.Y;
            double blocksFootprint = development.Bars.NumberOfGroundFloorUnits() * unitArea;

            summary.UsedArea = blocksFootprint + blocksCirculation + development.FacilitiesBlock.Boundary.Area();
            summary.Occupation = summary.UsedArea / summary.NetPlotArea *100;

            summary.InternalCirculation = development.Bars.NumberOfUnits() * parameters.PrototypeUnit.CirculationArea;

            double openSpaceArea = 0;
            foreach(ILandUse landUse in parameters.PlanParameters.LandUses)
            {
                if(landUse is OpenLandUse)
                {
                    OpenLandUse openLandUse = landUse as OpenLandUse;
                    if(openLandUse.Boundary!=null)
                        openSpaceArea += openLandUse.Boundary.Area();
                }
            }

            summary.ExternalCirculation = openSpaceArea + blocksCirculation;
            summary.TotalCirculation = summary.ExternalCirculation + summary.InternalCirculation;

            summary.HousingUnitsNumber = development.Bars.NumberOfApartments(parameters.PrototypeUnit);
            summary.HousingArea = summary.HousingUnitsNumber * parameters.PrototypeUnit.ApartmentArea;

            
            summary.ParkingSpaces = development.FacilitiesBlock.ParkingSpaces;

            summary.InternalCommercialArea = development.Bars.NumberOfGroundFloorUnits() * unitArea;
            development.FacilitiesBlock.Commercial.ForEach(x => summary.ExternalCirculation += x.Area());
            development.FacilitiesBlock.Communal.ForEach(x => summary.CommunalArea += x.Area());

            summary.TotalCommercialArea = summary.InternalCommercialArea + summary.ExternalCommercialArea;

            summary.GreenArea = summary.NetPlotArea - summary.UsedArea;
            summary.SellableArea = summary.HousingArea + summary.InternalCommercialArea + summary.ExternalCommercialArea;

            summary.EstimatedConstructionCost = summary.HousingArea * parameters.CostParameters.HousingMetreSquared +
                                                summary.ParkingArea * parameters.CostParameters.ParkingMetreSquared +
                                                summary.TotalCommercialArea * parameters.CostParameters.CommercialMetreSquared +
                                                summary.GreenArea * parameters.CostParameters.GreenAreaMetreSquared +
                                                summary.InternalCirculation * parameters.CostParameters.InternalCirculationMetreSquared +
                                                summary.ExternalCirculation * parameters.CostParameters.ExternalCirculationMetreSquared;

            return summary;
        }
    }
}
