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
        private const int MAP_DIMENSIONS = 15;


        /// <summary>
        /// Regular constructor.
        /// </summary>
     
        public Map()
        {
            _xSize = _ySize = (MAP_DIMENSIONS*(int)MAP_SIZE_MULTIPLIER);
            Tiles = new Tile[_xSize, _ySize];
            //Create top and bottom map boundaries
            for (int i = 0; i < _xSize; i++)
            {
                Tiles[i, 0] = new Tile(GameData.POSSIBLE_TILES[(int)CreateTile.Tiles.OrdinaryWall]);
                Tiles[i, _ySize-1] = new Tile(GameData.POSSIBLE_TILES[(int)CreateTile.Tiles.OrdinaryWall]);
            }
            //Create right and left boundaries
            for (int i = 1; i < _ySize-1; i++)
            {
                Tiles[0, i] = new Tile(GameData.POSSIBLE_TILES[(int)CreateTile.Tiles.OrdinaryWall]);
                Tiles[_xSize-1, i] = new Tile(GameData.POSSIBLE_TILES[(int)CreateTile.Tiles.OrdinaryWall]);
            }
            //Create floors
            for (int x = 1; x < _xSize-1; x++)
                for (int y = 1; y < _ySize-1; y++)
                    Tiles[x,y] = new Tile(GameData.POSSIBLE_TILES[(int)CreateTile.Tiles.OrdinaryFloor]);

        }

        //Properties
        public int XSize { get => _xSize; set => _xSize = value; }
        public int YSize { get => _ySize; set => _ySize = value; }
        public Tile[,] Tiles { get => _tiles; set => _tiles = value; }
    }
}
