using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheUndergroundTower.Pathfinding
{
    /// <summary>
    /// A mutable <int,int> tuple
    /// </summary>
    public class MapCoord : IEquatable<MapCoord>
    {
        protected int _x, _y;
        public int X
        {
            get { return _x; }
            set
            {
                if (value >= 0) _x = value;
                else _x = 0;
            }
        }

        public int Y
        {
            get { return _y; }
            set
            {
                if (value >= 0) _y = value;
                else _y = 0;
            }
        }

        public MapCoord() { }

        public MapCoord(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(MapCoord other)
        {
            if (other == null) return false;
            return _x == other._x && _y == other._y;
        }
    }
}
