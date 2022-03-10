using BH.oM.Geometry;
using BH.oM.Graphics;
using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using BH.Engine.Graphics;

namespace BH.Engine.TSP
{
    public static partial class Create
    {
        public static List<RenderMesh> Preview(Result result,  PreviewColourMap colourMap = null, Gradient solarAnalysisGradient = null, double minimum = 0, double maximum = 1)
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
                    gradient = Gradient(new List<Color>() { Color.FromArgb(77, 77, 255), Color.FromArgb(150, 255, 238)});
                }
                else
                    gradient = solarAnalysisGradient;
              
               foreach(SolarResult solarResult in result.SolarResults)
                    renderMeshes.Add(Convert.ToRenderMesh(solarResult.Unit.UnitMesh(), gradient.Color(solarResult.SolarAccess,minimum,maximum)));
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
            //foreach(Cell cell in result.Field.Cells)
            //{
            //    string useName = cell.Use.GetType().Name;
            //    if (m_UseIgnore.Contains(useName))
            //        continue;

            //    if(colourMap.ContainsKey(useName))
            //        renderMeshes.Add(Convert.ToRenderMesh(cell, colourMap[useName]));
            //    else
            //        renderMeshes.Add(Convert.ToRenderMesh(cell, Color.LightGray));
            //}

            renderMeshes.Add(Convert.ToRenderMesh(result.Development.CommunalBlock, colourMap.Colour("Communal")));
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
