using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheUndergroundTower.Pathfinding
{
    class Algorithms
    {

        //http://www.roguebasin.com/index.php?title=Bresenhams_Line_Algorithm
        public static List<Tile> Line(int x0, int x1, int y0, int y1, Map map, Func<Tile, bool> plot)
        {
            List<Tile> line = new List<Tile>();
            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (steep) { Swap<int>(ref x0, ref y0); Swap<int>(ref x1, ref y1); }
            if (x0 > x1) { Swap<int>(ref x0, ref x1); Swap<int>(ref y0, ref y1); }
            int dX = (x1 - x0), dY = Math.Abs(y1 - y0), err = (dX / 2), ystep = (y0 < y1 ? 1 : -1), y = y0;

            for (int x = x0; x <= x1; ++x)
            {
                if (!(steep ? plot(map.Tiles[y, x]) : plot(map.Tiles[x, y]))) return null;
                err = err - dY;
                if (err < 0) { y += ystep; err += dX; }
                line.Add(map.Tiles[steep ? y : x, steep ? x : y]);
            }
            return line;
        }

        private static void Swap<T>(ref T lhs, ref T rhs) { T temp; temp = lhs; lhs = rhs; rhs = temp; }


        //https://en.wikipedia.org/wiki/Dijkstras_algorithm
        public static List<Tile> FindPath(Map graph, Tile source, Tile target,Func<Tile,double> weighter)
        {
            List<Tile> Q = new List<Tile>();
            Dictionary<Tile, double> dist = new Dictionary<Tile, double>();
            Dictionary<Tile, Tile> prev = new Dictionary<Tile, Tile>();

            foreach (Tile vertex in graph.Tiles)
            {
                if (vertex != null && vertex.Walkable || vertex == target || vertex==source)
                { //initialize list
                    dist[vertex] = Double.PositiveInfinity;
                    prev[vertex] = null;
                    Q.Add(vertex);
                }
            }

            dist[source] = 0;

            while (Q.Count > 0)
            {
                Tile u = Q.Aggregate((a, b) => dist[a] < dist[b]?a:b); //find tile with least distance
                Q.Remove(u);
                List<Tile> neighbours = graph.AcquireNeighbours(u);
                foreach (Tile neighbour in neighbours)
                {
                    if (neighbour == target || neighbour.Walkable)
                    {
                        double alt = dist[u] + DistanceBetween(u, neighbour) + weighter(u);
                        if (alt<dist[neighbour])
                        {
                            dist[neighbour] = alt;
                            prev[neighbour] = u;
                        }
                    }
                }
            }

            return BuildPath(prev,target);

        }

        private static List<Tile> BuildPath(Dictionary<Tile, Tile> prev, Tile target)
        {
            if (prev[target] == null) return null;
            List<Tile> realPath = new List<Tile>();
            while (target!=null)
            {
                realPath.Insert(0,target);
                target = prev[target];
            }
            if (realPath.Count > 1) realPath.RemoveAt(0);
            return realPath;
        }

        //https://en.wikipedia.org/wiki/Distance#Geometry
        private static double DistanceBetween(Tile origin, Tile destination)
        {
            double distance = double.PositiveInfinity;
            try
            {
                distance = Math.Sqrt(Math.Pow(destination.X - origin.X, 2) + Math.Pow(destination.Y - origin.Y, 2));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return distance;
        }
    }
}
