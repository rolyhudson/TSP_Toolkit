﻿using BH.Engine.Base;
using BH.Engine.Geometry;
using BH.oM.Geometry;
using BH.oM.Geometry.CoordinateSystem;
using BH.oM.TSP;
using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.TSP
{
    public static partial class Create
    {
        public static Field IField(ILayout layout, SiteLandUse siteLandUse, Unit prototypeUnit)
        {
            Field field = Field(layout as dynamic, siteLandUse.Boundary, prototypeUnit);
            field.Layout = layout;
            return field; 
        }
        public static Field Field(BarsLayout layout, Polyline siteBoundary, Unit prototypeUnit)
        {
            
            Polyline alignedBoundary = Query.AlignedBoundingBox(layout.PrincipleDirection, siteBoundary);
            Cartesian origin = Query.GridCartesian(layout.PrincipleDirection, siteBoundary);
            int unitsX = (int)Math.Ceiling(alignedBoundary.ControlPoints[0].Distance(alignedBoundary.ControlPoints[3]) / prototypeUnit.X);
            int unitsY = (int)Math.Ceiling(alignedBoundary.ControlPoints[0].Distance(alignedBoundary.ControlPoints[1]) / prototypeUnit.Y);

            List<Point> points = prototypeUnit.BoundaryPoints();

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

            foreach (Cell f in field.Cells)
            {
                if (!siteBoundary.IIsContaining(f.Boundary))
                    f.Use = new OutsideSiteLandUse();
                
            }

            //we could check for circulation + open space
            return field;
        }

        //////////////////////////////////////////////////

        public static Field Field(PerimeterLayout layout, Polyline siteBoundary, Unit prototypeUnit)
        {
            List<Point> points = prototypeUnit.BoundaryPoints();
            
            Field field = new Field();
            if (siteBoundary.IsClockwise(Vector.ZAxis))
                siteBoundary = siteBoundary.Flip();

            foreach (Line line in siteBoundary.SubParts())
            {
                Polyline boundary = Geometry.Create.Polyline(points);
                if (line.Length() < prototypeUnit.Y)
                    continue;
                Vector yaxis = line.Direction();
                yaxis = yaxis.Normalise();
                Vector xaxis = yaxis.CrossProduct(Vector.ZAxis);
                Point origin = line.Start;
                Cartesian cartesian = Geometry.Create.CartesianCoordinateSystem(origin, xaxis, yaxis);
                TransformMatrix transform = Geometry.Create.OrientationMatrixGlobalToLocal(cartesian);
                boundary = boundary.Transform(transform);

                Cell basePrint = new Cell()
                {
                    Boundary = boundary,
                    CoordinateSystem = cartesian
                };

                Cell copy = basePrint.DeepClone();
                int cellCount = 0;
                double shift = prototypeUnit.Y * cellCount;
                List<Cell> cells = new List<Cell>();
                while (shift + prototypeUnit.Y < line.Length())
                {
                    Cell newCell = new Cell();
                    Vector v = cartesian.Y * shift;
                    newCell.Boundary = copy.Boundary.Translate(v);
                    newCell.CoordinateSystem = copy.CoordinateSystem.Translate(v);
                    newCell.Centre = Geometry.Query.Average(newCell.Boundary.ControlPoints);
                    newCell.BHoM_Guid = Guid.NewGuid();
                    //check for overlap
                    if (!newCell.Overlap(field,Math.Max(prototypeUnit.X,prototypeUnit.Y)))
                        cells.Add(newCell);

                    cellCount++;
                    shift = prototypeUnit.Y * cellCount;
                }
                field.Cells.AddRange(cells);
            }
            return field;
        }

        /// Fall back
        public static Field Field(ILayout layout, Polyline siteBoundary, Unit prototypeUnit)
        {
            return null;
        }
    }
}
