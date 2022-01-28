using BH.Engine.Base;
using BH.Engine.Geometry;
using BH.oM.Geometry;
using BH.oM.Geometry.CoordinateSystem;
using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Engine.TSP
{
    public static partial class Compute
    {
        public static List<Bar> Massing(this List<Bar> bars, Field field, Unit prototypeUnit)
        {
            List<Bar> barsCopy = bars.ShallowClone();
            foreach (Bar b in barsCopy)
            {
                //reset units
                b.Units = new List<Unit>();
                b.Massing(field, prototypeUnit);
            }
            return barsCopy;
        }
        public static Bar Massing(this Bar bar, Field field, Unit prototypeUnit)
        {
            foreach (Guid f in bar.Cells)
            {
                var refcell = field.Cells.Find(x => x.BHoM_Guid.Equals(f));
                List<Cartesian> cartesians = new List<Cartesian>();
                for(int i = 0;i< refcell.Levels;i++)
                {
                    Vector up = new Vector() { X = 0, Y = 0, Z = i * prototypeUnit.Z };
                    Cartesian cartesianCopy = refcell.CoordinateSystem.ShallowClone();
                    cartesians.Add(cartesianCopy.Translate(up));
                }

                foreach(Cartesian cartesian in cartesians)
                {
                    TransformMatrix transform = Geometry.Create.OrientationMatrixGlobalToLocal(cartesian);
                    Unit unit = prototypeUnit.ShallowClone();
                    unit.BHoM_Guid = Guid.NewGuid();
                    unit.CoordinateSystem = unit.CoordinateSystem.Transform(transform);
                    bar.Units.Add(unit);
                }
            }
            return bar;
        }

        
    }
}
