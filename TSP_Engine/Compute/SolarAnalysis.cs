﻿using BH.oM.Geometry;
using BH.Engine.Geometry;
using BH.oM.TSP;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using BH.oM.Geometry.CoordinateSystem;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace BH.Engine.TSP
{
    public static partial class Compute
    {
        private static SolarResult SolarAnalysis(this Unit unit, List<Point> sunPoints)
        {
            List<Plane> planes = unit.ObservationPlanes();
            int score = 0;
            foreach(Plane plane in planes)
            {
                Cartesian local = Geometry.Create.CartesianCoordinateSystem(plane.Origin, Vector.XAxis, Vector.YAxis);
                TransformMatrix transform = BH.Engine.Geometry.Create.OrientationMatrixGlobalToLocal(unit.CoordinateSystem);
                // translate obs points local
                List<Point> localPts = new List<Point>();
                foreach(Point p in sunPoints)
                    localPts.Add(p.Transform(transform));

                List<Point> points = Query.InHalfSpace(localPts, plane, true);

                foreach(Point p in localPts)
                {
                    //translate obs points local
                    
                    Line testLine = Geometry.Create.Line(plane.Origin, p);
                    Dictionary<Sphere, Mesh> potentialObs = GetPotentialObstructions(testLine);
                    if (!Obstructed(plane.Origin,testLine.Direction(), potentialObs.Values.ToList()))
                        score++;
                }
            }
            SolarResult solarResult = new SolarResult(unit.BHoM_Guid, 1, 1, score, unit);

            return solarResult;
        }
        public static List<SolarResult> SolarAnalysis(this List<Bar> bars, List<Point> sunPoints, List<Mesh> potentialObstructions)
        {
            m_MeshBoundingSpheres = new Dictionary<Sphere, Mesh>();
            foreach (Mesh m in potentialObstructions)
                m_MeshBoundingSpheres.Add(m.BoundingSphere(),m);
            ;
            ConcurrentBag<SolarResult> scores = new ConcurrentBag<SolarResult>();

            foreach (Bar b in bars)
            {
                
                Parallel.ForEach(b.Units, unit =>
                {
                    scores.Add(unit.SolarAnalysis(sunPoints));
                });

            }
            return scores.ToList();
        }
        

        private static Dictionary<Sphere, Mesh> GetPotentialObstructions(Line testLine)
        {
            Dictionary<Sphere, Mesh> obstructing = new Dictionary<Sphere, Mesh>();
            foreach(KeyValuePair<Sphere,Mesh> kvp in m_MeshBoundingSpheres)
            {
                if (kvp.Key.Centre.Z == testLine.Start.Z)
                    continue;
                Point closest = testLine.ClosestPoint(kvp.Key.Centre, false);
                if (closest.SquareDistance(kvp.Key.Centre) < (kvp.Key.Radius * kvp.Key.Radius))
                    obstructing.Add(kvp.Key, kvp.Value);
            }
            
            return obstructing;
        }

        //public static Dictionary<Sphere, Mesh> GetPotentialObstructions(Line testLine, List<Mesh> potentialObstructions)
        //{
        //    m_MeshBoundingSpheres = new Dictionary<Sphere, Mesh>();
        //    foreach (Mesh m in potentialObstructions)
        //        m_MeshBoundingSpheres.Add(m.BoundingSphere(), m);

        //    Dictionary<Sphere, Mesh> obstructing = new Dictionary<Sphere, Mesh>();
        //    foreach (KeyValuePair<Sphere, Mesh> kvp in m_MeshBoundingSpheres)
        //    {
        //        Point closest = testLine.ClosestPoint(kvp.Key.Centre, false);
        //        if (closest.SquareDistance(kvp.Key.Centre) < (kvp.Key.Radius * kvp.Key.Radius))
        //            obstructing.Add(kvp.Key, kvp.Value);
        //    }
        //    return obstructing;
        //}

        private static bool Obstructed(Point start, Vector direction, List<Mesh> meshes)
        {
            foreach(Mesh m in meshes)
            {
                if (m.MeshIntersect(start, direction))
                    return true;
            }
            return false;
        }

        //public static int SolarAnalysis(this Unit unit, List<Point> observationPoints, List<Mesh> potentialObstructions)
        //{
        //    m_MeshBoundingSpheres = new Dictionary<Sphere, Mesh>();
        //    foreach (Mesh m in potentialObstructions)
        //        m_MeshBoundingSpheres.Add(m.BoundingSphere(), m);

        //    List<Plane> planes = unit.ObservationPlanes();
        //    int score = 0;
        //    foreach (Plane plane in planes)
        //    {
        //        Cartesian local = Geometry.Create.CartesianCoordinateSystem(plane.Origin, Vector.XAxis, Vector.YAxis);
        //        TransformMatrix transform = BH.Engine.Geometry.Create.OrientationMatrixGlobalToLocal(unit.CoordinateSystem);
        //        // translate obs points local
        //        List<Point> localPts = new List<Point>();
        //        foreach (Point p in observationPoints)
        //            localPts.Add(p.Transform(transform));

        //        List<Point> points = Query.InHalfSpace(localPts, plane, true);

        //        foreach (Point p in localPts)
        //        {
        //            //translate obs points local

        //            Line testLine = Geometry.Create.Line(plane.Origin, p);
        //            Dictionary<Sphere, Mesh> potentialObs = GetPotentialObstructions(testLine);
        //            if (!Obstructed(plane.Origin, testLine.Direction(), potentialObs.Values.ToList()))
        //                score++;
        //        }
        //    }

        //    return score;
        //}
        private static Dictionary<Sphere, Mesh> m_MeshBoundingSpheres = new Dictionary<Sphere, Mesh>(); 
    }
}