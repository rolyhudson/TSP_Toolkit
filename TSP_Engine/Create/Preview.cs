using BH.oM.Geometry;
using BH.oM.Graphics;
using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using BH.Engine.Graphics;
using BH.Engine.Geometry;

namespace BH.Engine.TSP
{
    public static partial class Create
    {
        public static List<RenderMesh> Preview(Result result,  PreviewColourMap colourMap = null, Gradient solarAnalysisGradient = null, double minimum = 0, double maximum = 12)
        {
            List<RenderMesh> renderMeshes = new List<RenderMesh>();
            if (colourMap == null)
                colourMap = Create.PreviewColourMap();

            //colour units
            if (result.SolarResults.Count > 0)
            {
                Gradient gradient = new Gradient();
                if (solarAnalysisGradient == null)
                {
                    gradient = Gradient(new List<Color>() { Color.FromArgb(0, 150, 0), Color.FromArgb(255, 255, 0)});
                }
                else
                    gradient = solarAnalysisGradient;
              
               foreach(SolarResult solarResult in result.SolarResults)
                    renderMeshes.Add(Convert.ToRenderMesh(solarResult.Mesh, gradient.Color(solarResult.SolarAccess,minimum,maximum)));
            }
            else
            {
                foreach(Bar bar in result.Development.Bars)
                {
                    //ground floor
                    foreach(Unit unit in bar.Units.FindAll(x => x.CoordinateSystem.Origin.Z == 0))
                    {
                        Mesh m = Create.UnitMesh(unit);
                        renderMeshes.Add(Convert.ToRenderMesh(m, colourMap.Colour("Commercial")));
                    }
                    //ground floor
                    foreach (Unit unit in bar.Units.FindAll(x => x.CoordinateSystem.Origin.Z > 0))
                    {
                        Mesh m = Create.UnitMesh(unit);
                        renderMeshes.Add(Convert.ToRenderMesh(m, colourMap.Colour("Apartment")));
                    }
                }
                
            }
            result.Development.Bars.ForEach(x => renderMeshes.Add(Convert.ToRenderMesh(x.ExternalCirculation, colourMap.Colour("Circulation"))));
            result.Development.OpenSpace.ForEach(x => renderMeshes.Add(Convert.ToRenderMesh(x, colourMap.Colour("Open"))));
            result.Development.Roads.ForEach(x => renderMeshes.Add(Convert.ToRenderMesh(x, colourMap.Colour("Circulation"))));
            renderMeshes.Add(Convert.ToRenderMesh(result.Development.Boundary.Translate(Vector.ZAxis*-0.1), colourMap.Colour("Site")));
            renderMeshes.Add(Convert.ToRenderMesh(result.Development.FacilitiesBlock.Boundary.Translate(Vector.ZAxis * -0.05), colourMap.Colour("Circulation")));
            renderMeshes.AddRange(Convert.ToRenderMesh(result.Development.FacilitiesBlock, colourMap));
            return renderMeshes;
        }

        
        private static Gradient Gradient(List<Color> colors)
        {
            List<decimal> decimals = new List<decimal>();
            
            for (int c = 0; c < colors.Count; c++)
            {
                decimals.Add(System.Convert.ToDecimal((double)c / (double)(colors.Count-1)));
                
            }
            return Graphics.Create.Gradient(colors, decimals);
        }

    }
}
