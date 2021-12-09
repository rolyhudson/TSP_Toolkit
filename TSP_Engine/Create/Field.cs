using BH.Engine.Base;
using BH.Engine.Geometry;
using BH.oM.Geometry;
using BH.oM.Geometry.CoordinateSystem;
using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.TSP
{
    public static partial class Create
    {
        public static Field Field(Vector principleDirection, Polyline siteBoundary, Unit prototypeUnit)
        {
            
            Polyline alignedBoundary = Query.AlignedBoundingBox(principleDirection, siteBoundary);
            Cartesian origin = Query.GridCartesian(principleDirection, siteBoundary);
            int unitsX = (int)Math.Ceiling(alignedBoundary.ControlPoints[0].Distance(alignedBoundary.ControlPoints[3]) / prototypeUnit.X);
            int unitsY = (int)Math.Ceiling(alignedBoundary.ControlPoints[0].Distance(alignedBoundary.ControlPoints[1]) / prototypeUnit.Y);

            List<Point> points = new List<Point>()
            {
                new Point(),
                Geometry.Create.Point(prototypeUnit.X,0,0),
                Geometry.Create.Point(prototypeUnit.X,prototypeUnit.Y,0),
                Geometry.Create.Point(0,prototypeUnit.Y,0),
                new Point(),
            };

            Polyline boundary = Geometry.Create.Polyline(points);
            TransformMatrix transform = Geometry.Create.OrientationMatrixGlobalToLocal(origin);
            boundary = boundary.Transform(transform);

            Footprint basePrint = new Footprint()
            {
                Boundary = boundary,
                CoordinateSystem = origin
            };
            Field field = new Field();
            
            for(int i = 0;i <= unitsX;i++)
            {
                for(int j = 0;j< unitsY;j++)
                {
                    Footprint copy = basePrint.ShallowClone();
                    Cartesian cartesian = origin.ShallowClone();
                    Vector v = origin.X * i * prototypeUnit.X + origin.Y * j* prototypeUnit.Y;
                    copy.Boundary = copy.Boundary.Translate(v);
                    copy.CoordinateSystem = cartesian.Translate(v);
                    copy.Centre = Geometry.Query.Average(copy.Boundary.ControlPoints);
                    copy.BHoM_Guid = Guid.NewGuid();
                    field.Footprints.Add(copy);
                }
            }
            //check for site containment
            Parallel.ForEach(field.Footprints, f =>
            {
                f.Neighbourhoods(field);
            });
            foreach (Footprint f in field.Footprints)
            {
                if (!siteBoundary.IIsContaining(f.Boundary))
                    f.Use = Use.OutsideBoundary;
                
            }

            //we could check for circulation + open space
            return field;
        }
    }
}
