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
        public static List<RenderMesh> Preview(Result result, Gradient analysisGradient = null, double minimum = 0, double maximum = 1, List<ILandUse> ignore = null, Dictionary<ILandUse, Color> colourMap = null)
        {
            List<RenderMesh> renderMeshes = new List<RenderMesh>();
            SetColours();
            if (m_LandUseColourMap.Count == 0)
                SetColours();

            if(colourMap != null)
            {
                //update new colours
                foreach(var colourPair in colourMap)
                {
                    string useName = colourPair.Key.GetType().Name;
                    if (m_LandUseColourMap.ContainsKey(useName))
                        m_LandUseColourMap[useName] = colourPair.Value;

                }
            }
            if (ignore.Count > 0)
            {
                m_UseIgnore = new List<string>();
                foreach (ILandUse landUse in ignore)
                    m_UseIgnore.Add(landUse.GetType().Name);
            }
            else
                SetIgnore();
            //colour units
            if (result.SolarResults.Count > 0)
            {
                Gradient gradient = new Gradient();
                if (analysisGradient == null)
                {
                    gradient = Gradient(new List<Color>() { Color.FromArgb(77, 77, 255), Color.FromArgb(150, 255, 238)});
                }
                else
                    gradient = analysisGradient;
              
               foreach(SolarResult solarResult in result.SolarResults)
                    renderMeshes.Add(Convert.ToRenderMesh(solarResult.Unit.UnitMesh(), gradient.Color(solarResult.SolarAccess,minimum,maximum)));
            }
            else
            {
                foreach(Bar bar in result.Bars)
                {
                    //ground floor
                    foreach(Unit unit in bar.Units.FindAll(x => x.CoordinateSystem.Origin.Z == 0))
                    {
                        Mesh m = Create.UnitMesh(unit);
                        renderMeshes.Add(Convert.ToRenderMesh(m, m_LandUseColourMap["commercial"]));
                    }
                    //ground floor
                    foreach (Unit unit in bar.Units.FindAll(x => x.CoordinateSystem.Origin.Z > 0))
                    {
                        Mesh m = Create.UnitMesh(unit);
                        renderMeshes.Add(Convert.ToRenderMesh(m, m_LandUseColourMap["apartment"]));
                    }
                }
                
            }
            //foreach(Cell cell in result.Field.Cells)
            //{
            //    string useName = cell.Use.GetType().Name;
            //    if (m_UseIgnore.Contains(useName))
            //        continue;

            //    if(m_LandUseColourMap.ContainsKey(useName))
            //        renderMeshes.Add(Convert.ToRenderMesh(cell, m_LandUseColourMap[useName]));
            //    else
            //        renderMeshes.Add(Convert.ToRenderMesh(cell, Color.LightGray));
            //}

            renderMeshes.Add(Convert.ToRenderMesh(result.CommunalBlock, m_LandUseColourMap[typeof(CommunalLandUse).Name.ToString()]));
            return renderMeshes;
        }

        private static void SetColours()
        {
            m_LandUseColourMap = new Dictionary<string, Color>();
            m_LandUseColourMap.Add(typeof(OccupiedLandUse).Name, Color.FromArgb(64,64,64,64));
            m_LandUseColourMap.Add(typeof(OpenLandUse).Name, Color.FromArgb(117,234,135));
            m_LandUseColourMap.Add(typeof(UnoccupiedLandUse).Name, Color.FromArgb(0, 0, 0));
            m_LandUseColourMap.Add(typeof(OutsideSiteLandUse).Name, Color.FromArgb(0, 0, 0));
            m_LandUseColourMap.Add(typeof(ParkingLandUse).Name, Color.FromArgb(120, 64, 64, 250));
            m_LandUseColourMap.Add(typeof(SiteLandUse).Name, Color.FromArgb(0, 0, 0));
            m_LandUseColourMap.Add(typeof(RoadLandUse).Name, Color.FromArgb(64, 64, 64));
            m_LandUseColourMap.Add(typeof(CommunalLandUse).Name, Color.FromArgb(64, 64, 64, 250));
            m_LandUseColourMap.Add("apartment", Color.FromArgb(64, 64, 64, 64));
            m_LandUseColourMap.Add("commercial", Color.FromArgb(64, 255, 53, 18));
        }

        private static void SetIgnore()
        {
            m_UseIgnore.Add(typeof(OutsideSiteLandUse).Name);
            m_UseIgnore.Add(typeof(OccupiedLandUse).Name);
            m_UseIgnore.Add(typeof(SiteLandUse).Name);
            m_UseIgnore.Add(typeof(UnoccupiedLandUse).Name);
            m_UseIgnore.Add(typeof(RoadLandUse).Name);
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

        private static Dictionary<string, Color> m_LandUseColourMap = new Dictionary<string, Color>();
        private static List<string> m_UseIgnore = new List<string>();
    }
}
