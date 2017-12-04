using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheUndergroundTower.Pathfinding
{
    /// <summary>
    /// A mutable <int,int,int> tuple
    /// </summary>
    public class FullCoord : MapCoord
    {
        private int _z;
        public int Z
        {
            get { return _z; }
            set
            {
                if (value >= 0) _z = value;
                else _z = 0;
            }
        }

        public MapCoord Minified
        {
            get { return new MapCoord(_x, _y); }
            private set { }
        }

        public FullCoord() { }

        public FullCoord(int x, int y, int z) : base(x, y)
        {
            Z = z;
        }

    }
}
