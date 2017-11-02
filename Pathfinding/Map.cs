using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheUndergroundTower.Pathfinding
{
    public class Map
    {
        private int _xSize, _ySize;
        private Tile[,] _tiles;

        public Map()
        {

        }

        public int XSize { get => _xSize; set => _xSize = value; }
        public int YSize { get => _ySize; set => _ySize = value; }
    }
}
