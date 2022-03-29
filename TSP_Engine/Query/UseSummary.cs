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
            summary.PlotArea = Math.Round(siteland.Boundary.Area());
            double offsetArea = summary.PlotArea - development.Field.Boundary.Area();
            summary.NetPlotArea = Math.Round(summary.PlotArea - offsetArea);

            double blocksCirculation = 0;
            development.Bars.ForEach(x => blocksCirculation += x.ExternalCirculation.Area());
            double unitArea = parameters.PrototypeUnit.X * parameters.PrototypeUnit.Y;
            double blocksFootprint = development.Bars.NumberOfGroundFloorUnits() * unitArea;

            summary.UsedArea = Math.Round(blocksFootprint + blocksCirculation + development.FacilitiesBlock.Boundary.Area());
            summary.Occupation = Math.Round(summary.UsedArea / summary.NetPlotArea *100);

            summary.InternalCirculation = Math.Round(development.Bars.NumberOfUnits() * parameters.PrototypeUnit.CirculationArea);

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

            summary.ExternalCirculation = Math.Round(openSpaceArea + blocksCirculation);
            summary.TotalCirculation = Math.Round(summary.ExternalCirculation + summary.InternalCirculation);

            summary.HousingUnitsNumber = development.Bars.NumberOfApartments(parameters.PrototypeUnit);
            summary.HousingArea = Math.Round(summary.HousingUnitsNumber * parameters.PrototypeUnit.ApartmentArea);

            summary.ParkingSpaces = development.FacilitiesBlock.ParkingSpaces;
            development.FacilitiesBlock.Parking.ForEach(x => summary.ParkingArea += x.Area());
            summary.ParkingArea = Math.Round(summary.ParkingArea);
            
            development.FacilitiesBlock.Commercial.ForEach(x => summary.ExternalCommercialArea += x.Area());
            summary.ExternalCommercialArea = Math.Round(summary.ExternalCommercialArea);

            development.FacilitiesBlock.Communal.ForEach(x => summary.CommunalArea += x.Area());
            summary.CommunalArea = Math.Round(summary.CommunalArea);

            summary.InternalCommercialArea = Math.Round(development.Bars.NumberOfGroundFloorUnits() * unitArea);
            
            summary.TotalCommercialArea = Math.Round(summary.InternalCommercialArea + summary.ExternalCommercialArea);

            summary.GreenArea = Math.Round(summary.NetPlotArea - summary.UsedArea);
            summary.SellableArea = Math.Round(summary.HousingArea + summary.InternalCommercialArea + summary.ExternalCommercialArea);

            summary.EstimatedConstructionCost = Math.Round(summary.HousingArea * parameters.CostParameters.HousingMetreSquared +
                                                summary.ParkingArea * parameters.CostParameters.ParkingMetreSquared +
                                                summary.TotalCommercialArea * parameters.CostParameters.CommercialMetreSquared +
                                                summary.GreenArea * parameters.CostParameters.GreenAreaMetreSquared +
                                                summary.InternalCirculation * parameters.CostParameters.InternalCirculationMetreSquared +
                                                summary.ExternalCirculation * parameters.CostParameters.ExternalCirculationMetreSquared);

            return summary;
        }
    }
}
