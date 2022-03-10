using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace BH.Engine.TSP
{
    public static partial class Query
    {
        public static Color Colour(this PreviewColourMap previewColourMap, string useName)
        {
            if (previewColourMap.Map.ContainsKey(useName))
                return previewColourMap.Map[useName];
            else
                return Color.FromArgb(64, 64, 64, 64);
        }
    }
}
