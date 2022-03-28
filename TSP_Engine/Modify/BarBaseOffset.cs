using BH.Engine.Base;
using BH.oM.Geometry;
using BH.oM.TSP;
using BH.Engine.Geometry;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BH.Engine.TSP
{
    public static partial class Modify
    {
        public static Development BarBaseOffset(this Development development, double offset)
        {
            Development developmentClone = development.ShallowClone();
            for (int i = 0; i < development.Bars.Count; i++)
                development.Bars[i] = development.Bars[i].BarBaseOffset(development.Field, offset);
            return development;
        }
        public static Bar BarBaseOffset(this Bar bar, Field field,  double offset)
        {
            Bar barClone = bar.ShallowClone();
            List<Point> pts = new List<Point>();
            foreach(Guid guid in bar.Cells)
            {
                Cell cell = field.Cells.Find(x => x.BHoM_Guid.Equals(guid));
                pts.AddRange(cell.Boundary.ControlPoints);
            }
            pts = pts.CullDuplicates();
            Point centre = pts.Average();
            for(int i = 0;i< pts.Count;i++)
                pts[i] = pts[i].Translate(new Vector() { X = -centre.X, Y = -centre.Y });
            pts = pts.OrderBy(x => Math.Atan2(x.X, x.Y)).ToList();

            List<Point> ptsCorner = new List<Point>();
            for (int i = 0; i < pts.Count; i++)
            {
                Vector prev = new Vector();
                Vector next = new Vector();
                if (i == 0)
                    prev = pts.Last() - pts.First();
                else
                    prev = pts[i - 1] - pts[i];

                if (i == pts.Count - 1)
                    next = pts.First() - pts.Last();
                else
                    next = pts[i + 1] - pts[i];

                double a = Math.Round(prev.Angle(next),3);
                if (a <= 1.571)
                    ptsCorner.Add(pts[i]);

            }
            ptsCorner.Add(ptsCorner[0]);
            Polyline pl = BH.Engine.Geometry.Create.Polyline(ptsCorner);
            pl = pl.Translate(new Vector() { X = centre.X, Y = centre.Y });
            List<Line> segs = new List<Line>();
            //check for segment on boundary and ignore
            foreach(Line line in pl.SubParts())
            {
                Point s = field.Boundary.ClosestPoint(line.Start);
                Point e = field.Boundary.ClosestPoint(line.End);
                double d1 = Math.Round(s.SquareDistance(line.Start),3);
                double d2 = Math.Round(e.SquareDistance(line.End),3);
                if (d1 + d2 > 0)
                    segs.Add(line);
                
            }
            List<Polyline> join = segs.Join();
            Polyline off = join[0].Offset(-offset, Vector.ZAxis);
            List<Point> outer = off.ControlPoints;
            outer.Reverse();
            outer.AddRange(join[0].ControlPoints);
            outer.Add(outer[0]);
            if(segs.Count == 4)
            {
                //leave 1mm gap 
                Vector shiftA = outer[1] - outer[0];
                shiftA.Normalise();
                outer[0] = outer[0] + shiftA * 0.001;
                outer[9] = outer[9] + shiftA * 0.001;
                outer[10] = outer[10] + shiftA * 0.001;
            }
            
            barClone.ExternalCirculation = BH.Engine.Geometry.Create.Polyline(outer);
            return barClone;
        }
    }
}
