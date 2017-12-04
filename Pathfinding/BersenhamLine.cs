using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheUndergroundTower.Pathfinding
{
    //http://www.roguebasin.com/index.php?title=Bresenham's_Line_Algorithm
    public static class BersenhamLine
    {
        
        public static List<MapCoord> Line(MapCoord origin, MapCoord target, Map map , Func<Tile, bool> plot)
        {
            List<MapCoord> line = new List<MapCoord>();
            int x0 = origin.X;
            int y0 = origin.Y;
            int x1 = target.X;
            int y1 = target.Y;
            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (steep) { Swap<int>(ref x0, ref y0); Swap<int>(ref x1, ref y1); }
            if (x0 > x1) { Swap<int>(ref x0, ref x1); Swap<int>(ref y0, ref y1); }
            int dX = (x1 - x0), dY = Math.Abs(y1 - y0), err = (dX / 2), ystep = (y0 < y1 ? 1 : -1), y = y0;

            for (int x = x0; x <= x1; ++x)
            {
                if (!(steep ? plot(map.Tiles[y,x]) : plot(map.Tiles[x,y]))) return null;
                err = err - dY;
                if (err < 0) { y += ystep; err += dX; }
                line.Add(new MapCoord(steep ? y : x, steep ? x: y));
            }
            return line;
        }

        private static void Swap<T>(ref T lhs, ref T rhs) { T temp; temp = lhs; lhs = rhs; rhs = temp; }

    }
}
