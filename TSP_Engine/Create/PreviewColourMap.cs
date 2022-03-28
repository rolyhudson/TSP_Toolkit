using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace BH.Engine.TSP
{
    public static partial class Create
    {
        public static PreviewColourMap PreviewColourMap (List<Use> uses = null , List<Color> colours = null)
        {
            if(uses == null && colours == null || uses.Count == 0 && colours.Count == 0)
            {
                colours = DefaultColours();
                uses = Enum.GetValues(typeof(Use)).Cast<Use>().ToList();
            }

            if (uses.Count != colours.Count)
                return new PreviewColourMap();

            PreviewColourMap map = new PreviewColourMap();
            int i = 0;
            foreach (Use use in uses)
            {
                map.Map[use.ToString()] = colours[i];
                i++;
            }
                
            return map;
        }

        private static List<Color> DefaultColours()
        {

            List<Color> colors = new List<Color>()
            {
                 Color.FromArgb(196, 55, 150, 188),
                 Color.FromArgb(167, 0, 255, 21),
                 Color.FromArgb(125, 125, 125, 125),
                 Color.FromArgb(190,255, 152, 206),
                 Color.FromArgb(165, 165, 165, 165),
                 Color.FromArgb(167, 152, 251, 152)
            };
            return colors;
        }
    }
}
