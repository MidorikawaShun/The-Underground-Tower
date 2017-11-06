using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1;

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

        private static double MAP_SIZE_MULTIPLIER = 1;
        private const int MAP_DIMENSIONS = 5;

        /// <summary>
        /// Regular constructor.
        /// </summary>
     
        public Map()
        {
            _xSize = _ySize = (MAP_DIMENSIONS*(int)MAP_SIZE_MULTIPLIER);
            Tiles = new Tile[XSize, YSize]; 
            for (int i = 0; i < XSize; i++)
            {
                Tiles[i, 0] = new Tile(GameData.POSSIBLE_TILES[0]);
                Tiles[i, XSize-1] = new Tile(GameData.POSSIBLE_TILES[0]);
                Tiles[0, i] = new Tile(GameData.POSSIBLE_TILES[0]);
                Tiles[XSize-1, i] = new Tile(GameData.POSSIBLE_TILES[0]);

                for (int j = 0; j < YSize-1 && i< XSize-2; j++)
                {
                    Tiles[(i + 1), j] = new Tile(GameData.POSSIBLE_TILES[1]);
                }
            }

        }

        //Properties
        public int XSize { get => _xSize; set => _xSize = value; }
        public int YSize { get => _ySize; set => _ySize = value; }
        public Tile[,] Tiles { get => _tiles; set => _tiles = value; }
    }
}
