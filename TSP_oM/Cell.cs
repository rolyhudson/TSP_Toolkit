using BH.oM.Base;
using BH.oM.Geometry;
using BH.oM.Geometry.CoordinateSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.TSP
{
    public class Cell : BHoMObject
    {
        public virtual int Levels { get; set; } = 1;
        public List<Cell> FourNeighbourhood { get; set; } = new List<Cell>();
        public List<Cell> EightNeighbourhood { get; set; } = new List<Cell>();
        public Polyline Boundary { get; set; } = new Polyline();
        public virtual Cartesian CoordinateSystem { get; set; } = new Cartesian();
        public virtual Use Use { get; set; } = Use.Unoccupied;

        public virtual Point Centre { get; set; } = new Point();
    }
}
