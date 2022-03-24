using BH.oM.Base;
using BH.oM.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.TSP
{
    public class CommunalBlock : BHoMObject
    {
        public List<Polyline> Parking { get; set; } = new List<Polyline>();

        public virtual List<Polyline> Commercial { get; set; } = new List<Polyline>();

        public virtual List<Polyline> Social { get; set; } = new List<Polyline>();

        public virtual Polyline Boundary { get; set; } = new Polyline();

        public virtual double SocialArea { get; set; } = 0;
        public virtual double CommercialArea { get; set; } = 0;

        public virtual int ParkingSpaces { get; set; } = 0;
    }
}
