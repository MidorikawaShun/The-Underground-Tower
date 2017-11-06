using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheUndergroundTower.Pathfinding
{
    /// <summary>
    /// A class that represents a level in the dungeon.
    /// </summary>
    public class Map
    {
        /// <summary>
        /// The dimensions of the map (width, height).
        /// </summary>
        private int _xSize, _ySize;

        /// <summary>
        /// All the tiles that are contained in this map (Floors, walls).
        /// </summary>
        private Tile[,] _tiles;

        /// <summary>
        /// Regular constructor.
        /// </summary>
        public Map()
        {

        }

        //Properties
        public int XSize { get => _xSize; set => _xSize = value; }
        public int YSize { get => _ySize; set => _ySize = value; }
    }
}
