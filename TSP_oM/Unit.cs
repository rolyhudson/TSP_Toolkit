using BH.oM.Base;
using BH.oM.Geometry.CoordinateSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.TSP
{
    public class Unit : BHoMObject
    {
        public virtual double X { get; set; } = 10.0;
        public virtual double Y { get; set; } = 27.5;
        public virtual double Z { get; set; } = 3.5;
        public virtual Cartesian CoordinateSystem { get; set; } = new Cartesian();

        public virtual int NumberOfApartments { get; set; } = 4;

        public virtual double ApartmentArea { get; set; } = 65;

        public virtual double CirculationArea { get; set; } = 4;

    }
}
