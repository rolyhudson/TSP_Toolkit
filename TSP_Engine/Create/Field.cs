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

            Cell basePrint = new Cell()
            {
                Boundary = boundary,
                CoordinateSystem = origin
            };
            Field field = new Field();
            List<List<Cell>> cells2d = new List<List<Cell>>();
            for(int i = 0; i <= unitsX; i++)
            {
                List<Cell> cells = new List<Cell>();
                for (int j = 0; j < unitsY; j++)
                {
                    Cell copy = basePrint.DeepClone();
                    
                    Vector v = origin.X * i * prototypeUnit.X + origin.Y * j* prototypeUnit.Y;
                    Cell newCell = new Cell();
                    newCell.Boundary = copy.Boundary.Translate(v);
                    newCell.CoordinateSystem = copy.CoordinateSystem.Translate(v);
                    newCell.Centre = Geometry.Query.Average(newCell.Boundary.ControlPoints);
                    newCell.BHoM_Guid = Guid.NewGuid();
                    field.Cells.Add(newCell);
                    cells.Add(newCell);
                }
                cells2d.Add(cells);
            }
            //set the adjacency
            for(int i = 0; i < cells2d.Count;i++)
            {
                for(int j = 0; j < cells2d[i].Count; j++)
                {
                    
                    List<Guid> guids = new List<Guid>();
                    if(i > 0 && j > 0)
                        guids.Add(cells2d[i - 1][j - 1].BHoM_Guid);
                    if (i > 0)
                        guids.Add(cells2d[i - 1][j].BHoM_Guid);
                    if (i > 0 && j < cells2d[i].Count-1)
                        guids.Add(cells2d[i - 1][j + 1].BHoM_Guid);
                    if(j > 0)
                        guids.Add(cells2d[i][j - 1].BHoM_Guid);
                    if(j < cells2d[i].Count - 1)
                        guids.Add(cells2d[i][j + 1].BHoM_Guid);
                    if(i < cells2d.Count-1 && j > 0)
                        guids.Add(cells2d[i + 1][j - 1].BHoM_Guid);
                    if (i < cells2d.Count - 1 && j < cells2d[i].Count - 1)
                        guids.Add(cells2d[i + 1][j + 1].BHoM_Guid);
                    if (i < cells2d.Count -1)
                        guids.Add(cells2d[i + 1][j].BHoM_Guid);

                    field.Adjacency.Add(cells2d[i][j].BHoM_Guid, guids);
                }
            }
            //check for site containment
            foreach(var cell in field.Cells)
            {
                cell.Neighbourhoods(field);
            }
            //Parallel.ForEach(field.Cells, f =>
            //{
            //    f.Neighbourhoods(field);
            //});
            foreach (Cell f in field.Cells)
            {
                if (!siteBoundary.IIsContaining(f.Boundary))
                    f.Use = Use.OutsideBoundary;
                
            }

            //we could check for circulation + open space
            return field;
        }
    }
}
